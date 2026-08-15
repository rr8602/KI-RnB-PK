using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using NationalInstruments.DAQmx;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

//using Emgu.CV;
//using Emgu.CV.Structure;
//using Emgu.CV.UI;
//using Emgu.CV.Util;

namespace KI_RnB
{
    public static class NI
    {
        #region Plot Variables
        static double[] data_array_X = new double[100];
        static double[] data_array_Y = new double[100];
        static double[] Kalman_array_Y = new double[100];
        static double[] Kalman_array_X = new double[100];
        static Random rand = new Random();
        #endregion

        //#region Kalman Filter and Poins Lists
        //PointF[] oup = new PointF[2];
        //private Kalman kal;
        //private SyntheticData syntheticData;
        //private List<PointF> mousePoints;
        //private List<PointF> kalmanPoints;
        //#endregion

        //NI PCI 6624 Board
        private static string CH_FL = "Dev1/ctr0";
        private static string CH_FR = "Dev1/ctr1";
        private static string CH_RL = "Dev1/ctr2";
        private static string CH_RR = "Dev1/ctr3";

        private static string SourceFL = "/Dev1/PFI39";  //A:39, B:37, Z:38
        private static string SourceFR = "/Dev1/PFI35";  //A:35, B:33, Z:34
        private static string SourceRL = "/Dev1/PFI31";  //A:31, B:29, Z:30
        private static string SourceRR = "/Dev1/PFI27";  //A:27, B:25, Z:26

        public static Thread thread;
        public static bool threadStop;

        private static Task Task_FL;
        private static Task Task_FR;
        private static Task Task_RL;
        private static Task Task_RR;
        private static Task runningTask_FL;
        private static Task runningTask_FR;
        private static Task runningTask_RL;
        private static Task runningTask_RR;
        private static CounterReader counterInReader_FL;
        private static CounterReader counterInReader_FR;
        private static CounterReader counterInReader_RL;
        private static CounterReader counterInReader_RR;
        private static AsyncCallback asyncCB_FL;
        private static AsyncCallback asyncCB_FR;
        private static AsyncCallback asyncCB_RL;
        private static AsyncCallback asyncCB_RR;
        private static double[] data_FL;
        private static double[] data_FR;
        private static double[] data_RL;
        private static double[] data_RR;

        public static bool Run_FL { get; set; }            //동작 상태 FL
        public static bool Run_FR { get; set; }            //동작 상태 FR
        public static bool Run_RL { get; set; }            //동작 상태 RL
        public static bool Run_RR { get; set; }            //동작 상태 RR

        private static CIEncoderDecodingType encoderType;
        private static CIEncoderZIndexPhase encoderPhase;

        //Channel Parameters
        public static bool zIndexEnable { get; set; }   //엔코더 Z 상 사용 여부
        public static int ENC_Type      { get; set; }   //채배수
        public static int ENCPhase      { get; set; }   //펄스 수신 조건
        public static double zIndexValue { get; set; }   //엔코더 Z 
        public static int ENCPulse      { get; set; }   //엔코더 펄스 수 / 회전당
        public static double InitDist   { get; set; }   //초기화 값

        //Timing Parameters
        public static double ScanRate { get; set; }   //읽기 속도
        public static int numberOfSamples { get; set; }   //읽을 갯수

        public static long OfstTime;
        public static Double ReadTime;
        public static Double One_Time;

        public static long count;
        public static long Herz;
        public static long Get___FL;    //엔코더 펄스(필터 전)
        public static long Get___FR;
        public static long Get___RL;
        public static long Get___RR;

        public static long Zero__FL;   //엔코더 펄스(필터 후)
        public static long Zero__FR;
        public static long Zero__RL;
        public static long Zero__RR;

        public static long ENC___FL;   //엔코더 펄스(필터 후)
        public static long ENC___FR;
        public static long ENC___RL;
        public static long ENC___RR;

        public static Queue<long> Q_ENC_FL = new Queue<long>();
        public static Queue<long> Q_ENC_FR = new Queue<long>();
        public static Queue<long> Q_ENC_RL = new Queue<long>();
        public static Queue<long> Q_ENC_RR = new Queue<long>();

        public struct Wheel
        {
            public long ENC, oENC, GapE;            //엔코더 펄스(현재, 이전, 차이)
            public Double RPM, oRPM;                //각 바퀴 RPM
            public Double GapT, oldT;               //시간 계산
            public Double Speed, oSpeed;            //각 바퀴 Speed(km/h)
            public Double Accel;                    //Acceleration 가속도
            public Double Force, oForce;            //Force 제동력 [kg * mm/s^2]
            public Double Calc;                     //-(Force / 1000 / PSet.NewTGain);
            public Double Kgf, oKgf;                //Force 제동력 [kgf]
            public Double Devia;                    //Deviation %
            public Double Jerk;
            public Double Loss;                     // Loss = (M x Roller RPM) + B
            public Double Move;
            public Double Factor;

            public Double RPM_0, RPM_1, RPM_2, RPM_3, RPM_4, RPM_5, RPM_6, RPM_7, RPM_8, RPM_9, RPM10, RPM11, RPM12, RPM13, RPM14, RPM15, RPM16, RPM17, RPM18, RPM19; //가속도 RPM
            public Double Now_0, Now_1, Now_2, Now_3, Now_4, Now_5, Now_6, Now_7, Now_8, Now_9, Now10, Now11, Now12, Now13, Now14, Now15, Now16, Now17, Now18, Now19; //가속도 시간 
            
            public double FLD_Time, FLD_Ofst;
            public bool FLD_Flag;

            public Queue<long> Q_ENC;
            public Queue<double> Q_RPM;
            public Queue<double> Q_Acc;
            public Queue<double> Q_kgf;
        }

        public struct Scan_Data
        {
            public Double Scan__Hz;

            public int RPM_Filt;    //RPM 필터 모드 (0:Average, 1:Sort)
            public int RPM_Cunt;    //RPM 필터 갯수

