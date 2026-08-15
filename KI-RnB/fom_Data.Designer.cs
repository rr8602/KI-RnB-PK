namespace KI_RnB
{
    partial class fom_Data
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
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtp__End = new System.Windows.Forms.DateTimePicker();
            this.btn_Find = new System.Windows.Forms.Button();
            this.dgv_List = new System.Windows.Forms.DataGridView();
            this.gbx_Data = new System.Windows.Forms.GroupBox();
            this.cbo_Days = new System.Windows.Forms.ComboBox();
            this.cboModel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExcel = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pgb_Data = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_List)).BeginInit();
            this.gbx_Data.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(378, 19);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(157, 21);
            this.dtpStart.TabIndex = 1;
            // 
            // dtp__End
            // 
            this.dtp__End.Location = new System.Drawing.Point(554, 19);
            this.dtp__End.Name = "dtp__End";
            this.dtp__End.Size = new System.Drawing.Size(157, 21);
            this.dtp__End.TabIndex = 1;
            // 
            // btn_Find
            // 
            this.btn_Find.Location = new System.Drawing.Point(717, 17);
            this.btn_Find.Name = "btn_Find";
            this.btn_Find.Size = new System.Drawing.Size(75, 23);
            this.btn_Find.TabIndex = 2;
            this.btn_Find.Text = "Search";
            this.btn_Find.UseVisualStyleBackColor = true;
            this.btn_Find.Click += new System.EventHandler(this.btn_Search);
            // 
            // dgv_List
            // 
            this.dgv_List.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_List.Location = new System.Drawing.Point(12, 69);
            this.dgv_List.Name = "dgv_List";
            this.dgv_List.RowTemplate.Height = 23;
            this.dgv_List.Size = new System.Drawing.Size(984, 649);
            this.dgv_List.TabIndex = 3;
            this.dgv_List.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgv_List_RowPostPaint);
            // 
            // gbx_Data
            // 
            this.gbx_Data.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbx_Data.Controls.Add(this.cbo_Days);
            this.gbx_Data.Controls.Add(this.cboModel);
            this.gbx_Data.Controls.Add(this.label1);
            this.gbx_Data.Controls.Add(this.btnExcel);
            this.gbx_Data.Controls.Add(this.btn_Find);
            this.gbx_Data.Controls.Add(this.dtp__End);
            this.gbx_Data.Controls.Add(this.dtpStart);
            this.gbx_Data.Location = new System.Drawing.Point(12, 12);
            this.gbx_Data.Name = "gbx_Data";
            this.gbx_Data.Size = new System.Drawing.Size(984, 51);
            this.gbx_Data.TabIndex = 4;
            this.gbx_Data.TabStop = false;
            this.gbx_Data.Text = "Data search condition";
            // 
            // cbo_Days
            // 
            this.cbo_Days.FormattingEnabled = true;
            this.cbo_Days.Location = new System.Drawing.Point(225, 19);
            this.cbo_Days.Name = "cbo_Days";
            this.cbo_Days.Size = new System.Drawing.Size(147, 20);
            this.cbo_Days.TabIndex = 4;
            this.cbo_Days.Click += new System.EventHandler(this.cbo_Days_Click);
            // 
            // cboModel
            // 
            this.cboModel.FormattingEnabled = true;
            this.cboModel.Location = new System.Drawing.Point(6, 19);
            this.cboModel.Name = "cboModel";
            this.cboModel.Size = new System.Drawing.Size(213, 20);
            this.cboModel.TabIndex = 4;
            this.cboModel.Click += new System.EventHandler(this.cboModel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(537, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "~";
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(872, 14);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(106, 23);
            this.btnExcel.TabIndex = 2;
            this.btnExcel.Text = "Excel Save";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btn_Search);
            // 
            // pgb_Data
            // 
            this.pgb_Data.Location = new System.Drawing.Point(189, 339);
            this.pgb_Data.Name = "pgb_Data";
            this.pgb_Data.Size = new System.Drawing.Size(630, 50);
            this.pgb_Data.TabIndex = 5;
            this.pgb_Data.Visible = false;
            // 
            // fom_Data
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.pgb_Data);
            this.Controls.Add(this.gbx_Data);
            this.Controls.Add(this.dgv_List);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fom_Data";
            this.Text = "Test Data";
            this.Load += new System.EventHandler(this.fom_Data_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_List)).EndInit();
            this.gbx_Data.ResumeLayout(false);
            this.gbx_Data.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtp__End;
        private System.Windows.Forms.Button btn_Find;
        private System.Windows.Forms.DataGridView dgv_List;
        private System.Windows.Forms.GroupBox gbx_Data;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar pgb_Data;
        private System.Windows.Forms.ComboBox cboModel;
        private System.Windows.Forms.ComboBox cbo_Days;

    }
}