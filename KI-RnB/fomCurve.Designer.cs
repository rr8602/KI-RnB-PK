namespace KI_RnB
{
    partial class fomCurve
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
            this.lbl___04 = new System.Windows.Forms.Label();
            this.btnIInit = new System.Windows.Forms.Button();
            this.btnI_Add = new System.Windows.Forms.Button();
            this.btnIEdit = new System.Windows.Forms.Button();
            this.lbl___07 = new System.Windows.Forms.Label();
            this.btnI_Del = new System.Windows.Forms.Button();
            this.lbl___06 = new System.Windows.Forms.Label();
            this.lbl___05 = new System.Windows.Forms.Label();
            this.lbl___03 = new System.Windows.Forms.Label();
            this.lbl___01 = new System.Windows.Forms.Label();
            this.lbl___00 = new System.Windows.Forms.Label();
            this.txt_Desc = new System.Windows.Forms.TextBox();
            this.txtItems = new System.Windows.Forms.TextBox();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.txt_Time = new System.Windows.Forms.TextBox();
            this.txt_Segm = new System.Windows.Forms.TextBox();
            this.dgvCurve = new System.Windows.Forms.DataGridView();
            this.picGraph = new System.Windows.Forms.PictureBox();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.chk_Item = new System.Windows.Forms.CheckBox();
            this.txtMDB00 = new System.Windows.Forms.TextBox();
            this.txtMDB01 = new System.Windows.Forms.TextBox();
            this.cboMDB02 = new System.Windows.Forms.ComboBox();
            this.cbo_Vehi = new System.Windows.Forms.ComboBox();
            this.cbo_Roll = new System.Windows.Forms.ComboBox();
            this.lst_Item = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl___04
            // 
            this.lbl___04.Location = new System.Drawing.Point(290, 63);
            this.lbl___04.Name = "lbl___04";
            this.lbl___04.Size = new System.Drawing.Size(72, 12);
            this.lbl___04.TabIndex = 21;
            this.lbl___04.Text = "Roll";
            this.lbl___04.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnIInit
            // 
            this.btnIInit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIInit.Location = new System.Drawing.Point(632, 12);
            this.btnIInit.Name = "btnIInit";
            this.btnIInit.Size = new System.Drawing.Size(120, 21);
            this.btnIInit.TabIndex = 7;
            this.btnIInit.Text = "초기화";
            this.btnIInit.UseVisualStyleBackColor = true;
            this.btnIInit.Click += new System.EventHandler(this.btnCurve_Click);
            // 
            // btnI_Add
            // 
            this.btnI_Add.Location = new System.Drawing.Point(12, 11);
            this.btnI_Add.Name = "btnI_Add";
            this.btnI_Add.Size = new System.Drawing.Size(120, 21);
            this.btnI_Add.TabIndex = 8;
            this.btnI_Add.Text = "추가";
            this.btnI_Add.UseVisualStyleBackColor = true;
            this.btnI_Add.Click += new System.EventHandler(this.btnCurve_Click);
            // 
            // btnIEdit
            // 
            this.btnIEdit.Location = new System.Drawing.Point(12, 35);
            this.btnIEdit.Name = "btnIEdit";
            this.btnIEdit.Size = new System.Drawing.Size(120, 21);
            this.btnIEdit.TabIndex = 9;
            this.btnIEdit.Text = "변경";
            this.btnIEdit.UseVisualStyleBackColor = true;
            this.btnIEdit.Click += new System.EventHandler(this.btnCurve_Click);
            // 
            // lbl___07
            // 
            this.lbl___07.Location = new System.Drawing.Point(437, 63);
            this.lbl___07.Name = "lbl___07";
            this.lbl___07.Size = new System.Drawing.Size(101, 12);
            this.lbl___07.TabIndex = 23;
            this.lbl___07.Text = "Description";
            this.lbl___07.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnI_Del
            // 
            this.btnI_Del.Location = new System.Drawing.Point(12, 59);
            this.btnI_Del.Name = "btnI_Del";
            this.btnI_Del.Size = new System.Drawing.Size(120, 21);
            this.btnI_Del.TabIndex = 10;
            this.btnI_Del.Text = "삭제";
            this.btnI_Del.UseVisualStyleBackColor = true;
            this.btnI_Del.Click += new System.EventHandler(this.btnCurve_Click);
            // 
            // lbl___06
            // 
            this.lbl___06.Location = new System.Drawing.Point(437, 39);
            this.lbl___06.Name = "lbl___06";
            this.lbl___06.Size = new System.Drawing.Size(101, 12);
            this.lbl___06.TabIndex = 22;
            this.lbl___06.Text = "Test Items";
            this.lbl___06.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl___05
            // 
            this.lbl___05.Location = new System.Drawing.Point(437, 15);
            this.lbl___05.Name = "lbl___05";
            this.lbl___05.Size = new System.Drawing.Size(101, 12);
            this.lbl___05.TabIndex = 25;
            this.lbl___05.Text = "Speed";
            this.lbl___05.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl___03
            // 
            this.lbl___03.Location = new System.Drawing.Point(290, 43);
            this.lbl___03.Name = "lbl___03";
            this.lbl___03.Size = new System.Drawing.Size(72, 12);
            this.lbl___03.TabIndex = 28;
            this.lbl___03.Text = "Vehicle";
            this.lbl___03.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl___01
            // 
            this.lbl___01.Location = new System.Drawing.Point(130, 41);
            this.lbl___01.Name = "lbl___01";
            this.lbl___01.Size = new System.Drawing.Size(92, 12);
            this.lbl___01.TabIndex = 27;
            this.lbl___01.Text = "Time";
            this.lbl___01.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl___00
            // 
            this.lbl___00.Location = new System.Drawing.Point(130, 17);
            this.lbl___00.Name = "lbl___00";
            this.lbl___00.Size = new System.Drawing.Size(92, 12);
            this.lbl___00.TabIndex = 26;
            this.lbl___00.Text = "Segment";
            this.lbl___00.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_Desc
            // 
            this.txt_Desc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Desc.Location = new System.Drawing.Point(538, 60);
            this.txt_Desc.Name = "txt_Desc";
            this.txt_Desc.Size = new System.Drawing.Size(466, 21);
            this.txt_Desc.TabIndex = 14;
            // 
            // txtItems
            // 
            this.txtItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItems.Location = new System.Drawing.Point(538, 36);
            this.txtItems.Name = "txtItems";
            this.txtItems.Size = new System.Drawing.Size(466, 21);
            this.txtItems.TabIndex = 17;
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(538, 12);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(68, 21);
            this.txtSpeed.TabIndex = 20;
            this.txtSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_Time
            // 
            this.txt_Time.Location = new System.Drawing.Point(223, 36);
            this.txt_Time.Name = "txt_Time";
            this.txt_Time.Size = new System.Drawing.Size(68, 21);
            this.txt_Time.TabIndex = 19;
            this.txt_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_Segm
            // 
            this.txt_Segm.Location = new System.Drawing.Point(223, 12);
            this.txt_Segm.Name = "txt_Segm";
            this.txt_Segm.Size = new System.Drawing.Size(208, 21);
            this.txt_Segm.TabIndex = 18;
            // 
            // dgvCurve
            // 
            this.dgvCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCurve.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCurve.Location = new System.Drawing.Point(12, 87);
            this.dgvCurve.Name = "dgvCurve";
            this.dgvCurve.ReadOnly = true;
            this.dgvCurve.RowTemplate.Height = 23;
            this.dgvCurve.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCurve.Size = new System.Drawing.Size(992, 304);
            this.dgvCurve.TabIndex = 12;
            this.dgvCurve.CurrentCellChanged += new System.EventHandler(this.dgvCurve_CurrentCellChanged);
            this.dgvCurve.Click += new System.EventHandler(this.dgvCurve_Click);
            // 
            // picGraph
            // 
            this.picGraph.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picGraph.BackColor = System.Drawing.Color.White;
            this.picGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGraph.Location = new System.Drawing.Point(12, 397);
            this.picGraph.Name = "picGraph";
            this.picGraph.Size = new System.Drawing.Size(992, 325);
            this.picGraph.TabIndex = 11;
            this.picGraph.TabStop = false;
            // 
            // btn_Save
            // 
            this.btn_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Save.Location = new System.Drawing.Point(758, 12);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(120, 21);
            this.btn_Save.TabIndex = 7;
            this.btn_Save.Text = "저장";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btnCurve_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(884, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 21);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnCurve_Click);
            // 
            // chk_Item
            // 
            this.chk_Item.AutoSize = true;
            this.chk_Item.Location = new System.Drawing.Point(205, 62);
            this.chk_Item.Name = "chk_Item";
            this.chk_Item.Size = new System.Drawing.Size(80, 16);
            this.chk_Item.TabIndex = 30;
            this.chk_Item.Text = "Items Edit";
            this.chk_Item.UseVisualStyleBackColor = true;
            this.chk_Item.CheckedChanged += new System.EventHandler(this.chk_Item_CheckedChanged);
            // 
            // txtMDB00
            // 
            this.txtMDB00.Location = new System.Drawing.Point(12, 87);
            this.txtMDB00.Name = "txtMDB00";
            this.txtMDB00.Size = new System.Drawing.Size(205, 21);
            this.txtMDB00.TabIndex = 15;
            // 
            // txtMDB01
            // 
            this.txtMDB01.Location = new System.Drawing.Point(12, 107);
            this.txtMDB01.Name = "txtMDB01";
            this.txtMDB01.Size = new System.Drawing.Size(67, 21);
            this.txtMDB01.TabIndex = 15;
            this.txtMDB01.Text = "번호";
            this.txtMDB01.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cboMDB02
            // 
            this.cboMDB02.FormattingEnabled = true;
            this.cboMDB02.Location = new System.Drawing.Point(85, 108);
            this.cboMDB02.Name = "cboMDB02";
            this.cboMDB02.Size = new System.Drawing.Size(132, 20);
            this.cboMDB02.TabIndex = 29;
            this.cboMDB02.SelectedIndexChanged += new System.EventHandler(this.cboMDB02_SelectedIndexChanged);
            // 
            // cbo_Vehi
            // 
            this.cbo_Vehi.FormattingEnabled = true;
            this.cbo_Vehi.Location = new System.Drawing.Point(363, 36);
            this.cbo_Vehi.Name = "cbo_Vehi";
            this.cbo_Vehi.Size = new System.Drawing.Size(68, 20);
            this.cbo_Vehi.TabIndex = 31;
            // 
            // cbo_Roll
            // 
            this.cbo_Roll.FormattingEnabled = true;
            this.cbo_Roll.Location = new System.Drawing.Point(363, 61);
            this.cbo_Roll.Name = "cbo_Roll";
            this.cbo_Roll.Size = new System.Drawing.Size(68, 20);
            this.cbo_Roll.TabIndex = 32;
            // 
            // lst_Item
            // 
            this.lst_Item.FormattingEnabled = true;
            this.lst_Item.ItemHeight = 12;
            this.lst_Item.Location = new System.Drawing.Point(12, 87);
            this.lst_Item.Name = "lst_Item";
            this.lst_Item.Size = new System.Drawing.Size(205, 304);
            this.lst_Item.TabIndex = 33;
            this.lst_Item.SelectedIndexChanged += new System.EventHandler(this.lst_Item_SelectedIndexChanged);
            // 
            // fomCurve
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1016, 734);
            this.ControlBox = false;
            this.Controls.Add(this.dgvCurve);
            this.Controls.Add(this.lst_Item);
            this.Controls.Add(this.cbo_Roll);
            this.Controls.Add(this.cbo_Vehi);
            this.Controls.Add(this.cboMDB02);
            this.Controls.Add(this.chk_Item);
            this.Controls.Add(this.txtMDB01);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtMDB00);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.btnIInit);
            this.Controls.Add(this.btnI_Add);
            this.Controls.Add(this.btnIEdit);
            this.Controls.Add(this.btnI_Del);
            this.Controls.Add(this.txt_Desc);
            this.Controls.Add(this.txtItems);
            this.Controls.Add(this.txtSpeed);
            this.Controls.Add(this.txt_Time);
            this.Controls.Add(this.txt_Segm);
            this.Controls.Add(this.picGraph);
            this.Controls.Add(this.lbl___04);
            this.Controls.Add(this.lbl___07);
            this.Controls.Add(this.lbl___06);
            this.Controls.Add(this.lbl___05);
            this.Controls.Add(this.lbl___03);
            this.Controls.Add(this.lbl___01);
            this.Controls.Add(this.lbl___00);
            this.Name = "fomCurve";
            this.Text = "주행 곡선 데이터";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fomCurve_FormClosed);
            this.Load += new System.EventHandler(this.fomCurve_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl___04;
        private System.Windows.Forms.Button btnIInit;
        private System.Windows.Forms.Button btnI_Add;
        private System.Windows.Forms.Button btnIEdit;
        private System.Windows.Forms.Label lbl___07;
        private System.Windows.Forms.Button btnI_Del;
        private System.Windows.Forms.Label lbl___06;
        private System.Windows.Forms.Label lbl___05;
        private System.Windows.Forms.Label lbl___03;
        private System.Windows.Forms.Label lbl___01;
        private System.Windows.Forms.Label lbl___00;
        private System.Windows.Forms.TextBox txt_Desc;
        private System.Windows.Forms.TextBox txtItems;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.TextBox txt_Time;
        private System.Windows.Forms.TextBox txt_Segm;
        private System.Windows.Forms.DataGridView dgvCurve;
        private System.Windows.Forms.PictureBox picGraph;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox chk_Item;
        private System.Windows.Forms.TextBox txtMDB01;
        private System.Windows.Forms.TextBox txtMDB00;
        private System.Windows.Forms.ComboBox cboMDB02;
        private System.Windows.Forms.ComboBox cbo_Vehi;
        private System.Windows.Forms.ComboBox cbo_Roll;
        private System.Windows.Forms.ListBox lst_Item;
    }
}