            public int Acc_Filt;    //RPM 필터 모드 (0:Average, 1:Sort)
            public int Acc_Cunt;    //RPM 필터 갯수

            public int Before_C;    //이전 데이터 
            public int kgf_Aver;    //결과 필터 

            public Double Gap_Time, Old_Time;   //시간 계산
            public Double PTS;
            public Double PTSvalue;
            public Queue<double> Q_PTS;

            public Wheel FL;
            public Wheel FR;
            public Wheel RL;
            public Wheel RR;
        }

        public static Scan_Data Loss = new Scan_Data();
        
        public static void Init()
        {
            //string[] CJ =  DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External);

            ScanRate = 1000000;
            InitDist = 0.0;
            numberOfSamples = 1000;
            zIndexValue = 0;
            zIndexEnable = false;

            ENCPulse = 1;
            ENC_Type = 0;
            ENCPhase = 1;

            try
            {
                switch (ENC_Type)
                {
                    case 0: encoderType = CIEncoderDecodingType.X1; break;  //X1 ㅡ
                    case 1: encoderType = CIEncoderDecodingType.X2; break;  //X2
                    case 2: encoderType = CIEncoderDecodingType.X4; break;  //X4
                }

                switch (ENCPhase)
                {
                    case 0: encoderPhase = CIEncoderZIndexPhase.AHighBHigh; break;  //A High B High
                    case 1: encoderPhase = CIEncoderZIndexPhase.AHighBLow;  break;  //A High B Low
                    case 2: encoderPhase = CIEncoderZIndexPhase.ALowBHigh;  break;  //A Low  B High
                    case 3: encoderPhase = CIEncoderZIndexPhase.ALowBLow;   break;  //A Low  B Low
                }

                //asyncCB_FL = new AsyncCallback(CounterInCallback_FL);
                //asyncCB_FR = new AsyncCallback(CounterInCallback_FR);
                //asyncCB_RL = new AsyncCallback(CounterInCallback_RL);
                //asyncCB_RR = new AsyncCallback(CounterInCallback_RR);
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Init: " + ex.Message);
            }
        }

        public static void Close()
        {
            Stop();
        }

        public static void Start()
        {
            CH_FL = "Dev1/ctr0";
            CH_FR = "Dev1/ctr1";
            CH_RL = "Dev1/ctr2";
            CH_RR = "Dev1/ctr3";

            SourceFL = "/Dev1/PFI39";  //A:39, B:37, Z:38
            SourceFR = "/Dev1/PFI35";  //A:35, B:33, Z:34
            SourceRL = "/Dev1/PFI31";  //A:31, B:29, Z:30
            SourceRR = "/Dev1/PFI27";  //A:27, B:25, Z:26

            if (PSet.ENC_FL_0 != "") { SourceFL = PSet.ENC_FL_0; }
            if (PSet.ENC_FR_0 != "") { SourceFR = PSet.ENC_FR_0; }
            if (PSet.ENC_RL_0 != "") { SourceRL = PSet.ENC_RL_0; }
            if (PSet.ENC_RR_0 != "") { SourceRR = PSet.ENC_RR_0; }

            if (PSet.ENC_FL_1 != "") { CH_FL = PSet.ENC_FL_1; }
            if (PSet.ENC_FR_1 != "") { CH_FR = PSet.ENC_FR_1; }
            if (PSet.ENC_RL_1 != "") { CH_RL = PSet.ENC_RL_1; }
            if (PSet.ENC_RR_1 != "") { CH_RR = PSet.ENC_RR_1; }

            OfstTime = DateTime.Now.Ticks;
            Loss.Old_Time = 0.0d; 
            Loss.Gap_Time = 0.0d; 

            Loss.Scan__Hz = 0.200;

            Loss.RPM_Filt = 3; //RPM 필터 모드 (0:None, 1:Average, 2:Sort, 3:Sort 3 Average, 4:Sort 5 Average)
            Loss.RPM_Cunt = 5; //RPM 필터 갯수

            Loss.Acc_Filt = 0; //RPM 필터 모드 (0:None, 1:Average, 2:Sort, 3:Sort 3 Average, 4:Sort 5 Average)
            Loss.Acc_Cunt = 9; //RPM 필터 갯수

            Loss.Before_C = 9;
            Loss.kgf_Aver = 2;

            Loss.FL.Q_ENC = new Queue<long>(); 
            Loss.FR.Q_ENC = new Queue<long>(); 
            Loss.RL.Q_ENC = new Queue<long>(); 
            Loss.RR.Q_ENC = new Queue<long>(); 

            Loss.FL.Q_RPM = new Queue<double>(); 
            Loss.FR.Q_RPM = new Queue<double>(); 
            Loss.RL.Q_RPM = new Queue<double>(); 
            Loss.RR.Q_RPM = new Queue<double>(); 

            Loss.FL.Q_Acc = new Queue<double>(); 
            Loss.FR.Q_Acc = new Queue<double>(); 
            Loss.RL.Q_Acc = new Queue<double>(); 
            Loss.RR.Q_Acc = new Queue<double>(); 
            
            TaskStart_FL();
            TaskStart_FR();
            TaskStart_RL();
            TaskStart_RR();

            thread = new Thread(new ThreadStart(NIDAQmx_Run));
            if (!thread.IsAlive)
            {
                threadStop = false;
                thread.Start(); 
            }
        }

        public static void Stop()
        {
            threadStop = true;
            if (thread != null && thread.IsAlive) thread.Abort();

            Task_Stop_FL();
            Task_Stop_FR();
            Task_Stop_RL();
            Task_Stop_RR();
        }

