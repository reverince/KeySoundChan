namespace ProjectKeySound
{
    partial class FormTransparent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureMain = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureMain)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureMain
            // 
            this.pictureMain.BackColor = System.Drawing.Color.Transparent;
            this.pictureMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureMain.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.pictureMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureMain.Location = new System.Drawing.Point(0, 0);
            this.pictureMain.Margin = new System.Windows.Forms.Padding(0);
            this.pictureMain.Name = "pictureMain";
            this.pictureMain.Size = new System.Drawing.Size(200, 200);
            this.pictureMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureMain.TabIndex = 1;
            this.pictureMain.TabStop = false;
            this.pictureMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureMain_MouseDown);
            // 
            // FormTransparent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(200, 200);
            this.Controls.Add(this.pictureMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormTransparent";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FormTransparent";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormTransparent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureMain;
    }
}