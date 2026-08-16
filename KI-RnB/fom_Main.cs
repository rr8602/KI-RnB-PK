using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Threading;

namespace KI_RnB
{
    public partial class fom_Main : Form
    {
        public MDB_Alls DB_All = new MDB_Alls();

        public fom_Init  Fom_Init = new fom_Init();
        public fomFlash  FomFlash = new fomFlash();
        public fomDebug  FomDebug;
        public fom_Stop  Fom_Stop;
        public fom_Loss  Fom_Loss;
        public fom_Load  Fom_Load;

        private fomNIDaq FomNIDaq = new fomNIDaq();
        private fomSpeed FomSpeed;
        private fom_Dist Fom_Dist;
        private fom1Gage Fom1Gage;
        private fom4Gage Fom4Gage;
        private fom_Keys Fom_Keys;
        private fom__SST Fom_SSTs = new fom__SST();
        private fom__WGT Fom_WGTs = new fom__WGT();
        private fomBrake Fom_BRKs = new fomBrake();
        private fomSetup FomSetup;
        private fom_PsWd Fom_PwWd; 

        public ABS_Board ABSBoard;
        public clsNidec Nidec;
        public Barcode Barcode1;
        public Indicator Pedal;
        
        cls_Test Test;
        int SelectRow;
        long Err_GapT = DateTime.Now.AddSeconds(0.5).Ticks;
        long Onf_GapT = DateTime.Now.AddSeconds(0.5).Ticks;

        string Porg_Msg = "";
        public Chery_TestPresentThread CheryEchoThread;
        public fom_Main()
        {
            InitializeComponent();

            PSet.Sel_Date = DateTime.Now;

            ABSBoard = new ABS_Board(this);
            Barcode1 = new Barcode(this);
            Pedal = new Indicator(this);

            H2Y.BitArray();
            Refresh_Sets();

            this.Text = "KIyasaka Roll & Brake Test Machine [" + Application.ProductVersion.ToString() + "]";
            #region Language
            if (PSet.OwnerS00 > PSet.Def_Lang)
            {
                tabPage1.Text = PSet.LangMain[1];   //데이터 리스트
                tabPage2.Text = PSet.LangMain[2];   //장비 상태
                tabPage3.Text = PSet.LangMain[3];   //디버그
                tpg_Info.Text = PSet.LangMain[4];   //차량 정보
                tpg_Data.Text = PSet.LangMain[5];   //데이터
                btn_Search.Text = PSet.LangMain[6]; //검색
                btn_Rept.Text = PSet.LangMain[7];   //보고서
                lbl_FL.Text = PSet.LangMain[8];     //전축 좌
                lbl_FR.Text = PSet.LangMain[9];     //전축 우
                lbl_RL.Text = PSet.LangMain[10];    //후축 좌
                lbl_RR.Text = PSet.LangMain[11];    //후축 우
                btn_Down.Text = PSet.LangMain[12];  //하강
                btn_Up.Text = PSet.LangMain[13];    //상승
                btn_Active.Text = PSet.LangMain[14];//동작중
                btn_Error.Text = PSet.LangMain[15]; //에러
                btnRFlap.Text = PSet.LangMain[16];  //교정 주기
                btn_Auto.Text = PSet.LangMain[17];  //자동 모드
                btn_Manu.Text = PSet.LangMain[18];  //수동 모드
                btn_Cali.Text = PSet.LangMain[19];  //교정 모드
                btnPHome.Text = PSet.LangMain[20];  //홈 포지션
                btnPEntr.Text = PSet.LangMain[21];  //진입 포지션
                btnPTest.Text = PSet.LangMain[22];  //측정 포지션
                btnPExit.Text = PSet.LangMain[23];  //퇴출 포지션
                lbl0Head.Text = PSet.LangMain[24];  //작업번호
                lbl1Head.Text = PSet.LangMain[25];  //차량번호
                lbl2Head.Text = PSet.LangMain[26];  //시간
                lbl3Head.Text = PSet.LangMain[27];  //모델명
                lbl4Head.Text = PSet.LangMain[28];  //엔진
                lbl5Head.Text = PSet.LangMain[29];  //트렌스미션
                lbl6Head.Text = PSet.LangMain[30];  //ABS
                lbl7Head.Text = PSet.LangMain[31];  //드라이브커브
                lbl8Head.Text = PSet.LangMain[32];  //구동축
                lbl9Head.Text = PSet.LangMain[33];  //휠베이스
                lblAHead.Text = PSet.LangMain[34];  //파라미터
                btn_Sett.Text = PSet.LangMain[35];  //설정
                btn_Test.Text = PSet.LangMain[36];  //측정
            }
            #endregion
        }

        private void fom_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program_Exit();
        }

