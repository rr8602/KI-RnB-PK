using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace KI_RnB
{
    public static class PLC
    {
        //192.0.1.254 : 7000
        public const int LentGain = 10000;

        public static fom_Main Main;
        public static Thread thread;
        public static Socket Sock;
        public static string Recive;
        public static IPEndPoint endpoint;
        public static byte[] rec_Data;

        public static bool IsOpen { get; set; } //PLC 통신 OK
        public static bool IsStop { get; set; } //통신 정지
        public static bool IsRedy { get; set; } //PC  준비 OK
        
        public static bool Is_Log { get; set; } //로그 데이터
        public static int  Is_Idx { get; set; } //로그 Index (0:All, 1:100, 2:300, 3:500, 4:700)

        private static string IP;
        private static int PORT;

        private static byte PutIndex;
        private static int PHerz;
        private static int P_Cnt = 0;
        private static long mOfst = 0;
        private static int Init_Cnt = 0;

        private static float GapT = 0.3f;
        private static float Wait = 1f;

        #region PLC Input Variable
        public struct PLC_input
        {
            public bool Flag_01, Flag_02, Flag_03, Flag_04;

            public bool PBCalMode { get; set; } //D102.9 CALIBRATION PB
            public bool SST_Enter { get; set; } //D102.E SST IN-PHS
            public bool SST_GoOut { get; set; } //D102.F SST OUT-PHS

            public bool FL_MotErr { get; set; } //D103.4 FL-MOTOR DRIVER ERROR
            public bool FR_MotErr { get; set; } //D103.5 FR-MOTOR DRIVER ERROR
            public bool RL_MotErr { get; set; } //D103.6 RL-MOTOR DRIVER ERROR
            public bool RR_MotErr { get; set; } //D103.7 RR-MOTOR DRIVER ERROR
            public bool PSW_Start { get; set; } //D103.8 PULL SW START
            public bool PSW__Stop { get; set; } //D103.9 PULL SW STOP
            public bool PSW_Check { get; set; } //D103.A PULL SW 40Km/CANCEL 
            public bool PSW___Emg { get; set; } //D103.B PULL SW EMERGENCY 
            public bool Rmt_State { get; set; } //D103.E Wireless Remote Ready 

            public bool FL_RBFree { get; set; } //D104.1 FL-ROLLER BRAKE FREE LS
            public bool FLF_SR_Dn { get; set; } //D104.2 FL-FRONT SAFETY ROLLER DOWN LS
            public bool FLF_SR_Up { get; set; } //D104.3 FL-FRONT SAFETY ROLLER UP LS
            public bool FLR_SR_Dn { get; set; } //D104.4 FL-REAR SAFETY ROLLER DOWN LS
            public bool FLR_SR_Up { get; set; } //D104.5 FL-REAR SAFETY ROLLER UP LS
            public bool L_Post_Dn { get; set; } //D104.E L-SAFETY POST DOWN LS
            public bool L_Post_Up { get; set; } //D104.F L-SAFETY POST UP LS

            public bool PHO_Brake { get; set; } //D105.0 BT-PHS
            public bool RL_RBFree { get; set; } //D105.1 RL-ROLLER BRAKE FREE LS
            public bool RLR_SR_Dn { get; set; } //D105.2 RL-SAFETY ROLLER DOWN LS
            public bool RLR_SR_Up { get; set; } //D105.3 RL-SAFETY ROLLER UP LS
            public bool L_Flap_Dn { get; set; } //D105.4 L-EXHAUST FLAP DOWN LS
            public bool L_Flap_Up { get; set; } //D105.5 L-EXHAUST FLAP UP LS
            public bool PHO_Front { get; set; } //D105.9 F-PHS
            public bool BT_LiftUp { get; set; } //D105.A BT-Lift Up LS
            public bool BT_LiftDn { get; set; } //D105.B BT-Lift Down LS
            public bool WB_L_Home { get; set; } //D105.C L-WB HOME LS(X23D 참조)
            public bool WB_L__Min { get; set; } //D105.D L-WB MIN LS(X23E 참조)
            public bool WB_L__Max { get; set; } //D105.E L-WB MAX LS(X23F 참조)

            public bool FR_RBFree { get; set; } //D106.1 FR-ROLLER BRAKE FREE LS
            public bool FRF_SR_Dn { get; set; } //D106.2 FR-FRONT SAFETY ROLLER DOWN LS 
            public bool FRF_SR_Up { get; set; } //D106.3 FR-FRONT SAFETY ROLLER UP LS 
            public bool FRR_SR_Dn { get; set; } //D106.4 FR-REAR SAFETY ROLLER DOWN LS 
            public bool FRR_SR_Up { get; set; } //D106.5 FR-REAR SAFETY ROLLER UP LS
            public bool R_Post_Dn { get; set; } //D106.E R-SAFETY POST DOWN LS
            public bool R_Post_Up { get; set; } //D106.F R-SAFETY POST UP LS

            public bool RR_RBFree { get; set; } //D107.1 RR-ROLLER BRAKE FREE LS
            public bool RRR_SR_Dn { get; set; } //D107.2 RR-SAFETY ROLLER DOWN LS
            public bool RRR_SR_Up { get; set; } //D107.3 RR-SAFETY ROLLER UP LS
            public bool R_Flap_Dn { get; set; } //D107.4 R-EXHAUST FLAP DOWN LS
            public bool R_Flap_Up { get; set; } //D107.5 R-EXHAUST FLAP Up LS
            public bool PHO__Rear { get; set; } //D107.9 R-PHS
            public bool WB_R_Home { get; set; } //D107.D R-WB HOME LS
            public bool WB_R__Min { get; set; } //D107.E R-WB MIN  LS
            public bool WB_R__Max { get; set; } //D107.F R-WB MAX  LS

            public bool StandByUp { get; set; } //FLF_SR_Up, FRF_SR_Up, FLR_SR_Up, FRR_SR_Up, RLR_SR_Up, RRR_SR_Up == true
            public bool StandByDn { get; set; } //FLF_SR_Dn, FRF_SR_Dn, FLR_SR_Dn, FRR_SR_Dn, RLR_SR_Dn, RRR_SR_Dn == true

            public bool Start__PB { get; set; } //D108.A VEHICLE START     PB
            public bool Stop___PB { get; set; } //D108.B VEHICLE STOP      PB
            public bool Cancel_PB { get; set; } //D108.C VEHICLE CANCEL    PB
            public bool EMG____PB { get; set; } //D108.D VEHICLE EMERGENCY PB

            public bool GOT_Start { get; set; } //D114.A VEHICLE START     GOT
            public bool GOT__Stop { get; set; } //D114.B VEHICLE STOP      GOT
            public bool GOTCancel { get; set; } //D114.C VEHICLE CANCEL    GOT
            public bool GOT___EMG { get; set; } //D114.D VEHICLE EMERGENCY GOT
        }
        public static PLC_input DI;

        public static int DI_100 { get; set; }
        public static int DI_101 { get; set; }
        public static int DI_102 { get; set; }
        public static int DI_103 { get; set; }
        public static int DI_104 { get; set; }
        public static int DI_105 { get; set; }
        public static int DI_106 { get; set; }
        public static int DI_107 { get; set; }
        public static int DI_108 { get; set; }
        public static int DI_109 { get; set; }
        public static int DI_110 { get; set; }
        public static int DI_111 { get; set; }
        public static int DI_112 { get; set; }
        public static int DI_113 { get; set; }
        public static int DI_114 { get; set; }
        public static int DI_115 { get; set; }
        public static int DI_116 { get; set; }
        public static int DI_117 { get; set; }
        public static int DI_118 { get; set; }
        public static int DI_119 { get; set; }
        public static int DI_120 { get; set; }

        public static bool[] DIB100 { get; set; }
        public static bool[] DIB101 { get; set; }
        public static bool[] DIB102 { get; set; }
        public static bool[] DIB103 { get; set; }
        public static bool[] DIB104 { get; set; }
        public static bool[] DIB105 { get; set; }
        public static bool[] DIB106 { get; set; }
        public static bool[] DIB107 { get; set; }
        public static bool[] DIB108 { get; set; }
        public static bool[] DIB109 { get; set; }
        public static bool[] DIB110 { get; set; }
        public static bool[] DIB111 { get; set; }
        public static bool[] DIB112 { get; set; }
        public static bool[] DIB113 { get; set; }
        public static bool[] DIB114 { get; set; }
        public static bool[] DIB115 { get; set; }
        public static bool[] DIB116 { get; set; }
        public static bool[] DIB117 { get; set; }
        public static bool[] DIB118 { get; set; }
        public static bool[] DIB119 { get; set; }
        public static bool[] DIB120 { get; set; }

        public static int DI_306 { get; set; }

        public static bool[] DIB306 { get; set; }

        private static bool old_Stop;
        private static int CNT_Stop = 0;

        #endregion

        #region PLC Output Variable
        public struct PLC_output
        {
            public bool WBMoveing { get; set; } //D301[00] WB Moving
            public bool BT_MotRun { get; set; } //D303[10] BT-Motor ON
            public bool Indicator { get; set; } //D303[13] Calibration Indicter Power ON

            public bool Select_01 { get; set; } //D306[00] VEHICLE SELECT 01 LP
            public bool Select_02 { get; set; } //D306[01] VEHICLE SELECT 02 LP
            public bool Select_03 { get; set; } //D306[02] VEHICLE SELECT 03 LP
            public bool Select_04 { get; set; } //D306[03] VEHICLE SELECT 04 LP
            public bool Select_05 { get; set; } //D306[04] VEHICLE SELECT 05 LP
            public bool Select_06 { get; set; } //D306[05] VEHICLE SELECT 06 LP
            public bool Select_07 { get; set; } //D306[06] VEHICLE SELECT 07 LP
            public bool Select_08 { get; set; } //D306[07] VEHICLE SELECT 08 LP
            public bool Select_09 { get; set; } //D306[08] VEHICLE SELECT 09 LP
            public bool Select_10 { get; set; } //D306[09] VEHICLE SELECT 10 LP

            public bool MD__Ready { get; set; } //D311[00] 장비 Ready
            public bool MD___Auto { get; set; } //D311[01] 장비 Auto 모드
            public bool MD_Manual { get; set; } //D311[02] 장비 Manual 모드
            public bool MD___Stop { get; set; } //D311[03] 장비 Stop 모드
            public bool MD_FirstH { get; set; } //D311[04] 홈 (처음) 인식
            public bool MD_Emerge { get; set; } //D311[05] 장비 비상
            public bool Pos__Home { get; set; } //D311[06] 홈   포지션
            public bool Pos_Enter { get; set; } //D311[07] 장비 포지션(진입)
            public bool Pos__Test { get; set; } //D311[08] 장비 포지션(측정)
            public bool Pos__Exit { get; set; } //D311[09] 장비 포지션(퇴출)

            public bool PC_Standy { get; set; } //D312[00] PC 준비
            public bool TestReady { get; set; } //D312[01] Test 준비
            public bool TestStart { get; set; } //D312[02] Test 시작
            public bool Test__End { get; set; } //D312[03] Test 종료
            public bool Spd_5kmph { get; set; } //D312[04] 속도 5km/h 이상
            public bool Test_Sett { get; set; } //D312[05] Test 세팅(위치, 속도)
            public bool Home_Move { get; set; } //D312[06] 홈으로 이동

            public bool DistReset { get; set; } //D312[08] 거리 갱신
            public bool CalAirSol { get; set; } //D312[09] 교정 솔
            public bool CalIndiOn { get; set; } //D312[10] Calibration Indicter Power ON
            public bool BT_Mot_On { get; set; } //D312[12] BT-Motor ON
            public bool BT_LiftUp { get; set; } //D312[13] BT-Lift UP
            public bool BT_LiftDn { get; set; } //D312[14] BT-LIFT Down
            public bool CalIndi_O { get; set; } //D312[15] 인디게이터 영점

            public bool FLMot_Stt { get; set; } //D530[00] FL-MOTOR START PC
            public bool FLMotSync { get; set; } //D530[01] FL-MOTOR 동기 PC
            public bool FLMotStop { get; set; } //D530[02] FL-MOTOR STOP BRAKE PC
            public bool FLMotPark { get; set; } //D530[03] FL-MOTOR PARKING 검사 PC

            public bool FRMot_Stt { get; set; } //D530[08] FR-MOTOR START PC
            public bool FRMotSync { get; set; } //D530[09] FR-MOTOR 동기 PC
            public bool FRMotStop { get; set; } //D530[10] FR-MOTOR STOP BRAKE PC
            public bool FRMotPark { get; set; } //D530[11] FR-MOTOR PARKING 검사 PC

            public bool RLMot_Stt { get; set; } //D531[00] RL-MOTOR START PC
            public bool RLMotSync { get; set; } //D531[01] RL-MOTOR 동기 PC
            public bool RLMotStop { get; set; } //D531[02] RL-MOTOR STOP BRAKE PC
            public bool RLMotPark { get; set; } //D531[03] RL-MOTOR PARKING 검사 PC

            public bool RRMot_Stt { get; set; } //D531[08] RR-MOTOR START PC
            public bool RRMotSync { get; set; } //D531[09] RR-MOTOR 동기 PC
            public bool RRMotStop { get; set; } //D531[10] RR-MOTOR STOP BRAKE PC
            public bool RRMotPark { get; set; } //D531[11] RR-MOTOR PARKING 검사 PC

            public bool Door_Open { get; set; } //D531[14] BOOTH DOOR OPEN  PC
            public bool DoorClose { get; set; } //D531[15] BOOTH DOOR CLOSE PC

            public bool Vehicle01 { get; set; } //D562[00] VEHICLE SELECT 01 PC
            public bool Vehicle02 { get; set; } //D562[01] VEHICLE SELECT 02 PC
            public bool Vehicle03 { get; set; } //D562[02] VEHICLE SELECT 03 PC
            public bool Vehicle04 { get; set; } //D562[03] VEHICLE SELECT 04 PC
            public bool Vehicle05 { get; set; } //D562[04] VEHICLE SELECT 05 PC
            public bool Vehicle06 { get; set; } //D562[05] VEHICLE SELECT 06 PC
            public bool Vehicle07 { get; set; } //D562[06] VEHICLE SELECT 07 PC
            public bool Vehicle08 { get; set; } //D562[07] VEHICLE SELECT 08 PC
            public bool Vehicle09 { get; set; } //D562[08] VEHICLE SELECT 09 PC
            public bool Vehicle10 { get; set; } //D562[09] VEHICLE SELECT 10 PC
            public bool PC__Start { get; set; } //D562[10] VEHICLE START PC
            public bool PC___Stop { get; set; } //D562[11] VEHICLE STOP PC
            public bool PC_Cancel { get; set; } //D562[12] VEHICLE CANCEL PC

            public bool PC_StadBy { get; set; } //D562[14] 운전준비 PC
            public bool PC__Reset { get; set; } //D562[15] 모터컨트롤러 리세트 PC
        }
        public static PLC_output DO;

        public static int DO_300 { get; set; }
        public static int DO_301 { get; set; }
        public static int DO_302 { get; set; }
        public static int DO_303 { get; set; }
        public static int DO_304 { get; set; }
        public static int DO_305 { get; set; }
        public static int DO_306 { get; set; }
        public static int DO_307 { get; set; }
        public static int DO_308 { get; set; }
        public static int DO_309 { get; set; }
        public static int DO_310 { get; set; }
        public static int DO_311 { get; set; }
        public static int DO_312 { get; set; }
        public static int DO_313 { get; set; }
        public static int DO_314 { get; set; }
        public static int DO_315 { get; set; }
        public static int DO_316 { get; set; }
        public static int DO_317 { get; set; }

        public static bool[] DOB300 { get; set; }
        public static bool[] DOB301 { get; set; }
        public static bool[] DOB302 { get; set; }
        public static bool[] DOB303 { get; set; }
        public static bool[] DOB304 { get; set; }
        public static bool[] DOB305 { get; set; }
        public static bool[] DOB306 { get; set; }
        public static bool[] DOB307 { get; set; }
        public static bool[] DOB308 { get; set; }
        public static bool[] DOB309 { get; set; }
        public static bool[] DOB310 { get; set; }
        public static bool[] DOB311 { get; set; }
        public static bool[] DOB312 { get; set; }
        public static bool[] DOB313 { get; set; }
        public static bool[] DOB314 { get; set; }
        public static bool[] DOB315 { get; set; }
        public static bool[] DOB316 { get; set; }
        public static bool[] DOB317 { get; set; }
        public static bool[] DOB318 { get; set; }
        public static bool[] DOB319 { get; set; }

        public static int DO_530 { get; set; }  //Front Left / Right Contorl
        public static int DO_531 { get; set; }  //Rear  Left / Right Contorl
        public static int DO_562 { get; set; }  //차량 선택기

        public static bool[] DOB530 { get; set; }
        public static bool[] DOB531 { get; set; }
        public static bool[] DOB562 { get; set; }
        #endregion

        #region PLC Memory
        public static int Length { get; set; }
        public static int Seting { get; set; }

        public static int DM_500; public static int DM_700;
        public static int DM_501; public static int DM_701;
        public static int DM_502; public static int DM_702;
        public static int DM_503; public static int DM_703;
        public static int DM_504; public static int DM_704;
        public static int DM_505; public static int DM_705;
        public static int DM_506; public static int DM_706;
        public static int DM_507; public static int DM_707;
        public static int DM_508; public static int DM_708;
        public static int DM_509; public static int DM_709;
        public static int DM_510; public static int DM_710;
        public static int DM_511; public static int DM_711;
        public static int DM_512; public static int DM_712;
        public static int DM_513; public static int DM_713;
        public static int DM_514; public static int DM_714;
        public static int DM_515; public static int DM_715;
        public static int DM_516; public static int DM_716;
        public static int DM_517; public static int DM_717;
        public static int DM_518; public static int DM_718;
        public static int DM_519; public static int DM_719;
        public static int DM_520; public static int DM_720;
        public static int DM_521; public static int DM_721;
        public static int DM_522; public static int DM_722;
        public static int DM_523; public static int DM_723;
        public static int DM_524; public static int DM_724;
        public static int DM_525; public static int DM_725;
        public static int DM_526; public static int DM_726;
        public static int DM_527; public static int DM_727;
        public static int DM_528; public static int DM_728;
        public static int DM_529; public static int DM_729;
        public static int DM_530; public static int DM_730;
        public static int DM_531; public static int DM_731;
        public static int DM_532; public static int DM_732;
        public static int DM_533; public static int DM_733;
        public static int DM_534;
        public static int DM_535;
        public static int DM_536;
        public static int DM_537;
        public static int DM_538;
        public static int DM_539;
        public static int DM_540;
        public static int DM_541;
        public static int DM_542;
        public static int DM_543;
        public static int DM_544;
        public static int DM_545;
        public static int DM_546;
        public static int DM_547;
        public static int DM_548;
        public static int DM_549;
        public static int DM_550;
        public static int DM_551;
        public static int DM_552;
        public static int DM_553;
        public static int DM_554;
        public static int DM_555;
        public static int DM_556;
        public static int DM_557;
        public static int DM_558;
        public static int DM_559;
        public static int DM_560;
        public static int DM_561;
        public static int DM_562;
        public static int DM_563;
        public static int DM_564;
        public static int DM_565;
        public static int DM_566;
        public static int DM_567;
        public static int DM_568;
        public static int DM_569;
        public static int DM_570;
        public static int DM_571;
        public static int DM_572;
        public static int DM_573;
        public static int DM_574;
        public static int DM_575;
        public static int DM_576;
        public static int DM_577;
        public static int DM_578;
        public static int DM_579;
        public static int DM_580;
        public static int DM_581;
        public static int DM_582;
        public static int DM_583;
        public static int DM_584;
        public static int DM_585;
        public static int DM_586;
        public static int DM_587;
        public static int DM_588;
        public static int DM_589;
        public static int DM_590;
        public static int DM_591;
        public static int DM_592;
        public static int DM_593;
        public static int DM_594;
        public static int DM_595;
        public static int DM_596;
        public static int DM_597;
        public static int DM_598;
        public static int DM_599;

        public static int Dist_A { get; set; }
        public static int Dist_B { get; set; }
        public static int Dist_C { get; set; }
        public static int Dist_D { get; set; }
        public static int Dist_E { get; set; }
        public static int Dist_F { get; set; }
        public static int Dist_G { get; set; }
        public static int Dist_H { get; set; }
        public static int Dist_I { get; set; }
        public static int Dist_J { get; set; }
        public static int OfSetL { get; set; }
        public static int OfSetR { get; set; }
        #endregion

        public static void Setting(fom_Main main, string ip, int port)
        {
            Main = main;
            IP = ip;
            PORT = port;

            #region PLC init
            DIB100 = new bool[16];
            DIB101 = new bool[16];
            DIB102 = new bool[16];
            DIB103 = new bool[16];
            DIB104 = new bool[16];
            DIB105 = new bool[16];
            DIB106 = new bool[16];
            DIB107 = new bool[16];
            DIB108 = new bool[16];
            DIB109 = new bool[16];
            DIB110 = new bool[16];
            DIB111 = new bool[16];
            DIB112 = new bool[16];
            DIB113 = new bool[16];
            DIB114 = new bool[16];
            DIB115 = new bool[16];
            DIB116 = new bool[16];
            DIB117 = new bool[16];
            DIB118 = new bool[16];
            DIB119 = new bool[16];
            DIB120 = new bool[16];

            DOB300 = new bool[16];
            DOB301 = new bool[16];
            DOB302 = new bool[16];
            DOB303 = new bool[16];
            DOB304 = new bool[16];
            DOB305 = new bool[16];
            DOB306 = new bool[16];
            DOB307 = new bool[16];
            DOB308 = new bool[16];
            DOB309 = new bool[16];
            DOB310 = new bool[16];
            DOB311 = new bool[16];
            DOB312 = new bool[16];
            DOB313 = new bool[16];
            DOB314 = new bool[16];
            DOB315 = new bool[16];
            DOB316 = new bool[16];
            DOB317 = new bool[16];
            DOB318 = new bool[16];
            DOB319 = new bool[16];

            DOB530 = new bool[16];
            DOB531 = new bool[16];

            DOB562 = new bool[16];
            #endregion
        }

        public static void Start()
        {
            if (IP == null) { return; }
            if (PORT == 0) { return; }

            thread = new Thread(new ThreadStart(PLC_Run));
            thread.Start();
        }

        public static void PLC_Run()
        {
            try
            {
                IsStop = false;
                IsOpen = false; 
                Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                endpoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
                Sock.ReceiveTimeout = 500;
                Sock.Connect(endpoint);
                IsOpen = true; 

                PLC_Ask_Data();

                rec_Data = new byte[10000];

                while (thread.IsAlive)
                {
                    Thread.Sleep(100);
                    if (IsStop) break;

                    //try
                    {
                        if (Sock.Connected)
                        {
                            int length = Sock.Receive(rec_Data, rec_Data.Length, SocketFlags.None);
                            Recive = Encoding.UTF8.GetString(rec_Data, 0, length);

                            if (Recive != "")
                            {
                                PLC_Get_Data(Recive);
                                Thread.Sleep(PSet.PLC_GapT);
                                PLC_Ask_Data();
                            }
                            else
                            {
                                PLC_Ask_Data();
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    //catch (Exception ex)
                    //{
                    //    //MessageBox.Show(ex.Message);
                    //    Main.Prog_LogData(ex.Message);
                    //    break;
                    //}

                    //System.Windows.Forms.Application.DoEvents();
                }
            }
            catch (SocketException ex)
            {
                IsOpen = false;
                //if(ex.ErrorCode == 10061)
                //MessageBox.Show(ex.Message);
                Main.Prog_LogData("PLC ERROR : " + ex.Message);
            }

            if (PSet.Onf_Prog)
            {
                IsOpen = false;
                if (Sock != null && Sock.Connected) { Sock.Disconnect(true); }
                if (!IsStop) Start();
            }
        }

        public static void Close()
        {
            IsStop = true;
            IsOpen = false;
            if (Sock != null) Sock.Close();
            if (thread != null) thread.Abort();
            Main.Prog_LogData("PLC Close");
        }

        #region PLC Input 처리
        /// <summary>
        /// PLC에 데이터 요청
        /// </summary>
        public static void PLC_Ask_Data()
        {
            string Send_Str = "";

            PutIndex++;
            switch (PutIndex)
            {
                case 1: Send_Str = "01" + "FF" + "000A" + "4420" + "0000" + "0064" + "15" + "00"; break;  //D100  21EA
                case 2: Send_Str = "01" + "FF" + "000A" + "4420" + "0000" + "012C" + "12" + "00"; break;  //D300  18EA
                case 3: Send_Str = "01" + "FF" + "000A" + "4420" + "0000" + "01F4" + "64" + "00"; break;  //D500 100EA
                case 4: Send_Str = "01" + "FF" + "000A" + "4420" + "0000" + "02BC" + "22" + "00"; PutIndex = 0; break;  //D700  34EA
            }
            Send_Message(Send_Str);
        }

        /// <summary>
        /// 받은 PLC 데이터 분기
        /// </summary>
        /// <param name="pData">받은 문자열</param>
        public static void PLC_Get_Data(string pData)
        {
            if (Is_Log && Is_Idx == 0) { Main.PLC_Log_Data(pData); }

            PLC_1E_Data(pData);
            
            P_Cnt++;
            if (P_Cnt % 2 == 0)
            {
                Main.lbl_PLCs.ForeColor = Main.lbl_PLCs.ForeColor == H2Y.Black ? H2Y.Yellow : H2Y.Black;
            }
            if (DateTime.Now.Ticks - mOfst >= TimeSpan.TicksPerSecond)
            {
                mOfst = DateTime.Now.Ticks;
                PHerz = P_Cnt;
                P_Cnt = 0;

                Main.PLC_ScanHerz(PHerz.ToString());
                //if (IsRedy && !PLC.DO.PC_Run)
                //{
                //    PLC.DO.PC_Run = true;   PLC.PLC_312_Puts();
                //}
            }
        }

        /// <summary>
        /// PLC 외부 연결
        /// </summary>
        /// <param name="pData">받은 문자열</param>
        public static void PLC_1E_Data(string pData)
        {
            if (pData.Substring(0, 4) != "8100") return;
            //           1           2          3         4         5         6         7         8         9         0         1         2         3         4         5         6         7890
            //123456789012 3456 7890 123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890
            //81000001007E 00C0 0014 0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000
            //810000030000 E360 0016 00000000A12000070000000000000000000000000000000000000000

            int Stt = 04;
            int CNT = 0;

            switch (pData.Length)
            {
                #region DI 100
                case 88: DI.Flag_01 = !DI.Flag_01;
                         if (Is_Log && Is_Idx == 1) { Main.PLC_Log_Data(pData); } 
                         DI_100 = HexToDecimal(pData.Substring(Stt + (4 * 0), 4));
                         DI_101 = HexToDecimal(pData.Substring(Stt + (4 * 1), 4));
                         DI_102 = HexToDecimal(pData.Substring(Stt + (4 * 2), 4));
                         DI_103 = HexToDecimal(pData.Substring(Stt + (4 * 3), 4));
                         DI_104 = HexToDecimal(pData.Substring(Stt + (4 * 4), 4));
                         DI_105 = HexToDecimal(pData.Substring(Stt + (4 * 5), 4));
                         DI_106 = HexToDecimal(pData.Substring(Stt + (4 * 6), 4));
                         DI_107 = HexToDecimal(pData.Substring(Stt + (4 * 7), 4));
                         DI_108 = HexToDecimal(pData.Substring(Stt + (4 * 8), 4));
                         DI_109 = HexToDecimal(pData.Substring(Stt + (4 * 9), 4));
                         DI_110 = HexToDecimal(pData.Substring(Stt + (4 * 10), 4));
                         DI_111 = HexToDecimal(pData.Substring(Stt + (4 * 11), 4));
                         DI_112 = HexToDecimal(pData.Substring(Stt + (4 * 12), 4));
                         DI_113 = HexToDecimal(pData.Substring(Stt + (4 * 13), 4));
                         DI_114 = HexToDecimal(pData.Substring(Stt + (4 * 14), 4));
                         DI_115 = HexToDecimal(pData.Substring(Stt + (4 * 15), 4));
                         DI_116 = HexToDecimal(pData.Substring(Stt + (4 * 16), 4));
                         DI_117 = HexToDecimal(pData.Substring(Stt + (4 * 17), 4));
                         DI_118 = HexToDecimal(pData.Substring(Stt + (4 * 18), 4));
                         DI_119 = HexToDecimal(pData.Substring(Stt + (4 * 19), 4));
                         DI_120 = HexToDecimal(pData.Substring(Stt + (4 * 20), 4)); DI_HexToBits();
                         break;
                #endregion

                #region DO 300
                case 76: DI.Flag_02 = !DI.Flag_02;
                         if (Is_Log && Is_Idx == 2) { Main.PLC_Log_Data(pData); }
                         DO_300 = HexToDecimal(pData.Substring(Stt + (4 * 0), 4));
                         DO_301 = HexToDecimal(pData.Substring(Stt + (4 * 1), 4));
                         DO_302 = HexToDecimal(pData.Substring(Stt + (4 * 2), 4));
                         DO_303 = HexToDecimal(pData.Substring(Stt + (4 * 3), 4));
                         DO_304 = HexToDecimal(pData.Substring(Stt + (4 * 4), 4));
                         DO_305 = HexToDecimal(pData.Substring(Stt + (4 * 5), 4));
                         DO_306 = HexToDecimal(pData.Substring(Stt + (4 * 6), 4));
                         DO_307 = HexToDecimal(pData.Substring(Stt + (4 * 7), 4));
                         DO_308 = HexToDecimal(pData.Substring(Stt + (4 * 8), 4));
                         DO_309 = HexToDecimal(pData.Substring(Stt + (4 * 9), 4));
                         DO_310 = HexToDecimal(pData.Substring(Stt + (4 * 10), 4));
                         DO_311 = HexToDecimal(pData.Substring(Stt + (4 * 11), 4));
                         DO_312 = HexToDecimal(pData.Substring(Stt + (4 * 12), 4));
                         DO_313 = HexToDecimal(pData.Substring(Stt + (4 * 13), 4));
                         DO_314 = HexToDecimal(pData.Substring(Stt + (4 * 14), 4));
                         DO_315 = HexToDecimal(pData.Substring(Stt + (4 * 15), 4));
                         DO_316 = HexToDecimal(pData.Substring(Stt + (4 * 16), 4));
                         DO_317 = HexToDecimal(pData.Substring(Stt + (4 * 17), 4));  DO_HexToBits();
                         break;
                #endregion 

                #region DM 500
                case 404: DI.Flag_03 = !DI.Flag_03; 
                         if (Is_Log && Is_Idx == 3) { Main.PLC_Log_Data(pData); } 
                         DM_500 = HexToDecimal(pData.Substring(Stt + (4 * 0), 4));
                         DM_501 = HexToDecimal(pData.Substring(Stt + (4 * 1), 4));
                         DM_502 = HexToDecimal(pData.Substring(Stt + (4 * 2), 4));
                         DM_503 = HexToDecimal(pData.Substring(Stt + (4 * 3), 4));
                         DM_504 = HexToDecimal(pData.Substring(Stt + (4 * 4), 4));
                         DM_505 = HexToDecimal(pData.Substring(Stt + (4 * 5), 4));
                         DM_506 = HexToDecimal(pData.Substring(Stt + (4 * 6), 4));
                         DM_507 = HexToDecimal(pData.Substring(Stt + (4 * 7), 4));
                         DM_508 = HexToDecimal(pData.Substring(Stt + (4 * 8), 4));
                         DM_509 = HexToDecimal(pData.Substring(Stt + (4 * 9), 4));

                         DM_510 = HexToDecimal(pData.Substring(Stt + (4 * 10), 4));
                         DM_511 = HexToDecimal(pData.Substring(Stt + (4 * 11), 4));
                         DM_512 = HexToDecimal(pData.Substring(Stt + (4 * 12), 4));
                         DM_513 = HexToDecimal(pData.Substring(Stt + (4 * 13), 4));
                         DM_514 = HexToDecimal(pData.Substring(Stt + (4 * 14), 4));
                         DM_515 = HexToDecimal(pData.Substring(Stt + (4 * 15), 4));
                         DM_516 = HexToDecimal(pData.Substring(Stt + (4 * 16), 4));
                         DM_517 = HexToDecimal(pData.Substring(Stt + (4 * 17), 4));
                         DM_518 = HexToDecimal(pData.Substring(Stt + (4 * 18), 4));
                         DM_519 = HexToDecimal(pData.Substring(Stt + (4 * 19), 4));

                         DM_520 = HexToDecimal(pData.Substring(Stt + (4 * 20), 4));      
                         DM_521 = HexToDecimal(pData.Substring(Stt + (4 * 21), 4));
                         DM_522 = HexToDecimal(pData.Substring(Stt + (4 * 22), 4));
                         DM_523 = HexToDecimal(pData.Substring(Stt + (4 * 23), 4));
                         DM_524 = HexToDecimal(pData.Substring(Stt + (4 * 24), 4));
                         DM_525 = HexToDecimal(pData.Substring(Stt + (4 * 25), 4));
                         DM_526 = HexToDecimal(pData.Substring(Stt + (4 * 26), 4));
                         DM_527 = HexToDecimal(pData.Substring(Stt + (4 * 27), 4));
                         DM_528 = HexToDecimal(pData.Substring(Stt + (4 * 28), 4));
                         DM_529 = HexToDecimal(pData.Substring(Stt + (4 * 29), 4));

                         DM_530 = HexToDecimal(pData.Substring(Stt + (4 * 30), 4)); 
                         DM_531 = HexToDecimal(pData.Substring(Stt + (4 * 31), 4)); 
                         DM_532 = HexToDecimal(pData.Substring(Stt + (4 * 32), 4));
                         DM_533 = HexToDecimal(pData.Substring(Stt + (4 * 33), 4));
                         DM_534 = HexToDecimal(pData.Substring(Stt + (4 * 34), 4));
                         DM_535 = HexToDecimal(pData.Substring(Stt + (4 * 35), 4));
                         DM_536 = HexToDecimal(pData.Substring(Stt + (4 * 36), 4));
                         DM_537 = HexToDecimal(pData.Substring(Stt + (4 * 37), 4));
                         DM_538 = HexToDecimal(pData.Substring(Stt + (4 * 38), 4));
                         DM_539 = HexToDecimal(pData.Substring(Stt + (4 * 39), 4));
                         
                         DM_540 = HexToDecimal(pData.Substring(Stt + (4 * 40), 4));
                         DM_541 = HexToDecimal(pData.Substring(Stt + (4 * 41), 4));
                         DM_542 = HexToDecimal(pData.Substring(Stt + (4 * 42), 4));
                         DM_543 = HexToDecimal(pData.Substring(Stt + (4 * 43), 4));
                         DM_544 = HexToDecimal(pData.Substring(Stt + (4 * 44), 4));
                         DM_545 = HexToDecimal(pData.Substring(Stt + (4 * 45), 4));
                         DM_546 = HexToDecimal(pData.Substring(Stt + (4 * 46), 4));
                         DM_547 = HexToDecimal(pData.Substring(Stt + (4 * 47), 4));
                         DM_548 = HexToDecimal(pData.Substring(Stt + (4 * 48), 4));
                         DM_549 = HexToDecimal(pData.Substring(Stt + (4 * 49), 4));
                         
                         DM_550 = HexToDecimal(pData.Substring(Stt + (4 * 50), 4));
                         DM_551 = HexToDecimal(pData.Substring(Stt + (4 * 51), 4));
                         DM_552 = HexToDecimal(pData.Substring(Stt + (4 * 52), 4));
                         DM_553 = HexToDecimal(pData.Substring(Stt + (4 * 53), 4));
                         DM_554 = HexToDecimal(pData.Substring(Stt + (4 * 54), 4));
                         DM_555 = HexToDecimal(pData.Substring(Stt + (4 * 55), 4));
                         DM_556 = HexToDecimal(pData.Substring(Stt + (4 * 56), 4));
                         DM_557 = HexToDecimal(pData.Substring(Stt + (4 * 57), 4));
                         DM_558 = HexToDecimal(pData.Substring(Stt + (4 * 58), 4));
                         DM_559 = HexToDecimal(pData.Substring(Stt + (4 * 59), 4));

                         DM_560 = HexToDecimal(pData.Substring(Stt + (4 * 60), 4));
                         DM_561 = HexToDecimal(pData.Substring(Stt + (4 * 61), 4));
                         DM_562 = HexToDecimal(pData.Substring(Stt + (4 * 62), 4));
                         DM_563 = HexToDecimal(pData.Substring(Stt + (4 * 63), 4));
                         DM_564 = HexToDecimal(pData.Substring(Stt + (4 * 64), 4));
                         DM_565 = HexToDecimal(pData.Substring(Stt + (4 * 65), 4));
                         DM_566 = HexToDecimal(pData.Substring(Stt + (4 * 66), 4));
                         DM_567 = HexToDecimal(pData.Substring(Stt + (4 * 67), 4));
                         DM_568 = HexToDecimal(pData.Substring(Stt + (4 * 68), 4));
                         DM_569 = HexToDecimal(pData.Substring(Stt + (4 * 69), 4));

                         DM_570 = HexToDecimal(pData.Substring(Stt + (4 * 70), 4));
                         DM_571 = HexToDecimal(pData.Substring(Stt + (4 * 71), 4));
                         DM_572 = HexToDecimal(pData.Substring(Stt + (4 * 72), 4));
                         DM_573 = HexToDecimal(pData.Substring(Stt + (4 * 73), 4));
                         DM_574 = HexToDecimal(pData.Substring(Stt + (4 * 74), 4));
                         DM_575 = HexToDecimal(pData.Substring(Stt + (4 * 75), 4));
                         DM_576 = HexToDecimal(pData.Substring(Stt + (4 * 76), 4));
                         DM_577 = HexToDecimal(pData.Substring(Stt + (4 * 77), 4));
                         DM_578 = HexToDecimal(pData.Substring(Stt + (4 * 78), 4));
                         DM_579 = HexToDecimal(pData.Substring(Stt + (4 * 79), 4));

                         DM_580 = HexToDecimal(pData.Substring(Stt + (4 * 80), 4));
                         DM_581 = HexToDecimal(pData.Substring(Stt + (4 * 81), 4));
                         DM_582 = HexToDecimal(pData.Substring(Stt + (4 * 82), 4));
                         DM_583 = HexToDecimal(pData.Substring(Stt + (4 * 83), 4));
                         DM_584 = HexToDecimal(pData.Substring(Stt + (4 * 84), 4));

                         DM_585 = HexToDecimal(pData.Substring(Stt + (4 * 85), 4));
                         DM_586 = HexToDecimal(pData.Substring(Stt + (4 * 86), 4));
                         DM_587 = HexToDecimal(pData.Substring(Stt + (4 * 87), 4));
                         DM_588 = HexToDecimal(pData.Substring(Stt + (4 * 88), 4));
                         DM_589 = HexToDecimal(pData.Substring(Stt + (4 * 89), 4));

                         DM_590 = HexToDecimal(pData.Substring(Stt + (4 * 90), 4));
                         DM_591 = HexToDecimal(pData.Substring(Stt + (4 * 91), 4));
                         DM_592 = HexToDecimal(pData.Substring(Stt + (4 * 92), 4));
                         DM_593 = HexToDecimal(pData.Substring(Stt + (4 * 93), 4));
                         DM_594 = HexToDecimal(pData.Substring(Stt + (4 * 94), 4));
                         DM_595 = HexToDecimal(pData.Substring(Stt + (4 * 95), 4));
                         DM_596 = HexToDecimal(pData.Substring(Stt + (4 * 96), 4));
                         DM_597 = HexToDecimal(pData.Substring(Stt + (4 * 97), 4));
                         DM_598 = HexToDecimal(pData.Substring(Stt + (4 * 98), 4));
                         DM_599 = HexToDecimal(pData.Substring(Stt + (4 * 99), 4));

                         DO_530 = DM_530;
                         DO_531 = DM_531;
                         DO_562 = DM_562;   

                         DO_LengthSet();
                        
                         Init_Cnt++;
                         if (Init_Cnt > 10)
                         {
                             Init_Cnt = 11;
                             IsRedy = true;
                         }
                         break;
                #endregion

                #region DM 700
                case 140: DI.Flag_04 = !DI.Flag_04; 
                         if (Is_Log && Is_Idx == 4) { Main.PLC_Log_Data(pData); }
                         DM_700 = HexToDecimal(pData.Substring(Stt + (4 * 0), 4));
                         DM_701 = HexToDecimal(pData.Substring(Stt + (4 * 1), 4));
                         DM_702 = HexToDecimal(pData.Substring(Stt + (4 * 2), 4));
                         DM_703 = HexToDecimal(pData.Substring(Stt + (4 * 3), 4));
                         DM_704 = HexToDecimal(pData.Substring(Stt + (4 * 4), 4));
                         DM_705 = HexToDecimal(pData.Substring(Stt + (4 * 5), 4));
                         DM_706 = HexToDecimal(pData.Substring(Stt + (4 * 6), 4));
                         DM_707 = HexToDecimal(pData.Substring(Stt + (4 * 7), 4));
                         DM_708 = HexToDecimal(pData.Substring(Stt + (4 * 8), 4));
                         DM_709 = HexToDecimal(pData.Substring(Stt + (4 * 9), 4));

                         DM_710 = HexToDecimal(pData.Substring(Stt + (4 * 10), 4));
                         DM_711 = HexToDecimal(pData.Substring(Stt + (4 * 11), 4));
                         DM_712 = HexToDecimal(pData.Substring(Stt + (4 * 12), 4));
                         DM_713 = HexToDecimal(pData.Substring(Stt + (4 * 13), 4));
                         DM_714 = HexToDecimal(pData.Substring(Stt + (4 * 14), 4));
                         DM_715 = HexToDecimal(pData.Substring(Stt + (4 * 15), 4));
                         DM_716 = HexToDecimal(pData.Substring(Stt + (4 * 16), 4));
                         DM_717 = HexToDecimal(pData.Substring(Stt + (4 * 17), 4));
                         DM_718 = HexToDecimal(pData.Substring(Stt + (4 * 18), 4));
                         DM_719 = HexToDecimal(pData.Substring(Stt + (4 * 19), 4));

                         DM_720 = HexToDecimal(pData.Substring(Stt + (4 * 20), 4));
                         DM_721 = HexToDecimal(pData.Substring(Stt + (4 * 21), 4));
                         DM_722 = HexToDecimal(pData.Substring(Stt + (4 * 22), 4));
                         DM_723 = HexToDecimal(pData.Substring(Stt + (4 * 23), 4));
                         DM_724 = HexToDecimal(pData.Substring(Stt + (4 * 24), 4));
                         DM_725 = HexToDecimal(pData.Substring(Stt + (4 * 25), 4));
                         DM_726 = HexToDecimal(pData.Substring(Stt + (4 * 26), 4));
                         DM_727 = HexToDecimal(pData.Substring(Stt + (4 * 27), 4));
                         DM_728 = HexToDecimal(pData.Substring(Stt + (4 * 28), 4));
                         DM_729 = HexToDecimal(pData.Substring(Stt + (4 * 29), 4));

                         DM_730 = HexToDecimal(pData.Substring(Stt + (4 * 30), 4));
                         DM_731 = HexToDecimal(pData.Substring(Stt + (4 * 31), 4));
                         DM_732 = HexToDecimal(pData.Substring(Stt + (4 * 32), 4));
                         DM_733 = HexToDecimal(pData.Substring(Stt + (4 * 33), 4));
                         break;
                #endregion
            }
        }
        
        public static int HexToDecimal(string pHex)
        {
            if ((pHex == "") || (pHex == null)) pHex = "0";
            try
            {
                int Ret_Int = int.Parse(pHex, System.Globalization.NumberStyles.HexNumber);

                return Ret_Int;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region Wheelbase
        public static void PLC_Set_Dist(int[] Lent)
        {
            if (Lent.Length != 12) return;

            string Send_Str = "";
            long[] put = new long[24];

            for (int cnt = 0; cnt < Lent.Length; cnt++)
            {
                Lent[cnt] *= LentGain;
            }

            put[0] = Lent[0] % 65536; put[1] = (Lent[0] - put[0]) / 65536;
            put[2] = Lent[1] % 65536; put[3] = (Lent[1] - put[2]) / 65536;
            put[4] = Lent[2] % 65536; put[5] = (Lent[2] - put[4]) / 65536;
            put[6] = Lent[3] % 65536; put[7] = (Lent[3] - put[6]) / 65536;
            put[8] = Lent[4] % 65536; put[9] = (Lent[4] - put[8]) / 65536;
            put[10] = Lent[5] % 65536; put[11] = (Lent[5] - put[10]) / 65536;
            put[12] = Lent[6] % 65536; put[13] = (Lent[6] - put[12]) / 65536;
            put[14] = Lent[7] % 65536; put[15] = (Lent[7] - put[14]) / 65536;
            put[16] = Lent[8] % 65536; put[17] = (Lent[8] - put[16]) / 65536;
            put[18] = Lent[9] % 65536; put[19] = (Lent[9] - put[18]) / 65536;
            put[20] = Lent[10] % 65536; put[21] = (Lent[10] - put[20]) / 65536;
            put[22] = Lent[11] % 65536; put[23] = (Lent[11] - put[22]) / 65536;

            Send_Str = "03FF000A44200000" + "0214" + "18" + "00"; //D532 24EA
            Send_Str += put[0].ToString("X4");
            Send_Str += put[1].ToString("X4");
            Send_Str += put[2].ToString("X4");
            Send_Str += put[3].ToString("X4");
            Send_Str += put[4].ToString("X4");
            Send_Str += put[5].ToString("X4");
            Send_Str += put[6].ToString("X4");
            Send_Str += put[7].ToString("X4");
            Send_Str += put[8].ToString("X4");
            Send_Str += put[9].ToString("X4");
            Send_Str += put[10].ToString("X4");
            Send_Str += put[11].ToString("X4");
            Send_Str += put[12].ToString("X4");
            Send_Str += put[13].ToString("X4");
            Send_Str += put[14].ToString("X4");
            Send_Str += put[15].ToString("X4");
            Send_Str += put[16].ToString("X4");
            Send_Str += put[17].ToString("X4");
            Send_Str += put[18].ToString("X4");
            Send_Str += put[19].ToString("X4");
            Send_Str += put[20].ToString("X4");
            Send_Str += put[21].ToString("X4");
            Send_Str += put[22].ToString("X4");
            Send_Str += put[23].ToString("X4");

            Send_Message(Send_Str);
        }
        public static bool PLC_WhelBase(int Move)   //WB 거리설정 
        {
            bool Ret_Flag;
            string Send_Str = "";
            long[] put = new long[2];

            int Lent = (Move - PLC.OfSetL) * LentGain;

            put[0] = Lent % 65536;
            put[1] = (Lent - put[0]) / 65536;

            PLC.DO.DistReset = true; PLC.PLC_312_Puts();
            System.Threading.Thread.Sleep(100);

            Send_Str = "03FF000A44200000" + "0226" + "02" + "00"; //D550 2EA
            Send_Str += put[0].ToString("X4");
            Send_Str += put[1].ToString("X4");

            Send_Message(Send_Str);

            long OfstTime = DateTime.Now.Ticks;
            long WaitTime = DateTime.Now.AddSeconds(3).Ticks;
            long Gap_Time = DateTime.Now.AddSeconds(0.5).Ticks;

            while (true)
            {
                if (WaitTime - DateTime.Now.Ticks > 0) { Ret_Flag = false; break; }
                if (Move == PLC.Dist_J) { Ret_Flag = true; break; }

                if ((DateTime.Now.Ticks - Gap_Time) > 0)
                {
                    Gap_Time = DateTime.Now.AddSeconds(0.5).Ticks;

                    Send_Message(Send_Str);
                }

                System.Windows.Forms.Application.DoEvents();
            }

            System.Threading.Thread.Sleep(100);
            PLC.DO.DistReset = false; PLC.PLC_312_Puts();

            return Ret_Flag;
        }
        public static void DO_LengthSet()
        {
            Dist_A = HexToDecimal(DM_565.ToString("X4") + DM_564.ToString("X4")) / LentGain;
            Dist_B = HexToDecimal(DM_567.ToString("X4") + DM_566.ToString("X4")) / LentGain;
            Dist_C = HexToDecimal(DM_569.ToString("X4") + DM_568.ToString("X4")) / LentGain;
            Dist_D = HexToDecimal(DM_571.ToString("X4") + DM_570.ToString("X4")) / LentGain;
            Dist_E = HexToDecimal(DM_573.ToString("X4") + DM_572.ToString("X4")) / LentGain;
            Dist_F = HexToDecimal(DM_575.ToString("X4") + DM_574.ToString("X4")) / LentGain;
            Dist_G = HexToDecimal(DM_577.ToString("X4") + DM_576.ToString("X4")) / LentGain;
            Dist_H = HexToDecimal(DM_579.ToString("X4") + DM_578.ToString("X4")) / LentGain;
            Dist_I = HexToDecimal(DM_581.ToString("X4") + DM_580.ToString("X4")) / LentGain;
            Dist_J = HexToDecimal(DM_583.ToString("X4") + DM_582.ToString("X4")) / LentGain;
            OfSetL = HexToDecimal(DM_585.ToString("X4") + DM_584.ToString("X4")) / LentGain;
            OfSetR = HexToDecimal(DM_587.ToString("X4") + DM_586.ToString("X4")) / LentGain;
        }

        public static string Sel_Distance(string pkey)
        {
            string distance = "";

            switch (pkey)
            {
                case "Sel A": distance = (OfSetL + Dist_A).ToString(); break;
                case "Sel B": distance = (OfSetL + Dist_B).ToString(); break;
                case "Sel C": distance = (OfSetL + Dist_C).ToString(); break;
                case "Sel D": distance = (OfSetL + Dist_D).ToString(); break;
                case "Sel E": distance = (OfSetL + Dist_E).ToString(); break;
                case "Sel F": distance = (OfSetL + Dist_F).ToString(); break;
                case "Sel G": distance = (OfSetL + Dist_G).ToString(); break;
                case "Sel H": distance = (OfSetL + Dist_H).ToString(); break;
                case "Sel I": distance = (OfSetL + Dist_I).ToString(); break;
                case "Sel J": distance = (OfSetL + Dist_J).ToString(); break;
            }

            return distance;
        }
        #endregion

        #region PLC Output 처리
        public static void PLC_312_Mapp()
        {
            DOB312[00] = DO.PC_Standy; //D312[00] PC 준비
            DOB312[01] = DO.TestReady; //D312[01] Test 준비
            DOB312[02] = DO.TestStart; //D312[02] Test 시작
            DOB312[03] = DO.Test__End; //D312[03] Test 종료
            DOB312[04] = DO.Spd_5kmph; //D312[04] 속도 5km/h 이상
            DOB312[05] = DO.Test_Sett; //D312[05] Test 세팅(위치, 속도)
            DOB312[06] = DO.Home_Move; //D312[06] 홈으로 이동

            DOB312[08] = DO.DistReset; //D312[08] 거리 갱신
            DOB312[09] = DO.CalAirSol; //D312[09] 교정 솔
            DOB312[10] = DO.CalIndiOn; //D312[10] Calibration Indicter Power ON
            DOB312[12] = DO.BT_Mot_On; //D312[12] BT-Motor ON
            DOB312[13] = DO.BT_LiftUp; //D312[13] BT-Lift UP
            DOB312[14] = DO.BT_LiftDn; //D312[14] BT-LIFT Down
            DOB312[15] = DO.CalIndi_O; //D312[15] 인디게이터 영점
        }
        public static void PLC_312_Puts()
        {
            string Send_Str = "";
            long[] put = new long[1];

            PLC_312_Mapp();

            for (int cnt = 0; cnt < 16; cnt++)
            {
                put[0] = put[0] + (DOB312[cnt] ? H2Y.BitA[cnt] : 0);
            }

            Send_Str = "03FF000A44200000" + "0138" + "01" + "00"; //D312 1EA
            Send_Str += put[0].ToString("X4");

            Send_Message(Send_Str);

            return;

            long OfstTime = DateTime.Now.Ticks;
            long WaitTime = DateTime.Now.AddSeconds(Wait).Ticks;
            long Gap_Time = DateTime.Now.AddSeconds(GapT).Ticks;

            while (true)
            {
                System.Windows.Forms.Application.DoEvents();

                if ((DateTime.Now.Ticks - Gap_Time) > 0)
                {
                    Gap_Time = DateTime.Now.AddSeconds(GapT).Ticks;
                    Send_Message(Send_Str);
                }

                if (put[0] == DO_312)
                {
                    break;
                }

                if (DateTime.Now.Ticks - WaitTime > 0)
                {
                    Main.Prog_LogData("PLC D312 Send Time Over.");
                    Logs.MakeLog_File(Log_His.Err_, "PLC D312 Send Time Over. -> DO 312 " + put[0].ToString() + " : " + DO_312.ToString());
                    break;
                }
            }
        }

        public static void PLC_500_Mapp()
        {
            DOB530[00] = DO.FLMot_Stt; DOB530[08] = DO.FRMot_Stt;   //MOTOR START PC
            DOB530[01] = DO.FLMotSync; DOB530[09] = DO.FRMotSync;   //MOTOR 동기 PC
            DOB530[02] = DO.FLMotStop; DOB530[10] = DO.FRMotStop;   //MOTOR STOP BRAKE PC
            DOB530[03] = DO.FLMotPark; DOB530[11] = DO.FRMotPark;   //MOTOR PARKING 검사 PC

            DOB531[00] = DO.RLMot_Stt; DOB531[08] = DO.RRMot_Stt;   //MOTOR START PC
            DOB531[01] = DO.RLMotSync; DOB531[09] = DO.RRMotSync;   //MOTOR 동기 PC
            DOB531[02] = DO.RLMotStop; DOB531[10] = DO.RRMotStop;   //MOTOR STOP BRAKE PC
            DOB531[03] = DO.RLMotPark; DOB531[11] = DO.RRMotPark;   //MOTOR PARKING 검사 PC

                                       DOB531[14] = DO.Door_Open;   //BOOTH DOOR OPEN  PC
                                       DOB531[15] = DO.DoorClose;   //BOOTH DOOR CLOSE PC
        }
        public static void PLC_Put_D500()
        {
            string Send_Str = "";
            long[] put = new long[2];

            PLC_500_Mapp();

            for (int cnt = 0; cnt < 16; cnt++)
            {
                put[0] = put[0] + (DOB530[cnt] ? H2Y.BitA[cnt] : 0);
                put[1] = put[1] + (DOB531[cnt] ? H2Y.BitA[cnt] : 0);
            }

            Send_Str = "03FF000A44200000" + "0212" + "02" + "00"; //D100 17EA
            Send_Str += put[0].ToString("X4");
            Send_Str += put[1].ToString("X4");

            Send_Message(Send_Str);

            return;

            long OfstTime = DateTime.Now.Ticks;
            long WaitTime = DateTime.Now.AddSeconds(Wait).Ticks;
            long Gap_Time = DateTime.Now.AddSeconds(GapT).Ticks;

            while (true)
            {
                System.Windows.Forms.Application.DoEvents();

                if ((DateTime.Now.Ticks - Gap_Time) > 0)
                {
                    Gap_Time = DateTime.Now.AddSeconds(GapT).Ticks;
                    Send_Message(Send_Str);
                }

                if (put[0] == DO_530 && put[1] == DO_531)
                {
                    break;
                }

                if (DateTime.Now.Ticks - WaitTime > 0)
                {
                    Main.Prog_LogData("PLC D500 Send Time Over.");
                    Logs.MakeLog_File(Log_His.Err_, "PLC D500 Send Time Over. -> DO 530 " + put[0].ToString() + " : " + DO_530.ToString() + ", " + 
                                                                                "DO 531 " + put[1].ToString() + " : " + DO_531.ToString());
                    break;
                }
            }
        }

        public static void PLC_562_Mapp()
        {
            DOB562[00] = DO.Vehicle01;  //D562[00] VEHICLE SELECT 01 PC
            DOB562[01] = DO.Vehicle02;  //D562[01] VEHICLE SELECT 02 PC
            DOB562[02] = DO.Vehicle03;  //D562[02] VEHICLE SELECT 03 PC
            DOB562[03] = DO.Vehicle04;  //D562[03] VEHICLE SELECT 04 PC
            DOB562[04] = DO.Vehicle05;  //D562[04] VEHICLE SELECT 05 PC
            DOB562[05] = DO.Vehicle06;  //D562[05] VEHICLE SELECT 06 PC
            DOB562[06] = DO.Vehicle07;  //D562[06] VEHICLE SELECT 07 PC
            DOB562[07] = DO.Vehicle08;  //D562[07] VEHICLE SELECT 08 PC
            DOB562[08] = DO.Vehicle09;  //D562[08] VEHICLE SELECT 09 PC
            DOB562[09] = DO.Vehicle10;  //D562[09] VEHICLE SELECT 10 PC
            DOB562[10] = DO.PC__Start;  //D562[10] VEHICLE START PC
            DOB562[11] = DO.PC___Stop;  //D562[11] VEHICLE STOP PC
            DOB562[12] = DO.PC_Cancel;  //D562[12] VEHICLE CANCEL PC
          //DOB562[13]
            DOB562[14] = DO.PC_StadBy;  //D562[14] 운전준비 PC
            DOB562[15] = DO.PC__Reset;  //D562[15] 모터컨트롤러 리세트 PC
        }
        public static void PLC_Put_D562()
        {
            string Send_Str = "";
            long[] put = new long[1];

            PLC_562_Mapp();

            for (int cnt = 0; cnt < 16; cnt++)
            {
                put[0] = put[0] + (DOB562[cnt] ? H2Y.BitA[cnt] : 0);
            }

            Send_Str = "03FF000A44200000" + "0232" + "01" + "00"; //D562 1EA
            Send_Str += put[0].ToString("X4");

            Send_Message(Send_Str);

            return;

            long OfstTime = DateTime.Now.Ticks;
            long WaitTime = DateTime.Now.AddSeconds(Wait).Ticks;
            long Gap_Time = DateTime.Now.AddSeconds(GapT).Ticks;

            while (true)
            {
                System.Windows.Forms.Application.DoEvents();

                if ((DateTime.Now.Ticks - Gap_Time) > 0)
                {
                    Gap_Time = DateTime.Now.AddSeconds(GapT).Ticks;
                    Send_Message(Send_Str);
                }

                if (put[0] == DO_562)
                {
                    break;
                }

                if (DateTime.Now.Ticks - WaitTime > 0)
                {
                    Main.Prog_LogData("PLC D562 Send Time Over.");
                    Logs.MakeLog_File(Log_His.Err_, "PLC D562 Send Time Over. -> DO 562 " + put[0].ToString() + " : " + DO_562.ToString() );
                    break;
                }
            }
        }

        public static void Send_Message(string msg)
        {
            byte[] strBuffer = Encoding.UTF8.GetBytes(msg);

            if (Sock != null && Sock.Connected)
            {
                Sock.Send(strBuffer);
            }
        }
        #endregion

        public static void DI_HexToBits()
        {
            for (int i = 0; i < 16; i++)
            {
                DIB100[i] = ((DI_100 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB101[i] = ((DI_101 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB102[i] = ((DI_102 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB103[i] = ((DI_103 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB104[i] = ((DI_104 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB105[i] = ((DI_105 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB106[i] = ((DI_106 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB107[i] = ((DI_107 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB108[i] = ((DI_108 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB109[i] = ((DI_109 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB110[i] = ((DI_110 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB111[i] = ((DI_111 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB112[i] = ((DI_112 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB113[i] = ((DI_113 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB114[i] = ((DI_114 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB115[i] = ((DI_115 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB116[i] = ((DI_116 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB117[i] = ((DI_117 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB118[i] = ((DI_118 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB119[i] = ((DI_119 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DIB120[i] = ((DI_120 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
            }

            #region IO Mapping
            DI.PBCalMode = DIB102[09];  //D102.9 CALIBRATION PB
            DI.SST_Enter = DIB102[14];  //D102.E SST IN-PHS
            DI.SST_GoOut = DIB102[15];  //D102.F SST OUT-PHS

            DI.FL_MotErr = DIB103[04];  //D103.4 FL-MOTOR DRIVER ERROR
            DI.FR_MotErr = DIB103[05];  //D103.5 FR-MOTOR DRIVER ERROR
            DI.RL_MotErr = DIB103[06];  //D103.6 RL-MOTOR DRIVER ERROR
            DI.RR_MotErr = DIB103[07];  //D103.7 RR-MOTOR DRIVER ERROR
            DI.PSW___Emg = DIB103[08];  //D103.8 PULL SW START

            if (old_Stop == DIB103[09]) 
            {
                CNT_Stop++;
            }
            else
            {
                CNT_Stop = 0;
            }
            if (CNT_Stop > PSet.CNT_Stop)
            {
                CNT_Stop = 100;
                DI.PSW__Stop = DIB103[9]; //D103.9 PULL SW STOP
            }
            old_Stop = DIB103[9];

            DI.PSW_Check = DIB103[10];  //D103.A PULL SW 40Km/CANCEL 
            DI.PSW___Emg = DIB103[11];  //D103.B PULL SW EMERGENCY 
            DI.Rmt_State = DIB103[14];  //D103.E Wireless Remote Ready 

            DI.FL_RBFree = DIB104[01];  //D104.1 FL-ROLLER BRAKE FREE LS
            DI.FLF_SR_Dn = DIB104[02];  //D104.2 FL-FRONT SAFETY ROLLER DOWN LS
            DI.FLF_SR_Up = DIB104[03];  //D104.3 FL-FRONT SAFETY ROLLER UP LS
            DI.FLR_SR_Dn = DIB104[04];  //D104.4 FL-REAR SAFETY ROLLER DOWN LS
            DI.FLR_SR_Up = DIB104[05];  //D104.5 FL-REAR SAFETY ROLLER UP LS
            DI.L_Post_Dn = DIB104[14];  //D104.E L-SAFETY POST DOWN LS
            DI.L_Post_Up = DIB104[15];  //D104.F L-SAFETY POST UP LS

            DI.PHO_Brake = DIB105[00];  //D105.0 BT-PHS
            DI.RL_RBFree = DIB105[01];  //D105.1 RL-ROLLER BRAKE FREE LS
            DI.RLR_SR_Dn = DIB105[02];  //D105.2 RL-SAFETY ROLLER DOWN LS
            DI.RLR_SR_Up = DIB105[03];  //D105.3 RL-SAFETY ROLLER UP LS
            DI.L_Flap_Dn = DIB105[04];  //D105.4 L-EXHAUST FLAP DOWN LS
            DI.L_Flap_Up = DIB105[05];  //D105.5 L-EXHAUST FLAP UP LS
            DI.PHO_Front = DIB105[09];  //D105.9 F-PHS
            DI.BT_LiftUp = DIB105[10];  //D105.A BT-Lift Up LS
            DI.BT_LiftDn = DIB105[11];  //D105.B BT-Lift Down LS
            DI.WB_L_Home = DIB105[12];  //D105.C L-WB HOME LS(X23D 참조)
            DIB105[13] = !DIB105[13]; DI.WB_L__Min = DIB105[13]; //D105.D L-WB MIN LS(X23E 참조)
            DIB105[14] = !DIB105[14]; DI.WB_L__Max = DIB105[14]; //D105.E L-WB MAX LS(X23F 참조)


            DI.FR_RBFree = DIB106[01];  //D106.1 FR-ROLLER BRAKE FREE LSb
            DI.FRF_SR_Dn = DIB106[02];  //D106.2 FR-FRONT SAFETY ROLLER DOWN LS 
            DI.FRF_SR_Up = DIB106[03];  //D106.3 FR-FRONT SAFETY ROLLER UP LS 
            DI.FRR_SR_Dn = DIB106[04];  //D106.4 FR-REAR SAFETY ROLLER DOWN LS 
            DI.FRR_SR_Up = DIB106[05];  //D106.5 FR-REAR SAFETY ROLLER UP LS            
            DI.R_Post_Dn = DIB106[14];  //D106.E R-SAFETY POST DOWN LS
            DI.R_Post_Up = DIB106[15];  //D106.F R-SAFETY POST UP LS


            DI.RR_RBFree = DIB107[01];  //D107.1 RR-ROLLER BRAKE FREE LS
            DI.RRR_SR_Dn = DIB107[02];  //D107.2 RR-SAFETY ROLLER DOWN LS
            DI.RRR_SR_Up = DIB107[03];  //D107.3 RR-SAFETY ROLLER UP LS
            DI.R_Flap_Dn = DIB107[04];  //D107.4 R-EXHAUST FLAP DOWN LS
            DI.R_Flap_Up = DIB107[05];  //D107.5 R-EXHAUST FLAP Up LS
            DI.PHO__Rear = DIB107[09];  //D107.9 R-PHS
            DI.WB_R_Home = DIB107[13];  //D107.D R-WB HOME LS
            DIB107[14] = !DIB107[14]; DI.WB_R__Min = DIB107[14];  //D107.E R-WB MIN  LS
            DIB107[15] = !DIB107[15]; DI.WB_R__Max = DIB107[15];  //D107.F R-WB MAX  LS

            if (DI.FLF_SR_Up == true & DI.FRF_SR_Up == true & DI.FLR_SR_Up == true & DI.FRR_SR_Up == true &
                DI.RLR_SR_Up == true & DI.RRR_SR_Up == true)
            {
                DI.StandByUp = true;
            }
            else
            {
                DI.StandByUp = false;
            }

            if (DI.FLF_SR_Dn == true & DI.FRF_SR_Dn == true & DI.FLR_SR_Dn == true & DI.FRR_SR_Dn == true & 
                DI.RLR_SR_Dn == true & DI.RRR_SR_Dn == true)
            {
                DI.StandByDn = true;
            }
            else
            {
                DI.StandByDn = false;
            }

            DI.Start__PB = DIB108[13];  //D108.A VEHICLE START     PB
            DI.Stop___PB = DIB108[14];  //D108.B VEHICLE STOP      PB
            DI.Cancel_PB = DIB108[15];  //D108.C VEHICLE CANCEL    PB
            DI.EMG____PB = DIB108[15];  //D108.D VEHICLE EMERGENCY PB

            DI.GOT_Start = DIB114[10];  //D114.A VEHICLE START     GOT
            DI.GOT__Stop = DIB114[11];  //D114.B VEHICLE STOP      GOT
            DI.GOTCancel = DIB114[12];  //D114.C VEHICLE CANCEL    GOT
            DI.GOT___EMG = DIB114[13];  //D114.D VEHICLE EMERGENCY GOT

            if (DO.MD_Emerge) { TSet.StopFlag = 1; }    //EMERGENCY
            if (DI.PSW__Stop) { TSet.StopFlag = 2; }    //정지 스위치

            Main.lbl_Rmt1.BackColor = DI.PSW___Emg ? System.Drawing.Color.Red : System.Drawing.Color.White;
            Main.lbl_Rmt2.BackColor = DI.PSW__Stop ? System.Drawing.Color.Red : System.Drawing.Color.White;
            Main.lbl_Rmt3.BackColor = DI.PSW_Check ? System.Drawing.Color.Red : System.Drawing.Color.White;
            #endregion
        }
        public static void DO_HexToBits()
        {
            for (int i = 0; i < 16; i++)
            {
                DOB300[i] = ((DO_300 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB301[i] = ((DO_301 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB302[i] = ((DO_302 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB303[i] = ((DO_303 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB304[i] = ((DO_304 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB305[i] = ((DO_305 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB306[i] = ((DO_306 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB307[i] = ((DO_307 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB308[i] = ((DO_308 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB309[i] = ((DO_309 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB310[i] = ((DO_310 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB311[i] = ((DO_311 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB312[i] = ((DO_312 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB313[i] = ((DO_313 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB314[i] = ((DO_314 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB315[i] = ((DO_315 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB316[i] = ((DO_316 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
                DOB317[i] = ((DO_317 & H2Y.BitA[i]) == H2Y.BitA[i] ? true : false);
            }

            #region IO Mapping
            DO.WBMoveing = DOB301[00];  //WB Motor Moving
            DO.BT_MotRun = DOB303[10];  //D303[10] BT-Motor ON
            DO.Indicator = DOB303[13];  //D303[13] Calibration Indicter Power ON
            
            //읽기 전용
            DO.Select_01 = DOB306[00];  //VEHICLE SELECT 01 LP
            DO.Select_02 = DOB306[01];  //VEHICLE SELECT 02 LP
            DO.Select_03 = DOB306[02];  //VEHICLE SELECT 03 LP
            DO.Select_04 = DOB306[03];  //VEHICLE SELECT 04 LP
            DO.Select_05 = DOB306[04];  //VEHICLE SELECT 05 LP
            DO.Select_06 = DOB306[05];  //VEHICLE SELECT 06 LP
            DO.Select_07 = DOB306[06];  //VEHICLE SELECT 07 LP
            DO.Select_08 = DOB306[07];  //VEHICLE SELECT 08 LP
            DO.Select_09 = DOB306[08];  //VEHICLE SELECT 09 LP
            DO.Select_10 = DOB306[09];  //VEHICLE SELECT 10 LP
            
            DO.MD__Ready = DOB311[0];  //D311[0] 장비 Ready
            DO.MD___Auto = DOB311[1];  //D311[1] 장비 Auto 모드
            DO.MD_Manual = DOB311[2];  //D311[2] 장비 Manual 모드
            DO.MD___Stop = DOB311[3];  //D311[3] 장비 Stop 모드
            DO.MD_FirstH = DOB311[4];  //D311[4] 홈 (처음) 인식
            DO.MD_Emerge = DOB311[5];  //D311[5] 장비 비상
            DO.Pos__Home = DOB311[6];  //D311[6] 홈   포지션
            DO.Pos_Enter = DOB311[7];  //D311[7] 장비 포지션(진입)    
            DO.Pos__Test = DOB311[8];  //D311[8] 장비 포지션(측정)
            DO.Pos__Exit = DOB311[9];  //D311[9] 장비 포지션(퇴출)

            DO.PC_Standy = DOB312[0];  //D312[0] PC 준비
            DO.TestReady = DOB312[1];  //D312[1] Test 준비
            DO.TestStart = DOB312[2];  //D312[2] Test 시작
            DO.Test__End = DOB312[3];  //D312[3] Test 종료
            DO.Spd_5kmph = DOB312[4];  //D312[4] 속도 5km/h 이상
            DO.Test_Sett = DOB312[5];  //D312[5] Test 세팅(위치, 속도)
            DO.Home_Move = DOB312[6];  //D312[6] 홈으로 이동

            DO.DistReset = DOB312[8];  //D312[8] 거리 갱신
            DO.CalAirSol = DOB312[9];  //D312[9] 교정 솔
            DO.CalIndiOn = DOB312[10]; //D312[10] Calibration Indicter Power ON
            DO.BT_Mot_On = DOB312[12]; //D312[12] BT-Motor ON
            DO.BT_LiftUp = DOB312[13]; //D312[13] BT-Lift UP
            DO.BT_LiftDn = DOB312[14]; //D312[14] BT-LIFT Down
            DO.CalIndi_O = DOB312[15]; //D312[15] 인디게이터 영점
            
            Main.lbl_Home.BackColor = DO.Pos__Home ? System.Drawing.Color.Lime : System.Drawing.Color.White;
            Main.lblEnter.BackColor = DO.Pos_Enter ? System.Drawing.Color.Lime : System.Drawing.Color.White;
            Main.lbl_Test.BackColor = DO.Pos__Test ? System.Drawing.Color.Lime : System.Drawing.Color.White;
            Main.lbl_Exit.BackColor = DO.Pos__Exit ? System.Drawing.Color.Lime : System.Drawing.Color.White;

            Main.lblReady.BackColor = DO.TestReady ? System.Drawing.Color.Lime : System.Drawing.Color.White;
            Main.lblStart.BackColor = DO.TestStart ? System.Drawing.Color.Lime : System.Drawing.Color.White;
            Main.lbl__End.BackColor = DO.Test__End ? System.Drawing.Color.Lime : System.Drawing.Color.White;

            Main.lbl_Open.BackColor = DO.Door_Open ? System.Drawing.Color.Lime : System.Drawing.Color.White;
            Main.lblClose.BackColor = DO.DoorClose ? System.Drawing.Color.Lime : System.Drawing.Color.White;
            #endregion
        }

        public static void Test___Ready()
        {
            if (DO.TestReady && !DO.TestStart && !DO.Test__End) { return; }

            if (Main != null)
            {
                Main.Prog_LogData("PLC Test Ready");
                Logs.MakeLog_File(Log_His.Msg_, "PLC Test Ready");
            }
            
            DO.TestReady = true;
            DO.TestStart = false;
            DO.Test__End = false;  
            PLC_312_Puts();
        }
        public static void Test___Start()
        {
            if (DO.TestReady && DO.TestStart && !DO.Test__End) { return; }

            if (Main != null)
            {
                Main.Prog_LogData("PLC Test Start");
                Logs.MakeLog_File(Log_His.Msg_, "PLC Test Start");
            }

            DO.TestReady = true;
            DO.TestStart = true;
            DO.Test__End = false;
            PLC_312_Puts();
        }
        public static void Test__Finish()
        {
            //if (!DO.TestReady && !DO.TestStart && DO.Test__End) { return; }

            if (Main != null)
            {
                Main.Prog_LogData("PLC Test Finish");
                Logs.MakeLog_File(Log_His.Msg_, "PLC Test Finish");
            }

            DO.TestReady = false;
            DO.TestStart = false;
            DO.Test__End = true;
            PLC_312_Puts();
        }
        public static void Test_All_Off()
        {
            if (!DO.TestReady && !DO.TestStart && !DO.Test__End) { return; }

            if (Main != null)
            {
                Main.Prog_LogData("PLC Test All Off");
                Logs.MakeLog_File(Log_His.Msg_, "PLC Test All Off");
            }

            DO.TestReady = false;
            DO.TestStart = false;
            DO.Test__End = false;
            PLC_312_Puts();
        }
        
        public static void Door____Open()
        {
            if (PSet.Use_Door == 0) { return; }
            if (DO.Door_Open) { return; }

            if (Main != null)
            {
                Main.Prog_LogData("Booth Door Open");
                Logs.MakeLog_File(Log_His.Msg_, "Booth Door Open");
            }

            DO.Door_Open = true;
            DO.DoorClose = false;

            PLC_Put_D500();
        }
        public static void Door___Close()
        {
            if (PSet.Use_Door == 0) { return; }
            if (DO.DoorClose) { return; }

            if (Main != null)
            {
                Main.Prog_LogData("Booth Door Close");
                Logs.MakeLog_File(Log_His.Msg_, "Booth Door Close");
            }

            DO.Door_Open = false;
            DO.DoorClose = true;

            PLC_Put_D500();
        }

        #region Normal Brake
        public static void Brk_LiftDown()
        {
            if (!TSet.SimulOnf)
            {
                DO.BT_Mot_On = false; //D312[12] BT-Motor ON
                DO.BT_LiftUp = false; //D312[13] BT-Lift UP
                DO.BT_LiftDn = true;  //D312[14] BT-LIFT Down
                PLC_312_Puts();
            }

            if (TSet.Test_Run) { return; }

            double OfstTime = DateTime.Now.Ticks;
            double ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;
            double Gap_Ofst = 0;
            
            while (true)
            {
                TSet.Scan_Sensors();

                if (TSet.TestStop) { break; }
                if (TSet.BT_LiftDn) { break; }

                ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;

                if (ReadTime - Gap_Ofst > 0.5)
                {
                    Gap_Ofst = ReadTime;
                    PLC_312_Puts();
                }

                System.Windows.Forms.Application.DoEvents();
            }

            if (!TSet.SimulOnf)
            {
                DO.BT_Mot_On = false; //D312[12] BT-Motor ON
                DO.BT_LiftUp = false; //D312[13] BT-Lift UP
                DO.BT_LiftDn = false;//D312[14] BT-LIFT Down
                PLC_312_Puts();
            }
        }
        public static void Brk_Lift__Up()
        {
            if (!TSet.SimulOnf)
            {
                DO.BT_Mot_On = false; //D312[12] BT-Motor ON
                DO.BT_LiftUp = true;  //D312[13] BT-Lift UP
                DO.BT_LiftDn = false; //D312[14] BT-LIFT Down
                PLC_312_Puts();
            }

            if (TSet.Test_Run) { return; }

            double OfstTime = DateTime.Now.Ticks;
            double ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;
            double Gap_Ofst = 0;

            while (true)
            {
                TSet.Scan_Sensors();

                if (TSet.TestStop) { break; }
                if (TSet.BT_LiftUp) { break; }

                ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;

                if (ReadTime - Gap_Ofst > 0.5)
                {
                    Gap_Ofst = ReadTime;
                    PLC_312_Puts();
                }

                System.Windows.Forms.Application.DoEvents();
            }

            if (!TSet.SimulOnf)
            {
                DO.BT_Mot_On = false; //D312[12] BT-Motor ON
                DO.BT_LiftUp = false; //D312[13] BT-Lift UP
                DO.BT_LiftDn = false; //D312[14] BT-LIFT Down
                PLC_312_Puts();
            }
        }

        public static void Brk_MotorRun(int pMode)
        {
            if (!TSet.SimulOnf)
            {
                DO.BT_Mot_On = (pMode == 0 ? false : true); //D312[12] BT-Motor ON
                PLC_312_Puts();
            }

            if (TSet.Test_Run) { return; }

            double OfstTime = DateTime.Now.Ticks;
            double ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;
            double Gap_Ofst = 0;
            
            while (true)
            {
                TSet.Scan_Sensors();

                if (TSet.TestStop) { break; }
                if (pMode == 0 && TSet.BT_MotRun == false) { break; }
                if (pMode == 1 && TSet.BT_MotRun == true) { break; }

                ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;

                if (ReadTime - Gap_Ofst > 0.5)
                {
                    Gap_Ofst = ReadTime;
                    PLC_312_Puts();
                }

                System.Windows.Forms.Application.DoEvents();
            }
        }
        #endregion

        #region Motor Drive Controll
        public static void MOT_SyncTest(string drive)  //동기 Test On/Off(0:Off, 1:Front On, 2:Rear On, 3:All On)
        {
            int axle = 0;

            switch (drive)
            {
                case "Front": axle = 1; break;
                case "Rear":  axle = 2; break;
                case "4WD":   axle = 0; break;
                case "Free":  axle = 0; break;
            }

            MOT_ErrClear();
            MOT__BrkStop(0, 0);     //Off
            MOT__Parking(0, 0);     //Off
            MOT_Synchron(axle, 0); //동기 신호
            MOT_PowerOnf(axle, 0); //구동 신호

            PLC_Put_D500();
        }

        public static void MOT_ParkTest(string drive)  //동기 Test On/Off(0:Off, 1:Front On, 2:Rear On, 3:All On)
        {
            int axle = 0;

            switch (drive)
            {
                case "Front": axle = 1; break;
                case "Rear":  axle = 2; break;
                case "4WD":   axle = 0; break;
                case "Free":  axle = 0; break;
            }

            MOT_ErrClear();
            MOT__BrkStop(0, 0);     //Off
            MOT__Parking(2, 0);     //Off
            MOT_Synchron(0, 0);     //Off
            MOT_PowerOnf(axle, 0);  //구동 신호

            PLC_Put_D500();
        }

        private static void MOT_ErrClear()
        {
            if (PLC.DO.MD__Ready != false)
            {
                PLC.DO.PC_StadBy = false; PLC.DO.PC__Reset = true;  PLC.PLC_Put_D562(); //에러 확인 시 Reset
                H2Y.Sleep(200);
                PLC.DO.PC_StadBy = true;  PLC.DO.PC__Reset = false; PLC.PLC_Put_D562(); //에러 확인 시 Ready
                H2Y.Sleep(200);
                PLC.DO.PC_StadBy = false; PLC.DO.PC__Reset = false; PLC.PLC_Put_D562(); //에러 확인 시 Reset/Ready 해제
                H2Y.Sleep(200);
            }
        }
        public static void MOT__BrkStop(int pOnf, int mode)  //Stop Brake(0:Off, 1:Front On, 2:Rear On, 3:All On)
        {
            switch (pOnf)
            {
                case 0: PLC.DO.FLMotStop = false;  PLC.DO.FRMotStop = false;  PLC.DO.RLMotStop = false;  PLC.DO.RRMotStop = false;  break;
                case 1: PLC.DO.FLMotStop = true;   PLC.DO.FRMotStop = true;   PLC.DO.RLMotStop = false;  PLC.DO.RRMotStop = false;  break;
                case 2: PLC.DO.FLMotStop = false;  PLC.DO.FRMotStop = false;  PLC.DO.RLMotStop = true;   PLC.DO.RRMotStop = true;   break;
                case 3: PLC.DO.FLMotStop = true;   PLC.DO.FRMotStop = true;   PLC.DO.RLMotStop = true;   PLC.DO.RRMotStop = true;   break;
            }

            if (mode == 0) { PLC_Put_D500(); }
        }
        public static void MOT__Parking(int pOnf, int mode)  //Parking Brake(0:Off, 1:Front On, 2:Rear On, 3:All On)
        {
            switch (pOnf)
            {
                case 0: PLC.DO.FLMotPark = false;  PLC.DO.FRMotPark = false;  PLC.DO.RLMotPark = false;  PLC.DO.RRMotPark = false;  break;
                case 1: PLC.DO.FLMotPark = true;   PLC.DO.FRMotPark = true;   PLC.DO.RLMotPark = false;  PLC.DO.RRMotPark = false;  break;
                case 2: PLC.DO.FLMotPark = false;  PLC.DO.FRMotPark = false;  PLC.DO.RLMotPark = true;   PLC.DO.RRMotPark = true;   break;
                case 3: PLC.DO.FLMotPark = true;   PLC.DO.FRMotPark = true;   PLC.DO.RLMotPark = true;   PLC.DO.RRMotPark = true;   break;
            }

            if (mode == 0) { PLC_Put_D500(); }
        }
        public static void MOT_Synchron(int pOnf, int mode)  //Synchronization(0:Off, 1:Front On, 2:Rear On, 3:All On)
        {
            switch (pOnf)
            {
                case 0: PLC.DO.FLMotSync = false;  PLC.DO.FRMotSync = false;  PLC.DO.RLMotSync = false;  PLC.DO.RRMotSync = false;  break;
                case 1: PLC.DO.FLMotSync = false;  PLC.DO.FRMotSync = false;  PLC.DO.RLMotSync = true;   PLC.DO.RRMotSync = true;   break;
                case 2: PLC.DO.FLMotSync = true;   PLC.DO.FRMotSync = true;   PLC.DO.RLMotSync = false;  PLC.DO.RRMotSync = false;  break;
                case 3: PLC.DO.FLMotSync = true;   PLC.DO.FRMotSync = true;   PLC.DO.RLMotSync = true;   PLC.DO.RRMotSync = true;   break;
            }

            if (mode == 0) { PLC_Put_D500(); }
        }
        public static void MOT_PowerOnf(int pOnf, int mode)  //Power On/Off(0:Off, 1:Front On, 2:Rear On, 3:All On)
        {
            switch (pOnf)
            {
                case 0: PLC.DO.FLMot_Stt = false;  PLC.DO.FRMot_Stt = false;  PLC.DO.RLMot_Stt = false;  PLC.DO.RRMot_Stt = false;  break;
                case 1: PLC.DO.FLMot_Stt = false;  PLC.DO.FRMot_Stt = false;  PLC.DO.RLMot_Stt = true;   PLC.DO.RRMot_Stt = true;   break;
                case 2: PLC.DO.FLMot_Stt = true;   PLC.DO.FRMot_Stt = true;   PLC.DO.RLMot_Stt = false;  PLC.DO.RRMot_Stt = false;  break;
                case 3: PLC.DO.FLMot_Stt = true;   PLC.DO.FRMot_Stt = true;   PLC.DO.RLMot_Stt = true;   PLC.DO.RRMot_Stt = true;   break;
            }

            if (mode == 0) { PLC_Put_D500(); }
        }
        #endregion
    }
}
