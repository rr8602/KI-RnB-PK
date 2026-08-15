namespace KI_RnB
{
    partial class fom__SST
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
            this.components = new System.ComponentModel.Container();
            this.tmr_SSTs = new System.Windows.Forms.Timer(this.components);
            this.pic_SSTs = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic_SSTs)).BeginInit();
            this.SuspendLayout();
            // 
            // tmr_SSTs
            // 
            this.tmr_SSTs.Tick += new System.EventHandler(this.tmr_SSTs_Tick);
            // 
            // pic_SSTs
            // 
            this.pic_SSTs.BackgroundImage = global::KI_RnB.Properties.Resources.SST_Back1;
            this.pic_SSTs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pic_SSTs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pic_SSTs.Location = new System.Drawing.Point(0, 0);
            this.pic_SSTs.Name = "pic_SSTs";
            this.pic_SSTs.Size = new System.Drawing.Size(1024, 768);
            this.pic_SSTs.TabIndex = 11;
            this.pic_SSTs.TabStop = false;
            this.pic_SSTs.Paint += new System.Windows.Forms.PaintEventHandler(this.pic_SSTs_Paint);
            // 
            // fom__SST
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.pic_SSTs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fom__SST";
            this.Text = "fom__SST";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fom__SST_FormClosed);
            this.Load += new System.EventHandler(this.fom__SST_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_SSTs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_SSTs;
        private System.Windows.Forms.Timer tmr_SSTs;

    }
}