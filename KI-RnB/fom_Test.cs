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
    public partial class fom_Test : Form
    {
        public bool IsOpen;

        clsCurve crv_Test;
        Bitmap bmp;
        Bitmap memoryImage;

        const long BreakMax = 200;
        const long SpeedMax = 100;

        public int Graph_MD;
        private System.Windows.Forms.Timer _CycleTimer;
        private double _currentCycleTime = 0.0;

        private void InitializeTimer()
        {
            _CycleTimer = new System.Windows.Forms.Timer();
            _CycleTimer.Tick += new EventHandler(CycleTImer_Tick);
            _CycleTimer.Interval = 100;
        }
        public void StartCycleTime()
        {
            _currentCycleTime = 0.0;
            _CycleTimer.Start();
        }
        public void StopCycleTIme()
        {
            _CycleTimer.Stop();
        }
        private void CycleTImer_Tick(object sender, EventArgs e)
        {
            double increment = _CycleTimer.Interval / 1000.0;
            _currentCycleTime += increment;
            lbl_Time.Text = _currentCycleTime.ToString("F1");
          
        }

        //1.  차량 선택 & 휠베이스 이동
        //2.  차량 진입및 RFID Read
        //3.  STOP POST, 안전롤러 상승
        //4.  차량 감지
        //5.  Roll Ready(상태: 전,후륜 안전롤러 상승, 리프트 하강, 배기플랩 상승)
        //6.  바코드 스캔 및 진단 커넥터 연결
        //7.  작업 시작 버튼 감지
        //8.  ECU Init
        //9.  DTC Code read -> Clear
        //10. Input/Output Test
        //11. 4H Mode Test(4WD Option)
        //12. Wheel Speed Test
        //13. Driving Program Mode Test
        //14. 40Km Speedometer Test
        //15. ATM Up-Shift Test (1->2->3->4->5)
        //16. Drivability Test (쏠림, 소음 및 엑슬 이종검사)
        //17. Cruise Dynamic Test (Option)
        //18. Arm Speed Test
        //19. Base Drag Test
        //20. Base Brake Force & Balance Test
        //21. ABS Dynamic Test (Reduction, Recovery)
        //22. 4L Mode Test (4WD Option)
        //23. Reverse Test (5km)
        //24. Air Conditioner Test
        //25. Parking Brake Test
        //26. DTC confirm Test
        //27. Test Fail? -> Retest Go to 8 Step
        //28. Test Results Print Out
        //29. DLC Disconnection, Vehicle Out
        //30. All Unit Return(에러 항목만 재검사?)


        public bool Pedal_Onf
        {
            get { return pic_PTSs.Visible; }
            set { pic_PTSs.Visible = value; pic_PTSs.BringToFront(); }
        }

        public int Data_show
        {
            get { return Data_show; }
            set 
            {
                lst_Test.Visible = false;
                lbl_OkNg.Visible = false;
                gbx__WSS.Visible = false;
                gbxBrake.Visible = false;

                switch (value)
                {
                    case 0: lst_Test.Visible = true; break;  //Error list
                    case 1: lbl_OkNg.Visible = true; break;  //Judge show
                    case 2: gbx__WSS.Visible = true; break;  //WSS   data
                    case 3: gbxBrake.Visible = true; break;  //Brake show
                }
            }
        }
        public int ABS_Type
        {
            set
            {
                switch (value)
                {
                    case 0: btnVinNo.Left = 249; btnVinNo.Width = 528;
                        btn_ABSv.Text = "---";
                        btn_ABSv.BackColor = Color.White; btn_ABSv.ForeColor = Color.Black; btn_ABSv.Visible = false;
                        break;

                    case 1: btnVinNo.Left = 356; btnVinNo.Width = 421;
                        btn_ABSv.Text = "ABS";
                        btn_ABSv.BackColor = Color.Red; btn_ABSv.ForeColor = Color.Yellow; btn_ABSv.Visible = true;
                        break;

                    case 2: btnVinNo.Left = 356; btnVinNo.Width = 421;
                        btn_ABSv.Text = "ESC";
                        btn_ABSv.BackColor = Color.Lime; btn_ABSv.ForeColor = Color.Black; btn_ABSv.Visible = true;
                        break;
                }
            }
        }
        
        public fom_Test()
        {
            InitializeComponent();

            if (PSet.OwnerS00 > PSet.Def_Lang)
            {
            }
        }

        public fom_Test(clsCurve crv_Data, int grp)
            : this()
        {
            crv_Test = crv_Data;
            Graph_MD = grp;
            ABS_Type = 0;
        }

        private void fom_Test_Load(object sender, EventArgs e)
        {
            this.Top = PSet.siz__Sub.Top;
            this.Left = PSet.siz__Sub.Left;
            this.Width = 1024;
            this.Height = 768;

            picSpeed.Location = new Point(262, 248); picSpeed.Size = new System.Drawing.Size(498, 46);
            pic_PTSs.Location = new Point(262, 248); pic_PTSs.Size = new System.Drawing.Size(498, 46);

            gbxBrake.Location = new Point(13, 247);  gbxBrake.Size = new System.Drawing.Size(244, 148);
            gbx__WSS.Location = new Point(13, 247);  gbx__WSS.Size = new System.Drawing.Size(244, 148);
            lst_Test.Location = new Point(13, 247);  lst_Test.Size = new System.Drawing.Size(244, 148);
            lbl_OkNg.Location = new Point(13, 247);  lbl_OkNg.Size = new System.Drawing.Size(244, 148);

            btnWBase.Location = new Point(1, 66);    btnWBase.Size = new System.Drawing.Size(1022, 692);
            
            Init_AllData();
            InitializeTimer();
            IsOpen = true;
        }
        private void fom_Test_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsOpen = false;
        }

        public void Init_AllData()
        {
            lbl_Mode.Text = "";
            lbl_Time.Text = "";
            lbl_Msgs.Text = "";
            lblOrder.Text = "";
            lst_Test.Items.Clear();

            lbl1Type.Text = "--";
            lbl2Type.Text = "--";
            lbl3Type.Text = "--";
            lbl4Type.Text = "--";
            lbl5Type.Text = "--";

            switch (Graph_MD)
            {
                case 0: picGraph.Location = new Point(262, 399); 
                        picGraph.Size = new Size(750, 360);         break;

                case 1: picGraph.Location = new Point(12, 399);
                        picGraph.Size = new Size(1000, 360);        break;
            }

            //picGraph.Image = Init_Graph();
            picGraph.Image = Data_Graph();

            Refresh_Errs(0, "");
            Refresh_Data(0);
        }

        public void Refresh_Data(int pStep)
        {
            if (InvokeRequired) { BeginInvoke(new Action<int>(Refresh_Data), pStep); return; }

            lblStart.BackColor = PLC.DI.PSW_Start ? Color.Lime : Color.Transparent;
            lbl_Stop.BackColor = PLC.DI.PSW__Stop ? Color.Red : Color.Transparent;
            lblCheck.BackColor = PLC.DI.PSW_Check ? Color.Yellow : Color.Transparent;

            lblStart.ForeColor = PLC.DI.PSW_Start ? Color.Black : Color.White;
            lbl_Stop.ForeColor = PLC.DI.PSW__Stop ? Color.Black : Color.White;
            lblCheck.ForeColor = PLC.DI.PSW_Check ? Color.Black : Color.White;

            lblPedal.Text = NI.Loss.PTS.ToString("0");

            lbl_FL.Text = NI.Loss.FL.Kgf.ToString("0");
            lbl_FR.Text = NI.Loss.FR.Kgf.ToString("0");
            lbl_RL.Text = NI.Loss.RL.Kgf.ToString("0");
            lbl_RR.Text = NI.Loss.RR.Kgf.ToString("0");


            if (ECUs.WSS_FL != -1)
            {
                lblWSSFL.Text = ECUs.WSS_FL.ToString("F1");
                lblWSSFR.Text = ECUs.WSS_FR.ToString("F1");
                lblWSSRL.Text = ECUs.WSS_RL.ToString("F1");
                lblWSSRR.Text = ECUs.WSS_RR.ToString("F1");
            }
            
            pic_FL.Image = Show5_Values(pStep, "FL", pic_FL);
            pic_FR.Image = Show5_Values(pStep, "FR", pic_FR);
            pic_RL.Image = Show5_Values(pStep, "RL", pic_RL);
            pic_RR.Image = Show5_Values(pStep, "RR", pic_RR);
            picRPM.Image = Show5_Values(pStep, "RPM", picRPM);
            picSpeed.Image = Show1_Values(pStep, picSpeed);
            pic_PTSs.Image = Show1_Pedals(pStep, pic_PTSs);

            picFront.Image = Show_Balance(pStep, "Front", picFront);
            pic_Rear.Image = Show_Balance(pStep, "Rear", pic_Rear);
            pic_F_R.Image = Show1Balance(pStep, "F/R", pic_F_R);

            if (crv_Test.G_Data[pStep].Segment != "")
            {
                //Refresh_Msgs(crv_Test.G_Data[pStep].Description);
                Refresh_Mode(crv_Test.G_Data[pStep].Speed.ToString() + " km/h");
            }
            
            Refresh_Graph(pStep);
        }

        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = new System.Drawing.Size(1024, 768);
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            memoryImage.Save("");

            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
        }

        public void Refresh_Errs(byte pMode, string pMsgs)
        {
            if (InvokeRequired) { BeginInvoke(new Action<byte, string>(Refresh_Errs), pMode, pMsgs); return; }

            if (pMode == 1 && pMsgs == "") return;

            switch (pMode)
            {
                case 0: lst_Test.Items.Clear();             break;
                case 1: lst_Test.Items.Insert(0, pMsgs);    break;
            }
        }

        public void Refresh_Font(Single pSize)
        {
            this.lbl_Msgs.Font = new System.Drawing.Font("굴림", pSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
        }
        public void Refresh_Msgs(string pMsgs, Color pColor)
        {
            if (InvokeRequired) { BeginInvoke(new Action<string, Color>(Refresh_Msgs), pMsgs, pColor); return; }

            if (pMsgs == lbl_Msgs.Text && lbl_Msgs.ForeColor == pColor) return;
            if (pMsgs == "") return;

            if (pMsgs.Length > 50)
            {
                Refresh_Font(30f);
            }
            else if (pMsgs.Length > 25)
            {
                Refresh_Font(40f);
            }
            else if (pMsgs.Length > 18)
            {
                Refresh_Font(50f);
            }
            else
            {
                Refresh_Font(65f);
            }
            
            lbl_Msgs.ForeColor = pColor;
            lbl_Msgs.Text = pMsgs;
        }
        public void Judge___Msgs(string pPan)
        {
            if (InvokeRequired) { BeginInvoke(new Action<string>(Judge___Msgs), pPan); return; }

            if (pPan == lbl_OkNg.Text) return;
            if (pPan == "") return;

            lbl_OkNg.ForeColor = pPan == H2Y.OK ? Color.Lime : Color.Red;
            lbl_OkNg.Text = pPan;
        }
        public void RefreshOrder(string pOrder)
        {
            if (InvokeRequired) { BeginInvoke(new Action<string>(RefreshOrder), pOrder); return; }

            if (lblOrder.Text == pOrder) return;

            lblOrder.Text = pOrder;
            lblOrder.ForeColor = Color.LightYellow;
            System.Windows.Forms.Application.DoEvents();
        }
        public void Refresh_Mode(string pMode)
        {
            if (InvokeRequired) { BeginInvoke(new Action<string>(Refresh_Mode), pMode); return; }

            if (lbl_Mode.Text == pMode) return;

            lbl_Mode.Text = pMode;
        }
        public void Refresh_Time(string pTime)
        {
            if (lbl_Time.InvokeRequired)
            {
                Action<string> safeUpdate = new Action<string>(Refresh_Time);
                lbl_Time.BeginInvoke(safeUpdate, new object[] { pTime });

                return; 
            }
            if (lbl_Time.Text == pTime) return;

            lbl_Time.Text = pTime;
        }
        
        public void Refresh_Info()
        {
            if (InvokeRequired) { BeginInvoke(new Action(Refresh_Info)); return; }

            btnVinNo.Text = TSet.Vin___No;
            btnCount.Text = PSet.T__CNT.ToString() + "/" + PSet.CalCyc.ToString();
            btnModel.Text = TSet.CarModel;

            lbl1Type.Text = TSet.CarEngin;
            lbl2Type.Text = TSet.CarTranM;
            lbl3Type.Text = TSet.Car_ABST;
            lbl4Type.Text = TSet.CarCurve;
            lbl5Type.Text = TSet.CarWbase;

            if (PSet.T__CNT < PSet.CalCyc)
            {
                btnCount.BackColor = Color.WhiteSmoke;
                btnCount.ForeColor = Color.Black;
            }
            else
            {
                btnCount.BackColor = Color.Red;
                btnCount.ForeColor = Color.Lime;
            }
        }

        public void Refresh_Graph(int Idx)
        {
            picGraph.Image = Test_Graph(Idx);
        }

        #region Graph Data
        private Bitmap Init_Graph()
        {
            bmp = new Bitmap(picGraph.Width, picGraph.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float gap_l = 40;
                float gap_r = 20;
                float gap_t = 30;
                float gap_b = 40;
                float bmp_w = (bmp.Width - (gap_l + gap_r));
                float bmp_h = (bmp.Height - (gap_t + gap_b));
                int w_cnt = 17;
                int h_cnt = 10;
                float gap_w = bmp_w / w_cnt;
                float gap_h = bmp_h / h_cnt;

                float x;
                float y;
                float width;
                float height;
                RectangleF drawRect;

                Pen blackPen = new Pen(Color.Gray, 1);
                Font drawFont = new Font("굴림", 8);
                SolidBrush drawBrush = new SolidBrush(Color.Black);

                //Set format of string.
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;

                //X축(Time-sec)
                for (int i = 0; i <= w_cnt; i++)
                {
                    x = gap_l + (gap_w * i) - 15.0F;
                    y = gap_t + bmp_h + 10.0F;
                    width = 30.0F;
                    height = 20.0F;
                    drawRect = new RectangleF(x, y, width, height);

                    g.DrawLine(blackPen, gap_l + (gap_w * i), gap_t, gap_l + (gap_w * i), gap_t + bmp_h);
                    g.DrawString((i * 10).ToString(), drawFont, drawBrush, drawRect, drawFormat);
                }

                drawFormat.Alignment = StringAlignment.Far;
                drawFormat.LineAlignment = StringAlignment.Center;

                //Y축(Speed-km/h)
                for (int i = 0; i <= h_cnt; i++)
                {
                    x = 0.0F;
                    y = gap_t + (gap_h * i) - 10.0F;
                    width = gap_l - 10.0F;
                    height = 20.0F;
                    drawRect = new RectangleF(x, y, width, height);

                    g.DrawLine(blackPen, gap_l, gap_t + (gap_h * i), gap_l + bmp_w, gap_t + (gap_h * i));
                    g.DrawString((180 - (i * 20)).ToString(), drawFont, drawBrush, drawRect, drawFormat);
                }

                x = gap_l + 10.0F;
                y = 10.0F;
                width = 200.0F;
                height = 20.0F;
                drawRect = new RectangleF(x, y, width, height);

                //Draw rectangle to screen
                //g.DrawRectangle(blackPen, x, y, width, height);

                drawFormat.Alignment = StringAlignment.Near;
                g.DrawString("Vehicle Speed(km/h)", drawFont, drawBrush, drawRect, drawFormat);

                x = (bmp.Width / 2) - 100.0F;
                y = gap_t + bmp_h + 20.0F;
                width = 200.0F;
                height = 20.0F;
                drawRect = new RectangleF(x, y, width, height);

                //Draw rectangle to screen
                //g.DrawRectangle(blackPen, x, y, width, height);

                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString("Time(sec)", drawFont, drawBrush, drawRect, drawFormat);
            }

            return bmp;
        }
        private Bitmap Data_Graph()
        {
            if (bmp == null) return null;

            //Init_Graph();

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float gap_l = 40;
                float gap_r = 20;
                float gap_t = 30;
                float gap_b = 40;
                float bmp_w = (bmp.Width - (gap_l + gap_r));
                float bmp_h = (bmp.Height - (gap_t + gap_b));
                int w_cnt = 17;
                int h_cnt = 10;
                float gap_w = bmp_w / w_cnt;
                float gap_h = bmp_h / h_cnt;

                float Ofst_X = gap_l;
                float Ofst_Y = (gap_t + bmp_h - (gap_h * 1));
                float x, o_x = Ofst_X;
                float y, o_y = Ofst_Y;
                float scal_X = bmp_w / 170;
                float scal_Y = bmp_h / 200;

                int CNT = crv_Test.G_Data.Count - 1;
                Pen RedPen = new Pen(Color.Red, 2);
                Pen BluePen = new Pen(Color.Blue, 3);
                RectangleF drawRect;

                for (int i = 0; i <= CNT; i++)
                {
                    x = Ofst_X + (scal_X * crv_Test.G_Data[i].T_Time);
                    y = Ofst_Y - (scal_Y * crv_Test.G_Data[i].Speed);

                    g.DrawLine(RedPen, o_x, o_y, x, y);

                    drawRect = new RectangleF(x - 1.5f, y - 1.5f, 3, 3);
                    g.DrawEllipse(BluePen, drawRect);

                    o_x = x; o_y = y;
                }
            }

            return bmp;
        }
        private Bitmap Test_Graph(int pStep)
        {
            if (bmp == null) return null;

            Init_Graph();

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float gap_l = 40;
                float gap_r = 20;
                float gap_t = 30;
                float gap_b = 40;
                float bmp_w = (bmp.Width - (gap_l + gap_r));
                float bmp_h = (bmp.Height - (gap_t + gap_b));
                int w_cnt = 17;
                int h_cnt = 10;
                float gap_w = bmp_w / w_cnt;
                float gap_h = bmp_h / h_cnt;

                float Ofst_X = gap_l;
                float Ofst_Y = (gap_t + bmp_h - (gap_h * 1));
                float x, o_x = Ofst_X;
                float y, o_y = Ofst_Y;
                float scal_X = bmp_w / 170;
                float scal_Y = bmp_h / 200;

                int CNT = crv_Test.G_Data.Count - 1;
                Pen RedPen = new Pen(Color.Red, 2);
                Pen BluePen = new Pen(Color.Blue, 3);
                RectangleF drawRect;

                if (pStep > 0)
                {
                    float x_1, x_2, x_3, x_4;
                    float y_1, y_2, y_3, y_4;
                    PointF point1, point2, point3, point4;
                    SolidBrush RedBrush = new SolidBrush(Color.Red);
                    SolidBrush blueBrush = new SolidBrush(Color.Blue);
                    SolidBrush GreenBrush = new SolidBrush(Color.Green);

                    for (int i = 1; i <= pStep; i++)
                    {
                        x_1 = Ofst_X + (scal_X * crv_Test.G_Data[i].T_Time);
                        y_1 = Ofst_Y - (scal_Y * crv_Test.G_Data[i].Speed);

                        x_2 = Ofst_X + (scal_X * crv_Test.G_Data[i - 1].T_Time);
                        y_2 = Ofst_Y - (scal_Y * crv_Test.G_Data[i - 1].Speed);

                        x_3 = Ofst_X + (scal_X * crv_Test.G_Data[i - 1].T_Time);
                        y_3 = gap_t + bmp_h;

                        x_4 = Ofst_X + (scal_X * crv_Test.G_Data[i].T_Time);
                        y_4 = gap_t + bmp_h;

                        point1 = new PointF(x_1, y_1);
                        point2 = new PointF(x_2, y_2);
                        point3 = new PointF(x_3, y_3);
                        point4 = new PointF(x_4, y_4);

                        PointF[] curvePoints1 = { point1, point2, point3, point4 };
                        g.FillPolygon(GreenBrush, curvePoints1);
                        //g.FillPolygon(RedBrush, curvePoints1);
                        
                    }

                    x_1 = Ofst_X + (scal_X * crv_Test.G_Data[pStep].T_Time);
                    y_1 = Ofst_Y - (scal_Y * crv_Test.G_Data[pStep].Speed);

                    x_2 = Ofst_X + (scal_X * crv_Test.G_Data[pStep - 1].T_Time);
                    y_2 = Ofst_Y - (scal_Y * crv_Test.G_Data[pStep - 1].Speed);

                    x_3 = Ofst_X + (scal_X * crv_Test.G_Data[pStep - 1].T_Time);
                    y_3 = gap_t + bmp_h;

                    x_4 = Ofst_X + (scal_X * crv_Test.G_Data[pStep].T_Time);
                    y_4 = gap_t + bmp_h;

                    point1 = new PointF(x_1, y_1);
                    point2 = new PointF(x_2, y_2);
                    point3 = new PointF(x_3, y_3);
                    point4 = new PointF(x_4, y_4);

                    PointF[] curvePoints = { point1, point2, point3, point4 };
                    g.FillPolygon(blueBrush, curvePoints);
                }

                for (int i = 0; i <= CNT; i++)
                {
                    x = Ofst_X + (scal_X * crv_Test.G_Data[i].T_Time);
                    y = Ofst_Y - (scal_Y * crv_Test.G_Data[i].Speed);

                    g.DrawLine(RedPen, o_x, o_y, x, y);

                    drawRect = new RectangleF(x - 1.5f, y - 1.5f, 3, 3);
                    g.DrawEllipse(BluePen, drawRect);

                    o_x = x; o_y = y;
                }

                if (pStep > -1 & crv_Test.G_Data.Count > 0)
                {
                    Pen PointPen = new Pen(Color.Green, 7);

                    x = Ofst_X + (scal_X * crv_Test.G_Data[pStep].T_Time);
                    y = Ofst_Y - (scal_Y * crv_Test.G_Data[pStep].Speed);

                    drawRect = new RectangleF(x - 3.5f, y - 3.5f, 7, 7);
                    g.DrawEllipse(PointPen, drawRect);
                }
            }

            return bmp;
        }
        #endregion

        #region General Data
        private Bitmap Show5_Values(int pStep, string pAxle, PictureBox pBox)
        {
            bmp = new Bitmap(pBox.Width, pBox.Height);

            Single brkForce = 0;
            switch (pAxle)
            {
                case "FL":  brkForce = Convert.ToSingle(TSet.Read__FL); break;
                case "FR":  brkForce = Convert.ToSingle(TSet.Read__FR); break;
                case "RL":  brkForce = Convert.ToSingle(TSet.Read__RL); break;
                case "RR":  brkForce = Convert.ToSingle(TSet.Read__RR); break;
                case "RPM": brkForce = Convert.ToSingle(TSet.Read_RPM); break;
                case "PTS": brkForce = Convert.ToSingle(TSet.Read_PTS); break;
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float bmp_w = bmp.Width;
                float bmp_h = bmp.Height;
                float value = bmp_h - (brkForce / BreakMax * bmp_h);

                Font drawFont = new Font("Arial", 20, FontStyle.Bold);

                Pen blackPen = new Pen(Color.Gray, 1);
                Pen RedPen = new Pen(Color.Red, 2);
                Pen BluePen = new Pen(Color.Blue, 3);

                SolidBrush RedBrush = new SolidBrush(Color.Red);
                SolidBrush BuleBrush = new SolidBrush(Color.Blue);
                SolidBrush GreenBrush = new SolidBrush(Color.Lime);
                SolidBrush BlackBrush = new SolidBrush(Color.Black);

                //Set format of string.
                float x = 0.0F;
                float y = 3.0F;
                float width = bmp_w;
                float height = 50.0F;

                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;

                RectangleF drawRect = new RectangleF(x, y, width, height);

                switch(pAxle)
                {
                    case "RPM": g.FillRectangle(BuleBrush, 0f, value, bmp_w, bmp.Height);   break;
                    case "PTS": g.FillRectangle(RedBrush, 0f, value, bmp_w, bmp.Height);    break;
                    default: g.FillRectangle(GreenBrush, 0f, value, bmp_w, bmp.Height);     break;
                }

                g.DrawString(brkForce.ToString("0.0"), drawFont, BlackBrush, drawRect, drawFormat);

                if (pAxle != "RPM" & pAxle != "PTS")
                {
                    drawRect = new RectangleF(x, 60, width, height);
                    g.DrawString(pAxle, drawFont, BlackBrush, drawRect, drawFormat);
                }
            }

            return bmp;
        }
        private Bitmap Show1_Values(int pStep, PictureBox pBox)
        {
            bmp = new Bitmap(pBox.Width, pBox.Height);

            Single brkSpeed = Convert.ToSingle(TSet.SpeedStd);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float bmp_w = bmp.Width;
                float bmp_h = bmp.Height;
                float speed = brkSpeed / BreakMax * bmp_w;
                float Target = 0;

                Font drawFont = new Font("Arial", 20, FontStyle.Bold);
                Pen RedPen = new Pen(Color.Red, 2);
                Pen GreenPen = new Pen(Color.Green, 5);
                Pen BluePen = new Pen(Color.Blue, 5);
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                SolidBrush RedBrush = new SolidBrush(Color.Red);
                SolidBrush GreenBrush = new SolidBrush(Color.Lime);

                speed = brkSpeed / BreakMax * bmp_w;
                g.FillRectangle(GreenBrush, 0f, 5f, speed, bmp_h - 15);

                if (crv_Test.G_Data[pStep].Speed > 15)
                {
                    float TSpeed = crv_Test.G_Data[pStep].Speed;

                    Target = (TSpeed / BreakMax) * bmp_w;

                    if (Target < speed)
                    {
                        g.FillRectangle(RedBrush, Target, 5f, (speed - Target), bmp_h - 15);
                    }

                    g.DrawLine(BluePen, Target, 0, Target, bmp_h);
                }

                //Set format of string.
                float x = 0;
                float y = 7.0F;
                float width = 120;
                float height = bmp_h - y;

                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;
                RectangleF drawRect = new RectangleF(x, y, width, height);
                g.DrawString("SPEED", drawFont, drawBrush, drawRect, drawFormat);

                x = bmp.Width - 120;
                y = 7.0F;
                width = 120;
                height = bmp.Height - y;
                drawRect = new RectangleF(x, y, width, height);
                g.DrawString(brkSpeed.ToString("0.0"), drawFont, drawBrush, drawRect, drawFormat);
            }

            return bmp;
        }

        private Bitmap Show1_Pedals(int pStep, PictureBox pBox)
        {
            bmp = new Bitmap(pBox.Width, pBox.Height);

            try
            {
                Single brkPedal = 0;

                switch (PSet.OwnerPdl)
                {
                    case 0: brkPedal = Convert.ToSingle(NI.Loss.PTS); break;
                    case 1: brkPedal = Convert.ToSingle(NI.Loss.PTS); break;
                }
                
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    float bmp_w = bmp.Width;
                    float bmp_h = bmp.Height;
                    float Break = brkPedal / PSet.RnB.PTSGraph * bmp_w;
                    float Target = 0;

                    Font drawFont = new Font("Arial", 20, FontStyle.Bold);
                    Pen RedPen = new Pen(Color.Red, 2);
                    Pen GreenPen = new Pen(Color.Cyan, 50);
                    Pen BluePen = new Pen(Color.Aqua, 5);
                    SolidBrush drawBrush = new SolidBrush(Color.Black);
                    SolidBrush RedBrush = new SolidBrush(Color.Red);
                    SolidBrush GreenBrush = new SolidBrush(Color.Lime);

                    Break = brkPedal / PSet.RnB.PTSGraph * bmp_w;

                    g.FillRectangle(GreenBrush, 0f, 5f, Break, bmp_h - 15);

                    if (crv_Test.G_Data[pStep].Segment.ToUpper() == "BRAKE TEST")
                    {
                        float TValue = PSet.RnB.PTSValue;

                        Target = (TValue / PSet.RnB.PTSGraph) * bmp_w;

                        g.DrawLine(GreenPen, Target, 0, Target, bmp_h);
                        g.FillRectangle(GreenBrush, 0f, 5f, Break, bmp_h - 15);

                        if (Target < Break)
                        {
                            g.FillRectangle(RedBrush, Target, 5f, (Break - Target), bmp_h - 15);
                        }

                        g.DrawLine(BluePen, Target, 0, Target, bmp_h);
                    }

                    //Set format of string.
                    float x = 0;
                    float y = 7.0F;
                    float width = 120;
                    float height = bmp_h - y;

                    StringFormat drawFormat = new StringFormat();
                    drawFormat.Alignment = StringAlignment.Center;
                    RectangleF drawRect = new RectangleF(x, y, width, height);
                    g.DrawString("PTS", drawFont, drawBrush, drawRect, drawFormat);

                    x = bmp.Width - 120;
                    y = 7.0F;
                    width = 120;
                    height = bmp.Height - y;
                    drawRect = new RectangleF(x, y, width, height);
                    g.DrawString(brkPedal.ToString("0.0"), drawFont, drawBrush, drawRect, drawFormat);
                }
            }
            catch(Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Show1_Pedals: " + ex.Message);
            }

            return bmp;
        }

        private Bitmap Show_Balance(int pStep, string pAxle, PictureBox pBox)
        {
            bmp = new Bitmap(pBox.Width, pBox.Height);

            Single brkForce = 0;
            switch (pAxle)
            {
                case "Front": brkForce = Convert.ToSingle(H2Y.Dbl_Balance(TSet.Bal___FL, TSet.Bal___FR)); break;
                case "Rear":  brkForce = Convert.ToSingle(H2Y.Dbl_Balance(TSet.Bal___RL, TSet.Bal___RR)); break;
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float bmp_w = bmp.Width;
                float bmp_h = bmp.Height;
                float value = H2Y.DVD(bmp_w, 100) * brkForce;

                Font drawFont = new Font("Arial", 20, FontStyle.Bold);

                Pen GreenPen = new Pen(Color.Lime, 5);
                Pen RedPen = new Pen(Color.Red, 2);
                Pen BluePen = new Pen(Color.Blue, 3);

                SolidBrush RedBrush = new SolidBrush(Color.Red);
                SolidBrush BuleBrush = new SolidBrush(Color.Blue);
                SolidBrush GreenBrush = new SolidBrush(Color.Green);
                SolidBrush BlackBrush = new SolidBrush(Color.Black);

                //if (brkForce >= 50)
                //{
                //    g.FillRectangle(RedBrush, 0f, 0f, value, bmp_h);
                //    g.FillRectangle(BuleBrush, value, 0f, bmp_w, bmp_h);
                //}
                //else
                //{
                    g.FillRectangle(BuleBrush, 0f, 0f, value, bmp_h);
                    g.FillRectangle(RedBrush, value, 0f, bmp_w, bmp_h);
                //}
                g.DrawLine(GreenPen, bmp_w / 2, 0, bmp_w / 2, bmp_h);

                ////Set format of string.
                //float x = 0.0F;
                //float y = 0.0F;
                
                //StringFormat drawFormat = new StringFormat();
                //drawFormat.Alignment = StringAlignment.Center;
                //RectangleF drawRect = new RectangleF(x, y, bmp_w, bmp_h);

                //g.DrawString(brkForce.ToString(), drawFont, BlackBrush, drawRect, drawFormat);
                //g.DrawString(pAxle, drawFont, BlackBrush, drawRect, drawFormat);
            }

            return bmp;
        }
        private Bitmap Show1Balance(int pStep, string pAxle, PictureBox pBox)
        {
            bmp = new Bitmap(pBox.Width, pBox.Height);

            Single brkForce = 0;
            switch (pAxle)
            {
                case "Front": brkForce = Convert.ToSingle(H2Y.Dbl_Balance(TSet.Bal___FL, TSet.Bal___FR)); break;
                case "Rear":  brkForce = Convert.ToSingle(H2Y.Dbl_Balance(TSet.Bal___RL, TSet.Bal___RR)); break;
                case "F/R":   brkForce = Convert.ToSingle(H2Y.Dbl_Balance2((TSet.Bal___FL + TSet.Bal___FR), (TSet.Bal___RL + TSet.Bal___RR)));
                    break;
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float bmp_w = bmp.Width;
                float bmp_h = bmp.Height;
                float value = (H2Y.DVD(bmp_h, 100) * brkForce); //(brkForce / BreakMax * bmp_h);

                Font drawFont = new Font("Arial", 20, FontStyle.Bold);

                Pen GreenPen = new Pen(Color.Lime, 5);
                Pen RedPen = new Pen(Color.Red, 2);
                Pen BluePen = new Pen(Color.Blue, 3);

                SolidBrush RedBrush = new SolidBrush(Color.Red);
                SolidBrush BuleBrush = new SolidBrush(Color.Blue);
                SolidBrush GreenBrush = new SolidBrush(Color.Green);
                SolidBrush BlackBrush = new SolidBrush(Color.Black);

                //if (brkForce >= 50)
                //{
                //    g.FillRectangle(RedBrush, 0f, 0f, bmp_w, value);
                //    g.FillRectangle(BuleBrush, 0f, value, bmp_w, bmp_h);
                //}
                //else
                //{
                    g.FillRectangle(BuleBrush, 0f, 0f, bmp_w, value);
                    g.FillRectangle(RedBrush, 0f, value, bmp_w, bmp_h);
                //}
                g.DrawLine(GreenPen, 0, bmp_h / 2, bmp_w, bmp_h / 2);

                ////Set format of string.
                //float x = 0.0F;
                //float y = 0.0F;

                //StringFormat drawFormat = new StringFormat();
                //drawFormat.Alignment = StringAlignment.Center;
                //RectangleF drawRect = new RectangleF(x, y, bmp_w, bmp_h);

                //g.DrawString(brkForce.ToString(), drawFont, BlackBrush, drawRect, drawFormat);
                //g.DrawString(pAxle, drawFont, BlackBrush, drawRect, drawFormat);
            }

            return bmp;
        }
        #endregion
    }
}
