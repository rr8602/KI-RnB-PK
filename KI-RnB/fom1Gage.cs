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
    public partial class fom1Gage : Form
    {
        cls_Gage Sel_Gage;
        Bitmap bmp;
        long ForceMax = 50;
        int Index = 0;
        Single Min, Max;

        public fom1Gage()
        {
            InitializeComponent();

            if (PSet.OwnerS00 > PSet.Def_Lang)
            {
                btnClose.Text = PSet.LangGage[0]; //닫기
                gbx_Time.Text = PSet.LangGage[1]; //시간
                gbx_Indi.Text = PSet.LangGage[2]; //인디게이터
                lbl_Free.Text = PSet.LangGage[3]; //구동/프리
                lblBrake.Text = PSet.LangGage[5]; //브레이크
            }
        }

        public fom1Gage(int idx, Single Min, Single Max)
            : this()
        {
            Index = idx; this.Min = Min; this.Max = Max;
        }

        private void fom1Gage_Load(object sender, EventArgs e)
        {
            this.Top = PSet.siz__Sub.Top;
            this.Left = PSet.siz__Sub.Left;
            this.Width = 1024;
            this.Height = 768;

            string title = "";

            switch(Index)
            {
                case 0: title = PSet.LangGage[6]; break; //"FL Speed"
                case 1: title = PSet.LangGage[7]; break; //"FR Speed"
                case 2: title = PSet.LangGage[8]; break; //"RL Speed"
                case 3: title = PSet.LangGage[9]; break; //"RR Speed"
            }

            Sel_Gage = new cls_Gage(pic_FL_M, pic_FL_M.Width, 200, 1.6, title, "km/h");
            
            Sel_Gage.Yellow_Zone(Min, Max);
            Sel_Gage.FontSize = 40;
            tmr_Gage.Enabled = true;
        }

        private void fomPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmr_Gage.Enabled = false;
        }

        private void btnCllose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tmr_Gage_Tick(object sender, EventArgs e)
        {
            TestGageShow();
        }

        private void TestGageShow()
        {
            try
            {
                switch (Index)
                {
                    case 0: Sel_Gage.Value = NI.Loss.FL.Speed;
                        lbl_Free.Text = PLC.DO.FLMot_Stt ? PSet.LangGage[3] : PSet.LangGage[4]; //구동, 프리
                        lbl_Free.BackColor = PLC.DO.FLMot_Stt ? Color.Yellow : Color.Green;
                        lblBrake.BackColor = PLC.DO.FLMotStop ? Color.Red : Color.White;
                        break;

                    case 1: Sel_Gage.Value = NI.Loss.FR.Speed;
                        lbl_Free.Text = PLC.DO.FRMot_Stt ? PSet.LangGage[3] : PSet.LangGage[4];//구동, 프리
                        lbl_Free.BackColor = PLC.DO.FRMot_Stt ? Color.Yellow : Color.Green;
                        lblBrake.BackColor = PLC.DO.FRMotStop ? Color.Red : Color.White;
                        break;

                    case 2: Sel_Gage.Value = NI.Loss.RL.Speed;
                        lbl_Free.Text = PLC.DO.RLMot_Stt ? PSet.LangGage[3] : PSet.LangGage[4];//구동, 프리
                        lbl_Free.BackColor = PLC.DO.RLMot_Stt ? Color.Yellow : Color.Green;
                        lblBrake.BackColor = PLC.DO.RLMotStop ? Color.Red : Color.White;
                        break;

                    case 3: Sel_Gage.Value = NI.Loss.RR.Speed;
                        lbl_Free.Text = PLC.DO.RRMot_Stt ? PSet.LangGage[3] : PSet.LangGage[4];//구동, 프리
                        lbl_Free.BackColor = PLC.DO.RRMot_Stt ? Color.Yellow : Color.Green;
                        lblBrake.BackColor = PLC.DO.RRMotStop ? Color.Red : Color.White;
                        break;
                }
                pic_Indi.Image = Show_Indicator(pic_Indi, Convert.ToSingle(TSet.Bongshin));
                lbl_Indi.Text = TSet.Bongshin.ToString("#0.0");
            }
            catch (Exception ex)
            {
            }
        }

        public void TestTimeShow(double FTime, double TTime)
        {
            lbl_Time.Text = TTime.ToString("#0.00");
            lblFTime.Text = FTime.ToString("#0.00");
        }

        private Bitmap Show_Indicator(PictureBox pBox, Single Force)
        {
            bmp = new Bitmap(pBox.Width, pBox.Height);

            Single brkForce = Force;

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float bmp_w = bmp.Width;
                float bmp_h = bmp.Height;
                float value = bmp_h - (brkForce / ForceMax * bmp_h);

                SolidBrush RedBrush = new SolidBrush(Color.Red);
                SolidBrush BuleBrush = new SolidBrush(Color.Blue);
                SolidBrush GreenBrush = new SolidBrush(Color.Lime);
                SolidBrush BlackBrush = new SolidBrush(Color.Black);

                g.FillRectangle(GreenBrush, 0f, value, bmp_w, bmp.Height);
            }

            return bmp;
        }

        private void pic_Gage_Paint(object sender, PaintEventArgs e)
        {
            Sel_Gage.Gage_Show(e.Graphics);
        }

        private void pic_FL_M_DoubleClick(object sender, EventArgs e)
        {
            btnClose.Visible = !btnClose.Visible;
        }
    }
}
