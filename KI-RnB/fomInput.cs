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
    public partial class fomInput : Form
    {
        public string strValue;
        public int IsLock;

        private int Inp_Mode;

        public fomInput()
        {
            InitializeComponent();
        }
        public fomInput(int pMode, string pValue)
            :this()
        {
            strValue = "";
            Inp_Mode = pMode;

            switch (Inp_Mode)
            {
                case 0: lbl_Msgs.Text = "Enter the SST value";                      lbl_Unit.Text = "m/km"; break;
                case 1: lbl_Msgs.Text = "Enter the SST value";                      lbl_Unit.Text = "m/km"; break;
                case 2: lbl_Msgs.Text = "Please enter the weight";                  lbl_Unit.Text = "kg";   break;
                case 3: lbl_Msgs.Text = "Please enter the weight";                  lbl_Unit.Text = "kg";   break;
                case 4: lbl_Msgs.Text = "Enter the braking force";                  lbl_Unit.Text = "kg";   break;
                case 5: lbl_Msgs.Text = "Enter the braking force";                  lbl_Unit.Text = "kg";   break;
                case 98: lbl_Msgs.Text = "Please enter a password";                 lbl_Unit.Text = "";     break;
                case 99: lbl_Msgs.Text = "Please enter the administrator password"; lbl_Unit.Text = "";     break;
            }

            if (pMode == 98 || pMode == 99)
            {
                chk_Show.Visible = true;
                txtInput.PasswordChar = '*';
            }
            else
            {
                chk_Show.Visible = false;
                txtInput.PasswordChar = '\0';
            }

            if (Inp_Mode == 2 || Inp_Mode == 3 || Inp_Mode == 4 || Inp_Mode == 5)
            {
                float val;
                if (!float.TryParse(pValue, out val)) val = 0;
                if (PSet.BrkRatio != 0) val = val / PSet.BrkRatio;

                txtInput.Text = val.ToString("#0");
            }
            else
            {
                txtInput.Text = pValue.ToString();
            }

            txtInput.Focus();
            txtInput.SelectAll();
        }

        private void Bottons_Click(object sender, EventArgs e)
        {
            strValue = txtInput.Text;

            switch (((Button)sender).Name)
            {
                case "btn_OK":

                    switch (Inp_Mode)
                    {
                        case 0: { float v; if (float.TryParse(strValue, out v)) { PSet.CH0Span = H2Y.DVD(v, PSet.CH0Last); IsLock = 0; } } break;
                        case 1: { float v; if (float.TryParse(strValue, out v)) { PSet.CH1Span = H2Y.DVD(v, PSet.CH1Last); IsLock = 0; } } break;
                        case 2: { float v; if (float.TryParse(strValue, out v)) { PSet.CH2Span = H2Y.DVD(v, PSet.CH2Last); IsLock = 0; } } break;
                        case 3: { float v; if (float.TryParse(strValue, out v)) { PSet.CH3Span = H2Y.DVD(v, PSet.CH3Last); IsLock = 0; } } break;
                        case 4: { float v; if (float.TryParse(strValue, out v)) { PSet.CH4Span = H2Y.DVD(v * PSet.BrkRatio, PSet.CH4Last); IsLock = 0; } } break;
                        case 5: { float v; if (float.TryParse(strValue, out v)) { PSet.CH5Span = H2Y.DVD(v * PSet.BrkRatio, PSet.CH5Last); IsLock = 0; } } break;
                        case 98: IsLock = Ret_Password(strValue); break;
                        case 99: IsLock = Ret_Password(strValue); break;
                    }
                    break;

                case "btnCancel": IsLock = -1; 
                    this.Close();
                    break;
            }

            if (IsLock >= 0)
            {
                this.Close();
            }
        }

        private int Ret_Password(string pPassword)
        {
            if(pPassword == PSet.Passwd)
            {
                //H2Y.Msg_Speash("인증되었습니다.");
                return 1;
            } if (pPassword == "kimc" + DateTime.Now.Month.ToString())
            {
                //H2Y.Msg_Speash("인증되었습니다.");
                return 2;
            }
            else
            {
                //H2Y.Msg_Speash("잘못 입력 하였습니다.");
                return -1;
            }
        }

        private void chk_Show_CheckedChanged(object sender, EventArgs e)
        {
            txtInput.PasswordChar = chk_Show.Checked ? '\0' : '*';
        }
        
        private void fomInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Bottons_Click(btnCancel, EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                Bottons_Click(btn_OK, EventArgs.Empty);
            }
            else
            {
            }
        }
    }
}
