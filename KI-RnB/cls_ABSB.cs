using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Collections;

namespace KI_RnB
{
    public class ABS_Board
    {
        fom_Main Main = null;

        public bool IsOpen { get; set; }
        public bool Is_Log { get; set; }

        public SerialPort serialABSB = null;
        public long DIGet = 0;
        public bool[] D_Ins = new bool[10];
        public long DOGet = 0;
        public bool[] DOuts = new bool[10];
        public int[] DLoad = new int[6];
        public int[] oLoad = new int[6];

        public Int64[] G_Enc = new Int64[2];    //보드에서 읽은 초기 값
        public Int64[] GoEnc = new Int64[2];    //보드에서 읽은 초기 값(이전 데이터)
        public Int64[] O_Enc = new Int64[2];    //65535    초과 갯수
        public Int64[] T_Enc = new Int64[2];    //누적 펄스 값
        public Int64[] T0Enc = new Int64[2];    //누적 펄스 영점 값
        public Int64[] L_Enc = new Int64[2];    //최종 펄스 값
        public Int64[] LoEnc = new Int64[2];    //최종 펄스 값(이전 데이터)
        public float[] M_Enc = new float[2];    //이동 거리
        public float[] M_Gap = new float[2];    //Gap Time 간 펄스 수
        public float[] R_RPM = new float[2];    //RPM
        public float[] Speed = new float[2];    //속도(km/h)
        public float BSped = 0;
        public int BHerz = 0;
        public string str_List = "";

        private int B_Cnt = 0;      //정상 데이터 수
        private long mOfst = 0;
        private List<int> data = new List<int>();

        private string add_Buff;
        private Thread thread_ABSB;
        public bool threadStop;
        public int Get_Mode;

        private int Scan_CNT;
        private double ReadTime;
        private double Gap_Time;
        private double Old_Time;
        private double OfstTime;

        private Queue<int> AverQ_0 = new Queue<int>();
        private Queue<int> AverQ_1 = new Queue<int>();
        private Queue<int> AverQ_2 = new Queue<int>();
        private Queue<int> AverQ_3 = new Queue<int>();
        private Queue<int> AverQ_4 = new Queue<int>();
        private Queue<int> AverQ_5 = new Queue<int>();

        private Queue<int> SortQ_0 = new Queue<int>();
        private Queue<int> SortQ_1 = new Queue<int>();
        private Queue<int> SortQ_2 = new Queue<int>();
        private Queue<int> SortQ_3 = new Queue<int>();
        private Queue<int> SortQ_4 = new Queue<int>();
        private Queue<int> SortQ_5 = new Queue<int>();

        private int Sum0, Av_0;
        private int Sum1, Av_1;
        private int Sum2, Av_2;
        private int Sum3, Av_3;
        private int Sum4, Av_4;
        private int Sum5, Av_5;

        private int[] Ar0 = new int[60];
        private int[] Ar1 = new int[60];
        private int[] Ar2 = new int[60];
        private int[] Ar3 = new int[60];
        private int[] Ar4 = new int[60];
        private int[] Ar5 = new int[60];

        private int[] Value = new int[6];

        public Queue<double> Q0RPM = new Queue<double>();
        public Queue<double> Q1RPM = new Queue<double>();

        private string ErrM = "";

        private int counter = 0;

        private System.Drawing.Color white = System.Drawing.Color.White;
        private System.Drawing.Color black = System.Drawing.Color.Black;
        private System.Drawing.Color yellow = System.Drawing.Color.Yellow;
        private System.Drawing.Color green = System.Drawing.Color.Lime;
        
        public ABS_Board()
        {
            serialABSB = new System.IO.Ports.SerialPort();
        }

        public ABS_Board(fom_Main Main): 
            this()
        {
            this.Main = Main;
            GoEnc[0] = 0;
            GoEnc[1] = 0;
            ErrM = "KIMC :";
        }

        public bool Close()
        {
            if (serialABSB.IsOpen)
            {
                IsOpen = false;
                threadStop = true;
                if (Get_Mode == 0)
                {
                    thread_ABSB.Abort();
                }
                serialABSB.Close();
                serialABSB.ReadTimeout = 3;
                //serialABSB.ReceivedBytesThreshold = 50;
                //serialABSB.Close();   //닫고
                //serialABSB.Dispose(); //소멸
            }

            return serialABSB.IsOpen;
        }

