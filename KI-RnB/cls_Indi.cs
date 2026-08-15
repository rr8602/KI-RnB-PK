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
    public class Indicator
    {
        fom_Main Fom_Main = null;

        public bool IsOpen { get; set; }
        public bool Is_Log { get; set; }

        private static Queue<double> Q_Sort = new Queue<double>();

        private static int IHerz;
        private static int I_Cnt = 0;
        private static long mOfst = 0;

        private SerialPort com_Indi;
        private string Ret_Hex = "";
        private int Idx;
        public Thread thread;
        public bool threadStop;
        
        public Indicator()
        {
            com_Indi = new SerialPort();
        }

        public Indicator(Form Main) : this()
        {
            Fom_Main = (fom_Main)Main;
        }

        public bool Close()
        {
            if (com_Indi.IsOpen)
            {
                threadStop = true;
                com_Indi.ReadTimeout = 10;
                //com_Indi.ReceivedBytesThreshold = 50;
                com_Indi.Close();   //닫고
                com_Indi.Dispose(); //소멸
            }

            return com_Indi.IsOpen;
        }

        public bool Setting(string pSett, string pPort)
        {
            string[] arStr = pSett.Split(',');
            if (arStr.Length != 4) return false;

            if (com_Indi.IsOpen) { com_Indi.Close(); }

            try
            {
                pPort = pPort.Replace("COM", "");
                com_Indi.PortName = "COM" + pPort;
                com_Indi.BaudRate = Convert.ToInt16(arStr[0]);  //9600

                if (H2Y.SerialP_Scan(com_Indi.PortName))
                {
                    switch (arStr[1].ToUpper())
                    {
                        case "E": com_Indi.Parity = Parity.Even; break;
                        case "O": com_Indi.Parity = Parity.Odd; break;
                        case "N": com_Indi.Parity = Parity.None; break;
                        case "M": com_Indi.Parity = Parity.Mark; break;
                        case "S": com_Indi.Parity = Parity.Space; break;
                    }

                    com_Indi.DataBits = Convert.ToInt16(arStr[2]);  //8

                    switch (arStr[3].ToUpper())
                    {
                        case "0": com_Indi.StopBits = StopBits.None; break;
                        case "1": com_Indi.StopBits = StopBits.One; break;
                        case "1.5": com_Indi.StopBits = StopBits.OnePointFive; break;
                        case "2": com_Indi.StopBits = StopBits.Two; break;
                    }

                    com_Indi.Open();
                    com_Indi.ReadTimeout = 200;
                    com_Indi.DataReceived += new SerialDataReceivedEventHandler(Indi_DataReceived);
                    IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Fom_Main.Prog_LogData(ex.Message);
                IsOpen = false;
            }

            return com_Indi.IsOpen;
        }

        private void Indi_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // 버퍼에서 대기중인 모든 데이터 읽기
            string data = com_Indi.ReadExisting();

            if (data.Length == 14)
            {
                string str_Gets = data.Substring(3, 9);
                double indi_Val;
                if (!double.TryParse(str_Gets, out indi_Val)) return;
                
                TSet.Bongshin = H2Y.QSort_Filter(5, indi_Val, Q_Sort);

                if (Is_Log) { Fom_Main.Indi_LogData(data); }


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

                    Fom_Main.IndiScanHerz(IHerz.ToString());
                }
            }
        }
    }
}