namespace KI_RnB
{
    partial class fom__WGT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fom__WGT));
            this.picR_Wgt = new System.Windows.Forms.PictureBox();
            this.picL_Wgt = new System.Windows.Forms.PictureBox();
            this.pic_Msgs = new System.Windows.Forms.PictureBox();
            this.pic_Head = new System.Windows.Forms.PictureBox();
            this.pic_WGTs = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picR_Wgt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picL_Wgt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Msgs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Head)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_WGTs)).BeginInit();
            this.SuspendLayout();
            // 
            // picR_Wgt
            // 
            this.picR_Wgt.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picR_Wgt.BackgroundImage")));
            this.picR_Wgt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picR_Wgt.Location = new System.Drawing.Point(736, 337);
            this.picR_Wgt.Name = "picR_Wgt";
            this.picR_Wgt.Size = new System.Drawing.Size(123, 369);
            this.picR_Wgt.TabIndex = 5;
            this.picR_Wgt.TabStop = false;
            // 
            // picL_Wgt
            // 
            this.picL_Wgt.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picL_Wgt.BackgroundImage")));
            this.picL_Wgt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picL_Wgt.Location = new System.Drawing.Point(166, 337);
            this.picL_Wgt.Name = "picL_Wgt";
            this.picL_Wgt.Size = new System.Drawing.Size(123, 369);
            this.picL_Wgt.TabIndex = 4;
            this.picL_Wgt.TabStop = false;
            // 
            // pic_Msgs
            // 
            this.pic_Msgs.BackColor = System.Drawing.Color.Yellow;
            this.pic_Msgs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pic_Msgs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pic_Msgs.Location = new System.Drawing.Point(0, 86);
            this.pic_Msgs.Name = "pic_Msgs";
            this.pic_Msgs.Size = new System.Drawing.Size(1024, 112);
            this.pic_Msgs.TabIndex = 12;
            this.pic_Msgs.TabStop = false;
            // 
            // pic_Head
            // 
            this.pic_Head.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pic_Head.Dock = System.Windows.Forms.DockStyle.Top;
            this.pic_Head.Location = new System.Drawing.Point(0, 0);
            this.pic_Head.Name = "pic_Head";
            this.pic_Head.Size = new System.Drawing.Size(1024, 84);
            this.pic_Head.TabIndex = 11;
            this.pic_Head.TabStop = false;
            this.pic_Head.DoubleClick += new System.EventHandler(this.pic_Head_DoubleClick);
            // 
            // pic_WGTs
            // 
            this.pic_WGTs.BackColor = System.Drawing.Color.White;
            this.pic_WGTs.Location = new System.Drawing.Point(0, 204);
            this.pic_WGTs.Name = "pic_WGTs";
            this.pic_WGTs.Size = new System.Drawing.Size(1024, 564);
            this.pic_WGTs.TabIndex = 10;
            this.pic_WGTs.TabStop = false;
            // 
            // fom__WGT
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.picR_Wgt);
            this.Controls.Add(this.picL_Wgt);
            this.Controls.Add(this.pic_Msgs);
            this.Controls.Add(this.pic_Head);
            this.Controls.Add(this.pic_WGTs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fom__WGT";
            this.Text = "fom__WGT";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fom__WGT_FormClosed);
            this.Load += new System.EventHandler(this.fom__WGT_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picR_Wgt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picL_Wgt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Msgs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Head)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_WGTs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picR_Wgt;
        private System.Windows.Forms.PictureBox picL_Wgt;
        private System.Windows.Forms.PictureBox pic_Msgs;
        private System.Windows.Forms.PictureBox pic_Head;
        private System.Windows.Forms.PictureBox pic_WGTs;
    }
}