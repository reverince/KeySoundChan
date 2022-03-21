using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace ProjectKeySound
{
    public partial class FormMain : Form
    {
        private const string DEFAULT_TEXT = "⚓⛴포항항ꉂꉂ(ᵔᗜᵔ*)";
        private const int WH_KEYBOARD_LL = 13;
        private static class KeyCode
        {
            public const int VK_SHIFT = 0x10;
            public const int VK_CONTROL = 0x11;
            public const int VK_MENU = 0x12;
        }
        private static class KeyEvent
        {
            public const int WM_KEYDOWN = 0x100;
            public const int WM_KEYUP = 0x101;
            public const int WM_SYSKEYDOWN = 0x104;
            public const int WM_SYSKEYUP = 0x105;
        }

        #region Windows input hook
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadid);
        [DllImport("user32.dll")]
        internal static extern bool UnhookWindowsHookEx(IntPtr idHook);
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct iParam);
        [DllImport("user32.dll")]
        static extern short GetKeyState(int nCode);
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string fileName);
        public delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct iParam);
        public struct KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        #endregion Windows input hook

        #region Properties
        private KeyboardHookProc keyboardHookProc;
        private IntPtr hHook = IntPtr.Zero;

        private FormTransparent formTransparent;

        private OpenFileDialog openWavFileDialog;
        private MediaPlayer[] downMediaPlayers;
        private MediaPlayer[] upMediaPlayers;
        private int downMediaPlayerIndex = 0;
        private int upMediaPlayerIndex = 0;
        private Dictionary<int, bool> pressedKeyDict = new Dictionary<int, bool>();
        #endregion Properties

        public FormMain()
        {
            InitializeComponent();

            formTransparent = new FormTransparent(this);
            
            openWavFileDialog = new OpenFileDialog();
            openWavFileDialog.Filter = "사운드 파일|*.wav;*.mp3;*.m4a";
            openWavFileDialog.FilterIndex = 1;
            openWavFileDialog.Multiselect = false;

            downMediaPlayers = new MediaPlayer[10];
            upMediaPlayers = new MediaPlayer[downMediaPlayers.Length];
            for (int i = 0; i < downMediaPlayers.Length; i++)
            {
                downMediaPlayers[i] = new MediaPlayer();
                upMediaPlayers[i] = new MediaPlayer();
            }

            keyboardHookProc = new KeyboardHookProc(HandleKeyboardHook);
            Hook();
        }

        ~FormMain()
        {
            UnhookWindowsHookEx(hHook);

            foreach (MediaPlayer mediaPlayer in downMediaPlayers)
            {
                mediaPlayer.Close();
            }
            foreach (MediaPlayer mediaPlayer in upMediaPlayers)
            {
                mediaPlayer.Close();
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.MainFormLocation != new Point(-1, -1))
            {
                this.Location = Properties.Settings.Default.MainFormLocation;
            }

            trayIcon.Visible = true;

            // Image
            checkShowTransPanel.Checked = Properties.Settings.Default.ShowImage;
            checkShowTransPanel_CheckedChanged(this, EventArgs.Empty);

            // Wav path
            labelDownWavPath.Text = Properties.Settings.Default.DownWavPath;
            if (labelDownWavPath.Text == string.Empty)
            {
                labelDownWavPath.Text = DEFAULT_TEXT;
            }
            else
            {
                foreach (MediaPlayer mediaPlayer in downMediaPlayers)
                {
                    SetWav(mediaPlayer, labelDownWavPath.Text);
                }
            }
            labelUpWavPath.Text = Properties.Settings.Default.UpWavPath;
            if (labelUpWavPath.Text == string.Empty)
            {
                labelUpWavPath.Text = DEFAULT_TEXT;
            }
            else
            {
                foreach (MediaPlayer mediaPlayer in upMediaPlayers)
                {
                    SetWav(mediaPlayer, labelUpWavPath.Text);
                }
            }

            // Volume
            sliderVolume.Value = Properties.Settings.Default.Volume;
            labelVolume.Text = $"{sliderVolume.Value}%";
        }

        private void SetWav(MediaPlayer mediaPlayer, string wavPath)
        {
            // Mute and play once to preload media.
            Uri wavUri = new Uri(wavPath);
            mediaPlayer.Stop();
            mediaPlayer.Close();
            mediaPlayer.MediaEnded += OnMediaPlayerReady;
            mediaPlayer.Volume = 0.0;
            mediaPlayer.Open(wavUri);
            mediaPlayer.Play();
        }

        private void OnMediaPlayerReady(object? sender, EventArgs e)
        {
            MediaPlayer? mediaPlayer = sender as MediaPlayer;
            if (mediaPlayer != null)
            {
                mediaPlayer.Volume = Properties.Settings.Default.Volume / 100.0;
                mediaPlayer.MediaEnded -= OnMediaPlayerReady;
            }
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                this.ShowInTaskbar = false;
            }
        }

        private void Hook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hHook = SetWindowsHookEx(WH_KEYBOARD_LL, HandleKeyboardHook, hInstance, 0);
        }

        public int HandleKeyboardHook(int code, int wParam, ref KeyboardHookStruct iParam)
        {
            if (code >= 0)
            {
                //Keys keys = (Keys)iParam.vkCode;
                //if ((GetKeyState(KeyCode.VK_CONTROL) & 0x80) != 0) keys |= Keys.Control;
                //if ((GetKeyState(KeyCode.VK_SHIFT) & 0x80) != 0) keys |= Keys.Shift;
                //if ((GetKeyState(KeyCode.VK_MENU) & 0x80) != 0) keys |= Keys.Menu;
                //KeyEventArgs keyEventArgs = new KeyEventArgs(keys);

                if (wParam == KeyEvent.WM_KEYDOWN || wParam == KeyEvent.WM_SYSKEYDOWN)
                {
                    HandleKeyDownHook(iParam.vkCode);
                }
                else if (wParam == KeyEvent.WM_KEYUP || wParam == KeyEvent.WM_SYSKEYUP)
                {
                    pressedKeyDict[iParam.vkCode] = false;
                    upMediaPlayers[upMediaPlayerIndex].Stop();
                    upMediaPlayers[upMediaPlayerIndex].Play();
                    upMediaPlayerIndex++;
                    upMediaPlayerIndex %= upMediaPlayers.Length;
                }
            }

            return CallNextHookEx(hHook, code, wParam, ref iParam);
        }

        private void HandleKeyDownHook(int keyCode)
        {
            if (!pressedKeyDict.ContainsKey(keyCode))
            {
                pressedKeyDict[keyCode] = false;
            }

            // Do not repeat sound when the key is pressed already.
            if (pressedKeyDict[keyCode])
            {
                return;
            }

            try
            {
                downMediaPlayers[downMediaPlayerIndex].Stop();
                downMediaPlayers[downMediaPlayerIndex].Play();
                downMediaPlayerIndex++;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return;
            }
            finally
            {
                downMediaPlayerIndex %= downMediaPlayers.Length;
            }

            pressedKeyDict[keyCode] = true;
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.Focus();
        }

        private void trayItemShow_Click(object sender, EventArgs e)
        {
            trayIcon_DoubleClick(sender, e);
        }

        private void trayItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkShowTransPanel_CheckedChanged(object sender, EventArgs e)
        {
            if (checkShowTransPanel.Checked)
            {
                if (formTransparent.IsDisposed)
                {
                    formTransparent.Close();
                    formTransparent = new FormTransparent(this);
                }

                formTransparent.Show();
            }
            else
            {
                formTransparent.Hide();
            }

            Properties.Settings.Default.ShowImage = checkShowTransPanel.Checked;
        }

        public void UpdateCheckShowTransPanel()
        {
            checkShowTransPanel.Checked = false;
        }

        private bool SetWavPath(string filePath, bool isDown = true)
        {
            try
            {
                MediaPlayer[] mediaPlayers = isDown ? downMediaPlayers : upMediaPlayers;
                bool isFilePathEmpty = filePath == string.Empty;
                foreach (MediaPlayer mediaPlayer in mediaPlayers)
                {
                    if (isFilePathEmpty)
                    {
                        mediaPlayer.Close();
                        continue;
                    }
                    SetWav(mediaPlayer, filePath);
                }

                // Success?
                if (isDown)
                {
                    labelDownWavPath.Text = isFilePathEmpty ? DEFAULT_TEXT : filePath;
                    Properties.Settings.Default.DownWavPath = filePath;
                }
                else
                {
                    labelUpWavPath.Text = isFilePathEmpty ? DEFAULT_TEXT : filePath;
                    Properties.Settings.Default.UpWavPath = filePath;
                }

                return true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        private void btnSelectDownWav_Click(object sender, EventArgs e)
        {
            if (openWavFileDialog.ShowDialog() == DialogResult.OK)
            {
                    SetWavPath(openWavFileDialog.FileName, true);
            }
        }

        private void btnSelectUpWav_Click(object sender, EventArgs e)
        {
            if (openWavFileDialog.ShowDialog() == DialogResult.OK)
            {
                SetWavPath(openWavFileDialog.FileName, false);
            }
        }

        private void btnRemoveDownWav_Click(object sender, EventArgs e)
        {
            SetWavPath(string.Empty, true);
        }

        private void btnRemoveUpWav_Click(object sender, EventArgs e)
        {
            SetWavPath(string.Empty, false);
        }

        private void labelWavPath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }
        
        private void labelDownWavPath_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                string[] filePath = (string[])e.Data.GetData(DataFormats.FileDrop);
                SetWavPath(filePath[0], true);
            }
        }

        private void labelUpWavPath_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                string[] filePath = (string[])e.Data.GetData(DataFormats.FileDrop);
                SetWavPath(filePath[0], false);
            }
        }

        private void HandleException(Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}");
        }

        private void SetVolume(int volume)
        {
            sliderVolume.Value = volume;
            labelVolume.Text = $"{sliderVolume.Value}%";
            double newVolume = volume / 100.0;
            foreach (MediaPlayer mediaPlayer in downMediaPlayers)
            {
                mediaPlayer.Volume = newVolume;
            }
            foreach (MediaPlayer mediaPlayer in upMediaPlayers)
            {
                mediaPlayer.Volume = newVolume;
            }
            Properties.Settings.Default.Volume = volume;
        }

        private void sliderVolume_Scroll(object sender, EventArgs e)
        {
            SetVolume(sliderVolume.Value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Properties.Settings.Default.ImageFormSize = formTransparent.Size;
            Properties.Settings.Default.ImageFormLocation = formTransparent.Location;
            Properties.Settings.Default.MainFormLocation = this.Location;
            Properties.Settings.Default.Save();
            base.OnFormClosing(e);
        }
    }  // End of Form
}  // End of namespace
