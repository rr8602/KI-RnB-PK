using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Threading;
using System.Net.NetworkInformation;
using KI_TTS;

namespace KI_RnB
{
    public static class H2Y
    {
        public static double tick_Dvd = 10000000;

        public const string OK = "O";
        public const string NG = "X";

        public const string format_Date = "yyyy-MM-dd";
        public const string formatsDate = "yyyyMMdd";
        public const string format0Time = "HH:mm:ss";
        public const string format3Time = "HH:mm:ss.fff";

        public static Color Black = System.Drawing.Color.Black;
        public static Color Yellow = System.Drawing.Color.Yellow;
        public static Color Lime = System.Drawing.Color.Lime;

        public const string CrLf = Microsoft.VisualBasic.ControlChars.CrLf;
        public static string App_Path = System.Windows.Forms.Application.StartupPath;

        public static int[] BitA = new int[16];

        private static string old_Msgs = "";
        private static string old_Axle = "";
        private static string old_RegNo = "";
        private static string old_Model = "";
        private static string old_PrsNo = "";

        private static string old_Std = "";
        private static string old_Pan = "";
        private static Color std_Color = Color.Black;

        private static string old_msg = "";
        private static Color msg_color = Color.Black;

        #region Virtual-Key Codes
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern short GetKeyState(int key);

        public const int KeyPressed = 0x8000;
        public const int VK_LBUTTON = 0x01;     //Left mouse button
        public const int VK_RBUTTON = 0x02;     //Right mouse button
        public const int VK_CANCEL = 0x03;      //Control-break processing
        public const int VK_MBUTTON = 0x04;     //Middle mouse button (three-button mouse)
        public const int VK_XBUTTON1 = 0x05;    //X1 mouse button
        public const int VK_XBUTTON2 = 0x06;    //X2 mouse button
        public const int VK_BACK = 0x08;        //BACKSPACE key
        public const int VK_TAB = 0x09;         //TAB key
        public const int VK_CLEAR = 0x0C;       //CLEAR key
        public const int VK_RETURN = 0x0D;      //ENTER key
        public const int VK_SHIFT = 0x10;       //SHIFT key
        public const int VK_CONTROL = 0x11;     //CTRL key
        public const int VK_MENU = 0x12;        //ALT key
        public const int VK_PAUSE = 0x13;       //PAUSE key
        public const int VK_CAPITAL = 0x14;     //CAPS Lock key
        public const int VK_KANA = 0x15;        //IME Kana mode
        public const int VK_HANGUEL = 0x15;     //IME Hanguel mode (maintained for compatibility; use VK_HANGUL)
        public const int VK_HANGUL = 0x15;      //IME Hangul mode
        public const int VK_JUNJA = 0x17;       //IME Junja mode
        public const int VK_FINAL = 0x18;       //IME final mode
        public const int VK_HANJA = 0x19;       //IME Hanja mode
        public const int VK_KANJI = 0x19;       //IME Kanji mode
        public const int VK_ESCAPE = 0x1B;      //ESC key
        public const int VK_CONVERT = 0x1C;     //IME convert
        public const int VK_NONCONVERT = 0x1D;  //IME nonconvert
        public const int VK_ACCEPT = 0x1E;      //IME accept
        public const int VK_MODECHANGE = 0x1F;  //IME mode change request
        public const int VK_SPACE = 0x20;       //SPACEBAR
        public const int VK_PRIOR = 0x21;       //PAGE UP key
        public const int VK_NEXT = 0x22;        //PAGE DOWN key
        public const int VK_END = 0x23;         //END key
        public const int VK_HOME = 0x24;        //HOME key
        public const int VK_LEFT = 0x25;        //LEFT ARROW key
        public const int VK_UP = 0x26;          //UP ARROW key
        public const int VK_RIGHT = 0x27;       //RIGHT ARROW key
        public const int VK_DOWN = 0x28;        //DOWN ARROW key
        public const int VK_SELECT = 0x29;      //SELECT key
        public const int VK_PRINT = 0x2A;       //PRINT key
        public const int VK_EXECUTE = 0x2B;     //EXECUTE key
        public const int VK_SNAPSHOT = 0x2C;    //PRINT SCREEN key
        public const int VK_INSERT = 0x2D;      //INS key
        public const int VK_DELETE = 0x2E;      //DEL key
        public const int VK_HELP = 0x2F;        //HELP key
        #endregion
        
        //Bitmap memoryImage;

        //public static void CaptureScreen()
        //{
        //    Graphics myGraphics = CreateGraphics();
        //    Size s = new System.Drawing.Size(500, 600);
        //    memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
        //    memoryImage.Save("");
        //}

