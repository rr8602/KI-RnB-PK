namespace KI_RnB
{
    partial class fomBrake
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
            this.pic_Msgs = new System.Windows.Forms.PictureBox();
            this.pic_Head = new System.Windows.Forms.PictureBox();
            this.pic_Brks = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Msgs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Head)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Brks)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_Msgs
            // 
            this.pic_Msgs.BackColor = System.Drawing.Color.Yellow;
            this.pic_Msgs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pic_Msgs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pic_Msgs.Location = new System.Drawing.Point(0, 88);
            this.pic_Msgs.Name = "pic_Msgs";
            this.pic_Msgs.Size = new System.Drawing.Size(1024, 112);
            this.pic_Msgs.TabIndex = 9;
            this.pic_Msgs.TabStop = false;
            // 
            // pic_Head
            // 
            this.pic_Head.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pic_Head.Dock = System.Windows.Forms.DockStyle.Top;
            this.pic_Head.Location = new System.Drawing.Point(0, 0);
            this.pic_Head.Name = "pic_Head";
            this.pic_Head.Size = new System.Drawing.Size(1024, 90);
            this.pic_Head.TabIndex = 8;
            this.pic_Head.TabStop = false;
            this.pic_Head.DoubleClick += new System.EventHandler(this.pic_Head_DoubleClick);
            // 
            // pic_Brks
            // 
            this.pic_Brks.BackColor = System.Drawing.Color.White;
            this.pic_Brks.Location = new System.Drawing.Point(43, 302);
            this.pic_Brks.Name = "pic_Brks";
            this.pic_Brks.Size = new System.Drawing.Size(773, 405);
            this.pic_Brks.TabIndex = 0;
            this.pic_Brks.TabStop = false;
            // 
            // fomBrake
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.ControlBox = false;
            this.Controls.Add(this.pic_Msgs);
            this.Controls.Add(this.pic_Head);
            this.Controls.Add(this.pic_Brks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fomBrake";
            this.Text = "fom__ABS";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fomBrake_FormClosed);
            this.Load += new System.EventHandler(this.fomBrake_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Msgs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Head)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Brks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_Brks;
        private System.Windows.Forms.PictureBox pic_Head;
        private System.Windows.Forms.PictureBox pic_Msgs;
    }
}