using System.Runtime.InteropServices;

namespace ProjectKeySound
{
    public partial class FormTransparent : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        private Form parent;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public FormTransparent(Form parent)
        {
            InitializeComponent();
            this.parent = parent;
            this.TransparencyKey = Color.Black;
            this.MouseWheel += new MouseEventHandler(OnMouseWheel);
        }

        private void FormTransparent_Load(object sender, EventArgs e)
        {
            this.Size = Properties.Settings.Default.ImageFormSize;
            if (Properties.Settings.Default.ImageFormLocation != new Point(-1, -1))
            {
                this.Location = Properties.Settings.Default.ImageFormLocation;
            }
        }

        private void OnMouseWheel(object? sender, MouseEventArgs e)
        {
            float ImageScale = e.Delta * 0.0005f;
            this.Size += new Size((int)(this.Width * ImageScale), (int)(this.Height * ImageScale));
        }

        private void pictureMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public void ResetLocation()
        {
            this.CenterToParent();
            this.Size = new Size(200, 200);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            FormMain formMain = (FormMain)parent;
            formMain.UpdateCheckShowTransPanel();
            Properties.Settings.Default.ImageFormSize = this.Size;
            Properties.Settings.Default.ImageFormLocation = this.Location;
            base.OnFormClosing(e);
        }
    }  // End of Form
}  // End of namespace
