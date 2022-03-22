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
            this.창위치초기화ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trayItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSelectDownWav = new System.Windows.Forms.Button();
            this.labelDownWavPath = new System.Windows.Forms.Label();
            this.sliderVolume = new System.Windows.Forms.TrackBar();
            this.labelVolume = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkShowTransPanel = new System.Windows.Forms.CheckBox();
            this.pictureMain = new System.Windows.Forms.PictureBox();
            this.labelUpWavPath = new System.Windows.Forms.Label();
            this.btnSelectUpWav = new System.Windows.Forms.Button();
            this.btnRemoveDownWav = new System.Windows.Forms.Button();
            this.btnRemoveUpWav = new System.Windows.Forms.Button();
            this.labelTrayNotice = new System.Windows.Forms.Label();
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
            this.창위치초기화ToolStripMenuItem,
            this.trayItemExit});
            this.contextTrayIcon.Name = "contextTrayIcon";
            this.contextTrayIcon.Size = new System.Drawing.Size(155, 70);
            // 
            // trayItemShow
            // 
            this.trayItemShow.Name = "trayItemShow";
            this.trayItemShow.Size = new System.Drawing.Size(154, 22);
            this.trayItemShow.Text = "설정 창 띄우기";
            this.trayItemShow.Click += new System.EventHandler(this.trayItemShow_Click);
            // 
            // 창위치초기화ToolStripMenuItem
            // 
            this.창위치초기화ToolStripMenuItem.Name = "창위치초기화ToolStripMenuItem";
            this.창위치초기화ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.창위치초기화ToolStripMenuItem.Text = "창 위치 초기화";
            this.창위치초기화ToolStripMenuItem.Click += new System.EventHandler(this.trayItemResetLocation_Click);
            // 
            // trayItemExit
            // 
            this.trayItemExit.Name = "trayItemExit";
            this.trayItemExit.Size = new System.Drawing.Size(154, 22);
            this.trayItemExit.Text = "끄기";
            this.trayItemExit.Click += new System.EventHandler(this.trayItemExit_Click);
            // 
            // btnSelectDownWav
            // 
            this.btnSelectDownWav.AutoSize = true;
            this.btnSelectDownWav.Location = new System.Drawing.Point(178, 54);
            this.btnSelectDownWav.Name = "btnSelectDownWav";
            this.btnSelectDownWav.Size = new System.Drawing.Size(165, 25);
            this.btnSelectDownWav.TabIndex = 1;
            this.btnSelectDownWav.Text = "누를 때 재생할 사운드 선택";
            this.btnSelectDownWav.UseVisualStyleBackColor = true;
            this.btnSelectDownWav.Click += new System.EventHandler(this.btnSelectDownWav_Click);
            // 
            // labelDownWavPath
            // 
            this.labelDownWavPath.AllowDrop = true;
            this.labelDownWavPath.AutoEllipsis = true;
            this.labelDownWavPath.AutoSize = true;
            this.labelDownWavPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDownWavPath.Location = new System.Drawing.Point(178, 34);
            this.labelDownWavPath.Name = "labelDownWavPath";
            this.labelDownWavPath.Size = new System.Drawing.Size(248, 17);
            this.labelDownWavPath.TabIndex = 0;
            this.labelDownWavPath.Text = "버튼을 눌러 재생할 사운드를 선택해 주세요.";
            this.labelDownWavPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.labelDownWavPath_DragDrop);
            this.labelDownWavPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.labelWavPath_DragEnter);
            // 
            // sliderVolume
            // 
            this.sliderVolume.Location = new System.Drawing.Point(215, 138);
            this.sliderVolume.Maximum = 100;
            this.sliderVolume.Name = "sliderVolume";
            this.sliderVolume.Size = new System.Drawing.Size(386, 45);
            this.sliderVolume.TabIndex = 5;
            this.sliderVolume.Value = 1;
            this.sliderVolume.Scroll += new System.EventHandler(this.sliderVolume_Scroll);
            // 
            // labelVolume
            // 
            this.labelVolume.Location = new System.Drawing.Point(607, 140);
            this.labelVolume.Name = "labelVolume";
            this.labelVolume.Size = new System.Drawing.Size(39, 15);
            this.labelVolume.TabIndex = 4;
            this.labelVolume.Text = "???%";
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
            this.checkShowTransPanel.Location = new System.Drawing.Point(178, 12);
            this.checkShowTransPanel.Name = "checkShowTransPanel";
            this.checkShowTransPanel.Size = new System.Drawing.Size(137, 19);
            this.checkShowTransPanel.TabIndex = 0;
            this.checkShowTransPanel.Text = "이미지 띄우기 (v1.1)";
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
            // labelUpWavPath
            // 
            this.labelUpWavPath.AllowDrop = true;
            this.labelUpWavPath.AutoEllipsis = true;
            this.labelUpWavPath.AutoSize = true;
            this.labelUpWavPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelUpWavPath.Location = new System.Drawing.Point(178, 82);
            this.labelUpWavPath.Name = "labelUpWavPath";
            this.labelUpWavPath.Size = new System.Drawing.Size(248, 17);
            this.labelUpWavPath.TabIndex = 10;
            this.labelUpWavPath.Text = "버튼을 눌러 재생할 사운드를 선택해 주세요.";
            this.labelUpWavPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.labelUpWavPath_DragDrop);
            this.labelUpWavPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.labelWavPath_DragEnter);
            // 
            // btnSelectUpWav
            // 
            this.btnSelectUpWav.AutoSize = true;
            this.btnSelectUpWav.Location = new System.Drawing.Point(178, 102);
            this.btnSelectUpWav.Name = "btnSelectUpWav";
            this.btnSelectUpWav.Size = new System.Drawing.Size(153, 25);
            this.btnSelectUpWav.TabIndex = 3;
            this.btnSelectUpWav.Text = "뗄 때 재생할 사운드 선택";
            this.btnSelectUpWav.UseVisualStyleBackColor = true;
            this.btnSelectUpWav.Click += new System.EventHandler(this.btnSelectUpWav_Click);
            // 
            // btnRemoveDownWav
            // 
            this.btnRemoveDownWav.Location = new System.Drawing.Point(349, 55);
            this.btnRemoveDownWav.Name = "btnRemoveDownWav";
            this.btnRemoveDownWav.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveDownWav.TabIndex = 2;
            this.btnRemoveDownWav.Text = "없애기";
            this.btnRemoveDownWav.UseVisualStyleBackColor = true;
            this.btnRemoveDownWav.Click += new System.EventHandler(this.btnRemoveDownWav_Click);
            // 
            // btnRemoveUpWav
            // 
            this.btnRemoveUpWav.Location = new System.Drawing.Point(349, 103);
            this.btnRemoveUpWav.Name = "btnRemoveUpWav";
            this.btnRemoveUpWav.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveUpWav.TabIndex = 4;
            this.btnRemoveUpWav.Text = "없애기";
            this.btnRemoveUpWav.UseVisualStyleBackColor = true;
            this.btnRemoveUpWav.Click += new System.EventHandler(this.btnRemoveUpWav_Click);
            // 
            // labelTrayNotice
            // 
            this.labelTrayNotice.AutoSize = true;
            this.labelTrayNotice.Location = new System.Drawing.Point(488, 9);
            this.labelTrayNotice.Name = "labelTrayNotice";
            this.labelTrayNotice.Size = new System.Drawing.Size(174, 15);
            this.labelTrayNotice.TabIndex = 11;
            this.labelTrayNotice.Text = "최소화하면 트레이로 이동해요.";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(674, 184);
            this.Controls.Add(this.labelTrayNotice);
            this.Controls.Add(this.btnRemoveUpWav);
            this.Controls.Add(this.btnRemoveDownWav);
            this.Controls.Add(this.btnSelectUpWav);
            this.Controls.Add(this.labelUpWavPath);
            this.Controls.Add(this.pictureMain);
            this.Controls.Add(this.checkShowTransPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelVolume);
            this.Controls.Add(this.sliderVolume);
            this.Controls.Add(this.labelDownWavPath);
            this.Controls.Add(this.btnSelectDownWav);
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
        private Button btnSelectDownWav;
        private Label labelDownWavPath;
        private TrackBar sliderVolume;
        private Label labelVolume;
        private Label label2;
        private CheckBox checkShowTransPanel;
        private PictureBox pictureMain;
        private Label labelUpWavPath;
        private Button btnSelectUpWav;
        private Button btnRemoveDownWav;
        private Button btnRemoveUpWav;
        private Label labelTrayNotice;
        private ToolStripMenuItem 창위치초기화ToolStripMenuItem;
    }
}