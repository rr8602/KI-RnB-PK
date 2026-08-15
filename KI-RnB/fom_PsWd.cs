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
    public partial class fom_PsWd : Form
    {
        public byte     PassLock;
        public string   NewModel;
        public float    NewValue;
        private byte    PassMode;

        public fom_PsWd()
        {
            InitializeComponent();
        }

        public fom_PsWd(byte pMode) : this()
        {
            PassMode = pMode;
            NewModel = "";

            chk_Pass.Visible = PassMode < 10 ? true : false; 

            switch (PassMode)
            {                       //비밀번호
                case 0: this.Text = PSet.LangPsWd[0]; lbl_Msgs.Text = PSet.LangPsWd[1]; txt_Pswd.PasswordChar = '*'; break;
                case 1: this.Text = PSet.LangPsWd[0]; lbl_Msgs.Text = PSet.LangPsWd[1]; txt_Pswd.PasswordChar = '*'; break;
                case 11: this.Text = PSet.LangPsWd[2]; lbl_Msgs.Text = PSet.LangPsWd[3]; break;
            }
        }
        public fom_PsWd(float pVal) : this()
        {
            PassMode = 21;
            NewModel = "";
            NewValue = pVal;
            this.Text = PSet.LangPsWd[4];       //"Calibration"
            lbl_Msgs.Text = PSet.LangPsWd[5];   //"Please enter the value.";
            txt_Pswd.Text = NewValue.ToString(); 
        }

        private void fom_PsWd_Load(object sender, EventArgs e)
        {
            PSet.Onf_PsWd = true;
        }
        private void fom_PsWd_FormClosed(object sender, FormClosedEventArgs e)
        {
            PSet.Onf_PsWd = false;
        }

        private void fom_PsWd_Activated(object sender, EventArgs e)
        {
            PassLock = 0;
            txt_Pswd.Focus();
        }

        private void btn_Pass_Click(object sender, EventArgs e)
        {
            EnterMessage();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            PassLock = 0;
            this.Close();
        }

        private void fom_PsWd_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Return: EnterMessage();                 break;
                case (char)Keys.Cancel: PassLock = 0;   this.Close();   break;
            }
        }

        private void EnterMessage()
        {
            switch (PassMode)
            {
                case 0: PassworkLock(0);    break;
                case 1: PassworkLock(1);    break;
               case 11: Enter_NModel();     break;
               case 21: Enter_Values();     break;
            }
        }

        private void Enter_Values()
        {
            if (txt_Pswd.Text == "" || txt_Pswd.Text == "0")
            {
                MessageBox.Show(PSet.LangPsWd[5]);   //"Please enter the value"
            }
            else
            {
                NewValue = float.Parse(txt_Pswd.Text);
                this.Close();
            }
        }

        private void Enter_NModel()
        {
            if (txt_Pswd.Text == "")
            {
                MessageBox.Show(PSet.LangPsWd[3]); //Please enter a new model name.
            }
            else
            {
                NewModel = txt_Pswd.Text;
                this.Close();
            }
        }

        private void PassworkLock(byte pMode)
        {
            string password = "";

            if (txt_Pswd.Text == "")
            {
                switch (pMode)
                {
                    case 0: MessageBox.Show(PSet.LangPsWd[1]); break;  //"Please enter a password"
                    case 1: MessageBox.Show(PSet.LangPsWd[1]); break;  //"Please enter a password"
                }
            }
            else
            {
                switch (pMode)
                {
                    case 0: password = "kimc" + DateTime.Today.Month.ToString(); break;
                    case 1: password = "kimc" + DateTime.Today.Month.ToString(); break;
                }

                if (txt_Pswd.Text == password || txt_Pswd.Text == PSet.Passwd)
                {
                    if (txt_Pswd.Text == password) { PassLock = 1; }
                    if (txt_Pswd.Text == PSet.Passwd) { PassLock = 2; }
                    this.Close();
                }
                else
                {
                    MessageBox.Show(PSet.LangPsWd[6]);  //잘못 입력하셨습니다.
                }
            }
        }

        private void chk_Pass_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Pass.Checked)
            {
                txt_Pswd.PasswordChar = '\0';
            }
            else
            {
                txt_Pswd.PasswordChar = '*';
            }
        }
    }
}
