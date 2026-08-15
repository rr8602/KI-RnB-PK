using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace KI_RnB
{
    public partial class fom_Rslt : Form
    {
        private fom_Main main;
        
        private string Sel_AcptNo;
        private byte Print_Md;
        private string PrintECU;
        private Font printFont;

        public int Ret_Test;

        private PrintDocument printDocument1 = new PrintDocument();
        Bitmap memoryImage;

        public fom_Rslt()
        {
            InitializeComponent();

            if (PSet.OwnerS00 > PSet.Def_Lang)
            {
                lblTitle.Text = PSet.LangRslt[0];   //보고서
                btnPrint.Text = PSet.LangRslt[1];   //출력
                btnClose.Text = PSet.LangRslt[2];   //닫기
                lblAxle1.Text = PSet.LangRslt[3];   //전축 좌
                lblAxle2.Text = PSet.LangRslt[4];   //전축 우
                lblAxle3.Text = PSet.LangRslt[5];   //후축 좌
                lblAxle4.Text = PSet.LangRslt[6];   //후축 우
                lblAxle5.Text = PSet.LangRslt[7];   //판정
                lblDrag0.Text = PSet.LangRslt[8];   //끌림 (kg)
                lblBrk_0.Text = PSet.LangRslt[9];   //제동력 (kg)
                lblPark0.Text = PSet.LangRslt[10];  //주차 (cm)
                //lblH_SST.Text = PSet.LangRslt[11];  //SST (m/km)
                lblSped0.Text = PSet.LangRslt[12];  //SMT(km/h)
                lblBal_0.Text = PSet.LangRslt[13];  //밸런스 %
                lblFront.Text = PSet.LangRslt[14];  //전축
                lbl_Rear.Text = PSet.LangRslt[15];  //후축
                lbl_Alls.Text = PSet.LangRslt[16];  //전축/후축
                lblJudeg.Text = PSet.LangRslt[17];  //판정

                lbl_WSS0.Text = PSet.LangRslt[18];  //WSS
                lblWtube.Text = PSet.LangRslt[19];  //기준값
                lbl_WSSH.Text = PSet.LangRslt[20];  //속도 (km/h)
                lblABSB0.Text = PSet.LangRslt[21];  //ABS
                lblDecrease.Text = PSet.LangRslt[22];//Dec. (kg)
                lblIncrease.Text = PSet.LangRslt[23];//Inc. (kg)
                lblError.Text = PSet.LangRslt[24];  //DTC 에러 리스트
            }
        }

        public fom_Rslt(fom_Main main, byte mode, string pAcptNo)
            : this()
        {
            this.main = main;
            Print_Md = mode;
            Sel_AcptNo = pAcptNo;
        }

        private void fom_Rslt_Load(object sender, EventArgs e)
        {
            if (Print_Md == 0)
            {
                this.Top = PSet.siz__Sub.Top;
                this.Left = PSet.siz__Sub.Left;
                this.Width = 1024;
                this.Height = 768;

                btnPrint.Visible = false;
                btnClose.Visible = false;

                lbl_Msgs.Visible = true;
                tmr_Stop.Enabled = true;
            }
            else
            {
                this.Top = PSet.siz_Main.Top;
                this.Left = PSet.siz_Main.Left;
                this.Width = 1024;
                this.Height = 768;

                btnPrint.Visible = true;
                btnClose.Visible = true;

                lbl_Msgs.Visible = false;
                tmr_Stop.Enabled = false;
            }

            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

            ClearAllData();
            Sel_DataShow(Sel_AcptNo);
        }

        private void tmr_Stop_Tick(object sender, EventArgs e)
        {
            tmr_Stop.Enabled = false;

        }

        public void Results_Show(string pAcptNo)
        {
            this.Show();
            
            ClearAllData();
            Sel_DataShow(pAcptNo);
            
            double ReadTime = 0, Old_Time = 0;
            double OfstTime = DateTime.Now.Ticks;
            double Gap_Time = DateTime.Now.AddSeconds(0.1).Ticks;
            bool Flag_Onf = false;

            Ret_Test = 0;

            PLC.Test__Finish();

            while (true)
            {
                if (TSet.Debug_Md) break;
                if (PLC.DO.MD_Emerge) Ret_Test = 1;
                if (TSet.TestStop) Ret_Test = 2;
                if (PLC.DO.Pos__Exit) Ret_Test = 3;
                if (PLC.DO.Pos_Enter) Ret_Test = 4;
                if (!PLC.DI.PSW__Stop && !PLC.DI.PSW_Start) break; 
                
                if (Ret_Test > 0) break;

                lblTitle.ForeColor = PLC.DI.PSW__Stop ? Color.Red : Color.White;

                System.Windows.Forms.Application.DoEvents();
            }

            while (true)
            {
                if (TSet.Debug_Md) break;
                if (PLC.DO.MD_Emerge) Ret_Test = 1;
                if (TSet.TestStop) Ret_Test = 2;
                if (PLC.DO.Pos__Exit) Ret_Test = 3;
                if (PLC.DO.Pos_Enter) Ret_Test = 4;
                if (PLC.DI.PSW__Stop) Ret_Test = 5;
                //if (PLC.DI.SWStat) Ret_Test = 6;

                if (Ret_Test > 0) break;

                ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;

                if (PLC.DO.Pos__Test || PLC.DO.Pos__Exit)
                {
                    if ((ReadTime - Old_Time) >= 2)
                    {
                        Old_Time = ReadTime;
                        Flag_Onf = !Flag_Onf;

                        PLC.Test__Finish();

                        if (Flag_Onf)
                        {               //(완료) STOP을 당기세요
                            Refresh_Msgs(PSet.LangRslt[25], System.Drawing.Color.Yellow);
                        }
                        else
                        {               //(재측정) TEST을 당기세요
                            //Refresh_Msgs(PSet.LangRslt[26], System.Drawing.Color.Red);
                            
                            //(완료) STOP을 당기세요
                            Refresh_Msgs(PSet.LangRslt[25], System.Drawing.Color.Red);
                        }
                    }
                }

                lblTitle.ForeColor = PLC.DI.PSW__Stop ? Color.Red : Color.White;
                System.Windows.Forms.Application.DoEvents();
            }

            if (PSet.sPrint == 1)
            {
                if (Ret_Test == 3 || Ret_Test == 4)
                {
                    Prints_Order();
                }
            }

            this.Close();
        }
        
        public void Refresh_Msgs(string pMsgs, Color pColor)
        {
            if (pMsgs == lbl_Msgs.Text && lbl_Msgs.ForeColor == pColor) return;
            if (pMsgs == "") return;

            lbl_Msgs.ForeColor = pColor;
            lbl_Msgs.Text = pMsgs;
        }

        private void ClearAllData()
        {
            lblVinNo.Text = ""; lbl_Date.Text = "";
            lblModel.Text = ""; lbl_Time.Text = "";

            lbl_Wgt1.Text = ""; lbl_Wgt2.Text = ""; lbl_Wgt3.Text = ""; lbl_Wgt4.Text = ""; lbl_Wgt5.Text = ""; ; lbl_Wgt6.Text = "";
            lblLDrg1.Text = ""; lblLDrg2.Text = ""; lblLDrg3.Text = ""; lblLDrg4.Text = "";
            lblRDrg1.Text = ""; lblRDrg2.Text = ""; lblRDrg3.Text = ""; lblRDrg4.Text = "";
            lblSDrg1.Text = ""; lblSDrg2.Text = ""; lblSDrg3.Text = ""; lblSDrg4.Text = "";
            lblLBrk1.Text = ""; lblLBrk2.Text = ""; lblLBrk3.Text = ""; lblLBrk4.Text = "";
            lblRBrk1.Text = ""; lblRBrk2.Text = ""; lblRBrk3.Text = ""; lblRBrk4.Text = "";
            lblSBrk1.Text = ""; lblSBrk2.Text = ""; lblSBrk3.Text = ""; lblSBrk4.Text = "";
            lblDBrk1.Text = ""; lblDBrk2.Text = ""; lblDBrk3.Text = ""; lblDBrk4.Text = "";
            lbl_Pan1.Text = ""; lbl_Pan2.Text = ""; lbl_Pan3.Text = ""; lbl_Pan4.Text = "";

            lblDrag1.Text = ""; lblDrag2.Text = ""; lblDrag3.Text = ""; lblDrag4.Text = ""; lblDrag5.Text = "";
            lblBrk_1.Text = ""; lblBrk_2.Text = ""; lblBrk_3.Text = ""; lblBrk_4.Text = ""; lblBrk_5.Text = "";
            lblPark1.Text = ""; lblPark2.Text = ""; lblPark3.Text = ""; lblPark4.Text = ""; lblPark5.Text = "";
            lblBal_1.Text = ""; lblBal_2.Text = ""; lblBal_3.Text = ""; lblBal_4.Text = ""; lblBal_5.Text = "";
            lblSpeed.Text = ""; lblSpdOX.Text = "";

            lblWSS_1.Text = ""; lblWSS_2.Text = ""; lblWSS_3.Text = ""; lblWSS_4.Text = ""; lblWSS_5.Text = "";
            lblWTub1.Text = ""; lblWTub2.Text = ""; lblWTub3.Text = ""; lblWTub4.Text = ""; lblWTub5.Text = "";
            lblDec_1.Text = ""; lblDec_2.Text = ""; lblDec_3.Text = ""; lblDec_4.Text = ""; lblDec_5.Text = "";
            lblInc_1.Text = ""; lblInc_2.Text = ""; lblInc_3.Text = ""; lblInc_4.Text = ""; lblInc_5.Text = "";
            lblATub1.Text = ""; lblATub2.Text = ""; lblATub3.Text = ""; lblATub4.Text = ""; lblATub5.Text = "";
        }

        private void Sel_DataShow(string pAcptNo)
        {
            if (pAcptNo.ToString().Trim() == "") return;

            byte Test_NGs = 0;

            int Info_CNT = main.DB_All.DB_Info.Select(pAcptNo);

            if (Info_CNT == 1)
            {
                string str_Year = main.DB_All.DB_Info.dbAcceptNo.Substring(0, 4);
                string str_MMMM = main.DB_All.DB_Info.dbAcceptNo.Substring(4, 2);
                string str_DDDD = main.DB_All.DB_Info.dbAcceptNo.Substring(6, 2);

                lblVinNo.Text = main.DB_All.DB_Info.dbVin___No;
                lblModel.Text = main.DB_All.DB_Info.dbCarModel;
                lbl_Date.Text = "Date : " +  str_Year + "/" + str_MMMM + "/" + str_DDDD;
                lbl_Time.Text = "Time : " + main.DB_All.DB_Info.dbTestTime;
                PrintECU = main.DB_All.DB_Info.dbECUModel;

                cls_Data Standard = new cls_Data(main);
                Standard.Sel_Parameter(main.DB_All.DB_Info.dbCarModel);
            }

            if (PrintECU == "None")
            {
                lbl_WSS0.Visible = false; lblABSB0.Visible = false;     lblError.Visible = false;
                lblWtube.Visible = false; lblDecrease.Visible = false;  lbl_DTCs.Visible = false;     
                lbl_WSSH.Visible = false; lblIncrease.Visible = false;
                                          lblAtube.Visible = false; 

                lblWSS_1.Visible = false; lblWSS_2.Visible = false; lblWSS_3.Visible = false; lblWSS_4.Visible = false; lblWSS_5.Visible = false;
                lblWTub1.Visible = false; lblWTub2.Visible = false; lblWTub3.Visible = false; lblWTub4.Visible = false; lblWTub5.Visible = false;
                lblDec_1.Visible = false; lblDec_2.Visible = false; lblDec_3.Visible = false; lblDec_4.Visible = false; lblDec_5.Visible = false;
                lblInc_1.Visible = false; lblInc_2.Visible = false; lblInc_3.Visible = false; lblInc_4.Visible = false; lblInc_5.Visible = false;
                lblATub1.Visible = false; lblATub2.Visible = false; lblATub3.Visible = false; lblATub4.Visible = false; lblATub5.Visible = false;
            }

            #region Conventional Brake
            int Brk_CNT = main.DB_All.DBBrake.Select(pAcptNo);
            if (Brk_CNT == 1)
            {
                lbl_Wgt1.Text = main.DB_All.DBBrake.db1_Weight.ToString("#0");
                lbl_Wgt2.Text = main.DB_All.DBBrake.db2_Weight.ToString("#0");
                lbl_Wgt3.Text = main.DB_All.DBBrake.dbT_Weight.ToString("#0");
                lbl_Wgt4.Text = main.DB_All.DBBrake.dbT_Weight.ToString("#0");
                lbl_Wgt5.Text = main.DB_All.DBBrake.db1_Wgt__L.ToString("#0") + " / " + main.DB_All.DBBrake.db1_Wgt__R.ToString("#0"); ;
                lbl_Wgt6.Text = main.DB_All.DBBrake.db2_Wgt__L.ToString("#0") + " / " + main.DB_All.DBBrake.db2_Wgt__R.ToString("#0"); ;

                lblLDrg1.Text = main.DB_All.DBBrake.db1Drag__L.ToString("#0");
                lblLDrg2.Text = main.DB_All.DBBrake.db2Drag__L.ToString("#0");
                lblLDrg3.Text = "-";
                lblLDrg4.Text = "-";

                lblRDrg1.Text = main.DB_All.DBBrake.db1Drag__R.ToString("#0");
                lblRDrg2.Text = main.DB_All.DBBrake.db2Drag__R.ToString("#0");
                lblRDrg3.Text = "-";
                lblRDrg4.Text = "-";

                lblSDrg1.Text = main.DB_All.DBBrake.db1Drag__V.ToString("#0.0"); lblSDrg1.ForeColor = (main.DB_All.DBBrake.db1Drag_OX == H2Y.OK) ? Color.Black : Color.Red;
                lblSDrg2.Text = main.DB_All.DBBrake.db2Drag__V.ToString("#0.0"); lblSDrg2.ForeColor = (main.DB_All.DBBrake.db2Drag_OX == H2Y.OK) ? Color.Black : Color.Red;
                lblSDrg3.Text = "-";
                lblSDrg4.Text = "-";

                lblLBrk1.Text = main.DB_All.DBBrake.db1Brake_L.ToString("#0");
                lblLBrk2.Text = main.DB_All.DBBrake.db2Brake_L.ToString("#0");
                lblLBrk3.Text = main.DB_All.DBBrake.dbAPark__L.ToString("#0");
                lblLBrk4.Text = main.DB_All.DBBrake.dbTBrake_L.ToString("#0");

                lblRBrk1.Text = main.DB_All.DBBrake.db1Brake_R.ToString("#0");
                lblRBrk2.Text = main.DB_All.DBBrake.db2Brake_R.ToString("#0");
                lblRBrk3.Text = main.DB_All.DBBrake.dbAPark__R.ToString("#0");
                lblRBrk4.Text = main.DB_All.DBBrake.dbTBrake_R.ToString("#0");

                lblSBrk1.Text = main.DB_All.DBBrake.db1Sum___V.ToString("#0.0"); lblSBrk1.ForeColor = (main.DB_All.DBBrake.db1Sum__OX == H2Y.OK) ? Color.Black : Color.Red;
                lblSBrk2.Text = main.DB_All.DBBrake.db2Sum___V.ToString("#0.0"); lblSBrk2.ForeColor = (main.DB_All.DBBrake.db2Sum__OX == H2Y.OK) ? Color.Black : Color.Red;
                lblSBrk3.Text = main.DB_All.DBBrake.dbAPark__V.ToString("#0.0"); lblSBrk3.ForeColor = (main.DB_All.DBBrake.dbAPark_OX == H2Y.OK) ? Color.Black : Color.Red;
                lblSBrk4.Text = main.DB_All.DBBrake.dbTBrake_V.ToString("#0.0"); lblSBrk4.ForeColor = (main.DB_All.DBBrake.dbTBrakeOX == H2Y.OK) ? Color.Black : Color.Red;

                lblDBrk1.Text = main.DB_All.DBBrake.db1Diff__V.ToString("#0.0"); lblDBrk1.ForeColor = (main.DB_All.DBBrake.db1Diff_OX == H2Y.OK) ? Color.Black : Color.Red;
                lblDBrk2.Text = main.DB_All.DBBrake.db2Diff__V.ToString("#0.0"); lblDBrk2.ForeColor = (main.DB_All.DBBrake.db2Diff_OX == H2Y.OK) ? Color.Black : Color.Red;
                lblDBrk3.Text = "-";
                lblDBrk4.Text = "-";

                lbl_Pan1.Text = main.DB_All.DBBrake.db1BrakeOX; lbl_Pan1.ForeColor = (main.DB_All.DBBrake.db1BrakeOX == H2Y.OK) ? Color.Black : Color.Red;
                lbl_Pan2.Text = main.DB_All.DBBrake.db2BrakeOX; lbl_Pan2.ForeColor = (main.DB_All.DBBrake.db2BrakeOX == H2Y.OK) ? Color.Black : Color.Red;
                lbl_Pan3.Text = main.DB_All.DBBrake.dbAPark_OX; lbl_Pan3.ForeColor = (main.DB_All.DBBrake.dbAPark_OX == H2Y.OK) ? Color.Black : Color.Red;
                lbl_Pan4.Text = main.DB_All.DBBrake.dbTBrakeOX; lbl_Pan4.ForeColor = (main.DB_All.DBBrake.dbTBrakeOX == H2Y.OK) ? Color.Black : Color.Red;
            }
            #endregion

            int RnB_CNT = main.DB_All.DB_RnBs.Select(pAcptNo);
            if (RnB_CNT == 1)
            {
                //Brk_Data.dbAcceptNo
                //Brk_Data.db1_Weight = 0; Brk_Data.db2_Weight = 0;

                #region RnB Data
                lbl__SST.Text = main.DB_All.DB_RnBs.db1SST_Val;
                //lbl__SST.Text = main.DB_All.DB_RnBs.db1SSTSine + " " + main.DB_All.DB_RnBs.db1SST_Val;
                lblSSTOX.Text = main.DB_All.DB_RnBs.db1SSTOkNg;
                lblSSTOX.ForeColor = H2Y.Col_JudgeOX(main.DB_All.DB_RnBs.db1SSTOkNg);

                lblDrag1.Text = main.DB_All.DB_RnBs.db1Drag__L.ToString("#0");
                lblDrag2.Text = main.DB_All.DB_RnBs.db1Drag__R.ToString("#0");
                lblDrag3.Text = main.DB_All.DB_RnBs.db2Drag__L.ToString("#0");
                lblDrag4.Text = main.DB_All.DB_RnBs.db2Drag__R.ToString("#0");

                lblDrag1.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.DragFMin, PSet.RnB.DragFMax, main.DB_All.DB_RnBs.db1Drag__L);
                lblDrag2.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.DragFMin, PSet.RnB.DragFMax, main.DB_All.DB_RnBs.db1Drag__R);
                lblDrag3.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.DragRMin, PSet.RnB.DragRMax, main.DB_All.DB_RnBs.db2Drag__L);
                lblDrag4.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.DragRMin, PSet.RnB.DragRMax, main.DB_All.DB_RnBs.db2Drag__R);

                if (PSet.OwnerS05 == 0 && main.DB_All.DBModel.dbECUModel != ECUs.Chery_1box)
                {
                    if (H2Y.Ret_JudgeOX(PSet.RnB.DragFMin, PSet.RnB.DragFMax, main.DB_All.DB_RnBs.db1Drag__L) == H2Y.OK &&
                        H2Y.Ret_JudgeOX(PSet.RnB.DragFMin, PSet.RnB.DragFMax, main.DB_All.DB_RnBs.db1Drag__R) == H2Y.OK &&
                        H2Y.Ret_JudgeOX(PSet.RnB.DragRMin, PSet.RnB.DragRMax, main.DB_All.DB_RnBs.db2Drag__L) == H2Y.OK &&
                        H2Y.Ret_JudgeOX(PSet.RnB.DragRMin, PSet.RnB.DragRMax, main.DB_All.DB_RnBs.db2Drag__R) == H2Y.OK)
                    {
                        lblDrag5.Text = H2Y.OK;
                        lblDrag5.ForeColor = Color.Black;
                    }
                    else
                    {
                        Test_NGs++;
                        lblDrag5.Text = H2Y.NG;
                        lblDrag5.ForeColor = Color.Red;
                    }
                }

                lblBrk_1.Text = main.DB_All.DB_RnBs.db1Brake_L.ToString("#0");
                lblBrk_2.Text = main.DB_All.DB_RnBs.db1Brake_R.ToString("#0");
                lblBrk_3.Text = main.DB_All.DB_RnBs.db2Brake_L.ToString("#0");
                lblBrk_4.Text = main.DB_All.DB_RnBs.db2Brake_R.ToString("#0");

                lblBrk_1.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.Brk_FMin, PSet.RnB.Brk_FMax, main.DB_All.DB_RnBs.db1Brake_L);
                lblBrk_2.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.Brk_FMin, PSet.RnB.Brk_FMax, main.DB_All.DB_RnBs.db1Brake_R);
                lblBrk_3.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.Brk_RMin, PSet.RnB.Brk_RMax, main.DB_All.DB_RnBs.db2Brake_L);
                lblBrk_4.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.Brk_RMin, PSet.RnB.Brk_RMax, main.DB_All.DB_RnBs.db2Brake_R);

                if (PSet.OwnerS06 == 0 && main.DB_All.DBModel.dbECUModel != ECUs.Chery_1box)
                {
                    if (H2Y.Ret_JudgeOX(PSet.RnB.Brk_FMin, PSet.RnB.Brk_FMax, main.DB_All.DB_RnBs.db1Brake_L) == H2Y.OK &&
                        H2Y.Ret_JudgeOX(PSet.RnB.Brk_FMin, PSet.RnB.Brk_FMax, main.DB_All.DB_RnBs.db1Brake_R) == H2Y.OK &&
                        H2Y.Ret_JudgeOX(PSet.RnB.Brk_RMin, PSet.RnB.Brk_RMax, main.DB_All.DB_RnBs.db2Brake_L) == H2Y.OK &&
                        H2Y.Ret_JudgeOX(PSet.RnB.Brk_RMin, PSet.RnB.Brk_RMax, main.DB_All.DB_RnBs.db2Brake_R) == H2Y.OK)
                    {
                        lblBrk_5.Text = H2Y.OK;
                        lblBrk_5.ForeColor = Color.Black;
                    }
                    else
                    {
                        Test_NGs++;
                        lblBrk_5.Text = H2Y.NG;
                        lblBrk_5.ForeColor = Color.Red;
                    }
                }

                lblPark1.Text = "-"; 
                lblPark2.Text = "-";
                lblPark3.Text = main.DB_All.DB_RnBs.db2Park__L.ToString("#0");
                lblPark4.Text = main.DB_All.DB_RnBs.db2Park__R.ToString("#0");

                lblPark3.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.Park_Min, PSet.RnB.Park_Max, main.DB_All.DB_RnBs.db2Park__L);
                lblPark4.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.Park_Min, PSet.RnB.Park_Max, main.DB_All.DB_RnBs.db2Park__R);

                if (PSet.OwnerS07 == 0 && main.DB_All.DBModel.dbECUModel != ECUs.Chery_1box)
                {
                    if (H2Y.Ret_JudgeOX(PSet.RnB.Park_Min, PSet.RnB.Park_Max, main.DB_All.DB_RnBs.db2Park__L) == H2Y.OK &&
                        H2Y.Ret_JudgeOX(PSet.RnB.Park_Min, PSet.RnB.Park_Max, main.DB_All.DB_RnBs.db2Park__R) == H2Y.OK)
                    {
                        lblPark5.Text = H2Y.OK;
                        lblPark5.ForeColor = Color.Black;
                    }
                    else
                    {
                        Test_NGs++;
                        lblPark5.Text = H2Y.NG;
                        lblPark5.ForeColor = Color.Red;
                    }
                }

                main.DB_All.DB_RnBs.db1Balance = H2Y.Dbl_Balance(main.DB_All.DB_RnBs.db1Brake_L, main.DB_All.DB_RnBs.db1Brake_R);
                main.DB_All.DB_RnBs.db2Balance = H2Y.Dbl_Balance(main.DB_All.DB_RnBs.db2Brake_L, main.DB_All.DB_RnBs.db2Brake_R);
                main.DB_All.DB_RnBs.db_BalForR = H2Y.Dbl_Balance2((main.DB_All.DB_RnBs.db2Brake_L + main.DB_All.DB_RnBs.db2Brake_R), (main.DB_All.DB_RnBs.db1Brake_L + main.DB_All.DB_RnBs.db1Brake_R));

                if (PSet.OwnerS09 == 0 && main.DB_All.DBModel.dbECUModel != ECUs.Chery_1box)
                {
                    if (H2Y.Ret_JudgeOX(PSet.RnB.Bal_FMin, PSet.RnB.Bal_FMax, main.DB_All.DB_RnBs.db1Balance) == H2Y.OK &&
                        H2Y.Ret_JudgeOX(PSet.RnB.Bal_RMin, PSet.RnB.Bal_RMax, main.DB_All.DB_RnBs.db2Balance) == H2Y.OK &&
                        H2Y.Ret_JudgeOX(PSet.RnB.Bal_AMin, PSet.RnB.Bal_AMax, main.DB_All.DB_RnBs.db_BalForR) == H2Y.OK)
                    {
                        main.DB_All.DB_RnBs.db_Balance = H2Y.OK;
                        lblBal_5.ForeColor = Color.Black;
                    }
                    else
                    {
                        Test_NGs++;
                        main.DB_All.DB_RnBs.db_Balance = H2Y.NG;
                        lblBal_5.ForeColor = Color.Red;
                    }
                }

                lblBal_1.Text = main.DB_All.DB_RnBs.db1Balance.ToString("#0.0");
                lblBal_2.Text = main.DB_All.DB_RnBs.db2Balance.ToString("#0.0");
                lblBal_3.Text = main.DB_All.DB_RnBs.db_BalForR.ToString("#0.0");
                lblBal_4.Text = "";
                lblBal_5.Text = main.DB_All.DB_RnBs.db_Balance.ToString();

                lblBal_1.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.Bal_FMin, PSet.RnB.Bal_FMax, main.DB_All.DB_RnBs.db1Balance);
                lblBal_2.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.Bal_RMin, PSet.RnB.Bal_RMax, main.DB_All.DB_RnBs.db2Balance);
                lblBal_3.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.Bal_AMin, PSet.RnB.Bal_AMax, main.DB_All.DB_RnBs.db_BalForR);

                if (H2Y.Ret_JudgeOX(PSet.RnB.Bal_FMin, PSet.RnB.Bal_FMax, main.DB_All.DB_RnBs.db1Balance) != H2Y.OK) { Test_NGs++; }
                if (H2Y.Ret_JudgeOX(PSet.RnB.Bal_RMin, PSet.RnB.Bal_RMax, main.DB_All.DB_RnBs.db2Balance) != H2Y.OK) { Test_NGs++; }
                if (H2Y.Ret_JudgeOX(PSet.RnB.Bal_AMin, PSet.RnB.Bal_AMax, main.DB_All.DB_RnBs.db_BalForR) != H2Y.OK) { Test_NGs++; }

                main.DB_All.DB_RnBs.dbSMT_OkNg = H2Y.Ret_JudgeOX(PSet.RnB.SpeedMin, PSet.RnB.SpeedMax, main.DB_All.DB_RnBs.dbSMTValue);
                lblSpdOX.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.SpeedMin, PSet.RnB.SpeedMax, main.DB_All.DB_RnBs.dbSMTValue);

                lblSpeed.Text = main.DB_All.DB_RnBs.dbSMTValue.ToString("#0.0");
                lblSpeed.ForeColor = H2Y.Col_JudgeOX(PSet.RnB.SpeedMin, PSet.RnB.SpeedMax, main.DB_All.DB_RnBs.dbSMTValue);

                if (PSet.OwnerS08 == 0)
                {
                    lblSpdOX.Text = main.DB_All.DB_RnBs.dbSMT_OkNg.ToString();

                    if (H2Y.Ret_JudgeOX(PSet.RnB.SpeedMin, PSet.RnB.SpeedMax, main.DB_All.DB_RnBs.dbSMTValue) != H2Y.OK)
                    {
                        Test_NGs++;
                    }
                }
                #endregion

                #region ECU Data
                if (PrintECU != "None")
                {
                    lblWSS_1.Text = main.DB_All.DB_RnBs.db1SenSpdL.ToString("#0.0");
                    lblWSS_2.Text = main.DB_All.DB_RnBs.db1SenSpdR.ToString("#0.0");
                    lblWSS_3.Text = main.DB_All.DB_RnBs.db2SenSpdL.ToString("#0.0");
                    lblWSS_4.Text = main.DB_All.DB_RnBs.db2SenSpdR.ToString("#0.0");

                    lblWSS_1.ForeColor = H2Y.Col_JudgeOX(PSet.ECU.WSSFLMin, PSet.ECU.WSSFLMax, main.DB_All.DB_RnBs.db1SenSpdL);
                    lblWSS_2.ForeColor = H2Y.Col_JudgeOX(PSet.ECU.WSSFRMin, PSet.ECU.WSSFRMax, main.DB_All.DB_RnBs.db1SenSpdR);
                    lblWSS_3.ForeColor = H2Y.Col_JudgeOX(PSet.ECU.WSSRLMin, PSet.ECU.WSSRLMax, main.DB_All.DB_RnBs.db2SenSpdL);
                    lblWSS_4.ForeColor = H2Y.Col_JudgeOX(PSet.ECU.WSSRRMin, PSet.ECU.WSSRRMax, main.DB_All.DB_RnBs.db2SenSpdR);

                    if (PSet.OwnerS0A == 0)
                    {
                        if (H2Y.Ret_JudgeOX(PSet.ECU.WSSFLMin, PSet.ECU.WSSFLMax, main.DB_All.DB_RnBs.db1SenSpdL) == H2Y.OK &&
                            H2Y.Ret_JudgeOX(PSet.ECU.WSSFRMin, PSet.ECU.WSSFRMax, main.DB_All.DB_RnBs.db1SenSpdR) == H2Y.OK &&
                            H2Y.Ret_JudgeOX(PSet.ECU.WSSRLMin, PSet.ECU.WSSRLMax, main.DB_All.DB_RnBs.db2SenSpdL) == H2Y.OK &&
                            H2Y.Ret_JudgeOX(PSet.ECU.WSSRRMin, PSet.ECU.WSSRRMax, main.DB_All.DB_RnBs.db2SenSpdR) == H2Y.OK)
                        {
                            lblWSS_5.Text = H2Y.OK;
                            lblWSS_5.ForeColor = Color.Black;
                        }
                        else
                        {
                            Test_NGs++;
                            lblWSS_5.Text = H2Y.NG;
                            lblWSS_5.ForeColor = Color.Red;
                        }
                    }

                    lblWTub1.Text = PSet.OwnerSFL.ToString("#0.0");
                    lblWTub2.Text = PSet.OwnerSFR.ToString("#0.0");
                    lblWTub3.Text = PSet.OwnerSRL.ToString("#0.0");
                    lblWTub4.Text = PSet.OwnerSRR.ToString("#0.0");
                    lblWTub5.Text = "";

                    lblDec_1.Text = main.DB_All.DB_RnBs.db1ABS_DeL.ToString("#0");
                    lblDec_2.Text = main.DB_All.DB_RnBs.db1ABS_DeR.ToString("#0");
                    lblDec_3.Text = main.DB_All.DB_RnBs.db2ABS_DeL.ToString("#0");
                    lblDec_4.Text = main.DB_All.DB_RnBs.db2ABS_DeR.ToString("#0");

                    lblDec_1.ForeColor = H2Y.Col_JudgeOX(PSet.ECU.Dec_FMin, PSet.ECU.Dec_FMax, main.DB_All.DB_RnBs.db1ABS_DeL);
                    lblDec_2.ForeColor = H2Y.Col_JudgeOX(PSet.ECU.Dec_FMin, PSet.ECU.Dec_FMax, main.DB_All.DB_RnBs.db1ABS_DeR);
                    lblDec_3.ForeColor = H2Y.Col_JudgeOX(PSet.ECU.Dec_RMin, PSet.ECU.Dec_RMax, main.DB_All.DB_RnBs.db2ABS_DeL);
                    lblDec_4.ForeColor = H2Y.Col_JudgeOX(PSet.ECU.Dec_RMin, PSet.ECU.Dec_RMax, main.DB_All.DB_RnBs.db2ABS_DeR);

                    if (PSet.OwnerS0B == 0)
                    {
                        if (H2Y.Ret_JudgeOX(PSet.ECU.Dec_FMin, PSet.ECU.Dec_FMax, main.DB_All.DB_RnBs.db1ABS_DeL) == H2Y.OK &&
                            H2Y.Ret_JudgeOX(PSet.ECU.Dec_FMin, PSet.ECU.Dec_FMax, main.DB_All.DB_RnBs.db1ABS_DeR) == H2Y.OK &&
                            H2Y.Ret_JudgeOX(PSet.ECU.Dec_RMin, PSet.ECU.Dec_RMax, main.DB_All.DB_RnBs.db2ABS_DeL) == H2Y.OK &&
                            H2Y.Ret_JudgeOX(PSet.ECU.Dec_RMin, PSet.ECU.Dec_RMax, main.DB_All.DB_RnBs.db2ABS_DeR) == H2Y.OK)
                        {
                            lblDec_5.Text = H2Y.OK;
                            lblDec_5.ForeColor = Color.Black;
                        }
                        else
                        {
                            Test_NGs++;
                            lblDec_5.Text = H2Y.NG;
                            lblDec_5.ForeColor = Color.Red;
                        }
                    }

                    lblInc_1.Text = main.DB_All.DB_RnBs.db1ABS_InL.ToString("#0");
                    lblInc_2.Text = main.DB_All.DB_RnBs.db1ABS_InR.ToString("#0");
                    lblInc_3.Text = main.DB_All.DB_RnBs.db2ABS_InL.ToString("#0");
                    lblInc_4.Text = main.DB_All.DB_RnBs.db2ABS_InR.ToString("#0");

                    lblInc_1.ForeColor = H2Y.Col_JudgeOX(PSet.ECU.Inc_FMin, PSet.ECU.Inc_FMax, main.DB_All.DB_RnBs.db1ABS_InL);
                    lblInc_2.ForeColor = H2Y.Col_JudgeOX(PSet.ECU.Inc_FMin, PSet.ECU.Inc_FMax, main.DB_All.DB_RnBs.db1ABS_InR);
                    lblInc_3.ForeColor = H2Y.Col_JudgeOX(PSet.ECU.Inc_RMin, PSet.ECU.Inc_RMax, main.DB_All.DB_RnBs.db2ABS_InL);
                    lblInc_4.ForeColor = H2Y.Col_JudgeOX(PSet.ECU.Inc_RMin, PSet.ECU.Inc_RMax, main.DB_All.DB_RnBs.db2ABS_InR);

                    if (PSet.OwnerS0C == 0)
                    {
                        if (H2Y.Ret_JudgeOX(PSet.ECU.Inc_FMin, PSet.ECU.Inc_FMax, main.DB_All.DB_RnBs.db1ABS_InL) == H2Y.OK &&
                            H2Y.Ret_JudgeOX(PSet.ECU.Inc_FMin, PSet.ECU.Inc_FMax, main.DB_All.DB_RnBs.db1ABS_InR) == H2Y.OK &&
                            H2Y.Ret_JudgeOX(PSet.ECU.Inc_RMin, PSet.ECU.Inc_RMax, main.DB_All.DB_RnBs.db2ABS_InL) == H2Y.OK &&
                            H2Y.Ret_JudgeOX(PSet.ECU.Inc_RMin, PSet.ECU.Inc_RMax, main.DB_All.DB_RnBs.db2ABS_InR) == H2Y.OK)
                        {
                            lblInc_5.Text = H2Y.OK;
                            lblInc_5.ForeColor = Color.Black;
                        }
                        else
                        {
                            Test_NGs++;
                            lblInc_5.Text = H2Y.NG;
                            lblInc_5.ForeColor = Color.Red;
                        }
                    }

                    lblATub1.Text = "";
                    lblATub2.Text = "";
                    lblATub3.Text = "";
                    lblATub4.Text = "";
                    lblATub5.Text = "";
                }
                #endregion
            }

            if (RnB_CNT == 1) { lbl_Msgs.Visible = true; }
            if (lbl_Msgs.Visible == true)
            {
                if (TSet.StopFlag == 0)
                {
                    lbl_Msgs.Text = (Test_NGs == 0 ? H2Y.OK : "Fail");
                }
                else
                {
                    lbl_Msgs.Text = "Test Stop";
                }
            }

            int ECUs_CNT = main.DB_All.DB_ECUs.Select(pAcptNo);

            if (ECUs_CNT == 1)
            {
                lbl_DTCs.Text = main.DB_All.DB_ECUs.dbDTC_Read;
            }


            if (main.DB_All.DBModel.dbECUModel == ECUs.Chery_1box)
            {
                lblDrag1.Text = "" ; lblDrag2.Text = "" ; lblDrag3.Text = "" ; lblDrag4.Text = "" ; lblDrag5.Text = "" ;
                lblBrk_1.Text = "" ; lblBrk_2.Text = "" ; lblBrk_3.Text = "" ; lblBrk_4.Text = "" ; lblBrk_5.Text = "" ;
                lblPark1.Text = ""; lblPark2.Text = ""; lblPark3.Text = ""; lblPark4.Text = ""; lblPark5.Text = "";
                lblBal_1.Text = ""; lblBal_2.Text = ""; lblBal_3.Text = ""; lblBal_4.Text = ""; lblBal_5.Text = "";
                
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Prints_Order();
        }

        private void Prints_Order()
        {
            try
            {
                printFont = new Font("Arial", 10);
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // The PrintPage event is raised for each page to be printed.
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float YPos = 0;

            Font drawFont = new Font("Arial", 20, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            StringFormat drawFormat = new StringFormat();

            drawFormat.Alignment = StringAlignment.Center;
            RectangleF drawRect = Rect_String(50, 50, 700, 100);
            ev.Graphics.DrawString("Report", drawFont, Brushes.Black, drawRect, drawFormat);

            drawFont = new Font("Arial", 14, FontStyle.Bold);
            drawFormat.Alignment = StringAlignment.Center;
            drawRect = Rect_String(50, 90, 150, 100);
            ev.Graphics.DrawString("Vin No : ", drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(50, 120, 150, 100);
            ev.Graphics.DrawString("Model : ", drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(150, 90, 250, 100);
            ev.Graphics.DrawString(lblVinNo.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(150, 120, 250, 100);
            ev.Graphics.DrawString(lblModel.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawFormat.Alignment = StringAlignment.Near;
            drawRect = Rect_String(550, 90, 250, 100);
            ev.Graphics.DrawString(lbl_Date.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(550, 120, 250, 100);
            ev.Graphics.DrawString(lbl_Time.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawFormat.Alignment = StringAlignment.Center;
            drawFont = new Font("Arial", 12, FontStyle.Regular);
            drawRect = Rect_String(50, 105, 120, 30);

            YPos = 90;

            #region Conventional Brake
            #region Graph Box
            drawFont = new Font("Arial", 14, FontStyle.Bold);
            drawFormat.Alignment = StringAlignment.Near;
            drawRect = Rect_String(50, 160, 250, 100);
            ev.Graphics.DrawString(" - Conventional Brake ", drawFont, Brushes.Black, drawRect, drawFormat);

            Paint_Line(ev,new Pen(Color.Black, 1.5F), 50, 100 + YPos, 800, 100 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 260, 130 + YPos, 730, 130 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 50, 160 + YPos, 800, 160 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 50, 190 + YPos, 800, 190 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 50, 220 + YPos, 800, 220 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 50, 250 + YPos, 800, 250 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 50, 280 + YPos, 800, 280 + YPos);

            Paint_Line(ev,new Pen(Color.Black, 1.5F), 50, 100 + YPos, 50, 280 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 130, 100 + YPos, 130, 280 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 260, 100 + YPos, 260, 280 + YPos);

            Paint_Line(ev,new Pen(Color.Black, 1), 330, 130 + YPos, 330, 280 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 400, 130 + YPos, 400, 280 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 470, 100 + YPos, 470, 280 + YPos);

            Paint_Line(ev,new Pen(Color.Black, 1), 540, 130 + YPos, 540, 280 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 610, 130 + YPos, 610, 280 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 680, 130 + YPos, 680, 280 + YPos);

            Paint_Line(ev,new Pen(Color.Black, 1.5F), 730, 100 + YPos, 730, 280 + YPos);

            Paint_Line(ev,new Pen(Color.Black, 1.5F), 800, 100 + YPos, 800, 280 + YPos);
            #endregion

            #region Graph Box Header
            drawFormat.Alignment = StringAlignment.Center;
            ev.Graphics.DrawString("", drawFont, Brushes.Black, drawRect, drawFormat);
            
            drawRect = Rect_String(50, 165 + YPos, 80, 30);
            ev.Graphics.DrawString(lblHead0.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(50, 195 + YPos, 80, 30);
            ev.Graphics.DrawString(lblHead1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(50, 225 + YPos, 80, 30);
            ev.Graphics.DrawString(lblHead2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(50, 255 + YPos, 80, 30);
            ev.Graphics.DrawString(lblHead3.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(130, 130 + YPos, 130, 60);
            ev.Graphics.DrawString(lblHead4.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(260, 100 + YPos, 210, 30);
            ev.Graphics.DrawString(lblHead5.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawFont = new Font("Arial", 10, FontStyle.Regular);            
            drawRect = Rect_String(260, 130 + YPos, 70, 30);
            ev.Graphics.DrawString(lblHead6.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(330, 130 + YPos, 70, 30);
            ev.Graphics.DrawString(lblHead7.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(400, 130 + YPos, 70, 30);
            ev.Graphics.DrawString(lblHead8.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawFont = new Font("Arial", 14, FontStyle.Bold);
            drawRect = Rect_String(470, 100 + YPos, 280, 30);
            ev.Graphics.DrawString(lblHead9.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawFont = new Font("Arial", 10, FontStyle.Regular);        
            drawRect = Rect_String(470, 130 + YPos, 70, 30);
            ev.Graphics.DrawString(lblHeadA.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(540, 130 + YPos, 70, 30);
            ev.Graphics.DrawString(lblHeadB.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(610, 130 + YPos, 70, 30);
            ev.Graphics.DrawString(lblHeadC.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(680, 130 + YPos, 50, 30);
            ev.Graphics.DrawString(lblHeadD.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawFont = new Font("Arial", 14, FontStyle.Bold);
            drawRect = Rect_String(730, 130 + YPos, 70, 30);
            ev.Graphics.DrawString(lblHeadE.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            #endregion

            #region Weight
            drawFormat.Alignment = StringAlignment.Center;
            drawFont = new Font("Arial", 8, FontStyle.Regular);
            drawRect = Rect_String(130, 169 + YPos, 70, 30);
            ev.Graphics.DrawString(lbl_Wgt5.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(130, 199 + YPos, 70, 30);
            ev.Graphics.DrawString(lbl_Wgt6.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawFormat.Alignment = StringAlignment.Center;
            drawFont = new Font("Arial", 12, FontStyle.Regular);        
            drawRect = Rect_String(200, 165 + YPos, 60, 30);
            ev.Graphics.DrawString(lbl_Wgt1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(200, 195 + YPos, 60, 30);
            ev.Graphics.DrawString(lbl_Wgt2.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawFormat.Alignment = StringAlignment.Center;
            drawRect = Rect_String(130, 225 + YPos, 130, 30);
            ev.Graphics.DrawString(lbl_Wgt3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(130, 255 + YPos, 130, 30);
            ev.Graphics.DrawString(lbl_Wgt4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            #endregion

            #region Drag
            drawRect = Rect_String(260, 165 + YPos, 70, 30);
            ev.Graphics.DrawString(lblLDrg1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(330, 165 + YPos, 70, 30);
            ev.Graphics.DrawString(lblRDrg1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(400, 165 + YPos, 70, 30);
            ev.Graphics.DrawString(lblSDrg1.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(260, 195 + YPos, 70, 30);
            ev.Graphics.DrawString(lblLDrg2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(330, 195 + YPos, 70, 30);
            ev.Graphics.DrawString(lblRDrg2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(400, 195 + YPos, 70, 30);
            ev.Graphics.DrawString(lblSDrg2.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(260, 225 + YPos, 70, 30);
            ev.Graphics.DrawString(lblLDrg3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(330, 225 + YPos, 70, 30);
            ev.Graphics.DrawString(lblRDrg3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(400, 225 + YPos, 70, 30);
            ev.Graphics.DrawString(lblSDrg3.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(260, 255 + YPos, 70, 30);
            ev.Graphics.DrawString(lblLDrg4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(330, 255 + YPos, 70, 30);
            ev.Graphics.DrawString(lblRDrg4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(400, 255 + YPos, 70, 30);
            ev.Graphics.DrawString(lblSDrg4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            #endregion

            #region Brake
            drawRect = Rect_String(470, 165 + YPos, 70, 30);
            ev.Graphics.DrawString(lblLBrk1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(540, 165 + YPos, 70, 30);
            ev.Graphics.DrawString(lblRBrk1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(610, 165 + YPos, 70, 30);
            ev.Graphics.DrawString(lblSBrk1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(680, 165 + YPos, 50, 30);
            ev.Graphics.DrawString(lblDBrk1.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(470, 195 + YPos, 70, 30);
            ev.Graphics.DrawString(lblLBrk2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(540, 195 + YPos, 70, 30);
            ev.Graphics.DrawString(lblRBrk2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(610, 195 + YPos, 70, 30);
            ev.Graphics.DrawString(lblSBrk2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(680, 195 + YPos, 50, 30);
            ev.Graphics.DrawString(lblDBrk2.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(470, 225 + YPos, 70, 30);
            ev.Graphics.DrawString(lblLBrk3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(540, 225 + YPos, 70, 30);
            ev.Graphics.DrawString(lblRBrk3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(610, 225 + YPos, 70, 30);
            ev.Graphics.DrawString(lblSBrk3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(680, 225 + YPos, 50, 30);
            ev.Graphics.DrawString(lblDBrk3.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(470, 255 + YPos, 70, 30);
            ev.Graphics.DrawString(lblLBrk4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(540, 255 + YPos, 70, 30);
            ev.Graphics.DrawString(lblRBrk4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(610, 255 + YPos, 70, 30);
            ev.Graphics.DrawString(lblSBrk4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(680, 255 + YPos, 50, 30);
            ev.Graphics.DrawString(lblDBrk4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            #endregion

            #region Judge
            drawRect = Rect_String(730, 165 + YPos, 70, 30);
            ev.Graphics.DrawString(lbl_Pan1.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(730, 195 + YPos, 70, 30);
            ev.Graphics.DrawString(lbl_Pan2.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(730, 225 + YPos, 70, 30);
            ev.Graphics.DrawString(lbl_Pan3.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(730, 255 + YPos, 70, 30);
            ev.Graphics.DrawString(lbl_Pan4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            #endregion
            #endregion

            YPos = 310;

            #region RnB Brake
            drawFont = new Font("Arial", 14, FontStyle.Bold);
            drawFormat.Alignment = StringAlignment.Near;
            drawRect = Rect_String(50, 380, 250, 100);
            ev.Graphics.DrawString(" - Roll && Brake ", drawFont, Brushes.Black, drawRect, drawFormat);

            Paint_Line(ev,new Pen(Color.Black, 1.5F), 50, 100 + YPos, 615, 100 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 50, 130 + YPos, 615, 130 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 50, 160 + YPos, 615, 160 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 50, 190 + YPos, 615, 190 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 50, 220 + YPos, 615, 220 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 50, 250 + YPos, 615, 250 + YPos);

            Paint_Line(ev,new Pen(Color.Black, 1.5F), 50, 100 + YPos, 50, 250 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 170, 100 + YPos, 170, 250 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 260, 100 + YPos, 260, 250 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 350, 100 + YPos, 350, 250 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 440, 100 + YPos, 440, 250 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 530, 100 + YPos, 530, 250 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 615, 100 + YPos, 615, 250 + YPos);


            drawFormat.Alignment = StringAlignment.Center;
            ev.Graphics.DrawString("", drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(50, 135 + YPos, 120, 30);
            ev.Graphics.DrawString(lblDrag0.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(50, 165 + YPos, 120, 30);
            ev.Graphics.DrawString(lblBrk_0.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawFont = new Font("Arial", 12, FontStyle.Bold);
            drawRect = Rect_String(50, 195 + YPos, 120, 30);
            ev.Graphics.DrawString(lblPark0.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawFont = new Font("Arial", 14, FontStyle.Bold);
            drawRect = Rect_String(50, 225 + YPos, 120, 30);
            ev.Graphics.DrawString(lblH_SST.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawFont = new Font("Arial", 10, FontStyle.Regular);            
            drawRect = Rect_String(350, 225 + YPos, 90, 30);
            ev.Graphics.DrawString(lblSped0.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawFont = new Font("Arial", 12, FontStyle.Regular);
            drawRect = Rect_String(170, 105 + YPos, 90, 30);
            ev.Graphics.DrawString(lblAxle1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(260, 105 + YPos, 90, 30);
            ev.Graphics.DrawString(lblAxle2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(350, 105 + YPos, 90, 30);
            ev.Graphics.DrawString(lblAxle3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(440, 105 + YPos, 90, 30);
            ev.Graphics.DrawString(lblAxle4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(530, 105 + YPos, 90, 30);
            ev.Graphics.DrawString(lblAxle5.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(170, 135 + YPos, 90, 30);    //Drag Data
            ev.Graphics.DrawString(lblDrag1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(260, 135 + YPos, 90, 30);
            ev.Graphics.DrawString(lblDrag2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(350, 135 + YPos, 90, 30);
            ev.Graphics.DrawString(lblDrag3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(440, 135 + YPos, 90, 30);
            ev.Graphics.DrawString(lblDrag4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(530, 135 + YPos, 90, 30);
            ev.Graphics.DrawString(lblDrag5.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(170, 165 + YPos, 90, 30);    //Brake Data
            ev.Graphics.DrawString(lblBrk_1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(260, 165 + YPos, 90, 30);
            ev.Graphics.DrawString(lblBrk_2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(350, 165 + YPos, 90, 30);
            ev.Graphics.DrawString(lblBrk_3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(440, 165 + YPos, 90, 30);
            ev.Graphics.DrawString(lblBrk_4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(530, 165 + YPos, 90, 30);
            ev.Graphics.DrawString(lblBrk_5.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(170, 195 + YPos, 90, 30);    //Parking Data
            ev.Graphics.DrawString(lblPark1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(260, 195 + YPos, 90, 30);
            ev.Graphics.DrawString(lblPark2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(350, 195 + YPos, 90, 30);
            ev.Graphics.DrawString(lblPark3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(440, 195 + YPos, 90, 30);
            ev.Graphics.DrawString(lblPark4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(530, 195 + YPos, 90, 30);
            ev.Graphics.DrawString(lblPark5.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(170, 225 + YPos, 90, 30);    //Sideslip Data
            ev.Graphics.DrawString(lbl__SST.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(260, 225 + YPos, 90, 30);
            ev.Graphics.DrawString(lblSSTOX.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(440, 225 + YPos, 90, 30);    //Speedometer Data
            ev.Graphics.DrawString(lblSpeed.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(530, 225 + YPos, 90, 30);
            ev.Graphics.DrawString(lblSpdOX.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            //Balance Area ===========================================================================
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 620, 100 + YPos, 800, 100 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 620, 130 + YPos, 800, 130 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 620, 160 + YPos, 800, 160 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 620, 190 + YPos, 800, 190 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 620, 220 + YPos, 800, 220 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 620, 250 + YPos, 800, 250 + YPos);

            Paint_Line(ev,new Pen(Color.Black, 1.5F), 620, 100 + YPos, 620, 250 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1), 710, 130 + YPos, 710, 250 + YPos);
            Paint_Line(ev,new Pen(Color.Black, 1.5F), 800, 100 + YPos, 800, 250 + YPos);

            drawRect = Rect_String(620, 105 + YPos, 180, 30);
            ev.Graphics.DrawString(lblBal_0.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(620, 135 + YPos, 90, 30);
            ev.Graphics.DrawString(lblFront.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(620, 165 + YPos, 90, 30);
            ev.Graphics.DrawString(lbl_Rear.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(620, 195 + YPos, 90, 30);
            ev.Graphics.DrawString(lbl_Alls.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(620, 225 + YPos, 90, 30);
            ev.Graphics.DrawString(lblJudeg.Text, drawFont, Brushes.Black, drawRect, drawFormat);

            drawRect = Rect_String(710, 135 + YPos, 90, 30);    //Balance Data
            ev.Graphics.DrawString(lblBal_1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(710, 165 + YPos, 90, 30);
            ev.Graphics.DrawString(lblBal_2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(710, 195 + YPos, 90, 30);
            ev.Graphics.DrawString(lblBal_3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            drawRect = Rect_String(710, 225 + YPos, 90, 30);
            ev.Graphics.DrawString(lblBal_5.Text, drawFont, Brushes.Black, drawRect, drawFormat);
            //Balance Area ===========================================================================
            #endregion

            if (PrintECU != "None")
            {
                YPos += 10;

                #region ECU Test
                Paint_Line(ev,new Pen(Color.Black, 1.5F), 50, 250 + YPos, 800, 250 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1), 80, 280 + YPos, 620, 280 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1.5F), 620, 280 + YPos, 800, 280 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1.5F), 50, 310 + YPos, 620, 310 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1), 80, 340 + YPos, 620, 340 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1), 80, 370 + YPos, 620, 370 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1.5F), 50, 400 + YPos, 800, 400 + YPos);

                Paint_Line(ev,new Pen(Color.Black, 1.5F), 50, 250 + YPos, 50, 400 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1), 80, 250 + YPos, 80, 400 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1.5F), 170, 250 + YPos, 170, 400 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1), 260, 250 + YPos, 260, 400 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1), 350, 250 + YPos, 350, 400 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1), 440, 250 + YPos, 440, 400 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1.5F), 530, 250 + YPos, 530, 400 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1.5F), 620, 250 + YPos, 620, 400 + YPos);
                Paint_Line(ev,new Pen(Color.Black, 1.5F), 800, 250 + YPos, 800, 400 + YPos);

                //Wheel Sensor Speed =====================================================================
                drawRect = Rect_String(50, 255 + YPos, 30, 30);
                ev.Graphics.DrawString("W", drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(50, 270 + YPos, 30, 30);
                ev.Graphics.DrawString("S", drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(50, 285 + YPos, 30, 30);
                ev.Graphics.DrawString("S", drawFont, Brushes.Black, drawRect, drawFormat);

                drawRect = Rect_String(78, 255 + YPos, 94, 30);
                ev.Graphics.DrawString(lblWtube.Text, drawFont, Brushes.Black, drawRect, drawFormat);

                drawFont = new Font("Arial", 10, FontStyle.Regular);
                drawRect = Rect_String(78, 285 + YPos, 94, 30);
                ev.Graphics.DrawString(lbl_WSSH.Text, drawFont, Brushes.Black, drawRect, drawFormat);

                drawFont = new Font("Arial", 12, FontStyle.Regular);
                drawRect = Rect_String(170, 255 + YPos, 90, 30);    //Wheel Sensor Tube 
                ev.Graphics.DrawString(lblWTub1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(260, 255 + YPos, 90, 30);
                ev.Graphics.DrawString(lblWTub2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(350, 255 + YPos, 90, 30);
                ev.Graphics.DrawString(lblWTub3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(440, 255 + YPos, 90, 30);
                ev.Graphics.DrawString(lblWTub4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(530, 255 + YPos, 90, 30);
                ev.Graphics.DrawString(lblWTub5.Text, drawFont, Brushes.Black, drawRect, drawFormat);

                drawRect = Rect_String(170, 285 + YPos, 90, 30);    //Wheel Sensor Speed
                ev.Graphics.DrawString(lblWSS_1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(260, 285 + YPos, 90, 30);
                ev.Graphics.DrawString(lblWSS_2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(350, 285 + YPos, 90, 30);
                ev.Graphics.DrawString(lblWSS_3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(440, 285 + YPos, 90, 30);
                ev.Graphics.DrawString(lblWSS_4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(530, 285 + YPos, 90, 30);
                ev.Graphics.DrawString(lblWSS_5.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                //Wheel Sensor Speed =====================================================================

                //ABS Brake ==============================================================================
                drawRect = Rect_String(50, 330 + YPos, 30, 30);
                ev.Graphics.DrawString("A", drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(50, 345 + YPos, 30, 30);
                ev.Graphics.DrawString("B", drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(50, 360 + YPos, 30, 30);
                ev.Graphics.DrawString("S", drawFont, Brushes.Black, drawRect, drawFormat);

                drawRect = Rect_String(80, 315 + YPos, 90, 30);
                ev.Graphics.DrawString(lblDecrease.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(80, 345 + YPos, 90, 30);
                ev.Graphics.DrawString(lblIncrease.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(80, 375 + YPos, 90, 30);
                ev.Graphics.DrawString(lblAtube.Text, drawFont, Brushes.Black, drawRect, drawFormat);

                drawRect = Rect_String(170, 315 + YPos, 90, 30);    //ABS Valve Decrease
                ev.Graphics.DrawString(lblDec_1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(260, 315 + YPos, 90, 30);
                ev.Graphics.DrawString(lblDec_2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(350, 315 + YPos, 90, 30);
                ev.Graphics.DrawString(lblDec_3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(440, 315 + YPos, 90, 30);
                ev.Graphics.DrawString(lblDec_4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(530, 315 + YPos, 90, 30);
                ev.Graphics.DrawString(lblDec_5.Text, drawFont, Brushes.Black, drawRect, drawFormat);

                drawRect = Rect_String(170, 345 + YPos, 90, 30);    //ABS Valve Increase
                ev.Graphics.DrawString(lblInc_1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(260, 345 + YPos, 90, 30);
                ev.Graphics.DrawString(lblInc_2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(350, 345 + YPos, 90, 30);
                ev.Graphics.DrawString(lblInc_3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(440, 345 + YPos, 90, 30);
                ev.Graphics.DrawString(lblInc_4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(530, 345 + YPos, 90, 30);
                ev.Graphics.DrawString(lblInc_5.Text, drawFont, Brushes.Black, drawRect, drawFormat);

                drawRect = Rect_String(170, 375 + YPos, 90, 30);    //ABS Valve Tube
                ev.Graphics.DrawString(lblATub1.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(260, 375 + YPos, 90, 30);
                ev.Graphics.DrawString(lblATub2.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(350, 375 + YPos, 90, 30);
                ev.Graphics.DrawString(lblATub3.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(440, 375 + YPos, 90, 30);
                ev.Graphics.DrawString(lblATub4.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                drawRect = Rect_String(530, 375 + YPos, 90, 30);
                ev.Graphics.DrawString(lblATub5.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                //ABS Brake ==============================================================================

                drawRect = Rect_String(620, 255 + YPos, 180, 30);
                ev.Graphics.DrawString(lblError.Text, drawFont, Brushes.Black, drawRect, drawFormat);

                drawRect = Rect_String(620, 285 + YPos, 180, 150);
                ev.Graphics.DrawString(lbl_DTCs.Text, drawFont, Brushes.Black, drawRect, drawFormat);
                #endregion
            }
        }

        private RectangleF Rect_String(float X, float Y, float Width, float Height)
        {
            return new RectangleF((X + PSet.Print__X), (Y + PSet.Print__Y), Width, Height);
        }

        private void Paint_Line(PrintPageEventArgs ev, Pen pen, float x1, float y1, float x2, float y2)
        {
            ev.Graphics.DrawLine(pen, (x1 + PSet.Print__X), (y1 + PSet.Print__Y), (x2 + PSet.Print__X), (y2  +PSet.Print__Y));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CaptureScreen();
            printDocument1.Print();
        }

        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = new System.Drawing.Size(500, 600);
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            memoryImage.Save("");

            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
        }

        private void printDocument1_PrintPage(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);

            e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(100, 150, 250, 250));
        }
    }
}
