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
    public partial class fom_Init : Form
    {
        public bool IsOpen;

        public fom_Init()
        {
            InitializeComponent();
        }

        private void fom_Init_Load(object sender, EventArgs e)
        {
            IsOpen = true;

            timer1.Enabled = true;
        }
        private void fom_Init_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsOpen = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            long ReadTime;
            long OfstTime = DateTime.Now.Ticks;
            long WaitTime = DateTime.Now.AddSeconds(10).Ticks;
            long Gap_Time = DateTime.Now.AddSeconds(0.1).Ticks;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;

            do
            {
                ReadTime = (DateTime.Now.Ticks - OfstTime) / 1000000;

                if (ReadTime > 100) ReadTime = 100;

                progressBar1.Value = Convert.ToInt16(ReadTime);

                if ((DateTime.Now.Ticks - Gap_Time) > 0)
                {
                    Gap_Time = DateTime.Now.AddSeconds(0.3).Ticks;

                    lblCount.Text = Convert.ToInt16(ReadTime / 10).ToString();
                    //lbl_Msgs.Visible = !lbl_Msgs.Visible;
                    lbl_Msgs.ForeColor = (lbl_Msgs.ForeColor == Color.Black ? Color.Red : Color.Black);
                }

                System.Windows.Forms.Application.DoEvents();

            } while (WaitTime - DateTime.Now.Ticks > 0);

            this.Close();
        }
    }
}