        private void fom_Main_Load(object sender, EventArgs e)
        {
            PSet.Onf_Prog = true;
            lblOwner.BackColor = Color.Transparent;

            //Bitmap bmp = new Bitmap(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream("KI_RnB.Properties.Resources.Car_1.png"));
            //Bitmap bmp = new Bitmap(KI_RnB.Properties.Resources.Car_1);
            //bmp.MakeTransparent(Color.Magenta);
            //pic__Car.Image = bmp;
            pic__Car.BackColor = Color.Transparent;
            pic__Car.Parent = groupBox1;

            this.Location = new Point(0, 0);        this.Size = new System.Drawing.Size(1024, 768);
            tab_Main.Location = new Point(0, 86);   tab_Main.Size = new System.Drawing.Size(1018, 546); tab_Main.Visible = true; tab_Main.SendToBack();
            dgv_List.Location = new Point(6, 6);    dgv_List.Size = new System.Drawing.Size(661, 508);
            pnl_Date.Location = new Point(6, 6);    pnl_Date.Size = new System.Drawing.Size(661, 508);  pnl_Date.BringToFront(); Calendar.Dock = DockStyle.Fill;
            pic_View.Location = new Point(6, 6);    pic_View.Size = new System.Drawing.Size(661, 508);  pic_View.BringToFront(); pic_View.Visible = false;
            lst_Logs.Location = new Point(0, 635);  lst_Logs.Size = new System.Drawing.Size(676, 80);   lst_Logs.Visible = true;
            
            tab_Left.Location = new Point(676, 86); tab_Left.Size = new System.Drawing.Size(343, 546);
            pnl_Find.Location = new Point(0, 72);   pnl_Find.Size = new System.Drawing.Size(334, 112);  pnl_Find.Visible = false; pnl_Find.BringToFront();

            btn_Stop.Location = new Point(676, 635); btn_Stop.Size = new System.Drawing.Size(340, 80); btn_Stop.Visible = false;
            btn_Sett.Location = new Point(676, 635); btn_Sett.Size = new System.Drawing.Size(170, 80); btn_Sett.Visible = true;
            btn_Test.Location = new Point(846, 635); btn_Test.Size = new System.Drawing.Size(170, 80); btn_Test.Visible = true;
                        
            if (!PSet.OnfOwner)
            {
                tab_Main.Controls.RemoveByKey("tabPage3");
            }

            string now = DateTime.Now.ToString(H2Y.format0Time);
            lst_Logs.Items.Clear();
            lst_Logs.Items.Add("[" + now + "] KI-RnB " + PSet.LangMain[37]); //프로그램을 시작합니다.
            Logs.MakeLog_File(Log_His.Msg_, "KI-RnB " + PSet.LangMain[37]);
            lbl_Date.Text = PSet.Sel_Date.ToString("yyyy-MM-dd");

            cboIndex.Items.Clear();
            cboIndex.Items.Add("All");
            cboIndex.Items.Add("D 100");
            cboIndex.Items.Add("D 300");
            cboIndex.Items.Add("D 500");
            cboIndex.Items.Add("D 700");
            cboIndex.SelectedIndex = 0;

            tab_Left.Controls.RemoveByKey("tpg_Data");

            Calendar.Value = PSet.Sel_Date;
            All_DataList(PSet.Sel_Date.ToString("yyyyMMdd"));

            H2Y.Make_Dir(Application.StartupPath + @"\Data\");

            Excel_Backup();

            tmrStart.Enabled = true;

            CheryEchoThread = new Chery_TestPresentThread();
            CheryEchoThread.StartThread();

        }

        private void Excel_Backup()
        {
            try
            {
                DateTime BackDate = DateTime.Now.AddMonths(-1);
                string back_Mon = BackDate.ToString();
                back_Mon = back_Mon.Replace("-", "");
                string crv_File = Application.StartupPath + @"\Data\" + back_Mon.Substring(0, 6) + ".csv";

                if (System.IO.File.Exists(crv_File))
                {
                }
                else
                {
                    bool ret = PSet.Backup___CSV(back_Mon.Substring(0, 6));
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Excel_Backup: " + ex.Message);
            }
        }

        private void lblExcel_DoubleClick(object sender, EventArgs e)
        {
            fom_Data data = new fom_Data(this);
            data.Show();
        }
        
        private void Refresh_Sets()
        {
            PSet.Prog_SetRead();    //장비   설정 값
            PSet.Ini_SizeRead();    //화면   설정 값
            PSet.Cal_LossRead();    //Loss Factor 설정 값
            PSet.Cal_LoadRead();    //Load Factor 설정 값
            PSet.NIDAQmx_Read();    //NI PCI 6624 Board
            PSet.Read_ExcelDB();    //언어팩

            AllModelList();
            InitInfoData();
        }

        private void AllModelList()//Model List
        {
            DataTable dt = DB_All.DBModel.Search();

            cboModel.Items.Clear();
            foreach (DataRow model in dt.Rows)
            {
                cboModel.Items.Add(model["dbCarModel"].ToString());
            }
        }
        private void All_DataList(string date)//Test Data List
        {
            string Sel_Date = date.Replace("-", "");
            DataTable dt = DB_All.DB_Info.Search(Sel_Date);
            dgv_List.DataSource = dt;

            System.Windows.Forms.DataGridViewCellStyle CellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            CellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            if (dgv_List.Columns.Count == 15)
            {
                dgv_List.Columns[0].DefaultCellStyle = CellStyle;
                dgv_List.Columns[1].DefaultCellStyle = CellStyle;
                dgv_List.Columns[2].DefaultCellStyle = CellStyle;
                dgv_List.Columns[3].DefaultCellStyle = CellStyle;
                dgv_List.Columns[4].DefaultCellStyle = CellStyle;
                dgv_List.Columns[5].DefaultCellStyle = CellStyle;
                dgv_List.Columns[6].DefaultCellStyle = CellStyle;
                dgv_List.Columns[7].DefaultCellStyle = CellStyle;
                dgv_List.Columns[8].DefaultCellStyle = CellStyle;
                dgv_List.Columns[9].DefaultCellStyle = CellStyle;
                dgv_List.Columns[10].DefaultCellStyle = CellStyle;
                dgv_List.Columns[11].DefaultCellStyle = CellStyle;
                dgv_List.Columns[12].DefaultCellStyle = CellStyle;
                dgv_List.Columns[13].DefaultCellStyle = CellStyle;
                dgv_List.Columns[14].DefaultCellStyle = CellStyle;

                dgv_List.Columns[0].HeaderText = PSet.LangMain[38]; //"Work No";
                dgv_List.Columns[1].HeaderText = PSet.LangMain[39]; //"Vin";
                dgv_List.Columns[2].HeaderText = PSet.LangMain[40]; //"Model";
                dgv_List.Columns[3].HeaderText = "ECU"; //"ECU";
                dgv_List.Columns[4].HeaderText = PSet.LangMain[41]; //"Barcode";
                dgv_List.Columns[5].HeaderText = PSet.LangMain[42]; //"Wheelbase";
                dgv_List.Columns[6].HeaderText = PSet.LangMain[43]; //"Engine";
                dgv_List.Columns[7].HeaderText = PSet.LangMain[44]; //"Transmission";
                dgv_List.Columns[8].HeaderText = PSet.LangMain[45]; //"ABS Type";
                dgv_List.Columns[9].HeaderText = PSet.LangMain[46]; //"Drive curve";
                dgv_List.Columns[10].HeaderText = PSet.LangMain[47]; //"Drive axle";
                dgv_List.Columns[11].HeaderText = PSet.LangMain[48]; //"Date";
                dgv_List.Columns[12].HeaderText = PSet.LangMain[49]; //"Start Time";
                dgv_List.Columns[13].HeaderText = PSet.LangMain[50]; //"Test Time";
                dgv_List.Columns[14].HeaderText = PSet.LangMain[51]; //"End Time";
            }
            //if (dgv_List.Rows.Count > 1)
            //{
            //    for (int cnt = 0; cnt < dgv_List.Rows.Count; cnt++)
            //    {
            //        //dgv_List.Rows[cnt].HeaderCell
            //        dgv_List.Rows[cnt].HeaderCell.Value = cnt.ToString();
            //        //dgv_List[0, cnt].Value = cnt.ToString();
            //    }
            //}

            //int rowNumber = 1;
            //foreach (DataGridViewRow row in dgv_List.Rows)
            //{
            //    if (row.IsNewRow) continue;
            //    row.HeaderCell.Value = rowNumber.ToString();
            //    rowNumber++;
            //}
            dgv_List.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            //int Test_No = Info.Max_No(Sel_Date);
        }
        private void Calendar_DblClick(object sender, EventArgs e)
        {
            string sel_date = Calendar.Value.ToString();
            PSet.Sel_Date = (DateTime)Calendar.Value;
            lbl_Date.Text = PSet.Sel_Date.ToString("yyyy-MM-dd");
            All_DataList(PSet.Sel_Date.ToString("yyyyMMdd"));
            pnl_Date.Visible = false;
        }

        private void lbl_Date_DoubleClick(object sender, EventArgs e)
        {
            pnl_Date.Visible = !pnl_Date.Visible;
        }

        private void tmrStart_Tick(object sender, EventArgs e)//프로그램 시작시 설정
        {
            tmrStart.Enabled = false;

            try
            {
                PLC.Setting(this, PSet.PLC__S, PSet.PLC__P);
                if (!PLC.IsOpen) PLC.Start();

                Thread.Sleep(100);

                NI.Init();
                NI.Start();

                ABSBoard.Setting(PSet.Ctrl_S, PSet.Ctrl_P.ToString(), 0);
                Barcode1.Setting(PSet.BarC1S, PSet.BarC1P.ToString());
                Pedal.Setting(PSet.PedalS, PSet.PedalP.ToString());

                if (PSet.OwnerDrv > 0)
                {
                    Nidec = new clsNidec(this, PSet.MDrv_S, PSet.MDrv_P);
                    Nidec.Connect();
                    MDrive__Read();
                }

                NeoVI.Setting(this);
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "tmrStart_Tick: " + ex.Message);
            }

            FomFlash.Show();
            FomFlash.Play(0);
            //Fom_Init.ShowDialog(this);


            PLC.Door____Open();

            tmr_Main.Enabled = true;
        }

        private void tmr_Main_Tick(object sender, EventArgs e)
        {
            string strModel = "";

            #region ABS Board
            if (chk_ABSB.Checked)
            {
                lbl0Scan.Text = ABSBoard.DLoad[0].ToString();
                lbl1Scan.Text = ABSBoard.DLoad[1].ToString();
                lbl2Scan.Text = ABSBoard.DLoad[2].ToString();
                lbl3Scan.Text = ABSBoard.DLoad[3].ToString();
                lbl4Scan.Text = ABSBoard.DLoad[4].ToString();
                lbl5Scan.Text = ABSBoard.DLoad[5].ToString();

                lbl6Scan.Text = ABSBoard.DIGet.ToString();
                lbl7Scan.Text = ABSBoard.DOGet.ToString();

                lbl8Scan.Text = ABSBoard.G_Enc[0].ToString();
                lbl9Scan.Text = ABSBoard.G_Enc[1].ToString();
            }
            #endregion

            #region Machine State
            if (!Fom_Init.IsOpen)
            {
                lbl_ECUs.BackColor = NeoVI.IsOpen ? Color.Lime : Color.Transparent;
                lbl_BarC.BackColor = Barcode1.IsOpen ? Color.Lime : Color.Transparent;

                if ((H2Y.GetKeyState(H2Y.VK_CONTROL) & H2Y.KeyPressed) != 0 && (H2Y.GetKeyState(H2Y.VK_SHIFT) & H2Y.KeyPressed) != 0 && (H2Y.GetKeyState(H2Y.VK_RETURN) & H2Y.KeyPressed) != 0)
                {
                    PSet.OnfOwner = !PSet.OnfOwner;
                    DebugMode_On();
                }

                if (!FomFlash.IsOpen)
                {
                    FomFlash.Show();
                    FomFlash.Play(0);
                }

                if (PSet.OwnerDrv > 0 && !PSet.OnfDebug)
                {
                    if (Nidec != null)
                    {
                        if (!Nidec.IsOpen)
                        {
                            //if (H2Y.Ping_Test(PSet.MDrv_S))
                            //{
                            //    Nidec.Connect();
                            //    MDrive__Read();
                            //}
                        }
                        else
                        {
                            lblM_Drv.BackColor = Nidec.IsOpen ? Color.Lime : Color.Red;
                            if (Nidec.IsOpen)
                            {
                                lblM_Drv.ForeColor = Nidec.IsSame ? Color.Black : Color.Red;
                            }
                            else
                            {
                                lblM_Drv.ForeColor = Nidec.IsSame ? Color.Black : Color.Yellow;
                            }
                        }
                    }
                }

                if (PLC.IsRedy)
                {
                    MachineState();

                    if ((Err_GapT - DateTime.Now.Ticks) < 0)
                    {
                        Err_GapT = DateTime.Now.AddSeconds(0.5).Ticks;
                        if (Error_States() > 0)
                        {
                            if (!PSet.OnfOwner && !PSet.Onf_Stop && !PSet.OnfSetup && !PSet.Onf_PsWd)
                            {
                                Fom_Stop = new fom_Stop(this);
                                Fom_Stop.Show();
                                FomFlash.Play(0);   //20201119
                            }
                        }
                    }

                    label73.Text = (PLC.OfSetL + PLC.Length).ToString();
                    label74.Text = (PLC.OfSetL + PLC.Seting).ToString();

                    if (PLC.DO.Select_01) { TSet.SelectNo = "1"; strModel = DB_All.DBModel.KeyBox((PLC.OfSetL + PLC.Dist_A).ToString()); }
                    if (PLC.DO.Select_02) { TSet.SelectNo = "2"; strModel = DB_All.DBModel.KeyBox((PLC.OfSetL + PLC.Dist_B).ToString()); }
                    if (PLC.DO.Select_03) { TSet.SelectNo = "3"; strModel = DB_All.DBModel.KeyBox((PLC.OfSetL + PLC.Dist_C).ToString()); }
                    if (PLC.DO.Select_04) { TSet.SelectNo = "4"; strModel = DB_All.DBModel.KeyBox((PLC.OfSetL + PLC.Dist_D).ToString()); }
                    if (PLC.DO.Select_05) { TSet.SelectNo = "5"; strModel = DB_All.DBModel.KeyBox((PLC.OfSetL + PLC.Dist_E).ToString()); }
                    if (PLC.DO.Select_06) { TSet.SelectNo = "6"; strModel = DB_All.DBModel.KeyBox((PLC.OfSetL + PLC.Dist_F).ToString()); }
                    if (PLC.DO.Select_07) { TSet.SelectNo = "7"; strModel = DB_All.DBModel.KeyBox((PLC.OfSetL + PLC.Dist_G).ToString()); }
                    if (PLC.DO.Select_08) { TSet.SelectNo = "8"; strModel = DB_All.DBModel.KeyBox((PLC.OfSetL + PLC.Dist_H).ToString()); }
                    if (PLC.DO.Select_09) { TSet.SelectNo = "9"; strModel = DB_All.DBModel.KeyBox((PLC.OfSetL + PLC.Dist_I).ToString()); }
                    if (PLC.DO.Select_10) { TSet.SelectNo = "10"; strModel = DB_All.DBModel.KeyBox((PLC.OfSetL + PLC.Dist_J).ToString()); }

                    if (TSet.SelectNo != "" && TSet.Select_o != TSet.SelectNo && !cboModel.DroppedDown)
                    {                  //12345678901234567
                        txtVinNo.Text = "TEST-000000000000";
                        TSet.Select_o = TSet.SelectNo;
                        Key_Vehicles(strModel);
                    }

                    if (PLC.DI.Start__PB || PLC.DI.GOT_Start)
                    {
                        OrderStarted();
                    }
                }
            }
            #endregion

            Test_LogData();
        }
        
        private int Error_States()//장비 상태 체크
        {
            int Err_flag = 0;

            Err_flag += Ret_ErrValue(ABSBoard.BHerz == 0);  //ABS Board Error

            Err_flag += Ret_ErrValue(!PLC.DO.MD___Auto);   //Auto Mode Check    
            Err_flag += Ret_ErrValue(!PLC.DO.MD__Ready);   //Ready Hold

            Err_flag += Ret_ErrValue(!PLC.DO.MD_FirstH);   //WB Homeposition

            if (!PLC.DO.WBMoveing)
            {
                Err_flag += Ret_ErrValue(!PLC.DO.Pos_Enter);   //Enter Position
            }

            Err_flag += Ret_ErrValue(PLC.DI.WB_L__Min);    //WB Min Position
            Err_flag += Ret_ErrValue(PLC.DI.WB_L__Max);    //WB Max Position

            Err_flag += Ret_ErrValue(!PLC.DI.L_Post_Dn);    //Front Left  Post
            Err_flag += Ret_ErrValue(!PLC.DI.R_Post_Dn);   //Front Right Post

            Err_flag += Ret_ErrValue(!PLC.DI.FLF_SR_Dn);   //Front Front-Left  Safty Roller
            Err_flag += Ret_ErrValue(!PLC.DI.FRF_SR_Dn);   //Front Front-Right Safty Roller
            Err_flag += Ret_ErrValue(PLC.DI.FL_MotErr);    //Front Left  Motor
            Err_flag += Ret_ErrValue(PLC.DI.FR_MotErr);    //Front Right Motor
            Err_flag += Ret_ErrValue(!PLC.DI.FLR_SR_Dn);   //Front Rear-Left  Safty Roller
            Err_flag += Ret_ErrValue(!PLC.DI.FRR_SR_Dn);   //Front Rear-Right Safty Roller

            Err_flag += Ret_ErrValue(!PLC.DI.RLR_SR_Dn);   //Rear  Front-Left  Safty Roller
            Err_flag += Ret_ErrValue(!PLC.DI.RRR_SR_Dn);   //Rear  Front-Right Safty Roller
            Err_flag += Ret_ErrValue(PLC.DI.RL_MotErr);    //Rear  Left  Motor
            Err_flag += Ret_ErrValue(PLC.DI.RR_MotErr);    //Rear  Right Motor

            Err_flag += Ret_ErrValue(PLC.DI.PHO_Front);    //Front Photo Sensor
            Err_flag += Ret_ErrValue(PLC.DI.PHO__Rear);    //Rear  Photo Sensor

            Err_flag += Ret_ErrValue(PLC.DI.L_Flap_Up);    //Exhaust Flap
            Err_flag += Ret_ErrValue(PLC.DI.R_Flap_Up);    //Exhaust Flap

            //Err_flag += Ret_ErrValue(!PLC.DI.Rmt_ST);    //리모컨 준비 상태

            btn_Test.Enabled = (Err_flag == 0) ? true : false;

            if (!TSet.BT_LiftUp) { PLC.Brk_Lift__Up(); }

            return Err_flag;
        }

        private int Ret_ErrValue(bool Onf)
        {
            return (Onf ? 1 : 0);
        }

        #region Form
        private void cboModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string model = cboModel.Text;

            Key_Vehicles(model);
        }
        public bool Key_Vehicles(string pModel)
        {
            if (DB_All.DBModel.Select(pModel) == 1)
            {
                cboModel.Text = pModel;
                //txtVinNo.Text = ""; 
                TSet.CarBarID = DB_All.DBModel.dbCarBarID;
                cboModel.Text = DB_All.DBModel.dbCarModel; TSet.CarModel = DB_All.DBModel.dbCarModel;
                lbl__ECU.Text = DB_All.DBModel.dbECUModel; TSet.ECUModel = DB_All.DBModel.dbECUModel;
                lblEngin.Text = DB_All.DBModel.dbCarEngin; TSet.CarEngin = DB_All.DBModel.dbCarEngin;
                lblTranM.Text = DB_All.DBModel.dbCarTranM; TSet.CarTranM = DB_All.DBModel.dbCarTranM;
                lbl_ABST.Text = DB_All.DBModel.dbCar_ABST; TSet.Car_ABST = DB_All.DBModel.dbCar_ABST;
                lblCurve.Text = DB_All.DBModel.dbCarCurve; TSet.CarCurve = DB_All.DBModel.dbCarCurve;
                lblDrive.Text = DB_All.DBModel.dbCarDrive; TSet.CarDrive = DB_All.DBModel.dbCarDrive;
                lblWBase.Text = DB_All.DBModel.dbCarWbase; TSet.CarWbase = DB_All.DBModel.dbCarWbase;

                cls_Data Standard = new cls_Data(this);
                lblParam.Text = Standard.Sel_Parameter(TSet.CarModel);
                Sel_Vehicles(TSet.CarModel);

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Sel_Vehicles(string pModel)
        {
            if (DB_All.DBModel.Select(pModel) == 1)
            {
                cboModel.Text = pModel;
                //txtVinNo.Text = ""; 
                                                           TSet.CarBarID = DB_All.DBModel.dbCarBarID;
                cboModel.Text = DB_All.DBModel.dbCarModel; TSet.CarModel = DB_All.DBModel.dbCarModel;
                lbl__ECU.Text = DB_All.DBModel.dbECUModel; TSet.ECUModel = DB_All.DBModel.dbECUModel;
                lblEngin.Text = DB_All.DBModel.dbCarEngin; TSet.CarEngin = DB_All.DBModel.dbCarEngin;
                lblTranM.Text = DB_All.DBModel.dbCarTranM; TSet.CarTranM = DB_All.DBModel.dbCarTranM;
                lbl_ABST.Text = DB_All.DBModel.dbCar_ABST; TSet.Car_ABST = DB_All.DBModel.dbCar_ABST;
                lblCurve.Text = DB_All.DBModel.dbCarCurve; TSet.CarCurve = DB_All.DBModel.dbCarCurve;
                lblDrive.Text = DB_All.DBModel.dbCarDrive; TSet.CarDrive = DB_All.DBModel.dbCarDrive;
                lblWBase.Text = DB_All.DBModel.dbCarWbase; TSet.CarWbase = DB_All.DBModel.dbCarWbase;
                
                cls_Data Standard = new cls_Data(this);
                lblParam.Text = Standard.Sel_Parameter(TSet.CarModel);

                string SelWBase = "";
                int carWbase; int.TryParse(DB_All.DBModel.dbCarWbase, out carWbase);

                if (carWbase == PLC.OfSetL + PLC.Dist_A) SelWBase = "1";
                if (carWbase == PLC.OfSetL + PLC.Dist_B) SelWBase = "2";
                if (carWbase == PLC.OfSetL + PLC.Dist_C) SelWBase = "3";
                if (carWbase == PLC.OfSetL + PLC.Dist_D) SelWBase = "4";
                if (carWbase == PLC.OfSetL + PLC.Dist_E) SelWBase = "5";
                if (carWbase == PLC.OfSetL + PLC.Dist_F) SelWBase = "6";
                if (carWbase == PLC.OfSetL + PLC.Dist_G) SelWBase = "7";
                if (carWbase == PLC.OfSetL + PLC.Dist_H) SelWBase = "8";
                if (carWbase == PLC.OfSetL + PLC.Dist_I) SelWBase = "9";
                if (carWbase == PLC.OfSetL + PLC.Dist_J) SelWBase = "10";

                PLC.DO.Vehicle_Balance = DB_All.DBModel.dbCarBalance.ToUpper() == "Y";

                PLC.DO.Vehicle01 = false;
                PLC.DO.Vehicle02 = false;
                PLC.DO.Vehicle03 = false;
                PLC.DO.Vehicle04 = false;
                PLC.DO.Vehicle05 = false;
                PLC.DO.Vehicle06 = false;
                PLC.DO.Vehicle07 = false;
                PLC.DO.Vehicle08 = false;
                PLC.DO.Vehicle09 = false;
                PLC.DO.Vehicle10 = false;

                switch (SelWBase)
                {
                    case "1": PLC.DO.Vehicle01 = true; break;
                    case "2": PLC.DO.Vehicle02 = true; break;
                    case "3": PLC.DO.Vehicle03 = true; break;
                    case "4": PLC.DO.Vehicle04 = true; break;
                    case "5": PLC.DO.Vehicle05 = true; break;
                    case "6": PLC.DO.Vehicle06 = true; break;
                    case "7": PLC.DO.Vehicle07 = true; break;
                    case "8": PLC.DO.Vehicle08 = true; break;
                    case "9": PLC.DO.Vehicle09 = true; break;
                    case "10": PLC.DO.Vehicle10 = true; break;
                    default:
                        int wbase; int.TryParse(DB_All.DBModel.dbCarWbase, out wbase);
                        PLC.PLC_WhelBase(wbase);
                        H2Y.Sleep(100);
                        PLC.DO.Vehicle10 = true;
                        break;
                }

                PLC.PLC_Put_D562();

                H2Y.Sleep(100);

                PLC.DO.Vehicle01 = false;
                PLC.DO.Vehicle02 = false;
                PLC.DO.Vehicle03 = false;
                PLC.DO.Vehicle04 = false;
                PLC.DO.Vehicle05 = false;
                PLC.DO.Vehicle06 = false;
                PLC.DO.Vehicle07 = false;
                PLC.DO.Vehicle08 = false;
                PLC.DO.Vehicle09 = false;
                PLC.DO.Vehicle10 = false;
                PLC.PLC_Put_D562();

                Prog_LogData("Vehicle Select OK");
                return true;
            }
            else
            {
                Prog_LogData("Vehicle Select Error");
                return false;
            }
        }

        private void buttons_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnClose": Program_Exit();                break;

                case "btn_Search": pnl_Date.Visible = !pnl_Date.Visible; break; //pnl_Find.Visible = !pnl_Find.Visible; break;
                case "btn_Cancel": pnl_Find.Visible = false; break;
                case "btn_Find": pnl_Find.Visible = false; break;

                case "btnResend": Order_Resend();               break;
                case "btnDelete": Order_Delete();               break;
                case "btn_Help": Order___Help();                break;
                case "btn_Sett": Program_Sett();                break;
                case "btn_Test": OrderStarted();                break;
                case "btn_Stop": OrderStopped();                break;

                case "btnDebug": if (PSet.OnfDebug) { return; }
                                 FomDebug = new fomDebug(this);
                                 FomDebug.Show();               break;

                case "btnNIDaq": if (FomNIDaq.IsOpen) { return; }
                                 FomNIDaq.Show(this);           break;

                case "btnSpeed": FomSpeed = new fomSpeed();
                                 FomSpeed.Show(this);           break;

                case "btnFStop": Fom_Stop = new fom_Stop();
                                 Fom_Stop.Show(this);           break;

                case "btn_Dist": Fom_Dist = new fom_Dist();
                                 Fom_Dist.Show(this);           break;

                case "btn1Gage": Fom1Gage = new fom1Gage();
                                 Fom1Gage.Show(this); break;

                case "btn4Gage": Fom4Gage = new fom4Gage();
                                 Fom4Gage.Show(this);           break;

                case "btn_Keys": Fom_Keys = new fom_Keys();     
                                 Fom_Keys.Show(this);           break;

                case "btn_Loss": Loss_Calibration();            break;
                case "btn_Load": Load_Calibration();            break;
                case "btn_SSTs": SSTs_Running();                break;
                case "btn_BRKs": BRKs_Running();                break;
                case "btn__Ask": ASK__Running();                break;

                case "btnFlash": int idx; if (!int.TryParse(txtFlash.Text, out idx)) return;

                                          FomFlash.Play(idx);
                                            break;

                case "btn_Logs": Logs.MakeLog_File(Log_His.Test, "Log Test");    break;
            }
        }

