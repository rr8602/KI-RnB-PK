namespace KI_RnB
{
    partial class uct_Sett
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_Set1 = new System.Windows.Forms.TextBox();
            this.lbl_1 = new System.Windows.Forms.Label();
            this.txt_Set2 = new System.Windows.Forms.TextBox();
            this.lbl_2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_Set1
            // 
            this.txt_Set1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_Set1.Location = new System.Drawing.Point(89, 3);
            this.txt_Set1.Name = "txt_Set1";
            this.txt_Set1.Size = new System.Drawing.Size(146, 21);
            this.txt_Set1.TabIndex = 1;
            // 
            // lbl_1
            // 
            this.lbl_1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_1.Location = new System.Drawing.Point(3, 6);
            this.lbl_1.Name = "lbl_1";
            this.lbl_1.Size = new System.Drawing.Size(80, 12);
            this.lbl_1.TabIndex = 2;
            this.lbl_1.Text = "label1";
            this.lbl_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_Set2
            // 
            this.txt_Set2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txt_Set2.Location = new System.Drawing.Point(346, 3);
            this.txt_Set2.Name = "txt_Set2";
            this.txt_Set2.Size = new System.Drawing.Size(77, 21);
            this.txt_Set2.TabIndex = 1;
            // 
            // lbl_2
            // 
            this.lbl_2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_2.AutoSize = true;
            this.lbl_2.Location = new System.Drawing.Point(302, 6);
            this.lbl_2.Name = "lbl_2";
            this.lbl_2.Size = new System.Drawing.Size(38, 12);
            this.lbl_2.TabIndex = 2;
            this.lbl_2.Text = "label1";
            // 
            // uct_Sett
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_2);
            this.Controls.Add(this.lbl_1);
            this.Controls.Add(this.txt_Set2);
            this.Controls.Add(this.txt_Set1);
            this.Name = "uct_Sett";
            this.Size = new System.Drawing.Size(437, 30);
            this.Load += new System.EventHandler(this.uct_Sett_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Set1;
        private System.Windows.Forms.Label lbl_1;
        private System.Windows.Forms.TextBox txt_Set2;
        private System.Windows.Forms.Label lbl_2;
    }
}
