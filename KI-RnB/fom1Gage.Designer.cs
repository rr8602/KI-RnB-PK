namespace KI_RnB
{
    partial class fom1Gage
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
            this.tmr_Gage = new System.Windows.Forms.Timer(this.components);
            this.pic_FL_M = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbx_Indi = new System.Windows.Forms.GroupBox();
            this.lblBrake = new System.Windows.Forms.Label();
            this.lbl_Free = new System.Windows.Forms.Label();
            this.lbl_Indi = new System.Windows.Forms.Label();
            this.pic_Indi = new System.Windows.Forms.PictureBox();
            this.gbx_Time = new System.Windows.Forms.GroupBox();
            this.lbl_Time = new System.Windows.Forms.Label();
            this.lblFTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic_FL_M)).BeginInit();
            this.gbx_Indi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Indi)).BeginInit();
            this.gbx_Time.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmr_Gage
            // 
            this.tmr_Gage.Tick += new System.EventHandler(this.tmr_Gage_Tick);
            // 
            // pic_FL_M
            // 
            this.pic_FL_M.BackColor = System.Drawing.Color.Transparent;
            this.pic_FL_M.BackgroundImage = global::KI_RnB.Properties.Resources.Gage_0;
            this.pic_FL_M.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pic_FL_M.Location = new System.Drawing.Point(12, 12);
            this.pic_FL_M.Name = "pic_FL_M";
            this.pic_FL_M.Size = new System.Drawing.Size(744, 744);
            this.pic_FL_M.TabIndex = 5;
            this.pic_FL_M.TabStop = false;
            this.pic_FL_M.Paint += new System.Windows.Forms.PaintEventHandler(this.pic_Gage_Paint);
            this.pic_FL_M.DoubleClick += new System.EventHandler(this.pic_FL_M_DoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(693, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(63, 29);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnCllose_Click);
            // 
            // gbx_Indi
            // 
            this.gbx_Indi.Controls.Add(this.lblBrake);
            this.gbx_Indi.Controls.Add(this.lbl_Free);
            this.gbx_Indi.Controls.Add(this.lbl_Indi);
            this.gbx_Indi.Controls.Add(this.pic_Indi);
            this.gbx_Indi.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbx_Indi.ForeColor = System.Drawing.Color.White;
            this.gbx_Indi.Location = new System.Drawing.Point(764, 204);
            this.gbx_Indi.Name = "gbx_Indi";
            this.gbx_Indi.Size = new System.Drawing.Size(251, 552);
            this.gbx_Indi.TabIndex = 10;
            this.gbx_Indi.TabStop = false;
            this.gbx_Indi.Text = "인디게이터";
            // 
            // lblBrake
            // 
            this.lblBrake.BackColor = System.Drawing.Color.White;
            this.lblBrake.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrake.ForeColor = System.Drawing.Color.Black;
            this.lblBrake.Location = new System.Drawing.Point(131, 26);
            this.lblBrake.Name = "lblBrake";
            this.lblBrake.Size = new System.Drawing.Size(105, 42);
            this.lblBrake.TabIndex = 41;
            this.lblBrake.Text = "브레이크";
            this.lblBrake.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Free
            // 
            this.lbl_Free.BackColor = System.Drawing.Color.White;
            this.lbl_Free.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Free.ForeColor = System.Drawing.Color.Black;
            this.lbl_Free.Location = new System.Drawing.Point(16, 26);
            this.lbl_Free.Name = "lbl_Free";
            this.lbl_Free.Size = new System.Drawing.Size(105, 42);
            this.lbl_Free.TabIndex = 40;
            this.lbl_Free.Text = "구동";
            this.lbl_Free.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Indi
            // 
            this.lbl_Indi.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbl_Indi.BackColor = System.Drawing.Color.Black;
            this.lbl_Indi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_Indi.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Indi.ForeColor = System.Drawing.Color.Lime;
            this.lbl_Indi.Location = new System.Drawing.Point(16, 459);
            this.lbl_Indi.Name = "lbl_Indi";
            this.lbl_Indi.Size = new System.Drawing.Size(220, 82);
            this.lbl_Indi.TabIndex = 4;
            this.lbl_Indi.Text = "0.0";
            this.lbl_Indi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pic_Indi
            // 
            this.pic_Indi.BackColor = System.Drawing.Color.Silver;
            this.pic_Indi.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pic_Indi.Location = new System.Drawing.Point(16, 76);
            this.pic_Indi.Name = "pic_Indi";
            this.pic_Indi.Size = new System.Drawing.Size(220, 379);
            this.pic_Indi.TabIndex = 39;
            this.pic_Indi.TabStop = false;
            // 
            // gbx_Time
            // 
            this.gbx_Time.Controls.Add(this.lbl_Time);
            this.gbx_Time.Controls.Add(this.lblFTime);
            this.gbx_Time.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbx_Time.ForeColor = System.Drawing.Color.White;
            this.gbx_Time.Location = new System.Drawing.Point(764, 12);
            this.gbx_Time.Name = "gbx_Time";
            this.gbx_Time.Size = new System.Drawing.Size(251, 186);
            this.gbx_Time.TabIndex = 10;
            this.gbx_Time.TabStop = false;
            this.gbx_Time.Text = "시간";
            // 
            // lbl_Time
            // 
            this.lbl_Time.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbl_Time.BackColor = System.Drawing.Color.Black;
            this.lbl_Time.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_Time.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Time.ForeColor = System.Drawing.Color.Lime;
            this.lbl_Time.Location = new System.Drawing.Point(16, 22);
            this.lbl_Time.Name = "lbl_Time";
            this.lbl_Time.Size = new System.Drawing.Size(220, 72);
            this.lbl_Time.TabIndex = 6;
            this.lbl_Time.Text = "0.0";
            this.lbl_Time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFTime
            // 
            this.lblFTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblFTime.BackColor = System.Drawing.Color.Black;
            this.lblFTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFTime.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFTime.ForeColor = System.Drawing.Color.Yellow;
            this.lblFTime.Location = new System.Drawing.Point(16, 98);
            this.lblFTime.Name = "lblFTime";
            this.lblFTime.Size = new System.Drawing.Size(220, 72);
            this.lblFTime.TabIndex = 5;
            this.lblFTime.Text = "0.0";
            this.lblFTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fom1Gage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1027, 768);
            this.Controls.Add(this.gbx_Time);
            this.Controls.Add(this.gbx_Indi);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pic_FL_M);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fom1Gage";
            this.Text = "fom1Gage";
            this.Load += new System.EventHandler(this.fom1Gage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_FL_M)).EndInit();
            this.gbx_Indi.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Indi)).EndInit();
            this.gbx_Time.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmr_Gage;
        private System.Windows.Forms.PictureBox pic_FL_M;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbx_Indi;
        private System.Windows.Forms.Label lbl_Indi;
        private System.Windows.Forms.PictureBox pic_Indi;
        private System.Windows.Forms.GroupBox gbx_Time;
        private System.Windows.Forms.Label lbl_Time;
        private System.Windows.Forms.Label lblFTime;
        private System.Windows.Forms.Label lblBrake;
        private System.Windows.Forms.Label lbl_Free;
    }
}