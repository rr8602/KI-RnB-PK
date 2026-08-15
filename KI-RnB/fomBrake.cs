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
    public partial class fomBrake : Form
    {
        public bool IsOpen;

        private fom_Main main;
        private const int Gap_Wait = 2;
        private const int Divider = 5;
        private const int ParkMode = 0; //0:전/후 파킹, 1:후 파킹(전체 축중), 2:후 파킹(후 축중)
        
        private const string A0_Init = "Init";      //"초기화";
        private const string A1_Drag = "F.Drag";    //"전축 끌림";
        private const string A1Front = "Front";     //"전축 제동력";
        private const string A2_Drag = "R.Drag";    //"후축 끌림";
        private const string A2_Rear = "Rear";      //"후축 제동력";
        private const string Parking = "Parking";   //"주차";
        private const string Total_B = "Total";     //"전체";
        
        private string Brake_OX = "";

        private double BrkL_Max;  //좌 제동력(kgf)
        private double BrkR_Max;  //우 제동력(kgf)
        private double Brk_Diff;  //제동력 편차(%)
        private double Brk__Sum;  //제동력 합(%)

        private string Pan_Drag;
        private string Pan__Sum;
        private string Pan_Diff;
        private string PanJudge;

        private float Pan_Show = 2;
        private float CheckGap = 0.2f;
                
        private string old_Axle;
        private double old_Time;

        public fomBrake()
        {
            InitializeComponent();
        }
        public fomBrake(fom_Main main)
            : this()
        {
            this.main = main;
        }
        private void fomBrake_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsOpen = false;
        }
        private void fomBrake_Load(object sender, EventArgs e)
        {
            IsOpen = true;
            
            this.Top = PSet.siz__Sub.Top;
            this.Left = PSet.siz__Sub.Left;
            
            pic_Head.Size = new System.Drawing.Size(1024, 90);  pic_Head.Dock = DockStyle.Top;
            pic_Msgs.Size = new System.Drawing.Size(1024, 110); pic_Msgs.Location = new Point(0, 88);
            pic_Brks.Size = new System.Drawing.Size(1024, 577); pic_Brks.Dock = DockStyle.Bottom;
            pic_Msgs.BringToFront();

            H2Y.Screen__Head(pic_Head, A1Front, "Vehicle", "Model");
            H2Y.Message_Show(pic_Msgs, "Please enter");
            Brk__Measure(pic_Brks, A0_Init);   
        }

        public void BRKs_Running()
        {
            this.Show();

            TSet.StopFlag = TSet.TestStopFlag(TSet.StopFlag);
            if (TSet.StopFlag > 0) return;

            if (TSet.StopFlag == 0) Brk_Axle_Run(0);
            if (TSet.StopFlag == 0) Brk_Axle_Run(1);

            if (TSet.StopFlag == 0) Total__Brake(); H2Y.Sleep(2);
            if (ParkMode == 0)
            {
                if (TSet.StopFlag == 0) TotalParking(); H2Y.Sleep(2);
            }
            if (TSet.StopFlag == 0) Brake__Judge(); H2Y.Sleep(2);

            if (TSet.StopFlag == 0) Brk_MDB_Dave(TSet.AcceptNo);

            this.Close();
        }

        private void Total__Brake()
        {
            H2Y.Screen__Head(pic_Head, Total_B, TSet.Vin___No, TSet.CarModel);
            H2Y.Message_Show(pic_Msgs, "Total Brake");
            Brk__Measure(pic_Brks, Total_B);
            System.Windows.Forms.Application.DoEvents();

            TotalB.BrakeL = Math.Round(H2Y.Sum_Val(Axle_1.Brk__L, Axle_2.Brk__L), 0);                  //전체 좌 제동력(kgf)
            TotalB.BrakeR = Math.Round(H2Y.Sum_Val(Axle_1.Brk__R, Axle_2.Brk__R), 0);                  //전체 우 제동력(kgf)
            TotalB.Weight = Math.Round(H2Y.Sum_Val(Axle_1.Weight, Axle_2.Weight), 0);                  //전체 중량(kgf)
            TotalB.BrakeV = Math.Round(H2Y.Sum_Pst(TotalB.BrakeL, TotalB.BrakeR, TotalB.Weight), 1);   //전체 제동력 합(%)

            if (PSet.BRK.BrkTotal <= TotalB.BrakeV)
            {
                TotalB.BrakeP = H2Y.OK;   //전체 제동력 판정
                H2Y.Message_Show(pic_Msgs, "Total Brake " + TotalB.BrakeP, Color.Yellow);
            }
            else
            {
                TotalB.BrakeP = H2Y.NG;
                H2Y.Message_Show(pic_Msgs, "Total Brake " + TotalB.BrakeP, Color.Red);
            }

            H2Y.Sleep(Pan_Show * 1000);
        }

        private void TotalParking()
        {
            H2Y.Screen__Head(pic_Head, Parking, TSet.Vin___No, TSet.CarModel);
            H2Y.Message_Show(pic_Msgs, "Total Parking");
            Brk__Measure(pic_Brks, Parking);
            System.Windows.Forms.Application.DoEvents();

            Parkin.BrakeL = Math.Round(H2Y.Sum_Val(Axle_1.Park_L, Axle_2.Park_L), 0);                  //전체 좌 제동력(kgf)
            Parkin.BrakeR = Math.Round(H2Y.Sum_Val(Axle_1.Park_R, Axle_2.Park_R), 0);                  //전체 우 제동력(kgf)
            Parkin.Weight = Math.Round(H2Y.Sum_Val(Axle_1.Weight, Axle_2.Weight), 0);                  //전체 중량(kgf)
            Parkin.BrakeV = Math.Round(H2Y.Sum_Pst(Parkin.BrakeL, Parkin.BrakeR, Parkin.Weight), 1);   //전체 제동력 합(%)

            if (PSet.BRK.Brk_Park <= Parkin.BrakeV)
            {
                Parkin.BrakeP = H2Y.OK;   //전체 제동력 판정
                H2Y.Message_Show(pic_Msgs, "Parking " + Parkin.BrakeP, Color.Yellow);
            }
            else
            {
                Parkin.BrakeP = H2Y.NG;
                H2Y.Message_Show(pic_Msgs, "Parking " + Parkin.BrakeP, Color.Red);
            }

            H2Y.Sleep(Pan_Show * 1000);
        }

        private void Brake__Judge()
        {
            H2Y.Screen__Head(pic_Head, "", TSet.Vin___No, TSet.CarModel);
            H2Y.Message_Show(pic_Msgs, "Brake judgment");
            Brk__Measure(pic_Brks, Total_B);
            System.Windows.Forms.Application.DoEvents();

            if (Axle_1.JudgeP == H2Y.OK && Axle_2.JudgeP == H2Y.OK && TotalB.BrakeP == H2Y.OK && Parkin.BrakeP == H2Y.OK)
            {
                Brake_OX = H2Y.OK;   //제동력 판정
                H2Y.Message_Show(pic_Msgs, "Brake judgment " + Brake_OX, Color.Yellow);
                System.Windows.Forms.Application.DoEvents();
            }
            else
            {
                Brake_OX = H2Y.NG;
                string JudgeMsg = "";
                if (Axle_1.JudgeP != H2Y.OK) { JudgeMsg = "Front "; }
                if (Axle_2.JudgeP != H2Y.OK) { JudgeMsg = "Rear "; }
                if (TotalB.BrakeP != H2Y.OK) { JudgeMsg = "Total "; }
                if (Parkin.BrakeP != H2Y.OK) { JudgeMsg = "Parking "; }
                H2Y.Message_Show(pic_Msgs, JudgeMsg + Brake_OX, Color.Red);
                System.Windows.Forms.Application.DoEvents();
            }

            H2Y.Sleep(Pan_Show * 1000);
        }

        private void Brk_Axle_Run(int Axle)
        {
            double ReadTime = 0;
            double OfstTime = DateTime.Now.Ticks;
            double Gap_Ofst = 0;
            int TestStep = 0;
            bool TestFlag = false;
            bool Key_Pass = false;
            bool old_Pass = false;
            int ErrorCNT = 0;
            string JudgeMsg = "";

            Brk__Measure(pic_Brks, A0_Init);

            switch (Axle)
            {
                case 0: Brk__Measure(pic_Brks, A1Front); break;
                case 1: Brk__Measure(pic_Brks, A2_Rear); break;
            }
            H2Y.Message_Show(pic_Msgs, "Braking force");

            TestStep = 0;
            while (true)
            {
                TSet.StopFlag = TSet.TestStopFlag(TSet.StopFlag);
                if (TSet.StopFlag > 0) break;

                #region 측정 헤더
                if (TestFlag)
                {
                    TestFlag = false;
                }

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

                TSet.Scan_Sensors();
                
                if (0 <= TestStep && TestStep <= 7)
                {
                    BrkL_Max = 0;
                    BrkR_Max = 0;

                    Brk__Measure(pic_Brks, A0_Init);
                }

                if (TestStep == 0 && !TestFlag)
                {
                    ErrorCNT = 0;

                    switch (Axle)
                    {
                        case 0: H2Y.Screen__Head(pic_Head, "Front", TSet.Vin___No, TSet.CarModel); break;
                        case 1: H2Y.Screen__Head(pic_Head, "Rear", TSet.Vin___No, TSet.CarModel); break;
                    }

                    TestFlag = true; TestStep = 1; Gap_Ofst = ReadTime;
                }

                if (TestStep == 1 && !TestFlag)
                {
                    switch (Axle)
                    {
                        case 0: H2Y.Msg_Speash("Come in the front"); H2Y.Message_Show(pic_Msgs, "Come in the front", Color.Lime); break;
                        case 1: H2Y.Msg_Speash("Come in the rear"); H2Y.Message_Show(pic_Msgs, "Come in the rear", Color.Lime); break;
                    }

                    fom__WGT Wgt_T = new fom__WGT(main);
                    switch (Axle)
                    {
                        case 0: Axle_1.Weight = Math.Round(Wgt_T.WGTs_Running(0), 0); break; //전축 축중 측정
                        case 1: Axle_2.Weight = Math.Round(Wgt_T.WGTs_Running(1), 0); break; //후축 축중 측정
                    }

                    if (TSet.PHO_Brake) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 2; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 2 && !TestFlag)
                {
                    TSet.StepNext = true;
                    if (TSet.StepNext) { TestFlag = true; TestStep = 3; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 3 && !TestFlag)
                {
                    H2Y.Message_Show(pic_Msgs, "Lift Down", Color.Lime);
                    if (ReadTime - Gap_Ofst > 0.2)
                    {
                        Gap_Ofst = ReadTime;
                        PLC.Brk_LiftDown();
                    }

                    if (TSet.BT_LiftDn) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 4; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 4 && !TestFlag)
                {
                    TSet.StepNext = true;
                    if (TSet.StepNext) { TestFlag = true; TestStep = 5; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 5 && !TestFlag)
                {
                    if (TSet.PHO_Brake)
                    {
                        if (ReadTime - Gap_Ofst > Gap_Wait) { TSet.StepNext = true; }
                    }
                    else
                    {
                        H2Y.Message_Show(pic_Msgs, "Location check", Color.Lime);
                    }


                    if (TSet.StepNext) { TestFlag = true; TestStep = 6; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 6 && !TestFlag)
                {
                    if (ErrorCNT == 0)
                    {
                        H2Y.Message_Show(pic_Msgs, "Motor drive", Color.Red);
                    }
                    else
                    {
                        H2Y.Message_Show(pic_Msgs, "Remeasurement", Color.Red);
                    }

                    if (ReadTime - Gap_Ofst > 0.2)
                    {
                        Gap_Ofst = ReadTime;
                        PLC.Brk_MotorRun(1);
                    }
                    
                    if (TSet.BT_MotRun == true) { TSet.StepNext = true; } 
                    if (TSet.StepNext) { TestFlag = true; TestStep = 7; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 7 && !TestFlag)
                {
                    if (ReadTime - Gap_Ofst > Gap_Wait) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 8; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 8 && !TestFlag)
                {
                    ErrorCNT++; //재측정 카운터
                    BrkL_Max = 0;
                    BrkR_Max = 0;

                    TSet.StepNext = true;
                    if (TSet.StepNext) { TestFlag = true; TestStep = 9; Gap_Ofst = ReadTime; }
                }
                
                if (TestStep == 9 && !TestFlag)
                {
                    TSet.StepNext = true;
                    if (TSet.StepNext) { TestFlag = true; TestStep = 10; Gap_Ofst = ReadTime; }
                }
                
                if (TestStep == 10 && !TestFlag)
                {
                    H2Y.Msg_Speash("Release the pedal");
                    H2Y.Message_Show(pic_Msgs, "Release the pedal " + (PSet.BRK.DragTime - (ReadTime - Gap_Ofst)).ToString("#0"), Color.White);

                    switch (Axle)
                    {
                        case 0: Pan_Drag = Brk__Measure(pic_Brks, A1_Drag); break;
                        case 1: Pan_Drag = Brk__Measure(pic_Brks, A2_Drag); break;
                    }

                    if (ReadTime - Gap_Ofst > PSet.BRK.DragTime)
                    {
                        switch (Axle)
                        {
                            case 0: 
                                Pan_Drag = Brk__Measure(pic_Brks, A1_Drag);
                                Axle_1.Drag_L = Math.Round(BrkL_Max, 0);    //1축 좌 끌림 (kgf)
                                Axle_1.Drag_R = Math.Round(BrkR_Max, 0);    //1축 우 끌림 (kgf)
                                Axle_1.Drag_V = Math.Round(Brk__Sum, 1);    //1축 끌림 (%)
                                Axle_1.Drag_P = Pan_Drag;                   //1축 끌림 판정(OK, NG)
                                break;

                            case 1: 
                                Pan_Drag = Brk__Measure(pic_Brks, A2_Drag);
                                Axle_2.Drag_L = Math.Round(BrkL_Max, 0);    //2축 좌 끌림 (kgf)
                                Axle_2.Drag_R = Math.Round(BrkR_Max, 0);    //2축 우 끌림 (kgf)
                                Axle_2.Drag_V = Math.Round(Brk__Sum, 1);    //2축 끌림 (%)
                                Axle_2.Drag_P = Pan_Drag;       //2축 끌림 판정(OK, NG)
                                break;
                        }

                        TSet.StepNext = true;
                        if (Pan_Drag == H2Y.OK)
                        {
                            if (TSet.StepNext) { TestFlag = true; TestStep = 11; Gap_Ofst = ReadTime; }
                        }
                        else
                        {
                            if (TSet.StepNext) { TestFlag = true; TestStep = 12; Gap_Ofst = ReadTime; }
                        }
                    }
                }

                if (TestStep == 11 && !TestFlag)
                {
                    TSet.StepNext = true;
                    if (TSet.StepNext) { TestFlag = true; TestStep = 12; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 12 && !TestFlag)
                {
                    switch (Axle)
                    {
                        case 0: Pan__Sum = Brk__Measure(pic_Brks, A1Front); break;
                        case 1: Pan__Sum = Brk__Measure(pic_Brks, A2_Rear); break;
                    }

                    H2Y.Msg_Speash("Step on the brake");
                    H2Y.Message_Show(pic_Msgs, "Step on the brake " + (PSet.BRK.Brk_Time - (ReadTime - Gap_Ofst)).ToString("#0"), Color.Magenta);

                    if (ReadTime - Gap_Ofst > PSet.BRK.Brk_Time)
                    {
                        switch (Axle)
                        {
                            case 0:
                                Pan__Sum = Brk__Measure(pic_Brks, A1Front);
                                Axle_1.Brk__L = Math.Round(BrkL_Max, 0);    //1축 좌 제동력(kgf)
                                Axle_1.Brk__R = Math.Round(BrkR_Max, 0);    //1축 우 제동력(kgf)
                                Axle_1.Diff_V = Math.Round(Brk_Diff, 1);    //1축 제동력 편차(%)
                                Axle_1.Sum__V = Math.Round(Brk__Sum, 1);    //1축 제동력 합(%)
                                Axle_1.Diff_P = Pan_Diff;
                                Axle_1.Sum__P = Pan__Sum;
                                break;

                            case 1:
                                Pan__Sum = Brk__Measure(pic_Brks, A2_Rear); 
                                Axle_2.Brk__L = Math.Round(BrkL_Max, 0);    //2축 좌 제동력(kgf)
                                Axle_2.Brk__R = Math.Round(BrkR_Max, 0);    //2축 우 제동력(kgf)
                                Axle_2.Diff_V = Math.Round(Brk_Diff, 1);    //2축 제동력 편차(%)
                                Axle_2.Sum__V = Math.Round(Brk__Sum, 1);    //2축 제동력 합(%)
                                Axle_2.Diff_P = Pan_Diff;
                                Axle_2.Sum__P = Pan__Sum;
                                break;
                        }
                        TSet.StepNext = true;
                    }

                    if (TSet.StepNext) { TestFlag = true; TestStep = 13; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 13 && !TestFlag)
                {
                    H2Y.Message_Show(pic_Msgs, "Motor stop", Color.Lime);
                    if (ReadTime - Gap_Ofst > 0.2) 
                    {
                        Gap_Ofst = ReadTime; 
                        PLC.Brk_MotorRun(0); 
                    }

                    if (TSet.BT_MotRun == false) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 14; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 14 && !TestFlag)
                {
                    if (Pan_Drag == H2Y.OK && Pan_Diff == H2Y.OK && Pan__Sum == H2Y.OK)
                    {
                        PanJudge = H2Y.OK;       //제동력 판정(OK, NG)
                        H2Y.Message_Show(pic_Msgs, "Brake " + PanJudge, Color.Lime);
                    }
                    else
                    {
                        PanJudge = H2Y.NG;
                        JudgeMsg = "";
                        if (Pan_Drag == H2Y.NG) { JudgeMsg += "Drag "; }
                        if (Pan_Diff == H2Y.NG) { JudgeMsg += "Diff. "; }
                        if (Pan__Sum == H2Y.NG) { JudgeMsg += "Sum "; }

                        H2Y.Message_Show(pic_Msgs, JudgeMsg + " " + PanJudge, Color.Red);
                    }

                    switch (Axle)
                    {
                        case 0: Axle_1.JudgeP = PanJudge; break;
                        case 1: Axle_2.JudgeP = PanJudge; break;
                    }

                    if (Math.Abs(ReadTime - Gap_Ofst) > Pan_Show)
                    {
                        BrkL_Max = 0;
                        BrkR_Max = 0;

                        TSet.StepNext = true;
                    }

                    if (TSet.StepNext)
                    {
                        if (PanJudge == H2Y.NG)
                        {
                            if (ErrorCNT >= PSet.BRKCount + 1)
                            {
                                TestFlag = true; TestStep = 15; Gap_Ofst = ReadTime;
                            }
                            else
                            {
                                TestFlag = true; TestStep = 5; Gap_Ofst = ReadTime;
                            }
                        }
                        else
                        {
                            TestFlag = true; TestStep = 15; Gap_Ofst = ReadTime;
                        }
                    }
                }

                if (TestStep == 15 && !TestFlag)
                {
                    switch (Axle)
                    {
                        case 0: TSet.StepNext = true; break;
                        case 1: Parking__Run(Axle); TSet.StepNext = true; break;
                    }

                    if (TSet.StepNext) { TestFlag = true; TestStep = 16; Gap_Ofst = ReadTime; }
                }
                

                if (TestStep == 16 && !TestFlag)
                {
                    H2Y.Message_Show(pic_Msgs, "Lift Up.", Color.Lime);
                    if (ReadTime - Gap_Ofst > 0.2)
                    {
                        Gap_Ofst = ReadTime;
                        PLC.Brk_Lift__Up();
                    }

                    if (TSet.BT_LiftUp) { TSet.StepNext = true; } 
                    if (TSet.StepNext) { TestFlag = true; TestStep = 17; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 17 && !TestFlag)
                {
                    switch (Axle)
                    {
                        case 0: H2Y.Screen__Head(pic_Head, "Front", TSet.Vin___No, TSet.CarModel); break;
                        case 1: H2Y.Screen__Head(pic_Head, "Rear", TSet.Vin___No, TSet.CarModel); break;
                    }

                    H2Y.Message_Show(pic_Msgs, "Waiting", Color.Lime);

                    TSet.StepNext = true;
                    if (TSet.StepNext) { TestFlag = true; TestStep = 18; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 18 && !TestFlag)
                {
                    if (ReadTime - Gap_Ofst > Gap_Wait) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 19; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 19 && !TestFlag)
                {
                    H2Y.Msg_Speash("Move on");
                    H2Y.Message_Show(pic_Msgs, "Move on", Color.Magenta);

                    if (!TSet.PHO_Brake) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 20; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 20 && !TestFlag)
                {
                    break;
                }
                                    
                System.Windows.Forms.Application.DoEvents();
            }

            PLC.Brk_Lift__Up();
        }

        public void Parking__Run(int Axle)
        {
            double ReadTime = 0;
            double OfstTime = DateTime.Now.Ticks;
            double Gap_Ofst = 0;
            int Park_CNT = 0;
            int TestStep = 0;
            bool TestFlag = false;
            bool Key_Pass = false;
            bool old_Pass = false;

            H2Y.Screen__Head(pic_Head, "Parking", TSet.Vin___No, TSet.CarModel);
            Brk__Measure(pic_Brks, Parking);
            H2Y.Message_Show(pic_Msgs, "Release the PB");

            TestStep = 0;
            while (true)
            {
                TSet.StopFlag = TSet.TestStopFlag(TSet.StopFlag);
                if (TSet.StopFlag > 0) break;

                #region 측정 헤더
                if (TestFlag)
                {
                    TestFlag = false;
                }

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

                TSet.Scan_Sensors();

                if (TestStep == 0 && !TestFlag)
                {
                    Park_CNT = 0;
                    BrkL_Max = 0; 
                    BrkR_Max = 0;

                    H2Y.Screen__Head(pic_Head, "Parking", TSet.Vin___No, TSet.CarModel);
                    switch (Axle)
                    {
                        case 0: H2Y.Message_Show(pic_Msgs, "Front parking"); break;
                        case 1: H2Y.Message_Show(pic_Msgs, "Rear parking"); break;
                    }
                    Brk__Measure(pic_Brks, A0_Init);

                        TSet.StepNext = true;
                    if (TSet.StepNext) { TestFlag = true; TestStep = 1; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 1 && !TestFlag)
                {
                    if (TSet.PHO_Brake)
                    {
                        if (Math.Abs(ReadTime - Gap_Ofst) > Gap_Wait) { TSet.StepNext = true; }
                    }
                    else
                    {
                        H2Y.Message_Show(pic_Msgs, "Location check", Color.Lime);
                    }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 2; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 2 && !TestFlag)
                {
                    if (Park_CNT == 0)
                    {
                        H2Y.Message_Show(pic_Msgs, "Motor drive", Color.Red);
                    }
                    else
                    {
                        H2Y.Message_Show(pic_Msgs, "Remeasurement", Color.Red);
                    }

                    if (ReadTime - Gap_Ofst > 0.2)
                    {
                        Gap_Ofst = ReadTime;
                        PLC.Brk_MotorRun(1);
                    }

                    if (TSet.BT_MotRun == true) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 3; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 3 && !TestFlag)
                {
                    BrkL_Max = 0;
                    BrkR_Max = 0;
                    Park_CNT++;

                    TSet.StepNext = true; 
                    if (TSet.StepNext) { TestFlag = true; TestStep = 4; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 4 && !TestFlag)
                {
                    H2Y.Msg_Speash("Release the parking brake");
                    H2Y.Message_Show(pic_Msgs, "Release the PB");
                    BrkL_Max = 0; 
                    BrkR_Max = 0;

                    if (Math.Abs(ReadTime - Gap_Ofst) > 2) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 6; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 6 && !TestFlag)
                {
                    H2Y.Msg_Speash("Apply the parking brake");
                    H2Y.Message_Show(pic_Msgs, "Apply the PB " + (PSet.BRK.ParkTime - (ReadTime - Gap_Ofst)).ToString("#0"), Color.Yellow);

                    Pan__Sum = Brk__Measure(pic_Brks, Parking);

                    if (Math.Abs(ReadTime - Gap_Ofst) > PSet.BRK.ParkTime)
                    {
                        switch (Axle)
                        {
                            case 0:
                                Pan__Sum = Brk__Measure(pic_Brks, Parking);
                                Axle_1.Park_L = Math.Round(BrkL_Max, 0);    //1축 좌 주차 (kgf)
                                Axle_1.Park_R = Math.Round(BrkR_Max, 0);    //1축 우 주차 (kgf)
                                break;

                            case 1:
                                Pan__Sum = Brk__Measure(pic_Brks, Parking);
                                Axle_2.Park_L = Math.Round(BrkL_Max, 0);    //1축 좌 주차 (kgf)
                                Axle_2.Park_R = Math.Round(BrkR_Max, 0);    //1축 우 주차 (kgf)

                                if (ParkMode == 2)
                                {
                                    Parkin.BrakeL = Math.Round(Axle_2.Park_L, 0);                  //전체 좌 제동력(kgf)
                                    Parkin.BrakeR = Math.Round(Axle_2.Park_R, 0);                  //전체 우 제동력(kgf)
                                    Parkin.Weight = Math.Round(Axle_2.Weight, 0);                  //전체 중량(kgf)
                                    Parkin.BrakeV = Math.Round(H2Y.Sum_Pst(Parkin.BrakeL, Parkin.BrakeR, Parkin.Weight), 1);   //전체 제동력 합(%)
                                    Parkin.BrakeP = Pan__Sum;
                                }
                                break;
                        }

                        TSet.StepNext = true;
                    }

                    if (TSet.StepNext) { TestFlag = true; TestStep = 7; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 7 && !TestFlag)
                {
                    H2Y.Message_Show(pic_Msgs, "Motor stop", Color.Red);

                    if (ReadTime - Gap_Ofst > 0.2)
                    {
                        Gap_Ofst = ReadTime;
                        PLC.Brk_MotorRun(0);
                    }

                    if (TSet.BT_MotRun == false) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 8; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 8 && !TestFlag)
                {
                    if (Pan__Sum == H2Y.NG)
                    {
                        if (ParkMode == 0)
                        {
                            switch (Axle)
                            {
                                case 0: H2Y.Message_Show(pic_Msgs, "Front parking " + Pan__Sum, Color.Red); break;
                                case 1: H2Y.Message_Show(pic_Msgs, "Rear parking " + Pan__Sum, Color.Red); break;
                            }
                        }
                        else
                        {
                            H2Y.Message_Show(pic_Msgs, "parking " + Pan__Sum, Color.Red);
                        }


                        if (Park_CNT >= PSet.BRKCount + 1)
                        {
                            TestFlag = true; TestStep = 9; Gap_Ofst = ReadTime;
                        }
                        else
                        {
                            TestFlag = true; TestStep = 2; Gap_Ofst = ReadTime; //재측정
                        }
                    }
                    else
                    {
                        switch (Axle)
                        {
                            case 0: H2Y.Message_Show(pic_Msgs, "Front parking " + Pan__Sum, Color.Lime); break;
                            case 1: H2Y.Message_Show(pic_Msgs, "Rear parking " + Pan__Sum, Color.Lime); break;
                        }
                        
                        TestFlag = true; TestStep = 9; Gap_Ofst = ReadTime;
                    }
                }

                if (TestStep == 9 && !TestFlag)
                {
                    if (Math.Abs(ReadTime - Gap_Ofst) > Pan_Show) { TSet.StepNext = true; }

                    if (TSet.StepNext) { TestFlag = true; TestStep = 10; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 10 && !TestFlag)
                {
                    break;
                }
                
                System.Windows.Forms.Application.DoEvents();
            }
        }

        private void Brk_MDB_Dave(string pAcptNo)
        {
            int Data_CNT = 0;

            if (pAcptNo == null) return;

            Data_CNT = main.DB_All.DBBrake.Select(pAcptNo);

            main.DB_All.DBBrake.dbAcceptNo = pAcptNo;

            main.DB_All.DBBrake.db1_Weight = Math.Round(Axle_1.Weight, 0);
            main.DB_All.DBBrake.db1_Wgt__L = Math.Round(Axle_1.Wgt__L, 0);
            main.DB_All.DBBrake.db1_Wgt__R = Math.Round(Axle_1.Wgt__R, 0);

            main.DB_All.DBBrake.db1Drag__L = Math.Round(Axle_1.Drag_L, 0);
            main.DB_All.DBBrake.db1Drag__R = Math.Round(Axle_1.Drag_R, 0);
            main.DB_All.DBBrake.db1Drag__V = Math.Round(Axle_1.Drag_V, 1);
            main.DB_All.DBBrake.db1Drag_OX = Axle_1.Drag_P;
            main.DB_All.DBBrake.db1Brake_L = Math.Round(Axle_1.Brk__L, 0);
            main.DB_All.DBBrake.db1Brake_R = Math.Round(Axle_1.Brk__R, 0);
            main.DB_All.DBBrake.db1Diff__V = Math.Round(Axle_1.Diff_V, 1);
            main.DB_All.DBBrake.db1Diff_OX = Axle_1.Diff_P;
            main.DB_All.DBBrake.db1Sum___V = Math.Round(Axle_1.Sum__V, 1);
            main.DB_All.DBBrake.db1Sum__OX = Axle_1.Sum__P;
            main.DB_All.DBBrake.db1BrakeOX = Axle_1.JudgeP;
            main.DB_All.DBBrake.db1Park__L = Math.Round(Axle_1.Park_L, 0);
            main.DB_All.DBBrake.db1Park__R = Math.Round(Axle_1.Park_R, 0);

            main.DB_All.DBBrake.db2_Weight = Math.Round(Axle_2.Weight, 0);
            main.DB_All.DBBrake.db2_Wgt__L = Math.Round(Axle_2.Wgt__L, 0);
            main.DB_All.DBBrake.db2_Wgt__R = Math.Round(Axle_2.Wgt__R, 0);

            main.DB_All.DBBrake.db2Drag__L = Math.Round(Axle_2.Drag_L, 0);
            main.DB_All.DBBrake.db2Drag__R = Math.Round(Axle_2.Drag_R, 0);
            main.DB_All.DBBrake.db2Drag__V = Math.Round(Axle_2.Drag_V, 1);
            main.DB_All.DBBrake.db2Drag_OX = Axle_2.Drag_P;
            main.DB_All.DBBrake.db2Brake_L = Math.Round(Axle_2.Brk__L, 0);
            main.DB_All.DBBrake.db2Brake_R = Math.Round(Axle_2.Brk__R, 0);
            main.DB_All.DBBrake.db2Diff__V = Math.Round(Axle_2.Diff_V, 1);
            main.DB_All.DBBrake.db2Diff_OX = Axle_2.Diff_P;
            main.DB_All.DBBrake.db2Sum___V = Math.Round(Axle_2.Sum__V, 1);
            main.DB_All.DBBrake.db2Sum__OX = Axle_2.Sum__P;
            main.DB_All.DBBrake.db2BrakeOX = Axle_2.JudgeP;
            main.DB_All.DBBrake.db2Park__L = Math.Round(Axle_2.Park_L, 0);
            main.DB_All.DBBrake.db2Park__R = Math.Round(Axle_2.Park_R, 0);

            main.DB_All.DBBrake.dbT_Weight = Math.Round(TotalB.Weight, 0);
            main.DB_All.DBBrake.dbTBrake_L = Math.Round(TotalB.BrakeL, 0);
            main.DB_All.DBBrake.dbTBrake_R = Math.Round(TotalB.BrakeR, 0);
            main.DB_All.DBBrake.dbTBrake_V = Math.Round(TotalB.BrakeV, 1);
            main.DB_All.DBBrake.dbTBrakeOX = TotalB.BrakeP;

            main.DB_All.DBBrake.dbAPark__L = Math.Round(Parkin.BrakeL, 0);
            main.DB_All.DBBrake.dbAPark__R = Math.Round(Parkin.BrakeR, 0);
            main.DB_All.DBBrake.dbAPark__V = Math.Round(Parkin.BrakeV, 1);
            main.DB_All.DBBrake.dbAPark_OX = Parkin.BrakeP;

            main.DB_All.DBBrake.dbBrake_OX = Brake_OX;

            if (Data_CNT == 0)
            {
                main.DB_All.DBBrake.Insert();
            }
            else
            {
                main.DB_All.DBBrake.Update(pAcptNo);
            }
        }

        //이미지 처리
        private string Brk__Measure(PictureBox pic, string Axle)   
        {
            double  weight = 0;
            string Judge_OX = H2Y.OK; 

            if (TSet.SimulOnf)
            {
                PSet.CH4_Val = TSet.VirtualL_B;
                PSet.CH5_Val = TSet.VirtualR_B;
            }
            else
            {
                TSet.PHO_Brake = PLC.DI.PHO_Brake;
            }

            try
            {
                switch (Axle)
                {
                    case A0_Init: 
                        weight = 0;
                        BrkL_Max = Math.Round(PSet.CH4_Val, 0); 
                        BrkR_Max = Math.Round(PSet.CH5_Val, 0); 
                        Brk__Sum = Math.Round(H2Y.DVD(BrkL_Max + BrkR_Max, weight) * 100, 1);
                        Brk_Diff = Math.Round(H2Y.DVD(Math.Abs(BrkL_Max - BrkR_Max), weight) * 100, 1);

                        Pan_Diff = H2Y.OK;
                        Pan__Sum = H2Y.OK;
                        break;

                    case A1_Drag: 
                        weight = Math.Round(Axle_1.Weight, 0);
                        if (BrkL_Max < PSet.CH4_Val) { BrkL_Max = PSet.CH4_Val; }
                        if (BrkR_Max < PSet.CH5_Val) { BrkR_Max = PSet.CH5_Val; }
                        Brk__Sum = Math.Round(H2Y.DVD(BrkL_Max + BrkR_Max, weight) * 100, 1);
                        Brk_Diff = Math.Round(H2Y.DVD(Math.Abs(BrkL_Max - BrkR_Max), weight) * 100, 1);

                        Pan_Diff = H2Y.OK;
                        Pan__Sum = H2Y.Ret_JudgeOX(0, PSet.BRK.Brk_Drag, Brk__Sum);
                        break;

                    case A1Front: 
                        weight = Math.Round(Axle_1.Weight, 0);
                        if (BrkL_Max < PSet.CH4_Val) { BrkL_Max = PSet.CH4_Val; }
                        if (BrkR_Max < PSet.CH5_Val) { BrkR_Max = PSet.CH5_Val; }
                        Brk__Sum = Math.Round(H2Y.DVD(BrkL_Max + BrkR_Max, weight) * 100, 1);
                        Brk_Diff = Math.Round(H2Y.DVD(Math.Abs(BrkL_Max - BrkR_Max), weight) * 100, 1);

                        Pan_Diff = H2Y.Ret_JudgeOX(0, PSet.BRK.Brk_Diff, Brk_Diff);
                        Pan__Sum = H2Y.Ret_JudgeOX(PSet.BRK.Brk_1Std, 10000, Brk__Sum);
                        break;

                    case A2_Drag: 
                        weight = Math.Round(Axle_2.Weight, 0);
                        if (BrkL_Max < PSet.CH4_Val) { BrkL_Max = PSet.CH4_Val; }
                        if (BrkR_Max < PSet.CH5_Val) { BrkR_Max = PSet.CH5_Val; }
                        Brk__Sum = Math.Round(H2Y.DVD(BrkL_Max + BrkR_Max, weight) * 100, 1);
                        Brk_Diff = Math.Round(H2Y.DVD(Math.Abs(BrkL_Max - BrkR_Max), weight) * 100, 1);

                        Pan_Diff = H2Y.OK;
                        Pan__Sum = H2Y.Ret_JudgeOX(0, PSet.BRK.Brk_Drag, Brk__Sum);
                        break;

                    case A2_Rear: 
                        weight = Math.Round(Axle_2.Weight, 0);
                        if (BrkL_Max < PSet.CH4_Val) { BrkL_Max = PSet.CH4_Val; }
                        if (BrkR_Max < PSet.CH5_Val) { BrkR_Max = PSet.CH5_Val; }
                        Brk__Sum = Math.Round(H2Y.DVD(BrkL_Max + BrkR_Max, weight) * 100, 1);
                        Brk_Diff = Math.Round(H2Y.DVD(Math.Abs(BrkL_Max - BrkR_Max), weight) * 100, 1);

                        Pan_Diff = H2Y.Ret_JudgeOX(0, PSet.BRK.Brk_Diff, Brk_Diff);
                        Pan__Sum = H2Y.Ret_JudgeOX(PSet.BRK.Brk_2Std, 10000, Brk__Sum);
                        break;

                    case Parking:
                        if (ParkMode == 2)
                        {
                            weight = Math.Round(Axle_2.Weight, 0);
                        }
                        else
                        {
                            weight = Math.Round(Axle_1.Weight + Axle_2.Weight, 0);
                        }
                        if (BrkL_Max < PSet.CH4_Val) { BrkL_Max = PSet.CH4_Val; }
                        if (BrkR_Max < PSet.CH5_Val) { BrkR_Max = PSet.CH5_Val; }
                        Brk__Sum = Math.Round(H2Y.DVD(BrkL_Max + BrkR_Max, weight) * 100, 1);
                        Brk_Diff = Math.Round(H2Y.DVD(Math.Abs(BrkL_Max - BrkR_Max), weight) * 100,1);

                        Pan_Diff = H2Y.OK;
                        Pan__Sum = H2Y.Ret_JudgeOX(PSet.BRK.Brk_Park, 10000, Brk__Sum);
                        break;

                    case Total_B: 
                        weight = Math.Round(Axle_1.Weight + Axle_2.Weight, 0);
                        BrkL_Max = Math.Round(H2Y.Sum_Val(Axle_1.Brk__L, Axle_2.Brk__L), 0);   //전체 좌 제동력(kgf)
                        BrkR_Max = Math.Round(H2Y.Sum_Val(Axle_1.Brk__R, Axle_2.Brk__R), 0);   //전체 우 제동력(kgf)
                        Brk__Sum = Math.Round(H2Y.DVD(BrkL_Max + BrkR_Max, weight) * 100, 1);
                        Brk_Diff = Math.Round(H2Y.DVD(Math.Abs(BrkL_Max - BrkR_Max), weight) * 100, 1);

                        Pan_Diff = H2Y.OK;
                        Pan__Sum = H2Y.Ret_JudgeOX(PSet.BRK.BrkTotal, 10000, Brk__Sum);
                        break;
                }

                Brk_Scr_Show(pic, weight, BrkL_Max, BrkR_Max, Pan__Sum, Pan_Diff);

                Judge_OX = Pan_Diff == H2Y.OK && Pan__Sum == H2Y.OK ? H2Y.OK : H2Y.NG;
                return Judge_OX;
            }
            catch (Exception ex)
            {
                Logs.ExceptionErr(ex);
                return "Err";
            }
        }
        private void Brk_Scr_Show(PictureBox pic, double weight, double Brk_L, double Brk_R, string Sum_OX, string Diff_OX)
        {
            double now_Time = DateTime.Now.Ticks / H2Y.tick_Dvd;

            if (Math.Abs(now_Time - old_Time) < 0.1) { return ; }
                         old_Time = now_Time;

            double Sum = Math.Round(H2Y.DVD(Brk_L + Brk_R, weight) * 100, 1);
            double Diff = Math.Round(H2Y.DVD(Math.Abs(Brk_L - Brk_R), weight) * 100, 1);
            
            try
            {
                Bitmap bmp = new Bitmap(Properties.Resources.Brake);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    #region 이미지
                    float bmp_w = 620;
                    float top = 191f;

                    RectangleF drawRect;
                    SolidBrush blackBrush = new SolidBrush(Color.Black);
                    SolidBrush grayBrush = new SolidBrush(Color.Gray);
                    SolidBrush whiteBrush = new SolidBrush(Color.White);
                    SolidBrush redBrush = new SolidBrush(Color.Red);
                    SolidBrush greenBrush = new SolidBrush(Color.Lime);
                    SolidBrush blueBrush = new SolidBrush(Color.Blue);

                    H2Y.DrawFillRect(g, grayBrush, 60f, 361f - top, bmp_w, 20f);    //좌 제동력 눈금
                    H2Y.DrawFillRect(g, whiteBrush, 60f, 381f - top, bmp_w, 72f);   //좌 제동력
                    H2Y.DrawFillRect(g, whiteBrush, 60f, 459f - top, bmp_w, 23f);   //합
                    H2Y.DrawFillRect(g, whiteBrush, 60f, 488f - top, bmp_w, 23f);   //차
                    H2Y.DrawFillRect(g, whiteBrush, 60f, 517f - top, bmp_w, 72f);   //우 제동력
                    H2Y.DrawFillRect(g, grayBrush, 60f, 590f - top, bmp_w, 20f);    //우 제동력 눈금

                    H2Y.DrawFillRect(g, redBrush, 60f, 381f - top, (float)H2Y.DVD(bmp_w * Brk_L, PSet.BRK_Capa), 72f); //좌 제동력
                    H2Y.DrawFillRect(g, redBrush, 60f, 517f - top, (float)H2Y.DVD(bmp_w * Brk_R, PSet.BRK_Capa), 72f); //우 제동력
                    H2Y.DrawFillRect(g, greenBrush, 60f, 459f - top, (float)H2Y.DVD(bmp_w * Sum, 100), 23f);      //합  (%)
                    H2Y.DrawFillRect(g, blueBrush, 60f, 488f - top, (float)H2Y.DVD(bmp_w * Diff, 100), 23f);       //편차(%)

                    Pen whitePen = new Pen(Color.White);
                    float point = 0;

                    whitePen.Width = 2;

                    for (int cnt = 0; cnt <= Divider; cnt++)
                    {
                        point = 60f + (bmp_w / Divider) * cnt;
                        g.DrawLine(whitePen, point, 361f - top, point, 361f - top + 20f);
                        g.DrawLine(whitePen, point, 590f - top, point, 590f - top + 20f);
                    }

                    drawRect = new RectangleF(60f, 291f - top, 250.0f, 40.0F);
                    H2Y.Draw__String(g, 30, "Left brake", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(60f, 641f - top, 250.0f, 40.0F);
                    H2Y.Draw__String(g, 30, "Right brake", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(120f, 251f - top, 400.0f, 100.0F);
                    H2Y.Draw__String(g, 70, Brk_L.ToString("#0"), Color.Yellow, drawRect, StringAlignment.Far);

                    drawRect = new RectangleF(120f, 621f - top, 400.0f, 100.0F);
                    H2Y.Draw__String(g, 70, Brk_R.ToString("#0"), Color.Yellow, drawRect, StringAlignment.Far);


                    drawRect = new RectangleF(520f, 291f - top, 100.0f, 100.0F);
                    H2Y.Draw__String(g, 30, "kg", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(520f, 661f - top, 100.0f, 100.0F);
                    H2Y.Draw__String(g, 30, "kg", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(50f, 331f - top, 20.0f, 25.0F);
                    H2Y.Draw__String(g, 20, "0", Color.White, drawRect, StringAlignment.Center);

                    drawRect = new RectangleF(50f, 611f - top, 20.0f, 25.0F);
                    H2Y.Draw__String(g, 20, "0", Color.White, drawRect, StringAlignment.Center);

                    drawRect = new RectangleF(620f, 331f - top, 100.0f, 25.0F);
                    H2Y.Draw__String(g, 20, PSet.BRK_Capa.ToString(), Color.White, drawRect, StringAlignment.Center);

                    drawRect = new RectangleF(620f, 611f - top, 100.0f, 25.0F);
                    H2Y.Draw__String(g, 20, PSet.BRK_Capa.ToString(), Color.White, drawRect, StringAlignment.Center);


                    drawRect = new RectangleF(480f, 461f - top, 200.0f, 25.0F);
                    H2Y.Draw__String(g, 12, "Sum 100%", Color.Black, drawRect, StringAlignment.Far);

                    drawRect = new RectangleF(480f, 490f - top, 200.0f, 25.0F);
                    H2Y.Draw__String(g, 12, "Diff. 100%", Color.Black, drawRect, StringAlignment.Far);

                    H2Y.DrawFillRect(g, blackBrush, 740, 262 - top, 241, 136);    //합(%)
                    H2Y.DrawFillRect(g, blackBrush, 740, 412 - top, 241, 136);    //편차(%)
                    H2Y.DrawFillRect(g, blackBrush, 740, 561 - top, 241, 136);    //축중(kg)

                    drawRect = new RectangleF(740, 262 - top, 190, 50);
                    H2Y.Draw__String(g, 32, "Sum", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(920, 332 - top, 100, 50);
                    H2Y.Draw__String(g, 30, "%", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(740, 312 - top, 190, 80);
                    H2Y.Draw__String(g, 50, Sum.ToString("#0.0"), (Sum_OX == H2Y.OK ? Color.Yellow : Color.Red), drawRect, StringAlignment.Far);

                    drawRect = new RectangleF(740, 412 - top, 190, 40);
                    H2Y.Draw__String(g, 32, "Diff.", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(920, 482 - top, 100, 50);
                    H2Y.Draw__String(g, 30, "%", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(740, 462 - top, 190, 80);
                    H2Y.Draw__String(g, 50, Diff.ToString("#0.0"), (Diff_OX == H2Y.OK ? Color.Yellow : Color.Red), drawRect, StringAlignment.Far);

                    drawRect = new RectangleF(740, 462 - top, 50, 80);
                    if (Brk_L > Brk_R)
                    {
                        H2Y.Draw__String(g, 50, "R", Color.Cyan, drawRect, StringAlignment.Far);
                    }
                    else
                    {
                        H2Y.Draw__String(g, 50, "L", Color.Cyan, drawRect, StringAlignment.Far);
                    }

                    drawRect = new RectangleF(740, 561 - top, 190, 50);
                    H2Y.Draw__String(g, 32, "Weight", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(920, 561 - top, 100, 50);
                    H2Y.Draw__String(g, 30, "kg", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(740, 612 - top, 230, 80);
                    H2Y.Draw__String(g, 50, weight.ToString(), Color.Yellow, drawRect, StringAlignment.Far);
                    
                    #endregion
                    g.Dispose();
                }

                pic.Image = bmp;
            }
            catch (Exception ex)
            {
                Logs.ExceptionErr(ex);
                return;
            }
        }

        private void pic_Head_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
