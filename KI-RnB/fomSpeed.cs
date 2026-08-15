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
    public partial class fomSpeed : Form
    {
        public bool IsOpen;

        public fomSpeed()
        {
            InitializeComponent();
        }

        private void fomSpeed_Load(object sender, EventArgs e)
        {
            IsOpen = true;

            tmr_Loop.Enabled = true;
        }
        private void fomSpeed_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsOpen = false;
        }

        private void btnWheel_Click(object sender, EventArgs e)
        {
            lbl_FL_0.BackColor = Color.White; lbl_FL_1.BackColor = Color.White; lbl_FL_2.BackColor = Color.White; lbl_FL_3.BackColor = Color.White; lbl_FL_4.BackColor = Color.White; lbl_FL_5.BackColor = Color.White; lbl_FL_6.BackColor = Color.White; lbl_FL_7.BackColor = Color.White;
            lbl_FR_0.BackColor = Color.White; lbl_FR_1.BackColor = Color.White; lbl_FR_2.BackColor = Color.White; lbl_FR_3.BackColor = Color.White; lbl_FR_4.BackColor = Color.White; lbl_FR_5.BackColor = Color.White; lbl_FR_6.BackColor = Color.White; lbl_FR_7.BackColor = Color.White;
            lbl_RL_0.BackColor = Color.White; lbl_RL_1.BackColor = Color.White; lbl_RL_2.BackColor = Color.White; lbl_RL_3.BackColor = Color.White; lbl_RL_4.BackColor = Color.White; lbl_RL_5.BackColor = Color.White; lbl_RL_6.BackColor = Color.White; lbl_RL_7.BackColor = Color.White;
            lbl_RR_0.BackColor = Color.White; lbl_RR_1.BackColor = Color.White; lbl_RR_2.BackColor = Color.White; lbl_RR_3.BackColor = Color.White; lbl_RR_4.BackColor = Color.White; lbl_RR_5.BackColor = Color.White; lbl_RR_6.BackColor = Color.White; lbl_RR_7.BackColor = Color.White;
            
            switch (((Button)sender).Name)
            {
                case "btn_FL": NI.Zero__FL = NI.Get___FL; lbl_FL_0.BackColor = Color.SkyBlue; lbl_FL_1.BackColor = Color.SkyBlue; lbl_FL_2.BackColor = Color.SkyBlue; lbl_FL_3.BackColor = Color.SkyBlue; lbl_FL_4.BackColor = Color.SkyBlue; lbl_FL_5.BackColor = Color.SkyBlue; lbl_FL_6.BackColor = Color.SkyBlue; lbl_FL_7.BackColor = Color.SkyBlue; break;
                case "btn_FR": NI.Zero__FR = NI.Get___FR; lbl_FR_0.BackColor = Color.SkyBlue; lbl_FR_1.BackColor = Color.SkyBlue; lbl_FR_2.BackColor = Color.SkyBlue; lbl_FR_3.BackColor = Color.SkyBlue; lbl_FR_4.BackColor = Color.SkyBlue; lbl_FR_5.BackColor = Color.SkyBlue; lbl_FR_6.BackColor = Color.SkyBlue; lbl_FR_7.BackColor = Color.SkyBlue; break;
                case "btn_RL": NI.Zero__RL = NI.Get___RL; lbl_RL_0.BackColor = Color.SkyBlue; lbl_RL_1.BackColor = Color.SkyBlue; lbl_RL_2.BackColor = Color.SkyBlue; lbl_RL_3.BackColor = Color.SkyBlue; lbl_RL_4.BackColor = Color.SkyBlue; lbl_RL_5.BackColor = Color.SkyBlue; lbl_RL_6.BackColor = Color.SkyBlue; lbl_RL_7.BackColor = Color.SkyBlue; break;
                case "btn_RR": NI.Zero__RR = NI.Get___RR; lbl_RR_0.BackColor = Color.SkyBlue; lbl_RR_1.BackColor = Color.SkyBlue; lbl_RR_2.BackColor = Color.SkyBlue; lbl_RR_3.BackColor = Color.SkyBlue; lbl_RR_4.BackColor = Color.SkyBlue; lbl_RR_5.BackColor = Color.SkyBlue; lbl_RR_6.BackColor = Color.SkyBlue; lbl_RR_7.BackColor = Color.SkyBlue; break;
            }
        }

        private void tmr_Loop_Tick(object sender, EventArgs e)
        {
            NI.Scan_Data kind = NI.Loss;
            
            lblCount.Text = NI.Herz.ToString();

            //Encorder Signal
            lbl_FL_0.Text = NI.ENC___FL.ToString();
            lbl_FR_0.Text = NI.ENC___FR.ToString();
            lbl_RL_0.Text = NI.ENC___RL.ToString();
            lbl_RR_0.Text = NI.ENC___RR.ToString();

            //RPM
            lbl_FL_1.Text = kind.FL.RPM.ToString("0.00");
            lbl_FR_1.Text = kind.FR.RPM.ToString("0.00");
            lbl_RL_1.Text = kind.RL.RPM.ToString("0.00");
            lbl_RR_1.Text = kind.RR.RPM.ToString("0.00");

            //Speed (km/h)
            lbl_FL_2.Text = kind.FL.Speed.ToString("0.00");
            lbl_FR_2.Text = kind.FR.Speed.ToString("0.00");
            lbl_RL_2.Text = kind.RL.Speed.ToString("0.00");
            lbl_RR_2.Text = kind.RR.Speed.ToString("0.00");

            //Force (kgf)
            lbl_FL_3.Text = kind.FL.Kgf.ToString("0.00");
            lbl_FR_3.Text = kind.FR.Kgf.ToString("0.00");
            lbl_RL_3.Text = kind.RL.Kgf.ToString("0.00");
            lbl_RR_3.Text = kind.RR.Kgf.ToString("0.00");

            //Acceleration 가속도
            lbl_FL_4.Text = kind.FL.Accel.ToString("0.00000000");
            lbl_FR_4.Text = kind.FR.Accel.ToString("0.00000000");
            lbl_RL_4.Text = kind.RL.Accel.ToString("0.00000000");
            lbl_RR_4.Text = kind.RR.Accel.ToString("0.00000000");

            //Motor Herz
            lbl_FL_5.Text = (kind.FL.Speed / 13).ToString("0.00");
            lbl_FR_5.Text = (kind.FR.Speed / 13).ToString("0.00");
            lbl_RL_5.Text = (kind.RL.Speed / 13).ToString("0.00");
            lbl_RR_5.Text = (kind.RR.Speed / 13).ToString("0.00");

            //wheel.GapE = ENC - wheel.oENC; 차이값
            lbl_FL_6.Text = kind.FL.GapE.ToString("0.00");
            lbl_FR_6.Text = kind.FR.GapE.ToString("0.00");
            lbl_RL_6.Text = kind.RL.GapE.ToString("0.00");
            lbl_RR_6.Text = kind.RR.GapE.ToString("0.00");

            //Moving
            lbl_FL_7.Text = NI.Loss.FL.Move.ToString("0.00");
            lbl_FR_7.Text = NI.Loss.FR.Move.ToString("0.00");
            lbl_RL_7.Text = NI.Loss.RL.Move.ToString("0.00");
            lbl_RR_7.Text = NI.Loss.RR.Move.ToString("0.00");

            btn_FL.BackColor = NI.Run_FL == true ? Color.Lime : Color.Red;
            btn_FR.BackColor = NI.Run_FR == true ? Color.Lime : Color.Red;
            btn_RL.BackColor = NI.Run_RL == true ? Color.Lime : Color.Red;
            btn_RR.BackColor = NI.Run_RR == true ? Color.Lime : Color.Red;
        }
    }
}
