using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KI_RnB
{
    class cls_Data
    {
        fom_Main main;

        public string AcceptNo { get; set; }  //측정 번호
        public string Vin___No { get; set; }  //바코드
        public string CarModel { get; set; }  //모델명
        public string ECUModel { get; set; }  //ECU 모델명
        public string CarBarID { get; set; }  //바코드 구분자
        public string CarWbase { get; set; }  //휠베이스 거리
        public string CarEngin { get; set; }  //엔진 모델
        public string CarTranM { get; set; }  //트렌스미션 모델
        public string Car_ABST { get; set; }  //ABS 모델
        public string CarCurve { get; set; }  //드라이브 커브
        public string CarDrive { get; set; }  //드라이브 축(Front, Rear, 4WD)
        public string TestDate { get; set; }  //측정 일자
        public string Run_Time { get; set; }  //진행 시작 시간
        public string TestTime { get; set; }  //측정 시각 시간
        public string End_Time { get; set; }  //종료 시간

        public TSet.wheel_Test SST_Last = new TSet.wheel_Test();

        public TSet.wheel_Test Drag_Min = new TSet.wheel_Test();
        public TSet.wheel_Test Drag_Max = new TSet.wheel_Test();
        public TSet.wheel_Test GBrk_Min = new TSet.wheel_Test();
        public TSet.wheel_Test GBrk_Max = new TSet.wheel_Test();
        public TSet.wheel_Test Park_Min = new TSet.wheel_Test();
        public TSet.wheel_Test Park_Max = new TSet.wheel_Test();

        public TSet.wheel_Test SMT_Last = new TSet.wheel_Test();
        public TSet.wheel_Test Rev_Last = new TSet.wheel_Test();

        public TSet.wheel_Test ABSB_Min = new TSet.wheel_Test();
        public TSet.wheel_Test ABSB_Max = new TSet.wheel_Test();
        public TSet.wheel_Test WSS__Min = new TSet.wheel_Test();
        public TSet.wheel_Test WSS__Max = new TSet.wheel_Test();

        public cls_Data(fom_Main main)
        {
            this.main = main;
        }

        public void Read_AllData(string pAcptNo)
        {
            ReadInfoData(pAcptNo);
            Read_BrkData(pAcptNo);
        }

        private void InitInfoData()
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
            TestDate = "";     //측정 일자
            Run_Time = "";     //진행 시작 시간
            TestTime = "";     //측정 시각 시간
            End_Time = "";     //종료 시간
        }

        public int ReadInfoData(string pAcptNo)
        {
            int Ret_CNT = main.DB_All.DB_Info.Select(pAcptNo);

            InitInfoData();

            if (Ret_CNT == 1)
            {
                AcceptNo = main.DB_All.DB_Info.dbAcceptNo;     //측정 번호
                Vin___No = main.DB_All.DB_Info.dbVin___No;     //바코드
                CarModel = main.DB_All.DB_Info.dbCarModel;     //모델명
                ECUModel = main.DB_All.DB_Info.dbECUModel;     //ECU 모델명
                CarBarID = main.DB_All.DB_Info.dbCarBarID;     //바코드 구분자
                CarWbase = main.DB_All.DB_Info.dbCarWbase;     //휠베이스 거리
                CarEngin = main.DB_All.DB_Info.dbCarEngin;     //엔진 모델
                CarTranM = main.DB_All.DB_Info.dbCarTranM;     //트렌스미션 모델
                Car_ABST = main.DB_All.DB_Info.dbCar_ABST;     //ABS 모델
                CarCurve = main.DB_All.DB_Info.dbCarCurve;     //드라이브 커브
                CarDrive = main.DB_All.DB_Info.dbCarDrive;     //드라이브 축
                TestDate = main.DB_All.DB_Info.dbTestDate;     //측정 일자
                Run_Time = main.DB_All.DB_Info.dbRun_Time;     //진행 시작 시간
                TestTime = main.DB_All.DB_Info.dbTestTime;     //측정 시각 시간
                End_Time = main.DB_All.DB_Info.dbEnd_Time;     //종료 시간

                Sel_Parameter(CarModel);
            }

            return Ret_CNT;
        }

        public string Sel_Parameter(string pModel)
        {
            if (main.DB_All.DBModel.Select(pModel) == 1)
            {
                if (main.DB_All.DBParam.Select(main.DB_All.DBModel.dbCarParam) == 1)
                {
                    PSet.RnB.SpeedMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam001);   //속도계 min
                    PSet.RnB.SpeedMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam002);   //속도계 max
                    PSet.RnB.SST__Min = H2Y.StrToSingles(main.DB_All.DBParam.dbParam003);   //SST    min
                    PSet.RnB.SST__Max = H2Y.StrToSingles(main.DB_All.DBParam.dbParam004);   //SST    max

                    PSet.RnB.PTSValue = H2Y.StrToSingles(main.DB_All.DBParam.dbParam009);   //Pedal Brake Target Force(kg)
                    PSet.RnB.PTSGraph = H2Y.StrToSingles(main.DB_All.DBParam.dbParam010);   //Pedal Brake Max    Graph(kg)

                    PSet.RnB.DragFMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam011);   //전축 끌림 min
                    PSet.RnB.DragFMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam012);   //전축 끌림 max
                    PSet.RnB.DragRMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam013);   //후축 끌림 min
                    PSet.RnB.DragRMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam014);   //후축 끌림 max

                    PSet.RnB.Brk_FMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam021);   //전축 제동력 min
                    PSet.RnB.Brk_FMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam022);   //전축 제동력 max
                    PSet.RnB.Brk_RMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam023);   //후축 제동력 min
                    PSet.RnB.Brk_RMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam024);   //후축 제동력 max

                    PSet.RnB.Park_Min = H2Y.StrToSingles(main.DB_All.DBParam.dbParam029);   //주차 제동력 min (cm)
                    PSet.RnB.Park_Max = H2Y.StrToSingles(main.DB_All.DBParam.dbParam030);   //주차 제동력 max (cm)

                    PSet.RnB.Bal_FMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam031);   //전축 발란스 min
                    PSet.RnB.Bal_FMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam032);   //전축 발란스 max
                    PSet.RnB.Bal_RMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam033);   //후축 발란스 min
                    PSet.RnB.Bal_RMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam034);   //후축 발란스 max
                    PSet.RnB.Bal_AMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam035);   //전체 발란스 min
                    PSet.RnB.Bal_AMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam036);   //전체 발란스 max

                    PSet.ECU.WSSFLMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam051);   //WSS F-L Min
                    PSet.ECU.WSSFLMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam052);   //WSS F-L Max
                    PSet.ECU.WSSFRMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam053);   //WSS F-R Min
                    PSet.ECU.WSSFRMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam054);   //WSS F-R Max
                    PSet.ECU.WSSRLMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam055);   //WSS R-L Min
                    PSet.ECU.WSSRLMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam056);   //WSS R-L Max
                    PSet.ECU.WSSRRMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam057);   //WSS R-R Min
                    PSet.ECU.WSSRRMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam058);   //WSS R-R Max

                    PSet.ECU.Dec_FMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam061);   //전축 감소(Dec) min
                    PSet.ECU.Dec_FMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam062);   //전축 감소(Dec) max
                    PSet.ECU.Inc_FMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam063);   //전축 증가(Inc) min
                    PSet.ECU.Inc_FMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam064);   //전축 증가(Inc) max
                    PSet.ECU.Dec_RMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam065);   //후축 감소(Dec) min
                    PSet.ECU.Dec_RMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam066);   //후축 감소(Dec) max
                    PSet.ECU.Inc_RMin = H2Y.StrToSingles(main.DB_All.DBParam.dbParam067);   //후축 증가(Inc) min
                    PSet.ECU.Inc_RMax = H2Y.StrToSingles(main.DB_All.DBParam.dbParam068);   //후축 증가(Inc) max

                    PSet.BRK.Wgt_1Min = H2Y.StrToSingles(main.DB_All.DBParam.dbParam071);   //전축  축중 최소
                    PSet.BRK.Wgt_1Max = H2Y.StrToSingles(main.DB_All.DBParam.dbParam072);   //전축  축중 최대
                    PSet.BRK.Wgt_2Min = H2Y.StrToSingles(main.DB_All.DBParam.dbParam074);   //후축  축중 최소
                    PSet.BRK.Wgt_2Max = H2Y.StrToSingles(main.DB_All.DBParam.dbParam075);   //후축  축중 최대
                    PSet.BRK.Wgt_Time = H2Y.StrToSingles(main.DB_All.DBParam.dbParam078);   //축중 측정 시간

                    PSet.BRK.Brk_1Std = H2Y.StrToSingles(main.DB_All.DBParam.dbParam081);   //전축 제동력(%)
                    PSet.BRK.Brk_2Std = H2Y.StrToSingles(main.DB_All.DBParam.dbParam082);   //후축 제동력(%)
                    PSet.BRK.Brk_Drag = H2Y.StrToSingles(main.DB_All.DBParam.dbParam083);   //끌림 제동력(%)
                    PSet.BRK.Brk_Diff = H2Y.StrToSingles(main.DB_All.DBParam.dbParam084);   //편차 제동력(%)
                    PSet.BRK.BrkTotal = H2Y.StrToSingles(main.DB_All.DBParam.dbParam085);   //  합 제동력(%)
                    PSet.BRK.Brk_Park = H2Y.StrToSingles(main.DB_All.DBParam.dbParam086);   //주차 제동력(%)
                    PSet.BRK.Brk_Time = H2Y.StrToSingles(main.DB_All.DBParam.dbParam088);   //일반 제동력 측정 시간(sec)
                    PSet.BRK.DragTime = H2Y.StrToSingles(main.DB_All.DBParam.dbParam089);   //끌림 제동력 측정 시간(sec)
                    PSet.BRK.ParkTime = H2Y.StrToSingles(main.DB_All.DBParam.dbParam090);   //주차 제동력 측정 시간(sec)
                    
                    return main.DB_All.DBModel.dbCarParam;    // Param.dbParamSeq;
                }
                else
                {
                    PSet.RnB.SpeedMin = 0;
                    PSet.RnB.SpeedMax = 0;

                    PSet.RnB.SST__Min = 0;
                    PSet.RnB.SST__Max = 0;

                    PSet.RnB.PTSValue = 0;
                    PSet.RnB.PTSGraph = 0;

                    PSet.RnB.DragFMin = 0;
                    PSet.RnB.DragFMax = 0;
                    PSet.RnB.DragRMin = 0;
                    PSet.RnB.DragRMax = 0;

                    PSet.RnB.Brk_FMin = 0;
                    PSet.RnB.Brk_FMax = 0;
                    PSet.RnB.Brk_RMin = 0;
                    PSet.RnB.Brk_RMax = 0;

                    PSet.RnB.Park_Min = 0;
                    PSet.RnB.Park_Max = 0;

                    PSet.RnB.Bal_FMin = 0;
                    PSet.RnB.Bal_FMax = 0;
                    PSet.RnB.Bal_RMin = 0;
                    PSet.RnB.Bal_RMax = 0;
                    PSet.RnB.Bal_AMin = 0;
                    PSet.RnB.Bal_AMax = 0;

                    PSet.ECU.WSSFLMin = 0;
                    PSet.ECU.WSSFLMax = 0;
                    PSet.ECU.WSSFRMin = 0;
                    PSet.ECU.WSSFRMax = 0;
                    PSet.ECU.WSSRLMin = 0;
                    PSet.ECU.WSSRLMax = 0;
                    PSet.ECU.WSSRRMin = 0;
                    PSet.ECU.WSSRRMax = 0;

                    PSet.ECU.Dec_FMin = 0;
                    PSet.ECU.Dec_FMax = 0;
                    PSet.ECU.Inc_FMin = 0;
                    PSet.ECU.Inc_FMax = 0;
                    PSet.ECU.Dec_RMin = 0;
                    PSet.ECU.Dec_RMax = 0;
                    PSet.ECU.Inc_RMin = 0;
                    PSet.ECU.Inc_RMax = 0;

                    PSet.BRK.Wgt_1Min = 0;
                    PSet.BRK.Wgt_1Max = 0;
                    PSet.BRK.Wgt_2Min = 0;
                    PSet.BRK.Wgt_2Max = 0;
                    PSet.BRK.Wgt_Time = 0;

                    PSet.BRK.Brk_1Std = 0;
                    PSet.BRK.Brk_2Std = 0;
                    PSet.BRK.Brk_Drag = 0;
                    PSet.BRK.Brk_Diff = 0;
                    PSet.BRK.BrkTotal = 0;
                    PSet.BRK.Brk_Park = 0;
                    PSet.BRK.Brk_Time = 0;
                    PSet.BRK.DragTime = 0;
                    PSet.BRK.ParkTime = 0;

                    return "";
                }
            }

            return "";
        }
        
        private void Init_BrkData()
        {
            SST_Last.FL = -1; SST_Last.FR = -1; SST_Last.RL = -1; SST_Last.RR = -1;

            Drag_Min.FL = -1; Drag_Min.FR = -1; Drag_Min.RL = -1; Drag_Min.RR = -1;
            Drag_Min.FL = -1; Drag_Min.FR = -1; Drag_Min.RL = -1; Drag_Min.RR = -1;

            GBrk_Min.FL = -1; GBrk_Min.FR = -1; GBrk_Min.RL = -1; GBrk_Min.RR = -1;
            GBrk_Max.FL = -1; GBrk_Max.FR = -1; GBrk_Max.RL = -1; GBrk_Max.RR = -1;

            Park_Min.FL = -1; Park_Min.FR = -1; Park_Min.RL = -1; Park_Min.RR = -1;
            Park_Max.FL = -1; Park_Max.FR = -1; Park_Max.RL = -1; Park_Max.RR = -1;

            SMT_Last.FL = -1; SMT_Last.FR = -1; SMT_Last.RL = -1; SMT_Last.RR = -1;
            Rev_Last.FL = -1; Rev_Last.FR = -1; Rev_Last.RL = -1; Rev_Last.RR = -1;

            ABSB_Min.FL = -1; ABSB_Min.FR = -1; ABSB_Min.RL = -1; ABSB_Min.RR = -1;
            ABSB_Max.FL = -1; ABSB_Max.FR = -1; ABSB_Max.RL = -1; ABSB_Max.RR = -1;

            WSS__Min.FL = -1; WSS__Min.FR = -1; WSS__Min.RL = -1; WSS__Min.RR = -1;
            WSS__Max.FL = -1; WSS__Max.FR = -1; WSS__Max.RL = -1; WSS__Max.RR = -1;
        }

        public int Read_BrkData(string pAcptNo)
        {
            int Ret_CNT = main.DB_All.DB_RnBs.Select(pAcptNo);

            Init_BrkData();

            if (Ret_CNT == 1)
            {
                //main.DB_RnB.DB_Brks.dbAcceptNo
                SST_Last.FL = H2Y.toDbl(main.DB_All.DB_RnBs.db1SST_Val);
                //Drag_Max.FL = main.DB_RnB.DB_Brks.db1SSTSine;
                //Drag_Max.FL = main.DB_RnB.DB_Brks.db1SSTOkNg;
                //Drag_Max.FL = main.DB_RnB.DB_Brks.db2SST_Val ;
                //Drag_Max.FL = main.DB_RnB.DB_Brks.db2SSTSine;
                //Drag_Max.FL = main.DB_RnB.DB_Brks.db2SSTOkNg;
                //Drag_Max.FL = main.DB_RnB.DB_Brks.db_SSTOkNg;


                //main.DB_RnB.DB_Brks.db1_Weight = 0; 
                Drag_Max.FL = main.DB_All.DB_RnBs.db1Drag__L;
                Drag_Max.FR = main.DB_All.DB_RnBs.db1Drag__R;

                GBrk_Max.FL = main.DB_All.DB_RnBs.db1Brake_L;
                GBrk_Max.FR = main.DB_All.DB_RnBs.db1Brake_R;

                Park_Max.FL = main.DB_All.DB_RnBs.db1Park__L;
                Park_Max.FR = main.DB_All.DB_RnBs.db1Park__R;
                //main.DB_RnB.DB_Brks.db1BrakeOX = H2Y.OK;

                //main.DB_RnB.DB_Brks.db2_Weight = 0;
                Drag_Max.RL = main.DB_All.DB_RnBs.db2Drag__L;
                Drag_Max.RR = main.DB_All.DB_RnBs.db2Drag__R;

                GBrk_Max.RL = main.DB_All.DB_RnBs.db2Brake_L;
                GBrk_Max.RR = main.DB_All.DB_RnBs.db2Brake_R;

                Park_Max.RL = main.DB_All.DB_RnBs.db2Park__L;
                Park_Max.RR = main.DB_All.DB_RnBs.db2Park__R;
                //main.DB_RnB.DB_Brks.db2BrakeOX = H2Y.OK;

                main.DB_All.DB_RnBs.db1Balan_L = 0;
                main.DB_All.DB_RnBs.db1Balan_R = 0;
                main.DB_All.DB_RnBs.db1Balance = H2Y.Dbl_Balance(main.DB_All.DB_RnBs.db1Brake_L, main.DB_All.DB_RnBs.db1Brake_R);
                //main.DB_RnB.DB_Brks.db1Bal_Pan = ""; 

                main.DB_All.DB_RnBs.db2Balan_L = 0;
                main.DB_All.DB_RnBs.db2Balan_R = 0;
                main.DB_All.DB_RnBs.db2Balance = H2Y.Dbl_Balance(main.DB_All.DB_RnBs.db2Brake_L, main.DB_All.DB_RnBs.db2Brake_R);
                //main.DB_RnB.DB_Brks.db2Bal_Pan = "";

                main.DB_All.DB_RnBs.db_BalForR = H2Y.Dbl_Balance2((main.DB_All.DB_RnBs.db2Brake_L + main.DB_All.DB_RnBs.db2Brake_R), (main.DB_All.DB_RnBs.db1Brake_L + main.DB_All.DB_RnBs.db1Brake_R));
                //main.DB_RnB.DB_Brks.db_Balance = "";

                SMT_Last.FL = main.DB_All.DB_RnBs.dbSMTValue;
                SMT_Last.FR = main.DB_All.DB_RnBs.dbSMTValue;
                SMT_Last.RL = main.DB_All.DB_RnBs.dbSMTValue;
                SMT_Last.RR = main.DB_All.DB_RnBs.dbSMTValue;
                //main.DB_RnB.DB_Brks.dbSMT_OkNg

                Rev_Last.FL = main.DB_All.DB_RnBs.db_Reverse;
                Rev_Last.FR = main.DB_All.DB_RnBs.db_Reverse;
                Rev_Last.RL = main.DB_All.DB_RnBs.db_Reverse;
                Rev_Last.RR = main.DB_All.DB_RnBs.db_Reverse;
                //main.DB_RnB.DB_Brks.db_Rev_Pan

                ABSB_Min.FL = main.DB_All.DB_RnBs.db1ABS_DeL;
                ABSB_Max.FL = main.DB_All.DB_RnBs.db1ABS_InL;
                ABSB_Min.FR = main.DB_All.DB_RnBs.db1ABS_DeR;
                ABSB_Max.FR = main.DB_All.DB_RnBs.db1ABS_InR;

                ABSB_Min.RL = main.DB_All.DB_RnBs.db2ABS_DeL;
                ABSB_Max.RL = main.DB_All.DB_RnBs.db2ABS_InL;
                ABSB_Min.RR = main.DB_All.DB_RnBs.db2ABS_DeR;
                ABSB_Max.RR = main.DB_All.DB_RnBs.db2ABS_InR;

                WSS__Min.FL = main.DB_All.DB_RnBs.db1SenSpdL;
                WSS__Min.FR = main.DB_All.DB_RnBs.db1SenSpdR;
                WSS__Min.RL = main.DB_All.DB_RnBs.db2SenSpdL;
                WSS__Min.RR = main.DB_All.DB_RnBs.db2SenSpdR;

                WSS__Max.FL = main.DB_All.DB_RnBs.db1SenSpdL;
                WSS__Max.FR = main.DB_All.DB_RnBs.db1SenSpdR;
                WSS__Max.RL = main.DB_All.DB_RnBs.db2SenSpdL;
                WSS__Max.RR = main.DB_All.DB_RnBs.db2SenSpdR;
            }

            return Ret_CNT;
        }
    }
}