        #region 이미지 처리
        public static void Screen__Head(System.Windows.Forms.PictureBox pic, string Axle, string RegNo, string Model)
        {
            //if (old_Axle == Axle && old_RegNo == RegNo && old_Model == Model) { return; }
            old_Axle = Axle; old_RegNo = RegNo; old_Model = Model;

            try
            {
                Bitmap bmp = new Bitmap(Properties.Resources.Head);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    RectangleF drawRect = new RectangleF(0, 15, 154, 70);
                    H2Y.Draw__String(g, 40, Axle, Color.White, drawRect, StringAlignment.Center);

                    //drawRect = new RectangleF(161, 30, 120, 27);
                    //Draw__String(g, 20, "차량번호", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(160, 30, 350, 27);
                    H2Y.Draw__String(g, 20, RegNo, Color.Yellow, drawRect, StringAlignment.Center);

                    drawRect = new RectangleF(522, 30, 120, 27);
                    H2Y.Draw__String(g, 20, "Model", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(648, 30, 218, 27);
                    H2Y.Draw__String(g, 20, Model, Color.Lime, drawRect, StringAlignment.Near);

                    g.Dispose();
                }

                pic.Image = bmp;
            }
            catch (Exception ex)
            {
                Logs.ExceptionErr(ex);
                return;
            }
        }
        public static void Screen__Head(System.Windows.Forms.PictureBox pic, string Axle, string RegNo, string Model, string PrsNo)
        {
            //if (old_Axle == Axle && old_RegNo == RegNo && old_Model == Model && old_PrsNo == PrsNo) { return; }
            old_Axle = Axle; old_RegNo = RegNo; old_Model = Model; old_PrsNo = PrsNo;

            try
            {
                //Bitmap bmp = new Bitmap(pic.Width, pic.Height);
                Bitmap bmp = new Bitmap(Properties.Resources.Head);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    RectangleF drawRect = new RectangleF(0, 15, 154, 70);
                    Draw__String(g, 40, Axle, Color.White, drawRect, StringAlignment.Center);

                    //drawRect = new RectangleF(161, 30, 120, 27);
                    //Draw__String(g, 20, "차량번호", Color.White, drawRect, StringAlignment.Near);

                    //drawRect = new RectangleF(287, 30, 229, 27);
                    //Draw__String(g, 20, RegNo, Color.Yellow, drawRect, StringAlignment.Near);
                    drawRect = new RectangleF(160, 30, 350, 27);
                    Draw__String(g, 20, RegNo, Color.Yellow, drawRect, StringAlignment.Center);

                    drawRect = new RectangleF(522, 30, 120, 27);
                    Draw__String(g, 20, "Model", Color.White, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(648, 30, 218, 27);
                    Draw__String(g, 20, Model, Color.Lime, drawRect, StringAlignment.Near);

                    drawRect = new RectangleF(866, 15, 134, 70);
                    Draw__String(g, 40, PrsNo, Color.White, drawRect, StringAlignment.Center);

                    g.Dispose();
                }

                pic.Image = bmp;
            }
            catch (Exception ex)
            {
                Logs.ExceptionErr(ex);
                return;
            }
        }

