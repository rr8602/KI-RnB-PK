namespace KI_RnB
{
    partial class fomInput
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
            this.lbl_Msgs = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.lbl_Unit = new System.Windows.Forms.Label();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chk_Show = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lbl_Msgs
            // 
            this.lbl_Msgs.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Msgs.Location = new System.Drawing.Point(12, 9);
            this.lbl_Msgs.Name = "lbl_Msgs";
            this.lbl_Msgs.Size = new System.Drawing.Size(363, 72);
            this.lbl_Msgs.TabIndex = 0;
            this.lbl_Msgs.Text = "Please enter the weight";
            this.lbl_Msgs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtInput.Location = new System.Drawing.Point(114, 84);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(158, 39);
            this.txtInput.TabIndex = 1;
            this.txtInput.Text = "0";
            this.txtInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fomInput_KeyDown);
            // 
            // lbl_Unit
            // 
            this.lbl_Unit.AutoSize = true;
            this.lbl_Unit.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Unit.Location = new System.Drawing.Point(278, 92);
            this.lbl_Unit.Name = "lbl_Unit";
            this.lbl_Unit.Size = new System.Drawing.Size(44, 27);
            this.lbl_Unit.TabIndex = 2;
            this.lbl_Unit.Text = "kg";
            // 
            // btn_OK
            // 
            this.btn_OK.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OK.Location = new System.Drawing.Point(62, 169);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(128, 49);
            this.btn_OK.TabIndex = 3;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.Bottons_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.Location = new System.Drawing.Point(196, 169);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(128, 49);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.Bottons_Click);
            // 
            // chk_Show
            // 
            this.chk_Show.AutoSize = true;
            this.chk_Show.Location = new System.Drawing.Point(114, 130);
            this.chk_Show.Name = "chk_Show";
            this.chk_Show.Size = new System.Drawing.Size(112, 16);
            this.chk_Show.TabIndex = 4;
            this.chk_Show.Text = "View password";
            this.chk_Show.UseVisualStyleBackColor = true;
            this.chk_Show.CheckedChanged += new System.EventHandler(this.chk_Show_CheckedChanged);
            // 
            // fomInput
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(387, 230);
            this.ControlBox = false;
            this.Controls.Add(this.chk_Show);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.lbl_Unit);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lbl_Msgs);
            this.Name = "fomInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Input";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fomInput_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Msgs;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label lbl_Unit;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chk_Show;
    }
}