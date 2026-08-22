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
    public static class NeoVI
    {
        private static StringBuilder Get_Buf = new StringBuilder();
        private static readonly object TxLock = new object();
        private static IntPtr m_hObject;		 //handle for device
        private static OptionsNeoEx neoDeviceOption = new OptionsNeoEx();
        private static int iOpenDeviceType; //Storage for the device type that is open
        private static icsSpyMessage[] stMessages = new icsSpyMessage[20000];   //TempSpace for messages
        //private static string FF_0, CF_1, CF_2, CF_3, CF_4, CF_5, CF_6, CF_7, CF_8, CF_9, CF_A, CF_B, CF_C, CF_D, CF_E, CF_F;
        private static string Put_Time, Put_Head, Put_Olds;
        private static string Get_Time, Get_Head, Get_Olds;
        private static Thread thread_NeoVI;
        private static string Thread_Data;

        public static fom_Main Main = null;

        public static bool IsOpen;
        public static bool Is_Log;
        public static bool IsRead;

        public static string Put_Data;
        public static string Get_Data;
        public static int Read_Count;
        public static int Read_Error;
        public static int CAN_Len;

        public static string Send_Key;
        public static string SerialNo;
        public static bool Return;

        public static int tmr__CNT = 0;
        public static int tmrCount = 10;
        public static System.Windows.Forms.Timer tmr_Wait;

        private static void tmr_Wait_Tick(object sender, EventArgs e)
        {
            tmr__CNT++;

            if (tmr__CNT > tmrCount || tmr__CNT > 30)
            {
                tmr__CNT = 0;
                tmr_Wait.Enabled = false;
            }

            if (IsOpen)
            {
                if (!IsRead) Device_Read();
            }
            else
            {
                tmr_Wait.Enabled = false;
            }

            if (Return) tmr_Wait.Enabled = false;
        }
        public static void ECU_Clear()
        {
            IsRead = false;
            Send_Key = "";
            CAN_Len = 0;                                                  // 추가
            Put_Time = ""; Put_Head = ""; Put_Data = ""; Put_Olds = "";
            Get_Time = ""; Get_Head = ""; Get_Data = ""; Get_Olds = "";
            Get_Buf.Length = 0;                                            // FF_0 / CF_1~CF_F 대체
        }

        public static void Setting(fom_Main main)
        {
            Main = main;

            Device_Open();

            tmr_Wait = new System.Windows.Forms.Timer();
            tmr_Wait.Interval = 100;
            tmr_Wait.Enabled = false;
            tmr_Wait.Tick += new EventHandler(tmr_Wait_Tick);
        }
        public static void Debug_Message(string pMsg)
        {
            if (PSet.OnfDebug)
            {
                Main.FomDebug.ECUs_Message(pMsg);
            }
        }

        public static bool Close()
        {
            Device_Close();
            return NeoVI.IsOpen;
        }       
        public static String Device_Time(double dTime)
        {
            long lTime = Convert.ToInt64(dTime);
            double bTime = dTime - lTime;
            DateTime dt = new DateTime(1970, 1, 1, 9, 0, 0); // 한국은 GMT+9시간
            dt = dt.AddSeconds(lTime);
            dt = dt.AddYears(2007 - 1970);
            dt = dt.AddMilliseconds(bTime * 1000);
            
            return dt.ToString("HH:mm:ss.fff");
        }
        //public static bool Device_Set()
        //{
        //    int iBitRateToUse;
        //    int iNetworkID = 0;
        //    int iResult;
        //    int CAN_CH = 0;

        //    //Get the network name index to set the baud rate of 
        //    switch (CAN_CH)
        //    {
        //        case 0: iNetworkID = (int)eNETWORK_ID.NETID_HSCAN; break; //HS CAN
        //        case 1: iNetworkID = (int)eNETWORK_ID.NETID_MSCAN; break; //MS CAN
        //        case 2: iNetworkID = (int)eNETWORK_ID.NETID_SWCAN; break; //SW CAN
        //        case 3: iNetworkID = (int)eNETWORK_ID.NETID_LSFTCAN; break; //LSFT CAN
        //        case 4: iNetworkID = (int)eNETWORK_ID.NETID_HSCAN2; break; //HS CAN2
        //        case 5: iNetworkID = (int)eNETWORK_ID.NETID_HSCAN3; break; //HS CAN3
        //    }
        //    iBitRateToUse = 50000;

        //    //Set the bit rate
        //    iResult = icsNeoDll.icsneoSetBitRate(m_hObject, iBitRateToUse, iNetworkID);
        //    if (iResult != 1)
        //    {
        //        MessageBox.Show("Problem setting bit rate");
        //        return false;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Bit Rate Set");
        //        return true;
        //    }
        //}
        public static bool Device_Open()
        {
            if (IsOpen == true) { return true; }

            int iResult;
            NeoDeviceEx[] ndNeoToOpenex = new NeoDeviceEx[16];	//Struct holding detected hardware information
            NeoDevice ndNeoToOpen;
            byte[] bNetwork = new byte[255];                    //List of hardware IDs
            int iNumberOfDevices;                               //Number of hardware devices to look for 
            int iCount;		                                    //counter
            UInt32 StringSize = 6;
            string sConvertedSN = "      ";
            byte[] bSN = new byte[6];

            //File NetworkID array
            for (iCount = 0; iCount < 255; iCount++)
            {
                bNetwork[iCount] = Convert.ToByte(iCount);
            }

            //Set the number of devices to find, for this example look for 16.  This example will only work with the first.
            iNumberOfDevices = 15;

            //Search for connected hardware
            //iResult = icsNeoDll.icsneoFindNeoDevices((uint)eHardwareTypes.NEODEVICE_ALL, ref ndNeoToOpen, ref iNumberOfDevices);
            iResult = icsNeoDll.icsneoFindDevices(ref ndNeoToOpenex[0], ref iNumberOfDevices, 0, 0, ref neoDeviceOption, 0);
            if (iResult == 0)
            {
                //MessageBox.Show("Problem finding devices");
                IsOpen = false;
                return false;
            }

            if (iNumberOfDevices < 1)
            {
                //MessageBox.Show("No devices found");
                IsOpen = false;
                return false; 
            }

            ndNeoToOpen = ndNeoToOpenex[0].neoDevice;
            //Open the first found device
            iResult = icsNeoDll.icsneoOpenNeoDevice(ref ndNeoToOpen, ref m_hObject, ref bNetwork[0], 1, 0);
            if (iResult == 1)
            {
                //MessageBox.Show("Port Opened OK!");
                IsOpen = true;
            }
            else
            {
                //MessageBox.Show("Problem Opening Port");
                IsOpen = false;
                return false;
            }

            //Set the device type for later use
            iOpenDeviceType = ndNeoToOpen.DeviceType;

            //display the connect hardware type
            switch (ndNeoToOpen.DeviceType)
            {
                case (int)eHardwareTypes.NEODEVICE_FIRE:
                    SerialNo = "neoVI FIRE SN " + Convert.ToString(ndNeoToOpen.SerialNumber);
                    break;

                case (int)eHardwareTypes.NEODEVICE_VCAN4_IND:
                    icsNeoDll.icsneoSerialNumberToString(Convert.ToUInt32(ndNeoToOpen.SerialNumber), ref bSN[0], ref StringSize);
                    sConvertedSN = System.Text.ASCIIEncoding.ASCII.GetString(bSN);
                    SerialNo = "ValueCAN4-IND SN " + sConvertedSN;
                    break;

                default:
                    SerialNo = "Unknown neoVI SN " + Convert.ToString(ndNeoToOpen.SerialNumber);
                    break;
            }

            Logs.ECUsLog_File(Log_ECU.IDSN, SerialNo);

            IsOpen = true;
            return true;
        }
        public static bool Device_Close()
        {
            try
            {
                int iNumberOfErrors = 0;
                int iResult = icsNeoDll.icsneoClosePort(m_hObject, ref iNumberOfErrors);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
            //if (iResult == 1)
            //{
            //    MessageBox.Show("Port Closed OK!");
            //}
            //else
            //{
            //    MessageBox.Show("Problem ClosingPort");
            //}

            IsOpen = false;
            return true;
        }

        public static bool Device_Read()
        {
            bool Ret = false;

            switch (ECUs.Comm)
            {
                case 0: Ret = KWP2000_Read(); break;
                case 1: Ret = STD_CAN_Read(); break;
                case 2: Ret = STD_CAN_Read(); break;
                case 3: Ret = STD_CAN_Read(); break;
                case 4: Ret = STD_CAN_Read(); break;
            }

            return Ret;
        }
        public static bool Device_Write(string Puts)
        {
            bool Ret = false;

            try
            {
                switch (ECUs.Comm)
                {
                    case 0: Ret = KWP2000_Write(Puts.Trim()); break;

                    case 1: //Ret = STD_CAN_Write(Puts.Trim()); break;
                            Thread_Data = Puts;
                            thread_NeoVI = new Thread(Thread_NeoVI);
                            thread_NeoVI.Start();
                            break;

                    case 2: //Ret = STD_CAN_Write(Puts.Trim()); break;
                            Thread_Data = Puts;
                            thread_NeoVI = new Thread(Thread_NeoVI);
                            thread_NeoVI.Start();
                            break;

                    case 3: //Ret = STD_CAN_Write(Puts.Trim()); break;
                            Thread_Data = Puts;
                            thread_NeoVI = new Thread(Thread_NeoVI);
                            thread_NeoVI.Start();
                            break;

                    case 4: //Ret = STD_CAN_Write(Puts.Trim()); break;
                            Thread_Data = Puts;
                            thread_NeoVI = new Thread(Thread_NeoVI);
                            thread_NeoVI.Start();
                            break;
                }
            }
            catch (Exception ex)
            {
                Logs.MakeLog_File(Log_His.Err_, "Device_Write: " + ex.Message);
            }

            return Ret;
        }

        public static void Thread_NeoVI()
        {
            bool Ret = false;

            try
            {
                switch (ECUs.Comm)
                {
                    case 0: Ret = KWP2000_Write(Thread_Data.Trim()); break;
                    case 1: Ret = STD_CAN_Write(Thread_Data.Trim()); break;
                    case 2: Ret = STD_CAN_Write(Thread_Data.Trim()); break;
                    case 3: Ret = CAN_FD_Write1(Thread_Data.Trim()); break;
                    case 4: Ret = CAN_FD_Write1(Thread_Data.Trim()); break;
                }

                thread_NeoVI.Abort();
            }
            catch (Exception ex)
            {
                //MessageBoxEx.Show(ex.Message);
                Logs.MakeLog_File(Log_His.Err_, "Thread_NeoVI: " + ex.Message);
            }
        }

        #region KWP2000 통신
        public static bool KWP2000_Read()
        {
            long lResult;
            int lNumberOfMessages = 0;
            int lNumberOfErrors = 0;
            long lCount;
            string Head_Str = "";
            string Data_Str = "";
            string Gets_Str;
            icsSpyMessageJ1850 stJMsg = new icsSpyMessageJ1850();
            double dTime = 0;
                        
            if (IsOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return false;  // do not read messages if we haven't opened neoVI yet
            }

            // read the messages from the driver
            lResult = icsNeoDll.icsneoGetMessages(m_hObject, ref stMessages[0], ref lNumberOfMessages, ref lNumberOfErrors);
            
            // was the read successful?
            if (lResult == 1)
            {
                // clear the previous list of 
                Read_Count = lNumberOfMessages;
                Read_Error = lNumberOfErrors;
                
                // for each message we read
                for (lCount = 1; lCount <= lNumberOfMessages; lCount++)
                {
                    IsRead = true;
                    #region Calculate the messages timestamp
                    lResult = icsNeoDll.icsneoGetTimeStampForMsg(m_hObject, ref stMessages[lCount - 1], ref dTime);

                    //Decode based on the protocol
                    if (stMessages[lCount - 1].Protocol == 5)
                    {
                        // list the headers bytes
                        icsNeoDll.ConvertCANtoJ1850Message(ref stMessages[lCount - 1], ref stJMsg);
                        Gets_Str =  "Network " + icsNeoDll.GetStringForNetworkID(stJMsg.NetworkID) + " Data : ";
                    }

                    // Was it a tx or rx message
                    if ((stMessages[lCount - 1].StatusBitField & Convert.ToInt32(eDATA_STATUS_BITFIELD_1.SPY_STATUS_TX_MSG)) > 0)
                    {
                        Put_Time = "[" + Device_Time(dTime) + "] Tx -> ";
                                                           Put_Head = ""; 
                        if (stJMsg.NumberBytesHeader >= 1) Put_Head += stJMsg.Header1.ToString("X2") + " ";
                        if (stJMsg.NumberBytesHeader >= 2) Put_Head += stJMsg.Header2.ToString("X2") + " ";
                        if (stJMsg.NumberBytesHeader >= 3) Put_Head += stJMsg.Header3.ToString("X2") + " ";
                        if (stJMsg.NumberBytesHeader >= 4) Put_Head += stJMsg.Header4.ToString("X2") + " ";
                                                         
                                                       //Put_Data = "";
                        if (stJMsg.NumberBytesData >= 1) Put_Data += stJMsg.Data1.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 2) Put_Data += stJMsg.Data2.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 3) Put_Data += stJMsg.Data3.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 4) Put_Data += stJMsg.Data4.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 5) Put_Data += stJMsg.Data5.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 6) Put_Data += stJMsg.Data6.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 7) Put_Data += stJMsg.Data7.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 8) Put_Data += stJMsg.Data8.ToString("X2") + " ";
                    }
                    else
                    {
                        Get_Time = "[" + Device_Time(dTime) + "] Rx <- ";
                                                           Get_Head = "";
                        if (stJMsg.NumberBytesHeader >= 1) Get_Head += stJMsg.Header1.ToString("X2") + " ";
                        if (stJMsg.NumberBytesHeader >= 2) Get_Head += stJMsg.Header2.ToString("X2") + " ";
                        if (stJMsg.NumberBytesHeader >= 3) Get_Head += stJMsg.Header3.ToString("X2") + " ";
                        if (stJMsg.NumberBytesHeader >= 4) Get_Head += stJMsg.Header4.ToString("X2") + " ";

                                                       //Get_Data = "";   
                        if (stJMsg.NumberBytesData >= 1) Get_Data += stJMsg.Data1.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 2) Get_Data += stJMsg.Data2.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 3) Get_Data += stJMsg.Data3.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 4) Get_Data += stJMsg.Data4.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 5) Get_Data += stJMsg.Data5.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 6) Get_Data += stJMsg.Data6.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 7) Get_Data += stJMsg.Data7.ToString("X2") + " ";
                        if (stJMsg.NumberBytesData >= 8) Get_Data += stJMsg.Data8.ToString("X2") + " ";
                    }

                    if (stMessages[lCount - 1].Protocol == (int)ePROTOCOL.SPY_PROTOCOL_LIN)
                    {
                        Head_Str += Head_Str + Data_Str + "CS=" + stJMsg.ACK1.ToString("X2");
                    }
                    #endregion
                }
                IsRead = false;
            }

            #region 데이터 처리
            if (lNumberOfMessages > 0)
            {
                if ((Put_Data != "") && (Put_Olds != Put_Data)) { Logs.ECUsLog_File(Log_ECU.Puts, Put_Time + Put_Head + "- " + Put_Data); }
                if ((Get_Data != "") && (Get_Olds != Get_Data)) { Logs.ECUsLog_File(Log_ECU.Gets, Get_Time + Get_Head + "- " + Get_Data); }

                if ((Get_Data != "") && (Put_Data != ""))
                {
                    if (Put_Data.IndexOf("14 FF 00") > -1)
                    {
                        int Ret_Cnt = Get_Data.IndexOf("03 54 FF 00");

                        if (Ret_Cnt > -1)
                        {
                            Get_Data = Get_Data.Substring(Ret_Cnt, (Get_Data.Length - Ret_Cnt));
                        }
                    }

                    string[] Puts = Put_Data.Trim().Split(' ');
                    string[] Head = Get_Head.Trim().Split(' ');
                    string[] GetD = Get_Data.Trim().Split(' ');

                    CAN_Len = Convert.ToInt32(GetD[0], 16);

                    int Tgt = 0;
                    int Count = 0;

                    if (Head[0] == "80")
                    {
                        Tgt = 1; Count = H2Y.HexTobyte(GetD[0]);
                    }
                    else
                    {
                        Tgt = 0; Count = H2Y.HexTobyte(Head[0]) - H2Y.HexTobyte("80");
                    }

                    if (GetD[Tgt] == "7F")
                    {
                        Return = false;
                    }
                    else
                    {
                        if (H2Y.HexTobyte(Puts[0]) + H2Y.HexTobyte("40") == H2Y.HexTobyte(GetD[Tgt]))
                        {
                            Return = true;
                        }
                        else
                        {
                            Return = false;
                        }
                    }

                    if (PSet.OnfDebug)
                    {
                        if ((Put_Data != "") && (Put_Olds != Put_Data)) { Main.FomDebug.ECUs_Message(Put_Time + Put_Head + "- " + Put_Data); Put_Olds = Put_Data; }
                        if ((Get_Data != "") && (Get_Olds != Get_Data)) { Main.FomDebug.ECUs_Message(Get_Time + Get_Head + "- " + Get_Data); Get_Olds = Get_Data; }

                        Main.FomDebug.Flag_Message(Return.ToString(), CAN_Len.ToString(), Ret_Length(Get_Data));
                    }
                }
                
                return true;
            }
            else
            {
                return false;
            }
            #endregion
        }
        public static bool KWP2000_Write(string Puts)
        {
            string[] PutD = Puts.Split(' ');
            string[] Data = new string[24];
            long lResult;
            icsSpyMessageJ1850 stMessagesTxJ1850 = new icsSpyMessageJ1850();
            string lNetworkNM = "ISO";
            long lNetworkID = icsNeoDll.GetNetworkIDfromString(ref lNetworkNM);

            //load the message structure
            //copy headder bytes
            //ECUs.Set_ID
            stMessagesTxJ1850.Header1 = Convert.ToByte(128 + PutD.Length);
            stMessagesTxJ1850.Header2 = Convert.ToByte(ECUs.Set_ID, 16);
            stMessagesTxJ1850.Header3 = Convert.ToByte(ECUs.Ret_ID, 16);
            stMessagesTxJ1850.NumberBytesHeader = 3;
            stMessagesTxJ1850.NumberBytesData = Convert.ToByte(PutD.Length);    // The number of Data Bytes

            // Load all of the data bytes in the structure
            for (int CNT = 0; CNT < 24; CNT++)
            {
                Data[CNT] = "00";
            }
            for (int CNT = 0; CNT < PutD.Length; CNT++)
            {
                Data[CNT] = PutD[CNT];
            }

            stMessagesTxJ1850.Data1 = Convert.ToByte(Data[0], 16);
            stMessagesTxJ1850.Data2 = Convert.ToByte(Data[1], 16);
            stMessagesTxJ1850.Data3 = Convert.ToByte(Data[2], 16);
            stMessagesTxJ1850.Data4 = Convert.ToByte(Data[3], 16);
            stMessagesTxJ1850.Data5 = Convert.ToByte(Data[4], 16);
            stMessagesTxJ1850.Data6 = Convert.ToByte(Data[5], 16);
            stMessagesTxJ1850.Data7 = Convert.ToByte(Data[6], 16);
            stMessagesTxJ1850.Data8 = Convert.ToByte(Data[7], 16);

            stMessagesTxJ1850.StatusBitField = Convert.ToInt32(eDATA_STATUS_BITFIELD_1.SPY_STATUS_INIT_MESSAGE);

            // Transmit the assembled message
            lResult = icsNeoDll.icsneoTxMessages(m_hObject, ref stMessagesTxJ1850, Convert.ToInt32(lNetworkID), 1);
            // Test the returned result
            if (lResult != 1)
            {
                //MessageBox.Show("Problem Transmitting Message");
                return false;
            }
            else
            {
                //KWP2000_Read();
                return true;
            }
        }
        #endregion

        #region Standard CAN 통신
        public static bool STD_CAN_Read()
        {
            long lResult;
            int lNumberOfMessages = 0;
            int lNumberOfErrors = 0;
            long lCount;
            string sListString = "";
            double dTime = 0;
            string Get_ID = "";
            long iByteCount;
            int iLongMessageTotalByteCount;
            byte[] iDataBytes;
            
            if (IsOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return false;  // do not read messages if we haven't opened neoVI yet
            }

            // read the messages from the driver
            lResult = icsNeoDll.icsneoGetMessages(m_hObject, ref stMessages[0], ref lNumberOfMessages, ref lNumberOfErrors);

            // was the read successful?
            if (lResult == 1)
            {
                // clear the previous list of messages
                Read_Count = lNumberOfMessages;
                Read_Error = lNumberOfErrors;

                // for each message we read
                for (lCount = 1; lCount <= lNumberOfMessages; lCount++)
                {
                    IsRead = true;
                    #region Calculate the messages timestamp
                    lResult = icsNeoDll.icsneoGetTimeStampForMsg(m_hObject, ref stMessages[lCount - 1], ref dTime);

                    //Decode based on the protocol
                    if (stMessages[lCount - 1].Protocol == (int)ePROTOCOL.SPY_PROTOCOL_CAN)
                    {
                        // list the arb id
                        Get_ID = icsNeoDll.ConvertToHex(Convert.ToString(stMessages[lCount - 1].ArbIDOrHeader));

                        //if (stMessages[lCount - 1].Data1 == 0x10)
                        //{
                        //    CAN_Transmit("30 00 64");
                        //    break;
                        //}
                        sListString = icsNeoDll.GetStringForNetworkID(Convert.ToInt16(stMessages[lCount - 1].NetworkID)) + " ArbID : " + Get_ID + " Data : ";
                    }
                    else if (stMessages[lCount - 1].Protocol == (int)ePROTOCOL.SPY_PROTOCOL_CANFD)
                    {
                        if (stMessages[lCount - 1].ExtraDataPtrEnabled == 0)
                        {
                            //Get_ID = icsNeoDll.ConvertToHex(Convert.ToString(stMessages[lCount - 1].ArbIDOrHeader));
                            Get_ID = Convert.ToString(stMessages[lCount - 1].ArbIDOrHeader, 16);
                            //8 bytes of data
                            sListString = sListString + "Network " + icsNeoDll.GetStringForNetworkID(stMessages[lCount - 1].NetworkID) + " FD ArbID : " + Get_ID + "  Data ";
                        }
                        else
                        {
                            //Count for CAN FD
                            iLongMessageTotalByteCount = stMessages[lCount - 1].NumberBytesData;

                            //More than 8 bytes of data
                            iDataBytes = new byte[iLongMessageTotalByteCount];

                            System.Runtime.InteropServices.Marshal.Copy(stMessages[lCount - 1].iExtraDataPtr, iDataBytes, 0, iLongMessageTotalByteCount);
                            System.Runtime.InteropServices.GCHandle gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(iDataBytes, System.Runtime.InteropServices.GCHandleType.Pinned);

                            sListString = sListString + "Network " + icsNeoDll.GetStringForNetworkID(stMessages[lCount - 1].NetworkID);
                            if (stMessages[lCount - 1].Protocol == (int)ePROTOCOL.SPY_PROTOCOL_CANFD) sListString = sListString + " FD ArbID : " + Convert.ToString(stMessages[lCount - 1].ArbIDOrHeader, 16);
                            sListString = sListString + "  Data ";
                            for (iByteCount = 0; iByteCount < iLongMessageTotalByteCount; iByteCount++)
                            {
                                sListString = sListString + Convert.ToString(iDataBytes[iByteCount], 16) + " ";
                            }
                            gcHandle.Free();
                        }
                    }

                    if (Get_ID.ToUpper() == ECUs.Set_ID || Get_ID.ToUpper() == ECUs.Ret_ID)
                    {
                        #region Was it a tx or rx message
                        if ((stMessages[lCount - 1].StatusBitField & Convert.ToInt32(eDATA_STATUS_BITFIELD_1.SPY_STATUS_TX_MSG)) > 0)
                        {
                            Put_Time = "Time : " + Convert.ToString(dTime);  //Build String
                            Put_Head = Get_ID;
                        }
                        else
                        {
                            Get_Time = "Time : " + Convert.ToString(dTime);  //Build String
                            Get_Head = Get_ID;
                        }

                        string[] GetD = new string[8];

                        if (stMessages[lCount - 1].NumberBytesData >= 1 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) GetD[0] = stMessages[lCount - 1].Data1.ToString("X2");
                        if (stMessages[lCount - 1].NumberBytesData >= 2 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) GetD[1] = stMessages[lCount - 1].Data2.ToString("X2");
                        if (stMessages[lCount - 1].NumberBytesData >= 3 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) GetD[2] = stMessages[lCount - 1].Data3.ToString("X2");
                        if (stMessages[lCount - 1].NumberBytesData >= 4 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) GetD[3] = stMessages[lCount - 1].Data4.ToString("X2");
                        if (stMessages[lCount - 1].NumberBytesData >= 5 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) GetD[4] = stMessages[lCount - 1].Data5.ToString("X2");
                        if (stMessages[lCount - 1].NumberBytesData >= 6 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) GetD[5] = stMessages[lCount - 1].Data6.ToString("X2");
                        if (stMessages[lCount - 1].NumberBytesData >= 7 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) GetD[6] = stMessages[lCount - 1].Data7.ToString("X2");
                        if (stMessages[lCount - 1].NumberBytesData >= 8 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) GetD[7] = stMessages[lCount - 1].Data8.ToString("X2");
                        
                        //sListString = sListString + GetD[0] + " ";
                        //sListString = sListString + GetD[1] + " ";
                        //sListString = sListString + GetD[2] + " ";
                        //sListString = sListString + GetD[3] + " ";
                        //sListString = sListString + GetD[4] + " ";
                        //sListString = sListString + GetD[5] + " ";
                        //sListString = sListString + GetD[6] + " ";
                        //sListString = sListString + GetD[7] + " ";


                        if (Get_ID.ToUpper() == ECUs.Set_ID)
                            sListString = "TX : ";
                        else
                            sListString = "RX : ";

                        sListString += Get_ID + " : " + GetD[0] + " " + GetD[1] + " " + GetD[2] + " " + GetD[3] + " " + GetD[4] + " " + GetD[5] + " " + GetD[6] + " " + GetD[7] + " ";

                        //Add the message to the list
                        if (Get_ID.ToUpper() == ECUs.Ret_ID)
                        {
                            #region Get Data
                            switch (GetD[0].Substring(0, 1))
                            {
                                case "0":
                                    #region Single Frame (SF)
                                    Get_Data = GetD[0] + " " + GetD[1] + " " + GetD[2] + " " + GetD[3] + " " + GetD[4] + " " + GetD[5] + " " + GetD[6] + " " + GetD[7];
                                    CAN_Len = Convert.ToInt32(GetD[0].Substring(1, 1), 16);

                                    if (GetD[1] == "7F")
                                    {
                                        if (GetD[3] == "78")
                                        {
                                            //NRC 0x78: requestCorrectlyReceivedResponsePending - 다음 응답 대기
                                        }
                                        else
                                        {
                                            Return = false;
                                        }
                                    }
                                    else
                                    {
                                        if (Send_Key == "")
                                        {
                                            Return = true;
                                        }
                                        else
                                        {
                                            int Data_Key = Convert.ToInt32(Send_Key, 16) + Convert.ToInt32("40", 16);

                                            if (GetD[1] == Data_Key.ToString("X2"))
                                            {
                                                Return = true;
                                            }
                                            else
                                            {
                                                Return = false;
                                            }
                                        }
                                    }
                                    break;
                                    #endregion
                                case "1":
                                    #region First  Frame (FF)
                                    CAN_Len = Convert.ToInt32(GetD[0].Substring(1, 1) + GetD[1], 16);
                                    Get_Buf.Length = 0;
                                    Get_Buf.Append(GetD[1] + " " + GetD[2] + " " + GetD[3] + " " + GetD[4] + " " + GetD[5] + " " + GetD[6] + " " + GetD[7]);
                                    Get_Data = Get_Buf.ToString();
                                    if (GetD[2] == "7F") { Return = false; }
                                    CAN_Transmit("30 00 0A");
                                    break;
                                    #endregion
                                case "2":
                                    #region Consecutive Frame (CF)
                                    if (Get_Buf.Length == 0) break;                 // FF 없이 온 CF = 잔재, 폐기
                                    Get_Buf.Append(" " + GetD[1] + " " + GetD[2] + " " + GetD[3] + " " + GetD[4] + " " + GetD[5] + " " + GetD[6] + " " + GetD[7]);
                                    Get_Data = Get_Buf.ToString();
                                    if (Ret_Length(Get_Data) - 1 >= CAN_Len) { Return = true; }
                                    break;
                                    #endregion

                                case "3":   //Flow Control (FC)
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            if (stMessages[lCount - 1].Protocol == (int)ePROTOCOL.SPY_PROTOCOL_CAN)
                            {
                                #region Put Data
                                switch (GetD[0].Substring(0, 1))
                                {
                                    case "0": //Single Frame (SF)
                                        Put_Data = GetD[0] + " " + GetD[1] + " " + GetD[2] + " " + GetD[3] + " " + GetD[4] + " " + GetD[5] + " " + GetD[6] + " " + GetD[7];
                                        break;

                                    case "1": //First  Frame (FF)
                                        Put_Data = GetD[1] + " " + GetD[2] + " " + GetD[3] + " " + GetD[4] + " " + GetD[5] + " " + GetD[6] + " " + GetD[7];
                                        break;

                                    case "2": //Consecutive Frame (CF)
                                        Put_Data = Put_Data + " " + GetD[1] + " " + GetD[2] + " " + GetD[3] + " " + GetD[4] + " " + GetD[5] + " " + GetD[6] + " " + GetD[7];
                                        break;

                                    case "3": //Flow Control (FC)
                                        break;
                                }
                                #endregion
                            }
                        }

                        #endregion

                        if (Get_ID == ECUs.Set_ID)
                        {
                            Logs.ECUsLog_File(Log_ECU.Puts, sListString);
                        }
                        else
                        {
                            Logs.ECUsLog_File(Log_ECU.Gets, sListString);
                        }

                        if (PSet.OnfDebug)
                        {
                            Main.FomDebug.ECUs_Message(sListString);
                            Main.FomDebug.Puts_Message(Put_Data);
                            Main.FomDebug.Gets_Message(Get_Data);
                            Main.FomDebug.Flag_Message(Return.ToString(), CAN_Len.ToString(), Ret_Length(Get_Data));
                        }
                    }
                    #endregion
                }
                IsRead = false;
                return true;
            }
            else
            {
                //MessageBox.Show("Problem Reading Messages");
                return false;
            }
        }
        public static bool STD_CAN_Write(string Puts)
        {
            //CAN_Transmit(Puts);
            //return true;

            //Logs.ECUsLog_File(Log_ECU.Puts, Puts);

            string[] ArrH = Puts.Split(' ');
            Send_Key = ArrH[0];
            Puts = ArrH.Length.ToString("X2") + " " + Puts;

            if (Puts.Split(' ').Length > 8)
            {

            }
            else
            {
                return CAN_Transmit(Puts);
            }

            string[] ArrD = Puts.Split(' ');
            int Counter = (ArrD.Length / 8) + 1;
            string[] str_ArrD = new string[Counter];

            int AoA = 0;
            int Tgt = 0;
            for (int cnt = 0; cnt < ArrD.Length; cnt++)
            {
                if (AoA == 0)
                {
                    if (Tgt == 0)
                    {
                        str_ArrD[Tgt] = "10" + " " + ArrD[cnt];
                    }
                    else
                    {
                        str_ArrD[Tgt] = "2" + Tgt.ToString("X1") + " " + ArrD[cnt];
                    }
                }
                else
                {
                    str_ArrD[Tgt] = str_ArrD[Tgt] + " " + ArrD[cnt];
                }

                AoA++;
                if (AoA == 7) { Tgt++; AoA = 0; }
            }

            bool Ret = true;
            for (int cnt = 0; cnt < str_ArrD.Length; cnt++)
            {
                if (ECUs.ECU == ECUs.Chery_1box)
                {
                    H2Y.Sleep(30);
                }
                else
                {
                    H2Y.Sleep(30);

                }
                
                Ret = CAN_Transmit(str_ArrD[cnt]);
            }

            return Ret;
        }
        public static bool CAN_Transmit(string pSend)
        {
            icsSpyMessage stMessagesTx = new icsSpyMessage();
            string[] HexD = pSend.Split(' ');
            string[] PutD = new string[8];

            for (int cnt = 0; cnt < 8; cnt++)
            {
                if (cnt < HexD.Length)
                {
                    PutD[cnt] = HexD[cnt];
                }
                else
                {
                    PutD[cnt] = "00";
                }
            }

            // Has the uset open neoVI yet?;
            if (IsOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return false; // do not read messages if we haven't opened neoVI yet
            }

            // Read the Network we will transmit on (indicated by lstNetwork ListBox)
            long lNetworkID = (int)eNETWORK_ID.NETID_HSCAN;

            // Is this a CAN network or a J1850/ISO one?

            if (ECUs.Comm == 1 || ECUs.Comm == 3)
            {
                //Use Normal ID
                stMessagesTx.StatusBitField = 0;
            }
            else
            {
                //Make id Extended
                stMessagesTx.StatusBitField = Convert.ToInt16(eDATA_STATUS_BITFIELD_1.SPY_STATUS_XTD_FRAME);
            }
            stMessagesTx.ArbIDOrHeader = Convert.ToInt32(ECUs.Set_ID, 16);  // The ArbID
            stMessagesTx.NumberBytesData = 8;                               // You can only have 8 databytes with CAN

            // Load all of the data bytes in the structure
            stMessagesTx.Data1 = Convert.ToByte(PutD[0], 16);
            stMessagesTx.Data2 = Convert.ToByte(PutD[1], 16);
            stMessagesTx.Data3 = Convert.ToByte(PutD[2], 16);
            stMessagesTx.Data4 = Convert.ToByte(PutD[3], 16);
            stMessagesTx.Data5 = Convert.ToByte(PutD[4], 16);
            stMessagesTx.Data6 = Convert.ToByte(PutD[5], 16);
            stMessagesTx.Data7 = Convert.ToByte(PutD[6], 16);
            stMessagesTx.Data8 = Convert.ToByte(PutD[7], 16);

            // Transmit the assembled message
            long lResult = icsNeoDll.icsneoTxMessages(m_hObject, ref stMessagesTx, Convert.ToInt32(lNetworkID), 0);
            // Test the returned result
            if (lResult != 1)
            {
                //MessageBox.Show("Problem Transmitting Message");
                //Device_Close();
                return false;
            }
            else
            {
                //STD_CAN_Read();
                return true;
            }
        }
        #endregion

        #region CAN FD 통신
        public static bool CAN_FD_Write0(string Puts)
        {
            //CAN_Transmit(Puts);
            //return true;

            //Logs.ECUsLog_File(Log_ECU.Puts, Puts);

            string[] ArrH = Puts.Split(' ');
            Send_Key = ArrH[0];
            Puts = ArrH.Length.ToString("X2") + " " + Puts;

            if (Puts.Split(' ').Length > 8)
            {

            }
            else
            {
                return CAN_Transmit(Puts);
            }

            string[] ArrD = Puts.Split(' ');
            int Counter = (ArrD.Length / 8) + 1;
            string[] str_ArrD = new string[Counter];

            int AoA = 0;
            int Tgt = 0;
            for (int cnt = 0; cnt < ArrD.Length; cnt++)
            {
                if (AoA == 0)
                {
                    if (Tgt == 0)
                    {
                        str_ArrD[Tgt] = "10" + " " + ArrD[cnt];
                    }
                    else
                    {
                        str_ArrD[Tgt] = "2" + Tgt.ToString("X1") + " " + ArrD[cnt];
                    }
                }
                else
                {
                    str_ArrD[Tgt] = str_ArrD[Tgt] + " " + ArrD[cnt];
                }

                AoA++;
                if (AoA == 7) { Tgt++; AoA = 0; }
            }

            bool Ret = true;
            for (int cnt = 0; cnt < str_ArrD.Length; cnt++)
            {
                H2Y.Sleep(500);
                Ret = CAN_Transmit(str_ArrD[cnt]);
            }

            return Ret;
        }
        public static bool CAN_FD_Write1(string Puts)
        {
            string[] ArrH = Puts.Split(' ');
            Send_Key = ArrH[0];
            Puts = ArrH.Length.ToString("X2") + " " + Puts;

            //Hardware must have CAN FD support for this tor work. 
            int lResult;
            icsSpyMessage stMessagesTx = new icsSpyMessage();
            uint iNumberTxed = 0;
            string[] HexD = Puts.Split(' ');
            int Hex_Cunt = HexD.Length + 1;

            if (Hex_Cunt < 8) { Hex_Cunt = 8; }

            byte[] PutB = new byte[Hex_Cunt];

            for (int cnt = 0; cnt < Hex_Cunt; cnt++)
            {
                if (cnt < HexD.Length)
                {
                    PutB[cnt] = Convert.ToByte(HexD[cnt], 16);
                }
                else
                {
                    PutB[cnt] = Convert.ToByte("00", 16); 
                }
            }
            
            //Get pointer to data (could also be done with Unsafe code)
            System.Runtime.InteropServices.GCHandle gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(PutB, System.Runtime.InteropServices.GCHandleType.Pinned);
            IntPtr CanFDptr = gcHandle.AddrOfPinnedObject();

            //Send on HS CAN
            int lNetworkID = (int)eNETWORK_ID.NETID_HSCAN;

            //Set the ID
            stMessagesTx.ArbIDOrHeader = Convert.ToInt32(ECUs.Set_ID, 16);  // The ArbID

            // The number of Data Bytes
            stMessagesTx.NumberBytesData = (byte)Hex_Cunt;    // Convert.ToByte(HexD.GetUpperBound(0) + 1);
            stMessagesTx.NumberBytesHeader = 0;

            stMessagesTx.iExtraDataPtr = CanFDptr;
            stMessagesTx.Protocol = Convert.ToByte(ePROTOCOL.SPY_PROTOCOL_CANFD);

            if (ECUs.Comm == 1 || ECUs.Comm == 3)
            {
                //Use Normal ID
                stMessagesTx.StatusBitField = (int)eDATA_STATUS_BITFIELD_1.SPY_STATUS_TX_MSG;
            }
            else
            {
                //Make id Extended
                stMessagesTx.StatusBitField = Convert.ToInt16(eDATA_STATUS_BITFIELD_1.SPY_STATUS_XTD_FRAME);
            }

            stMessagesTx.StatusBitField2 = 0;
            stMessagesTx.StatusBitField3 = 16; //Enable bitrate switch

            if (Convert.ToInt32(HexD.GetUpperBound(0) + 1) > 8)
            {
                //CAN FD More than 8 bytes
                //Enable Extra Data Pointer
                stMessagesTx.ExtraDataPtrEnabled = 1;
                lResult = icsNeoDll.icsneoTxMessagesEx(m_hObject, ref stMessagesTx, Convert.ToUInt32(lNetworkID), 1, ref iNumberTxed, 0);
                Console.WriteLine("");
            }
            else
            {
                //CAN FD 8 bytes or less
                stMessagesTx.ExtraDataPtrEnabled = 0;
                stMessagesTx.Data1 = PutB[0];
                stMessagesTx.Data2 = PutB[1];
                stMessagesTx.Data3 = PutB[2];
                stMessagesTx.Data4 = PutB[3];
                stMessagesTx.Data5 = PutB[4];
                stMessagesTx.Data6 = PutB[5];
                stMessagesTx.Data7 = PutB[6];
                stMessagesTx.Data8 = PutB[7];

                // Transmit the assembled message
                lResult = icsNeoDll.icsneoTxMessages(m_hObject, ref stMessagesTx, Convert.ToInt32(lNetworkID), 1);
                //lResult = icsNeoDll.icsneoTxMessages(m_hObject, ref stMessagesTx, Convert.ToInt32(lNetworkID), 0);
            }
            
            gcHandle.Free();


            // Test the returned result
            if (lResult != 1)
            {
                //MessageBox.Show("Problem Transmitting Message");
                //Device_Close();
                return false;
            }
            else
            {
                //STD_CAN_Read();
                return true;
            }
        }
        #endregion


        public static bool Ret_SendMsgs(string Msgs)
        {
            lock (TxLock)
            {
                bool Ret = true;
                double ReadTime = 0, Old_Time = 0;
                double OfstTime = DateTime.Now.Ticks;
                double Gap_Time = 1;
                double Gap_Ofst = 0;
                int Old_Len = 0;
                bool TestFlag = false;
                bool Key_Pass = false;
                bool old_Pass = false;

                ECU_Clear();

                if (!NeoVI.IsOpen) { return false; }

                Return = false;
                Device_Write(Msgs);

                while (true)
                {
                    //Device_Read(); 
                    //break;
                    #region 측정 헤더
                    if (TSet.TestStop) break;
                    if (PLC.DO.MD_Emerge) break;
                    if (PLC.DI.PSW__Stop) break;
                    if (TestFlag) { TestFlag = false; }
                    if (TSet.StepNext) TSet.StepNext = false;

                    ReadTime = (DateTime.Now.Ticks - OfstTime) / H2Y.tick_Dvd;

                    if ((H2Y.GetKeyState(H2Y.VK_SHIFT) & H2Y.KeyPressed) != 0 && (H2Y.GetKeyState(H2Y.VK_RETURN) & H2Y.KeyPressed) != 0)
                    {
                        Key_Pass = true;
                    }
                    else
                    {
                        Key_Pass = false;
                    }

                    if (Key_Pass && Key_Pass != old_Pass) { TSet.StepNext = true; }
                    if (Key_Pass != old_Pass) { old_Pass = Key_Pass; }
                    #endregion

                    if (Get_Buf.Length != Old_Len) { Old_Len = Get_Buf.Length; Gap_Ofst = ReadTime; }
                    if (Return) { Ret = true; break; }
                    if (ReadTime - Gap_Ofst > Gap_Time) { Ret = false; break; }

                    if ((ReadTime - Old_Time) >= 0.05)
                    {
                        Old_Time = ReadTime;
                        if (!IsRead)
                        {
                            Device_Read();
                        }
                    }

                    System.Windows.Forms.Application.DoEvents();
                }

                if (PSet.OnfDebug)
                {
                    //Main.FomDebug.ECUs_Message(Ret.ToString());
                }

                tmr_Wait.Interval = 1000;
                tmr_Wait.Enabled = true;
                return Ret;
            }
        }

        public static void Send_TesterPresent()
        {
            if (!IsOpen) return;
            if (!System.Threading.Monitor.TryEnter(TxLock)) return;   // 통신 중이면 건너뜀
            try { CAN_Transmit("02 3E 80"); }
            finally { System.Threading.Monitor.Exit(TxLock); }
        }

        private static int Ret_Length(string Msgs)
        {
            Msgs = Msgs.TrimStart(' ');
            Msgs = Msgs.TrimEnd(' ');

            if (Msgs == null)
            {
                return 0;
            }
            {
                return Msgs.Split(' ').Length;
            }
        }
    }
}



