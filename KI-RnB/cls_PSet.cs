using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;

namespace KI_RnB
{
    public struct Loss_Items
    {
        public double SpdS { get; set; }
        public double SpdE { get; set; }
        public double RpmS { get; set; }
        public double RpmE { get; set; }
        public double Time { get; set; }
        public double ChkM { get; set; }
        public double ChkB { get; set; }
        public double Loss { get; set; }
    }
    public class Loss_Cal
    {
        public Loss_Items[] Item = new Loss_Items[3];
        public Loss_Items Aver;

        public void Clear()
        {
            for (int cnt = 0; cnt < Item.Length; cnt++)
            {
                Item[cnt].SpdS = 0;
                Item[cnt].SpdE = 0;
                Item[cnt].RpmS = 0;
                Item[cnt].RpmE = 0;
                Item[cnt].Time = 0;
                Item[cnt].ChkM = 0;
                Item[cnt].ChkB = 0;
                Item[cnt].Loss = 0;
            }
        }
    }

    public struct Load_Items
    {
        public double SpdS { get; set; }
        public double SpdE { get; set; }
        public double RpmS { get; set; }
        public double RpmE { get; set; }
        public double Time { get; set; }
        public double Devi { get; set; }
        public double CalD { get; set; }
        public double Indi { get; set; }
        public double Calc { get; set; }
    }
    public class Load_Cal
    {
        public Load_Items[] Item = new Load_Items[3];
        public Load_Items Aver;

        public void Clear()
        {
            for (int cnt = 0; cnt < Item.Length; cnt++)
            {
                Item[cnt].SpdS = 0;
                Item[cnt].SpdE = 0;
                Item[cnt].RpmS = 0;
                Item[cnt].RpmE = 0;
                Item[cnt].Time = 0;
                Item[cnt].Devi = 0;
                Item[cnt].CalD = 0;
                Item[cnt].Indi = 0;
                Item[cnt].Calc = 0;
            }
        }
    }

    public static class PSet
    {
        public static string Sett_Def = Application.StartupPath + @"\Setting\MachineSet.def";
        public static string Size_Def = Application.StartupPath + @"\Setting\siz_Form.def";
        public static string ParamDef = Application.StartupPath + @"\Setting\Parameter.def";
        public static string LossFDef = Application.StartupPath + @"\Setting\LossFactor.def";
        public static string LoadFDef = Application.StartupPath + @"\Setting\LoadFactor.def";
        public static string LangFile = Application.StartupPath + @"\Setting\Language.xlsx";
        
        public static DateTime Sel_Date;
        public const double NewTGain = 9.80665;
        public const float  Stop_Spd = 0.2f;    //이하 수치시 정지로 간주
        public const int FlashOnf = 1;
        public const float GurveMax = 200f; 
        public const string ECU_Serial = "";    //ECU 연결 장비 시리얼 번호 비교
        
        #region 화면 활성화 상태
        public static bool OnfOwner;    //관리자 
        public static bool Onf_Prog;    //종료 신호

        public static bool OnfDebug;    //Debug Screen
        public static bool Onf_Stop;
        public static bool OnfSetup;
        public static bool Onf_PsWd;
        #endregion

        #region Program Setting Variable declaration
        public static string Passwd { get; set; }   //Password

        public static string PLC__S { get; set; }   //PLC Setting
        public static int PLC__P    { get; set; }   //PLC Port

        public static string Ctrl_S { get; set; }   //Controller Setting
        public static int Ctrl_P    { get; set; }   //Controller Port

        public static string Indi_S { get; set; }   //Indicator Setting
        public static int Indi_P    { get; set; }   //Indicator Port

        public static string MDrv_S { get; set; }   //Motor Drive Setting
        public static int MDrv_P    { get; set; }   //Motor Drive Port

        public static string BarC1S { get; set; }   //Barcode 1 Setting
        public static int BarC1P    { get; set; }   //Barcode 1 Port

        public static string PedalS { get; set; }   //Pedal Brake Setting
        public static int PedalP    { get; set; }   //Pedal Brake Port 

        public static int CalCyc    { get; set; }   //Calibration Cycle
        public static int T__CNT    { get; set; }   //Test Count
        public static int T_Fail    { get; set; }   //Test Fail Count
        public static int T_Pass    { get; set; }   //Test Pass Count
        public static int sPrint    { get; set; }   //Print Mode
        public static int KISpeach  { get; set; }   //음성 지원

        public static int WB_Min    { get; set; }   //Wheelbase Min.
        public static int WB_Max    { get; set; }   //Wheelbase Max.
        public static int WB_Ofs    { get; set; }   //Wheelbase Off Set

        public static int RFLDia    { get; set; }   //Roll FL Diameter
        public static int RFLPul    { get; set; }   //Roll FL Pulse

        public static int RFRDia    { get; set; }   //Roll FR Diameter
        public static int RFRPul    { get; set; }   //Roll FR Pulse

        public static int RRLDia    { get; set; }   //Roll RL Diameter
        public static int RRLPul    { get; set; }   //Roll RL Pulse

        public static int RRRDia    { get; set; }   //Roll RR Diameter
        public static int RRRPul    { get; set; }   //Roll RR Pulse

        public static int PLC_GapT  { get; set; }   //PLC Gap Time
        public static int CNT_Stop  { get; set; }   //STOP Signal

        public static float WGT_Capa { get; set; }   //축중   용량 kg
        public static float WGTLimit { get; set; }   //축중   최저 kg
        public static float WGT_Safe { get; set; }   //축중   안정 kg
        
        public static float BRK_Capa { get; set; }   //제동력 용량 kg
        public static float BrkRatio { get; set; }   //제동력 교정 배율 (%)
        public static float BRKCount { get; set; }   //제동력 재측정

        public static string Dir_Sett { get; set; }
        public static string Dir_DB { get; set; }
        public static string Dir_Log { get; set; }
        public static string Dir_Data { get; set; }
        public static string Dir_Img { get; set; }
        public static string Dir_Crv { get; set; }

        public static int CH0Scan { get; set; }     //AD 0 Scan
        public static int CH1Scan { get; set; }     //AD 1 Scan
        public static int CH2Scan { get; set; }     //AD 2 Scan
        public static int CH3Scan { get; set; }     //AD 3 Scan
        public static int CH4Scan { get; set; }     //AD 4 Scan
        public static int CH5Scan { get; set; }     //AD 5 Scan

        public static int CH0Zero { get; set; }     //AD 0 Zero
        public static int CH1Zero { get; set; }     //AD 1 Zero
        public static int CH2Zero { get; set; }     //AD 2 Zero
        public static int CH3Zero { get; set; }     //AD 3 Zero
        public static int CH4Zero { get; set; }     //AD 4 Zero
        public static int CH5Zero { get; set; }     //AD 5 Zero

        public static int CH0Last { get; set; }     //AD 0 Last
        public static int CH1Last { get; set; }     //AD 1 Last
        public static int CH2Last { get; set; }     //AD 2 Last
        public static int CH3Last { get; set; }     //AD 3 Last
        public static int CH4Last { get; set; }     //AD 4 Last
        public static int CH5Last { get; set; }     //AD 5 Last

        public static float CH0Span { get; set; }   //AD 0 Zero
        public static float CH1Span { get; set; }   //AD 1 Zero
        public static float CH2Span { get; set; }   //AD 2 Zero
        public static float CH3Span { get; set; }   //AD 3 Zero
        public static float CH4Span { get; set; }   //AD 4 Zero
        public static float CH5Span { get; set; }   //AD 5 Zero

        public static float CH0_Val { get; set; }   //AD 0 Value
        public static float CH1_Val { get; set; }   //AD 1 Value
        public static float CH2_Val { get; set; }   //AD 2 Value
        public static float CH3_Val { get; set; }   //AD 3 Value
        public static float CH4_Val { get; set; }   //AD 4 Value
        public static float CH5_Val { get; set; }   //AD 5 Value

        public static int Filter { get; set; }      //Filter Mode
        public static int Av_Filt { get; set; }     //Average Filter
        public static int St_Filt { get; set; }     //Sort    Filter

        public static int USpeed { get; set; }      //Speed    Unit
        public static int UBrake { get; set; }      //Brake    Unit
        public static int U_Dist { get; set; }      //Distance Unit

        public static int Lent_A { get; set; }      //Distance A
        public static int Lent_B { get; set; }      //Distance B
        public static int Lent_C { get; set; }      //Distance C
        public static int Lent_D { get; set; }      //Distance D
        public static int Lent_E { get; set; }      //Distance E
        public static int Lent_F { get; set; }      //Distance F
        public static int Lent_G { get; set; }      //Distance G
        public static int Lent_H { get; set; }      //Distance H
        public static int Lent_I { get; set; }      //Distance I
        public static int Lent_J { get; set; }      //Distance J

        public static double FL_Moment { get; set; }   //Moment of inertia(관성 모멘트) 55909249.02 kg.mm2
        public static double FR_Moment { get; set; }   //Moment of inertia(관성 모멘트) 55909249.02 kg.mm2
        public static double RL_Moment { get; set; }   //Moment of inertia(관성 모멘트) 55909249.02 kg.mm2
        public static double RR_Moment { get; set; }   //Moment of inertia(관성 모멘트) 55909249.02 kg.mm2

        public static double FL_MRatio { get; set; }   //Motor to Roller ratio (기어비) 6.6666666
        public static double FR_MRatio { get; set; }   //Motor to Roller ratio (기어비) 6.6666666
        public static double RL_MRatio { get; set; }   //Motor to Roller ratio (기어비) 6.6666666
        public static double RR_MRatio { get; set; }   //Motor to Roller ratio (기어비) 6.6666666

        public static double FL_Factor { get; set; }   //Factor
        public static double FR_Factor { get; set; }   //Factor
        public static double RL_Factor { get; set; }   //Factor
        public static double RR_Factor { get; set; }   //Factor

        public static int OwnerS00 { get; set; }        //언어 선택
        public static int OwnerS01 { get; set; }        //기준 속도 설정
        public static int OwnerS02 { get; set; }        //끌림 수식 설정
        public static int OwnerS03 { get; set; }        //RED(시리얼번호) 사용 설정
        public static int OwnerS04 { get; set; }        //RED(시리얼번호)

        public static int OwnerS05 { get; set; }        //Drag Judge
        public static int OwnerS06 { get; set; }        //Brake Judge
        public static int OwnerS07 { get; set; }        //Parking Judge
        public static int OwnerS08 { get; set; }        //Speedometer Judge
        public static int OwnerS09 { get; set; }        //Balance Judge
        public static int OwnerS0A { get; set; }        //WSS Judge
        public static int OwnerS0B { get; set; }        //Decrease Judge
        public static int OwnerS0C { get; set; }        //Increase Judge

        public static int SST_Type { get; set; }        //0:측정 않음, 1:막대 그래프, 2:숫자만
        public static int Brk_Type { get; set; }        //0:측정 않음, 1:측정
        public static int Use_Door { get; set; }        //0:사용 않음, 1:사용
        
        public static int OwnerDrv { get; set; }        //Motor Drive
        public static int OwnerSpd { get; set; }        //Calibration Speed
        public static int OwnerToq { get; set; }        //Parking     Torque
        public static int OwnerPBS { get; set; }        //Parking     Speed

        public static float OwnerSFL { get; set; }        //WSS Speed FL (km/h)
        public static float OwnerSFR { get; set; }        //WSS Speed FR (km/h)
        public static float OwnerSRL { get; set; }        //WSS Speed RL (km/h)
        public static float OwnerSRR { get; set; }        //WSS Speed RR (km/h)

        public static int Owner_FL { get; set; }        //WSS Speed FL (RPM)
        public static int Owner_FR { get; set; }        //WSS Speed FR (RPM)
        public static int Owner_RL { get; set; }        //WSS Speed RL (RPM)
        public static int Owner_RR { get; set; }        //WSS Speed RR (RPM)

        public static int OwnerPdl { get; set; }        //Pedal Brake Use
        public static int OwnerCrv { get; set; }        //드라이브 커브 파일 설정

        public static float Print__X { get; set; }      //보고서 X Offset
        public static float Print__Y { get; set; }      //보고서 Y Offset
        #endregion

        #region Form Setting Variable declaration
        public struct Fom_Size
        {
            public int Top;
            public int Left;
            public int Width;
            public int Height;
        }

        public static Fom_Size siz_Main = new Fom_Size();
        public static Fom_Size siz__Sub = new Fom_Size();
        public static Fom_Size siz__Ask = new Fom_Size();
        #endregion

        #region Parameter Variable declaration
        public struct RnB_Judge
        {
            public Single SpeedMin; //속도계 min
            public Single SpeedMax; //속도계 max
            public Single SST__Min; //SST    min
            public Single SST__Max; //SST    max

            public Single PTSValue; //Pedal Brake Target Force(kg)
            public Single PTSGraph; //Pedal Brake Max    Graph(kg)

            public Single DragFMin; //전축 끌림 min
            public Single DragFMax; //전축 끌림 max
            public Single DragRMin; //후축 끌림 min
            public Single DragRMax; //후축 끌림 max

