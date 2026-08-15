using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace KI_RnB
{
    public partial class fomDebug : Form
    {
        fom_Main main;
        cls_Test Test;
        Bitmap bmp;
        
        clsCurve crv_Data = new clsCurve();
        
        #region PLC Input
        string[] def_D100 = new string[] { "QD77 준비완료", "동기용 플래그", "Spair 2", "Spair 3", "축1 M 코드 ON", "축2 M 코드 ON", "Spair 6", "Spair 7", 
                                           "축1 에러검출", "축2 에러검출", "Spair A", "Spair B", "축1 BUSY", "축2 BUSY", "Spair E", "Spair F" };

        string[] def_D101 = new string[] { "축1 기동완료", "축2 기동완료", "Spair 2", "Spair 3", "축1 위치결정완료", "축2 위치결정완료", "Spair 6", "Spair 7", 
                                           "Spair 8", "Spair 9", "Spair A", "Spair B", "Spair C", "Spair D", "Spair E", "Spair F" };

        string[] def_D102 = new string[] { "AUTO SS/Manual(Off)", "Ready PB", "Reset PB", "Buzzer Stop PB", "Homeposion PB", "Emergency Stop PB", "Stop PB", "Spair PB", 
                                           "Lamp Test PB", "Calibration PB", "Spair A", "ECU Connect", "Booth Door Open PB", "Booth Door Close PB", "SST IN Photo", "SST OUT Photo" };

        string[] def_D103 = new string[] { "FL-Motor D.-Run", "FR-Motor D.-Run", "RL-Motor D.-Run", "RR-Motor D.-Run", "FL-Motor D.-Error", "FR-Motor D.-Error", "RL-Motor D.-Error", "RR-Motor D.-Error", 
                                           "Pull SW Start", "Pull SW Stop", "Pull SW Cancel", "Pull SW Emergency", "Exhaust Motor On PB", "Exhaust Motor Error", "리모컨 준비", "AIR LOW LS" };

        string[] def_D104 = new string[] { "Spair 0", "FL R.B Free LS", "FL-Front S.R Down LS", "FL-Front S.R Up LS", "FL-Rear S.R Down LS", "FL-Rear S.R Up LS", "FL-F S.R Pin Lock LS", "FL-F S.R Pin Free LS", 
                                           "Spair 8", "Spair 9", "FL-R S.R Pin Lock LS", "FL-R S.R Pin Free LS", "Spair C", "Spair D", "L-Safety Post Down LS", "L-Safety Post Up LS" };

        string[] def_D105 = new string[] { "BT-PHS", "RL R.B Free LS", "RL S.R Down LS", "RL S.R Up LS", "L-Exhaust Flap Down LS", "L-Exhaust Flap Up LS", "L-Frame Lock LS", "L-Frame Free LS", 
                                           "L-Check Vehicle", "F-PHS Vehicle Check", "BT-Lift Up LS", "BT-Lift Down LS", "L-WB Home LS", "L-WB Min LS", "L-WB Max LS", "Air Low" };

        string[] def_D106 = new string[] { "Spair 0", "FR R.B Free LS", "FR-Front S.R Down LS", "FR-Front S.R Up LS", "FR-Rear S.R Down LS", "FR-Rear S.R Up LS", "Spair 6", "Spair 7", 
                                           "FR-F S.R Pin Lock LS", "FR-F S.R Pin Free LS", "Spair A", "Spair B", "FR-R S.R Pin Lock LS", "FR-R S.R Pin Free LS", "R-Safety Post Down LS", "R-Safety Post Up LS" };

        string[] def_D107 = new string[] { "Spair 0", "RR R.B Free LS", "RR S.R Down LS", "RR S.R Up LS", "R-Exhaust Flap Down LS", "R-Exhaust Flap Up LS", "R-Frame Lock LS", "R-FRAME FREE LS", 
                                           "R-Check Vehicle", "R-PHS Vehicle Check", "Spair A", "Spair B", "Spair C", "R-WB Home LS", "R-WB Min LS", "R-WB Max LS" };

        string[] def_D108 = new string[] { "Vehicle Select 01", "Vehicle Select 02", "Vehicle Select 03", "Vehicle Select 04", "Vehicle Select 05", "Vehicle Select 06", "Vehicle Select 07", "Vehicle Select 08", 
                                           "Vehicle Select 09", "Vehicle Select 10", "Spair A", "Spair B", "Spair C", "Vehicle Start", "Vehicle Stop", "Vehicle Emergency" };

        string[] def_D109 = new string[] { "Spair 0", "Spair 1", "Spair 2", "Spair 3", "Spair 4", "Spair 5", "Spair 6", "Spair 7", 
                                           "Spair 8", "Spair 9", "Spair A", "Spair B", "Spair C", "Spair D", "Spair E", "Spair F" };

        string[] def_D110 = new string[] { "Auto SS/Manual(OFF)", "Ready PB", "Reset PB", "Buzzer Stop PB", "Homeposition PB", "Emergency Stop PB", "Stop PB", "Lamp Test PB", 
                                           "Wheelbase 조그 고속", "모터 고속", "Wheelbase 조그 저속", "모터 저속", "전체 Motor Off", "전체 Motor On", "Spair E", "Spair F" };

        string[] def_D111 = new string[] { "FL-Front S.R Down PB", "FL-Front S.R Up PB", "FR-Front S.R Down PB", "FR-Front S.R Up PB", "FL-Rear S.R Down PB", "FL-Rear S.R Up PB", "FR-Rear S.R Down PB", "FR-Rear S.R Up PB", 
                                           "RL-S.R Down PB", "RL-S.R Up PB", "RR-S.R Down PB", "RR-S.R Up PB", "FL-Front S.R Pin Lock PB", "FL-Front S.R Pin Free PB", "FL-Rear S.R Pin Lock PB", "FL-Rear S.R Pin Free PB" };

        string[] def_D112 = new string[] { "FR-Front S.R Pin Lock PB", "FR-Front S.R Pin Free PB", "FR-Rear S.R Pin Lock PB", "FR-Rear S.R Pin Free PB", "FL R.B Lock PB", "FL R.B Free PB", "FR R.B Lock PB", "FR R.B Free PB", 
                                           "RL R.B Lock PB", "RL R.B Free PB", "RR R.B Lock PB", "RR R.B Free PB", "Safety Post Down PB", "Safety Post Up PB", "Exhaust Flap Down PB", "Exhaust Flap Up PB" };

        string[] def_D113 = new string[] { "Frame Lock PB", "Frame Free PB", "W/B Forward PB", "W/B Backword PB", "FL Motor Off PB", "FL Motor On PB", "FR Motor Off PB", "FR Motor On PB", 
                                           "RL Motor Off PB", "RL Motor On PB", "RR Motor Off PB", "RR Motor On PB", "Home position PB", "Enter position PB", "Test position PB", "Exit position PB" };

        string[] def_D114 = new string[] { "Vehicle Select 01", "Vehicle Select 02", "Vehicle Select 03", "Vehicle Select 04", "Vehicle Select 05", "Vehicle Select 06", "Vehicle Select 07", "Vehicle Select 08", 
                                           "Vehicle Select 09", "Vehicle Select 10", "Vehicle Start", "Vehicle Stop", "Vehicle Cancel", "Emergency Stop", "Spair E", "Spair F" };

        string[] def_D115 = new string[] { "Wheelbase 저속(조그)", "Wheelbase 고속(조그)", "원위치 PB", "Manual Start(GOT)", "Manual Stop(GOT)", "Auto(GOT Header)", "Manual(GOT Header)", "Indicator(GOT Header)", 
                                           "Layout(GOT Header)", "Alarm(GOT Header)", "Spair A", "Spair B", "Exhaust Flap Motor Off PB", "Exhaust Flap Motor On PB", "Booth Door Close", "Booth Door Open" };

        string[] def_D116 = new string[] { "수동 정전 이동 PLC(좌측)", "수동 역전 이동 PLC(좌측)", "홈포지션 이동중 PLC(좌측)", "이동중 PLC(좌측)", "이동중 완료 PLC(좌측)", "Spair 5", "Spair 6", "Spair 7", 
                                           "FL-S.R Brake Lock PB", "FL-S.R Brake Free PB", "FR-S.R Brake Lock PB", "FR-S.R Brake Free PB", "RL-S.R Brake Lock PB", "RL-S.R Brake Free PB", "RR-S.R Brake Lock PB", "RR-S.R Brake Free PB" };

        string[] def_D117 = new string[] { "수동 정전 이동 PLC(우측)", "수동 역전 이동 PLC(우측)", "홈포지션 이동중 PLC(우측)", "이동중 PLC(우측)", "이동중 완료 PLC(우측)", "Spair 5", "Spair 6", "Spair 7", 
                                           "Spair 8", "Spair 9", "Spair A", "Spair B", "Spair C", "Spair D", "Spair E", "Spair F" };

        string[] def_D118 = new string[] { "BT-Lift Motor Off PB GOT", "BT-Lift Motor ON PB GOT", "BT-Lift Up PB GOT", "BT-Lift Down PB GOT", "Spair 4", "Spair 5", "Spair 6", "Spair 7", 
                                           "Spair 8", "Spair 9", "Spair A", "Spair B", "Spair C", "Spair D", "Spair E", "Spair F" };

        string[] def_D119 = new string[] { "Spair 0", "Spair 1", "Spair 2", "Spair 3", "Spair 4", "Spair 5", "Spair 6", "Spair 7", 
                                           "Spair 8", "Spair 9", "Spair A", "Spair B", "Spair C", "Spair D", "Spair E", "Spair F" };

        Label[] lbi_100 = new Label[16];
        Label[] lbi_101 = new Label[16];
        Label[] lbi_102 = new Label[16];
        Label[] lbi_103 = new Label[16];
        Label[] lbi_104 = new Label[16];
        Label[] lbi_105 = new Label[16];
        Label[] lbi_106 = new Label[16];
        Label[] lbi_107 = new Label[16];
        Label[] lbi_108 = new Label[16];
        Label[] lbi_109 = new Label[16];
        Label[] lbi_110 = new Label[16];
        Label[] lbi_111 = new Label[16];
        Label[] lbi_112 = new Label[16];
        Label[] lbi_113 = new Label[16];
        Label[] lbi_114 = new Label[16];
        Label[] lbi_115 = new Label[16];
        Label[] lbi_116 = new Label[16];
        Label[] lbi_117 = new Label[16];
        Label[] lbi_118 = new Label[16];
        Label[] lbi_119 = new Label[16];

        #endregion
        int o100, o101, o102, o103, o104, o105, o106, o107, o108, o109, o110, o111, o112, o113, o114, o115, o116, o117, o118, o119;
        
        #region PLC Output
        string[] def_D300 = new string[] { "PLC Ready", "전체축 서보 ON", "Spair 2", "Spair 3", "축1 정지", "축2 정지", "축3 정지", "축4 정지", 
                                           "정전 JOG 기동(좌측)", "역전 JOG 기동(좌측)", "정전 JOG 기동(우측)", " 역전 JOG 기동(우측)", " 축3 정전 JOG 기동", " 축3 역전 JOG 기동", " 축4 정전 JOG 기동", " 축4 역전 JOG 기동" };

        string[] def_D301 = new string[] { "축1 위치결정 기동", "축2 위치결정 기동", "축3 위치결정 기동", "축4 위치결정 기동", "축1 실행금지 플레그", "축2 실행금지 플레그", "축3 실행금지 플레그", "축4 실행금지 플레그", 
                                           "Spair 8", "Spair 9", "Spair A", "Spair B", "Spair C", "Spair D", "Spair E", "Spair F" };

        string[] def_D302 = new string[] { "PLC Run LP", "Ready LP", "Auto LP", "Manual LP", "Calibration LP", "Home Position 최초 LP", "WHEEL BASE MIN. LP", "WHEEL BASE MAX. LP", 
                                           "Spair LP", "Motor Error LP", "AIR Low LP", "Emergency Stop LP", "Booth Front Door Open RY", "Booth Rear Door Open RY", "Booth Front Door Close RY", "Booth Rear Door Close RY" };
        
        string[] def_D303 = new string[] { "Green LP", "Yellow LP", "Red LP", "Melody1(W/B Move)", "Melody2(Error)", "Melody3(Emergency)", "Melody4", "Melody5", 
                                           "좌-모터 브레이크 OFF", "Exhaust Flap Motor On RY", "BT-Motor ON", "Adjust Calibration Zero", "R-Servo Motor Brake OFF", "Calibration Indicter Power ON", "Motor Driver Reset", "Calibration Solenoid ON" };
        
        string[] def_D304 = new string[] { "FL Motor Auto RY", "FL Motor Manual RY", "FL Motor Calibration RY", "FL Motor Start RY", "FL Motor 동기 RY", "FL Motor Stop Brake RY", "FL Motor Parking 검사 RY ", "FL Motor Emergency Stop RY", 
                                           "FR Motor Auto RY", "FR Motor Manual RY", "FR Motor Calibration RY", "FR Motor Start RY", "FR Motor 동기 RY", "FR Motor Stop Brake RY", "FR Motor Parking 검사 RY ", "FR Motor Emergency Stop RY" };

        string[] def_D305 = new string[] { "RL Motor Auto RY", "RL Motor Manual RY", "RL Motor Calibration RY", "RL Motor Start RY", "RL Motor 동기 RY", "RL Motor Stop Brake RY", "RL Motor Parking 검사 RY ", "RL Motor Emergency Stop RY", 
                                           "RR Motor Auto RY", "RR Motor Manual RY", "RR Motor Calibration RY", "RR Motor Start RY", "RR Motor 동기 RY", "RR Motor Stop Brake RY", "RR Motor Parking 검사 RY ", "RR Motor Emergency Stop RY" };

        string[] def_D306 = new string[] { "Vehicle Select 01 LP", "Vehicle Select 02 LP", "Vehicle Select 03 LP", "Vehicle Select 04 LP", "Vehicle Select 05 LP", "Vehicle Select 06 LP", "Vehicle Select 07 LP", "Vehicle Select 08 LP", 
                                           "Vehicle Select 09 LP", "Vehicle Select 10 LP", "Spair A", "Spair B", "Spair C", "Spair D", "Spair E", "Spair F" };

        string[] def_D307 = new string[] { "FL R.B Lock RY", "FL R.B Free RY", "FL-Front S.R Down RY", "FL-Front S.R Up RY", "FL-Front S.R Pin Free RY", "FL-Front S.R Pin Lock RY", "FL-Rear S.R Down RY", "FL-Rear S.R Up RY", 
                                           "FL-Rear S.R Pin Free RY", "FL-Rear S.R Pin Lock RY", "FL S.R Brake Free", "Spair B", "Spair C", "Spair D", "L-Safety Post Down RY", "L-Safety Post Up RY" };

        string[] def_D308 = new string[] { "RL R.B Lock RY", "RL R.B Free RY", "RL S.R Down RY", "RL S.R Up RY", "RL S.R Brake Free", "Spair 5", "L-Exhaust Flap Down RY", "L-Exhaust Flap Up RY", 
                                           "L-Frame Lock RY", "L-Frame Free RY", "Spair A", "Spair B", "Spair C", "Spair D", "BT-Lift Up Sol", "BT-Lift Down Sol" };

        string[] def_D309 = new string[] { "FR R.B Lock RY", "FR R.B Free RY", "FR-Front S.R Down RY", "FR-Front S.R Up RY", "FR-Front S.R Pin Free RY", "FR-Front S.R Pin Lock RY", "FR-Rear S.R Down RY", "FR-Rear S.R Up RY", 
                                           "FR-Rear S.R Pin Free RY", "FR-Rear S.R Pin Lock RY", "FR S.R Brake Free", "Spair B", "Spair C", "Spair D", "R-Safety Post Down RY", "R-Safety Post Up RY" };
        
        string[] def_D310 = new string[] { "RR R.B Lock RY", "RR R.B Free RY", "RR S.R Down RY", "RR S.R Up RY", "RR S.R Brake Free", "Spair 5", "R-Exhaust Flap Down RY", "R-Exhaust Flap Up RY", 
                                           "R-Frame Lock RY", "R-Frame Free RY", "Spair A", "Spair B", "Spair C", "Spair D", "Spair E", "Spair F" };

        string[] def_D311 = new string[] { "Ready Hold PLC", "Auto PLC", "Manual PLC", "Stop PLC", "Homeposition First PLC", "Emergency Stop PLC", "Home Position PLC", "Enter Position PLC", 
                                           "Test Position PLC", "Exit Position PLC", "Spair A", "Spair B", "Spair C", "Spair D", "Spair E", "Spair F" };

        string[] def_D312 = new string[] { "PC 차종선택 완료", "PC 검사시작 준비 PC", "PC 검사시작 PC", "PC 검사완료 PC", "속도체크 5km/h 이상 PC", "위치, 속도 정보 명령 PC", "홈으로(원위치)이동신호PC", "Spair 7", 
                                           "PLC 거리 갱신", "교정용 솔레노이드 ON", "Calibration Indicter Power ON", "Spair B", "BT-Motor ON", "BT-Lift UP", "BT-LIFT Down", "교정기 영점" };

        string[] def_D313 = new string[] { "L-CHECK VEHICLE (W)", "R-CHECK VEHICLE (W)", "F-PHS (Y)", "R-PHS (Y)", "WHEEL BASE POSITION (G)", "HOME POSITION POSITION (G)", "FL ROLLER BRAKE LOCK (G)", "FL ROLLER BRAKE FREE (O)", 
                                           "RL ROLLER BRAKE LOCK (G)", "RL ROLLER BRAKE FREE (O)", "ENTER POSITION (G)", "TEST POSITION (G)", "FR ROLLER BRAKE LOCK (G)", "FR ROLLER BRAKE FREE (O)", "RR ROLLER BRAKE LOCK (G)", "RR ROLLER BRAKE FREE (O)" };

        string[] def_D314 = new string[] { "SST PHS ENTER (W)", "SST PHS EXIT (W)", "FL-FRONT SAFETY ROLLER DOWN (G)", "FL-FRONT SAFETY ROLLER UP (O)", "FL-REAR SAFETY ROLLER DOWN (G)", "FL-REAR SAFETY ROLLER UP (O)", "RL SAFETY ROLLER DOWN (G)", "RL SAFETY ROLLER UP (O)", 
                                           "FR-FRONT SAFETY ROLLER DOWN (G)", "FR-FRONT SAFETY ROLLER UP (O)", "FR-REAR SAFETY ROLLER DOWN (G)", "FR-REAR SAFETY ROLLER UP (O)", "RR SAFETY ROLLER DOWN (G)", "RR SAFETY ROLLER UP (O)", "FL-FL SAFETY ROLLER PIN FREE (G)", "FL-FL SAFETY ROLLER PIN LOCK (O)" };

        string[] def_D315 = new string[] { "FL-FR SAFETY ROLLER PIN FREE (G)", "FL-FR SAFETY ROLLER PIN LOCK (O)", "L-SAFETY POST DOWN (G)", "L-SAFETY POST UP (O)", "FL-RL SAFETY ROLLER PIN FREE (G)", "FL-RL SAFETY ROLLER PIN LOCK (O)", "FL-RR SAFETY ROLLER PIN FREE (G)", "FL-RR SAFETY ROLLER PIN LOCK (O)", 
                                           "R-SAFETY POST DOWN (G)", "R-SAFETY POST UP (O)", "FR-FL SAFETY ROLLER PIN FREE (G)", "FR-FL SAFETY ROLLER PIN LOCK (O)", "FR-FR SAFETY ROLLER PIN FREE (G)", "FR-FR SAFETY ROLLER PIN LOCK (O)", "FRAME LOCK (G)", "FRAME FREE (O)" };

        string[] def_D316 = new string[] { "FR-RL SAFETY ROLLER PIN FREE (G)", "FR-RL SAFETY ROLLER PIN LOCK (O)", "FR-RR SAFETY ROLLER PIN FREE (G)", "FR-RR SAFETY ROLLER PIN LOCK (O)", "EXHAUST FLAP DOWN (G)", "EXHAUST FLAP UP (O)", "FL-MOTOR RUN (O)", "FR-MOTOR RUN (O)", 
                                           "RL-MOTOR RUN (O)", "RR-MOTOR RUN (O)", "WHEEL BASE FORWARD (O)", "WHEEL BASE BACKWARD (O)", "BT-Lift UP (G)", "BT-Lift DOWN (O)", "AIR LOW (R)", "EMERGENCY STOP (R)" };

        string[] def_D317 = new string[] { "WHEEL BASE MIN. (R)", "WHEEL BASE MAX. (R)", "FL-MOTOR ERROR (R)", "FR-MOTOR ERROR (R)", "RL-MOTOR ERROR (R)", "RR-MOTOR ERROR (R)", "Spair 6", "BT-Motor ON LP", 
                                           "AUTO(GOT HEADER)", "MANUAL(GOT HEADER)", "INDICATOR(GOT HEADER)", "LAYOUT(GOT HEADER)", "ALARM(GOT HEADER)", "Spair D", "WHEEL BASE MOVING", "EXIT POSITION (G)" };

        string[] def_D318 = new string[] { "Spair 0", "Spair 1", "Spair 2", "Spair 3", "Spair 4", "Spair 5", "Spair 6", "Spair 7", 
                                           "Spair 8", "Spair 9", "Spair A", "Spair B", "Spair C", "Spair D", "Spair E", "Spair F" };

        string[] def_D319 = new string[] { "Spair 0", "Spair 1", "Spair 2", "Spair 3", "Spair 4", "Spair 5", "Spair 6", "7", 
                                           "Spair 8", "Spair 9", "Spair A", "Spair B", "Spair C", "Spair D", "Spair E", "Spair F" };


        string[] def_D530 = new string[] { "FL Motor Start PC", "FL Motor Sync PC", "FL Motor Stop Brake PC", "FL Motor Parking 검사 PC", "Spair 4", "Spair 5", "Spair 6", "Spair 7", 
                                           "FR Motor Start PC", "FR Motor Sync PC", "FR Motor Stop Brake PC", "FR Motor Parking 검사 PC", "Spair C", "Spair D", "Spair E", "Spair F" };

        string[] def_D531 = new string[] { "RL Motor Start PC", "RL Motor Sync PC", "RL Motor Stop Brake PC", "RL Motor Parking 검사 PC", "Spair 4", "Spair 5", "Spair 6", "Spair 7", 
                                           "RR Motor Start PC", "RR Motor Sync PC", "RR Motor Stop Brake PC", "RR Motor Parking 검사 PC", "Spair C", "Spair D", "BOOTH DOOR OPEN PC", "BOOTH DOOR CLOSE PC" };

        string[] def_D562 = new string[] { "Vehicle Select 01 LP", "Vehicle Select 02 LP", "Vehicle Select 03 LP", "Vehicle Select 04 LP", "Vehicle Select 05 LP", "Vehicle Select 06 LP", "Vehicle Select 07 LP", "Vehicle Select 08 LP", 
                                           "Vehicle Select 09 LP", "Vehicle Select 10 LP", "Vehicle Start PC", "Vehicle Stop PC", "Vehicle Cancel PC", "Spair D", "Ready PC", "Motor Controller Reset" };

        Label[] lbo_300 = new Label[16];
        Label[] lbo_301 = new Label[16];
        Label[] lbo_302 = new Label[16];
        Label[] lbo_303 = new Label[16];
        Label[] lbo_304 = new Label[16];
        Label[] lbo_305 = new Label[16];
        Label[] lbo_306 = new Label[16]; 
        Label[] lbo_307 = new Label[16];
        Label[] lbo_308 = new Label[16];
        Label[] lbo_309 = new Label[16];
        Label[] lbo_310 = new Label[16];
        Label[] lbo_311 = new Label[16]; 
        Label[] lbo_312 = new Label[16];
        Label[] lbo_313 = new Label[16];
        Label[] lbo_314 = new Label[16];
        Label[] lbo_315 = new Label[16];
        Label[] lbo_316 = new Label[16];
        Label[] lbo_317 = new Label[16];
        Label[] lbo_318 = new Label[16];
        Label[] lbo_319 = new Label[16];

        Label[] lbo_530 = new Label[16];
        Label[] lbo_531 = new Label[16];

        Label[] lbo_562 = new Label[16];
        #endregion
        int o300, o301, o302, o303, o304, o305, o306, o307, o308, o309, o310, o311, o312, o313, o314, o315, o316, o317, o318, o319;
        int o530, o531;
        int o562;

        public fomDebug()
        {
            InitializeComponent();
        }
        public fomDebug(fom_Main main)
            : this()
        {
            this.main = main;
        }
        private void fomDebug_Load(object sender, EventArgs e)
        {
            PSet.OnfDebug = true;

            lbl_IDSN.Text = NeoVI.SerialNo;

            cbo_ECUs.Items.Clear();
            cbo_ECUs.Items.Add(ECUs.Mobis___AD);
            cbo_ECUs.Items.Add(ECUs.Mobis__DN8);
            cbo_ECUs.Items.Add(ECUs.Mobis___FL);
            cbo_ECUs.Items.Add(ECUs.Mando___TL);
            cbo_ECUs.Items.Add(ECUs.Mando___TM);
            cbo_ECUs.Items.Add(ECUs.Mando__HEV);
            cbo_ECUs.Items.Add(ECUs.Mando_NX4H);
            cbo_ECUs.Items.Add(ECUs.Mando_NX4I);
            cbo_ECUs.Items.Add(ECUs.Mobis_LX3H);
            cbo_ECUs.Items.Add(ECUs.Mobis_LX3I);
            cbo_ECUs.Items.Add(ECUs.Chery_1box);
            cbo_ECUs.SelectedIndex = 0;

            cboIdent.Items.Clear();
            cboIdent.Items.Add("0xF021 Motor");
            cboIdent.Items.Add("0xF022 Front Left Valve (Input)");
            cboIdent.Items.Add("0xF023 Front Right Valve (Input)");
            cboIdent.Items.Add("0xF024 Rear Left Valve (Input)");
            cboIdent.Items.Add("0xF025 Rear Right Valve (Input)");
            cboIdent.Items.Add("0xF026 Front Left Valve (Output)");
            cboIdent.Items.Add("0xF027 Front Right Valve (Output)");
            cboIdent.Items.Add("0xF028 Rear Left Valve (Output)");
            cboIdent.Items.Add("0xF029 Rear Right Valve (Output)");
            cboIdent.Items.Add("0xF02A TCS Front Left Valve : TCL (ESC only)");
            cboIdent.Items.Add("0xF02B TCS Front Right Valve : TCR (ESC only)");
            cboIdent.Items.Add("0xF02E Electric Shuttle Valve : ESV-R (ESC only)");
            cboIdent.Items.Add("0xF02F Electric Shuttle Valve : ESV-L (ESC only)");
            cboIdent.Items.Add("0xF031 DBC Brake Lamp Relay (Only for System applied DBC or HAC)");
            cboIdent.Items.Add("0xF032 ESS Brake Lamp Relay (Only for System applied ESS)");
            cboIdent.Items.Add("0xF033 Vacuum Pump Relay (Only for System applied Vacuum Pump)");
            cboIdent.SelectedIndex = 0;

            cbo_Filt.Items.Clear();
            cbo_Filt.Items.Add("X Filter");
            cbo_Filt.Items.Add("Q Filter");
            cbo_Filt.Items.Add("VB Filter");

            crv_Data.Get_DriveCurve("Default");

            if (crv_Data.G_Data != null)
            {
                foreach (Curve_Data kind in crv_Data.G_Data)
                {
                    lst_Step.Items.Add(kind.Items);
                }
            }
            
            CreateILabel(gbx_D100, "D100", lbi_100, def_D100, false);
            CreateILabel(gbx_D101, "D101", lbi_101, def_D101, false);
            CreateILabel(gbx_D102, "D102", lbi_102, def_D102, false);
            CreateILabel(gbx_D103, "D103", lbi_103, def_D103, false);
            CreateILabel(gbx_D104, "D104", lbi_104, def_D104, false);
            CreateILabel(gbx_D105, "D105", lbi_105, def_D105, false);
            CreateILabel(gbx_D106, "D106", lbi_106, def_D106, false);
            CreateILabel(gbx_D107, "D107", lbi_107, def_D107, false);
            CreateILabel(gbx_D108, "D108", lbi_108, def_D108, false);
            CreateILabel(gbx_D109, "D109", lbi_109, def_D109, false);
            CreateILabel(gbx_D110, "D110", lbi_110, def_D110, false);
            CreateILabel(gbx_D111, "D111", lbi_111, def_D111, false);
            CreateILabel(gbx_D112, "D112", lbi_112, def_D112, false);
            CreateILabel(gbx_D113, "D113", lbi_113, def_D113, false);
            CreateILabel(gbx_D114, "D114", lbi_114, def_D114, false);
            CreateILabel(gbx_D115, "D115", lbi_115, def_D115, false);
            CreateILabel(gbx_D116, "D116", lbi_116, def_D116, false);
            CreateILabel(gbx_D117, "D117", lbi_117, def_D117, false);
            CreateILabel(gbx_D118, "D118", lbi_118, def_D118, false);
            CreateILabel(gbx_D119, "D119", lbi_119, def_D119, false);

            CreateOLabel(gbx_D300, "D300", lbo_300, def_D300, false);
            CreateOLabel(gbx_D301, "D301", lbo_301, def_D301, false);
            CreateOLabel(gbx_D302, "D302", lbo_302, def_D302, false);
            CreateOLabel(gbx_D303, "D303", lbo_303, def_D303, false);
            CreateOLabel(gbx_D304, "D304", lbo_304, def_D304, false);
            CreateOLabel(gbx_D305, "D305", lbo_305, def_D305, false);
            CreateOLabel(gbx_D306, "D306", lbo_306, def_D306, false);
            CreateOLabel(gbx_D307, "D307", lbo_307, def_D307, false);
            CreateOLabel(gbx_D308, "D308", lbo_308, def_D308, false);
            CreateOLabel(gbx_D309, "D309", lbo_309, def_D309, false);
            CreateOLabel(gbx_D310, "D310", lbo_310, def_D310, false);
            CreateOLabel(gbx_D311, "D311", lbo_311, def_D311, false);
            CreateOLabel(gbx_D312, "D312", lbo_312, def_D312, true);
            CreateOLabel(gbx_D313, "D313", lbo_313, def_D313, false);
            CreateOLabel(gbx_D314, "D314", lbo_314, def_D314, false);
            CreateOLabel(gbx_D315, "D315", lbo_315, def_D315, false);
            CreateOLabel(gbx_D316, "D316", lbo_316, def_D316, false);
            CreateOLabel(gbx_D317, "D317", lbo_317, def_D317, false);
            CreateOLabel(gbx_D318, "D318", lbo_318, def_D318, false);
            CreateOLabel(gbx_D319, "D319", lbo_319, def_D319, false);

            CreateOLabel(gbx_D530, "D530", lbo_530, def_D530, true);
            CreateOLabel(gbx_D531, "D531", lbo_531, def_D531, true);
            CreateOLabel(gbx_D562, "D562", lbo_562, def_D562, true);

            o100 = -1; o300 = -1; o530 = -1;
            o101 = -1; o301 = -1; o531 = -1;
            o102 = -1; o302 = -1; o562 = -1;
            o103 = -1; o303 = -1;
            o104 = -1; o304 = -1;
            o105 = -1; o305 = -1;
            o106 = -1; o306 = -1;
            o107 = -1; o307 = -1;
            o108 = -1; o308 = -1;
            o109 = -1; o309 = -1;
            o110 = -1; o310 = -1;
            o111 = -1; o311 = -1;
            o112 = -1; o312 = -1;
            o113 = -1; o313 = -1;
            o114 = -1; o314 = -1;
            o115 = -1; o315 = -1;
            o116 = -1; o316 = -1;
            o117 = -1; o317 = -1;
            o118 = -1; o318 = -1;
            o119 = -1; o319 = -1;

            Cal_DataShow();
            PLC_IOs_Show();
            PLC_DistShow();
            MDrive__Read();
            tmr_Loop.Enabled = true;
        }
        private void fomDebug_FormClosed(object sender, FormClosedEventArgs e)
        {
            PSet.OnfDebug = false;
            tmr_Loop.Enabled = false;
        }
        private void tmr_Loop_Tick(object sender, EventArgs e)
        {
            btn_Open.BackColor = NeoVI.IsOpen ? Color.Lime : Color.Transparent;
            btn_Read.BackColor = NeoVI.IsRead ? Color.Lime : Color.Transparent;

            //PLC.DO.m_Redy
            if (PLC.DO.MD___Auto) { lbl_Mode.BackColor = Color.Lime; lbl_Mode.Text = "AUTO MODE"; }
            if (PLC.DO.MD_Manual) { lbl_Mode.BackColor = Color.Yellow; lbl_Mode.Text = "MANUAL MODE"; }
            if (PLC.DI.PBCalMode) { lbl_Mode.BackColor = Color.Red; lbl_Mode.Text = "CALIBRATION MODE"; }

            btnFSync.Enabled = PLC.DO.MD___Auto ? true : false;
            btnRSync.Enabled = PLC.DO.MD___Auto ? true : false;
            btn__WSS.Enabled = PLC.DO.MD___Auto ? true : false;

            PLC_IOs_Show();

            lbl0Scan.Text = PSet.CH0Scan.ToString();
            lbl1Scan.Text = PSet.CH1Scan.ToString();
            lbl2Scan.Text = PSet.CH2Scan.ToString();
            lbl3Scan.Text = PSet.CH3Scan.ToString();
            lbl4Scan.Text = PSet.CH4Scan.ToString();
            lbl5Scan.Text = PSet.CH5Scan.ToString();

            lbl0Last.Text = PSet.CH0Last.ToString();
            lbl1Last.Text = PSet.CH1Last.ToString();
            lbl2Last.Text = PSet.CH2Last.ToString();
            lbl3Last.Text = PSet.CH3Last.ToString();
            lbl4Last.Text = PSet.CH4Last.ToString();
            lbl5Last.Text = PSet.CH5Last.ToString();

            lbl0_Val.Text = PSet.CH0_Val.ToString("0.00");
            lbl1_Val.Text = PSet.CH1_Val.ToString("0.00");
            lbl2_Val.Text = PSet.CH2_Val.ToString("0.00");
            lbl3_Val.Text = PSet.CH3_Val.ToString("0.00");
            lbl4_Val.Text = PSet.CH4_Val.ToString("0.00");
            lbl5_Val.Text = PSet.CH5_Val.ToString("0.00");
        }

        #region PLC In/Output
        private void PLC_IOs_Show()
        {
            for (int cnt = 0; cnt < 16; cnt++)
            {
                Ret_LabelSet(lbi_100[cnt], PLC.DIB100[cnt], 0);
                Ret_LabelSet(lbi_101[cnt], PLC.DIB101[cnt], 0);
                Ret_LabelSet(lbi_102[cnt], PLC.DIB102[cnt], 0);
                Ret_LabelSet(lbi_103[cnt], PLC.DIB103[cnt], 0);
                Ret_LabelSet(lbi_104[cnt], PLC.DIB104[cnt], 0);
                Ret_LabelSet(lbi_105[cnt], PLC.DIB105[cnt], 0);
                Ret_LabelSet(lbi_106[cnt], PLC.DIB106[cnt], 0);
                Ret_LabelSet(lbi_107[cnt], PLC.DIB107[cnt], 0);
                Ret_LabelSet(lbi_108[cnt], PLC.DIB108[cnt], 0);
                Ret_LabelSet(lbi_109[cnt], PLC.DIB109[cnt], 0);
                Ret_LabelSet(lbi_110[cnt], PLC.DIB110[cnt], 0);
                Ret_LabelSet(lbi_111[cnt], PLC.DIB111[cnt], 0);
                Ret_LabelSet(lbi_112[cnt], PLC.DIB112[cnt], 0);
                Ret_LabelSet(lbi_113[cnt], PLC.DIB113[cnt], 0);
                Ret_LabelSet(lbi_114[cnt], PLC.DIB114[cnt], 0);
                Ret_LabelSet(lbi_115[cnt], PLC.DIB115[cnt], 0);
                Ret_LabelSet(lbi_116[cnt], PLC.DIB116[cnt], 0);
                Ret_LabelSet(lbi_117[cnt], PLC.DIB117[cnt], 0);
                Ret_LabelSet(lbi_118[cnt], PLC.DIB118[cnt], 0);
                Ret_LabelSet(lbi_119[cnt], PLC.DIB119[cnt], 0);

                Ret_LabelSet(lbo_300[cnt], PLC.DOB300[cnt], 1);
                Ret_LabelSet(lbo_301[cnt], PLC.DOB301[cnt], 1);
                Ret_LabelSet(lbo_302[cnt], PLC.DOB302[cnt], 1);
                Ret_LabelSet(lbo_303[cnt], PLC.DOB303[cnt], 1);
                Ret_LabelSet(lbo_304[cnt], PLC.DOB304[cnt], 1);
                Ret_LabelSet(lbo_305[cnt], PLC.DOB305[cnt], 1);
                Ret_LabelSet(lbo_306[cnt], PLC.DOB306[cnt], 1);
                Ret_LabelSet(lbo_307[cnt], PLC.DOB307[cnt], 1);
                Ret_LabelSet(lbo_308[cnt], PLC.DOB308[cnt], 1);
                Ret_LabelSet(lbo_309[cnt], PLC.DOB309[cnt], 1);
                Ret_LabelSet(lbo_310[cnt], PLC.DOB310[cnt], 1);
                Ret_LabelSet(lbo_311[cnt], PLC.DOB311[cnt], 1);
                Ret_LabelSet(lbo_312[cnt], PLC.DOB312[cnt], 1);
                Ret_LabelSet(lbo_313[cnt], PLC.DOB313[cnt], 1);
                Ret_LabelSet(lbo_314[cnt], PLC.DOB314[cnt], 1);
                Ret_LabelSet(lbo_315[cnt], PLC.DOB315[cnt], 1);
                Ret_LabelSet(lbo_316[cnt], PLC.DOB316[cnt], 1);
                Ret_LabelSet(lbo_317[cnt], PLC.DOB317[cnt], 1);
                Ret_LabelSet(lbo_318[cnt], PLC.DOB318[cnt], 1);
                Ret_LabelSet(lbo_319[cnt], PLC.DOB319[cnt], 1);

                Ret_LabelSet(lbo_530[cnt], PLC.DOB530[cnt], 1);
                Ret_LabelSet(lbo_531[cnt], PLC.DOB531[cnt], 1);

                Ret_LabelSet(lbo_562[cnt], PLC.DOB562[cnt], 1);
            }

            if (o100 != PLC.DI_100) gbx_D100.Text = "D100 - " + PLC.DI_100.ToString() + " (HEX : " + PLC.DI_100.ToString("X4") + ")";
            if (o101 != PLC.DI_101) gbx_D101.Text = "D101 - " + PLC.DI_101.ToString() + " (HEX : " + PLC.DI_101.ToString("X4") + ")";
            if (o102 != PLC.DI_102) gbx_D102.Text = "D102 - " + PLC.DI_102.ToString() + " (HEX : " + PLC.DI_102.ToString("X4") + ")";
            if (o103 != PLC.DI_103) gbx_D103.Text = "D103 - " + PLC.DI_103.ToString() + " (HEX : " + PLC.DI_103.ToString("X4") + ")";
            if (o104 != PLC.DI_104) gbx_D104.Text = "D104 - " + PLC.DI_104.ToString() + " (HEX : " + PLC.DI_104.ToString("X4") + ")";
            if (o105 != PLC.DI_105) gbx_D105.Text = "D105 - " + PLC.DI_105.ToString() + " (HEX : " + PLC.DI_105.ToString("X4") + ")";
            if (o106 != PLC.DI_106) gbx_D106.Text = "D106 - " + PLC.DI_106.ToString() + " (HEX : " + PLC.DI_106.ToString("X4") + ")";
            if (o107 != PLC.DI_107) gbx_D107.Text = "D107 - " + PLC.DI_107.ToString() + " (HEX : " + PLC.DI_107.ToString("X4") + ")";
            if (o108 != PLC.DI_108) gbx_D108.Text = "D108 - " + PLC.DI_108.ToString() + " (HEX : " + PLC.DI_108.ToString("X4") + ")";
            if (o109 != PLC.DI_109) gbx_D109.Text = "D109 - " + PLC.DI_109.ToString() + " (HEX : " + PLC.DI_109.ToString("X4") + ")";
            if (o110 != PLC.DI_110) gbx_D110.Text = "D110 - " + PLC.DI_110.ToString() + " (HEX : " + PLC.DI_110.ToString("X4") + ")";
            if (o111 != PLC.DI_111) gbx_D111.Text = "D111 - " + PLC.DI_111.ToString() + " (HEX : " + PLC.DI_111.ToString("X4") + ")";
            if (o112 != PLC.DI_112) gbx_D112.Text = "D112 - " + PLC.DI_112.ToString() + " (HEX : " + PLC.DI_112.ToString("X4") + ")";
            if (o113 != PLC.DI_113) gbx_D113.Text = "D113 - " + PLC.DI_113.ToString() + " (HEX : " + PLC.DI_113.ToString("X4") + ")";
            if (o114 != PLC.DI_114) gbx_D114.Text = "D113 - " + PLC.DI_114.ToString() + " (HEX : " + PLC.DI_114.ToString("X4") + ")";
            if (o115 != PLC.DI_115) gbx_D115.Text = "D115 - " + PLC.DI_115.ToString() + " (HEX : " + PLC.DI_115.ToString("X4") + ")";
            if (o116 != PLC.DI_116) gbx_D116.Text = "D116 - " + PLC.DI_116.ToString() + " (HEX : " + PLC.DI_116.ToString("X4") + ")";
            if (o117 != PLC.DI_117) gbx_D117.Text = "D117 - " + PLC.DI_117.ToString() + " (HEX : " + PLC.DI_117.ToString("X4") + ")";
            if (o118 != PLC.DI_118) gbx_D118.Text = "D118 - " + PLC.DI_118.ToString() + " (HEX : " + PLC.DI_118.ToString("X4") + ")";
            if (o118 != PLC.DI_119) gbx_D119.Text = "D119 - " + PLC.DI_119.ToString() + " (HEX : " + PLC.DI_119.ToString("X4") + ")";

            if (o300 != PLC.DO_300) gbx_D300.Text = "D300 - " + PLC.DO_300.ToString() + " (HEX : " + PLC.DO_300.ToString("X4") + ")";
            if (o301 != PLC.DO_301) gbx_D301.Text = "D301 - " + PLC.DO_301.ToString() + " (HEX : " + PLC.DO_301.ToString("X4") + ")";
            if (o302 != PLC.DO_302) gbx_D302.Text = "D302 - " + PLC.DO_302.ToString() + " (HEX : " + PLC.DO_302.ToString("X4") + ")";
            if (o303 != PLC.DO_303) gbx_D303.Text = "D303 - " + PLC.DO_303.ToString() + " (HEX : " + PLC.DO_303.ToString("X4") + ")";
            if (o304 != PLC.DO_304) gbx_D304.Text = "D304 - " + PLC.DO_304.ToString() + " (HEX : " + PLC.DO_304.ToString("X4") + ")";
            if (o305 != PLC.DO_305) gbx_D305.Text = "D305 - " + PLC.DO_305.ToString() + " (HEX : " + PLC.DO_305.ToString("X4") + ")";
            if (o306 != PLC.DO_306) gbx_D306.Text = "D306 - " + PLC.DO_306.ToString() + " (HEX : " + PLC.DO_306.ToString("X4") + ")";
            if (o307 != PLC.DO_307) gbx_D307.Text = "D307 - " + PLC.DO_307.ToString() + " (HEX : " + PLC.DO_307.ToString("X4") + ")";
            if (o308 != PLC.DO_308) gbx_D308.Text = "D308 - " + PLC.DO_308.ToString() + " (HEX : " + PLC.DO_308.ToString("X4") + ")";
            if (o309 != PLC.DO_309) gbx_D309.Text = "D309 - " + PLC.DO_309.ToString() + " (HEX : " + PLC.DO_309.ToString("X4") + ")";
            if (o310 != PLC.DO_310) gbx_D310.Text = "D310 - " + PLC.DO_310.ToString() + " (HEX : " + PLC.DO_310.ToString("X4") + ")";
            if (o311 != PLC.DO_311) gbx_D311.Text = "D311 - " + PLC.DO_311.ToString() + " (HEX : " + PLC.DO_311.ToString("X4") + ")";
            if (o312 != PLC.DO_312) gbx_D312.Text = "D312 - " + PLC.DO_312.ToString() + " (HEX : " + PLC.DO_312.ToString("X4") + ")";
            if (o313 != PLC.DO_313) gbx_D313.Text = "D313 - " + PLC.DO_313.ToString() + " (HEX : " + PLC.DO_313.ToString("X4") + ")";
            if (o314 != PLC.DO_314) gbx_D314.Text = "D314 - " + PLC.DO_314.ToString() + " (HEX : " + PLC.DO_314.ToString("X4") + ")";
            if (o315 != PLC.DO_315) gbx_D315.Text = "D315 - " + PLC.DO_315.ToString() + " (HEX : " + PLC.DO_315.ToString("X4") + ")";
            if (o316 != PLC.DO_316) gbx_D316.Text = "D316 - " + PLC.DO_316.ToString() + " (HEX : " + PLC.DO_316.ToString("X4") + ")";
            if (o317 != PLC.DO_317) gbx_D317.Text = "D317 - " + PLC.DO_317.ToString() + " (HEX : " + PLC.DO_317.ToString("X4") + ")";

            if (o530 != PLC.DO_530) gbx_D530.Text = "D530 - " + PLC.DO_530.ToString() + " (HEX : " + PLC.DO_530.ToString("X4") + ")";
            if (o531 != PLC.DO_531) gbx_D531.Text = "D531 - " + PLC.DO_531.ToString() + " (HEX : " + PLC.DO_531.ToString("X4") + ")";
            if (o562 != PLC.DO_562) gbx_D562.Text = "D562 - " + PLC.DO_562.ToString() + " (HEX : " + PLC.DO_562.ToString("X4") + ")";

            lbl_D500.Text = PLC.DM_500.ToString("X4"); lbl_D501.Text = PLC.DM_501.ToString("X4"); lbl_L_00.Text = (PLC.HexToDecimal(PLC.DM_500.ToString("X4") + PLC.DM_501.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D502.Text = PLC.DM_502.ToString("X4"); lbl_D503.Text = PLC.DM_503.ToString("X4"); lbl_L_01.Text = (PLC.HexToDecimal(PLC.DM_502.ToString("X4") + PLC.DM_503.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D504.Text = PLC.DM_504.ToString("X4"); lbl_D505.Text = PLC.DM_505.ToString("X4"); lbl_L_02.Text = (PLC.HexToDecimal(PLC.DM_504.ToString("X4") + PLC.DM_505.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D506.Text = PLC.DM_506.ToString("X4"); lbl_D507.Text = PLC.DM_507.ToString("X4"); lbl_L_03.Text = (PLC.HexToDecimal(PLC.DM_506.ToString("X4") + PLC.DM_507.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D508.Text = PLC.DM_508.ToString("X4"); lbl_D509.Text = PLC.DM_509.ToString("X4"); lbl_L_04.Text = (PLC.HexToDecimal(PLC.DM_508.ToString("X4") + PLC.DM_509.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D510.Text = PLC.DM_510.ToString("X4"); lbl_D511.Text = PLC.DM_511.ToString("X4"); lbl_L_05.Text = (PLC.HexToDecimal(PLC.DM_510.ToString("X4") + PLC.DM_511.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D512.Text = PLC.DM_512.ToString("X4"); lbl_D513.Text = PLC.DM_513.ToString("X4"); lbl_L_06.Text = (PLC.HexToDecimal(PLC.DM_512.ToString("X4") + PLC.DM_513.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D514.Text = PLC.DM_514.ToString("X4"); lbl_D515.Text = PLC.DM_515.ToString("X4"); lbl_L_07.Text = (PLC.HexToDecimal(PLC.DM_514.ToString("X4") + PLC.DM_515.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D516.Text = PLC.DM_516.ToString("X4"); lbl_D517.Text = PLC.DM_517.ToString("X4"); lbl_L_08.Text = (PLC.HexToDecimal(PLC.DM_516.ToString("X4") + PLC.DM_517.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D518.Text = PLC.DM_518.ToString("X4"); lbl_D519.Text = PLC.DM_519.ToString("X4"); lbl_L_09.Text = (PLC.HexToDecimal(PLC.DM_518.ToString("X4") + PLC.DM_519.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D520.Text = PLC.DM_520.ToString("X4"); lbl_D521.Text = PLC.DM_521.ToString("X4"); lbl_L_10.Text = (PLC.HexToDecimal(PLC.DM_520.ToString("X4") + PLC.DM_521.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D522.Text = PLC.DM_522.ToString("X4"); lbl_D523.Text = PLC.DM_523.ToString("X4"); lbl_L_11.Text = (PLC.HexToDecimal(PLC.DM_522.ToString("X4") + PLC.DM_523.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D524.Text = PLC.DM_524.ToString("X4"); lbl_D525.Text = PLC.DM_525.ToString("X4"); lbl_L_12.Text = (PLC.HexToDecimal(PLC.DM_524.ToString("X4") + PLC.DM_525.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D526.Text = PLC.DM_526.ToString("X4"); lbl_D527.Text = PLC.DM_527.ToString("X4"); lbl_L_13.Text = (PLC.HexToDecimal(PLC.DM_526.ToString("X4") + PLC.DM_527.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D528.Text = PLC.DM_528.ToString("X4"); lbl_D529.Text = PLC.DM_529.ToString("X4"); lbl_L_14.Text = (PLC.HexToDecimal(PLC.DM_528.ToString("X4") + PLC.DM_529.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D530.Text = PLC.DM_530.ToString("X4"); lbl_D531.Text = PLC.DM_531.ToString("X4"); lbl_L_15.Text = (PLC.HexToDecimal(PLC.DM_530.ToString("X4") + PLC.DM_531.ToString("X4")) / PLC.LentGain).ToString();

            lbl1D508.Text = (PLC.HexToDecimal(PLC.DM_509.ToString("X4") + PLC.DM_508.ToString("X4")) / PLC.LentGain).ToString();
            lbl1D510.Text = (PLC.HexToDecimal(PLC.DM_511.ToString("X4") + PLC.DM_510.ToString("X4")) / PLC.LentGain).ToString();
            lbl1D512.Text = (PLC.HexToDecimal(PLC.DM_513.ToString("X4") + PLC.DM_512.ToString("X4")) / PLC.LentGain).ToString();

            lbl_D700.Text = PLC.DM_700.ToString("X4"); lbl_D701.Text = PLC.DM_701.ToString("X4"); lbl_R_00.Text = (PLC.HexToDecimal(PLC.DM_700.ToString("X4") + PLC.DM_701.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D702.Text = PLC.DM_702.ToString("X4"); lbl_D703.Text = PLC.DM_703.ToString("X4"); lbl_R_01.Text = (PLC.HexToDecimal(PLC.DM_702.ToString("X4") + PLC.DM_703.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D704.Text = PLC.DM_704.ToString("X4"); lbl_D705.Text = PLC.DM_705.ToString("X4"); lbl_R_02.Text = (PLC.HexToDecimal(PLC.DM_704.ToString("X4") + PLC.DM_705.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D706.Text = PLC.DM_706.ToString("X4"); lbl_D707.Text = PLC.DM_707.ToString("X4"); lbl_R_03.Text = (PLC.HexToDecimal(PLC.DM_706.ToString("X4") + PLC.DM_707.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D708.Text = PLC.DM_708.ToString("X4"); lbl_D709.Text = PLC.DM_709.ToString("X4"); lbl_R_04.Text = (PLC.HexToDecimal(PLC.DM_708.ToString("X4") + PLC.DM_709.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D710.Text = PLC.DM_710.ToString("X4"); lbl_D711.Text = PLC.DM_711.ToString("X4"); lbl_R_05.Text = (PLC.HexToDecimal(PLC.DM_710.ToString("X4") + PLC.DM_711.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D712.Text = PLC.DM_712.ToString("X4"); lbl_D713.Text = PLC.DM_713.ToString("X4"); lbl_R_06.Text = (PLC.HexToDecimal(PLC.DM_712.ToString("X4") + PLC.DM_713.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D714.Text = PLC.DM_714.ToString("X4"); lbl_D715.Text = PLC.DM_715.ToString("X4"); lbl_R_07.Text = (PLC.HexToDecimal(PLC.DM_714.ToString("X4") + PLC.DM_715.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D716.Text = PLC.DM_716.ToString("X4"); lbl_D717.Text = PLC.DM_717.ToString("X4"); lbl_R_08.Text = (PLC.HexToDecimal(PLC.DM_716.ToString("X4") + PLC.DM_717.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D718.Text = PLC.DM_718.ToString("X4"); lbl_D719.Text = PLC.DM_719.ToString("X4"); lbl_R_09.Text = (PLC.HexToDecimal(PLC.DM_718.ToString("X4") + PLC.DM_719.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D720.Text = PLC.DM_720.ToString("X4"); lbl_D721.Text = PLC.DM_721.ToString("X4"); lbl_R_10.Text = (PLC.HexToDecimal(PLC.DM_720.ToString("X4") + PLC.DM_721.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D722.Text = PLC.DM_722.ToString("X4"); lbl_D723.Text = PLC.DM_723.ToString("X4"); lbl_R_11.Text = (PLC.HexToDecimal(PLC.DM_722.ToString("X4") + PLC.DM_723.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D724.Text = PLC.DM_724.ToString("X4"); lbl_D725.Text = PLC.DM_725.ToString("X4"); lbl_R_12.Text = (PLC.HexToDecimal(PLC.DM_724.ToString("X4") + PLC.DM_725.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D726.Text = PLC.DM_726.ToString("X4"); lbl_D727.Text = PLC.DM_727.ToString("X4"); lbl_R_13.Text = (PLC.HexToDecimal(PLC.DM_726.ToString("X4") + PLC.DM_727.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D728.Text = PLC.DM_728.ToString("X4"); lbl_D729.Text = PLC.DM_729.ToString("X4"); lbl_R_14.Text = (PLC.HexToDecimal(PLC.DM_728.ToString("X4") + PLC.DM_729.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D730.Text = PLC.DM_730.ToString("X4"); lbl_D731.Text = PLC.DM_731.ToString("X4"); lbl_R_15.Text = (PLC.HexToDecimal(PLC.DM_730.ToString("X4") + PLC.DM_731.ToString("X4")) / PLC.LentGain).ToString();
            lbl_D732.Text = PLC.DM_732.ToString("X4"); lbl_D733.Text = PLC.DM_733.ToString("X4"); lbl_R_16.Text = (PLC.HexToDecimal(PLC.DM_732.ToString("X4") + PLC.DM_733.ToString("X4")) / PLC.LentGain).ToString();

            lbl1D708.Text = (PLC.HexToDecimal(PLC.DM_709.ToString("X4") + PLC.DM_708.ToString("X4")) / PLC.LentGain).ToString();
            lbl1D710.Text = (PLC.HexToDecimal(PLC.DM_711.ToString("X4") + PLC.DM_710.ToString("X4")) / PLC.LentGain).ToString();
            lbl1D712.Text = (PLC.HexToDecimal(PLC.DM_713.ToString("X4") + PLC.DM_712.ToString("X4")) / PLC.LentGain).ToString();

            lbl_D554.Text = PLC.DM_554.ToString("X4"); lbl_D555.Text = PLC.DM_555.ToString("X4");
            lbl_D556.Text = PLC.DM_556.ToString("X4"); lbl_D557.Text = PLC.DM_557.ToString("X4");
            lbl_D558.Text = PLC.DM_558.ToString("X4"); lbl_D559.Text = PLC.DM_559.ToString("X4");
            lbl_D560.Text = PLC.DM_560.ToString("X4"); lbl_D561.Text = PLC.DM_561.ToString("X4");
            lbl_D562.Text = PLC.DM_562.ToString("X4"); lbl_D563.Text = PLC.DM_563.ToString("X4");

            if (chk_Dist.Checked)
            {
                txtRead1.Text = (PLC.OfSetL + PLC.Dist_A).ToString();
                txtRead2.Text = (PLC.OfSetL + PLC.Dist_B).ToString();
                txtRead3.Text = (PLC.OfSetL + PLC.Dist_C).ToString();
                txtRead4.Text = (PLC.OfSetL + PLC.Dist_D).ToString();
                txtRead5.Text = (PLC.OfSetL + PLC.Dist_E).ToString();
                txtRead6.Text = (PLC.OfSetL + PLC.Dist_F).ToString();
                txtRead7.Text = (PLC.OfSetL + PLC.Dist_G).ToString();
                txtRead8.Text = (PLC.OfSetL + PLC.Dist_H).ToString();
                txtRead9.Text = (PLC.OfSetL + PLC.Dist_I).ToString();
                txtReadA.Text = (PLC.OfSetL + PLC.Dist_J).ToString();
                txtReadL.Text = PLC.OfSetL.ToString();
                txtReadR.Text = PLC.OfSetR.ToString();
            }
            else
            {
                txtRead1.Text = PLC.Dist_A.ToString();
                txtRead2.Text = PLC.Dist_B.ToString();
                txtRead3.Text = PLC.Dist_C.ToString();
                txtRead4.Text = PLC.Dist_D.ToString();
                txtRead5.Text = PLC.Dist_E.ToString();
                txtRead6.Text = PLC.Dist_F.ToString();
                txtRead7.Text = PLC.Dist_G.ToString();
                txtRead8.Text = PLC.Dist_H.ToString();
                txtRead9.Text = PLC.Dist_I.ToString();
                txtReadA.Text = PLC.Dist_J.ToString();
                txtReadL.Text = PLC.OfSetL.ToString();
                txtReadR.Text = PLC.OfSetR.ToString();
            }

            lbl_D586.Text = PLC.DM_586.ToString("X4"); lbl_D587.Text = PLC.DM_587.ToString("X4");
            lbl_D588.Text = PLC.DM_588.ToString("X4"); lbl_D589.Text = PLC.DM_589.ToString("X4");
            lbl_D590.Text = PLC.DM_590.ToString("X4"); lbl_D591.Text = PLC.DM_591.ToString("X4");
            lbl_D592.Text = PLC.DM_592.ToString("X4"); lbl_D593.Text = PLC.DM_593.ToString("X4");
            lbl_D594.Text = PLC.DM_594.ToString("X4"); lbl_D595.Text = PLC.DM_595.ToString("X4");
            lbl_D596.Text = PLC.DM_596.ToString("X4"); lbl_D597.Text = PLC.DM_597.ToString("X4");
            lbl_D598.Text = PLC.DM_598.ToString("X4"); lbl_D599.Text = PLC.DM_599.ToString("X4");

            o100 = PLC.DI_100; o300 = PLC.DO_300; o530 = PLC.DO_530;
            o101 = PLC.DI_101; o301 = PLC.DO_301; o531 = PLC.DO_531;
            o102 = PLC.DI_102; o302 = PLC.DO_302; o562 = PLC.DO_562;
            o103 = PLC.DI_103; o303 = PLC.DO_303;
            o104 = PLC.DI_104; o304 = PLC.DO_304;
            o105 = PLC.DI_105; o305 = PLC.DO_305;
            o106 = PLC.DI_106; o306 = PLC.DO_306;
            o107 = PLC.DI_107; o307 = PLC.DO_307;
            o108 = PLC.DI_108; o308 = PLC.DO_308;
            o109 = PLC.DI_109; o309 = PLC.DO_309;
            o110 = PLC.DI_110; o310 = PLC.DO_310;
            o111 = PLC.DI_111; o311 = PLC.DO_311;
            o112 = PLC.DI_112; o312 = PLC.DO_312;
            o113 = PLC.DI_113; 
            o114 = PLC.DI_114; 
            o115 = PLC.DI_115;
            o116 = PLC.DI_116;
            o117 = PLC.DI_117;
            o118 = PLC.DI_118;
        }

        private void Distance_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btn_Puts": PLCWriteData(); break;
                case "btn_Gets": PLC_DistShow(); break;
            }
        }
        private void PLCWriteData()
        {
            int Offset_L = int.Parse(txtSetLH.Text);
            int Offset_R = int.Parse(txtSetRH.Text);

            int[] Lent = new int[12];

            Lent[0] = int.Parse(txtSet01.Text) - Offset_L;
            Lent[1] = int.Parse(txtSet02.Text) - Offset_L;
            Lent[2] = int.Parse(txtSet03.Text) - Offset_L;
            Lent[3] = int.Parse(txtSet04.Text) - Offset_L;
            Lent[4] = int.Parse(txtSet05.Text) - Offset_L;
            Lent[5] = int.Parse(txtSet06.Text) - Offset_L;
            Lent[6] = int.Parse(txtSet07.Text) - Offset_L;
            Lent[7] = int.Parse(txtSet08.Text) - Offset_L;
            Lent[8] = int.Parse(txtSet09.Text) - Offset_L;
            Lent[9] = int.Parse(txtSet10.Text) - Offset_L;
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
                if ((Lent[00] == PLC.Dist_A) & (Lent[01] == PLC.Dist_B) & 
                    (Lent[02] == PLC.Dist_C) & (Lent[03] == PLC.Dist_D) & 
                    (Lent[04] == PLC.Dist_E) & (Lent[05] == PLC.Dist_F) & 
                    (Lent[06] == PLC.Dist_G) & (Lent[07] == PLC.Dist_H) & 
                    (Lent[08] == PLC.Dist_I) & (Lent[09] == PLC.Dist_J) &
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

            PLC_DistShow();
        }
        private void PLC_DistShow()
        {
            txtSet01.Text = (PLC.HexToDecimal(PLC.DM_533.ToString("X4") + PLC.DM_532.ToString("X4")) / PLC.LentGain).ToString();
            txtSet02.Text = (PLC.HexToDecimal(PLC.DM_535.ToString("X4") + PLC.DM_534.ToString("X4")) / PLC.LentGain).ToString();
            txtSet03.Text = (PLC.HexToDecimal(PLC.DM_537.ToString("X4") + PLC.DM_536.ToString("X4")) / PLC.LentGain).ToString();
            txtSet04.Text = (PLC.HexToDecimal(PLC.DM_539.ToString("X4") + PLC.DM_538.ToString("X4")) / PLC.LentGain).ToString();
            txtSet05.Text = (PLC.HexToDecimal(PLC.DM_541.ToString("X4") + PLC.DM_540.ToString("X4")) / PLC.LentGain).ToString();
            txtSet06.Text = (PLC.HexToDecimal(PLC.DM_543.ToString("X4") + PLC.DM_542.ToString("X4")) / PLC.LentGain).ToString();
            txtSet07.Text = (PLC.HexToDecimal(PLC.DM_545.ToString("X4") + PLC.DM_544.ToString("X4")) / PLC.LentGain).ToString();
            txtSet08.Text = (PLC.HexToDecimal(PLC.DM_547.ToString("X4") + PLC.DM_546.ToString("X4")) / PLC.LentGain).ToString();
            txtSet09.Text = (PLC.HexToDecimal(PLC.DM_549.ToString("X4") + PLC.DM_548.ToString("X4")) / PLC.LentGain).ToString();
            txtSet10.Text = (PLC.HexToDecimal(PLC.DM_551.ToString("X4") + PLC.DM_550.ToString("X4")) / PLC.LentGain).ToString();
            txtSetLH.Text = (PLC.HexToDecimal(PLC.DM_553.ToString("X4") + PLC.DM_552.ToString("X4")) / PLC.LentGain).ToString();
            txtSetRH.Text = (PLC.HexToDecimal(PLC.DM_555.ToString("X4") + PLC.DM_554.ToString("X4")) / PLC.LentGain).ToString();
        }

        private void Ret_LabelSet(Label lbl, bool Onf, byte pMode)
        {
            try
            {

                switch (pMode)
                {
                    case 0:
                        if (lbl.Text.Substring(lbl.Text.Length - 2, 2).ToUpper() == "LS")
                        {
                            lbl.BackColor = (Onf ? Color.Lime : Color.White);
                            lbl.ForeColor = (Onf ? Color.Red : Color.Black);
                        }
                        else if (lbl.Text.Substring(lbl.Text.Length - 2, 2).ToUpper() == "PB")
                        {
                            lbl.BackColor = (Onf ? Color.Yellow : Color.White);
                            lbl.ForeColor = (Onf ? Color.Black : Color.Black);
                        }
                        else
                        {
                            if (lbl.Text.Substring(lbl.Text.Length - 3, 3).ToUpper() == "RUN")
                            {
                                lbl.BackColor = (Onf ? Color.Yellow : Color.White);
                                lbl.ForeColor = (Onf ? Color.Red : Color.Black);
                            }
                            else
                            {
                                if (lbl.Text.Substring(lbl.Text.Length - 3, 3).ToUpper() == "ERROR")
                                {
                                    lbl.BackColor = (Onf ? Color.Red : Color.White);
                                    lbl.ForeColor = (Onf ? Color.Black : Color.Black);
                                }
                                else
                                {
                                    lbl.BackColor = (Onf ? Color.Green : Color.White);
                                    lbl.ForeColor = (Onf ? Color.Yellow : Color.Black);
                                }
                            }
                        }
                        break;

                    case 1: lbl.BackColor = (Onf ? Color.Red : Color.White);
                        lbl.ForeColor = (Onf ? Color.Yellow : Color.Black); break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CreateILabel(GroupBox gBox, string name, Label[] lbl, string[] txt, bool Onf)
        {
            for (int i = 0; i < lbl.Length; i++)
            {
                // 새 인스턴스 생성
                lbl[i] = new Label();

                // 기본옵션 설정
                lbl[i].Name = name + "[" + i.ToString() + "]"; // 텍스트상자에 이름을 부여한다. 예) txt1
                lbl[i].Text = txt[i];   //name + " " + i.ToString();
                lbl[i].Width = 120;
                lbl[i].Height = 17;
                lbl[i].AutoSize = false;
                lbl[i].BorderStyle = BorderStyle.Fixed3D;
                lbl[i].TextAlign = ContentAlignment.MiddleCenter;
                lbl[i].Tag = name;
                lbl[i].Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 8.0F);
                if (txt[i].IndexOf("Spair") > -1)
                {
                    lbl[i].BackColor = Color.Gray;
                }
                else
                {
                    lbl[i].BackColor = Color.White;
                }
                lbl[i].ForeColor = Color.Black;

                // 그룹박스에 텍스트상자 추가
                gBox.Controls.Add(lbl[i]);

                // 이벤트 등록
                lbl[i].MouseLeave += new EventHandler(fomDebug_MouseLeave);
                lbl[i].MouseHover += new EventHandler(fomDebug_MouseHover);

                if (Onf)
                {
                    lbl[i].DoubleClick += new EventHandler(fomDebug_DoubleClick);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (i == 0)
                {
                    SetBox(lbl[i], 1, 20, 120);
                    SetBox(lbl[i + 8], 122, 20, 120);
                }
                else
                {
                    SetBox(lbl[i], 1, lbl[i - 1].Top + 18, 120);
                    SetBox(lbl[i + 8], 122, lbl[i + 8 - 1].Top + 18, 120);
                }
            }
        }

        private void CreateOLabel(GroupBox gBox, string name, Label[] lbl, string[] txt, bool Onf)
        {
            for (int i = 0; i < lbl.Length; i++)
            {
                // 새 인스턴스 생성
                lbl[i] = new Label();

                // 기본옵션 설정
                lbl[i].Name = name + "[" + i.ToString() + "]"; // 텍스트상자에 이름을 부여한다. 예) txt1
                lbl[i].Text = txt[i];   //name + " " + i.ToString();
                lbl[i].Width = 120;
                lbl[i].Height = 17;
                lbl[i].AutoSize = false;
                lbl[i].BorderStyle = BorderStyle.Fixed3D;
                lbl[i].TextAlign = ContentAlignment.MiddleCenter;
                lbl[i].Tag = name;
                lbl[i].Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 8.0F);
                lbl[i].BackColor = Color.White;
                lbl[i].ForeColor = Color.Black;
                if (Onf) lbl[i].Cursor = Cursors.Hand;
                
                // 그룹박스에 텍스트상자 추가
                gBox.Controls.Add(lbl[i]);

                // 이벤트 등록
                lbl[i].MouseLeave += new EventHandler(fomDebug_MouseLeave);
                lbl[i].MouseHover += new EventHandler(fomDebug_MouseHover);

                if (Onf)
                {
                    lbl[i].DoubleClick += new EventHandler(fomDebug_DoubleClick);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (i == 0)
                {
                    SetBox(lbl[i], 1, 20, 120);
                    SetBox(lbl[i + 8], 122, 20, 120);
                }
                else
                {
                    SetBox(lbl[i], 1, lbl[i - 1].Top + 18, 120);
                    SetBox(lbl[i + 8], 122, lbl[i + 8 - 1].Top + 18, 120);
                }
            }
        }

        private void CreateOLabel_1(GroupBox gBox, string name, Label[] lbl, string[] txt, bool Onf)
        {
            for (int i = 0; i < lbl.Length; i++)
            {
                // 새 인스턴스 생성
                lbl[i] = new Label();

                // 기본옵션 설정
                lbl[i].Name = name + "[" + i.ToString() + "]"; // 텍스트상자에 이름을 부여한다. 예) txt1
                lbl[i].Text = txt[i];   //name + " " + i.ToString();
                lbl[i].Width = 120;
                lbl[i].Height = 21;
                lbl[i].AutoSize = false;
                lbl[i].BorderStyle = BorderStyle.Fixed3D;
                lbl[i].TextAlign = ContentAlignment.MiddleCenter;
                lbl[i].Tag = name;
                lbl[i].Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 8.0F);
                lbl[i].BackColor = Color.White;
                lbl[i].ForeColor = Color.Black;
                if (Onf) lbl[i].Cursor = Cursors.Hand;

                // 그룹박스에 텍스트상자 추가
                gBox.Controls.Add(lbl[i]);

                // 이벤트 등록
                lbl[i].MouseLeave += new EventHandler(fomDebug_MouseLeave);
                lbl[i].MouseHover += new EventHandler(fomDebug_MouseHover);

                if (Onf)
                {
                    lbl[i].DoubleClick += new EventHandler(fomDebug_DoubleClick);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (i == 0)
                {
                    SetBox(lbl[i], 1, 20, 120);
                    SetBox(lbl[i + 8], 122, 20, 120);
                }
                else
                {
                    SetBox(lbl[i], 1, lbl[i - 1].Top + 25, 120);
                    SetBox(lbl[i + 8], 122, lbl[i + 8 - 1].Top + 25, 120);
                }
            }
        }
        #region 이벤트 등록
        void fomDebug_MouseHover(object sender, EventArgs e)
        {
            toolTip.Show(((Label)sender).Text, ((Label)sender), 0, -((Label)sender).Height);
        }
        void fomDebug_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Hide(((Label)sender));
        }
        void fomDebug_DoubleClick(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;

            if (lbl.Text.IndexOf("Spair") > -1) return;

            string name = Ret_CtrlName(lbl.Name);
            int Idx = Ret0CtrlName(lbl.Name);
            int Val = 0;

            switch (lbl.Tag.ToString())
            {
                case "D312": PLC.DOB312[Idx] = !PLC.DOB312[Idx];
                    lbo_312[Idx].BackColor = (PLC.DOB312[Idx] ? Color.Red : Color.White);
                    lbo_312[Idx].ForeColor = (PLC.DOB312[Idx] ? Color.Yellow : Color.Black);

                    for (int cnt = 0; cnt < 16; cnt++)
                    {
                        Val = Val + (PLC.DOB312[cnt] ? H2Y.BitA[cnt] : 0);
                    }

                    PLC.DO.PC_Standy = PLC.DOB312[00]; //D311[00] PC 준비
                    PLC.DO.TestReady = PLC.DOB312[01]; //D311[01] Test 준비
                    PLC.DO.TestStart = PLC.DOB312[02]; //D311[02] Test 시작
                    PLC.DO.Test__End = PLC.DOB312[03]; //D311[03] Test 종료
                    PLC.DO.Spd_5kmph = PLC.DOB312[04]; //D311[04] 속도 5km/h 이상
                    PLC.DO.Test_Sett = PLC.DOB312[05]; //D311[05] Test 세팅(위치, 속도)
                    PLC.DO.Home_Move = PLC.DOB312[06]; //D311[06] 홈으로 이동

                    PLC.DO.DistReset = PLC.DOB312[08]; //D311[08] 거리 갱신
                    PLC.DO.CalAirSol = PLC.DOB312[09]; //D311[09] 교정 솔
                    PLC.DO.CalIndiOn = PLC.DOB312[10]; //D312[10] Calibration Indicter Power ON
                    PLC.DO.BT_Mot_On = PLC.DOB312[12]; //D312[12] BT-Motor ON
                    PLC.DO.BT_LiftUp = PLC.DOB312[13]; //D312[13] BT-Lift UP
                    PLC.DO.BT_LiftDn = PLC.DOB312[14]; //D312[14] BT-LIFT Down
                    PLC.DO.CalIndi_O = PLC.DOB312[15]; //D311[15] 인디게이터 영점

                    gbx_D312.Text = "D312 - " + Val.ToString() + " (HEX : " + Val.ToString("X2") + ")";
                    PLC.PLC_312_Puts();
                    break;

                case "D530": PLC.DOB530[Idx] = !PLC.DOB530[Idx];
                    lbo_530[Idx].BackColor = (PLC.DOB530[Idx] ? Color.Red : Color.White);
                    lbo_530[Idx].ForeColor = (PLC.DOB530[Idx] ? Color.Yellow : Color.Black);

                    for (int cnt = 0; cnt < 16; cnt++)
                    {
                        Val = Val + (PLC.DOB530[cnt] ? H2Y.BitA[cnt] : 0);
                    }

                    PLC.DO.FLMot_Stt = PLC.DOB530[00]; PLC.DO.FRMot_Stt = PLC.DOB530[08];
                    PLC.DO.FLMotSync = PLC.DOB530[01]; PLC.DO.FRMotSync = PLC.DOB530[09];
                    PLC.DO.FLMotStop = PLC.DOB530[02]; PLC.DO.FRMotStop = PLC.DOB530[10];
                    PLC.DO.FLMotPark = PLC.DOB530[03]; PLC.DO.FRMotPark = PLC.DOB530[11];

                    gbx_D530.Text = "D530 - " + Val.ToString() + " (HEX : " + Val.ToString("X2") + ")";
                    PLC.PLC_Put_D500();
                    break;

                case "D531": PLC.DOB531[Idx] = !PLC.DOB531[Idx];
                    lbo_531[Idx].BackColor = (PLC.DOB531[Idx] ? Color.Red : Color.White);
                    lbo_531[Idx].ForeColor = (PLC.DOB531[Idx] ? Color.Yellow : Color.Black);

                    for (int cnt = 0; cnt < 16; cnt++)
                    {
                        Val = Val + (PLC.DOB531[cnt] ? H2Y.BitA[cnt] : 0);
                    }

                    PLC.DO.RLMot_Stt = PLC.DOB531[00]; PLC.DO.RRMot_Stt = PLC.DOB531[08];
                    PLC.DO.RLMotSync = PLC.DOB531[01]; PLC.DO.RRMotSync = PLC.DOB531[09];
                    PLC.DO.RLMotStop = PLC.DOB531[02]; PLC.DO.RRMotStop = PLC.DOB531[10];
                    PLC.DO.RLMotPark = PLC.DOB531[03]; PLC.DO.RRMotPark = PLC.DOB531[11];
                                                       PLC.DO.Door_Open = PLC.DOB531[14];
                                                       PLC.DO.DoorClose = PLC.DOB531[15];
                    
                    gbx_D531.Text = "D531 - " + Val.ToString() + " (HEX : " + Val.ToString("X2") + ")";
                    PLC.PLC_Put_D500();
                    break;

                case "D562": PLC.DOB562[Idx] = !PLC.DOB562[Idx];
                    lbo_562[Idx].BackColor = (PLC.DOB562[Idx] ? Color.Red : Color.White);
                    lbo_562[Idx].ForeColor = (PLC.DOB562[Idx] ? Color.Yellow : Color.Black);

                    for (int cnt = 0; cnt < 16; cnt++)
                    {
                        Val = Val + (PLC.DOB562[cnt] ? H2Y.BitA[cnt] : 0);
                    }

                    PLC.DO.Vehicle01 = PLC.DOB562[00];
                    PLC.DO.Vehicle02 = PLC.DOB562[01];
                    PLC.DO.Vehicle03 = PLC.DOB562[02];
                    PLC.DO.Vehicle04 = PLC.DOB562[03];
                    PLC.DO.Vehicle05 = PLC.DOB562[04];
                    PLC.DO.Vehicle06 = PLC.DOB562[05];
                    PLC.DO.Vehicle07 = PLC.DOB562[06];
                    PLC.DO.Vehicle08 = PLC.DOB562[07];
                    PLC.DO.Vehicle09 = PLC.DOB562[08];
                    PLC.DO.Vehicle10 = PLC.DOB562[09];
                    PLC.DO.PC__Start = PLC.DOB562[10];
                    PLC.DO.PC___Stop = PLC.DOB562[11];
                    PLC.DO.PC_Cancel = PLC.DOB562[12];
                    //PLC.DOB562[13]
                    PLC.DO.PC_StadBy = PLC.DOB562[14];
                    PLC.DO.PC__Reset = PLC.DOB562[15]; 
                    
                    gbx_D562.Text = "D562 - " + Val.ToString() + " (HEX : " + Val.ToString("X2") + ")";
                    PLC.PLC_Put_D562();
                    break;
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btn_Post": break;
                case "btnFFL_R": break;
                case "btnFFR_R": break;
                case "btn_FL_M": break;
                case "btn_FR_M": break;
                case "btnFRL_R": break;
                case "btnFRR_R": break;
                case "btnRFL_R": break;
                case "btnRFR_R": break;
                case "btn_RL_M": break;
                case "btn_RR_M": break;
                case "btn_Flap": break;
            }
        }
        private void chkSensor_CheckedChanged(object sender, EventArgs e)
        {
            switch (((CheckBox)sender).Name)
            {
                case "chkEnter": TSet.SST_Enter = chkEnter.Checked; break;
                case "chkGoOut": TSet.SST_GoOut = chkGoOut.Checked; break;
                case "chkWgtOn": TSet.PHO_Brake = chkWgtOn.Checked; break;
                case "chk___Up": TSet.BT_LiftUp = chk___Up.Checked; break;
                case "chk_Down": TSet.BT_LiftDn = chk_Down.Checked; break;
                case "chkMotOn": TSet.BT_MotRun = chkMotOn.Checked; break;
            }
        }
        #endregion

        private string Ret_CtrlName(string pName)
        {
            int stt = pName.IndexOf("[");
            int ett = pName.IndexOf("]");

            if (stt >= ett) return "";

            return pName.Substring(0, stt - 1); ;
        }
        private int Ret0CtrlName(string pName)
        {
            int stt = pName.IndexOf("[");
            int ett = pName.IndexOf("]");

            if (stt >= ett) return 0;

            string val = pName.Substring(stt + 1, (ett - stt) - 1);
            int Idx = Convert.ToInt32(val);

            return Idx;
        }

        // 텍스트 박스 위치 설정
        private void SetBox(Control control, int Left, int Top, int Width)
        {
            control.Left = Left;
            control.Top = Top;
            control.Width = Width;
        }
        #endregion

        #region Simulation Test
        private void chkSimul_Click(object sender, EventArgs e)
        {
            TSet.SimulOnf = (chkSimul.Checked ? true : false);
        }
        private void lst_Step_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSet.Simul_No = lst_Step.SelectedIndex;
        }
        private void btnSimul_Click(object sender, EventArgs e)
        {
            Double RetValue = 0;

            switch (((Button)sender).Name)
            {
                case "btn_Drag": Order___Drag(); break;
                case "btnApend": Order__Apend(); break;
                case "btnStart": Order__Start(); break;
                case "btn_Stop": Order___Stop(); break;
                case "btn___Up": Order___Stop(); break;
                case "btn_Down": Order___Stop(); break;
                case "btnSpeed": RetValue = ConvertSpeed(txtValue.Text); MessageBox.Show(RetValue.ToString()); break;
                case "btn_RPMs": RetValue = Convert_RPMs(txtValue.Text); MessageBox.Show(RetValue.ToString()); break;
            }
        }

        private void Order___Drag()
        {
            TSet.VirtualL_B = Convert.ToSingle(txtLDrag.Text);
            TSet.VirtualR_B = Convert.ToSingle(txtRDrag.Text);
        }

        private void Order__Apend()
        {
            TSet.Virtual_FL = Convert.ToSingle(txt_W_FL.Text);
            TSet.Virtual_FR = Convert.ToSingle(txt_W_FR.Text);
            TSet.Virtual_RL = Convert.ToSingle(txt_W_RL.Text);
            TSet.Virtual_RR = Convert.ToSingle(txt_W_RR.Text);
            TSet.VirtualRPM = Convert.ToSingle(txt__RPM.Text);
            TSet.VirtualPTS = Convert.ToSingle(txt__PTS.Text);
            TSet.VirtualSST = Convert.ToSingle(txt__SST.Text);
            TSet.VirtualL_W = Convert.ToSingle(txtL_Wgt.Text);
            TSet.VirtualR_W = Convert.ToSingle(txtR_Wgt.Text);
            TSet.VirtualL_B = Convert.ToSingle(txtL_Brk.Text);
            TSet.VirtualR_B = Convert.ToSingle(txtR_Brk.Text);
        }
        
        private void Order__Start()
        {
            string Now_Date = DateTime.Now.ToString("yyyyMMdd");
            int Number = main.DB_All.DB_Info.Max_No(Now_Date) + 1;
            string AcptNo = Now_Date + Number.ToString("00000");

            Test = new cls_Test(main);
            Test.Test_Running(AcptNo);
        }

        private void Order___Stop()
        {
            TSet.TestStop = true;
        }
        private void Speed_Change(int pMode)
        {
            float value = float.Parse(txt_UpDn.Text);

            float W_FL = float.Parse(txt_W_FL.Text);
            float W_FR = float.Parse(txt_W_FR.Text);
            float W_RL = float.Parse(txt_W_RL.Text);
            float W_RR = float.Parse(txt_W_RR.Text);

            switch (pMode)
            {
                case 0:
                    W_FL += value;
                    W_FR += value;
                    W_RL += value;
                    W_RR += value;
                    break;

                case 1: 
                    W_FL -= value;
                    W_FR -= value;
                    W_RL -= value;
                    W_RR -= value;
                    break;
            }

            txt_W_FL.Text = W_FL.ToString();
            txt_W_FR.Text = W_FR.ToString();
            txt_W_RL.Text = W_RL.ToString();
            txt_W_RR.Text = W_RR.ToString();
        }
        
        private Double ConvertSpeed(string pVal)
        {
            Double DiaM = Convert.ToSingle(txt_DiaM.Text);
            Double MinM = (DiaM * Math.PI) * Convert.ToSingle(pVal);
            Double Sped = H2Y.DVD(H2Y.DVD(MinM * 60, 1000), 1000);

            return Sped;
        }
        private Double Convert_RPMs(string pVal)
        {
            Double DiaM = Convert.ToSingle(txt_DiaM.Text);
            Double Sped = H2Y.DVD(Convert.ToSingle(pVal) * 1000 * 1000, 60);
            Double RPMs = H2Y.DVD(Sped, (DiaM * Math.PI));

            return RPMs;
        }
        #endregion

        #region AD Calibration
        private void Cal_DataShow()
        {
            txtAFilt.Text = PSet.Av_Filt.ToString();
            txtSFilt.Text = PSet.St_Filt.ToString();
            cbo_Filt.SelectedIndex = PSet.Filter;

            lbl0Zero.Text = PSet.CH0Zero.ToString();
            lbl1Zero.Text = PSet.CH1Zero.ToString();
            lbl2Zero.Text = PSet.CH2Zero.ToString();
            lbl3Zero.Text = PSet.CH3Zero.ToString();
            lbl4Zero.Text = PSet.CH4Zero.ToString();
            lbl5Zero.Text = PSet.CH5Zero.ToString();

            txt0Span.Text = PSet.CH0Span.ToString("0.00000");
            txt1Span.Text = PSet.CH1Span.ToString("0.00000");
            txt2Span.Text = PSet.CH2Span.ToString("0.00000");
            txt3Span.Text = PSet.CH3Span.ToString("0.00000");
            txt4Span.Text = PSet.CH4Span.ToString("0.00000");
            txt5Span.Text = PSet.CH5Span.ToString("0.00000");
        }
        public void ABSB_Message(string msg)
        {
            this.Invoke(new MethodInvoker(delegate { lst_ABSB.Items.Insert(0, msg); }));
        }

        private void lbl_Zero_DoubleClick(object sender, EventArgs e)
        {
            switch (((Label)sender).Name)
            {
                case "lbl0Zero": PSet.CH0Zero = PSet.CH0Scan; lbl0Zero.Text = PSet.CH0Zero.ToString(); break;
                case "lbl1Zero": PSet.CH1Zero = PSet.CH1Scan; lbl1Zero.Text = PSet.CH1Zero.ToString(); break;
                case "lbl2Zero": PSet.CH2Zero = PSet.CH2Scan; lbl2Zero.Text = PSet.CH2Zero.ToString(); break;
                case "lbl3Zero": PSet.CH3Zero = PSet.CH3Scan; lbl3Zero.Text = PSet.CH3Zero.ToString(); break;
                case "lbl4Zero": PSet.CH4Zero = PSet.CH4Scan; lbl4Zero.Text = PSet.CH4Zero.ToString(); break;
                case "lbl5Zero": PSet.CH5Zero = PSet.CH5Scan; lbl5Zero.Text = PSet.CH5Zero.ToString(); break;
            }
        }

        private void lblValue_DoubleClick(object sender, EventArgs e)
        {
            fom_PsWd pass = new fom_PsWd(float.Parse(((Label)sender).Text));

            try
            {
                pass.ShowDialog();

                switch (((Label)sender).Name)
                {
                    case "lbl0_Val": PSet.CH0Span = pass.NewValue / PSet.CH0Last; txt0Span.Text = PSet.CH0Span.ToString("0.00000"); break;
                    case "lbl1_Val": PSet.CH1Span = pass.NewValue / PSet.CH1Last; txt1Span.Text = PSet.CH1Span.ToString("0.00000"); break;
                    case "lbl2_Val": PSet.CH2Span = pass.NewValue / PSet.CH2Last; txt2Span.Text = PSet.CH2Span.ToString("0.00000"); break;
                    case "lbl3_Val": PSet.CH3Span = pass.NewValue / PSet.CH3Last; txt3Span.Text = PSet.CH3Span.ToString("0.00000"); break;
                    case "lbl4_Val": PSet.CH4Span = pass.NewValue / PSet.CH4Last; txt4Span.Text = PSet.CH4Span.ToString("0.00000"); break;
                    case "lbl5_Val": PSet.CH5Span = pass.NewValue / PSet.CH5Last; txt5Span.Text = PSet.CH5Span.ToString("0.00000"); break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                PSet.Av_Filt = int.Parse(txtAFilt.Text);
                PSet.St_Filt = int.Parse(txtSFilt.Text);
                PSet.Filter = cbo_Filt.SelectedIndex;

                PSet.CH0Zero = H2Y.toInt(lbl0Zero.Text);
                PSet.CH1Zero = H2Y.toInt(lbl1Zero.Text);
                PSet.CH2Zero = H2Y.toInt(lbl2Zero.Text);
                PSet.CH3Zero = H2Y.toInt(lbl3Zero.Text);
                PSet.CH4Zero = H2Y.toInt(lbl4Zero.Text);
                PSet.CH5Zero = H2Y.toInt(lbl5Zero.Text);

                PSet.CH0Span = H2Y.toFloat(txt0Span.Text);
                PSet.CH1Span = H2Y.toFloat(txt1Span.Text);
                PSet.CH2Span = H2Y.toFloat(txt2Span.Text);
                PSet.CH3Span = H2Y.toFloat(txt3Span.Text);
                PSet.CH4Span = H2Y.toFloat(txt4Span.Text);
                PSet.CH5Span = H2Y.toFloat(txt5Span.Text);

                PSet.Prog_CalMake();
            }
            catch (Exception ex)
            {
                Cal_DataShow();
            }
        }
        #endregion

        #region Motor Controller
        private void btnFSync_Click(object sender, EventArgs e)
        {
            if (NI.Loss.FL.Speed > 0.5f) return;
            if (NI.Loss.FR.Speed > 0.5f) return;
            if (NI.Loss.RL.Speed > 0.5f) return;
            if (NI.Loss.RR.Speed > 0.5f) return;

            PLC.DO.FLMot_Stt = false; PLC.DO.FRMot_Stt = false;
            PLC.DO.FLMotSync = false; PLC.DO.FRMotSync = false;
            PLC.DO.FLMotStop = false; PLC.DO.FRMotStop = false;
            PLC.DO.FLMotPark = false; PLC.DO.FRMotPark = false;

            PLC.DO.RLMot_Stt = true; PLC.DO.RRMot_Stt = true;
            PLC.DO.RLMotSync = true; PLC.DO.RRMotSync = true;
            PLC.DO.RLMotStop = false; PLC.DO.RRMotStop = false;
            PLC.DO.RLMotPark = false; PLC.DO.RRMotPark = false;

            PLC.PLC_Put_D500();
        }
        private void btnRSync_Click(object sender, EventArgs e)
        {
            if (NI.Loss.FL.Speed > 0.5f) return;
            if (NI.Loss.FR.Speed > 0.5f) return;
            if (NI.Loss.RL.Speed > 0.5f) return;
            if (NI.Loss.RR.Speed > 0.5f) return;

            PLC.DO.FLMot_Stt = true; PLC.DO.FRMot_Stt = true;
            PLC.DO.FLMotSync = true; PLC.DO.FRMotSync = true;
            PLC.DO.FLMotStop = false; PLC.DO.FRMotStop = false;
            PLC.DO.FLMotPark = false; PLC.DO.FRMotPark = false;

            PLC.DO.RLMot_Stt = false; PLC.DO.RRMot_Stt = false;
            PLC.DO.RLMotSync = false; PLC.DO.RRMotSync = false;
            PLC.DO.RLMotStop = false; PLC.DO.RRMotStop = false;
            PLC.DO.RLMotPark = false; PLC.DO.RRMotPark = false;

            PLC.PLC_Put_D500();
        }
        private void btn__WSS_Click(object sender, EventArgs e)
        {
            if (NI.Loss.FL.Speed > 0.5f) return;
            if (NI.Loss.FR.Speed > 0.5f) return;
            if (NI.Loss.RL.Speed > 0.5f) return;
            if (NI.Loss.RR.Speed > 0.5f) return;

            PLC.DO.FLMot_Stt = true;  PLC.DO.FRMot_Stt = true;
            PLC.DO.FLMotSync = false; PLC.DO.FRMotSync = false;
            PLC.DO.FLMotStop = false; PLC.DO.FRMotStop = false;
            PLC.DO.FLMotPark = false; PLC.DO.FRMotPark = false;

            PLC.DO.RLMot_Stt = true;  PLC.DO.RRMot_Stt = true;
            PLC.DO.RLMotSync = false; PLC.DO.RRMotSync = false;
            PLC.DO.RLMotStop = false; PLC.DO.RRMotStop = false;
            PLC.DO.RLMotPark = false; PLC.DO.RRMotPark = false;

            PLC.PLC_Put_D500();
        }
        private void btnBrake_Click(object sender, EventArgs e)
        {
            PLC.DO.FLMot_Stt = false; PLC.DO.FRMot_Stt = false;
            PLC.DO.FLMotSync = false; PLC.DO.FRMotSync = false;
            PLC.DO.FLMotStop = true; PLC.DO.FRMotStop = true;
            PLC.DO.FLMotPark = false; PLC.DO.FRMotPark = false;

            PLC.DO.RLMot_Stt = false; PLC.DO.RRMot_Stt = false;
            PLC.DO.RLMotSync = false; PLC.DO.RRMotSync = false;
            PLC.DO.RLMotStop = true; PLC.DO.RRMotStop = true;
            PLC.DO.RLMotPark = false; PLC.DO.RRMotPark = false;

            PLC.PLC_Put_D500();
        }
        private void btnAStop_Click(object sender, EventArgs e)
        {
            PLC.DO.FLMot_Stt = false; PLC.DO.FRMot_Stt = false;
            PLC.DO.FLMotSync = false; PLC.DO.FRMotSync = false;
            PLC.DO.FLMotStop = false; PLC.DO.FRMotStop = false;
            PLC.DO.FLMotPark = false; PLC.DO.FRMotPark = false;

            PLC.DO.RLMot_Stt = false; PLC.DO.RRMot_Stt = false;
            PLC.DO.RLMotSync = false; PLC.DO.RRMotSync = false;
            PLC.DO.RLMotStop = false; PLC.DO.RRMotStop = false;
            PLC.DO.RLMotPark = false; PLC.DO.RRMotPark = false;

            PLC.PLC_Put_D500();
        }
        #endregion

        #region ECU Check
        private void ECU_NeoVI()
        {
            if (NeoVI.IsOpen)
            {
                NeoVI.Device_Close();
            }
            else
            {
                NeoVI.Device_Open();
            }
        }

        public void Puts_Message(string msg)
        {
            this.Invoke(new MethodInvoker(delegate { txt_Puts.Text = msg; }));
        }
        public void Gets_Message(string msg)
        {
            this.Invoke(new MethodInvoker(delegate { txt_Gets.Text = msg; }));
        }
        public void ECUs_Message(string msg)
        {
            string now = DateTime.Now.ToString(H2Y.format3Time);

            this.Invoke(new MethodInvoker(delegate { lst_ECUs.Items.Insert(0, "[" + now + "] " + msg); }));
            //this.Invoke(new MethodInvoker(delegate { lst_ECUs.Items.Add("[" + now + "] " + msg); }));
        }
        public void Flag_Message(string Ret)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lbl_Msg0.Text = Ret;
                lbl_Msg0.BackColor = Ret.ToUpper() == "TRUE" ? Color.Lime : Color.Red;
            }));
        }
        public void Flag_Message(string Ret, string Msgs, int cnt)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lbl_Msg0.Text = Ret;
                lbl_Msg1.Text = Msgs + " / " + cnt.ToString();

                if (Ret.ToUpper() == "TRUE")
                {
                    lbl_Msg0.BackColor = Color.Lime;
                    if (int.Parse(Msgs) < cnt)
                    {
                        lbl_Msg1.BackColor = Color.Lime;
                    }
                    else
                    {
                        lbl_Msg1.BackColor = Color.Red;
                    }
                }
                else
                {
                    lbl_Msg0.BackColor = Color.Red;
                    lbl_Msg1.BackColor = Color.Red;
                }

                lblError.Text = NeoVI.Read_Error.ToString();
                lblCount.Text = NeoVI.Read_Count.ToString();

            }));

           
        }
        public void Ret4Messages(string msg1, string msg2, string msg3, string msg4, string msg5)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                if (msg1 != "") lbl_Msg1.Text = msg1;
                if (msg2 != "") lbl_Msg2.Text = msg2;
                if (msg3 != "") lbl_Msg3.Text = msg3;
                if (msg4 != "") lbl_Msg4.Text = msg4;
                if (msg5 != "") lbl_Msg5.Text = msg5;
            }));
        }
        
        private void cbo_ECUs_SelectedIndexChanged(object sender, EventArgs e)
        {
            ECUs.ECU_Selector(cbo_ECUs.Items[cbo_ECUs.SelectedIndex].ToString());
            txtSetID.Text = ECUs.Set_ID;
            txtGetID.Text = ECUs.Ret_ID;

            btnECU13.Text = "";
            btnECU14.Text = "";
            btnECU15.Text = "";
            btnECU16.Text = "";
            btnECU17.Text = "";
            btnECU18.Text = "";
            btnECU19.Text = "";
            btnECU20.Text = "";
            btnECU21.Text = "";


            cboIndex.Items.Clear();
            switch (ECUs.ECU)
            {
                case ECUs.Mobis___AD:
                    #region ESP Step
                    cboIndex.Items.Add("1. ABS Pressure release FL");
                    cboIndex.Items.Add("2. ABS Pressure release FR");
                    cboIndex.Items.Add("3. ABS Pressure release RL");
                    cboIndex.Items.Add("4. ABS Pressure release RR");
                    cboIndex.Items.Add("5. ABS Pump Motor On for 2 Seconds");
                    break;
                    #endregion
                
                case ECUs.Mobis__DN8:
                    #region ESP Step
                    cboIndex.Items.Add("1. ABS Pressure release FL");
                    cboIndex.Items.Add("2. ABS Pressure release FR");
                    cboIndex.Items.Add("3. ABS Pressure release RL");
                    cboIndex.Items.Add("4. ABS Pressure release RR");
                    cboIndex.Items.Add("5. ABS Pump Motor On for 2 Seconds");
                    break;
                    #endregion

                case ECUs.Mobis___FL:
                    #region ESP Step
                    cboIndex.Items.Add("1. ABS Pressure release FL");
                    cboIndex.Items.Add("2. ABS Pressure release FR");
                    cboIndex.Items.Add("3. ABS Pressure release RL");
                    cboIndex.Items.Add("4. ABS Pressure release RR");
                    cboIndex.Items.Add("5. ABS Pump Motor On for 2 Seconds");
                    break;
                    #endregion

                case ECUs.Mando___TL:
                    #region ESP Step
                    cboIndex.Items.Add("1. ABS Pressure release FL");
                    cboIndex.Items.Add("2. ABS Pressure release FR");
                    cboIndex.Items.Add("3. ABS Pressure release RL");
                    cboIndex.Items.Add("4. ABS Pressure release RR");
                    cboIndex.Items.Add("5. ABS Pump Motor On for 2 Seconds");
                    break;
                    #endregion

                case ECUs.Mando___TM:
                    #region ESP Step
                    cboIndex.Items.Add("1. ABS Pressure release FL");
                    cboIndex.Items.Add("2. ABS Pressure release FR");
                    cboIndex.Items.Add("3. ABS Pressure release RL");
                    cboIndex.Items.Add("4. ABS Pressure release RR");
                    cboIndex.Items.Add("5. ABS Pump Motor On for 2 Seconds");
                    break;
                    #endregion
                case ECUs.Mando__HEV:
                    #region ESP Step
                    cboIndex.Items.Add("1. ABS Pressure release FL");
                    cboIndex.Items.Add("2. ABS Pressure release FR");
                    cboIndex.Items.Add("3. ABS Pressure release RL");
                    cboIndex.Items.Add("4. ABS Pressure release RR");
                    cboIndex.Items.Add("5. ABS Pump Motor On for 2 Seconds");
                    break;
                    #endregion
                case ECUs.Mando_NX4H:
                    #region ESP Step
                    cboIndex.Items.Add("1. ABS Pressure release FL");
                    cboIndex.Items.Add("2. ABS Pressure release FR");
                    cboIndex.Items.Add("3. ABS Pressure release RL");
                    cboIndex.Items.Add("4. ABS Pressure release RR");
                    cboIndex.Items.Add("5. ABS Pump Motor On for 2 Seconds");
                    break;
                    #endregion
                case ECUs.Mando_NX4I:
                    #region ESP Step
                    cboIndex.Items.Add("1. ABS Pressure release FL");
                    cboIndex.Items.Add("2. ABS Pressure release FR");
                    cboIndex.Items.Add("3. ABS Pressure release RL");
                    cboIndex.Items.Add("4. ABS Pressure release RR");
                    cboIndex.Items.Add("5. ABS Pump Motor On for 2 Seconds");
                    break;
                    #endregion

                case ECUs.Mobis_LX3H:
                    #region ESP Step
                    cboIndex.Items.Add("1. FR+RL Valves / PSV+MCV#5 (F01E)");
                    cboIndex.Items.Add("2. FR+RL Valves / MCV#2+MCV#6 (F01E)");
                    cboIndex.Items.Add("3. FR+RL Valves / WSV+LSV (F01E)");
                    cboIndex.Items.Add("4. FR+RL Valves / RCV (F01E)");
                    cboIndex.Items.Add("5. HEV Pump Motor On (F01F)");
                    cboIndex.Items.Add("6. Stop");
                    break;
                    #endregion

                case ECUs.Mobis_LX3I:
                    #region ESP Step
                    cboIndex.Items.Add("1. FR+RL Valves / PSV+MCV#5 (F01E)");
                    cboIndex.Items.Add("2. FR+RL Valves / MCV#2+MCV#6 (F01E)");
                    cboIndex.Items.Add("3. FR+RL Valves / WSV+LSV (F01E)");
                    cboIndex.Items.Add("4. FR+RL Valves / RCV (F01E)");
                    cboIndex.Items.Add("5. ICE Pump Motor On (F011)");
                    cboIndex.Items.Add("6. Stop");
                    break;
                    #endregion

                case ECUs.Chery_1box:
                    #region ESP Step
                    cboIndex.Items.Add("1. ABS Pressure FL");
                    cboIndex.Items.Add("2. ABS Release  FL");
                    cboIndex.Items.Add("3. ABS Pressure FR");
                    cboIndex.Items.Add("4. ABS Release  FR");
                    cboIndex.Items.Add("5. ABS Pressure RL");
                    cboIndex.Items.Add("6. ABS Release  RL");
                    cboIndex.Items.Add("7. ABS Pressure RR");
                    cboIndex.Items.Add("8. ABS Release  RR");
                    #endregion

                    btnECU13.Text = "Read_BatteryVoltage";
                    btnECU14.Text = "Comfort_Pulse";
                    btnECU15.Text = "LeakageAndAirTest";
                    btnECU16.Text = "BrakeConditioningTest";
                    btnECU17.Text = "TMC_Without_PFS_Test";
                    btnECU18.Text = "TMC_With_PFS_Test";
                    btnECU19.Text = "MasterCylinder_Test";
                    btnECU20.Text = "SpeedLimitedTest";
                    btnECU21.Text = "Start_WSS_Test";
                    break;
            }
        }
        private void btn_ECUs_Click(object sender, EventArgs e)
        {
            cls_Test TEST = new cls_Test(main);
            ECUs.Set_ID = txtSetID.Text;
            ECUs.Ret_ID = txtGetID.Text;
            
            string send_Str = txt_Puts.Text;
            
            switch (((Button)sender).Name)
            {
                case "btnClear": lst_ECUs.Items.Clear();
                    lbl_Msg0.Text = "";
                    lbl_Msg1.Text = "";
                    lbl_Msg2.Text = "";
                    lbl_Msg3.Text = "";
                    lbl_Msg4.Text = "";
                    lbl_Msg5.Text = "";                         return;
                case "btn_Open": ECU_NeoVI(); lbl_IDSN.Text = NeoVI.SerialNo;   return;
                case "btn_Send": NeoVI.ECU_Clear(); 
                                 NeoVI.Device_Write(send_Str);  return;
                case "btn_Read": NeoVI.Device_Read();           return;
            }

            //lst_ECUs.Items.Add(((Button)sender).Text);
            lst_ECUs.Items.Insert(0, ((Button)sender).Text);
            switch (((Button)sender).Name)
            {
                case "btnECU00": ECUs.SecurityAccess(); break;
                case "btnECU01": ECUs.Start_Communication(); break;
                case "btnECU02": ECUs.Stop_Communication(); break;
                case "btnECU03": ECUs.ECU_Reset(); break;
                case "btnECU04": ECUs.ECU_Identification(); break;
                case "btnECU05": ECUs.Read__DTC(); break;
                case "btnECU06": ECUs.Clear_DTC(); break;
                case "btnECU07": ECUs.Check_Signals(); break;
                case "btnECU08": ECUs.WSS_Test(); break;
                case "btnECU09": ECUs.Dynamic_Step(cboIndex.SelectedIndex + 1); break;
                case "btnECU10": ECUs.Tester_Present(); break;
                case "btnECU11": ECUs.Message_Falg(); break;
                case "btnECU12": ECUs.ESS_LampTest(); break;

                case "btnECU13": CHERY1BOX.Read_BatteryVoltage(); break;
                case "btnECU14": CHERY1BOX.Comfort_Pulse(); break;
                case "btnECU15": CHERY1BOX.LeakageAndAirTest(); break;
                case "btnECU16": CHERY1BOX.BrakeConditioningTest(); break;
                case "btnECU17": CHERY1BOX.TMC_Without_PFS_Test(); break;
                case "btnECU18": CHERY1BOX.TMC_With_PFS_Test(); break;
                case "btnECU19": CHERY1BOX.MasterCylinder_Test(); break;
                case "btnECU20": CHERY1BOX.SpeedLimitedTest(); break;
                case "btnECU21": CHERY1BOX.Start_WSS_Test(); break;

                case "btnECU22":
                    string HexValue = txtInput.Text;
                    string RetValue = CHERY1BOX.Aloirithm(HexValue).ToString("X8");
                    txt_OutP.Text = RetValue.Substring(0, 2) + " " + RetValue.Substring(2, 2) + " " + RetValue.Substring(4, 2) + " " + RetValue.Substring(6, 2); 
                    break;
            }
        }
        private void btn_Dynmic_Click(object sender, EventArgs e)
        {
            #region Dynamic Test

            string SingleIO = "";

            switch (cboIdent.SelectedIndex)
            {
                case 0: SingleIO = "2F F0 21"; break;
                case 1: SingleIO = "2F F0 22"; break;
                case 2: SingleIO = "2F F0 23"; break;
                case 3: SingleIO = "2F F0 24"; break;
                case 4: SingleIO = "2F F0 25"; break;
                case 5: SingleIO = "2F F0 26"; break;
                case 6: SingleIO = "2F F0 27"; break;
                case 7: SingleIO = "2F F0 28"; break;
                case 8: SingleIO = "2F F0 29"; break;
                case 9: SingleIO = "2F F0 2A"; break;
                case 10: SingleIO = "2F F0 2B"; break;
                case 11: SingleIO = "2F F0 2E"; break;
                case 12: SingleIO = "2F F0 2F"; break;
                case 13: SingleIO = "2F F0 31"; break;
                case 14: SingleIO = "2F F0 32"; break;
                case 15: SingleIO = "2F F0 33"; break;
            }

            lst_ECUs.Items.Insert(0, ((Button)sender).Text);
            switch (((Button)sender).Name)
            {
                case "btn_Test01": ECUs.ESP_Step(1); break;  //1. ESP Pressure increase FL (200ms)
                case "btn_Test02": ECUs.ESP_Step(2); break;  //2. ESP Pressure increase FR (200ms)
                case "btn_Test03": ECUs.ESP_Step(3); break;  //3. ESP Pressure increase RL (200ms)
                case "btn_Test04": ECUs.ESP_Step(4); break;  //4. ESP Pressure increase RR (200ms)

                case "btn_Test05": ECUs.Dynamic_Step(1); break;  //1. ABS Pressure release FL (600ms)
                case "btn_Test06": ECUs.Dynamic_Step(2); break;  //2. ABS Pressure release FR (600ms)
                case "btn_Test07": ECUs.Dynamic_Step(3); break;  //3. ABS Pressure release RL (600ms)
                case "btn_Test08": ECUs.Dynamic_Step(4); break;  //4. ABS Pressure release RR (600ms)

                case "btn_Test09": ECUs.Dynamic_Step(5); break;  //5. ABS Pump Motor On for 2 Seconds

                case "btn_Test10": MANDO__TL.Single_IO_Idx(SingleIO, 0); break;
                case "btn_Test11": MANDO__TL.Single_IO_Idx(SingleIO, 1); break;
            }
            H2Y.Sleep(100);
            NeoVI.Device_Read();
            #endregion
        }
        private void lst_ECUs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lst_ECUs.SelectedIndex > -1)
            {
                txt_Gets.Text = lst_ECUs.Items[lst_ECUs.SelectedIndex].ToString();
            }
        }
        #endregion

        #region Nidec Drive
        private void btnMRead_Click(object sender, EventArgs e)
        {
            btnMRead.Enabled = false;
            MDrive__Read();
            btnMRead.Enabled = true;
        }

        private void btnMSave_Click(object sender, EventArgs e)
        {
            btnMSave.Enabled = false;
            MDrive_Write();
            btnMSave.Enabled = true;
        }

        private void btnM_Set_Click(object sender, EventArgs e)
        {
            btnM_Set.Enabled = false;

            txt_FL_0.Text = "0";
            txt_FL_1.Text = PSet.OwnerSpd.ToString();
            txt_FL_2.Text = PSet.Owner_FL.ToString();
            txt_FL_3.Text = PSet.OwnerToq.ToString();
            txt_FL_4.Text = PSet.OwnerPBS.ToString();

            txt_FR_0.Text = "0";
            txt_FR_1.Text = PSet.OwnerSpd.ToString();
            txt_FR_2.Text = PSet.Owner_FR.ToString();
            txt_FR_3.Text = PSet.OwnerToq.ToString();
            txt_FR_4.Text = PSet.OwnerPBS.ToString();

            txt_RL_0.Text = "0";
            txt_RL_1.Text = PSet.OwnerSpd.ToString();
            txt_RL_2.Text = PSet.Owner_RL.ToString();
            txt_RL_3.Text = PSet.OwnerToq.ToString();
            txt_RL_4.Text = PSet.OwnerPBS.ToString();

            txt_RR_0.Text = "0";
            txt_RR_1.Text = PSet.OwnerSpd.ToString();
            txt_RR_2.Text = PSet.Owner_RR.ToString();
            txt_RR_3.Text = PSet.OwnerToq.ToString();
            txt_RR_4.Text = PSet.OwnerPBS.ToString();

            MDrive_Write();

            btnM_Set.Enabled = true;
        }

        private void lbl_FL_DoubleClick(object sender, EventArgs e)
        {
            switch (((Label)sender).Name)
            {
                case "lbl_FL_0": TSet.Nidec_FL.Cal_MD = !TSet.Nidec_FL.Cal_MD; break;
                case "lbl_FL_1": TSet.Nidec_FL.MT_Run = !TSet.Nidec_FL.MT_Run; break;
                case "lbl_FL_2": TSet.Nidec_FL.MTSync = !TSet.Nidec_FL.MTSync; break;
                case "lbl_FL_3": TSet.Nidec_FL.MT_Brk = !TSet.Nidec_FL.MT_Brk; break;
                case "lbl_FL_4": TSet.Nidec_FL.MTPark = !TSet.Nidec_FL.MTPark; break;
            }

            TSet.Nidec_FL.Status = Ret_Status(TSet.Nidec_FL);
            txt_FL_0.Text = TSet.Nidec_FL.Status.ToString();
            MDrive__Show();
        }
        private void lbl_FR_DoubleClick(object sender, EventArgs e)
        {
            switch (((Label)sender).Name)
            {
                case "lbl_FR_0": TSet.Nidec_FR.Cal_MD = !TSet.Nidec_FR.Cal_MD; break;
                case "lbl_FR_1": TSet.Nidec_FR.MT_Run = !TSet.Nidec_FR.MT_Run; break;
                case "lbl_FR_2": TSet.Nidec_FR.MTSync = !TSet.Nidec_FR.MTSync; break;
                case "lbl_FR_3": TSet.Nidec_FR.MT_Brk = !TSet.Nidec_FR.MT_Brk; break;
                case "lbl_FR_4": TSet.Nidec_FR.MTPark = !TSet.Nidec_FR.MTPark; break;
            }

            TSet.Nidec_FR.Status = Ret_Status(TSet.Nidec_FR);
            txt_FR_0.Text = TSet.Nidec_FR.Status.ToString();
            MDrive__Show();
        }
        private void lbl_RL_DoubleClick(object sender, EventArgs e)
        {
            switch (((Label)sender).Name)
            {
                case "lbl_RL_0": TSet.Nidec_RL.Cal_MD = !TSet.Nidec_RL.Cal_MD; break;
                case "lbl_RL_1": TSet.Nidec_RL.MT_Run = !TSet.Nidec_RL.MT_Run; break;
                case "lbl_RL_2": TSet.Nidec_RL.MTSync = !TSet.Nidec_RL.MTSync; break;
                case "lbl_RL_3": TSet.Nidec_RL.MT_Brk = !TSet.Nidec_RL.MT_Brk; break;
                case "lbl_RL_4": TSet.Nidec_RL.MTPark = !TSet.Nidec_RL.MTPark; break;
            }

            TSet.Nidec_RL.Status = Ret_Status(TSet.Nidec_RL);
            txt_RL_0.Text = TSet.Nidec_RL.Status.ToString();
            MDrive__Show();
        }
        private void lbl_RR_DoubleClick(object sender, EventArgs e)
        {
            switch (((Label)sender).Name)
            {
                case "lbl_RR_0": TSet.Nidec_RR.Cal_MD = !TSet.Nidec_RR.Cal_MD; break;
                case "lbl_RR_1": TSet.Nidec_RR.MT_Run = !TSet.Nidec_RR.MT_Run; break;
                case "lbl_RR_2": TSet.Nidec_RR.MTSync = !TSet.Nidec_RR.MTSync; break;
                case "lbl_RR_3": TSet.Nidec_RR.MT_Brk = !TSet.Nidec_RR.MT_Brk; break;
                case "lbl_RR_4": TSet.Nidec_RR.MTPark = !TSet.Nidec_RR.MTPark; break;
            }

            TSet.Nidec_RR.Status = Ret_Status(TSet.Nidec_RR);
            txt_RR_0.Text = TSet.Nidec_RR.Status.ToString();
            MDrive__Show();
        }

        private int Ret_Status(TSet.Nidec_Drive wheel)
        {
            int value = 0;

            value += wheel.Cal_MD ? 1 : 0;
            value += wheel.MT_Run ? 2 : 0;
            value += wheel.MTSync ? 4 : 0;
            value += wheel.MT_Brk ? 8 : 0;
            value += wheel.MTPark ? 16 : 0;

            return value;
        }

        private void MDrive__Read()
        {
            if (main.Nidec != null)
            {
                if (main.Nidec.IsOpen)
                {
                    main.Nidec.All__Read();

                    MDrive__Show();
                }
            }
        }
        private void MDrive_Write()
        {
            if (main.Nidec.IsOpen)
            {
                TSet.Nidec_FL.Status = int.Parse(txt_FL_0.Text);
                TSet.Nidec_FL.CalSpd = int.Parse(txt_FL_1.Text);
                TSet.Nidec_FL.WSSSpd = int.Parse(txt_FL_2.Text);
                TSet.Nidec_FL.PB_Toq = int.Parse(txt_FL_3.Text);
                TSet.Nidec_FL.PB_Spd = int.Parse(txt_FL_4.Text);

                TSet.Nidec_FR.Status = int.Parse(txt_FR_0.Text);
                TSet.Nidec_FR.CalSpd = int.Parse(txt_FR_1.Text);
                TSet.Nidec_FR.WSSSpd = int.Parse(txt_FR_2.Text);
                TSet.Nidec_FR.PB_Toq = int.Parse(txt_FR_3.Text);
                TSet.Nidec_FR.PB_Spd = int.Parse(txt_FR_4.Text);

                TSet.Nidec_RL.Status = int.Parse(txt_RL_0.Text);
                TSet.Nidec_RL.CalSpd = int.Parse(txt_RL_1.Text);
                TSet.Nidec_RL.WSSSpd = int.Parse(txt_RL_2.Text);
                TSet.Nidec_RL.PB_Toq = int.Parse(txt_RL_3.Text);
                TSet.Nidec_RL.PB_Spd = int.Parse(txt_RL_4.Text);

                TSet.Nidec_RR.Status = int.Parse(txt_RR_0.Text);
                TSet.Nidec_RR.CalSpd = int.Parse(txt_RR_1.Text);
                TSet.Nidec_RR.WSSSpd = int.Parse(txt_RR_2.Text);
                TSet.Nidec_RR.PB_Toq = int.Parse(txt_RR_3.Text);
                TSet.Nidec_RR.PB_Spd = int.Parse(txt_RR_4.Text);

                main.Nidec.All_Write();
                //H2Y.Sleep(1000);
                //main.Nidec.All__Read();

                MDrive__Show();
            }
        }
        private void MDrive__Show()
        {
            if (main.Nidec.IsOpen)
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

        private void btn__Bal_Click(object sender, EventArgs e)
        {
            double value1, value2;

            value1 = H2Y.toDbl(txt_Bal1.Text);
            value2 = H2Y.toDbl(txt_Bal2.Text);

            if (chk__Bal.Checked)
            {
                picBalance.Image = Show_Balance(picBalance, value1, value2);
            }
            else
            {
                picBalance.Image = Show1Balance(picBalance, value1, value2);
            }
        }

        private Bitmap Show_Balance(PictureBox pBox, double Bal1, double Bal2)
        {
            bmp = new Bitmap(pBox.Width, pBox.Height);

            Single brkForce = Convert.ToSingle(H2Y.Dbl_Balance(Bal1, Bal2));

            txtBalan.Text = brkForce.ToString("#0.00");

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

                g.FillRectangle(BuleBrush, 0f, 0f, bmp_w, value);
                g.FillRectangle(RedBrush, 0f, value, bmp_w, bmp_h);
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
        private Bitmap Show1Balance(PictureBox pBox, double Bal1, double Bal2)
        {
            bmp = new Bitmap(pBox.Width, pBox.Height);

            Single brkForce = Convert.ToSingle(H2Y.Dbl_Balance(Bal1, Bal2));

            txtBalan.Text = brkForce.ToString("#0.00");

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

                g.FillRectangle(BuleBrush, 0f, 0f, value, bmp_h);
                g.FillRectangle(RedBrush, value, 0f, bmp_w, bmp_h);
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

        private void chkAdres_Click(object sender, EventArgs e)
        {
            lblSet01.Text = chkAdres.Checked ? "D532/D533" : "Vehicle Set A";
            lblSet02.Text = chkAdres.Checked ? "D534/D535" : "Vehicle Set B";
            lblSet03.Text = chkAdres.Checked ? "D536/D537" : "Vehicle Set C";
            lblSet04.Text = chkAdres.Checked ? "D538/D539" : "Vehicle Set D";
            lblSet05.Text = chkAdres.Checked ? "D540/D541" : "Vehicle Set E";
            lblSet06.Text = chkAdres.Checked ? "D542/D543" : "Vehicle Set F";
            lblSet07.Text = chkAdres.Checked ? "D544/D545" : "Vehicle Set G";
            lblSet08.Text = chkAdres.Checked ? "D546/D547" : "Vehicle Set H";
            lblSet09.Text = chkAdres.Checked ? "D548/D549" : "Vehicle Set I";
            lblSet10.Text = chkAdres.Checked ? "D550/D551" : "Vehicle Set J";
            lblSet11.Text = chkAdres.Checked ? "D552/D553" : "Left Home";
            lblSet12.Text = chkAdres.Checked ? "D554/D555" : "Right Home";

            lblRead1.Text = chkAdres.Checked ? "D564/D565" : "Vehicle Set A";
            lblRead2.Text = chkAdres.Checked ? "D566/D567" : "Vehicle Set B";
            lblRead3.Text = chkAdres.Checked ? "D568/D569" : "Vehicle Set C";
            lblRead4.Text = chkAdres.Checked ? "D570/D571" : "Vehicle Set D";
            lblRead5.Text = chkAdres.Checked ? "D572/D573" : "Vehicle Set E";
            lblRead6.Text = chkAdres.Checked ? "D574/D575" : "Vehicle Set F";
            lblRead7.Text = chkAdres.Checked ? "D576/D577" : "Vehicle Set G";
            lblRead8.Text = chkAdres.Checked ? "D578/D579" : "Vehicle Set H";
            lblRead9.Text = chkAdres.Checked ? "D580/D581" : "Vehicle Set I";
            lblReadA.Text = chkAdres.Checked ? "D582/D583" : "Vehicle Set J";
            lblReadL.Text = chkAdres.Checked ? "D584/D585" : "Left Home";
            lblReadR.Text = chkAdres.Checked ? "D586/D587" : "Right Home";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte sttV = H2Y.HexTobyte(txt_SttV.Text);
            byte endV = H2Y.HexTobyte(txt_EndV.Text);
            
            lblReturn.Text = H2Y.HexToBinary(txt__Val.Text, sttV, endV);
            lbl_Bits.Text = H2Y.HexToBinary(txt__Val.Text);
        }

        private void cboIndex_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CHERY1BOX.Start_Communication();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CHERY1BOX.Start_Secceon();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CHERY1BOX.ECU_Identification();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CHERY1BOX.SecurityAccess();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CHERY1BOX.Write_EOLProcessByte();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CHERY1BOX.Calibration_Result();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CHERY1BOX.PlungerTest();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CHERY1BOX.PlungerTest_Result();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (lst_ECUs.Items.Count == 0)
            {
                MessageBox.Show("저장할 항목이 ListBox에 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

    // 1. ListBox 항목을 문자열 배열로 변환 및 역순 정렬
    // a. object 타입으로 캐스팅 (Cast<object>())
    // b. 순서를 역순으로 뒤집음 (Reverse())
    // c. 각 항목을 문자열로 변환 (Select(item => item.ToString()))
    // d. 결과물을 문자열 배열로 저장 (ToArray())
            string[] reversedLines = lst_ECUs.Items
                                              .Cast<object>()
                                              .Reverse() // <--- 순서를 역순으로 변경하는 핵심
                                              .Select(item => item.ToString())
                                              .ToArray();

            // 2. SaveFileDialog를 사용하여 저장 경로 및 파일명 지정
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일 (*.*)|*.*";
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.FileName = "reversed_list_data.txt"; // 기본 파일 이름 설정

                // 사용자가 [저장] 버튼을 눌렀을 경우
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    try
                    {
                        // 3. 역순으로 정렬된 문자열 배열을 파일에 저장
                        // File.WriteAllLines를 사용하여 모든 항목을 한 번에 파일에 씁니다.
                        // System.Text.Encoding.UTF8 사용을 권장합니다.
                        File.WriteAllLines(filePath, reversedLines, System.Text.Encoding.UTF8);

                        MessageBox.Show("ListBox 내용(역순)이 성공적으로 저장되었습니다.\n\n저장 위치: {filePath}", 
                                        "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("파일 저장 중 오류가 발생했습니다: {ex.Message}", 
                                        "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
             }
            }

        private void button10_Click(object sender, EventArgs e)
        {
            if (NI.Loss.FL.Speed > 0.5f) return;
            if (NI.Loss.FR.Speed > 0.5f) return;
            if (NI.Loss.RL.Speed > 0.5f) return;
            if (NI.Loss.RR.Speed > 0.5f) return;

            PLC.DO.FLMot_Stt = true; PLC.DO.FRMot_Stt = true;
            PLC.DO.FLMotSync = false; PLC.DO.FRMotSync = false;
            PLC.DO.FLMotStop = false; PLC.DO.FRMotStop = false;
            PLC.DO.FLMotPark = false; PLC.DO.FRMotPark = false;

            PLC.DO.RLMot_Stt = true; PLC.DO.RRMot_Stt = true;
            PLC.DO.RLMotSync = false; PLC.DO.RRMotSync = false;
            PLC.DO.RLMotStop = false; PLC.DO.RRMotStop = false;
            PLC.DO.RLMotPark = false; PLC.DO.RRMotPark = false;

            PLC.PLC_Put_D500();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            PLC.DO.FLMot_Stt = false; PLC.DO.FRMot_Stt = false;
            PLC.DO.FLMotSync = false; PLC.DO.FRMotSync = false;
            PLC.DO.FLMotStop = false; PLC.DO.FRMotStop = false;
            PLC.DO.FLMotPark = false; PLC.DO.FRMotPark = false;

            PLC.DO.RLMot_Stt = false; PLC.DO.RRMot_Stt = false;
            PLC.DO.RLMotSync = false; PLC.DO.RRMotSync = false;
            PLC.DO.RLMotStop = false; PLC.DO.RRMotStop = false;
            PLC.DO.RLMotPark = false; PLC.DO.RRMotPark = false;

            PLC.PLC_Put_D500();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            main.CheryEchoThread.SendEcho(checkBox1.Checked);
        }

    }
}
