namespace ProjectKeySound
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextTrayIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trayItemShow = new System.Windows.Forms.ToolStripMenuItem();
            this.trayItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSelectWav = new System.Windows.Forms.Button();
            this.labelWavPath = new System.Windows.Forms.Label();
            this.labelSoundDisabled = new System.Windows.Forms.Label();
            this.sliderVolume = new System.Windows.Forms.TrackBar();
            this.labelVolume = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkShowTransPanel = new System.Windows.Forms.CheckBox();
            this.pictureMain = new System.Windows.Forms.PictureBox();
            this.contextTrayIcon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureMain)).BeginInit();
            this.SuspendLayout();
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.contextTrayIcon;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "KeySoundChan";
            this.trayIcon.Visible = true;
            this.trayIcon.DoubleClick += new System.EventHandler(this.trayIcon_DoubleClick);
            // 
            // contextTrayIcon
            // 
            this.contextTrayIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayItemShow,
            this.trayItemExit});
            this.contextTrayIcon.Name = "contextTrayIcon";
            this.contextTrayIcon.Size = new System.Drawing.Size(105, 48);
            // 
            // trayItemShow
            // 
            this.trayItemShow.Name = "trayItemShow";
            this.trayItemShow.Size = new System.Drawing.Size(104, 22);
            this.trayItemShow.Text = "Show";
            this.trayItemShow.Click += new System.EventHandler(this.trayItemShow_Click);
            // 
            // trayItemExit
            // 
            this.trayItemExit.Name = "trayItemExit";
            this.trayItemExit.Size = new System.Drawing.Size(104, 22);
            this.trayItemExit.Text = "Exit";
            this.trayItemExit.Click += new System.EventHandler(this.trayItemExit_Click);
            // 
            // btnSelectWav
            // 
            this.btnSelectWav.AutoSize = true;
            this.btnSelectWav.Location = new System.Drawing.Point(178, 109);
            this.btnSelectWav.Name = "btnSelectWav";
            this.btnSelectWav.Size = new System.Drawing.Size(81, 25);
            this.btnSelectWav.TabIndex = 1;
            this.btnSelectWav.Text = "사운드 선택";
            this.btnSelectWav.UseVisualStyleBackColor = true;
            this.btnSelectWav.Click += new System.EventHandler(this.btnSelectWav_Click);
            // 
            // labelWavPath
            // 
            this.labelWavPath.AllowDrop = true;
            this.labelWavPath.AutoEllipsis = true;
            this.labelWavPath.AutoSize = true;
            this.labelWavPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelWavPath.Location = new System.Drawing.Point(178, 89);
            this.labelWavPath.Name = "labelWavPath";
            this.labelWavPath.Size = new System.Drawing.Size(248, 17);
            this.labelWavPath.TabIndex = 0;
            this.labelWavPath.Text = "버튼을 눌러 재생할 사운드를 선택해 주세요.";
            this.labelWavPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.labelWavPath_DragDrop);
            this.labelWavPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.labelWavPath_DragEnter);
            // 
            // labelSoundDisabled
            // 
            this.labelSoundDisabled.AutoSize = true;
            this.labelSoundDisabled.Location = new System.Drawing.Point(265, 114);
            this.labelSoundDisabled.Name = "labelSoundDisabled";
            this.labelSoundDisabled.Size = new System.Drawing.Size(325, 15);
            this.labelSoundDisabled.TabIndex = 2;
            this.labelSoundDisabled.Text = "사운드 재생이 비활성화됐어요. 다시 파일을 선택해 주세요.";
            // 
            // sliderVolume
            // 
            this.sliderVolume.Location = new System.Drawing.Point(215, 138);
            this.sliderVolume.Maximum = 100;
            this.sliderVolume.Name = "sliderVolume";
            this.sliderVolume.Size = new System.Drawing.Size(386, 45);
            this.sliderVolume.TabIndex = 3;
            this.sliderVolume.Value = 1;
            this.sliderVolume.Scroll += new System.EventHandler(this.sliderVolume_Scroll);
            // 
            // labelVolume
            // 
            this.labelVolume.Location = new System.Drawing.Point(607, 140);
            this.labelVolume.Name = "labelVolume";
            this.labelVolume.Size = new System.Drawing.Size(39, 15);
            this.labelVolume.TabIndex = 4;
            this.labelVolume.Text = "100%";
            this.labelVolume.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "볼륨";
            // 
            // checkShowTransPanel
            // 
            this.checkShowTransPanel.AutoSize = true;
            this.checkShowTransPanel.Enabled = false;
            this.checkShowTransPanel.Location = new System.Drawing.Point(178, 67);
            this.checkShowTransPanel.Name = "checkShowTransPanel";
            this.checkShowTransPanel.Size = new System.Drawing.Size(102, 19);
            this.checkShowTransPanel.TabIndex = 8;
            this.checkShowTransPanel.Text = "이미지 띄우기";
            this.checkShowTransPanel.UseVisualStyleBackColor = true;
            this.checkShowTransPanel.CheckedChanged += new System.EventHandler(this.checkShowTransPanel_CheckedChanged);
            // 
            // pictureMain
            // 
            this.pictureMain.BackColor = System.Drawing.Color.Transparent;
            this.pictureMain.Location = new System.Drawing.Point(12, 12);
            this.pictureMain.Name = "pictureMain";
            this.pictureMain.Size = new System.Drawing.Size(160, 160);
            this.pictureMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureMain.TabIndex = 9;
            this.pictureMain.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(674, 184);
            this.Controls.Add(this.pictureMain);
            this.Controls.Add(this.checkShowTransPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelVolume);
            this.Controls.Add(this.sliderVolume);
            this.Controls.Add(this.labelSoundDisabled);
            this.Controls.Add(this.labelWavPath);
            this.Controls.Add(this.btnSelectWav);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "KeySoundChan";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.contextTrayIcon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sliderVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private NotifyIcon trayIcon;
        private ContextMenuStrip contextTrayIcon;
        private ToolStripMenuItem trayItemShow;
        private ToolStripMenuItem trayItemExit;
        private Button btnSelectWav;
        private Label labelWavPath;
        private Label labelSoundDisabled;
        private TrackBar sliderVolume;
        private Label labelVolume;
        private Label label2;
        private CheckBox checkShowTransPanel;
        private PictureBox pictureMain;
    }
}