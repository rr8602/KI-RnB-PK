using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KI_RnB
{
    public partial class fomSetup : Form
    {
        fom_Main main = null;

        clsCurve Curve = new clsCurve();    //Curve List
        clsCurve Drive = new clsCurve();    //Model Curve List
        fom_Dist Fom_Dist = new fom_Dist();
        byte LockMode;

        private Bitmap bmp;

        private int Divider = 5;
        private int LineTime = 25;

        private float Read_SST;
        private float ReadLWgt;
        private float ReadRWgt;
        private float ReadLBrk;
        private float ReadRBrk;
        private float Read_SMT;
        private float Read_RPM;

        private int cal_Mode = 0;
        private float calValue = 0;
        
        public fomSetup()
        {
            InitializeComponent();

            #region Language
            if (PSet.OwnerS00 > PSet.Def_Lang)
            {
                this.Text = PSet.Lang_Set[0]; //KI&T RnBT 설정
                tpgMachine.Text = PSet.Lang_Set[1]; //장비 설정
                tpg_Info.Text = PSet.Lang_Set[2]; //차량 정보
                tpgDrive.Text = PSet.Lang_Set[3]; //드라이브 모드
                tpgParam.Text = PSet.Lang_Set[4]; //파라미터
                tpgCalibration.Text = PSet.Lang_Set[5]; //교정
                tpgOwner.Text = PSet.Lang_Set[6]; //관리자 설정

                gbx_Pswd.Text = PSet.Lang_Set[7]; //관리자 비밀번호
                lbl0__00.Text = PSet.Lang_Set[8]; //비밀번호
                chk_Pswd.Text = PSet.Lang_Set[9]; //보기
                gbx_Calc.Text = PSet.Lang_Set[10]; //교정
                lbl0__01.Text = PSet.Lang_Set[11]; //교정 주기
                gbxPrint.Text = PSet.Lang_Set[12]; //보고서
                gbxWBase.Text = PSet.Lang_Set[13]; //휠베이스 설정

                lbl0__02.Text = PSet.Lang_Set[14]; //홈포지션 거리
                lbl0__03.Text = PSet.Lang_Set[15]; //최속 거리
                lbl0__04.Text = PSet.Lang_Set[16]; //최대 거리

                gbx_Roll.Text = PSet.Lang_Set[17]; //롤 설정
                lbl0__05.Text = PSet.Lang_Set[18]; //전축-좌 롤 직경
                lbl0__06.Text = PSet.Lang_Set[19]; //기어비율
                lbl0__07.Text = PSet.Lang_Set[20]; //관성 모멘트
                lbl0__08.Text = PSet.Lang_Set[21]; //펄스
                lbl0__09.Text = PSet.Lang_Set[22]; //전축-우 롤 직경
                lbl0__10.Text = PSet.Lang_Set[23]; //기어비율
                lbl0__11.Text = PSet.Lang_Set[24]; //관성 모멘트
                lbl0__13.Text = PSet.Lang_Set[25]; //펄스
                lbl0__14.Text = PSet.Lang_Set[26]; //후축-좌 롤 직경
                lbl0__15.Text = PSet.Lang_Set[27]; //기어비율
                lbl0__16.Text = PSet.Lang_Set[28]; //관성 모멘트
                lbl0__17.Text = PSet.Lang_Set[29]; //펄스
                lbl0__18.Text = PSet.Lang_Set[30]; //후축-우 롤 직경
                lbl0__19.Text = PSet.Lang_Set[31]; //기어비율
                lbl0__20.Text = PSet.Lang_Set[32]; //관성 모멘트
                lbl0__21.Text = PSet.Lang_Set[33]; //펄스

                gbx_Comm.Text = "Communication"; //통신 설정
                lbl0__22.Text = PSet.Lang_Set[34]; //PLC IP
                lbl0__23.Text = PSet.Lang_Set[35]; //포트
                lbl0__24.Text = PSet.Lang_Set[36]; //Kimc V14U
                lbl0__25.Text = PSet.Lang_Set[37]; //포트
                lbl0__26.Text = PSet.Lang_Set[38]; //교정 인디게이터
                lbl0__27.Text = PSet.Lang_Set[39]; //포트
                lbl0__28.Text = PSet.Lang_Set[40]; //바코드 1
                lbl0__29.Text = PSet.Lang_Set[41]; //포트
                lbl0__30.Text = PSet.Lang_Set[42]; //DLC
                lbl0__31.Text = PSet.Lang_Set[43]; //포트
                lbl0__32.Text = PSet.Lang_Set[44]; //마이티 전축 좌
                lbl0__33.Text = PSet.Lang_Set[45]; //포트

                gbx_Form.Text = PSet.Lang_Set[54]; //화면 위치
                lbl0__42.Text = PSet.Lang_Set[55]; //메인화면 상단
                lbl0__43.Text = PSet.Lang_Set[56]; //메인화면 좌측
                lbl0__44.Text = PSet.Lang_Set[57]; //정보화면 상단
                lbl0__45.Text = PSet.Lang_Set[58]; //정보화면 좌측

                gbxMList.Text = PSet.Lang_Set[61]; //모델 리스트
                btnM_Add.Text = PSet.Lang_Set[62]; //추가
                btnMEdit.Text = PSet.Lang_Set[63]; //변경
                btnM_Del.Text = PSet.Lang_Set[64]; //삭제
                btnMSave.Text = PSet.Lang_Set[65]; //저장

                lbl0List.Text = PSet.Lang_Set[66]; //모델명
                lbl1List.Text = PSet.Lang_Set[67]; //차량 ID
                lbl2List.Text = PSet.Lang_Set[68]; //엔진
                lbl3List.Text = PSet.Lang_Set[69]; //트렌스미션
                lbl4List.Text = PSet.Lang_Set[70]; //ABS 타입
                lbl5List.Text = PSet.Lang_Set[71]; //드라이브 커브
                lbl6List.Text = PSet.Lang_Set[72]; //구동축
                lbl7List.Text = PSet.Lang_Set[73]; //휠베이스
                lbl8List.Text = PSet.Lang_Set[74]; //파라미터

                gbx_Dist.Text = PSet.Lang_Set[75]; //휠베이스 거리
                lblDistA.Text = PSet.Lang_Set[76]; //A
                lblDistB.Text = PSet.Lang_Set[77]; //B
                lblDistC.Text = PSet.Lang_Set[78]; //C
                lblDistD.Text = PSet.Lang_Set[79]; //D
                lblDistE.Text = PSet.Lang_Set[80]; //E
                lblDistF.Text = PSet.Lang_Set[81]; //F
                lblDistG.Text = PSet.Lang_Set[82]; //G
                lblDistH.Text = PSet.Lang_Set[83]; //H
                lblDistI.Text = PSet.Lang_Set[84]; //I
                lblDistJ.Text = PSet.Lang_Set[85]; //J
                btn_Edit.Text = PSet.Lang_Set[86]; //거리 변경

                gbxCurve.Text = PSet.Lang_Set[87]; //드라이브 커브
                btnC_New.Text = PSet.Lang_Set[88]; //등록
                btnCEdit.Text = PSet.Lang_Set[89]; //변경
                btnC_Del.Text = PSet.Lang_Set[90]; //삭제

                btnP_Add.Text = PSet.Lang_Set[91]; //추가
                btnPEdit.Text = PSet.Lang_Set[92]; //변경
                btnP_Del.Text = PSet.Lang_Set[93]; //삭제

                gbx_Std1.Text = PSet.Lang_Set[94]; //기본 설정
                lblP__00.Text = PSet.Lang_Set[95]; //사이드슬립 (m/km)
                lblP__01.Text = PSet.Lang_Set[96]; //속도계 (km/h)
                lblP__02.Text = PSet.Lang_Set[97]; //전축 끌림 (kg)
                lblP__03.Text = PSet.Lang_Set[98]; //후축 끌림 (kg)
                lblP__04.Text = PSet.Lang_Set[99]; //전축 제동력 (kg)
                lblP__05.Text = PSet.Lang_Set[100]; //후축 제동력 (kg)
                lblP__06.Text = PSet.Lang_Set[101]; //주차 제동 거리 (cm)
                lblP__07.Text = PSet.Lang_Set[102]; //전축 발란스 (%)
                lblP__08.Text = PSet.Lang_Set[103]; //후축 발란스 (%)
                lblP__09.Text = PSet.Lang_Set[104]; //전축/후축 발란스 (%)

                gbx_Std2.Text = PSet.Lang_Set[105]; //ECU 설정
                lblP__10.Text = PSet.Lang_Set[106]; //전축 좌 휠 속도 센서 (km/h)
                lblP__11.Text = PSet.Lang_Set[107]; //전축 우 휠 속도 센서 (km/h)
                lblP__12.Text = PSet.Lang_Set[108]; //후축 좌 휠 속도 센서 (km/h)
                lblP__13.Text = PSet.Lang_Set[109]; //후축 우 휠 속도 센서 (km/h)
                lblP__14.Text = PSet.Lang_Set[110]; //전축 ABS 제동력 감압 (kg)
                lblP__15.Text = PSet.Lang_Set[111]; //전축 ABS 제동력 증압 (kg)
                lblP__16.Text = PSet.Lang_Set[112]; //후축 ABS 제동력 감압 (kg)
                lblP__17.Text = PSet.Lang_Set[113]; //후축 ABS 제동력 증압 (kg)

                lblM__00.Text = PSet.Lang_Set[114]; //<= 값 <=
                lblM__01.Text = PSet.Lang_Set[115]; //<= 값 <=
                lblM__02.Text = PSet.Lang_Set[116]; //<= 값 <=
                lblM__03.Text = PSet.Lang_Set[117]; //<= 값 <=
                lblM__04.Text = PSet.Lang_Set[118]; //<= 값 <=
                lblM__05.Text = PSet.Lang_Set[119]; //<= 값 <=
                lblM__06.Text = PSet.Lang_Set[120]; //<= 값 <=
                lblM__07.Text = PSet.Lang_Set[121]; //<= 값 <=
                lblM__08.Text = PSet.Lang_Set[122]; //<= 값 <=
                lblM__09.Text = PSet.Lang_Set[123]; //<= 값 <=
                lblM__10.Text = PSet.Lang_Set[124]; //<= 값 <=
                lblM__11.Text = PSet.Lang_Set[125]; //<= 값 <=
                lblM__12.Text = PSet.Lang_Set[126]; //<= 값 <=
                lblM__13.Text = PSet.Lang_Set[127]; //<= 값 <=
                lblM__14.Text = PSet.Lang_Set[128]; //<= 값 <=
                lblM__15.Text = PSet.Lang_Set[129]; //<= 값 <=
                lblM__16.Text = PSet.Lang_Set[130]; //<= 값 <=
                lblM__17.Text = PSet.Lang_Set[131]; //<= 값 <=
                
                tpg__SST.Text = PSet.Lang_Set[132]; //사이드슬립
                tpg__WGT.Text = PSet.Lang_Set[133]; //축중
                tpgBrake.Text = PSet.Lang_Set[134]; //제동력
                tpg__SMT.Text = PSet.Lang_Set[135]; //속도계
                tpg_RnBT.Text = PSet.Lang_Set[136]; //RnBT

                btn_In10.Text = PSet.Lang_Set[137]; //IN 10
                btn_In_5.Text = PSet.Lang_Set[138]; //IN 5
                btn_Zero.Text = PSet.Lang_Set[139]; //영점
                btn_Ot_5.Text = PSet.Lang_Set[140]; //OUT 5
                btn_Ot10.Text = PSet.Lang_Set[141]; //OUT 10

                btnWgtL0.Text = PSet.Lang_Set[144]; //축중 영점
                btnWgtR0.Text = PSet.Lang_Set[145]; //축중 영점
                btnWgtLC.Text = PSet.Lang_Set[146]; //축중 교정
                btnWgtRC.Text = PSet.Lang_Set[147]; //축중 교정

                btnBrkL0.Text = PSet.Lang_Set[144]; //좌   영점
                btnBrkR0.Text = PSet.Lang_Set[145]; //우   영점
                btnBrkLC.Text = PSet.Lang_Set[146]; //좌   교정
                btnBrkRC.Text = PSet.Lang_Set[147]; //우   교정

                lblCLoss.Text = PSet.Lang_Set[148]; //1. 장비 손실 교정.
                lblCLoad.Text = PSet.Lang_Set[149]; //2. 장비 부하 교정.
                btn_Cal2.Text = PSet.Lang_Set[150]; //Loss
                btn_Cal3.Text = PSet.Lang_Set[151]; //Load

                lblP__18.Text = PSet.Lang_Set[167]; //Pedal Brake Force(kg)
                lblM__18.Text = PSet.Lang_Set[168]; //Max Graph
                lblP__19.Text = PSet.Lang_Set[169]; //전축 축중 (kg)
                lblM__19.Text = PSet.Lang_Set[170]; //<= 값 <=
                lblP__20.Text = PSet.Lang_Set[171]; //후축 축중 (kg)
                lblM__20.Text = PSet.Lang_Set[172]; //<= 값 <=
                lblP__21.Text = PSet.Lang_Set[173]; //전축 제동력 (%)
                lblM__21.Text = PSet.Lang_Set[174]; //후축 제동력 (%)
                lblP__22.Text = PSet.Lang_Set[175]; //전체 제동력 (%)
                lblM__22.Text = PSet.Lang_Set[176]; //주차 제동력 (%)
                lblP__23.Text = PSet.Lang_Set[177]; //좌 / 우 편차 (%)
                lblM__23.Text = PSet.Lang_Set[178]; //끌림 (%)
                lblP__24.Text = PSet.Lang_Set[179]; //축중 측정 시간 (sec)
                lblM__24.Text = PSet.Lang_Set[180]; //제동중 측정 시간 (sec)
                lblP__25.Text = PSet.Lang_Set[181]; //끌림 측정 시간 (sec)
                lblM__25.Text = PSet.Lang_Set[182]; //주차 측정 시간 (sec)

                gbxBrake.Text = "Conventional brake";   //제동력 설정
                lbl0__34.Text = "Weight capacity";      //축중 용량
                lbl0__35.Text = "Brake capacity";       //제동력 용량
                lbl0__36.Text = "Minimum weight";       //축중 최소 하중
                lbl0__37.Text = "Braking force calibration ratio"; //교정 비율
                lbl0__38.Text = "Safety Load";          //축중 안정화 하중
                lbl0__39.Text = "number of retests";    //재측정 횟수
            }
            #endregion
            
            cboULoad.Items.Clear();
            cboULoad.Items.Add("kg");

            cboUSped.Items.Clear();
            cboUSped.Items.Add("km/h");

            cboUDist.Items.Clear();
            cboUDist.Items.Add("cm");
            
            cbo_ECUs.Items.Clear();
            cbo_ECUs.Items.Add("None");
            cbo_ECUs.Items.Add(ECUs.Mobis___AD);
            cbo_ECUs.Items.Add(ECUs.Mobis__DN8);
            cbo_ECUs.Items.Add(ECUs.Mobis___FL);
            cbo_ECUs.Items.Add(ECUs.Mando___TL);
            cbo_ECUs.Items.Add(ECUs.Mando___TM);
            cbo_ECUs.Items.Add(ECUs.Mando__HEV);
            //250416 NX4추가
            cbo_ECUs.Items.Add(ECUs.Mando_NX4H);
            cbo_ECUs.Items.Add(ECUs.Mando_NX4I);
            //250710 LX3추가
            cbo_ECUs.Items.Add(ECUs.Mobis_LX3H);
            cbo_ECUs.Items.Add(ECUs.Mobis_LX3I);
            cbo_ECUs.Items.Add(ECUs.Chery_1box);
        }

        public fomSetup(byte pMode, fom_Main main)
            : this()
        {
            LockMode = pMode;
            this.main = main;
        }

        private void fomSetup_Load(object sender, EventArgs e)
        {
            PSet.OnfSetup = true;
            
            if (PSet.Onf_Stop && main.Fom_Stop != null) { main.Fom_Stop.Close(); }

            this.Top = 0;
            this.Left = 0;
            this.Width = 1024;
            this.Height = 768;

            tab_Calc.Controls.RemoveByKey("tpg__SST");
            //tab_Calc.Controls.RemoveByKey("tpg__WGT");
            //tab_Calc.Controls.RemoveByKey("tpgBrake");
            tab_Calc.Controls.RemoveByKey("tpg__SMT");

            #region Owner Setting
            cboM_Drv.Items.Clear();
            cboM_Drv.Items.Add("Mity");
            cboM_Drv.Items.Add("Nidec + PLC");
            cboM_Drv.Items.Add("Nidec + Socket");

            cbo_Lang.Items.Clear();
            cbo_Lang.Items.Add("Korean");
            cbo_Lang.Items.Add("English");

            cbo_Own1.Items.Clear();
            cbo_Own1.Items.Add("F-L Wheel");
            cbo_Own1.Items.Add("F-R Wheel");
            cbo_Own1.Items.Add("R-L Wheel");
            cbo_Own1.Items.Add("R-R Wheel");
            cbo_Own1.Items.Add("Front Axle Average");
            cbo_Own1.Items.Add("Rear Axle Average");
            cbo_Own1.Items.Add("All Wheel Average");

            cbo_Own2.Items.Clear();
            cbo_Own2.Items.Add("교정 수식 적용");
            cbo_Own2.Items.Add("계산 수식 적용");

            cbo_Own3.Items.Clear();
            cbo_Own3.Items.Add("Not Use");
            cbo_Own3.Items.Add("Use");

            cbo_Own5.Items.Clear();
            cbo_Own5.Items.Add("Use");
            cbo_Own5.Items.Add("Not Use");

            cbo_Own6.Items.Clear();
            cbo_Own6.Items.Add("Use");
            cbo_Own6.Items.Add("Not Use");

            cbo_Own7.Items.Clear();
            cbo_Own7.Items.Add("Use");
            cbo_Own7.Items.Add("Not Use");

            cbo_Own8.Items.Clear();
            cbo_Own8.Items.Add("Use");
            cbo_Own8.Items.Add("Not Use");

            cbo_Own9.Items.Clear();
            cbo_Own9.Items.Add("Use");
            cbo_Own9.Items.Add("Not Use");

            cbo_OwnA.Items.Clear();
            cbo_OwnA.Items.Add("Use");
            cbo_OwnA.Items.Add("Not Use");

            cbo_OwnB.Items.Clear();
            cbo_OwnB.Items.Add("Use");
            cbo_OwnB.Items.Add("Not Use");

            cbo_OwnC.Items.Clear();
            cbo_OwnC.Items.Add("Use");
            cbo_OwnC.Items.Add("Not Use");

            cbo__SST.Items.Clear();     //0:측정 않음, 1:막대 그래프, 2:숫자만
            cbo__SST.Items.Add("Not Use");
            cbo__SST.Items.Add("Graph");
            cbo__SST.Items.Add("Value");

            cbo__Brk.Items.Clear();
            cbo__Brk.Items.Add("Not Use");
            cbo__Brk.Items.Add("Use");
            
            cbo_Door.Items.Clear();
            cbo_Door.Items.Add("Not Use");
            cbo_Door.Items.Add("Use");

            cboPedal.Items.Clear();
            cboPedal.Items.Add("Not Use");
            cboPedal.Items.Add("Use");

            cbo_File.Items.Clear();
            cbo_File.Items.Add("(Text(crv) 파일");
            cbo_File.Items.Add("Excel(xlsx) 파일");
            cbo_File.Items.Add("MDB");
            #endregion

            cboDrive.Items.Clear();
            cboDrive.Items.Add(PSet.Lang_Set[152]); //전륜구동
            cboDrive.Items.Add(PSet.Lang_Set[153]); //후륜구동
            cboDrive.Items.Add(PSet.Lang_Set[154]); //상시사륜

            cboPrint.Items.Clear();
            cboPrint.Items.Add(PSet.Lang_Set[155]);//수동 인쇄
            cboPrint.Items.Add(PSet.Lang_Set[156]);//자동 인쇄 

            picGraph.Image = Init_Graph(picGraph);
            picDrive.Image = Init_Graph(picDrive);

            List_D_Curve();
            ProgSet_Show();
            AllModelList();
            CarParameter();

            lbl2Scan.Visible = LockMode != 1 ? false : true;
            lbl3Scan.Visible = LockMode != 1 ? false : true;
            lbl4Scan.Visible = LockMode != 1 ? false : true;
            lbl5Scan.Visible = LockMode != 1 ? false : true;
            btnClear.Visible = LockMode != 1 ? false : true;

            lstCurve.Top = LockMode != 1 ? 18 : 83;
            lstCurve.Left = LockMode != 1 ? 3 : 3;
            lstCurve.Width = LockMode != 1 ? 253 : 253;
            lstCurve.Height = LockMode != 1 ? 303 : 238;
            lstCurve.Dock = LockMode != 1 ? DockStyle.Fill : DockStyle.Bottom;
            lstCurve.BringToFront();

            if (LockMode != 1)
            {
                tab_Sett.Controls.RemoveByKey("tpgOwner");
            }

            tmrSetup.Enabled = true;
        }
        private void fomSetup_FormClosed(object sender, FormClosedEventArgs e)
        {
            PSet.OnfSetup = false;
            tmrSetup.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            main.FomFlash.Play(0);
            this.Close();
        }

        #region Calibration
        private void tmrSetup_Tick(object sender, EventArgs e)
        {
            if (tab_Sett.SelectedTab.Name == "tpgCalibration" && tab_Calc.SelectedTab.Name != "tpg_RnBT")
            {
                main.FomFlash.Play(99);

                switch (tab_Calc.SelectedTab.Name)
                {
                    case "tpg__SST": SST_Cal_Show(); break;
                    case "tpg__WGT": WGT_Cal_Data(); break;
                    case "tpgBrake": Brk_Cal_Show(); break;
                    case "tpg__SMT": SMT_Cal_Show(); break;
                }

                Screen__Copy();
            }
            else
            {
                main.FomFlash.Play(0);
            }
        }

        private void Screen__Copy()
        {
            int top = this.Top + tab_Sett.Top + tpgCalibration.Top + tab_Calc.Top + tpg__SST.Top + 38;
            int left = this.Left + tab_Sett.Left + tpgCalibration.Left + tab_Calc.Left + tpg__SST.Left + 16;

            Bitmap bmp = new Bitmap(952, 521);
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.CopyFromScreen(new Point(left, top), new Point(0, 0), new Size(1024, 768));
            }

            main.FomFlash.picImage.Image = bmp;
        }

        private Bitmap Level___Show(PictureBox pBox, float pCapa, float pValue)
        {
            //Bitmap bmp = new Bitmap(pBox.Width, pBox.Height);
            Bitmap bmp = new Bitmap(Properties.Resources.Level);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float bmp_w = bmp.Width;
                float bmp_h = bmp.Height;
                float point = 0;
                float value = bmp_h - (pValue / pCapa * bmp_h);

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
            }

            return bmp;
        }

        private void SST_Cal_Show()
        {
            if (TSet.SimulOnf)
            {
                Read_SST = TSet.VirtualSST;
            }
            else
            {
                Read_SST = PSet.CH0_Val;
            }

            pic__SST.Image = SST_Cal_Show(pic__SST, Read_SST);
        }
        private Bitmap SST_Cal_Show(PictureBox pBox, float pValue)
        {
            Bitmap bmp = new Bitmap(Properties.Resources.SST_Cal);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                string str_Sine = "";
                float bmp_w = bmp.Width;
                float x = 106;
                float y = 25;
                float width = 752.0F;
                float height = 73.0F;
                float In_Scale = 366.5f / 10;
                float OutScale = 366.5f / 10;

                Font drawFont = new Font("굴림", 50, FontStyle.Bold);
                SolidBrush drawBrush = new SolidBrush(Color.White);
                StringFormat drawFormat = new StringFormat();
                RectangleF drawRect = new RectangleF(x, y, width, height);

                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString("Sideslip Calibration", drawFont, drawBrush, drawRect, drawFormat);

                SolidBrush redPen = new SolidBrush(Color.Red);
                SolidBrush greenPen = new SolidBrush(Color.Lime);
                SolidBrush yellowPen = new SolidBrush(Color.Yellow);
                SolidBrush blackPen = new SolidBrush(Color.Black);

                g.FillRectangle(redPen, 109, 355, 732, 21);
                //g.FillRectangle(yellowPen, 292, 355, 366, 21);
                //g.FillRectangle(greenPen, 364.2f, 355f, 219.6f, 21f);
                g.FillRectangle(blackPen, 474, 355, 2, 21);

                Image pin;
                PointF pinPoint;
                if (pValue <= 0)
                {
                    str_Sine = "IN ";
                    pin = Image.FromFile(@"Image\Pin-Green.gif");
                    pinPoint = new PointF(456.5f + (In_Scale * pValue), 276);
                }
                else
                {
                    str_Sine = "OUT ";
                    pin = Image.FromFile(@"Image\Pin-Green.gif");
                    pinPoint = new PointF(456.5f + (OutScale * pValue), 276);
                }
                g.DrawImage(pin, pinPoint);

                bmp_w = bmp.Width;
                x = 10;
                y = 380;
                width = 932.0F;
                height = 150.0F;

                drawFont = new Font("굴림", 80, FontStyle.Bold);
                drawBrush = new SolidBrush(Color.Yellow);
                drawFormat = new StringFormat();
                drawRect = new RectangleF(x, y, width, height);

                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString(str_Sine + (Math.Abs(pValue)).ToString("#0.0"), drawFont, drawBrush, drawRect, drawFormat);
            }

            return bmp;
        }

        private void WGT_Cal_Data()
        {
            if (TSet.SimulOnf)
            {
                ReadLWgt = TSet.VirtualL_W;
                ReadRWgt = TSet.VirtualR_W;
            }
            else
            {
                ReadLWgt = PSet.CH2_Val;
                ReadRWgt = PSet.CH3_Val;
            }

            picL_Wgt.Image = Level___Show(picL_Wgt, PSet.WGT_Capa, ReadLWgt);
            picR_Wgt.Image = Level___Show(picR_Wgt, PSet.WGT_Capa, ReadRWgt);

            pic__WGT.Image = WGT_Cal_Show(pic__WGT, ReadLWgt, ReadRWgt);

            lbl2Scan.Text = "L = (" + PSet.CH2Scan.ToString() + "-" + PSet.CH2Zero.ToString() + ") X " + PSet.CH2Span.ToString();
            lbl3Scan.Text = "R = (" + PSet.CH3Scan.ToString() + "-" + PSet.CH3Zero.ToString() + ") X " + PSet.CH3Span.ToString();
        }
        private Bitmap WGT_Cal_Show(PictureBox pBox, float L_Wgt, float R_Wgt)
        {
            Bitmap bmp = new Bitmap(Properties.Resources.Cal);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float bmp_w = bmp.Width;
                float x = 106;
                float y = 25;
                float width = 752.0F;
                float height = 73.0F;

                Font drawFont = new Font("굴림", 50, FontStyle.Bold);
                SolidBrush drawBrush = new SolidBrush(Color.White);
                StringFormat drawFormat = new StringFormat();
                RectangleF drawRect = new RectangleF(x, y, width, height);

                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString("Weight Calibration", drawFont, drawBrush, drawRect, drawFormat);

                bmp_w = bmp.Width;
                x = 240;
                y = 300;
                width = 270.0F;
                height = 120.0F;

                drawFont = new Font("굴림", 50, FontStyle.Bold);
                drawBrush = new SolidBrush(Color.Yellow);
                drawFormat = new StringFormat();
                drawRect = new RectangleF(x, y, width, height);

                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString(L_Wgt.ToString("#0"), drawFont, drawBrush, drawRect, drawFormat);

                x = 460;
                y = 300;
                width = 270.0F;
                height = 120.0F;

                drawRect = new RectangleF(x, y, width, height);
                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString(R_Wgt.ToString("#0"), drawFont, drawBrush, drawRect, drawFormat);

                x = 106;
                y = 360;
                width = 752.0F;
                height = 150.0F;

                drawFont = new Font("굴림", 60, FontStyle.Bold);
                drawBrush = new SolidBrush(Color.White);
                drawRect = new RectangleF(x, y, width, height);
                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString("kg", drawFont, drawBrush, drawRect, drawFormat);

                for (int cnt = 0; cnt <= Divider; cnt++)    //좌측 중량계
                {
                    x = 10;
                    y = (100 + 369) - (369 / Divider * (cnt));
                    width = 120.0F;
                    height = 25.0F;

                    drawFont = new Font("굴림", 20, FontStyle.Bold);
                    drawBrush = new SolidBrush(Color.White);
                    drawRect = new RectangleF(x, y, width, height);
                    drawFormat.Alignment = StringAlignment.Far;
                    g.DrawString((cnt * (PSet.WGT_Capa / Divider)).ToString("#0"), drawFont, drawBrush, drawRect, drawFormat);
                }

                for (int cnt = 0; cnt <= Divider; cnt++)    //우측 중량계
                {
                    x = 820;
                    y = (100 + 369) - (369 / Divider * (cnt));
                    width = 120.0F;
                    height = 25.0F;

                    drawFont = new Font("굴림", 20, FontStyle.Bold);
                    drawBrush = new SolidBrush(Color.White);
                    drawRect = new RectangleF(x, y, width, height);
                    drawFormat.Alignment = StringAlignment.Near;
                    g.DrawString((cnt * (PSet.WGT_Capa / Divider)).ToString("#0"), drawFont, drawBrush, drawRect, drawFormat);
                }
            }

            return bmp;
        }

        private void Brk_Cal_Show()
        {
            if (TSet.SimulOnf)
            {
                ReadLBrk = TSet.VirtualL_B;
                ReadRBrk = TSet.VirtualR_B;
            }
            else
            {
                ReadLBrk = PSet.CH4_Val;
                ReadRBrk = PSet.CH5_Val;
            }

            lblLIndi.Text = (ReadLBrk / PSet.BrkRatio).ToString("#0");
            lblRIndi.Text = (ReadRBrk / PSet.BrkRatio).ToString("#0");

            picL_Brk.Image = Level___Show(picL_Brk, PSet.BRK_Capa, ReadLBrk);
            picR_Brk.Image = Level___Show(picR_Brk, PSet.BRK_Capa, ReadRBrk);
            picBrake.Image = Brk_Cal_Show(picBrake, ReadLBrk, ReadRBrk);

            lbl4Scan.Text = "L = (" + PSet.CH4Scan.ToString() + "-" + PSet.CH4Zero.ToString() + ") X " + PSet.CH4Span.ToString();
            lbl5Scan.Text = "R = (" + PSet.CH5Scan.ToString() + "-" + PSet.CH5Zero.ToString() + ") X " + PSet.CH5Span.ToString();
        }
        private Bitmap Brk_Cal_Show(PictureBox pBox, float L_Brk, float R_Brk)
        {
            Bitmap bmp = new Bitmap(Properties.Resources.Cal);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float bmp_w = bmp.Width;
                float x = 106;
                float y = 25;
                float width = 752.0F;
                float height = 73.0F;

                Font drawFont = new Font("굴림", 50, FontStyle.Bold);
                SolidBrush drawBrush = new SolidBrush(Color.White);
                StringFormat drawFormat = new StringFormat();
                RectangleF drawRect = new RectangleF(x, y, width, height);

                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString("Brake Calibration", drawFont, drawBrush, drawRect, drawFormat);

                bmp_w = bmp.Width;
                x = 240;
                y = 300;
                width = 270.0F;
                height = 120.0F;

                drawFont = new Font("굴림", 50, FontStyle.Bold);
                drawBrush = new SolidBrush(Color.Yellow);
                drawFormat = new StringFormat();
                drawRect = new RectangleF(x, y, width, height);

                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString(L_Brk.ToString("#0"), drawFont, drawBrush, drawRect, drawFormat);

                x = 460;
                y = 300;
                width = 270.0F;
                height = 120.0F;

                drawRect = new RectangleF(x, y, width, height);
                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString(R_Brk.ToString("#0"), drawFont, drawBrush, drawRect, drawFormat);

                x = 106;
                y = 360;
                width = 752.0F;
                height = 150.0F;

                drawFont = new Font("굴림", 60, FontStyle.Bold);
                drawBrush = new SolidBrush(Color.White);
                drawRect = new RectangleF(x, y, width, height);
                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString("kg", drawFont, drawBrush, drawRect, drawFormat);

                for (int cnt = 0; cnt <= Divider; cnt++)    //좌측 중량계
                {
                    x = 10;
                    y = (100 + 369) - (369 / Divider * (cnt));
                    width = 120.0F;
                    height = 25.0F;

                    drawFont = new Font("굴림", 20, FontStyle.Bold);
                    drawBrush = new SolidBrush(Color.White);
                    drawRect = new RectangleF(x, y, width, height);
                    drawFormat.Alignment = StringAlignment.Far;
                    g.DrawString((cnt * (PSet.BRK_Capa / Divider)).ToString("#0"), drawFont, drawBrush, drawRect, drawFormat);
                }

                for (int cnt = 0; cnt <= Divider; cnt++)    //우측 중량계
                {
                    x = 820;
                    y = (100 + 369) - (369 / Divider * (cnt));
                    width = 120.0F;
                    height = 25.0F;

                    drawFont = new Font("굴림", 20, FontStyle.Bold);
                    drawBrush = new SolidBrush(Color.White);
                    drawRect = new RectangleF(x, y, width, height);
                    drawFormat.Alignment = StringAlignment.Near;
                    g.DrawString((cnt * (PSet.BRK_Capa / Divider)).ToString("#0"), drawFont, drawBrush, drawRect, drawFormat);
                }
            }

            return bmp;
        }

        private void SMT_Cal_Show()
        {
            if (TSet.SimulOnf)
            {
                Read_SMT = TSet.VirtualSMT;
                Read_RPM = TSet.VirtualRPM;
            }
            else
            {

                Read_SMT = main.ABSBoard.Speed[0];
                Read_RPM = main.ABSBoard.R_RPM[0];
            }

            pic__SMT.Image = SMT_Cal_Show(pic__SMT, Read_SMT, Read_RPM);
        }
        private Bitmap SMT_Cal_Show(PictureBox pBox, double pSpd, double pRPM)
        {
            Bitmap bmp = new Bitmap(Properties.Resources.Cal);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float bmp_w = bmp.Width;
                float x = 106;
                float y = 25;
                float width = 752.0F;
                float height = 73.0F;

                Font drawFont = new Font("굴림", 50, FontStyle.Bold);
                SolidBrush drawBrush = new SolidBrush(Color.White);
                StringFormat drawFormat = new StringFormat();
                RectangleF drawRect = new RectangleF(x, y, width, height);

                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString("속도계 확인", drawFont, drawBrush, drawRect, drawFormat);

                bmp_w = bmp.Width;
                x = 45;
                y = 180;
                width = 875.0F;
                height = 180.0F;

                drawFont = new Font("굴림", 120, FontStyle.Bold);
                drawBrush = new SolidBrush(Color.Yellow);
                drawFormat = new StringFormat();
                drawRect = new RectangleF(x, y, width, height);

                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString(pSpd.ToString("#0.0"), drawFont, drawBrush, drawRect, drawFormat);

                x = 106;
                y = 360;
                width = 752.0F;
                height = 150.0F;

                drawFont = new Font("굴림", 80, FontStyle.Bold);
                drawBrush = new SolidBrush(Color.White);
                drawRect = new RectangleF(x, y, width, height);
                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString("km/h", drawFont, drawBrush, drawRect, drawFormat);

                x = 45;
                y = 115;
                width = 875.0F;
                height = 150.0F;

                drawFont = new Font("굴림", 30, FontStyle.Bold);
                drawBrush = new SolidBrush(Color.White);
                drawRect = new RectangleF(x, y, width, height);
                drawFormat.Alignment = StringAlignment.Near;
                //g.DrawString("RPM : " + pRPM.ToString("#0"), drawFont, drawBrush, drawRect, drawFormat);
            }

            return bmp;
        }

        private void RnB_Calibration_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btn_Cal2": ((fom_Main)this.Owner).Loss_Calibration(); break;
                case "btn_Cal3": ((fom_Main)this.Owner).Load_Calibration(); break;
            }
        }

        private void SST_Calibration_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btn_In10": PSet.CH0Span = H2Y.DVD(-10, PSet.CH0Last); break;
                case "btn_In_5": PSet.CH0Span = H2Y.DVD(-5, PSet.CH0Last); break;
                case "btn_Zero": PSet.CH0Zero = PSet.CH0Scan; break;
                case "btn_Ot_5": PSet.CH0Span = H2Y.DVD(5, PSet.CH0Last); break;
                case "btn_Ot10": PSet.CH0Span = H2Y.DVD(10, PSet.CH0Last); break;
            }

            PSet.Prog_CalMake();
        }

        private void Wgt_Calibration_Click(object sender, EventArgs e)
        {
            Control_Flag(false);
            ((Button)sender).BackColor = Color.White;

            cal_Mode = 0;
            switch (((Button)sender).Name)
            {
                case "btnWgtL0": 
                    if (!H2Y.Question("Do you want to set the zero point?", "Left weight")) { break; }
                    PSet.CH2Zero = PSet.CH2Scan; 
                    break;

                case "btnWgtR0":
                    if (!H2Y.Question("Do you want to set the zero point?", "Right weight")) { break; }
                    PSet.CH3Zero = PSet.CH3Scan; 
                    break;

                case "btnWgtLC": cal_Mode = 2; calValue = PSet.CH2_Val; break;
                case "btnWgtRC": cal_Mode = 3; calValue = PSet.CH3_Val; break;

                case "btnWgtUp":
                    if (!H2Y.Question("Do you want to climb the lift?", "Lift Up")) { break; }
                    PLC.Brk_Lift__Up();
                    break;

                case "btnWgtDn":
                    if (!H2Y.Question("Do you want to descend the lift?", "Lift Down")) { break; }
                    PLC.Brk_LiftDown();
                    break;
            }

            if (cal_Mode == 2 || cal_Mode == 3)
            {
                //H2Y.Msg_Speash("현재 수치를 " + calValue + "로 교정하니다.");
                fomInput input = new fomInput(cal_Mode, calValue.ToString());
                input.ShowDialog();
            }

            PSet.Prog_CalMake();

            Control_Flag(true);
            ((Button)sender).BackColor = Color.Transparent;
        }
        private void Brk_Calibration_Click(object sender, EventArgs e)
        {
            Control_Flag(false);
            ((Button)sender).BackColor = Color.White;

            cal_Mode = 0;
            switch (((Button)sender).Name)
            {
                case "btnBrkL0":
                    if (!H2Y.Question("Do you want to set the zero point?", "Left brake")) { break; }
                    PSet.CH4Zero = PSet.CH4Scan; 
                    break;

                case "btnBrkR0":
                    if (!H2Y.Question("Do you want to set the zero point?", "Right brake")) { break; }
                    PSet.CH5Zero = PSet.CH5Scan; 
                    break;

                case "btnBrkLC": cal_Mode = 4; calValue = PSet.CH4_Val; break;
                case "btnBrkRC": cal_Mode = 5; calValue = PSet.CH5_Val; break;

                case "btnBrkUp":
                    if (!H2Y.Question("Do you want to climb the lift?", "Lift Up")) { break; }
                    PLC.Brk_Lift__Up();
                    break;

                case "btnBrkDn":
                    if (!H2Y.Question("Do you want to descend the lift?", "Lift Down")) { break; }
                    PLC.Brk_LiftDown();
                    break;

                case "btnMotor":
                    if (btnMotor.Text == "Motor On")
                    {
                        if (!H2Y.Question("Do you want to run the motor?", "BT Motor")) { break; }
                        PLC.Brk_MotorRun(1);
                        btnMotor.ForeColor = Color.Red;
                        btnMotor.Text = "Motor Off";
                    }
                    else
                    {
                        if (!H2Y.Question("Do you want to stop the motor?", "BT Motor")) { break; }
                        PLC.Brk_MotorRun(0);
                        btnMotor.ForeColor = Color.Black;
                        btnMotor.Text = "Motor On";
                    }

                    break;
            }

            if (cal_Mode == 4 || cal_Mode == 5)
            {
                //H2Y.Msg_Speash("현재 수치를 " + calValue + "로 교정하니다.");
                fomInput input = new fomInput(cal_Mode, calValue.ToString());
                input.ShowDialog();
            }

            PSet.Prog_CalMake();

            Control_Flag(true);
            ((Button)sender).BackColor = Color.Transparent;
        }

        private void Control_Flag(bool Onf)
        {
            btnWgtL0.Enabled = Onf; btnWgtR0.Enabled = Onf;
            btnWgtLC.Enabled = Onf; btnWgtRC.Enabled = Onf;
            btnWgtUp.Enabled = Onf; btnWgtDn.Enabled = Onf;

            btnBrkL0.Enabled = Onf; btnBrkR0.Enabled = Onf;
            btnBrkLC.Enabled = Onf; btnBrkRC.Enabled = Onf;
            btnBrkUp.Enabled = Onf; btnBrkDn.Enabled = Onf;
            btnMotor.Enabled = Onf; 
        }
        #endregion

        #region Program Setting Save/Show
        private void btn_Save_Click(object sender, EventArgs e)
        {   //"데이터를 저장하시겠습니까?", 저장
            if (!H2Y.Question(PSet.Lang_Set[157], PSet.Lang_Set[158])) { return; }

            PSet.Passwd = txt_Pswd.Text;                    //Password
            PSet.CalCyc = Ret_IntValue(txtCal_C.Text);      //Calibration Cycle
            PSet.sPrint = cboPrint.SelectedIndex;           //Print Mode

            #region Wheelbase distance
            PSet.WB_Min = Ret_IntValue(txt_WMin.Text);      //Wheelbase Min.
            PSet.WB_Max = Ret_IntValue(txt_WMax.Text);      //Wheelbase Max.
            //PSet.WB_Ofs = Ret_IntValue(txt_WOfs.Text);      //Wheelbase Off set
            #endregion

            #region Roll Drum & Encoder
            PSet.RFLDia = Ret_IntValue(txt_D_FL.Text);      //Roll FL Diameter
            PSet.RFLPul = Ret_IntValue(txt_P_FL.Text);      //Roll FL Pulse
            PSet.FL_MRatio = Ret_DblValue(txt_G_FL.Text);   //Motor to Roller ratio (기어비)
            PSet.FL_Moment = Ret_DblValue(txt_M_FL.Text);   //Moment of inertia(관성 모멘트)

            PSet.RFRDia = Ret_IntValue(txt_D_FR.Text);      //Roll FR Diameter
            PSet.RFRPul = Ret_IntValue(txt_P_FR.Text);      //Roll FR Pulse
            PSet.FR_MRatio = Ret_DblValue(txt_G_FR.Text);   //Motor to Roller ratio (기어비)
            PSet.FR_Moment = Ret_DblValue(txt_M_FR.Text);   //Moment of inertia(관성 모멘트)

            PSet.RRLDia = Ret_IntValue(txt_D_RL.Text);      //Roll RL Diameter
            PSet.RRLPul = Ret_IntValue(txt_P_RL.Text);      //Roll RL Pulse
            PSet.RL_MRatio = Ret_DblValue(txt_G_RL.Text);   //Motor to Roller ratio (기어비)
            PSet.RL_Moment = Ret_DblValue(txt_M_RL.Text);   //Moment of inertia(관성 모멘트)

            PSet.RRRDia = Ret_IntValue(txt_D_RR.Text);      //Roll RR Diameter
            PSet.RRRPul = Ret_IntValue(txt_P_RR.Text);      //Roll RR Pulse
            PSet.RR_MRatio = Ret_DblValue(txt_G_RR.Text);   //Motor to Roller ratio (기어비)
            PSet.RR_Moment = Ret_DblValue(txt_M_RR.Text);   //Moment of inertia(관성 모멘트)
            #endregion

            #region Communication
            PSet.PLC__S = txt_PLC1.Text;                    //PLC Setting
            PSet.PLC__P = Ret_IntValue(txt_PLC2.Text);      //PLC Port

            PSet.Ctrl_S = txt_CTR1.Text;                    //Controller Setting
            PSet.Ctrl_P = Ret_IntValue(txt_CTR2.Text);      //Controller Port

            PSet.Indi_S = txtIndi1.Text;                    //Indicator Setting
            PSet.Indi_P = Ret_IntValue(txtIndi2.Text);      //Indicator Port

            PSet.MDrv_S = txt_DLC1.Text;                    //Motor Drive Setting
            PSet.MDrv_P = Ret_IntValue(txt_DLC2.Text);      //Motor Drive Port

            PSet.BarC1S = txt1Bar1.Text;                    //Barcode 1 Setting
            PSet.BarC1P = Ret_IntValue(txt1Bar2.Text);      //Barcode 1 Port

            PSet.PedalS = txt_Pdl1.Text;                    //Pedal Brake Setting
            PSet.PedalP = Ret_IntValue(txt_Pdl2.Text);      //Pedal Brake Port
            #endregion

            #region Brake Test
            PSet.WGT_Capa = Ret_IntValue(txt_Wgt0.Text);    //축중   용량 kg
            PSet.WGTLimit = Ret_IntValue(txt_Wgt1.Text);    //축중   최저 kg
            PSet.WGT_Safe = Ret_IntValue(txt_Wgt2.Text);    //축중   안정 kg

            PSet.BRK_Capa = Ret_IntValue(txt_Brk0.Text);    //제동력 용량 kg
            PSet.BrkRatio = Ret_SngValue(txt_Brk1.Text);    //제동력 교정 배율 (%)
            PSet.BRKCount = Ret_IntValue(txt_Brk2.Text);    //제동력 재측정
            #endregion

            #region Owner Setting
            PSet.OwnerS00 = cbo_Lang.SelectedIndex;     //언어 선택
            PSet.OwnerS01 = cbo_Own1.SelectedIndex;     //기준 속도 설정
            PSet.OwnerS02 = cbo_Own2.SelectedIndex;     //끌림 수식 설정
            PSet.OwnerS03 = cbo_Own3.SelectedIndex;     //RED(시리얼번호) 사용 설정
            PSet.OwnerS04 = H2Y.StrToInt(txt_Own4.Text);//RED(시리얼번호)

            PSet.OwnerS05 = cbo_Own5.SelectedIndex;     //Drag Judge
            PSet.OwnerS06 = cbo_Own6.SelectedIndex;     //Brake Judge
            PSet.OwnerS07 = cbo_Own7.SelectedIndex;     //Parking Judge
            PSet.OwnerS08 = cbo_Own8.SelectedIndex;     //Speedometer Judge
            PSet.OwnerS09 = cbo_Own9.SelectedIndex;     //Balance Judge
            PSet.OwnerS0A = cbo_OwnA.SelectedIndex;     //WSS Judge
            PSet.OwnerS0B = cbo_OwnB.SelectedIndex;     //Decrease Judge
            PSet.OwnerS0C = cbo_OwnC.SelectedIndex;     //Increase Judge

            PSet.SST_Type = cbo__SST.SelectedIndex;     //0:측정 않음, 1:막대 그래프, 2:숫자만
            PSet.Brk_Type = cbo__Brk.SelectedIndex;     //0:측정 않음, 1:측정
            PSet.Use_Door = cbo_Door.SelectedIndex;     //0:사용 않음, 1:사용

            PSet.OwnerDrv = cboM_Drv.SelectedIndex;     //Motor Drive
            int tmpI; float tmpF;
            PSet.OwnerSpd = int.TryParse(txtCSped.Text, out tmpI) ? tmpI : PSet.OwnerSpd;
            PSet.OwnerToq = int.TryParse(txtCTorq.Text, out tmpI) ? tmpI : PSet.OwnerToq;
            PSet.OwnerPBS = int.TryParse(txtCPark.Text, out tmpI) ? tmpI : PSet.OwnerPBS;

            PSet.OwnerSFL = float.TryParse(txtWSS_0.Text, out tmpF) ? tmpF : PSet.OwnerSFL;
            PSet.OwnerSFR = float.TryParse(txtWSS_1.Text, out tmpF) ? tmpF : PSet.OwnerSFR;
            PSet.OwnerSRL = float.TryParse(txtWSS_2.Text, out tmpF) ? tmpF : PSet.OwnerSRL;
            PSet.OwnerSRR = float.TryParse(txtWSS_3.Text, out tmpF) ? tmpF : PSet.OwnerSRR;

            PSet.Owner_FL = int.TryParse(txtWSSFL.Text, out tmpI) ? tmpI : PSet.Owner_FL;
            PSet.Owner_FR = int.TryParse(txtWSSFR.Text, out tmpI) ? tmpI : PSet.Owner_FR;
            PSet.Owner_RL = int.TryParse(txtWSSRL.Text, out tmpI) ? tmpI : PSet.Owner_RL;
            PSet.Owner_RR = int.TryParse(txtWSSRR.Text, out tmpI) ? tmpI : PSet.Owner_RR;

            PSet.PLC_GapT = int.TryParse(txt_GapT.Text, out tmpI) ? tmpI : PSet.PLC_GapT;
            PSet.CNT_Stop = int.TryParse(txtS_CNT.Text, out tmpI) ? tmpI : PSet.CNT_Stop;

            PSet.OwnerPdl = cboPedal.SelectedIndex;     //Pedal Brake
            PSet.OwnerCrv = cbo_File.SelectedIndex;     //드라이브 커브 파일 서렁

            PSet.Print__X = int.TryParse(txt_XPos.Text, out tmpI) ? tmpI : (int)PSet.Print__X;
            PSet.Print__Y = int.TryParse(txt_YPos.Text, out tmpI) ? tmpI : (int)PSet.Print__Y;
            #endregion
            
            PSet.Prog_SetMake();

            #region Screen Size
            PSet.siz_Main.Top = Ret_IntValue(txtTMain.Text);
            PSet.siz_Main.Left = Ret_IntValue(txtLMain.Text);
            PSet.siz_Main.Width = Ret_IntValue("1024");
            PSet.siz_Main.Height = Ret_IntValue("768");

            PSet.siz__Sub.Top = Ret_IntValue(txtTInfo.Text);
            PSet.siz__Sub.Left = Ret_IntValue(txtLInfo.Text);
            PSet.siz__Sub.Width = Ret_IntValue("1024");
            PSet.siz__Sub.Height = Ret_IntValue("768");
            
            PSet.Ini_SizeMake();
            #endregion

            ProgSet_Show();

            MessageBoxEx.Show(PSet.Lang_Set[159]); //"저장되었습니다."
        }
        private int Ret_IntValue(string pVal)
        {
            try
            {
                int val;
                if (!int.TryParse(pVal, out val)) return 0;
                return val;
            }
            catch
            {
                return 0;
            }
        }
        private double Ret_DblValue(string pVal)
        {
            try
            {
                if (pVal == "") return 0;

                return Convert.ToDouble(pVal);
            }
            catch
            {
                return 0;
            }
        }
        private Single Ret_SngValue(string pVal)
        {
            try
            {
                if (pVal == "") return 0;

                return Convert.ToSingle(pVal);
            }
            catch
            {
                return 0;
            }
        }
        
        private void ProgSet_Show()
        {
            if (PSet.Prog_SetRead())    //장비 설정
            {
                txt_Pswd.Text = PSet.Passwd.ToString(); //Password
                txtCal_C.Text = PSet.CalCyc.ToString(); //Calibration Cycle
                if (PSet.sPrint >= 0 && PSet.sPrint < cboPrint.Items.Count) cboPrint.SelectedIndex = PSet.sPrint;   //Print Mode

                #region Wheelbase distance
                txt_WMin.Text = PSet.WB_Min.ToString(); //Wheelbase Min.
                txt_WMax.Text = PSet.WB_Max.ToString(); //Wheelbase Max.
                txt_WOfs.Text = PSet.WB_Ofs.ToString(); //Wheelbase Off set
                #endregion

                #region Roll Drum & Encoder
                txt_D_FL.Text = PSet.RFLDia.ToString(); //Roll FL Diameter
                txt_P_FL.Text = PSet.RFLPul.ToString(); //Roll FL Pulse
                txt_G_FL.Text = PSet.FL_MRatio.ToString(); //Motor to Roller ratio (기어비)
                txt_M_FL.Text = PSet.FL_Moment.ToString(); //Moment of inertia(관성 모멘트)

                txt_D_FR.Text = PSet.RFRDia.ToString(); //Roll FR Diameter
                txt_P_FR.Text = PSet.RFRPul.ToString(); //Roll FR Pulse
                txt_G_FR.Text = PSet.FR_MRatio.ToString(); //Motor to Roller ratio (기어비)
                txt_M_FR.Text = PSet.FR_Moment.ToString(); //Moment of inertia(관성 모멘트)

                txt_D_RL.Text = PSet.RRLDia.ToString(); //Roll RL Diameter
                txt_P_RL.Text = PSet.RRLPul.ToString(); //Roll RL Pulse
                txt_G_RL.Text = PSet.RL_MRatio.ToString(); //Motor to Roller ratio (기어비)
                txt_M_RL.Text = PSet.RL_Moment.ToString(); //Moment of inertia(관성 모멘트)

                txt_D_RR.Text = PSet.RRRDia.ToString(); //Roll RR Diameter
                txt_P_RR.Text = PSet.RRRPul.ToString(); //Roll RR Pulse
                txt_G_RR.Text = PSet.RR_MRatio.ToString(); //Motor to Roller ratio (기어비)
                txt_M_RR.Text = PSet.RR_Moment.ToString(); //Moment of inertia(관성 모멘트)
                #endregion

                #region Communication
                txt_PLC1.Text = PSet.PLC__S.ToString(); //PLC Setting
                txt_PLC2.Text = PSet.PLC__P.ToString(); //PLC Port

                txt_CTR1.Text = PSet.Ctrl_S.ToString(); //Controller Setting
                txt_CTR2.Text = PSet.Ctrl_P.ToString(); //Controller Port

                txtIndi1.Text = PSet.Indi_S.ToString(); //Indicator Setting
                txtIndi2.Text = PSet.Indi_P.ToString(); //Indicator Port

                txt_DLC1.Text = PSet.MDrv_S.ToString(); //Motor Drive Setting
                txt_DLC2.Text = PSet.MDrv_P.ToString(); //Motor Drive Port

                txt1Bar1.Text = PSet.BarC1S.ToString(); //Barcode 1 Setting
                txt1Bar2.Text = PSet.BarC1P.ToString(); //Barcode 1 Port

                txt_Pdl1.Text = PSet.PedalS.ToString(); //Pedal Brake Setting
                txt_Pdl2.Text = PSet.PedalP.ToString(); //Pedal Brake Port
                #endregion

                #region Brake Test
                txt_Wgt0.Text = PSet.WGT_Capa.ToString();   //축중   용량 kg
                txt_Wgt1.Text = PSet.WGTLimit.ToString();   //축중   최저 kg
                txt_Wgt2.Text = PSet.WGT_Safe.ToString();   //축중   안정 kg

                txt_Brk0.Text = PSet.BRK_Capa.ToString();   //제동력 용량 kg
                txt_Brk1.Text = PSet.BrkRatio.ToString();   //제동력 교정 배율 (%)
                txt_Brk2.Text = PSet.BRKCount.ToString();   //제동력 재측정
                #endregion

                #region Owner Setting
                if (PSet.OwnerS00 >= 0 && PSet.OwnerS00 < cbo_Lang.Items.Count) cbo_Lang.SelectedIndex = PSet.OwnerS00;     //언어 선택
                if (PSet.OwnerS01 >= 0 && PSet.OwnerS01 < cbo_Own1.Items.Count) cbo_Own1.SelectedIndex = PSet.OwnerS01;     //기준 속도 설정
                if (PSet.OwnerS02 >= 0 && PSet.OwnerS02 < cbo_Own2.Items.Count) cbo_Own2.SelectedIndex = PSet.OwnerS02;     //끌림 수식 설정
                if (PSet.OwnerS03 >= 0 && PSet.OwnerS03 < cbo_Own3.Items.Count) cbo_Own3.SelectedIndex = PSet.OwnerS03;     //RED(시리얼번호) 사용 설정
                txt_Own4.Text = PSet.OwnerS04.ToString();   //RED(시리얼번호)

                if (PSet.OwnerS05 >= 0 && PSet.OwnerS05 < cbo_Own5.Items.Count) cbo_Own5.SelectedIndex = PSet.OwnerS05;     //Drag Judge
                if (PSet.OwnerS06 >= 0 && PSet.OwnerS06 < cbo_Own6.Items.Count) cbo_Own6.SelectedIndex = PSet.OwnerS06;     //Brake Judge
                if (PSet.OwnerS07 >= 0 && PSet.OwnerS07 < cbo_Own7.Items.Count) cbo_Own7.SelectedIndex = PSet.OwnerS07;     //Parking Judge
                if (PSet.OwnerS08 >= 0 && PSet.OwnerS08 < cbo_Own8.Items.Count) cbo_Own8.SelectedIndex = PSet.OwnerS08;     //Speedometer Judge
                if (PSet.OwnerS09 >= 0 && PSet.OwnerS09 < cbo_Own9.Items.Count) cbo_Own9.SelectedIndex = PSet.OwnerS09;     //Balance Judge
                if (PSet.OwnerS0A >= 0 && PSet.OwnerS0A < cbo_OwnA.Items.Count) cbo_OwnA.SelectedIndex = PSet.OwnerS0A;     //WSS Judge
                if (PSet.OwnerS0B >= 0 && PSet.OwnerS0B < cbo_OwnB.Items.Count) cbo_OwnB.SelectedIndex = PSet.OwnerS0B;     //Decrease Judge
                if (PSet.OwnerS0C >= 0 && PSet.OwnerS0C < cbo_OwnC.Items.Count) cbo_OwnC.SelectedIndex = PSet.OwnerS0C;     //Increase Judge

                if (PSet.SST_Type >= 0 && PSet.SST_Type < cbo__SST.Items.Count) cbo__SST.SelectedIndex = PSet.SST_Type;     //0:측정 않음, 1:막대 그래프, 2:숫자만
                if (PSet.Brk_Type >= 0 && PSet.Brk_Type < cbo__Brk.Items.Count) cbo__Brk.SelectedIndex = PSet.Brk_Type;     //0:측정 않음, 1:측정
                if (PSet.Use_Door >= 0 && PSet.Use_Door < cbo_Door.Items.Count) cbo_Door.SelectedIndex = PSet.Use_Door;     //0:사용 않음, 1:사용

                if (PSet.OwnerDrv >= 0 && PSet.OwnerDrv < cboM_Drv.Items.Count) cboM_Drv.SelectedIndex = PSet.OwnerDrv;     //Motor Drive
                txtCSped.Text = PSet.OwnerSpd.ToString();   //Calibration Speed
                txtCTorq.Text = PSet.OwnerToq.ToString();   //Parking     Torque
                txtCPark.Text = PSet.OwnerPBS.ToString();   //Parking     Speed

                txtWSS_0.Text = PSet.OwnerSFL.ToString();   //WSS Speed FL (km/h)
                txtWSS_1.Text = PSet.OwnerSFR.ToString();   //WSS Speed FR (km/h)
                txtWSS_2.Text = PSet.OwnerSRL.ToString();   //WSS Speed RL (km/h)
                txtWSS_3.Text = PSet.OwnerSRR.ToString();   //WSS Speed RR (km/h)

                txtWSSFL.Text = PSet.Owner_FL.ToString();   //WSS Speed FL (RPM)
                txtWSSFR.Text = PSet.Owner_FR.ToString();   //WSS Speed FR (RPM)
                txtWSSRL.Text = PSet.Owner_RL.ToString();   //WSS Speed RL (RPM)
                txtWSSRR.Text = PSet.Owner_RR.ToString();   //WSS Speed RR (RPM)

                if (PSet.OwnerPdl >= 0 && PSet.OwnerPdl < cboPedal.Items.Count) cboPedal.SelectedIndex = PSet.OwnerPdl;     //Pedal Brake
                if (PSet.OwnerCrv >= 0 && PSet.OwnerCrv < cbo_File.Items.Count) cbo_File.SelectedIndex = PSet.OwnerCrv;     //드라이브 커브 파일 선택

                txt_XPos.Text = PSet.Print__X.ToString();   //보고서 X Offset
                txt_YPos.Text = PSet.Print__Y.ToString();   //보고서 Y Offset

                pnlM_Drv.Visible = PSet.OwnerDrv > 0 ? true : false;

                txt_GapT.Text = PSet.PLC_GapT.ToString();
                txtS_CNT.Text = PSet.CNT_Stop.ToString();
                #endregion
                
                if (PLC.IsOpen)
                {
                    txt1Dist.Text = (PLC.OfSetL + PLC.Dist_A).ToString();  //Distance A
                    txt2Dist.Text = (PLC.OfSetL + PLC.Dist_B).ToString();  //Distance B
                    txt3Dist.Text = (PLC.OfSetL + PLC.Dist_C).ToString();  //Distance C
                    txt4Dist.Text = (PLC.OfSetL + PLC.Dist_D).ToString();  //Distance D
                    txt5Dist.Text = (PLC.OfSetL + PLC.Dist_E).ToString();  //Distance E
                    txt6Dist.Text = (PLC.OfSetL + PLC.Dist_F).ToString();  //Distance F
                    txt7Dist.Text = (PLC.OfSetL + PLC.Dist_G).ToString();  //Distance G
                    txt8Dist.Text = (PLC.OfSetL + PLC.Dist_H).ToString();  //Distance H
                    txt9Dist.Text = (PLC.OfSetL + PLC.Dist_I).ToString();  //Distance I
                    txtADist.Text = (PLC.OfSetL + PLC.Dist_J).ToString();  //Distance J
                }
                else
                {
                    txt1Dist.Text = (PSet.WB_Ofs + PSet.Lent_A).ToString();  //Distance A
                    txt2Dist.Text = (PSet.WB_Ofs + PSet.Lent_B).ToString();  //Distance B
                    txt3Dist.Text = (PSet.WB_Ofs + PSet.Lent_C).ToString();  //Distance C
                    txt4Dist.Text = (PSet.WB_Ofs + PSet.Lent_D).ToString();  //Distance D
                    txt5Dist.Text = (PSet.WB_Ofs + PSet.Lent_E).ToString();  //Distance E
                    txt6Dist.Text = (PSet.WB_Ofs + PSet.Lent_F).ToString();  //Distance F
                    txt7Dist.Text = (PSet.WB_Ofs + PSet.Lent_G).ToString();  //Distance G
                    txt8Dist.Text = (PSet.WB_Ofs + PSet.Lent_H).ToString();  //Distance H
                    txt9Dist.Text = (PSet.WB_Ofs + PSet.Lent_I).ToString();  //Distance I
                    txtADist.Text = (PSet.WB_Ofs + PSet.Lent_J).ToString();  //Distance J
                }
            }

            #region Screen Size
            PSet.Ini_SizeRead();    //화면 설정

            txtTMain.Text = PSet.siz_Main.Top.ToString();
            txtLMain.Text = PSet.siz_Main.Left.ToString();
            //cls_PSet.siz_Main.Width = Ret_IntValue("1024");
            //cls_PSet.siz_Main.Height = Ret_IntValue("768");

            txtTInfo.Text = PSet.siz__Sub.Top.ToString();
            txtLInfo.Text = PSet.siz__Sub.Left.ToString(); 
            //cls_PSet.siz_Info.Width = Ret_IntValue("1024");
            //cls_PSet.siz_Info.Height = Ret_IntValue("768");
            #endregion

            WheelBaseSet();
        }
        #endregion

        #region Vehicle Infomation
        private void btnModel_Click(object sender, EventArgs e)
        {
            string btn_Name = ((Button)sender).Name;
            string strModel = txtModel.Text;
            
            switch (btn_Name)
            {
                case "btnM_Add": Add_ModelList(strModel);   break;
                case "btnMEdit": EditModelList(strModel);   break;
                case "btnM_Del": Del_ModelList(strModel);   break;
                case "btn_Edit": Fom_Dist = new fom_Dist();
                                 Fom_Dist.ShowDialog();
                                 ProgSet_Show();            break;
                //case "btnMSave": Del_ModelList(strModel); break;
            }
        }

        private void AllModelList()
        {
            dgvModel.DataSource = main.DB_All.DBModel.Search();
            dgvModel.RowHeadersVisible = true;

            System.Windows.Forms.DataGridViewCellStyle CellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            CellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            if (dgvModel.Columns.Count == 10)
            {
                dgvModel.Columns[0].DefaultCellStyle = CellStyle;
                dgvModel.Columns[1].DefaultCellStyle = CellStyle;
                dgvModel.Columns[2].DefaultCellStyle = CellStyle;
                dgvModel.Columns[3].DefaultCellStyle = CellStyle;
                dgvModel.Columns[4].DefaultCellStyle = CellStyle;
                dgvModel.Columns[5].DefaultCellStyle = CellStyle;
                dgvModel.Columns[6].DefaultCellStyle = CellStyle;
                dgvModel.Columns[7].DefaultCellStyle = CellStyle;
                dgvModel.Columns[8].DefaultCellStyle = CellStyle;
                dgvModel.Columns[9].DefaultCellStyle = CellStyle;

                dgvModel.Columns[0].HeaderText = PSet.Lang_Set[66]; //모델명
                dgvModel.Columns[1].HeaderText = "ECU Model"; //ECU
                dgvModel.Columns[2].HeaderText = PSet.Lang_Set[67]; //차량 ID
                dgvModel.Columns[3].HeaderText = PSet.Lang_Set[68]; //엔진
                dgvModel.Columns[4].HeaderText = PSet.Lang_Set[69]; //트렌스미션
                dgvModel.Columns[5].HeaderText = PSet.Lang_Set[70]; //ABS 타입
                dgvModel.Columns[6].HeaderText = PSet.Lang_Set[71]; //드라이브 커브
                dgvModel.Columns[7].HeaderText = PSet.Lang_Set[72]; //구동축
                dgvModel.Columns[8].HeaderText = PSet.Lang_Set[73]; //휠베이스
                dgvModel.Columns[9].HeaderText = PSet.Lang_Set[74]; //파라미터
            }
        }

        private void Add_ModelList(string pModel)
        {
            if (main.DB_All.DBModel.Barcode(txt_BarC.Text.Substring(0, 4)) == 1)
            {
                MessageBoxEx.Show("Change the barcode");
                txt_BarC.Focus();
                return;
            }
            main.DB_All.DBModel.dbCarIndex = (main.DB_All.DBModel.Number() + 1).ToString();
            main.DB_All.DBModel.dbCarModel = txtModel.Text;
            main.DB_All.DBModel.dbECUModel = cbo_ECUs.Text;
            main.DB_All.DBModel.dbCarBarID = txt_BarC.Text;
            main.DB_All.DBModel.dbCarEngin = txtEngin.Text;
            main.DB_All.DBModel.dbCarTranM = txtTranM.Text;
            main.DB_All.DBModel.dbCar_ABST = txt_ABST.Text;
            main.DB_All.DBModel.dbCarCurve = cboCurve.Text;
            main.DB_All.DBModel.dbCarDrive = cboDrive.Text;
            main.DB_All.DBModel.dbCarWbase = cboWBase.Text;
            main.DB_All.DBModel.dbCarParam = cboParam.Text;
            main.DB_All.DBModel.dbBalance = chk_balance.Checked ? "Y" : "N";
            main.DB_All.DBModel.Insert();

            AllModelList();

            MessageBoxEx.Show(PSet.Lang_Set[160]);  //등록 되었습니다.
        }
        private void EditModelList(string pModel)
        {
            if (main.DB_All.DBModel.Select(pModel) == 1)
            {
                main.DB_All.DBModel.dbCarIndex = main.DB_All.DBModel.dbCarIndex;
                main.DB_All.DBModel.dbCarModel = txtModel.Text;
                main.DB_All.DBModel.dbECUModel = cbo_ECUs.Text;
                main.DB_All.DBModel.dbCarBarID = txt_BarC.Text;
                main.DB_All.DBModel.dbCarEngin = txtEngin.Text;
                main.DB_All.DBModel.dbCarTranM = txtTranM.Text;
                main.DB_All.DBModel.dbCar_ABST = txt_ABST.Text;
                main.DB_All.DBModel.dbCarCurve = cboCurve.Text;
                main.DB_All.DBModel.dbCarDrive = cboDrive.Text;
                main.DB_All.DBModel.dbCarWbase = cboWBase.Text;
                main.DB_All.DBModel.dbCarParam = cboParam.Text;
                main.DB_All.DBModel.dbBalance = chk_balance.Checked ? "Y" : "N";

                main.DB_All.DBModel.Update(pModel);
            }
            AllModelList();

            MessageBoxEx.Show(PSet.Lang_Set[161]);  //변경 되었습니다.
        }
        private void Del_ModelList(string pModel)
        {
            main.DB_All.DBModel.Delete(pModel);

            AllModelList();

            MessageBoxEx.Show(PSet.Lang_Set[162]);  //삭제 되었습니다.
        }

        private void cboCurve_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Drive.Get_DriveCurve(cboCurve.Text))
            {
                picDrive.Image = Data_Graph(picDrive, Drive, -1);
            }
        }
        
        private void dgvModel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int SelectRow = e.RowIndex;

                SelectModel(SelectRow);
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "dgvModel_CellClick: " + ex.Message);
            }
        }
        private void dgvModel_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                int SelectRow = ((DataGridView)sender).CurrentRow.Index;

                SelectModel(SelectRow);
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "dgvModel_CurrentCellChanged: " + ex.Message);
            }
        }
        private void SelectModel(int idx)
        {
            try
            {
                txtModel.Text = dgvModel[0, idx].Value.ToString();
                cbo_ECUs.Text = dgvModel[1, idx].Value.ToString();
                txt_BarC.Text = dgvModel[2, idx].Value.ToString();
                txtEngin.Text = dgvModel[3, idx].Value.ToString();
                txtTranM.Text = dgvModel[4, idx].Value.ToString();
                txt_ABST.Text = dgvModel[5, idx].Value.ToString();
                cboCurve.Text = dgvModel[6, idx].Value.ToString();
                cboDrive.Text = dgvModel[7, idx].Value.ToString();
                cboWBase.Text = dgvModel[8, idx].Value.ToString();
                cboParam.Text = dgvModel[9, idx].Value.ToString();
                chk_balance.Checked = dgvModel[10, idx].Value.ToString().ToUpper() == "Y";
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "SelectModel: " + ex.Message);
            }
        }

        private void WheelBaseSet()
        {
            cboWBase.Items.Clear();
            cboWBase.Items.Add(txt1Dist.Text);
            cboWBase.Items.Add(txt2Dist.Text);
            cboWBase.Items.Add(txt3Dist.Text);
            cboWBase.Items.Add(txt4Dist.Text);
            cboWBase.Items.Add(txt5Dist.Text);
            cboWBase.Items.Add(txt6Dist.Text);
            cboWBase.Items.Add(txt7Dist.Text);
            cboWBase.Items.Add(txt8Dist.Text);
            cboWBase.Items.Add(txt9Dist.Text);
            cboWBase.Items.Add(txtADist.Text);
        }
        #endregion

        #region Drive Curve
        private void List_D_Curve()
        {
            Queue<string> lists = new Queue<string>();

            lstCurve.Items.Clear();
            cboCurve.Items.Clear();

            switch (PSet.OwnerCrv)
            {
                case 0: lists = Drive.Crv__FileList(); break;
                case 1: lists = Drive.xlsx_CurveList();
                        //lists = Drive.ExcelCurveList(); 속도가 느림
                        break;
                case 2: lists = Drive.MDB__FileList(); break;
            }

            foreach (string list in lists)
            {
                lstCurve.Items.Add(list);
                cboCurve.Items.Add(list);
            }
            if (cboCurve.Items.Count > -1)
            {
                //!lstCurve.SelectedIndex = 0;
            }
        }
                        
        private void btnCurve_Click(object sender, EventArgs e)
        {
            string btn_Name = ((Button)sender).Name;
            string Crv_Name = txtCurve.Text;

            this.Enabled = false;

            switch (btn_Name)
            {
                case "btnC_New": New_CurveList(Crv_Name); break;
                case "btnCEdit": EditCurveList(Crv_Name); break;
                case "btnC_Del": Del_CurveList(Crv_Name); break;
                case "btnClear": New_CurveList();         break;
            }

            List_D_Curve();

            this.Enabled = true;
        }

        private void New_CurveList()
        {
            dgvCurve.DataSource = null;
            picGraph.Image = Init_Graph(picGraph); 
        }
        private void New_CurveList(string cName)
        {
            fom_PsWd pass = new fom_PsWd(11);
            pass.ShowDialog();

            if (pass.NewModel != "")
            {
                fomCurve FomCurve = new fomCurve(main, pass.NewModel, LockMode);
                FomCurve.ShowDialog();
            }
        }
        private void EditCurveList(string cName)
        {
            fomCurve FomCurve = new fomCurve(main, cName, LockMode);
            FomCurve.ShowDialog();

            ReadCrv_Name(cName);
        }
        private void Del_CurveList(string cName)
        {
            if (cName == "Default")
            {
                //"Default는 삭제할수 없습니다.", "삭제 에러"
                MessageBox.Show(PSet.Lang_Set[163], PSet.Lang_Set[164]);
                return;
            }

            switch (PSet.OwnerCrv)
            {
                case 0: Drive.Crv__Curve_Del(cName); break;
                case 1: //Drive.xlsx_Curve_Del(cName);  
                        Drive.ExcelCurve_Del(cName); 
                        break;
                case 2: Drive.MDB__Curve_Del(cName); break;
            }

            txtCurve.Text = "";
            picGraph.Image = Init_Graph(picGraph);
        }

        private void lstCurve_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCurve.Text = "";
            if (lstCurve.SelectedItem == null) return;

            string Crv_Name = lstCurve.SelectedItem.ToString();

            txtCurve.Text = Crv_Name;
            ReadCrv_Name(Crv_Name);
        }
        private void ReadCrv_Name(string cName)
        {
            //dgvCurve.Rows[0].Selected = true;
            //dgvCurve.CurrentCell = dgvCurve.FirstDisplayedCell;

            double OfstTime = DateTime.Now.Ticks;
            double ReadTime = OfstTime;
            
            this.Enabled = false;

            dgvCurve.DataSource = null;
            picGraph.Image = Init_Graph(picGraph); 

            try
            {
                if (cName != "")
                {
                    if (Drive.Get_DriveCurve(cName))
                    {
                        dgvCurve.DataSource = Drive.G_Data;
                        dgvCurve.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvCurve.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvCurve.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                        dgvCurve.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

                        picGraph.Image = Data_Graph(picGraph, Drive, -1);
                    }

                    //if (tab_Sett.SelectedTab.Name == "tpgDrive")
                    //{
                    //    ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;

                    //    if (LockMode == 1)
                    //    {
                    //        MessageBoxEx.Show(PSet.Lang_Set[161] + " : " + ReadTime.ToString("#0.000"));  //변경 되었습니다.
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }

            this.Enabled = true;
        }

        private void dgvCurve_Click(object sender, EventArgs e)
        {
            if (dgvCurve.CurrentRow != null)
            {
                int Idx = dgvCurve.CurrentRow.Index;

                picGraph.Image = Data_Graph(picGraph, Drive, Idx);
            }
        }
        private void dgvCurve_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCurve.CurrentRow != null)
            {
                int Idx = dgvCurve.CurrentRow.Index;

                picGraph.Image = Data_Graph(picGraph, Drive, Idx);
            }
        }

        private Bitmap Init_Graph(PictureBox pic)
        {
            bmp = new Bitmap(pic.Width, pic.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float gap_l = 40;
                float gap_r = 20;
                float gap_t = 30;
                float gap_b = 40;
                float bmp_w = (bmp.Width - (gap_l + gap_r));
                float bmp_h = (bmp.Height - (gap_t + gap_b));

                float gap_w = bmp_w / LineTime;
                float gap_h = bmp_h / 8;

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

                for (int i = 0; i <= LineTime; i++)
                {
                    x = gap_l + (gap_w * i) - 15.0F;
                    y = gap_t + bmp_h + 10.0F;
                    width = 30.0F;
                    height = 20.0F;
                    drawRect = new RectangleF(x, y, width, height);

                    g.DrawLine(blackPen, gap_l + (gap_w * i), gap_t, gap_l + (gap_w * i), gap_t + bmp_h);
                    g.DrawString((i*10).ToString(), drawFont, drawBrush, drawRect, drawFormat);
                }

                drawFormat.Alignment = StringAlignment.Far;
                drawFormat.LineAlignment = StringAlignment.Center;

                for (int i = 0; i <= 8; i++)
                {
                    x =  0.0F;
                    y = gap_t + (gap_h * i) - 10.0F;
                    width = gap_l - 10.0F;
                    height = 20.0F;
                    drawRect = new RectangleF(x, y, width, height);

                    g.DrawLine(blackPen, gap_l, gap_t + (gap_h * i), gap_l + bmp_w, gap_t + (gap_h * i));
                    g.DrawString((160-(i * 20)).ToString(), drawFont, drawBrush, drawRect, drawFormat);
                }

                x = gap_l +10.0F;
                y = 10.0F;
                width = 200.0F;
                height = 20.0F;
                drawRect = new RectangleF(x, y, width, height);
                
                //Draw rectangle to screen
                //g.DrawRectangle(blackPen, x, y, width, height);

                drawFormat.Alignment = StringAlignment.Near;
                g.DrawString(PSet.Lang_Set[165], drawFont, drawBrush, drawRect, drawFormat); //"Vehicle Speed(km/h)"

                x = (bmp.Width/2) - 100.0F;
                y = gap_t + bmp_h + 20.0F;
                width = 200.0F;
                height = 20.0F;
                drawRect = new RectangleF(x, y, width, height);

                //Draw rectangle to screen
                //g.DrawRectangle(blackPen, x, y, width, height);

                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString(PSet.Lang_Set[166], drawFont, drawBrush, drawRect, drawFormat);    //"Time(sec)"
            }

            return bmp;
        }
        private Bitmap Data_Graph(PictureBox pic, clsCurve curve, int Idx)
        {
            Init_Graph(pic);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float gap_l = 40;
                float gap_r = 20;
                float gap_t = 30;
                float gap_b = 40;
                float bmp_w = (bmp.Width - (gap_l + gap_r));
                float bmp_h = (bmp.Height - (gap_t + gap_b));

                float gap_w = bmp_w / LineTime;
                float gap_h = bmp_h / 8;

                float x, o_x = gap_l;
                float y, o_y = (gap_t + bmp_h);
                float scal_X = bmp_w / (LineTime * 10);
                float scal_Y = bmp_h / 160;

                int CNT = curve.G_Data.Count - 1;
                Pen RedPen = new Pen(Color.Red, 2);
                Pen BluePen = new Pen(Color.Blue, 3);
                Pen GreenPen = new Pen(Color.Lime, 5);
                RectangleF drawRect;

                for (int i = 0; i <= CNT; i++)
                {
                    x = gap_l + (scal_X * curve.G_Data[i].T_Time);
                    y = (gap_t + bmp_h) - (scal_Y * curve.G_Data[i].Speed);

                    if (Idx != i)
                    {
                        g.DrawLine(RedPen, o_x, o_y, x, y);
                    }
                    else
                    {
                        g.DrawLine(GreenPen, o_x, o_y, x, y);
                    }

                    o_x = x; o_y = y;
                }

                for (int i = 0; i <= CNT; i++)
                {
                    x = gap_l + (scal_X * curve.G_Data[i].T_Time);
                    y = (gap_t + bmp_h) - (scal_Y * curve.G_Data[i].Speed);

                    drawRect = new RectangleF(x - 1.5f, y - 1.5f, 3, 3);
                    g.DrawEllipse(BluePen, drawRect);

                    o_x = x; o_y = y;
                }

                if (curve.G_Data.Count > 0)
                {
                    if (-1 < Idx & Idx < curve.G_Data.Count)
                    {
                        Pen PointPen = new Pen(Color.Green, 7);

                        x = gap_l + (scal_X * curve.G_Data[Idx].T_Time);
                        y = (gap_t + bmp_h) - (scal_Y * curve.G_Data[Idx].Speed);

                        drawRect = new RectangleF(x - 3.5f, y - 3.5f, 7, 7);
                        g.DrawEllipse(PointPen, drawRect);
                    }
                }
            }

            return bmp;
        }
        #endregion

        #region Parameter
        private void CarParameter()
        {
            DataTable dt = main.DB_All.DBParam.Search();

            if (dt.Rows != null)
            {
                lstParam.Items.Clear();
                cboParam.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    lstParam.Items.Add(dr["dbParamSeq"].ToString());
                    cboParam.Items.Add(dr["dbParamSeq"].ToString());
                }

                if (dt.Rows.Count > 0 )lstParam.SelectedIndex = 0;
            }
        }

        private void lstParam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstParam.SelectedIndex < 0) return;

            string strParam = lstParam.Items[lstParam.SelectedIndex].ToString();
            txtParam.Text = strParam;

            Params_Clear();

            if (main.DB_All.DBParam.Select(strParam) > 0)
            {
                #region RnB Setting
                txtSpeedMin.Text = main.DB_All.DBParam.dbParam001;  //속도계 min
                txtSpeedMax.Text = main.DB_All.DBParam.dbParam002;  //속도계 max
                txtSSlipMin.Text = main.DB_All.DBParam.dbParam003;  //SST    min
                txtSSlipMax.Text = main.DB_All.DBParam.dbParam004;  //SST    max

                txtPTSValue.Text = main.DB_All.DBParam.dbParam009;  //Pedal Brake Target Force(kg)
                txtPTSGraph.Text = main.DB_All.DBParam.dbParam010;  //Pedal Brake Max    Graph(kg)

                txtDragFMin.Text = main.DB_All.DBParam.dbParam011;  //전축 끌림 min
                txtDragFMax.Text = main.DB_All.DBParam.dbParam012;  //전축 끌림 max
                txtDragRMin.Text = main.DB_All.DBParam.dbParam013;  //후축 끌림 min
                txtDragRMax.Text = main.DB_All.DBParam.dbParam014;  //후축 끌림 max

                txtBrk_FMin.Text = main.DB_All.DBParam.dbParam021;  //전축 제동력 min
                txtBrk_FMax.Text = main.DB_All.DBParam.dbParam022;  //전축 제동력 max
                txtBrk_RMin.Text = main.DB_All.DBParam.dbParam023;  //후축 제동력 min
                txtBrk_RMax.Text = main.DB_All.DBParam.dbParam024;  //후축 제동력 max

                txtPark_Min.Text = main.DB_All.DBParam.dbParam029;  //주차 제동력 min (cm)
                txtPark_Max.Text = main.DB_All.DBParam.dbParam030;  //주차 제동력 max (cm)

                txtBal_FMin.Text = main.DB_All.DBParam.dbParam031;  //전축 발란스 min
                txtBal_FMax.Text = main.DB_All.DBParam.dbParam032;  //전축 발란스 max
                txtBal_RMin.Text = main.DB_All.DBParam.dbParam033;  //후축 발란스 min
                txtBal_RMax.Text = main.DB_All.DBParam.dbParam034;  //후축 발란스 max
                txtBal_AMin.Text = main.DB_All.DBParam.dbParam035;  //전체 발란스 min
                txtBal_AMax.Text = main.DB_All.DBParam.dbParam036;  //전체 발란스 max
                #endregion

                #region ECU Setting
                txtWSSFLMin.Text = main.DB_All.DBParam.dbParam051;  //WSS F-L Min
                txtWSSFLMax.Text = main.DB_All.DBParam.dbParam052;  //WSS F-L Max
                txtWSSFRMin.Text = main.DB_All.DBParam.dbParam053;  //WSS F-R Min
                txtWSSFRMax.Text = main.DB_All.DBParam.dbParam054;  //WSS F-R Max
                txtWSSRLMin.Text = main.DB_All.DBParam.dbParam055;  //WSS R-L Min
                txtWSSRLMax.Text = main.DB_All.DBParam.dbParam056;  //WSS R-L Max
                txtWSSRRMin.Text = main.DB_All.DBParam.dbParam057;  //WSS R-R Min
                txtWSSRRMax.Text = main.DB_All.DBParam.dbParam058;  //WSS R-R Max

                txtDec_FMin.Text = main.DB_All.DBParam.dbParam061;  //전축 감소(Dec) min
                txtDec_FMax.Text = main.DB_All.DBParam.dbParam062;  //전축 감소(Dec) max
                txtInc_FMin.Text = main.DB_All.DBParam.dbParam063;  //전축 증가(Inc) min
                txtInc_FMax.Text = main.DB_All.DBParam.dbParam064;  //전축 증가(Inc) max
                txtDec_RMin.Text = main.DB_All.DBParam.dbParam065;  //후축 감소(Dec) min
                txtDec_RMax.Text = main.DB_All.DBParam.dbParam066;  //후축 감소(Dec) max
                txtInc_RMin.Text = main.DB_All.DBParam.dbParam067;  //후축 증가(Inc) min
                txtInc_RMax.Text = main.DB_All.DBParam.dbParam068;  //후축 증가(Inc) max
                #endregion

                #region Normal Brake Setting
                txtWgt1_Min.Text = main.DB_All.DBParam.dbParam071;  //전축  축중 최소
                txtWgt1_Max.Text = main.DB_All.DBParam.dbParam072;  //전축  축중 최대
                txtWgt2_Min.Text = main.DB_All.DBParam.dbParam074;  //후축  축중 최소
                txtWgt2_Max.Text = main.DB_All.DBParam.dbParam075;  //후축  축중 최대
                txtWgt_Time.Text = main.DB_All.DBParam.dbParam078;  //축중 측정 시간

                txtBrk1_Min.Text = main.DB_All.DBParam.dbParam081;  //전축 제동력(%)
                txtBrk2_Min.Text = main.DB_All.DBParam.dbParam082;  //후축 제동력(%)
                txtDrag_Max.Text = main.DB_All.DBParam.dbParam083;  //끌림 제동력(%)
                txtDiff_Max.Text = main.DB_All.DBParam.dbParam084;  //편차 제동력(%)
                txtBrkA_Min.Text = main.DB_All.DBParam.dbParam085;  //  합 제동력(%)
                txtBrkP_Min.Text = main.DB_All.DBParam.dbParam086;  //주차 제동력(%)
                txtBrk_Time.Text = main.DB_All.DBParam.dbParam088;  //일반 제동력 측정 시간(sec)
                txtDragTime.Text = main.DB_All.DBParam.dbParam089;  //끌림 제동력 측정 시간(sec)
                txtParkTime.Text = main.DB_All.DBParam.dbParam090;  //주차 제동력 측정 시간(sec)
                #endregion
            }
        }

        private void btnParam_Click(object sender, EventArgs e)
        {
            string btn_Name = ((Button)sender).Name;
            string strParam = txtParam.Text;

            if (strParam.Trim() == "") return;

            switch (btn_Name)
            {
                case "btnP_Add": if (!H2Y.Question("Would you like to add " + strParam + " parameters?", "Parameter")) { return; } 
                    break;
                case "btnPEdit": if (!H2Y.Question("Would you like to change " + strParam + " parameters?", "Parameter")) { return; } 
                    break;
                case "btnP_Del": if (!H2Y.Question("Are you sure you want to delete " + strParam + " parameters?", "Parameter")) { return; } 
                    break;
            }

            switch (btn_Name)
            {
                case "btnP_Add": Add_ParamList(strParam); break;
                case "btnPEdit": EditParamList(strParam); break;
                case "btnP_Del": Del_ParamList(strParam); break;
            }
        }

        private void Add_ParamList(string pParam)
        {
            if (main.DB_All.DBParam.Select(pParam) == 0)
            {
                main.DB_All.DBParam.dbParamSeq = pParam;
                Param_Select();
                main.DB_All.DBParam.Insert();

                CarParameter();

                MessageBoxEx.Show(PSet.Lang_Set[160]);  //등록 되었습니다.
            }
            else
            {
                MessageBoxEx.Show("Change the parameter name and try again");  
            }
        }
        private void EditParamList(string pParam)
        {
            if (main.DB_All.DBParam.Select(pParam) == 1)
            {
                main.DB_All.DBParam.dbParamSeq = pParam;
                Param_Select();
                main.DB_All.DBParam.Update(pParam);
            }
            CarParameter();

            MessageBoxEx.Show(PSet.Lang_Set[161]);  //변경 되었습니다.
        }
        private void Del_ParamList(string pParam)
        {
            main.DB_All.DBParam.Delete(pParam);

            CarParameter();

            MessageBoxEx.Show(PSet.Lang_Set[162]);  //삭제 되었습니다.
        }

        private void Param_Select()
        {
            #region RnB Setting
            main.DB_All.DBParam.dbParam001 = txtSpeedMin.Text;  //속도계 min
            main.DB_All.DBParam.dbParam002 = txtSpeedMax.Text;  //속도계 max
            main.DB_All.DBParam.dbParam003 = txtSSlipMin.Text;  //SST    min
            main.DB_All.DBParam.dbParam004 = txtSSlipMax.Text;  //SST    max
            main.DB_All.DBParam.dbParam005 = "0";
            main.DB_All.DBParam.dbParam006 = "0";
            main.DB_All.DBParam.dbParam007 = "0";
            main.DB_All.DBParam.dbParam008 = "0";
            main.DB_All.DBParam.dbParam009 = txtPTSValue.Text;  //Pedal Brake Target Force(kg)
            main.DB_All.DBParam.dbParam010 = txtPTSGraph.Text;  //Pedal Brake Max    Graph(kg)

            main.DB_All.DBParam.dbParam011 = txtDragFMin.Text;  //전축 끌림 min
            main.DB_All.DBParam.dbParam012 = txtDragFMax.Text;  //전축 끌림 max
            main.DB_All.DBParam.dbParam013 = txtDragRMin.Text;  //후축 끌림 min
            main.DB_All.DBParam.dbParam014 = txtDragRMax.Text;  //후축 끌림 max
            main.DB_All.DBParam.dbParam015 = "0";
            main.DB_All.DBParam.dbParam016 = "0";
            main.DB_All.DBParam.dbParam017 = "0";
            main.DB_All.DBParam.dbParam018 = "0";
            main.DB_All.DBParam.dbParam019 = "0";
            main.DB_All.DBParam.dbParam020 = "0";

            main.DB_All.DBParam.dbParam021 = txtBrk_FMin.Text;  //전축 제동력 min
            main.DB_All.DBParam.dbParam022 = txtBrk_FMax.Text;  //전축 제동력 max
            main.DB_All.DBParam.dbParam023 = txtBrk_RMin.Text;  //후축 제동력 min
            main.DB_All.DBParam.dbParam024 = txtBrk_RMax.Text;  //후축 제동력 max
            main.DB_All.DBParam.dbParam025 = "0";
            main.DB_All.DBParam.dbParam026 = "0";
            main.DB_All.DBParam.dbParam027 = "0";
            main.DB_All.DBParam.dbParam028 = "0";
            main.DB_All.DBParam.dbParam029 = txtPark_Min.Text;  //주차 제동력 min (cm)
            main.DB_All.DBParam.dbParam030 = txtPark_Max.Text;  //주차 제동력 max (cm)

            main.DB_All.DBParam.dbParam031 = txtBal_FMin.Text;  //전축 발란스 min
            main.DB_All.DBParam.dbParam032 = txtBal_FMax.Text;  //전축 발란스 max
            main.DB_All.DBParam.dbParam033 = txtBal_RMin.Text;  //후축 발란스 min
            main.DB_All.DBParam.dbParam034 = txtBal_RMax.Text;  //후축 발란스 max
            main.DB_All.DBParam.dbParam035 = txtBal_AMin.Text;  //전체 발란스 min
            main.DB_All.DBParam.dbParam036 = txtBal_AMax.Text;  //전체 발란스 max
            main.DB_All.DBParam.dbParam037 = "0";
            main.DB_All.DBParam.dbParam038 = "0";
            main.DB_All.DBParam.dbParam039 = "0";
            main.DB_All.DBParam.dbParam040 = "0";

            main.DB_All.DBParam.dbParam041 = "0";
            main.DB_All.DBParam.dbParam042 = "0";
            main.DB_All.DBParam.dbParam043 = "0";
            main.DB_All.DBParam.dbParam044 = "0";
            main.DB_All.DBParam.dbParam045 = "0";
            main.DB_All.DBParam.dbParam046 = "0";
            main.DB_All.DBParam.dbParam047 = "0";
            main.DB_All.DBParam.dbParam048 = "0";
            main.DB_All.DBParam.dbParam049 = "0";
            main.DB_All.DBParam.dbParam040 = "0";
            #endregion

            #region ECU Setting
            main.DB_All.DBParam.dbParam051 = txtWSSFLMin.Text;  //WSS F-L Min
            main.DB_All.DBParam.dbParam052 = txtWSSFLMax.Text;  //WSS F-L Max
            main.DB_All.DBParam.dbParam053 = txtWSSFRMin.Text;  //WSS F-R Min
            main.DB_All.DBParam.dbParam054 = txtWSSFRMax.Text;  //WSS F-R Max
            main.DB_All.DBParam.dbParam055 = txtWSSRLMin.Text;  //WSS R-L Min
            main.DB_All.DBParam.dbParam056 = txtWSSRLMax.Text;  //WSS R-L Max
            main.DB_All.DBParam.dbParam057 = txtWSSRRMin.Text;  //WSS R-R Min
            main.DB_All.DBParam.dbParam058 = txtWSSRRMax.Text;  //WSS R-R Max
            main.DB_All.DBParam.dbParam059 = "0";
            main.DB_All.DBParam.dbParam060 = "0";

            main.DB_All.DBParam.dbParam061 = txtDec_FMin.Text;  //전축 감소(Dec) min
            main.DB_All.DBParam.dbParam062 = txtDec_FMax.Text;  //전축 감소(Dec) max
            main.DB_All.DBParam.dbParam063 = txtInc_FMin.Text;  //전축 증가(Inc) min
            main.DB_All.DBParam.dbParam064 = txtInc_FMax.Text;  //전축 증가(Inc) max
            main.DB_All.DBParam.dbParam065 = txtDec_RMin.Text;  //후축 감소(Dec) min
            main.DB_All.DBParam.dbParam066 = txtDec_RMax.Text;  //후축 감소(Dec) max
            main.DB_All.DBParam.dbParam067 = txtInc_RMin.Text;  //후축 증가(Inc) min
            main.DB_All.DBParam.dbParam068 = txtInc_RMax.Text;  //후축 증가(Inc) max
            main.DB_All.DBParam.dbParam069 = "0";
            main.DB_All.DBParam.dbParam070 = "0";
            #endregion

            #region Normal Brake Setting
            main.DB_All.DBParam.dbParam071 = txtWgt1_Min.Text;  //전축  축중 최소
            main.DB_All.DBParam.dbParam072 = txtWgt1_Max.Text;  //전축  축중 최대
            main.DB_All.DBParam.dbParam073 = "0";
            main.DB_All.DBParam.dbParam074 = txtWgt2_Min.Text;  //후축  축중 최소
            main.DB_All.DBParam.dbParam075 = txtWgt2_Max.Text;  //후축  축중 최대
            main.DB_All.DBParam.dbParam076 = "0";
            main.DB_All.DBParam.dbParam077 = "0";
            main.DB_All.DBParam.dbParam078 = txtWgt_Time.Text;  //축중 측정 시간
            main.DB_All.DBParam.dbParam079 = "0";
            main.DB_All.DBParam.dbParam080 = "0";

            main.DB_All.DBParam.dbParam081 = txtBrk1_Min.Text;  //전축 제동력(%)
            main.DB_All.DBParam.dbParam082 = txtBrk2_Min.Text;  //후축 제동력(%)
            main.DB_All.DBParam.dbParam083 = txtDrag_Max.Text;  //끌림 제동력(%)
            main.DB_All.DBParam.dbParam084 = txtDiff_Max.Text;  //편차 제동력(%)
            main.DB_All.DBParam.dbParam085 = txtBrkA_Min.Text;  //  합 제동력(%)
            main.DB_All.DBParam.dbParam086 = txtBrkP_Min.Text;  //주차 제동력(%)
            main.DB_All.DBParam.dbParam087 = "0";
            main.DB_All.DBParam.dbParam088 = txtBrk_Time.Text;  //일반 제동력 측정 시간(sec)
            main.DB_All.DBParam.dbParam089 = txtDragTime.Text;  //끌림 제동력 측정 시간(sec)
            main.DB_All.DBParam.dbParam090 = txtParkTime.Text;  //주차 제동력 측정 시간(sec)
            #endregion

            #region Other Setting
            main.DB_All.DBParam.dbParam091 = "0";
            main.DB_All.DBParam.dbParam092 = "0";
            main.DB_All.DBParam.dbParam093 = "0";
            main.DB_All.DBParam.dbParam094 = "0";
            main.DB_All.DBParam.dbParam095 = "0";
            main.DB_All.DBParam.dbParam096 = "0";
            main.DB_All.DBParam.dbParam097 = "0";
            main.DB_All.DBParam.dbParam098 = "0";
            main.DB_All.DBParam.dbParam099 = "0";
            main.DB_All.DBParam.dbParam100 = "0";

            main.DB_All.DBParam.dbParam101 = "0";
            main.DB_All.DBParam.dbParam102 = "0";
            main.DB_All.DBParam.dbParam103 = "0";
            main.DB_All.DBParam.dbParam104 = "0";
            main.DB_All.DBParam.dbParam105 = "0";
            main.DB_All.DBParam.dbParam106 = "0";
            main.DB_All.DBParam.dbParam107 = "0";
            main.DB_All.DBParam.dbParam108 = "0";
            main.DB_All.DBParam.dbParam109 = "0";
            main.DB_All.DBParam.dbParam110 = "0";

            main.DB_All.DBParam.dbParam111 = "0";
            main.DB_All.DBParam.dbParam112 = "0";
            main.DB_All.DBParam.dbParam113 = "0";
            main.DB_All.DBParam.dbParam114 = "0";
            main.DB_All.DBParam.dbParam115 = "0";
            main.DB_All.DBParam.dbParam116 = "0";
            main.DB_All.DBParam.dbParam117 = "0";
            main.DB_All.DBParam.dbParam118 = "0";
            main.DB_All.DBParam.dbParam119 = "0";
            main.DB_All.DBParam.dbParam120 = "0";
            #endregion
        }
        private void Params_Clear()
        {
            txtSpeedMin.Text = "";  txtSpeedMax.Text = "";
            txtSSlipMin.Text = "";  txtSSlipMax.Text = "";
            txtPTSValue.Text = "";  txtPTSGraph.Text = "";

            txtDragFMin.Text = "";  txtDragFMax.Text = ""; 
            txtDragRMin.Text = "";  txtDragRMax.Text = ""; 
            txtBrk_FMin.Text = "";  txtBrk_FMax.Text = ""; 
            txtBrk_RMin.Text = "";  txtBrk_RMax.Text = ""; 
            txtPark_Min.Text = "";  txtPark_Max.Text = ""; 

            txtBal_FMin.Text = "";  txtBal_FMax.Text = ""; 
            txtBal_RMin.Text = "";  txtBal_RMax.Text = ""; 
            txtBal_AMin.Text = "";  txtBal_AMax.Text = ""; 

            txtWSSFLMin.Text = "";  txtWSSFLMax.Text = ""; 
            txtWSSFRMin.Text = "";  txtWSSFRMax.Text = "";
            txtWSSRLMin.Text = "";  txtWSSRLMax.Text = ""; 
            txtWSSRRMin.Text = "";  txtWSSRRMax.Text = ""; 

            txtDec_FMin.Text = "";  txtDec_FMax.Text = ""; 
            txtInc_FMin.Text = "";  txtInc_FMax.Text = ""; 
            txtDec_RMin.Text = "";  txtDec_RMax.Text = "";
            txtInc_RMin.Text = "";  txtInc_RMax.Text = "";

            txtWgt1_Min.Text = "";  txtWgt1_Max.Text = "";
            txtWgt2_Min.Text = "";  txtWgt2_Max.Text = "";
            txtBrk1_Min.Text = "";  txtBrk2_Min.Text = "";
            txtBrkA_Min.Text = "";  txtBrkP_Min.Text = "";
            txtDiff_Max.Text = "";  txtDrag_Max.Text = "";
            txtWgt_Time.Text = "";  txtBrk_Time.Text = "";
            txtDragTime.Text = "";  txtParkTime.Text = "";
        }
        #endregion

        #region TextBox Editor
        private void textBoxs_Enter(object sender, EventArgs e)
        {
            textbox_SetFocus(((TextBox)sender));
        }

        private void textBoxs_Leave(object sender, EventArgs e)
        {
            textbox_LostFocus(((TextBox)sender));
        }

        private void textbox_SetFocus(TextBox txtB)
        {
            txtB.BackColor = Color.FromArgb(244,244,244);
            txtB.ForeColor = Color.Black;
            txtB.SelectionStart = 0;
            txtB.SelectionLength = txtB.TextLength;
            txtB.SelectAll();
            txtB.Select(0, txtB.TextLength);
            //MessageBox.Show(txtB.SelectedText);
        }

        private void textbox_LostFocus(TextBox txtB)
        {
            txtB.BackColor = Color.White;
            txtB.ForeColor = Color.Black;
        }
        #endregion

        #region Other
        private void dgvModel_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rect = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgvModel.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dgvModel.RowHeadersDefaultCellStyle.Font, rect,
                         dgvModel.ForeColor = Color.DimGray, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }

        private void dgvCurve_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rect = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgvCurve.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dgvCurve.RowHeadersDefaultCellStyle.Font, rect,
                         dgvCurve.ForeColor = Color.DimGray, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }

        private void chk_Pswd_CheckedChanged(object sender, EventArgs e)
        {
            txt_Pswd.PasswordChar = chk_Pswd.Checked ? '\0' : '*';
        }
        #endregion
    }
}