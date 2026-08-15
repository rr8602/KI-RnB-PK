using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace KI_RnB
{

    public class Chery_TestPresentThread
    {
        private Thread Thread_ECHO;

        private bool m_bSendECHO = false ;
        private bool m_bEndProgram = false;

        public void SendEcho(bool bSend)
        {
            //ECUs.Start_Communication();
            //Thread.Sleep(300);
            m_bSendECHO = bSend ;

        }
        public void Terminate()
        {
            m_bEndProgram = true;
        }

        
        public void StartThread()
        {
            Thread_ECHO = new Thread(ThreadEcho);
            Thread_ECHO.Start();
        }

        public void ThreadEcho()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(500);
                    if (m_bEndProgram == true) break;
                    if (m_bSendECHO == false) continue;
                    

                    ECUs.Tester_Present();

                }
                
            }
            catch (Exception e)
            { }

            Console.WriteLine("");
        }

    }
}