            public Single Brk_FMin; //전축 제동력 min
            public Single Brk_FMax; //전축 제동력 max
            public Single Brk_RMin; //후축 제동력 min
            public Single Brk_RMax; //후축 제동력 max

            public Single Park_Min; //주차 제동력 min
            public Single Park_Max; //주차 제동력 max

            public Single Bal_FMin; //전축 발란스 min
            public Single Bal_FMax; //전축 발란스 max
            public Single Bal_RMin; //후축 발란스 min
            public Single Bal_RMax; //후축 발란스 max
            public Single Bal_AMin; //전체 발란스 min
            public Single Bal_AMax; //전체 발란스 max

            public Single Wgt_1Min;
            public Single Wgt_1Max;
            public Single Wgt_2Min;
            public Single Wgt_2Max;
            public Single Wgt_Time;
        }
        public static RnB_Judge RnB = new RnB_Judge();

        public struct Brk_Judge
        {
            public Single Wgt_1Min; //전축  축중 최소
            public Single Wgt_1Max; //전축  축중 최대
            public Single Wgt_2Min; //후축  축중 최소
            public Single Wgt_2Max; //후축  축중 최대
            public Single Wgt_Time; //축중 측정 시간

            public Single Brk_1Std; //전축 제동력(%)
            public Single Brk_2Std; //후축 제동력(%)
            public Single Brk_Drag; //끌림 제동력(%)
            public Single Brk_Diff; //편차 제동력(%)
            public Single BrkTotal; //  합 제동력(%)
            public Single Brk_Park; //주차 제동력(%)
            public Single Brk_Time; //일반 제동력 측정 시간(sec)
            public Single DragTime; //끌림 제동력 측정 시간(sec)
            public Single ParkTime; //주차 제동력 측정 시간(sec)
        }
        public static Brk_Judge BRK = new Brk_Judge();

        public struct ECU_Judge
        {
            public Single WSSFLMin; //WSS F-L Min
            public Single WSSFLMax; //WSS F-L Max
            public Single WSSFRMin; //WSS F-R Min
            public Single WSSFRMax; //WSS F-R Max
            public Single WSSRLMin; //WSS R-L Min
            public Single WSSRLMax; //WSS R-L Max
            public Single WSSRRMin; //WSS R-R Min
            public Single WSSRRMax; //WSS R-R Max

            public Single Dec_FMin; //전축 감소(Dec) min
            public Single Dec_FMax; //전축 감소(Dec) max
            public Single Inc_FMin; //전축 증가(Inc) min
            public Single Inc_FMax; //전축 증가(Inc) max
            public Single Dec_RMin; //후축 감소(Dec) min
            public Single Dec_RMax; //후축 감소(Dec) max
            public Single Inc_RMin; //후축 증가(Inc) min
            public Single Inc_RMax; //후축 증가(Inc) max
        }
        public static ECU_Judge ECU = new ECU_Judge();
        #endregion


        #region (Loss / Load) Factor Variable declaration
        public static Loss_Cal Loss_FL = new Loss_Cal();
        public static Loss_Cal Loss_FR = new Loss_Cal();
        public static Loss_Cal Loss_RL = new Loss_Cal();
        public static Loss_Cal Loss_RR = new Loss_Cal();

        public static Load_Cal Load_FL = new Load_Cal();
        public static Load_Cal Load_FR = new Load_Cal();
        public static Load_Cal Load_RL = new Load_Cal();
        public static Load_Cal Load_RR = new Load_Cal();
        #endregion

        #region NI-DAQmx Counter Board Variable declaration
        //Channel Parameters
        public static string ENC_Z_On { get; set; }   //엔코더 Z 상 사용 여부
        public static string ENC_Type { get; set; }   //채배수
        public static string ENCPhase { get; set; }   //펄스 수신 조건
        public static string ENC_ZVal { get; set; }   //엔코더 Z 
        public static string ENCPulse { get; set; }   //엔코더 펄스 수 / 회전당
        public static string InitDist { get; set; }   //초기화 값

        //Timing Parameters
        public static string ScanRate { get; set; }   //읽기 속도
        public static string Scan_CNT { get; set; }   //읽을 갯수

        public static string ENC_FL_0 { get; set; }   //ENC FL 0 Setting
        public static string ENC_FL_1 { get; set; }   //ENC FL 1 Setting
        public static string ENC_FR_0 { get; set; }   //ENC FR 0 Setting
        public static string ENC_FR_1 { get; set; }   //ENC FR 1 Setting
        public static string ENC_RL_0 { get; set; }   //ENC RL 0 Setting
        public static string ENC_RL_1 { get; set; }   //ENC RL 1 Setting
        public static string ENC_RR_0 { get; set; }   //ENC RR 0 Setting
        public static string ENC_RR_1 { get; set; }   //ENC RR 1 Setting
        #endregion

        #region 언어팩
        public static int Def_Lang = -1;
        public static string[] Language = new string[3];
        public static string[] Lang_SST = new string[4];
        public static string[] LangDist = new string[20];
        public static string[] Lang_Key = new string[3];
        public static string[] LangLoad = new string[32];
        public static string[] LangLoss = new string[34];
        public static string[] LangMain = new string[55];
        public static string[] LangPsWd = new string[7];
        public static string[] LangRslt = new string[27];
        public static string[] LangStop = new string[23];
        public static string[] LangGage = new string[10];
        public static string[] Lang_Crv = new string[19];
        public static string[] Lang_Set = new string[183];
        #endregion

