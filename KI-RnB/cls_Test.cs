using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KI_RnB
{
//ECM  (Engine ECU)
//TCM  (Transmission ECU)
//ABS  (ABS ECU)
//ECS  (Electric Control Suspention)
//SSPS (Speed Sensitive Power Steering Wheel)
//TOD/TCCU(Torque on Demand/Transfer Case Control Unit)
//TGS-Lever(Transmission Gear Shift Lever)
//EPB  (Electric Parking Brake System)
//TPMS (Tire Pressure Monitoring System)
//EAS  (Electric Air Suspension)

//ABS       Antilock braking system
    //ABS-SILA  ABS warning lighthttp://checkout.secomps.co.kr/block?Culture=1042

    //AV        ABS outlet valve
//AX/AY     Acceleration sensor
//BLS       Break light switch
//DTC       Diagnostic trouble code
//ECU       Electronic control unit
//ESP       Electronic stability program
//EV        ABS inlet valve
//FA        Front axle
//FCM       Fault code memory
//FL        Front left
//FR        Front right
//HSV       Prime valve (ASV)
//IIS       Integrated Inertial Sensor
//LWS (SAS) (SAS)steering angle sensor
//PB        Process byte
//PS        Pressure sensor
//RA        Rear axle
//RFP       Recirculation pump
//RL        Rear left
//RR        Rear right
//SV        Solenoid valve
//UM        Pump motor feedback
//USV       Pilot valve
//WSS       Wheel speed sensor
//YRS       Yaw rate sensor
//PTS       Pedal Travel Sensor
//iTPMS     intelligent Tire Pressure Monitoring system (Circumference based)

    public struct Axle_1
    {
        public static double Weight { get; set; }  //전축 총 중량(kg)
        public static double Wgt__L { get; set; }  //전축 좌 중량(kg)
        public static double Wgt__R { get; set; }  //전축 우 중량(kg)
        public static double Brk__L { get; set; }  //1축 좌 제동력(kgf)
        public static double Brk__R { get; set; }  //1축 우 제동력(kgf)
        public static double Drag_L { get; set; }  //1축 좌 끌림 (kgf)
        public static double Drag_R { get; set; }  //1축 우 끌림 (kgf)
        public static double Drag_V { get; set; }  //1축 끌림 (%)
        public static string Drag_P { get; set; }  //1축 끌림 판정(OK, NG)

        public static double Park_L { get; set; }  //1축 좌 주차 (kgf)
        public static double Park_R { get; set; }  //1축 우 주차 (kgf)

        public static double Diff_V { get; set; }  //1축 제동력 편차(%)
        public static string Diff_P { get; set; }  //1축 제동력 편차(%) 판정(OK, NG)
        public static double Sum__V { get; set; }  //1축 제동력 합(%)
        public static string Sum__P { get; set; }  //1축 제동력 합(%) 판정(OK, NG)
        public static string JudgeP { get; set; }  //1축 제동력 판정(OK, NG)
    }

    public struct Axle_2
    {
        public static double Weight { get; set; }  //후축 총 중량(kg)
        public static double Wgt__L { get; set; }  //후축 좌 중량(kg)
        public static double Wgt__R { get; set; }  //후축 우 중량(kg)
        public static double Brk__L { get; set; }  //1축 좌 제동력(kgf)
        public static double Brk__R { get; set; }  //1축 우 제동력(kgf)
        public static double Drag_L { get; set; }  //1축 좌 끌림 (kgf)
        public static double Drag_R { get; set; }  //1축 우 끌림 (kgf)
        public static double Drag_V { get; set; }  //1축 끌림 (%)
        public static string Drag_P { get; set; }  //1축 끌림 판정(OK, NG)

        public static double Park_L { get; set; }  //1축 좌 주차 (kgf)
        public static double Park_R { get; set; }  //1축 우 주차 (kgf)

        public static double Diff_V { get; set; }  //1축 제동력 편차(%)
        public static string Diff_P { get; set; }  //1축 제동력 편차(%) 판정(OK, NG)
        public static double Sum__V { get; set; }  //1축 제동력 합(%)
        public static string Sum__P { get; set; }  //1축 제동력 합(%) 판정(OK, NG)
        public static string JudgeP { get; set; }  //1축 제동력 판정(OK, NG)
    }

    public struct TotalB
    {
        public static double Weight { get; set; }  //차량 총 중량(kg)
        public static double BrakeL { get; set; }  //전체 좌 제동력(kgf)
        public static double BrakeR { get; set; }  //전체 우 제동력(kgf)
        public static double BrakeV { get; set; }  //전체 제동력 합(%)
        public static string BrakeP { get; set; }  //전체 제동력 판정
    }

    public struct Parkin
    {
        public static double Weight { get; set; }  //차량 총 중량(kg)
        public static double BrakeL { get; set; }  //전체 좌 제동력(kgf)
        public static double BrakeR { get; set; }  //전체 우 제동력(kgf)
        public static double BrakeV { get; set; }  //전체 제동력 합(%)
        public static string BrakeP { get; set; }  //전체 제동력 판정
    }


    public static class TSet
    {
        #region Test Infomation
        public static string Select_o { get; set; }  //버튼 선택기
        public static string SelectNo { get; set; }  //버튼 선택기

        public static string AcceptNo { get; set; }  //측정 번호
        public static string Vin___No { get; set; }  //바코드
        public static string CarModel { get; set; }  //모델명
        public static string ECUModel { get; set; }  //ECU 모델명
        public static string CarBarID { get; set; }  //바코드 구분자
        public static string CarWbase { get; set; }  //휠베이스 거리
        public static string CarEngin { get; set; }  //엔진 모델
        public static string CarTranM { get; set; }  //트렌스미션 모델
        public static string Car_ABST { get; set; }  //ABS 모델
        public static string CarCurve { get; set; }  //드라이브 커브
        public static string CarDrive { get; set; }  //드라이브 축(Front, Rear, 4WD)
        public static string CarParam { get; set; }  //Parameter
        public static string TestDate { get; set; }  //측정 일자
        public static string Run_Time { get; set; }  //진행 시작 시간
        public static string TestTime { get; set; }  //측정 시각 시간
        public static string End_Time { get; set; }  //종료 시간
        #endregion

        #region Test Simulation
        public static bool SimulOnf;    //가상 테스트
        public static int Simul_No;    //가상 테스트 스텝 번호

        public static float Virtual_FL;
        public static float Virtual_FR;
        public static float Virtual_RL;
        public static float Virtual_RR;
        public static float VirtualRPM;
        public static float VirtualPTS;
        public static float VirtualSST;
        public static float VirtualL_W;
        public static float VirtualR_W;
        public static float VirtualL_B;
        public static float VirtualR_B;
        public static float VirtualSMT;
        #endregion

        #region Loss Data acquisition condition(Loss 데이터 취득 조건)
        public struct wheel_Loss
        {
            public Double Speed1, Speed2;
            public Double RPM__1, RPM__2;
            public Double Force1, Force2;
            public Double Kgf__1, Kgf__2;
            public Double Loss_M;
            public Double Loss_B;
            public Double C_Loss;
        }
        public static wheel_Loss FL = new wheel_Loss();
        public static wheel_Loss FR = new wheel_Loss();
        public static wheel_Loss RL = new wheel_Loss();
        public static wheel_Loss RR = new wheel_Loss();
        
        //         Force1 - Force2
        // M ---------------------------
        //    Roller RPM1 - Roller RPM2        

        // B = M x RPM1 - Force1  
        
        //             읽기 시점 RPM
        // Loss = (M x Roller RPM) + B
        
        #endregion

        #region Test Data
        public static Double StepTime;
        public static Double Step_old;

        public static Double SpeedStd;  //기준 속도 정의
        public static Double SpeedAll;  //전체 평균
        public static Double Bongshin;  //인디게이터

        public static Double Move__FL;  //각 바퀴 이동 거리(m)
        public static Double Move__FR;
        public static Double Move__RL;
        public static Double Move__RR;

        public static Double Ofst_MFL;  //각 바퀴 이동 거리 Offset (m)
        public static Double Ofst_MFR;
        public static Double Ofst_MRL;
        public static Double Ofst_MRR;

        public static Double Read__FL;
        public static Double Read__FR;
        public static Double Read__RL;
        public static Double Read__RR;
        public static Double Read_RPM;
        public static Double Read_PTS;
        public static float Read_SST;
        public static float Read_L_W;
        public static float Read_R_W;
        public static float Read_L_B;
        public static float Read_R_B;

        public static Double Bal___FL;  //Balance
        public static Double Bal___FR;
        public static Double Bal___RL;
        public static Double Bal___RR;
        public static Double BalFront;
        public static Double Bal_Rear;
        #endregion

        #region Test Results
        public static bool WSS__Onf;
        public static bool SpeedOnf;
        public static bool Drag_Onf;
        public static bool BrakeOnf;
        public static bool ABSv_Onf;
        public static bool Park_Onf;

        public static bool BrakeRun;
        public static bool ABSv_Run;
        public static int ABSvStep;

        public static  double ABSv_Time;

        public struct wheel_Test
        {
            public double FL;
            public double FR;
            public double RL;
            public double RR;

            public string OX;
        }

        public struct Nidec_Drive
        {
            public bool Cal_MD;
            public bool MT_Run;
            public bool MTSync;
            public bool MT_Brk;
            public bool MTPark;

            public int Status;
            public int CalSpd;
            public int WSSSpd;
            public int PB_Toq;
            public int PB_Spd;
        }

        public static Nidec_Drive Nidec_FL;
        public static Nidec_Drive Nidec_FR;
        public static Nidec_Drive Nidec_RL;
        public static Nidec_Drive Nidec_RR;

        public static float SST1_Val;
        public static string SST1Sine;
        public static string SST1_Pan;
        public static float SST2_Val;
        public static string SST2Sine;
        public static string SST2_Pan;
        public static string SST__Pan;

        public static double MaxSpeed;
        public static double SMTValue;
        public static double ReverseV;

        public static wheel_Test CH_Speed = new wheel_Test();
        public static wheel_Test Drag_Max = new wheel_Test();
        public static wheel_Test GBrk_Max = new wheel_Test();
        public static wheel_Test BrakeMax = new wheel_Test();
        public static wheel_Test Park_Max = new wheel_Test();

        public static wheel_Test BalanMin = new wheel_Test();
        public static wheel_Test BalanMax = new wheel_Test();

        public static wheel_Test SMT_Last = new wheel_Test();
        public static wheel_Test Rev_Last = new wheel_Test();

        public static wheel_Test WSS_Last = new wheel_Test();

        public static wheel_Test ABSB_Min = new wheel_Test();
        public static wheel_Test ABSB_Max = new wheel_Test();
        #endregion
        
        public static bool SST_Enter;
        public static bool SST_GoOut;
        public static bool PHO_Brake;
        public static bool BT_LiftUp;
        public static bool BT_LiftDn;
        public static bool BT_MotRun;

        public static bool Test_SST;    //Sideslip     검사 
        public static bool Test__BT;    //일반 제동력  검사 
        public static bool Test_RnB;    //Roll & Brake 검사 

        public static int StopFlag;    //시험 정보(0:완료, 1:비상정지, 2:에러 정지, 3:사용자 정지 ....
        public static bool Test_Run;    
        public static bool TestStop;    //프로그램 사용자 정지
        public static bool StepPrev;    //이전 스텝
        public static bool StepNext;    //다음 스텝
        public static bool Debug_Md;    //디버그 모드
        
        public static bool ChkBrake;    //제동력 검사 시작
        public static bool Spd_5kmh;    //개별 롤 속도 5km/h 이상 시 신호
        public static bool Old_5kmh;    //개별 롤 속도 5km/h 이상 시 신호
        public static bool ECU_Errs;    //ECU  검사 에러 진행

        public static bool Run_Stby;
        public static bool Out_Stby;

        public static bool ECU_Flag;
        public static byte ECU_Setp;
        public static double ECU_Ofst = DateTime.Now.Ticks;
        public static double ECU_Time = 0;
        public static double ECU_oldT = 0;

        public static void Scan_Sensors()
        {
            if (TSet.SimulOnf)
            {
                TSet.Read__FL = TSet.Virtual_FL;
                TSet.Read__FR = TSet.Virtual_FR;
                TSet.Read__RL = TSet.Virtual_RL;
                TSet.Read__RR = TSet.Virtual_RR;
                TSet.Read_RPM = TSet.VirtualRPM;
                TSet.Read_PTS = TSet.VirtualPTS;
                TSet.Read_SST = TSet.VirtualSST;
                TSet.Read_L_W = TSet.VirtualL_W;
                TSet.Read_R_W = TSet.VirtualR_W;
                TSet.Read_L_B = TSet.VirtualL_B;
                TSet.Read_R_B = TSet.VirtualR_B;
            }
            else
            {
                TSet.SST_Enter = PLC.DI.SST_Enter;
                TSet.SST_GoOut = PLC.DI.SST_GoOut;
                TSet.PHO_Brake = PLC.DI.PHO_Brake;
                TSet.BT_LiftUp = PLC.DI.BT_LiftUp;
                TSet.BT_LiftDn = PLC.DI.BT_LiftDn;
                TSet.BT_MotRun = PLC.DO.BT_MotRun;

                TSet.Read__FL = NI.Loss.FL.Speed;
                TSet.Read__FR = NI.Loss.FR.Speed;
                TSet.Read__RL = NI.Loss.RL.Speed;
                TSet.Read__RR = NI.Loss.RR.Speed;
                TSet.Read_RPM = TSet.Read__FL;
                //TSet.Read_PTS = TSet.VirtualPTS;
                TSet.Read_SST = PSet.CH0_Val;
                TSet.Read_L_W = PSet.CH2_Val;
                TSet.Read_R_W = PSet.CH3_Val;
                TSet.Read_L_B = PSet.CH4_Val;
                TSet.Read_R_B = PSet.CH5_Val;
            }
        }

        public static void Info_Clear()
        {
            AcceptNo = "";     //측정 번호
            Vin___No = "";     //바코드
            CarModel = "";     //모델명
            ECUModel = "";     //ECU 모델명
            CarBarID = "";     //바코드 구분자
            CarWbase = "";     //휠베이스 거리
            CarEngin = "";     //엔진 모델
            CarTranM = "";     //트렌스미션 모델
            Car_ABST = "";     //ABS 모델
            CarCurve = "";     //드라이브 커브
            CarDrive = "";     //드라이브 축
            CarParam = "";     //Parameter
            TestDate = "";     //측정 일자
            Run_Time = "";     //진행 시작 시간
            TestTime = "";     //측정 시각 시간
            End_Time = "";     //종료 시간
        }

        public static int TestStopFlag(int Flag)
        {
            int Ret_Flag = Flag;

            if (!TSet.Debug_Md)
            {
                if (Flag == 0 && PLC.DO.MD_Emerge) { Ret_Flag = 1; }    //D311[05] 장비 비상
                if (Flag == 0 && PLC.DI.PSW__Stop) { Ret_Flag = 2; }    //D103.9 PULL SW STOP
                if (Flag == 0 && TSet.TestStop) { Ret_Flag = 3; }       //프로그램 정지

                if (Flag == 0 && PLC.DI.Stop___PB) { Ret_Flag = 4; }    //D108.B VEHICLE STOP      PB
                if (Flag == 0 && PLC.DI.GOT__Stop) { Ret_Flag = 5; }    //D114.B VEHICLE STOP      GOT
                if (Flag == 0 && PLC.DI.Cancel_PB) { Ret_Flag = 6; }    //D108.C VEHICLE CANCEL    PB
                if (Flag == 0 && PLC.DI.GOTCancel) { Ret_Flag = 7; }    //D114.C VEHICLE CANCEL    GOT

                //if (!PLC.DO.MD___Auto) { TSet.StopFlag = 10; }  //자동 모드
                //if (!PLC.DO.MD__Ready) { TSet.StopFlag = 11; }  //레디 모드

                if (Flag == 0 && PLC.DI.FL_MotErr) { Ret_Flag = 21; }   //D103.4 FL-MOTOR DRIVER ERROR
                if (Flag == 0 && PLC.DI.FR_MotErr) { Ret_Flag = 22; }   //D103.5 FR-MOTOR DRIVER ERROR
                if (Flag == 0 && PLC.DI.RL_MotErr) { Ret_Flag = 23; }   //D103.6 RL-MOTOR DRIVER ERROR
                if (Flag == 0 && PLC.DI.RR_MotErr) { Ret_Flag = 24; }   //D103.7 RR-MOTOR DRIVER ERROR
            }

            return Ret_Flag;
        }
        public static void Info_DataAdd(fom_Main main)
        {
            int count = main.DB_All.DB_Info.Select(TSet.AcceptNo);

            main.DB_All.DB_Info.dbAcceptNo = TSet.AcceptNo;
            main.DB_All.DB_Info.dbVin___No = TSet.Vin___No;
            main.DB_All.DB_Info.dbCarModel = TSet.CarModel;
            main.DB_All.DB_Info.dbECUModel = TSet.ECUModel;
            main.DB_All.DB_Info.dbCarBarID = TSet.CarBarID;
            main.DB_All.DB_Info.dbCarWbase = TSet.CarWbase;
            main.DB_All.DB_Info.dbCarEngin = TSet.CarEngin;
            main.DB_All.DB_Info.dbCarTranM = TSet.CarTranM;
            main.DB_All.DB_Info.dbCar_ABST = TSet.Car_ABST;
            main.DB_All.DB_Info.dbCarCurve = TSet.CarCurve;
            main.DB_All.DB_Info.dbCarDrive = TSet.CarDrive;
            main.DB_All.DB_Info.dbTestDate = TSet.TestDate;
            main.DB_All.DB_Info.dbRun_Time = TSet.Run_Time;
            main.DB_All.DB_Info.dbTestTime = TSet.TestTime;
            main.DB_All.DB_Info.dbEnd_Time = TSet.End_Time;
            main.DB_All.DB_Info.dbStopFlag = TSet.StopFlag.ToString();

            if (count > 0)
            {
                main.DB_All.DB_Info.Update(TSet.AcceptNo);
            }
            else
            {
                main.DB_All.DB_Info.Insert();
            }
        }

        public static void LossData_Cls()
        {
            FL = WheelDataCls(FL);
            FR = WheelDataCls(FR);
            RL = WheelDataCls(RL);
            RR = WheelDataCls(RR);
        }
        
        private static wheel_Loss WheelDataCls(wheel_Loss wh)
        {
            wh.Speed1 = -1; wh.Speed2 = -1; 
            wh.RPM__1 = -1; wh.RPM__2 = -1; 
            wh.Force1 = -1; wh.Force2 = -1;
            wh.Kgf__1 = -1; wh.Kgf__2 = -1; 
            wh.Loss_M = -1; wh.Loss_B = -1; wh.C_Loss = -1;

            return wh;
        }
    }

    public class cls_Test
    {
        public fom_Main main;

        private int TestStep;
        private int Old_Step;
        private string old_Data;
        private byte Cycle = 3;
                
        private bool[] TestOn = new bool[28];    //시험 진행

        public fom_Test Fom_Test;
        private clsCurve crv_Test = new clsCurve();
        

        public cls_Test(fom_Main main)
        {
            this.main = main;

            if (crv_Test.Get_DriveCurve(TSet.CarCurve))
            {
                Fom_Test = new fom_Test(crv_Test, 1);   //그래프 풀 모드
            }
            else
            {
                if (main != null) main.Prog_LogData("Can not load drive curve data.");
                return;
            }

          
        }

        private bool Stop_Vehicle()
        {
            double ReadTime = 0, Old_Time = 0;
            double OfstTime = DateTime.Now.Ticks;
            double Gap_Time = DateTime.Now.AddSeconds(0.1).Ticks;
            bool Flag_Onf = false;

            while (true)
            {
                if (NI.Loss.FL.Speed < PSet.Stop_Spd && NI.Loss.FR.Speed < PSet.Stop_Spd &&
                    NI.Loss.RL.Speed < PSet.Stop_Spd && NI.Loss.RR.Speed < PSet.Stop_Spd)
                {
                    break;
                }
                else
                {
                    ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;

                    if ((ReadTime - Old_Time) >= 2)
                    {
                        Old_Time = ReadTime;
                        Flag_Onf = !Flag_Onf;

                        if (Flag_Onf)
                        {
                            Fom_Test.Refresh_Msgs("STOP Vehicle", System.Drawing.Color.Yellow);
                        }
                        else
                        {
                            Fom_Test.Refresh_Msgs("STOP Vehicle", System.Drawing.Color.Red);
                        }
                    }
                }

                Scan_Sensors(0, 0);
                System.Windows.Forms.Application.DoEvents();
            }

            return true;
        }

        public void Init_AllData()//데이터 초기화
        {
            TestStep = 0;

            for (int CNT = 0; CNT < TestOn.Length; CNT++)
            {
                TestOn[CNT] = false;
            }

            TSet.StopFlag = 0;
            TSet.TestStop = false;
            TSet.StepPrev = false;
            TSet.StepNext = false;

            TSet.ChkBrake = false;
            TSet.Spd_5kmh = false;
            TSet.Old_5kmh = false;
            TSet.ECU_Errs = false;

            TSet.MaxSpeed = 0;
            TSet.SMTValue = 0;

            TSet.WSS_Last = WheelDataCls(TSet.WSS_Last);
            TSet.SMT_Last = WheelDataCls(TSet.SMT_Last);

            TSet.CH_Speed = WheelDataCls(TSet.CH_Speed);
            TSet.Drag_Max = WheelDataCls(TSet.Drag_Max);
            TSet.GBrk_Max = WheelDataCls(TSet.GBrk_Max);
            TSet.BrakeMax = WheelDataCls(TSet.BrakeMax);
            TSet.Park_Max = WheelDataCls(TSet.Park_Max);

            TSet.BalanMin = WheelDataCls(TSet.BalanMin);
            TSet.BalanMax = WheelDataCls(TSet.BalanMax);

            TSet.ABSB_Min = WheelDataCls(TSet.ABSB_Min);
            TSet.ABSB_Max = WheelDataCls(TSet.ABSB_Max);

            ECUs.Ret_Data_Cls();
            Fom_Test.Refresh_Errs(0, "");
        }

        public bool Test_Running(string pTestNo)//Test 시작
        {
            double ReadTime = 0, Old_Time = 0;
            double OfstTime = DateTime.Now.Ticks;
            double Gap_Time = DateTime.Now.AddSeconds(0.1).Ticks;
            bool Flag_Onf = false;
            bool Ret_Flag = false;

            TSet.StopFlag = 0;
            TSet.TestStop = false;

            main.FomFlash.Play(8);
            main.FomFlash.VinNo_Show(TSet.CarModel, TSet.Vin___No);

            while (true)
            {
                TSet.StopFlag = TSet.TestStopFlag(TSet.StopFlag);
                if (TSet.StopFlag > 0) return false;

                if (TSet.Debug_Md) break;
                if (PLC.DI.Start__PB) break;
                if (PLC.DI.GOT_Start) break;

                ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;

                Scan_Sensors(0, 0);
                System.Windows.Forms.Application.DoEvents();
            }

            main.FomFlash.Play(100);

            Logs.Init_History(pTestNo);
            Logs.Test_History(Log_His.Info, TSet.AcceptNo);
            Logs.Test_History(Log_His.Info, TSet.Vin___No);
            Logs.Test_History(Log_His.Info, TSet.CarModel);
            Logs.Test_History(Log_His.Info, TSet.ECUModel);
            Logs.Test_History(Log_His.Info, TSet.CarBarID);
            Logs.Test_History(Log_His.Info, TSet.CarWbase);
            Logs.Test_History(Log_His.Info, TSet.CarEngin);
            Logs.Test_History(Log_His.Info, TSet.CarTranM);
            Logs.Test_History(Log_His.Info, TSet.Car_ABST);
            Logs.Test_History(Log_His.Info, TSet.CarCurve);
            Logs.Test_History(Log_His.Info, TSet.CarDrive);

            ECUs.ECU_Selector(TSet.ECUModel);

            Fom_Test.Show();
            Fom_Test.Refresh_Info();
            System.Windows.Forms.Application.DoEvents();

            PLC.Test___Ready();

            #region Move wheel base
            if (TSet.Test_RnB == true)
            {
                Logs.Test_History(Log_His.Base, TSet.CarWbase + "mm");
                Fom_Test.Refresh_Msgs("Move wheel base", System.Drawing.Color.Yellow);
                Fom_Test.Refresh_Mode("");
                Fom_Test.Refresh_Time("");
                Fom_Test.RefreshOrder("");
                Fom_Test.btnWBase.Visible = true;
                while (true)
                {
                    TSet.StopFlag = TSet.TestStopFlag(TSet.StopFlag);
                    if (TSet.StopFlag > 0) return false;

                    if (!PLC.DO.WBMoveing)      //Ready Remote Controller)
                    {
                        if (Math.Abs((PLC.OfSetL + PLC.Length) - (PLC.OfSetL + PLC.Seting)) <= 3) break;
                    }

                    ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;

                    if ((ReadTime - Old_Time) >= 1)
                    {
                        Old_Time = ReadTime;
                        Flag_Onf = !Flag_Onf;

                        if (Flag_Onf)
                        {
                            Fom_Test.Refresh_Msgs("Move wheel base", System.Drawing.Color.Red);
                            Fom_Test.btnWBase.ForeColor = Fom_Test.btnWBase.ForeColor == System.Drawing.Color.Red ? System.Drawing.Color.Lime : System.Drawing.Color.Red;
                        }
                        else
                        {
                            if (!PLC.DO.WBMoveing)
                            {
                                Fom_Test.Refresh_Msgs("START PUSH", System.Drawing.Color.Yellow);
                            }
                            else
                            {
                                Fom_Test.Refresh_Msgs("Move wheel base", System.Drawing.Color.LightYellow);
                                Fom_Test.btnWBase.ForeColor = Fom_Test.btnWBase.ForeColor == System.Drawing.Color.Red ? System.Drawing.Color.Lime : System.Drawing.Color.Red;
                            }
                        }
                    }

                    Scan_Sensors(0, 0);
                    System.Windows.Forms.Application.DoEvents();
                }
                Fom_Test.btnWBase.Visible = false;
            }
            #endregion

            if (PSet.SST_Type > 0 && TSet.Test_SST == true)  //0:측정 않음, 1:막대 그래프, 2:숫자만
            {
                fom__SST SST_Test = new fom__SST(main);
                SST_Test.BringToFront();
                SST_Test.SSTs_Running();
            }

            if (PSet.Brk_Type > 0 && TSet.Test__BT == true)
            {
                fomBrake Brk_Test = new fomBrake(main);
                Brk_Test.BringToFront();
                Brk_Test.BRKs_Running();
            }

            if (TSet.Test_RnB == true)
            {
                #region Vehicle Enter
                if (TSet.StopFlag == 0)
                {
                    Fom_Test.Refresh_Msgs("Vehicle Enter", System.Drawing.Color.White);
                    Fom_Test.Refresh_Mode("");
                    Fom_Test.Refresh_Time("");
                    Fom_Test.RefreshOrder("");

                    while (true)
                    {
                        Fom_Test.RefreshOrder("Enter Position");

                        TSet.StopFlag = TSet.TestStopFlag(TSet.StopFlag);
                        if (TSet.StopFlag > 0)
                        {
                            Ret_Flag = false;
                            break;
                            //return false;
                        }

                        if (!TSet.Debug_Md)
                        {
                            if (PLC.DI.FLF_SR_Up == true && PLC.DI.FRF_SR_Up == true &&
                                PLC.DI.FLR_SR_Up == true && PLC.DI.FRR_SR_Up == true &&
                                PLC.DI.RLR_SR_Up == true && PLC.DI.RRR_SR_Up == true &&
                                PLC.DI.PHO_Front == true && PLC.DI.PHO__Rear == true)
                            {
                                TSet.Run_Stby = true;
                            }
                            else
                            {
                                TSet.Run_Stby = false;
                            }

                            if (PLC.DO.Pos__Test)
                            {
                                Ret_Flag = true;
                                break;
                            }
                        }

                        if (PLC.DI.PHO_Front && PLC.DI.PHO__Rear)
                        {
                            if (TSet.ECUModel != "None")
                            {
                                Fom_Test.Refresh_Font(50f);
                                Fom_Test.Refresh_Msgs("ECU connect && Start pull", System.Drawing.Color.Lime);
                            }
                            else
                            {
                                Fom_Test.Refresh_Font(65f);
                                if (!TSet.Run_Stby)
                                {
                                    Fom_Test.Refresh_Msgs("Test Setting", System.Drawing.Color.Yellow);
                                }
                                else
                                {
                                    if (PLC.DI.L_Post_Dn == true && PLC.DI.R_Post_Dn == true)
                                    {
                                        Fom_Test.Refresh_Msgs("Start pull !!!", System.Drawing.Color.Lime);
                                    }
                                    else
                                    {
                                        Fom_Test.Refresh_Msgs("Test Setting", System.Drawing.Color.Yellow);
                                    }
                                }
                            }
                        }

                        Scan_Sensors(0, 0);
                        System.Windows.Forms.Application.DoEvents();
                    }
                }
                else
                {
                    Ret_Flag= false;
                    return false;
                }
                #endregion

                Fom_Test.Refresh_Font(65f);
                Fom_Test.Refresh_Msgs("Test Start", System.Drawing.Color.White);

//Retest_Run:
                if (Ret_Flag)       //리모컨 STOP 패스
                {
                    Stop_Vehicle();

                    if (TSet.StopFlag == 0)
                    {
                        PLC.Door___Close();

                        NI.Stop();
                        NI.Start();
                        RnB__Started(pTestNo);
                    }
                }

                Stop_Vehicle();
                PLC.Test__Finish();

                if (Ret_Flag)   //리모컨 STOP 패스
                {
                    //if (!PLC.DO.m_EmgS && !TSet.TestStop)
                    {
                        fom_Rslt Report = new fom_Rslt(main, 0, pTestNo);
                        Report.Results_Show(pTestNo);

                        //if (Report.Ret_Test == 5)
                        //{
                        //    if (Report != null) { Report.Close(); }
                        //    TSet.StopFlag = 0;
                        //    TSet.TestStop = false;
                        //    goto Retest_Run;
                        //}
                        if (Report != null) { Report.Close(); }
                    }
                }

                while (true)
                {
                    if (!TSet.Debug_Md)
                    {
                        if (PLC.DO.MD_Emerge) break;
                        //if (TSet.TestStop) break;
                        //if (PLC.DI.SWStop) break;
                        //if (PLC.DO.pEnter) break;
                        if (!PLC.DI.PHO_Front && !PLC.DI.PHO__Rear) break;
                    }

                    if (PLC.DI.L_Post_Dn == true && PLC.DI.R_Post_Dn == true &&
                        PLC.DI.FLF_SR_Dn == true && PLC.DI.FRF_SR_Dn == true &&
                        PLC.DI.FLR_SR_Dn == true && PLC.DI.FRR_SR_Dn == true &&
                        PLC.DI.RLR_SR_Dn == true && PLC.DI.RRR_SR_Dn == true)
                    {
                        PLC.Door____Open();
                        TSet.Out_Stby = true;
                    }
                    else
                    {
                        TSet.Out_Stby = false;
                    }

                    ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;

                    if ((ReadTime - Old_Time) >= 1)
                    {
                        Old_Time = ReadTime;
                        Flag_Onf = !Flag_Onf;

                        if (!TSet.Out_Stby)
                        {
                            if (Flag_Onf)
                            {
                                Fom_Test.Refresh_Msgs("Finish Setting", System.Drawing.Color.Yellow);
                            }
                            else
                            {
                                Fom_Test.Refresh_Msgs("Finish Setting", System.Drawing.Color.Red);
                            }
                        }
                        else
                        {
                            if (Flag_Onf)
                            {
                                Fom_Test.Refresh_Msgs("Go Out", System.Drawing.Color.Yellow);
                            }
                            else
                            {
                                Fom_Test.Refresh_Msgs("Go Out", System.Drawing.Color.Red);
                            }
                        }
                    }

                    //Scan_Sensors(0, 0);
                    System.Windows.Forms.Application.DoEvents();
                }

                Fom_Test.Close();
            }

            main.FomFlash.Play(0);
            return true;
        }

        private bool RnB__Started(string pTestNo)//Test 진행
        {
            double ReadTime = 0, Old_Time = 0, TestTime = 0, Err_Time = 0;
            double StepOfst = DateTime.Now.Ticks;
            double OfstTime = DateTime.Now.Ticks;
            double Gap_Time = DateTime.Now.AddSeconds(0.1).Ticks;
            bool Ret_ECUs = false;
            bool Max_Flag = false;
            double MaxSpeed = 0;
            int TSpeed = 0;  

            Logs.ECUsLog_File(Log_ECU.Test, pTestNo);

            TSet.ECU_Flag = false;
            TSet.ECU_Setp = 0;
            TSet.TestTime = DateTime.Now.ToString(H2Y.format0Time);  //측정 시각 시간
            int LastStep = crv_Test.G_Data.Count - 1;

            PLC.Test___Start();
            PLC.MOT_SyncTest("Free");

            Fom_Test.Init_AllData();        //화면   초기화
            Init_AllData();                 //데이터 초기화
            OfstTime = DateTime.Now.Ticks;  //시간   초기화

            Fom_Test.StartCycleTime();
            while (true)
            {

                if (TestStep >= LastStep) break;
                if (!TSet.Debug_Md)
                {
                    if (PLC.DO.MD_Emerge) { TSet.StopFlag = 1; }   //EMERGENCY
                    if (PLC.DI.PSW__Stop) { TSet.StopFlag = 2; }   //정지 스위치
                    if (TSet.TestStop)    { TSet.StopFlag = 3; }   //프로그램 정지

                    if (!PLC.DO.MD___Auto) { TSet.StopFlag = 10; }   //자동 모드
                    if (!PLC.DO.MD__Ready) { TSet.StopFlag = 11; }   //레디 모드

                    if (PLC.DI.FL_MotErr) { TSet.StopFlag = 21; }   //FL   Motor
                    if (PLC.DI.FR_MotErr) { TSet.StopFlag = 22; }   //FR   Motor
                    if (PLC.DI.RL_MotErr) { TSet.StopFlag = 23; }   //RL   Motor
                    if (PLC.DI.RR_MotErr) { TSet.StopFlag = 24; }   //RR   Motor

                    if (TSet.StopFlag > 0) break;
                }

                ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;
                TestTime = ReadTime - Err_Time;
                TSet.StepTime = ReadTime - StepOfst;

                if (TSet.MaxSpeed < Math.Round(TSet.SpeedStd, 1))
                {
                    TSet.MaxSpeed = Math.Round(TSet.SpeedStd, 1);
                }

                #region Test Step 변경시 동작
                if (TestStep == 0 || Old_Step != TestStep)
                {
                    if (TestStep >= LastStep) { TestStep = LastStep; }
                    Fom_Test.Refresh_Msgs(crv_Test.G_Data[TestStep].Items, System.Drawing.Color.White);

                    TSpeed = crv_Test.G_Data[TestStep].Speed;

                    Old_Step = TestStep;
                    StepOfst = ReadTime;
                    Fom_Test.Pedal_Onf = false;
                    Fom_Test.Data_show = 0;
                    Fom_Test.RefreshOrder("");

                    TSet.CH_Speed.FL = NI.Loss.FL.Speed;
                    TSet.CH_Speed.FR = NI.Loss.FR.Speed;
                    TSet.CH_Speed.RL = NI.Loss.RL.Speed;
                    TSet.CH_Speed.RR = NI.Loss.RR.Speed;

                    switch (crv_Test.G_Data[TestStep].Segment.ToString().ToUpper())
                    {
                        //
                        case "BATTERY VOLTAGE":
                            Ret_ECUs = CHERY1BOX.Read_BatteryVoltage();
                            Fom_Test.RefreshOrder("Read Battery Voltage : " + CHERY1BOX.Voltage);
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                            break;

                        case "VPC CALIBRATION":
                            CHERY1BOX.Comfort_Pulse();
                            Fom_Test.RefreshOrder("Comfort Pulse Calibration");
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                            break;

                        case "PLUNGER TEST":
                            CHERY1BOX.LeakageAndAirTest();
                            Fom_Test.RefreshOrder("Plunger test");
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                            break;

                        case "BRAKE CONDITIONING":
                            CHERY1BOX.BrakeConditioningTest();
                            Fom_Test.RefreshOrder("Comfort Pulse Calibration");
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                            break;

                        case "TMC WITHOUT PFS":
                            CHERY1BOX.TMC_Without_PFS_Test();
                            Fom_Test.RefreshOrder("Brake test with TMC and without PFS");
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                            break;

                        case "TMC WITH PFS":
                            CHERY1BOX.TMC_With_PFS_Test();
                            Fom_Test.RefreshOrder("Brake test with TMC and with PFS");
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                            break;

                        case "MASTER CYLINDER":
                            CHERY1BOX.MasterCylinder_Test();
                            Fom_Test.RefreshOrder("Onebox Tandem Master Cylinder");
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                            break;

                        case "EOL PROCESS":

                            if (ECUs.ECU == ECUs.Chery_1box)
                            {
                                main.CheryEchoThread.SendEcho(false);
                            }

                            H2Y.Sleep(300);
                            CHERY1BOX.Write_EOLProcessByte();
                            Fom_Test.RefreshOrder("Write EOL Process Byte");
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                            break;
                       
                        case "ACTIVATION":
                            CHERY1BOX.SpeedLimitedActivation();
                            Fom_Test.RefreshOrder("Speed limited activation");
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                            break;
                        case "DEACTIVATION":
                            CHERY1BOX.SpeedLimitedDeactivation();
                            PLC.MOT_PowerOnf(3, 0);  //구동 신호
                            Fom_Test.RefreshOrder("Speed limited deactivation");
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                            break;
                        case "TEST PRESENT":
                            for (int ni = 0; ni < 14; ni++)
                            {
                                CHERY1BOX.Tester_Present();
                                H2Y.Sleep(500);
                            }
                            Fom_Test.RefreshOrder("Tester_Present");
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                            break;
                        case "WSS READY":   //"WSS TEST":  
                            
                            if (ECUs.ECU == ECUs.Chery_1box)
                            {
                                CHERY1BOX.Start_WSS_Test();
                            }
                            else
                            {
                                PLC.MOT_PowerOnf(3, 0);  //구동 신호
                                Ret_ECUs = Start_Communication(this);
                            }
                            H2Y.Sleep(100);
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                            break;

                        case "WSS TEST":
                            Fom_Test.Data_show = 2;
                            //CHERY1BOX.Start_WSS_Test();
                            Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);

                            if (ECUs.ECU == ECUs.Chery_1box)
                            {
                                PLC.MOT_PowerOnf(0, 0);  //구동 신호
                            }
                            break;

                        case "SWITCH":
                             PLC.MOT_SyncTest("Free");
                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                             break;

                        case "MODE CHECK":
                             PLC.MOT_SyncTest(TSet.CarDrive);    //동기 검사 시작

                             TSet.Ofst_MFL = NI.Loss.FL.Move;
                             TSet.Ofst_MFR = NI.Loss.FR.Move;
                             TSet.Ofst_MRL = NI.Loss.RL.Move;
                             TSet.Ofst_MRR = NI.Loss.RR.Move;

                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                             break;

                        case "MAX SPEED":
                             Fom_Test.RefreshOrder(crv_Test.G_Data[TestStep].Description);
                             break;

                        case "ENGINE BRAKING":
                             PLC.MOT_SyncTest("Free");
                             TSet.ChkBrake = true;
                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                             break;
                             
                        case "[N] STOP":
                             PLC.MOT_SyncTest("Free");

                             TSet.Ofst_MFL = NI.Loss.FL.Move;
                             TSet.Ofst_MFR = NI.Loss.FR.Move;
                             TSet.Ofst_MRL = NI.Loss.RL.Move;
                             TSet.Ofst_MRR = NI.Loss.RR.Move;

                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                             break;

                        case "IDLE":
                             PLC.MOT_SyncTest("Free");

                             TSet.Ofst_MFL = NI.Loss.FL.Move;
                             TSet.Ofst_MFR = NI.Loss.FR.Move;
                             TSet.Ofst_MRL = NI.Loss.RL.Move;
                             TSet.Ofst_MRR = NI.Loss.RR.Move;
                             break;

                        case "BRAKE TEST":
                             Fom_Test.Pedal_Onf = true;
                             //Fom_Test.Brake_Onf = true;
                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                             break;

                        case "DYNAMIC READY":
                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                             break;

                        case "REVERSE":
                             PLC.MOT_SyncTest(TSet.CarDrive);    //동기 후진
                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                             break;

                        case "PARKING READY":
                             TSet.Ofst_MFL = NI.Loss.FL.Move;
                             TSet.Ofst_MFR = NI.Loss.FR.Move;
                             TSet.Ofst_MRL = NI.Loss.RL.Move;
                             TSet.Ofst_MRR = NI.Loss.RR.Move;

                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                             break;

                        case "PARKING TEST":
                             PLC.MOT_ParkTest(TSet.CarDrive);

                             TSet.Ofst_MFL = NI.Loss.FL.Move;
                             TSet.Ofst_MFR = NI.Loss.FR.Move;
                             TSet.Ofst_MRL = NI.Loss.RL.Move;
                             TSet.Ofst_MRR = NI.Loss.RR.Move;

                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                             break;

                        case "APB FUNCTION":
                             PLC.MOT_ParkTest(TSet.CarDrive);

                             TSet.Ofst_MFL = NI.Loss.FL.Move;
                             TSet.Ofst_MFR = NI.Loss.FR.Move;
                             TSet.Ofst_MRL = NI.Loss.RL.Move;
                             TSet.Ofst_MRR = NI.Loss.RR.Move;

                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment);
                             break;

                        case "START COMMUNICATION":
Retest_COMMUNICATION:
                             Ret_ECUs = Start_Communication(this);
                             Fom_Test.Data_show = 1;
                             Fom_Test.Judge___Msgs(Ret_ECUs ? H2Y.OK : H2Y.NG);
                             Fom_Test.RefreshOrder(NeoVI.Get_Data);

                             if (!TSet.Debug_Md)
                             {
                                 if (!Ret_ECUs)
                                 {
                                     fom__ASK Ask = new fom__ASK(main);

                                     int Ret = Ask.Ret_Message();
                                     if (Ret == 2)
                                     {
                                         TSet.TestStop = true;
                                     }
                                     else
                                     {
                                         goto Retest_COMMUNICATION;
                                     }
                                 }
                             }

                             if (!Ret_ECUs) { Fom_Test.Refresh_Errs(1, crv_Test.G_Data[TestStep].Segment.ToString()); }
                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment + " " + (Ret_ECUs ? H2Y.OK : H2Y.NG));
                             break;

                        case "ECU IDENTIFICATION":
                             Ret_ECUs = ECU_Identification(this);
                             Fom_Test.Data_show = 1;
                             Fom_Test.Judge___Msgs(Ret_ECUs ? H2Y.OK : H2Y.NG);
                             Fom_Test.RefreshOrder(NeoVI.Get_Data);
                             if (!Ret_ECUs) { Fom_Test.Refresh_Errs(1, crv_Test.G_Data[TestStep].Segment.ToString()); }
                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment + " " + (Ret_ECUs ? H2Y.OK : H2Y.NG));
                             break;

                        case "CHECK SIGNAL":
                             Ret_ECUs = Read_Signals(this);
                             Fom_Test.Data_show = 1;
                             Fom_Test.Judge___Msgs(Ret_ECUs ? H2Y.OK : H2Y.NG);
                             Fom_Test.RefreshOrder(NeoVI.Get_Data);
                             if (!Ret_ECUs) { Fom_Test.Refresh_Errs(1, crv_Test.G_Data[TestStep].Segment.ToString()); }
                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment + " " + (Ret_ECUs ? H2Y.OK : H2Y.NG));
                             H2Y.Sleep(100);
                             if (ECUs.ECU == ECUs.Chery_1box)
                             {
                                 main.CheryEchoThread.SendEcho(true);
                             }
                             break;

                        case "DYNAMIC TEST":
                             TSet.ECU_Flag = false;
                             TSet.ECU_Setp = 0;
                             TSet.ECU_Ofst = DateTime.Now.Ticks;
                             TSet.ABSvStep = TestStep;                         
                             //Ret_ECUs = Dynamic_Test(this);
                             //Fom_Test.Data_show = 1;
                             //Fom_Test.Judge___Msgs(Ret_ECUs ? H2Y.OK : H2Y.NG);

                             if (ECUs.ECU == ECUs.Chery_1box)
                             {
                                 main.CheryEchoThread.SendEcho(false);
                             }

                             if (!Ret_ECUs) { Fom_Test.Refresh_Errs(1, crv_Test.G_Data[TestStep].Segment.ToString()); }
                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment + " " + (Ret_ECUs ? H2Y.OK : H2Y.NG));
                             break;

                        case "DTC READ":
                             Ret_ECUs = Read__DTC(this);
                             Fom_Test.Data_show = 1;
                             Fom_Test.Judge___Msgs(Ret_ECUs ? H2Y.OK : H2Y.NG);
                             Fom_Test.RefreshOrder(NeoVI.Get_Data);
                             if (!Ret_ECUs) { Fom_Test.Refresh_Errs(1, crv_Test.G_Data[TestStep].Segment.ToString()); }
                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment + " " + (Ret_ECUs ? H2Y.OK : H2Y.NG));
                             break;

                        case "DTC CLEAR":
                             Ret_ECUs = Clear_DTC(this);
                             Fom_Test.Data_show = 1;
                             Fom_Test.Judge___Msgs(Ret_ECUs ? H2Y.OK : H2Y.NG);
                             Fom_Test.RefreshOrder(NeoVI.Get_Data);
                             if (!Ret_ECUs) { Fom_Test.Refresh_Errs(1, crv_Test.G_Data[TestStep].Segment.ToString()); }
                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment + " " + (Ret_ECUs ? H2Y.OK : H2Y.NG));
                             break;

                        case "STOP COMMUNICATION":
                             Ret_ECUs = Stop_Communication(this);
                             Fom_Test.Data_show = 1;
                             Fom_Test.Judge___Msgs(Ret_ECUs ? H2Y.OK : H2Y.NG);
                             Fom_Test.RefreshOrder(NeoVI.Get_Data);
                             if (!Ret_ECUs) { Fom_Test.Refresh_Errs(1, crv_Test.G_Data[TestStep].Segment.ToString()); }
                             Logs.MakeLog_File(Log_His.Step, crv_Test.G_Data[TestStep].Segment + " " + (Ret_ECUs ? H2Y.OK : H2Y.NG));
                             break;
                    }
                }
                #endregion

                if (!TSet.Debug_Md)
                {
                    #region Test Step 변경 조건
                    if (crv_Test.G_Data[TestStep].Vehicle.ToString().ToUpper() == "X")  //차량 Free, 롤 조건 구동
                    {
                        if (crv_Test.G_Data[TestStep].Speed == 0)
                        {
                            if ((NI.Loss.FL.Speed < PSet.Stop_Spd) &&
                                (NI.Loss.FR.Speed < PSet.Stop_Spd) &&
                                (NI.Loss.RL.Speed < PSet.Stop_Spd) &&
                                (NI.Loss.RR.Speed < PSet.Stop_Spd))
                            {
                                if (crv_Test.G_Data[TestStep].Time < TestTime)
                                {
                                    if (TestStep > 0)
                                    {
                                        Err_Time = Ret_StepTime(ReadTime, TestStep);
                                    }

                                    TestStep++;

                                    TSet.StepPrev = false;
                                    TSet.StepNext = false;
                                }
                            }
                        }
                        else
                        {
                            if (crv_Test.G_Data[TestStep].Time < TestTime)
                            {
                                if (TestStep > 0)
                                {
                                    Err_Time = Ret_StepTime(ReadTime, TestStep);
                                }

                                TestStep++;

                                TSet.StepPrev = false;
                                TSet.StepNext = false;
                            }
                        }
                    }
                    else
                    {
                        if (crv_Test.G_Data[TestStep].Speed == 0)
                        {
                            if ((NI.Loss.FL.Speed < PSet.Stop_Spd) &&
                                (NI.Loss.FR.Speed < PSet.Stop_Spd) &&
                                (NI.Loss.RL.Speed < PSet.Stop_Spd) &&
                                (NI.Loss.RR.Speed < PSet.Stop_Spd))
                            {
                                if (crv_Test.G_Data[TestStep].Time < TestTime)
                                {
                                    if (TestStep > 0)
                                    {
                                        Err_Time = Ret_StepTime(ReadTime, TestStep);
                                    }

                                    TestStep++;

                                    TSet.StepPrev = false;
                                    TSet.StepNext = false;
                                }
                            }
                            else
                            {
                                Err_Time = Ret_StepTime(ReadTime, TestStep);
                            }
                        }
                        else
                        {
                            #region 차량 구동
                            switch (crv_Test.G_Data[TestStep].Segment.ToString().ToUpper())
                            {
                                #region SPEED UP
                                case "SPEED UP":
                                    if (PSet.RnB.SpeedMin - 1 < TSet.SpeedStd)
                                    {
                                        if (TestStep > 0)
                                        {
                                            Err_Time = Ret_StepTime(ReadTime, TestStep);
                                        }

                                        TestStep++;

                                        TSet.StepPrev = false;
                                        TSet.StepNext = false;
                                    }
                                    break;
                                #endregion

                                #region SMT TEST
                                case "SMT TEST":
                                    if (PLC.DI.PSW_Check)
                                    {
                                        TSet.SMTValue = Math.Round(TSet.SpeedStd, 1);

                                        Fom_Test.Data_show = 1;
                                        if (H2Y.Ret_JudgeOX(PSet.RnB.SpeedMin, PSet.RnB.SpeedMax, TSet.SMTValue) == H2Y.OK)
                                        {
                                            Fom_Test.Judge___Msgs(H2Y.OK);
                                            Fom_Test.Refresh_Msgs(TSet.SMTValue.ToString("#0.0") + " " + H2Y.OK, System.Drawing.Color.Lime);
                                        }
                                        else
                                        {
                                            Fom_Test.Judge___Msgs(H2Y.NG);
                                            Fom_Test.Refresh_Msgs(TSet.SMTValue.ToString("#0.0") + " " + H2Y.NG, System.Drawing.Color.Red);
                                        }

                                        if (TestStep > 0)
                                        {
                                            Err_Time = Ret_StepTime(ReadTime, TestStep);
                                        }

                                        TestStep++;

                                        TSet.StepPrev = false;
                                        TSet.StepNext = false;
                                    }
                                    break;
                                #endregion

                                #region MAX SPEED
                                case "MAX SPEED":
                                    if (Max_Flag == true)
                                    {
                                        Fom_Test.Refresh_Msgs(crv_Test.G_Data[TestStep + 1].Description, System.Drawing.Color.White);
                                    }
                                    else
                                    {
                                        Fom_Test.Refresh_Msgs(TSet.SpeedStd.ToString("#0.0") + " km/h", System.Drawing.Color.Yellow);
                                    }

                                    if (MaxSpeed < TSet.SpeedStd) { MaxSpeed = TSet.SpeedStd; }
                                    if (TSpeed < TSet.SpeedStd) { Max_Flag = true; }

                                    if ((Max_Flag == true) && (MaxSpeed - 2 > TSet.SpeedStd))
                                    {
                                        if (TestStep > 0)
                                        {
                                            Err_Time = Ret_StepTime(ReadTime, TestStep);
                                        }

                                        TestStep++;

                                        TSet.StepPrev = false;
                                        TSet.StepNext = false;
                                        MaxSpeed = 0;
                                        Max_Flag = false;
                                    }
                                    break;
                                #endregion

                                #region ENGINE BRAKING
                                case "ENGINE BRAKING":
                                    if (TSpeed > TSet.SpeedStd)
                                    {
                                        Fom_Test.Refresh_Msgs(crv_Test.G_Data[TestStep + 1].Description, System.Drawing.Color.White);

                                        if (TestStep > 0)
                                        {
                                            Err_Time = Ret_StepTime(ReadTime, TestStep);
                                        }

                                        TestStep++;

                                        TSet.StepPrev = false;
                                        TSet.StepNext = false;
                                    }
                                    break;
                                #endregion

                                #region DRAG TEST
                                case "DRAG TEST":
                                    if (TSpeed > TSet.SpeedStd)
                                    {
                                        Fom_Test.Refresh_Msgs(crv_Test.G_Data[TestStep + 1].Description, System.Drawing.Color.White);

                                        if (TestStep > 0)
                                        {
                                            Err_Time = Ret_StepTime(ReadTime, TestStep);
                                        }

                                        TestStep++;

                                        TSet.StepPrev = false;
                                        TSet.StepNext = false;

                                        Fom_Test.Data_show = 1;
                                        Fom_Test.Judge___Msgs(TSet.Drag_Max.OX);
                                    }
                                    break;
                                #endregion

                                #region BRAKE TEST
                                case "BRAKE TEST":
                                    Fom_Test.Refresh_Msgs(TSet.SpeedStd.ToString("#0.0") + " km/h", System.Drawing.Color.Yellow);

                                    if (TSpeed > TSet.SpeedStd)
                                    {
                                        Fom_Test.Refresh_Msgs(crv_Test.G_Data[TestStep + 1].Description, System.Drawing.Color.White);

                                        if (TestStep > 0)
                                        {
                                            Err_Time = Ret_StepTime(ReadTime, TestStep);
                                        }

                                        TestStep++;

                                        TSet.StepPrev = false;
                                        TSet.StepNext = false;

                                        Fom_Test.Data_show = 1;
                                        Fom_Test.Judge___Msgs(TSet.GBrk_Max.OX);
                                    }
                                    break;
                                #endregion

                                #region DYNAMIC READY
                                case "DYNAMIC READY":
                                    if (TSpeed > TSet.SpeedStd)
                                    {
                                        Fom_Test.Refresh_Msgs(crv_Test.G_Data[TestStep + 1].Description, System.Drawing.Color.White);

                                        if (TestStep > 0)
                                        {
                                            Err_Time = Ret_StepTime(ReadTime, TestStep);
                                        }

                                        TestStep++;

                                        TSet.StepPrev = false;
                                        TSet.StepNext = false;
                                    }
                                    break;
                                #endregion

                                #region DYNAMIC TEST
                                case "DYNAMIC TEST":
                                    TSet.ECU_Flag = false;

                                    if (ECUs.ECU == ECUs.Chery_1box)
                                    {
                                        Ret_ECUs = Dynamic_Chery();
                                    }
                                    else
                                    {
                                        Ret_ECUs = Dynamic_Test(this);
                                    }
                                    //Fom_Test.Data_show = 1;
                                    //Fom_Test.Judge___Msgs(Ret_ECUs ? H2Y.OK : H2Y.NG);

                                    if ((ECUs.ABS_Step == 5) && (crv_Test.G_Data[TestStep].Time < TestTime))
                                    {
                                        if (TestStep > 0)
                                        {
                                            Err_Time = Ret_StepTime(ReadTime, TestStep);
                                        }

                                        TestStep++;

                                        TSet.StepPrev = false;
                                        TSet.StepNext = false;
                                        if (ECUs.ECU == ECUs.Chery_1box)
                                        {
                                            main.CheryEchoThread.SendEcho(true);
                                        }
                                    }
                                    break;
                                #endregion

                                #region [R] 5KPH
                                case "[R] 5KPH":
                                    if (MaxSpeed < TSet.SpeedStd) { MaxSpeed = TSet.SpeedStd; }
                                    if (TSpeed < TSet.SpeedStd) { Max_Flag = true; }

                                    if (Max_Flag == true)
                                    {
                                        Fom_Test.Refresh_Msgs(crv_Test.G_Data[TestStep + 1].Description, System.Drawing.Color.White);
                                    }

                                    if ((Max_Flag == true) && (MaxSpeed - 2 > TSet.SpeedStd))
                                    {
                                        TSet.ReverseV = Math.Round(MaxSpeed, 1);

                                        if (TestStep > 0)
                                        {
                                            Err_Time = Ret_StepTime(ReadTime, TestStep);
                                        }

                                        TestStep++;

                                        TSet.StepPrev = false;
                                        TSet.StepNext = false;
                                        MaxSpeed = 0;
                                        Max_Flag = false;
                                    }
                                    break;
                                #endregion

                                #region 기타
                                default:
                                    if (((TSpeed - 1) < TSet.SpeedStd && TSet.SpeedStd < (TSpeed + 1)) || (crv_Test.G_Data[TestStep].Time < TestTime))
                                    {
                                        if (TestStep > 0)
                                        {
                                            Err_Time = Ret_StepTime(ReadTime, TestStep);
                                        }

                                        TestStep++;

                                        TSet.StepPrev = false;
                                        TSet.StepNext = false;
                                    }
                                    break;
                                #endregion
                            }
                            #endregion  
                        }

                        if (TestStep >= LastStep) { TestStep = LastStep; }
                    }
                    #endregion
                }
                else
                {
                    if (crv_Test.G_Data[TestStep].Time < TestTime)
                    {
                        if (TestStep > 0)
                        {
                            Err_Time = Ret_StepTime(ReadTime, TestStep);
                        }

                        TestStep++;

                        TSet.StepPrev = false;
                        TSet.StepNext = false;
                    }
                }

                Scan_Sensors(TestStep, TSet.StepTime);

                System.Windows.Forms.Application.DoEvents();

                if ((ReadTime - Old_Time) >= 0.1)
                {
                    Old_Time = ReadTime;
                   // Fom_Test.Refresh_Time(ReadTime.ToString("0.00"));
                }
            }

            PLC.MOT_SyncTest("Free");
            Logs.ECUsLog_File(Log_ECU.TEnd, pTestNo);

            //if (TSet.StopFlag == 0)
            {
                TestDataSave(pTestNo);
            }


            Fom_Test.StartCycleTime();
            return true;
        }
        
        private double Ret_StepTime(double ReadTime, int TestStep)
        {
            try
            {
                return ReadTime;    //-crv_Test.G_Data[TestStep].T_Time; ;
            }
            catch(Exception ex)
            {
                return ReadTime;
            }
        }

        private void Test_LogSave(double pTime, Log_His mode)//Test Log 데이터
        {
            string str_Data = "";

            if (pTime < 0) { pTime = 0; }
            
            str_Data += H2Y.DigitToStr(NI.Loss.FL.Speed.ToString("#0.0000"), 12);
            str_Data += ",";
            str_Data += H2Y.DigitToStr(NI.Loss.FR.Speed.ToString("#0.0000"), 12);
            str_Data += ",";
            str_Data += H2Y.DigitToStr(NI.Loss.RL.Speed.ToString("#0.0000"), 12);
            str_Data += ",";
            str_Data += H2Y.DigitToStr(NI.Loss.RR.Speed.ToString("#0.0000"), 12);

            if (old_Data == str_Data) { return; }
            old_Data = str_Data;

            str_Data = H2Y.DigitToStr(pTime.ToString("#0.0000"), 10) + "," + str_Data;

            Logs.Test_History(mode, str_Data);
        }

        public void Scan_Sensors(int pStep, double StepT)
        {
            try
            {
                TSet.Scan_Sensors();

                //기준 속도 정의
                switch (PSet.OwnerS01)
                {
                    case 0: TSet.SpeedStd = TSet.Read__FL; break;
                    case 1: TSet.SpeedStd = TSet.Read__FR; break;
                    case 2: TSet.SpeedStd = TSet.Read__RL; break;
                    case 3: TSet.SpeedStd = TSet.Read__RR; break;
                    case 4: TSet.SpeedStd = H2Y.DVD(TSet.Read__FL + TSet.Read__FR, 2); break;
                    case 5: TSet.SpeedStd = H2Y.DVD(TSet.Read__RL + TSet.Read__RR, 2); break;
                    case 6: TSet.SpeedStd = H2Y.DVD(TSet.Read__FL + TSet.Read__FR + TSet.Read__RL + TSet.Read__RR, 4); break;
                    default: TSet.SpeedStd = TSet.Read__FR; break;
                }

                TSet.Bal___FL = TSet.Read__FL;
                TSet.Bal___FR = TSet.Read__FR;
                TSet.Bal___RL = TSet.Read__RL;
                TSet.Bal___RR = TSet.Read__RR;

                #region 구간 별 데이터 }
                switch (crv_Test.G_Data[TestStep].Segment.ToString().ToUpper())
                {
                    case "WSS TEST": TSet.WSS__Onf = !TSet.WSS__Onf;

                        ECUs.WSS_Test();
                        H2Y.Sleep(100);
                        
                        TSet.WSS_Last.FL = ECUs.WSS_FL;
                        TSet.WSS_Last.FR = ECUs.WSS_FR;
                        TSet.WSS_Last.RL = ECUs.WSS_RL;
                        TSet.WSS_Last.RR = ECUs.WSS_RR;

                        if ((PSet.ECU.WSSFLMin < TSet.WSS_Last.FL && TSet.WSS_Last.FL < PSet.ECU.WSSFLMax) &&
                            (PSet.ECU.WSSFRMin < TSet.WSS_Last.FR && TSet.WSS_Last.FR < PSet.ECU.WSSFRMax) &&
                            (PSet.ECU.WSSRLMin < TSet.WSS_Last.RL && TSet.WSS_Last.RL < PSet.ECU.WSSRLMax) &&
                            (PSet.ECU.WSSRRMin < TSet.WSS_Last.RR && TSet.WSS_Last.RR < PSet.ECU.WSSRRMax))
                        {
                            TSet.WSS_Last.OX = H2Y.OK;
                        }
                        else
                        {
                            TSet.WSS_Last.OX = H2Y.NG;
                        }

                        Test_LogSave(StepT, Log_His.WSS_);              //Test Log 데이터
                        main.Test_Results("WSS_", TSet.WSS_Last); break;

                    case "SMT TEST": TSet.SpeedOnf = !TSet.SpeedOnf;

                        Test_LogSave(StepT, Log_His.Sped);              //Test Log 데이터
                        main.Test_Results("Sped", TSet.SMT_Last); break;

                    case "DRAG TEST": TSet.Drag_Onf = !TSet.Drag_Onf;

                        TSet.Drag_Max = Ret_Max_Data(TSet.Drag_Max, NI.Loss);

                        if ((PSet.RnB.DragFMin < TSet.Drag_Max.FL && TSet.Drag_Max.FL < PSet.RnB.DragFMax) &&
                            (PSet.RnB.DragFMin < TSet.Drag_Max.FR && TSet.Drag_Max.FR < PSet.RnB.DragFMax) &&
                            (PSet.RnB.DragRMin < TSet.Drag_Max.RL && TSet.Drag_Max.RL < PSet.RnB.DragRMax) &&
                            (PSet.RnB.DragRMin < TSet.Drag_Max.RR && TSet.Drag_Max.RR < PSet.RnB.DragRMax))
                        {
                            TSet.Drag_Max.OX = H2Y.OK;
                        }
                        else
                        {
                            TSet.Drag_Max.OX = H2Y.NG;
                        }

                        Test_LogSave(StepT, Log_His.Drag);
                        main.Test_Results("Drag", TSet.Drag_Max); break;

                    case "BRAKE TEST": TSet.BrakeOnf = !TSet.BrakeOnf;

                        TSet.BrakeMax = Ret_Max_Data(TSet.BrakeMax, NI.Loss);

                        TSet.GBrk_Max.FL = TSet.Drag_Max.FL + TSet.BrakeMax.FL;
                        TSet.GBrk_Max.FR = TSet.Drag_Max.FR + TSet.BrakeMax.FR;
                        TSet.GBrk_Max.RL = TSet.Drag_Max.RL + TSet.BrakeMax.RL;
                        TSet.GBrk_Max.RR = TSet.Drag_Max.RR + TSet.BrakeMax.RR;

                        if ((PSet.RnB.Brk_FMin < TSet.GBrk_Max.FL && TSet.GBrk_Max.FL < PSet.RnB.Brk_FMax) &&
                            (PSet.RnB.Brk_FMin < TSet.GBrk_Max.FR && TSet.GBrk_Max.FR < PSet.RnB.Brk_FMax) &&
                            (PSet.RnB.Brk_RMin < TSet.GBrk_Max.RL && TSet.GBrk_Max.RL < PSet.RnB.Brk_RMax) &&
                            (PSet.RnB.Brk_RMin < TSet.GBrk_Max.RR && TSet.GBrk_Max.RR < PSet.RnB.Brk_RMax))
                        {
                            TSet.GBrk_Max.OX = H2Y.OK;
                        }
                        else
                        {
                            TSet.GBrk_Max.OX = H2Y.NG;
                        }

                        Test_LogSave(StepT, Log_His.Brak);
                        main.Test_Results("Brake", TSet.GBrk_Max); break;

                    case "DYNAMIC TEST": TSet.ABSv_Onf = !TSet.ABSv_Onf;
                        TSet.ABSB_Min = Ret_Min_Data(TSet.ABSB_Min, NI.Loss);
                        TSet.ABSB_Max = Ret_Max_Data(TSet.ABSB_Max, NI.Loss);

                        //if (ECUs.ABS_Step == 1)
                        {
                            if (TSet.ABSB_Min.FL <= 1) { TSet.ABSB_Min.FL = Math.Round(NI.Loss.FL.Kgf, 0); }

                            if (TSet.ABSB_Min.FL >= NI.Loss.FL.Kgf) { TSet.ABSB_Min.FL = Math.Round(NI.Loss.FL.Kgf, 0); }
                            if (TSet.ABSB_Max.FL <= NI.Loss.FL.Kgf) { TSet.ABSB_Max.FL = Math.Round(NI.Loss.FL.Kgf, 0); }
                        }
                        //else if (ECUs.ABS_Step == 2)
                        {
                            if (TSet.ABSB_Min.FR <= 1) { TSet.ABSB_Min.FR = Math.Round(NI.Loss.FR.Kgf, 0); }

                            if (TSet.ABSB_Min.FR >= NI.Loss.FR.Kgf) { TSet.ABSB_Min.FR = Math.Round(NI.Loss.FR.Kgf, 0); }
                            if (TSet.ABSB_Max.FR <= NI.Loss.FR.Kgf) { TSet.ABSB_Max.FR = Math.Round(NI.Loss.FR.Kgf, 0); }
                        }
                        //else if (ECUs.ABS_Step == 3)
                        {
                            if (TSet.ABSB_Min.RL <= 1) { TSet.ABSB_Min.RL = Math.Round(NI.Loss.RL.Kgf, 0); }

                            if (TSet.ABSB_Min.RL >= NI.Loss.RL.Kgf) { TSet.ABSB_Min.RL = Math.Round(NI.Loss.RL.Kgf, 0); }
                            if (TSet.ABSB_Max.RL <= NI.Loss.RL.Kgf) { TSet.ABSB_Max.RL = Math.Round(NI.Loss.RL.Kgf, 0); }
                        }
                        //else if (ECUs.ABS_Step == 4)
                        {
                            if (TSet.ABSB_Min.RR <= 1) { TSet.ABSB_Min.RR = Math.Round(NI.Loss.RR.Kgf, 0); }

                            if (TSet.ABSB_Min.RR >= NI.Loss.RR.Kgf) { TSet.ABSB_Min.RR = Math.Round(NI.Loss.RR.Kgf, 0); }
                            if (TSet.ABSB_Max.RR <= NI.Loss.RR.Kgf) { TSet.ABSB_Max.RR = Math.Round(NI.Loss.RR.Kgf, 0); }
                        }

                        main.Test_Results("Dec.", TSet.ABSB_Min);
                        main.Test_Results("Inc.", TSet.ABSB_Max); 
                        break;

                    case "PARKING TEST": TSet.Park_Onf = !TSet.Park_Onf;

                        TSet.Move__FL = (NI.Loss.FL.Move - TSet.Ofst_MFL) / 10;
                        TSet.Move__FR = (NI.Loss.FR.Move - TSet.Ofst_MFR) / 10;
                        TSet.Move__RL = (NI.Loss.RL.Move - TSet.Ofst_MRL) / 10;
                        TSet.Move__RR = (NI.Loss.RR.Move - TSet.Ofst_MRR) / 10;

                        TSet.Park_Max = Ret_Max_Park(TSet.Park_Max, NI.Loss);

                        Test_LogSave(StepT, Log_His.Park);
                        main.Test_Results("Park", TSet.Park_Max); break;

                    case "APB FUNCTION": TSet.Park_Onf = !TSet.Park_Onf;

                        TSet.Move__FL = (NI.Loss.FL.Move - TSet.Ofst_MFL) / 10;
                        TSet.Move__FR = (NI.Loss.FR.Move - TSet.Ofst_MFR) / 10;
                        TSet.Move__RL = (NI.Loss.RL.Move - TSet.Ofst_MRL) / 10;
                        TSet.Move__RR = (NI.Loss.RR.Move - TSet.Ofst_MRR) / 10;

                        TSet.Park_Max = Ret_Max_Park(TSet.Park_Max, NI.Loss);

                        Test_LogSave(StepT, Log_His.Park);
                        main.Test_Results("Park", TSet.Park_Max); break;

                    default: Test_LogSave(StepT, Log_His.Gaph); break;
                }
                #endregion

                main.Test_LogData();    //Main 화면에 데이터 표시

                Fom_Test.Refresh_Data(pStep);
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Scan_Sensors: " + ex.Message);
            }
        }

        private TSet.wheel_Test WheelDataCls(TSet.wheel_Test kind)
        {
            kind.FL = -1; kind.FR = -1; kind.RL = -1; kind.RR = -1; kind.OX = "";

            return kind;
        }

        public TSet.wheel_Test Ret_Min_Data(TSet.wheel_Test Axle, NI.Scan_Data Data)//각 바퀴의 최소/최대값
        {
            int point = 0;

            if (Axle.FL == -1) { Axle.FL = Math.Round(Data.FL.Kgf, point); }
            if (Axle.FR == -1) { Axle.FR = Math.Round(Data.FR.Kgf, point); }
            if (Axle.RL == -1) { Axle.RL = Math.Round(Data.RL.Kgf, point); }
            if (Axle.RR == -1) { Axle.RR = Math.Round(Data.RR.Kgf, point); }

            if (Axle.FL >= Data.FL.Kgf) { Axle.FL = Math.Round(Data.FL.Kgf, point); }
            if (Axle.FR >= Data.FR.Kgf) { Axle.FR = Math.Round(Data.FR.Kgf, point); }
            if (Axle.RL >= Data.RL.Kgf) { Axle.RL = Math.Round(Data.RL.Kgf, point); }
            if (Axle.RR >= Data.RR.Kgf) { Axle.RR = Math.Round(Data.RR.Kgf, point); }

            return Axle;
        }

        public TSet.wheel_Test Ret_Max_Data(TSet.wheel_Test Axle, NI.Scan_Data Data)//각 바퀴의 최소/최대값
        {
            int point = 0;

            if (Axle.FL == -1) { Axle.FL = Math.Round(Data.FL.Kgf, point); }
            if (Axle.FR == -1) { Axle.FR = Math.Round(Data.FR.Kgf, point); }
            if (Axle.RL == -1) { Axle.RL = Math.Round(Data.RL.Kgf, point); }
            if (Axle.RR == -1) { Axle.RR = Math.Round(Data.RR.Kgf, point); }

            if (Axle.FL <= Data.FL.Kgf) { Axle.FL = Math.Round(Data.FL.Kgf, point); }
            if (Axle.FR <= Data.FR.Kgf) { Axle.FR = Math.Round(Data.FR.Kgf, point); }
            if (Axle.RL <= Data.RL.Kgf) { Axle.RL = Math.Round(Data.RL.Kgf, point); }
            if (Axle.RR <= Data.RR.Kgf) { Axle.RR = Math.Round(Data.RR.Kgf, point); }

            return Axle;
        }
        
        public TSet.wheel_Test Ret_Max_Park(TSet.wheel_Test Axle, NI.Scan_Data Data)//각 바퀴의 최소/최대값
        {
            int point = 0;

            if (Axle.FL == -1) { Axle.FL = Math.Round(TSet.Move__FL, point); }
            if (Axle.FR == -1) { Axle.FR = Math.Round(TSet.Move__FR, point); }
            if (Axle.RL == -1) { Axle.RL = Math.Round(TSet.Move__RL, point); }
            if (Axle.RR == -1) { Axle.RR = Math.Round(TSet.Move__RR, point); }

            if (Axle.FL <= TSet.Move__FL) { Axle.FL = Math.Round(TSet.Move__FL, point); }
            if (Axle.FR <= TSet.Move__FR) { Axle.FR = Math.Round(TSet.Move__FR, point); }
            if (Axle.RL <= TSet.Move__RL) { Axle.RL = Math.Round(TSet.Move__RL, point); }
            if (Axle.RR <= TSet.Move__RR) { Axle.RR = Math.Round(TSet.Move__RR, point); }

            return Axle;
        }

        public void TestDataSave(string pTestNo)//완료 데이터 저장
        {
            RnB_Data_Save(pTestNo);

            if (TSet.ECUModel != "None")
            {
                ECU_Data_Save(pTestNo);
            }
        }

        private void RnB_Data_Save(string pTestNo)//제동력 데이터 저장
        {
            int Data_CNT = 0;

            Data_CNT = main.DB_All.DB_RnBs.Select(pTestNo);

            main.DB_All.DB_RnBs.dbAcceptNo = pTestNo;

            if (TSet.SST1_Pan == "" && TSet.SST2_Pan == "")
            {
                TSet.SST__Pan = "";
            }
            else
            {
                if (TSet.SST1_Pan == "" && TSet.SST2_Pan == "")
                {
                    TSet.SST__Pan = TSet.SST1_Pan;
                }
                else
                {
                }
            }

            main.DB_All.DB_RnBs.db1SST_Val = TSet.MaxSpeed.ToString("#0.0");    //TSet.SST1_Val.ToString("#0.0");
            main.DB_All.DB_RnBs.db1SSTSine = TSet.SST1Sine;
            main.DB_All.DB_RnBs.db1SSTOkNg = TSet.SST1_Pan;
            main.DB_All.DB_RnBs.db2SST_Val = TSet.SST2_Val.ToString("#0.0");
            main.DB_All.DB_RnBs.db2SSTSine = TSet.SST2Sine;
            main.DB_All.DB_RnBs.db2SSTOkNg = TSet.SST2_Pan;
            main.DB_All.DB_RnBs.db_SSTOkNg = TSet.SST__Pan;

            main.DB_All.DB_RnBs.db1_Weight = 0;
            main.DB_All.DB_RnBs.db1Drag__L = TSet.Drag_Max.FL;
            main.DB_All.DB_RnBs.db1Drag__R = TSet.Drag_Max.FR;

            main.DB_All.DB_RnBs.db1Brake_L = TSet.GBrk_Max.FL;
            main.DB_All.DB_RnBs.db1Brake_R = TSet.GBrk_Max.FR;

            main.DB_All.DB_RnBs.db1Park__L = -1;
            main.DB_All.DB_RnBs.db1Park__R = -1;

            main.DB_All.DB_RnBs.db1BrakeOX = H2Y.OK;

            main.DB_All.DB_RnBs.db2_Weight = 0;
            main.DB_All.DB_RnBs.db2Drag__L = TSet.Drag_Max.RL;
            main.DB_All.DB_RnBs.db2Drag__R = TSet.Drag_Max.RR;

            main.DB_All.DB_RnBs.db2Brake_L = TSet.GBrk_Max.RL;
            main.DB_All.DB_RnBs.db2Brake_R = TSet.GBrk_Max.RR;

            main.DB_All.DB_RnBs.db2Park__L = TSet.Park_Max.RL;
            main.DB_All.DB_RnBs.db2Park__R = TSet.Park_Max.RR;

            main.DB_All.DB_RnBs.db2BrakeOX = H2Y.OK;

            main.DB_All.DB_RnBs.db1Balan_L = TSet.GBrk_Max.FL;
            main.DB_All.DB_RnBs.db1Balan_R = TSet.GBrk_Max.FR;
            main.DB_All.DB_RnBs.db1Balance = H2Y.Dbl_Balance(TSet.GBrk_Max.FL, TSet.GBrk_Max.FR);
            main.DB_All.DB_RnBs.db1Bal_Pan = H2Y.Ret_JudgeOX(PSet.RnB.Bal_FMin, PSet.RnB.Bal_FMax, main.DB_All.DB_RnBs.db1Balance);

            main.DB_All.DB_RnBs.db2Balan_L = TSet.GBrk_Max.RL;
            main.DB_All.DB_RnBs.db2Balan_R = TSet.GBrk_Max.RR;
            main.DB_All.DB_RnBs.db2Balance = H2Y.Dbl_Balance(TSet.GBrk_Max.RL, TSet.GBrk_Max.RR);
            main.DB_All.DB_RnBs.db2Bal_Pan = H2Y.Ret_JudgeOX(PSet.RnB.Bal_RMin, PSet.RnB.Bal_RMax, main.DB_All.DB_RnBs.db2Balance);

            main.DB_All.DB_RnBs.db_BalForR = H2Y.Dbl_Balance2((TSet.GBrk_Max.RL + TSet.GBrk_Max.RR), (TSet.GBrk_Max.FL + TSet.GBrk_Max.FR));
            main.DB_All.DB_RnBs.db_Balance = H2Y.Ret_JudgeOX(PSet.RnB.Bal_AMin, PSet.RnB.Bal_AMax, main.DB_All.DB_RnBs.db_BalForR);

            main.DB_All.DB_RnBs.dbSMTValue = Math.Round(TSet.SMTValue, 2);
            main.DB_All.DB_RnBs.dbSMT_OkNg = H2Y.Ret_JudgeOX(PSet.RnB.SpeedMin, PSet.RnB.SpeedMax, main.DB_All.DB_RnBs.dbSMTValue);

            main.DB_All.DB_RnBs.db_Reverse = Math.Round(TSet.ReverseV, 2);
            main.DB_All.DB_RnBs.db_Rev_Pan = H2Y.Ret_JudgeOX(0, 5, main.DB_All.DB_RnBs.db_Reverse);

            main.DB_All.DB_RnBs.db1SenSpdL = Math.Round(TSet.WSS_Last.FL, 2);
            main.DB_All.DB_RnBs.db1SenSpdR = Math.Round(TSet.WSS_Last.FR, 2);
            main.DB_All.DB_RnBs.db2SenSpdL = Math.Round(TSet.WSS_Last.RL, 2);
            main.DB_All.DB_RnBs.db2SenSpdR = Math.Round(TSet.WSS_Last.RR, 2);

            if (H2Y.Ret_JudgeOX(PSet.ECU.WSSFLMin, PSet.ECU.WSSFLMax, main.DB_All.DB_RnBs.db1SenSpdL) == H2Y.OK &&
                H2Y.Ret_JudgeOX(PSet.ECU.WSSFRMin, PSet.ECU.WSSFRMax, main.DB_All.DB_RnBs.db1SenSpdR) == H2Y.OK &&
                H2Y.Ret_JudgeOX(PSet.ECU.WSSRLMin, PSet.ECU.WSSRLMax, main.DB_All.DB_RnBs.db2SenSpdL) == H2Y.OK &&
                H2Y.Ret_JudgeOX(PSet.ECU.WSSRRMin, PSet.ECU.WSSRRMax, main.DB_All.DB_RnBs.db2SenSpdR) == H2Y.OK)
            {
                TSet.WSS_Last.OX = H2Y.OK;
                
            }
            else
            {
                TSet.WSS_Last.OX = H2Y.NG;
            }

            main.DB_All.DB_RnBs.db_Sen_Spd = TSet.WSS_Last.OX;

            main.DB_All.DB_RnBs.db1ABS_DeL = Math.Abs(TSet.ABSB_Min.FL);
            main.DB_All.DB_RnBs.db1ABS_InL = Math.Abs(TSet.ABSB_Max.FL);
            main.DB_All.DB_RnBs.db1ABS_DeR = Math.Abs(TSet.ABSB_Min.FR);
            main.DB_All.DB_RnBs.db1ABS_InR = Math.Abs(TSet.ABSB_Max.FR);

            main.DB_All.DB_RnBs.db2ABS_DeL = Math.Abs(TSet.ABSB_Min.RL);
            main.DB_All.DB_RnBs.db2ABS_InL = Math.Abs(TSet.ABSB_Max.RL);
            main.DB_All.DB_RnBs.db2ABS_DeR = Math.Abs(TSet.ABSB_Min.RR);
            main.DB_All.DB_RnBs.db2ABS_InR = Math.Abs(TSet.ABSB_Max.RR);

            if (Data_CNT == 0)
            {
                main.DB_All.DB_RnBs.Insert();
            }
            else
            {
                main.DB_All.DB_RnBs.Update(pTestNo);
            }
        }
        private void ECU_Data_Save(string pTestNo)//기타   데이터 저장
        {
            int Data_CNT = main.DB_All.DB_ECUs.Select(pTestNo);

            main.DB_All.DB_ECUs.dbAcceptNo = pTestNo;

            main.DB_All.DB_ECUs.dbStt_Comm = Ret_ECU_Logs(ECUs.Stt_Comm, 50);
            main.DB_All.DB_ECUs.dbIden80_1 = Ret_ECU_Logs(ECUs.Veh_Name, 50);
            main.DB_All.DB_ECUs.dbIden80_2 = Ret_ECU_Logs(ECUs.Sys_Name, 50);
            main.DB_All.DB_ECUs.dbIden80_3 = Ret_ECU_Logs(ECUs.Var_Code, 50);
            main.DB_All.DB_ECUs.dbIden80_4 = Ret_ECU_Logs(ECUs.HW__Vers, 50);
            main.DB_All.DB_ECUs.dbIden80_5 = Ret_ECU_Logs(ECUs.SW__Vers, 50);
            main.DB_All.DB_ECUs.dbIden80_6 = Ret_ECU_Logs(ECUs.BCD_Date, 50);
            main.DB_All.DB_ECUs.dbIden80_7 = Ret_ECU_Logs(ECUs.Part_Num, 50);

            main.DB_All.DB_ECUs.dbProcessB = Ret_ECU_Logs(ECUs.ProcessB, 50);
            main.DB_All.DB_ECUs.dbRead_Vin = Ret_ECU_Logs(ECUs.Read_Vin, 50);
            main.DB_All.DB_ECUs.dbReadTire = Ret_ECU_Logs(ECUs.ReadTire, 50);
            main.DB_All.DB_ECUs.dbSAS_Zero = Ret_ECU_Logs(ECUs.SAS_Zero, 50);

            main.DB_All.DB_ECUs.dbSigPedal = Ret_ECU_Logs(ECUs.SigPedal, 50);
            
            main.DB_All.DB_ECUs.dbDTC_Read = Ret_ECU_Logs(ECUs.DTC_Read, 50);
            main.DB_All.DB_ECUs.dbDTCClear = Ret_ECU_Logs(ECUs.DTCClear, 50);
            main.DB_All.DB_ECUs.dbSpLmt_On = Ret_ECU_Logs(ECUs.SpLmt_On, 50);
            main.DB_All.DB_ECUs.dbSpLmtOff = Ret_ECU_Logs(ECUs.SpLmtOff, 50);
            main.DB_All.DB_ECUs.dbEnd_Comm = Ret_ECU_Logs(ECUs.End_Comm, 50);

            if (Data_CNT == 0)
            {
                main.DB_All.DB_ECUs.Insert();
            }
            else
            {
                main.DB_All.DB_ECUs.Update(pTestNo);
            }
        }

        private string Ret_ECU_Logs(string pLog, int pLen)
        {
            if (pLog == null) { return ""; }

            string Ret_Strs = "";

            try
            {
                if (pLog.Length < pLen)
                {
                    Ret_Strs = pLog;
                }
                else
                {
                    Ret_Strs = pLog.Substring(0, pLen);
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Ret_ECU_Logs: " + ex.Message);
            }

            return Ret_Strs;
        }

        #region ECU Control
        private bool Start_Communication(cls_Test Test)
        {
            for (int cnt = 1; cnt <= 5; cnt++)
            {
                ECUs.Start_Communication();
                H2Y.Sleep(200);

                if (NeoVI.Return == true) break;
            }

            if (!TSet.ECU_Errs) TSet.ECU_Errs = NeoVI.Return;

            return NeoVI.Return;
        }

        private bool ECU_Identification(cls_Test Test)
        {
            for (int cnt = 1; cnt <= Cycle; cnt++)
            {
                ECUs.ECU_Identification();

                if (NeoVI.Return == true) break;
            }

            if (!TSet.ECU_Errs) TSet.ECU_Errs = NeoVI.Return;

            return NeoVI.Return;
        }

        private bool Read_Signals(cls_Test Test)
        {
            for (int cnt = 1; cnt <= Cycle; cnt++)
            {
                ECUs.Check_Signals();

                if (NeoVI.Return == true) break;
            }

            if (!TSet.ECU_Errs) TSet.ECU_Errs = NeoVI.Return;

            return NeoVI.Return;
        }


        public static bool Dynamic_Chery()           //Dynamic Chery
        {
            bool Ret = true;

            float T2 = 400;
            if (!TSet.ECU_Flag) { TSet.ECU_Flag = true; }

            TSet.ECU_Time = TSet.ABSv_Time + ((DateTime.Now.Ticks - TSet.ECU_Ofst) / H2Y.tick_Dvd);

            if (TSet.ECU_Flag && TSet.ECU_Setp == 0)
            {
                //Ret = ECUs.Start_Communication();
                if (Ret)
                {
                    //Ret = SecurityAccess();
                    if (Ret)
                    {
                        TSet.ECU_Setp = 1;

                        TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time; 
                        
                    }
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 1)
            {
                //if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    H2Y.Sleep(1000);
                    TSet.ECU_Setp = 2;
                    TSet.ECU_Flag = false;
                    TSet.ECU_oldT = TSet.ECU_Time; 
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 2)
            {
                Ret = ECUs.Dynamic_Step(1);
                TSet.ECU_Setp = 3;
                TSet.ECU_Flag = false;
                TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 3)
            {
                //if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    H2Y.Sleep(1000);
                    TSet.ECU_Setp = 4;
                    TSet.ECU_Flag = false;
                    TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 4)
            {
                Ret = ECUs.Dynamic_Step(2);
                TSet.ECU_Setp = 5;
                TSet.ECU_Flag = false;
                TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 5)
            {
                //if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    H2Y.Sleep(1000);
                    TSet.ECU_Setp = 6;
                    TSet.ECU_Flag = false;
                    TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 6)
            {
                Ret = ECUs.Dynamic_Step(3);
                TSet.ECU_Setp = 7;
                TSet.ECU_Flag = false;
                TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 7)
            {
                //if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    H2Y.Sleep(1000);
                    TSet.ECU_Setp = 8;
                    TSet.ECU_Flag = false;
                    TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 8)
            {
                Ret = ECUs.Dynamic_Step(4);
                TSet.ECU_Setp = 9;
                TSet.ECU_Flag = false;
                TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 9)
            {
                //if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    H2Y.Sleep(1000);
                    TSet.ECU_Setp = 10;
                    TSet.ECU_Flag = false;
                    TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 10)
            {
                Ret = ECUs.Dynamic_Step(5);
                TSet.ECU_Setp = 11;
                TSet.ECU_Flag = false;
                TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 11)
            {
                //if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    H2Y.Sleep(1000);
                    TSet.ECU_Setp = 12;
                    TSet.ECU_Flag = false;
                    TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 12)
            {
                Ret = ECUs.Dynamic_Step(6);
                TSet.ECU_Setp = 13;
                TSet.ECU_Flag = false;
                TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 13)
            {
                //if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    H2Y.Sleep(1000);
                    TSet.ECU_Setp = 14;
                    TSet.ECU_Flag = false;
                    TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 14)
            {
                Ret = ECUs.Dynamic_Step(7);
                TSet.ECU_Setp = 15;
                TSet.ECU_Flag = false;
                TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 15)
            {
                //if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    H2Y.Sleep(1000);
                    TSet.ECU_Setp = 16;
                    TSet.ECU_Flag = false;
                    TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 16)
            {
                Ret = ECUs.Dynamic_Step(8);
                TSet.ECU_Setp = 17;
                TSet.ECU_Flag = false;
                TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 17)
            {
                //if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    
                    TSet.ECU_Setp = 18;
                    TSet.ECU_Flag = false;
                    TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 18)
            {
                
               
                ECUs.ABS_Step = 5;                    
            }
            return NeoVI.Return;
        }

        private bool Dynamic_Test(cls_Test Test)
        {
            bool Ret = true;
            
            float T2 = 600;
            float T3 = 2000;

            if (!TSet.ECU_Flag) { TSet.ECU_Flag = true; }

            TSet.ECU_Time = TSet.ABSv_Time + ((DateTime.Now.Ticks - TSet.ECU_Ofst) / H2Y.tick_Dvd);

            if (TSet.ECU_Flag && TSet.ECU_Setp == 0)
            {
                Ret = ECUs.Dynamic_Step(0);                                 
                TSet.ECU_Setp = 1;      TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 1)
            {
                if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    TSet.ECU_Setp = 2;  TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 2)
            {
                Ret = ECUs.Dynamic_Step(1); 
                TSet.ECU_Setp = 3;      TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 3)
            {
                if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    TSet.ECU_Setp = 4;  TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 4)
            {
                Ret = ECUs.Dynamic_Step(2);
                TSet.ECU_Setp = 5;      TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 5)
            {
                if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000)) 
                {
                    TSet.ECU_Setp = 6;  TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 6)
            {
                Ret = ECUs.Dynamic_Step(3);
                TSet.ECU_Setp = 7;      TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 7)
            {
                if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    TSet.ECU_Setp = 8;  TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 8)
            {
                Ret = ECUs.Dynamic_Step(4);
                TSet.ECU_Setp = 9;      TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 9)
            {
                if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    TSet.ECU_Setp = 10; TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 10)
            {
                Ret = ECUs.Dynamic_Step(5);
                TSet.ECU_Setp = 11;     TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 11)
            {
                if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T3, 1000))
                {
                    TSet.ECU_Setp = 12; TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 12)
            {
                if (ECUs.ECU == ECUs.Mobis_LX3H || ECUs.ECU == ECUs.Mobis_LX3I)
                {
                    Ret = ECUs.Dynamic_Step(6);
                    TSet.ECU_Setp = 13; TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
                }
                else
                {
                    ECUs.ABS_Step = 5;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 13)
            {
                if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    TSet.ECU_Setp = 14; TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 14)
            {
                Ret = ECUs.Dynamic_Step(7);
                TSet.ECU_Setp = 15; TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 15)
            {
                if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    TSet.ECU_Setp = 16; TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 16)
            {
                Ret = ECUs.Dynamic_Step(8);
                TSet.ECU_Setp = 17; TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 17)
            {
                if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T2, 1000))
                {
                    TSet.ECU_Setp = 18; TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 18)
            {
                Ret = ECUs.Dynamic_Step(9);
                TSet.ECU_Setp = 19; TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 19)
            {
                if (TSet.ECU_Time - TSet.ECU_oldT > H2Y.DVD(T3, 1000))
                {
                    TSet.ECU_Setp = 20; TSet.ECU_Flag = false; TSet.ECU_oldT = TSet.ECU_Time;
                }
            }

            if (TSet.ECU_Flag && TSet.ECU_Setp == 20)
            {
                ECUs.ABS_Step = 5;
            }

            return NeoVI.Return;
        }

        private bool Read__DTC(cls_Test Test)
        {
            for (int cnt = 1; cnt <= Cycle; cnt++)
            {
                ECUs.Read__DTC();

                if (NeoVI.Return == true) break;
            }

            if (!TSet.ECU_Errs) TSet.ECU_Errs = NeoVI.Return;

            return NeoVI.Return;
        }

        private bool Clear_DTC(cls_Test Test)
        {
            for (int cnt = 1; cnt <= Cycle; cnt++)
            {
                ECUs.Clear_DTC();

                if (NeoVI.Return == true) break;
            }

            if (!TSet.ECU_Errs) TSet.ECU_Errs = NeoVI.Return;

            return NeoVI.Return;
        }

        private bool Stop_Communication(cls_Test Test)
        {
            for (int cnt = 1; cnt <= Cycle; cnt++)
            {
                ECUs.Stop_Communication();

                if (NeoVI.Return == true) break;
            }

            if (!TSet.ECU_Errs) TSet.ECU_Errs = NeoVI.Return;

            return NeoVI.Return;
        }
        #endregion
    }
}
