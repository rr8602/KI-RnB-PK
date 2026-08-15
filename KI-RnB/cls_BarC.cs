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
    public class Barcode
    {
        fom_Main Fom_Main = null;

        public bool IsOpen { get; set; }
        public bool Is_Log { get; set; }

        private static Queue<double> Q_Sort = new Queue<double>();

        private static int IHerz;
        private static int I_Cnt = 0;
        private static long mOfst = 0;

        private SerialPort com_BarC;
        private string Ret_Hex = "";
        private int Idx;
        public Thread thread;
        public bool threadStop;
        
        public Barcode()
        {
            com_BarC = new SerialPort();
        }

        public Barcode(Form Main)
            : this()
        {
            Fom_Main = (fom_Main)Main;
        }

        public bool Close()
        {
            if (com_BarC.IsOpen)
            {
                threadStop = true;
                com_BarC.ReadTimeout = 10;
                //com_Indi.ReceivedBytesThreshold = 50;
                com_BarC.Close();   //닫고
                com_BarC.Dispose(); //소멸
            }

            return com_BarC.IsOpen;
        }

        public bool Setting(string pSett, string pPort)
        {
            string[] arStr = pSett.Split(',');
            if (arStr.Length != 4) return false;

            if (com_BarC.IsOpen) { com_BarC.Close(); }

            try
            {
                pPort = pPort.Replace("COM", "");
                com_BarC.PortName = "COM" + pPort;

                com_BarC.RtsEnable = true;
                com_BarC.DtrEnable = true;
                com_BarC.BaudRate = Convert.ToInt16(arStr[0]);  //9600

                if (H2Y.SerialP_Scan(com_BarC.PortName))
                {
                    switch (arStr[1].ToUpper())
                    {
                        case "E": com_BarC.Parity = Parity.Even;    break;
                        case "O": com_BarC.Parity = Parity.Odd;     break;
                        case "N": com_BarC.Parity = Parity.None;    break;
                        case "M": com_BarC.Parity = Parity.Mark;    break;
                        case "S": com_BarC.Parity = Parity.Space;   break;
                    }

                    com_BarC.DataBits = Convert.ToInt16(arStr[2]);  //8

                    switch (arStr[3].ToUpper())
                    {
                        case "0":   com_BarC.StopBits = StopBits.None;          break;
                        case "1":   com_BarC.StopBits = StopBits.One;           break;
                        case "1.5": com_BarC.StopBits = StopBits.OnePointFive;  break;
                        case "2":   com_BarC.StopBits = StopBits.Two;           break;
                    }

                    com_BarC.Open();
                    com_BarC.ReadTimeout = 200;
                    com_BarC.DataReceived += new SerialDataReceivedEventHandler(BarC_DataReceived);
                    IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                System.Diagnostics.Debug.WriteLine("BarCode : " + ex.Message);
                Fom_Main.Prog_LogData("BarCode : " + ex.Message);
                IsOpen = false;
            }

            return com_BarC.IsOpen;
        }

        private void BarC_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // 버퍼에서 대기중인 모든 데이터 읽기

            H2Y.Sleep(100);
            string data = com_BarC.ReadExisting();
            data = data.Replace(H2Y.CrLf, "");

            if (data.IndexOf("?") > -1)
            {
                Logs.MakeLog_File(Log_His.Err_, data);
                return;
            }

            if (!TSet.Test_Run)
            {
                Fom_Main.Bar1_LogData(data);
            }

            if (PSet.OnfSetup) return;
            if (PSet.Onf_Stop) return;
            if (PSet.Onf_PsWd) return;
        }
    }
}
