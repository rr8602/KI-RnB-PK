using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Windows.Forms;

namespace KI_RnB
{
    /// <summary>
    /// DB 연결 관리
    /// </summary>
    public class MDB_Sql
    {
        private string con_MDBs = Application.StartupPath + @"\DB\KI-RnB.mdb";
        private string con_MDFs = Application.StartupPath + @"\DB\KI-RnB.mdf";
        private OleDbConnection DBs_Conn;

        public MDB_Sql()
        {
            if (!System.IO.File.Exists(con_MDBs))
            {
                MessageBoxEx.Show(con_MDBs + " There are no files. Please contact your system administrator.");
                return;
            }

            //확장자 .mdb   => Provider=Microsoft.Jet.OleDb.4.0
            //확장자 .accdb => Provider=Microsoft.ACE.OLEDB.12.0
            DBs_Conn = new OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0;" + "Data Source=" + con_MDBs);    // + ";Jet OLEDB:Database Password=kimc8511");

            try
            {
                DBs_Conn.Open();
            }
            catch (OleDbException ex)
            {
                string errorMessages = "";

                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages += "Index #" + i + "\n" +
                                     "Message: " + ex.Errors[i].Message + "\n" +
                                     "NativeError: " + ex.Errors[i].NativeError + "\n" +
                                     "Source: " + ex.Errors[i].Source + "\n" +
                                     "SQLState: " + ex.Errors[i].SQLState + "\n";
                }

                System.Diagnostics.EventLog log = new System.Diagnostics.EventLog();
                log.Source = "My Application";
                log.WriteEntry(errorMessages);
                System.Windows.Forms.MessageBox.Show("An exception occurred. Please contact your system administrator.");
            }
        }

        /// <summary>
        /// 다중 레코드 선택
        /// </summary>
        /// <param name="pSql">일자별 검색</param>
        /// <returns>DataSet</returns>
        public DataSet DS_Select(string pSql)
        {
            DataSet ds = new DataSet();

            using (OleDbDataAdapter adapter = new OleDbDataAdapter(pSql, DBs_Conn))
            {
                try
                {
                    adapter.Fill(ds);
                }
                catch (Exception ex)
                {
                    Logs.MakeLog_File(Log_His.DBEr, ex.Message + " " + pSql);
                    MessageBoxEx.Show(ex.Message + " " + pSql);
                }
            }

            return ds;
        }

        /// <summary>
        /// 단일 테일블 검색
        /// </summary>
        /// <param name="pSql">검색 조건</param>
        /// <returns>단일 테이블</returns>
        public DataTable DT_Select(string pSql)
        {
            DataTable dt = new DataTable();

            using (OleDbDataAdapter adapter = new OleDbDataAdapter(pSql, DBs_Conn))
            {
                try
                {
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    Logs.MakeLog_File(Log_His.DBEr, ex.Message + " " + pSql);
                    MessageBoxEx.Show(ex.Message + " " + pSql);
                }
            }

            return dt;
        }

        /// <summary>
        /// 즉시 실행 명령문
        /// </summary>
        /// <param name="pSql">실행 sql</param>
        public void Execute(string pSql)
        {
            using (OleDbCommand cmd = new OleDbCommand(pSql, DBs_Conn))
            {
                try
                {
                    //OleDbTransaction trans = DBs_Conn.BeginTransaction();
                    //cmd.Transaction = trans;
                    //cmd.CommandText = pSql;
                    cmd.ExecuteNonQuery();
                    Logs.MakeLog_File(Log_His.Save, pSql);

                    //trans.Commit();
                    //trans = null;

                }
                catch (Exception ex)
                {
                    Logs.MakeLog_File(Log_His.DBEr, ex.Message + " " + pSql);
                    MessageBoxEx.Show(ex.Message + " " + pSql);
                }
            }
        }

        /// <summary>
        /// 필드 생성용
        /// </summary>
        /// <param name="pSql"></param>
        public void Create(string pSql)
        {
            using (OleDbCommand cmd = new OleDbCommand(pSql, DBs_Conn))
            {
                //OleDbTransaction trans = DBs_Conn.BeginTransaction();
                try
                {
                    //cmd.Transaction = trans;
                    //cmd.CommandText = pSql;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                //trans.Commit();
                //trans = null;
            }
        }

        public void Close()
        {
        }
    }

    public class MS__Sql
    {
        /*
             * 203.251.73.188   :   외부에서 사용 할 시
             * 172.16.10.20     :   내부에서 사용 할 시
             */
        //string sConnectionString = "server=203.251.73.188; database=kiyasakadb;user id=kiyasaka;Password=daedongkiyasaka;";
        private string sConnectionString = "server=172.16.10.20; database=kiyasakadb;user id=kiyasaka;Password=daedongkiyasaka;";

        private SqlConnection dbConn;
        private SqlCommand dbCmd;
        private SqlDataAdapter dbDa;
        private SqlTransaction dbTrans;

        public MS__Sql()
        {
            //생성
            dbConn = new SqlConnection(sConnectionString);
            dbTrans = null;

            Console.WriteLine("DB 연결 정보 생성");

            //연결 하기
            if (dbConn.State != ConnectionState.Closed)
            {
                dbConn.Close();
            }
            dbConn = new SqlConnection(sConnectionString);
            dbConn.Open();

            dbCmd = new SqlCommand(string.Empty, dbConn);
            dbCmd.CommandTimeout = 600;
            dbDa = new SqlDataAdapter(string.Empty, dbConn);
        }

        /// <summary>
        /// 다중 레코드 선택
        /// </summary>
        /// <param name="pSql">일자별 검색</param>
        /// <returns>DataSet</returns>
        public DataSet DS_Select(string pSql)
        {
            DataSet ds = new DataSet();
            dbCmd.CommandText = pSql;
            dbDa.SelectCommand = dbCmd;
            dbDa.Fill(ds);

            return ds;
        }

        /// <summary>
        /// 단일 테일블 검색
        /// </summary>
        /// <param name="pSql">검색 조건</param>
        /// <returns>단일 테이블</returns>
        public DataTable DT_Select(string pSql)
        {
            DataTable dt = new DataTable();

            dbCmd.CommandText = pSql;
            dbDa.SelectCommand = dbCmd;
            dbDa.Fill(dt);

            return dt;
        }

        /// <summary>
        /// 즉시 실행 명령문
        /// </summary>
        /// <param name="pSql">실행 sql</param>
        public void Execute(string pSql)
        {
            //Begin
            dbTrans = dbConn.BeginTransaction();
            dbCmd.Transaction = dbTrans;

            dbCmd.CommandText = pSql;
            dbCmd.ExecuteNonQuery();

            //Commit
            dbTrans.Commit();
            dbTrans = null;

            //Rollback
            //dbTrans.Rollback();
            //dbTrans = null;
        }

        public void Close()
        {
            //연결 끊기
            if (dbConn != null)
            {
                if (dbCmd != null)
                {
                    dbCmd.Dispose();
                }
                if (dbDa != null)
                {
                    dbDa.Dispose();
                }
                if (dbTrans != null)
                {
                    dbTrans.Dispose();
                }
                if (dbConn.State != ConnectionState.Closed)
                {
                    dbConn.Close();
                }
                dbConn.Dispose();
            }
        }
    }
    
    public class MDB_Alls
    {
        public tbl_Info DB_Info;
        public tblModel DBModel;
        public tbl_RnBs DB_RnBs;
        public tbl_Item DB_Item;
        public tblCurve DBCurve;
        public tbl_ECUs DB_ECUs;
        public tblParam DBParam;
        public tblBrake DBBrake;

        MDB_Sql DB_Sql;

        public MDB_Alls()
        {
            DB_Sql = new MDB_Sql();

            DB_Info.MDB = DB_Sql;
            DBModel.MDB = DB_Sql;
            DB_RnBs.MDB = DB_Sql;
            DB_Item.MDB = DB_Sql;
            DBCurve.MDB = DB_Sql;
            DB_ECUs.MDB = DB_Sql;
            DBParam.MDB = DB_Sql;
            DBBrake.MDB = DB_Sql;
        }
    }
    
    public struct tbl_Info
    {
        public MDB_Sql MDB;
        const string str_Info = "dbAcceptNo, dbVin___No, dbCarModel, dbECUModel, dbCarBarID, dbCarEngin, dbCarTranM, dbCar_ABST, dbCarCurve, dbCarDrive, dbCarWbase, dbTestDate, dbRun_Time, dbTestTime, dbEnd_Time ";
        
        #region Vehicle Info DB Table - Methods
        public string dbAcceptNo { get; set; }  //측정 벊소
        public string dbVin___No { get; set; }  //바코드
        public string dbCarModel { get; set; }  //모델명
        public string dbECUModel { get; set; }  //ECU 모델명
        public string dbCarBarID { get; set; }  //바코드 구분자
        public string dbCarEngin { get; set; }  //엔진 모델
        public string dbCarTranM { get; set; }  //트렌스미션 모델
        public string dbCar_ABST { get; set; }  //ABS Brake 모델

        public string dbCarCurve { get; set; }  //드라이브 커브
        public string dbCarDrive { get; set; }  //드라이브 모드
        public string dbCarWbase { get; set; }  //휠베이스 거리
        public string dbTestDate { get; set; }  //측정 일자
        public string dbRun_Time { get; set; }  //진행 시작 시간
        public string dbTestTime { get; set; }  //측정 시각 시간
        public string dbEnd_Time { get; set; }  //종료 시간
        #endregion

        #region ABS Brake DB Table - Functions
        private void Init()
        {
            dbAcceptNo = "";
            dbVin___No = ""; 
            dbCarModel = "";
            dbECUModel = "";
            dbCarBarID = "";
            dbCarEngin = "";
            dbCarTranM = "";
            dbCar_ABST = "";

            dbCarCurve = "";
            dbCarDrive = "";
            dbCarWbase = "";
            dbTestDate = "";
            dbRun_Time = "";
            dbTestTime = "";
            dbEnd_Time = "";
        }

        public DataTable Search(string pDate)
        {
            string sql =  "SELECT * FROM tbl_InfoData ";
                   sql += "WHERE dbTestDate='" + pDate + "' ";
                   sql += "ORDER BY dbAcceptNo DESC";   //ASC

            return MDB.DT_Select(sql);
        }

        public DataTable Search(string model, int idx, string pStt, string pEnd)
        {
            string sql = "SELECT * FROM tbl_InfoData ";
            //switch(idx)
            //{
            //    case 0: sql += "WHERE dbTestDate='" + pStt + "' "; break;   //today
            //    case 1: sql += "WHERE dbTestDate='" + pStt + "' "; break;   //This week
            //    case 2: sql += "WHERE dbTestDate='" + pStt + "' "; break;   //This month
            //    case 3: sql += "WHERE dbTestDate='" + pStt + "' "; break;   //Search
            //}
            if (model == "All Model")
            {
                sql += "WHERE dbTestDate>='" + pStt + "' AND dbTestDate<='" + pEnd + "' ";
            }
            else
            {
                sql += "WHERE dbCarModel='" + model + "' AND dbTestDate>='" + pStt + "' AND dbTestDate<='" + pEnd + "' ";
            }

            sql += "ORDER BY dbAcceptNo ASC";   

            return MDB.DT_Select(sql);
        }

        public DataTable Backup(string pDate)
        {
            string sql = "SELECT * FROM tbl_InfoData    ";
            sql += "WHERE MID(dbTestDate, 1, 6)='" + pDate + "'";
            sql += "ORDER BY dbAcceptNo ASC";           //DESC

            return MDB.DT_Select(sql);
        }

