using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.Windows.Forms;

namespace KI_RnB
{
    public class clsBS205
    {
        fom_Main Main = null;

        public bool IsOpen { get; set; }
        public bool Is_Log { get; set; }
        public bool Is_Onf = false;

        private static Queue<double> Q_Sort = new Queue<double>();

        private static int IHerz;
        private static int I_Cnt = 0;
        private static long mOfst = 0;

        private SerialPort comBS205;
        private string Ret_Hex = "";
        private System.Windows.Forms.Timer tmrBS205;

        public clsBS205()
        {
            comBS205 = new SerialPort();
            tmrBS205 = new System.Windows.Forms.Timer();
        }

        public clsBS205(Form Main)
            : this()
        {
            Main = (fom_Main)Main;
        }

        public bool Close()
        {
            if (comBS205.IsOpen)
            {
                comBS205.ReadTimeout = 10;
                //com_Indi.ReceivedBytesThreshold = 50;
                comBS205.Close();   //닫고
                comBS205.Dispose(); //소멸
            }

            if (tmrBS205 != null) { tmrBS205.Enabled = false; }
            return comBS205.IsOpen;
        }

        public bool Setting(string pSett, string pPort)
        {
            string[] arStr = pSett.Split(',');
            if (arStr.Length != 4) return false;

            if (comBS205.IsOpen) { comBS205.Close(); }

            try
            {
                pPort = pPort.Replace("COM", "");
                comBS205.PortName = "COM" + pPort;
                comBS205.BaudRate = Convert.ToInt16(arStr[0]);  //9600

                if (H2Y.SerialP_Scan(comBS205.PortName))
                {
                    switch (arStr[1].ToUpper())
                    {
                        case "E": comBS205.Parity = Parity.Even; break;
                        case "O": comBS205.Parity = Parity.Odd; break;
                        case "N": comBS205.Parity = Parity.None; break;
                        case "M": comBS205.Parity = Parity.Mark; break;
                        case "S": comBS205.Parity = Parity.Space; break;
                    }

                    comBS205.DataBits = Convert.ToInt16(arStr[2]);  //8

                    switch (arStr[3].ToUpper())
                    {
                        case "0": comBS205.StopBits = StopBits.None; break;
                        case "1": comBS205.StopBits = StopBits.One; break;
                        case "1.5": comBS205.StopBits = StopBits.OnePointFive; break;
                        case "2": comBS205.StopBits = StopBits.Two; break;
                    }

                    comBS205.Open();
                    comBS205.ReadTimeout = 200;
                    comBS205.DataReceived += new SerialDataReceivedEventHandler(BS205_DataReceived);
                    IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Main.Prog_LogData(ex.Message);
                IsOpen = false;
            }

            tmrBS205.Tick += new EventHandler(tmrBS205_Tick);
            tmrBS205.Interval = 100;
            tmrBS205.Enabled = true;

            return comBS205.IsOpen;
        }

        void tmrBS205_Tick(object sender, EventArgs e)
        {
            if (comBS205.IsOpen)
            {
                Send_Reading();
            }
        }

        public void Send_Zero()
        {
            if (comBS205.IsOpen)
            {
                comBS205.Write("0Z");
                if (Is_Log) { Main.Indi_LogData("0Z"); }
            }
        }

        public void Send_Reading()
        {
            if (comBS205.IsOpen)
            {
                comBS205.Write("0R");
                if (Is_Log) { Main.Indi_LogData("0R"); } 

            }
        }

        private void BS205_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // 버퍼에서 대기중인 모든 데이터 읽기
            string data = comBS205.ReadExisting();
            //string str_Buff =  data.Replace('\x0003', Char.Parse(" "));

            try
            {
                string[] str_B = data.Split('\x0003');

                if (Is_Log) { Main.Indi_LogData(data); }

                int stt = str_B[0].IndexOf("+");
                int end = str_B[0].IndexOf('\x0003');

                //2+  1292
                str_B[0] = str_B[0].Substring(3, 6);

                if (str_B[0].Length < 4) { return; }

                string sine_Val = data.Substring(2, 1);
                float indi_Val;
                if (!float.TryParse(str_B[0], out indi_Val)) return;

                Is_Onf = !Is_Onf;

                if (sine_Val == "-")
                {
                    indi_Val = indi_Val * -1;
                }
                else
                {
                    indi_Val = indi_Val * 1;
                }

                TSet.Bongshin = H2Y.QSort_Filter(5, indi_Val, Q_Sort);

                if (Is_Log) { Main.Indi_LogData(data); }

                I_Cnt++;
                //if (I_Cnt % 30 == 0)
                //{
                //    ((fom_Main)Main).lbl_PLCs.ForeColor = (((fom_Main)Main).lbl_PLCs.ForeColor == System.Drawing.Color.Black ? System.Drawing.Color.Green : System.Drawing.Color.Black);
                //}
                if (DateTime.Now.Ticks - mOfst >= TimeSpan.TicksPerSecond)
                {
                    mOfst = DateTime.Now.Ticks;
                    IHerz = I_Cnt;
                    I_Cnt = 0;
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "BS205_DataReceived: " + ex.Message);
            }
        }
    }
}
