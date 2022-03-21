using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace ProjectKeySound
{
    public partial class FormMain : Form
    {
        public const int WH_KEYBOARD_LL = 13;
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
        private MediaPlayer[] soundPlayers;
        private int soundPlayerIndex = 0;
        private Dictionary<int, bool> pressedKeyDict = new Dictionary<int, bool>();
        private bool bSoundDisabledByError = false;
        #endregion Properties

        public FormMain()
        {
            InitializeComponent();

            formTransparent = new FormTransparent(this);
            
            openWavFileDialog = new OpenFileDialog();
            openWavFileDialog.Filter = "사운드 파일 (*.wav, *.mp3)|*.wav;*.mp3";
            openWavFileDialog.FilterIndex = 1;
            openWavFileDialog.Multiselect = false;

            soundPlayers = new MediaPlayer[10];
            for (int i = 0; i < soundPlayers.Length; i++)
            {
                soundPlayers[i] = new MediaPlayer();
            }
            labelSoundDisabled.Visible = false;

            keyboardHookProc = new KeyboardHookProc(HandleKeyboardHook);
            Hook();
        }

        ~FormMain()
        {
            UnhookWindowsHookEx(hHook);
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
            labelWavPath.Text = Properties.Settings.Default.WavFilePath;
            if (labelWavPath.Text == String.Empty)
            {
                labelWavPath.Text = Path.Combine(Directory.GetCurrentDirectory(), "default.mp3");
            }
            foreach (MediaPlayer soundPlayer in soundPlayers)
            {
                Uri wavUri = new Uri(labelWavPath.Text);
                soundPlayer.Open(wavUri);
            }

            // Volume
            SetVolume(Properties.Settings.Default.Volume);
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
            if (!bSoundDisabledByError && code >= 0)
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
                soundPlayers[soundPlayerIndex].Stop();
                soundPlayers[soundPlayerIndex].Play();
                soundPlayerIndex++;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return;
            }
            finally
            {
                soundPlayerIndex %= soundPlayers.Length;
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

        private bool SetWavPath(string filePath)
        {
            try
            {
                foreach (MediaPlayer soundPlayer in soundPlayers)
                {
                    Uri wavUri = new Uri(filePath);
                    soundPlayer.Open(wavUri);
                }

                // Success?
                DisableSoundByError(false);
                labelWavPath.Text = filePath;
                Properties.Settings.Default.WavFilePath = filePath;

                return true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        private void btnSelectWav_Click(object sender, EventArgs e)
        {
            if (openWavFileDialog.ShowDialog() == DialogResult.OK)
            {
                    labelWavPath.Text = openWavFileDialog.FileName;
                    SetWavPath(openWavFileDialog.FileName);
            }
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
        
        private void labelWavPath_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                string[] filePath = (string[])e.Data.GetData(DataFormats.FileDrop);
                SetWavPath(filePath[0]);
            }
        }

        private void HandleException(Exception ex)
        {
            DisableSoundByError(true);
            MessageBox.Show($"Error: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}");
        }

        private void DisableSoundByError(bool bDisable)
        {
            bSoundDisabledByError = bDisable;
            labelSoundDisabled.Visible = bDisable;
        }

        private void SetVolume(int volume)
        {
            sliderVolume.Value = volume;
            labelVolume.Text = $"{sliderVolume.Value}%";
            double newVolume = volume / 100.0;
            foreach (MediaPlayer soundPlayer in soundPlayers)
            {
                soundPlayer.Volume = newVolume;
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
