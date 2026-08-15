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
    public partial class fom_Dist : Form
    {
        public fom_Dist()
        {
            InitializeComponent();

            if (PSet.OwnerS00 > PSet.Def_Lang)
            {
                    this.Text = PSet.LangDist[0];   //PLC 거리 설정
                lbl_Msgs.Text = PSet.LangDist[1];   //PLC와 각 항목의 거리를 설정하거나 읽습니다.
                btnClose.Text = PSet.LangDist[2];   //닫기
                lblLHome.Text = PSet.LangDist[18];  //좌 홈 위치 거리
                lblRHome.Text = PSet.LangDist[19];  //우 홈 위치 거리
                lbl_KeyA.Text = PSet.LangDist[4];   //설정 A
                lbl_KeyB.Text = PSet.LangDist[5];   //설정 B
                lbl_KeyC.Text = PSet.LangDist[6];   //설정 C
                lbl_KeyD.Text = PSet.LangDist[7];   //설정 D
                lbl_KeyE.Text = PSet.LangDist[8];   //설정 E
                lbl_KeyF.Text = PSet.LangDist[9];   //설정 F
                lbl_KeyG.Text = PSet.LangDist[10];  //설정 G
                lbl_KeyH.Text = PSet.LangDist[11];  //설정 H
                lbl_KeyI.Text = PSet.LangDist[12];  //설정 I
                lbl_KeyJ.Text = PSet.LangDist[13];  //설정 J
                btn_Read.Text = PSet.LangDist[14];  //PC <- PLC
                btnWrite.Text = PSet.LangDist[15];  //PC -> PLC
            }
        }

        private void fom_Dist_Load(object sender, EventArgs e)
        {
            PLC_ReadData();
        }

        private void txt_Dist_Enter(object sender, EventArgs e)
        {
            string strValue = ((TextBox)sender).Text;

            ((TextBox)sender).BackColor = Color.AliceBlue;

            if (strValue.Length > 0)
            {
                ((TextBox)sender).Select(0, strValue.Length);
            }
        }

        private void txt_Dist_Leave(object sender, EventArgs e)
        {
            try
            {
                int offset, value_;
                if (!int.TryParse(txtLHome.Text, out offset)) return;
                if (!int.TryParse(((TextBox)sender).Text, out value_)) return;

                ((TextBox)sender).BackColor = Color.White;

                if (((TextBox)sender).Name != "txtLHome")
                {
                    if (offset + PSet.WB_Min > value_)
                    {
                        ((TextBox)sender).Text = (offset + PSet.WB_Min).ToString();
                    }

                    if (offset + PSet.WB_Max < value_)
                    {
                        ((TextBox)sender).Text = (offset + PSet.WB_Max).ToString();
                    }
                }
                else
                {
                    if (offset < 0)
                    {
                        txtLHome.Text = PLC.OfSetL.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                switch (((TextBox)sender).Name)
                {
                    case "txtDistA": txtDistA.Text = (PLC.OfSetL + PLC.Dist_A).ToString(); break;
                    case "txtDistB": txtDistB.Text = (PLC.OfSetL + PLC.Dist_B).ToString(); break;
                    case "txtDistC": txtDistC.Text = (PLC.OfSetL + PLC.Dist_C).ToString(); break;
                    case "txtDistD": txtDistD.Text = (PLC.OfSetL + PLC.Dist_D).ToString(); break;
                    case "txtDistE": txtDistE.Text = (PLC.OfSetL + PLC.Dist_E).ToString(); break;
                    case "txtDistF": txtDistF.Text = (PLC.OfSetL + PLC.Dist_F).ToString(); break;
                    case "txtDistG": txtDistG.Text = (PLC.OfSetL + PLC.Dist_G).ToString(); break;
                    case "txtDistH": txtDistH.Text = (PLC.OfSetL + PLC.Dist_H).ToString(); break;
                    case "txtDistI": txtDistI.Text = (PLC.OfSetL + PLC.Dist_I).ToString(); break;
                    case "txtDistJ": txtDistJ.Text = (PLC.OfSetL + PLC.Dist_J).ToString(); break;
                    case "txtLHome": txtLHome.Text = PLC.OfSetL.ToString();                break;
                    case "txtRHome": txtRHome.Text = PLC.OfSetR.ToString();                break;
                }
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnClose": this.Close(); break;
                case "btn_Read": PLC_ReadData(); break;
                case "btnWrite": PLCWriteData(); break;
            }
        }

        private void PLC_ReadData()
        {
            txtDistA.Text = (PLC.OfSetL + PLC.Dist_A).ToString();
            txtDistB.Text = (PLC.OfSetL + PLC.Dist_B).ToString();
            txtDistC.Text = (PLC.OfSetL + PLC.Dist_C).ToString();
            txtDistD.Text = (PLC.OfSetL + PLC.Dist_D).ToString();
            txtDistE.Text = (PLC.OfSetL + PLC.Dist_E).ToString();
            txtDistF.Text = (PLC.OfSetL + PLC.Dist_F).ToString();
            txtDistG.Text = (PLC.OfSetL + PLC.Dist_G).ToString();
            txtDistH.Text = (PLC.OfSetL + PLC.Dist_H).ToString();
            txtDistI.Text = (PLC.OfSetL + PLC.Dist_I).ToString();
            txtDistJ.Text = (PLC.OfSetL + PLC.Dist_J).ToString();
            txtLHome.Text = PLC.OfSetL.ToString();
            txtRHome.Text = PLC.OfSetR.ToString();

            MessageBoxEx.Show(PSet.LangDist[16]);   //PLC 설정을 읽었습니다.
        }

        private void PLCWriteData()
        {
            int Offset_L, Offset_R;
            if (!int.TryParse(txtLHome.Text, out Offset_L)) return;
            if (!int.TryParse(txtRHome.Text, out Offset_R)) return;

            int[] Lent = new int[12];
            int tmp;

            Lent[0] = int.TryParse(txtDistA.Text, out tmp) ? tmp - Offset_L : 0;
            Lent[1] = int.TryParse(txtDistB.Text, out tmp) ? tmp - Offset_L : 0;
            Lent[2] = int.TryParse(txtDistC.Text, out tmp) ? tmp - Offset_L : 0;
            Lent[3] = int.TryParse(txtDistD.Text, out tmp) ? tmp - Offset_L : 0;
            Lent[4] = int.TryParse(txtDistE.Text, out tmp) ? tmp - Offset_L : 0;
            Lent[5] = int.TryParse(txtDistF.Text, out tmp) ? tmp - Offset_L : 0;
            Lent[6] = int.TryParse(txtDistG.Text, out tmp) ? tmp - Offset_L : 0;
            Lent[7] = int.TryParse(txtDistH.Text, out tmp) ? tmp - Offset_L : 0;
            Lent[8] = int.TryParse(txtDistI.Text, out tmp) ? tmp - Offset_L : 0;
            Lent[9] = int.TryParse(txtDistJ.Text, out tmp) ? tmp - Offset_L : 0;
            Lent[10] = Offset_L;
            Lent[11] = Offset_R;    

            long OfstTime = DateTime.Now.Ticks;
            long WaitTime = DateTime.Now.AddSeconds(3).Ticks;
            long Gap_Time = DateTime.Now.AddSeconds(0.5).Ticks;

            PLC.DO.DistReset = true; PLC.PLC_312_Puts();
            System.Threading.Thread.Sleep(100);
            PLC.PLC_Set_Dist(Lent);

            while (true)
            {
                if (WaitTime - DateTime.Now.Ticks > 0) break;
                if ((Lent[0] == PLC.Dist_A) & (Lent[1] == PLC.Dist_B) & 
                    (Lent[2] == PLC.Dist_C) & (Lent[3] == PLC.Dist_D) & 
                    (Lent[4] == PLC.Dist_E) & (Lent[5] == PLC.Dist_F) & 
                    (Lent[6] == PLC.Dist_G) & (Lent[7] == PLC.Dist_H) & 
                    (Lent[8] == PLC.Dist_I) & (Lent[9] == PLC.Dist_J) &
                    (Lent[10] == PLC.OfSetL) & (Lent[11] == PLC.OfSetR)) break;

                if ((DateTime.Now.Ticks - Gap_Time) > 0)
                {
                    Gap_Time = DateTime.Now.AddSeconds(0.5).Ticks;

                    PLC.PLC_Set_Dist(Lent);
                }

                System.Windows.Forms.Application.DoEvents();
            }

            System.Threading.Thread.Sleep(100);
            PLC.DO.DistReset = false; PLC.PLC_312_Puts();

            PSet.Lent_A = PLC.Dist_A;
            PSet.Lent_B = PLC.Dist_B;
            PSet.Lent_C = PLC.Dist_C;
            PSet.Lent_D = PLC.Dist_D;
            PSet.Lent_E = PLC.Dist_E;
            PSet.Lent_F = PLC.Dist_F;
            PSet.Lent_G = PLC.Dist_G;
            PSet.Lent_H = PLC.Dist_H;
            PSet.Lent_I = PLC.Dist_I;
            PSet.Lent_J = PLC.Dist_J;
            PSet.WB_Ofs = PLC.OfSetL;
                        //PLC.Offset_R

            PSet.DistanceMake();

            MessageBoxEx.Show(PSet.LangDist[17]);   //PLC에 설정을 적용하였습니다.
        }
    }
}