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
    public partial class fom_Loss : Form
    {
        public bool IsOpen;

        fom_Main Fom_Main = null;
        
        Bitmap bmp;
        const long SpeedMax = 200;
        bool StartOnf = false;
        bool Free_Onf = false;
        bool Stop_Onf = false;

        public double Labv_Out;

        private Loss_Cal FL_Roll = new Loss_Cal();
        private Loss_Cal FR_Roll = new Loss_Cal();
        private Loss_Cal RL_Roll = new Loss_Cal();
        private Loss_Cal RR_Roll = new Loss_Cal();

        double[] T_Data;
        double[] FLSped; double[] FL_RPM; double[] FL_Kgf;
        double[] FRSped; double[] FR_RPM; double[] FR_Kgf;
        double[] RLSped; double[] RL_RPM; double[] RL_Kgf;
        double[] RRSped; double[] RR_RPM; double[] RR_Kgf;

        string Cal_Path;
        int CalIndex = 0;
        bool Ask_Save = false;

        public fom_Loss()
        {
            InitializeComponent();

            if (PSet.OwnerS00 > PSet.Def_Lang)
            {
                this.Text = PSet.LangLoss[0];       //장비 손실 측정
                gbxSpeed.Text = PSet.LangLoss[1];   //휠 속도
                chk_FL.Text = PSet.LangLoss[2];     //전축 좌
                chk_FR.Text = PSet.LangLoss[3];     //전축 우
                chk_RL.Text = PSet.LangLoss[4];     //후축 좌
                chk_RR.Text = PSet.LangLoss[5];     //후축 우
                gbxWheel.Text = PSet.LangLoss[6];   //휠 컨트롤
                lbl___00.Text = PSet.LangLoss[7];   //회차
                lbl___01.Text = PSet.LangLoss[8];   //최고 속도
                lbl___02.Text = PSet.LangLoss[9];   //시작 속도
                lbl___03.Text = PSet.LangLoss[10];  //종료 속도
                lbl___04.Text = PSet.LangLoss[11];  //전체 시간
                chk_Auto.Text = PSet.LangLoss[12];  //자동 조정
                btnStart.Text = PSet.LangLoss[13];  //시작
                btn_Free.Text = PSet.LangLoss[14];  //롤러 프리
                btn1Free.Text = PSet.LangLoss[14];  //롤러 프리
                btn_Stop.Text = PSet.LangLoss[15];  //정지
                btn1Stop.Text = PSet.LangLoss[15];  //정지
                tabPage1.Text = PSet.LangLoss[16];  //교정 데이터
                tabPage2.Text = PSet.LangLoss[17];  //그래프 데이터
                tabPage3.Text = PSet.LangLoss[18];  //기초 데이터
                btn_Save.Text = PSet.LangLoss[19];  //저장
                btnClear.Text = PSet.LangLoss[20];  //클리어
                btnReSet.Text = PSet.LangLoss[21];  //재설정
                btnClose.Text = PSet.LangLoss[22];  //닫기
            }
        }

        private void fom_Loss_Load(object sender, EventArgs e)
        {
            IsOpen = true;

            Fom_Main = (fom_Main)this.Owner;

            #region DataGridView
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();
            CellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            dgv_FL.TopLeftHeaderCell.Value = chk_FL.Text; dgv_FL.TopLeftHeaderCell.Style = CellStyle;
            dgv_FR.TopLeftHeaderCell.Value = chk_FR.Text; dgv_FR.TopLeftHeaderCell.Style = CellStyle;
            dgv_RL.TopLeftHeaderCell.Value = chk_RL.Text; dgv_RL.TopLeftHeaderCell.Style = CellStyle;
            dgv_RR.TopLeftHeaderCell.Value = chk_RR.Text; dgv_RR.TopLeftHeaderCell.Style = CellStyle;

            dgv_FL.RowHeadersDefaultCellStyle = CellStyle; dgv_FL.ColumnHeadersDefaultCellStyle = CellStyle;
            dgv_FR.RowHeadersDefaultCellStyle = CellStyle; dgv_FR.ColumnHeadersDefaultCellStyle = CellStyle;
            dgv_RL.RowHeadersDefaultCellStyle = CellStyle; dgv_RL.ColumnHeadersDefaultCellStyle = CellStyle;
            dgv_RR.RowHeadersDefaultCellStyle = CellStyle; dgv_RR.ColumnHeadersDefaultCellStyle = CellStyle;

            if (!PSet.OnfOwner)
            {
                tab_Loss.Controls.RemoveByKey("tabPage2");
                tab_Loss.Controls.RemoveByKey("tabPage3");

                dgv_FL.Columns["col1SttS"].DefaultCellStyle = CellStyle; dgv_FL.Columns["col1SttS"].Width = 80;
                dgv_FL.Columns["col1EndS"].DefaultCellStyle = CellStyle; dgv_FL.Columns["col1EndS"].Width = 80;
                dgv_FL.Columns["col1Time"].DefaultCellStyle = CellStyle; dgv_FL.Columns["col1Time"].Width = 80;
                dgv_FL.Columns["col1Loss"].DefaultCellStyle = CellStyle; dgv_FL.Columns["col1Loss"].Width = 140;
                dgv_FL.Columns["col1___M"].DefaultCellStyle = CellStyle; dgv_FL.Columns["col1___M"].Visible = false;
                dgv_FL.Columns["col1___B"].DefaultCellStyle = CellStyle; dgv_FL.Columns["col1___B"].Visible = false;

                dgv_FR.Columns["col2SttS"].DefaultCellStyle = CellStyle; dgv_FR.Columns["col2SttS"].Width = 80;
                dgv_FR.Columns["col2EndS"].DefaultCellStyle = CellStyle; dgv_FR.Columns["col2EndS"].Width = 80;
                dgv_FR.Columns["col2Time"].DefaultCellStyle = CellStyle; dgv_FR.Columns["col2Time"].Width = 80;
                dgv_FR.Columns["col2Loss"].DefaultCellStyle = CellStyle; dgv_FR.Columns["col2Loss"].Width = 140;
                dgv_FR.Columns["col2___M"].DefaultCellStyle = CellStyle; dgv_FR.Columns["col2___M"].Visible = false;
                dgv_FR.Columns["col2___B"].DefaultCellStyle = CellStyle; dgv_FR.Columns["col2___B"].Visible = false;

                dgv_RL.Columns["col3SttS"].DefaultCellStyle = CellStyle; dgv_RL.Columns["col3SttS"].Width = 80;
                dgv_RL.Columns["col3EndS"].DefaultCellStyle = CellStyle; dgv_RL.Columns["col3EndS"].Width = 80;
                dgv_RL.Columns["col3Time"].DefaultCellStyle = CellStyle; dgv_RL.Columns["col3Time"].Width = 80;
                dgv_RL.Columns["col3Loss"].DefaultCellStyle = CellStyle; dgv_RL.Columns["col3Loss"].Width = 140;
                dgv_RL.Columns["col3___M"].DefaultCellStyle = CellStyle; dgv_RL.Columns["col3___M"].Visible = false;
                dgv_RL.Columns["col3___B"].DefaultCellStyle = CellStyle; dgv_RL.Columns["col3___B"].Visible = false;

                dgv_RR.Columns["col4SttS"].DefaultCellStyle = CellStyle; dgv_RR.Columns["col4SttS"].Width = 80;
                dgv_RR.Columns["col4EndS"].DefaultCellStyle = CellStyle; dgv_RR.Columns["col4EndS"].Width = 80;
                dgv_RR.Columns["col4Time"].DefaultCellStyle = CellStyle; dgv_RR.Columns["col4Time"].Width = 80;
                dgv_RR.Columns["col4Loss"].DefaultCellStyle = CellStyle; dgv_RR.Columns["col4Loss"].Width = 140;
                dgv_RR.Columns["col4___M"].DefaultCellStyle = CellStyle; dgv_RR.Columns["col4___M"].Visible = false;
                dgv_RR.Columns["col4___B"].DefaultCellStyle = CellStyle; dgv_RR.Columns["col4___B"].Visible = false;
            }

            PSet.Cal_LossRead();
            FL_Roll = PSet.Loss_FL;
            FR_Roll = PSet.Loss_FR;
            RL_Roll = PSet.Loss_RL;
            RR_Roll = PSet.Loss_RR;

            LossdgvDatas(dgv_FL, FL_Roll);
            LossdgvDatas(dgv_FR, FR_Roll);
            LossdgvDatas(dgv_RL, RL_Roll);
            LossdgvDatas(dgv_RR, RR_Roll);
            #endregion

            Cal_Path = System.Windows.Forms.Application.StartupPath + @"\Cal\log\" + DateTime.Now.ToString("yyyyMMdd");

            cboIndex.Items.Clear();
            cboIndex.Items.Add("1");
            cboIndex.Items.Add("2");
            cboIndex.Items.Add("3");
            cboIndex.SelectedIndex = 0;

            cboYAxle.Items.Clear();
            cboYAxle.Items.Add("Speed km/h");
            cboYAxle.Items.Add("RPM");
            cboYAxle.Items.Add("Force kgf");
            cboYAxle.SelectedIndex = 0;

            cboF_RPM.Items.Clear();
            cboF_RPM.Items.Add("None");
            cboF_RPM.Items.Add("Average");
            cboF_RPM.Items.Add("Sort");
            cboF_RPM.Items.Add("Sort+3Average");
            cboF_RPM.Items.Add("Sort+5Average");
            cboF_RPM.SelectedIndex = NI.Loss.RPM_Filt;

            cboC_RPM.Items.Clear();
            cboC_RPM.Items.Add("5"); cboC_RPM.Items.Add("7"); cboC_RPM.Items.Add("9"); cboC_RPM.Items.Add("11"); cboC_RPM.Items.Add("13"); cboC_RPM.Items.Add("15");
            cboC_RPM.Text = NI.Loss.RPM_Cunt.ToString();

            cboF_Acc.Items.Clear();
            cboF_Acc.Items.Add("None");
            cboF_Acc.Items.Add("Average");
            cboF_Acc.Items.Add("Sort");
            cboF_Acc.Items.Add("Sort+3Average");
            cboF_Acc.Items.Add("Sort+5Average");
            cboF_Acc.SelectedIndex = NI.Loss.Acc_Filt;

            cboC_Acc.Items.Clear();
            cboC_Acc.Items.Add("5"); cboC_Acc.Items.Add("7"); cboC_Acc.Items.Add("9"); cboC_Acc.Items.Add("11"); cboC_Acc.Items.Add("13"); cboC_Acc.Items.Add("15");
            cboC_Acc.Items.Add("17"); cboC_Acc.Items.Add("19"); cboC_Acc.Items.Add("21"); cboC_Acc.Items.Add("23"); cboC_Acc.Items.Add("25"); 
            cboC_Acc.Items.Add("59");
            cboC_Acc.Items.Add("101");
            cboC_Acc.Text = NI.Loss.Acc_Cunt.ToString();

            btnReSet.Visible = PSet.OnfOwner;
            btnReSet.Visible = true;
            tmr_Loop.Enabled = true;
        }
        private void fom_Cals_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((NI.Loss.FL.Speed <= PSet.Stop_Spd) &&
                (NI.Loss.FR.Speed <= PSet.Stop_Spd) &&
                (NI.Loss.RL.Speed <= PSet.Stop_Spd) &&
                (NI.Loss.RR.Speed <= PSet.Stop_Spd))
            {
                tmr_Loop.Enabled = false;
                tmr_Cals.Enabled = false;

                if (Ask_Save)
                {   //"교정 데이터를 저장하시겠습니까?", "저장"
                    if (H2Y.Question(PSet.LangLoss[23], PSet.LangLoss[24])) { Order__Save(); }
                }
                IsOpen = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void chk_Axle_Click(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                switch (((CheckBox)sender).Name)
                {
                    case "chk_FL":
                        if (PLC.DIB104[1] != true)
                        {
                            chk_FL.Checked = false;
                            MessageBox.Show(PSet.LangLoss[25]); //전축 좌 롤러 브레이크 점검
                        }
                        break;

                    case "chk_FR":
                        if (PLC.DIB106[1] != true)
                        {
                            chk_FR.Checked = false;
                            MessageBox.Show(PSet.LangLoss[26]); //전우 좌 롤러 브레이크 점검
                        }
                        break;

                    case "chk_RL":
                        if (PLC.DIB105[1] != true)
                        {
                            chk_RL.Checked = false;
                            MessageBox.Show(PSet.LangLoss[27]); //후축 좌 롤러 브레이크 점검
                        }
                        break;

                    case "chk_RR":
                        if (PLC.DIB107[1] != true)
                        {
                            chk_RR.Checked = false;
                            MessageBox.Show(PSet.LangLoss[28]); //후축 우 롤러 브레이크 점검
                        }
                        break;
                }
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnStart": Order_Start(); break;
                case "btn_Free": Order__Free(); break;
                case "btn1Free": Order__Free(); break;
                case "btn_Stop": Order__Stop(); break;
                case "btn1Stop": Order__Stop(); break;
                case "btn_Read": Order__Read(); break;
                case "btn_Save": Order__Save(); break;
                case "btnClear": Order_Clear(); break;
                case "btnC_Cls": OrderCClear(); break;
                case "btnClose": Order_Close(); break;
                case "btn_Filt": OrderFilter(); break;
                case "btnReSet": Order_Reset(); break;
            }
        }

        private void Order_Start()  //Calibration Start
        {
            if ((NI.Loss.FL.Speed >= 0.1d) || (NI.Loss.FR.Speed >= 0.1d) ||
                (NI.Loss.RL.Speed >= 0.1d) || (NI.Loss.RR.Speed >= 0.1d))
            {
                MessageBox.Show(PSet.LangLoss[29]); //"롤러 움직임이 감지되었습니다."
                return;
            }

            Ask_Save = true;

            int MaxSpeed = (int)(Ret_Values(txtSpeed.Text) * 100 / 1.5);
            bool Start_On = false;

            lbl_FL_T.BackColor = Color.White; lbl_FL_T.Text = "--";
            lbl_FR_T.BackColor = Color.White; lbl_FR_T.Text = "--";
            lbl_RL_T.BackColor = Color.White; lbl_RL_T.Text = "--";
            lbl_RR_T.BackColor = Color.White; lbl_RR_T.Text = "--";

            if (chk_FL.Checked) { Start_On = true; }  //On/Off(0:Off, 1:On)
            if (chk_FR.Checked) { Start_On = true; }  //On/Off(0:Off, 1:On)
            if (chk_RL.Checked) { Start_On = true; }  //On/Off(0:Off, 1:On)
            if (chk_RR.Checked) { Start_On = true; }  //On/Off(0:Off, 1:On)

            if (!PLC.DO.MD__Ready) { MessageBox.Show(PSet.LangLoss[30]); return; } //"준비 모드로 전환"
            if (!PLC.DI.PBCalMode) { MessageBox.Show(PSet.LangLoss[31]); return; } //"교정 모드로 전환"
            if (!Start_On) { MessageBox.Show(PSet.LangLoss[32]); return; }      //"측정 롤을 선택하세요"

            chk_Auto.Enabled = false;
            btnStart.Enabled = false;
            btn_Free.Enabled = true;
            btn1Free.Enabled = true;
            btn_Stop.Enabled = true;
            btn1Stop.Enabled = true;

            StartOnf = true;
            Free_Onf = false;
            Stop_Onf = false;

            #region 20210402 
            //if (chk_FL.Checked && !Free_Onf && !Stop_Onf) 
            //{
            //    PLC.DO.FLMot_Stt = true;               //On/Off(0:Off, 1:On)
            //    //PLC.DO.FLStop = true;               //강제 정지(15Hz이하 작동)
            //    PLC.PLC_Put_D500();
            //    H2Y.Sleep(3000f);
            //}

            //if (chk_FR.Checked && !Free_Onf && !Stop_Onf) 
            //{ 
            //    PLC.DO.FRMot_Stt = true;               //On/Off(0:Off, 1:On)
            //    //PLC.DO.FRStop = true;               //강제 정지(15Hz이하 작동)
            //    PLC.PLC_Put_D500();
            //    H2Y.Sleep(3000f);
            //}

            //if (chk_RL.Checked && !Free_Onf && !Stop_Onf) 
            //{ 
            //    PLC.DO.RLMot_Stt = true;               //On/Off(0:Off, 1:On)
            //    //PLC.DO.RLStop = true;               //강제 정지(15Hz이하 작동)
            //    PLC.PLC_Put_D500();
            //    H2Y.Sleep(3000f);
            //}

            //if (chk_RR.Checked && !Free_Onf && !Stop_Onf)
            //{
            //    PLC.DO.RRMot_Stt = true;               //On/Off(0:Off, 1:On)
            //    //PLC.DO.RRStop = true;               //강제 정지(15Hz이하 작동)
            //    PLC.PLC_Put_D500();
            //}
            #endregion

            tmr_Cals.Enabled = true;
        }
        private void Order__Free()  //Roll Free
        {
            Free_Onf = true;
            btn_Free.Enabled = false;
            btn1Free.Enabled = false;

            { PLC.DO.FLMot_Stt = false; } //On/Off(0:Off, 1:On)
            { PLC.DO.FRMot_Stt = false; } //On/Off(0:Off, 1:On)
            { PLC.DO.RLMot_Stt = false; } //On/Off(0:Off, 1:On
            { PLC.DO.RRMot_Stt = false; } //On/Off(0:Off, 1:On)

            PLC.PLC_Put_D500();
        }
        private void Order__Stop()  //Roll Brake
        {
            Stop_Onf = true;
            btn_Stop.Enabled = false;
            btn1Stop.Enabled = false;

            { PLC.DO.FLMotStop = true; }  //강제 정지(15Hz이하 작동)
            { PLC.DO.FRMotStop = true; }  //강제 정지(15Hz이하 작동)
            { PLC.DO.RLMotStop = true; }  //강제 정지(15Hz이하 작동)
            { PLC.DO.RRMotStop = true; }  //강제 정지(15Hz이하 작동)

            PLC.PLC_Put_D500();
        }
        private void Order__Read()  //Calibration Data Read
        {
            string Cal_File = Cal_Path + @"\Loss-" + CalIndex.ToString() + ".log";
            H2Y.Make_Dir(Cal_Path);

            List<string> Cal_Data = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(Cal_File))
                {
                    while (sr.ReadLine() != null)
                    {
                        Cal_Data.Add(sr.ReadLine());
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Order__Read: " + ex.Message);
            }

            if (Cal_Data.Count > 0)
            {
                T_Data = new double[Cal_Data.Count];
                FLSped = new double[Cal_Data.Count];
                FRSped = new double[Cal_Data.Count];
                RLSped = new double[Cal_Data.Count];
                RRSped = new double[Cal_Data.Count];
                
                FL_RPM = new double[Cal_Data.Count];
                FR_RPM = new double[Cal_Data.Count];
                RL_RPM = new double[Cal_Data.Count];
                RR_RPM = new double[Cal_Data.Count];

                FL_Kgf = new double[Cal_Data.Count];
                FR_Kgf = new double[Cal_Data.Count];
                RL_Kgf = new double[Cal_Data.Count];
                RR_Kgf = new double[Cal_Data.Count];

                int CNT = 0;

                comGraph.XAxisRangeMin = 0;
                comGraph.XAxisRangeMax = 300;
                comGraph.DeletePlot(-1);
                comGraph.UpdatePlot();

                foreach (string CalD in Cal_Data)
                {
                    if (CalD != null)
                    {
                        string[] ArrD = CalD.Split(',');
                        if (ArrD.Length < 13) continue;
                        double dv;

                        T_Data[CNT] = double.TryParse(ArrD[0], out dv) ? dv : 0;
                        FLSped[CNT] = double.TryParse(ArrD[1], out dv) ? dv : 0;
                        FRSped[CNT] = double.TryParse(ArrD[2], out dv) ? dv : 0;
                        RLSped[CNT] = double.TryParse(ArrD[3], out dv) ? dv : 0;
                        RRSped[CNT] = double.TryParse(ArrD[4], out dv) ? dv : 0;

                        FL_RPM[CNT] = double.TryParse(ArrD[5], out dv) ? dv : 0;
                        FR_RPM[CNT] = double.TryParse(ArrD[6], out dv) ? dv : 0;
                        RL_RPM[CNT] = double.TryParse(ArrD[7], out dv) ? dv : 0;
                        RR_RPM[CNT] = double.TryParse(ArrD[8], out dv) ? dv : 0;

                        FL_Kgf[CNT] = double.TryParse(ArrD[9], out dv) ? dv : 0;
                        FR_Kgf[CNT] = double.TryParse(ArrD[10], out dv) ? dv : 0;
                        RL_Kgf[CNT] = double.TryParse(ArrD[11], out dv) ? dv : 0;
                        RR_Kgf[CNT] = double.TryParse(ArrD[12], out dv) ? dv : 0;

                        switch (cboYAxle.SelectedIndex)
                        {
                            case 0: comGraph.PlotChart(0, ref T_Data[CNT], ref FLSped[CNT], 1);
                                    comGraph.PlotChart(1, ref T_Data[CNT], ref FRSped[CNT], 1);
                                    comGraph.PlotChart(2, ref T_Data[CNT], ref RLSped[CNT], 1);
                                    comGraph.PlotChart(3, ref T_Data[CNT], ref RRSped[CNT], 1); break;

                            case 1: comGraph.PlotChart(0, ref T_Data[CNT], ref FL_RPM[CNT], 1);
                                    comGraph.PlotChart(1, ref T_Data[CNT], ref FR_RPM[CNT], 1);
                                    comGraph.PlotChart(2, ref T_Data[CNT], ref RL_RPM[CNT], 1);
                                    comGraph.PlotChart(3, ref T_Data[CNT], ref RR_RPM[CNT], 1); break;

                            case 2: comGraph.PlotChart(0, ref T_Data[CNT], ref FL_Kgf[CNT], 1);
                                    comGraph.PlotChart(1, ref T_Data[CNT], ref FR_Kgf[CNT], 1);
                                    comGraph.PlotChart(2, ref T_Data[CNT], ref RL_Kgf[CNT], 1);
                                    comGraph.PlotChart(3, ref T_Data[CNT], ref RR_Kgf[CNT], 1); break;
                        }

                        CNT++;
                    }
                }

                comGraph.SetChartPause(0);
                comGraph.RefreshAll();
            }
        }
        private void Order__Save()  //Calibration Save
        {
            Ask_Save = false;

            PSet.Loss_FL = FL_Roll;
            PSet.Loss_FR = FR_Roll;
            PSet.Loss_RL = RL_Roll;
            PSet.Loss_RR = RR_Roll;

            PSet.Cal_LossMake();
            PSet.Cal_LossRead();
            FL_Roll = PSet.Loss_FL;
            FR_Roll = PSet.Loss_FR;
            RL_Roll = PSet.Loss_RL;
            RR_Roll = PSet.Loss_RR;

            LossdgvDatas(dgv_FL, FL_Roll);
            LossdgvDatas(dgv_FR, FR_Roll);
            LossdgvDatas(dgv_RL, RL_Roll);
            LossdgvDatas(dgv_RR, RR_Roll);

            H2Y.Make_Dir(Cal_Path);

            Save_LogLoss(Cal_Path + @"\Loss.cal");

            MessageBoxEx.Show(PSet.LangLoss[33]);   //"저장되었습니다."
        }
        private void Order_Clear()  //교정 데이터 클리어
        {
            cboIndex.SelectedIndex = 0;

            if (FL_Roll.Item != null) { FL_Roll.Clear(); LossdgvDatas(dgv_FL, FL_Roll); }
            if (FR_Roll.Item != null) { FR_Roll.Clear(); LossdgvDatas(dgv_FR, FR_Roll); }
            if (RL_Roll.Item != null) { RL_Roll.Clear(); LossdgvDatas(dgv_RL, RL_Roll); }
            if (RR_Roll.Item != null) { RR_Roll.Clear(); LossdgvDatas(dgv_RR, RR_Roll); }
        }
        private void OrderCClear()  //교정 반복 데이터 클리어
        {
            txtCycle.Text = "";
            Logs.Make_LossCal(1, "");    //Cycle 데이터 초기화
        }
        private void Order_Close()  //교정 종료
        {
            this.Close();
        }
        private void OrderFilter()
        {
            NI.Loss.RPM_Filt = cboF_RPM.SelectedIndex;
            if (cboC_RPM.SelectedIndex >= 0) { int tmp; if (int.TryParse(cboC_RPM.Items[cboC_RPM.SelectedIndex].ToString(), out tmp)) NI.Loss.RPM_Cunt = tmp; }
            NI.Loss.Acc_Filt = cboF_Acc.SelectedIndex;
            if (cboC_Acc.SelectedIndex >= 0) { int tmp; if (int.TryParse(cboC_Acc.Items[cboC_Acc.SelectedIndex].ToString(), out tmp)) NI.Loss.Acc_Cunt = tmp; } 
        }
        private void Order_Excel()
        {
            StreamWriter sw = null;

            try
            {
                string Cal_File = Cal_Path + @"\Loss-Cycle.csv";

                H2Y.Make_Dir(Cal_Path);

                FileStream fs = new FileStream(Cal_File, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);

                string str_Data = "Start Speed, End Speed, Time, M, B, Loss,,Start Speed, End Speed, Time, M, B, Loss,,Start Speed, End Speed, Time, M, B, Loss,,Start Speed, End Speed, Time, M, B, Loss";
                sw.WriteLine(str_Data);

                for (int cnt = 0; cnt < FL_Roll.Item.Length; cnt++)
                {
                    str_Data = "";

                    str_Data += FL_Roll.Item[cnt].SpdS + ",";
                    str_Data += FL_Roll.Item[cnt].SpdE + ",";
                    str_Data += FL_Roll.Item[cnt].Time + ",";
                    str_Data += FL_Roll.Item[cnt].ChkM + ",";
                    str_Data += FL_Roll.Item[cnt].ChkB + ",";
                    str_Data += FL_Roll.Item[cnt].Loss + ",,";

                    str_Data += FR_Roll.Item[cnt].SpdS + ",";
                    str_Data += FR_Roll.Item[cnt].SpdE + ",";
                    str_Data += FR_Roll.Item[cnt].Time + ",";
                    str_Data += FR_Roll.Item[cnt].ChkM + ",";
                    str_Data += FR_Roll.Item[cnt].ChkB + ",";
                    str_Data += FR_Roll.Item[cnt].Loss + ",,";

                    str_Data += RL_Roll.Item[cnt].SpdS + ",";
                    str_Data += RL_Roll.Item[cnt].SpdE + ",";
                    str_Data += RL_Roll.Item[cnt].Time + ",";
                    str_Data += RL_Roll.Item[cnt].ChkM + ",";
                    str_Data += RL_Roll.Item[cnt].ChkB + ",";
                    str_Data += RL_Roll.Item[cnt].Loss + ",,";

                    str_Data += RR_Roll.Item[cnt].SpdS + ",";
                    str_Data += RR_Roll.Item[cnt].SpdE + ",";
                    str_Data += RR_Roll.Item[cnt].Time + ",";
                    str_Data += RR_Roll.Item[cnt].ChkM + ",";
                    str_Data += RR_Roll.Item[cnt].ChkB + ",";
                    str_Data += RR_Roll.Item[cnt].Loss + "";

                    sw.WriteLine(str_Data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Excel Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sw != null) { sw.Flush(); sw.Close(); }
            }
        }
        private void Order_Reset()
        {
            using (OpenFileDialog openfile = new OpenFileDialog())
            {
                openfile.FileName = Cal_Path;
                openfile.DefaultExt = ".cal";
                openfile.Filter = "Calibration data (.cal)|*.cal";
                openfile.ShowDialog();
                if (openfile.FileName != "")
                {
                    Read_LogLoss(openfile.FileName);

                    LossdgvDatas(dgv_FL, FL_Roll);
                    LossdgvDatas(dgv_FR, FR_Roll);
                    LossdgvDatas(dgv_RL, RL_Roll);
                    LossdgvDatas(dgv_RR, RR_Roll);
                }
            }
        }

        private void tmr_Loop_Tick(object sender, EventArgs e)
        {           
            lblReady.BackColor = PLC.DO.MD__Ready ? Color.Lime : Color.Red;
            lbl_Mode.BackColor = PLC.DI.PBCalMode ? Color.Lime : Color.Red;

            lblLock1.Visible = PLC.DI.FL_RBFree ? false : true;
            lblLock2.Visible = PLC.DI.FR_RBFree ? false : true;
            lblLock3.Visible = PLC.DI.RL_RBFree ? false : true;
            lblLock4.Visible = PLC.DI.RR_RBFree ? false : true;

            Refresh_Data();

            if (!StartOnf)
            {
                if ((NI.Loss.FL.Speed <= PSet.Stop_Spd) && (NI.Loss.FR.Speed <= PSet.Stop_Spd) &&
                    (NI.Loss.RL.Speed <= PSet.Stop_Spd) && (NI.Loss.RR.Speed <= PSet.Stop_Spd))
                {
                    btn_Free.Enabled = true;
                    btn_Stop.Enabled = true;

                    if (PLC.DO.FLMotStop || PLC.DO.FRMotStop || PLC.DO.RLMotStop || PLC.DO.RRMotStop)
                    {
                        PLC.DO.FLMotStop = false;
                        PLC.DO.FRMotStop = false;
                        PLC.DO.RLMotStop = false;
                        PLC.DO.RRMotStop = false; 
                        PLC.PLC_Put_D500(); //D530
                    }

                    if (PLC.DO.FLMot_Stt || PLC.DO.FRMot_Stt || PLC.DO.RLMot_Stt || PLC.DO.RRMot_Stt)
                    {
                        PLC.DO.FLMot_Stt = false;
                        PLC.DO.FRMot_Stt = false;
                        PLC.DO.RLMot_Stt = false;
                        PLC.DO.RRMot_Stt = false;
                        PLC.PLC_Put_D500(); //D530
                    }

                    chk_Auto.Enabled = true;
                    chk_FL.Enabled = true;
                    chk_FR.Enabled = true;
                    chk_RL.Enabled = true;
                    chk_RR.Enabled = true;
                }
                else
                {
                    if ((NI.Loss.FL.Speed <= PSet.Stop_Spd && NI.Loss.FR.Speed <= PSet.Stop_Spd &&
                         NI.Loss.RL.Speed <= PSet.Stop_Spd && NI.Loss.RR.Speed <= PSet.Stop_Spd))
                    {
                        btnStart.Enabled = true;
                    }
                    else
                    {
                        btnStart.Enabled = false;
                    }

                    chk_Auto.Enabled = false;
                    chk_FL.Enabled = false;
                    chk_FR.Enabled = false;
                    chk_RL.Enabled = false;
                    chk_RR.Enabled = false;
                }
            }
        }

        private void tmr_Cals_Tick(object sender, EventArgs e)
        {
            tmr_Cals.Enabled = false;

            txtSpeed.Enabled = false;
            txtStart.Enabled = false;
            txt_Ends.Enabled = false;
            btnClose.Enabled = false;
            btn_Read.Enabled = false;

            Single Max = Single.Parse(txtStart.Text);
            Single Min = Single.Parse(txt_Ends.Text);

            fom4Gage fom_Gage = new fom4Gage(Min, Max);
            fom_Gage.Show();

            comGraph.XAxisRangeMin = 0;
            comGraph.XAxisRangeMax = 700;

            switch (cboYAxle.SelectedIndex)
            {
                case 0: comGraph.YAxisRangeMin = 0; comGraph.YAxisRangeMax = 200; break;
                case 1: comGraph.YAxisRangeMin = 0; comGraph.YAxisRangeMax = 1000; break;
                case 2: comGraph.YAxisRangeMin = -100; comGraph.YAxisRangeMax = 100; break;
            }

            comGraph.DeletePlot(-1);
            comGraph.Update();
            comGraph.SetChartPause(0);

            PLC.DO.FLMotStop = false;
            PLC.DO.FRMotStop = false;
            PLC.DO.RLMotStop = false;
            PLC.DO.RRMotStop = false; PLC.PLC_Put_D500(); //D530
            PLC.DO.CalAirSol = false; PLC.PLC_312_Puts(); //D312

            try
            {
                Calibrations();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            StartOnf = false;

            PLC.DO.FLMotStop = false;
            PLC.DO.FRMotStop = false;
            PLC.DO.RLMotStop = false;
            PLC.DO.RRMotStop = false; PLC.PLC_Put_D500(); //D530
            PLC.DO.CalAirSol = false; PLC.PLC_312_Puts(); //D312

            fom_Gage.Close();
            btnStart.Enabled = true;
            btn_Free.Enabled = true; btn1Free.Enabled = true;
            btn_Stop.Enabled = true; btn1Stop.Enabled = true;

            txtSpeed.Enabled = true;
            txtStart.Enabled = true;
            txt_Ends.Enabled = true;
            btnClose.Enabled = true;
            btn_Read.Enabled = true;
        }

        private int Ret_Values(string pVal) //String => Int
        {
            if (pVal == "") pVal = "0";
            try
            {
                return int.Parse(pVal);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private void Calibrations() 
        {
            long ReadTick;
            double ReadTime = 0;
            double Gap_Time = 0;
            double Old_Time = 0;
            double Gap_Wait = 3;
            int LossStep = 0;
            bool LossFlag = false;

            bool FL__Free = false, FR__Free = false, RL__Free = false, RR__Free = false;
            bool FL_Start = false, FR_Start = false, RL_Start = false, RR_Start = false;
            bool FLFinish = false, FRFinish = false, RLFinish = false, RRFinish = false;
            double FL__Ofst = 0, FR__Ofst = 0, RL__Ofst = 0, RR__Ofst = 0;
            double FL__Time = 0, FR__Time = 0, RL__Time = 0, RR__Time = 0;
            double FreeOfst = 0;

            double Max_Sped, Stt_Sped, End_Sped;
            if (!double.TryParse(txtSpeed.Text, out Max_Sped)) return;
            if (!double.TryParse(txtStart.Text, out Stt_Sped)) return;
            if (!double.TryParse(txt_Ends.Text, out End_Sped)) return;
            int Idx = cboIndex.SelectedIndex;

            long OfstTick = DateTime.Now.Ticks;
            bool AutoMode = chk_Auto.Checked;
            bool AutoFree = false;
            bool Auto_Brk = false;
            FileStream fs;
            StreamWriter sw;
            string str_Data = "";

            //if (PSet.OnfOwner)
            //{
                Cal_Path = System.Windows.Forms.Application.StartupPath + @"\Cal\log\" + DateTime.Now.ToString("yyyyMMdd");
                H2Y.Make_Dir(Cal_Path);
                string Cal_File = Cal_Path + @"\Loss-" + cboIndex.SelectedIndex.ToString() + ".log";

                fs = new FileStream(Cal_File, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);
                str_Data = "[  Time  ] [FL Speed] [FR Speed] [RL Speed] [RR Speed] [FL   RPM] [FR   RPM] [RL   RPM] [RR   RPM] [FL   Kgf] [FR   Kgf] [RL   Kgf] [RR   Kgf]";

                sw.WriteLine(str_Data);
            //}

            if (AutoMode)
            {
                //btnStart.Enabled = false;
                //btn_Free.Enabled = false;
                //btn_Stop.Enabled = false;

                txtSpeed.Enabled = false;
                txtStart.Enabled = false;
                txt_Ends.Enabled = false;
                btnClose.Enabled = false;
                btn_Read.Enabled = false;
            }

            TSet.LossData_Cls();
            while (true)
            {
                if (LossFlag) { LossFlag = false; }
                ReadTick = (DateTime.Now.Ticks - OfstTick) / 100000;
                ReadTime = (double)ReadTick / 100;

                #region 20210402
                if (LossStep == 0 && !LossFlag)
                {
                    LossFlag = true;
                    if (LossFlag) { LossStep = 1; Gap_Time = ReadTime; }
                }

                if (LossStep == 1 && !LossFlag)
                {
                    if (chk_FL.Checked && !Free_Onf && !Stop_Onf)
                    {
                        PLC.DO.FLMot_Stt = true;               //On/Off(0:Off, 1:On)
                        PLC.PLC_Put_D500();
                        H2Y.Sleep(100f);

                        if (ReadTime - Gap_Time > Gap_Wait) { LossFlag = true; }
                    }
                    else
                    {
                        LossFlag = true; 
                    }

                    if (LossFlag) { LossStep = 2; Gap_Time = ReadTime; }
                }

                if (LossStep == 2 && !LossFlag)
                {
                    if (chk_FR.Checked && !Free_Onf && !Stop_Onf)
                    {
                        PLC.DO.FRMot_Stt = true;               //On/Off(0:Off, 1:On)
                        PLC.PLC_Put_D500();
                        H2Y.Sleep(100f);

                        if (ReadTime - Gap_Time > Gap_Wait) { LossFlag = true; }
                    }
                    else
                    {
                        LossFlag = true; 
                    }

                    if (LossFlag) { LossStep = 3; Gap_Time = ReadTime; }
                }

                if (LossStep == 3 && !LossFlag)
                {
                    if (chk_RL.Checked && !Free_Onf && !Stop_Onf)
                    {
                        PLC.DO.RLMot_Stt = true;               //On/Off(0:Off, 1:On)
                        PLC.PLC_Put_D500();
                        H2Y.Sleep(100f);

                        if (ReadTime - Gap_Time > Gap_Wait) { LossFlag = true; }
                    }
                    else
                    {
                        LossFlag = true; 
                    }

                    if (LossFlag) { LossStep = 4; Gap_Time = ReadTime; }
                }

                if (LossStep == 4 && !LossFlag)
                {
                    if (chk_RR.Checked && !Free_Onf && !Stop_Onf)
                    {
                        PLC.DO.RRMot_Stt = true;               //On/Off(0:Off, 1:On)
                        PLC.PLC_Put_D500();
                        H2Y.Sleep(100f);

                        if (ReadTime - Gap_Time > Gap_Wait) { LossFlag = true; }
                    }
                    else
                    {
                        LossFlag = true; 
                    }

                    if (LossFlag) { LossStep = 5; Gap_Time = ReadTime; }
                }
                #endregion

                if (Free_Onf && (NI.Loss.FL.Speed <= End_Sped && NI.Loss.FR.Speed <= End_Sped &&
                                 NI.Loss.RL.Speed <= End_Sped && NI.Loss.RR.Speed <= End_Sped))
                {
                    btn_Stop.Enabled = true;
                    btn1Stop.Enabled = true;
                }
                else
                {
                    btn_Stop.Enabled = false;
                    btn1Stop.Enabled = false;
                }


                if ((ReadTime - Old_Time) >= NI.Loss.Scan__Hz)
                {
                    Old_Time = ReadTime;
                    Refresh_Data();

                    #region FL_Roll
                    if (chk_FL.Checked)
                    {
                        switch (cboYAxle.SelectedIndex)
                        {
                            case 0: comGraph.PlotChart(0, ref ReadTime, ref NI.Loss.FL.Speed, 1); break;
                            case 1: comGraph.PlotChart(0, ref ReadTime, ref NI.Loss.FL.RPM, 1); break;
                            case 2: comGraph.PlotChart(0, ref ReadTime, ref NI.Loss.FL.Kgf, 1); break;
                        }
                        
                        if (!FL__Free)
                        {
                            if (NI.Loss.FL.Speed >= Max_Sped)
                            {
                                FL__Free = true; lbl_FL_T.BackColor = Color.Gray;
                            }
                        }
                        else
                        {
                            if (!FL_Start)
                            {
                                if (0 < NI.Loss.FL.Speed && NI.Loss.FL.Speed <= Stt_Sped)
                                {
                                    if (NI.Loss.FL.Force < 0) { FL_Start = true; }
                                    FL__Ofst = ReadTime;
                                    lbl_FL_T.ForeColor = Color.Lime;
                                    
                                    TSet.FL = Set1_Point(TSet.FL, NI.Loss.FL);
                                    lbl1RPM1.Text = TSet.FL.RPM__1.ToString("0.0000"); 
                                    lbl1Kgf1.Text = TSet.FL.Force1.ToString("0.00000000");
                                }
                            }
                            else
                            {
                                if (!FLFinish)
                                {
                                    FL__Time = ReadTime - FL__Ofst;
                                    lbl_FL_T.Text = FL__Time.ToString("0.00");

                                    TSet.FL = Set2_Point(TSet.FL, NI.Loss.FL);

                                    lbl1RPM2.Text = TSet.FL.RPM__2.ToString("0.0000"); 
                                    lbl1Kgf2.Text = TSet.FL.Force2.ToString("0.00000000"); 
                                    lbl1___M.Text = TSet.FL.Loss_M.ToString("0.00000000"); 
                                    lbl1___B.Text = TSet.FL.Loss_B.ToString("0.00000000"); 
                                    lbl1Loss.Text = TSet.FL.C_Loss.ToString("0.00000000");

                                    if (0 < NI.Loss.FL.Speed && NI.Loss.FL.Speed <= End_Sped)
                                    {
                                        if (NI.Loss.FL.Force < 0) { FLFinish = true; }
                                        FL__Time = ReadTime - FL__Ofst;
                                        lbl_FL_T.Text = FL__Time.ToString("0.00");
                                        lbl_FL_T.ForeColor = Color.Red;

                                        FL_Roll.Item[Idx] = Loss_Data(FL_Roll.Item[Idx], FL__Time, TSet.FL);
                                        LossdgvDatas(dgv_FL, FL_Roll);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        FL__Free = true;
                    }
                    #endregion

                    #region FR_Roll
                    if (chk_FR.Checked)
                    {
                        switch (cboYAxle.SelectedIndex)
                        {
                            case 0: comGraph.PlotChart(1, ref ReadTime, ref NI.Loss.FR.Speed, 1); break;
                            case 1: comGraph.PlotChart(1, ref ReadTime, ref NI.Loss.FR.RPM, 1); break;
                            case 2: comGraph.PlotChart(1, ref ReadTime, ref NI.Loss.FR.Kgf, 1); break;
                        }
                        
                        if (!FR__Free)
                        {
                            if (NI.Loss.FR.Speed >= Max_Sped)
                            {
                                FR__Free = true; lbl_FR_T.BackColor = Color.Gray;
                            }
                        }
                        else
                        {
                            if (!FR_Start)
                            {
                                if (0 < NI.Loss.FR.Speed && NI.Loss.FR.Speed <= Stt_Sped)
                                {
                                    if (NI.Loss.FR.Force < 0) { FR_Start = true; }
                                    FR__Ofst = ReadTime;
                                    lbl_FR_T.ForeColor = Color.Lime;

                                    TSet.FR = Set1_Point(TSet.FR, NI.Loss.FR);
                                    lbl2RPM1.Text = TSet.FR.RPM__1.ToString("0.0000"); 
                                    lbl2Kgf1.Text = TSet.FR.Force1.ToString("0.00000000"); 
                                }
                            }
                            else
                            {
                                if (!FRFinish)
                                {
                                    FR__Time = ReadTime - FR__Ofst;
                                    lbl_FR_T.Text = FR__Time.ToString("0.00");

                                    TSet.FR = Set2_Point(TSet.FR, NI.Loss.FR);

                                    lbl2RPM2.Text = TSet.FR.RPM__2.ToString("0.0000"); 
                                    lbl2Kgf2.Text = TSet.FR.Force2.ToString("0.00000000"); 
                                    lbl2___M.Text = TSet.FR.Loss_M.ToString("0.00000000");
                                    lbl2___B.Text = TSet.FR.Loss_B.ToString("0.00000000");
                                    lbl2Loss.Text = TSet.FR.C_Loss.ToString("0.00000000");

                                    if (0 < NI.Loss.FR.Speed && NI.Loss.FR.Speed <= End_Sped)
                                    {
                                        if (NI.Loss.FR.Force < 0) { FRFinish = true; }
                                        FR__Time = ReadTime - FR__Ofst;
                                        lbl_FR_T.Text = FR__Time.ToString("0.00");
                                        lbl_FR_T.ForeColor = Color.Red;

                                        FR_Roll.Item[Idx] = Loss_Data(FR_Roll.Item[Idx], FR__Time, TSet.FR);
                                        LossdgvDatas(dgv_FR, FR_Roll);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        FR__Free = true;
                    }
                    #endregion

                    #region RL_Roll
                    if (chk_RL.Checked)
                    {
                        switch (cboYAxle.SelectedIndex)
                        {
                            case 0: comGraph.PlotChart(2, ref ReadTime, ref NI.Loss.RL.Speed, 1); break;
                            case 1: comGraph.PlotChart(2, ref ReadTime, ref NI.Loss.RL.RPM, 1); break;
                            case 2: comGraph.PlotChart(2, ref ReadTime, ref NI.Loss.RL.Kgf, 1); break;
                        }

                        if (!RL__Free)
                        {
                            if (NI.Loss.RL.Speed >= Max_Sped)
                            {
                                RL__Free = true; lbl_RL_T.BackColor = Color.Gray;
                            }
                        }
                        else
                        {
                            if (!RL_Start)
                            {
                                if (0 < NI.Loss.RL.Speed && NI.Loss.RL.Speed <= Stt_Sped)
                                {
                                    if (NI.Loss.RL.Force < 0) { RL_Start = true; }
                                    RL__Ofst = ReadTime;
                                    lbl_RL_T.ForeColor = Color.Lime;

                                    TSet.RL = Set1_Point(TSet.RL, NI.Loss.RL);
                                    lbl3RPM1.Text = TSet.RL.RPM__1.ToString("0.0000");
                                    lbl3Kgf1.Text = TSet.RL.Force1.ToString("0.00000000"); 
                                }
                            }
                            else
                            {
                                if (!RLFinish)
                                {
                                    RL__Time = ReadTime - RL__Ofst;
                                    lbl_RL_T.Text = RL__Time.ToString("0.00");

                                    TSet.RL = Set2_Point(TSet.RL, NI.Loss.RL);

                                    lbl3RPM2.Text = TSet.RL.RPM__2.ToString("0.0000"); 
                                    lbl3Kgf2.Text = TSet.RL.Force2.ToString("0.00000000");
                                    lbl3___M.Text = TSet.RL.Loss_M.ToString("0.00000000");
                                    lbl3___B.Text = TSet.RL.Loss_B.ToString("0.00000000");
                                    lbl3Loss.Text = TSet.RL.C_Loss.ToString("0.00000000");

                                    if (0 < NI.Loss.RL.Speed && NI.Loss.RL.Speed <= End_Sped)
                                    {
                                        if (NI.Loss.RL.Force < 0) { RLFinish = true; }
                                        RL__Time = ReadTime - RL__Ofst;
                                        lbl_RL_T.Text = RL__Time.ToString("0.00");
                                        lbl_RL_T.ForeColor = Color.Red;

                                        RL_Roll.Item[Idx] = Loss_Data(RL_Roll.Item[Idx], RL__Time, TSet.RL);
                                        LossdgvDatas(dgv_RL, RL_Roll);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        RL__Free = true;
                    }
                    #endregion

                    #region RR_Roll
                    if (chk_RR.Checked)
                    {
                        switch (cboYAxle.SelectedIndex)
                        {
                            case 0: comGraph.PlotChart(3, ref ReadTime, ref NI.Loss.RR.Speed, 1); break;
                            case 1: comGraph.PlotChart(3, ref ReadTime, ref NI.Loss.RR.RPM, 1); break;
                            case 2: comGraph.PlotChart(3, ref ReadTime, ref NI.Loss.RR.Kgf, 1); break;
                        }

                        if (!RR__Free)
                        {
                            if (NI.Loss.RR.Speed >= Max_Sped)
                            {
                                RR__Free = true; lbl_RR_T.BackColor = Color.Gray;
                            }
                        }
                        else
                        {
                            if (!RR_Start)
                            {
                                if (0 < NI.Loss.RR.Speed && NI.Loss.RR.Speed <= Stt_Sped)
                                {
                                    if (NI.Loss.RR.Force < 0) { RR_Start = true; }
                                    RR__Ofst = ReadTime;
                                    lbl_RR_T.ForeColor = Color.Lime;

                                    TSet.RR = Set1_Point(TSet.RR, NI.Loss.RR);
                                    lbl4RPM1.Text = TSet.RR.RPM__1.ToString("0.0000");
                                    lbl4Kgf1.Text = TSet.RR.Force1.ToString("0.00000000"); 
                                }
                            }
                            else
                            {
                                if (!RRFinish)
                                {
                                    RR__Time = ReadTime - RR__Ofst;
                                    lbl_RR_T.Text = RR__Time.ToString("0.00");

                                    TSet.RR = Set2_Point(TSet.RR, NI.Loss.RR);

                                    lbl4RPM2.Text = TSet.RR.RPM__2.ToString("0.0000"); 
                                    lbl4Kgf2.Text = TSet.RR.Force2.ToString("0.00000000"); 
                                    lbl4___M.Text = TSet.RR.Loss_M.ToString("0.00000000");
                                    lbl4___B.Text = TSet.RR.Loss_B.ToString("0.00000000"); 
                                    lbl4Loss.Text = TSet.RR.C_Loss.ToString("0.00000000");

                                    if (0 < NI.Loss.RR.Speed && NI.Loss.RR.Speed <= End_Sped)
                                    {
                                        if (NI.Loss.RR.Force < 0) { RRFinish = true; }
                                        RR__Time = ReadTime - RR__Ofst;
                                        lbl_RR_T.Text = RR__Time.ToString("0.00");
                                        lbl_RR_T.ForeColor = Color.Red;

                                        RR_Roll.Item[Idx] = Loss_Data(RR_Roll.Item[Idx], RR__Time, TSet.RR);
                                        LossdgvDatas(dgv_RR, RR_Roll);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        RR__Free = true;
                    }
                    #endregion

                    if (AutoMode == true)
                    {
                        if (AutoFree == false)
                        {
                            if (FL__Free == true && FR__Free == true && RL__Free == true && RR__Free == true)
                            {
                                if (ReadTime - FreeOfst > 10)
                                {
                                    AutoFree = true;
                                    Order__Free();
                                }
                            }
                            else
                            {
                                FreeOfst = ReadTime;
                            }
                        }
                        else
                        {
                            if (Auto_Brk == false)
                            {
                                if (NI.Loss.FL.Speed <= (End_Sped - 5) &&
                                    NI.Loss.FR.Speed <= (End_Sped - 5) &&
                                    NI.Loss.RL.Speed <= (End_Sped - 5) &&
                                    NI.Loss.RR.Speed <= (End_Sped - 5))
                                {
                                    Auto_Brk = true;
                                    Order__Stop();
                                }
                            }
                        }
                    }

                    lbl1_Kgf.Text = NI.Loss.FL.Kgf.ToString("0.00000");
                    lbl2_Kgf.Text = NI.Loss.FR.Kgf.ToString("0.00000");
                    lbl3_Kgf.Text = NI.Loss.RL.Kgf.ToString("0.00000");
                    lbl4_Kgf.Text = NI.Loss.RR.Kgf.ToString("0.00000");

                    comGraph.UpdatePlot();

                    lblFTime.Text = ReadTime.ToString("0.00");

                    #region Real Time Data
                    //if (PSet.OnfOwner)
                    //{
                        str_Data = H2Y.DigitToStr(ReadTime.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(NI.Loss.FL.Speed.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(NI.Loss.FR.Speed.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(NI.Loss.RL.Speed.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(NI.Loss.RR.Speed.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(NI.Loss.FL.RPM.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(NI.Loss.FR.RPM.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(NI.Loss.RL.RPM.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(NI.Loss.RR.RPM.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(NI.Loss.FL.Kgf.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(NI.Loss.FR.Kgf.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(NI.Loss.RL.Kgf.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(NI.Loss.RR.Kgf.ToString("#0.0000"), 10);

                        sw.WriteLine(str_Data);
                    //}
                    #endregion
                }
                Application.DoEvents();

                if (Free_Onf)
                {
                    if ((NI.Loss.FL.Speed <= PSet.Stop_Spd) &&
                        (NI.Loss.FR.Speed <= PSet.Stop_Spd) &&
                        (NI.Loss.RL.Speed <= PSet.Stop_Spd) &&
                        (NI.Loss.RR.Speed <= PSet.Stop_Spd)) break;
                }
            }

            CalIndex++;

            lbl_FL_T.BackColor = Color.Black;
            lbl_FR_T.BackColor = Color.Black;
            lbl_RL_T.BackColor = Color.Black;
            lbl_RR_T.BackColor = Color.Black;

            #region Loss Cycle Data
            string strCycle = "";
            lbl1RPM1.Text = TSet.FL.RPM__1.ToString("0.0000");      strCycle += lbl1RPM1.Text + ",";
            lbl1Kgf1.Text = TSet.FL.Force1.ToString("0.00000000");  strCycle += lbl1Kgf1.Text + ",";
            lbl1RPM2.Text = TSet.FL.RPM__2.ToString("0.0000");      strCycle += lbl1RPM2.Text + ",";
            lbl1Kgf2.Text = TSet.FL.Force2.ToString("0.00000000");  strCycle += lbl1Kgf2.Text + ",";

            lbl1___M.Text = TSet.FL.Loss_M.ToString("0.00000000");  strCycle += lbl1___M.Text + ",";
            lbl1___B.Text = TSet.FL.Loss_B.ToString("0.00000000");  strCycle += lbl1___B.Text + ",";
            lbl1Loss.Text = TSet.FL.C_Loss.ToString("0.00000000");  strCycle += lbl1Loss.Text + ",";
            
            lbl2RPM1.Text = TSet.FR.RPM__1.ToString("0.0000");      strCycle += lbl2RPM1.Text + ",";
            lbl2Kgf1.Text = TSet.FR.Force1.ToString("0.00000000");  strCycle += lbl2Kgf1.Text + ",";
            lbl2RPM2.Text = TSet.FR.RPM__2.ToString("0.0000");      strCycle += lbl2RPM2.Text + ",";
            lbl2Kgf2.Text = TSet.FR.Force2.ToString("0.00000000");  strCycle += lbl2Kgf2.Text + ",";

            lbl2___M.Text = TSet.FR.Loss_M.ToString("0.00000000");  strCycle += lbl2___M.Text + ",";
            lbl2___B.Text = TSet.FR.Loss_B.ToString("0.00000000");  strCycle += lbl2___B.Text + ",";
            lbl2Loss.Text = TSet.FR.C_Loss.ToString("0.00000000");  strCycle += lbl2Loss.Text + ",";
            
            lbl3RPM1.Text = TSet.RL.RPM__1.ToString("0.0000");      strCycle += lbl3RPM1.Text + ",";
            lbl3Kgf1.Text = TSet.RL.Force1.ToString("0.00000000");  strCycle += lbl3Kgf1.Text + ",";
            lbl3RPM2.Text = TSet.RL.RPM__2.ToString("0.0000");      strCycle += lbl3RPM2.Text + ",";
            lbl3Kgf2.Text = TSet.RL.Force2.ToString("0.00000000");  strCycle += lbl3Kgf2.Text + ",";

            lbl3___M.Text = TSet.RL.Loss_M.ToString("0.00000000");  strCycle += lbl3___M.Text + ",";
            lbl3___B.Text = TSet.RL.Loss_B.ToString("0.00000000");  strCycle += lbl3___B.Text + ",";
            lbl3Loss.Text = TSet.RL.C_Loss.ToString("0.00000000");  strCycle += lbl3Loss.Text + ",";
            
            lbl4RPM1.Text = TSet.RR.RPM__1.ToString("0.0000");      strCycle += lbl4RPM1.Text + ",";
            lbl4Kgf1.Text = TSet.RR.Force1.ToString("0.00000000");  strCycle += lbl4Kgf1.Text + ",";
            lbl4RPM2.Text = TSet.RR.RPM__2.ToString("0.0000");      strCycle += lbl4RPM2.Text + ",";
            lbl4Kgf2.Text = TSet.RR.Force2.ToString("0.00000000");  strCycle += lbl4Kgf2.Text + ",";

            lbl4___M.Text = TSet.RR.Loss_M.ToString("0.00000000");  strCycle += lbl4___M.Text + ",";
            lbl4___B.Text = TSet.RR.Loss_B.ToString("0.00000000");  strCycle += lbl4___B.Text + ",";
            lbl4Loss.Text = TSet.RR.C_Loss.ToString("0.00000000");  strCycle += lbl4Loss.Text + ",";

            txtCycle.Text += strCycle + "\r";
            Logs.Make_LossCal(0, strCycle);
            #endregion

            sw.Flush();
            sw.Close();
            fs.Close();

            if (cboIndex.SelectedIndex < cboIndex.Items.Count - 1)
            {
                cboIndex.SelectedIndex = cboIndex.SelectedIndex + 1;
            }
        }

        private TSet.wheel_Loss Set1_Point(TSet.wheel_Loss wheel, NI.Wheel scan)
        {
            if (scan.Force < 0)
            {
                wheel.RPM__1 = scan.RPM;
                wheel.Speed1 = scan.Speed;
                wheel.Force1 = scan.Force;
                wheel.Kgf__1 = scan.Kgf;
            }

            return wheel;
        }

        private TSet.wheel_Loss Set2_Point(TSet.wheel_Loss wheel, NI.Wheel scan)
        {
            double imsi_M, imsi_B;

            if (scan.Force < 0)
            {
                wheel.RPM__2 = scan.RPM;
                wheel.Speed2 = scan.Speed;
                wheel.Force2 = scan.Force;
                wheel.Kgf__2 = scan.Kgf;

                imsi_M= (wheel.Force1 - wheel.Force2) / (wheel.RPM__1 - wheel.RPM__2);
                imsi_B = wheel.Force1 - (wheel.Loss_M * wheel.RPM__1);

                if (imsi_M < 0) { wheel.Loss_M = imsi_M; }
                if (imsi_B < 0) { wheel.Loss_B = imsi_B; }
                
                wheel.C_Loss = (wheel.Loss_M * scan.RPM) + wheel.Loss_B;
            }

            return wheel;
        }

        private Loss_Items Loss_Data(Loss_Items loss, double time, TSet.wheel_Loss wheel)
        {
            loss.SpdS = wheel.Speed1;
            loss.SpdE = wheel.Speed2;
            loss.RpmS = wheel.RPM__1;
            loss.RpmE = wheel.RPM__2;
            loss.Time = time;
            loss.ChkM = wheel.Loss_M;
            loss.ChkB = wheel.Loss_B;
            loss.Loss = wheel.C_Loss;
            
            return loss;
        }
        
        private void LossdgvDatas(DataGridView dgv, Loss_Cal Roll)
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

            dgv.Rows.Clear();
            
            for (int cnt = 0; cnt < Roll.Item.Length; cnt++)
            {
                if (Roll.Item[cnt].SpdS > 0)
                {
                    string[] rows = { Roll.Item[cnt].SpdS.ToString("0.0"),
                                      Roll.Item[cnt].SpdE.ToString("0.0"),
                                      //Roll.Item[cnt].RpmS.ToString("0.0"),
                                      //Roll.Item[cnt].RpmE.ToString("0.0"),
                                      Roll.Item[cnt].Time.ToString("0.00"),
                                      Roll.Item[cnt].ChkM.ToString("0.0000"),
                                      Roll.Item[cnt].ChkB.ToString("0.0000"),
                                      Roll.Item[cnt].Loss.ToString("0.0000")};

                    dgv.Rows.Add(rows);

                    spdS += Roll.Item[cnt].SpdS;
                    spdE += Roll.Item[cnt].SpdE;
                    rpmS += Roll.Item[cnt].RpmS;
                    rpmE += Roll.Item[cnt].RpmE;
                    time += Roll.Item[cnt].Time;
                    chkM += Roll.Item[cnt].ChkM;
                    chkB += Roll.Item[cnt].ChkB;
                    loss += Roll.Item[cnt].Loss;

                    count++;
                    CalIndex++;
                }
                else
                { 
                    break; 
                }
            }

            if (count > 0)
            {
                Roll.Aver.SpdS = spdS / count;
                Roll.Aver.SpdE = spdE / count;
                Roll.Aver.RpmS = rpmS / count;
                Roll.Aver.RpmE = rpmE / count;
                Roll.Aver.Time = time / count;
                Roll.Aver.ChkM = chkM / count;
                Roll.Aver.ChkB = chkB / count;
                Roll.Aver.Loss = loss / count;
            }
            else
            {
                Roll.Aver.SpdS = 0;
                Roll.Aver.SpdE = 0;
                Roll.Aver.RpmS = 0;
                Roll.Aver.RpmE = 0;
                Roll.Aver.Time = 0;
                Roll.Aver.ChkM = 0;
                Roll.Aver.ChkB = 0;
                Roll.Aver.Loss = 0;

                return;
            }

            int rowNumber = 1;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                row.HeaderCell.Value = rowNumber.ToString();
                rowNumber++;
            }
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            string[] rows_Aver = { Roll.Aver.SpdS.ToString("0.0"),
                                   Roll.Aver.SpdE.ToString("0.0"),
                                   //Roll.Aver.RpmS.ToString("0.0"),
                                   //Roll.Aver.RpmE.ToString("0.0"),
                                   Roll.Aver.Time.ToString("0.00"),
                                   Roll.Aver.ChkM.ToString("0.0000"),
                                   Roll.Aver.ChkB.ToString("0.0000"),
                                   Roll.Aver.Loss.ToString("0.0000")};

            dgv.Rows.Add(rows_Aver);
            dgv.Rows[dgv.Rows.Count - 1].HeaderCell.Value = "Average";
        }
        
        public void Refresh_Data()
        {
            pic_FL.Image = Show1_Values(pic_FL, Convert.ToSingle(NI.Loss.FL.Speed));
            pic_FR.Image = Show1_Values(pic_FR, Convert.ToSingle(NI.Loss.FR.Speed));
            pic_RL.Image = Show1_Values(pic_FL, Convert.ToSingle(NI.Loss.RL.Speed));
            pic_RR.Image = Show1_Values(pic_FL, Convert.ToSingle(NI.Loss.RR.Speed));
        }

        private Bitmap Show1_Values(PictureBox pBox, Single pSpeed)
        {
            bmp = new Bitmap(pBox.Width, pBox.Height);

            Single brkSpeed = pSpeed;

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float bmp_w = bmp.Width;
                float bmp_h = bmp.Height;
                float value = brkSpeed / SpeedMax * bmp_w;

                Font drawFont = new Font("Arial", 20, FontStyle.Bold);
                Pen RedPen = new Pen(Color.Red, 2);
                Pen GreenPen = new Pen(Color.Green, 5);
                Pen BluePen = new Pen(Color.Blue, 5);
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                SolidBrush RedBrush = new SolidBrush(Color.Red);
                
                value = brkSpeed / SpeedMax * bmp_w;
                g.FillRectangle(RedBrush, 0f, 5f, value, bmp_h - 15);

                //Set format of string.
                float x = 0;
                float y = 7.0F;
                float width = 120;
                float height = bmp_h - y;

                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;
                RectangleF drawRect = new RectangleF(x, y, width, height);
                //g.DrawString("SPEED", drawFont, drawBrush, drawRect, drawFormat);

                x = bmp.Width - 120;
                y = 7.0F;
                width = 120;
                height = bmp.Height - y;
                drawRect = new RectangleF(x, y, width, height);
                g.DrawString(brkSpeed.ToString("0.00"), drawFont, drawBrush, drawRect, drawFormat);
            }

            return bmp;
        }

        private void chk_Line_Click(object sender, EventArgs e)
        {
            switch (((CheckBox)sender).Name)
            {
                case "chkGFL": comGraph.TargetChannel = 0; comGraph.ChannelVisible = ((CheckBox)sender).Checked; break;
                case "chkGFR": comGraph.TargetChannel = 1; comGraph.ChannelVisible = ((CheckBox)sender).Checked; break;
                case "chkGRL": comGraph.TargetChannel = 2; comGraph.ChannelVisible = ((CheckBox)sender).Checked; break;
                case "chkGRR": comGraph.TargetChannel = 3; comGraph.ChannelVisible = ((CheckBox)sender).Checked; break;
            }
        }

        private void comGraph_DblClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                tab_Loss.Dock = DockStyle.None;
                btn1Free.Visible = false;
                btn1Stop.Visible = false;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                tab_Loss.Dock = DockStyle.Fill;
                btn1Free.Visible = true;
                btn1Stop.Visible = true;
            }
        }

        private void dgv_Paint(object sender, PaintEventArgs e)
        {
            //string title = "";

            //switch (((DataGridView)sender).Name)
            //{
            //    case "dgv_FL": title = "FL"; break;
            //    case "dgv_FR": title = "FR"; break;
            //    case "dgv_RL": title = "RL"; break;
            //    case "dgv_RR": title = "RR"; break;
            //}

            //Rectangle rect = new Rectangle(0, 2, ((DataGridView)sender).RowHeadersWidth, ((DataGridView)sender).ColumnHeadersHeight);

            //Font font = new Font(((DataGridView)sender).RowHeadersDefaultCellStyle.Font, FontStyle.Bold);

            //TextRenderer.DrawText(e.Graphics, title, font, rect, Color.DarkBlue, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }

        #region Struct Save / Read
        private void Save_LogLoss(string pFile)
        {
            StringBuilder builder = new StringBuilder();

            for (int cnt = 0; cnt < FL_Roll.Item.Length; cnt++)
            {
                if (FL_Roll.Item[cnt].SpdS > 0)
                {
                    builder.AppendLine(FL_Roll.Item[cnt].SpdS.ToString() + "$" +
                                       FL_Roll.Item[cnt].SpdE.ToString() + "$" +
                                       FL_Roll.Item[cnt].RpmS.ToString() + "$" +
                                       FL_Roll.Item[cnt].RpmE.ToString() + "$" +
                                       FL_Roll.Item[cnt].Time.ToString() + "$" +
                                       FL_Roll.Item[cnt].ChkM.ToString() + "$" +
                                       FL_Roll.Item[cnt].ChkB.ToString() + "$" +
                                       FL_Roll.Item[cnt].Loss.ToString() + "*" +

                                       FR_Roll.Item[cnt].SpdS.ToString() + "$" +
                                       FR_Roll.Item[cnt].SpdE.ToString() + "$" +
                                       FR_Roll.Item[cnt].RpmS.ToString() + "$" +
                                       FR_Roll.Item[cnt].RpmE.ToString() + "$" +
                                       FR_Roll.Item[cnt].Time.ToString() + "$" +
                                       FR_Roll.Item[cnt].ChkM.ToString() + "$" +
                                       FR_Roll.Item[cnt].ChkB.ToString() + "$" +
                                       FR_Roll.Item[cnt].Loss.ToString() + "*" +

                                       RL_Roll.Item[cnt].SpdS.ToString() + "$" +
                                       RL_Roll.Item[cnt].SpdE.ToString() + "$" +
                                       RL_Roll.Item[cnt].RpmS.ToString() + "$" +
                                       RL_Roll.Item[cnt].RpmE.ToString() + "$" +
                                       RL_Roll.Item[cnt].Time.ToString() + "$" +
                                       RL_Roll.Item[cnt].ChkM.ToString() + "$" +
                                       RL_Roll.Item[cnt].ChkB.ToString() + "$" +
                                       RL_Roll.Item[cnt].Loss.ToString() + "*" +

                                       RR_Roll.Item[cnt].SpdS.ToString() + "$" +
                                       RR_Roll.Item[cnt].SpdE.ToString() + "$" +
                                       RR_Roll.Item[cnt].RpmS.ToString() + "$" +
                                       RR_Roll.Item[cnt].RpmE.ToString() + "$" +
                                       RR_Roll.Item[cnt].Time.ToString() + "$" +
                                       RR_Roll.Item[cnt].ChkM.ToString() + "$" +
                                       RR_Roll.Item[cnt].ChkB.ToString() + "$" +
                                       RR_Roll.Item[cnt].Loss.ToString() 
                                       );
                }
                else
                {
                    break;
                }
            }

            System.IO.File.WriteAllText(pFile, builder.ToString());
        }
        private void Read_LogLoss(string pFile)
        {
            if (File.Exists(pFile))
            {
                string[] arrStudents = System.IO.File.ReadAllLines(pFile);
                int index = 0;

                foreach (string wheel in arrStudents)
                {
                    string[] lines = wheel.Split('*');

                    FL_Roll.Item[index] = Kind_LogLoss(lines[0]);
                    FR_Roll.Item[index] = Kind_LogLoss(lines[1]);
                    RL_Roll.Item[index] = Kind_LogLoss(lines[2]);
                    RR_Roll.Item[index] = Kind_LogLoss(lines[3]);

                    index++;
                }

                FL_Roll = Aver_LogLoss(FL_Roll);
                FR_Roll = Aver_LogLoss(FR_Roll);
                RL_Roll = Aver_LogLoss(RL_Roll);
                RR_Roll = Aver_LogLoss(RR_Roll);

                LossdgvDatas(dgv_FL, FL_Roll);
                LossdgvDatas(dgv_FR, FR_Roll);
                LossdgvDatas(dgv_RL, RL_Roll);
                LossdgvDatas(dgv_RR, RR_Roll);
            }
        }
        private Loss_Items Kind_LogLoss(string pLoss)
        {
            string[] columns = pLoss.Split('$');
            if (columns.Length < 8) return new Loss_Items();
            double dv;

            Loss_Items member = new Loss_Items()
            {
                SpdS = double.TryParse(columns[0], out dv) ? dv : 0,
                SpdE = double.TryParse(columns[1], out dv) ? dv : 0,
                RpmS = double.TryParse(columns[2], out dv) ? dv : 0,
                RpmE = double.TryParse(columns[3], out dv) ? dv : 0,
                Time = double.TryParse(columns[4], out dv) ? dv : 0,
                ChkM = double.TryParse(columns[5], out dv) ? dv : 0,
                ChkB = double.TryParse(columns[6], out dv) ? dv : 0,
                Loss = double.TryParse(columns[7], out dv) ? dv : 0
            };

            return member;
        }
        private Loss_Cal Aver_LogLoss(Loss_Cal Roll)
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

            for (int cnt = 0; cnt < Roll.Item.Length; cnt++)
            {
                spdS += Roll.Item[cnt].SpdS;
                spdE += Roll.Item[cnt].SpdE;
                rpmS += Roll.Item[cnt].RpmS;
                rpmE += Roll.Item[cnt].RpmE;
                time += Roll.Item[cnt].Time;
                chkM += Roll.Item[cnt].ChkM;
                chkB += Roll.Item[cnt].ChkB;
                loss += Roll.Item[cnt].Loss;
            }

            if (count > 0)
            {
                Roll.Aver.SpdS = spdS / count;
                Roll.Aver.SpdE = spdE / count;
                Roll.Aver.RpmS = rpmS / count;
                Roll.Aver.RpmE = rpmE / count;
                Roll.Aver.Time = time / count;
                Roll.Aver.ChkM = chkM / count;
                Roll.Aver.ChkB = chkB / count;
                Roll.Aver.Loss = loss / count;
            }
            else
            {
                Roll.Aver.SpdS = 0;
                Roll.Aver.SpdE = 0;
                Roll.Aver.RpmS = 0;
                Roll.Aver.RpmE = 0;
                Roll.Aver.Time = 0;
                Roll.Aver.ChkM = 0;
                Roll.Aver.ChkB = 0;
                Roll.Aver.Loss = 0;
            }

            return Roll;
        }
        #endregion
    }
}