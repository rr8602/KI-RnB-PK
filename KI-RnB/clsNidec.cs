using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KI_RnB
{
    public class clsNidec
    {
        fom_Main Main = null;
        public bool IsOpen = false;
        public bool IsSame = false;
        string IP_Adr = "";
        int Port = 0;
        System.Net.Sockets.TcpClient tcpSocket;
        System.Net.Sockets.NetworkStream networkStream;     //'TCP stream used for sending/receiving bytes 

        public clsNidec(fom_Main main, string ip_adr, int port)
        {
            Main = main;
            IP_Adr = ip_adr;
            Port = port;
            IsOpen = false;
        }
        public void Connect()
        {
            try
            {
                tcpSocket = new System.Net.Sockets.TcpClient();
                tcpSocket.Connect(System.Net.IPAddress.Parse(IP_Adr), Port);
                tcpSocket.ReceiveTimeout = 500;
                networkStream = tcpSocket.GetStream();
                IsOpen = true;
            }
            catch (Exception ex)
            {
                IsOpen = false;
            }
        }
        public void Disconnect()
        {
            if (tcpSocket != null) { tcpSocket.Close(); }   //dispose of connection 
            IsOpen = false;
        }

        public void Send_Order(string pHex)
        {
            if (pHex.Length > 20)
            {
                Send_Stream(pHex);
                Read_Stream();
            }
        }

        private void Send_Stream(string pHex)
        {
            if (!IsOpen) { return; }

            pHex = pHex.Replace(",", " ");
            string[] Hex = pHex.Split(' ');
            byte[] ArrD = new byte[Hex.Length];
            byte[] query = new byte[Hex.Length];

            //txt_Puts.Text = pHex;
            //lst_List.Items.Add("Put -> " + pHex);

            if (Hex.Length == 12)
            {
                for (int cnt = 0; cnt < Hex.Length; cnt++)
                {
                    ArrD[cnt] = HexTobyte(Hex[cnt]);
                    query[cnt] = ArrD[cnt];
                }
            }
            else
            {
                return;
            }

            //Dim value As String 'Long
            // 'read 32bit value
            //query[0] = ArrD[0]; //transaction identifier
            //query[1] = ArrD[1]; //transaction identifier
            //query[2] = ArrD[2]; //protocol identifier
            //query[3] = ArrD[3]; //protocol identifier
            //query[4] = ArrD[4]; //length field - Upper byte (0 since all messages smaller than 256)
            //query[5] = ArrD[5]; //length field - Lower byte
            //query[6] = ArrD[6]; //unit identifier
            //query[7] = ArrD[7]; //Modbus function code
            //query[8] = ArrD[8]; //add 16384 to parameter value to set bit 14, indicating a 32bit parameter
            //query[9] = ArrD[9];
            //query[10] = ArrD[10];  //length high
            //query[11] = ArrD[11];  //length low

            try
            {
                if (networkStream.CanWrite)
                {
                    networkStream.Write(query, 0, Hex.Length);
                }
                else
                {
                    Console.WriteLine("Sorry.  You cannot write to this NetworkStream.");
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("Error Send_Stream");
            }
        }
        private void Read_Stream()
        {
            if (!IsOpen) { return; }

            byte[] bytes = new byte[41];
            byte[] data = new byte[4];
            string ret_Hex = "";
            string value = "";

            try
            {
                if (networkStream.CanRead)
                {
                    networkStream.Read(bytes, 0, bytes.Length);
                }
                else
                {
                    Console.WriteLine("Sorry.  You cannot write to this NetworkStream.");
                    return;
                }

                foreach (byte bt in bytes)
                {
                    ret_Hex = ret_Hex + " " + bt.ToString("X2");
                }

                //txt_Gets.Text = ret_Hex.Trim();
                //lst_List.Items.Add("Get <- " + ret_Hex.Trim());

                if (bytes[7].Equals(128 + 3))  //Error denoted by 128+function code
                {
                    value = "err" + bytes[8].ToString();
                }
                else    //Reorder response bytes
                {
                    data[0] = bytes[12];
                    data[1] = bytes[11];
                    data[2] = bytes[10];
                    data[3] = bytes[9];
                    value = BitConverter.ToInt32(data, 0).ToString();   //convert 4 bytes to 32 bit integer
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("Error Read_Stream");
            }
        }

        public void All__Read()
        {
            if (!IsOpen) { return; }

            //16Bit Read (Pr 19.011: 07 76) 20개 읽기 
            //16Bit Read (Pr 18.011: 07 12) 20개 읽기
            Send_Stream("00 00 00 00 00 06 00 03 07 12 00 14");

            byte[] bytes = new byte[49];
            int[] data = new int[20];
            string ret_Hex = "";
            string value = "";
            int Diff_Set = 0;

            if (networkStream.CanRead)
            {
                networkStream.Read(bytes, 0, bytes.Length);
            }
            else
            {
                //Console.WriteLine("Sorry.  You cannot write to this NetworkStream.");
                return;
            }

            foreach (byte bt in bytes)
            {
                ret_Hex = ret_Hex + " " + bt.ToString("X2");
            }
            //txt_Gets.Text = ret_Hex.Trim();
            //lst_List.Items.Add("Get <- " + ret_Hex.Trim());

            if (bytes[7].Equals(128 + 3))  //Error denoted by 128+function code
            {
                value = "err" + bytes[8].ToString();
                IsSame = false;
            }
            else    //Reorder response bytes
            {
                data[0] = Ret_ToInt(bytes[9], bytes[10]); TSet.Nidec_FL.Status = data[0];
                data[1] = Ret_ToInt(bytes[11], bytes[12]); TSet.Nidec_FL.CalSpd = data[1];
                data[2] = Ret_ToInt(bytes[13], bytes[14]); TSet.Nidec_FL.WSSSpd = data[2];
                data[3] = Ret_ToInt(bytes[15], bytes[16]); TSet.Nidec_FL.PB_Toq = data[3];
                data[4] = Ret_ToInt(bytes[17], bytes[18]); TSet.Nidec_FL.PB_Spd = data[4];
                Bit_Status(TSet.Nidec_FL, TSet.Nidec_FL.Status);

                data[5] = Ret_ToInt(bytes[19], bytes[20]); TSet.Nidec_FR.Status = data[5];
                data[6] = Ret_ToInt(bytes[21], bytes[22]); TSet.Nidec_FR.CalSpd = data[6];
                data[7] = Ret_ToInt(bytes[23], bytes[24]); TSet.Nidec_FR.WSSSpd = data[7];
                data[8] = Ret_ToInt(bytes[25], bytes[26]); TSet.Nidec_FR.PB_Toq = data[8];
                data[9] = Ret_ToInt(bytes[27], bytes[28]); TSet.Nidec_FR.PB_Spd = data[9];
                Bit_Status(TSet.Nidec_FR, TSet.Nidec_FR.Status);

                data[10] = Ret_ToInt(bytes[29], bytes[30]); TSet.Nidec_RL.Status = data[10];
                data[11] = Ret_ToInt(bytes[31], bytes[32]); TSet.Nidec_RL.CalSpd = data[11];
                data[12] = Ret_ToInt(bytes[33], bytes[34]); TSet.Nidec_RL.WSSSpd = data[12];
                data[13] = Ret_ToInt(bytes[35], bytes[36]); TSet.Nidec_RL.PB_Toq = data[13];
                data[14] = Ret_ToInt(bytes[37], bytes[38]); TSet.Nidec_RL.PB_Spd = data[14];
                Bit_Status(TSet.Nidec_RL, TSet.Nidec_RL.Status);

                data[15] = Ret_ToInt(bytes[39], bytes[40]); TSet.Nidec_RR.Status = data[15];
                data[16] = Ret_ToInt(bytes[41], bytes[42]); TSet.Nidec_RR.CalSpd = data[16];
                data[17] = Ret_ToInt(bytes[43], bytes[44]); TSet.Nidec_RR.WSSSpd = data[17];
                data[18] = Ret_ToInt(bytes[45], bytes[46]); TSet.Nidec_RR.PB_Toq = data[18];
                data[19] = Ret_ToInt(bytes[47], bytes[48]); TSet.Nidec_RR.PB_Spd = data[19];
                Bit_Status(TSet.Nidec_RR, TSet.Nidec_RR.Status);

                //txt_RL_1.Text = PSet.OwnerSpd.ToString();
                //txt_RL_2.Text = PSet.Owner_RL.ToString();
                //txt_RL_3.Text = PSet.OwnerToq.ToString();
                //txt_RL_4.Text = PSet.OwnerPBS.ToString();

                if (PSet.OwnerSpd != TSet.Nidec_FL.CalSpd) { Diff_Set++; }
                if (PSet.OwnerSpd != TSet.Nidec_FR.CalSpd) { Diff_Set++; }
                if (PSet.OwnerSpd != TSet.Nidec_RL.CalSpd) { Diff_Set++; }
                if (PSet.OwnerSpd != TSet.Nidec_RR.CalSpd) { Diff_Set++; }

                if (PSet.Owner_FL != TSet.Nidec_FL.WSSSpd) { Diff_Set++; }
                if (PSet.Owner_FR != TSet.Nidec_FR.WSSSpd) { Diff_Set++; }
                if (PSet.Owner_RL != TSet.Nidec_RL.WSSSpd) { Diff_Set++; }
                if (PSet.Owner_RR != TSet.Nidec_RR.WSSSpd) { Diff_Set++; }

                if (PSet.OwnerToq != TSet.Nidec_FL.PB_Toq) { Diff_Set++; }
                if (PSet.OwnerToq != TSet.Nidec_FR.PB_Toq) { Diff_Set++; }
                if (PSet.OwnerToq != TSet.Nidec_RL.PB_Toq) { Diff_Set++; }
                if (PSet.OwnerToq != TSet.Nidec_RR.PB_Toq) { Diff_Set++; }

                if (PSet.OwnerPBS != TSet.Nidec_FL.PB_Spd) { Diff_Set++; }
                if (PSet.OwnerPBS != TSet.Nidec_FR.PB_Spd) { Diff_Set++; }
                if (PSet.OwnerPBS != TSet.Nidec_RL.PB_Spd) { Diff_Set++; }
                if (PSet.OwnerPBS != TSet.Nidec_RR.PB_Spd) { Diff_Set++; }

                IsSame = Diff_Set > 0 ? false : true;
            }
        }
        public void One_Write(byte Wheel, int[] value)
        {
            if (!IsOpen) { return; }

            byte[] query = new byte[23];

            byte[] val00 = BitConverter.GetBytes(value[0]);
            byte[] val01 = BitConverter.GetBytes(value[1]);
            byte[] val02 = BitConverter.GetBytes(value[2]);
            byte[] val03 = BitConverter.GetBytes(value[3]);
            byte[] val04 = BitConverter.GetBytes(value[4]);

            query[0] = 0;                   //transaction identifier
            query[1] = 0;                   //transaction identifier
            query[2] = 0;                   //protocol identifier
            query[3] = 0;                   //protocol identifier
            query[4] = 0;                   //length field - Upper byte (0 since all messages smaller than 256)
            query[5] = HexTobyte("2E");     //length field - Lower byte
            query[6] = 0;                   //unit identifier
            query[7] = 16;                  //Modbus function code

            switch (Wheel)                  //add 16384 to parameter value to set bit 14, indicating a 32bit parameter
            {
                case 0: query[8] = HexTobyte("07"); query[9] = HexTobyte("12"); break;
                case 1: query[8] = HexTobyte("07"); query[9] = HexTobyte("17"); break;
                case 2: query[8] = HexTobyte("07"); query[9] = HexTobyte("1C"); break;
                case 3: query[8] = HexTobyte("07"); query[9] = HexTobyte("21"); break;
            }

            query[10] = HexTobyte("00");    //length high
            query[11] = HexTobyte("05");    //length low
            query[12] = HexTobyte("0A");    //length of register data to write in bytes 

            query[13] = val00[1]; query[14] = val00[0]; //Val00
            query[15] = val01[1]; query[16] = val01[0]; //Val01
            query[17] = val02[1]; query[18] = val02[0]; //Val02
            query[19] = val03[1]; query[20] = val03[0]; //Val03
            query[21] = val04[1]; query[22] = val04[0]; //Val04     

            if (networkStream.CanWrite)
            {
                networkStream.Write(query, 0, query.Length);
            }
            else
            {
                Console.WriteLine("Sorry.  You cannot write to this NetworkStream.");
            }
        }
        public void All_Write()
        {
            if (!IsOpen) { return; }

            byte[] query = new byte[53];

            byte[] val00 = BitConverter.GetBytes(TSet.Nidec_FL.Status);
            byte[] val01 = BitConverter.GetBytes(TSet.Nidec_FL.CalSpd);
            byte[] val02 = BitConverter.GetBytes(TSet.Nidec_FL.WSSSpd);
            byte[] val03 = BitConverter.GetBytes(TSet.Nidec_FL.PB_Toq);
            byte[] val04 = BitConverter.GetBytes(TSet.Nidec_FL.PB_Spd);

            byte[] val05 = BitConverter.GetBytes(TSet.Nidec_FR.Status);
            byte[] val06 = BitConverter.GetBytes(TSet.Nidec_FR.CalSpd);
            byte[] val07 = BitConverter.GetBytes(TSet.Nidec_FR.WSSSpd);
            byte[] val08 = BitConverter.GetBytes(TSet.Nidec_FR.PB_Toq);
            byte[] val09 = BitConverter.GetBytes(TSet.Nidec_FR.PB_Spd);

            byte[] val10 = BitConverter.GetBytes(TSet.Nidec_RL.Status);
            byte[] val11 = BitConverter.GetBytes(TSet.Nidec_RL.CalSpd);
            byte[] val12 = BitConverter.GetBytes(TSet.Nidec_RL.WSSSpd);
            byte[] val13 = BitConverter.GetBytes(TSet.Nidec_RL.PB_Toq);
            byte[] val14 = BitConverter.GetBytes(TSet.Nidec_RL.PB_Spd);

            byte[] val15 = BitConverter.GetBytes(TSet.Nidec_RR.Status);
            byte[] val16 = BitConverter.GetBytes(TSet.Nidec_RR.CalSpd);
            byte[] val17 = BitConverter.GetBytes(TSet.Nidec_RR.WSSSpd);
            byte[] val18 = BitConverter.GetBytes(TSet.Nidec_RR.PB_Toq);
            byte[] val19 = BitConverter.GetBytes(TSet.Nidec_RR.PB_Spd);

            query[0] = 0;                   //transaction identifier
            query[1] = 0;                   //transaction identifier
            query[2] = 0;                   //protocol identifier
            query[3] = 0;                   //protocol identifier
            query[4] = 0;                   //length field - Upper byte (0 since all messages smaller than 256)
            query[5] = HexTobyte("2E");     //length field - Lower byte
            query[6] = 0;                   //unit identifier
            query[7] = 16;                  //Modbus function code
            query[8] = HexTobyte("07");     //add 16384 to parameter value to set bit 14, indicating a 32bit parameter
            query[9] = HexTobyte("12");
            query[10] = HexTobyte("00");    //length high
            query[11] = HexTobyte("14");    //length low
            query[12] = HexTobyte("28");    //length of register data to write in bytes 

            query[13] = val00[1]; query[14] = val00[0]; //Val00
            query[15] = val01[1]; query[16] = val01[0]; //Val01
            query[17] = val02[1]; query[18] = val02[0]; //Val02
            query[19] = val03[1]; query[20] = val03[0]; //Val03
            query[21] = val04[1]; query[22] = val04[0]; //Val04

            query[23] = val05[1]; query[24] = val05[0]; //Val05
            query[25] = val06[1]; query[26] = val06[0]; //Val06
            query[27] = val07[1]; query[28] = val07[0]; //Val07
            query[29] = val08[1]; query[30] = val08[0]; //Val08
            query[31] = val09[1]; query[32] = val09[0]; //Val09

            query[33] = val10[1]; query[34] = val10[0]; //Val10
            query[35] = val11[1]; query[36] = val11[0]; //Val11
            query[37] = val12[1]; query[38] = val12[0]; //Val12
            query[39] = val13[1]; query[40] = val13[0]; //Val13
            query[41] = val14[1]; query[42] = val14[0]; //Val14

            query[43] = val15[1]; query[44] = val15[0]; //Val15
            query[45] = val16[1]; query[46] = val16[0]; //Val16
            query[47] = val17[1]; query[48] = val17[0]; //Val17
            query[49] = val18[1]; query[50] = val18[0]; //Val18
            query[51] = val19[1]; query[52] = val19[0]; //Val19            

            if (networkStream.CanWrite)
            {
                networkStream.Write(query, 0, query.Length);
            }
            else
            {
                Console.WriteLine("Sorry.  You cannot write to this NetworkStream.");
            }
        }

        private void Bit_Status(TSet.Nidec_Drive wheel, int val)
        {
            wheel.Cal_MD = (val & H2Y.BitA[0]) > 0 ? true : false;
            wheel.MT_Run = (val & H2Y.BitA[1]) > 0 ? true : false;
            wheel.MTSync = (val & H2Y.BitA[2]) > 0 ? true : false;
            wheel.MT_Brk = (val & H2Y.BitA[3]) > 0 ? true : false;
            wheel.MTPark = (val & H2Y.BitA[4]) > 0 ? true : false;
        }

        private int Ret_ToInt(byte val1, byte val2)
        {
            return (val1 * 256) + val2;
        }
        private byte HexTobyte(string pHex)
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
    }
}
