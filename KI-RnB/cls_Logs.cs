using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KI_RnB
{
    [Flags]
    public enum Log_His { Info = 1, Test, Step, Base, Butn, Data, Gaph, WSS_, Sped, Drag, Brak, ABSB, Park, Stop, TEnd, Msg_, Err_, DBEr, LogE, Save, Del_, ImgE, Back }

    [Flags]
    public enum Log_PLC { PLCG = 20, PLCP, PLCE }

    [Flags]
    public enum Log_ECU { IDSN = 0, Test, Puts, Gets, TEnd }

    public static class Logs
    {
        private static string old_Logs = "";
        private static string old_ECUs = "";
        private static string old_His_ = "";

        public static void ExceptionErr(Exception ex)
        {
            try
            {
                Logs.MakeLog_File(Log_His.ImgE, "Data : " + ex.Data.ToString());
                Logs.MakeLog_File(Log_His.ImgE, "HelpLink : " + ex.HelpLink);
                Logs.MakeLog_File(Log_His.ImgE, "InnerException : " + ex.InnerException.ToString());
                Logs.MakeLog_File(Log_His.ImgE, "Message : " + ex.Message);
                Logs.MakeLog_File(Log_His.ImgE, "Source : " + ex.Source);
                Logs.MakeLog_File(Log_His.ImgE, "StackTrace : " + ex.StackTrace);
                Logs.MakeLog_File(Log_His.ImgE, "TargetSite : " + ex.TargetSite.ToString());
                Logs.MakeLog_File(Log_His.ImgE, "ToString : " + ex.ToString());
            }
            catch (Exception Log_ex)
            {
            }
        }

        public static void MakeLog_File(Log_His pMode, string pText)
        {
            if (old_Logs == pText) { return; }
                old_Logs = pText;

            string Log_Date = DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MM") + @"\" + DateTime.Now.ToString("dd");
            string Log_Path = System.Windows.Forms.Application.StartupPath + @"\Log\" + Log_Date;
            string Log_File = Log_Path + @"\Log File.log";
            string Log_Time = "[" + DateTime.Now.ToString(H2Y.format0Time) + "] ";
            string Log_Kind = "";
            string Log_Data;

            H2Y.Make_Dir(Log_Path);

            switch (pMode)
            {
                case Log_His.Butn: Log_Kind = "Buttons: "; break;
                case Log_His.Test: Log_Kind = "T Start: "; break;
                case Log_His.Step: Log_Kind = "T  Step: "; break;
                case Log_His.Stop: Log_Kind = "T  Stop: "; break;
                case Log_His.TEnd: Log_Kind = "TFinish: "; break;
                case Log_His.Msg_: Log_Kind = "Message: "; break;
                case Log_His.Err_: Log_Kind = "  Error: "; break;
                case Log_His.Save: Log_Kind = "T  Save: "; break;
                case Log_His.Del_: Log_Kind = " Delete: "; break;
                case Log_His.Back: Log_Kind = "Back-up: "; break;
            }

            Log_Data = Log_Time + Log_Kind + pText;

            Write_Append(Log_File, Log_Data);
        }

        public static void ECUsLog_File(Log_ECU pMode, string pText)
        {
            if (old_ECUs == pText) { return; }
                old_ECUs = pText;

            string Log_Date = DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MM") + @"\" + DateTime.Now.ToString("dd");
            string Log_Path = System.Windows.Forms.Application.StartupPath + @"\Log\" + Log_Date;
            string Log_File = Log_Path + @"\Log ECUs.log";
            string Log_Time = "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] ";
            string Log_Kind = "";
            string Log_Data;

            H2Y.Make_Dir(Log_Path);

            switch (pMode)
            {
                case Log_ECU.IDSN: Log_Kind = "Serial : "; break;
                case Log_ECU.Test: Log_Kind = "T Start: "; break;
                case Log_ECU.Puts: Log_Kind = "-> ECUs: "; break;
                case Log_ECU.Gets: Log_Kind = "<- ECUs: "; break;
                case Log_ECU.TEnd: Log_Kind = "TFinish: "; break;
            }

            Log_Data = Log_Time + Log_Kind + pText;

            Write_Append(Log_File, Log_Data);
        }

        public static void Days_History(Log_His pMode, string pText)
        {
            if (old_His_ == pText) { return; }
                old_His_ = pText;

            string His_Date = DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MM") + @"\" + DateTime.Now.ToString("dd");
            string His_Path = System.Windows.Forms.Application.StartupPath + @"\Log\" + His_Date;
            string His_File = System.Windows.Forms.Application.StartupPath + @"\Log\" + His_Date + @"\History.Log";
            string His_Time = "[" + DateTime.Now.ToString(H2Y.format0Time) + "] ";
            string His_Kind = "";
            string His_Data;

            H2Y.Make_Dir(His_Path);

            switch (pMode)
            {
                case Log_His.Test: His_Kind = "T Start: "; break;
                case Log_His.Stop: His_Kind = "T  Stop: "; break;
                case Log_His.TEnd: His_Kind = "TFinish: "; break;
                case Log_His.Err_: His_Kind = "  Error: "; break;
            }

            His_Data = His_Time + His_Kind + pText;

            Write_Append(His_File, His_Data);
        }

        public static bool Init_History(string pTestNo)
        {
            string His_Date = DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MM") + @"\" + DateTime.Now.ToString("dd");
            string His_Path = System.Windows.Forms.Application.StartupPath + @"\Log\" + His_Date;
            string His_File = System.Windows.Forms.Application.StartupPath + @"\Log\" + His_Date + @"\" + TSet.AcceptNo + ".Log";
            bool Ret_Flag;

            H2Y.Make_Dir(His_Path);

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(His_File, false, Encoding.Default))
            {
                try
                {
                    sw.WriteLine(pTestNo + " Log File");

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

        public static void Test_History(Log_His pMode, string pText)
        {
            string His_Date = DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MM") + @"\" + DateTime.Now.ToString("dd");
            string His_Path = System.Windows.Forms.Application.StartupPath + @"\Log\" + His_Date;
            string His_File = System.Windows.Forms.Application.StartupPath + @"\Log\" + His_Date + @"\" + TSet.AcceptNo + ".Log";
            string His_Time = "[" + DateTime.Now.ToString(H2Y.format3Time) + "] ";
            string His_Kind = "";
            string His_Data;

            H2Y.Make_Dir(His_Path);

            switch (pMode)
            {
                case Log_His.Info: His_Kind = "Info.  : "; break;
                case Log_His.Test: His_Kind = "T Start: "; break;
                case Log_His.Base: His_Kind = "WB Move: "; break;
                case Log_His.Butn: His_Kind = "Buttons: "; break;
                case Log_His.Data: His_Kind = "T  Data: "; break;
                case Log_His.Gaph: His_Kind = "T Graph: "; break;
                case Log_His.WSS_: His_Kind = "T  WSS_: "; break;
                case Log_His.Sped: His_Kind = "T Speed: "; break;
                case Log_His.Drag: His_Kind = "T  Drag: "; break;
                case Log_His.Brak: His_Kind = "T Brake: "; break;
                case Log_His.ABSB: His_Kind = "T ABS B: "; break;
                case Log_His.Park: His_Kind = "T  Park: "; break;
                case Log_His.Stop: His_Kind = "T  Stop: "; break;
                case Log_His.TEnd: His_Kind = "TFinish: "; break;
                case Log_His.Err_: His_Kind = "  Error: "; break;
            }

            His_Data = His_Time + His_Kind + pText;

            Write_Append(His_File, His_Data);
        }

        public static void Make_LossCal(int pMode, string pText)
        {
            string Cal_Path = System.Windows.Forms.Application.StartupPath + @"\Cal";
            string Cal_File = Cal_Path + @"\Loss-Cycle.log";

            string Cal_Time = "[" + DateTime.Now.ToString(H2Y.format0Time) + "] ";
            string Cal_Data;

            if (pMode == 1)
            {
                H2Y.Delete_File(Cal_File);
            }

            H2Y.Make_Dir(Cal_Path);

            Cal_Data = Cal_Time + pText;

            Write_Append(Cal_File, Cal_Data);
        }


        private static void Write_Append(string pFile, string pText)
        {
            FileStream fs = new FileStream(pFile, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(pText);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
