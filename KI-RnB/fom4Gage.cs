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
    public partial class fom4Gage : Form
    {
        Single Min, Max;

        cls_Gage FL_Gage;
        cls_Gage FR_Gage;
        cls_Gage RL_Gage;
        cls_Gage RR_Gage;

        public fom4Gage()
        {
            InitializeComponent();

            if (PSet.OwnerS00 > PSet.Def_Lang)
            {
                btnClose.Text = PSet.LangGage[0]; //닫기
                lbl_FL_1.Text = PSet.LangGage[3]; //구동
                lbl_FL_2.Text = PSet.LangGage[4]; //프리
                lbl_FL_3.Text = PSet.LangGage[5]; //브레이크

                lbl_FR_1.Text = PSet.LangGage[3]; //구동
                lbl_FR_2.Text = PSet.LangGage[4]; //프리
                lbl_FR_3.Text = PSet.LangGage[5]; //브레이크

                lbl_RL_1.Text = PSet.LangGage[3]; //구동
                lbl_RL_2.Text = PSet.LangGage[4]; //프리
                lbl_RL_3.Text = PSet.LangGage[5]; //브레이크

                lbl_RR_1.Text = PSet.LangGage[3]; //구동
                lbl_RR_2.Text = PSet.LangGage[4]; //프리
                lbl_RR_3.Text = PSet.LangGage[5]; //브레이크
                
            }
        }

        public fom4Gage(Single Min, Single Max) : this()
        {
            this.Min = Min; this.Max = Max;
        }

        private void fomPanel_Load(object sender, EventArgs e)
        {
            this.Top = PSet.siz__Sub.Top;
            this.Left = PSet.siz__Sub.Left;
            this.Width = 1024;
            this.Height = 768;

            FL_Gage = new cls_Gage(pic_FL_M, pic_FL_M.Width, 200, 1.6, PSet.LangGage[6], "km/h"); //"FL Speed"
            FR_Gage = new cls_Gage(pic_FR_M, pic_FR_M.Width, 200, 1.6, PSet.LangGage[7], "km/h"); //"FR Speed"
            RL_Gage = new cls_Gage(pic_RL_M, pic_RL_M.Width, 200, 1.6, PSet.LangGage[8], "km/h"); //"RL Speed"
            RR_Gage = new cls_Gage(pic_RR_M, pic_RR_M.Width, 200, 1.6, PSet.LangGage[9], "km/h"); //"RR Speed" 

            FL_Gage.Yellow_Zone(Min, Max);
            FR_Gage.Yellow_Zone(Min, Max);
            RL_Gage.Yellow_Zone(Min, Max);
            RR_Gage.Yellow_Zone(Min, Max);

            tmr_Gage.Enabled = true;
        }

        private void fomPanel_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void tmr_Gage_Tick(object sender, EventArgs e)
        {
            Speeds_Panel();
        }
        public void Speeds_Panel()
        {
            try
            {
                FL_Gage.Value = NI.Loss.FL.Speed;
                FR_Gage.Value = NI.Loss.FR.Speed;
                RL_Gage.Value = NI.Loss.RL.Speed;
                RR_Gage.Value = NI.Loss.RR.Speed;

                lbl_FL.Text = NI.Loss.FL.Speed.ToString("#0.0");
                lbl_FR.Text = NI.Loss.FR.Speed.ToString("#0.0");
                lbl_RL.Text = NI.Loss.RL.Speed.ToString("#0.0");
                lbl_RR.Text = NI.Loss.RR.Speed.ToString("#0.0");

                lbl_FL_1.BackColor = PLC.DO.FLMot_Stt ? Color.Red : Color.White;
                lbl_FR_1.BackColor = PLC.DO.FRMot_Stt ? Color.Red : Color.White;
                lbl_RL_1.BackColor = PLC.DO.RLMot_Stt ? Color.Red : Color.White;
                lbl_RR_1.BackColor = PLC.DO.RRMot_Stt ? Color.Red : Color.White;

                lbl_FL_2.BackColor = !PLC.DO.FLMot_Stt ? Color.Lime : Color.White;
                lbl_FR_2.BackColor = !PLC.DO.FRMot_Stt ? Color.Lime : Color.White;
                lbl_RL_2.BackColor = !PLC.DO.RLMot_Stt ? Color.Lime : Color.White;
                lbl_RR_2.BackColor = !PLC.DO.RRMot_Stt ? Color.Lime : Color.White;

                lbl_FL_3.BackColor = PLC.DO.FLMotStop ? Color.Yellow : Color.White;
                lbl_FR_3.BackColor = PLC.DO.FRMotStop ? Color.Yellow : Color.White;
                lbl_RL_3.BackColor = PLC.DO.RLMotStop ? Color.Yellow : Color.White;
                lbl_RR_3.BackColor = PLC.DO.RRMotStop ? Color.Yellow : Color.White;

            }
            catch(Exception ex)
            {
            }
        }

        private void pic_Gage_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                switch (((PictureBox)sender).Name)
                {
                    case "pic_FL_M": FL_Gage.Gage_Show(e.Graphics); break;
                    case "pic_FR_M": FR_Gage.Gage_Show(e.Graphics); break;
                    case "pic_RL_M": RL_Gage.Gage_Show(e.Graphics); break;
                    case "pic_RR_M": RR_Gage.Gage_Show(e.Graphics); break;
                }
            }
            catch (Exception ex)
            { 
                throw ex; 
            }
        }

        private void fom4Gage_DoubleClick(object sender, EventArgs e)
        {
            btnClose.Visible = !btnClose.Visible;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
