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
    public partial class fom_Load : Form
    {
        public bool IsOpen;

        fom_Main Fom_Main = null;
        fom1Gage fom_Gage = null;

        Bitmap bmp;
        long ForceMax = 50;
        bool StartOnf = false;
        bool Free_Onf = false;

        int selWheel = 0;
        int oldWheel = 0;

        private Load_Cal FL_Roll = new Load_Cal();
        private Load_Cal FR_Roll = new Load_Cal();
        private Load_Cal RL_Roll = new Load_Cal();
        private Load_Cal RR_Roll = new Load_Cal();

        private clsBS205 BS205;
        private cls_Gage GageSped;

        double[] T_Data;
        double[] T_Sped; double[] T__RPM; double[] T__Kgf; double[] T_Indi; double[] T_Load;
        bool Ask_Save = false;

        public fom_Load()
        {
            InitializeComponent();

            BS205 = new clsBS205(this.Owner);

            #region Ranguage
            if (PSet.OwnerS00 > PSet.Def_Lang)
            {
                this.Text = PSet.LangLoad[0];       //부하 손실 측정
                gbx_Indi.Text = "";                 //인디게이터
                lbl1Indi.Text = PSet.LangLoad[1];   //인디게이터
                gbxWheel.Text = PSet.LangLoad[2];   //휠 컨트롤
                rdo_FL.Text = PSet.LangLoad[3];     //전축 좌
                rdo_FR.Text = PSet.LangLoad[4];     //전축 우
                rdo_RL.Text = PSet.LangLoad[5];     //후축 좌
                rdo_RR.Text = PSet.LangLoad[6];     //후축 우
                btnStart.Text = PSet.LangLoad[7];   //시작
                btn_Free.Text = PSet.LangLoad[8];   //롤러 프리
                btn_Stop.Text = PSet.LangLoad[9];   //정지
                btn_Zero.Text = PSet.LangLoad[10];  //인디게이터 영점
                btnSolOn.Text = PSet.LangLoad[11];  //솔 ON
                btnSolOf.Text = PSet.LangLoad[12];  //솔 OFF
                lbl___00.Text = PSet.LangLoad[13];  //회차
                lbl___01.Text = PSet.LangLoad[14];  //최고 속도
                lbl___02.Text = PSet.LangLoad[15];  //시작 속도
                lbl___03.Text = PSet.LangLoad[16];  //종료 속도
                lbl___04.Text = PSet.LangLoad[17];  //측정 시간
                lbl___05.Text = PSet.LangLoad[18];  //전체 시간
                tabPage1.Text = PSet.LangLoad[19];  //교정 데이터
                tabPage2.Text = PSet.LangLoad[20];  //그래프 데이터
                tabPage3.Text = PSet.LangLoad[21];  //기초 데이터
                btn_Save.Text = PSet.LangLoad[22];  //저장
                btnClear.Text = PSet.LangLoad[23];  //클리어
                btnExcel.Text = PSet.LangLoad[24];  //엑셀
                btnClose.Text = PSet.LangLoad[25];  //닫기
            }
            #endregion
        }

        private void fom_Load_Load(object sender, EventArgs e)
        {
            IsOpen = true;

            Fom_Main = (fom_Main)this.Owner;

            IndicatorOn();
            BS205.Setting(PSet.Indi_S, PSet.Indi_P.ToString());

            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();
            CellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            dgv_FL.TopLeftHeaderCell.Value = rdo_FL.Text;
            dgv_FR.TopLeftHeaderCell.Value = rdo_FR.Text;
            dgv_RL.TopLeftHeaderCell.Value = rdo_RL.Text;
            dgv_RR.TopLeftHeaderCell.Value = rdo_RR.Text;

            dgv_FL.TopLeftHeaderCell.Style = CellStyle;
            dgv_FR.TopLeftHeaderCell.Style = CellStyle;
            dgv_RL.TopLeftHeaderCell.Style = CellStyle;
            dgv_RR.TopLeftHeaderCell.Style = CellStyle;

            dgv_FL.RowHeadersDefaultCellStyle = CellStyle; dgv_FL.ColumnHeadersDefaultCellStyle = CellStyle;
            dgv_FR.RowHeadersDefaultCellStyle = CellStyle; dgv_FR.ColumnHeadersDefaultCellStyle = CellStyle;
            dgv_RL.RowHeadersDefaultCellStyle = CellStyle; dgv_RL.ColumnHeadersDefaultCellStyle = CellStyle;
            dgv_RR.RowHeadersDefaultCellStyle = CellStyle; dgv_RR.ColumnHeadersDefaultCellStyle = CellStyle;

            if (!PSet.OnfOwner)
            {
                tab_Load.Controls.RemoveByKey("tabPage2");
                tab_Load.Controls.RemoveByKey("tabPage3");
                gbx_Hide.Visible = false;
            }

            cboIndex.Items.Clear();
            for (int cnt = 1; cnt <= 3; cnt++)
            {
                cboIndex.Items.Add(cnt.ToString());
            }
            cboIndex.SelectedIndex = 0;

            cboF_RPM.Items.Clear();
            cboF_RPM.Items.Add("None");
            cboF_RPM.Items.Add("Average");
            cboF_RPM.Items.Add("Sort");
            cboF_RPM.Items.Add("Sort+3Average");
            cboF_RPM.Items.Add("Sort+5Average");
            cboF_RPM.SelectedIndex = (NI.Loss.RPM_Filt >= 0 && NI.Loss.RPM_Filt < cboF_RPM.Items.Count) ? NI.Loss.RPM_Filt : 0;

            cboC_RPM.Items.Clear();
            cboC_RPM.Items.Add("5"); cboC_RPM.Items.Add("7"); cboC_RPM.Items.Add("9"); cboC_RPM.Items.Add("11"); cboC_RPM.Items.Add("13"); cboC_RPM.Items.Add("15");
            cboC_RPM.Text = NI.Loss.RPM_Cunt.ToString();

            cboF_Acc.Items.Clear();
            cboF_Acc.Items.Add("None");
            cboF_Acc.Items.Add("Average");
            cboF_Acc.Items.Add("Sort");
            cboF_Acc.Items.Add("Sort+3Average");
            cboF_Acc.Items.Add("Sort+5Average");
            cboF_Acc.SelectedIndex = (NI.Loss.Acc_Filt >= 0 && NI.Loss.Acc_Filt < cboF_Acc.Items.Count) ? NI.Loss.Acc_Filt : 0;

            cboC_Acc.Items.Clear();
            cboC_Acc.Items.Add("5"); cboC_Acc.Items.Add("7"); cboC_Acc.Items.Add("9"); cboC_Acc.Items.Add("11"); cboC_Acc.Items.Add("13"); cboC_Acc.Items.Add("15");
            cboC_Acc.Text = NI.Loss.Acc_Cunt.ToString();

            cbo_Befo.Items.Clear();
            cbo_Befo.Items.Add("0");
            cbo_Befo.Items.Add("1");
            cbo_Befo.Items.Add("2");
            cbo_Befo.Items.Add("3");
            cbo_Befo.Items.Add("4");
            cbo_Befo.Items.Add("5");
            cbo_Befo.Items.Add("6");
            cbo_Befo.Items.Add("7");
            cbo_Befo.Items.Add("8");
            cbo_Befo.Items.Add("9");
            cbo_Befo.SelectedIndex = (NI.Loss.Before_C >= 0 && NI.Loss.Before_C < cbo_Befo.Items.Count) ? NI.Loss.Before_C : 0;

            cbo_Aver.Items.Clear();
            cbo_Aver.Items.Add("1");
            cbo_Aver.Items.Add("2");
            cbo_Aver.Items.Add("3");
            cbo_Aver.Items.Add("4");
            cbo_Aver.Items.Add("5");
            cbo_Aver.Items.Add("6");
            cbo_Aver.Items.Add("7");
            cbo_Aver.Items.Add("8");
            cbo_Aver.Items.Add("9");
            cbo_Aver.SelectedIndex = (NI.Loss.kgf_Aver >= 0 && NI.Loss.kgf_Aver < cbo_Aver.Items.Count) ? NI.Loss.kgf_Aver : 0;

            PSet.Cal_LoadRead();
            FL_Roll = PSet.Load_FL;
            FR_Roll = PSet.Load_FR;
            RL_Roll = PSet.Load_RL;
            RR_Roll = PSet.Load_RR;

            Load___Datas(dgv_FL, FL_Roll);
            Load___Datas(dgv_FR, FR_Roll);
            Load___Datas(dgv_RL, RL_Roll);
            Load___Datas(dgv_RR, RR_Roll);

            GageSped = new cls_Gage(picSpeed, picSpeed.Width, 200, 1.6, "Speed", "km/h");
            GageSped.Yellow_Zone(30, 100);

            tmr_Loop.Enabled = true;
        }
        private void fom_Load_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((NI.Loss.FL.Speed <= PSet.Stop_Spd) &&
                (NI.Loss.FR.Speed <= PSet.Stop_Spd) &&
                (NI.Loss.RL.Speed <= PSet.Stop_Spd) &&
                (NI.Loss.RR.Speed <= PSet.Stop_Spd))
            {
                tmr_Loop.Enabled = false;
                tmr_Cals.Enabled = false;

                if (Ask_Save)
                {   //교정 데이터를 저장하시겠습니까? , 저장
                    if (H2Y.Question(PSet.LangLoad[26], PSet.LangLoad[27])) { Order__Save(); }
                }

                IndicatorOf();
                IsOpen = false;
                BS205.Close();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnStart": Order_Start(); break;
                case "btn_Free": Order__Free(); break;
                case "btn_Stop": Order__Stop(); break;
                case "btn_Zero": Order__Zero(); break;
                case "btn_Read": Order__Read(); break;
                case "btn_Save": Order__Save(); break;
                case "btnClear": Order1Clear(); break;
                case "btnA_Cls": Order_Clear(); break;
                case "btnSolOn": Order_SolOn(); break;
                case "btnSolOf": Order_SolOf(); break;
                case "btn_Filt": OrderFilter(); break;
                case "btnExcel": Order_Excel(); break;
                case "btnClose": Order_Close(); break;
            }
        }

        private void Order_Start()  //Calibration Start
        {
            if ((NI.Loss.FL.Speed >= 0.1d) || (NI.Loss.FR.Speed >= 0.1d) ||
                (NI.Loss.RL.Speed >= 0.1d) || (NI.Loss.RR.Speed >= 0.1d))
            {
                MessageBox.Show(PSet.LangLoad[28]); //롤러 움직임이 감지되었습니다.
                return;
            }

            int MaxSpeed = (int)(Ret_Values(txtSpeed.Text) * 100 / 1.5);

            if (!PLC.DO.MD__Ready) { MessageBox.Show(PSet.LangLoad[29]); return; } //"준비 모드로 전환"
            if (!PLC.DI.PBCalMode) { MessageBox.Show(PSet.LangLoad[30]); return; } //"교정 모드로 전환"

            switch (selWheel)
            {
                case 0: if (!rdo_FL.Enabled) return; break;
                case 1: if (!rdo_FR.Enabled) return; break;
                case 2: if (!rdo_RL.Enabled) return; break;
                case 3: if (!rdo_RR.Enabled) return; break;
            }

            if (rdo_FL.Checked) { btnStart.Enabled = rdo_FL.Enabled; }
            if (rdo_FR.Checked) { btnStart.Enabled = rdo_FR.Enabled; }
            if (rdo_RL.Checked) { btnStart.Enabled = rdo_RL.Enabled; }
            if (rdo_RR.Checked) { btnStart.Enabled = rdo_RR.Enabled; }

            Order__Zero();

            btnStart.BackColor = Color.DarkGray;
            btnStart.Enabled = false;
            Ask_Save = true;

            switch (selWheel)
            {
                case 0: PLC.DO.FLMot_Stt = true; break;
                case 1: PLC.DO.FRMot_Stt = true; break;
                case 2: PLC.DO.RLMot_Stt = true; break;
                case 3: PLC.DO.RRMot_Stt = true; break;
            }

            PLC.PLC_Put_D500();

            tmr_Cals.Enabled = true;
        }
        private void Order__Free()  //Roll Free
        {
            if (!StartOnf) { return; }

            Free_Onf = true;
            btn_Free.BackColor = Color.DarkGray;
            btn_Free.Enabled = false;

            switch (selWheel)
            {
                case 0: PLC.DO.FLMot_Stt = false; break;
                case 1: PLC.DO.FRMot_Stt = false; break;
                case 2: PLC.DO.RLMot_Stt = false; break;
                case 3: PLC.DO.RRMot_Stt = false; break;
            }
            PLC.PLC_Put_D500();
        }
        private void Order__Stop()  //Roll Brake
        {
            if (!StartOnf) { return; }

            btn_Stop.BackColor = Color.DarkGray;
            btn_Stop.Enabled = false;

            switch (selWheel)
            {
                case 0: PLC.DO.FLMotStop = true; break;    //강제 정지(15Hz이하 작동)
                case 1: PLC.DO.FRMotStop = true; break;    //강제 정지(15Hz이하 작동)
                case 2: PLC.DO.RLMotStop = true; break;    //강제 정지(15Hz이하 작동)
                case 3: PLC.DO.RRMotStop = true; break;    //강제 정지(15Hz이하 작동)
            }

            PLC.PLC_Put_D500();
        }
        private void Order__Zero()  //Roll Brake
        {
            btn_Zero.BackColor = Color.Lime;
            BS205.Send_Zero();
            //PLC.DO.CalIndi_O = true; PLC.PLC_312_Puts(); //D312[15] 인디게이터 영점
            H2Y.Sleep(500);
            //PLC.DO.CalIndi_O = false; PLC.PLC_312_Puts(); //D312[15] 인디게이터 영점
            btn_Zero.BackColor = SystemColors.ControlLight;
        }

        private void Order__Read()  //Calibration Data Read
        {
            string Cal_Path = System.Windows.Forms.Application.StartupPath + @"\Cal";
            string Cal_File = "";

            switch (selWheel)
            {
                case 0: Cal_File = Cal_Path + @"\Coast-FL-" + cboIndex.SelectedItem.ToString() + ".log"; break;
                case 1: Cal_File = Cal_Path + @"\Coast-FR-" + cboIndex.SelectedItem.ToString() + ".log"; break;
                case 2: Cal_File = Cal_Path + @"\Coast-RL-" + cboIndex.SelectedItem.ToString() + ".log"; break;
                case 3: Cal_File = Cal_Path + @"\Coast-RR-" + cboIndex.SelectedItem.ToString() + ".log"; break;
            }

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
            }

            if (Cal_Data.Count > 0)
            {
                MessageBox.Show(Cal_Data.Count.ToString());

                T_Data = new double[Cal_Data.Count];
                T_Sped = new double[Cal_Data.Count];
                T__RPM = new double[Cal_Data.Count];
                T__Kgf = new double[Cal_Data.Count];
                T_Indi = new double[Cal_Data.Count];
                T_Load = new double[Cal_Data.Count];

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
                        if (ArrD.Length < 6) continue;
                        double dv;

                        T_Data[CNT] = double.TryParse(ArrD[0], out dv) ? dv : 0;
                        T_Sped[CNT] = double.TryParse(ArrD[1], out dv) ? dv : 0;
                        T__RPM[CNT] = double.TryParse(ArrD[2], out dv) ? dv : 0;
                        T__Kgf[CNT] = double.TryParse(ArrD[3], out dv) ? dv : 0;
                        T_Indi[CNT] = double.TryParse(ArrD[4], out dv) ? dv : 0;
                        T_Load[CNT] = double.TryParse(ArrD[5], out dv) ? dv : 0;

                        comGraph.PlotChart(0, ref T_Data[CNT], ref T_Sped[CNT], 1);
                        comGraph.PlotChart(1, ref T_Data[CNT], ref T_Indi[CNT], 1);
                        comGraph.PlotChart(2, ref T_Data[CNT], ref T_Load[CNT], 1);

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

            PSet.Load_FL = FL_Roll;
            PSet.Load_FR = FR_Roll;
            PSet.Load_RL = RL_Roll;
            PSet.Load_RR = RR_Roll;

            PSet.Cal_LoadMake();
            PSet.Cal_LoadRead();
            FL_Roll = PSet.Load_FL;
            FR_Roll = PSet.Load_FR;
            RL_Roll = PSet.Load_RL;
            RR_Roll = PSet.Load_RR;

            Load___Datas(dgv_FL, FL_Roll);
            Load___Datas(dgv_FR, FR_Roll);
            Load___Datas(dgv_RL, RL_Roll);
            Load___Datas(dgv_RR, RR_Roll);

            PSet.Test_CNTMake("Init");              //카운터 초기화

            MessageBoxEx.Show(PSet.LangLoad[31]);   //"저장되었습니다."
        }
        private void Order1Clear()
        {
            cboIndex.SelectedIndex = 0;

            switch (selWheel)
            {
                case 0: if (FL_Roll.Item != null) { FL_Roll.Clear(); Load___Datas(dgv_FL, FL_Roll); }; break;
                case 1: if (FR_Roll.Item != null) { FR_Roll.Clear(); Load___Datas(dgv_FR, FR_Roll); }; break;
                case 2: if (RL_Roll.Item != null) { RL_Roll.Clear(); Load___Datas(dgv_RL, RL_Roll); }; break;
                case 3: if (RR_Roll.Item != null) { RR_Roll.Clear(); Load___Datas(dgv_RR, RR_Roll); }; break;
            }
        }
        private void Order_Clear()
        {
            cboIndex.SelectedIndex = 0;

            if (FL_Roll.Item != null) { FL_Roll.Clear(); Load___Datas(dgv_FL, FL_Roll); }
            if (FR_Roll.Item != null) { FR_Roll.Clear(); Load___Datas(dgv_FR, FR_Roll); }
            if (RL_Roll.Item != null) { RL_Roll.Clear(); Load___Datas(dgv_RL, RL_Roll); }
            if (RR_Roll.Item != null) { RR_Roll.Clear(); Load___Datas(dgv_RR, RR_Roll); }
        }
        private void Order_SolOn()
        {
            PLC.DO.CalAirSol = true; PLC.PLC_312_Puts();   //D312
        }
        private void Order_SolOf()
        {
            PLC.DO.CalAirSol = false; PLC.PLC_312_Puts();  //D312
        }
        private void IndicatorOn()
        {
            PLC.DO.CalIndiOn = true; PLC.PLC_312_Puts();   //D312
        }
        private void IndicatorOf()
        {
            PLC.DO.CalIndiOn = false; PLC.PLC_312_Puts();  //D312
        }

        private void OrderFilter()
        {
            NI.Loss.RPM_Filt = cboF_RPM.SelectedIndex;
            if (cboC_RPM.SelectedIndex >= 0) { int tmp; if (int.TryParse(cboC_RPM.Items[cboC_RPM.SelectedIndex].ToString(), out tmp)) NI.Loss.RPM_Cunt = tmp; }
            NI.Loss.Acc_Filt = cboF_Acc.SelectedIndex;
            if (cboC_Acc.SelectedIndex >= 0) { int tmp; if (int.TryParse(cboC_Acc.Items[cboC_Acc.SelectedIndex].ToString(), out tmp)) NI.Loss.Acc_Cunt = tmp; }

            NI.Loss.Before_C = cbo_Befo.SelectedIndex;
            NI.Loss.kgf_Aver = cboC_Acc.SelectedIndex + 1;
        }
        private void Order_Excel()
        {
            StreamWriter sw = null;

            try
            {
                string Cal_Path = System.Windows.Forms.Application.StartupPath + @"\Cal";
                string Cal_File = Cal_Path + @"\Load-Cycle.csv";
                Load_Cal RLoad = FL_Roll;

                switch (selWheel)
                {
                    case 0: Cal_File = Cal_Path + @"\Load-Cycle FL.csv"; RLoad = FL_Roll; break;
                    case 1: Cal_File = Cal_Path + @"\Load-Cycle FR.csv"; RLoad = FR_Roll; break;
                    case 2: Cal_File = Cal_Path + @"\Load-Cycle RL.csv"; RLoad = RL_Roll; break;
                    case 3: Cal_File = Cal_Path + @"\Load-Cycle RR.csv"; RLoad = RR_Roll; break;
                }

                H2Y.Make_Dir(Cal_Path);

                FileStream fs = new FileStream(Cal_File, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);

                string str_Data = "Start RPM, End EPM, Time, Deviation %, Calibration, Indicator, Calculation";
                sw.WriteLine(str_Data);

                for (int cnt = 0; cnt < RLoad.Item.Length; cnt++)
                {
                    str_Data = "";

                    str_Data += RLoad.Item[cnt].SpdS + ",";
                    str_Data += RLoad.Item[cnt].SpdE + ",";
                    str_Data += RLoad.Item[cnt].RpmS + ",";
                    str_Data += RLoad.Item[cnt].RpmE + ",";
                    str_Data += RLoad.Item[cnt].Time + ",";
                    str_Data += RLoad.Item[cnt].Devi + ",";
                    str_Data += RLoad.Item[cnt].CalD + ",";
                    str_Data += RLoad.Item[cnt].Indi + ",";
                    str_Data += RLoad.Item[cnt].Calc + ",";

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
        private void Order_Close()  //교정 종료
        {
            this.Close();
        }

        private void tmr_Loop_Tick(object sender, EventArgs e)
        {
            rdo_FL.Text = PLC.DI.FL_RBFree ? PSet.LangLoad[3] : "FL Lock";     //전축 좌
            rdo_FR.Text = PLC.DI.FR_RBFree ? PSet.LangLoad[4] : "FR Lock";     //전축 우
            rdo_RL.Text = PLC.DI.RL_RBFree ? PSet.LangLoad[5] : "RL Lock";     //후축 좌
            rdo_RR.Text = PLC.DI.RR_RBFree ? PSet.LangLoad[6] : "RR Lock";     //후축 우

            rdo_FL.Enabled = PLC.DI.FL_RBFree;
            rdo_FR.Enabled = PLC.DI.FR_RBFree;
            rdo_RL.Enabled = PLC.DI.RL_RBFree;
            rdo_RR.Enabled = PLC.DI.RR_RBFree;

            lbl_Indi.BackColor = PLC.DO.Indicator ? Color.Black : Color.Red;
            lbl1Indi.BackColor = BS205.Is_Onf ? Color.Black : Color.Lime;

            Refresh_Data();
        }

        private void tmr_Cals_Tick(object sender, EventArgs e)
        {
            tmr_Cals.Enabled = false;

            cboIndex.Enabled = false;
            txtSpeed.Enabled = false;
            txtStart.Enabled = false;
            txt_Ends.Enabled = false;

            Single Max = Single.Parse(txtStart.Text);
            Single Min = Single.Parse(txt_Ends.Text);

            fom_Gage = new fom1Gage(selWheel, Min, Max);
            fom_Gage.Show();

            comGraph.XAxisRangeMin = 0;
            comGraph.XAxisRangeMax = 200;
            comGraph.DeletePlot(-1);
            comGraph.Update();
            comGraph.SetChartPause(0);

            StartOnf = true;
            Free_Onf = false;

            PLC.DO.FLMotStop = false;
            PLC.DO.FRMotStop = false;
            PLC.DO.RLMotStop = false;
            PLC.DO.RRMotStop = false; PLC.PLC_Put_D500(); //D530
            PLC.DO.CalAirSol = false;
            PLC.DO.CalIndi_O = false; PLC.PLC_312_Puts(); //D312

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
            PLC.DO.CalAirSol = false;
            PLC.DO.CalIndi_O = false; PLC.PLC_312_Puts(); //D312

            fom_Gage.Close();
            btnStart.Enabled = true; btnStart.BackColor = SystemColors.ControlLight;
            btn_Free.Enabled = true; btn_Free.BackColor = SystemColors.ControlLight;
            btn_Stop.Enabled = true; btn_Stop.BackColor = SystemColors.ControlLight;
            btn_Zero.Enabled = true; btn_Zero.BackColor = SystemColors.ControlLight;

            cboIndex.Enabled = true;
            txtSpeed.Enabled = true;
            txtStart.Enabled = true;
            txt_Ends.Enabled = true;
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
            double Old_Time = 0;
            bool Sel_Free = false;
            bool SelStart = false;
            bool S_Finish = false;
            bool Sel_Stop = false;
            double Sel_Ofst = 0;
            double Sel_Wait = 0;
            double Sel_Time = 0;
            double old__RPM = 0;
            double SelLossM = 0, SelLossB = 0;
            double oldForce = 0;

            double Sel_Jerk = 0;
            double Sel_Loss = 0;
            double Sel_CalD = 0;
            double SelDevia = 0;
            double Sel_Load = 0;
            double Sel__Sum = 0;
            long SelCount = 0;

            double Max_Sped, Stt_Sped, End_Sped;
            if (!double.TryParse(txtSpeed.Text, out Max_Sped)) return;
            if (!double.TryParse(txtStart.Text, out Stt_Sped)) return;
            if (!double.TryParse(txt_Ends.Text, out End_Sped)) return;
            double Stt_RPMs = 0;
            double End_RPMs = 0;

            NI.Wheel wheel = new NI.Wheel();

            long OfstTick = DateTime.Now.Ticks;

            string Cal_Path = System.Windows.Forms.Application.StartupPath + @"\Cal";
            string Cal_File = "";

            switch (selWheel)
            {
                case 0: Cal_File = Cal_Path + @"\Coast-FL-" + cboIndex.SelectedItem.ToString() + ".log"; break;
                case 1: Cal_File = Cal_Path + @"\Coast-FR-" + cboIndex.SelectedItem.ToString() + ".log"; break;
                case 2: Cal_File = Cal_Path + @"\Coast-RL-" + cboIndex.SelectedItem.ToString() + ".log"; break;
                case 3: Cal_File = Cal_Path + @"\Coast-RR-" + cboIndex.SelectedItem.ToString() + ".log"; break;
            }

            H2Y.Make_Dir(Cal_Path);

            FileStream fs = new FileStream(Cal_File, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            string str_Data = "[  Time  ] [ Speed  ] [   RPM  ] [ForceKgf]";
            if (PSet.OnfOwner)
            {
                sw.WriteLine(str_Data);
            }

            GageSped.Yellow_Zone(Convert.ToInt16(End_Sped), Convert.ToInt16(Stt_Sped));

            lbl_Time.BackColor = Color.White;
            lbl_Time.Text = "--";

            switch (selWheel)
            {
                case 0: SelLossM = PSet.Loss_FL.Aver.ChkM; SelLossB = PSet.Loss_FL.Aver.ChkB; break;
                case 1: SelLossM = PSet.Loss_FR.Aver.ChkM; SelLossB = PSet.Loss_FR.Aver.ChkB; break;
                case 2: SelLossM = PSet.Loss_RL.Aver.ChkM; SelLossB = PSet.Loss_RL.Aver.ChkB; break;
                case 3: SelLossM = PSet.Loss_RR.Aver.ChkM; SelLossB = PSet.Loss_RR.Aver.ChkB; break;
            }

            while (true)
            {
                ReadTick = (DateTime.Now.Ticks - OfstTick) / 100000;
                ReadTime = (double)ReadTick / 100;

                switch (selWheel)
                {
                    case 0: wheel = NI.Loss.FL; break;
                    case 1: wheel = NI.Loss.FR; break;
                    case 2: wheel = NI.Loss.RL; break;
                    case 3: wheel = NI.Loss.RR; break;
                }

                #region Load Calibration
                if ((ReadTime - Old_Time) >= 0.1)
                {
                    Refresh_Data();

                    if (!Sel_Free)
                    {
                        if (wheel.Speed >= Max_Sped)
                        {
                            Sel_Wait = ReadTime;
                            Sel_Free = true; lbl_Time.BackColor = Color.Gray;
                        }
                    }
                    else
                    {
                        if (!Free_Onf)
                        {
                            if ((ReadTime - Sel_Wait) >= 5)
                            {
                                Order__Free();
                                PLC.DO.CalAirSol = true; PLC.PLC_312_Puts(); //D312
                            }
                        }

                        if (!SelStart)
                        {
                            if (wheel.Speed <= Stt_Sped)
                            {
                                SelStart = true; Sel_Ofst = ReadTime;
                                lbl_Time.ForeColor = Color.Lime;
                                Stt_RPMs = wheel.RPM;
                            }
                        }
                        else
                        {
                            if (!S_Finish)
                            {
                                Sel_Time = ReadTime - Sel_Ofst;
                                lbl_Time.Text = Sel_Time.ToString("#0.00");
                                Sel__Sum += TSet.Bongshin;
                                SelCount++;

                                if (SelCount > 0) { Sel_Load = Sel__Sum / SelCount; }

                                if (wheel.Speed <= (End_Sped - 1))
                                {
                                    S_Finish = true;
                                    Sel_Time = ReadTime - Sel_Ofst;
                                    lbl_Time.Text = Sel_Time.ToString("#0.00");
                                    lbl_Time.ForeColor = Color.Red;
                                    End_RPMs = wheel.RPM;
                                }
                            }
                            else
                            {
                                if (!Sel_Stop)
                                {
                                    if (wheel.Speed <= 5)
                                    {
                                        PLC.DO.CalAirSol = false; PLC.PLC_312_Puts(); //D312
                                        Sel_Stop = true;
                                        Order__Stop();
                                    }
                                }
                            }

                        }
                    }

                    if (PLC.DO.CalAirSol)
                    {
                        if ((Stt_Sped - 0) > wheel.Speed && wheel.Speed > (End_Sped - 0))
                        {
                            if (wheel.Force < -48000)
                            {
                                Sel_Jerk = wheel.Force - oldForce;

                                if (-150 < Sel_Jerk && Sel_Jerk < 150)
                                {
                                    Sel_Loss = (wheel.RPM * SelLossM) + SelLossB;
                                    Sel_CalD = Math.Abs((wheel.Force - Sel_Loss) / (1000 * PSet.NewTGain));
                                    SelDevia = (TSet.Bongshin - Sel_CalD) / TSet.Bongshin * 100;
                                }
                                oldForce = wheel.Force;
                            }
                        }
                    }

                    lbl_D1.Text = wheel.Force.ToString();
                    lbl_D2.Text = Sel_Jerk.ToString();
                    lbl_D3.Text = Sel_Loss.ToString();
                    lbl_D4.Text = Sel_CalD.ToString();
                    lbl_D5.Text = SelDevia.ToString();
                    lbl_D6.Text = wheel.Kgf.ToString();
                    lbl_D7.Text = wheel.Calc.ToString();
                    lbl_D8.Text = wheel.RPM.ToString();
                    lbl_D9.Text = old__RPM.ToString();
                    lbl_DA.Text = (ReadTime - Old_Time).ToString();

                    comGraph.PlotChart(0, ref ReadTime, ref wheel.Speed, 1);
                    comGraph.PlotChart(1, ref ReadTime, ref TSet.Bongshin, 1);
                    comGraph.PlotChart(2, ref ReadTime, ref wheel.Calc, 1);
                    comGraph.PlotChart(3, ref ReadTime, ref wheel.Kgf, 1);
                    comGraph.UpdatePlot();

                    fom_Gage.TestTimeShow(ReadTime, Sel_Time);

                    #region Real Time Data
                    if (PSet.OnfOwner)
                    {
                        str_Data = H2Y.DigitToStr(ReadTime.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(wheel.Speed.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(wheel.RPM.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(wheel.Kgf.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(TSet.Bongshin.ToString("#0.0000"), 10);
                        str_Data += ",";
                        str_Data += H2Y.DigitToStr(Sel_Load.ToString("#0.0000"), 10);

                        sw.WriteLine(str_Data);
                    }
                    #endregion

                    Old_Time = ReadTime;
                    old__RPM = wheel.RPM;
                }
                #endregion

                lblFTime.Text = ReadTime.ToString("#0.00");
                Application.DoEvents();

                if (Free_Onf)
                {
                    if (wheel.Speed <= 0.2) { break; }
                }
            }
            lbl_Time.BackColor = Color.Black;

            sw.Flush();
            sw.Close();
            fs.Close();

            int Idx = cboIndex.SelectedIndex;

            switch (selWheel)
            {
                case 0: FL_Roll = LoadDataSave(selWheel, FL_Roll, cboIndex.SelectedIndex, Stt_RPMs, End_RPMs, Sel_Time, SelDevia, Sel_CalD, Sel_Load); Load___Datas(dgv_FL, FL_Roll); break;
                case 1: FR_Roll = LoadDataSave(selWheel, FR_Roll, cboIndex.SelectedIndex, Stt_RPMs, End_RPMs, Sel_Time, SelDevia, Sel_CalD, Sel_Load); Load___Datas(dgv_FR, FR_Roll); break;
                case 2: RL_Roll = LoadDataSave(selWheel, RL_Roll, cboIndex.SelectedIndex, Stt_RPMs, End_RPMs, Sel_Time, SelDevia, Sel_CalD, Sel_Load); Load___Datas(dgv_RL, RL_Roll); break;
                case 3: RR_Roll = LoadDataSave(selWheel, RR_Roll, cboIndex.SelectedIndex, Stt_RPMs, End_RPMs, Sel_Time, SelDevia, Sel_CalD, Sel_Load); Load___Datas(dgv_RR, RR_Roll); break;
            }

            if (cboIndex.SelectedIndex < cboIndex.Items.Count - 1)
            {
                cboIndex.SelectedIndex = cboIndex.SelectedIndex + 1;
            }

        }

        private Load_Cal LoadDataSave(int Wheel, Load_Cal load, int idx, double start, double end, double time, double Devia, double CalD, double Indi)
        {
            double Moment = 0;
            int DiaM = 0;
            switch (selWheel)
            {
                case 0: Moment = PSet.FL_Moment; DiaM = PSet.RFLDia; break;
                case 1: Moment = PSet.FR_Moment; DiaM = PSet.RFRDia; break;
                case 2: Moment = PSet.RL_Moment; DiaM = PSet.RRLDia; break;
                case 3: Moment = PSet.RR_Moment; DiaM = PSet.RRRDia; break;
            }

            double RPM1 = start * 2 * Math.PI / 60;
            double RPM2 = end * 2 * Math.PI / 60;
            double RPM3 = (RPM1 - RPM2) * Moment;
            double Calc = RPM3 / (time * (DiaM / 2)) / 1000 / PSet.NewTGain;

            load.Item[idx].SpdS = start;
            load.Item[idx].SpdE = end;
            load.Item[idx].Time = time;
            load.Item[idx].Devi = Devia;
            load.Item[idx].CalD = CalD;
            load.Item[idx].Indi = Indi;
            load.Item[idx].Calc = Calc;

            return load;
        }

        private double Cal_Indigator(int Wheel, double Stt, double end, double time)
        {
            double Moment = 0;
            int DiaM = 0;
            switch (selWheel)
            {
                case 0: Moment = PSet.FL_Moment; DiaM = PSet.RFLDia; break;
                case 1: Moment = PSet.FR_Moment; DiaM = PSet.RFRDia; break;
                case 2: Moment = PSet.RL_Moment; DiaM = PSet.RRLDia; break;
                case 3: Moment = PSet.RR_Moment; DiaM = PSet.RRRDia; break;
            }

            double RPM1 = Stt * 2 * Math.PI / 60;
            double RPM2 = end * 2 * Math.PI / 60;
            double RPM3 = (RPM1 - RPM2) * Moment;
            double load = RPM3 / (time * (DiaM / 2)) / 1000 / PSet.NewTGain;

            return load;
        }

        private void Load___Datas(DataGridView dgv, Load_Cal Roll)
        {
            int count = 0;
            double Stt_ = 0;
            double End_ = 0;
            double time = 0;
            double Devi = 0;
            double CalD = 0;
            double Indi = 0;
            double Calc = 0;

            if (Roll.Item == null) return;

            dgv.Rows.Clear();
            for (int cnt = 0; cnt < Roll.Item.Length; cnt++)
            {
                if (Roll.Item[cnt].SpdS > 0)
                {
                    string[] rows = { Roll.Item[cnt].SpdS.ToString("#0.0"),
                                      Roll.Item[cnt].SpdE.ToString("#0.0"),
                                      Roll.Item[cnt].Time.ToString("#0.00"),
                                      Roll.Item[cnt].Devi.ToString("#0.00"),
                                      Roll.Item[cnt].CalD.ToString("#0.00"),
                                      Roll.Item[cnt].Indi.ToString("#0.00"),
                                      Roll.Item[cnt].Calc.ToString("#0.00")};

                    dgv.Rows.Add(rows);

                    Stt_ += Roll.Item[cnt].SpdS;
                    End_ += Roll.Item[cnt].SpdE;
                    time += Roll.Item[cnt].Time;
                    Devi += Roll.Item[cnt].Devi;
                    CalD += Roll.Item[cnt].CalD;
                    Indi += Roll.Item[cnt].Indi;
                    Calc += Roll.Item[cnt].Calc;

                    count++;
                }
                else
                {
                    break;
                }
            }

            if (count > 0)
            {
                Roll.Aver.SpdS = Stt_ / count;
                Roll.Aver.SpdE = End_ / count;
                Roll.Aver.Time = time / count;
                Roll.Aver.Devi = Devi / count;
                Roll.Aver.CalD = CalD / count;
                Roll.Aver.Indi = Indi / count;
                Roll.Aver.Calc = Calc / count;
            }
            else
            {
                Roll.Aver.SpdS = 0;
                Roll.Aver.SpdE = 0;
                Roll.Aver.Time = 0;
                Roll.Aver.Devi = 0;
                Roll.Aver.CalD = 0;
                Roll.Aver.Indi = 0;
                Roll.Aver.Calc = 0;

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

            string[] rows_Aver = { Roll.Aver.SpdS.ToString("#0.0"),
                                   Roll.Aver.SpdE.ToString("#0.0"),
                                   Roll.Aver.Time.ToString("#0.00"),
                                   Roll.Aver.Devi.ToString("#0.00"),
                                   Roll.Aver.CalD.ToString("#0.00"),
                                   Roll.Aver.Indi.ToString("#0.00"),
                                   Roll.Aver.Calc.ToString("#0.00")};

            dgv.Rows.Add(rows_Aver);
            dgv.Rows[dgv.Rows.Count - 1].HeaderCell.Value = "Average";
        }

        public void Refresh_Data()
        {
            lblReady.BackColor = PLC.DO.MD__Ready ? Color.Lime : Color.Red;
            lbl_Mode.BackColor = PLC.DI.PBCalMode ? Color.Lime : Color.Red;

            lbl_Indi.Text = TSet.Bongshin.ToString("#0.0");
            pic_Indi.Image = Show_Indicator(pic_Indi, Convert.ToSingle(TSet.Bongshin));

            switch (selWheel)
            {
                case 0: GageSped.Value = NI.Loss.FL.Speed; break;
                case 1: GageSped.Value = NI.Loss.FR.Speed; break;
                case 2: GageSped.Value = NI.Loss.RL.Speed; break;
                case 3: GageSped.Value = NI.Loss.RR.Speed; break;
            }

            if (PLC.DO.CalAirSol)
            {
                btnSolOn.Enabled = false; btnSolOn.BackColor = Color.DarkGray;
                btnSolOf.Enabled = true; btnSolOf.BackColor = Color.Chartreuse;
            }
            else
            {
                btnSolOn.Enabled = true; btnSolOn.BackColor = Color.Chartreuse;
                btnSolOf.Enabled = false; btnSolOf.BackColor = Color.DarkGray;
            }
        }
        private Bitmap Show_Indicator(PictureBox pBox, Single Force)
        {
            bmp = new Bitmap(pBox.Width, pBox.Height);

            Single brkForce = Force;

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float bmp_w = bmp.Width;
                float bmp_h = bmp.Height;
                float value = bmp_h - (brkForce / ForceMax * bmp_h);

                SolidBrush RedBrush = new SolidBrush(Color.Red);
                SolidBrush BuleBrush = new SolidBrush(Color.Blue);
                SolidBrush GreenBrush = new SolidBrush(Color.Lime);
                SolidBrush BlackBrush = new SolidBrush(Color.Black);

                g.FillRectangle(GreenBrush, 0f, value, bmp_w, bmp.Height);
            }

            return bmp;
        }

        private void rdoWheel_Click(object sender, EventArgs e)
        {
            switch (((RadioButton)sender).Name)
            {
                case "rdo_FL": selWheel = 0; break;
                case "rdo_FR": selWheel = 1; break;
                case "rdo_RL": selWheel = 2; break;
                case "rdo_RR": selWheel = 3; break;
            }

            Select_Wheel(selWheel);

            if (oldWheel != selWheel)
            {
                oldWheel = selWheel;
                cboIndex.SelectedIndex = 0;
            }
        }

        private void Select_Wheel(int wheel)
        {
            rdo_FL.BackColor = Color.White;
            rdo_FR.BackColor = Color.White;
            rdo_RL.BackColor = Color.White;
            rdo_RR.BackColor = Color.White;

            switch (wheel)
            {
                case 0: rdo_FL.BackColor = Color.Lime; break;
                case 1: rdo_FR.BackColor = Color.Lime; break;
                case 2: rdo_RL.BackColor = Color.Lime; break;
                case 3: rdo_RR.BackColor = Color.Lime; break;
            }
        }

        private void picSpeed_Paint(object sender, PaintEventArgs e)
        {
            GageSped.Gage_Show(e.Graphics);
        }

        private void chk_Line_Click(object sender, EventArgs e)
        {
            switch (((CheckBox)sender).Name)
            {
                case "chkCH1": comGraph.TargetChannel = 0; comGraph.ChannelVisible = ((CheckBox)sender).Checked; break;
                case "chkCH2": comGraph.TargetChannel = 1; comGraph.ChannelVisible = ((CheckBox)sender).Checked; break;
                case "chkCH3": comGraph.TargetChannel = 2; comGraph.ChannelVisible = ((CheckBox)sender).Checked; break;
                case "chkCH4": comGraph.TargetChannel = 3; comGraph.ChannelVisible = ((CheckBox)sender).Checked; break;
            }
        }

        private void comGraph_DblClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                tab_Load.Dock = DockStyle.None;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                tab_Load.Dock = DockStyle.Fill;
            }
        }
    }
}
