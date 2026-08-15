using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KI_RnB
{
    public partial class fomNIDaq : Form
    {
        public bool IsOpen;

        public fomNIDaq()
        {
            InitializeComponent();
        }

        private void fomNIDaq_Load(object sender, EventArgs e)
        {
            NIDaqmx_Read();

            IsOpen = true;
        }
        private void fomNIDaq_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
        }

        private void NIDaqmx_Read()
        {
            if (PSet.NIDAQmx_Read())
            {
                zIndexEnabledCheckBox.Checked = PSet.ENC_Z_On == "1" ? true : false;
                int encType; int.TryParse(PSet.ENC_Type, out encType);
                decodingTypeComboBox.SelectedIndex = (encType >= 0 && encType < decodingTypeComboBox.Items.Count) ? encType : 0;
                zIndexValueTextBox.Text = PSet.ENC_ZVal;
                int encPhase; int.TryParse(PSet.ENCPhase, out encPhase);
                zIndexPhaseComboBox.SelectedIndex = (encPhase >= 0 && encPhase < zIndexPhaseComboBox.Items.Count) ? encPhase : 0;
                pulsePerRevTextBox.Text = PSet.ENCPulse;

                rateTextBox.Text = PSet.ScanRate;
                samplesToReadTextBox.Text = PSet.Scan_CNT;

                txt_FL_0.Text = PSet.ENC_FL_0;
                txt_FL_1.Text = PSet.ENC_FL_1;
                txt_FR_0.Text = PSet.ENC_FR_0;
                txt_FR_1.Text = PSet.ENC_FR_1;
                txt_RL_0.Text = PSet.ENC_RL_0;
                txt_RL_1.Text = PSet.ENC_RL_1;
                txt_RR_0.Text = PSet.ENC_RR_0;
                txt_RR_1.Text = PSet.ENC_RR_1;
            }
        }
        
        private void btn_Save_Click(object sender, EventArgs e)
        {
            PSet.ENC_Z_On = zIndexEnabledCheckBox.Checked == true  ? "1" : "0";
            PSet.ENC_Type = decodingTypeComboBox.SelectedIndex.ToString();
            PSet.ENC_ZVal = zIndexValueTextBox.Text;
            PSet.ENCPhase = zIndexPhaseComboBox.SelectedIndex.ToString();
            PSet.ENCPulse = pulsePerRevTextBox.Text;

            PSet.ScanRate = rateTextBox.Text;
            PSet.Scan_CNT = samplesToReadTextBox.Text;

            PSet.ENC_FL_0 = txt_FL_0.Text;
            PSet.ENC_FL_1 = txt_FL_1.Text;
            PSet.ENC_FR_0 = txt_FR_0.Text;
            PSet.ENC_FR_1 = txt_FR_1.Text;
            PSet.ENC_RL_0 = txt_RL_0.Text;
            PSet.ENC_RL_1 = txt_RL_1.Text;
            PSet.ENC_RR_0 = txt_RR_0.Text;
            PSet.ENC_RR_1 = txt_RR_1.Text;

            PSet.NIDAQmx_Make();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
