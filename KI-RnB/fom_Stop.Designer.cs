namespace KI_RnB
{
    partial class fom_Stop
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.tmr_Stop = new System.Windows.Forms.Timer(this.components);
            this.lbl_PLCs = new System.Windows.Forms.Label();
            this.lblReady = new System.Windows.Forms.Label();
            this.lblLPost = new System.Windows.Forms.Label();
            this.lblFLSRF = new System.Windows.Forms.Label();
            this.lblRPost = new System.Windows.Forms.Label();
            this.lblFRSRF = new System.Windows.Forms.Label();
            this.lblFLMot = new System.Windows.Forms.Label();
            this.lblFRMot = new System.Windows.Forms.Label();
            this.lblFLSRR = new System.Windows.Forms.Label();
            this.lblFRSRR = new System.Windows.Forms.Label();
            this.lblRLSR_ = new System.Windows.Forms.Label();
            this.lblRRSR_ = new System.Windows.Forms.Label();
            this.lblRLMot = new System.Windows.Forms.Label();
            this.lblRRMot = new System.Windows.Forms.Label();
            this.lbl_Flap = new System.Windows.Forms.Label();
            this.lbl_EMGs = new System.Windows.Forms.Label();
            this.lblFSens = new System.Windows.Forms.Label();
            this.lblRSens = new System.Windows.Forms.Label();
            this.lblWBMin = new System.Windows.Forms.Label();
            this.lblWBase = new System.Windows.Forms.Label();
            this.lblWBMax = new System.Windows.Forms.Label();
            this.lbl_WB_L = new System.Windows.Forms.Label();
            this.lbl_WB_R = new System.Windows.Forms.Label();
            this.picEnter = new System.Windows.Forms.PictureBox();
            this.lbl_kimc = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picEnter)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Black;
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Red;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1000, 83);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "정지";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmr_Stop
            // 
            this.tmr_Stop.Tick += new System.EventHandler(this.tmr_Stop_Tick);
            // 
            // lbl_PLCs
            // 
            this.lbl_PLCs.BackColor = System.Drawing.Color.LightGray;
            this.lbl_PLCs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_PLCs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_PLCs.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_PLCs.ForeColor = System.Drawing.Color.Black;
            this.lbl_PLCs.Location = new System.Drawing.Point(12, 94);
            this.lbl_PLCs.Name = "lbl_PLCs";
            this.lbl_PLCs.Size = new System.Drawing.Size(256, 72);
            this.lbl_PLCs.TabIndex = 0;
            this.lbl_PLCs.Text = "PLC";
            this.lbl_PLCs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReady
            // 
            this.lblReady.BackColor = System.Drawing.Color.LightGray;
            this.lblReady.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblReady.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblReady.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblReady.ForeColor = System.Drawing.Color.Black;
            this.lblReady.Location = new System.Drawing.Point(513, 94);
            this.lblReady.Name = "lblReady";
            this.lblReady.Size = new System.Drawing.Size(499, 72);
            this.lblReady.TabIndex = 0;
            this.lblReady.Text = "운전 준비";
            this.lblReady.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLPost
            // 
            this.lblLPost.BackColor = System.Drawing.Color.LightGray;
            this.lblLPost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLPost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblLPost.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLPost.ForeColor = System.Drawing.Color.Black;
            this.lblLPost.Location = new System.Drawing.Point(12, 246);
            this.lblLPost.Name = "lblLPost";
            this.lblLPost.Size = new System.Drawing.Size(499, 72);
            this.lblLPost.TabIndex = 0;
            this.lblLPost.Text = "안전 포스트";
            this.lblLPost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFLSRF
            // 
            this.lblFLSRF.BackColor = System.Drawing.Color.LightGray;
            this.lblFLSRF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFLSRF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFLSRF.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFLSRF.ForeColor = System.Drawing.Color.Black;
            this.lblFLSRF.Location = new System.Drawing.Point(12, 321);
            this.lblFLSRF.Name = "lblFLSRF";
            this.lblFLSRF.Size = new System.Drawing.Size(425, 72);
            this.lblFLSRF.TabIndex = 0;
            this.lblFLSRF.Text = "안전 롤러";
            this.lblFLSRF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRPost
            // 
            this.lblRPost.BackColor = System.Drawing.Color.LightGray;
            this.lblRPost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRPost.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRPost.ForeColor = System.Drawing.Color.Black;
            this.lblRPost.Location = new System.Drawing.Point(513, 246);
            this.lblRPost.Name = "lblRPost";
            this.lblRPost.Size = new System.Drawing.Size(499, 72);
            this.lblRPost.TabIndex = 0;
            this.lblRPost.Text = "안전 포스트";
            this.lblRPost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFRSRF
            // 
            this.lblFRSRF.BackColor = System.Drawing.Color.LightGray;
            this.lblFRSRF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFRSRF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFRSRF.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFRSRF.ForeColor = System.Drawing.Color.Black;
            this.lblFRSRF.Location = new System.Drawing.Point(587, 321);
            this.lblFRSRF.Name = "lblFRSRF";
            this.lblFRSRF.Size = new System.Drawing.Size(425, 72);
            this.lblFRSRF.TabIndex = 0;
            this.lblFRSRF.Text = "안전 롤러";
            this.lblFRSRF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFLMot
            // 
            this.lblFLMot.BackColor = System.Drawing.Color.LightGray;
            this.lblFLMot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFLMot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFLMot.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFLMot.ForeColor = System.Drawing.Color.Black;
            this.lblFLMot.Location = new System.Drawing.Point(12, 393);
            this.lblFLMot.Name = "lblFLMot";
            this.lblFLMot.Size = new System.Drawing.Size(425, 72);
            this.lblFLMot.TabIndex = 0;
            this.lblFLMot.Text = "모터";
            this.lblFLMot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFRMot
            // 
            this.lblFRMot.BackColor = System.Drawing.Color.LightGray;
            this.lblFRMot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFRMot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFRMot.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFRMot.ForeColor = System.Drawing.Color.Black;
            this.lblFRMot.Location = new System.Drawing.Point(587, 393);
            this.lblFRMot.Name = "lblFRMot";
            this.lblFRMot.Size = new System.Drawing.Size(425, 72);
            this.lblFRMot.TabIndex = 0;
            this.lblFRMot.Text = "모터";
            this.lblFRMot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFLSRR
            // 
            this.lblFLSRR.BackColor = System.Drawing.Color.LightGray;
            this.lblFLSRR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFLSRR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFLSRR.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFLSRR.ForeColor = System.Drawing.Color.Black;
            this.lblFLSRR.Location = new System.Drawing.Point(12, 465);
            this.lblFLSRR.Name = "lblFLSRR";
            this.lblFLSRR.Size = new System.Drawing.Size(425, 72);
            this.lblFLSRR.TabIndex = 0;
            this.lblFLSRR.Text = "안전 롤러";
            this.lblFLSRR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFRSRR
            // 
            this.lblFRSRR.BackColor = System.Drawing.Color.LightGray;
            this.lblFRSRR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFRSRR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFRSRR.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFRSRR.ForeColor = System.Drawing.Color.Black;
            this.lblFRSRR.Location = new System.Drawing.Point(587, 465);
            this.lblFRSRR.Name = "lblFRSRR";
            this.lblFRSRR.Size = new System.Drawing.Size(425, 72);
            this.lblFRSRR.TabIndex = 0;
            this.lblFRSRR.Text = "안전 롤러";
            this.lblFRSRR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRLSR_
            // 
            this.lblRLSR_.BackColor = System.Drawing.Color.LightGray;
            this.lblRLSR_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRLSR_.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRLSR_.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRLSR_.ForeColor = System.Drawing.Color.Black;
            this.lblRLSR_.Location = new System.Drawing.Point(12, 612);
            this.lblRLSR_.Name = "lblRLSR_";
            this.lblRLSR_.Size = new System.Drawing.Size(425, 72);
            this.lblRLSR_.TabIndex = 0;
            this.lblRLSR_.Text = "Safty Roller";
            this.lblRLSR_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRRSR_
            // 
            this.lblRRSR_.BackColor = System.Drawing.Color.LightGray;
            this.lblRRSR_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRRSR_.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRRSR_.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRRSR_.ForeColor = System.Drawing.Color.Black;
            this.lblRRSR_.Location = new System.Drawing.Point(587, 612);
            this.lblRRSR_.Name = "lblRRSR_";
            this.lblRRSR_.Size = new System.Drawing.Size(425, 72);
            this.lblRRSR_.TabIndex = 0;
            this.lblRRSR_.Text = "Safty Roller";
            this.lblRRSR_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRLMot
            // 
            this.lblRLMot.BackColor = System.Drawing.Color.LightGray;
            this.lblRLMot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRLMot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRLMot.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRLMot.ForeColor = System.Drawing.Color.Black;
            this.lblRLMot.Location = new System.Drawing.Point(12, 540);
            this.lblRLMot.Name = "lblRLMot";
            this.lblRLMot.Size = new System.Drawing.Size(425, 72);
            this.lblRLMot.TabIndex = 0;
            this.lblRLMot.Text = "모터";
            this.lblRLMot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRRMot
            // 
            this.lblRRMot.BackColor = System.Drawing.Color.LightGray;
            this.lblRRMot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRRMot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRRMot.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRRMot.ForeColor = System.Drawing.Color.Black;
            this.lblRRMot.Location = new System.Drawing.Point(587, 540);
            this.lblRRMot.Name = "lblRRMot";
            this.lblRRMot.Size = new System.Drawing.Size(425, 72);
            this.lblRRMot.TabIndex = 0;
            this.lblRRMot.Text = "모터";
            this.lblRRMot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Flap
            // 
            this.lbl_Flap.BackColor = System.Drawing.Color.LightGray;
            this.lbl_Flap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_Flap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Flap.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Flap.ForeColor = System.Drawing.Color.Black;
            this.lbl_Flap.Location = new System.Drawing.Point(12, 687);
            this.lbl_Flap.Name = "lbl_Flap";
            this.lbl_Flap.Size = new System.Drawing.Size(499, 72);
            this.lbl_Flap.TabIndex = 0;
            this.lbl_Flap.Text = "Exhaust gas flap";
            this.lbl_Flap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EMGs
            // 
            this.lbl_EMGs.BackColor = System.Drawing.Color.LightGray;
            this.lbl_EMGs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_EMGs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_EMGs.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_EMGs.ForeColor = System.Drawing.Color.Black;
            this.lbl_EMGs.Location = new System.Drawing.Point(12, 687);
            this.lbl_EMGs.Name = "lbl_EMGs";
            this.lbl_EMGs.Size = new System.Drawing.Size(1000, 72);
            this.lbl_EMGs.TabIndex = 0;
            this.lbl_EMGs.Text = "비상 정지 스위치";
            this.lbl_EMGs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFSens
            // 
            this.lblFSens.BackColor = System.Drawing.Color.LightGray;
            this.lblFSens.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFSens.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFSens.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFSens.ForeColor = System.Drawing.Color.Black;
            this.lblFSens.Location = new System.Drawing.Point(439, 321);
            this.lblFSens.Name = "lblFSens";
            this.lblFSens.Size = new System.Drawing.Size(146, 216);
            this.lblFSens.TabIndex = 1;
            this.lblFSens.Text = " 광전  센서";
            this.lblFSens.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRSens
            // 
            this.lblRSens.BackColor = System.Drawing.Color.LightGray;
            this.lblRSens.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRSens.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRSens.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRSens.ForeColor = System.Drawing.Color.Black;
            this.lblRSens.Location = new System.Drawing.Point(439, 540);
            this.lblRSens.Name = "lblRSens";
            this.lblRSens.Size = new System.Drawing.Size(146, 144);
            this.lblRSens.TabIndex = 1;
            this.lblRSens.Text = " 광전  센서";
            this.lblRSens.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWBMin
            // 
            this.lblWBMin.BackColor = System.Drawing.Color.LightGray;
            this.lblWBMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWBMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblWBMin.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWBMin.ForeColor = System.Drawing.Color.Black;
            this.lblWBMin.Location = new System.Drawing.Point(12, 170);
            this.lblWBMin.Name = "lblWBMin";
            this.lblWBMin.Size = new System.Drawing.Size(178, 72);
            this.lblWBMin.TabIndex = 0;
            this.lblWBMin.Text = "최소";
            this.lblWBMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWBase
            // 
            this.lblWBase.BackColor = System.Drawing.Color.LightGray;
            this.lblWBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWBase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblWBase.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWBase.ForeColor = System.Drawing.Color.Black;
            this.lblWBase.Location = new System.Drawing.Point(194, 170);
            this.lblWBase.Name = "lblWBase";
            this.lblWBase.Size = new System.Drawing.Size(636, 72);
            this.lblWBase.TabIndex = 0;
            this.lblWBase.Text = "휠베이스";
            this.lblWBase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWBMax
            // 
            this.lblWBMax.BackColor = System.Drawing.Color.LightGray;
            this.lblWBMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWBMax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblWBMax.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWBMax.ForeColor = System.Drawing.Color.Black;
            this.lblWBMax.Location = new System.Drawing.Point(834, 170);
            this.lblWBMax.Name = "lblWBMax";
            this.lblWBMax.Size = new System.Drawing.Size(178, 72);
            this.lblWBMax.TabIndex = 0;
            this.lblWBMax.Text = "최대";
            this.lblWBMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WB_L
            // 
            this.lbl_WB_L.BackColor = System.Drawing.Color.LightGray;
            this.lbl_WB_L.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_WB_L.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_WB_L.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_WB_L.ForeColor = System.Drawing.Color.Black;
            this.lbl_WB_L.Location = new System.Drawing.Point(196, 170);
            this.lbl_WB_L.Name = "lbl_WB_L";
            this.lbl_WB_L.Size = new System.Drawing.Size(72, 72);
            this.lbl_WB_L.TabIndex = 2;
            this.lbl_WB_L.Text = "L";
            this.lbl_WB_L.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WB_R
            // 
            this.lbl_WB_R.BackColor = System.Drawing.Color.LightGray;
            this.lbl_WB_R.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_WB_R.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_WB_R.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_WB_R.ForeColor = System.Drawing.Color.Black;
            this.lbl_WB_R.Location = new System.Drawing.Point(756, 170);
            this.lbl_WB_R.Name = "lbl_WB_R";
            this.lbl_WB_R.Size = new System.Drawing.Size(72, 72);
            this.lbl_WB_R.TabIndex = 3;
            this.lbl_WB_R.Text = "R";
            this.lbl_WB_R.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picEnter
            // 
            this.picEnter.BackgroundImage = global::KI_RnB.Properties.Resources.EnterPos;
            this.picEnter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picEnter.Location = new System.Drawing.Point(591, 348);
            this.picEnter.Name = "picEnter";
            this.picEnter.Size = new System.Drawing.Size(302, 198);
            this.picEnter.TabIndex = 4;
            this.picEnter.TabStop = false;
            // 
            // lbl_kimc
            // 
            this.lbl_kimc.BackColor = System.Drawing.Color.LightGray;
            this.lbl_kimc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_kimc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_kimc.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_kimc.ForeColor = System.Drawing.Color.Black;
            this.lbl_kimc.Location = new System.Drawing.Point(270, 94);
            this.lbl_kimc.Name = "lbl_kimc";
            this.lbl_kimc.Size = new System.Drawing.Size(241, 72);
            this.lbl_kimc.TabIndex = 5;
            this.lbl_kimc.Text = "AD";
            this.lbl_kimc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fom_Stop
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Yellow;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.lbl_kimc);
            this.Controls.Add(this.picEnter);
            this.Controls.Add(this.lbl_WB_R);
            this.Controls.Add(this.lbl_WB_L);
            this.Controls.Add(this.lblRRMot);
            this.Controls.Add(this.lblRSens);
            this.Controls.Add(this.lblFSens);
            this.Controls.Add(this.lblWBase);
            this.Controls.Add(this.lblReady);
            this.Controls.Add(this.lbl_EMGs);
            this.Controls.Add(this.lblFRMot);
            this.Controls.Add(this.lblRRSR_);
            this.Controls.Add(this.lblFRSRR);
            this.Controls.Add(this.lblFRSRF);
            this.Controls.Add(this.lblRPost);
            this.Controls.Add(this.lblFLMot);
            this.Controls.Add(this.lblRLSR_);
            this.Controls.Add(this.lblFLSRR);
            this.Controls.Add(this.lblFLSRF);
            this.Controls.Add(this.lblLPost);
            this.Controls.Add(this.lblWBMax);
            this.Controls.Add(this.lblWBMin);
            this.Controls.Add(this.lbl_PLCs);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lbl_Flap);
            this.Controls.Add(this.lblRLMot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fom_Stop";
            this.Text = "fom_Stop";
            this.Load += new System.EventHandler(this.fom_Stop_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picEnter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Timer tmr_Stop;
        private System.Windows.Forms.Label lbl_PLCs;
        private System.Windows.Forms.Label lblReady;
        private System.Windows.Forms.Label lblLPost;
        private System.Windows.Forms.Label lblFLSRF;
        private System.Windows.Forms.Label lblRPost;
        private System.Windows.Forms.Label lblFRSRF;
        private System.Windows.Forms.Label lblFLMot;
        private System.Windows.Forms.Label lblFRMot;
        private System.Windows.Forms.Label lblFLSRR;
        private System.Windows.Forms.Label lblFRSRR;
        private System.Windows.Forms.Label lblRLSR_;
        private System.Windows.Forms.Label lblRRSR_;
        private System.Windows.Forms.Label lblRLMot;
        private System.Windows.Forms.Label lblRRMot;
        private System.Windows.Forms.Label lbl_Flap;
        private System.Windows.Forms.Label lbl_EMGs;
        private System.Windows.Forms.Label lblFSens;
        private System.Windows.Forms.Label lblRSens;
        private System.Windows.Forms.Label lblWBMin;
        private System.Windows.Forms.Label lblWBase;
        private System.Windows.Forms.Label lblWBMax;
        private System.Windows.Forms.Label lbl_WB_L;
        private System.Windows.Forms.Label lbl_WB_R;
        private System.Windows.Forms.PictureBox picEnter;
        private System.Windows.Forms.Label lbl_kimc;
    }
}