        public static void Screen__Stds(System.Windows.Forms.PictureBox pic, string Std)
        {
            if (old_Std == Std) { return; }
            old_Std = Std;

            try
            {
                Bitmap bmp = new Bitmap(Properties.Resources.STD);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    RectangleF drawRect = new RectangleF(10, 15, 484, 57);
                    H2Y.Draw__String(g, 40, Std, Color.White, drawRect, StringAlignment.Center);

                    g.Dispose();
                }

                pic.Image = bmp;
            }
            catch (Exception ex)
            {
                Logs.ExceptionErr(ex);
                return;
            }
        }
        public static void Screen__Stds(System.Windows.Forms.PictureBox pic, string Std, Color pColor)
        {
            if (old_Std == Std && std_Color == pColor) { return; }
            old_Std = Std; std_Color = pColor;

            try
            {
                Bitmap bmp = new Bitmap(Properties.Resources.STD);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    RectangleF drawRect = new RectangleF(10, 15, 484, 57);
                    H2Y.Draw__String(g, 40, Std, pColor, drawRect, StringAlignment.Center);

                    g.Dispose();
                }

                pic.Image = bmp;
            }
            catch (Exception ex)
            {
                Logs.ExceptionErr(ex);
                return;
            }
        }
        public static void Screen__Stds(System.Windows.Forms.PictureBox pic, string Std, string Pan)
        {
            if (old_Std == Std && old_Pan == Pan) { return; }
            old_Std = Std; old_Pan = Pan;

            try
            {
                Bitmap bmp = new Bitmap(Properties.Resources.STD);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    RectangleF drawRect = new RectangleF(10, 15, 484, 57);
                    if (Pan == "OK")
                    {
                        H2Y.Draw__String(g, 40, Std, Color.Lime, drawRect, StringAlignment.Center);
                    }
                    else
                    {
                        H2Y.Draw__String(g, 40, Std, Color.Red, drawRect, StringAlignment.Center);
                    }

                    g.Dispose();
                }

                pic.Image = bmp;
            }
            catch (Exception ex)
            {
                Logs.ExceptionErr(ex);
                return;
            }
        }

        public static void Message_Show(System.Windows.Forms.PictureBox pic, string msg)
        {
            if (old_msg == msg) { return; }
            old_msg = msg;

            try
            {
                Bitmap bmp = new Bitmap(Properties.Resources.Message);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    RectangleF drawRect;
                    drawRect = new RectangleF(12f, 5f, 1000f, 95F);
                    Draw__String(g, 70, msg, Color.White, drawRect, StringAlignment.Center);

                    g.Dispose();
                }

                pic.Image = bmp;
            }
            catch (Exception ex)
            {
                Logs.ExceptionErr(ex);
                return;
            }
        }
        public static void Message_Show(System.Windows.Forms.PictureBox pic, string msg, Color color)
        {
            if (old_msg == msg && msg_color == color) { return; }
            old_msg = msg; msg_color = color;

            try
            {
                Bitmap bmp = new Bitmap(Properties.Resources.Message);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    RectangleF drawRect;
                    drawRect = new RectangleF(12f, 10f, 1000f, 95F);
                    Draw__String(g, 70, msg, color, drawRect, StringAlignment.Center);

                    g.Dispose();
                }

                pic.Image = bmp;
            }
            catch (Exception ex)
            {
                Logs.ExceptionErr(ex);
                return;
            }
        }

        public static void Draw__String(Graphics g, float fontSize, string str, Color color, RectangleF rect, StringAlignment Align)
        {
            try
            {
                Font drawFont = new Font("굴림", fontSize, FontStyle.Bold);
                SolidBrush drawBrush = new SolidBrush(color);
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = Align;

                g.DrawString(str, drawFont, drawBrush, rect, drawFormat);
            }
            catch (Exception ex)
            {
                Logs.ExceptionErr(ex);
            }
        }
        public static void DrawFillRect(Graphics g, SolidBrush brush, float X, float Y, float W, float H)
        {
            try
            {
                g.FillRectangle(brush, X, Y, W, H);
            }
            catch (Exception ex)
            {
                Logs.ExceptionErr(ex);
            }
        }
        #endregion

        #region Normal Brake
        public static float Sum_Val(float Val1, float Val2)
        {
            float sum = Val1 + Val2;

            return sum;
        }
        public static double Sum_Val(double Val1, double Val2)
        {
            double sum = Val1 + Val2;

            return sum;
        }
        public static float Sum_Pst(float Val1, float Val2, float Wgt)
        {
            float sumP = DVD(Val1 + Val2, Wgt) * 100;

            return sumP;
        }
        public static double Sum_Pst(double Val1, double Val2, double Wgt)
        {
            double sumP = DVD(Val1 + Val2, Wgt) * 100;

            return sumP;
        }
        #endregion

        public static bool Ping_Test(string IP_Adr)
        {
            string now = DateTime.Now.ToString(H2Y.format3Time);

            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();

                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                options.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;

                PingReply reply = pingSender.Send(IP_Adr, timeout, buffer, options);

                if (reply.Status == IPStatus.Success) //핑이 제대로 들어가고 있을 경우
                {
                    string line_Msg = "";

                    line_Msg += reply.Address.ToString() + "의 응답:";
                    line_Msg += " 바이트=" + reply.Buffer.Length;
                    line_Msg += " 시간=" + reply.RoundtripTime + "ms";
                    line_Msg += " TTL=" + reply.Options.Ttl;
                    //lst_Ping.Items.Add(line_Msg);
                    //Logs.MakeLog_File(Log_His.Ping, line_Msg);
                    //lst_Logs.Items.Insert(0, "[" + now + "] " + line_Msg);

                    return true;
                }
                else //핑이 제대로 들어가지 않고 있을 경우 
                {
                    //lst_Ping.Items.Add(reply.Status);
                    //Logs.MakeLog_File(Log_His.Ping, reply.Status.ToString());
                    //lst_Logs.Items.Insert(0, "[" + now + "] " + reply.Status.ToString());

                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool SerialP_Scan(string port)
        {
            string[] arr_Port;
            arr_Port = System.IO.Ports.SerialPort.GetPortNames();

            for (int i = arr_Port.Length - 1; i >= 0; i--)
            {
                if (port == arr_Port[i]) return true;
            }

            return false;
        }

        static Thread thread;
        public static void Msg_Speash(string Msg)
        {
            if (Msg.Trim() == "") { return; }
            if (PSet.KISpeach == 0) { return; }
            if (old_Msgs == Msg) { return; }
            old_Msgs = Msg;

            //Speash_Run(Msg);

            if (thread != null) thread.Abort();
            thread = new Thread(new ParameterizedThreadStart(Speash_Run));
            thread.Start(Msg);
        }
        private static void Speash_Run(object Msg)
        {
            KI_Speech KI_Speek = new KI_Speech();

            try
            {
                KI_Speek.Speech(Msg.ToString());
            }
            catch (Exception ex)
            {
            }
            if (thread != null) thread.Abort();
        }
        public static bool Question(string pMsg, string pTitle)
        {
            var ret = MessageBoxEx.Show(pMsg, pTitle,
                System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

            if (ret == System.Windows.Forms.DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int toInt(string pStr)
        {
            try
            {
                if (pStr == "")
                {
                    return 0;
                }
                else
                {
                    return int.Parse(pStr);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static double toDbl(string pStr)
        {
            try
            {
                if (pStr == "")
                {
                    return 0;
                }
                else
                {
                    return double.Parse(pStr);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static float toFloat(string pStr)
        {
            try
            {
                if (pStr == "")
                {
                    return 0;
                }
                else
                {
                    return float.Parse(pStr);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static string Ret_JudgeOX(double pMin, double pMax, double pVal)
        {
            string Ret_Val = "";

            if (pMin <= pVal && pVal <= pMax)
            {
                Ret_Val = H2Y.OK;
            }
            else
            {
                Ret_Val = H2Y.NG;
            }

            return Ret_Val;
        }

        public static System.Drawing.Color Col_JudgeOX(string pPan)
        {
            if (pPan == H2Y.OK)
            {
                return System.Drawing.Color.Black;
            }
            else
            {
                return System.Drawing.Color.Red;
            }
        }
        public static System.Drawing.Color Col_JudgeOX(double pMin, double pMax, double pVal)
        {
            if (pMin <= pVal && pVal <= pMax)
            {
                return System.Drawing.Color.Black;
            }
            else
            {
                return System.Drawing.Color.Red;
            }
        }

        public static string Ret_Balance(double Val1, double Val2)
        {
            if (Val1 <= -1 || Val2 <= -1 || (Val1 + Val2) == 0)
            {
                return "";
            }

            Single Ret_Val = Convert.ToSingle(Val1 / (Val1 + Val2)) * 100;
            return Ret_Val.ToString("#0.0");
        }
        public static double Dbl_Balance(double Val1, double Val2)
        {
            if (Val1 == 0 || Val2 == 0 || (Val1 + Val2) == 0) { return 0; }

            double Ret_Val = (Val1 / (Val1 + Val2)) * 100;

            return toDbl(Ret_Val.ToString("#0.00"));
        }
        public static double Dbl_Balance2(double Val2, double Val1)
        {
            if (Val1 == 0 || Val2 == 0 || (Val1 + Val2) == 0) { return 0; }

            double Ret_Val = (Val1 / (Val1 + Val2)) * 100;

            return toDbl(Ret_Val.ToString("#0.00"));
        }

        public static Single DVD(Single Val1, Single Val2)
        {
            if ((Val1 == 0) || (Val2 == 0))
            {
                return 0;
            }
            else
            {
                return Val1 / Val2;
            }
        }
        public static Double DVD(Double Val1, Double Val2)
        {
            if ((Val1 == 0) || (Val2 == 0))
            {
                return 0;
            }
            else
            {
                return Val1 / Val2;
            }
        }

        public static void Sleep(Single mSec)
        {
            long WaitTime = DateTime.Now.AddMilliseconds(mSec).Ticks;

            do
            {
                System.Windows.Forms.Application.DoEvents();

            } while (WaitTime - DateTime.Now.Ticks > 0);
        }

        public static Single StrToSingles(string pValue)
        {
            if ((pValue == null) || (pValue == "")) return 0;

            return Convert.ToSingle(pValue);
        }
        public static Int32 StrToInt(string pValue)
        {
            if ((pValue == null) || (pValue == "")) return 0;

            return Convert.ToInt32(pValue);
        }

        public static void BitArray()
        {
            BitA[0] = 1;
            for (int i = 1; i < BitA.Length; i++)
            {
                BitA[i] = BitA[i - 1] * 2;
            }
        }

        public static double Ret_ForceKgf(double stt, double end, double time, double dia, double moment)
        {
            //Speed = Math.Abs(((Dia * Math.PI) * RPM * 60) / 1000 / 1000);
            double speed = ((dia * Math.PI) * 60) / 1000 / 1000;
            double RPM_1 = (stt / speed) * 2 * Math.PI / 60;
            double RPM_2 = (end / speed) * 2 * Math.PI / 60;
            double force = ((RPM_1 - RPM_2) * moment) / (time * (dia / 2));
            double kgf__ = force / 1000 / PSet.NewTGain;

            return Math.Abs(kgf__);
        }

        #region Filter 
        public static double QSoAv3Filter(int cnt, double put, Queue<double> sort)
        {
            int Idx = 0;
            double sum = 0;

            sort.Enqueue(put);

            if (sort.Count > cnt) { sort.Dequeue(); }

            if (cnt < 3) return put;

            double[] Arr = new double[cnt];

            sort.CopyTo(Arr, 0); Array.Sort(Arr);

            Idx = ((cnt - 1) / 2);
            sum = Arr.ElementAt<double>(Idx - 1) + Arr.ElementAt<double>(Idx) + Arr.ElementAt<double>(Idx + 1);
            return sum / 3;
        }
        public static double QSoAv5Filter(int cnt, double put, Queue<double> sort)
        {
            int Idx = 0;
            double sum = 0;

            sort.Enqueue(put);

            if (sort.Count > cnt) { sort.Dequeue(); }

            if (cnt < 5) return put;

            double[] Arr = new double[cnt];

            sort.CopyTo(Arr, 0); Array.Sort(Arr);

            Idx = ((cnt - 1) / 2);
            sum = Arr.ElementAt<double>(Idx - 2) + Arr.ElementAt<double>(Idx - 1) + Arr.ElementAt<double>(Idx) + Arr.ElementAt<double>(Idx + 1) + Arr.ElementAt<double>(Idx + 2);
            return sum / 5;
        }
        
        public static long QSoAv3Filter(int cnt, long put, Queue<long> sort)
        {
            int Idx = 0;
            long sum = 0;

            sort.Enqueue(put);

            if (sort.Count > cnt) { sort.Dequeue(); }

            long[] Arr = new long[cnt];

            sort.CopyTo(Arr, 0); Array.Sort(Arr);

            Idx = ((cnt - 1) / 2);
            sum = Arr.ElementAt<long>(Idx - 1) + Arr.ElementAt<long>(Idx) + Arr.ElementAt<long>(Idx + 1);

            return Convert.ToInt64(sum / 3);
        }
        public static long QSoAv5Filter(int cnt, long put, Queue<long> sort)
        {
            int Idx = 0;
            long sum = 0;

            sort.Enqueue(put);

            if (sort.Count > cnt) { sort.Dequeue(); }

            long[] Arr = new long[cnt];

            sort.CopyTo(Arr, 0); Array.Sort(Arr);

            Idx = ((cnt - 1) / 2);
            sum = Arr.ElementAt<long>(Idx - 2) + Arr.ElementAt<long>(Idx - 1) + Arr.ElementAt<long>(Idx) + Arr.ElementAt<long>(Idx + 1) + Arr.ElementAt<long>(Idx + 2) ;

            return Convert.ToInt64(sum / 5);
        }

        public static double QSort_Filter(int cnt, double put, Queue<double> sort)
        {
            int Idx = 0;

            sort.Enqueue(put);

            if (sort.Count > cnt) { sort.Dequeue(); }

            double[] Arr = new double[cnt];

            sort.CopyTo(Arr, 0); Array.Sort(Arr);

            Idx = ((cnt - 1) / 2);

            return Arr.ElementAt<double>(Idx);
        }
        public static long QSort_Filter(int cnt, long put, Queue<long> sort)
        {
            int Idx = 0;

            sort.Enqueue(put);

            if (sort.Count > cnt) { sort.Dequeue(); }

            long[] Arr = new long[cnt];

            sort.CopyTo(Arr, 0); Array.Sort(Arr);

            Idx = ((cnt - 1) / 2);

            return Arr.ElementAt<long>(Idx);
        }

        public static double QAver_Filter(int cnt, double put, Queue<double> Aver)
        {
            Aver.Enqueue(put);

            if (Aver.Count > cnt)
            {
                Aver.Dequeue();
            }
            else
            {
                return put;
            }
            return Aver.Average();
        }
        public static long QAver_Filter(int cnt, long put, Queue<long> Aver)
        {
            Aver.Enqueue(put);

            if (Aver.Count > cnt)
            {
                Aver.Dequeue();
            }
            else
            {
                return put;
            }
            return Convert.ToInt64(Aver.Average());
        }

        public static double Sort_Filter(int sort, double Av, double[] Arr)
        {
            if (sort < 3) return Av;

            for (int cnt = 0; cnt < sort - 1; cnt++)
            {
                Arr[cnt] = Arr[cnt + 1];
            }
            Arr[PSet.St_Filt - 1] = Av;

            Array.Reverse(Arr);

            return Arr[(sort + 1) / 2];
        }
        public static long Sort_Filter(int sort, long Av, long[] Arr)
        {
            if (sort < 3) return Av;

            for (int cnt = 0; cnt < sort - 1; cnt++)
            {
                Arr[cnt] = Arr[cnt + 1];
            }
            Arr[PSet.St_Filt - 1] = Av;

            Array.Reverse(Arr);

            return Arr[(sort + 1) / 2];
        }
        public static int Sort_Filter(int sort, int Av, int[] Arr)
        {
            if (PSet.St_Filt < 3) return Av;

            for (int cnt = 0; cnt < PSet.St_Filt - 1; cnt++)
            {
                Arr[cnt] = Arr[cnt + 1];
            }
            Arr[PSet.St_Filt - 1] = Av;

            Array.Reverse(Arr);
            //Array.Sort(Arr);

            return Arr[Arr.Length - (PSet.St_Filt + 1) / 2];
        }
        #endregion

        public static string HexToASCII(string pHex)
        {
            if ((pHex == "") || (pHex == null)) pHex = "";
            try
            {
                byte bt = byte.Parse(pHex, System.Globalization.NumberStyles.HexNumber);

                return Convert.ToString(Convert.ToChar(bt));
            }
            catch
            {
                return "";
            }
        }
        public static string HexToBinary(string pHex)
        {
            if ((pHex == "") || (pHex == null)) pHex = "";
            try
            {
                byte bt = byte.Parse(pHex, System.Globalization.NumberStyles.HexNumber);
                string bin = Convert.ToString(bt, 2);
                string ret_Bin = bin;

                for (int cnt = bin.Length; cnt < 8; cnt++)
                {
                    ret_Bin = "0" + ret_Bin;
                }
                
                return ret_Bin.ToString();
            }
            catch
            {
                return "";
            }
        }
        public static string HexToBinary(string pHex, byte Target, byte Len)
        {
            string strBinary = HexToBinary(pHex);

            for (int cnt = strBinary.Length; cnt <= 8; cnt++)
            {
                strBinary = "0" + strBinary;
            }

            return strBinary.Substring(Target, Len);
        }
        public static byte HexTobyte(string pHex)
        {
            if ((pHex == "") || (pHex == null)) pHex = "-1";
            try
            {
                byte RetByte = byte.Parse(pHex, System.Globalization.NumberStyles.HexNumber);

                return RetByte;
            }
            catch
            {
                return 0;
            }
        }
        public static int HexToInt(string pHex)
        {
            if ((pHex == "") || (pHex == null)) pHex = "-1";
            try
            {
                int RetByte = int.Parse(pHex, System.Globalization.NumberStyles.HexNumber);

                return RetByte;
            }
            catch
            {
                return 0;
            }
        }
        public static int HexToInt(string Hex1, string Hex2)
        {
            if ((Hex1 == "") || (Hex2 == null)) Hex1 = "-1";
            try
            {
                int Int1 = byte.Parse(Hex1, System.Globalization.NumberStyles.HexNumber);
                int Int2 = byte.Parse(Hex2, System.Globalization.NumberStyles.HexNumber);

                return (Int1 * 256) + Int2;
            }
            catch
            {
                return 0;
            }
        }
        public static uint HexToUInt(string Hex1, string Hex2)
        {
            if ((Hex1 == "") || (Hex2 == null)) Hex1 = "-1";
            try
            {
                uint Int1 = byte.Parse(Hex1, System.Globalization.NumberStyles.HexNumber);
                uint Int2 = byte.Parse(Hex2, System.Globalization.NumberStyles.HexNumber);

                return (Int1 * 256) + Int2;
            }
            catch
            {
                return 0;
            }
        }
        public static uint HexToUInt(string Hex1, string Hex2, string Hex3, string Hex4)
        {
            if ((Hex1 == "") || (Hex2 == null) || (Hex3 == null) || (Hex4 == null)) Hex1 = "-1";
            try
            {
                uint Int1 = byte.Parse(Hex1, System.Globalization.NumberStyles.HexNumber);
                uint Int2 = byte.Parse(Hex2, System.Globalization.NumberStyles.HexNumber);
                uint Int3 = byte.Parse(Hex3, System.Globalization.NumberStyles.HexNumber);
                uint Int4 = byte.Parse(Hex4, System.Globalization.NumberStyles.HexNumber);

                return (Int1 << 24) + (Int2 << 16) + (Int3 << 8) + Int4;
            }
            catch
            {
                return 0;
            }
        }

        public static string DigitToStr(string Str, int Digit)
        {
            string strValue = Str;

            for (int cnt = Str.Length; cnt < Digit; cnt++)
            {
                strValue = " " + strValue;
            }

            return strValue;
        }
        
        public static string Sql_Insert(string pData)
        {
            string[] arr_Data = pData.Split(',');
            string Ret_Data = "";

            for (int cnt = 0; cnt < arr_Data.Length - 1; cnt++)
            {
                Ret_Data += "'{" + cnt.ToString() + "}', ";
            }

            Ret_Data += "'{" + (arr_Data.Length - 1).ToString() + "}' ";
            return Ret_Data;
        }

        public static string Sql_Update(string pData)
        {
            string[] arr_Data = pData.Split(',');
            string Ret_Data = "";

            for (int cnt = 0; cnt < arr_Data.Length - 1; cnt++)
            {
                Ret_Data += arr_Data[cnt] + "='{" + cnt.ToString() + "}', ";
            }

            Ret_Data += arr_Data[arr_Data.Length - 1] + "='{" + (arr_Data.Length - 1).ToString() + "}' ";
            return Ret_Data;    //JSNO='{0}'
        }

        public static void Make_Dir(string pPath)
        {
            if (System.IO.Directory.Exists(pPath) == false)
            {
                System.IO.Directory.CreateDirectory(pPath);
            }
        }

        public static void Delete_File(string pFile)
        {
            if (System.IO.File.Exists(pFile))
            {
                System.IO.File.Delete(pFile);
            }
        }

        public static string ret_Error(System.Net.Sockets.SocketException se_Err)
        {
            string strError = "";
            switch (se_Err.ErrorCode)
            {
                case 6: strError = "지정된 이벤트 오브젝트 핸들이 유효하지 않습니다."; break;
                case 8: strError = "사용 가능한 메모리가 부족합니다."; break;
                case 87: strError = "하나 이상의 매개 변수가 유효하지 않습니다."; break;
                case 995: strError = "겹쳐진 작업이 중단되었습니다."; break;
                case 996: strError = "신호가있는 상태가 아닌 겹친 I / O 이벤트 객체입니다."; break;
                case 997: strError = "겹친 작업은 나중에 완료됩니다."; break;
                case 10004: strError = "중단 된 함수 호출."; break;
                case 10009: strError = "파일 핸들이 유효하지 않습니다."; break;
                case 10013: strError = "허가가 거부되었습니다."; break;
                case 10014: strError = "잘못된 주소."; break;
                case 10022: strError = "인수가 잘못되었습니다."; break;
                case 10024: strError = "열려있는 파일이 너무 많습니다."; break;
                case 10035: strError = "자원을 일시적으로 사용할 수 없습니다."; break;
                case 10036: strError = "작업이 현재 진행 중입니다."; break;
                case 10037: strError = "작업이 이미 진행 중입니다."; break;
                case 10038: strError = "소켓이 아닌 소켓에서의 조작."; break;
                case 10039: strError = "목적지 주소가 필요합니다."; break;
                case 10040: strError = "메시지가 너무 깁니다."; break;
                case 10041: strError = "프로토콜에 잘못된 소켓 유형이 있습니다."; break;
                case 10042: strError = "잘못된 프로토콜 옵션입니다."; break;
                case 10043: strError = "프로토콜이 지원되지 않습니다."; break;
                case 10044: strError = "소켓 유형이 지원되지 않습니다."; break;
                case 10045: strError = "작동이 지원되지 않습니다."; break;
                case 10046: strError = "프로토콜 패밀리가 지원되지 않습니다."; break;
                case 10047: strError = "프로토콜 패밀리가 지원하지 않는 주소 패밀리."; break;
                case 10048: strError = "주소가 이미 사용 중입니다."; break;
                case 10049: strError = "요청한 주소를 할당 할 수 없습니다."; break;
                case 10050: strError = "네트워크가 다운되었습니다."; break;
                case 10051: strError = "네트워크에 연결할 수 없습니다."; break;
                case 10052: strError = "재설정시 네트워크 연결 끊김."; break;
                case 10053: strError = "소프트웨어로 인해 연결이 중단되었습니다."; break;
                case 10054: strError = "피어의 연결 재설정."; break;
                case 10055: strError = "사용 가능한 버퍼 공간이 없습니다."; break;
                case 10056: strError = "소켓이 이미 연결되었습니다."; break;
                case 10057: strError = "소켓이 연결되어 있지 않습니다."; break;
                case 10058: strError = "소켓 종료 후 보낼 수 없습니다."; break;
                case 10059: strError = "참조가 너무 많습니다."; break;
                case 10060: strError = "연결 시간이 초과되었습니다."; break;
                case 10061: strError = "연결이 거부되었습니다."; break;
                case 10062: strError = "이름을 번역 할 수 없습니다."; break;
                case 10063: strError = "이름이 너무 깁니다."; break;
                case 10064: strError = "호스트가 다운되었습니다."; break;
                case 10065: strError = "접속 대상으로가는 길이 없음."; break;
                case 10066: strError = "디렉토리가 비어 있지 않습니다."; break;
                case 10067: strError = "프로세스가 너무 많습니다."; break;
                case 10068: strError = "사용자 할당량이 초과되었습니다."; break;
                case 10069: strError = "디스크 할당량 초과."; break;
                case 10070: strError = "부실 파일 핸들 참조."; break;
                case 10071: strError = "항목이 원격입니다."; break;
                case 10091: strError = "네트워크 하위 시스템을 사용할 수 없습니다."; break;
                case 10092: strError = "Winsock.dll 버전이 범위를 벗어났습니다."; break;
                case 10093: strError = "성공적인 WSAStartup은 아직 수행되지 않았습니다."; break;
                case 10101: strError = "정상적인 종료가 진행 중입니다."; break;
                case 10102: strError = "더 이상 결과가 없습니다."; break;
                case 10103: strError = "통화가 취소되었습니다."; break;
                case 10104: strError = "프로 시저 호출 테이블이 유효하지 않습니다."; break;
                case 10105: strError = "서비스 제공 업체가 잘못되었습니다."; break;
                case 10106: strError = "서비스 공급자를 초기화하지 못했습니다."; break;
                case 10107: strError = "시스템 호출 실패."; break;
                case 10108: strError = "서비스를 찾을 수 없습니다."; break;
                case 10109: strError = "클래스 유형을 찾을 수 없습니다."; break;
                case 10110: strError = "더 이상 결과가 없습니다."; break;
                case 10111: strError = "통화가 취소되었습니다."; break;
                case 10112: strError = "데이터베이스 쿼리가 거부되었습니다."; break;
                case 11001: strError = "호스트를 찾을 수 없습니다."; break;
                case 11002: strError = "신뢰할 수없는 호스트를 찾을 수 없습니다."; break;
                case 11003: strError = "이는 복구 할 수없는 오류입니다."; break;
                case 11004: strError = "유효한 이름이며 요청한 유형의 데이터 레코드가 없습니다."; break;
                case 11005: strError = "QoS 수신기."; break;
                case 11006: strError = "QoS 발신자."; break;
                case 11007: strError = "QoS 발신자가 없습니다."; break;
                case 11008: strError = "QoS 수신기가 없습니다."; break;
                case 11009: strError = "QoS 요청이 확인되었습니다."; break;
                case 11010: strError = "QoS 허용 오류입니다."; break;
                case 11011: strError = "QoS 정책 실패."; break;
                case 11012: strError = "QoS 나쁜 스타일."; break;
                case 11013: strError = "QoS 나쁜 객체."; break;
                case 11014: strError = "QoS 트래픽 제어 오류."; break;
                case 11015: strError = "QoS 일반 오류입니다."; break;
                case 11016: strError = "QoS 서비스 유형 오류입니다."; break;
                case 11017: strError = "QoS flowspec 오류입니다."; break;
                case 11018: strError = "QoS 공급자 버퍼가 잘못되었습니다."; break;
                case 11019: strError = "QoS 필터 스타일이 잘못되었습니다."; break;
                case 11020: strError = "잘못된 QoS 필터 유형입니다."; break;
                case 11021: strError = "잘못된 QoS 필터 수."; break;
                case 11022: strError = "QoS 개체 길이가 잘못되었습니다."; break;
                case 11023: strError = "잘못된 QoS 흐름 수."; break;
                case 11024: strError = "인식 할 수없는 QoS 개체입니다."; break;
                case 11025: strError = "잘못된 QoS 정책 개체입니다."; break;
                case 11026: strError = "잘못된 QoS 흐름 설명자."; break;
                case 11027: strError = "잘못된 QoS 공급자 별 flowspec입니다."; break;
                case 11028: strError = "잘못된 QoS 공급자 별 filterspec입니다."; break;
                case 11029: strError = "잘못된 QoS 모양 삭제 모드 개체입니다."; break;
                case 11030: strError = "잘못된 QoS 셰이핑 속도 개체입니다."; break;
                case 11031: strError = "예약 된 정책 QoS 요소 유형입니다."; break;
                default: strError = se_Err.ToString(); break;
            }

            return strError;
        }
    }
}