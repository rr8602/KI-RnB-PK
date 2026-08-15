namespace KI_RnB
{
    partial class fom4Gage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fom4Gage));
            this.btnClose = new System.Windows.Forms.Button();
            this.tmr_Gage = new System.Windows.Forms.Timer(this.components);
            this.lbl_FL = new System.Windows.Forms.Label();
            this.lbl_FR = new System.Windows.Forms.Label();
            this.lbl_RL = new System.Windows.Forms.Label();
            this.lbl_RR = new System.Windows.Forms.Label();
            this.pic_RR_M = new System.Windows.Forms.PictureBox();
            this.pic_FR_M = new System.Windows.Forms.PictureBox();
            this.pic_FL_M = new System.Windows.Forms.PictureBox();
            this.pic_RL_M = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lbl_FL_1 = new System.Windows.Forms.Label();
            this.lbl_FL_2 = new System.Windows.Forms.Label();
            this.lbl_FL_3 = new System.Windows.Forms.Label();
            this.lbl_FR_1 = new System.Windows.Forms.Label();
            this.lbl_FR_2 = new System.Windows.Forms.Label();
            this.lbl_FR_3 = new System.Windows.Forms.Label();
            this.lbl_RL_1 = new System.Windows.Forms.Label();
            this.lbl_RL_2 = new System.Windows.Forms.Label();
            this.lbl_RR_1 = new System.Windows.Forms.Label();
            this.lbl_RL_3 = new System.Windows.Forms.Label();
            this.lbl_RR_2 = new System.Windows.Forms.Label();
            this.lbl_RR_3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic_RR_M)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_FR_M)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_FL_M)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_RL_M)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(960, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(63, 29);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tmr_Gage
            // 
            this.tmr_Gage.Tick += new System.EventHandler(this.tmr_Gage_Tick);
            // 
            // lbl_FL
            // 
            this.lbl_FL.BackColor = System.Drawing.Color.White;
            this.lbl_FL.Location = new System.Drawing.Point(341, 318);
            this.lbl_FL.Name = "lbl_FL";
            this.lbl_FL.Size = new System.Drawing.Size(166, 53);
            this.lbl_FL.TabIndex = 4;
            this.lbl_FL.Text = "0.0";
            this.lbl_FL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_FR
            // 
            this.lbl_FR.BackColor = System.Drawing.Color.White;
            this.lbl_FR.Location = new System.Drawing.Point(513, 318);
            this.lbl_FR.Name = "lbl_FR";
            this.lbl_FR.Size = new System.Drawing.Size(166, 53);
            this.lbl_FR.TabIndex = 4;
            this.lbl_FR.Text = "0.0";
            this.lbl_FR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_RL
            // 
            this.lbl_RL.BackColor = System.Drawing.Color.White;
            this.lbl_RL.Location = new System.Drawing.Point(341, 397);
            this.lbl_RL.Name = "lbl_RL";
            this.lbl_RL.Size = new System.Drawing.Size(166, 53);
            this.lbl_RL.TabIndex = 4;
            this.lbl_RL.Text = "0.0";
            this.lbl_RL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_RR
            // 
            this.lbl_RR.BackColor = System.Drawing.Color.White;
            this.lbl_RR.Location = new System.Drawing.Point(513, 397);
            this.lbl_RR.Name = "lbl_RR";
            this.lbl_RR.Size = new System.Drawing.Size(166, 53);
            this.lbl_RR.TabIndex = 4;
            this.lbl_RR.Text = "0.0";
            this.lbl_RR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pic_RR_M
            // 
            this.pic_RR_M.BackColor = System.Drawing.Color.Transparent;
            this.pic_RR_M.BackgroundImage = global::KI_RnB.Properties.Resources.Gage_0;
            this.pic_RR_M.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pic_RR_M.Location = new System.Drawing.Point(650, 393);
            this.pic_RR_M.Name = "pic_RR_M";
            this.pic_RR_M.Size = new System.Drawing.Size(370, 370);
            this.pic_RR_M.TabIndex = 1;
            this.pic_RR_M.TabStop = false;
            this.pic_RR_M.Paint += new System.Windows.Forms.PaintEventHandler(this.pic_Gage_Paint);
            // 
            // pic_FR_M
            // 
            this.pic_FR_M.BackColor = System.Drawing.Color.Transparent;
            this.pic_FR_M.BackgroundImage = global::KI_RnB.Properties.Resources.Gage_0;
            this.pic_FR_M.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pic_FR_M.Location = new System.Drawing.Point(650, 5);
            this.pic_FR_M.Name = "pic_FR_M";
            this.pic_FR_M.Size = new System.Drawing.Size(370, 370);
            this.pic_FR_M.TabIndex = 1;
            this.pic_FR_M.TabStop = false;
            this.pic_FR_M.Paint += new System.Windows.Forms.PaintEventHandler(this.pic_Gage_Paint);
            // 
            // pic_FL_M
            // 
            this.pic_FL_M.BackColor = System.Drawing.Color.Transparent;
            this.pic_FL_M.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pic_FL_M.BackgroundImage")));
            this.pic_FL_M.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pic_FL_M.Location = new System.Drawing.Point(5, 5);
            this.pic_FL_M.Name = "pic_FL_M";
            this.pic_FL_M.Size = new System.Drawing.Size(370, 370);
            this.pic_FL_M.TabIndex = 1;
            this.pic_FL_M.TabStop = false;
            this.pic_FL_M.Paint += new System.Windows.Forms.PaintEventHandler(this.pic_Gage_Paint);
            // 
            // pic_RL_M
            // 
            this.pic_RL_M.BackColor = System.Drawing.Color.Transparent;
            this.pic_RL_M.BackgroundImage = global::KI_RnB.Properties.Resources.Gage_0;
            this.pic_RL_M.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pic_RL_M.Location = new System.Drawing.Point(5, 393);
            this.pic_RL_M.Name = "pic_RL_M";
            this.pic_RL_M.Size = new System.Drawing.Size(370, 370);
            this.pic_RL_M.TabIndex = 1;
            this.pic_RL_M.TabStop = false;
            this.pic_RL_M.Paint += new System.Windows.Forms.PaintEventHandler(this.pic_Gage_Paint);
            // 
            // lblTitle
            // 
            this.lblTitle.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.lblTitle.Location = new System.Drawing.Point(381, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(263, 128);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "Loss Calibration";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_FL_1
            // 
            this.lbl_FL_1.BackColor = System.Drawing.Color.White;
            this.lbl_FL_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FL_1.Location = new System.Drawing.Point(390, 144);
            this.lbl_FL_1.Name = "lbl_FL_1";
            this.lbl_FL_1.Size = new System.Drawing.Size(92, 42);
            this.lbl_FL_1.TabIndex = 6;
            this.lbl_FL_1.Text = "구동";
            this.lbl_FL_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_FL_2
            // 
            this.lbl_FL_2.BackColor = System.Drawing.Color.White;
            this.lbl_FL_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FL_2.Location = new System.Drawing.Point(390, 198);
            this.lbl_FL_2.Name = "lbl_FL_2";
            this.lbl_FL_2.Size = new System.Drawing.Size(92, 42);
            this.lbl_FL_2.TabIndex = 6;
            this.lbl_FL_2.Text = "프리";
            this.lbl_FL_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_FL_3
            // 
            this.lbl_FL_3.BackColor = System.Drawing.Color.White;
            this.lbl_FL_3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FL_3.Location = new System.Drawing.Point(390, 252);
            this.lbl_FL_3.Name = "lbl_FL_3";
            this.lbl_FL_3.Size = new System.Drawing.Size(92, 42);
            this.lbl_FL_3.TabIndex = 6;
            this.lbl_FL_3.Text = "브레이크";
            this.lbl_FL_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_FR_1
            // 
            this.lbl_FR_1.BackColor = System.Drawing.Color.White;
            this.lbl_FR_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FR_1.Location = new System.Drawing.Point(543, 144);
            this.lbl_FR_1.Name = "lbl_FR_1";
            this.lbl_FR_1.Size = new System.Drawing.Size(92, 42);
            this.lbl_FR_1.TabIndex = 6;
            this.lbl_FR_1.Text = "구동";
            this.lbl_FR_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_FR_2
            // 
            this.lbl_FR_2.BackColor = System.Drawing.Color.White;
            this.lbl_FR_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FR_2.Location = new System.Drawing.Point(543, 198);
            this.lbl_FR_2.Name = "lbl_FR_2";
            this.lbl_FR_2.Size = new System.Drawing.Size(92, 42);
            this.lbl_FR_2.TabIndex = 6;
            this.lbl_FR_2.Text = "프리";
            this.lbl_FR_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_FR_3
            // 
            this.lbl_FR_3.BackColor = System.Drawing.Color.White;
            this.lbl_FR_3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FR_3.Location = new System.Drawing.Point(543, 252);
            this.lbl_FR_3.Name = "lbl_FR_3";
            this.lbl_FR_3.Size = new System.Drawing.Size(92, 42);
            this.lbl_FR_3.TabIndex = 6;
            this.lbl_FR_3.Text = "브레이크";
            this.lbl_FR_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_RL_1
            // 
            this.lbl_RL_1.BackColor = System.Drawing.Color.White;
            this.lbl_RL_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RL_1.Location = new System.Drawing.Point(390, 499);
            this.lbl_RL_1.Name = "lbl_RL_1";
            this.lbl_RL_1.Size = new System.Drawing.Size(92, 42);
            this.lbl_RL_1.TabIndex = 6;
            this.lbl_RL_1.Text = "구동";
            this.lbl_RL_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_RL_2
            // 
            this.lbl_RL_2.BackColor = System.Drawing.Color.White;
            this.lbl_RL_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RL_2.Location = new System.Drawing.Point(390, 553);
            this.lbl_RL_2.Name = "lbl_RL_2";
            this.lbl_RL_2.Size = new System.Drawing.Size(92, 42);
            this.lbl_RL_2.TabIndex = 6;
            this.lbl_RL_2.Text = "프리";
            this.lbl_RL_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_RR_1
            // 
            this.lbl_RR_1.BackColor = System.Drawing.Color.White;
            this.lbl_RR_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RR_1.Location = new System.Drawing.Point(543, 499);
            this.lbl_RR_1.Name = "lbl_RR_1";
            this.lbl_RR_1.Size = new System.Drawing.Size(92, 42);
            this.lbl_RR_1.TabIndex = 6;
            this.lbl_RR_1.Text = "구동";
            this.lbl_RR_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_RL_3
            // 
            this.lbl_RL_3.BackColor = System.Drawing.Color.White;
            this.lbl_RL_3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RL_3.Location = new System.Drawing.Point(390, 607);
            this.lbl_RL_3.Name = "lbl_RL_3";
            this.lbl_RL_3.Size = new System.Drawing.Size(92, 42);
            this.lbl_RL_3.TabIndex = 6;
            this.lbl_RL_3.Text = "브레이크";
            this.lbl_RL_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_RR_2
            // 
            this.lbl_RR_2.BackColor = System.Drawing.Color.White;
            this.lbl_RR_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RR_2.Location = new System.Drawing.Point(543, 553);
            this.lbl_RR_2.Name = "lbl_RR_2";
            this.lbl_RR_2.Size = new System.Drawing.Size(92, 42);
            this.lbl_RR_2.TabIndex = 6;
            this.lbl_RR_2.Text = "프리";
            this.lbl_RR_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_RR_3
            // 
            this.lbl_RR_3.BackColor = System.Drawing.Color.White;
            this.lbl_RR_3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RR_3.Location = new System.Drawing.Point(543, 607);
            this.lbl_RR_3.Name = "lbl_RR_3";
            this.lbl_RR_3.Size = new System.Drawing.Size(92, 42);
            this.lbl_RR_3.TabIndex = 6;
            this.lbl_RR_3.Text = "브레이크";
            this.lbl_RR_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fom4Gage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1024, 749);
            this.Controls.Add(this.lbl_RR_3);
            this.Controls.Add(this.lbl_FR_3);
            this.Controls.Add(this.lbl_RR_2);
            this.Controls.Add(this.lbl_FR_2);
            this.Controls.Add(this.lbl_RL_3);
            this.Controls.Add(this.lbl_FL_3);
            this.Controls.Add(this.lbl_RR_1);
            this.Controls.Add(this.lbl_FR_1);
            this.Controls.Add(this.lbl_RL_2);
            this.Controls.Add(this.lbl_FL_2);
            this.Controls.Add(this.lbl_RL_1);
            this.Controls.Add(this.lbl_FL_1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lbl_RR);
            this.Controls.Add(this.lbl_FR);
            this.Controls.Add(this.lbl_RL);
            this.Controls.Add(this.lbl_FL);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pic_RR_M);
            this.Controls.Add(this.pic_FR_M);
            this.Controls.Add(this.pic_FL_M);
            this.Controls.Add(this.pic_RL_M);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fom4Gage";
            this.Text = "fomPanel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fomPanel_FormClosing);
            this.Load += new System.EventHandler(this.fomPanel_Load);
            this.DoubleClick += new System.EventHandler(this.fom4Gage_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.pic_RR_M)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_FR_M)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_FL_M)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_RL_M)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_FL_M;
        private System.Windows.Forms.PictureBox pic_FR_M;
        private System.Windows.Forms.PictureBox pic_RL_M;
        private System.Windows.Forms.PictureBox pic_RR_M;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Timer tmr_Gage;
        private System.Windows.Forms.Label lbl_FL;
        private System.Windows.Forms.Label lbl_FR;
        private System.Windows.Forms.Label lbl_RL;
        private System.Windows.Forms.Label lbl_RR;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lbl_FL_1;
        private System.Windows.Forms.Label lbl_FL_2;
        private System.Windows.Forms.Label lbl_FL_3;
        private System.Windows.Forms.Label lbl_FR_1;
        private System.Windows.Forms.Label lbl_FR_2;
        private System.Windows.Forms.Label lbl_FR_3;
        private System.Windows.Forms.Label lbl_RL_1;
        private System.Windows.Forms.Label lbl_RL_2;
        private System.Windows.Forms.Label lbl_RR_1;
        private System.Windows.Forms.Label lbl_RL_3;
        private System.Windows.Forms.Label lbl_RR_2;
        private System.Windows.Forms.Label lbl_RR_3;
    }
}