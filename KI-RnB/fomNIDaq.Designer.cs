namespace KI_RnB
{
    partial class fomNIDaq
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
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.zIndexPhaseLabel = new System.Windows.Forms.Label();
            this.zIndexPhaseComboBox = new System.Windows.Forms.ComboBox();
            this.decodingTypeLabel = new System.Windows.Forms.Label();
            this.pulsesPerRevLabel = new System.Windows.Forms.Label();
            this.zIndexValueLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.pulsePerRevTextBox = new System.Windows.Forms.TextBox();
            this.zIndexValueTextBox = new System.Windows.Forms.TextBox();
            this.decodingTypeComboBox = new System.Windows.Forms.ComboBox();
            this.zIndexEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.sampleClkSourceLabel = new System.Windows.Forms.Label();
            this.txt_RR_1 = new System.Windows.Forms.TextBox();
            this.txt_RR_0 = new System.Windows.Forms.TextBox();
            this.txt_RL_1 = new System.Windows.Forms.TextBox();
            this.txt_RL_0 = new System.Windows.Forms.TextBox();
            this.txt_FR_1 = new System.Windows.Forms.TextBox();
            this.txt_FR_0 = new System.Windows.Forms.TextBox();
            this.txt_FL_1 = new System.Windows.Forms.TextBox();
            this.txt_FL_0 = new System.Windows.Forms.TextBox();
            this.samplesToReadTextBox = new System.Windows.Forms.TextBox();
            this.samplesToReadLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.rateTextBox = new System.Windows.Forms.TextBox();
            this.channelParametersGroupBox.SuspendLayout();
            this.timingParametersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.counterComboBox);
            this.channelParametersGroupBox.Controls.Add(this.zIndexPhaseLabel);
            this.channelParametersGroupBox.Controls.Add(this.zIndexPhaseComboBox);
            this.channelParametersGroupBox.Controls.Add(this.decodingTypeLabel);
            this.channelParametersGroupBox.Controls.Add(this.pulsesPerRevLabel);
            this.channelParametersGroupBox.Controls.Add(this.zIndexValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.pulsePerRevTextBox);
            this.channelParametersGroupBox.Controls.Add(this.zIndexValueTextBox);
            this.channelParametersGroupBox.Controls.Add(this.decodingTypeComboBox);
            this.channelParametersGroupBox.Controls.Add(this.zIndexEnabledCheckBox);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(12, 12);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(326, 241);
            this.channelParametersGroupBox.TabIndex = 4;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(163, 26);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(145, 20);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // zIndexPhaseLabel
            // 
            this.zIndexPhaseLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.zIndexPhaseLabel.Location = new System.Drawing.Point(14, 172);
            this.zIndexPhaseLabel.Name = "zIndexPhaseLabel";
            this.zIndexPhaseLabel.Size = new System.Drawing.Size(111, 18);
            this.zIndexPhaseLabel.TabIndex = 7;
            this.zIndexPhaseLabel.Text = "Z Index Phase:";
            // 
            // zIndexPhaseComboBox
            // 
            this.zIndexPhaseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zIndexPhaseComboBox.Items.AddRange(new object[] {
            "A High B High",
            "A High B Low",
            "A Low B High",
            "A Low B Low"});
            this.zIndexPhaseComboBox.Location = new System.Drawing.Point(163, 172);
            this.zIndexPhaseComboBox.Name = "zIndexPhaseComboBox";
            this.zIndexPhaseComboBox.Size = new System.Drawing.Size(144, 20);
            this.zIndexPhaseComboBox.TabIndex = 8;
            // 
            // decodingTypeLabel
            // 
            this.decodingTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.decodingTypeLabel.Location = new System.Drawing.Point(14, 103);
            this.decodingTypeLabel.Name = "decodingTypeLabel";
            this.decodingTypeLabel.Size = new System.Drawing.Size(135, 18);
            this.decodingTypeLabel.TabIndex = 3;
            this.decodingTypeLabel.Text = "Decoding Type:";
            // 
            // pulsesPerRevLabel
            // 
            this.pulsesPerRevLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pulsesPerRevLabel.Location = new System.Drawing.Point(14, 207);
            this.pulsesPerRevLabel.Name = "pulsesPerRevLabel";
            this.pulsesPerRevLabel.Size = new System.Drawing.Size(144, 17);
            this.pulsesPerRevLabel.TabIndex = 9;
            this.pulsesPerRevLabel.Text = "Pulses per Revolution:";
            // 
            // zIndexValueLabel
            // 
            this.zIndexValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.zIndexValueLabel.Location = new System.Drawing.Point(14, 138);
            this.zIndexValueLabel.Name = "zIndexValueLabel";
            this.zIndexValueLabel.Size = new System.Drawing.Size(144, 19);
            this.zIndexValueLabel.TabIndex = 5;
            this.zIndexValueLabel.Text = "Z Index Value:";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(14, 28);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(116, 17);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Counter(s):";
            // 
            // pulsePerRevTextBox
            // 
            this.pulsePerRevTextBox.Location = new System.Drawing.Point(163, 207);
            this.pulsePerRevTextBox.Name = "pulsePerRevTextBox";
            this.pulsePerRevTextBox.Size = new System.Drawing.Size(144, 21);
            this.pulsePerRevTextBox.TabIndex = 10;
            this.pulsePerRevTextBox.Text = "24";
            // 
            // zIndexValueTextBox
            // 
            this.zIndexValueTextBox.Location = new System.Drawing.Point(163, 138);
            this.zIndexValueTextBox.Name = "zIndexValueTextBox";
            this.zIndexValueTextBox.Size = new System.Drawing.Size(144, 21);
            this.zIndexValueTextBox.TabIndex = 6;
            this.zIndexValueTextBox.Text = "0";
            // 
            // decodingTypeComboBox
            // 
            this.decodingTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.decodingTypeComboBox.Items.AddRange(new object[] {
            "X1",
            "X2",
            "X4"});
            this.decodingTypeComboBox.Location = new System.Drawing.Point(163, 103);
            this.decodingTypeComboBox.Name = "decodingTypeComboBox";
            this.decodingTypeComboBox.Size = new System.Drawing.Size(144, 20);
            this.decodingTypeComboBox.TabIndex = 4;
            // 
            // zIndexEnabledCheckBox
            // 
            this.zIndexEnabledCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.zIndexEnabledCheckBox.Location = new System.Drawing.Point(163, 60);
            this.zIndexEnabledCheckBox.Name = "zIndexEnabledCheckBox";
            this.zIndexEnabledCheckBox.Size = new System.Drawing.Size(144, 26);
            this.zIndexEnabledCheckBox.TabIndex = 2;
            this.zIndexEnabledCheckBox.Text = "Z Index Enabled";
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(344, 21);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(112, 36);
            this.btn_Save.TabIndex = 6;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(344, 62);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(112, 36);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.label3);
            this.timingParametersGroupBox.Controls.Add(this.label2);
            this.timingParametersGroupBox.Controls.Add(this.label1);
            this.timingParametersGroupBox.Controls.Add(this.sampleClkSourceLabel);
            this.timingParametersGroupBox.Controls.Add(this.txt_RR_1);
            this.timingParametersGroupBox.Controls.Add(this.txt_RR_0);
            this.timingParametersGroupBox.Controls.Add(this.txt_RL_1);
            this.timingParametersGroupBox.Controls.Add(this.txt_RL_0);
            this.timingParametersGroupBox.Controls.Add(this.txt_FR_1);
            this.timingParametersGroupBox.Controls.Add(this.txt_FR_0);
            this.timingParametersGroupBox.Controls.Add(this.txt_FL_1);
            this.timingParametersGroupBox.Controls.Add(this.txt_FL_0);
            this.timingParametersGroupBox.Controls.Add(this.samplesToReadTextBox);
            this.timingParametersGroupBox.Controls.Add(this.samplesToReadLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateTextBox);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(12, 260);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(444, 203);
            this.timingParametersGroupBox.TabIndex = 7;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(14, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Sample Clock Source RR";
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(14, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Sample Clock Source RL";
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(13, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Sample Clock Source FR";
            // 
            // sampleClkSourceLabel
            // 
            this.sampleClkSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleClkSourceLabel.Location = new System.Drawing.Point(14, 89);
            this.sampleClkSourceLabel.Name = "sampleClkSourceLabel";
            this.sampleClkSourceLabel.Size = new System.Drawing.Size(144, 17);
            this.sampleClkSourceLabel.TabIndex = 4;
            this.sampleClkSourceLabel.Text = "Sample Clock Source FL";
            // 
            // txt_RR_1
            // 
            this.txt_RR_1.Location = new System.Drawing.Point(313, 167);
            this.txt_RR_1.Name = "txt_RR_1";
            this.txt_RR_1.Size = new System.Drawing.Size(125, 21);
            this.txt_RR_1.TabIndex = 5;
            this.txt_RR_1.Text = "Dev1/ctr3";
            // 
            // txt_RR_0
            // 
            this.txt_RR_0.Location = new System.Drawing.Point(163, 167);
            this.txt_RR_0.Name = "txt_RR_0";
            this.txt_RR_0.Size = new System.Drawing.Size(144, 21);
            this.txt_RR_0.TabIndex = 5;
            this.txt_RR_0.Text = "/Dev1/PFI27";
            // 
            // txt_RL_1
            // 
            this.txt_RL_1.Location = new System.Drawing.Point(313, 140);
            this.txt_RL_1.Name = "txt_RL_1";
            this.txt_RL_1.Size = new System.Drawing.Size(125, 21);
            this.txt_RL_1.TabIndex = 5;
            this.txt_RL_1.Text = "Dev1/ctr2";
            // 
            // txt_RL_0
            // 
            this.txt_RL_0.Location = new System.Drawing.Point(163, 140);
            this.txt_RL_0.Name = "txt_RL_0";
            this.txt_RL_0.Size = new System.Drawing.Size(144, 21);
            this.txt_RL_0.TabIndex = 5;
            this.txt_RL_0.Text = "/Dev1/PFI31";
            // 
            // txt_FR_1
            // 
            this.txt_FR_1.Location = new System.Drawing.Point(313, 113);
            this.txt_FR_1.Name = "txt_FR_1";
            this.txt_FR_1.Size = new System.Drawing.Size(125, 21);
            this.txt_FR_1.TabIndex = 5;
            this.txt_FR_1.Text = "Dev1/ctr1";
            // 
            // txt_FR_0
            // 
            this.txt_FR_0.Location = new System.Drawing.Point(163, 113);
            this.txt_FR_0.Name = "txt_FR_0";
            this.txt_FR_0.Size = new System.Drawing.Size(144, 21);
            this.txt_FR_0.TabIndex = 5;
            this.txt_FR_0.Text = "/Dev1/PFI35";
            // 
            // txt_FL_1
            // 
            this.txt_FL_1.Location = new System.Drawing.Point(313, 86);
            this.txt_FL_1.Name = "txt_FL_1";
            this.txt_FL_1.Size = new System.Drawing.Size(125, 21);
            this.txt_FL_1.TabIndex = 5;
            this.txt_FL_1.Text = "Dev1/ctr0";
            // 
            // txt_FL_0
            // 
            this.txt_FL_0.Location = new System.Drawing.Point(163, 86);
            this.txt_FL_0.Name = "txt_FL_0";
            this.txt_FL_0.Size = new System.Drawing.Size(144, 21);
            this.txt_FL_0.TabIndex = 5;
            this.txt_FL_0.Text = "/Dev1/PFI39";
            // 
            // samplesToReadTextBox
            // 
            this.samplesToReadTextBox.Location = new System.Drawing.Point(163, 52);
            this.samplesToReadTextBox.Name = "samplesToReadTextBox";
            this.samplesToReadTextBox.Size = new System.Drawing.Size(144, 21);
            this.samplesToReadTextBox.TabIndex = 3;
            this.samplesToReadTextBox.Text = "100";
            // 
            // samplesToReadLabel
            // 
            this.samplesToReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesToReadLabel.Location = new System.Drawing.Point(14, 55);
            this.samplesToReadLabel.Name = "samplesToReadLabel";
            this.samplesToReadLabel.Size = new System.Drawing.Size(118, 17);
            this.samplesToReadLabel.TabIndex = 2;
            this.samplesToReadLabel.Text = "Samples to Read:";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(14, 20);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(68, 17);
            this.rateLabel.TabIndex = 0;
            this.rateLabel.Text = "Rate:";
            // 
            // rateTextBox
            // 
            this.rateTextBox.Location = new System.Drawing.Point(163, 17);
            this.rateTextBox.Name = "rateTextBox";
            this.rateTextBox.Size = new System.Drawing.Size(144, 21);
            this.rateTextBox.TabIndex = 1;
            this.rateTextBox.Text = "1000";
            // 
            // fomNIDaq
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(467, 475);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Name = "fomNIDaq";
            this.Text = "fomNIDaq";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fomNIDaq_FormClosing);
            this.Load += new System.EventHandler(this.fomNIDaq_Load);
            this.channelParametersGroupBox.ResumeLayout(false);
            this.channelParametersGroupBox.PerformLayout();
            this.timingParametersGroupBox.ResumeLayout(false);
            this.timingParametersGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.ComboBox counterComboBox;
        private System.Windows.Forms.Label zIndexPhaseLabel;
        private System.Windows.Forms.ComboBox zIndexPhaseComboBox;
        private System.Windows.Forms.Label decodingTypeLabel;
        private System.Windows.Forms.Label pulsesPerRevLabel;
        private System.Windows.Forms.Label zIndexValueLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.TextBox pulsePerRevTextBox;
        private System.Windows.Forms.TextBox zIndexValueTextBox;
        private System.Windows.Forms.ComboBox decodingTypeComboBox;
        private System.Windows.Forms.CheckBox zIndexEnabledCheckBox;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label sampleClkSourceLabel;
        private System.Windows.Forms.TextBox txt_RR_1;
        private System.Windows.Forms.TextBox txt_RR_0;
        private System.Windows.Forms.TextBox txt_RL_1;
        private System.Windows.Forms.TextBox txt_RL_0;
        private System.Windows.Forms.TextBox txt_FR_1;
        private System.Windows.Forms.TextBox txt_FR_0;
        private System.Windows.Forms.TextBox txt_FL_1;
        private System.Windows.Forms.TextBox txt_FL_0;
        private System.Windows.Forms.TextBox samplesToReadTextBox;
        private System.Windows.Forms.Label samplesToReadLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.TextBox rateTextBox;
    }
}