        #region Program Setting Read/Write
        public static bool Prog_SetMake()
        {
            bool Ret_Flag = true;

            cls_INI Prog_Set = new cls_INI(Sett_Def);
            try
            {
                Prog_Set.SetIniValue("Machine", "Password", Passwd.ToString());

                Prog_Set.SetIniValue("Machine", "PLC Setting", PLC__S.ToString());
                Prog_Set.SetIniValue("Machine", "PLC Port", PLC__P.ToString());
                Prog_Set.SetIniValue("Machine", "Control Setting", Ctrl_S.ToString());
                Prog_Set.SetIniValue("Machine", "Control Port", Ctrl_P.ToString());
                Prog_Set.SetIniValue("Machine", "Indicator Setting", Indi_S.ToString());
                Prog_Set.SetIniValue("Machine", "Indicator Port", Indi_P.ToString());
                Prog_Set.SetIniValue("Machine", "Drive Setting", MDrv_S.ToString());
                Prog_Set.SetIniValue("Machine", "Drive Port", MDrv_P.ToString());
                Prog_Set.SetIniValue("Machine", "Barcode1 Setting", BarC1S.ToString());
                Prog_Set.SetIniValue("Machine", "Barcode1 Port", BarC1P.ToString());
                Prog_Set.SetIniValue("Machine", "Pedal Brake Setting", PedalS.ToString());
                Prog_Set.SetIniValue("Machine", "Pedal Brake Port", PedalP.ToString());
                Prog_Set.SetIniValue("Machine", "Calibration Cycle", CalCyc.ToString());
                Prog_Set.SetIniValue("Machine", "Print Mode", sPrint.ToString());
                Prog_Set.SetIniValue("Machine", "Speach", KISpeach.ToString());
                
                Prog_Set.SetIniValue("Wheelbase", "Min. Limit", WB_Min.ToString());
                Prog_Set.SetIniValue("Wheelbase", "Max. Limit", WB_Max.ToString());
                                
                Prog_Set.SetIniValue("Roll", "Diameter FL", RFLDia.ToString());
                Prog_Set.SetIniValue("Roll", "Pulse FL", RFLPul.ToString());
                Prog_Set.SetIniValue("Roll", "Diameter FR", RFRDia.ToString());
                Prog_Set.SetIniValue("Roll", "Pulse FR", RFRPul.ToString());
                Prog_Set.SetIniValue("Roll", "Diameter RL", RRLDia.ToString());
                Prog_Set.SetIniValue("Roll", "Pulse RL", RRLPul.ToString());
                Prog_Set.SetIniValue("Roll", "Diameter RR", RRRDia.ToString());
                Prog_Set.SetIniValue("Roll", "Pulse RR", RRRPul.ToString());

                Prog_Set.SetIniValue("Directory", "Sett", Dir_Sett);
                Prog_Set.SetIniValue("Directory", "DB", Dir_DB);
                Prog_Set.SetIniValue("Directory", "Log", Dir_Log);
                Prog_Set.SetIniValue("Directory", "Data", Dir_Data);
                Prog_Set.SetIniValue("Directory", "Image", Dir_Img);
                Prog_Set.SetIniValue("Directory", "Curve", Dir_Crv);

                Prog_Set.SetIniValue("Unit", "Speed", USpeed.ToString());
                Prog_Set.SetIniValue("Unit", "Brake", UBrake.ToString());
                Prog_Set.SetIniValue("Unit", "Distance", U_Dist.ToString());

                Prog_Set.SetIniValue("Moment", "F-L", FL_Moment.ToString());
                Prog_Set.SetIniValue("Moment", "F-R", FR_Moment.ToString());
                Prog_Set.SetIniValue("Moment", "R-L", RL_Moment.ToString());
                Prog_Set.SetIniValue("Moment", "R-R", RR_Moment.ToString());

                Prog_Set.SetIniValue("Ratio", "F-L", FL_MRatio.ToString());
                Prog_Set.SetIniValue("Ratio", "F-R", FR_MRatio.ToString());
                Prog_Set.SetIniValue("Ratio", "R-L", RL_MRatio.ToString());
                Prog_Set.SetIniValue("Ratio", "R-R", RR_MRatio.ToString());

                Prog_Set.SetIniValue("Onwer Setting", "Onwer 00", OwnerS00.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Onwer 01", OwnerS01.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Onwer 02", OwnerS02.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Onwer 03", OwnerS03.ToString()); //RED(시리얼번호) 사용 설정
                Prog_Set.SetIniValue("Onwer Setting", "Onwer 04", OwnerS04.ToString()); //RED(시리얼번호)

                Prog_Set.SetIniValue("Onwer Setting", "Onwer 05", OwnerS05.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Onwer 06", OwnerS06.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Onwer 07", OwnerS07.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Onwer 08", OwnerS08.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Onwer 09", OwnerS09.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Onwer 0A", OwnerS0A.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Onwer 0B", OwnerS0B.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Onwer 0C", OwnerS0C.ToString());

                Prog_Set.SetIniValue("Onwer Setting", "SST_Type", SST_Type.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Brk_Type", Brk_Type.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Use_Door", Use_Door.ToString());
                
                Prog_Set.SetIniValue("Onwer Setting", "OnwerDrv", OwnerDrv.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "OwnerSpd", OwnerSpd.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "OwnerToq", OwnerToq.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "OwnerPBS", OwnerPBS.ToString());

                Prog_Set.SetIniValue("Onwer Setting", "Owner STD FL", OwnerSFL.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Owner STD FR", OwnerSFR.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Owner STD RL", OwnerSRL.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Owner STD RR", OwnerSRR.ToString());

                Prog_Set.SetIniValue("Onwer Setting", "Owner FL", Owner_FL.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Owner FR", Owner_FR.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Owner RL", Owner_RL.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Owner RR", Owner_RR.ToString());

                Prog_Set.SetIniValue("Onwer Setting", "Print__X", Print__X.ToString());
                Prog_Set.SetIniValue("Onwer Setting", "Print__Y", Print__Y.ToString());

                Prog_Set.SetIniValue("PLC Setting", "PLC_GapT", PLC_GapT.ToString());
                Prog_Set.SetIniValue("PLC Setting", "CNT_Stop", CNT_Stop.ToString());

                Prog_Set.SetIniValue("Weight Setting", "WGT_Capa", WGT_Capa.ToString());
                Prog_Set.SetIniValue("Weight Setting", "WGTLimit", WGTLimit.ToString());
                Prog_Set.SetIniValue("Weight Setting", "WGT_Safe", WGT_Safe.ToString());

                Prog_Set.SetIniValue("Brake Setting", "BRK_Capa", BRK_Capa.ToString());
                Prog_Set.SetIniValue("Brake Setting", "BrkRatio", BrkRatio.ToString());
                Prog_Set.SetIniValue("Brake Setting", "BRKCount", BRKCount.ToString());

                Prog_Set.SetIniValue("Brake Setting", "BRKPedal", OwnerPdl.ToString());
                Prog_Set.SetIniValue("Brake Setting", "OwnerCrv", OwnerCrv.ToString());

                Ret_Flag = true;
            }
            catch (Exception ex)
            {
                Ret_Flag = false;
            }

            return Ret_Flag;
        }
        public static bool Prog_CalMake()
        {
            bool Ret_Flag = true;

            cls_INI Prog_Set = new cls_INI(Sett_Def);
            try
            {
                Prog_Set.SetIniValue("AD Signal", "CH0Zero", CH0Zero.ToString());
                Prog_Set.SetIniValue("AD Signal", "CH1Zero", CH1Zero.ToString());
                Prog_Set.SetIniValue("AD Signal", "CH2Zero", CH2Zero.ToString());
                Prog_Set.SetIniValue("AD Signal", "CH3Zero", CH3Zero.ToString());
                Prog_Set.SetIniValue("AD Signal", "CH4Zero", CH4Zero.ToString());
                Prog_Set.SetIniValue("AD Signal", "CH5Zero", CH5Zero.ToString());

                Prog_Set.SetIniValue("AD Signal", "CH0Span", CH0Span.ToString());
                Prog_Set.SetIniValue("AD Signal", "CH1Span", CH1Span.ToString());
                Prog_Set.SetIniValue("AD Signal", "CH2Span", CH2Span.ToString());
                Prog_Set.SetIniValue("AD Signal", "CH3Span", CH3Span.ToString());
                Prog_Set.SetIniValue("AD Signal", "CH4Span", CH4Span.ToString());
                Prog_Set.SetIniValue("AD Signal", "CH5Span", CH5Span.ToString());

                Prog_Set.SetIniValue("Filter", "Average", Av_Filt.ToString());
                Prog_Set.SetIniValue("Filter", "Sort", St_Filt.ToString());
                Prog_Set.SetIniValue("Filter", "Mode", Filter.ToString());

                Ret_Flag = true;
            }
            catch (Exception ex)
            {
                Ret_Flag = false;
            }

            return Ret_Flag;
        }
        public static bool DistanceMake()
        {
            bool Ret_Flag = true;

            cls_INI Prog_Set = new cls_INI(Sett_Def);
            try
            {
                Prog_Set.SetIniValue("Distance", "Set A", Ret_Distance(Lent_A).ToString());
                Prog_Set.SetIniValue("Distance", "Set B", Ret_Distance(Lent_B).ToString());
                Prog_Set.SetIniValue("Distance", "Set C", Ret_Distance(Lent_C).ToString());
                Prog_Set.SetIniValue("Distance", "Set D", Ret_Distance(Lent_D).ToString());
                Prog_Set.SetIniValue("Distance", "Set E", Ret_Distance(Lent_E).ToString());
                Prog_Set.SetIniValue("Distance", "Set F", Ret_Distance(Lent_F).ToString());
                Prog_Set.SetIniValue("Distance", "Set G", Ret_Distance(Lent_G).ToString());
                Prog_Set.SetIniValue("Distance", "Set H", Ret_Distance(Lent_H).ToString());
                Prog_Set.SetIniValue("Distance", "Set I", Ret_Distance(Lent_I).ToString());
                Prog_Set.SetIniValue("Distance", "Set J", Ret_Distance(Lent_J).ToString());
                Prog_Set.SetIniValue("Wheelbase", "Off set", WB_Ofs.ToString());

                Ret_Flag = true;
            }
            catch (Exception ex)
            {
                Ret_Flag = false;
            }

            return Ret_Flag;
        }
        public static bool Prog_SetRead()
        {
            bool Ret_Flag = true;
            if (File.Exists(Sett_Def))
            {
                cls_INI Prog_Set = new cls_INI(Sett_Def);
                try
                {
                    Passwd = Prog_Set.GetIniString("Machine", "Password");

                    PLC__S = Prog_Set.GetIniString("Machine", "PLC Setting");
                    PLC__P = Prog_Set.GetIniNumber("Machine", "PLC Port");
                    Ctrl_S = Prog_Set.GetIniString("Machine", "Control Setting");
                    Ctrl_P = Prog_Set.GetIniNumber("Machine", "Control Port");
                    Indi_S = Prog_Set.GetIniString("Machine", "Indicator Setting");
                    Indi_P = Prog_Set.GetIniNumber("Machine", "Indicator Port");
                    MDrv_S = Prog_Set.GetIniString("Machine", "Drive Setting");
                    MDrv_P = Prog_Set.GetIniNumber("Machine", "Drive Port");
                    BarC1S = Prog_Set.GetIniString("Machine", "Barcode1 Setting");
                    BarC1P = Prog_Set.GetIniNumber("Machine", "Barcode1 Port");
                    PedalS = Prog_Set.GetIniString("Machine", "Pedal Brake Setting");
                    PedalP = Prog_Set.GetIniNumber("Machine", "Pedal Brake Port");
                    CalCyc = Prog_Set.GetIniNumber("Machine", "Calibration Cycle");
                    sPrint = Prog_Set.GetIniNumber("Machine", "Print Mode");
                    KISpeach = Prog_Set.GetIniNumber("Machine", "Speach");

                    WB_Min = Prog_Set.GetIniNumber("Wheelbase", "Min. Limit");
                    WB_Max = Prog_Set.GetIniNumber("Wheelbase", "Max. Limit");
                    WB_Ofs = Prog_Set.GetIniNumber("Wheelbase", "Off set");

                    RFLDia = Prog_Set.GetIniNumber("Roll", "Diameter FL");
                    RFLPul = Prog_Set.GetIniNumber("Roll", "Pulse FL");
                    RFRDia = Prog_Set.GetIniNumber("Roll", "Diameter FR");
                    RFRPul = Prog_Set.GetIniNumber("Roll", "Pulse FR");
                    RRLDia = Prog_Set.GetIniNumber("Roll", "Diameter RL");
                    RRLPul = Prog_Set.GetIniNumber("Roll", "Pulse RL");
                    RRRDia = Prog_Set.GetIniNumber("Roll", "Diameter RR");
                    RRRPul = Prog_Set.GetIniNumber("Roll", "Pulse RR");

                  Dir_Sett = Prog_Set.GetIniString("Directory", "Sett");
                    Dir_DB = Prog_Set.GetIniString("Directory", "DB");
                   Dir_Log = Prog_Set.GetIniString("Directory", "Log");
                  Dir_Data = Prog_Set.GetIniString("Directory", "Data");
                   Dir_Img = Prog_Set.GetIniString("Directory", "Image");
                   Dir_Crv = Prog_Set.GetIniString("Directory", "Curve");

                   CH0Zero = Prog_Set.GetIniNumber("AD Signal", "CH0Zero");
                   CH1Zero = Prog_Set.GetIniNumber("AD Signal", "CH1Zero");
                   CH2Zero = Prog_Set.GetIniNumber("AD Signal", "CH2Zero");
                   CH3Zero = Prog_Set.GetIniNumber("AD Signal", "CH3Zero");
                   CH4Zero = Prog_Set.GetIniNumber("AD Signal", "CH4Zero");
                   CH5Zero = Prog_Set.GetIniNumber("AD Signal", "CH5Zero");

                   CH0Span = Prog_Set.GetIni_Float("AD Signal", "CH0Span");
                   CH1Span = Prog_Set.GetIni_Float("AD Signal", "CH1Span");
                   CH2Span = Prog_Set.GetIni_Float("AD Signal", "CH2Span");
                   CH3Span = Prog_Set.GetIni_Float("AD Signal", "CH3Span");
                   CH4Span = Prog_Set.GetIni_Float("AD Signal", "CH4Span");
                   CH5Span = Prog_Set.GetIni_Float("AD Signal", "CH5Span");

                   Av_Filt = Prog_Set.GetIniNumber("Filter", "Average");
                   St_Filt = Prog_Set.GetIniNumber("Filter", "Sort");
                   Filter = Prog_Set.GetIniNumber("Filter", "Mode");

                   USpeed = Prog_Set.GetIniNumber("Unit", "Speed");
                   UBrake = Prog_Set.GetIniNumber("Unit", "Brake");
                   U_Dist = Prog_Set.GetIniNumber("Unit", "Distance");

                   Lent_A = Ret_Distance(Prog_Set.GetIniNumber("Distance", "Set A"));
                   Lent_B = Ret_Distance(Prog_Set.GetIniNumber("Distance", "Set B"));
                   Lent_C = Ret_Distance(Prog_Set.GetIniNumber("Distance", "Set C"));
                   Lent_D = Ret_Distance(Prog_Set.GetIniNumber("Distance", "Set D"));
                   Lent_E = Ret_Distance(Prog_Set.GetIniNumber("Distance", "Set E"));
                   Lent_F = Ret_Distance(Prog_Set.GetIniNumber("Distance", "Set F"));
                   Lent_G = Ret_Distance(Prog_Set.GetIniNumber("Distance", "Set G"));
                   Lent_H = Ret_Distance(Prog_Set.GetIniNumber("Distance", "Set H"));
                   Lent_I = Ret_Distance(Prog_Set.GetIniNumber("Distance", "Set I"));
                   Lent_J = Ret_Distance(Prog_Set.GetIniNumber("Distance", "Set J"));

                   FL_Moment = Prog_Set.GetIniDouble("Moment", "F-L");
                   FR_Moment = Prog_Set.GetIniDouble("Moment", "F-R");
                   RL_Moment = Prog_Set.GetIniDouble("Moment", "R-L");
                   RR_Moment = Prog_Set.GetIniDouble("Moment", "R-R");
                   
                   FL_MRatio = Prog_Set.GetIniDouble("Ratio", "F-L");
                   FR_MRatio = Prog_Set.GetIniDouble("Ratio", "F-R");
                   RL_MRatio = Prog_Set.GetIniDouble("Ratio", "R-L");
                   RR_MRatio = Prog_Set.GetIniDouble("Ratio", "R-R");

                   OwnerS00 = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 00");
                   OwnerS01 = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 01");
                   OwnerS02 = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 02");
                   OwnerS03 = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 03");   //RED(시리얼번호) 사용 설정
                   OwnerS04 = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 04");   //RED(시리얼번호)

                   OwnerS05 = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 05");
                   OwnerS06 = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 06");
                   OwnerS07 = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 07");
                   OwnerS08 = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 08");
                   OwnerS09 = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 09");
                   OwnerS0A = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 0A");
                   OwnerS0B = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 0B");
                   OwnerS0C = Prog_Set.GetIniNumber("Onwer Setting", "Onwer 0C");

                   SST_Type = Prog_Set.GetIniNumber("Onwer Setting", "SST_Type");
                   Brk_Type = Prog_Set.GetIniNumber("Onwer Setting", "Brk_Type");
                   Use_Door = Prog_Set.GetIniNumber("Onwer Setting", "Use_Door");
                   
                   OwnerDrv = Prog_Set.GetIniNumber("Onwer Setting", "OnwerDrv");
                   OwnerSpd = Prog_Set.GetIniNumber("Onwer Setting", "OwnerSpd");
                   OwnerToq = Prog_Set.GetIniNumber("Onwer Setting", "OwnerToq");
                   OwnerPBS = Prog_Set.GetIniNumber("Onwer Setting", "OwnerPBS");

                   OwnerSFL = Prog_Set.GetIni_Float("Onwer Setting", "Owner STD FL");
                   OwnerSFR = Prog_Set.GetIni_Float("Onwer Setting", "Owner STD FR");
                   OwnerSRL = Prog_Set.GetIni_Float("Onwer Setting", "Owner STD RL");
                   OwnerSRR = Prog_Set.GetIni_Float("Onwer Setting", "Owner STD RR");

                   Owner_FL = Prog_Set.GetIniNumber("Onwer Setting", "Owner FL");
                   Owner_FR = Prog_Set.GetIniNumber("Onwer Setting", "Owner FR");
                   Owner_RL = Prog_Set.GetIniNumber("Onwer Setting", "Owner RL");
                   Owner_RR = Prog_Set.GetIniNumber("Onwer Setting", "Owner RR");

                   Print__X = Prog_Set.GetIni_Float("Onwer Setting", "Print__X");
                   Print__Y = Prog_Set.GetIni_Float("Onwer Setting", "Print__Y");

                   PLC_GapT = Prog_Set.GetIniNumber("PLC Setting", "PLC_GapT");
                   CNT_Stop = Prog_Set.GetIniNumber("PLC Setting", "CNT_Stop");

                   WGT_Capa = Prog_Set.GetIniNumber("Weight Setting", "WGT_Capa");
                   WGTLimit = Prog_Set.GetIniNumber("Weight Setting", "WGTLimit");
                   WGT_Safe = Prog_Set.GetIniNumber("Weight Setting", "WGT_Safe");

                   BRK_Capa = Prog_Set.GetIniNumber("Brake Setting", "BRK_Capa");
                   BrkRatio = Prog_Set.GetIni_Float("Brake Setting", "BrkRatio");
                   BRKCount = Prog_Set.GetIniNumber("Brake Setting", "BRKCount");

                   OwnerPdl = Prog_Set.GetIniNumber("Brake Setting", "BRKPedal");
                   OwnerCrv = Prog_Set.GetIniNumber("Brake Setting", "OwnerCrv");
                   
                   if (WGT_Capa < 1000) { WGT_Capa = 1000; }
                   if (WGTLimit < 100) { WGTLimit = 100; }
                   if (WGT_Safe < 5) { WGT_Safe = 5; }

                   if (BRK_Capa < 1000) { BRK_Capa = 1000; }
                   if (BrkRatio < 1) { BrkRatio = 1; }
                   if (BRKCount < 1) { BRKCount = 1; }

                   if (PLC_GapT < 10) { PLC_GapT = 10; }
                   if (PLC_GapT > 1000) { PLC_GapT = 1000; }

                   if (CNT_Stop < 0)  { CNT_Stop = 1; }
                   if (CNT_Stop > 10) { CNT_Stop = 10; }
 
                   if (FL_Moment == 0) FL_Moment = 55909249.02;
                   if (FR_Moment == 0) FR_Moment = 55909249.02;
                   if (RL_Moment == 0) RL_Moment = 55909249.02;
                   if (RR_Moment == 0) RR_Moment = 55909249.02;

                   if (FL_MRatio == 0) FL_MRatio = 6.666666666;
                   if (FR_MRatio == 0) FR_MRatio = 6.666666666;
                   if (RL_MRatio == 0) RL_MRatio = 6.666666666;
                   if (RR_MRatio == 0) RR_MRatio = 6.666666666;

                   if (CH0Span == 0) CH0Span = 1;
                   if (CH1Span == 0) CH1Span = 1;
                   if (CH2Span == 0) CH2Span = 1;
                   if (CH3Span == 0) CH3Span = 1;
                   if (CH4Span == 0) CH4Span = 1;
                   if (CH5Span == 0) CH5Span = 1;

                    Ret_Flag = true;
                }
                catch (Exception ex)
                {
                    Ret_Flag = false;
                }
            }
            else
            {
                Ret_Flag = false;
            }
            return Ret_Flag;
        }

        public static bool Test_CNTMake(string OkNg)
        {
            bool Ret_Flag = true;

            cls_INI Prog_Set = new cls_INI(Sett_Def);
            try
            {
                if (OkNg == "Init")
                {
                    T__CNT = 0; T_Pass = 0; T_Fail = 0;
                }
                else
                {
                    T__CNT++;
                    if (OkNg == "Pass") { T_Pass++; } else { T_Fail++; }
                }

                Prog_Set.SetIniValue("Machine", "Test Count", T__CNT.ToString());
                Prog_Set.SetIniValue("Machine", "Test Fail", T_Fail.ToString());
                Prog_Set.SetIniValue("Machine", "Test Pass", T_Pass.ToString());

                Ret_Flag = true;
            }
            catch (Exception ex)
            {
                Ret_Flag = false;
            }

            return Ret_Flag;
        }
        public static bool Test_CNTRead()
        {
            bool Ret_Flag = true;
            if (File.Exists(Sett_Def))
            {
                cls_INI Prog_Set = new cls_INI(Sett_Def);
                try
                {
                    T__CNT = Prog_Set.GetIniNumber("Machine", "Test Count");
                    T_Fail = Prog_Set.GetIniNumber("Machine", "Test Fail");
                    T_Pass = Prog_Set.GetIniNumber("Machine", "Test Pass");

                    Ret_Flag = true;
                }
                catch (Exception ex)
                {
                    Ret_Flag = false;
                }
            }
            else
            {
                Ret_Flag = false;
            }
            return Ret_Flag;
        }

        public static int Ret_Distance(int pDist)
        {
            if (pDist < WB_Min)
            {
                return WB_Min;
            }
            else
            {
                if (pDist > WB_Max)
                {
                    return WB_Max;
                }
                else
                {
                    return pDist;
                }
            }
        }
        #endregion

        #region Form Setting Read/Write
        public static void Ini_SizeMake()
        {
            cls_INI siz_File = new cls_INI(Size_Def);
            
            siz_File.SetIniValue("Main Form", "Top", siz_Main.Top.ToString());
            siz_File.SetIniValue("Main Form", "Left", siz_Main.Left.ToString());
            siz_File.SetIniValue("Main Form", "Width", siz_Main.Width.ToString());
            siz_File.SetIniValue("Main Form", "Height", siz_Main.Height.ToString());

            siz_File.SetIniValue("Sub Form", "Top", siz__Sub.Top.ToString());
            siz_File.SetIniValue("Sub Form", "Left", siz__Sub.Left.ToString());
            siz_File.SetIniValue("Sub Form", "Width", siz__Sub.Width.ToString());
            siz_File.SetIniValue("Sub Form", "Height", siz__Sub.Height.ToString());

            siz_File.SetIniValue("Ask Form", "Top", siz__Ask.Top.ToString());
            siz_File.SetIniValue("Ask Form", "Left", siz__Ask.Left.ToString());
            siz_File.SetIniValue("Ask Form", "Width", siz__Ask.Width.ToString());
            siz_File.SetIniValue("Ask Form", "Height", siz__Ask.Height.ToString());
        }
        public static bool Ini_SizeRead()
        {
            bool Ret_Flag = true;

            if (File.Exists(Size_Def))
            {
                cls_INI siz_File = new cls_INI(Size_Def);
                try
                {
                    siz_Main.Top = siz_File.GetIniNumber("Main Form", "Top");
                    siz_Main.Left = siz_File.GetIniNumber("Main Form", "Left");
                    siz_Main.Width = siz_File.GetIniNumber("Main Form", "Width");
                    siz_Main.Height = siz_File.GetIniNumber("Main Form", "Height");

                    siz__Sub.Top = siz_File.GetIniNumber("Sub Form", "Top");
                    siz__Sub.Left = siz_File.GetIniNumber("Sub Form", "Left");
                    siz__Sub.Width = siz_File.GetIniNumber("Sub Form", "Width");
                    siz__Sub.Height = siz_File.GetIniNumber("Sub Form", "Height");

                    siz__Ask.Top = siz_File.GetIniNumber("Ask Form", "Top");
                    siz__Ask.Left = siz_File.GetIniNumber("Ask Form", "Left");
                    siz__Ask.Width = siz_File.GetIniNumber("Ask Form", "Width");
                    siz__Ask.Height = siz_File.GetIniNumber("Ask Form", "Height");
                    
                    Ret_Flag = true;
                }
                catch (Exception ex)
                {
                    Ret_Flag = false;
                }
            }
            else
            {
                Ret_Flag = false;
            }
            return Ret_Flag;
        }
        #endregion
        
        #region Roll Load Factor Read/Write
        public static void Cal_LoadMake()
        {
            if (File.Exists(LoadFDef))
            {
                File.Delete(LoadFDef);
            }

            LoadDataMake("FL Load", Load_FL);
            LoadDataMake("FR Load", Load_FR);
            LoadDataMake("RL Load", Load_RL);
            LoadDataMake("RR Load", Load_RR);
        }
        private static void LoadDataMake(string Kind, Load_Cal rload)
        {
            cls_INI Load = new cls_INI(LoadFDef);

            //for (int cnt = 0; cnt < rload.List.Count; cnt++)
            for (int cnt = 0; cnt < rload.Item.Length; cnt++)
            {
                Load.SetIniValue(Kind, "Stt" + cnt.ToString() + "Sped", rload.Item[cnt].SpdS.ToString());
                Load.SetIniValue(Kind, "End" + cnt.ToString() + "Sped", rload.Item[cnt].SpdE.ToString());
                Load.SetIniValue(Kind, "Stt" + cnt.ToString() + "RPM ", rload.Item[cnt].RpmS.ToString());
                Load.SetIniValue(Kind, "End" + cnt.ToString() + "RPM ", rload.Item[cnt].RpmE.ToString());
                Load.SetIniValue(Kind, "Chk" + cnt.ToString() + "Time", rload.Item[cnt].Time.ToString());
                Load.SetIniValue(Kind, "Chk" + cnt.ToString() + "Devi", rload.Item[cnt].Devi.ToString());
                Load.SetIniValue(Kind, "Chk" + cnt.ToString() + "CalD", rload.Item[cnt].CalD.ToString());
                Load.SetIniValue(Kind, "Chk" + cnt.ToString() + "Indi", rload.Item[cnt].Indi.ToString());
                Load.SetIniValue(Kind, "Chk" + cnt.ToString() + "Calc", rload.Item[cnt].Calc.ToString());
            }
        }
        public static bool Cal_LoadRead()
        {
            bool Ret_Flag = true;

            if (File.Exists(LoadFDef))
            {
                Load_FL = LoadDataRead("FL Load", Load_FL);
                Load_FR = LoadDataRead("FR Load", Load_FR);
                Load_RL = LoadDataRead("RL Load", Load_RL);
                Load_RR = LoadDataRead("RR Load", Load_RR);

                Ret_Flag = true;
            }
            else
            {
                Ret_Flag = false;
            }
            return Ret_Flag;
        }
        private static Load_Cal LoadDataRead(string Kind, Load_Cal rload)
        {
            cls_INI Load = new cls_INI(LoadFDef);
            try
            {
                int count = 0;
                double spdS = 0;
                double spdE = 0;
                double rpmS = 0;
                double rpmE = 0;
                double time = 0;
                double Devi = 0;
                double CalD = 0;
                double Indi = 0;
                double Calc = 0;

                for (int cnt = 0; cnt < rload.Item.Length; cnt++)
                {
                    rload.Item[cnt].SpdS = Load.GetIniDouble(Kind, "Stt" + cnt.ToString() + "Sped");
                    rload.Item[cnt].SpdE = Load.GetIniDouble(Kind, "End" + cnt.ToString() + "Sped");
                    rload.Item[cnt].RpmS = Load.GetIniDouble(Kind, "Stt" + cnt.ToString() + "RPM ");
                    rload.Item[cnt].RpmE = Load.GetIniDouble(Kind, "End" + cnt.ToString() + "RPM ");
                    rload.Item[cnt].Time = Load.GetIniDouble(Kind, "Chk" + cnt.ToString() + "Time");
                    rload.Item[cnt].Devi = Load.GetIniDouble(Kind, "Chk" + cnt.ToString() + "Devi");
                    rload.Item[cnt].CalD = Load.GetIniDouble(Kind, "Chk" + cnt.ToString() + "CalD");
                    rload.Item[cnt].Indi = Load.GetIniDouble(Kind, "Chk" + cnt.ToString() + "Indi");
                    rload.Item[cnt].Calc = Load.GetIniDouble(Kind, "Chk" + cnt.ToString() + "Calc");

                    spdS += rload.Item[cnt].SpdS;
                    spdE += rload.Item[cnt].SpdE;
                    rpmS += rload.Item[cnt].RpmS;
                    rpmE += rload.Item[cnt].RpmE;
                    time += rload.Item[cnt].Time;
                    Devi += rload.Item[cnt].Devi;
                    CalD += rload.Item[cnt].CalD;
                    Indi += rload.Item[cnt].Indi;
                    Calc += rload.Item[cnt].Calc;

                    if (rload.Item[cnt].SpdS > 0) { count++; }
                }

                if (count > 0)
                {
                    rload.Aver.SpdS = spdS / count;
                    rload.Aver.SpdE = spdE / count;
                    rload.Aver.RpmS = rpmS / count;
                    rload.Aver.RpmE = rpmE / count;
                    rload.Aver.Time = time / count;
                    rload.Aver.Devi = Devi / count;
                    rload.Aver.CalD = CalD / count;
                    rload.Aver.Indi = Indi / count;
                    rload.Aver.Calc = Calc / count;
                }
                else
                {
                    rload.Aver.SpdS = 0;
                    rload.Aver.SpdE = 0;
                    rload.Aver.RpmS = 0;
                    rload.Aver.RpmE = 0;
                    rload.Aver.Time = 0;
                    rload.Aver.Devi = 0;
                    rload.Aver.CalD = 0;
                    rload.Aver.Indi = 0;
                    rload.Aver.Calc = 0;
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "LoadDataRead: " + ex.Message);
            }

            return rload;
        }
        #endregion

        #region Roll Loss Factor Read/Write
        public static void Cal_LossMake()
        {
            if (File.Exists(LossFDef))
            {
                File.Delete(LossFDef);
            }

            LossDataMake("FL Loss", Loss_FL);
            LossDataMake("FR Loss", Loss_FR);
            LossDataMake("RL Loss", Loss_RL);
            LossDataMake("RR Loss", Loss_RR);
        }
        private static void LossDataMake(string Kind, Loss_Cal rloss)
        {
            cls_INI Loss = new cls_INI(LossFDef);

            for (int cnt = 0; cnt < rloss.Item.Length; cnt++)
            {
                Loss.SetIniValue(Kind, "Stt" + cnt.ToString() + "Sped", rloss.Item[cnt].SpdS.ToString());
                Loss.SetIniValue(Kind, "End" + cnt.ToString() + "Sped", rloss.Item[cnt].SpdE.ToString());
                Loss.SetIniValue(Kind, "Stt" + cnt.ToString() + "RPM", rloss.Item[cnt].SpdS.ToString());
                Loss.SetIniValue(Kind, "End" + cnt.ToString() + "RPM", rloss.Item[cnt].SpdE.ToString());
                Loss.SetIniValue(Kind, "Chk" + cnt.ToString() + "Time", rloss.Item[cnt].Time.ToString());
                Loss.SetIniValue(Kind, "Chk" + cnt.ToString() + "___M", rloss.Item[cnt].ChkM.ToString());
                Loss.SetIniValue(Kind, "Chk" + cnt.ToString() + "___B", rloss.Item[cnt].ChkB.ToString());
                Loss.SetIniValue(Kind, "Chk" + cnt.ToString() + "Loss", rloss.Item[cnt].Loss.ToString());
            }
        }
        public static bool Cal_LossRead()
        {
            bool Ret_Flag = true;

            if (File.Exists(LossFDef))
            {
                Loss_FL = LossDataRead("FL Loss", Loss_FL);
                Loss_FR = LossDataRead("FR Loss", Loss_FR);
                Loss_RL = LossDataRead("RL Loss", Loss_RL);
                Loss_RR = LossDataRead("RR Loss", Loss_RR);

                Ret_Flag = true;
            }
            else
            {
                Ret_Flag = false;
            }
            return Ret_Flag;
        }
        private static Loss_Cal LossDataRead(string Kind, Loss_Cal rloss)
        {
            cls_INI Loss = new cls_INI(LossFDef);
            try
            {
                int count = 0;
                double spdS = 0;
                double spdE = 0;
                double rpmS = 0;
                double rpmE = 0;
                double time = 0;
                double chkM = 0;
                double chkB = 0;
                double loss = 0;

                for (int cnt = 0; cnt < 3; cnt++)
                {
                    rloss.Item[cnt].SpdS = Loss.GetIniDouble(Kind, "Stt" + cnt.ToString() + "Sped");
                    rloss.Item[cnt].SpdE = Loss.GetIniDouble(Kind, "End" + cnt.ToString() + "Sped");
                    rloss.Item[cnt].RpmS = Loss.GetIniDouble(Kind, "Stt" + cnt.ToString() + "RPM");
                    rloss.Item[cnt].RpmE = Loss.GetIniDouble(Kind, "End" + cnt.ToString() + "RPM");
                    rloss.Item[cnt].Time = Loss.GetIniDouble(Kind, "Chk" + cnt.ToString() + "Time");
                    rloss.Item[cnt].ChkM = Loss.GetIniDouble(Kind, "Chk" + cnt.ToString() + "___M");
                    rloss.Item[cnt].ChkB = Loss.GetIniDouble(Kind, "Chk" + cnt.ToString() + "___B");
                    rloss.Item[cnt].Loss = Loss.GetIniDouble(Kind, "Chk" + cnt.ToString() + "Loss");

                    spdS += rloss.Item[cnt].SpdS;
                    spdE += rloss.Item[cnt].SpdE;
                    rpmS += rloss.Item[cnt].RpmS;
                    rpmE += rloss.Item[cnt].RpmE;
                    time += rloss.Item[cnt].Time;
                    chkM += rloss.Item[cnt].ChkM;
                    chkB += rloss.Item[cnt].ChkB;
                    loss += rloss.Item[cnt].Loss;

                    if (rloss.Item[cnt].SpdS > 0) { count++; }
                }

                if (count > 0)
                {
                    rloss.Aver.SpdS = spdS / count;
                    rloss.Aver.SpdE = spdE / count;
                    rloss.Aver.RpmS = rpmS / count;
                    rloss.Aver.RpmE = rpmE / count;
                    rloss.Aver.Time = time / count;
                    rloss.Aver.ChkM = chkM / count;
                    rloss.Aver.ChkB = chkB / count;
                    rloss.Aver.Loss = loss / count;
                }
                else
                {
                    rloss.Aver.SpdS = 0;
                    rloss.Aver.SpdE = 0;
                    rloss.Aver.RpmS = 0;
                    rloss.Aver.RpmE = 0;
                    rloss.Aver.Time = 0;
                    rloss.Aver.ChkM = 0;
                    rloss.Aver.ChkB = 0;
                    rloss.Aver.Loss = 0;
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "LossDataRead: " + ex.Message);
            }

            return rloss;
        }
        #endregion

        #region NI-DAQmx Counter Board Setting Read/Write
        public static void NIDAQmx_Make()
        {
            string NI_DAQmx_Set = Application.StartupPath + @"\Setting\NI-DAQmx.def";
            cls_INI siz_File = new cls_INI(NI_DAQmx_Set);

            siz_File.SetIniValue("Channel Parameters", "ENC_Z_On", ENC_Z_On);
            siz_File.SetIniValue("Channel Parameters", "ENC_Type", ENC_Type);
            siz_File.SetIniValue("Channel Parameters", "ENCPhase", ENCPhase);
            siz_File.SetIniValue("Channel Parameters", "ENC_ZVal", ENC_ZVal);
            siz_File.SetIniValue("Channel Parameters", "ENCPulse", ENCPulse);
            siz_File.SetIniValue("Channel Parameters", "InitDist", InitDist);

            siz_File.SetIniValue("Timing Parameters", "ScanRate", ScanRate);
            siz_File.SetIniValue("Timing Parameters", "Scan_CNT", Scan_CNT);

            siz_File.SetIniValue("ENC Channel Setting", "FL 0", ENC_FL_0);
            siz_File.SetIniValue("ENC Channel Setting", "FL 1", ENC_FL_1);
            siz_File.SetIniValue("ENC Channel Setting", "FR 0", ENC_FR_0);
            siz_File.SetIniValue("ENC Channel Setting", "FR 1", ENC_FR_1);
            siz_File.SetIniValue("ENC Channel Setting", "RL 0", ENC_RL_0);
            siz_File.SetIniValue("ENC Channel Setting", "RL 1", ENC_RL_1);
            siz_File.SetIniValue("ENC Channel Setting", "RR 0", ENC_RR_0);
            siz_File.SetIniValue("ENC Channel Setting", "RR 1", ENC_RR_1);
        }
        public static bool NIDAQmx_Read()
        {
            string NI_DAQmx_Set = Application.StartupPath + @"\Setting\NI-DAQmx.def";
            bool Ret_Flag = true;

            if (File.Exists(NI_DAQmx_Set))
            {
                cls_INI siz_File = new cls_INI(NI_DAQmx_Set);
                try
                {
                    ENC_Z_On = siz_File.GetIniString("Channel Parameters", "ENC_Z_On");
                    ENC_Type = siz_File.GetIniString("Channel Parameters", "ENC_Type");
                    ENCPhase = siz_File.GetIniString("Channel Parameters", "ENCPhase");
                    ENC_ZVal = siz_File.GetIniString("Channel Parameters", "ENC_ZVal");
                    ENCPulse = siz_File.GetIniString("Channel Parameters", "ENCPulse");
                    InitDist = siz_File.GetIniString("Channel Parameters", "InitDist");
                    
                    ScanRate = siz_File.GetIniString("Timing Parameters", "ScanRate");
                    Scan_CNT = siz_File.GetIniString("Timing Parameters", "Scan_CNT");

                    ENC_FL_0 = siz_File.GetIniString("ENC Channel Setting", "FL 0");
                    ENC_FL_1 = siz_File.GetIniString("ENC Channel Setting", "FL 1");
                    ENC_FR_0 = siz_File.GetIniString("ENC Channel Setting", "FR 0");
                    ENC_FR_1 = siz_File.GetIniString("ENC Channel Setting", "FR 1");
                    ENC_RL_0 = siz_File.GetIniString("ENC Channel Setting", "RL 0");
                    ENC_RL_1 = siz_File.GetIniString("ENC Channel Setting", "RL 1");
                    ENC_RR_0 = siz_File.GetIniString("ENC Channel Setting", "RR 0");
                    ENC_RR_1 = siz_File.GetIniString("ENC Channel Setting", "RR 1");

                    if (ENC_FL_0 == "") { ENC_FL_0 = "/Dev1/PFI39"; }  //A:39, B:37, Z:38
                    if (ENC_FR_0 == "") { ENC_FR_0 = "/Dev1/PFI35"; }  //A:35, B:33, Z:34
                    if (ENC_RL_0 == "") { ENC_RL_0 = "/Dev1/PFI31"; }  //A:31, B:29, Z:30
                    if (ENC_RR_0 == "") { ENC_RR_0 = "/Dev1/PFI27"; }  //A:27, B:25, Z:26

                    if (ENC_FL_1 == "") { ENC_FL_1 = "Dev1/ctr0"; }
                    if (ENC_FR_1 == "") { ENC_FR_1 = "Dev1/ctr1"; }
                    if (ENC_RL_1 == "") { ENC_RL_1 = "Dev1/ctr2"; }
                    if (ENC_RR_1 == "") { ENC_RR_1 = "Dev1/ctr3"; }

                    Ret_Flag = true;
                }
                catch (Exception ex)
                {
                    Ret_Flag = false;
                }
            }
            else
            {
                Ret_Flag = false;
            }
            return Ret_Flag;
        }
        #endregion

        #region 언어팩
        public static bool Read_ExcelDB()
        {
            try
            {
                // OLEDB를 이용한 엑셀 연결
                // Excel 97-2003 .xls
                // string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\x\test.xls;Extended Properties='Excel 8.0;HDR=No'";

                // Excel 2007 이후 .xlsx
                string szConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + LangFile + ";Extended Properties='Excel 8.0;HDR=No'";

                OleDbConnection conn = new OleDbConnection(szConn);
                conn.Open();

                // 엑셀로부터 데이타 읽기
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Language$]", conn);
                OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                adpt.Fill(ds);

                int Lang = OwnerS00 + 1;
                int idx = 1;
                Language[0] = ds.Tables[0].Rows[1][0].ToString();
                Language[1] = ds.Tables[0].Rows[2][0].ToString();
                Language[2] = ds.Tables[0].Rows[3][0].ToString();

                string title = "";

                for (int cnt = 0; cnt < Lang_SST.Length; cnt++)
                {
                    title = ds.Tables[0].Rows[idx][0].ToString();
                    if (title == "SST " + cnt.ToString())
                    {
                        Lang_SST[cnt] = ds.Tables[0].Rows[idx][Lang].ToString();
                    }
                    else
                    {
                        MessageBoxEx.Show(title);
                    }
                    idx++;
                }

                for (int cnt = 0; cnt < LangDist.Length; cnt++)
                {
                    title = ds.Tables[0].Rows[idx][0].ToString();
                    if (title == "Dist " + cnt.ToString())
                    {
                        LangDist[cnt] = ds.Tables[0].Rows[idx][Lang].ToString();
                    }
                    else
                    {
                        MessageBoxEx.Show(title);
                    }
                    idx++;
                }

                for (int cnt = 0; cnt < Lang_Key.Length; cnt++)
                {
                    title = ds.Tables[0].Rows[idx][0].ToString();
                    if (title == "Keys " + cnt.ToString())
                    {
                        Lang_Key[cnt] = ds.Tables[0].Rows[idx][Lang].ToString();
                    }
                    else
                    {
                        MessageBoxEx.Show(title);
                    }
                    idx++;
                }

                for (int cnt = 0; cnt < LangLoad.Length; cnt++)
                {
                    title = ds.Tables[0].Rows[idx][0].ToString();
                    if (title == "Load " + cnt.ToString())
                    {
                        LangLoad[cnt] = ds.Tables[0].Rows[idx][Lang].ToString();
                    }
                    else
                    {
                        MessageBoxEx.Show(title);
                    }
                    idx++;
                }

                for (int cnt = 0; cnt < LangLoss.Length; cnt++)
                {
                    title = ds.Tables[0].Rows[idx][0].ToString();
                    if (title == "Loss " + cnt.ToString())
                    {
                        LangLoss[cnt] = ds.Tables[0].Rows[idx][Lang].ToString();
                    }
                    else
                    {
                        MessageBoxEx.Show(title);
                    }
                    idx++;
                }

                for (int cnt = 0; cnt < LangMain.Length; cnt++)
                {
                    title = ds.Tables[0].Rows[idx][0].ToString();
                    if (title == "Main " + cnt.ToString())
                    {
                        LangMain[cnt] = ds.Tables[0].Rows[idx][Lang].ToString();
                    }
                    else
                    {
                        MessageBoxEx.Show(title);
                    }
                    idx++;
                }

                for (int cnt = 0; cnt < LangPsWd.Length; cnt++)
                {
                    title = ds.Tables[0].Rows[idx][0].ToString();
                    if (title == "PsWd " + cnt.ToString())
                    {
                        LangPsWd[cnt] = ds.Tables[0].Rows[idx][Lang].ToString();
                    }
                    else
                    {
                        MessageBoxEx.Show(title);
                    }
                    idx++;
                }

                for (int cnt = 0; cnt < LangRslt.Length; cnt++)
                {
                    title = ds.Tables[0].Rows[idx][0].ToString();
                    if (title == "Rslt " + cnt.ToString())
                    {
                        LangRslt[cnt] = ds.Tables[0].Rows[idx][Lang].ToString();
                    }
                    else
                    {
                        MessageBoxEx.Show(title);
                    }
                    idx++;
                }

                for (int cnt = 0; cnt < LangStop.Length; cnt++)
                {
                    title = ds.Tables[0].Rows[idx][0].ToString();
                    if (title == "Stop " + cnt.ToString())
                    {
                        LangStop[cnt] = ds.Tables[0].Rows[idx][Lang].ToString();
                    }
                    else
                    {
                        MessageBoxEx.Show(title);
                    }
                    idx++;
                }

                for (int cnt = 0; cnt < LangGage.Length; cnt++)
                {
                    title = ds.Tables[0].Rows[idx][0].ToString();
                    if (title == "Gage " + cnt.ToString())
                    {
                        LangGage[cnt] = ds.Tables[0].Rows[idx][Lang].ToString();
                    }
                    else
                    {
                        MessageBoxEx.Show(title);
                    }
                    idx++;
                }

                for (int cnt = 0; cnt < Lang_Crv.Length; cnt++)
                {
                    title = ds.Tables[0].Rows[idx][0].ToString();
                    if (title == "Curve " + cnt.ToString())
                    {
                        Lang_Crv[cnt] = ds.Tables[0].Rows[idx][Lang].ToString();
                    }
                    else
                    {
                        MessageBoxEx.Show(title);
                    }
                    idx++;
                }

                for (int cnt = 0; cnt < Lang_Set.Length; cnt++)
                {
                    title = ds.Tables[0].Rows[idx][0].ToString();
                    if (title == "Setup " + cnt.ToString())
                    {
                        Lang_Set[cnt] = ds.Tables[0].Rows[idx][Lang].ToString();
                    }
                    else
                    {
                        MessageBoxEx.Show(title);
                    }
                    idx++;
                }

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                return false;
            }
        }
        #endregion

        #region Backup File
        public static bool Backup___CSV(string pFile)
        {
            MDB_Alls DB_Back = new MDB_Alls();

            string csv_File = Application.StartupPath + @"\Data\" + pFile + ".csv";
            
            bool Ret = true;

            DataTable dt = DB_Back.DB_Info.Backup(pFile);

            using (StreamWriter sw = new StreamWriter(csv_File, false, Encoding.Default))
            {
                try
                {
                    #region Head
                    string str_Head = "Work No, Vin, Model, ECU, ID, W/B, Engine, T/M, ABS, Curve, Drive, Date, ";
                    str_Head += "Speed (km/h), Speed Judge, Max speed (km/h), ";
                    str_Head += "Drag F-L (kg), Drag F-R (kg), Drag R-L (kg), Drag R-R (kg), ";
                    str_Head += "Brake F-L (kg), Brake F-R (kg), Brake R-L (kg), Brake R-R (kg), ";
                    str_Head += "Parking R-L (cm), Parking R-R (cm), ";
                    str_Head += "Balance F-L (kg), Balance F-R (kg), Front Balance (%), Front Balance Judge, ";
                    str_Head += "Balance R-L (kg), Balance R-R (kg), Rear Balance (%), Rear Balance Judge, ";
                    str_Head += "Balance Front-Rear (%), Balance Judge, ";
                    str_Head += "Reverse (km/h), ";
                    str_Head += "WSS F-L (km/h), WSS F-R (km/h), WSS R-L (km/h), WSS R-R (km/h), WSS Judge, ";
                    str_Head += "ABS Min F-L (kg), ABS Max F-L (kg), ";
                    str_Head += "ABS Min F-R (kg), ABS Max F-R (kg), ";
                    str_Head += "ABS Min R-L (kg), ABS Max R-L (kg), ";
                    str_Head += "ABS Min R-R (kg), ABS Max R-R (kg), ";

                    str_Head += "Front weight (kg), Weight F-L (kg), Weight F-R (kg), ";
                    str_Head += "Front drag F-L (kg), Front drag F-R (kg), Front Drag (%), Front Drag Judge, ";
                    str_Head += "Front brake F-L (kg), Front brake F-R (kg), ";
                    str_Head += "Front Diff. (%), Front Diff. judge, ";
                    str_Head += "Front Sum (%), Front Sum judge, Front judge, ";

                    str_Head += "Rear weight (kg), Weight R-L (kg), Weight R-R (kg), ";
                    str_Head += "Rear drag F-L (kg), Rear drag F-R (kg), Rear Drag (%), Rear Drag Judge, ";
                    str_Head += "Rear brake F-L (kg), Rear brake F-R (kg), ";
                    str_Head += "Rear Diff. (%), Rear Diff. judge, ";
                    str_Head += "Rear Sum (%), Rear Sum judge, Rear judge, ";

                    str_Head += "Total weight (kg), Total left brake (kg), Total right brake (kg), ";
                    str_Head += "Total brake (%), Total brake judge, ";

                    str_Head += "Parking left brake (kg), Parking right brake (kg), ";
                    str_Head += "Parking brake (%), Parking brake judge, ";
                    str_Head += "Brake judge, ";

                    sw.WriteLine(str_Head);
                    #endregion

                    string str_Data = "";

                    foreach (DataRow row in dt.Rows)
                    {
                        str_Data = "";
                        #region Model
                        DB_Back.DB_Info.Select(row["dbAcceptNo"].ToString());

                        str_Data += Ret_WorkNo(DB_Back.DB_Info.dbAcceptNo) + ", ";
                        str_Data += DB_Back.DB_Info.dbVin___No + ", ";
                        str_Data += DB_Back.DB_Info.dbCarModel + ", ";
                        str_Data += DB_Back.DB_Info.dbECUModel + ", ";
                        str_Data += DB_Back.DB_Info.dbCarBarID + ", ";
                        str_Data += DB_Back.DB_Info.dbCarWbase + ", ";
                        str_Data += DB_Back.DB_Info.dbCarEngin + ", ";
                        str_Data += DB_Back.DB_Info.dbCarTranM + ", ";
                        str_Data += DB_Back.DB_Info.dbCar_ABST + ", ";
                        str_Data += DB_Back.DB_Info.dbCarCurve + ", ";
                        str_Data += DB_Back.DB_Info.dbCarDrive + ", ";
                        str_Data += DB_Back.DB_Info.dbTestDate + ", ";
                        #endregion

                        #region RnB Data
                        DB_Back.DB_RnBs.Select(row["dbAcceptNo"].ToString());

                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.dbSMTValue, 1) + ", ";
                        str_Data += DB_Back.DB_RnBs.dbSMT_OkNg + ", ";
                        str_Data += DB_Back.DB_RnBs.db1SST_Val + ", ";   //최대 속도
                        
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1Drag__L, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1Drag__R, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2Drag__L, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2Drag__R, 0)+ ", ";

                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1Brake_L, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1Brake_R, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2Brake_L, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2Brake_R, 0) + ", ";

                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2Park__L, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2Park__R, 0) + ", ";

                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1Balan_L, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1Balan_R, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1Balance, 2) + ", ";
                        str_Data += DB_Back.DB_RnBs.db1Bal_Pan + ", ";

                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2Balan_L, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2Balan_R, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2Balance, 2) + ", ";
                        str_Data += DB_Back.DB_RnBs.db2Bal_Pan + ", ";

                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db_BalForR, 2) + ", ";
                        str_Data += DB_Back.DB_RnBs.db_Balance + ", ";

                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db_Reverse, 1) + ", ";
                        
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1SenSpdL, 1) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1SenSpdR, 1) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2SenSpdL, 1) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2SenSpdR, 1) + ", ";
                        str_Data += DB_Back.DB_RnBs.db_Sen_Spd + ", ";

                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1ABS_DeL, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1ABS_InL, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1ABS_DeR, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db1ABS_InR, 0) + ", ";

                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2ABS_DeL, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2ABS_InL, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2ABS_DeR, 0) + ", ";
                        str_Data += Ret_Data_Val(DB_Back.DB_RnBs.db2ABS_InR, 0) + ", ";
                        
                        #endregion

                        #region Brake Data
                        DB_Back.DBBrake.Select(row["dbAcceptNo"].ToString());

                        if (DB_Back.DBBrake.dbBrake_OX != " ")
                        {
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db1_Weight, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db1_Wgt__L, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db1_Wgt__R, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db1Drag__L, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db1Drag__R, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db1Drag__V, 1) + ", ";
                            str_Data += DB_Back.DBBrake.db1Drag_OX + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db1Brake_L, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db1Brake_R, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db1Diff__V, 1) + ", ";
                            str_Data += DB_Back.DBBrake.db1Diff_OX + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db1Sum___V, 1) + ", ";
                            str_Data += DB_Back.DBBrake.db1Sum__OX + ", ";
                            str_Data += DB_Back.DBBrake.db1BrakeOX + ", ";

                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db2_Weight, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db2_Wgt__L, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db2_Wgt__R, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db2Drag__L, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db2Drag__R, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db2Drag__V, 1) + ", ";
                            str_Data += DB_Back.DBBrake.db2Drag_OX + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db2Brake_L, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db2Brake_R, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db2Diff__V, 1) + ", ";
                            str_Data += DB_Back.DBBrake.db2Diff_OX + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.db2Sum___V, 1) + ", ";
                            str_Data += DB_Back.DBBrake.db2Sum__OX + ", ";
                            str_Data += DB_Back.DBBrake.db2BrakeOX + ", ";

                            str_Data += Ret_Data_Val(DB_Back.DBBrake.dbT_Weight, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.dbTBrake_L, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.dbTBrake_R, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.dbTBrake_V, 1) + ", ";
                            str_Data += DB_Back.DBBrake.dbTBrakeOX + ", ";

                            str_Data += Ret_Data_Val(DB_Back.DBBrake.dbAPark__L, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.dbAPark__R, 0) + ", ";
                            str_Data += Ret_Data_Val(DB_Back.DBBrake.dbAPark__V, 1) + ", ";
                            str_Data += DB_Back.DBBrake.dbAPark_OX + ", ";

                            str_Data += DB_Back.DBBrake.dbBrake_OX + ", ";
                        }
                        #endregion

                        sw.WriteLine(str_Data);
                    }

                    sw.Close();

                    Logs.MakeLog_File(Log_His.Back, csv_File);
                    Ret = true;
                }
                catch (Exception ex)
                {
                    sw.Close();
                    Ret = false;
                }
            }

            return Ret;
        }

        private static string Ret_Data_Val(double value, int point)
        {
            if(value == -1)
            {
                return "";
            }
            else
            {
                return Math.Round(value, point).ToString();
            }
        }

        private static string Ret_WorkNo(string AcptNo)
        {
            return AcptNo.Substring(0, 8) + "-" + AcptNo.Substring(8, 5);
        }

        public static bool Backup__XLSX(string pFile)
        {
            bool Ret = false;

            Excel.Application xlApp = null;
            Excel.Workbook xlWBook = null;
            Excel.Worksheet xlSheet = null;

            try
            {
                bool flag = false;

                xlApp = new Excel.Application();
                xlWBook = xlApp.Workbooks.Open(pFile);


                if (flag)
                {
                    xlSheet = xlWBook.Worksheets.get_Item(pFile);
                }
                else
                {
                    xlSheet = xlWBook.Worksheets.Add(Type.Missing, xlWBook.Worksheets[1]);
                    xlSheet.Name = pFile;
                }

                xlApp.DisplayAlerts = false;
                xlApp.Visible = false;
                xlApp.ScreenUpdating = false;
                xlApp.DisplayStatusBar = false;
                xlApp.Calculation = Excel.XlCalculation.xlCalculationManual;
                xlApp.EnableEvents = false;
                //xlApp.UserControl = true;
                //xlApp.Interactive = true;

                Excel.Range range = xlSheet.UsedRange; // 사용중인 셀 범위를 가져오기
                for (int cnt = 1; cnt < range.Rows.Count; cnt++)
                {
                    xlSheet.Cells[cnt, 1] = "";
                    xlSheet.Cells[cnt, 2] = "";
                    xlSheet.Cells[cnt, 3] = "";
                    xlSheet.Cells[cnt, 4] = "";
                    xlSheet.Cells[cnt, 5] = "";
                    xlSheet.Cells[cnt, 6] = "";
                    xlSheet.Cells[cnt, 7] = "";
                    xlSheet.Cells[cnt, 8] = "";
                }

                xlSheet.Cells[1, 1] = "Segment";
                xlSheet.Cells[1, 2] = "Time";
                xlSheet.Cells[1, 3] = "Sum time";
                xlSheet.Cells[1, 4] = "Speed";
                xlSheet.Cells[1, 5] = "Items";
                xlSheet.Cells[1, 6] = "Vehicle";
                xlSheet.Cells[1, 7] = "Roll";
                xlSheet.Cells[1, 8] = "Description";


                xlWBook.SaveAs(pFile, Excel.XlFileFormat.xlWorkbookDefault);
                xlWBook.Close(true);
                xlApp.Quit();

                Ret = true;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                Ret = false;
            }
            finally
            {
                ReleaseObject(xlSheet);
                ReleaseObject(xlWBook);
                ReleaseObject(xlApp);

                xlSheet = null;
                xlWBook = null;
                xlApp = null;
            }
            return Ret;
        }

        public static void ReleaseObject(Object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj); // 액셀 객체 해제 
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect(); // 가비지 수집 
            }
        }
        #endregion
    }

    public class clsCurve
    {
        public List<Curve_Data> G_Data;
        string CurveSet = Application.StartupPath + @"\DCurve\Curve.xlsx";

        #region Curve Gpaph Read/Write
        public Queue<string> Crv__FileList()
        {
            Queue<string> list = new Queue<string>();
            string crv_Dir = Application.StartupPath + @"\DCurve\";

            if (System.IO.Directory.Exists(crv_Dir))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(crv_Dir);

                foreach (var fi in di.GetFiles("*.crv"))
                {
                    list.Enqueue(fi.Name.Replace(".crv", ""));
                }
            }

            return list;
        }
        public bool Get_DriveCurve(string pCurve)
        {
            bool ret = false;

            switch (PSet.OwnerCrv)
            {
                case 0: ret = Crv__CurveRead(pCurve); break;
                case 1: ret = xlsx_CurveRead(pCurve); 
                        //ret = ExcelCurveRead(pCurve); 속도가 느림
                        break;
                case 2: ret = MDB__CurveRead(pCurve); break;
            }

            return ret;
        }
        public bool Set_DriveCurve(string pCurve, clsCurve crv_Data)
        {
            bool ret = false;

            switch (PSet.OwnerCrv)
            {
                case 0: ret = Crv__CurveSave(pCurve, crv_Data); break;
                case 1: //ret = xlsx_CurveSave(pCurve, crv_Data);   시트 삭제가 안됨, 리스트 생성이 이상함
                        ret = ExcelCurveSave(pCurve, crv_Data); 
                        break;
                case 2: ret = MDB__CurveSave(pCurve, crv_Data); break;
            }

            return ret;
        }

        #region Text(curve.crv)
        public bool Crv__Curve_Del(string pCurve)
        {
            if (System.IO.Directory.Exists(pCurve))
            {
                System.IO.File.Delete(pCurve);
            }

            return true;
        }
        public bool Crv__CurveRead(string pCurve)
        {
            string crv_File = Application.StartupPath + @"\DCurve\" + pCurve + ".crv";
            bool Ret_Flag = true;
            int Sum_Time = 0;

            if (!File.Exists(crv_File)) return false;

            G_Data = new List<Curve_Data>();

            using (StreamReader sr = new StreamReader(crv_File, Encoding.Default))
            {
                try
                {
                    string input;
                    int cnt = 0;
                    input = sr.ReadLine();
                    while ((input = sr.ReadLine()) != null)
                    {
                        string[] data = input.Split('|');
                        if (data.Length < 8) continue;
                        int crvTime, crvSpeed;
                        if (!int.TryParse(data[1], out crvTime)) crvTime = 0;
                        if (!int.TryParse(data[3], out crvSpeed)) crvSpeed = 0;

                        Sum_Time += crvTime;

                        G_Data.Add(new Curve_Data
                        {
                            Segment = data[0].ToUpper(),
                            Time = crvTime,
                            T_Time = Sum_Time,
                            Speed = crvSpeed,
                            Items = data[4],
                            Vehicle = data[5],
                            Roll = data[6],
                            Description = data[7]
                        });
                        
                        cnt++;
                    }

                    sr.Close();
                    Ret_Flag = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    sr.Close();
                    Ret_Flag = false;
                }
            }

            return Ret_Flag;
        }
        public bool Crv__CurveSave(string pCurve, clsCurve crv_Data)
        {
            string crv_File = Application.StartupPath + @"\DCurve\" + pCurve + ".crv";
            bool Ret_Flag = true;
            int sum_Time = 0;

            using (StreamWriter sw = new StreamWriter(crv_File, false, Encoding.Default))
            {
                try
                {
                    sw.WriteLine("구분,소요시간,누적시간,속도,측정항목,차량,장비,설명");

                    //sw.WriteLine("Segment,Time,T_Sum,Speed,Items,Vehicle,Roll,Description");
                    string str_Data = "";

                    for (int cnt = 0; cnt < crv_Data.G_Data.Count; cnt++)
                    {
                        sum_Time += crv_Data.G_Data[cnt].Time;

                        str_Data = crv_Data.G_Data[cnt].Segment.ToUpper() + "|";
                        str_Data += crv_Data.G_Data[cnt].Time + "|";
                      //str_Data += crv_Data.G_Data[cnt].T_Time + "|";
                        str_Data += sum_Time + "|";
                        str_Data += crv_Data.G_Data[cnt].Speed + "|";
                        str_Data += crv_Data.G_Data[cnt].Items + "|";
                        str_Data += crv_Data.G_Data[cnt].Vehicle + "|";
                        str_Data += crv_Data.G_Data[cnt].Roll + "|";
                        str_Data += crv_Data.G_Data[cnt].Description + "|";

                        sw.WriteLine(str_Data);
                    }

                    sw.Close();
                    Ret_Flag = true;
                }
                catch (Exception ex)
                {
                    sw.Close();
                    Ret_Flag = false;
                }
            }
            return Ret_Flag;
        }
        #endregion

        #region Excel(curve.xlsx)
        #region OLEDB (C#에서 엑셀을 OLEDB로 이용할 경우에는 ADO.NET의 OleDb 클래스들을 사용하여 엑셀 데이타를 핸들링하게 된다.)
        public Queue<string> xlsx_CurveList()
        {
            Queue<string> list = new Queue<string>();
            DataTable dt = null;
            OleDbConnection conn = null;

            try
            {
                // OLEDB를 이용한 엑셀 연결
                // Excel 97-2003 .xls
                // string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\x\test.xls;Extended Properties='Excel 8.0;HDR=No'";

                // Excel 2007 이후 .xlsx
                string szConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + CurveSet + ";Extended Properties='Excel 8.0;HDR=No'";

                conn = new OleDbConnection(szConn);
                conn.Open();

                dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                string listname = "";
                list.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    listname = row["TABLE_NAME"].ToString();
                    listname = listname.Replace("$", "");
                    listname = listname.Replace("'", "");

                    list.Enqueue(listname);
                }

                conn.Close();
                return list;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                return null;
            }
            finally
            {
                // Clean up.
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }
        public void xlsx_Curve_Del(string CurveName)
        {
            OleDbConnection conn = null;

            try
            {
                // OLEDB를 이용한 엑셀 연결
                // Excel 97-2003 .xls
                // string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\x\test.xls;Extended Properties='Excel 8.0;HDR=No'";

                // Excel 2007 이후 .xlsx
                string szConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + CurveSet + ";Mode=ReadWrite;Extended Properties='Excel 8.0;HDR=No'";

                conn = new OleDbConnection(szConn);
                conn.Open();

                // 엑셀로부터 시트 안의 모든 데이터는 삭제되나 시트는 삭제가 안됨
                OleDbCommand cmd = new OleDbCommand("DROP TABLE [" + CurveName + "$]", conn);
                cmd.ExecuteNonQuery();

                //OleDbCommand cmd1 = new OleDbCommand("DROP [" + CurveName + "$]", conn);
                //cmd1.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public bool xlsx_CurveRead(string CurveName)
        {
            try
            {
                // OLEDB를 이용한 엑셀 연결
                // Excel 97-2003 .xls
                // string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\x\test.xls;Extended Properties='Excel 8.0;HDR=No'";

                // Excel 2007 이후 .xlsx
                string szConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + CurveSet + ";Extended Properties='Excel 8.0;Imex=1;HDR=No'";

                OleDbConnection conn = new OleDbConnection(szConn);
                conn.Open();

                // 엑셀로부터 데이타 읽기
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + CurveName + "$]", conn);
                OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);

                G_Data = new List<Curve_Data>();

                int Sum_Time = 0;
                string x_1, x_2, x_3, x_4, x_5, x_6, x_7, x_8;

                for (int cnt = 1; cnt < dt.Rows.Count; cnt++)
                {
                    x_1 = dt.Rows[cnt][0].ToString();
                    x_2 = dt.Rows[cnt][1].ToString();
                    x_3 = dt.Rows[cnt][2].ToString();
                    x_4 = dt.Rows[cnt][3].ToString();
                    x_5 = dt.Rows[cnt][4].ToString();
                    x_6 = dt.Rows[cnt][5].ToString();
                    x_7 = dt.Rows[cnt][6].ToString();
                    x_8 = dt.Rows[cnt][7].ToString();

                    if (x_1 != "" && x_2 != "" && x_3 != "" && x_4 != "" && x_5 != "" && x_6 != "" && x_7 != "" && x_8 != "")
                    {
                        int xTime, xSpeed;
                        if (!int.TryParse(x_2, out xTime)) xTime = 0;
                        if (!int.TryParse(x_4, out xSpeed)) xSpeed = 0;
                        Sum_Time += xTime;

                        G_Data.Add(new Curve_Data
                        {
                            Segment = x_1,
                            Time = xTime,
                            T_Time = Sum_Time,
                            Speed = xSpeed,
                            Items = x_5,
                            Vehicle = x_6,
                            Roll = x_7,
                            Description = x_8
                        });
                    }
                }

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                return false;
            }
        }
        public bool xlsx_CurveSave(string CurveName, clsCurve crv_Data)
        {
            bool Ret = false;
            OleDbConnection conn = null;

            try
            {
                // OLEDB를 이용한 엑셀 연결
                // Excel 97-2003 .xls
                // string szConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\x\test.xls;Extended Properties='Excel 8.0;HDR=No'";

                // Excel 2007 이후 .xlsx
                string szConn = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Mode=ReadWrite;Extended Properties='Excel 8.0;Imex=0;HDR=No'", CurveSet);

                conn = new OleDbConnection(szConn);
                conn.Open();

                string sql = "";

                // 엑셀로부터 시트 안의 모든 데이터는 삭제되나 시트는 삭제가 안됨
                sql = string.Format("DROP TABLE [{0}$]", CurveName);
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                cmd.ExecuteNonQuery();



                // 엑셀로부터 시트 안의 모든 데이터는 삭제되나 시트는 삭제가 안됨
                sql = string.Format("CREATE TABLE [{0}$]", CurveName);
                cmd = new OleDbCommand(sql, conn);
                cmd.ExecuteNonQuery();

                //sql = string.Format("DELETE FROM [{0}$] ", CurveName);
                //cmd = new OleDbCommand(sql, conn);
                //cmd.ExecuteNonQuery();

                // Header 라인 생성
                StringBuilder Header_Column = new StringBuilder();
                Header_Column.Append("[Segment] CHAR(255), ");
                Header_Column.Append("[Time] CHAR(255), ");
                Header_Column.Append("[Sum time] CHAR(255), ");
                Header_Column.Append("[Speed] CHAR(255), ");
                Header_Column.Append("[Items] CHAR(255), ");
                Header_Column.Append("[Vehicle] CHAR(255), ");
                Header_Column.Append("[Roll] CHAR(255), ");
                Header_Column.Append("[Description] CHAR(255) ");

                sql = string.Format("CREATE TABLE [{0}$] (" + Header_Column.ToString() + ")", CurveName);
                cmd = new OleDbCommand(sql, conn);
                cmd.ExecuteNonQuery();

                // 엑셀로부터 데이타 읽기
                OleDbCommand cmd1 = new OleDbCommand("SELECT * FROM [" + CurveName + "$]", conn);
                OleDbDataAdapter adpt = new OleDbDataAdapter(cmd1);
                DataSet ds = new DataSet();
                adpt.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string data = string.Format("F1:{0}, F2:{1}, F3:{2}", dr[0], dr[1], dr[2]);
                    MessageBox.Show(data);
                }
                
                //sql = string.Format("UPDATE [{0}$A1:B1] SET F1='Segment, Time'", CurveName);
                //cmd = new OleDbCommand(sql, conn);
                //cmd.ExecuteNonQuery();

                int sum_Time = 0;
                for (int cnt = 0; cnt < crv_Data.G_Data.Count; cnt++)
                {
                    sum_Time += crv_Data.G_Data[cnt].Time;

                    // 데이타 추가
                    sql = string.Format("INSERT INTO [{0}$] VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')", 
                        CurveName,
                        crv_Data.G_Data[cnt].Segment.ToUpper(),
                        crv_Data.G_Data[cnt].Time,
                        sum_Time,
                        crv_Data.G_Data[cnt].Speed,
                        crv_Data.G_Data[cnt].Items,
                        crv_Data.G_Data[cnt].Vehicle,
                        crv_Data.G_Data[cnt].Roll,
                        crv_Data.G_Data[cnt].Description);
                    cmd = new OleDbCommand(sql, conn);
                    cmd.ExecuteNonQuery();

                }

                // 엑셀로부터 데이타 읽기
                OleDbCommand cmd2 = new OleDbCommand("SELECT * FROM [" + CurveName + "$]", conn);
                OleDbDataAdapter adpt1 = new OleDbDataAdapter(cmd2);
                DataSet ds1 = new DataSet();
                adpt1.Fill(ds1);

                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    string data = string.Format("F1:{0}, F2:{1}, F3:{2}", dr[0], dr[1], dr[2]);
                    MessageBox.Show(data);
                }

                Ret = true;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                Ret = false;
            }
            finally
            {
                conn.Close();
            }
            return Ret;
        }
        #endregion

        #region Excel Automation (C#에서 엑셀 오토메이션을 이용하기 위해서는 Excel Interop 을 참조한 후, Office Automation COM API들을 사용하게 된다)
        public Queue<string> ExcelCurveList()
        {
            Queue<string> list = new Queue<string>();

            Excel.Application xlApp = null;
            Excel.Workbook xlWBook = null;
            Excel.Worksheet xlSheet = null;

            try
            {
                xlApp = new Excel.Application();
                xlWBook = xlApp.Workbooks.Open(CurveSet);
                
                xlApp.DisplayAlerts = false;
                xlApp.Visible = false;
                xlApp.ScreenUpdating = false;
                xlApp.DisplayStatusBar = false;
                xlApp.Calculation = Excel.XlCalculation.xlCalculationManual;
                xlApp.EnableEvents = false;
                //xlApp.UserControl = true;
                //xlApp.Interactive = true;

                list.Clear();
                foreach (Excel.Worksheet xlS in xlWBook.Worksheets)
                {
                    list.Enqueue(xlS.Name);
                }

                xlWBook.Close(false);
                xlApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
            finally
            {
                ReleaseObject(xlSheet);
                ReleaseObject(xlWBook);
                ReleaseObject(xlApp);

                xlSheet = null;
                xlWBook = null;
                xlApp = null;
            }
            return list;
        }
        public void ExcelCurve_Del(string CurveName)
        {
            Excel.Application xlApp = null;
            Excel.Workbook xlWBook = null;
            Excel.Worksheet xlSheet = null;

            try
            {
                xlApp = new Excel.Application();
                xlWBook = xlApp.Workbooks.Open(CurveSet);
                xlSheet = xlWBook.Worksheets.get_Item(CurveName);
                xlSheet.Delete();

                xlWBook.Close(true);
                xlApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
            finally
            {
                ReleaseObject(xlSheet);
                ReleaseObject(xlWBook);
                ReleaseObject(xlApp);

                xlSheet = null;
                xlWBook = null;
                xlApp = null;
            }
        }
        public bool ExcelCurveRead(string CurveName)
        {
            bool Ret = false;

            Excel.Application xlApp = null;
            Excel.Workbook xlWBook = null;
            Excel.Worksheet xlSheet = null;

            try
            {
                xlApp = new Excel.Application();
                xlWBook = xlApp.Workbooks.Open(CurveSet);
                xlSheet = xlWBook.Worksheets.get_Item(CurveName) as Excel.Worksheet;

                xlApp.DisplayAlerts = false;
                xlApp.Visible = false;
                xlApp.ScreenUpdating = false;
                xlApp.DisplayStatusBar = false;
                xlApp.Calculation = Excel.XlCalculation.xlCalculationManual;
                xlApp.EnableEvents = false;
                //xlApp.UserControl = true;
                //xlApp.Interactive = true;

                G_Data = new List<Curve_Data>();

                int Sum_Time = 0;
                string x_1, x_2, x_3, x_4, x_5, x_6, x_7, x_8;

                Excel.Range range = xlSheet.UsedRange; // 사용중인 셀 범위를 가져오기

                for (int cnt = 2; cnt < range.Rows.Count; cnt++)
                {
                    x_1 = xlSheet.Cells[cnt, 1].Value2.ToString();
                    x_2 = xlSheet.Cells[cnt, 2].Value2.ToString();
                    x_3 = xlSheet.Cells[cnt, 3].Value2.ToString();
                    x_4 = xlSheet.Cells[cnt, 4].Value2.ToString();
                    x_5 = xlSheet.Cells[cnt, 5].Value2.ToString();
                    x_6 = xlSheet.Cells[cnt, 6].Value2.ToString();
                    x_7 = xlSheet.Cells[cnt, 7].Value2.ToString();
                    x_8 = xlSheet.Cells[cnt, 8].Value2.ToString();

                    int xTime2, xSpeed2;
                    if (!int.TryParse(x_2, out xTime2)) xTime2 = 0;
                    if (!int.TryParse(x_4, out xSpeed2)) xSpeed2 = 0;
                    Sum_Time += xTime2;

                    G_Data.Add(new Curve_Data
                    {
                        Segment = x_1,
                        Time = xTime2,
                        T_Time = Sum_Time,
                        Speed = xSpeed2,
                        Items = x_5,
                        Vehicle = x_6,
                        Roll = x_7,
                        Description = x_8
                    });
                }

                xlWBook.Close(false);
                xlApp.Quit();

                Ret = true;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                Ret = false;
            }
            finally
            {
                ReleaseObject(xlSheet);
                ReleaseObject(xlWBook);
                ReleaseObject(xlApp);

                xlSheet = null;
                xlWBook = null;
                xlApp = null;
            }
            return Ret;
        }
        public bool ExcelCurveSave(string CurveName, clsCurve crv_Data)
        {
            bool Ret = false;

            Excel.Application xlApp = null;
            Excel.Workbook xlWBook = null;
            Excel.Worksheet xlSheet = null;

            try
            {
                bool flag = false;

                xlApp = new Excel.Application();
                xlWBook = xlApp.Workbooks.Open(CurveSet);

                foreach (Excel.Worksheet xlS in xlWBook.Worksheets)
                {
                    if (!flag && (xlS.Name == CurveName))
                    {
                        flag = true;
                    }
                }

                if (flag)
                {
                    xlSheet = xlWBook.Worksheets.get_Item(CurveName);
                }
                else
                {
                    xlSheet = xlWBook.Worksheets.Add(Type.Missing, xlWBook.Worksheets[1]);
                    xlSheet.Name = CurveName;
                }

                xlApp.DisplayAlerts = false;
                xlApp.Visible = false;
                xlApp.ScreenUpdating = false;
                xlApp.DisplayStatusBar = false;
                xlApp.Calculation = Excel.XlCalculation.xlCalculationManual;
                xlApp.EnableEvents = false;
                //xlApp.UserControl = true;
                //xlApp.Interactive = true;

                Excel.Range range = xlSheet.UsedRange; // 사용중인 셀 범위를 가져오기
                for (int cnt = 1; cnt < range.Rows.Count; cnt++)
                {
                    xlSheet.Cells[cnt, 1] = "";
                    xlSheet.Cells[cnt, 2] = "";
                    xlSheet.Cells[cnt, 3] = "";
                    xlSheet.Cells[cnt, 4] = "";
                    xlSheet.Cells[cnt, 5] = "";
                    xlSheet.Cells[cnt, 6] = "";
                    xlSheet.Cells[cnt, 7] = "";
                    xlSheet.Cells[cnt, 8] = "";
                }
                
                xlSheet.Cells[1, 1] = "Segment";
                xlSheet.Cells[1, 2] = "Time";
                xlSheet.Cells[1, 3] = "Sum time";
                xlSheet.Cells[1, 4] = "Speed";
                xlSheet.Cells[1, 5] = "Items";
                xlSheet.Cells[1, 6] = "Vehicle";
                xlSheet.Cells[1, 7] = "Roll";
                xlSheet.Cells[1, 8] = "Description";

                int sum_Time = 0;
                for (int cnt = 0; cnt < crv_Data.G_Data.Count; cnt++)
                {
                    sum_Time += crv_Data.G_Data[cnt].Time;

                    xlSheet.Cells[2 + cnt, 1] = crv_Data.G_Data[cnt].Segment.ToUpper();
                    xlSheet.Cells[2 + cnt, 2] = crv_Data.G_Data[cnt].Time;
                    xlSheet.Cells[2 + cnt, 3] = sum_Time;
                    xlSheet.Cells[2 + cnt, 4] = crv_Data.G_Data[cnt].Speed;
                    xlSheet.Cells[2 + cnt, 5] = crv_Data.G_Data[cnt].Items;
                    xlSheet.Cells[2 + cnt, 6] = crv_Data.G_Data[cnt].Vehicle;
                    xlSheet.Cells[2 + cnt, 7] = crv_Data.G_Data[cnt].Roll;
                    xlSheet.Cells[2 + cnt, 8] = crv_Data.G_Data[cnt].Description;
                }

                xlWBook.SaveAs(CurveSet, Excel.XlFileFormat.xlWorkbookDefault);
                xlWBook.Close(true);
                xlApp.Quit();

                Ret = true;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                Ret= false;
            }
            finally
            {
                ReleaseObject(xlSheet);
                ReleaseObject(xlWBook);
                ReleaseObject(xlApp);

                xlSheet = null;
                xlWBook = null;
                xlApp = null;
            }
            return Ret;
        }

        public void ReleaseObject(Object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj); // 액셀 객체 해제 
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect(); // 가비지 수집 
            }
        }
        #endregion
        #endregion

        #region MDB(curve)
        public Queue<string> MDB__FileList()
        {
            Queue<string> list = new Queue<string>();

            return list;
        }
        public bool MDB__Curve_Del(string pCurve)
        {
            bool Ret_Flag = true;


            return Ret_Flag;
        }
        public bool MDB__CurveRead(string pCurve)
        {
            bool Ret_Flag = true;


            return Ret_Flag;
        }
        public bool MDB__CurveSave(string pCurve, clsCurve crv_Data)
        {
            bool Ret_Flag = true;


            return Ret_Flag;
        }
        #endregion
        #endregion
    }

    public class Curve_Data
    {
        public string Segment { get; set; }
        public int Time { get; set; }
        public int T_Time { get; set; }
        public int Speed { get; set; }
        public string Items { get; set; }
        public string Vehicle { get; set; }
        public string Roll { get; set; }
        public string Description { get; set; }
    }

    class cls_INI
    {
        string Set_Path;

        public cls_INI(String sPath)
        {
            Set_Path = sPath;
        }

        // INI파일읽기함수(섹션설정)
        public string[] GetIniSection(string Section)
        {
            byte[] ba = new byte[255];
            uint Flag = GetPrivateProfileSection(Section, ba, 255, Set_Path);
            return Encoding.Default.GetString(ba).Split(new char[1] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
        }

        // INI파일읽기함수(섹션,키값설정)
        public string GetIniString(string Section, string Key)
        {
            StringBuilder Ret = new StringBuilder(500);
            int Flag = GetPrivateProfileString(Section, Key, "", Ret, 500, Set_Path);
            return Ret.ToString();
        }
        public int GetIniNumber(string Section, string Key)
        {
            return Convert.ToInt32(GetPrivateProfileInt(Section, Key, 0, Set_Path));
        }
        public double GetIniDouble(string Section, string Key)
        {
            StringBuilder Ret = new StringBuilder(500);
            int Flag = GetPrivateProfileString(Section, Key, "", Ret, 500, Set_Path);
            double val;
            if (!double.TryParse(Ret.ToString(), out val)) val = 0;
            return val;
        }
        public float GetIni_Float(string Section, string Key)
        {
            StringBuilder Ret = new StringBuilder(500);
            int Flag = GetPrivateProfileString(Section, Key, "", Ret, 500, Set_Path);
            float val;
            if (!float.TryParse(Ret.ToString(), out val)) val = 0;
            return val;
        }

        // INI파일쓰기함수(섹션,키값설정)
        public bool SetIniValue(string Section, string Key, string Value)
        {
            return (WritePrivateProfileString(Section, Key, Value, Set_Path));
        }

        #region INI 파일 읽고 쓰기 DllImport
        /// <summary>
        /// INI파일에섹션과키로검색하여값을문자열형으로읽어옵니다.
        /// </summary>
        /// <param name="lpAppName">섹션명</param>
        /// <param name="lpKeyName">키값</param>
        /// <param name="lpDefault">기본값</param>
        /// <param name="lpReturnedString">가져온문자열</param>
        /// <param name="nSize">문자열버퍼크기</param>
        /// <param name="lpFileName">파일이름</param>
        /// <returns>가져온문자열의크기</returns>
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        /// <summary>
        /// INI파일에섹션과키로검색하여값을저장합니다.
        /// </summary>
        /// <param name="lpAppName">섹션명</param>
        /// <param name="lpKeyName">키값</param>
        /// <param name="lpString">저장할문자열</param>
        /// <param name="lpFileName">파일이름</param>
        /// <returns>저장성공여부</returns>
        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        /// <summary>
        /// INI파일에섹션과키로검색하여값을Inteager형으로불러옵니다.
        /// </summary>
        /// <param name="lpAppName">섹션명</param>
        /// <param name="lpKeyName">키값</param>
        /// <param name="nDefault">기본값</param>
        /// <param name="lpFileName">파일이름</param>
        /// <returns> 검색된값, 해당키로검색실패시기본값으로대체됨.</returns>
        [DllImport("kernel32")]
        public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        /// <summary>
        /// INI파일에섹션으로검색하여키와값을Pair형태로가져옵니다.
        /// </summary>
        /// <param name="IpAppName">섹션명</param>
        /// <param name="IpPairValues">Pair한키와값을담을배열</param>
        /// <param name="nSize">배열의크기</param>
        /// <param name="IpFileName">파일이름</param>
        /// <returns>읽어온바이트수</returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetPrivateProfileSection(string IpAppName, byte[] IpPairValues, uint nSize, string IpFileName);

        /// <summary>
        /// INI파일의섹션을가져옵니다.
        /// </summary>
        /// <param name="IpSections">섹션의리스트를직렬화하여담을배열</param>
        /// <param name="nSize">배열의크기</param>
        /// <param name="IpFileName">파일이름</param>
        /// <returns>읽어온바이트수</returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetPrivateProfileSectionNames(byte[] IpSections, uint nSize, string IpFileName);
        #endregion

    }
}