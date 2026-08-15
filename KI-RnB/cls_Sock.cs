using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace KI_RnB
{
    class cls_Sock
    {
        public Thread thread;
        public Socket socket;

        public int Length { get; set; }
        public string Buffer { get; set; }

        private fom_Main main;
        private List<cls_Sock> socklist;
        private byte[] rec_Data;

        public cls_Sock(fom_Main Main)
        {
            this.main = Main;
        }

        public cls_Sock(fom_Main Main, Socket Sock, List<cls_Sock> SockList)
        {
            this.main = Main;
            this.socket = Sock;
            this.socklist = SockList;

            thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }

        private void Run()
        {
            rec_Data = new byte[100000];

            while (thread.IsAlive)
            {
                try
                {
                    Length = socket.Receive(rec_Data, rec_Data.Length, SocketFlags.None);
                    Buffer = Encoding.UTF8.GetString(rec_Data, 0, Length);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
    }
}