        public DataTable Backup_Join(string pDate)  //3개 조인 에러
        {
            string sql = "SELECT * ";
            sql += "FROM      [tbl_InfoData] AS I ";
            sql += "LEFT JOIN [tbl_RnB_Data] AS R ON I.dbAcceptNo=R.dbAcceptNo AND ";
            sql += "LEFT JOIN [tbl_ABS_Data] AS A ON I.dbAcceptNo=A.dbAcceptNo ";
            sql += "WHERE MID(I.dbTestDate, 1, 6)='" + pDate + "' ";
            sql += "ORDER BY I.dbAcceptNo ASC";           //DESC

            return MDB.DT_Select(sql);
        }

        public int Max_No(string pDate)
        {
            string sql = "SELECT MAX(MID(dbAcceptNo,9,5)) FROM tbl_InfoData    WHERE dbTestDate='" + pDate + "'";
            DataTable dt = MDB.DT_Select(sql);

            try
            {
                if (dt.Rows.Count == 1)
                {
                    string str_no = dt.Rows[0][0].ToString();
                    return int.Parse(str_no);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //Main.Prog_LogData(ex.Message);
                return 0;
            }
        }

        public int Select(string pAcptNo)
        {
            string sql = "SELECT * FROM tbl_InfoData WHERE dbAcceptNo='" + pAcptNo + "'";
            DataTable dt = MDB.DT_Select(sql);

            Init();

            if (dt.Rows.Count == 1)
            {
                dbAcceptNo = dt.Rows[0]["dbAcceptNo"].ToString();
                dbVin___No = dt.Rows[0]["dbVin___No"].ToString();
                dbCarModel = dt.Rows[0]["dbCarModel"].ToString();
                dbECUModel = dt.Rows[0]["dbECUModel"].ToString();
                dbCarBarID = dt.Rows[0]["dbCarBarID"].ToString();
                dbCarEngin = dt.Rows[0]["dbCarEngin"].ToString();
                dbCarTranM = dt.Rows[0]["dbCarTranM"].ToString();
                dbCar_ABST = dt.Rows[0]["dbCar_ABST"].ToString();

                dbCarCurve = dt.Rows[0]["dbCarCurve"].ToString();
                dbCarDrive = dt.Rows[0]["dbCarDrive"].ToString();
                dbCarWbase = dt.Rows[0]["dbCarWbase"].ToString();
                dbTestDate = dt.Rows[0]["dbTestDate"].ToString();
                dbRun_Time = dt.Rows[0]["dbRun_Time"].ToString();
                dbTestTime = dt.Rows[0]["dbTestTime"].ToString();
                dbEnd_Time = dt.Rows[0]["dbEnd_Time"].ToString();
            }

            return dt.Rows.Count;
        }

        public void Insert()
        {
            string sql = "INSERT INTO tbl_InfoData ( " + str_Info + " ) values (";
                   sql += string.Format(H2Y.Sql_Insert(str_Info) + ") ",
                                dbAcceptNo, dbVin___No, dbCarModel, dbECUModel, dbCarBarID, dbCarEngin, dbCarTranM, dbCar_ABST, dbCarCurve, dbCarDrive, dbCarWbase, dbTestDate, dbRun_Time, dbTestTime, dbEnd_Time);

            MDB.Execute(sql);
        }

        public void Update(string pAcptNo)
        {
            string sql = string.Format("UPDATE tbl_InfoData SET " + H2Y.Sql_Update(str_Info),
                                dbAcceptNo, dbVin___No, dbCarModel, dbECUModel, dbCarBarID, dbCarEngin, dbCarTranM, dbCar_ABST, dbCarCurve, dbCarDrive, dbCarWbase, dbTestDate, dbRun_Time, dbTestTime, dbEnd_Time);
                   sql += string.Format("WHERE dbAcceptNo='{0}'", pAcptNo);

            MDB.Execute(sql);
        }

        public void Delete(string pAcptNo)
        {
            string sql = "DELETE FROM tbl_InfoData WHERE dbAcceptNo = '" + pAcptNo + "'";

            MDB.Execute(sql);
        }
        #endregion
    }

    public struct tblModel
    {
        public MDB_Sql MDB;
        const string strModel = "dbCarIndex, dbCarModel, dbECUModel, dbCarBarID, dbCarEngin, dbCarTranM, dbCar_ABST, dbCarCurve, dbCarDrive, dbCarWbase, dbCarParam ";
        const string str_List = "            dbCarModel, dbECUModel, dbCarBarID, dbCarEngin, dbCarTranM, dbCar_ABST, dbCarCurve, dbCarDrive, dbCarWbase, dbCarParam ";
        
        #region Model DB Table - Methods
        public string dbCarIndex { get; set; }  //Model Number
        public string dbCarModel { get; set; }  //Model Name
        public string dbECUModel { get; set; }  //ECU Model
        public string dbCarBarID { get; set; }  //Barcode
        public string dbCarEngin { get; set; }  //Engine Name
        public string dbCarTranM { get; set; }  //Transmission
        public string dbCar_ABST { get; set; }  //ABS ECU Type
        public string dbCarCurve { get; set; }  //Drive Curve
        public string dbCarDrive { get; set; }  //Drive Mode
        public string dbCarWbase { get; set; }  //Wheelbase Length
        public string dbCarParam { get; set; }  //Parameter
        #endregion

        #region Model DB Table - Functions
        private void Init()
        {
            dbCarIndex = "";
            dbCarModel = "";
            dbECUModel = "";
            dbCarBarID = "";
            dbCarEngin = "";
            dbCarTranM = "";
            dbCar_ABST = "";
            dbCarCurve = "";
            dbCarDrive = "";
            dbCarWbase = "";
            dbCarParam = "";
        }

        public DataTable Search()
        {
            string sql = "SELECT " + str_List + " FROM tbl_CarModel    ORDER BY dbCarIndex ASC";   //DESC
            DataTable dt = MDB.DT_Select(sql);
            return dt;
        }

        public string KeyBox(string pWBase)
        {
            string sql = "SELECT * FROM tbl_CarModel WHERE dbCarWbase='" + pWBase + "'";
            DataTable dt = MDB.DT_Select(sql);

            Init();

            if (dt.Rows.Count == 1)
            {
                dbCarIndex = dt.Rows[0]["dbCarIndex"].ToString();
                dbCarModel = dt.Rows[0]["dbCarModel"].ToString();
                dbECUModel = dt.Rows[0]["dbECUModel"].ToString();
                dbCarBarID = dt.Rows[0]["dbCarBarID"].ToString();
                dbCarEngin = dt.Rows[0]["dbCarEngin"].ToString();
                dbCarTranM = dt.Rows[0]["dbCarTranM"].ToString();
                dbCar_ABST = dt.Rows[0]["dbCar_ABST"].ToString();
                dbCarCurve = dt.Rows[0]["dbCarCurve"].ToString();
                dbCarDrive = dt.Rows[0]["dbCarDrive"].ToString();
                dbCarWbase = dt.Rows[0]["dbCarWbase"].ToString();
                dbCarParam = dt.Rows[0]["dbCarParam"].ToString();
            }

            return dbCarModel;
        }
        public int Select(string pModel)
        {
            string sql = "SELECT * FROM tbl_CarModel WHERE dbCarModel='" + pModel + "'";
            DataTable dt = MDB.DT_Select(sql);

            Init();

            if (dt.Rows.Count == 1)
            {
                dbCarIndex = dt.Rows[0]["dbCarIndex"].ToString();
                dbCarModel = dt.Rows[0]["dbCarModel"].ToString();
                dbECUModel = dt.Rows[0]["dbECUModel"].ToString();
                dbCarBarID = dt.Rows[0]["dbCarBarID"].ToString();
                dbCarEngin = dt.Rows[0]["dbCarEngin"].ToString();
                dbCarTranM = dt.Rows[0]["dbCarTranM"].ToString();
                dbCar_ABST = dt.Rows[0]["dbCar_ABST"].ToString();
                dbCarCurve = dt.Rows[0]["dbCarCurve"].ToString();
                dbCarDrive = dt.Rows[0]["dbCarDrive"].ToString();
                dbCarWbase = dt.Rows[0]["dbCarWbase"].ToString();
                dbCarParam = dt.Rows[0]["dbCarParam"].ToString();
            }

            return dt.Rows.Count;
        }
        public int Select(int pIndex)
        {
            string sql = "SELECT * FROM tbl_CarModel WHERE dbCarIndex='" + pIndex + "'";
            DataTable dt = MDB.DT_Select(sql);

            Init();

            if (dt.Rows.Count == 1)
            {
                dbCarIndex = dt.Rows[0]["dbCarIndex"].ToString();
                dbCarModel = dt.Rows[0]["dbCarModel"].ToString();
                dbECUModel = dt.Rows[0]["dbECUModel"].ToString();
                dbCarBarID = dt.Rows[0]["dbCarBarID"].ToString();
                dbCarEngin = dt.Rows[0]["dbCarEngin"].ToString();
                dbCarTranM = dt.Rows[0]["dbCarTranM"].ToString();
                dbCar_ABST = dt.Rows[0]["dbCar_ABST"].ToString();
                dbCarCurve = dt.Rows[0]["dbCarCurve"].ToString();
                dbCarDrive = dt.Rows[0]["dbCarDrive"].ToString();
                dbCarWbase = dt.Rows[0]["dbCarWbase"].ToString();
                dbCarParam = dt.Rows[0]["dbCarParam"].ToString();
            }

            return dt.Rows.Count;
        }

        public int Barcode(string pBarcode)
        {
            string sql = "SELECT * FROM tbl_CarModel WHERE dbCarBarID ='" + pBarcode + "' ";
            DataTable dt = MDB.DT_Select(sql);

            Init();

            if (dt.Rows.Count == 1)
            {
                dbCarIndex = dt.Rows[0]["dbCarIndex"].ToString();
                dbCarModel = dt.Rows[0]["dbCarModel"].ToString();
                dbECUModel = dt.Rows[0]["dbECUModel"].ToString();
                dbCarBarID = dt.Rows[0]["dbCarBarID"].ToString();
                dbCarEngin = dt.Rows[0]["dbCarEngin"].ToString();
                dbCarTranM = dt.Rows[0]["dbCarTranM"].ToString();
                dbCar_ABST = dt.Rows[0]["dbCar_ABST"].ToString();
                dbCarCurve = dt.Rows[0]["dbCarCurve"].ToString();
                dbCarDrive = dt.Rows[0]["dbCarDrive"].ToString();
                dbCarWbase = dt.Rows[0]["dbCarWbase"].ToString();
                dbCarParam = dt.Rows[0]["dbCarParam"].ToString();
            }

            return dt.Rows.Count;
        }

