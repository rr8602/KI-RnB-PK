namespace KI_RnB
{
    partial class fom__ASK
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
            this.btn_OK = new System.Windows.Forms.Button();
            this.lbl_Msgs = new System.Windows.Forms.Label();
            this.btn_Pass = new System.Windows.Forms.Button();
            this.tmr_Msgs = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_OK.BackColor = System.Drawing.Color.Black;
            this.btn_OK.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_OK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_OK.Location = new System.Drawing.Point(90, 227);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(261, 62);
            this.btn_OK.TabIndex = 0;
            this.btn_OK.Text = "YES (CHECK)";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // lbl_Msgs
            // 
            this.lbl_Msgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Msgs.BackColor = System.Drawing.Color.Yellow;
            this.lbl_Msgs.Font = new System.Drawing.Font("Verdana", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Msgs.ForeColor = System.Drawing.Color.Red;
            this.lbl_Msgs.Location = new System.Drawing.Point(10, 10);
            this.lbl_Msgs.Name = "lbl_Msgs";
            this.lbl_Msgs.Size = new System.Drawing.Size(717, 195);
            this.lbl_Msgs.TabIndex = 1;
            this.lbl_Msgs.Text = "Communication error\nWould you like to reconnect?";
            this.lbl_Msgs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Pass
            // 
            this.btn_Pass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Pass.BackColor = System.Drawing.Color.Black;
            this.btn_Pass.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Pass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_Pass.Location = new System.Drawing.Point(387, 227);
            this.btn_Pass.Name = "btn_Pass";
            this.btn_Pass.Size = new System.Drawing.Size(261, 62);
            this.btn_Pass.TabIndex = 0;
            this.btn_Pass.Text = "NO (STOP)";
            this.btn_Pass.UseVisualStyleBackColor = false;
            this.btn_Pass.Click += new System.EventHandler(this.btn_Pass_Click);
            // 
            // tmr_Msgs
            // 
            this.tmr_Msgs.Tick += new System.EventHandler(this.tmr_Msgs_Tick);
            // 
            // fom__ASK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(737, 302);
            this.Controls.Add(this.lbl_Msgs);
            this.Controls.Add(this.btn_Pass);
            this.Controls.Add(this.btn_OK);
            this.Name = "fom__ASK";
            this.Text = "ECU ERROR";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fom__ASK_FormClosed);
            this.Load += new System.EventHandler(this.fom__ASK_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label lbl_Msgs;
        private System.Windows.Forms.Button btn_Pass;
        private System.Windows.Forms.Timer tmr_Msgs;
    }
}