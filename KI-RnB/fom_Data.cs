using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace KI_RnB
{
    public partial class fom_Data : Form
    {
        private fom_Main main;

        string strModel = "";
        int idx_Find = 0;
        
        public fom_Data()
        {
            InitializeComponent();
        }
        public fom_Data(fom_Main main)
            : this()
        {
            this.main = main;

            DataTable dt = main.DB_All.DBModel.Search();
            cboModel.Items.Clear();
            cboModel.Items.Add("All Model");
            foreach (DataRow row in dt.Rows)
            {
                cboModel.Items.Add(row[0].ToString());
            }
            cboModel.SelectedIndex = 0;
            strModel = cboModel.SelectedItem.ToString();

            cbo_Days.Items.Clear();
            cbo_Days.Items.Add("Today");
            cbo_Days.Items.Add("This week");
            cbo_Days.Items.Add("This month");
            cbo_Days.Items.Add("Search");
            cbo_Days.SelectedIndex = 0;
        }
        private void fom_Data_Load(object sender, EventArgs e)
        {
            Data__Header();
        }

        private void cboModel_Click(object sender, EventArgs e)
        {
            if (cboModel.SelectedItem == null) return;
            strModel = cboModel.SelectedItem.ToString();
        }

        private void cbo_Days_Click(object sender, EventArgs e)
        {
            DateTime dt_Date = DateTime.Today;

            int add_Date = 0;
            idx_Find = cbo_Days.SelectedIndex;
            switch (idx_Find)
            {
                case 0: 
                    dtpStart.Value = dt_Date;
                    dtp__End.Value = dt_Date;
                    break;

                case 1: add_Date = (int)dt_Date.DayOfWeek;
                    dtpStart.Value = dt_Date.Date.AddDays(-add_Date);
                    dtp__End.Value = dt_Date.Date.AddDays(6 - add_Date);
                    break;

                case 2: add_Date = DateTime.DaysInMonth(dt_Date.Year, dt_Date.Month);
                    dtpStart.Value = new DateTime(dt_Date.Year, dt_Date.Month, 1);
                    dtp__End.Value = new DateTime(dt_Date.Year, dt_Date.Month, add_Date);
                    break;

                case 3: 
                    dtpStart.Value = dt_Date;
                    dtp__End.Value = dt_Date;
                    break;
            }
        }

        private void rdo_Search(object sender, EventArgs e)
        {
            DateTime dt_Date = DateTime.Today;

            int add_Date = 0;

            switch (((RadioButton)sender).Name)
            {
                case "rdoFind0": idx_Find = 0; 
                    dtpStart.Value = dt_Date; 
                    dtp__End.Value = dt_Date; 
                    break;

                case "rdoFind1": idx_Find = 1; add_Date = (int)dt_Date.DayOfWeek;
                    dtpStart.Value = dt_Date.Date.AddDays(-add_Date); 
                    dtp__End.Value = dt_Date.Date.AddDays(6 - add_Date); 
                    break;

                case "rdoFind2": idx_Find = 2; add_Date = DateTime.DaysInMonth(dt_Date.Year, dt_Date.Month);
                    dtpStart.Value = new DateTime(dt_Date.Year, dt_Date.Month, 1);
                    dtp__End.Value = new DateTime(dt_Date.Year, dt_Date.Month, add_Date); 
                    break;

                case "rdoFind3": idx_Find = 3; 
                    dtpStart.Value = dt_Date; 
                    dtp__End.Value = dt_Date; 
                    break;
            }
        }

        private void btn_Search(object sender, EventArgs e)
        {
            gbx_Data.Enabled = false;
            switch (((Button)sender).Name)
            {
                case "btn_Find": Data__Search(); break;
                case "btnExcel": Data__Export(); break;
            }
            gbx_Data.Enabled = true;
        }

        private void Data__Header()
        {
            string[] str_Head = {"Work No", "Vin", "Model", "ECU", "ID", "W/B", "Engine", "T/M", "ABS", "Curve", "Drive", "Date", 
                                 "Speed (km/h)", "Speed Judge", "Max speed (km/h)", 
                                 "Drag F-L (kg)", "Drag F-R (kg)", "Drag R-L (kg)", "Drag R-R (kg)", 
                                 "Brake F-L (kg)", "Brake F-R (kg)", "Brake R-L (kg)", "Brake R-R (kg)", 
                                 "Parking R-L (cm)", "Parking R-R (cm)", 
                                 "Balance F-L (kg)", "Balance F-R (kg)", "Front Balance (%)", "Front Balance Judge", 
                                 "Balance R-L (kg)", "Balance R-R (kg)", "Rear Balance (%)", "Rear Balance Judge", 
                                 "Balance Front-Rear (%)", "Balance Judge", 
                                 "Reverse (km/h)", 
                                 "WSS F-L (km/h)", "WSS F-R (km/h)", "WSS R-L (km/h)", "WSS R-R (km/h)", "WSS Judge", 
                                 "ABS Min F-L (kg)", "ABS Max F-L (kg)", 
                                 "ABS Min F-R (kg)", "ABS Max F-R (kg)", 
                                 "ABS Min R-L (kg)", "ABS Max R-L (kg)", 
                                 "ABS Min R-R (kg)", "ABS Max R-R (kg)", 
                                 "Front weight (kg)", "Weight F-L (kg)", "Weight F-R (kg)", 
                                 "Front drag F-L (kg)", "Front drag F-R (kg)", "Front Drag (%)", "Front Drag Judge", 
                                 "Front brake F-L (kg)", "Front brake F-R (kg)", 
                                 "Front Diff. (%)", "Front Diff. judge", 
                                 "Front Sum (%)", "Front Sum judge", "Front judge", 
                                 "Rear weight (kg)", "Weight R-L (kg)", "Weight R-R (kg)", 
                                 "Rear drag F-L (kg)", "Rear drag F-R (kg)", "Rear Drag (%)", "Rear Drag Judge", 
                                 "Rear brake F-L (kg)", "Rear brake F-R (kg)", 
                                 "Rear Diff. (%)", "Rear Diff. judge", 
                                 "Rear Sum (%)", "Rear Sum judge", "Rear judge", 
                                 "Total weight (kg)", "Total left brake (kg)", "Total right brake (kg)", 
                                 "Total brake (%)", "Total brake judge", 
                                 "Parking left brake (kg)", "Parking right brake (kg)", 
                                 "Parking brake (%)", "Parking brake judge", 
                                 "Brake judge"};

            dgv_List.ColumnCount = str_Head.Length;

            for (int cnt = 0; cnt < dgv_List.ColumnCount; cnt++)
            {
                dgv_List.Columns[cnt].HeaderText = str_Head[cnt];
            }
        }

        private void Data__Search()
        {
            if (cboModel.SelectedItem == null) return;
            strModel = cboModel.SelectedItem.ToString();
            idx_Find = cbo_Days.SelectedIndex;

            DataTable dt = main.DB_All.DB_Info.Search(strModel, idx_Find, dtpStart.Value.ToString("yyyyMMdd"), dtp__End.Value.ToString("yyyyMMdd"));
            //dgv_List.DataSource = dt;

            int SelectRow = 0;
            dgv_List.Rows.Clear();
            pgb_Data.Value = SelectRow;
            pgb_Data.Maximum = dt.Rows.Count;
            pgb_Data.Visible = true;
            pgb_Data.BringToFront();
            foreach (DataRow row in dt.Rows)
            {
                string[] str_Data = new string[88];
                #region Model
                main.DB_All.DB_Info.Select(row["dbAcceptNo"].ToString());

                str_Data[0] = Ret_WorkNo(main.DB_All.DB_Info.dbAcceptNo);
                str_Data[1] = main.DB_All.DB_Info.dbVin___No;
                str_Data[2] = main.DB_All.DB_Info.dbCarModel;
                str_Data[3] = main.DB_All.DB_Info.dbECUModel;
                str_Data[4] = main.DB_All.DB_Info.dbCarBarID;
                str_Data[5] = main.DB_All.DB_Info.dbCarWbase;
                str_Data[6] = main.DB_All.DB_Info.dbCarEngin;
                str_Data[7] = main.DB_All.DB_Info.dbCarTranM;
                str_Data[8] = main.DB_All.DB_Info.dbCar_ABST;
                str_Data[9] = main.DB_All.DB_Info.dbCarCurve;
                str_Data[10] = main.DB_All.DB_Info.dbCarDrive;
                str_Data[11] = main.DB_All.DB_Info.dbTestDate;
                #endregion

                #region RnB Data
                main.DB_All.DB_RnBs.Select(row["dbAcceptNo"].ToString());

                str_Data[12] = Ret_Data_Val(main.DB_All.DB_RnBs.dbSMTValue, 1);
                str_Data[13] = main.DB_All.DB_RnBs.dbSMT_OkNg;
                str_Data[14] = main.DB_All.DB_RnBs.db1SST_Val;   //최대 속도

                str_Data[15] = Ret_Data_Val(main.DB_All.DB_RnBs.db1Drag__L, 0);
                str_Data[16] = Ret_Data_Val(main.DB_All.DB_RnBs.db1Drag__R, 0);
                str_Data[17] = Ret_Data_Val(main.DB_All.DB_RnBs.db2Drag__L, 0);
                str_Data[18] = Ret_Data_Val(main.DB_All.DB_RnBs.db2Drag__R, 0);

                str_Data[19] = Ret_Data_Val(main.DB_All.DB_RnBs.db1Brake_L, 0);
                str_Data[20] = Ret_Data_Val(main.DB_All.DB_RnBs.db1Brake_R, 0);
                str_Data[21] = Ret_Data_Val(main.DB_All.DB_RnBs.db2Brake_L, 0);
                str_Data[22] = Ret_Data_Val(main.DB_All.DB_RnBs.db2Brake_R, 0);

                str_Data[23] = Ret_Data_Val(main.DB_All.DB_RnBs.db2Park__L, 0);
                str_Data[24] = Ret_Data_Val(main.DB_All.DB_RnBs.db2Park__R, 0);

                str_Data[25] = Ret_Data_Val(main.DB_All.DB_RnBs.db1Balan_L, 0);
                str_Data[26] = Ret_Data_Val(main.DB_All.DB_RnBs.db1Balan_R, 0);
                str_Data[27] = Ret_Data_Val(main.DB_All.DB_RnBs.db1Balance, 2);
                str_Data[28] = main.DB_All.DB_RnBs.db1Bal_Pan;

                str_Data[29] = Ret_Data_Val(main.DB_All.DB_RnBs.db2Balan_L, 0);
                str_Data[30] = Ret_Data_Val(main.DB_All.DB_RnBs.db2Balan_R, 0);
                str_Data[31] = Ret_Data_Val(main.DB_All.DB_RnBs.db2Balance, 2);
                str_Data[32] = main.DB_All.DB_RnBs.db2Bal_Pan;

                str_Data[33] = Ret_Data_Val(main.DB_All.DB_RnBs.db_BalForR, 2);
                str_Data[34] = main.DB_All.DB_RnBs.db_Balance;

                str_Data[35] = Ret_Data_Val(main.DB_All.DB_RnBs.db_Reverse, 1);

                str_Data[36] = Ret_Data_Val(main.DB_All.DB_RnBs.db1SenSpdL, 1);
                str_Data[37] = Ret_Data_Val(main.DB_All.DB_RnBs.db1SenSpdR, 1);
                str_Data[38] = Ret_Data_Val(main.DB_All.DB_RnBs.db2SenSpdL, 1);
                str_Data[39] = Ret_Data_Val(main.DB_All.DB_RnBs.db2SenSpdR, 1);
                str_Data[40] = main.DB_All.DB_RnBs.db_Sen_Spd;

                str_Data[41] = Ret_Data_Val(main.DB_All.DB_RnBs.db1ABS_DeL, 0);
                str_Data[42] = Ret_Data_Val(main.DB_All.DB_RnBs.db1ABS_InL, 0);
                str_Data[43] = Ret_Data_Val(main.DB_All.DB_RnBs.db1ABS_DeR, 0);
                str_Data[44] = Ret_Data_Val(main.DB_All.DB_RnBs.db1ABS_InR, 0);

                str_Data[45] = Ret_Data_Val(main.DB_All.DB_RnBs.db2ABS_DeL, 0);
                str_Data[46] = Ret_Data_Val(main.DB_All.DB_RnBs.db2ABS_InL, 0);
                str_Data[47] = Ret_Data_Val(main.DB_All.DB_RnBs.db2ABS_DeR, 0);
                str_Data[48] = Ret_Data_Val(main.DB_All.DB_RnBs.db2ABS_InR, 0);

                #endregion

                #region Brake Data
                main.DB_All.DBBrake.Select(row["dbAcceptNo"].ToString());

                if (main.DB_All.DBBrake.dbBrake_OX != " ")
                {
                    str_Data[49] = Ret_Data_Val(main.DB_All.DBBrake.db1_Weight, 0);
                    str_Data[50] = Ret_Data_Val(main.DB_All.DBBrake.db1_Wgt__L, 0);
                    str_Data[51] = Ret_Data_Val(main.DB_All.DBBrake.db1_Wgt__R, 0);
                    str_Data[52] = Ret_Data_Val(main.DB_All.DBBrake.db1Drag__L, 0);
                    str_Data[53] = Ret_Data_Val(main.DB_All.DBBrake.db1Drag__R, 0);
                    str_Data[54] = Ret_Data_Val(main.DB_All.DBBrake.db1Drag__V, 1);
                    str_Data[55] = main.DB_All.DBBrake.db1Drag_OX;
                    str_Data[56] = Ret_Data_Val(main.DB_All.DBBrake.db1Brake_L, 0);
                    str_Data[57] = Ret_Data_Val(main.DB_All.DBBrake.db1Brake_R, 0);
                    str_Data[58] = Ret_Data_Val(main.DB_All.DBBrake.db1Diff__V, 1);
                    str_Data[59] = main.DB_All.DBBrake.db1Diff_OX;
                    str_Data[60] = Ret_Data_Val(main.DB_All.DBBrake.db1Sum___V, 1);
                    str_Data[61] = main.DB_All.DBBrake.db1Sum__OX;
                    str_Data[62] = main.DB_All.DBBrake.db1BrakeOX;

                    str_Data[63] = Ret_Data_Val(main.DB_All.DBBrake.db2_Weight, 0);
                    str_Data[64] = Ret_Data_Val(main.DB_All.DBBrake.db2_Wgt__L, 0);
                    str_Data[65] = Ret_Data_Val(main.DB_All.DBBrake.db2_Wgt__R, 0);
                    str_Data[66] = Ret_Data_Val(main.DB_All.DBBrake.db2Drag__L, 0);
                    str_Data[67] = Ret_Data_Val(main.DB_All.DBBrake.db2Drag__R, 0);
                    str_Data[68] = Ret_Data_Val(main.DB_All.DBBrake.db2Drag__V, 1);
                    str_Data[69] = main.DB_All.DBBrake.db2Drag_OX;
                    str_Data[70] = Ret_Data_Val(main.DB_All.DBBrake.db2Brake_L, 0);
                    str_Data[71] = Ret_Data_Val(main.DB_All.DBBrake.db2Brake_R, 0);
                    str_Data[72] = Ret_Data_Val(main.DB_All.DBBrake.db2Diff__V, 1);
                    str_Data[73] = main.DB_All.DBBrake.db2Diff_OX;
                    str_Data[74] = Ret_Data_Val(main.DB_All.DBBrake.db2Sum___V, 1);
                    str_Data[75] = main.DB_All.DBBrake.db2Sum__OX;
                    str_Data[76] = main.DB_All.DBBrake.db2BrakeOX;

                    str_Data[77] = Ret_Data_Val(main.DB_All.DBBrake.dbT_Weight, 0);
                    str_Data[78] = Ret_Data_Val(main.DB_All.DBBrake.dbTBrake_L, 0);
                    str_Data[79] = Ret_Data_Val(main.DB_All.DBBrake.dbTBrake_R, 0);
                    str_Data[80] = Ret_Data_Val(main.DB_All.DBBrake.dbTBrake_V, 1);
                    str_Data[81] = main.DB_All.DBBrake.dbTBrakeOX;

                    str_Data[82] = Ret_Data_Val(main.DB_All.DBBrake.dbAPark__L, 0);
                    str_Data[83] = Ret_Data_Val(main.DB_All.DBBrake.dbAPark__R, 0);
                    str_Data[84] = Ret_Data_Val(main.DB_All.DBBrake.dbAPark__V, 1);
                    str_Data[85] = main.DB_All.DBBrake.dbAPark_OX;

                    str_Data[86] = main.DB_All.DBBrake.dbBrake_OX;
                }
                #endregion

                dgv_List.Rows.Add();
                for (int cnt = 0; cnt < dgv_List.ColumnCount; cnt++)
                {
                    dgv_List.Rows[SelectRow].Cells[cnt].Value = str_Data[cnt];
                }
                SelectRow++;
                pgb_Data.Value = SelectRow;
            }
            pgb_Data.Visible = false;
        }
        private string Ret_WorkNo(string AcptNo)
        {
            return AcptNo.Substring(0, 8) + "-" + AcptNo.Substring(8, 5);
        }
        private string Ret_Data_Val(double value, int point)
        {
            if (value == -1)
            {
                return "";
            }
            else
            {
                return Math.Round(value, point).ToString();
            }
        }

        private void Data__Export()
        {
            saveFileDialog1.InitialDirectory = Application.StartupPath + @"\Data\";
            saveFileDialog1.Filter = "txt files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() != DialogResult.OK) { return; }
            string crv_File = saveFileDialog1.FileName;

            if (crv_File == "") { return; }

            using (StreamWriter sw = new StreamWriter(crv_File, false, Encoding.Default))
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

                    string strRegNo = "";
                    string str_Data = "";
                    pgb_Data.Value = 0;
                    pgb_Data.Maximum = dgv_List.Rows.Count;
                    pgb_Data.Visible = true;
                    pgb_Data.BringToFront();
                    for(int cnt = 0; cnt < dgv_List.Rows.Count; cnt++)
                    {
                        strRegNo = dgv_List.Rows[cnt].Cells[0].Value.ToString().Replace("-", "");
                        str_Data = "";
                        pgb_Data.Value = cnt;

                        #region Model
                        main.DB_All.DB_Info.Select(strRegNo);

                        str_Data += Ret_WorkNo(main.DB_All.DB_Info.dbAcceptNo) + ", ";
                        str_Data += main.DB_All.DB_Info.dbVin___No + ", ";
                        str_Data += main.DB_All.DB_Info.dbCarModel + ", ";
                        str_Data += main.DB_All.DB_Info.dbECUModel + ", ";
                        str_Data += main.DB_All.DB_Info.dbCarBarID + ", ";
                        str_Data += main.DB_All.DB_Info.dbCarWbase + ", ";
                        str_Data += main.DB_All.DB_Info.dbCarEngin + ", ";
                        str_Data += main.DB_All.DB_Info.dbCarTranM + ", ";
                        str_Data += main.DB_All.DB_Info.dbCar_ABST + ", ";
                        str_Data += main.DB_All.DB_Info.dbCarCurve + ", ";
                        str_Data += main.DB_All.DB_Info.dbCarDrive + ", ";
                        str_Data += main.DB_All.DB_Info.dbTestDate + ", ";
                        #endregion

                        #region RnB Data
                        main.DB_All.DB_RnBs.Select(strRegNo);

                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.dbSMTValue, 1) + ", ";
                        str_Data += main.DB_All.DB_RnBs.dbSMT_OkNg + ", ";
                        str_Data += main.DB_All.DB_RnBs.db1SST_Val + ", ";   //최대 속도

                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1Drag__L, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1Drag__R, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2Drag__L, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2Drag__R, 0) + ", ";

                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1Brake_L, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1Brake_R, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2Brake_L, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2Brake_R, 0) + ", ";

                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2Park__L, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2Park__R, 0) + ", ";

                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1Balan_L, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1Balan_R, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1Balance, 2) + ", ";
                        str_Data += main.DB_All.DB_RnBs.db1Bal_Pan + ", ";

                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2Balan_L, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2Balan_R, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2Balance, 2) + ", ";
                        str_Data += main.DB_All.DB_RnBs.db2Bal_Pan + ", ";

                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db_BalForR, 2) + ", ";
                        str_Data += main.DB_All.DB_RnBs.db_Balance + ", ";

                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db_Reverse, 1) + ", ";

                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1SenSpdL, 1) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1SenSpdR, 1) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2SenSpdL, 1) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2SenSpdR, 1) + ", ";
                        str_Data += main.DB_All.DB_RnBs.db_Sen_Spd + ", ";

                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1ABS_DeL, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1ABS_InL, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1ABS_DeR, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db1ABS_InR, 0) + ", ";

                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2ABS_DeL, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2ABS_InL, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2ABS_DeR, 0) + ", ";
                        str_Data += Ret_Data_Val(main.DB_All.DB_RnBs.db2ABS_InR, 0) + ", ";

                        #endregion

                        #region Brake Data
                        main.DB_All.DBBrake.Select(strRegNo);

                        if (main.DB_All.DBBrake.dbBrake_OX != " ")
                        {
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db1_Weight, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db1_Wgt__L, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db1_Wgt__R, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db1Drag__L, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db1Drag__R, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db1Drag__V, 1) + ", ";
                            str_Data += main.DB_All.DBBrake.db1Drag_OX + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db1Brake_L, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db1Brake_R, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db1Diff__V, 1) + ", ";
                            str_Data += main.DB_All.DBBrake.db1Diff_OX + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db1Sum___V, 1) + ", ";
                            str_Data += main.DB_All.DBBrake.db1Sum__OX + ", ";
                            str_Data += main.DB_All.DBBrake.db1BrakeOX + ", ";

                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db2_Weight, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db2_Wgt__L, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db2_Wgt__R, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db2Drag__L, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db2Drag__R, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db2Drag__V, 1) + ", ";
                            str_Data += main.DB_All.DBBrake.db2Drag_OX + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db2Brake_L, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db2Brake_R, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db2Diff__V, 1) + ", ";
                            str_Data += main.DB_All.DBBrake.db2Diff_OX + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.db2Sum___V, 1) + ", ";
                            str_Data += main.DB_All.DBBrake.db2Sum__OX + ", ";
                            str_Data += main.DB_All.DBBrake.db2BrakeOX + ", ";

                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.dbT_Weight, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.dbTBrake_L, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.dbTBrake_R, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.dbTBrake_V, 1) + ", ";
                            str_Data += main.DB_All.DBBrake.dbTBrakeOX + ", ";

                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.dbAPark__L, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.dbAPark__R, 0) + ", ";
                            str_Data += Ret_Data_Val(main.DB_All.DBBrake.dbAPark__V, 1) + ", ";
                            str_Data += main.DB_All.DBBrake.dbAPark_OX + ", ";

                            str_Data += main.DB_All.DBBrake.dbBrake_OX + ", ";
                        }
                        #endregion

                        sw.WriteLine(str_Data);
                    }

                    sw.Close();

                    Logs.MakeLog_File(Log_His.Back, crv_File);
                }
                catch (Exception ex)
                {
                    sw.Close();
                }
                pgb_Data.Visible = false;
            }
        }

        private void dgv_List_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rect = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgv_List.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dgv_List.RowHeadersDefaultCellStyle.Font, rect,
                         dgv_List.ForeColor = Color.DimGray, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }
    }
}