        private void dgv_List_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectRow = e.RowIndex;
        }
        private void dgv_List_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgv_List.CurrentRow != null)
            {
                SelectRow = dgv_List.CurrentRow.Index;
                string AcptNo = dgv_List[0, SelectRow].Value.ToString();

                SelectResult(AcptNo);     
            }
        }
        private void dgv_List_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectRow = e.RowIndex;
        }
        private void dgv_List_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectRow = e.RowIndex;
            if (SelectRow >= 0)
            {
                string AcptNo = dgv_List[0, SelectRow].Value.ToString();

                SelectResult(AcptNo);
            }
        }

        private void SelectResult(string pAcptNo)
        {
            try
            {
                cls_Data Results = new cls_Data(this);

                Results.Read_AllData(pAcptNo);

                InitInfoData(); //측정 정보 초기화

                lbl_Work.Text = Results.AcceptNo;
                txtVinNo.Text = Results.Vin___No;
                lbl_Time.Text = Results.TestTime;
                cboModel.Text = Results.CarModel;
                lbl__ECU.Text = Results.ECUModel;
                lblEngin.Text = Results.CarEngin;
                lblTranM.Text = Results.CarTranM;
                lbl_ABST.Text = Results.Car_ABST;
                lblCurve.Text = Results.CarCurve;
                lblDrive.Text = Results.CarDrive;
                lblWBase.Text = Results.CarWbase;

                cls_Data Standard = new cls_Data(this);
                lblParam.Text = Standard.Sel_Parameter(Results.CarModel);

                ClearResults(); //표기 라벨 클리어

                Test_Results("SST1", Results.SST_Last);
                Test_Results("Drag", Results.Drag_Max);
                Test_Results("Brake", Results.GBrk_Max);
                Test_Results("Park", Results.Park_Max);
                Test_Results("Dec.", Results.ABSB_Min);
                Test_Results("Inc.", Results.ABSB_Max);
                Test_Results("Sped", Results.SMT_Last);
                Test_Results("WSS_", Results.WSS__Max);
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "SelectResult: " + ex.Message);
            }
        }

        public void SSTs_Running()
        {
            if (!Fom_SSTs.IsOpen)
            {
                //Fom_SSTs.Show(this);
                //Fom_SSTs.Calibration();
                Fom_SSTs.SSTs_Running();
            }
        }

        public void BRKs_Running()
        {
            fomBrake Brk_Test = new fomBrake();
            Brk_Test.BringToFront();
            Brk_Test.BRKs_Running();
        }

        public void ASK__Running()
        {
            fom__ASK Ask = new fom__ASK(this);

            int Ret = Ask.Ret_Message();
        }

        public void Loss_Calibration()
        {
            if (Fom_Loss == null || Fom_Loss.IsOpen  == false)
            {
                Fom_Loss = new fom_Loss();
            }

            Fom_Loss.Show(this);
        }

        public void Load_Calibration()
        {
            if (Fom_Load == null || Fom_Load.IsOpen == false)
            {
                Fom_Load = new fom_Load();
            }

            Fom_Load.Show(this);
        }

        private void Order_Report()
        {
        }
        private void Order_Resend()
        {
            fom_Webs browser = new fom_Webs(this);
            browser.Show();
        }
        private void Order_Delete()
        {
            if (dgv_List.CurrentRow == null) return;

            SelectRow = dgv_List.CurrentRow.Index;

            try
            {
                string AcptNo = dgv_List[0, SelectRow].Value.ToString();
                string Vin_No = dgv_List[1, SelectRow].Value.ToString();

                if (!H2Y.Question(Vin_No + " Delete?", "Delete")) { return; }

                DB_All.DB_Info.Delete(AcptNo);
                DB_All.DB_RnBs.Delete(AcptNo);
                DB_All.DB_ECUs.Delete(AcptNo);

                MessageBoxEx.Show(PSet.LangMain[52]);  //삭제되었습니다.
                Logs.MakeLog_File(Log_His.Del_, AcptNo + "/" + Vin_No);
            }
            catch(Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Order_Delete: " + ex.Message);
            }

            All_DataList(PSet.Sel_Date.ToString("yyyyMMdd"));
        }
        private void Order___Help()
        {
            Process.Start("AcroRd32.exe", Application.StartupPath + @"\Flash\RnBT Manual.pdf");
        }

        private void Program_Exit()
        {                   //"프로그램을 종료하시겠습니까?", "종료"
            if (!H2Y.Question(PSet.LangMain[53], PSet.LangMain[54])) { return; }
            
            btnClose.Image = KI_RnB.Properties.Resources.Quit_2;
            tmrStart.Enabled = false;
            tmr_Main.Enabled = false;
            
            PSet.Onf_Prog = false;
            NeoVI.Device_Close();
            CheryEchoThread.Terminate();
            try
            {
                foreach (Form fom in OwnedForms)
                {
                    fom.Close();
                }

                //DLC.Device_Close();

                if (PLC.IsOpen)
                {
                    PLC.DO.PC_Standy = false;
                    PLC.PLC_312_Puts();
                    PLC.Close();
                }

                NI.Close();

                if (ABSBoard != null) ABSBoard.Close();
                if (Barcode1 != null) Barcode1.Close();
                
                Application.ExitThread();
                Application.Exit();

                //System.Environment.Exit();

                string proc = Process.GetCurrentProcess().ProcessName;
                Process[] processes = Process.GetProcessesByName(proc);

                if (processes.Length > 1)
                {
                    foreach (Process pro in processes)
                    {
                        pro.Kill();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Program_Sett()
        {
            if (PSet.OnfSetup)
            {
                //FomSetup.ShowDialog(this);
                FomSetup.Show(this);
            }
            else
            {
                Fom_PwWd = new fom_PsWd(0);

                if (PSet.Passwd == "")
                {
                    FomSetup = new fomSetup();
                    //FomSetup.ShowDialog(this);
                    FomSetup.Show(this);
                    Refresh_Sets();
                }
                else
                {
                    Fom_PwWd.ShowDialog();

                    if (Fom_PwWd.PassLock > 0)
                    {
                        FomSetup = new fomSetup(Fom_PwWd.PassLock, this);
                        //FomSetup.ShowDialog(this);
                        FomSetup.Show(this);
                        Refresh_Sets();
                    }
                }
            }
        }

        private void OrderStarted()
        {
            bool Ret_test = false;

            if (!TSet.SimulOnf)
            {
                if (Fom_Init.IsOpen) return;
                if (!PLC.DO.MD___Auto) return;
                if (!PLC.DO.MD__Ready) return;
                if (TSet.Test_Run) return;
            }

            ClearResults();

            tmr_Main.Enabled = false;

            btn_Sett.Visible = false;
            btn_Test.Visible = false;
            pic_View.Visible = false;
            btn_Stop.Visible = true;
            dgv_List.Enabled = false;
            
            string Now_Date = DateTime.Now.ToString("yyyyMMdd");
            int Number = DB_All.DB_Info.Max_No(Now_Date) + 1;
            string AcptNo = Now_Date + Number.ToString("00000");

            Control_State(false);

            TSet.Test_SST = chk__SST.Checked;
            TSet.Test__BT = chk___BT.Checked;
            TSet.Test_RnB = chk__RnB.Checked;

            TSet.Test_Run = true;
            TSet.TestStop = false;

            TSet.AcceptNo = AcptNo;                             //측정 번호
            if (TSet.Vin___No == "") TSet.Vin___No = txtVinNo.Text; //바코드 VIN이 이미 설정되어 있으면 유지, 아니면 txtVinNo 사용 - 260816 수정
            TSet.CarModel = cboModel.Text;                      //모델명
            TSet.ECUModel = lbl__ECU.Text;                      //ECU 모델명
            TSet.CarBarID = lblEngin.Text;                      //바코드 구분자
            TSet.CarWbase = lblWBase.Text;                      //휠베이스 거리
            TSet.CarEngin = lblEngin.Text;                      //엔진 모델
            TSet.CarTranM = lblTranM.Text;                      //트렌스미션 모델
            TSet.Car_ABST = lbl_ABST.Text;                      //ABS 모델
            TSet.CarCurve = lblCurve.Text;                      //드라이브 커브
            TSet.CarDrive = lblDrive.Text;                      //드라이브 축

            Prog_LogData("Test Start : " + TSet.Vin___No);
            Sel_Vehicles(TSet.CarModel);

            PLC.Door____Open();

            cls_Data Standard = new cls_Data(this);
            TSet.CarParam = Standard.Sel_Parameter(TSet.CarModel);
            CheryEchoThread.SendEcho(false);
            if (TSet.CarParam != "")
            {
                TSet.TestDate = Now_Date;                           //측정 일자
                TSet.Run_Time = DateTime.Now.ToString(H2Y.format0Time);  //진행 시작 시간
                //TSet.TestTime = DateTime.Now.ToString(H2Y.format0Time);  //측정 시각 시간

                lblParam.Text = TSet.CarParam;                      //Parameter

                lbl_Work.Text = TSet.AcceptNo;
                lbl_Time.Text = TSet.Run_Time;

                if (Test_Standby())
                {
                    FomFlash.VinNo_Hide();
                    TSet.Info_DataAdd(this);                                //Info 생성
                    PSet.Test_CNTRead();                                    //카운터 확인
                    Test = new cls_Test(this);                              //측정 생성
                    Ret_test = Test.Test_Running(AcptNo);                   //측정 모듈
                    TSet.End_Time = DateTime.Now.ToString(H2Y.format0Time); //종료 시간
                    TSet.Info_DataAdd(this);                                //Info 업데이트
                    PSet.Test_CNTMake("Pass");                              //카운터 저장
                }

                TSet.Info_Clear();
                InitInfoData();
                All_DataList(PSet.Sel_Date.ToString("yyyyMMdd"));
            }

            PLC.Test__Finish();
            H2Y.Sleep(500);
            PLC.Test_All_Off();

            string End_Msgs = "";

            switch (TSet.StopFlag)
            {
                case 0: End_Msgs = "Test Finish : " + TSet.Vin___No; break;
                case 1: End_Msgs = "Test Stop : EMERGENCY Switch";  break;
                case 2: End_Msgs = "Test Stop : Stop Switch";       break;
                case 3: End_Msgs = "Test Stop : Program button";    break;
                case 4: End_Msgs = "Test Stop : Stop button";       break;
                case 5: End_Msgs = "Test Stop : GOT Stop";          break;
                case 6: End_Msgs = "Test Cancel : Cancel Button";   break;
                case 7: End_Msgs = "Test Cancel : GOT Cancel";      break;

                case 10: End_Msgs = "Test Stop : Mode Change";      break;
                case 11: End_Msgs = "Test Stop : Ready Off";        break;

                case 21: End_Msgs = "Test Stop : F-L Motor Error";  break;
                case 22: End_Msgs = "Test Stop : F-R Motor Error";  break;
                case 23: End_Msgs = "Test Stop : R-L Motor Error";  break;
                case 24: End_Msgs = "Test Stop : R-R Motor Error";  break;
            }

            Prog_LogData(End_Msgs);
            Logs.MakeLog_File(Log_His.Save, End_Msgs);

            btn_Sett.Visible = true;
            btn_Test.Visible = true;
            btn_Stop.Visible = false;
            pic_View.Visible = false;
            tmr_Main.Enabled = true;
            dgv_List.Enabled = true;

            FomFlash.Play(0);
            PLC.Door____Open();

            TSet.Test_Run = false;
            Control_State(true);
            CheryEchoThread.SendEcho(false);
        }

        private bool Test_Standby() //측정 정보 확인
        {
            bool flag = true;

            if (flag && TSet.AcceptNo == "") flag = false;
            if (flag && TSet.Vin___No == "") flag = false; 
            if (flag && TSet.CarModel == "") flag = false;
            if (flag && TSet.ECUModel == "") flag = false;
            if (flag && TSet.CarBarID == "") flag = false;
            if (flag && TSet.CarWbase == "") flag = false;
            if (flag && TSet.CarEngin == "") flag = false;
            if (flag && TSet.CarTranM == "") flag = false;
            if (flag && TSet.Car_ABST == "") flag = false;
            if (flag && TSet.CarCurve == "") flag = false;
            if (flag && TSet.CarDrive == "") flag = false;
            if (flag && TSet.CarParam == "") flag = false;

            return flag;
        }

        private void InitInfoData() //측정 정보 초기화
        {
            lbl_Work.Text = "Test No";
            txtVinNo.Text = "";
            lbl_Time.Text = "Time";
            lbl__ECU.Text = "";
            cboModel.Text = "";
            lblEngin.Text = "";
            lblTranM.Text = "";
            lbl_ABST.Text = "";
            lblCurve.Text = "";
            lblDrive.Text = "";
            lblWBase.Text = "";
            lblParam.Text = "";
        }

        private void OrderStopped() //측정 정지 명령
        {
            TSet.TestStop = true;
            CheryEchoThread.SendEcho(false);
            Prog_LogData("Program STOP Button");
        }

        private void Control_State(bool OnOff)
        {
            btnClose.Enabled = OnOff;
            btn_Search.Enabled = OnOff;
            btn_Rept.Enabled = OnOff;
            btnResend.Enabled = OnOff;
            btnDelete.Enabled = OnOff;
            btn_Help.Enabled = OnOff;
            txtVinNo.Enabled = OnOff;
            cboModel.Enabled = OnOff;
            btn_Sett.Enabled = OnOff;
            btn_Test.Enabled = OnOff;
        }
        
        private void buttons_MouseEnter(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnClose": ((Button)sender).Image = KI_RnB.Properties.Resources.Quit_1;       break;
                case "btn_Search": ((Button)sender).Image = KI_RnB.Properties.Resources.Search_1;   break;
                case "btn_Find": ((Button)sender).Image = KI_RnB.Properties.Resources.Find_1;       break;
                case "btn_Cancel": ((Button)sender).Image = KI_RnB.Properties.Resources.Cancel_1;   break;
            }
        }

        private void buttons_MouseLeave(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnClose": ((Button)sender).Image = KI_RnB.Properties.Resources.Quit_0;       break;
                case "btn_Search": ((Button)sender).Image = KI_RnB.Properties.Resources.Search_0;   break;
                case "btn_Find": ((Button)sender).Image = KI_RnB.Properties.Resources.Find_0;       break;
                case "btn_Cancel": ((Button)sender).Image = KI_RnB.Properties.Resources.Cancel_0;   break;
            }
        }

        private void tab_Main_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab_Main.SelectedIndex == 2)
            {
                tab_Main.BringToFront();
            }
            else
            {
                tab_Main.SendToBack();
            }

            Thread.Sleep(200);
        }
        #endregion

        #region Log Data
        public void MachineState()
        {
            try
            {
                if ((Onf_GapT - DateTime.Now.Ticks) < 0)
                {
                    Onf_GapT = DateTime.Now.AddSeconds(0.3).Ticks;

                    btn_Auto.BackColor = (PLC.DO.MD___Auto ? System.Drawing.Color.LimeGreen : System.Drawing.Color.White);
                    btn_Manu.BackColor = (PLC.DO.MD_Manual ? System.Drawing.Color.LimeGreen : System.Drawing.Color.White);
                    btn_Cali.BackColor = (PLC.DI.PBCalMode ? System.Drawing.Color.LimeGreen : System.Drawing.Color.White);

                    btnPHome.BackColor = (PLC.DO.Pos__Home ? System.Drawing.Color.LimeGreen : System.Drawing.Color.White);
                    btnPEntr.BackColor = (PLC.DO.Pos_Enter ? System.Drawing.Color.LimeGreen : System.Drawing.Color.White);
                    btnPTest.BackColor = (PLC.DO.Pos__Test ? System.Drawing.Color.LimeGreen : System.Drawing.Color.White);
                    btnPExit.BackColor = (PLC.DO.Pos__Exit ? System.Drawing.Color.LimeGreen : System.Drawing.Color.White);

                    btnLPost.BackColor = Ret_UpOrDown(0, PLC.DI.L_Post_Up, PLC.DI.L_Post_Dn);
                    btnRPost.BackColor = Ret_UpOrDown(0, PLC.DI.R_Post_Up, PLC.DI.R_Post_Dn);

                    btnFPhoL.BackColor = Ret_ColorOnf(1, PLC.DI.PHO_Front);
                    btnFPhoR.BackColor = Ret_ColorOnf(1, PLC.DI.PHO_Front);

                    btnFLSRF.BackColor = Ret_UpOrDown(0, PLC.DI.FLF_SR_Up, PLC.DI.FLF_SR_Dn);
                    btnFRSRF.BackColor = Ret_UpOrDown(0, PLC.DI.FRF_SR_Up, PLC.DI.FRF_SR_Dn);

                    btnFLMot.BackColor = Ret_ColorOnf(0, PLC.DI.FL_MotErr);
                    btnFRMot.BackColor = Ret_ColorOnf(0, PLC.DI.FR_MotErr);

                    btnFLSRR.BackColor = Ret_UpOrDown(0, PLC.DI.FLR_SR_Up, PLC.DI.FLR_SR_Dn);
                    btnFRSRR.BackColor = Ret_UpOrDown(0, PLC.DI.FRR_SR_Up, PLC.DI.FRR_SR_Dn);

                    btnRPhoL.BackColor = Ret_ColorOnf(1, PLC.DI.PHO__Rear);
                    btnRPhoR.BackColor = Ret_ColorOnf(1, PLC.DI.PHO__Rear);

                    btnRLSRF.BackColor = Ret_UpOrDown(0, PLC.DI.RLR_SR_Up, PLC.DI.RLR_SR_Dn);
                    btnRRSRF.BackColor = Ret_UpOrDown(0, PLC.DI.RRR_SR_Up, PLC.DI.RRR_SR_Dn);

                    btnRLMot.BackColor = Ret_ColorOnf(0, PLC.DI.RL_MotErr);
                    btnRRMot.BackColor = Ret_ColorOnf(0, PLC.DI.RR_MotErr);

                    btnRFlap.BackColor = Ret_UpOrDown(0, PLC.DI.L_Flap_Up, PLC.DI.R_Flap_Dn);

                    btnFLMot.Text = NI.Loss.FL.Speed.ToString("0.0");
                    btnFRMot.Text = NI.Loss.FR.Speed.ToString("0.0");
                    btnRLMot.Text = NI.Loss.RL.Speed.ToString("0.0");
                    btnRRMot.Text = NI.Loss.RR.Speed.ToString("0.0");

                    btnFLMot.BackColor = NI.Loss.FL.Speed < 5 ? Color.Green : Color.Red;
                    btnFRMot.BackColor = NI.Loss.FR.Speed < 5 ? Color.Green : Color.Red;
                    btnRLMot.BackColor = NI.Loss.RL.Speed < 5 ? Color.Green : Color.Red;
                    btnRRMot.BackColor = NI.Loss.RR.Speed < 5 ? Color.Green : Color.Red;

                    //교정주기
                    btnRFlap.Text = PSet.LangMain[16] + " ( " + PSet.T__CNT.ToString() + "/" + PSet.CalCyc.ToString() + " )";

                    if (PSet.T__CNT < PSet.CalCyc)
                    {
                        btnRFlap.BackColor = Color.WhiteSmoke;
                        btnRFlap.ForeColor = Color.Black;
                    }
                    else
                    {
                        btnRFlap.BackColor = Color.Red;
                        btnRFlap.ForeColor = Color.Lime;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
                Logs.MakeLog_File(Log_His.Err_, "MachineState: " + ex.Message);
            }
        }
        private static Color Ret_UpOrDown(int pMode, bool Up, bool Down)
        {
            if (Up && Down)
            {
                return Color.Red;
            }
            else
            {
                if (Up)
                {
                    return Color.Yellow;
                }
                else
                {
                    if (Down)
                    {
                        return Color.White;
                    }
                    else
                    {
                        return Color.Khaki;
                    }
                }
            }
        }
        private Color Ret_ColorOnf(int pMode, bool Onf)
        {
            System.Drawing.Color Ret_Color;

            if (Onf)
            {
                switch (pMode)
                {
                    case 0: Ret_Color = Color.Lime; break;
                    case 1: Ret_Color = Color.Red; break;
                    default: Ret_Color = Color.Lime; break;
                }
            }
            else
            {
                Ret_Color = Color.White;
            }

            return Ret_Color;
        }

        public void Prog_LogData(string msg)
        {
            if (Porg_Msg == msg) { return; }
            Porg_Msg = msg;

            string now = DateTime.Now.ToString(H2Y.format0Time);
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    lst_Logs.Items.Insert(0, "[" + now + "] " + msg);
                    if (lst_Logs.Items.Count > 500) lst_Logs.Items.RemoveAt(lst_Logs.Items.Count - 1);
                }));
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Prog_LogData: " + ex.Message);
            }
        }

        #region Motor Drive
        private void btn_Read_Click(object sender, EventArgs e)
        {
            btn_Read.Enabled = false;
            MDrive__Read();
            btn_Read.Enabled = true;
        }

        public void MDrive__Read()
        {
            if (Nidec != null)
            {
                if (Nidec.IsOpen)
                {
                    Nidec.All__Read();

                    MDrive__Show();
                }
            }
        }
        public void MDrive_Write()
        {
            if (Nidec.IsOpen)
            {
                int tmp;
                TSet.Nidec_FL.Status = int.TryParse(txt_FL_0.Text, out tmp) ? tmp : 0;
                TSet.Nidec_FL.CalSpd = int.TryParse(txt_FL_1.Text, out tmp) ? tmp : 0;
                TSet.Nidec_FL.WSSSpd = int.TryParse(txt_FL_2.Text, out tmp) ? tmp : 0;
                TSet.Nidec_FL.PB_Toq = int.TryParse(txt_FL_3.Text, out tmp) ? tmp : 0;
                TSet.Nidec_FL.PB_Spd = int.TryParse(txt_FL_4.Text, out tmp) ? tmp : 0;

                TSet.Nidec_FR.Status = int.TryParse(txt_FR_0.Text, out tmp) ? tmp : 0;
                TSet.Nidec_FR.CalSpd = int.TryParse(txt_FR_1.Text, out tmp) ? tmp : 0;
                TSet.Nidec_FR.WSSSpd = int.TryParse(txt_FR_2.Text, out tmp) ? tmp : 0;
                TSet.Nidec_FR.PB_Toq = int.TryParse(txt_FR_3.Text, out tmp) ? tmp : 0;
                TSet.Nidec_FR.PB_Spd = int.TryParse(txt_FR_4.Text, out tmp) ? tmp : 0;

                TSet.Nidec_RL.Status = int.TryParse(txt_RL_0.Text, out tmp) ? tmp : 0;
                TSet.Nidec_RL.CalSpd = int.TryParse(txt_RL_1.Text, out tmp) ? tmp : 0;
                TSet.Nidec_RL.WSSSpd = int.TryParse(txt_RL_2.Text, out tmp) ? tmp : 0;
                TSet.Nidec_RL.PB_Toq = int.TryParse(txt_RL_3.Text, out tmp) ? tmp : 0;
                TSet.Nidec_RL.PB_Spd = int.TryParse(txt_RL_4.Text, out tmp) ? tmp : 0;

                TSet.Nidec_RR.Status = int.TryParse(txt_RR_0.Text, out tmp) ? tmp : 0;
                TSet.Nidec_RR.CalSpd = int.TryParse(txt_RR_1.Text, out tmp) ? tmp : 0;
                TSet.Nidec_RR.WSSSpd = int.TryParse(txt_RR_2.Text, out tmp) ? tmp : 0;
                TSet.Nidec_RR.PB_Toq = int.TryParse(txt_RR_3.Text, out tmp) ? tmp : 0;
                TSet.Nidec_RR.PB_Spd = int.TryParse(txt_RR_4.Text, out tmp) ? tmp : 0;

                Nidec.All_Write();
                H2Y.Sleep(300);
                Nidec.All__Read();

                MDrive__Show();
            }
        }
        public void MDrive__Show()
        {
            if (Nidec.IsOpen)
            {
                txt_FL_0.Text = TSet.Nidec_FL.Status.ToString();
                txt_FL_1.Text = TSet.Nidec_FL.CalSpd.ToString();
                txt_FL_2.Text = TSet.Nidec_FL.WSSSpd.ToString();
                txt_FL_3.Text = TSet.Nidec_FL.PB_Toq.ToString();
                txt_FL_4.Text = TSet.Nidec_FL.PB_Spd.ToString();
                lbl_FL_0.BackColor = TSet.Nidec_FL.Cal_MD ? Color.Lime : Color.Gray;
                lbl_FL_1.BackColor = TSet.Nidec_FL.MT_Run ? Color.Lime : Color.Gray;
                lbl_FL_2.BackColor = TSet.Nidec_FL.MTSync ? Color.Lime : Color.Gray;
                lbl_FL_3.BackColor = TSet.Nidec_FL.MT_Brk ? Color.Lime : Color.Gray;
                lbl_FL_4.BackColor = TSet.Nidec_FL.MTPark ? Color.Lime : Color.Gray;

                txt_FR_0.Text = TSet.Nidec_FR.Status.ToString();
                txt_FR_1.Text = TSet.Nidec_FR.CalSpd.ToString();
                txt_FR_2.Text = TSet.Nidec_FR.WSSSpd.ToString();
                txt_FR_3.Text = TSet.Nidec_FR.PB_Toq.ToString();
                txt_FR_4.Text = TSet.Nidec_FR.PB_Spd.ToString();
                lbl_FR_0.BackColor = TSet.Nidec_FR.Cal_MD ? Color.Lime : Color.Gray;
                lbl_FR_1.BackColor = TSet.Nidec_FR.MT_Run ? Color.Lime : Color.Gray;
                lbl_FR_2.BackColor = TSet.Nidec_FR.MTSync ? Color.Lime : Color.Gray;
                lbl_FR_3.BackColor = TSet.Nidec_FR.MT_Brk ? Color.Lime : Color.Gray;
                lbl_FR_4.BackColor = TSet.Nidec_FR.MTPark ? Color.Lime : Color.Gray;

                txt_RL_0.Text = TSet.Nidec_RL.Status.ToString();
                txt_RL_1.Text = TSet.Nidec_RL.CalSpd.ToString();
                txt_RL_2.Text = TSet.Nidec_RL.WSSSpd.ToString();
                txt_RL_3.Text = TSet.Nidec_RL.PB_Toq.ToString();
                txt_RL_4.Text = TSet.Nidec_RL.PB_Spd.ToString();
                lbl_RL_0.BackColor = TSet.Nidec_RL.Cal_MD ? Color.Lime : Color.Gray;
                lbl_RL_1.BackColor = TSet.Nidec_RL.MT_Run ? Color.Lime : Color.Gray;
                lbl_RL_2.BackColor = TSet.Nidec_RL.MTSync ? Color.Lime : Color.Gray;
                lbl_RL_3.BackColor = TSet.Nidec_RL.MT_Brk ? Color.Lime : Color.Gray;
                lbl_RL_4.BackColor = TSet.Nidec_RL.MTPark ? Color.Lime : Color.Gray;

                txt_RR_0.Text = TSet.Nidec_RR.Status.ToString();
                txt_RR_1.Text = TSet.Nidec_RR.CalSpd.ToString();
                txt_RR_2.Text = TSet.Nidec_RR.WSSSpd.ToString();
                txt_RR_3.Text = TSet.Nidec_RR.PB_Toq.ToString();
                txt_RR_4.Text = TSet.Nidec_RR.PB_Spd.ToString();
                lbl_RR_0.BackColor = TSet.Nidec_RR.Cal_MD ? Color.Lime : Color.Gray;
                lbl_RR_1.BackColor = TSet.Nidec_RR.MT_Run ? Color.Lime : Color.Gray;
                lbl_RR_2.BackColor = TSet.Nidec_RR.MTSync ? Color.Lime : Color.Gray;
                lbl_RR_3.BackColor = TSet.Nidec_RR.MT_Brk ? Color.Lime : Color.Gray;
                lbl_RR_4.BackColor = TSet.Nidec_RR.MTPark ? Color.Lime : Color.Gray;
            }
        }
        #endregion

        #region PLC Data
        private void chk_PLCs_Click(object sender, EventArgs e)
        {
            PLC.Is_Log = ((CheckBox)sender).Checked;
        }
        private void cboIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            PLC.Is_Idx = cboIndex.SelectedIndex;
        }
        public void PLC_Log_Data(string msg)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(PLC_Log_Data), msg);
                return;
            }

            if (!chk_PLCs.Checked) return;

            string now = DateTime.Now.ToString(H2Y.format0Time);

            try
            {
                lst_PLCs.Items.Insert(0, "[" + now + "] " + msg);
                if (lst_PLCs.Items.Count > 500) lst_PLCs.Items.RemoveAt(lst_PLCs.Items.Count - 1);
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "PLC_Log_Data: " + ex.Message);
            }
        }
        public void PLC_ScanHerz(string pHerz)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate { lblPLC_H.Text = pHerz; }));
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "PLC_ScanHerz: " + ex.Message);
            }
        }
        #endregion

        #region ABS Board Data
        private void chk_ABSB_Click(object sender, EventArgs e)
        {
            if (ABSBoard.IsOpen)
            {
                ABSBoard.Is_Log = ((CheckBox)sender).Checked;
            }
        }
        public void ABSB_LogData(string msg)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(ABSB_LogData), msg);
                return;
            }

            if (!chk_ABSB.Checked) return;

            string now = DateTime.Now.ToString(H2Y.format0Time);

            try
            {
                lst_ABSB.Items.Insert(0, "[" + now + "] " + msg);
                if (lst_ABSB.Items.Count > 500) lst_ABSB.Items.RemoveAt(lst_ABSB.Items.Count - 1);
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "ABSB_LogData: " + ex.Message);
            }
        }
        public void ABSBScanHerz(string pHerz)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(ABSBScanHerz), pHerz);
                return;
            }

            try
            {
                lblABSBH.Text = pHerz;
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "ABSBScanHerz: " + ex.Message);
            }
        }
        #endregion

        #region Indicator Data
        private void chk_Indi_Click(object sender, EventArgs e)
        {
            if (Pedal.IsOpen)
            {
                Pedal.Is_Log = ((CheckBox)sender).Checked;
            }
        }
        public void Indi_LogData(string msg)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(Indi_LogData), msg);
                return;
            }

            if (!chk_Indi.Checked) return;

            string now = DateTime.Now.ToString(H2Y.format0Time);

            try
            {
                lst_Indi.Items.Insert(0, "[" + now + "] " + msg);
                if (lst_Indi.Items.Count > 500) lst_Indi.Items.RemoveAt(lst_Indi.Items.Count - 1);
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Indi_LogData: " + ex.Message);
            }
        }
        public void IndiScanHerz(string pHerz)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate { lblIndiH.Text = pHerz; }));
            }
            catch (Exception ex)
            {
                //this.Invoke(new MethodInvoker(delegate { lst_Indi.Items.Insert(0, ex.Message); }));
                Logs.MakeLog_File(Log_His.Err_, "IndiScanHerz: " + ex.Message);
            }
        }
        #endregion

        #region Barcode1 Data
        public void Bar1_LogData(string msg)
        {
            string now = DateTime.Now.ToString(H2Y.format0Time);
            
            try
            {
                this.Invoke(new MethodInvoker(delegate 
                                            {
                                                Prog_LogData("Barcode Scan : " + msg);
                                                SelModelList(msg);
                                            }));
            }
            catch (Exception ex)
            {
                Prog_LogData(ex.Message);
            }
        }
        private void txtVinNo_KeyUp(object sender, KeyEventArgs e)
        {
            string strVinNo = txtVinNo.Text;

            if (e.KeyCode == Keys.Enter)
            {
                Prog_LogData("Barcode keyin : " + strVinNo);
                SelModelList(strVinNo);
            }
        }

        private void SelModelList(string pVinNo)
        {
            if (pVinNo.Length > 17)
            {
                FomFlash.VinNo_Show("ERROR", pVinNo);
                txtVinNo.Text = "";
                cboModel.Text = "";
                Prog_LogData("Barcode Digit Error");
                return;
            }

            DataTable dt = new DataTable();
            int Count = DB_All.DBModel.Barcode(pVinNo.Substring(0, 9));

            if (Count == 1)
            {
                FomFlash.VinNo_Show(DB_All.DBModel.dbCarModel, pVinNo);
                cboModel.Text = DB_All.DBModel.dbCarModel;

                Key_Vehicles(DB_All.DBModel.dbCarModel);
                txtVinNo.Text = pVinNo;
                TSet.Vin___No = pVinNo;                             //바코드 VIN 우선 적용 (OrderStarted에서 txtVinNo 덮어쓰기 방지) - 260816 수정
                Prog_LogData("Barcode OK");
                OrderStarted();
            }
            else
            {                
                FomFlash.VinNo_Show("ERROR", pVinNo);
                txtVinNo.Text = "";
                cboModel.Text = "";
                Prog_LogData("Barcode Compare Error");

                if (Count > 1)
                {
                    MessageBoxEx.Show("Barcode model Counter : " + Count);
                }
            }
        }
        #endregion

        #region DLC Data
        private void chk_DLCs_Click(object sender, EventArgs e)
        {
            if (NeoVI.IsOpen)
            {
                NeoVI.Is_Log = ((CheckBox)sender).Checked;
            }
        }
        public void DLCs_LogData(string msg)
        {
            if (!chk_DLCs.Checked) return;

            string now = DateTime.Now.ToString(H2Y.format0Time);

            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    lst_DLCs.Items.Insert(0, "[" + now + "] " + msg);
                    if (lst_DLCs.Items.Count > 500) lst_DLCs.Items.RemoveAt(lst_DLCs.Items.Count - 1);
                }));
            }
            catch (Exception ex)
            {
                lst_DLCs.Items.Insert(0, "[" + now + "] " + ex.Message);
            }
        }
        public void DLC_ScanHerz(string pHerz)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate { lbl_DLCH.Text = pHerz; }));
            }
            catch (Exception ex)
            {
                lst_DLCs.Items.Insert(0, ex.Message);
            }
        }
        #endregion
        #endregion

        #region Test Results Data
        public void ClearResults()
        {
            string cls = "--";

            lblWTub1.Text = PSet.OwnerSFL.ToString("#0.0");
            lblWTub2.Text = PSet.OwnerSFR.ToString("#0.0");
            lblWTub3.Text = PSet.OwnerSRL.ToString("#0.0");
            lblWTub4.Text = PSet.OwnerSRR.ToString("#0.0");

            lblWSS_1.Text = cls; lblWSS_2.Text = cls; lblWSS_3.Text = cls; lblWSS_4.Text = cls;            
            lblDrag1.Text = cls; lblDrag2.Text = cls; lblDrag3.Text = cls; lblDrag4.Text = cls;
            lblBrk_1.Text = cls; lblBrk_2.Text = cls; lblBrk_3.Text = cls; lblBrk_4.Text = cls;
            lblPark1.Text = cls; lblPark2.Text = cls; lblPark3.Text = cls; lblPark4.Text = cls;
            lblDec_1.Text = cls; lblDec_2.Text = cls; lblDec_3.Text = cls; lblDec_4.Text = cls;
            lblInc_1.Text = cls; lblInc_2.Text = cls; lblInc_3.Text = cls; lblInc_4.Text = cls;
            lblATub1.Text = cls; lblATub2.Text = cls; lblATub3.Text = cls; lblATub4.Text = cls;
            lblBal_1.Text = cls; lblBal_2.Text = cls; lblBal_3.Text = cls; lblBal_4.Text = cls;
        }

        public void Test_Results(string kind, TSet.wheel_Test Data)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate 
                {
                    switch (kind)
                    {
                        case "SST1": lbl_SST1.Text = Ret_Data_Str(Data.FL, "#0.0");
                            break;

                        case "Sped": lblSpeed.Text = Ret_Data_Str(Data.FL, "#0.0");                                  
                                     break;

                        case "Drag": lblDrag1.Text = Ret_Data_Str(Data.FL, ""); 
                                     lblDrag2.Text = Ret_Data_Str(Data.FR, ""); 
                                     lblDrag3.Text = Ret_Data_Str(Data.RL, ""); 
                                     lblDrag4.Text = Ret_Data_Str(Data.RR, "");    
                                     break;

                        case "Brake": lblBrk_1.Text = Ret_Data_Str(Data.FL, ""); 
                                     lblBrk_2.Text = Ret_Data_Str(Data.FR, "");
                                     lblBrk_3.Text = Ret_Data_Str(Data.RL, ""); 
                                     lblBrk_4.Text = Ret_Data_Str(Data.RR, "");
                            
                                     lblBal_1.Text = H2Y.Ret_Balance(Data.FL, Data.FR);
                                     lblBal_2.Text = H2Y.Ret_Balance(Data.RL, Data.RR);
                                     lblBal_3.Text = H2Y.Ret_Balance(Data.RL + Data.RR, Data.FL + Data.FR); 
                                     break;

                        case "Park": lblPark1.Text = "";    // Data.FL.ToString(); 
                                     lblPark2.Text = "";    // Data.FR.ToString();
                                     lblPark3.Text = Ret_Data_Str(Data.RL, ""); 
                                     lblPark4.Text = Ret_Data_Str(Data.RR, ""); 
                                     break;

                        case "ATub": lblATub1.Text = Ret_Data_Str(Data.FL, "");
                                     lblATub2.Text = Ret_Data_Str(Data.FR, "");
                                     lblATub3.Text = Ret_Data_Str(Data.RL, "");
                                     lblATub4.Text = Ret_Data_Str(Data.RR, "");
                                     break;

                        case "WSS_": lblWSS_1.Text = Ret_Data_Str(Data.FL, "#0.00");
                                     lblWSS_2.Text = Ret_Data_Str(Data.FR, "#0.00");
                                     lblWSS_3.Text = Ret_Data_Str(Data.RL, "#0.00");
                                     lblWSS_4.Text = Ret_Data_Str(Data.RR, "#0.00"); 
                                     break;

                        case "Dec.": lblDec_1.Text = Ret_Data_Str(Data.FL, "");
                                     lblDec_2.Text = Ret_Data_Str(Data.FR, "");
                                     lblDec_3.Text = Ret_Data_Str(Data.RL, "");
                                     lblDec_4.Text = Ret_Data_Str(Data.RR, "");
                                     break;

                        case "Inc.": lblInc_1.Text = Ret_Data_Str(Data.FL, "");
                                     lblInc_2.Text = Ret_Data_Str(Data.FR, "");
                                     lblInc_3.Text = Ret_Data_Str(Data.RL, "");
                                     lblInc_4.Text = Ret_Data_Str(Data.RR, "");
                                     break;

                        case "WTub": lblWTub1.Text = Ret_Data_Str(Data.FL, "");
                                     lblWTub2.Text = Ret_Data_Str(Data.FR, "");
                                     lblWTub3.Text = Ret_Data_Str(Data.RL, "");
                                     lblWTub4.Text = Ret_Data_Str(Data.RR, "");
                                     break;

                    }
                                    
                }));
            }
            catch (Exception ex)
            {
                lstError.Items.Add(ex.Message);
            }
        }

        private string Ret_Data_Str(double value, string format)
        {
            string Ret_Str = "";

            if (value == -1)
            {
                Ret_Str = "";
            }
            else
            {
                if (format == "")
                {
                    Ret_Str = value.ToString();
                }
                else
                {
                    Ret_Str = value.ToString(format);
                }
            }

            return Ret_Str;
        }

        public void Test_LogData()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    lbl_WSS0.BackColor = (TSet.WSS__Onf ? Color.Green : Color.White);
                    lblDrag0.BackColor = (TSet.Drag_Onf ? Color.Green : Color.White);
                    lblBrk_0.BackColor = (TSet.BrakeOnf ? Color.Green : Color.White);
                    lblPark0.BackColor = (TSet.Park_Onf ? Color.Green : Color.White);
                    lblABSB0.BackColor = (TSet.ABSv_Onf ? Color.Green : Color.White); 
                }));
            }
            catch (Exception ex)
            {
                lstError.Items.Add(ex.Message);
            }
        }
        #endregion

        #region Test
        private void btn_DSet_Click(object sender, EventArgs e)
        {
            int Mv_Lent; if (!int.TryParse(txt_DSet.Text, out Mv_Lent)) return;
            PLC.PLC_WhelBase(Mv_Lent);
        }

        private void btn_Mode_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnPHome": break;
                case "btnPEntr": break;
                case "btnPTest": break;
                case "btnPExit": break;
            }
        }

        private void btnReSet_Click(object sender, EventArgs e)
        {
            ABSBoard.Start();
        }
        #endregion

        public void TestImg_Show(Graphics gTest)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate 
                {
                    Bitmap bmp = new Bitmap(pic_View.Width, pic_View.Height);

                    //bmp = gTest.DrawImage(memoryImage, 0, 0);

                }));
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "TestImg_Show: " + ex.Message);
            }
        }

        private void lblOwner_DoubleClick(object sender, EventArgs e)
        {
            //if (PSet.OnfSetup)
            //{
            //    FomSetup.ShowDialog();
            //}
            //else
            //{
            //    if (!PSet.OnfOwner)
            //    {
            //        fom_PsWd pass = new fom_PsWd(0);
            //        pass.ShowDialog();

            //        if (pass.PassLock)
            //        {
            //            PSet.OnfOwner = true;
            //        }
            //    }
            //    else
            //    {
            //        PSet.OnfOwner = false;
            //    }
            //}

            if (PSet.OnfOwner)
            {
                PSet.OnfOwner = false;
            }
            else
            {
                Fom_PwWd = new fom_PsWd(0);
                Fom_PwWd.ShowDialog();

                if (Fom_PwWd.PassLock == 1)
                {
                    PSet.OnfOwner = true;
                }
                else
                {
                    return;
                }
            }

            DebugMode_On();
        }
        private void DebugMode_On()
        {
            lblOwner.BackColor = PSet.OnfOwner ? Color.BlueViolet : Color.Transparent;
            pic__Car.Visible = PSet.OnfOwner;

            if (!PSet.OnfOwner)
            {
                tab_Main.Controls.RemoveByKey("tabPage3");
            }
            else
            {
                tab_Main.Controls.Add(this.tabPage3);
            }
        }
        
        private void dgv_List_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rect = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgv_List.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dgv_List.RowHeadersDefaultCellStyle.Font, rect,
                         dgv_List.ForeColor = Color.DimGray, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }

        private void btn_Rept_Click(object sender, EventArgs e)
        {
            try
            {
                string AcptNo = dgv_List[0, SelectRow].Value.ToString();
                fom_Rslt Report = new fom_Rslt(this, 1, AcptNo);
                Report.Show();
            }
            catch (Exception EX)
            {
                Logs.MakeLog_File(Log_His.Err_, "btn_Rept_Click: " + EX.Message);
            }
        }

        private void fom_Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Shift && e.KeyCode == Keys.Control && e.KeyCode == Keys.D)
            {
                MessageBox.Show("1");
            }
        }

        private void chkDebug_Click(object sender, EventArgs e)
        {
            TSet.Debug_Md = chkDebug.Checked; 
        }

        private void lblReady_DoubleClick(object sender, EventArgs e)
        {
            PLC.Test___Ready();
        }

        private void lblStart_DoubleClick(object sender, EventArgs e)
        {
            PLC.Test___Start();
        }

        private void lbl__End_DoubleClick(object sender, EventArgs e)
        {
            PLC.Test__Finish();
        }

        private void lbl_Open_DoubleClick(object sender, EventArgs e)
        {
            PLC.Door____Open();
        }

        private void lblClose_DoubleClick(object sender, EventArgs e)
        {
            PLC.Door___Close();
        }

        private void lblOwner_Click(object sender, EventArgs e)
        {
            //FomDebug = new fomDebug(this);
          //  FomDebug.Show(); 
        }
    }
}