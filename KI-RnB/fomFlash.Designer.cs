namespace KI_RnB
{
    partial class fomFlash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fomFlash));
            this.btnModel = new System.Windows.Forms.Button();
            this.btnVinNo = new System.Windows.Forms.Button();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.picImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // btnModel
            // 
            this.btnModel.Font = new System.Drawing.Font("Tahoma", 39.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModel.Location = new System.Drawing.Point(3, 3);
            this.btnModel.Name = "btnModel";
            this.btnModel.Size = new System.Drawing.Size(324, 88);
            this.btnModel.TabIndex = 6;
            this.btnModel.Text = "M4";
            this.btnModel.UseVisualStyleBackColor = true;
            this.btnModel.Visible = false;
            // 
            // btnVinNo
            // 
            this.btnVinNo.Font = new System.Drawing.Font("Tahoma", 39.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVinNo.Location = new System.Drawing.Point(327, 3);
            this.btnVinNo.Name = "btnVinNo";
            this.btnVinNo.Size = new System.Drawing.Size(694, 88);
            this.btnVinNo.TabIndex = 6;
            this.btnVinNo.Text = "PPGGP7F08GG001861";
            this.btnVinNo.UseVisualStyleBackColor = true;
            this.btnVinNo.Visible = false;
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(229, 212);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(659, 385);
            this.axWindowsMediaPlayer1.TabIndex = 8;
            // 
            // picImage
            // 
            this.picImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picImage.Location = new System.Drawing.Point(350, 148);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(344, 279);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 7;
            this.picImage.TabStop = false;
            // 
            // fomFlash
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.btnVinNo);
            this.Controls.Add(this.btnModel);
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fomFlash";
            this.Text = "Flash Display";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fomFlash_FormClosed);
            this.Load += new System.EventHandler(this.fomFlash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnModel;
        private System.Windows.Forms.Button btnVinNo;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        public System.Windows.Forms.PictureBox picImage;
    }
}