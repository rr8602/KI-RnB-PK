namespace KI_RnB
{
    partial class fom_PsWd
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
            this.btn_Pass = new System.Windows.Forms.Button();
            this.txt_Pswd = new System.Windows.Forms.TextBox();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.chk_Pass = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lbl_Msgs
            // 
            this.lbl_Msgs.AutoSize = true;
            this.lbl_Msgs.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Msgs.Location = new System.Drawing.Point(146, 36);
            this.lbl_Msgs.Name = "lbl_Msgs";
            this.lbl_Msgs.Size = new System.Drawing.Size(146, 18);
            this.lbl_Msgs.TabIndex = 0;
            this.lbl_Msgs.Text = "비밀번호를 입력 하세요";
            this.lbl_Msgs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Pass
            // 
            this.btn_Pass.Location = new System.Drawing.Point(90, 151);
            this.btn_Pass.Name = "btn_Pass";
            this.btn_Pass.Size = new System.Drawing.Size(132, 30);
            this.btn_Pass.TabIndex = 1;
            this.btn_Pass.Text = "&OK";
            this.btn_Pass.UseVisualStyleBackColor = true;
            this.btn_Pass.Click += new System.EventHandler(this.btn_Pass_Click);
            // 
            // txt_Pswd
            // 
            this.txt_Pswd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Pswd.Location = new System.Drawing.Point(137, 72);
            this.txt_Pswd.Name = "txt_Pswd";
            this.txt_Pswd.Size = new System.Drawing.Size(189, 26);
            this.txt_Pswd.TabIndex = 2;
            this.txt_Pswd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_Pswd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fom_PsWd_KeyPress);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(240, 151);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(132, 30);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "&Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // chk_Pass
            // 
            this.chk_Pass.AutoSize = true;
            this.chk_Pass.Location = new System.Drawing.Point(137, 104);
            this.chk_Pass.Name = "chk_Pass";
            this.chk_Pass.Size = new System.Drawing.Size(56, 16);
            this.chk_Pass.TabIndex = 4;
            this.chk_Pass.Text = "Show";
            this.chk_Pass.UseVisualStyleBackColor = true;
            this.chk_Pass.CheckedChanged += new System.EventHandler(this.chk_Pass_CheckedChanged);
            // 
            // fom_PsWd
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(462, 206);
            this.Controls.Add(this.chk_Pass);
            this.Controls.Add(this.txt_Pswd);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Pass);
            this.Controls.Add(this.lbl_Msgs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fom_PsWd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Password";
            this.Activated += new System.EventHandler(this.fom_PsWd_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fom_PsWd_FormClosed);
            this.Load += new System.EventHandler(this.fom_PsWd_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fom_PsWd_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Msgs;
        private System.Windows.Forms.Button btn_Pass;
        private System.Windows.Forms.TextBox txt_Pswd;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.CheckBox chk_Pass;
    }
}