        public int Number()
        {
            string sql = "SELECT MAX(dbCarIndex) FROM tbl_CarModel";
            DataTable dt = MDB.DT_Select(sql);

            try
            {
                if (dt.Rows.Count == 1)
                {
                    string str_no = dt.Rows[0][0].ToString();
                    return int.Parse(str_no);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void Insert()
        {
            string sql = "INSERT INTO tbl_CarModel ( " + strModel + " ) values (";
                   sql += string.Format(H2Y.Sql_Insert(strModel) + ") ",
                                dbCarIndex, dbCarModel, dbECUModel, dbCarBarID, dbCarEngin, dbCarTranM, dbCar_ABST, dbCarCurve, dbCarDrive, dbCarWbase, dbCarParam);

                   MDB.Execute(sql);
        }

        public void Update(string pModel)
        {
            string sql = string.Format("UPDATE tbl_CarModel SET " + H2Y.Sql_Update(strModel),
                                dbCarIndex, dbCarModel, dbECUModel, dbCarBarID, dbCarEngin, dbCarTranM, dbCar_ABST, dbCarCurve, dbCarDrive, dbCarWbase, dbCarParam);
                   sql+= string.Format("WHERE dbCarModel='{0}'", pModel);

                   MDB.Execute(sql);
        }
        public void Update(int pIndex)
        {
            string sql = string.Format("UPDATE tbl_CarModel SET " + H2Y.Sql_Update(strModel),
                                dbCarIndex, dbCarModel, dbECUModel, dbCarBarID, dbCarEngin, dbCarTranM, dbCar_ABST, dbCarCurve, dbCarDrive, dbCarWbase, dbCarParam);
                   sql += string.Format("WHERE dbCarIndex='{0}'", pIndex);

            MDB.Execute(sql);
        }
        
        public void Delete(string pModel)
        {
            string sql = "DELETE FROM tbl_CarModel WHERE dbCarModel = '" + pModel + "'";

            MDB.Execute(sql);
        }
        public void Delete(int pIndex)
        {
            string sql = "DELETE FROM tbl_CarModel WHERE dbCarIndex = '" + pIndex + "'";

            MDB.Execute(sql);
        }
        #endregion
    }

    public struct tbl_RnBs
    {
        public MDB_Sql MDB;
        const string str_RnBs = "dbAcceptNo, db1SST_Val, db1SSTSine, db1SSTOkNg, db2SST_Val, db2SSTSine, db2SSTOkNg, db_SSTOkNg, " +
                                            "db1_Weight, db1Drag__L, db1Drag__R, db1Brake_L, db1Brake_R, db1Park__L, db1Park__R, db1BrakeOX, " +
                                            "db2_Weight, db2Drag__L, db2Drag__R, db2Brake_L, db2Brake_R, db2Park__L, db2Park__R, db2BrakeOX, " +
                                            "db1Balan_L, db1Balan_R, db1Balance, db1Bal_Pan, " +
                                            "db2Balan_L, db2Balan_R, db2Balance, db2Bal_Pan, db_BalForR, db_Balance, " +
                                            "dbSMTValue, dbSMT_OkNg, db_Reverse, db_Rev_Pan, db1SenSpdL, db1SenSpdR, db2SenSpdL, db2SenSpdR, db_Sen_Spd, " + 
                                            "db1ABS_DeL, db1ABS_InL, db1ABS_DeR, db1ABS_InR, db2ABS_DeL, db2ABS_InL, db2ABS_DeR, db2ABS_InR";


        #region RnB DB Table - Methods
        public string dbAcceptNo { get; set; }  //000A,2011109,00001
        public string db1SST_Val { get; set; }  //
        public string db1SSTSine { get; set; }  //
        public string db1SSTOkNg { get; set; }  //
        public string db2SST_Val { get; set; }  //
        public string db2SSTSine { get; set; }  //
        public string db2SSTOkNg { get; set; }  //
        public string db_SSTOkNg { get; set; }  //

        public double db1_Weight { get; set; }  //
        public double db1Drag__L { get; set; }  //
        public double db1Drag__R { get; set; }  //
        public double db1Brake_L { get; set; }  //
        public double db1Brake_R { get; set; }  //
        public double db1Park__L { get; set; }  //
        public double db1Park__R { get; set; }  //
        public string db1BrakeOX { get; set; }  //

        public double db2_Weight { get; set; }  //
        public double db2Drag__L { get; set; }  //
        public double db2Drag__R { get; set; }  //
        public double db2Brake_L { get; set; }  //
        public double db2Brake_R { get; set; }  //
        public double db2Park__L { get; set; }  //
        public double db2Park__R { get; set; }  //
        public string db2BrakeOX { get; set; }  //

        public double db1Balan_L { get; set; }  //
        public double db1Balan_R { get; set; }  //
        public double db1Balance { get; set; }  //
        public string db1Bal_Pan { get; set; }  //
        public double db2Balan_L { get; set; }  //
        public double db2Balan_R { get; set; }  //
        public double db2Balance { get; set; }  //
        public string db2Bal_Pan { get; set; }  //
        public double db_BalForR { get; set; }  //
        public string db_Balance { get; set; }  //

        public double dbSMTValue { get; set; }  //
        public string dbSMT_OkNg { get; set; }  //

        public double db_Reverse { get; set; }  //
        public string db_Rev_Pan { get; set; }  //

        public double db1SenSpdL { get; set; }  //
        public double db1SenSpdR { get; set; }  //
        public double db2SenSpdL { get; set; }  //
        public double db2SenSpdR { get; set; }  //
        public string db_Sen_Spd { get; set; }  //

        public double db1ABS_DeL { get; set; }  //
        public double db1ABS_InL { get; set; }  //
        public double db1ABS_DeR { get; set; }  //
        public double db1ABS_InR { get; set; }  //
        public double db2ABS_DeL { get; set; }  //
        public double db2ABS_InL { get; set; }  //
        public double db2ABS_DeR { get; set; }  //
        public double db2ABS_InR { get; set; }  //
        #endregion

        #region RnB DB Table - Functions
        private void Init()
        {
            dbAcceptNo = "";
            db1SST_Val = ""; db2SST_Val = "";
            db1SSTSine = ""; db2SSTSine = "";
            db1SSTOkNg = ""; db2SSTOkNg = "";
            db_SSTOkNg = "";
            
            db1_Weight = -1; db2_Weight = -1;
            db1Drag__L = -1; db2Drag__L = -1;
            db1Drag__R = -1; db2Drag__R = -1;
            db1Brake_L = -1; db2Brake_L = -1;
            db1Brake_R = -1; db2Brake_R = -1;
            db1Park__L = -1; db2Park__L = -1;
            db1Park__R = -1; db2Park__R = -1;
            db1BrakeOX = ""; db2BrakeOX = "";

            db1Balan_L = -1; db2Balan_L = -1;
            db1Balan_R = -1; db2Balan_R = -1;
            db1Balance = -1; db2Balance = -1;
            db1Bal_Pan = ""; db2Bal_Pan = "";
            db_BalForR = -1; db_Balance = "";

            dbSMTValue = -1; dbSMT_OkNg = "";
            db_Reverse = -1; db_Rev_Pan = "";

            db1SenSpdL = -1; db1SenSpdR = -1;
            db2SenSpdL = -1; db2SenSpdR = -1; db_Sen_Spd = "";

            db1ABS_DeL = -1; db2ABS_DeL = -1;
            db1ABS_InL = -1; db2ABS_InL = -1;
            db1ABS_DeR = -1; db2ABS_DeR = -1;
            db1ABS_InR = -1; db2ABS_InR = -1;
        }

        public DataTable Search()
        {
            string sql = "SELECT " + str_RnBs + " FROM tbl_RnB_Data    ORDER BY dbAcceptNo ASC";   //DESC
            DataTable dt = MDB.DT_Select(sql);
            return dt;
        }

        public int Select(string pAcptNo)
        {
            string sql = "SELECT * FROM tbl_RnB_Data WHERE dbAcceptNo='" + pAcptNo + "'";
            DataTable dt = MDB.DT_Select(sql);

            Init();

            if (dt.Rows.Count == 1)
            {
                dbAcceptNo = dt.Rows[0]["dbAcceptNo"].ToString();

                db1SST_Val = dt.Rows[0]["db1SST_Val"].ToString();
                db1SSTSine = dt.Rows[0]["db1SSTSine"].ToString();
                db1SSTOkNg = dt.Rows[0]["db1SSTOkNg"].ToString();
                db2SST_Val = dt.Rows[0]["db2SST_Val"].ToString();
                db2SSTSine = dt.Rows[0]["db2SSTSine"].ToString();
                db2SSTOkNg = dt.Rows[0]["db2SSTOkNg"].ToString();
                db_SSTOkNg = dt.Rows[0]["db_SSTOkNg"].ToString();

                db1_Weight = H2Y.StrToSingles(dt.Rows[0]["db1_Weight"].ToString());
                db1Drag__L = H2Y.StrToSingles(dt.Rows[0]["db1Drag__L"].ToString());
                db1Drag__R = H2Y.StrToSingles(dt.Rows[0]["db1Drag__R"].ToString());
                db1Brake_L = H2Y.StrToSingles(dt.Rows[0]["db1Brake_L"].ToString());
                db1Brake_R = H2Y.StrToSingles(dt.Rows[0]["db1Brake_R"].ToString());
                db1Park__L = H2Y.StrToSingles(dt.Rows[0]["db1Park__L"].ToString());
                db1Park__R = H2Y.StrToSingles(dt.Rows[0]["db1Park__R"].ToString());
                db1BrakeOX = dt.Rows[0]["db1BrakeOX"].ToString();

                db2_Weight = H2Y.StrToSingles(dt.Rows[0]["db2_Weight"].ToString());
                db2Drag__L = H2Y.StrToSingles(dt.Rows[0]["db2Drag__L"].ToString());
                db2Drag__R = H2Y.StrToSingles(dt.Rows[0]["db2Drag__R"].ToString());
                db2Brake_L = H2Y.StrToSingles(dt.Rows[0]["db2Brake_L"].ToString());
                db2Brake_R = H2Y.StrToSingles(dt.Rows[0]["db2Brake_R"].ToString());
                db2Park__L = H2Y.StrToSingles(dt.Rows[0]["db2Park__L"].ToString());
                db2Park__R = H2Y.StrToSingles(dt.Rows[0]["db2Park__R"].ToString());
                db2BrakeOX = dt.Rows[0]["db2BrakeOX"].ToString();

                db1Balan_L = H2Y.StrToSingles(dt.Rows[0]["db1Balan_L"].ToString());
                db1Balan_R = H2Y.StrToSingles(dt.Rows[0]["db1Balan_R"].ToString());
                db1Balance = H2Y.StrToSingles(dt.Rows[0]["db1Balance"].ToString());
                db1Bal_Pan = dt.Rows[0]["db1Bal_Pan"].ToString();
                db2Balan_L = H2Y.StrToSingles(dt.Rows[0]["db2Balan_L"].ToString());
                db2Balan_R = H2Y.StrToSingles(dt.Rows[0]["db2Balan_R"].ToString());
                db2Balance = H2Y.StrToSingles(dt.Rows[0]["db2Balance"].ToString());
                db2Bal_Pan = dt.Rows[0]["db2Bal_Pan"].ToString();
                db_BalForR = H2Y.StrToSingles(dt.Rows[0]["db_BalForR"].ToString());
                db_Balance = dt.Rows[0]["db_Balance"].ToString();

                dbSMTValue = H2Y.StrToSingles(dt.Rows[0]["dbSMTValue"].ToString());
                dbSMT_OkNg = dt.Rows[0]["dbSMT_OkNg"].ToString();

                db_Reverse = H2Y.StrToSingles(dt.Rows[0]["db_Reverse"].ToString());
                db_Rev_Pan = dt.Rows[0]["db_Rev_Pan"].ToString();

                db1SenSpdL = H2Y.StrToSingles(dt.Rows[0]["db1SenSpdL"].ToString());
                db1SenSpdR = H2Y.StrToSingles(dt.Rows[0]["db1SenSpdR"].ToString());
                db2SenSpdL = H2Y.StrToSingles(dt.Rows[0]["db2SenSpdL"].ToString());
                db2SenSpdR = H2Y.StrToSingles(dt.Rows[0]["db2SenSpdR"].ToString());
                db_Sen_Spd = dt.Rows[0]["db_Sen_Spd"].ToString();

                db1ABS_DeL = H2Y.StrToSingles(dt.Rows[0]["db1ABS_DeL"].ToString());
                db1ABS_InL = H2Y.StrToSingles(dt.Rows[0]["db1ABS_InL"].ToString());
                db1ABS_DeR = H2Y.StrToSingles(dt.Rows[0]["db1ABS_DeR"].ToString());
                db1ABS_InR = H2Y.StrToSingles(dt.Rows[0]["db1ABS_InR"].ToString());
                db2ABS_DeL = H2Y.StrToSingles(dt.Rows[0]["db2ABS_DeL"].ToString());
                db2ABS_InL = H2Y.StrToSingles(dt.Rows[0]["db2ABS_InL"].ToString());
                db2ABS_DeR = H2Y.StrToSingles(dt.Rows[0]["db2ABS_DeR"].ToString());
                db2ABS_InR = H2Y.StrToSingles(dt.Rows[0]["db2ABS_InR"].ToString());
            }

            return dt.Rows.Count;
        }

        public void Insert()
        {
            string sql = "INSERT INTO tbl_RnB_Data ( " + str_RnBs + " ) values (";
                   sql += string.Format(H2Y.Sql_Insert(str_RnBs) + ") ",
                                 dbAcceptNo, db1SST_Val, db1SSTSine, db1SSTOkNg, db2SST_Val, db2SSTSine, db2SSTOkNg, db_SSTOkNg, 
                                             db1_Weight, db1Drag__L, db1Drag__R, db1Brake_L, db1Brake_R, db1Park__L, db1Park__R, db1BrakeOX, 
                                             db2_Weight, db2Drag__L, db2Drag__R, db2Brake_L, db2Brake_R, db2Park__L, db2Park__R, db2BrakeOX, 
                                             db1Balan_L, db1Balan_R, db1Balance, db1Bal_Pan, 
                                             db2Balan_L, db2Balan_R, db2Balance, db2Bal_Pan, db_BalForR, db_Balance, 
                                             dbSMTValue, dbSMT_OkNg, db_Reverse, db_Rev_Pan, db1SenSpdL, db1SenSpdR, db2SenSpdL, db2SenSpdR, db_Sen_Spd, 
                                             db1ABS_DeL, db1ABS_InL, db1ABS_DeR, db1ABS_InR, db2ABS_DeL, db2ABS_InL, db2ABS_DeR, db2ABS_InR);

            MDB.Execute(sql);
        }

        public void Update(string pAcptNo)
        {
            string sql = string.Format("UPDATE tbl_RnB_Data SET " + H2Y.Sql_Update(str_RnBs),
                                 dbAcceptNo, db1SST_Val, db1SSTSine, db1SSTOkNg, db2SST_Val, db2SSTSine, db2SSTOkNg, db_SSTOkNg,
                                             db1_Weight, db1Drag__L, db1Drag__R, db1Brake_L, db1Brake_R, db1Park__L, db1Park__R, db1BrakeOX,
                                             db2_Weight, db2Drag__L, db2Drag__R, db2Brake_L, db2Brake_R, db2Park__L, db2Park__R, db2BrakeOX,
                                             db1Balan_L, db1Balan_R, db1Balance, db1Bal_Pan,
                                             db2Balan_L, db2Balan_R, db2Balance, db2Bal_Pan, db_BalForR, db_Balance,
                                             dbSMTValue, dbSMT_OkNg, db_Reverse, db_Rev_Pan, db1SenSpdL, db1SenSpdR, db2SenSpdL, db2SenSpdR, db_Sen_Spd,
                                             db1ABS_DeL, db1ABS_InL, db1ABS_DeR, db1ABS_InR, db2ABS_DeL, db2ABS_InL, db2ABS_DeR, db2ABS_InR);
                   sql += string.Format("WHERE dbAcceptNo='{0}'", pAcptNo);

            MDB.Execute(sql);
        }

        public void Delete(string pAcptNo)
        {
            string sql = "DELETE FROM tbl_RnB_Data WHERE dbAcceptNo = '" + pAcptNo + "'";

            MDB.Execute(sql);
        }
        #endregion
    }

    public struct tbl_Item
    {
        public MDB_Sql MDB;
        const string str_Item = "dbItemName, dbItem_Idx, dbRnB_Kind, dbSegments, dbRnB_Time, dbRnBSpeed, dbItem_Set, dbVeh__Set, dbRoll_Set, dbDesc_Set ";
        
        #region Item DB Table - Methods
        public string dbItemName { get; set; }  //00.아이템 이름
        public string dbItem_Idx { get; set; }  //01.아이템 순서
        public string dbRnB_Kind { get; set; }  //02.측정 구분(기본, ECU)
        public string dbSegments { get; set; }  //03.구분 명칭
        public string dbRnB_Time { get; set; }  //04.측정 시간(sec)
        public string dbRnBSpeed { get; set; }  //05.측정 속도(km/h)
        public string dbItem_Set { get; set; }  //06.표기 설명
        public string dbVeh__Set { get; set; }  //07.차량 구동 여부
        public string dbRoll_Set { get; set; }  //08.롤러 구동 여부
        public string dbDesc_Set { get; set; }  //09.내용 설명
        #endregion

        #region Item DB Table - Functions
        private void Init()
        {
            dbItemName = "";
            dbItem_Idx = "";
            dbRnB_Kind = "";
            dbSegments = "";
            dbRnB_Time = "";
            dbRnBSpeed = "";
            dbItem_Set = "";
            dbVeh__Set = "";
            dbRoll_Set = "";
            dbDesc_Set = "";
        }

        public DataTable Search(string sort)
        {
            string str_sort = (sort == "All items" ? "" : " WHERE   dbRnB_Kind='" + sort + "' ");
            string sql = "SELECT dbItemName, dbItem_Idx FROM tbl_RnB_Item " + str_sort + " ORDER BY dbItem_Idx ASC";   //DESC
            DataTable dt = MDB.DT_Select(sql);
            return dt;
        }

        public int Select(string pItem)
        {
            string sql = "SELECT * FROM tbl_RnB_Item WHERE dbItemName='" + pItem + "'";
            DataTable dt = MDB.DT_Select(sql);

            Init();

            if (dt.Rows.Count == 1)
            {
                dbItemName = dt.Rows[0]["dbItemName"].ToString();
                dbItem_Idx = dt.Rows[0]["dbItem_Idx"].ToString();
                dbRnB_Kind = dt.Rows[0]["dbRnB_Kind"].ToString();
                dbSegments = dt.Rows[0]["dbSegments"].ToString();
                dbRnB_Time = dt.Rows[0]["dbRnB_Time"].ToString();
                dbRnBSpeed = dt.Rows[0]["dbRnBSpeed"].ToString();
                dbItem_Set = dt.Rows[0]["dbItem_Set"].ToString();
                dbVeh__Set = dt.Rows[0]["dbVeh__Set"].ToString();
                dbRoll_Set = dt.Rows[0]["dbRoll_Set"].ToString();
                dbDesc_Set = dt.Rows[0]["dbDesc_Set"].ToString();
            }

            return dt.Rows.Count;
        }

        public void Insert()
        {
            string sql = "INSERT INTO tbl_RnB_Item ( " + str_Item + " ) values (";
            sql += string.Format(H2Y.Sql_Insert(str_Item) + ") ",
                        dbItemName, dbItem_Idx, dbRnB_Kind, dbSegments, dbRnB_Time, dbRnBSpeed, dbItem_Set, dbVeh__Set, dbRoll_Set, dbDesc_Set);

            MDB.Execute(sql);
        }

        public void Update(string pItem)
        {
            string sql = string.Format("UPDATE tbl_RnB_Item SET " + H2Y.Sql_Update(str_Item),
                        dbItemName, dbItem_Idx, dbRnB_Kind, dbSegments, dbRnB_Time, dbRnBSpeed, dbItem_Set, dbVeh__Set, dbRoll_Set, dbDesc_Set);
            sql += string.Format("WHERE dbItemName='{0}'", pItem);

            MDB.Execute(sql);
        }
        
        public void Delete(string pItem)
        {
            string sql = "DELETE FROM tbl_RnB_Item WHERE dbItemName = '" + pItem + "'";

            MDB.Execute(sql);
        }
        #endregion
    }

    public struct tblCurve
    {
        public MDB_Sql MDB;
        const string strCurve = "dbItemName, dbRnB_Kind, db_Methord, dbRnB_Step, dbRnB_Time, dbRnBSpeed, dbItem_Set, dbVeh__Set, dbRoll_Set, dbDesc_Set ";

        #region Curve DB Table - Methods
        public string dbItemName { get; set; }  //아이템 이름
        public string dbRnB_Kind { get; set; }  //측정 구분
        public string db_Methord { get; set; }  //사용 메소드
        public string dbSegments { get; set; }  //구분 명칭
        public string dbRnB_Step { get; set; }  //측정 측정 순서
        public string dbRnB_Time { get; set; }  //측정 시간(sec)
        public string dbRnBSpeed { get; set; }  //측정 속도(km/h)
        public string dbItem_Set { get; set; }  //표기 설명
        public string dbVeh__Set { get; set; }  //차량 구동 여부
        public string dbRoll_Set { get; set; }  //롤러 구동 여부
        public string dbDesc_Set { get; set; }  //내용 설명
        #endregion

        #region Curve DB Table - Functions
        private void Init()
        {
            dbRnB_Kind = "";
            dbRnB_Step = "";
            db_Methord = "";
            dbSegments = "";
            dbRnB_Time = "";
            dbRnBSpeed = "";
            dbItemName = "";
            dbVeh__Set = "";
            dbRoll_Set = "";
            dbDesc_Set = "";
        }

        public DataTable Search()
        {
            string sql = "SELECT " + strCurve + " FROM tbl_RnBCurve    ORDER BY dbItemName ASC";   //DESC
            DataTable dt = MDB.DT_Select(sql);
            return dt;
        }

        public int Select(string pItem)
        {
            string sql = "SELECT * FROM tbl_RnBCurve WHERE dbItemName='" + pItem + "'";
            DataTable dt = MDB.DT_Select(sql);

            Init();

            if (dt.Rows.Count == 1)
            {
                dbItemName = dt.Rows[0]["dbItemName"].ToString();
                dbRnB_Kind = dt.Rows[0]["dbRnB_Kind"].ToString();
                db_Methord = dt.Rows[0]["db_Methord"].ToString();
                dbSegments = dt.Rows[0]["dbSegments"].ToString();
                dbRnB_Step = dt.Rows[0]["dbRnB_Step"].ToString();
                dbRnB_Time = dt.Rows[0]["dbRnB_Time"].ToString();
                dbRnBSpeed = dt.Rows[0]["dbRnBSpeed"].ToString();
                dbItem_Set = dt.Rows[0]["dbItem_Set"].ToString();
                dbVeh__Set = dt.Rows[0]["dbVeh__Set"].ToString();
                dbRoll_Set = dt.Rows[0]["dbRoll_Set"].ToString();
                dbDesc_Set = dt.Rows[0]["dbDesc_Set"].ToString();
            }

            return dt.Rows.Count;
        }

        public void Insert()
        {
            string sql = "INSERT INTO tbl_RnBCurve ( " + strCurve + " ) values (";
            sql += string.Format(H2Y.Sql_Insert(strCurve) + ") ",
                         dbItemName, dbRnB_Kind, db_Methord, dbRnB_Step, dbRnB_Time, dbRnBSpeed, dbItem_Set, dbVeh__Set, dbRoll_Set, dbDesc_Set);

            MDB.Execute(sql);
        }

        public void Update(string pItem)
        {
            string sql = string.Format("UPDATE tbl_RnBCurve SET " + H2Y.Sql_Update(strCurve),
                                dbItemName, dbRnB_Kind, db_Methord, dbRnB_Step, dbRnB_Time, dbRnBSpeed, dbItem_Set, dbVeh__Set, dbRoll_Set, dbDesc_Set);
            sql += string.Format("WHERE dbItemName='{0}'", pItem);

            MDB.Execute(sql);
        }

        public void Delete(string pItem)
        {
            string sql = "DELETE FROM tbl_RnBCurve WHERE dbItemName = '" + pItem + "'";

            MDB.Execute(sql);
        }
        #endregion
    }

    public struct tbl_ECUs
    {
        public MDB_Sql MDB;
        const string str_ECUs = "dbAcceptNo, dbStt_Comm, dbIden80_1, dbIden80_2, dbIden80_3, dbIden80_4, dbIden80_5, dbIden80_6, dbIden80_7, dbIden80_8, dbIden80_9, dbIden80_A, dbIden80_B, " + 
                                                        "dbIden84_1, dbIden84_2, dbIden84_3, dbIden84_4, dbIden84_5, dbIden84_6, dbIden84_7, dbIden84_8, dbIden84_9, dbIden84_A, dbIden84_B, " + 
                                            "dbProcessB, dbRead_Vin, dbReadTire, dbSAS_Zero, " +
                                            "dbSigPedal, dbSig_Mast, dbSig__ASR, dbSig__SAS, dbSig__AXS, dbSig__YRS, dbSig__Acc, " +
                                            "dbDTC_Read, dbDTCClear, dbSpLmt_On, dbSpLmtOff, dbEnd_Comm ";

        #region Brake DB Table - Methods
        public string dbAcceptNo, dbStt_Comm, dbIden80_1, dbIden80_2, dbIden80_3, dbIden80_4, dbIden80_5, dbIden80_6, dbIden80_7, dbIden80_8, dbIden80_9, dbIden80_A, dbIden80_B,                                               
                                              dbIden84_1, dbIden84_2, dbIden84_3, dbIden84_4, dbIden84_5, dbIden84_6, dbIden84_7, dbIden84_8, dbIden84_9, dbIden84_A, dbIden84_B, 
                                  dbProcessB, dbRead_Vin, dbReadTire, dbSAS_Zero, 
                                  dbSigPedal, dbSig_Mast, dbSig__ASR, dbSig__SAS, dbSig__AXS, dbSig__YRS, dbSig__Acc, 
                                  dbDTC_Read, dbDTCClear, dbSpLmt_On, dbSpLmtOff, dbEnd_Comm ;
        #endregion

        #region Brake DB Table - Functions
        private void Init()
        {
            dbAcceptNo = "";
        }

        public DataTable Search()
        {
            string sql = "SELECT " + str_ECUs + " FROM tblECUs_Data    ORDER BY dbAcceptNo ASC";   //DESC
            DataTable dt = MDB.DT_Select(sql);
            return dt;
        }

        public int Select(string pAcptNo)
        {
            string sql = "SELECT * FROM tblECUs_Data WHERE dbAcceptNo='" + pAcptNo + "'";
            DataTable dt = MDB.DT_Select(sql);

            Init();

            if (dt.Rows.Count == 1)
            {
                dbAcceptNo = dt.Rows[0]["dbAcceptNo"].ToString();

                dbStt_Comm = dt.Rows[0]["dbStt_Comm"].ToString();
                dbIden80_1 = dt.Rows[0]["dbIden80_1"].ToString();
                dbIden80_2 = dt.Rows[0]["dbIden80_2"].ToString();
                dbIden80_3 = dt.Rows[0]["dbIden80_3"].ToString();
                dbIden80_4 = dt.Rows[0]["dbIden80_4"].ToString();
                dbIden80_5 = dt.Rows[0]["dbIden80_5"].ToString();
                dbIden80_6 = dt.Rows[0]["dbIden80_6"].ToString();
                dbIden80_7 = dt.Rows[0]["dbIden80_7"].ToString();
                dbIden80_8 = dt.Rows[0]["dbIden80_8"].ToString();
                dbIden80_9 = dt.Rows[0]["dbIden80_9"].ToString();
                dbIden80_A = dt.Rows[0]["dbIden80_A"].ToString();
                dbIden80_B = dt.Rows[0]["dbIden80_B"].ToString();

                dbIden84_1 = dt.Rows[0]["dbIden84_1"].ToString();
                dbIden84_2 = dt.Rows[0]["dbIden84_2"].ToString();
                dbIden84_3 = dt.Rows[0]["dbIden84_3"].ToString();
                dbIden84_4 = dt.Rows[0]["dbIden84_4"].ToString();
                dbIden84_5 = dt.Rows[0]["dbIden84_5"].ToString();
                dbIden84_6 = dt.Rows[0]["dbIden84_6"].ToString();
                dbIden84_7 = dt.Rows[0]["dbIden84_7"].ToString();
                dbIden84_8 = dt.Rows[0]["dbIden84_8"].ToString();
                dbIden84_9 = dt.Rows[0]["dbIden84_9"].ToString();
                dbIden84_A = dt.Rows[0]["dbIden84_A"].ToString();
                dbIden84_B = dt.Rows[0]["dbIden84_B"].ToString();

                dbProcessB = dt.Rows[0]["dbProcessB"].ToString();
                dbRead_Vin = dt.Rows[0]["dbRead_Vin"].ToString();
                dbReadTire = dt.Rows[0]["dbReadTire"].ToString();
                dbSAS_Zero = dt.Rows[0]["dbSAS_Zero"].ToString();

                dbSigPedal = dt.Rows[0]["dbSigPedal"].ToString();
                dbSig_Mast = dt.Rows[0]["dbSig_Mast"].ToString();
                dbSig__ASR = dt.Rows[0]["dbSig__ASR"].ToString();
                dbSig__SAS = dt.Rows[0]["dbSig__SAS"].ToString();
                dbSig__AXS = dt.Rows[0]["dbSig__AXS"].ToString();
                dbSig__YRS = dt.Rows[0]["dbSig__YRS"].ToString();
                dbSig__Acc = dt.Rows[0]["dbSig__Acc"].ToString();

                dbDTC_Read = dt.Rows[0]["dbDTC_Read"].ToString();
                dbDTCClear = dt.Rows[0]["dbDTCClear"].ToString();
                dbSpLmt_On = dt.Rows[0]["dbSpLmt_On"].ToString();
                dbSpLmtOff = dt.Rows[0]["dbSpLmtOff"].ToString();
                dbEnd_Comm = dt.Rows[0]["dbEnd_Comm"].ToString();
            }

            return dt.Rows.Count;
        }

        public void Insert()
        {
            string sql = "INSERT INTO tblECUs_Data ( " + str_ECUs + " ) values (";
                   sql += string.Format(H2Y.Sql_Insert(str_ECUs) + ") ",
                                dbAcceptNo, dbStt_Comm, dbIden80_1, dbIden80_2, dbIden80_3, dbIden80_4, dbIden80_5, dbIden80_6, dbIden80_7, dbIden80_8, dbIden80_9, dbIden80_A, dbIden80_B,
                                                        dbIden84_1, dbIden84_2, dbIden84_3, dbIden84_4, dbIden84_5, dbIden84_6, dbIden84_7, dbIden84_8, dbIden84_9, dbIden84_A, dbIden84_B,
                                            dbProcessB, dbRead_Vin, dbReadTire, dbSAS_Zero,
                                            dbSigPedal, dbSig_Mast, dbSig__ASR, dbSig__SAS, dbSig__AXS, dbSig__YRS, dbSig__Acc,
                                            dbDTC_Read, dbDTCClear, dbSpLmt_On, dbSpLmtOff, dbEnd_Comm);

            MDB.Execute(sql);
        }

        public void Update(string pAcptNo)
        {
            string sql = string.Format("UPDATE tblECUs_Data SET " + H2Y.Sql_Update(str_ECUs),
                                dbAcceptNo, dbStt_Comm, dbIden80_1, dbIden80_2, dbIden80_3, dbIden80_4, dbIden80_5, dbIden80_6, dbIden80_7, dbIden80_8, dbIden80_9, dbIden80_A, dbIden80_B,
                                                        dbIden84_1, dbIden84_2, dbIden84_3, dbIden84_4, dbIden84_5, dbIden84_6, dbIden84_7, dbIden84_8, dbIden84_9, dbIden84_A, dbIden84_B,
                                            dbProcessB, dbRead_Vin, dbReadTire, dbSAS_Zero,
                                            dbSigPedal, dbSig_Mast, dbSig__ASR, dbSig__SAS, dbSig__AXS, dbSig__YRS, dbSig__Acc,
                                            dbDTC_Read, dbDTCClear, dbSpLmt_On, dbSpLmtOff, dbEnd_Comm);
                   sql += string.Format("WHERE dbAcceptNo='{0}'", pAcptNo);

            MDB.Execute(sql);
        }

        public void Delete(string pAcptNo)
        {
            string sql = "DELETE FROM tblECUs_Data WHERE dbAcceptNo = '" + pAcptNo + "'";

            MDB.Execute(sql);
        }
        #endregion
    }

    public struct tblParam
    {
        public MDB_Sql MDB;
        const string strParam = "dbParamSeq, " +
                                "dbParam001, dbParam002, dbParam003, dbParam004, dbParam005, dbParam006, dbParam007, dbParam008, dbParam009, dbParam010, " +
                                "dbParam011, dbParam012, dbParam013, dbParam014, dbParam015, dbParam016, dbParam017, dbParam018, dbParam019, dbParam020, " +
                                "dbParam021, dbParam022, dbParam023, dbParam024, dbParam025, dbParam026, dbParam027, dbParam028, dbParam029, dbParam030, " +
                                "dbParam031, dbParam032, dbParam033, dbParam034, dbParam035, dbParam036, dbParam037, dbParam038, dbParam039, dbParam040, " +
                                "dbParam041, dbParam042, dbParam043, dbParam044, dbParam045, dbParam046, dbParam047, dbParam048, dbParam049, dbParam050, " +
                                "dbParam051, dbParam052, dbParam053, dbParam054, dbParam055, dbParam056, dbParam057, dbParam058, dbParam059, dbParam060, " +
                                "dbParam061, dbParam062, dbParam063, dbParam064, dbParam065, dbParam066, dbParam067, dbParam068, dbParam069, dbParam070, " +
                                "dbParam071, dbParam072, dbParam073, dbParam074, dbParam075, dbParam076, dbParam077, dbParam078, dbParam079, dbParam080, " +
                                "dbParam081, dbParam082, dbParam083, dbParam084, dbParam085, dbParam086, dbParam087, dbParam088, dbParam089, dbParam090, " +
                                "dbParam091, dbParam092, dbParam093, dbParam094, dbParam095, dbParam096, dbParam097, dbParam098, dbParam099, dbParam100, " +
                                "dbParam101, dbParam102, dbParam103, dbParam104, dbParam105, dbParam106, dbParam107, dbParam108, dbParam109, dbParam110, " +
                                "dbParam111, dbParam112, dbParam113, dbParam114, dbParam115, dbParam116, dbParam117, dbParam118, dbParam119, dbParam120 ";

        #region Model DB Table - Methods
        public string dbParamSeq { get; set; }  //Parameter Sequnce
        public string dbParam001 { get; set; }  //속도계 min
        public string dbParam002 { get; set; }  //속도계 max
        public string dbParam003 { get; set; }  //SST    min
        public string dbParam004 { get; set; }  //SST    max
        public string dbParam005 { get; set; }  //
        public string dbParam006 { get; set; }  //
        public string dbParam007 { get; set; }  //
        public string dbParam008 { get; set; }  //
        public string dbParam009 { get; set; }  //Pedal Brake Target Force(kg)
        public string dbParam010 { get; set; }  //Pedal Brake Max    Graph(kg)

        public string dbParam011 { get; set; }  //전축 끌림 min
        public string dbParam012 { get; set; }  //전축 끌림 max
        public string dbParam013 { get; set; }  //후축 끌림 min
        public string dbParam014 { get; set; }  //후축 끌림 max
        public string dbParam015 { get; set; }  //
        public string dbParam016 { get; set; }  //
        public string dbParam017 { get; set; }  //
        public string dbParam018 { get; set; }  //
        public string dbParam019 { get; set; }  //
        public string dbParam020 { get; set; }  //

        public string dbParam021 { get; set; }  //전축 제동력 min
        public string dbParam022 { get; set; }  //전축 제동력 max
        public string dbParam023 { get; set; }  //후축 제동력 min
        public string dbParam024 { get; set; }  //후축 제동력 max
        public string dbParam025 { get; set; }  //
        public string dbParam026 { get; set; }  //
        public string dbParam027 { get; set; }  //
        public string dbParam028 { get; set; }  //
        public string dbParam029 { get; set; }  //주차 제동력 min
        public string dbParam030 { get; set; }  //주차 제동력 max

        public string dbParam031 { get; set; }  //전축 발란스 min
        public string dbParam032 { get; set; }  //전축 발란스 max
        public string dbParam033 { get; set; }  //후축 발란스 min
        public string dbParam034 { get; set; }  //후축 발란스 max
        public string dbParam035 { get; set; }  //전체 발란스 min
        public string dbParam036 { get; set; }  //전체 발란스 max
        public string dbParam037 { get; set; }  //
        public string dbParam038 { get; set; }  //
        public string dbParam039 { get; set; }  //
        public string dbParam040 { get; set; }  //

        public string dbParam041 { get; set; }  //
        public string dbParam042 { get; set; }  //
        public string dbParam043 { get; set; }  //
        public string dbParam044 { get; set; }  //
        public string dbParam045 { get; set; }  //
        public string dbParam046 { get; set; }  //
        public string dbParam047 { get; set; }  //
        public string dbParam048 { get; set; }  //
        public string dbParam049 { get; set; }  //
        public string dbParam050 { get; set; }  //

        public string dbParam051 { get; set; }  //WSS F-L Min
        public string dbParam052 { get; set; }  //WSS F-L Max
        public string dbParam053 { get; set; }  //WSS F-R Min
        public string dbParam054 { get; set; }  //WSS F-R Max
        public string dbParam055 { get; set; }  //WSS R-L Min
        public string dbParam056 { get; set; }  //WSS R-L Max
        public string dbParam057 { get; set; }  //WSS R-R Min
        public string dbParam058 { get; set; }  //WSS R-R Max
        public string dbParam059 { get; set; }  //
        public string dbParam060 { get; set; }  //

        public string dbParam061 { get; set; }  //전축 감소(Dec) min
        public string dbParam062 { get; set; }  //전축 감소(Dec) max
        public string dbParam063 { get; set; }  //전축 증가(Inc) min
        public string dbParam064 { get; set; }  //전축 증가(Inc) max
        public string dbParam065 { get; set; }  //후축 감소(Dec) min
        public string dbParam066 { get; set; }  //후축 감소(Dec) max
        public string dbParam067 { get; set; }  //후축 증가(Inc) min
        public string dbParam068 { get; set; }  //후축 증가(Inc) max
        public string dbParam069 { get; set; }  //
        public string dbParam070 { get; set; }  //

        public string dbParam071 { get; set; }  //전축 최소 축중(kg)
        public string dbParam072 { get; set; }  //전축 최대 축중(kg)
        public string dbParam073 { get; set; }  //
        public string dbParam074 { get; set; }  //후축 최소 축중(kg)
        public string dbParam075 { get; set; }  //후축 최대 축중(kg)
        public string dbParam076 { get; set; }  //
        public string dbParam077 { get; set; }  //
        public string dbParam078 { get; set; }  //축중 측정 시간(sec)
        public string dbParam079 { get; set; }  //
        public string dbParam080 { get; set; }  //

        public string dbParam081 { get; set; }  //전축 제동력(%)
        public string dbParam082 { get; set; }  //후축 제동력(%)
        public string dbParam083 { get; set; }  //끌림 제동력(%)
        public string dbParam084 { get; set; }  //편차 제동력(%)
        public string dbParam085 { get; set; }  //  합 제동력(%)
        public string dbParam086 { get; set; }  //주차 제동력(%)
        public string dbParam087 { get; set; }  //
        public string dbParam088 { get; set; }  //일반 제동력 측정 시간(sec)
        public string dbParam089 { get; set; }  //끌림 제동력 측정 시간(sec)
        public string dbParam090 { get; set; }  //주차 제동력 측정 시간(sec)

        public string dbParam091 { get; set; }  //
        public string dbParam092 { get; set; }  //
        public string dbParam093 { get; set; }  //
        public string dbParam094 { get; set; }  //
        public string dbParam095 { get; set; }  //
        public string dbParam096 { get; set; }  //
        public string dbParam097 { get; set; }  //
        public string dbParam098 { get; set; }  //
        public string dbParam099 { get; set; }  //
        public string dbParam100 { get; set; }  //

        public string dbParam101 { get; set; }  //
        public string dbParam102 { get; set; }  //
        public string dbParam103 { get; set; }  //
        public string dbParam104 { get; set; }  //
        public string dbParam105 { get; set; }  //
        public string dbParam106 { get; set; }  //
        public string dbParam107 { get; set; }  //
        public string dbParam108 { get; set; }  //
        public string dbParam109 { get; set; }  //
        public string dbParam110 { get; set; }  //

        public string dbParam111 { get; set; }  //
        public string dbParam112 { get; set; }  //
        public string dbParam113 { get; set; }  //
        public string dbParam114 { get; set; }  //
        public string dbParam115 { get; set; }  //
        public string dbParam116 { get; set; }  //
        public string dbParam117 { get; set; }  //
        public string dbParam118 { get; set; }  //
        public string dbParam119 { get; set; }  //
        public string dbParam120 { get; set; }  //
        #endregion

        #region Model DB Table - Functions
        private void Init()
        {
            dbParamSeq = ""; 
            dbParam001 = ""; dbParam002 = ""; dbParam003 = ""; dbParam004 = ""; dbParam005 = ""; dbParam006 = ""; dbParam007 = ""; dbParam008 = ""; dbParam009 = ""; dbParam010 = "";
            dbParam011 = ""; dbParam012 = ""; dbParam013 = ""; dbParam014 = ""; dbParam015 = ""; dbParam016 = ""; dbParam017 = ""; dbParam018 = ""; dbParam019 = ""; dbParam020 = "";
            dbParam021 = ""; dbParam022 = ""; dbParam023 = ""; dbParam024 = ""; dbParam025 = ""; dbParam026 = ""; dbParam027 = ""; dbParam028 = ""; dbParam029 = ""; dbParam030 = "";
            dbParam031 = ""; dbParam032 = ""; dbParam033 = ""; dbParam034 = ""; dbParam035 = ""; dbParam036 = ""; dbParam037 = ""; dbParam038 = ""; dbParam039 = ""; dbParam040 = "";
            dbParam041 = ""; dbParam042 = ""; dbParam043 = ""; dbParam044 = ""; dbParam045 = ""; dbParam046 = ""; dbParam047 = ""; dbParam048 = ""; dbParam049 = ""; dbParam050 = "";
            dbParam051 = ""; dbParam052 = ""; dbParam053 = ""; dbParam054 = ""; dbParam055 = ""; dbParam056 = ""; dbParam057 = ""; dbParam058 = ""; dbParam059 = ""; dbParam060 = "";
            dbParam061 = ""; dbParam062 = ""; dbParam063 = ""; dbParam064 = ""; dbParam065 = ""; dbParam066 = ""; dbParam067 = ""; dbParam068 = ""; dbParam069 = ""; dbParam070 = "";
            dbParam071 = ""; dbParam072 = ""; dbParam073 = ""; dbParam074 = ""; dbParam075 = ""; dbParam076 = ""; dbParam077 = ""; dbParam078 = ""; dbParam079 = ""; dbParam080 = "";
            dbParam081 = ""; dbParam082 = ""; dbParam083 = ""; dbParam084 = ""; dbParam085 = ""; dbParam086 = ""; dbParam087 = ""; dbParam088 = ""; dbParam089 = ""; dbParam090 = "";
            dbParam091 = ""; dbParam092 = ""; dbParam093 = ""; dbParam094 = ""; dbParam095 = ""; dbParam096 = ""; dbParam097 = ""; dbParam098 = ""; dbParam099 = ""; dbParam100 = "";
            dbParam101 = ""; dbParam102 = ""; dbParam103 = ""; dbParam104 = ""; dbParam105 = ""; dbParam106 = ""; dbParam107 = ""; dbParam108 = ""; dbParam109 = ""; dbParam110 = "";
            dbParam111 = ""; dbParam112 = ""; dbParam113 = ""; dbParam114 = ""; dbParam115 = ""; dbParam116 = ""; dbParam117 = ""; dbParam118 = ""; dbParam119 = ""; dbParam120 = ""; 
        }

        public DataTable Search()
        {
            string sql = "SELECT * FROM tblParameter";   //DESC
            DataTable dt = MDB.DT_Select(sql);

            return dt;
        }

        public int Select(string dbParamSeq)
        {
            string sql = "SELECT * FROM tblParameter WHERE dbParamSeq='" + dbParamSeq + "'";
            DataTable dt = MDB.DT_Select(sql);

            Init();

            if (dt.Rows.Count == 1)
            {
                dbParamSeq = dt.Rows[0]["dbParamSeq"].ToString();
                dbParam001 = dt.Rows[0]["dbParam001"].ToString();
                dbParam002 = dt.Rows[0]["dbParam002"].ToString();
                dbParam003 = dt.Rows[0]["dbParam003"].ToString();
                dbParam004 = dt.Rows[0]["dbParam004"].ToString();
                dbParam005 = dt.Rows[0]["dbParam005"].ToString();
                dbParam006 = dt.Rows[0]["dbParam006"].ToString();
                dbParam007 = dt.Rows[0]["dbParam007"].ToString();
                dbParam008 = dt.Rows[0]["dbParam008"].ToString();
                dbParam009 = dt.Rows[0]["dbParam009"].ToString();
                dbParam010 = dt.Rows[0]["dbParam010"].ToString();

                dbParam011 = dt.Rows[0]["dbParam011"].ToString();
                dbParam012 = dt.Rows[0]["dbParam012"].ToString();
                dbParam013 = dt.Rows[0]["dbParam013"].ToString();
                dbParam014 = dt.Rows[0]["dbParam014"].ToString();
                dbParam015 = dt.Rows[0]["dbParam015"].ToString();
                dbParam016 = dt.Rows[0]["dbParam016"].ToString();
                dbParam017 = dt.Rows[0]["dbParam017"].ToString();
                dbParam018 = dt.Rows[0]["dbParam018"].ToString();
                dbParam019 = dt.Rows[0]["dbParam019"].ToString();
                dbParam020 = dt.Rows[0]["dbParam020"].ToString();

                dbParam021 = dt.Rows[0]["dbParam021"].ToString();
                dbParam022 = dt.Rows[0]["dbParam022"].ToString();
                dbParam023 = dt.Rows[0]["dbParam023"].ToString();
                dbParam024 = dt.Rows[0]["dbParam024"].ToString();
                dbParam025 = dt.Rows[0]["dbParam025"].ToString();
                dbParam026 = dt.Rows[0]["dbParam026"].ToString();
                dbParam027 = dt.Rows[0]["dbParam027"].ToString();
                dbParam028 = dt.Rows[0]["dbParam028"].ToString();
                dbParam029 = dt.Rows[0]["dbParam029"].ToString();
                dbParam030 = dt.Rows[0]["dbParam030"].ToString();

                dbParam031 = dt.Rows[0]["dbParam031"].ToString();
                dbParam032 = dt.Rows[0]["dbParam032"].ToString();
                dbParam033 = dt.Rows[0]["dbParam033"].ToString();
                dbParam034 = dt.Rows[0]["dbParam034"].ToString();
                dbParam035 = dt.Rows[0]["dbParam035"].ToString();
                dbParam036 = dt.Rows[0]["dbParam036"].ToString();
                dbParam037 = dt.Rows[0]["dbParam037"].ToString();
                dbParam038 = dt.Rows[0]["dbParam038"].ToString();
                dbParam039 = dt.Rows[0]["dbParam039"].ToString();
                dbParam040 = dt.Rows[0]["dbParam040"].ToString();

                dbParam041 = dt.Rows[0]["dbParam041"].ToString();
                dbParam042 = dt.Rows[0]["dbParam042"].ToString();
                dbParam043 = dt.Rows[0]["dbParam043"].ToString();
                dbParam044 = dt.Rows[0]["dbParam044"].ToString();
                dbParam045 = dt.Rows[0]["dbParam045"].ToString();
                dbParam046 = dt.Rows[0]["dbParam046"].ToString();
                dbParam047 = dt.Rows[0]["dbParam047"].ToString();
                dbParam048 = dt.Rows[0]["dbParam048"].ToString();
                dbParam049 = dt.Rows[0]["dbParam049"].ToString();
                dbParam050 = dt.Rows[0]["dbParam050"].ToString();

                dbParam051 = dt.Rows[0]["dbParam051"].ToString();
                dbParam052 = dt.Rows[0]["dbParam052"].ToString();
                dbParam053 = dt.Rows[0]["dbParam053"].ToString();
                dbParam054 = dt.Rows[0]["dbParam054"].ToString();
                dbParam055 = dt.Rows[0]["dbParam055"].ToString();
                dbParam056 = dt.Rows[0]["dbParam056"].ToString();
                dbParam057 = dt.Rows[0]["dbParam057"].ToString();
                dbParam058 = dt.Rows[0]["dbParam058"].ToString();
                dbParam059 = dt.Rows[0]["dbParam059"].ToString();
                dbParam060 = dt.Rows[0]["dbParam060"].ToString();

                dbParam061 = dt.Rows[0]["dbParam061"].ToString();
                dbParam062 = dt.Rows[0]["dbParam062"].ToString();
                dbParam063 = dt.Rows[0]["dbParam063"].ToString();
                dbParam064 = dt.Rows[0]["dbParam064"].ToString();
                dbParam065 = dt.Rows[0]["dbParam065"].ToString();
                dbParam066 = dt.Rows[0]["dbParam066"].ToString();
                dbParam067 = dt.Rows[0]["dbParam067"].ToString();
                dbParam068 = dt.Rows[0]["dbParam068"].ToString();
                dbParam069 = dt.Rows[0]["dbParam069"].ToString();
                dbParam070 = dt.Rows[0]["dbParam070"].ToString();

                dbParam071 = dt.Rows[0]["dbParam071"].ToString();
                dbParam072 = dt.Rows[0]["dbParam072"].ToString();
                dbParam073 = dt.Rows[0]["dbParam073"].ToString();
                dbParam074 = dt.Rows[0]["dbParam074"].ToString();
                dbParam075 = dt.Rows[0]["dbParam075"].ToString();
                dbParam076 = dt.Rows[0]["dbParam076"].ToString();
                dbParam077 = dt.Rows[0]["dbParam077"].ToString();
                dbParam078 = dt.Rows[0]["dbParam078"].ToString();
                dbParam079 = dt.Rows[0]["dbParam079"].ToString();
                dbParam080 = dt.Rows[0]["dbParam080"].ToString();

                dbParam081 = dt.Rows[0]["dbParam081"].ToString();
                dbParam082 = dt.Rows[0]["dbParam082"].ToString();
                dbParam083 = dt.Rows[0]["dbParam083"].ToString();
                dbParam084 = dt.Rows[0]["dbParam084"].ToString();
                dbParam085 = dt.Rows[0]["dbParam085"].ToString();
                dbParam086 = dt.Rows[0]["dbParam086"].ToString();
                dbParam087 = dt.Rows[0]["dbParam087"].ToString();
                dbParam088 = dt.Rows[0]["dbParam088"].ToString();
                dbParam089 = dt.Rows[0]["dbParam089"].ToString();
                dbParam090 = dt.Rows[0]["dbParam090"].ToString();

                dbParam091 = dt.Rows[0]["dbParam091"].ToString();
                dbParam092 = dt.Rows[0]["dbParam092"].ToString();
                dbParam093 = dt.Rows[0]["dbParam093"].ToString();
                dbParam094 = dt.Rows[0]["dbParam094"].ToString();
                dbParam095 = dt.Rows[0]["dbParam095"].ToString();
                dbParam096 = dt.Rows[0]["dbParam096"].ToString();
                dbParam097 = dt.Rows[0]["dbParam097"].ToString();
                dbParam098 = dt.Rows[0]["dbParam098"].ToString();
                dbParam099 = dt.Rows[0]["dbParam099"].ToString();
                dbParam100 = dt.Rows[0]["dbParam100"].ToString();

                dbParam101 = dt.Rows[0]["dbParam101"].ToString();
                dbParam102 = dt.Rows[0]["dbParam102"].ToString();
                dbParam103 = dt.Rows[0]["dbParam103"].ToString();
                dbParam104 = dt.Rows[0]["dbParam104"].ToString();
                dbParam105 = dt.Rows[0]["dbParam105"].ToString();
                dbParam106 = dt.Rows[0]["dbParam106"].ToString();
                dbParam107 = dt.Rows[0]["dbParam107"].ToString();
                dbParam108 = dt.Rows[0]["dbParam108"].ToString();
                dbParam109 = dt.Rows[0]["dbParam109"].ToString();
                dbParam110 = dt.Rows[0]["dbParam110"].ToString();

                dbParam111 = dt.Rows[0]["dbParam111"].ToString();
                dbParam112 = dt.Rows[0]["dbParam112"].ToString();
                dbParam113 = dt.Rows[0]["dbParam113"].ToString();
                dbParam114 = dt.Rows[0]["dbParam114"].ToString();
                dbParam115 = dt.Rows[0]["dbParam115"].ToString();
                dbParam116 = dt.Rows[0]["dbParam116"].ToString();
                dbParam117 = dt.Rows[0]["dbParam117"].ToString();
                dbParam118 = dt.Rows[0]["dbParam118"].ToString();
                dbParam119 = dt.Rows[0]["dbParam119"].ToString();
                dbParam120 = dt.Rows[0]["dbParam120"].ToString();
            }

            return dt.Rows.Count;
        }

        public void Insert()
        {
            string sql = "INSERT INTO tblParameter ( " + strParam + " ) values (";
                   sql += string.Format(H2Y.Sql_Insert(strParam) + ") ",
                                dbParamSeq,
                                dbParam001, dbParam002, dbParam003, dbParam004, dbParam005, dbParam006, dbParam007, dbParam008, dbParam009, dbParam010,
                                dbParam011, dbParam012, dbParam013, dbParam014, dbParam015, dbParam016, dbParam017, dbParam018, dbParam019, dbParam020,
                                dbParam021, dbParam022, dbParam023, dbParam024, dbParam025, dbParam026, dbParam027, dbParam028, dbParam029, dbParam030,
                                dbParam031, dbParam032, dbParam033, dbParam034, dbParam035, dbParam036, dbParam037, dbParam038, dbParam039, dbParam040,
                                dbParam041, dbParam042, dbParam043, dbParam044, dbParam045, dbParam046, dbParam047, dbParam048, dbParam049, dbParam050,
                                dbParam051, dbParam052, dbParam053, dbParam054, dbParam055, dbParam056, dbParam057, dbParam058, dbParam059, dbParam060,
                                dbParam061, dbParam062, dbParam063, dbParam064, dbParam065, dbParam066, dbParam067, dbParam068, dbParam069, dbParam070,
                                dbParam071, dbParam072, dbParam073, dbParam074, dbParam075, dbParam076, dbParam077, dbParam078, dbParam079, dbParam080,
                                dbParam081, dbParam082, dbParam083, dbParam084, dbParam085, dbParam086, dbParam087, dbParam088, dbParam089, dbParam090,
                                dbParam091, dbParam092, dbParam093, dbParam094, dbParam095, dbParam096, dbParam097, dbParam098, dbParam099, dbParam100,
                                dbParam101, dbParam102, dbParam103, dbParam104, dbParam105, dbParam106, dbParam107, dbParam108, dbParam109, dbParam110,
                                dbParam111, dbParam112, dbParam113, dbParam114, dbParam115, dbParam116, dbParam117, dbParam118, dbParam119, dbParam120);
            
            MDB.Execute(sql);
        }

        public void Update(string pParam)
        {
            string sql = string.Format("UPDATE tblParameter SET " + H2Y.Sql_Update(strParam),
                                dbParamSeq,
                                dbParam001, dbParam002, dbParam003, dbParam004, dbParam005, dbParam006, dbParam007, dbParam008, dbParam009, dbParam010,
                                dbParam011, dbParam012, dbParam013, dbParam014, dbParam015, dbParam016, dbParam017, dbParam018, dbParam019, dbParam020,
                                dbParam021, dbParam022, dbParam023, dbParam024, dbParam025, dbParam026, dbParam027, dbParam028, dbParam029, dbParam030,
                                dbParam031, dbParam032, dbParam033, dbParam034, dbParam035, dbParam036, dbParam037, dbParam038, dbParam039, dbParam040,
                                dbParam041, dbParam042, dbParam043, dbParam044, dbParam045, dbParam046, dbParam047, dbParam048, dbParam049, dbParam050,
                                dbParam051, dbParam052, dbParam053, dbParam054, dbParam055, dbParam056, dbParam057, dbParam058, dbParam059, dbParam060,
                                dbParam061, dbParam062, dbParam063, dbParam064, dbParam065, dbParam066, dbParam067, dbParam068, dbParam069, dbParam070,
                                dbParam071, dbParam072, dbParam073, dbParam074, dbParam075, dbParam076, dbParam077, dbParam078, dbParam079, dbParam080,
                                dbParam081, dbParam082, dbParam083, dbParam084, dbParam085, dbParam086, dbParam087, dbParam088, dbParam089, dbParam090,
                                dbParam091, dbParam092, dbParam093, dbParam094, dbParam095, dbParam096, dbParam097, dbParam098, dbParam099, dbParam100,
                                dbParam101, dbParam102, dbParam103, dbParam104, dbParam105, dbParam106, dbParam107, dbParam108, dbParam109, dbParam110,
                                dbParam111, dbParam112, dbParam113, dbParam114, dbParam115, dbParam116, dbParam117, dbParam118, dbParam119, dbParam120);
                   sql += string.Format("WHERE dbParamSeq='{0}'", pParam);

            MDB.Execute(sql);
        }

        public void Delete(string pParam)
        {
            string sql = "DELETE FROM tblParameter WHERE dbParamSeq = '" + pParam + "'";

            MDB.Execute(sql);
        }
        #endregion
    }

    public struct tblBrake
    {
        public MDB_Sql MDB;
        const string strBrake = "dbAcceptNo, db1_Weight, db1_Wgt__L, db1_Wgt__R, db1Drag__L, db1Drag__R, db1Drag__V, db1Drag_OX, db1Brake_L, db1Brake_R, db1Diff__V, db1Diff_OX, db1Sum___V, db1Sum__OX, db1BrakeOX, db1Park__L, db1Park__R, " +
                                            "db2_Weight, db2_Wgt__L, db2_Wgt__R, db2Drag__L, db2Drag__R, db2Drag__V, db2Drag_OX, db2Brake_L, db2Brake_R, db2Diff__V, db2Diff_OX, db2Sum___V, db2Sum__OX, db2BrakeOX, db2Park__L, db2Park__R, " +
                                            "dbT_Weight, dbTBrake_L, dbTBrake_R, dbTBrake_V, dbTBrakeOX, " +
                                                        "dbAPark__L, dbAPark__R, dbAPark__V, dbAPark_OX, dbBrake_OX ";  

        #region Brake DB Table - Methods
        public string dbAcceptNo { get; set; }  //000A,2011109,00001

        public double db1_Weight { get; set; }  //
        public double db1_Wgt__L { get; set; }  //
        public double db1_Wgt__R { get; set; }  //
        public double db1Drag__L { get; set; }  //
        public double db1Drag__R { get; set; }  //
        public double db1Drag__V { get; set; }  //
        public string db1Drag_OX { get; set; }  //
        public double db1Brake_L { get; set; }  //
        public double db1Brake_R { get; set; }  //
        public double db1Diff__V { get; set; }  //
        public string db1Diff_OX { get; set; }  //
        public double db1Sum___V { get; set; }  //
        public string db1Sum__OX { get; set; }  //
        public string db1BrakeOX { get; set; }  //
        public double db1Park__L { get; set; }  //
        public double db1Park__R { get; set; }  //

        public double db2_Weight { get; set; }  //
        public double db2_Wgt__L { get; set; }  //
        public double db2_Wgt__R { get; set; }  //
        public double db2Drag__L { get; set; }  //
        public double db2Drag__R { get; set; }  //
        public double db2Drag__V { get; set; }  //
        public string db2Drag_OX { get; set; }  //
        public double db2Brake_L { get; set; }  //
        public double db2Brake_R { get; set; }  //
        public double db2Diff__V { get; set; }  //
        public string db2Diff_OX { get; set; }  //
        public double db2Sum___V { get; set; }  //
        public string db2Sum__OX { get; set; }  //
        public string db2BrakeOX { get; set; }  //
        public double db2Park__L { get; set; }  //
        public double db2Park__R { get; set; }  //

        public double dbT_Weight { get; set; }  //
        public double dbTBrake_L { get; set; }  //
        public double dbTBrake_R { get; set; }  //
        public double dbTBrake_V { get; set; }  //
        public string dbTBrakeOX { get; set; }  //

        public double dbAPark__L { get; set; }  //
        public double dbAPark__R { get; set; }  //
        public double dbAPark__V { get; set; }  //
        public string dbAPark_OX { get; set; }  //

        public string dbBrake_OX { get; set; }  //
        #endregion

        #region Brake DB Table - Functions
        private void Init()
        {
            dbAcceptNo = "";
            db1_Weight = -1; db2_Weight = -1;
            db1_Wgt__L = -1; db2_Wgt__L = -1;
            db1_Wgt__R = -1; db2_Wgt__R = -1;
            db1Drag__L = -1; db2Drag__L = -1;
            db1Drag__R = -1; db2Drag__R = -1;
            db1Drag__V = -1; db2Drag__V = -1;
            db1Drag_OX = ""; db2Drag_OX = "";
            db1Brake_L = -1; db2Brake_L = -1;
            db1Brake_R = -1; db2Brake_R = -1;
            db1Diff__V = -1; db2Diff__V = -1;
            db1Diff_OX = ""; db2Diff_OX = "";
            db1Sum___V = -1; db2Sum___V = -1;
            db1Sum__OX = ""; db2Sum__OX = "";
            db1BrakeOX = ""; db2BrakeOX = "";
            db1Park__L = -1; db2Park__L = -1;
            db1Park__R = -1; db2Park__R = -1;

            dbT_Weight = -1; dbTBrake_L = -1; dbTBrake_R = -1; dbTBrake_V = -1; dbTBrakeOX = "";
            dbAPark__L = -1; dbAPark__R = -1; dbAPark__V = -1; dbAPark_OX = ""; dbBrake_OX = "";
        }

        public DataTable Search()
        {
            string sql = "SELECT " + strBrake + " FROM tbl_ABS_Data    ORDER BY dbAcceptNo ASC";   //DESC
            DataTable dt = MDB.DT_Select(sql);
            return dt;
        }

        public int Select(string pAcptNo)
        {
            string sql = "SELECT * FROM tbl_ABS_Data WHERE dbAcceptNo='" + pAcptNo + "'";
            DataTable dt = MDB.DT_Select(sql);

            Init();

            if (dt.Rows.Count == 1)
            {
                dbAcceptNo = dt.Rows[0]["dbAcceptNo"].ToString();
    
                db1_Weight = H2Y.StrToSingles(dt.Rows[0]["db1_Weight"].ToString());
                db1_Wgt__L = H2Y.StrToSingles(dt.Rows[0]["db1_Wgt__L"].ToString());
                db1_Wgt__R = H2Y.StrToSingles(dt.Rows[0]["db1_Wgt__R"].ToString());
                db1Drag__L = H2Y.StrToSingles(dt.Rows[0]["db1Drag__L"].ToString());
                db1Drag__R = H2Y.StrToSingles(dt.Rows[0]["db1Drag__R"].ToString());
                db1Drag__V = H2Y.StrToSingles(dt.Rows[0]["db1Drag__V"].ToString());
                db1Drag_OX = dt.Rows[0]["db1Drag_OX"].ToString();
                db1Brake_L = H2Y.StrToSingles(dt.Rows[0]["db1Brake_L"].ToString());
                db1Brake_R = H2Y.StrToSingles(dt.Rows[0]["db1Brake_R"].ToString());
                db1Diff__V = H2Y.StrToSingles(dt.Rows[0]["db1Diff__V"].ToString());
                db1Diff_OX = dt.Rows[0]["db1Diff_OX"].ToString();
                db1Sum___V = H2Y.StrToSingles(dt.Rows[0]["db1Sum___V"].ToString());
                db1Sum__OX = dt.Rows[0]["db1Sum__OX"].ToString();
                db1BrakeOX = dt.Rows[0]["db1BrakeOX"].ToString();
                db1Park__L = H2Y.StrToSingles(dt.Rows[0]["db1Park__L"].ToString());
                db1Park__R = H2Y.StrToSingles(dt.Rows[0]["db1Park__R"].ToString());

                db2_Weight = H2Y.StrToSingles(dt.Rows[0]["db2_Weight"].ToString());
                db2_Wgt__L = H2Y.StrToSingles(dt.Rows[0]["db2_Wgt__L"].ToString());
                db2_Wgt__R = H2Y.StrToSingles(dt.Rows[0]["db2_Wgt__R"].ToString());
                db2Drag__L = H2Y.StrToSingles(dt.Rows[0]["db2Drag__L"].ToString());
                db2Drag__R = H2Y.StrToSingles(dt.Rows[0]["db2Drag__R"].ToString());
                db2Drag__V = H2Y.StrToSingles(dt.Rows[0]["db2Drag__V"].ToString());
                db2Drag_OX = dt.Rows[0]["db2Drag_OX"].ToString();
                db2Brake_L = H2Y.StrToSingles(dt.Rows[0]["db2Brake_L"].ToString());
                db2Brake_R = H2Y.StrToSingles(dt.Rows[0]["db2Brake_R"].ToString());
                db2Diff__V = H2Y.StrToSingles(dt.Rows[0]["db2Diff__V"].ToString());
                db2Diff_OX = dt.Rows[0]["db2Diff_OX"].ToString();
                db2Sum___V = H2Y.StrToSingles(dt.Rows[0]["db2Sum___V"].ToString());
                db2Sum__OX = dt.Rows[0]["db2Sum__OX"].ToString();
                db2BrakeOX = dt.Rows[0]["db2BrakeOX"].ToString();
                db2Park__L = H2Y.StrToSingles(dt.Rows[0]["db2Park__L"].ToString());
                db2Park__R = H2Y.StrToSingles(dt.Rows[0]["db2Park__R"].ToString());

                dbT_Weight = H2Y.StrToSingles(dt.Rows[0]["dbT_Weight"].ToString());
                dbTBrake_L = H2Y.StrToSingles(dt.Rows[0]["dbTBrake_L"].ToString());
                dbTBrake_R = H2Y.StrToSingles(dt.Rows[0]["dbTBrake_R"].ToString());
                dbTBrake_V = H2Y.StrToSingles(dt.Rows[0]["dbTBrake_V"].ToString());
                dbTBrakeOX = dt.Rows[0]["dbTBrakeOX"].ToString();

                dbAPark__L = H2Y.StrToSingles(dt.Rows[0]["dbAPark__L"].ToString());
                dbAPark__R = H2Y.StrToSingles(dt.Rows[0]["dbAPark__R"].ToString());
                dbAPark__V = H2Y.StrToSingles(dt.Rows[0]["dbAPark__V"].ToString());
                dbAPark_OX = dt.Rows[0]["dbAPark_OX"].ToString();
                dbBrake_OX = dt.Rows[0]["dbBrake_OX"].ToString();
            }

            return dt.Rows.Count;
        }

        public void Insert()
        {
            string sql = "INSERT INTO tbl_ABS_Data ( " + strBrake + " ) values (";
            sql += string.Format(H2Y.Sql_Insert(strBrake) + ") ",
                          dbAcceptNo, db1_Weight, db1_Wgt__L, db1_Wgt__R, db1Drag__L, db1Drag__R, db1Drag__V, db1Drag_OX, db1Brake_L, db1Brake_R, db1Diff__V, db1Diff_OX, db1Sum___V, db1Sum__OX, db1BrakeOX, db1Park__L, db1Park__R,
                                      db2_Weight, db2_Wgt__L, db2_Wgt__R, db2Drag__L, db2Drag__R, db2Drag__V, db2Drag_OX, db2Brake_L, db2Brake_R, db2Diff__V, db2Diff_OX, db2Sum___V, db2Sum__OX, db2BrakeOX, db2Park__L, db2Park__R,
                                      dbT_Weight, dbTBrake_L, dbTBrake_R, dbTBrake_V, dbTBrakeOX,
                                                  dbAPark__L, dbAPark__R, dbAPark__V, dbAPark_OX, dbBrake_OX);

            MDB.Execute(sql);
        }

        public void Update(string pAcptNo)
        {
            string sql = string.Format("UPDATE tbl_ABS_Data SET " + H2Y.Sql_Update(strBrake),
                          dbAcceptNo, db1_Weight, db1_Wgt__L, db1_Wgt__R, db1Drag__L, db1Drag__R, db1Drag__V, db1Drag_OX, db1Brake_L, db1Brake_R, db1Diff__V, db1Diff_OX, db1Sum___V, db1Sum__OX, db1BrakeOX, db1Park__L, db1Park__R,
                                      db2_Weight, db2_Wgt__L, db2_Wgt__R, db2Drag__L, db2Drag__R, db2Drag__V, db2Drag_OX, db2Brake_L, db2Brake_R, db2Diff__V, db2Diff_OX, db2Sum___V, db2Sum__OX, db2BrakeOX, db2Park__L, db2Park__R,
                                      dbT_Weight, dbTBrake_L, dbTBrake_R, dbTBrake_V, dbTBrakeOX,
                                                  dbAPark__L, dbAPark__R, dbAPark__V, dbAPark_OX, dbBrake_OX);
            sql += string.Format("WHERE dbAcceptNo='{0}'", pAcptNo);

            MDB.Execute(sql);
        }

        public void Delete(string pAcptNo)
        {
            string sql = "DELETE FROM tbl_ABS_Data WHERE dbAcceptNo = '" + pAcptNo + "'";

            MDB.Execute(sql);
        }
        #endregion
    }
}
