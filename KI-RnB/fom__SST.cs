using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace KI_RnB
{
    public partial class fom__SST : Form
    {
        public bool IsOpen;

        private cls_SSTs img_SSTs;
        private fom_Main main;

        private float Max_SST;

        public fom__SST()
        {
            InitializeComponent();
        }

        public fom__SST(fom_Main main)
            : this()
        {
            this.main = main;
        }
        private void fom__SST_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsOpen = false;
            tmr_SSTs.Enabled = false;
        }
        private void fom__SST_Load(object sender, EventArgs e)
        {
            IsOpen = true;

            img_SSTs = new cls_SSTs(pic_SSTs);

            img_SSTs.Red_Zone(-10, 10);
            img_SSTs.Yellow_Zone(-5, 5);
            img_SSTs.Green_Zone(-3, 3);

            this.Top = PSet.siz__Sub.Top;
            this.Left = PSet.siz__Sub.Left;

            tmr_SSTs.Enabled = true;
        }

        public void SSTs_Running()
        {
            this.Show();

            double ReadTime = 0, Old_Time = 0;
            double OfstTime = DateTime.Now.Ticks;
            double Gap_Time = DateTime.Now.AddSeconds(0.1).Ticks;
            double Gap_Ofst = 0;
            int TestStep = 0;
            bool TestFlag = false;
            bool Key_Pass = false;
            bool old_Pass = false;

            img_SSTs.Green_Show = true;
            img_SSTs.Yellow_Show = true;
            img_SSTs.Red_Show = true;
            img_SSTs.Center_Show = false;
            img_SSTs.Niddle_Show = true;
            img_SSTs.Value_Show = true;

            img_SSTs.CarNo = TSet.Vin___No;
            img_SSTs.Title = PSet.Lang_SST[0];      //사이드슬립
            img_SSTs.Message = PSet.Lang_SST[1];    //진입하세요
            img_SSTs.Value = Convert.ToSingle(Max_SST);

            while (true)
            {
                Thread.Sleep(100);   
                #region 측정 헤더
                if (TestFlag) { TestFlag = false; }
                if (TSet.TestStop) { break; }
                if (!PSet.Onf_Prog) { break; }
                if (TSet.StepNext) { TSet.StepNext = false; }

                ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;

                if ((H2Y.GetKeyState(H2Y.VK_SHIFT) & H2Y.KeyPressed) != 0 && (H2Y.GetKeyState(H2Y.VK_RETURN) & H2Y.KeyPressed) != 0)
                {
                    Key_Pass = true;
                }
                else
                {
                    Key_Pass = false;
                }

                if (Key_Pass && Key_Pass != old_Pass) { TSet.StepNext = true; }
                if (Key_Pass != old_Pass) { old_Pass = Key_Pass; }
                #endregion

                if (PLC.DO.MD_Emerge) break;
                if (TSet.TestStop) break;
                if (PLC.DI.PSW__Stop) break;
                
                if (TestStep == 0 && !TestFlag)
                {
                    img_SSTs.Message = PSet.Lang_SST[1];    //진입하세요

                    TSet.Read_SST = 0; Max_SST = 0;
                    TSet.StepNext = true;
                    if (TSet.StepNext) { TestFlag = true; TestStep = 1; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 1 && !TestFlag)
                {
                    if (TSet.SST_Enter) TSet.StepNext = true;
                    if (TSet.StepNext) { TestFlag = true; TestStep = 2; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 2 && !TestFlag)
                {
                    img_SSTs.Message = PSet.Lang_SST[3];    //측정중입니다.

                    TSet.StepNext = true;
                    if (TSet.StepNext) { TestFlag = true; TestStep = 3; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 3 && !TestFlag)
                {
                    if (TSet.SST_GoOut) TSet.StepNext = true;
                    if (TSet.StepNext) { TestFlag = true; TestStep = 4; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 4 && !TestFlag)
                {
                    TSet.SST1_Val = H2Y.toFloat((Math.Abs(Max_SST)).ToString("#0.0"));
                    TSet.SST1Sine = Max_SST > 0 ? "OUT" : "IN";
                    TSet.SST1_Pan = H2Y.Ret_JudgeOX(PSet.RnB.SST__Min, PSet.RnB.SST__Max, Max_SST);
                    //TSet.SST1_Pan = Math.Abs(Max_SST) <= 5 ? H2Y.OK : H2Y.NG;

                    img_SSTs.Message = TSet.SST1_Pan;

                    TSet.StepNext = true;
                    if (TSet.StepNext) { TestFlag = true; TestStep = 5; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 5 && !TestFlag)
                {
                    if (ReadTime - Gap_Ofst > 0.5) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 6; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 6 && !TestFlag)
                {
                    break;
                }

                if ((ReadTime - Old_Time) >= 0.1)
                {
                    Old_Time = ReadTime;
                }

                System.Windows.Forms.Application.DoEvents();
            }
            this.Close();
        }

        public void Calibration()
        {
            this.Show();

            img_SSTs.Green_Show = false;
            img_SSTs.Yellow_Show = false;
            img_SSTs.Red_Show = false;
            img_SSTs.Center_Show = false;
            img_SSTs.Niddle_Show = true;
            img_SSTs.Value_Show = true;

            img_SSTs.CarNo = "";
            img_SSTs.Title = PSet.Lang_SST[2];    //교정
            img_SSTs.Message = "";
        }

        private void pic_SSTs_Paint(object sender, PaintEventArgs e)
        {
            if (IsOpen)
            {
                img_SSTs.Gage_Show(e.Graphics);
            }
        }

        private void tmr_SSTs_Tick(object sender, EventArgs e)
        {
            TSet.Scan_Sensors();

            if (Math.Abs(Max_SST) < Math.Abs(TSet.Read_SST))
            {
                Max_SST = TSet.Read_SST;
            }

            img_SSTs.Value = Convert.ToSingle(Max_SST);
        }
    }
}
