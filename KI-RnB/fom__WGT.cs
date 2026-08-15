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
    public partial class fom__WGT : Form
    {
        public bool IsOpen;
        public Bitmap Screen;
        
        private fom_Main main;
        
        private const int Divider = 5;
        private const float CheckGap = 0.2f;

        private float WGT__Val; //축중 측정 값
        private float std_Min;  //축중 규정 최소
        private float std_Max;  //축중 규정 최대

        private string old_std; //화면 표시 제거
        private float old_Wgt;  //화면 표시 제거
        private float old_Value;//화면 표시 제거
        
        public fom__WGT()
        {
            InitializeComponent();
        }
        public fom__WGT(fom_Main main)
            : this()
        {
            this.main = main;
        }
        private void fom__WGT_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsOpen = false;
        }
        private void fom__WGT_Load(object sender, EventArgs e)
        {
            IsOpen = true;

            this.Top = PSet.siz__Sub.Top;
            this.Left = PSet.siz__Sub.Left;
            this.BackgroundImage = Properties.Resources.Weight;

            pic_Head.Size = new System.Drawing.Size(1024, 90);  pic_Head.Dock = DockStyle.Top;
            pic_Msgs.Size = new System.Drawing.Size(1024, 110); pic_Msgs.Location = new Point(0, 88);
            pic_WGTs.Size = new System.Drawing.Size(1024, 564); pic_WGTs.Dock = DockStyle.Bottom;
            pic_Msgs.BringToFront();

            picL_Wgt.Size = new System.Drawing.Size(123, 369);
            picL_Wgt.Location = new Point(166, 337);
            picR_Wgt.Size = new System.Drawing.Size(123, 369);
            picR_Wgt.Location = new Point(736, 337);
            
            WGT_Scr_Data();
            
            H2Y.Screen__Head(pic_Head, "Axle", "Vehicle No", "Model");
            H2Y.Message_Show(pic_Msgs, "Message");
            WGT_Scr_Show(pic_WGTs, "Standard", -1, -1);
        }

        public float WGTs_Running(int pAxle)
        {
            double ReadTime = 0, Old_Time = 0;
            double OfstTime = DateTime.Now.Ticks;
            double Gap_Ofst = 0;
            float Wgts_Sum = 0;
            float Wgts_old = 0;
            int TestStep = 0;
            bool TestFlag = false;
            bool Key_Pass = false;
            bool old_Pass = false;
            bool WeightOK = false;
            string Range = "";

            this.Visible = true;

            switch (pAxle)
            {
                case 0: H2Y.Screen__Head(pic_Head, "Front", TSet.Vin___No, TSet.CarModel);
                    std_Min = PSet.BRK.Wgt_1Min;
                    std_Max = PSet.BRK.Wgt_1Max;
                    break;

                case 1: H2Y.Screen__Head(pic_Head, "Rear", TSet.Vin___No, TSet.CarModel); 
                    std_Min = PSet.BRK.Wgt_2Min;
                    std_Max = PSet.BRK.Wgt_2Max;
                    break;
            }

            if (std_Min == 0 && std_Max == 0)
            {
                Range = "Weight";
            }
            else
            {
                Range = std_Min + "~" + std_Max;
            }

            H2Y.Message_Show(pic_Msgs, "Please enter");
            WGT_Scr_Show(pic_WGTs, "", -1, -1);

            TestStep = 0; 
            while (true)
            {
                Thread.Sleep(100);
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
                Wgts_Sum = TSet.Read_L_W + TSet.Read_R_W;

                if (std_Min == 0 && std_Max == 0)
                {
                    if (Wgts_Sum > PSet.WGTLimit)
                    {
                        WeightOK = true;
                    }
                    else
                    {
                        WeightOK = false;
                    }
                }
                else
                {
                    if (std_Min <= Wgts_Sum && Wgts_Sum <= std_Max)
                    {
                        WeightOK = true;
                    }
                    else
                    {
                        WeightOK = false;
                    }
                }


                if (TestStep == 0 && !TestFlag)
                {
                    switch (pAxle)
                    {
                        case 0: H2Y.Msg_Speash("Please enter"); 
                                H2Y.Message_Show(pic_Msgs, "Please enter"); break;

                        case 1: H2Y.Msg_Speash("Please enter"); 
                                H2Y.Message_Show(pic_Msgs, "Please enter"); break;
                    }

                    WGT_Scr_Show(pic_WGTs, Range, TSet.Read_L_W, TSet.Read_R_W);
                    WGT_Scr_Data();

                        TSet.StepNext = true; 
                    if (TSet.StepNext) { TestFlag = true; TestStep = 1; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 1 && !TestFlag)
                {
                    WGT_Scr_Show(pic_WGTs, Range, TSet.Read_L_W, TSet.Read_R_W);
                    WGT_Scr_Data();

                    if (TSet.PHO_Brake) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 2; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 2 && !TestFlag)
                {
                    H2Y.Message_Show(pic_Msgs, "Check the location");
                    WGT_Scr_Show(pic_WGTs, Range, TSet.Read_L_W, TSet.Read_R_W);
                    WGT_Scr_Data();

                    if (TSet.PHO_Brake)
                    {
                        if (WeightOK)
                        {
                            if (ReadTime - Gap_Ofst >= 3) { TSet.StepNext = true; }
                        }
                        else
                        {
                            Gap_Ofst = ReadTime;
                        }
                        
                        if (TSet.StepNext) { TestFlag = true; TestStep = 3; Gap_Ofst = ReadTime; }
                    }
                    else
                    {
                        TestFlag = true; TestStep = 0; Gap_Ofst = ReadTime; 
                    }
                }

                if (TestStep == 3 && !TestFlag)
                {
                    WGT_Scr_Data();

                        TSet.StepNext = true; 
                    if (TSet.StepNext) { TestFlag = true; TestStep = 4; Gap_Ofst = ReadTime; }
                    if (!TSet.PHO_Brake) { TestFlag = true; TestStep = 2; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 4 && !TestFlag)
                {
                    H2Y.Message_Show(pic_Msgs, "Check the location");
                    WGT_Scr_Show(pic_WGTs, Range, TSet.Read_L_W, TSet.Read_R_W);
                    WGT_Scr_Data();

                    if (TSet.PHO_Brake && WeightOK) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 5; Gap_Ofst = ReadTime; }
                    if (!TSet.PHO_Brake) { TestFlag = true; TestStep = 2; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 5 && !TestFlag)
                {
                    H2Y.Message_Show(pic_Msgs, "Stabilizing..", Color.Yellow);
                    WGT_Scr_Show(pic_WGTs, Range, TSet.Read_L_W, TSet.Read_R_W);
                    WGT_Scr_Data();

                    if (TSet.PHO_Brake && WeightOK)
                    {
                        if (Math.Abs(Wgts_Sum - Wgts_old) < PSet.WGT_Safe)
                        {
                            if (Math.Abs(ReadTime - Gap_Ofst) >= 2) { TSet.StepNext = true; }
                        }
                        else
                        {
                            Gap_Ofst = ReadTime;
                        }
                    }
                    else
                    {
                        TestFlag = true; TestStep = 0; Gap_Ofst = ReadTime;
                    }

                    if (TSet.StepNext) { TestFlag = true; TestStep = 6; Gap_Ofst = ReadTime; }
                    if (!TSet.PHO_Brake) { TestFlag = true; TestStep = 2; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 6 && !TestFlag)
                {
                    H2Y.Message_Show(pic_Msgs, "Stabilizing. " + (PSet.BRK.Wgt_Time - (ReadTime - Gap_Ofst)).ToString("#0"), Color.Lime);
                    WGT_Scr_Show(pic_WGTs, Range, TSet.Read_L_W, TSet.Read_R_W);
                    WGT_Scr_Data();

                    if (TSet.PHO_Brake && WeightOK)
                    {
                        if (Math.Abs(Wgts_Sum - Wgts_old) < PSet.WGT_Safe)
                        {
                            if (Math.Abs(ReadTime - Gap_Ofst) > PSet.BRK.Wgt_Time) 
                            {
                                switch (pAxle)
                                {
                                    case 0: WGT__Val = Wgts_Sum;
                                        Axle_1.Weight = Math.Round(Wgts_Sum, 0);
                                        Axle_1.Wgt__L = Math.Round(TSet.Read_L_W, 0);
                                        Axle_1.Wgt__R = Math.Round(TSet.Read_R_W, 0);
                                        break;
                                    case 1: WGT__Val = Wgts_Sum; 
                                        Axle_2.Weight = Math.Round(Wgts_Sum, 0);
                                        Axle_2.Wgt__L = Math.Round(TSet.Read_L_W, 0);
                                        Axle_2.Wgt__R = Math.Round(TSet.Read_R_W, 0);
                                        break;
                                }

                                TSet.StepNext = true; 
                            }
                        }
                        else
                        {
                            TestFlag = true; TestStep = 4; Gap_Ofst = ReadTime;
                        }
                    }
                    else
                    {
                        TestFlag = true; TestStep = 0; Gap_Ofst = ReadTime;
                    }

                    if (TSet.StepNext) { TestFlag = true; TestStep = 7; Gap_Ofst = ReadTime; }
                    if (!TSet.PHO_Brake) { TestFlag = true; TestStep = 2; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 7 && !TestFlag)
                {
                    H2Y.Message_Show(pic_Msgs, "Completed");
                    WGT_Scr_Show(pic_WGTs, Range, TSet.Read_L_W, TSet.Read_R_W);
                    WGT_Scr_Data();

                    if (Math.Abs(ReadTime - Gap_Ofst) >= 2) { TSet.StepNext = true; }
                    if (TSet.StepNext) { TestFlag = true; TestStep = 8; Gap_Ofst = ReadTime; }
                }

                if (TestStep == 8 && !TestFlag) 
                {
                    break;
                }

                if (Math.Abs(ReadTime - Old_Time) > CheckGap)
                {
                    Old_Time = ReadTime;
                }

                Wgts_old = Wgts_Sum;   //축중
                
                System.Windows.Forms.Application.DoEvents();
            }

            this.Visible = false;

            return WGT__Val;
        }

        private void WGT_Scr_Data()
        {
            Level___Show(picL_Wgt, (TSet.Read_L_W < 10 ? 0 : TSet.Read_L_W));
            Level___Show(picR_Wgt, (TSet.Read_R_W < 10 ? 0 : TSet.Read_R_W));
            picL_Wgt.Visible = true;
            picR_Wgt.Visible = true;
        }

        //이미지 처리
        //private void WGT_Scr_Show(PictureBox pic, string std, float Axle_Wgt)
        //{
        //    if ((old_std == std) && (old_Wgt == Axle_Wgt)) { return; }
        //         old_std = std;      old_Wgt = Axle_Wgt;

        //    try
        //    {
        //        Bitmap bmp = new Bitmap(Properties.Resources.Speed);

        //        using (Graphics g = Graphics.FromImage(bmp))
        //        {
        //            float top = 204;
        //            float weight_M = (TSet.Read_L_W + TSet.Read_R_W < 10 ? 0 : TSet.Read_L_W + TSet.Read_R_W);
                    
        //            RectangleF drawRect = new RectangleF(10, 220 - top, 1004, 73);
        //            H2Y.Draw__String(g, 40, std, Color.White, drawRect, StringAlignment.Center);

        //            drawRect = new RectangleF(10, 380 - top, 1004, 180);
        //            if (std_Min == 0 && std_Max == 0)
        //            {
        //                H2Y.Draw__String(g, 110, Axle_Wgt.ToString("#0"), Color.Yellow, drawRect, StringAlignment.Center);
        //            }
        //            else
        //            {
        //                if (std_Min <= Axle_Wgt && Axle_Wgt <= std_Max)
        //                {
        //                    H2Y.Draw__String(g, 110, Axle_Wgt.ToString("#0"), Color.Yellow, drawRect, StringAlignment.Center);
        //                }
        //                else
        //                {
        //                    H2Y.Draw__String(g, 110, Axle_Wgt.ToString("#0"), Color.Yellow, drawRect, StringAlignment.Center);
        //                }
        //            }

        //            drawRect = new RectangleF(20, 560 - top, 1004, 150);
        //            H2Y.Draw__String(g, 80, "kg", Color.White, drawRect, StringAlignment.Center);

        //            float x = 0;
        //            float y = 0;
        //            float width = 0;
        //            float height = 0;

        //            for (int cnt = 0; cnt <= Divider; cnt++)    //좌측 중량계
        //            {
        //                x = 10;
        //                y = (320 + 369) - (369 / Divider * (cnt));
        //                width = 150.0F;
        //                height = 25.0F;

        //                drawRect = new RectangleF(x, y - top, width, height);
        //                H2Y.Draw__String(g, 20, (cnt * (PSet.WGT_Capa / Divider)).ToString("#0"), Color.White, drawRect, StringAlignment.Far);
        //            }

        //            for (int cnt = 0; cnt <= Divider; cnt++)    //우측 중량계
        //            {
        //                x = 869;
        //                y = (320 + 369) - (369 / Divider * (cnt));
        //                width = 150.0F;
        //                height = 25.0F;

        //                drawRect = new RectangleF(x, y - top, width, height);
        //                H2Y.Draw__String(g, 20, (cnt * (PSet.WGT_Capa / Divider)).ToString("#0"), Color.White, drawRect, StringAlignment.Near);
        //            }
        //            g.Dispose();
        //        }

        //        pic.Image = bmp;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logs.ExceptionErr(ex);
        //        return;
        //    }
        //}
        private void WGT_Scr_Show(PictureBox pic, string std, float L_Wgt, float R_Wgt)
        {
            if (!TSet.PHO_Brake)
            {
                L_Wgt = 0;
                R_Wgt = 0;
            }

            float Axle_Wgt = L_Wgt + R_Wgt;

            if ((old_std == std) && (old_Wgt == Axle_Wgt)) { return; }
            old_std = std; old_Wgt = Axle_Wgt;

            try
            {
                Bitmap bmp = new Bitmap(Properties.Resources.Speed);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    float top = 210;
                    float weight_M = (TSet.Read_L_W + TSet.Read_R_W < 10 ? 0 : TSet.Read_L_W + TSet.Read_R_W);

                    RectangleF drawRect = new RectangleF(10, 220 - top, 1004, 73);

                    if (TSet.PHO_Brake)
                    {
                        H2Y.Draw__String(g, 40, std, Color.Green, drawRect, StringAlignment.Center);
                    }
                    else
                    {
                        H2Y.Draw__String(g, 40, std, Color.White, drawRect, StringAlignment.Center);
                    }

                    drawRect = new RectangleF(10, 380 - top, 1004, 180);
                    if (std_Min == 0 && std_Max == 0)
                    {
                        H2Y.Draw__String(g, 110, Axle_Wgt.ToString("#0"), Color.Yellow, drawRect, StringAlignment.Center);
                    }
                    else
                    {
                        if (std_Min <= Axle_Wgt && Axle_Wgt <= std_Max)
                        {
                            H2Y.Draw__String(g, 110, Axle_Wgt.ToString("#0"), Color.Yellow, drawRect, StringAlignment.Center);
                        }
                        else
                        {
                            H2Y.Draw__String(g, 110, Axle_Wgt.ToString("#0"), Color.Yellow, drawRect, StringAlignment.Center);
                        }
                    }

                    drawRect = new RectangleF(20, 560 - top, 1004, 150);
                    H2Y.Draw__String(g, 80, "kg", Color.White, drawRect, StringAlignment.Center);

                    float x = 0;
                    float y = 0;
                    float width = 0;
                    float height = 0;

                    drawRect = new RectangleF(150, 50, 150, 73);
                    H2Y.Draw__String(g, 40, L_Wgt.ToString("#0"), Color.White, drawRect, StringAlignment.Center);

                    for (int cnt = 0; cnt <= Divider; cnt++)    //좌측 중량계
                    {
                        x = 10;
                        y = (320 + 369) - (369 / Divider * (cnt));
                        width = 150.0F;
                        height = 25.0F;

                        drawRect = new RectangleF(x, y - top, width, height);
                        H2Y.Draw__String(g, 20, (cnt * (PSet.WGT_Capa / Divider)).ToString("#0"), Color.White, drawRect, StringAlignment.Far);
                    }

                    drawRect = new RectangleF(720, 50, 150, 73);
                    H2Y.Draw__String(g, 40, R_Wgt.ToString("#0"), Color.White, drawRect, StringAlignment.Center);

                    for (int cnt = 0; cnt <= Divider; cnt++)    //우측 중량계
                    {
                        x = 869;
                        y = (320 + 369) - (369 / Divider * (cnt));
                        width = 150.0F;
                        height = 25.0F;

                        drawRect = new RectangleF(x, y - top, width, height);
                        H2Y.Draw__String(g, 20, (cnt * (PSet.WGT_Capa / Divider)).ToString("#0"), Color.White, drawRect, StringAlignment.Near);
                    }
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

        //이미지 처리
        private void Level___Show(PictureBox pBox, float pValue)
        {
            if (old_Value == pValue) { return; }
                old_Value = pValue; 
            
            try
            {
                Bitmap bmp = new Bitmap(Properties.Resources.Level);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    float bmp_w = bmp.Width;
                    float bmp_h = bmp.Height;
                    float point = 0;
                    float value = bmp_h - (pValue / PSet.WGT_Capa * bmp_h);

                    SolidBrush GreenBrush = new SolidBrush(Color.Lime);

                    g.FillRectangle(GreenBrush, 10f, value, bmp_w - 20f, bmp.Height);

                    Pen GreenPen = new Pen(Color.Lime);
                    Pen BluePen = new Pen(Color.Blue);

                    BluePen.Width = 2;

                    for (int cnt = 1; cnt < Divider; cnt++)
                    {
                        point = (bmp_h / Divider) * cnt;
                        g.DrawLine(BluePen, 0f, point, bmp_w, point);
                    }
                    g.Dispose();
                }

                pBox.Image = bmp;
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
