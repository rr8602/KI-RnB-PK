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
    public partial class fom__ASK : Form
    {
        fom_Main main;

        public bool IsOpen;
        public int Ret_Key = -1;
        public string Message = "";

        private int count = 0;

        public fom__ASK()
        {
            InitializeComponent();
        }
        public fom__ASK(fom_Main main)
            : this()
        {
            this.main = main;
        }


        private void fom__ASK_Load(object sender, EventArgs e)
        {
            IsOpen = true;

            if (Message != "")
            {
                lbl_Msgs.Text = Message;
            }

            this.Top = PSet.siz__Ask.Top;
            this.Left = PSet.siz__Ask.Left;

            tmr_Msgs.Enabled = true;
            tmr_Msgs.Interval = 100;
        }

        private void tmr_Msgs_Tick(object sender, EventArgs e)
        {
            if (PLC.DI.PSW_Check) { Ret_Key = 1; }
            if (PLC.DI.PSW__Stop) { Ret_Key = 2; }

            btn_OK.BackColor = (Ret_Key == 1) ? Color.Lime : Color.Black;
            btn_OK.ForeColor = (Ret_Key == 1) ? Color.Black : Color.White;

            btn_Pass.BackColor = (Ret_Key == 2) ? Color.Red : Color.Black;
            btn_Pass.ForeColor = (Ret_Key == 2) ? Color.Black : Color.White;

            if (Ret_Key > 0) 
            {
                count++;

                if (count > 10) { this.Close(); }
            }
        }

        public int Ret_Message()
        {
            Ret_Key = -1;

            this.Show();

            while (Ret_Key < 0)
            {
                if (!PSet.Onf_Prog) { Ret_Key = 2; }
                if (PLC.DI.PSW_Check) { Ret_Key = 1; }
                if (PLC.DI.PSW__Stop) { Ret_Key = 2; }

                btn_OK.BackColor = (Ret_Key == 1) ? Color.Lime : Color.Black;
                btn_OK.ForeColor = (Ret_Key == 1) ? Color.Black : Color.White;

                btn_Pass.BackColor = (Ret_Key == 2) ? Color.Red : Color.Black;
                btn_Pass.ForeColor = (Ret_Key == 2) ? Color.Black : Color.White;

                if (Ret_Key > 0)
                {
                    count++;

                    if (count > 10) { this.Close(); }
                }

                this.BringToFront();
                System.Windows.Forms.Application.DoEvents();
            }

            return Ret_Key;
        }
        
        private void fom__ASK_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmr_Msgs.Enabled = false;
            IsOpen = false;
            Message = "";

            PSet.siz__Ask.Top = this.Top;
            PSet.siz__Ask.Left = this.Left;

            PSet.Ini_SizeMake();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            Ret_Key = 1;
        }

        private void btn_Pass_Click(object sender, EventArgs e)
        {
            Ret_Key = 2;
        }
    }
}