        private static void NIDAQmx_Run()
        {
            long OfstTime = DateTime.Now.Ticks;
            long Gap_Time = DateTime.Now.AddSeconds(0.001).Ticks;

            try
            {
                while (thread.IsAlive)
                {
                    Thread.Sleep(100);
                    if ((DateTime.Now.Ticks - Gap_Time) > 0)
                    {
                        Gap_Time = DateTime.Now.AddSeconds(0.001).Ticks;
                        Scan_NIDAQmx();
                    }
                }
            }
            catch (Exception ex)
            {
                //if(ex.ErrorCode == 10061)
                //MessageBox.Show(ex.Message);
                //Fom_Main.Log_Data(ex.Message);
                Logs.MakeLog_File(Log_His.Err_, "NIDAQmx_Run: " + ex.Message);
            }

            if (!threadStop) Start();
        }

        public static void Scan_NIDAQmx()
        {
            try
            {
                if (Run_FL && runningTask_FL != null)
                {
                    Get___FL = counterInReader_FL.ReadSingleSampleUInt32();
                }
                if (Run_FR && runningTask_FR != null)
                {
                    Get___FR = counterInReader_FR.ReadSingleSampleUInt32();
                }
                if (Run_RL && runningTask_RL != null)
                {
                    Get___RL = counterInReader_RL.ReadSingleSampleUInt32();
                }
                if (Run_RR && runningTask_RR != null)
                {
                    Get___RR = counterInReader_RR.ReadSingleSampleUInt32();
                }

                ReadTime = (DateTime.Now.Ticks - OfstTime) / 10000000f;

                ENC___FL = Zero__FL - Get___FL;
                ENC___FR = Zero__FR - Get___FR;
                ENC___RL = Zero__RL - Get___RL;
                ENC___RR = Zero__RR - Get___RR;    

                if ((ReadTime - Loss.Old_Time) >= Loss.Scan__Hz)
                {
                    Loss_Calibration();
                    
                    Loss.Old_Time = ReadTime;
                }

                NI.Loss.FL.Move = (((PSet.RFLDia * Math.PI) * ENC___FL) / 1000 / 1);
                NI.Loss.FR.Move = (((PSet.RFLDia * Math.PI) * ENC___FR) / 1000 / 1);
                NI.Loss.RL.Move = (((PSet.RFLDia * Math.PI) * ENC___RL) / 1000 / 1);
                NI.Loss.RR.Move = (((PSet.RFLDia * Math.PI) * ENC___RR) / 1000 / 1);

                count++;
                if (ReadTime - One_Time >= 1)
                {
                    One_Time = ReadTime;
                    Herz = count; count = 0;
                }

                if (NI.Loss.FL.Speed < 5 && NI.Loss.FR.Speed < 5 && NI.Loss.RL.Speed < 5 && NI.Loss.RR.Speed < 5)
                {
                    TSet.Spd_5kmh = false;
                }
                else
                {
                    TSet.Spd_5kmh = true;
                }

                if ((TSet.Spd_5kmh != TSet.Old_5kmh) || (PLC.DO.Spd_5kmph == true && TSet.Spd_5kmh == false))
                {
                    TSet.Old_5kmh = TSet.Spd_5kmh;
                    PLC.DO.Spd_5kmph = TSet.Spd_5kmh;
                    PLC.PLC_312_Puts();
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Scan_NIDAQmx: " + ex.Message);
            }
        }

        public static void Loss_Calibration()
        {
            //if ((ReadTime - Loss.Old_Time) >= Loss.Scan__Hz)
            {
                Loss.Gap_Time = ReadTime - Loss.Old_Time;

                Loss.FL = Wheel_Loss(Loss, Loss.FL, ENC___FL, PSet.RFLPul, PSet.RFLDia, PSet.FL_Moment, PSet.Loss_FL.Aver, PSet.Load_FL.Aver);
                Loss.FR = Wheel_Loss(Loss, Loss.FR, ENC___FR, PSet.RFRPul, PSet.RFRDia, PSet.FR_Moment, PSet.Loss_FR.Aver, PSet.Load_FR.Aver);
                Loss.RL = Wheel_Loss(Loss, Loss.RL, ENC___RL, PSet.RRLPul, PSet.RRLDia, PSet.RL_Moment, PSet.Loss_RL.Aver, PSet.Load_RL.Aver);
                Loss.RR = Wheel_Loss(Loss, Loss.RR, ENC___RR, PSet.RRRPul, PSet.RRRDia, PSet.RR_Moment, PSet.Loss_RR.Aver, PSet.Load_RR.Aver);

                Random random = new Random();
                double PTSvalue = Math.Abs((Loss.FL.RPM - Loss.FL.oRPM) / Loss.Gap_Time * 0.10472 * Loss.FL.Factor) + (random.NextDouble() * 2);

                Loss.PTS = PTSvalue;    // Loss.FL.Kgf;
                //Loss.PTS = H2Y.QSoAv3Filter(7, Loss.FL.Kgf, Loss.Q_PTS);
                //Loss.PTS = H2Y.QSoAv3Filter(7, PTSvalue, Loss.Q_PTS);

                TSet.SpeedAll = (Loss.FL.Speed + Loss.FR.Speed + Loss.RL.Speed + Loss.RR.Speed) / 4;

                if (TSet.BrakeRun)
                {
                    TSet.GBrk_Max = Ret_Max_Data(TSet.GBrk_Max);
                }
                if (TSet.ABSv_Run)
                {
                    TSet.ABSB_Min = Ret_Min_Data(TSet.ABSB_Min);
                    TSet.ABSB_Max = Ret_Max_Data(TSet.ABSB_Max);
                }

                //Loss.Old_Time = ReadTime;
            }
        }

        public static TSet.wheel_Test Ret_Min_Data(TSet.wheel_Test Axle)   //각 바퀴의 최소/최대값
        {
            int point = 0;

            if (Axle.FL == -1) { Axle.FL = Math.Round(Loss.FL.Kgf, point); }
            if (Axle.FR == -1) { Axle.FR = Math.Round(Loss.FR.Kgf, point); }
            if (Axle.RL == -1) { Axle.RL = Math.Round(Loss.RL.Kgf, point); }
            if (Axle.RR == -1) { Axle.RR = Math.Round(Loss.RR.Kgf, point); }

            if (Axle.FL >= Loss.FL.Kgf) { Axle.FL = Math.Round(Loss.FL.Kgf, point); }
            if (Axle.FR >= Loss.FR.Kgf) { Axle.FR = Math.Round(Loss.FR.Kgf, point); }
            if (Axle.RL >= Loss.RL.Kgf) { Axle.RL = Math.Round(Loss.RL.Kgf, point); }
            if (Axle.RR >= Loss.RR.Kgf) { Axle.RR = Math.Round(Loss.RR.Kgf, point); }

            return Axle;
        }
        public static TSet.wheel_Test Ret_Max_Data(TSet.wheel_Test Axle)   //각 바퀴의 최소/최대값
        {
            int point = 0;

            if (Axle.FL == -1) { Axle.FL = Math.Round(Loss.FL.Kgf, point); }
            if (Axle.FR == -1) { Axle.FR = Math.Round(Loss.FR.Kgf, point); }
            if (Axle.RL == -1) { Axle.RL = Math.Round(Loss.RL.Kgf, point); }
            if (Axle.RR == -1) { Axle.RR = Math.Round(Loss.RR.Kgf, point); }

            if (Axle.FL <= Loss.FL.Kgf) { Axle.FL = Math.Round(Loss.FL.Kgf, point); }
            if (Axle.FR <= Loss.FR.Kgf) { Axle.FR = Math.Round(Loss.FR.Kgf, point); }
            if (Axle.RL <= Loss.RL.Kgf) { Axle.RL = Math.Round(Loss.RL.Kgf, point); }
            if (Axle.RR <= Loss.RR.Kgf) { Axle.RR = Math.Round(Loss.RR.Kgf, point); }

            return Axle;
        }

        private static Wheel Wheel_Loss(Scan_Data kind, Wheel wheel, long ENC, int pulse, int Dia, double Moment, Loss_Items Loss, Load_Items Load)
        {
            wheel.GapT = ReadTime - wheel.oldT; 
            wheel.oldT = ReadTime;
            wheel.ENC = ENC;    //H2Y.QAver_Filter(10, ENC, wheel.Q_ENC);
            wheel.GapE = ENC - wheel.oENC;

            double RPM = Math.Abs((double)wheel.GapE / pulse) * (60 / wheel.GapT);
            switch (kind.RPM_Filt)
            {
                case 0: wheel.RPM = RPM; break;
                case 1: wheel.RPM = H2Y.QAver_Filter(kind.RPM_Cunt, RPM, wheel.Q_RPM); break;
                case 2: wheel.RPM = H2Y.QSort_Filter(kind.RPM_Cunt, RPM, wheel.Q_RPM); break;
                case 3: wheel.RPM = H2Y.QSoAv3Filter(kind.RPM_Cunt, RPM, wheel.Q_RPM); break;
                case 4: wheel.RPM = H2Y.QSoAv5Filter(kind.RPM_Cunt, RPM, wheel.Q_RPM); break;
            }
            wheel.Speed = Math.Abs(((Dia * Math.PI) * wheel.RPM * 60) / 1000 / 1000);
            
            //Acceleration 가속도
            //if (Math.Abs(wheel.RPM - wheel.oRPM) > 0)
            //{
            //double Accel = (wheel.RPM - wheel.oRPM) / Loss.Gap_Time * (2 * Math.PI / 60);

            double RPMs_Gap = RPM - wheel.RPM_5;
            double Time_Gap = ReadTime - wheel.Now_5;

            switch (kind.Before_C)
            {
                case 0: RPMs_Gap = RPM - wheel.RPM_0; Time_Gap = ReadTime - wheel.Now_0; break;
                case 1: RPMs_Gap = RPM - wheel.RPM_1; Time_Gap = ReadTime - wheel.Now_1; break;
                case 2: RPMs_Gap = RPM - wheel.RPM_2; Time_Gap = ReadTime - wheel.Now_2; break;
                case 3: RPMs_Gap = RPM - wheel.RPM_3; Time_Gap = ReadTime - wheel.Now_3; break;
                case 4: RPMs_Gap = RPM - wheel.RPM_4; Time_Gap = ReadTime - wheel.Now_4; break;
                case 5: RPMs_Gap = RPM - wheel.RPM_5; Time_Gap = ReadTime - wheel.Now_5; break;
                case 6: RPMs_Gap = RPM - wheel.RPM_6; Time_Gap = ReadTime - wheel.Now_6; break;
                case 7: RPMs_Gap = RPM - wheel.RPM_7; Time_Gap = ReadTime - wheel.Now_7; break;
                case 8: RPMs_Gap = RPM - wheel.RPM_8; Time_Gap = ReadTime - wheel.Now_8; break;
                case 9: RPMs_Gap = RPM - wheel.RPM_9; Time_Gap = ReadTime - wheel.Now_9; break;

                case 10: RPMs_Gap = RPM - wheel.RPM10; Time_Gap = ReadTime - wheel.Now10; break;
                case 11: RPMs_Gap = RPM - wheel.RPM11; Time_Gap = ReadTime - wheel.Now11; break;
                case 12: RPMs_Gap = RPM - wheel.RPM12; Time_Gap = ReadTime - wheel.Now12; break;
                case 13: RPMs_Gap = RPM - wheel.RPM13; Time_Gap = ReadTime - wheel.Now13; break;
                case 14: RPMs_Gap = RPM - wheel.RPM14; Time_Gap = ReadTime - wheel.Now14; break;
                case 15: RPMs_Gap = RPM - wheel.RPM15; Time_Gap = ReadTime - wheel.Now15; break;
                case 16: RPMs_Gap = RPM - wheel.RPM16; Time_Gap = ReadTime - wheel.Now16; break;
                case 17: RPMs_Gap = RPM - wheel.RPM17; Time_Gap = ReadTime - wheel.Now17; break;
                case 18: RPMs_Gap = RPM - wheel.RPM18; Time_Gap = ReadTime - wheel.Now18; break;
                case 19: RPMs_Gap = RPM - wheel.RPM19; Time_Gap = ReadTime - wheel.Now19; break;
            }
            
            double Accel = RPMs_Gap / Time_Gap * (2 * Math.PI / 60);
            //double Accel = (RPM - wheel.oRPM) / kind.Gap_Time * (2 * Math.PI / 60);

            switch (kind.Acc_Filt)
            {
                case 0: wheel.Accel = Accel; break;
                case 1: wheel.Accel = H2Y.QAver_Filter(kind.Acc_Cunt, Accel, wheel.Q_Acc); break;
                case 2: wheel.Accel = H2Y.QSort_Filter(kind.Acc_Cunt, Accel, wheel.Q_Acc); break;
                case 3: wheel.Accel = H2Y.QSoAv3Filter(kind.Acc_Cunt, Accel, wheel.Q_Acc); break;
                case 4: wheel.Accel = H2Y.QSoAv5Filter(kind.Acc_Cunt, Accel, wheel.Q_Acc); break;
            }
            //}

            // I(관성 모멘트) = 55909249.02 kg.mm^2
            // r = PSet.RFLDia / 2          RFLDia = 908
            // @ rad/s^2
            //           I
            // Force = ----- @(kg * mm/s^2)
            //           r

            //                         1 m          1
            // force(kg * mm/s^2) X -------- X ------------
            //                       1000 mm    9.81 m/s^2

            wheel.RPM19 = wheel.RPM18;  wheel.Now19 = wheel.Now18;
            wheel.RPM18 = wheel.RPM17;  wheel.Now18 = wheel.Now17;
            wheel.RPM17 = wheel.RPM16;  wheel.Now17 = wheel.Now16;
            wheel.RPM16 = wheel.RPM15;  wheel.Now16 = wheel.Now15;
            wheel.RPM15 = wheel.RPM14;  wheel.Now15 = wheel.Now14;
            wheel.RPM14 = wheel.RPM13;  wheel.Now14 = wheel.Now13;
            wheel.RPM13 = wheel.RPM12;  wheel.Now13 = wheel.Now12;
            wheel.RPM12 = wheel.RPM11;  wheel.Now12 = wheel.Now11;
            wheel.RPM11 = wheel.RPM10;  wheel.Now11 = wheel.Now10;
            wheel.RPM10 = wheel.RPM_9;  wheel.Now10 = wheel.Now_9;
            
            wheel.RPM_9 = wheel.RPM_8;  wheel.Now_9 = wheel.Now_8;
            wheel.RPM_8 = wheel.RPM_7;  wheel.Now_8 = wheel.Now_7;
            wheel.RPM_7 = wheel.RPM_6;  wheel.Now_7 = wheel.Now_6;
            wheel.RPM_6 = wheel.RPM_5;  wheel.Now_6 = wheel.Now_5;
            wheel.RPM_5 = wheel.RPM_4;  wheel.Now_5 = wheel.Now_4;
            wheel.RPM_4 = wheel.RPM_3;  wheel.Now_4 = wheel.Now_3;
            wheel.RPM_3 = wheel.RPM_2;  wheel.Now_3 = wheel.Now_2;
            wheel.RPM_2 = wheel.RPM_1;  wheel.Now_2 = wheel.Now_1;
            wheel.RPM_1 = wheel.RPM_0;  wheel.Now_1 = wheel.Now_0;
            wheel.RPM_0 = RPM;          wheel.Now_0 = ReadTime;

            wheel.Factor = Moment / (Dia / 2) / 1000 / PSet.NewTGain;
            wheel.Loss = ((wheel.RPM * Loss.ChkM) + Loss.ChkB) / 1000 / PSet.NewTGain;
            wheel.Force = (wheel.Accel * Moment) / (Dia / 2) - wheel.Loss;
            wheel.Calc = -(wheel.Force / 1000 / PSet.NewTGain);

            double add = ((Load.Indi - Load.Calc) / Load.Indi);
            double kgf = (wheel.Calc * add) + wheel.Calc;
            wheel.Kgf = (wheel.oKgf + kgf) / 2;   //H2Y.QAver_Filter(kind.kgf_Aver, kgf, wheel.Q_kgf); 

            wheel.oENC = ENC;
            wheel.oRPM = RPM;   // wheel.RPM;
            wheel.oSpeed = wheel.Speed;
            wheel.oForce = wheel.Force;
            wheel.oKgf = wheel.Kgf;
            return wheel;
        }

        #region Task Start / Stop
        private static void TaskStart_FL()
        {
            try
            {
                //if (!threadStop)
                {
                    Task_FL = new Task();
                    Run_FL = true;

                    //Task_FL.CIChannels.CreateLinearEncoderChannel(CH_FL, "", encoderType, zIndexEnable, zIndexValue, encoderPhase, ENCPulse, InitDist, CILinearEncoderUnits.Meters);
                    Task_FL.CIChannels.CreateCountEdgesChannel(CH_FL, "Count Edges", CICountEdgesActiveEdge.Rising, Convert.ToInt64(0), CICountEdgesCountDirection.Up);
                    //Task_FL.Timing.ConfigureSampleClock(SourceFL, ScanRate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);

                    runningTask_FL = Task_FL;
                    counterInReader_FL = new CounterReader(Task_FL.Stream);

                    Task_FL.Start();
                }
            }
            catch (DaqException ex)
            {
                Task_Stop_FL();

                Logs.MakeLog_File(Log_His.Err_, "TaskStart_FL:" + ex.Message);
            }
        }
        private static void TaskStart_FR()
        {
            try
            {
                //if (!threadStop)
                {
                    Task_FR = new Task();
                    Run_FR = true;

                    //Task_FR.CIChannels.CreateLinearEncoderChannel(CH_FR, "", encoderType, zIndexEnable, zIndexValue, encoderPhase, ENCPulse, InitDist, CILinearEncoderUnits.Meters);
                    Task_FR.CIChannels.CreateCountEdgesChannel(CH_FR, "Count Edges", CICountEdgesActiveEdge.Rising, Convert.ToInt64(0), CICountEdgesCountDirection.Up);

                    runningTask_FR = Task_FR;
                    counterInReader_FR = new CounterReader(Task_FR.Stream);

                    Task_FR.Start();
                }
            }
            catch (DaqException ex)
            {
                Task_Stop_FR();

                Logs.MakeLog_File(Log_His.Err_, "TaskStart_FR:" + ex.Message);
            }
        }
        private static void TaskStart_RL()
        {
            try
            {
                //if (!threadStop)
                {
                    Task_RL = new Task();
                    Run_RL = true;

                    //Task_RL.CIChannels.CreateLinearEncoderChannel(CH_RL, "", encoderType, zIndexEnable, zIndexValue, encoderPhase, ENCPulse, InitDist, CILinearEncoderUnits.Meters);
                    Task_RL.CIChannels.CreateCountEdgesChannel(CH_RL, "Count Edges", CICountEdgesActiveEdge.Rising, Convert.ToInt64(0), CICountEdgesCountDirection.Up);

                    runningTask_RL = Task_RL;
                    counterInReader_RL = new CounterReader(Task_RL.Stream);

                    Task_RL.Start();
                }
            }
            catch (DaqException ex)
            {
                Task_Stop_RL();

                Logs.MakeLog_File(Log_His.Err_, "TaskStart_RL:" + ex.Message);
            }
        }
        private static void TaskStart_RR()
        {
            try
            {
                //if (!threadStop)
                {
                    Task_RR = new Task();
                    Run_RR = true;

                    //Task_RR.CIChannels.CreateLinearEncoderChannel(CH_RR, "", encoderType, zIndexEnable, zIndexValue, encoderPhase, ENCPulse, InitDist, CILinearEncoderUnits.Meters);
                    Task_RR.CIChannels.CreateCountEdgesChannel(CH_RR, "Count Edges", CICountEdgesActiveEdge.Rising, Convert.ToInt64(0), CICountEdgesCountDirection.Up);

                    runningTask_RR = Task_RR;
                    counterInReader_RR = new CounterReader(Task_RR.Stream);

                    Task_RR.Start();
                }
            }
            catch (DaqException ex)
            {
                Task_Stop_RR();

                Logs.MakeLog_File(Log_His.Err_, "TaskStart_RR:" + ex.Message);
            }
        }

        private static void Task_Stop_FL()
        {
            try
            {
                Run_FL = false;
                if (Task_FL != null)
                {
                    Task_FL.Stop();
                    Task_FL.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Stop Task_FL:" + ex.Message);
            }

            try
            {
                if (runningTask_FL != null)
                {
                    runningTask_FL.Dispose();
                    runningTask_FL = null;
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Stop runningTask_FL:" + ex.Message);
            }
        }
        private static void Task_Stop_FR()
        {
            try
            {
                Run_FR = false;
                if (Task_FR != null)
                {
                    Task_FR.Stop();
                    Task_FR.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Stop Task_FR:" + ex.Message);
            }

            try
            {
                if (runningTask_FR != null)
                {
                    runningTask_FR.Dispose();
                    runningTask_FR = null;
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Stop runningTask_FR:" + ex.Message);
            }
        }
        private static void Task_Stop_RL()
        {
            try
            {
                Run_RL = false;
                if (Task_RL != null)
                {
                    Task_RL.Stop();
                    Task_RL.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Stop Task_RL:" + ex.Message);
            }

            try
            {
                if (runningTask_RL != null)
                {
                    runningTask_RL.Dispose();
                    runningTask_RL = null;
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Stop runningTask_RL:" + ex.Message);
            }
        }
        private static void Task_Stop_RR()
        {
            try
            {
                Run_RR = false;
                if (Task_RR != null)
                {
                    Task_RR.Stop();
                    Task_RR.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Stop Task_RR:" + ex.Message);
            }

            try
            {
                if (runningTask_RR != null)
                {
                    runningTask_RR.Dispose();
                    runningTask_RR = null;
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Stop runningTask_RR:" + ex.Message);
            }
        }
        #endregion

        #region BeginReadSingleSampleDouble
        private static void StartSingle_FL()
        {
            try
            {
                Task_FL = new Task();
                Run_FL = true;

                Task_FL.CIChannels.CreateLinearEncoderChannel(CH_FL, "", encoderType, zIndexEnable, zIndexValue, encoderPhase, ENCPulse, InitDist, CILinearEncoderUnits.Meters);
                //Task_FL.Timing.ConfigureSampleClock(SourceFL, ScanRate, SampleClockActiveEdge.Rising, SampleQuantityMode.HardwareTimedSinglePoint);

                runningTask_FL = Task_FL;
                counterInReader_FL = new CounterReader(Task_FL.Stream);

                Task_FL.Start();

                // Use SynchronizeCallbacks to specify that the object      - SynchronizeCallbacks를 사용하여 개체를 지정합니다.
                // marshals callbacks across threads appropriately.         - 스레드 간의 콜백을 적절하게 마샬링합니다.
                //counterInReader_FL.SynchronizeCallbacks = true;

                // Memory Optimized Read method needs an initialized array. - 메모리 최적화 된 읽기 방법에는 초기화 된 배열이 필요합니다.
                //counterInReader_FL.BeginReadSingleSampleDouble(asyncCB_FL, Task_FL);
            }
            catch (DaqException ex)
            {
                Run_FL = false;
                Task_FL.Dispose();
                runningTask_FL.Dispose();
                runningTask_FL = null;

                Logs.MakeLog_File(Log_His.Err_, "StartSingle_FL:" + ex.Message);
            }
        }
        private static void StartSingle_FR()
        {
            try
            {
                Task_FR = new Task();
                Run_FR = true;

                Task_FR.CIChannels.CreateLinearEncoderChannel(CH_FR, "", encoderType, zIndexEnable, zIndexValue, encoderPhase, ENCPulse, InitDist, CILinearEncoderUnits.Meters);
                //Task_FR.Timing.ConfigureSampleClock(SourceFR, ScanRate, SampleClockActiveEdge.Rising, SampleQuantityMode.HardwareTimedSinglePoint);
                runningTask_FR = Task_FR;
                counterInReader_FR = new CounterReader(Task_FR.Stream);

                Task_FR.Start();

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                //counterInReader_FR.SynchronizeCallbacks = true;

                // Memory Optimized Read method needs an initialized array.
                //counterInReader_FR.BeginReadSingleSampleDouble(asyncCB_FR, Task_FR);
            }
            catch (DaqException ex)
            {
                Run_FR = false;
                Task_FR.Dispose();
                runningTask_FR.Dispose();
                runningTask_FR = null;

                Logs.MakeLog_File(Log_His.Err_, "StartSingle_FR:" + ex.Message);
            }
        }
        private static void StartSingle_RL()
        {
            try
            {
                Task_RL = new Task();
                Run_RL = true;

                Task_RL.CIChannels.CreateLinearEncoderChannel(CH_RL, "", encoderType, zIndexEnable, zIndexValue, encoderPhase, ENCPulse, InitDist, CILinearEncoderUnits.Meters);
                //Task_RL.Timing.ConfigureSampleClock(SourceRL, ScanRate, SampleClockActiveEdge.Rising, SampleQuantityMode.HardwareTimedSinglePoint);
                runningTask_RL = Task_RL;
                counterInReader_RL = new CounterReader(Task_RL.Stream);

                Task_RL.Start();

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                //counterInReader_RL.SynchronizeCallbacks = true;

                // Memory Optimized Read method needs an initialized array.
                //counterInReader_RL.BeginReadSingleSampleDouble(asyncCB_RL, Task_RL);
            }
            catch (DaqException ex)
            {
                Run_RL = false;
                Task_RL.Dispose();
                runningTask_RL.Dispose();
                runningTask_RL = null;

                Logs.MakeLog_File(Log_His.Err_, "StartSingle_RL:" + ex.Message);
            }
        }
        private static void StartSingle_RR()
        {
            try
            {
                Task_RR = new Task();
                Run_RR = true;

                Task_RR.CIChannels.CreateLinearEncoderChannel(CH_RR, "", encoderType, zIndexEnable, zIndexValue, encoderPhase, ENCPulse, InitDist, CILinearEncoderUnits.Meters);
                //Task_RR.Timing.ConfigureSampleClock(SourceRR, ScanRate, SampleClockActiveEdge.Rising, SampleQuantityMode.HardwareTimedSinglePoint);
                runningTask_RR = Task_RR;
                counterInReader_RR = new CounterReader(Task_RR.Stream);

                Task_RR.Start();

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                //counterInReader_RR.SynchronizeCallbacks = true;

                // Memory Optimized Read method needs an initialized array
                //counterInReader_RR.BeginReadSingleSampleDouble(asyncCB_RR, Task_RR);
            }
            catch (DaqException ex)
            {
                Run_RR = false;
                Task_RR.Dispose();
                runningTask_RR.Dispose();
                runningTask_RR = null;

                Logs.MakeLog_File(Log_His.Err_, "StartSingle_RR:" + ex.Message);
            }
        }
        #endregion

        #region BeginMemoryOptimizedReadMultiSampleDouble
        private static void Start_Multy_FL()
        {
            try
            {
                Task_FL = new Task();
                Run_FL = true;

                Task_FL.CIChannels.CreateLinearEncoderChannel(CH_FL, "", encoderType, zIndexEnable, zIndexValue, encoderPhase, ENCPulse, InitDist, CILinearEncoderUnits.Meters);
                Task_FL.Timing.ConfigureSampleClock(SourceFL, ScanRate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);
                
                runningTask_FL = Task_FL;
                counterInReader_FL = new CounterReader(Task_FL.Stream);

                // Use SynchronizeCallbacks to specify that the object      - SynchronizeCallbacks를 사용하여 개체를 지정합니다.
                // marshals callbacks across threads appropriately.         - 스레드 간의 콜백을 적절하게 마샬링합니다.
                counterInReader_FL.SynchronizeCallbacks = true;

                // Memory Optimized Read method needs an initialized array. - 메모리 최적화 된 읽기 방법에는 초기화 된 배열이 필요합니다.
                data_FL = new Double[numberOfSamples];
                counterInReader_FL.BeginMemoryOptimizedReadMultiSampleDouble(numberOfSamples, asyncCB_FL, Task_FL, data_FL);
            }
            catch (DaqException ex)
            {
                Run_FL = false;
                Task_FL.Dispose();
                runningTask_FL.Dispose();
                runningTask_FL = null;

                Logs.MakeLog_File(Log_His.Err_, "Start_Multy_FL:" + ex.Message);
            }
        }
        private static void Start_Multy_FR()
        {
            try
            {
                Task_FR = new Task();
                Run_FR = true;

                Task_FR.CIChannels.CreateLinearEncoderChannel(CH_FR, "", encoderType, zIndexEnable, zIndexValue, encoderPhase, ENCPulse, InitDist, CILinearEncoderUnits.Meters );
                Task_FR.Timing.ConfigureSampleClock(SourceFR, ScanRate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);
                runningTask_FR = Task_FR;
                counterInReader_FR = new CounterReader(Task_FR.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                counterInReader_FR.SynchronizeCallbacks = true;

                // Memory Optimized Read method needs an initialized array.
                data_FR = new Double[numberOfSamples];
                counterInReader_FR.BeginMemoryOptimizedReadMultiSampleDouble(numberOfSamples, asyncCB_FR, Task_FR, data_FR);
            }
            catch (DaqException ex)
            {
                Run_FR = false;
                Task_FR.Dispose();
                runningTask_FR.Dispose();
                runningTask_FR = null;

                Logs.MakeLog_File(Log_His.Err_, "Start_Multy_FR:" + ex.Message);
            }
        }
        private static void Start_Multy_RL()
        {
            try
            {
                Task_RL = new Task();
                Run_RL = true;

                Task_RL.CIChannels.CreateLinearEncoderChannel(CH_RL, "", encoderType, zIndexEnable, zIndexValue, encoderPhase, ENCPulse, InitDist, CILinearEncoderUnits.Meters);
                Task_RL.Timing.ConfigureSampleClock(SourceRL, ScanRate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);
                runningTask_RL = Task_RL;
                counterInReader_RL = new CounterReader(Task_RL.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                counterInReader_RL.SynchronizeCallbacks = true;

                // Memory Optimized Read method needs an initialized array.
                data_RL = new Double[numberOfSamples];
                counterInReader_RL.BeginMemoryOptimizedReadMultiSampleDouble(numberOfSamples, asyncCB_RL, Task_RL, data_RL);
            }
            catch (DaqException ex)
            {
                Run_RL = false;
                Task_RL.Dispose();
                runningTask_RL.Dispose();
                runningTask_RL = null;

                Logs.MakeLog_File(Log_His.Err_, "Start_Multy_RL:" + ex.Message);
            }
        }
        private static void Start_Multy_RR()
        {
            try
            {
                Task_RR = new Task();
                Run_RR = true;

                Task_RR.CIChannels.CreateLinearEncoderChannel(CH_RR, "", encoderType, zIndexEnable, zIndexValue, encoderPhase, ENCPulse, InitDist, CILinearEncoderUnits.Meters);
                Task_RR.Timing.ConfigureSampleClock(SourceRR, ScanRate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);
                runningTask_RR = Task_RR;
                counterInReader_RR = new CounterReader(Task_RR.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                counterInReader_RR.SynchronizeCallbacks = true;

                // Memory Optimized Read method needs an initialized array.
                data_RR = new Double[numberOfSamples];
                counterInReader_RR.BeginMemoryOptimizedReadMultiSampleDouble(numberOfSamples, asyncCB_RR, Task_RR, data_RR);
            }
            catch (DaqException ex)
            {
                Run_RR = false;
                Task_RR.Dispose();
                runningTask_RR.Dispose();
                runningTask_RR = null;

                Logs.MakeLog_File(Log_His.Err_, "Start_Multy_RR:" + ex.Message);
            }
        }
        #endregion

        #region Callback
        private static void CounterInCallback_FL(IAsyncResult ar)
        {
            try
            {
                if (runningTask_FL != null && runningTask_FL == ar.AsyncState)
                {
                    uint reading = counterInReader_FL.ReadSingleSampleUInt32();

                    Get___FL = reading;

                    counterInReader_FL.BeginReadSingleSampleDouble(asyncCB_FL, Task_FL);
                }
            }
            catch (DaqException ex)
            {
                CallbackStop_FL();

                Logs.MakeLog_File(Log_His.Err_, "CounterInCallback_FL:" + ex.Message);
            }
        }
        private static void CounterInCallback_FR(IAsyncResult ar)
        {
            try
            {
                if (runningTask_FR != null && runningTask_FR == ar.AsyncState)
                {
                    uint reading = counterInReader_FR.ReadSingleSampleUInt32();

                    Get___FR = reading;

                    counterInReader_FR.BeginReadSingleSampleDouble(asyncCB_FR, Task_FR);
                }
            }
            catch (DaqException ex)
            {
                CallbackStop_FR();

                Logs.MakeLog_File(Log_His.Err_, "CounterInCallback_FR:" + ex.Message);
            }
        }
        private static void CounterInCallback_RL(IAsyncResult ar)
        {
            try
            {
                if (runningTask_RL != null && runningTask_RL == ar.AsyncState)
                {
                    uint reading = counterInReader_RL.ReadSingleSampleUInt32();

                    Get___RL = reading; 

                    counterInReader_RL.BeginReadSingleSampleDouble(asyncCB_RL, Task_RL);
                }
            }
            catch (DaqException ex)
            {
                CallbackStop_RL();

                Logs.MakeLog_File(Log_His.Err_, "CounterInCallback_RL:" + ex.Message);
            }
        }
        private static void CounterInCallback_RR(IAsyncResult ar)
        {
            try
            {
                if (runningTask_RR != null && runningTask_RR == ar.AsyncState)
                {
                    uint reading = counterInReader_RR.ReadSingleSampleUInt32();

                    Get___RR = reading;

                    counterInReader_RR.BeginReadSingleSampleDouble(asyncCB_RR, Task_RR);
                }
            }
            catch (DaqException ex)
            {
                CallbackStop_RR();

                Logs.MakeLog_File(Log_His.Err_, "CounterInCallback_RR:" + ex.Message);
            }
        }

        private static void CallbackStop_FL()
        {
            Run_FL = false;
            runningTask_FL = null;
            System.Windows.Forms.Application.DoEvents();
            Task_FL.Dispose();
        }
        private static void CallbackStop_FR()
        {
            Run_FR = false;
            runningTask_FR = null;
            System.Windows.Forms.Application.DoEvents();
            Task_FR.Dispose();
        }
        private static void CallbackStop_RL()
        {
            Run_RL = false;
            runningTask_RL = null;
            System.Windows.Forms.Application.DoEvents();
            Task_RL.Dispose();
        }
        private static void CallbackStop_RR()
        {
            Run_RR = false;
            runningTask_RR = null;
            System.Windows.Forms.Application.DoEvents();
            Task_RR.Dispose();
        }
        #endregion
    }
}