        /// <summary>
        /// Kimc 보드 세팅
        /// </summary>
        /// <param name="pSett">"38400,n,8,1"</param>
        /// <param name="pPort">?</param>
        /// <returns>true/false</returns>
        public bool Setting(string pSett, string pPort, int pMode)
        {
            Get_Mode = pMode;       //통신 버퍼 읽기 방법(0: 쓰레드, 1:이벤트)

            if (pSett == null || pSett == "") return false; 

            string[] arStr = pSett.Split(',');
            if (arStr.Length != 4) return false;

            string mSerial_P = "COM" + PSet.Ctrl_P;
            if (!H2Y.SerialP_Scan(mSerial_P)) return false;

            try
            {
                int mBaudRate = int.Parse(arStr[0]);

                Parity mParity_B;
                switch (arStr[1].ToUpper())
                {
                    case "E": mParity_B = Parity.Even; break;
                    case "O": mParity_B = Parity.Odd; break;
                    case "N": mParity_B = Parity.None; break;
                    case "M": mParity_B = Parity.Mark; break;
                    case "S": mParity_B = Parity.Space; break;
                    default: mParity_B = Parity.None; break;
                }

                int mData_Bit = int.Parse(arStr[2]);
                StopBits mStop_Bit;
                switch (arStr[3].ToUpper())
                {
                    case "0": mStop_Bit = StopBits.None; break;
                    case "1": mStop_Bit = StopBits.One; break;
                    case "1.5": mStop_Bit = StopBits.OnePointFive; break;
                    case "2": mStop_Bit = StopBits.Two; break;
                    default: mStop_Bit = StopBits.One; break;
                }

                serialABSB.PortName = mSerial_P;
                serialABSB.BaudRate = mBaudRate;
                serialABSB.Parity = mParity_B;
                serialABSB.DataBits = mData_Bit;
                serialABSB.StopBits = mStop_Bit;

                if (!serialABSB.IsOpen)
                {
                    if (Get_Mode == 1)
                    {
                        serialABSB.DataReceived += new SerialDataReceivedEventHandler(serialABSB_DataReceived);
                    }
                    serialABSB.Open();

                    Start();
                    IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                Main.Prog_LogData(ex.Message);
                IsOpen = false;
            }

            return serialABSB.IsOpen;
        }

        public void Start()//Comfile 보드 초기화
        {
            OfstTime = DateTime.Now.Ticks;

            string str_Dia = PSet.RFLPul.ToString("00000");
            string str_ENC = PSet.RFLPul.ToString("00000");
            string Hex_Dia = PSet.RFLPul.ToString("X4");
            string Hex_ENC = PSet.RFLPul.ToString("X4");
            string Chk_Sum = "";
            byte CH_0 = byte.Parse(Hex_Dia.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte CH_1 = byte.Parse(Hex_Dia.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte CH_2 = byte.Parse(Hex_ENC.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte CH_3 = byte.Parse(Hex_ENC.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);

            if (serialABSB.IsOpen)
            {
                byte[] pData = new byte[16];
                pData[0] = (byte)'\x0002';
                pData[1] = (byte)'\x0082';
                pData[2] = Convert.ToByte(Convert.ToChar(str_Dia.Substring(0, 1)));
                pData[3] = Convert.ToByte(Convert.ToChar(str_Dia.Substring(1, 1)));
                pData[4] = Convert.ToByte(Convert.ToChar(str_Dia.Substring(2, 1)));
                pData[5] = Convert.ToByte(Convert.ToChar(str_Dia.Substring(3, 1)));
                pData[6] = Convert.ToByte(Convert.ToChar(str_Dia.Substring(4, 1)));

                pData[7] = Convert.ToByte(Convert.ToChar(str_ENC.Substring(0, 1)));
                pData[8] = Convert.ToByte(Convert.ToChar(str_ENC.Substring(1, 1)));
                pData[9] = Convert.ToByte(Convert.ToChar(str_ENC.Substring(2, 1)));
                pData[10] = Convert.ToByte(Convert.ToChar(str_ENC.Substring(3, 1)));
                pData[11] = Convert.ToByte(Convert.ToChar(str_ENC.Substring(4, 1)));

                Chk_Sum = (pData[1] ^ CH_0 ^ CH_1 ^ CH_2 ^ CH_3).ToString("000");
                pData[12] = Convert.ToByte(Convert.ToChar(Chk_Sum.Substring(0, 1)));
                pData[13] = Convert.ToByte(Convert.ToChar(Chk_Sum.Substring(1, 1)));
                pData[14] = Convert.ToByte(Convert.ToChar(Chk_Sum.Substring(2, 1)));

                pData[15] = (byte)'\x0003';

                counter = 0;

                serialABSB.Write(pData, 0, pData.Length);

                if (Get_Mode == 0)
                {
                    thread_ABSB = new Thread(ThreadABSB);
                    thread_ABSB.Start();
                }

                //while (!Read_Comfile())
                //{
                //    System.Windows.Forms.Application.DoEvents();
                //}               
            }
        }

        private void serialABSB_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Call_ABSB();
        }

        public void ThreadABSB()
        {
            try
            {
                while (thread_ABSB.IsAlive)
                {
                    if (!TSet.SimulOnf)
                    {
                        ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;

                        if (Math.Abs(ReadTime - Old_Time) >= 0.01)
                        {
                            Old_Time = ReadTime;

                            if (++counter > 1)
                            {
                                Call_Request();
                            }
                        }

                        if (BHerz < 5)
                        {
                            H2Y.Sleep(100);
                        }

                        Call_ABSB();

                        if (DateTime.Now.Ticks - mOfst >= TimeSpan.TicksPerSecond)
                        {
                            mOfst = DateTime.Now.Ticks;
                            BHerz = B_Cnt;
                            B_Cnt = 0;

                            Main.ABSBScanHerz(BHerz.ToString());
                            if (BHerz == 0)
                            {
                                Main.lbl_Ctrl.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                }
                return;
            }
            catch (Exception e)
            { }
        }

        /// <summary>
        /// 컴파일 보드 읽기
        /// </summary>
        public bool Call_ABSB()
        {            
            try
            {
                string str_Buff = serialABSB.ReadExisting();
                add_Buff = add_Buff + str_Buff;

                int SttP = add_Buff.IndexOf('\x0002');
                if (SttP < 0) return false;
                int EndP = add_Buff.IndexOf('\x0003', SttP);

                if (SttP + 59 == EndP)
                {
                    string Get_Buff = add_Buff.Substring(SttP + 1, 58);

                    Scan_Board(Get_Buff);
                    Old_Time = ReadTime;
                    add_Buff = "";
                }

                if (add_Buff.Length > 59) { add_Buff = ""; }

                return true;
            }
            catch (Exception e)
            { 
                Main.lbl_Ctrl.ForeColor = System.Drawing.Color.Red;
            }
            
            return false;
        }
        public void Call_Request()
        {
            if (serialABSB.IsOpen)
            {
                byte[] pData = new byte[6];
                pData[0] = (byte)'\x0002';
                pData[1] = Convert.ToByte('S');
                pData[2] = Convert.ToByte('0');
                pData[3] = Convert.ToByte('8');
                pData[4] = Convert.ToByte('3');
                pData[5] = (byte)'\x0003';

                counter = 0;

                serialABSB.Write(pData, 0, pData.Length);
            }
        }

        private bool Scan_Board(string pData)
        {
            if (pData.Length != 58) return false;

            string Chk_Sum = "";
            int chs = 0;

            try
            {
                data.Clear();
                foreach (byte chr in pData)
                {
                    data.Add(chr - 48);
                }

                for (int cnt = 0; cnt < 55; cnt++)
                {
                    chs = chs ^ (data[cnt] + 48);
                }

                Chk_Sum = ((data[55] * 100) + (data[56] * 10) + data[57]).ToString();
                if (chs.ToString() != Chk_Sum) return false;

                add_Buff = "";

                DIGet = ((data[0] * 100) + (data[1] * 10) + data[2]) + ((data[3] * 100) + (data[4] * 10) + data[5]) * 256;
                DOGet = ((data[6] * 100) + (data[7] * 10) + data[8]) + ((data[9] * 100) + (data[10] * 10) + data[11]) * 256;

                DLoad[0] = (data[12] * 1000) + (data[13] * 100) + (data[14] * 10) + data[15];
                DLoad[1] = (data[16] * 1000) + (data[17] * 100) + (data[18] * 10) + data[19];

                DLoad[2] = (data[20] * 10000) + (data[21] * 1000) + (data[22] * 100) + (data[23] * 10) + data[24];
                DLoad[3] = (data[25] * 10000) + (data[26] * 1000) + (data[27] * 100) + (data[28] * 10) + data[29];
                DLoad[4] = (data[30] * 10000) + (data[31] * 1000) + (data[32] * 100) + (data[33] * 10) + data[34];
                DLoad[5] = (data[35] * 10000) + (data[36] * 1000) + (data[37] * 100) + (data[38] * 10) + data[39];

                G_Enc[0] = (data[40] * 10000) + (data[41] * 1000) + (data[42] * 100) + (data[43] * 10) + data[44];
                G_Enc[1] = (data[45] * 10000) + (data[46] * 1000) + (data[47] * 100) + (data[48] * 10) + data[49];

                BSped = ((data[50] * 10000) + (data[51] * 1000) + (data[52] * 100) + (data[53] * 10) + data[54]) / 10;
                //Chk_Sum = ((data[55] * 100) + (data[56] * 10) + data[57]).ToString();

                if (GoEnc[0] > G_Enc[0]) { O_Enc[0]++; }
                if (GoEnc[1] > G_Enc[1]) { O_Enc[1]++; }

                AD_Scaning();
                DI_Scaning();
                DO_Scaning();
                ENCScaning();
                
                oLoad[0] = DLoad[0];
                oLoad[1] = DLoad[1];
                oLoad[2] = DLoad[2];
                oLoad[3] = DLoad[3];
                oLoad[4] = DLoad[4];
                oLoad[5] = DLoad[5];

                GoEnc[0] = G_Enc[0];
                GoEnc[1] = G_Enc[1];

                str_List = pData;
                if (PSet.OnfDebug)
                {
                    Main.FomDebug.ABSB_Message(pData);
                }

                if (Is_Log)
                {
                    Main.ABSB_LogData(pData);
                }

                System.Drawing.Color black = System.Drawing.Color.Black;
                System.Drawing.Color yellow = System.Drawing.Color.Yellow;

                B_Cnt++;
                if (B_Cnt % 5 == 0)
                {
                    Main.lbl_Ctrl.ForeColor = (Main.lbl_Ctrl.ForeColor == black ? yellow : black);
                }

                add_Buff = "";

                return true;
            }
            catch (Exception ex)
            {
                Main.lbl_Ctrl.ForeColor = System.Drawing.Color.Red;
                return false;
            }
        }

        private void AD_Scaning()
        {
            //Cutting filter
            //if (Math.Abs(oLoad[0] - DLoad[0]) > 2000) DLoad[0] = oLoad[0];
            //if (Math.Abs(oLoad[1] - DLoad[1]) > 2000) DLoad[1] = oLoad[1];
            //if (Math.Abs(oLoad[2] - DLoad[2]) > 50000) DLoad[2] = oLoad[2];
            //if (Math.Abs(oLoad[3] - DLoad[3]) > 50000) DLoad[3] = oLoad[3];
            //if (Math.Abs(oLoad[4] - DLoad[4]) > 50000) DLoad[4] = oLoad[4];
            //if (Math.Abs(oLoad[5] - DLoad[5]) > 50000) DLoad[5] = oLoad[5];

            switch (PSet.Filter)
            {
                case 0: X__Filtering(); break;
                case 1: Q__Filtering(); break;
                case 2: VB_Filtering(); break;
            }

            PSet.CH0Last = PSet.CH0Scan - PSet.CH0Zero;
            PSet.CH1Last = PSet.CH1Scan - PSet.CH1Zero;
            PSet.CH2Last = PSet.CH2Scan - PSet.CH2Zero;
            PSet.CH3Last = PSet.CH3Scan - PSet.CH3Zero;
            PSet.CH4Last = PSet.CH4Scan - PSet.CH4Zero;
            PSet.CH5Last = PSet.CH5Scan - PSet.CH5Zero;

            PSet.CH0_Val = PSet.CH0Last * PSet.CH0Span;
            PSet.CH1_Val = PSet.CH1Last * PSet.CH1Span;
            PSet.CH2_Val = PSet.CH2Last * PSet.CH2Span;
            PSet.CH3_Val = PSet.CH3Last * PSet.CH3Span;
            PSet.CH4_Val = PSet.CH4Last * PSet.CH4Span;
            PSet.CH5_Val = PSet.CH5Last * PSet.CH5Span;

            if (Math.Abs(PSet.CH0_Val) < 0.1) { PSet.CH0_Val = 0; }
            if (Math.Abs(PSet.CH1_Val) < 0.1) { PSet.CH1_Val = 0; }

            oLoad[0] = DLoad[0];
            oLoad[1] = DLoad[1];
            oLoad[2] = DLoad[2];
            oLoad[3] = DLoad[3];
            oLoad[4] = DLoad[4];
            oLoad[5] = DLoad[5];
        }
        private void DI_Scaning()
        {
            Din__Show(DIGet);
        }
        private void DO_Scaning()
        {
            DOut_Show(DOGet);
        }
        private void ENCScaning()
        {
            int RPM_Mode = 0;

            T_Enc[0] = (O_Enc[0] * 65536) + G_Enc[0];
            T_Enc[1] = (O_Enc[1] * 65536) + G_Enc[1];

            L_Enc[0] = T_Enc[0] - T0Enc[0];
            L_Enc[1] = T_Enc[1] - T0Enc[1];

            //M_Enc[0] = (((PSet.Roll_Dia * Math.PI) * L_Enc[0]) / 1000 / 1);
            //M_Enc[1] = (((PSet.Roll_Dia * Math.PI) * L_Enc[1]) / 1000 / 1);

            //Gap_Time = ReadTime - Old_Time;
            //if (Gap_Time >= 0.5)
            //{
            //    Old_Time = ReadTime;
            //    M_Gap[0] = L_Enc[0] - LoEnc[0];
            //    M_Gap[1] = L_Enc[1] - LoEnc[1];

            //    R_RPM[0] = Math.Abs(M_Gap[0] / PSet.Enc_Puls) * (60 / Gap_Time);
            //    R_RPM[1] = Math.Abs(M_Gap[1] / PSet.Enc_Puls) * (60 / Gap_Time);

            //    double[] RPM = new double[2];

            //    RPM_Mode = 3;
            //    switch (RPM_Mode)
            //    {
            //        case 0: RPM[0] = H2Y.QAver_Filter(25, R_RPM[0], Q0RPM);
            //                RPM[1] = H2Y.QAver_Filter(25, R_RPM[1], Q1RPM); break;

            //        case 1: RPM[0] = H2Y.QSort_Filter(25, R_RPM[0], Q0RPM);
            //                RPM[1] = H2Y.QSort_Filter(25, R_RPM[1], Q1RPM); break;

            //        case 2: RPM[0] = H2Y.QSoAv3Filter(25, R_RPM[0], Q0RPM);
            //                RPM[1] = H2Y.QSoAv3Filter(25, R_RPM[1], Q1RPM); break;

            //        case 3: RPM[0] = H2Y.QSoAv5Filter(25, R_RPM[0], Q0RPM);
            //                RPM[1] = H2Y.QSoAv5Filter(25, R_RPM[1], Q1RPM); break;
            //    }

            //    Speed[0] = Math.Abs(((PSet.Roll_Dia * 3.14f) * R_RPM[0] * 60) / 1000 / 1000);
            //    Speed[1] = Math.Abs(((PSet.Roll_Dia * 3.14f) * R_RPM[1] * 60) / 1000 / 1000);

            //    PSet.CH0Speed = Convert.ToSingle(Speed[0]);
            //    PSet.CH1Speed = Convert.ToSingle(Speed[1]);

            //    if (PSet.Simulator)
            //    {
            //        PSet.CH0Speed = PSet.Sp0simul;
            //        PSet.CH1Speed = PSet.Sp1simul;
            //    }

            //    LoEnc[0] = L_Enc[0];
            //    LoEnc[1] = L_Enc[1];
            //}
        }

        #region Filter System
        private void X__Filtering()
        {
            PSet.CH0Scan = DLoad[0];
            PSet.CH1Scan = DLoad[1];
            PSet.CH2Scan = DLoad[2];
            PSet.CH3Scan = DLoad[3];
            PSet.CH4Scan = DLoad[4];
            PSet.CH5Scan = DLoad[5];
        }
        private void Q__Filtering()
        {
            AverQ_0.Enqueue(DLoad[0]);
            AverQ_1.Enqueue(DLoad[1]);
            AverQ_2.Enqueue(DLoad[2]);
            AverQ_3.Enqueue(DLoad[3]);
            AverQ_4.Enqueue(DLoad[4]);
            AverQ_5.Enqueue(DLoad[5]);

            if (AverQ_0.Count > PSet.Av_Filt) AverQ_0.Dequeue();
            if (AverQ_1.Count > PSet.Av_Filt) AverQ_1.Dequeue();
            if (AverQ_2.Count > PSet.Av_Filt) AverQ_2.Dequeue();
            if (AverQ_3.Count > PSet.Av_Filt) AverQ_3.Dequeue();
            if (AverQ_4.Count > PSet.Av_Filt) AverQ_4.Dequeue();
            if (AverQ_5.Count > PSet.Av_Filt) AverQ_5.Dequeue();

            Av_0 = (int)AverQ_0.Average();
            Av_1 = (int)AverQ_1.Average();
            Av_2 = (int)AverQ_2.Average();
            Av_3 = (int)AverQ_3.Average();
            Av_4 = (int)AverQ_4.Average();
            Av_5 = (int)AverQ_5.Average();

            SortQ_0.Enqueue(Av_0);
            SortQ_1.Enqueue(Av_1);
            SortQ_2.Enqueue(Av_2);
            SortQ_3.Enqueue(Av_3);
            SortQ_4.Enqueue(Av_4);
            SortQ_5.Enqueue(Av_5);

            if (SortQ_0.Count > PSet.St_Filt) SortQ_0.Dequeue();
            if (SortQ_1.Count > PSet.St_Filt) SortQ_1.Dequeue();
            if (SortQ_2.Count > PSet.St_Filt) SortQ_2.Dequeue();
            if (SortQ_3.Count > PSet.St_Filt) SortQ_3.Dequeue();
            if (SortQ_4.Count > PSet.St_Filt) SortQ_4.Dequeue();
            if (SortQ_5.Count > PSet.St_Filt) SortQ_5.Dequeue();

            int[] Arr_0 = new int[PSet.St_Filt];
            int[] Arr_1 = new int[PSet.St_Filt];
            int[] Arr_2 = new int[PSet.St_Filt];
            int[] Arr_3 = new int[PSet.St_Filt];
            int[] Arr_4 = new int[PSet.St_Filt];
            int[] Arr_5 = new int[PSet.St_Filt];

            SortQ_0.CopyTo(Arr_0, 0);   Array.Sort(Arr_0);
            SortQ_1.CopyTo(Arr_1, 0);   Array.Sort(Arr_1);
            SortQ_2.CopyTo(Arr_2, 0);   Array.Sort(Arr_2);
            SortQ_3.CopyTo(Arr_3, 0);   Array.Sort(Arr_3);
            SortQ_4.CopyTo(Arr_4, 0);   Array.Sort(Arr_4);
            SortQ_5.CopyTo(Arr_5, 0);   Array.Sort(Arr_5);

            PSet.CH0Scan = Arr_0.ElementAt<int>((PSet.St_Filt - 1) / 2);
            PSet.CH1Scan = Arr_1.ElementAt<int>((PSet.St_Filt - 1) / 2);
            PSet.CH2Scan = Arr_2.ElementAt<int>((PSet.St_Filt - 1) / 2);
            PSet.CH3Scan = Arr_3.ElementAt<int>((PSet.St_Filt - 1) / 2);
            PSet.CH4Scan = Arr_4.ElementAt<int>((PSet.St_Filt - 1) / 2);
            PSet.CH5Scan = Arr_5.ElementAt<int>((PSet.St_Filt - 1) / 2);
        }
        private void VB_Filtering()
        {
            Sum0 += DLoad[0];
            Sum1 += DLoad[1];
            Sum2 += DLoad[2];
            Sum3 += DLoad[3];
            Sum4 += DLoad[4];
            Sum5 += DLoad[5];

            Scan_CNT++;
            if (Scan_CNT >= PSet.Av_Filt)
            {
                Av_0 = Sum0 / Scan_CNT; Sum0 = 0;
                Av_1 = Sum1 / Scan_CNT; Sum1 = 0;
                Av_2 = Sum2 / Scan_CNT; Sum2 = 0;
                Av_3 = Sum3 / Scan_CNT; Sum3 = 0;
                Av_4 = Sum4 / Scan_CNT; Sum4 = 0;
                Av_5 = Sum5 / Scan_CNT; Sum5 = 0;

                Scan_CNT = 0;
                PSet.CH0Scan = H2Y.Sort_Filter(PSet.St_Filt, Av_0, Ar0);
                PSet.CH1Scan = H2Y.Sort_Filter(PSet.St_Filt, Av_1, Ar1);
                PSet.CH2Scan = H2Y.Sort_Filter(PSet.St_Filt, Av_2, Ar2);
                PSet.CH3Scan = H2Y.Sort_Filter(PSet.St_Filt, Av_3, Ar3);
                PSet.CH4Scan = H2Y.Sort_Filter(PSet.St_Filt, Av_4, Ar4);
                PSet.CH5Scan = H2Y.Sort_Filter(PSet.St_Filt, Av_5, Ar5);
            }
            else
            {
                PSet.CH0Scan = DLoad[0];
                PSet.CH1Scan = DLoad[1];
                PSet.CH2Scan = DLoad[2];
                PSet.CH3Scan = DLoad[3];
                PSet.CH4Scan = DLoad[4];
                PSet.CH5Scan = DLoad[5];
            }
        }
        #endregion

        /// <summary>
        /// 컴파일 보드 Digital 출력
        /// </summary>
        /// <param name="pVal">출력 값</param>
        public void Puts_DOUT(long pVal)
        {
            string Hex_Val = pVal.ToString("X4");
            string Chk_Sum = "";
            byte Val_H = byte.Parse(Hex_Val.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte Val_L = byte.Parse(Hex_Val.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            string H_Str = Val_H.ToString("000");
            string L_Str = Val_L.ToString("000");

            if (serialABSB.IsOpen)
            {
                byte[] pData = new byte[11];
                pData[0] = (byte)'\x0002';
                pData[1] = Convert.ToByte(Convert.ToChar(H_Str.Substring(0, 1)));
                pData[2] = Convert.ToByte(Convert.ToChar(H_Str.Substring(1, 1)));
                pData[3] = Convert.ToByte(Convert.ToChar(H_Str.Substring(2, 1)));
                pData[4] = Convert.ToByte(Convert.ToChar(L_Str.Substring(0, 1)));
                pData[5] = Convert.ToByte(Convert.ToChar(L_Str.Substring(1, 1)));
                pData[6] = Convert.ToByte(Convert.ToChar(L_Str.Substring(2, 1)));

                Chk_Sum = (Val_H ^ Val_L).ToString("000");
                pData[7] = Convert.ToByte(Convert.ToChar(Chk_Sum.Substring(0, 1)));
                pData[8] = Convert.ToByte(Convert.ToChar(Chk_Sum.Substring(1, 1)));
                pData[9] = Convert.ToByte(Convert.ToChar(Chk_Sum.Substring(2, 1)));

                pData[10] = (byte)'\x0003';

                serialABSB.Write(pData, 0, pData.Length);
            }
        }

        /// <summary>
        /// Digital Intput 10 EA: True, False
        /// </summary>
        /// <param name="pVal">Digital Input</param>
        private void Din__Show(long pVal)
        {
            for (int cnt = 0; cnt < 10; cnt++)
            {
                D_Ins[cnt] = (DIGet & H2Y.BitA[cnt]) > 0 ? true : false;
            }
        }

        /// <summary>
        /// Digital Output 10 EA: True, False
        /// </summary>
        /// <param name="pVal">Digital Output</param>
        private void DOut_Show(long pVal)
        {
            for (int cnt = 0; cnt < 10; cnt++)
            {
                DOuts[cnt] = (DOGet & H2Y.BitA[cnt]) > 0 ? true : false;
            }
        }
    }
}