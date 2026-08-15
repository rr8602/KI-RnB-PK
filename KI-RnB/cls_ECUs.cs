using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace KI_RnB
{
    public static class ECUs
    {
        public const string Mobis___AD = "MOBIS AD";        //Avante AD
        public const string Mobis__DN8 = "MOBIS DN8";       //그랜저
        public const string Mando___TL = "MANDO TL";        //투산
        public const string Mando___TM = "MANDO TM";        //싼타페        
        public const string Mando__HEV = "MANDO CN7 HEV";   //11.20    - TM이랑 같이
        public const string Mobis___FL = "MOBIS DN8 FL";    //11.20     - MOBIS DN8이랑 같이
        
        //250416
        public const string Mando_NX4H = "MANDO NX4 HEV";
        public const string Mando_NX4I = "MANDO NX4 ICE";
        public const string Mobis_LX3H = "MOBIS LX3 HEV";   //LX3 iMEB2  HEV  (250710)
        public const string Mobis_LX3I = "MOBIS LX3 ICE";   //LX3 MEB5_1 ICE  (250710)

        //251122
        public const string Chery_1box = "Chery Onebox";    

        public static int Comm { get; set; }        //ECU 통신 방식(0:KWP2000, 1:CAN, 2:CAN ID(Ex), 3:CAN FD, 4:CAN FD ID(Ex))
        public static string ECU    { get; set; }   //ECU 모델명

        public static string Set_ID { get; set; }   //세팅 ID
        public static string Ret_ID { get; set; }   //응답 ID

        public static fom_Main Main = null;

        public static string Stt_Comm;
        #region ECU identification
        public static string Veh_Name;
        public static string Sys_Name;
        public static string Var_Code;
        public static string HW__Vers;
        public static string SW__Vers;
        public static string BCD_Date;
        public static string Part_Num;
        #endregion
        public static string ProcessB;
        public static string Read_Vin;
        public static string ReadTire;
        public static string SAS_Zero;
        public static string ESS_Lamp;
        #region Check Input-/ Output-Signals
        public static string SigPedal;
        public static string SigValve;
        public static string Sig_Pump;
        #endregion

        public static string DTC_Read;
        public static string DTCClear;
        public static string SpLmt_On;
        public static string SpLmtOff;
        public static string End_Comm;  

        public static float WSS_FL;
        public static float WSS_FR;
        public static float WSS_RL;
        public static float WSS_RR;
        public static int ABS_Step;

        public static void Ret_Data_Cls()
        {
            WSS_FL = -1;
            WSS_FR = -1;
            WSS_RL = -1;
            WSS_RR = -1;
            ABS_Step = 0;

            Stt_Comm = "";
            #region ECU identification
            Veh_Name = "";
            Sys_Name = "";
            Var_Code = "";
            HW__Vers = "";
            SW__Vers = "";
            BCD_Date = "";
            Part_Num = "";
            #endregion

            ProcessB = "";
            Read_Vin = "";
            ReadTire = "";
            SAS_Zero = "";
            #region Check Input-/ Output-Signals
            SigPedal = "";
            SigValve = "";
            Sig_Pump = "";
            #endregion

            DTC_Read = "";
            DTCClear = "";
            SpLmt_On = "";
            SpLmtOff = "";
            End_Comm = "";
        }

        public static bool ECU_Selector(string ECU_Name)
        {
            bool Ret = false;

            switch (ECU_Name)
            {
                case Mobis___AD: ECU = Mobis___AD; Comm = 1; Set_ID = "7D1"; Ret_ID = "7D9"; Ret = true; break;
                case Mobis__DN8: ECU = Mobis__DN8; Comm = 1; Set_ID = "7D1"; Ret_ID = "7D9"; Ret = true; break;
                case Mobis___FL: ECU = Mobis__DN8; Comm = 1; Set_ID = "7D1"; Ret_ID = "7D9"; Ret = true; break;
                case Mando___TL: ECU = Mando___TL; Comm = 1; Set_ID = "7D1"; Ret_ID = "7D9"; Ret = true; break;
                case Mando___TM: ECU = Mando___TM; Comm = 1; Set_ID = "7E7"; Ret_ID = "7EF"; Ret = true; break;
                case Mando__HEV: ECU = Mando___TM; Comm = 1; Set_ID = "7E7"; Ret_ID = "7EF"; Ret = true; break;
                case Mando_NX4H: ECU = Mando___TM; Comm = 1; Set_ID = "7E7"; Ret_ID = "7EF"; Ret = true; break; //250416
                case Mando_NX4I: ECU = Mando___TM; Comm = 1; Set_ID = "7E7"; Ret_ID = "7EF"; Ret = true; break; //250416
                case Mobis_LX3H: ECU = Mobis_LX3H; Comm = 1; Set_ID = "7E7"; Ret_ID = "7EF"; Ret = true; break;
                case Mobis_LX3I: ECU = Mobis_LX3I; Comm = 1; Set_ID = "7D1"; Ret_ID = "7D9"; Ret = true; break;
                case Chery_1box: ECU = Chery_1box; Comm = 1; Set_ID = "720"; Ret_ID = "730"; Ret = true; break; //251122
            }

            return Ret;
        }
        
        public static bool SecurityAccess()         //0. SecurityAccess
        {
            string Msgs = "Start of the vehicle Extended session";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = true; break;
                    case Mobis__DN8: ret = true; break;
                    case Mobis___FL: ret = true; break;
                    case Mando___TL: ret = MANDO__TL.SecurityAccess(); break;
                    case Mando___TM: ret = MANDO__TM.SecurityAccess(); break;
                    case Mando__HEV: ret = MANDO__TM.SecurityAccess(); break;
                    case Mando_NX4H: ret = MANDO__TM.SecurityAccess(); break; //250416
                    case Mando_NX4I: ret = MANDO__TM.SecurityAccess(); break; //250416
                    case Mobis_LX3H: ret = true; break;  // LX3: SecurityAccess 불필요
                    case Mobis_LX3I: ret = true; break;  // LX3: SecurityAccess 불필요
                    case Chery_1box: ret = CHERY1BOX.SecurityAccess(); break; //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }

        public static bool Start_Communication()    //1. communication Start
        {
            string Msgs = "Start of the vehicle Extended session";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.Start_Communication(); break;
                    case Mobis__DN8: ret = MOBIS_DN8.Start_Communication(); break;
                    case Mobis___FL: ret = MOBIS_DN8.Start_Communication(); break;
                    case Mando___TL: ret = MANDO__TL.Start_Communication(); break;
                    case Mando___TM: ret = MANDO__TM.Start_Communication(); break;
                    case Mando__HEV: ret = MANDO__TM.Start_Communication(); break;
                    case Mando_NX4H: ret = MANDO__TM.Start_Communication(); break; //250416
                    case Mando_NX4I: ret = MANDO__TM.Start_Communication(); break; //250416
                    case Mobis_LX3H: ret = MOBIS_LX3H.Start_Communication(); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.Start_Communication(); break;
                    case Chery_1box: ret = CHERY1BOX.Start_Communication(); break; //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }
        public static bool Stop_Communication()     //2. communication Stop
        {
            string Msgs = "ECU communication";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.Stop_Communication(); break;
                    case Mobis__DN8: ret = MOBIS_DN8.Stop_Communication(); break;
                    case Mobis___FL: ret = MOBIS_DN8.Stop_Communication(); break;
                    case Mando___TL: ret = MANDO__TL.Stop_Communication(); break;
                    case Mando___TM: ret = MANDO__TM.Stop_Communication(); break;
                    case Mando__HEV: ret = MANDO__TM.Stop_Communication(); break;
                    case Mando_NX4H: ret = MANDO__TM.Stop_Communication(); break;   //250416
                    case Mando_NX4I: ret = MANDO__TM.Stop_Communication(); break;   //250416
                    case Mobis_LX3H: ret = MOBIS_LX3H.Stop_Communication(); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.Stop_Communication(); break;
                    case Chery_1box: ret = CHERY1BOX.Stop_Communication(); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }
        public static bool ECU_Reset()              //3. ECU Reset
        {
            string Msgs = "ECU Reset";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.ECU_Reset(); break;
                    case Mobis__DN8: ret = MOBIS_DN8.ECU_Reset(); break;
                    case Mobis___FL: ret = MOBIS_DN8.ECU_Reset(); break;
                    case Mando___TL: ret = MANDO__TL.ECU_Reset(); break;
                    case Mando___TM: ret = MANDO__TM.ECU_Reset(); break;
                    case Mando__HEV: ret = MANDO__TM.ECU_Reset(); break;
                    case Mando_NX4H: ret = MANDO__TM.ECU_Reset(); break;
                    case Mando_NX4I: ret = MANDO__TM.ECU_Reset(); break;
                    case Mobis_LX3H: ret = MOBIS_LX3H.ECU_Reset(); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.ECU_Reset(); break;
                    case Chery_1box: ret = CHERY1BOX.ECU_Reset(); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }
        public static bool ECU_Identification()     //4. ECU identification
        {
            string Msgs = "ECU Identification";            
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.ECU_Identification(); break;
                    case Mobis__DN8: ret = MOBIS_DN8.ECU_Identification(); break;
                    case Mobis___FL: ret = MOBIS_DN8.ECU_Identification(); break;
                    case Mando___TL: ret = MANDO__TL.ECU_Identification(); break;
                    case Mando___TM: ret = MANDO__TM.ECU_Identification(); break;
                    case Mando__HEV: ret = MANDO__TM.ECU_Identification(); break;
                    case Mando_NX4H: ret = MANDO__TM.ECU_Identification(); break;
                    case Mando_NX4I: ret = MANDO__TM.ECU_Identification(); break;
                    case Mobis_LX3H: ret = MOBIS_LX3H.ECU_Identification(); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.ECU_Identification(); break;
                    case Chery_1box: ret = CHERY1BOX.ECU_Identification(); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }
        public static bool Read__DTC()              //5. DTC Read
        {
            string Msgs = "Read DTC";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.Read__DTC(); break;
                    case Mobis__DN8: ret = MOBIS_DN8.Read__DTC(); break;
                    case Mobis___FL: ret = MOBIS_DN8.Read__DTC(); break;
                    case Mando___TL: ret = MANDO__TL.Read__DTC(); break;
                    case Mando___TM: ret = MANDO__TM.Read__DTC(); break;
                    case Mando__HEV: ret = MANDO__TM.Read__DTC(); break;
                    case Mando_NX4H: ret = MANDO__TM.Read__DTC(); break;
                    case Mando_NX4I: ret = MANDO__TM.Read__DTC(); break;
                    case Mobis_LX3H: ret = MOBIS_LX3H.Read__DTC(); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.Read__DTC(); break;
                    case Chery_1box: ret = CHERY1BOX.Read__DTC(); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }
        public static bool Clear_DTC()              //6. DTC Clear
        {
            string Msgs = "Clear DTC";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.Clear_DTC(); break;
                    case Mobis__DN8: ret = MOBIS_DN8.Clear_DTC(); break;
                    case Mobis___FL: ret = MOBIS_DN8.Clear_DTC(); break;
                    case Mando___TL: ret = MANDO__TL.Clear_DTC(); break;
                    case Mando___TM: ret = MANDO__TM.Clear_DTC(); break;
                    case Mando__HEV: ret = MANDO__TM.Clear_DTC(); break;
                    case Mando_NX4H: ret = MANDO__TM.Clear_DTC(); break;
                    case Mando_NX4I: ret = MANDO__TM.Clear_DTC(); break;
                    case Mobis_LX3H: ret = MOBIS_LX3H.Clear_DTC(); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.Clear_DTC(); break;
                    case Chery_1box: ret = CHERY1BOX.Clear_DTC(); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }
        public static bool Check_Signals()          //7. Signals Check
        {
            string Msgs = "Check of the input and output signals";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.Check_Signals(); break;
                    case Mobis__DN8: ret = MOBIS_DN8.Check_Signals(); break;
                    case Mobis___FL: ret = MOBIS_DN8.Check_Signals(); break;
                    case Mando___TL: ret = MANDO__TL.Check_Signals(); break;
                    case Mando___TM: ret = MANDO__TM.Check_Signals(); break;
                    case Mando__HEV: ret = MANDO__TM.Check_Signals(); break;
                    case Mando_NX4I: ret = MANDO__TM.Check_Signals(); break;
                    case Mando_NX4H: ret = MANDO__TM.Check_Signals(); break;
                    case Mobis_LX3H: ret = MOBIS_LX3H.Check_Signals(); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.Check_Signals(); break;
                    case Chery_1box: ret = CHERY1BOX.Check_BLS_Signal(); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }
        public static bool WSS_Test()               //8. WSS Test
        {
            string Msgs = "WSS Test";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.WSS_Test(); break;
                    case Mobis__DN8: ret = MOBIS_DN8.WSS_Test(); break;   //10km/h 이상 올라가면 동작 정지
                    case Mobis___FL: ret = MOBIS_DN8.WSS_Test(); break;   //10km/h 이상 올라가면 동작 정지
                    case Mando___TL: ret = MANDO__TL.WSS_Test(); break;
                    case Mando___TM: ret = MANDO__TM.WSS_Test(); break;
                    case Mando__HEV: ret = MANDO__TM.WSS_Test(); break;
                    case Mando_NX4H: ret = MANDO__TM.WSS_Test(); break;
                    case Mando_NX4I: ret = MANDO__TM.WSS_Test(); break;
                    case Mobis_LX3H: ret = MOBIS_LX3H.WSS_Test(); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.WSS_Test(); break;
                    case Chery_1box: ret = CHERY1BOX.WSS_Test(); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }

        public static bool Tester_Present()         //TesterPresent(3E hex) Service 테스터 존재
        {
            string Msgs = "TesterPresent";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.Tester_Present(); break;
                    case Mobis__DN8: ret = MOBIS_DN8.Tester_Present(); break;
                    case Mobis___FL: ret = MOBIS_DN8.Tester_Present(); break;
                    case Mando___TL: ret = MANDO__TL.Tester_Present(); break;
                    case Mando___TM: ret = MANDO__TM.Tester_Present(); break;
                    case Mando__HEV: ret = MANDO__TM.Tester_Present(); break;
                    case Mando_NX4H: ret = MANDO__TM.Tester_Present(); break;
                    case Mando_NX4I: ret = MANDO__TM.Tester_Present(); break;
                    case Mobis_LX3H: ret = MOBIS_LX3H.Tester_Present(); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.Tester_Present(); break;
                    case Chery_1box: ret = CHERY1BOX.Tester_Present(); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }
        public static bool Message_Falg()
        {
            string Msgs = "EnableNormalMessageTransmission (29 hex) service";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.Message_Falg(); break;
                    case Mobis__DN8: ret = MOBIS_DN8.Message_Falg(); break;
                    case Mobis___FL: ret = MOBIS_DN8.Message_Falg(); break;
                    case Mando___TL: ret = MANDO__TL.Message_Falg(); break;
                    case Mando___TM: ret = MANDO__TM.Message_Falg(); break;
                    case Mando__HEV: ret = MANDO__TM.Message_Falg(); break;
                    case Mando_NX4H: ret = MANDO__TM.Message_Falg(); break;
                    case Mando_NX4I: ret = MANDO__TM.Message_Falg(); break;
                    case Mobis_LX3H: ret = MOBIS_LX3H.Message_Falg(); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.Message_Falg(); break;
                    case Chery_1box: ret = CHERY1BOX.Message_Falg(); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }

        public static bool Dynamic_Step(int Idx)    //9. 
        {
            string Msgs = "Dynamic Test";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.Dynamic_Step(Idx); break;
                    case Mobis__DN8: ret = MOBIS_DN8.Dynamic_Step(Idx); break;
                    case Mobis___FL: ret = MOBIS_DN8.Dynamic_Step(Idx); break;
                    case Mando___TL: ret = MANDO__TL.Dynamic_Step(Idx); break;
                    case Mando___TM: ret = MANDO__TM.Dynamic_Step(Idx); break;
                    case Mando__HEV: ret = MANDO__TM.Dynamic_Step(Idx); break;
                    case Mando_NX4H: ret = MANDO__TM.Dynamic_Step(Idx); break;
                    case Mando_NX4I: ret = MANDO__TM.Dynamic_Step(Idx); break;
                    case Mobis_LX3H: ret = MOBIS_LX3H.Dynamic_Step(Idx); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.Dynamic_Step(Idx); break;
                    case Chery_1box: ret = CHERY1BOX.Dynamic_Step(Idx); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }
        public static bool Dynamic_Auto()           //9.1 
        {
            string Msgs = "Dynamic Test";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.Dynamic_Auto(); break;
                    case Mobis__DN8: ret = MOBIS_DN8.Dynamic_Auto(); break;
                    case Mobis___FL: ret = MOBIS_DN8.Dynamic_Auto(); break;
                    case Mando___TL: ret = MANDO__TL.Dynamic_Auto(); break;
                    case Mando___TM: ret = MANDO__TM.Dynamic_Auto(); break;
                    case Mando__HEV: ret = MANDO__TM.Dynamic_Auto(); break;
                    case Mando_NX4H: ret = MANDO__TM.Dynamic_Auto(); break;
                    case Mando_NX4I: ret = MANDO__TM.Dynamic_Auto(); break;
                    case Mobis_LX3H: ret = MOBIS_LX3H.Dynamic_Auto(); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.Dynamic_Auto(); break;
                    case Chery_1box: ret = CHERY1BOX.Dynamic_Auto(); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }
            
            return ret;
        }
        public static bool ESP_Step(int Idx)        //9. 
        {
            string Msgs = "Dynamic Test";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.ESP_Step(Idx); break;
                    case Mobis__DN8: ret = MOBIS_DN8.ESP_Step(Idx); break;
                    case Mobis___FL: ret = MOBIS_DN8.ESP_Step(Idx); break;
                    case Mando___TL: ret = MANDO__TL.ESP_Step(Idx); break;
                    case Mando___TM: ret = MANDO__TM.ESP_Step(Idx); break;
                    case Mando__HEV: ret = MANDO__TM.ESP_Step(Idx); break;
                    case Mando_NX4H: ret = MANDO__TM.ESP_Step(Idx); break;
                    case Mando_NX4I: ret = MANDO__TM.ESP_Step(Idx); break;
                    case Mobis_LX3H: ret = MOBIS_LX3H.ESP_Step(Idx); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.ESP_Step(Idx); break;
                    case Chery_1box: ret = CHERY1BOX.ESP_Step(Idx); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }

        public static bool ESS_LampTest()           //10. ESS Lamp
        {
            string Msgs = "ESS Lamp";
            bool ret = false;

            try
            {
                switch (ECU)
                {
                    case Mobis___AD: ret = MOBIS__AD.ESS_LampTest(); break;
                    case Mobis__DN8: ret = MOBIS_DN8.ESS_LampTest(); break;
                    case Mobis___FL: ret = MOBIS_DN8.ESS_LampTest(); break;
                    case Mando___TL: ret = MANDO__TL.ESS_LampTest(); break;
                    case Mando___TM: ret = MANDO__TM.ESS_LampTest(); break;
                    case Mando__HEV: ret = MANDO__TM.ESS_LampTest(); break;
                    case Mando_NX4H: ret = MANDO__TM.ESS_LampTest(); break;
                    case Mando_NX4I: ret = MANDO__TM.ESS_LampTest(); break;
                    case Mobis_LX3H: ret = MOBIS_LX3H.ESS_LampTest(); break;
                    case Mobis_LX3I: ret = MOBIS_LX3I.ESS_LampTest(); break;
                    case Chery_1box: ret = CHERY1BOX.ESS_LampTest(); break;   //251122
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }

        #region Dynamic Step
        public static bool Dynamic_Step_00()
        {
            string Msgs = "Pump motor on";
            bool Ret = false;

            switch (ECU)
            {
                case Mobis__DN8: Ret = true; break;
                case Mobis___FL: Ret = true; break;
            }

            return Ret;
        }
        #endregion
    }

    public static class MOBIS__AD
    {
        #region Variable declaration
        public static string Veh_Name;
        public static string Sys_Name;
        public static string Var_Code;
        public static string HW__Vers;
        public static string SW__Vers;
        public static string BCD_Date;
        public static string Part_Num;
        #endregion

        #region Standard CAN
        public static bool Start_Communication()    //1 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("10 03");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Stop_Communication()     //2 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("20");
            if (Ret) { ECUs.End_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Reset()              //3 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("11 01");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Identification()     //4 
        {
            bool Ret = true;

            #region Identification Clear
            Veh_Name = "";
            Sys_Name = "";
            Var_Code = "";
            HW__Vers = "";
            SW__Vers = "";
            BCD_Date = "";
            Part_Num = "";
            #endregion

            try
            {
                if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 00"); }
                if (Ret) { Identification(NeoVI.Get_Data); }
            }
            catch (Exception ex)
            {
                Ret = false;
            }

            return Ret;
        }
        public static void Identification(string pData) //4.1 
        {
            string[] Ident_80 = pData.Split(' ');

            if (Ident_80.Length < 26) return;

            Veh_Name = H2Y.HexToASCII(Ident_80[4])
                     + H2Y.HexToASCII(Ident_80[5]);
            //Ident_80[6]
            Sys_Name = H2Y.HexToASCII(Ident_80[7])
                     + H2Y.HexToASCII(Ident_80[8])
                     + H2Y.HexToASCII(Ident_80[9]);
            //Ident_80[10]
            Var_Code = Ident_80[11];
            //Ident_80[12]
            HW__Vers = H2Y.HexToASCII(Ident_80[13]);
            SW__Vers = H2Y.HexToASCII(Ident_80[14]);
            //Ident_80[15]
            BCD_Date = Ident_80[16] + "/" + Ident_80[17] + "/" + Ident_80[18];
            //Ident_80[19]
            Part_Num = H2Y.HexToASCII(Ident_80[20])
                     + H2Y.HexToASCII(Ident_80[21])
                     + H2Y.HexToASCII(Ident_80[22])
                     + H2Y.HexToASCII(Ident_80[23])
                     + H2Y.HexToASCII(Ident_80[24])
                     + H2Y.HexToASCII(Ident_80[25])
                     + H2Y.HexToASCII(Ident_80[26])
                     + H2Y.HexToASCII(Ident_80[27])
                     + H2Y.HexToASCII(Ident_80[28])
                     + H2Y.HexToASCII(Ident_80[29])
                     + H2Y.HexToASCII(Ident_80[30]);

            ECUs.Veh_Name = Veh_Name;
            ECUs.Sys_Name = Sys_Name;
            ECUs.Var_Code = Var_Code;
            ECUs.HW__Vers = HW__Vers;
            ECUs.SW__Vers = SW__Vers;
            ECUs.BCD_Date = BCD_Date;
            ECUs.Part_Num = Part_Num;

            NeoVI.Debug_Message("1.Vehicle Name  : " + ECUs.Veh_Name);
            NeoVI.Debug_Message("2.System Name : " + ECUs.Sys_Name);
            NeoVI.Debug_Message("3.Variant code : " + ECUs.Var_Code);
            NeoVI.Debug_Message("4.H/W version : " + ECUs.HW__Vers);
            NeoVI.Debug_Message("5.S/W version : " + ECUs.SW__Vers);
            NeoVI.Debug_Message("6.Release date : " + ECUs.BCD_Date);
            NeoVI.Debug_Message("7.HMC/KMC part number : " + ECUs.Part_Num);
        }
        public static bool Read__DTC()              //5 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("19 02 08");
            if (Ret) { ECUs.DTC_Read = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Clear_DTC()              //6 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("14 FF FF FF");
            if (Ret) { ECUs.DTCClear = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Check_Signals()          //7 
        {
            bool Ret = true;

            #region Signals Clear

            #endregion

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 01 04"); }
            if (Ret)
            {
                Service_Data_0104(NeoVI.Get_Data);

                NeoVI.Debug_Message("1.Brake Light Switch : " + ECUs.SigPedal);
                NeoVI.Debug_Message("2.Valve relay Switch : " + ECUs.SigValve);
                NeoVI.Debug_Message("3.Pump Motor Status  : " + ECUs.Sig_Pump);
            }
            return Ret;
        }
        public static bool WSS_Test()               //8 
        {
            bool Ret = true;

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 01 04"); }  //FL:4B 00 
            if (Ret)
            {
                Service_WSS_0104(NeoVI.Get_Data);

                NeoVI.Debug_Message("WSS FL : " + ECUs.WSS_FL);
                NeoVI.Debug_Message("WSS FR : " + ECUs.WSS_FR);
                NeoVI.Debug_Message("WSS RL : " + ECUs.WSS_RL);
                NeoVI.Debug_Message("WSS RR : " + ECUs.WSS_RR);
            }

            return Ret;
        }
        private static void Service_WSS_0104(string pData)
        {
            string[] Ident_80 = pData.Split(' ');

            if (Ident_80.Length < 26) return;

            ECUs.WSS_FL = H2Y.HexTobyte(Ident_80[15]);
            ECUs.WSS_FR = H2Y.HexTobyte(Ident_80[16]);
            ECUs.WSS_RL = H2Y.HexTobyte(Ident_80[17]);
            ECUs.WSS_RR = H2Y.HexTobyte(Ident_80[18]);
        }
        private static void Service_Data_0104(string pData)
        {
            string[] Ident_80 = pData.Split(' ');

            if (Ident_80.Length < 26) return;

            string PID_Data = H2Y.HexToASCII(Ident_80[4])
                            + H2Y.HexToASCII(Ident_80[5])
                            + H2Y.HexToASCII(Ident_80[6])
                            + H2Y.HexToASCII(Ident_80[7]);
            NeoVI.Debug_Message("00.Supported PID:0x01~0x20 : " + PID_Data);

            float RPM_Data = H2Y.HexToInt(Ident_80[8], Ident_80[9]) * 0.25f;
            NeoVI.Debug_Message("01.Engine RPM  : " + RPM_Data);

            byte VehSpeed = H2Y.HexTobyte(Ident_80[10]);
            NeoVI.Debug_Message("02.Vehicle speed : " + VehSpeed);

            float Absolute = H2Y.HexTobyte(Ident_80[11]) * (100 / 255);
            NeoVI.Debug_Message("03.Absolute Throttle position sensor : " + Absolute);

            string Shift_lever = H2Y.HexToBinary(Ident_80[12]);
            NeoVI.Debug_Message("04.Shift lever position : " + Shift_lever);

            float ECU_Volt = H2Y.HexTobyte(Ident_80[13]) * (16 / 255);
            NeoVI.Debug_Message("05.ECU supply voltage : " + ECU_Volt);

            float Ref5Volt = H2Y.HexTobyte(Ident_80[14]);
            NeoVI.Debug_Message("06. 5 Volt reference : " + Ref5Volt);

            ECUs.WSS_FL = H2Y.HexTobyte(Ident_80[15]);
            ECUs.WSS_FR = H2Y.HexTobyte(Ident_80[16]);
            ECUs.WSS_RL = H2Y.HexTobyte(Ident_80[17]);
            ECUs.WSS_RR = H2Y.HexTobyte(Ident_80[18]);

            string Steering = Ident_80[19];
            NeoVI.Debug_Message("0B.Steering Sensor (00=low, 01=high, 11=Reserved) : " + Steering);

            string Longitudinal = Ident_80[20];
            NeoVI.Debug_Message("0C.G Sensor – Longitudinal (4WD only) : " + Longitudinal);

            string Lateral = Ident_80[21];
            NeoVI.Debug_Message("0D.G Sensor – Lateral (ESC only) : " + Lateral);

            string Warning = H2Y.HexToBinary(Ident_80[22]);
            NeoVI.Debug_Message("0E.Warning lamp (00=off, 01=on, 11=Reserved) : " + Warning);

            switch (Warning.Substring(6, 2))
            {
                case "00": NeoVI.Debug_Message("0E.1 ABS lamp : OFF"); break;
                case "01": NeoVI.Debug_Message("0E.1 ABS lamp : ON"); break;
                case "11": NeoVI.Debug_Message("0E.1 ABS lamp : Reserved"); break;
            }
            switch (Warning.Substring(4, 2))
            {
                case "00": NeoVI.Debug_Message("0E.2 EBD lamp : OFF"); break;
                case "01": NeoVI.Debug_Message("0E.2 EBD lamp : ON"); break;
                case "11": NeoVI.Debug_Message("0E.2 EBD lamp : Reserved"); break;
            }
            switch (Warning.Substring(2, 2))
            {
                case "00": NeoVI.Debug_Message("0E.3 ESC lamp : OFF"); break;
                case "01": NeoVI.Debug_Message("0E.3 ESC lamp : ON"); break;
                case "11": NeoVI.Debug_Message("0E.3 ESC lamp : Reserved"); break;
            }
            switch (Warning.Substring(0, 2))
            {
                case "00": NeoVI.Debug_Message("0E.4 ESC OFF lamp : OFF"); break;
                case "01": NeoVI.Debug_Message("0E.4 ESC OFF lamp : ON"); break;
                case "11": NeoVI.Debug_Message("0E.4 ESC OFF lamp : Reserved"); break;
            }

            string Switch = H2Y.HexToBinary(Ident_80[23]);
            NeoVI.Debug_Message("0F.Switch (00=off, 01=on, 11=Reserved) : " + Switch);
            ECUs.SigPedal = Switch.Substring(4, 2);

            switch (Switch.Substring(6, 2))
            {
                case "00": NeoVI.Debug_Message("0F.1 ESC On/Off switch : OFF"); break;
                case "01": NeoVI.Debug_Message("0F.1 ESC On/Off switch : ON"); break;
                case "11": NeoVI.Debug_Message("0F.1 ESC On/Off switch : Reserved"); break;
            }
            switch (Switch.Substring(4, 2))
            {
                case "00": NeoVI.Debug_Message("0F.2 Brake light switch (Normal open) : OFF"); break;
                case "01": NeoVI.Debug_Message("0F.2 Brake light switch (Normal open) : ON"); break;
                case "11": NeoVI.Debug_Message("0F.2 Brake light switch (Normal open) : Reserved"); break;
            }
            switch (Switch.Substring(2, 2))
            {
                case "00": NeoVI.Debug_Message("0F.3 Brake switch (Normal close) : OFF"); break;
                case "01": NeoVI.Debug_Message("0F.3 Brake switch (Normal close) : ON"); break;
                case "11": NeoVI.Debug_Message("0F.3 Brake switch (Normal close) : Reserved"); break;
            }

            string Relay = H2Y.HexToBinary(Ident_80[24]);
            NeoVI.Debug_Message("10.Relay (00=off, 01=on, 11=Reserved) : " + Relay);
            ECUs.Sig_Pump = Switch.Substring(6, 2);
            ECUs.SigValve = Switch.Substring(4, 2);

            switch (Relay.Substring(6, 2))
            {
                case "00": NeoVI.Debug_Message("10.1 Motor : OFF"); break;
                case "01": NeoVI.Debug_Message("10.1 Motor : ON"); break;
                case "11": NeoVI.Debug_Message("10.1 Motor : Reserved"); break;
            }
            switch (Relay.Substring(4, 2))
            {
                case "00": NeoVI.Debug_Message("10.2 Valve : OFF"); break;
                case "01": NeoVI.Debug_Message("10.2 Valve : ON"); break;
                case "11": NeoVI.Debug_Message("10.2 Valve : Reserved"); break;
            }
            switch (Relay.Substring(2, 2))
            {
                case "00": NeoVI.Debug_Message("10.3 HAC Brake Lamp Actuator (only for system applied HAC BLA) : OFF"); break;
                case "01": NeoVI.Debug_Message("10.3 HAC Brake Lamp Actuator (only for system applied HAC BLA) : ON"); break;
                case "11": NeoVI.Debug_Message("10.3 HAC Brake Lamp Actuator (only for system applied HAC BLA) : Reserved"); break;
            }
            switch (Relay.Substring(0, 2))
            {
                case "00": NeoVI.Debug_Message("10.4 ESS Lamp (only for system applied ESS System) : OFF"); break;
                case "01": NeoVI.Debug_Message("10.4 ESS Lamp (only for system applied ESS System) : ON"); break;
                case "11": NeoVI.Debug_Message("10.4 ESS Lamp (only for system applied ESS System) : Reserved"); break;
            }

            string Pump_Motor = H2Y.HexToBinary(Ident_80[25]);
            NeoVI.Debug_Message("11.Pump Motor (bit 1,0) : " + Pump_Motor);
            switch (Pump_Motor.Substring(6, 2))
            {
                case "00": NeoVI.Debug_Message("11.1 Pump Motor : OFF (Stopped)"); break;
                case "01": NeoVI.Debug_Message("11.1 Pump Motor : ON (Running)"); break;
            }

            string IV_Valves = Ident_80[26];
            NeoVI.Debug_Message("12.ABS Valves(IV) : " + IV_Valves);

            string OV_Valves = Ident_80[27];
            NeoVI.Debug_Message("13.ABS Valves(OV) : " + OV_Valves);

            string ESC_Valves = Ident_80[28];
            NeoVI.Debug_Message("14.ESC Valves : " + ESC_Valves);

            float Steering_Angle = H2Y.HexToInt(Ident_80[29], Ident_80[30]) * 0.1f;
            NeoVI.Debug_Message("15.Steering Angle : " + Steering_Angle);

            string Steering_Status = H2Y.HexToBinary(Ident_80[31]);
            NeoVI.Debug_Message("16.Steering Angle Sensor Status : " + Steering_Status);

            switch (Steering_Status.Substring(6, 2))
            {
                case "00": NeoVI.Debug_Message("16.1 Steering Angle Sensor Status(bit1,0) : Not OK"); break;
                case "01": NeoVI.Debug_Message("16.1 Steering Angle Sensor Status(bit1,0) : OK"); break;
                case "11": NeoVI.Debug_Message("16.1 Steering Angle Sensor Status(bit1,0) : Not supported"); break;
            }
            switch (Steering_Status.Substring(4, 2))
            {
                case "00": NeoVI.Debug_Message("16.2 Steering Angle Sensor Calibration(bit3,2) : Not Calibrated"); break;
                case "01": NeoVI.Debug_Message("16.2 Steering Angle Sensor Calibration(bit3,2) : Calibrated"); break;
                case "11": NeoVI.Debug_Message("16.2 Steering Angle Sensor Calibration(bit3,2) : Not supported"); break;
            }

            float Lateral_acceleration = H2Y.HexToInt(Ident_80[32], Ident_80[33]) * 0.004f;
            NeoVI.Debug_Message("17.Yaw Rate Sensor – Lateral acceleration : " + Lateral_acceleration);

            float Yaw_Rate = H2Y.HexToInt(Ident_80[34], Ident_80[35]) * 0.2f;
            NeoVI.Debug_Message("18.Yaw Rate Sensor – Yaw rate : " + Yaw_Rate);

            float Pressure = H2Y.HexToInt(Ident_80[36], Ident_80[37]) * 0.0153f;
            NeoVI.Debug_Message("19.Pressure Sensor – Pressure : " + Pressure);

            string Parking = H2Y.HexToBinary(Ident_80[38]);
            NeoVI.Debug_Message("1A.Parking brake signal(bit1,0) : " + Parking);

            switch (Parking.Substring(6, 2))
            {
                case "00": NeoVI.Debug_Message("1A.1 Parking brake signal(bit1,0) : Released"); break;
                case "01": NeoVI.Debug_Message("1A.1 Parking brake signal(bit1,0) : Activated"); break;
                case "11": NeoVI.Debug_Message("1A.1 Parking brake signal(bit1,0) : Not supported"); break;
            }

            float Longitudinal_acceleration = H2Y.HexToInt(Ident_80[39], Ident_80[40]) * 0.0004f;
            NeoVI.Debug_Message("1B.Yaw Rate Sensor – Longitudinal acceleration : " + Longitudinal_acceleration);
        }
        //연결 유지
        public static bool Tester_Present()         //TesterPresent(3E hex) Service 테스터 존재
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("3E 00");      //(00:Response required, 80:Response not required)
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Message_Falg()           //EnableNormalMessageTransmission (29 hex) service
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("29 01");      //(01:ResponseRequired, 02:NoResponseRequired)
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }

        public static bool Dynamic_Step(int idx)    //13. Dynamic ABS/ESP Test
        {
            bool Ret = true;

            switch(idx)
            {
                case 0: Ret = Start_Communication(); break;    
                case 1: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 03 80 1E"); break;    //1. ABS Pressure release FL (600ms)  0000 0011 - 1000 0000
                case 2: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 0C 80 1E"); break;    //2. ABS Pressure release FR (600ms)  0000 1100 - 1000 0000
                case 3: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 30 80 1E"); break;    //3. ABS Pressure release RL (600ms)  0011 0000 - 1000 0000
                case 4: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 C0 80 1E"); break;    //4. ABS Pressure release RR (600ms)  1100 0000 - 1000 0000
                
                case 5: Ret = NeoVI.Ret_SendMsgs("2F F0 11 03"); break;             //5. ABS Pump Motor On for 2 Seconds
            }

            return Ret;
        }
        public static bool Dynamic_Auto()           //Dynamic Test
        {
            bool Ret = true;
            
            float T2 = 600; 
            float T3 = 2000;

            double Vlv_Time = 0;
            double Old_Time = 0;
            double Off_Time = DateTime.Now.Ticks;
            bool ECU_Flag = true;
            byte ECU_Setp = 0;

            while (true)
            {
                if (!ECU_Flag) { ECU_Flag = true; }

                Vlv_Time = TSet.ABSv_Time + ((DateTime.Now.Ticks - Off_Time) / H2Y.tick_Dvd);

                if (ECU_Flag && ECU_Setp == 0)
                {
                    Ret = Start_Communication();                    ECU_Setp = 1; ECU_Flag = false; Old_Time = Vlv_Time; 
                }
                
                if (ECU_Flag && ECU_Setp == 1)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 2; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 2)
                {
                    Ret = Dynamic_Step(1);                          ECU_Setp = 3; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 3)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 4; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 4)
                {
                    Ret = Dynamic_Step(2);                          ECU_Setp = 5; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 5)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 6; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 6)
                {
                    Ret = Dynamic_Step(3);                          ECU_Setp = 7; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 7)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 8; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 8)
                {
                    Ret = Dynamic_Step(4);                          ECU_Setp = 9; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 9)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 10; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 10)
                {
                    Ret = Dynamic_Step(5);                          ECU_Setp = 11; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 11)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T3, 1000)) {  ECU_Setp = 12; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 12)
                {
                    ECUs.ABS_Step = 5;
                    break;
                }
            }

            return Ret;
        }
        public static bool ESP_Step(int idx)        //13. Dynamic ABS/ESP Test
        {
            bool Ret = true;

            switch (idx)
            {
                case 1: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 40 85 0A"); break;    //1. ESP Pressure increase FL (200ms) 0100 0000 - 1000 0101
                case 2: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 10 8A 0A"); break;    //2. ESP Pressure increase FR (200ms) 0001 0000 - 1000 1010
            }

            return Ret;
        }

        public static bool ESS_LampTest()           //10 ESS Lamp(Option) 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("2F F0 24 03");
            if (Ret) { ECUs.ESS_Lamp = NeoVI.Get_Data; }

            return Ret;
        }
        #endregion

        public static string ret_DTCs(string pCode)
        {
            string ret_Msgs = "";

            switch (pCode)
            {
                #region ABS
                case "C110101": ret_Msgs = "ECU voltage supply- high voltage"; break;                                   //고전압 불량
                case "C110201": ret_Msgs = "ECU voltage supply-low voltage"; break;                                     //저전압 불량
                case "C120001": ret_Msgs = "Wheel-speed sensor, front left: open/short"; break;                         //앞좌측 휠센서 – 단선/단락
                case "C120102": ret_Msgs = "Wheel-speed sensor, front left: range, performance, intermittent"; break;   //앞좌측 휠센서 – 신호불량
                case "C120202": ret_Msgs = "Wheel-speed sensor, front left: invalid/no signal"; break;                  //앞좌측 휠센서 – 에어갭 불량
                case "C120301": ret_Msgs = "Wheel-speed sensor, front right: open/short"; break;                        //앞우측 휠센서 – 단선/단락
                case "C120402": ret_Msgs = "Wheel-speed sensor, front right: range, performance, intermittent"; break;  //앞우측 휠센서 – 신호불량
                case "C120502": ret_Msgs = "Wheel-speed sensor, front right: invalid/no signal"; break;                 //앞우측 휠센서 – 에어갭 불량
                case "C120601": ret_Msgs = "Wheel-speed sensor, rear left: open/short"; break;                          //뒤좌측 휠센서 – 단선/단락
                case "C120702": ret_Msgs = "Wheel-speed sensor, rear left: range, performance, intermittent"; break;    //뒤좌측 휠센서 – 신호불량
                case "C120802": ret_Msgs = "Wheel-speed sensor, rear left: invalid/no signal"; break;                   //뒤좌측 휠센서 – 에어갭 불량
                case "C120901": ret_Msgs = "Wheel-speed sensor, rear right: open/short"; break;                         //뒤우측 휠센서 – 단선/단락
                case "C121002": ret_Msgs = "Wheel-speed sensor, rear right: range, performance, intermittent"; break;   //뒤우측 휠센서 – 신호불량
                case "C121102": ret_Msgs = "Wheel-speed sensor, rear right: invalid/no signal"; break;                  //뒤우측 휠센서 – 에어갭 불량
                case "C121302": ret_Msgs = "Wheel-speed sensor frequency error"; break;                                 //휠센서 주파수 이상
                case "C154201": ret_Msgs = "Brake Light Switch error"; break;                                           //브레이크 라이트 스위치 신호 불량
                case "C160404": ret_Msgs = "ECU hardware error"; break;                                                 //ECU 하드웨어 불량
                case "C211201": ret_Msgs = "Valve relay error-electrical"; break;                                       //밸브 릴레이 퓨즈 단선/단락
                case "C213001": ret_Msgs = "BLA Open / Short Error"; break;                                             //BLA 단선/단락 불량
                case "C213101": ret_Msgs = "ESS Relay Open/Short"; break;                                               //ESS 릴레이 단선/단락 불량
                case "C238001": ret_Msgs = "Solenoid Valve fault"; break;                                               //밸브 불량
                case "C240201": ret_Msgs = "Return pump fault (motor electrical)"; break;                               //펌프 모터 고장(퓨즈 단선/단락)
                #endregion
                #region ESC
                case "C123501": ret_Msgs = "Pressure sensor fault: electrical"; break;                                  //압력 센서 고장 – 단선/단락
                case "C123702": ret_Msgs = "Pressure sensor fault: signal error"; break;                                //압력 센서 고장 – 신호 이상
                case "C126002": ret_Msgs = "Steering angle sensor : signal error"; break;                               //조향각 센서 – 단선/단락
                case "C126104": ret_Msgs = "Steering angle sensor : not calibrated"; break;                             //조향각 센서 – 영점 설정 안됨
                case "C128302": ret_Msgs = "Lateral G sensor/Yaw rate sensor: signal error"; break;                     //횡 가속도 센서/요레이트 센서 – 신호 이상
                case "C128504": ret_Msgs = "AX calibration Error"; break;                                               //AX 영점조정 불량
                case "C128602": ret_Msgs = "IMU YRS signal error"; break;                                               //IMU YRS – 신호 이상
                case "C135801": ret_Msgs = "AVH Switch open/short"; break;                                              //AVH 스위치 단선/단락
                case "C150301": ret_Msgs = "TCS/ESC switch error"; break;                                               //TCS/ESC 스위치 이상
                case "C152001": ret_Msgs = "Clutch Signal error"; break;                                                //클러치 신호 불량
                case "C152601": ret_Msgs = "DBC Switch Error"; break;                                                   //DBC 스위치 이상
                case "C152701": ret_Msgs = "Reverse Gear Signal error"; break;                                          //후진 기어 신호 불량
                case "C160508": ret_Msgs = "CAN send fault"; break;                                                     //CAN 송신 불량
                case "C160E08": ret_Msgs = "P-CAN Bus off Error"; break;                                                //P-CAN 통신 불량
                case "C161108": ret_Msgs = "CAN time out EMS"; break;                                                   //EMS 측 CAN 신호 안나옴
                case "C161208": ret_Msgs = "CAN time out TCU"; break;                                                   //TCU 측 CAN 신호 안나옴
                case "C161308": ret_Msgs = "Wrong EMS CAN message"; break;                                              //EMS CAN 메시지 이상
                case "C161608": ret_Msgs = "C-CAN Bus off error"; break;                                                //C-CAN 통신 불량
                case "C162308": ret_Msgs = "CAN time out SAS"; break;                                                   //조향각 센서 측 신호 안나옴
                case "C162604": ret_Msgs = "ESC Implausible Control"; break;                                            //ESC 이상 작동
                case "C163808": ret_Msgs = "CAN timeout SCC"; break;                                                    //SCC 측 CAN 신호 안나옴
                case "C164308": ret_Msgs = "CAN time out YRS"; break;                                                   //요센서 측 CAN 신호 안나옴
                case "C164908": ret_Msgs = "CAN timeout/signal error EMS15"; break;                                     //EMS15 신호 안나옴/신호이상
                case "C165008": ret_Msgs = "Wrong SCC CAN message"; break;                                              //SCC CAN 메시지 이상
                case "C165108": ret_Msgs = "CAN timeout EPB"; break;                                                    //EPB 측 CAN 신호 안나옴
                case "C165208": ret_Msgs = "Wrong EPB CAN message"; break;                                              //EPB CAN 메시지 이상
                case "C168708": ret_Msgs = "VSM2(MDPS12) Message Timeout "; break;                                      //VSM2(MDPS12) 신호 없음
                case "C168808": ret_Msgs = "VSM2(MDPS12) Signal Error"; break;                                          //VSM2(MDPS12) 신호 이상
                case "C170204": ret_Msgs = "Variant coding error"; break;                                               //사양 설정 이상
                case "C181208": ret_Msgs = "CAN timeout CGW"; break;                                                    //CGW 측 CAN 신호 안나옴
                case "C181887": ret_Msgs = "CAN Timeout BCA"; break;                                                    //BCA 측 CAN 신호 안나옴
                case "C183786": ret_Msgs = "BCA Signal Error"; break;                                                   //BCA 신호 이상
                case "C222798": ret_Msgs = "DBC OverHeated Brake"; break;                                               //DBC 브레이크 과열
                case "C222808": ret_Msgs = "TCU message gear signal fault"; break;                                      //TCU 메시지 기어 신호이상
                case "C162708": ret_Msgs = "4WD Message timeout"; break;                                                //4WD측 CAN 신호 안 나옴
                case "C16B687": ret_Msgs = "AEB Message timeout"; break;                                                //AEB측 CAN 신호 안 나옴
                case "C16B781": ret_Msgs = "AEB Signal Error"; break;                                                   //AEB 신호 이상
                #endregion
            }

            return ret_Msgs;
        }
        public static string ret_Errs(string pErr)
        {
            string ret_Msgs = "";

            switch (pErr)
            {
                case "10": ret_Msgs ="GeneralReject"; break;
                case "11": ret_Msgs ="ServiceNotSupported"; break;
                case "12": ret_Msgs ="SubFunctionNotSupported"; break;
                case "13": ret_Msgs ="IncorrectMessageLengthOrInvalidFormat"; break;
                case "14": ret_Msgs ="RespondeTooLong"; break;
                case "21": ret_Msgs ="Busy RepeatRequest"; break;
                case "22": ret_Msgs ="ConditionsNotCorrect"; break;
                case "24": ret_Msgs ="RequestSequenceError"; break;
                case "31": ret_Msgs ="RequestOutOfRange"; break;
                case "33": ret_Msgs ="SecurityAccessDenied"; break;
                case "35": ret_Msgs ="InvalidKey"; break;
                case "36": ret_Msgs ="ExceedNumberOfAttempts"; break;
                case "37": ret_Msgs ="RequiredTimeDelayNotExpired"; break;
                case "70": ret_Msgs ="UploadDownNotAccepted"; break;
                case "71": ret_Msgs ="TransferSuspended"; break;
                case "72": ret_Msgs ="GeneralProgrammingFailure"; break;
                case "73": ret_Msgs ="WrongBlockSequenceCounter"; break;
                case "78": ret_Msgs ="ReqCorrectlyRcvd-RspPending (requestCorrectlyReceived-ResponsePending)"; break;
                case "7E": ret_Msgs ="SubFunctionNotSupportedInActiveSession"; break;
                case "7F": ret_Msgs ="ServiceNotSupportedInActiveSession"; break;
                case "80": ret_Msgs ="ServiceNotSupportedInActiveDiagnosticMode"; break;
                case "81": ret_Msgs ="RpmTooHigh"; break;
                case "82": ret_Msgs ="RpmTooLow"; break;
                case "83": ret_Msgs ="EngineIsRunning"; break;
                case "84": ret_Msgs ="EngineIsNotRunning"; break;
                case "85": ret_Msgs ="EngineRunTimeTooLow"; break;
                case "86": ret_Msgs ="TemperatureTooHigh"; break;
                case "87": ret_Msgs ="TemperatureTooLow"; break;
                case "88": ret_Msgs ="VehicleSpeedTooHigh"; break;
                case "89": ret_Msgs ="VehicleSpeedTooLow"; break;
                case "8A": ret_Msgs ="Throttle/PedalTooHigh"; break;
                case "8B": ret_Msgs ="Throttle/PedalTooLow"; break;
                case "8C": ret_Msgs ="TransmissionRangeNotInNeutral"; break;
                case "8D": ret_Msgs ="TransmissionRangeNotInGear"; break;
                case "8F": ret_Msgs ="BrakeSwitchNotClosed"; break;
                case "90": ret_Msgs ="ShiftLeverNotInPark"; break;
                case "91": ret_Msgs ="TorqueConverterClutchLocked"; break;
                case "92": ret_Msgs ="VoltageTooHigh"; break;
                case "93": ret_Msgs = "VoltageTooLow"; break;
            }

            return ret_Msgs;
        }
    }

    public static class MOBIS_DN8
    {
        #region Variable declaration
        public static string Veh_Name;
        public static string Sys_Name;
        public static string Var_Code;
        public static string HW__Vers;
        public static string SW__Vers;
        public static string BCD_Date;
        public static string Part_Num;
        #endregion

        #region Standard CAN
        public static bool Start_Communication()    //1 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("10 03");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Stop_Communication()     //2 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("20");
            if (Ret) { ECUs.End_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Reset()              //3 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("11 01");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Identification()     //4 
        {
            bool Ret = true;

            #region Identification Clear
            Veh_Name = "";
            Sys_Name = "";
            Var_Code = "";
            HW__Vers = "";
            SW__Vers = "";
            BCD_Date = "";
            Part_Num = "";
            #endregion

            try
            {
                if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 00"); }
                if (Ret) { Identification(NeoVI.Get_Data); }
            }
            catch (Exception ex)
            {
                Ret = false;
            }

            return Ret;
        }
        public static void Identification(string pData) //4.1 
        {
            string[] Ident_80 = pData.Split(' ');

            if (Ident_80.Length < 30) return;

            Veh_Name = H2Y.HexToASCII(Ident_80[4])
                     + H2Y.HexToASCII(Ident_80[5]);
            //Ident_80[6]
            Sys_Name = H2Y.HexToASCII(Ident_80[7])
                     + H2Y.HexToASCII(Ident_80[8])
                     + H2Y.HexToASCII(Ident_80[9]);
            //Ident_80[10]
            Var_Code = Ident_80[11];
            //Ident_80[12]
            HW__Vers = H2Y.HexToASCII(Ident_80[13]);
            SW__Vers = H2Y.HexToASCII(Ident_80[14]);
            //Ident_80[15]
            BCD_Date = Ident_80[16] + "/" + Ident_80[17] + "/" + Ident_80[18];
            //Ident_80[19]
            Part_Num = H2Y.HexToASCII(Ident_80[20])
                     + H2Y.HexToASCII(Ident_80[21])
                     + H2Y.HexToASCII(Ident_80[22])
                     + H2Y.HexToASCII(Ident_80[23])
                     + H2Y.HexToASCII(Ident_80[24])
                     + H2Y.HexToASCII(Ident_80[25])
                     + H2Y.HexToASCII(Ident_80[26])
                     + H2Y.HexToASCII(Ident_80[27])
                     + H2Y.HexToASCII(Ident_80[28])
                     + H2Y.HexToASCII(Ident_80[29])
                     + H2Y.HexToASCII(Ident_80[30]);

            ECUs.Veh_Name = Veh_Name;
            ECUs.Sys_Name = Sys_Name;
            ECUs.Var_Code = Var_Code;
            ECUs.HW__Vers = HW__Vers;
            ECUs.SW__Vers = SW__Vers;
            ECUs.BCD_Date = BCD_Date;
            ECUs.Part_Num = Part_Num;

            NeoVI.Debug_Message("1.Vehicle Name  : " + ECUs.Veh_Name);
            NeoVI.Debug_Message("2.System Name : " + ECUs.Sys_Name);
            NeoVI.Debug_Message("3.Variant code : " + ECUs.Var_Code);
            NeoVI.Debug_Message("4.H/W version : " + ECUs.HW__Vers);
            NeoVI.Debug_Message("5.S/W version : " + ECUs.SW__Vers);
            NeoVI.Debug_Message("6.Release date : " + ECUs.BCD_Date);
            NeoVI.Debug_Message("7.HMC/KMC part number : " + ECUs.Part_Num);
        }
        public static bool Read__DTC()              //5 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("19 02 08");
            if (Ret) { ECUs.DTC_Read = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Clear_DTC()              //6 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("14 FF FF FF");
            if (Ret) { ECUs.DTCClear = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Check_Signals()          //7 
        {
            bool Ret = true;

            #region Signals Clear

            #endregion

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 01 04"); }

            if (Ret)
            {
                Service_Data_0104(NeoVI.Get_Data);

                NeoVI.Debug_Message("1.Brake Light Switch : " + ECUs.SigPedal);
                NeoVI.Debug_Message("2.Valve relay Switch : " + ECUs.SigValve);
                NeoVI.Debug_Message("3.Pump Motor Status  : " + ECUs.Sig_Pump);
            }
            return Ret;
        }
        public static bool WSS_Test()               //8 
        {
            bool Ret = true;

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 01 04"); }  //FL:4B 00 
            if (Ret)
            {
                Service_WSS_0104(NeoVI.Get_Data);

                NeoVI.Debug_Message("WSS FL : " + ECUs.WSS_FL);
                NeoVI.Debug_Message("WSS FR : " + ECUs.WSS_FR);
                NeoVI.Debug_Message("WSS RL : " + ECUs.WSS_RL);
                NeoVI.Debug_Message("WSS RR : " + ECUs.WSS_RR);
            }

            return Ret;
        }
        private static void Service_WSS_0104(string pData)
        {
            string[] Ident_80 = pData.Split(' ');

            if (Ident_80.Length < 26) return;

            ECUs.WSS_FL = H2Y.HexTobyte(Ident_80[15]);
            ECUs.WSS_FR = H2Y.HexTobyte(Ident_80[16]);
            ECUs.WSS_RL = H2Y.HexTobyte(Ident_80[17]);
            ECUs.WSS_RR = H2Y.HexTobyte(Ident_80[18]);
        }
        private static void Service_Data_0104(string pData)
        {
            string[] Ident_80 = pData.Split(' ');

            if (Ident_80.Length < 26) return;

            string PID_Data = H2Y.HexToASCII(Ident_80[4])
                            + H2Y.HexToASCII(Ident_80[5])
                            + H2Y.HexToASCII(Ident_80[6])
                            + H2Y.HexToASCII(Ident_80[7]);
            NeoVI.Debug_Message("00.Supported PID:0x01~0x20 : " + PID_Data);

            float RPM_Data = H2Y.HexToInt(Ident_80[8], Ident_80[9]) * 0.25f;
            NeoVI.Debug_Message("01.Engine RPM  : " + RPM_Data);

            byte VehSpeed = H2Y.HexTobyte(Ident_80[10]);
            NeoVI.Debug_Message("02.Vehicle speed : " + VehSpeed);

            float Absolute = H2Y.HexTobyte(Ident_80[11]) * (100 / 255);
            NeoVI.Debug_Message("03.Absolute Throttle position sensor : " + Absolute);

            string Shift_lever = H2Y.HexToBinary(Ident_80[12]);
            NeoVI.Debug_Message("04.Shift lever position : " + Shift_lever);

            float ECU_Volt = H2Y.HexTobyte(Ident_80[13]) * (16 / 255);
            NeoVI.Debug_Message("05.ECU supply voltage : " + ECU_Volt);

            float Ref5Volt = H2Y.HexTobyte(Ident_80[14]);
            NeoVI.Debug_Message("06. 5 Volt reference : " + Ref5Volt);

            ECUs.WSS_FL = H2Y.HexTobyte(Ident_80[15]);
            ECUs.WSS_FR = H2Y.HexTobyte(Ident_80[16]);
            ECUs.WSS_RL = H2Y.HexTobyte(Ident_80[17]);
            ECUs.WSS_RR = H2Y.HexTobyte(Ident_80[18]);

            string Steering = Ident_80[19];
            NeoVI.Debug_Message("0B.Steering Sensor (00=low, 01=high, 11=Reserved) : " + Steering);

            string Longitudinal = Ident_80[20];
            NeoVI.Debug_Message("0C.G Sensor – Longitudinal (4WD only) : " + Longitudinal);

            string Lateral = Ident_80[21];
            NeoVI.Debug_Message("0D.G Sensor – Lateral (ESC only) : " + Lateral);

            string Warning = H2Y.HexToBinary(Ident_80[22]);
            NeoVI.Debug_Message("0E.Warning lamp (00=off, 01=on, 11=Reserved) : " + Warning);

            switch (Warning.Substring(6, 2))
            {
                case "00": NeoVI.Debug_Message("0E.1 ABS lamp : OFF"); break;
                case "01": NeoVI.Debug_Message("0E.1 ABS lamp : ON"); break;
                case "11": NeoVI.Debug_Message("0E.1 ABS lamp : Reserved"); break;
            }
            switch (Warning.Substring(4, 2))
            {
                case "00": NeoVI.Debug_Message("0E.2 EBD lamp : OFF"); break;
                case "01": NeoVI.Debug_Message("0E.2 EBD lamp : ON"); break;
                case "11": NeoVI.Debug_Message("0E.2 EBD lamp : Reserved"); break;
            }
            switch (Warning.Substring(2, 2))
            {
                case "00": NeoVI.Debug_Message("0E.3 ESC lamp : OFF"); break;
                case "01": NeoVI.Debug_Message("0E.3 ESC lamp : ON"); break;
                case "11": NeoVI.Debug_Message("0E.3 ESC lamp : Reserved"); break;
            }
            switch (Warning.Substring(0, 2))
            {
                case "00": NeoVI.Debug_Message("0E.4 ESC OFF lamp : OFF"); break;
                case "01": NeoVI.Debug_Message("0E.4 ESC OFF lamp : ON"); break;
                case "11": NeoVI.Debug_Message("0E.4 ESC OFF lamp : Reserved"); break;
            }

            string Switch = H2Y.HexToBinary(Ident_80[23]);
            NeoVI.Debug_Message("0F.Switch (00=off, 01=on, 11=Reserved) : " + Switch);
            ECUs.SigPedal = Switch.Substring(5, 2);

            switch (Switch.Substring(6, 2))
            {
                case "00": NeoVI.Debug_Message("0F.1 ESC On/Off switch : OFF"); break;
                case "01": NeoVI.Debug_Message("0F.1 ESC On/Off switch : ON"); break;
                case "11": NeoVI.Debug_Message("0F.1 ESC On/Off switch : Reserved"); break;
            }
            switch (Switch.Substring(4, 2))
            {
                case "00": NeoVI.Debug_Message("0F.2 Brake light switch (Normal open) : OFF"); break;
                case "01": NeoVI.Debug_Message("0F.2 Brake light switch (Normal open) : ON"); break;
                case "11": NeoVI.Debug_Message("0F.2 Brake light switch (Normal open) : Reserved"); break;
            }
            switch (Switch.Substring(2, 2))
            {
                case "00": NeoVI.Debug_Message("0F.3 Brake switch (Normal close) : OFF"); break;
                case "01": NeoVI.Debug_Message("0F.3 Brake switch (Normal close) : ON"); break;
                case "11": NeoVI.Debug_Message("0F.3 Brake switch (Normal close) : Reserved"); break;
            }

            string Relay = H2Y.HexToBinary(Ident_80[24]);
            NeoVI.Debug_Message("10.Relay (00=off, 01=on, 11=Reserved) : " + Relay);
            ECUs.Sig_Pump = Switch.Substring(6, 2);
            ECUs.SigValve = Switch.Substring(4, 2);

            switch (Relay.Substring(6, 2))
            {
                case "00": NeoVI.Debug_Message("10.1 Motor : OFF"); break;
                case "01": NeoVI.Debug_Message("10.1 Motor : ON"); break;
                case "11": NeoVI.Debug_Message("10.1 Motor : Reserved"); break;
            }
            switch (Relay.Substring(4, 2))
            {
                case "00": NeoVI.Debug_Message("10.2 Valve : OFF"); break;
                case "01": NeoVI.Debug_Message("10.2 Valve : ON"); break;
                case "11": NeoVI.Debug_Message("10.2 Valve : Reserved"); break;
            }
            switch (Relay.Substring(2, 2))
            {
                case "00": NeoVI.Debug_Message("10.3 HAC Brake Lamp Actuator (only for system applied HAC BLA) : OFF"); break;
                case "01": NeoVI.Debug_Message("10.3 HAC Brake Lamp Actuator (only for system applied HAC BLA) : ON"); break;
                case "11": NeoVI.Debug_Message("10.3 HAC Brake Lamp Actuator (only for system applied HAC BLA) : Reserved"); break;
            }
            switch (Relay.Substring(0, 2))
            {
                case "00": NeoVI.Debug_Message("10.4 ESS Lamp (only for system applied ESS System) : OFF"); break;
                case "01": NeoVI.Debug_Message("10.4 ESS Lamp (only for system applied ESS System) : ON"); break;
                case "11": NeoVI.Debug_Message("10.4 ESS Lamp (only for system applied ESS System) : Reserved"); break;
            }

            string Pump_Motor = H2Y.HexToBinary(Ident_80[25]);
            NeoVI.Debug_Message("11.Pump Motor (bit 1,0) : " + Pump_Motor);
            switch (Pump_Motor.Substring(6, 2))
            {
                case "00": NeoVI.Debug_Message("11.1 Pump Motor : OFF (Stopped)"); break;
                case "01": NeoVI.Debug_Message("11.1 Pump Motor : ON (Running)"); break;
            }

            string IV_Valves = Ident_80[26];
            NeoVI.Debug_Message("12.ABS Valves(IV) : " + IV_Valves);

            string OV_Valves = Ident_80[27];
            NeoVI.Debug_Message("13.ABS Valves(OV) : " + OV_Valves);

            string ESC_Valves = Ident_80[28];
            NeoVI.Debug_Message("14.ESC Valves : " + ESC_Valves);

            float Steering_Angle = H2Y.HexToInt(Ident_80[29], Ident_80[30]) * 0.1f;
            NeoVI.Debug_Message("15.Steering Angle : " + Steering_Angle);

            string Steering_Status = H2Y.HexToBinary(Ident_80[31]);
            NeoVI.Debug_Message("16.Steering Angle Sensor Status : " + Steering_Status);

            switch (Steering_Status.Substring(6, 2))
            {
                case "00": NeoVI.Debug_Message("16.1 Steering Angle Sensor Status(bit1,0) : Not OK"); break;
                case "01": NeoVI.Debug_Message("16.1 Steering Angle Sensor Status(bit1,0) : OK"); break;
                case "11": NeoVI.Debug_Message("16.1 Steering Angle Sensor Status(bit1,0) : Not supported"); break;
            }
            switch (Steering_Status.Substring(4, 2))
            {
                case "00": NeoVI.Debug_Message("16.2 Steering Angle Sensor Calibration(bit3,2) : Not Calibrated"); break;
                case "01": NeoVI.Debug_Message("16.2 Steering Angle Sensor Calibration(bit3,2) : Calibrated"); break;
                case "11": NeoVI.Debug_Message("16.2 Steering Angle Sensor Calibration(bit3,2) : Not supported"); break;
            }

            float Lateral_acceleration = H2Y.HexToInt(Ident_80[32], Ident_80[33]) * 0.004f;
            NeoVI.Debug_Message("17.Yaw Rate Sensor – Lateral acceleration : " + Lateral_acceleration);

            float Yaw_Rate = H2Y.HexToInt(Ident_80[34], Ident_80[35]) * 0.2f;
            NeoVI.Debug_Message("18.Yaw Rate Sensor – Yaw rate : " + Yaw_Rate);

            float Pressure = H2Y.HexToInt(Ident_80[36], Ident_80[37]) * 0.0153f;
            NeoVI.Debug_Message("19.Pressure Sensor – Pressure : " + Pressure);

            string Parking = H2Y.HexToBinary(Ident_80[38]);
            NeoVI.Debug_Message("1A.Parking brake signal(bit1,0) : " + Parking);

            switch (Parking.Substring(6, 2))
            {
                case "00": NeoVI.Debug_Message("1A.1 Parking brake signal(bit1,0) : Released"); break;
                case "01": NeoVI.Debug_Message("1A.1 Parking brake signal(bit1,0) : Activated"); break;
                case "11": NeoVI.Debug_Message("1A.1 Parking brake signal(bit1,0) : Not supported"); break;
            }

            float Longitudinal_acceleration = H2Y.HexToInt(Ident_80[39], Ident_80[40]) * 0.0004f;
            NeoVI.Debug_Message("1B.Yaw Rate Sensor – Longitudinal acceleration : " + Longitudinal_acceleration);
        }
        
        //연결 유지
        public static bool Tester_Present()         //TesterPresent(3E hex) Service 테스터 존재
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("3E 00");      //(00:Response required, 80:Response not required)
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Message_Falg()           //EnableNormalMessageTransmission (29 hex) service
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("29 01");      //(01:ResponseRequired, 02:NoResponseRequired)
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }

        public static bool Dynamic_Step(int idx)    //Dynamic ABS Test
        {
            bool Ret = true;

            switch (idx)
            {
                case 0: Ret = Start_Communication(); break;    
                case 1: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 03 80 1E"); break;    //1. ABS Pressure release FL (600ms)  0000 0011 - 1000 0000
                case 2: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 0C 80 1E"); break;    //2. ABS Pressure release FR (600ms)  0000 1100 - 1000 0000
                case 3: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 30 80 1E"); break;    //3. ABS Pressure release RL (600ms)  0011 0000 - 1000 0000
                case 4: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 C0 80 1E"); break;    //4. ABS Pressure release RR (600ms)  1100 0000 - 1000 0000

                case 5: Ret = NeoVI.Ret_SendMsgs("2F F0 11 03"); break;             //5. ABS Pump Motor On for 2 Seconds
            }

            return Ret;
        }
        public static bool Dynamic_Auto()           //Dynamic Test
        {
            bool Ret = true;

            float T2 = 600;
            float T3 = 2000;

            double Vlv_Time = 0;
            double Old_Time = 0;
            double Off_Time = DateTime.Now.Ticks;
            bool ECU_Flag = true;
            byte ECU_Setp = 0;

            while (true)
            {
                if (!ECU_Flag) { ECU_Flag = true; }

                Vlv_Time = TSet.ABSv_Time + ((DateTime.Now.Ticks - Off_Time) / H2Y.tick_Dvd);

                if (ECU_Flag && ECU_Setp == 0)
                {
                    Ret = Start_Communication();                    ECU_Setp = 1; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 1)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 2; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 2)
                {
                    Ret = Dynamic_Step(1);                          ECU_Setp = 3; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 3)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 4; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 4)
                {
                    Ret = Dynamic_Step(2);                          ECU_Setp = 5; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 5)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 6; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 6)
                {
                    Ret = Dynamic_Step(3);                          ECU_Setp = 7; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 7)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 8; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 8)
                {
                    Ret = Dynamic_Step(4);                          ECU_Setp = 9; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 9)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 10; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 10)
                {
                    Ret = Dynamic_Step(5);                          ECU_Setp = 11; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 11)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T3, 1000)) { ECU_Setp = 12; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 12)
                {
                    ECUs.ABS_Step = 5;
                    break;
                }
            }

            return Ret;
        }
        public static bool ESP_Step(int idx)        //Dynamic ESP Test
        {
            bool Ret = true;

            switch (idx)
            {
                case 1: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 40 85 0A"); break;    //1. ESP Pressure increase FL (200ms) 0100 0000 - 1000 0101
                case 2: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 10 8A 0A"); break;    //2. ESP Pressure increase FR (200ms) 0001 0000 - 1000 1010
            }

            return Ret;
        }

        public static bool ESS_LampTest()           //10 ESS Lamp(Option) 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("2F F0 24 03");
            if (Ret) { ECUs.ESS_Lamp = NeoVI.Get_Data; }

            return Ret;
        }
        #endregion

        public static string ret_DTCs(string pCode)
        {
            string ret_Msgs = "";

            switch (pCode)
            {
                #region ABS
                case "C110113": ret_Msgs = "ECU voltage supply- high voltage"; break;                                   //고전압 불량
                case "C110213": ret_Msgs = "ECU voltage supply-low voltage"; break;                                     //저전압 불량
                case "C120001": ret_Msgs = "Wheel-speed sensor, front left: open/short"; break;                         //앞좌측 휠센서 - 단선/단락
                case "C120102": ret_Msgs = "Wheel-speed sensor, front left: range, performance, intermittent"; break;   //앞좌측 휠센서 – 신호불량
                case "C120202": ret_Msgs = "Wheel-speed sensor, front left: invalid/no signa"; break;                   //앞좌측 휠센서 – 에어갭 불량
                case "C120301": ret_Msgs = "Wheel-speed sensor, front right: open/short"; break;                        //앞우측 휠센서 - 단선/단락
                case "C120402": ret_Msgs = "Wheel-speed sensor, front right: range, performance, intermittent"; break;  //앞우측 휠센서 - 신호불량
                case "C120502": ret_Msgs = "Wheel-speed sensor, front right: invalid/no signal"; break;                 //앞우측 휠센서 - 에어갭 불량
                case "C120601": ret_Msgs = "Wheel-speed sensor, rear left: open/short"; break;                          //뒤좌측 휠센서 - 단선/단락
                case "C120702": ret_Msgs = "Wheel-speed sensor, rear left: range, performance, intermittent"; break;    //뒤좌측 휠센서 - 신호불량
                case "C120802": ret_Msgs = "Wheel-speed sensor, rear left: invalid/no signal"; break;                   //뒤좌측 휠센서 - 에어갭 불량
                case "C120901": ret_Msgs = "Wheel-speed sensor, rear right: open/short"; break;                         //뒤우측 휠센서 - 단선/단락
                case "C121002": ret_Msgs = "Wheel-speed sensor, rear right: range, performance, intermittent"; break;   //뒤우측 휠센서 - 신호불량
                case "C121102": ret_Msgs = "Wheel-speed sensor, rear right: invalid/no signal"; break;                  //뒤우측 휠센서 - 에어갭 불량
                case "C121302": ret_Msgs = "Wheel-speed sensor frequency error"; break;                                 //휠센서 주파수 이상
                case "C151301": ret_Msgs = "Brake Light Switch error"; break;                                           //브레이크 라이트 스위치 신호 불량
                case "C160404": ret_Msgs = "ECU hardware error"; break;                                                 //ECU 하드웨어 불량
                case "C211201": ret_Msgs = "Valve relay error-electrical"; break;                                       //밸브 릴레이 퓨즈 단선/단락
                case "C213001": ret_Msgs = "BLA Open / Short Error"; break;                                             //BLA 단선/단락 불량
                case "C213101": ret_Msgs = "ESS Relay Open/Short"; break;                                               //ESS 릴레이 단선/단락 불량
                case "C238001": ret_Msgs = "Solenoid Valve fault"; break;                                               //밸브 불량
                case "C240201": ret_Msgs = "Return pump fault (motor electrical)"; break;                               //펌프 모터 고장(퓨즈 단선/단락)
                #endregion
                #region ESC
                case "C110101": ret_Msgs = "IGN Over Voltage Error"; break;                                             //IGN 과전압 오류
                case "C110201": ret_Msgs = "IGN Under Voltage Error"; break;                                            //IGN 저전압 오류
                case "C111301": ret_Msgs = "LVA Vacuum Sensor Supply fault"; break;                                     //LVA 센서 전원 이상
                case "C110913": ret_Msgs = "IGN Signal Mis-match"; break;                                               //IGN 신호 오류
                case "C117202": ret_Msgs = "LVA Vacuum – Low VAC"; break;                                               //LVA Vacuum – VAC 낮음
                case "C117501": ret_Msgs = "LVA Vacuum Sensor Fault"; break;                                            //LVA 센서 이상
                case "C123501": ret_Msgs = "Pressure sensor fault: electrical, signal"; break;                          //압력 센서 고장 – 단선/단락/신호이상
                case "C123702": ret_Msgs = "Pressure sensor Offset fault"; break;                                       //압력 센서 고장 – 오프셋 이상
                case "C126002": ret_Msgs = "Steering angle sensor : signal error"; break;                               //조향각 센서 – 단선/단락
                case "C126104": ret_Msgs = "Steering angle sensor : not calibrated"; break;                             //조향각 센서 - 영점 설정 안됨
                case "C126402": ret_Msgs = "Steering angle Model Error"; break;                                         //조향각 센서 – 모델 에러
                case "C128302": ret_Msgs = "Ax/Lateral G sensor/Yaw rate sensor: signal error"; break;                  //Ax/횡 가속도 센서/요레이트 센서 - 신호 이상
                case "C128504": ret_Msgs = "AX calibration Error"; break;                                               //AX 영점조정 불량
                case "C128602": ret_Msgs = "ACU YRS Status Error"; break;                                               //ACU YRS 상태 이상
                case "C135801": ret_Msgs = "AVH Switch open/short"; break;                                              //AVH 스위치 단선/단락
                case "C138501": ret_Msgs = "LVA Vacuum Sensor Circuit Open/Short"; break;                               //LVA Vaccum 센서 회로 단선/단락
                case "C138602": ret_Msgs = "LVA Vacuum Sensor Noise Occurance"; break;                                  //LVA Vacuum 센서 노이즈 발생
                case "C150301": ret_Msgs = "TCS/ESC switch error"; break;                                               //TCS/ESC 스위치 이상
                case "C153901": ret_Msgs = "Neutral Gear Switch Fault"; break;                                          //중립 기어 스위치 오류
                case "C154201": ret_Msgs = "Brake Light Switch error"; break;                                           //브레이크 라이트 스위치 신호 불량
                case "C152001": ret_Msgs = "Clutch Signal error"; break;                                                //클러치 신호 불량
                case "C152601": ret_Msgs = "DBC Switch Error"; break;                                                   //DBC 스위치 오류
                case "C152701": ret_Msgs = "Reverse Gear Signal error"; break;                                          //후진 기어 신호 불량
                case "C160496": ret_Msgs = "ECU Fault (Component Internal Failure)"; break;                             //ECU 내부 고장
                case "C160E08": ret_Msgs = "P-CAN Bus off error"; break;                                                //P-CAN 통신 불량
                case "C161108": ret_Msgs = "CAN time out EMS"; break;                                                   //EMS 측 CAN 신호 안나옴
                case "C161208": ret_Msgs = "CAN time out TCU"; break;                                                   //TCU 측 CAN 신호 안나옴
                case "C161308": ret_Msgs = "Wrong EMS CAN message"; break;                                              //EMS CAN 메시지 이상
                case "C161608": ret_Msgs = "C-CAN Bus off error"; break;                                                //C-CAN 통신 불량
                case "C161B08": ret_Msgs = "TCU_DCT3 CAN Message Fault"; break;                                         //TCU_DCT3 CAN 메시지 오류
                case "C162308": ret_Msgs = "CAN time out SAS"; break;                                                   //조향각 센서 측 신호 안나옴
                case "C162A87": ret_Msgs = "CLU13 Timeout"; break;                                                      //CLU13 CAN 신호 안나옴
                case "C162604": ret_Msgs = "ESC Implausible Control"; break;                                            //ESC 이상 작동
                case "C162708": ret_Msgs = "4WD Message timeout"; break;                                                //4WD측 CAN 신호 안 나옴
                case "C163808": ret_Msgs = "CAN timeout SCC"; break;                                                    //SCC 측 CAN 신호 안나옴
                case "C163A87": ret_Msgs = "ECL CAN Timeout"; break;                                                    //ECL 신호 안나옴
                case "C163B86": ret_Msgs = "ECL Signal Error"; break;                                                   //ECL 신호 이상
                case "C164308": ret_Msgs = "CAN time out YRS"; break;                                                   //요센서 측 CAN 신호 안나옴
                case "C164908": ret_Msgs = "EMS15 Signal Error"; break;                                                 //EMS15 신호 이상
                case "C165008": ret_Msgs = "Wrong SCC CAN message"; break;                                              //SCC CAN 메시지 이상
                case "C165A87": ret_Msgs = "RCS Timeout"; break;                                                        //RCS 측 CAN 신호 안나옴
                case "C168708": ret_Msgs = "MDPS Message Timeout"; break;                                               //MDPS 신호 없음
                case "C168808": ret_Msgs = "MDPS Signal Error"; break;                                                  //MDPS 신호 이상
                case "C16B687": ret_Msgs = "AEB Message timeout"; break;                                                //AEB측 CAN 신호 안 나옴
                case "C16B781": ret_Msgs = "AEB Signal Error"; break;                                                   //AEB 신호 이상
                case "C170204": ret_Msgs = "Variant coding error"; break;                                               //사양 설정 이상
                case "C181208": ret_Msgs = "CAN timeout CGW"; break;                                                    //CGW 측 CAN 신호 안나옴
                case "C181708": ret_Msgs = "CGW1 signal Fault"; break;                                                  //CGW1 신호 이상
                case "C181887": ret_Msgs = "BCA Time Out"; break;                                                       //BCA 신호 안나옴
                case "C183786": ret_Msgs = "BCA Signal Failure"; break;                                                 //BCA 신호 이상
                case "C183887": ret_Msgs = "RSPA Time Out"; break;                                                      //RSPA 측 신호 안나옴
                case "C183986": ret_Msgs = "RSPA Signal Error"; break;                                                  //RSPA 신호 오류
                case "C184786": ret_Msgs = "PCA Signal Failure"; break;                                                 //PCA 신호 이상
                case "C184787": ret_Msgs = "PCA Timeout"; break;                                                        //PCA CAN 신호 안나옴
                case "C222798": ret_Msgs = "Brake Pad temperature Fault"; break;                                        //브레이크 패드 온도 이상
                case "C222808": ret_Msgs = "TCU CAN Signal Error"; break;                                               //TCU CAN 신호 이상
                case "C223407": ret_Msgs = "LVA Vacuum – Low VAC"; break;                                               //LVA Vacuum – VAC 낮음
                #endregion
                #region EPB
                case "C110362": ret_Msgs = "Ignition Mismatch (Signal Compare Failure)"; break;                         //Ign 신호 불일치
                case "C150113": ret_Msgs = "EPB Switch fault – Switch Open/Short"; break;                               //EPB 동작 스위치 고장
                case "C15011F": ret_Msgs = "EPB Switch fault – Switch Signal Fault"; break;                             //EPB 동작 스위치 고장
                case "C150193": ret_Msgs = "EPB Switch fault – Switch Stuck"; break;                                    //EPB 동작 스위치 고장
                case "C160648": ret_Msgs = "Supervision software failure"; break;                                       //SW 감시 오류
                case "C16064A": ret_Msgs = "SW Version Error (Incorrect Component Installed)"; break;                   //SW 버전 오류
                case "C160662": ret_Msgs = "Abnormal Signal (Signal Compare Failure)"; break;                           //비정상 신호
                case "C160696": ret_Msgs = "EPB SW Execution Fault (Component Internal Failure)"; break;                //EPB 스위치 실행 오류
                case "C162808": ret_Msgs = "CAN time out CLU"; break;                                                   //CLU측 CAN 신호 안나옴
                case "C165608": ret_Msgs = "CLU Signal Error"; break;                                                   //CLU 신호 이상
                case "C220277": ret_Msgs = "EPB Latching Failure"; break;                                               //차량 Roll –Away 리클램프 3회 발생 감지
                case "C222071": ret_Msgs = "EPB Motor2 Over Current Error (Actuator Stuck)"; break;                     //EPB 모터2 과전류
                case "C222074": ret_Msgs = "Actuator2 Excessive Control (Actuator Slipping)"; break;                    //Actuator2 초과 제어
                case "C222077": ret_Msgs = "EPB Motor2 Stall (Commanded Position Not Reachable)"; break;                //EPB 모터2 멈춤
                case "C222094": ret_Msgs = "EPB Motor2 Run without Cmd (Unexpected Operation)"; break;                  //EPB 모터2 오동작
                case "C222471": ret_Msgs = "EPB Motor1 Over Current (Actuator Stuck)"; break;                           //EPB 모터1 과전류
                case "C222474": ret_Msgs = "EPB Actuator1 Excessive Control (Actuator Slipping)"; break;                //EPB 엑추에이터1 초과 제어
                case "C222477": ret_Msgs = "EPB Motor1 Stall (Commanded Position Not Reachable)"; break;                //EPB 모터1 중단
                case "C222494": ret_Msgs = "EPB Motor1 Run without Cmd (Unexpected Operation)"; break;                  //EPB 모터1 오동작
                case "C224077": ret_Msgs = "Actuator State Fault (Commanded Position Not Reachable)"; break;            //액츄에이터 상태 오류
                case "C224494": ret_Msgs = "Unexpected Power Down (Unexpected Operation)"; break;                       //전원 차단 오류
                case "C241611": ret_Msgs = "EPB Motor2 Error – Circuit Short to Ground"; break;                         //EPB 모터2 오류 – 회로 접지
                case "C241612": ret_Msgs = "EPB Motor2 Error – Circuit Short to Battery"; break;                        //EPB 모터2 오류 – 회로 전원과 배선
                case "C241613": ret_Msgs = "EPB Motor2 Error – Circuit Open"; break;                                    //EPB 모터2 오류 – 회로 단선
                case "C241671": ret_Msgs = "EPB Motor2 Error – Actuator Stuck"; break;                                  //EPB 모터2 오류 – Actuator 움직이지 않음
                case "C241711": ret_Msgs = "EPB Motor1 Error - ircuit Short to Ground"; break;                          //EPB 모터1 오류 - 회로 접지
                case "C241712": ret_Msgs = "EPB Motor1 Error - Circuit Short to Battery"; break;                        //EPB 모터1 오류 - 회로 전원과 배선
                case "C241713": ret_Msgs = "EPB Motor1 Error - Circuit Open"; break;                                    //EPB 모터1 오류 - 회로 단선
                case "C241771": ret_Msgs = "EPB Motor1 Error - Actuator Stuck"; break;                                  //EPB 모터1 오류 - Actuator 움직이지 않음
                case "C241877": ret_Msgs = "Actuator Stall (Commanded Position Not Reachable)"; break;                  //액츄에이터 중단
                case "C241977": ret_Msgs = "EPB Actuator1 Stall (Commanded Position Not Reachable)"; break;             //EPB Actuator1 중단
                #endregion
                #region iTPMS
                case "C130100": ret_Msgs = "MIL tire detection"; break;                                                 //Ign 신호 불일치
                case "C130200": ret_Msgs = "No get valid data for low pressure within 1 hour"; break;                   //불충분한 가용성
                case "C154907": ret_Msgs = "TPMS Reset SW Stick"; break;                                                //TPMS 리셋 스위치 고착
                case "C169408": ret_Msgs = "CAN time out FATC"; break;                                                  //FATC 측 CAN 신호 안나옴
                case "C169508": ret_Msgs = "FATC Signal Error"; break;                                                  //FATC 신호 이상
                case "C170100": ret_Msgs = "TPMS Variant Coding Error"; break;                                          //TPMS 사양코딩 이상
                case "C272200": ret_Msgs = "No get valid data Error"; break;                                            //불충분한 학습
                #endregion
            }
            return ret_Msgs;
        }
        public static string ret_Errs(string pErr)
        {
            string ret_Msgs = "";

            switch (pErr)
            {
                case "10": ret_Msgs = "GeneralReject"; break;
                case "11": ret_Msgs = "ServiceNotSupported"; break;
                case "12": ret_Msgs = "SubFunctionNotSupported"; break;
                case "13": ret_Msgs = "IncorrectMessageLengthOrInvalidFormat"; break;
                case "14": ret_Msgs = "RespondeTooLong"; break;
                case "21": ret_Msgs = "Busy RepeatRequest"; break;
                case "22": ret_Msgs = "ConditionsNotCorrect"; break;
                case "24": ret_Msgs = "RequestSequenceError"; break;
                case "31": ret_Msgs = "RequestOutOfRange"; break;
                case "33": ret_Msgs = "SecurityAccessDenied"; break;
                case "35": ret_Msgs = "InvalidKey"; break;
                case "36": ret_Msgs = "ExceedNumberOfAttempts"; break;
                case "37": ret_Msgs = "RequiredTimeDelayNotExpired"; break;
                case "70": ret_Msgs = "UploadDownNotAccepted"; break;
                case "71": ret_Msgs = "TransferSuspended"; break;
                case "72": ret_Msgs = "GeneralProgrammingFailure"; break;
                case "73": ret_Msgs = "WrongBlockSequenceCounter"; break;
                case "78": ret_Msgs = "ReqCorrectlyRcvd-RspPending (requestCorrectlyReceived-ResponsePending)"; break;
                case "7E": ret_Msgs = "SubFunctionNotSupportedInActiveSession"; break;
                case "7F": ret_Msgs = "ServiceNotSupportedInActiveSession"; break;
                case "80": ret_Msgs = "ServiceNotSupportedInActiveDiagnosticMode"; break;
                case "81": ret_Msgs = "RpmTooHigh"; break;
                case "82": ret_Msgs = "RpmTooLow"; break;
                case "83": ret_Msgs = "EngineIsRunning"; break;
                case "84": ret_Msgs = "EngineIsNotRunning"; break;
                case "85": ret_Msgs = "EngineRunTimeTooLow"; break;
                case "86": ret_Msgs = "TemperatureTooHigh"; break;
                case "87": ret_Msgs = "TemperatureTooLow"; break;
                case "88": ret_Msgs = "VehicleSpeedTooHigh"; break;
                case "89": ret_Msgs = "VehicleSpeedTooLow"; break;
                case "8A": ret_Msgs = "Throttle/PedalTooHigh"; break;
                case "8B": ret_Msgs = "Throttle/PedalTooLow"; break;
                case "8C": ret_Msgs = "TransmissionRangeNotInNeutral"; break;
                case "8D": ret_Msgs = "TransmissionRangeNotInGear"; break;
                case "8F": ret_Msgs = "BrakeSwitchNotClosed"; break;
                case "90": ret_Msgs = "ShiftLeverNotInPark"; break;
                case "91": ret_Msgs = "TorqueConverterClutchLocked"; break;
                case "92": ret_Msgs = "VoltageTooHigh"; break;
                case "93": ret_Msgs = "VoltageTooLow"; break;
            }

            return ret_Msgs;
        }
    }

    public static class MANDO__TL
    {
        #region Variable declaration
        public static string Part_No;
        public static string Diagnosis;
        public static string Supplier;
        public static string week;
        public static string Year;
        public static string Plant;
        public static string Line;
        public static string Shift;
        public static string DOW;
        public static string counter;
        public static string SW_ALG_1;
        public static string SW_ALG_2;
        public static string ALG_Edition_1;
        public static string ALG_Edition_2;

        public static string Reserved;
        public static string SW_Sys_1;
        public static string SW_Sys_2;
        public static string Edition_No_1;
        public static string Edition_No_2;

        public static string Pedal;
        #endregion

        #region Standard CAN
        public static bool SecurityAccess()         //0 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("27 01");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            string[] seed_key = NeoVI.Get_Data.Split(' ');

            if (seed_key[3] == "00" && seed_key[4] == "00") return true;

            string KeyString = Aloirithm(seed_key[3] + " " + seed_key[4]).ToString("X4");
            string Key1 = KeyString.Substring(0, 2);
            string Key2 = KeyString.Substring(2, 2);

            Ret = NeoVI.Ret_SendMsgs("27 02 " + Key1 + " " + Key2);
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static UInt32 Aloirithm(string HexSeed)
        {
            string[] HexD = HexSeed.Split(' ');
            uint Seed = H2Y.HexToUInt(HexD[0], HexD[1]);
            uint Ret = 0;
            uint DIAG_CRC_GEN_Code = 0x0D;
            uint GEN_CRC_KEY = (Seed << 3) & 0xFFFF;
            uint FCS_KEY = GEN_CRC_KEY;

            if (GEN_CRC_KEY >= 1024) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 128;
            if (512 <= GEN_CRC_KEY && GEN_CRC_KEY < 1024) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 64;
            if (256 <= GEN_CRC_KEY && GEN_CRC_KEY < 512) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 32;
            if (128 <= GEN_CRC_KEY && GEN_CRC_KEY < 256) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 16;
            if (64 <= GEN_CRC_KEY && GEN_CRC_KEY < 128) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 8;
            if (32 <= GEN_CRC_KEY && GEN_CRC_KEY < 64) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 4;
            if (16 <= GEN_CRC_KEY && GEN_CRC_KEY < 32) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 2;
            if (8 <= GEN_CRC_KEY && GEN_CRC_KEY < 16) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 1;

            Ret = (FCS_KEY + GEN_CRC_KEY) & 0xFFFF;

            return Ret; 
        }
        
        public static bool Start_Communication()    //1. 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("10 03");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Stop_Communication()     //2. 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("20");

            if (Ret) { ECUs.End_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Reset()              //3. 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("11 01");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Identification()     //4. 
        {
            bool Ret = true;

            #region Identification Clear
            Part_No = "";
            Diagnosis = "";
            Supplier = "";
            week = "";
            Year = "";
            Plant = "";
            Line = "";
            Shift = "";
            DOW = "";
            counter = "";
            SW_ALG_1 = "";
            SW_ALG_2 = "";
            ALG_Edition_1 = "";
            ALG_Edition_2 = "";

            Reserved = "";
            SW_Sys_1 = "";
            SW_Sys_2 = "";
            Edition_No_1 = "";
            Edition_No_2 = "";
            #endregion

            //0xC101 - 39
            //0xC102 - 11
            //0xC103 - 29(Integrated EPB system Only)
            //0xF101 - 2
            //0xF102 - 7
            //0xF103 - 3
            //0xF103 - 9
            //0xF104 - 12
            //0xF105 - 4
            //0xF107 - 4
            //0xF108 - 19
            //0xF18C - 15
            //0x2000 - 1

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 C1 01"); }
            if (Ret) { Identification_C101(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 C1 02"); }
            if (Ret) { Identification_C102(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 C1 03"); }
            if (Ret) { Identification_C103(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 01"); }
            if (Ret) { Identification_F101(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 02"); }
            if (Ret) { Identification_F102(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 03"); }
            if (Ret) { Identification_F103(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 04"); }
            if (Ret) { Identification_F104(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 05"); }
            if (Ret) { Identification_F105(NeoVI.Get_Data); }

            return Ret;
        }
        public static void Identification_C101(string pData) //4.1 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 42) return;

            string PID = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) + H2Y.HexToASCII(Ident[6]) + H2Y.HexToASCII(Ident[7]);
            NeoVI.Debug_Message("0.Supported PID : " + PID);

            string RPM = (H2Y.HexToInt(Ident[8], Ident[9]) * 0.25).ToString();
            NeoVI.Debug_Message("1.Engine RPM : " + RPM);

            string speed = H2Y.HexTobyte(Ident[10]).ToString();
            NeoVI.Debug_Message("2.Vehicle speed : " + speed);

            string TPS = H2Y.HexTobyte(Ident[11]).ToString();
            NeoVI.Debug_Message("3.TPS : " + TPS);

            string Gear = H2Y.HexToBinary(Ident[12]).ToString();
            NeoVI.Debug_Message("4.Gear Position : " + Gear);

            string Battery = (H2Y.HexTobyte(Ident[13]) * 16 / 255).ToString();
            NeoVI.Debug_Message("5.Battery voltage : " + Battery);

            string G_sensor = (H2Y.HexTobyte(Ident[14]) * 6 / 255).ToString();
            NeoVI.Debug_Message("6. 5 Volt reference (G sensor installed system Only) : " + G_sensor);

            string WSS_FL = H2Y.HexTobyte(Ident[15]).ToString();
            NeoVI.Debug_Message("7.Wheel speed sensor – Front Left : " + WSS_FL);

            string WSS_FR = H2Y.HexTobyte(Ident[16]).ToString();
            NeoVI.Debug_Message("8.Wheel speed sensor – Front Right : " + WSS_FR);

            string WSS_RL = H2Y.HexTobyte(Ident[17]).ToString();
            NeoVI.Debug_Message("9.Wheel speed sensor – Rear Left : " + WSS_RL);

            string WSS_RR = H2Y.HexTobyte(Ident[18]).ToString();
            NeoVI.Debug_Message("10.Wheel speed sensor – Rear Right : " + WSS_RR);

            string Steering = H2Y.HexToBinary(Ident[19]).ToString();
            NeoVI.Debug_Message("11.Steering sensor (00=Low, 01=High, 11=Reserved)(ESC Only) : " + Steering);

            string Longituinal = ((H2Y.HexTobyte(Ident[20]) - H2Y.HexTobyte("7F")) * 4 / 255).ToString();
            NeoVI.Debug_Message("12.G Sensor – Longituinal (G sensor installed system Only) : " + Longituinal);

            string Lateral = H2Y.HexTobyte(Ident[21]).ToString();
            NeoVI.Debug_Message("13.G Sensor – Lateral (ESC Only) : " + Lateral);

            string Lamp = H2Y.HexToBinary(Ident[22]).ToString();
            NeoVI.Debug_Message("14.Lamp (00=Off, 01=On, 11=Reserved) : " + Lamp);

            string Switch0 = H2Y.HexToBinary(Ident[23]).ToString();
            NeoVI.Debug_Message("15.Switch 0 (00=Off, 01=On, 11=Reserved) : " + Switch0);

            string Relay = H2Y.HexToBinary(Ident[24]).ToString();
            NeoVI.Debug_Message("16.Relay (00=Off, 01=On, 11=Reserved) : " + Relay);

            string Motor = H2Y.HexToBinary(Ident[25]).ToString();
            NeoVI.Debug_Message("17.Motor (xxh : 00h=Off, 01h=On) : " + Motor);

            string Inlet = H2Y.HexToBinary(Ident[26]).ToString();
            NeoVI.Debug_Message("18.ABS inlet valve(00=Off, 01=On, 11=Reserved) : " + Inlet);

            string Outlet = H2Y.HexToBinary(Ident[27]).ToString();
            NeoVI.Debug_Message("19.ABS outlet valve (00=Off, 01=On, 11=Reserved) : " + Outlet);

            string ESC = H2Y.HexToBinary(Ident[28]).ToString();
            NeoVI.Debug_Message("20.ESC valve (00=Off, 01=On, 11=Reserved) : " + ESC);

            string Switch1 = H2Y.HexToBinary(Ident[29]).ToString();
            NeoVI.Debug_Message("21.Switch 1 (00=Off, 01=On, 11=Reserved) : " + Switch1);

            string Steering1 = ((H2Y.HexToInt(Ident[30], Ident[31]) - H2Y.HexToInt("8000")) / 10).ToString();
            NeoVI.Debug_Message("22.Steering(CAN type) Sensor range : -780˚ ~ 780˚ (ESC Only) : " + Steering1);

            string Yaw = ((H2Y.HexToInt(Ident[32], Ident[33]) - H2Y.HexToInt("0800")) / 630).ToString();
            NeoVI.Debug_Message("23.Yaw Sensor range : -1.5g ~ 1.5g (ESC Only) : " + Yaw);

            string Yaw1 = ((H2Y.HexToInt(Ident[34], Ident[35]) - H2Y.HexToInt("0800")) / 16).ToString();
            NeoVI.Debug_Message("24.Yaw Sensor range : -75˚/s ~ 75˚/s (ESC Only) : " + Yaw1);

            string Pressure = (H2Y.HexToInt(Ident[36], Ident[37])).ToString();
            NeoVI.Debug_Message("25.Pressure Sensor range : 0bar ~ 200bar (ESC Only) : " + Pressure);

            string Pedal_PDT = (H2Y.HexTobyte(Ident[38])).ToString();
            NeoVI.Debug_Message("26.Sensor range : 0mm ~ 135mm (Only for system applied pedal travel sensor) : " + Pedal_PDT);

            string Pedal_PDF = (H2Y.HexTobyte(Ident[39])).ToString();
            NeoVI.Debug_Message("27.Sensor range : 0mm ~ 135mm (Only for system applied pedal travel sensor) : " + Pedal_PDF);

            string Lamp_Status = H2Y.HexToBinary(Ident[40]).ToString();
            NeoVI.Debug_Message("28.DBC Lamp & Switch (Only for system applied DBC) : " + Lamp_Status);

            string Sensor = (H2Y.HexToInt(Ident[41], Ident[42]) - H2Y.HexToInt("8000")).ToString();
            NeoVI.Debug_Message("29.Steering(Relative type) Sensor range : -800˚ ~ 800˚ (ESC Only) : " + Sensor);
        }
        public static void Identification_C102(string pData) //4.2 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 14) return;

            string PID = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) + H2Y.HexToASCII(Ident[6]) + H2Y.HexToASCII(Ident[7]);
            NeoVI.Debug_Message("0.Supported PID : " + PID);

            string FL = H2Y.HexToASCII(Ident[8]);
            string FR = H2Y.HexToASCII(Ident[9]);
            string RL = H2Y.HexToASCII(Ident[10]);
            string RR = H2Y.HexToASCII(Ident[11]);
            NeoVI.Debug_Message("1.Pressure range : 0bar ~ 200bar : FL:" + FL + ", FR:" + FR + ", RL:" + RL + ", RR:" + RR);

            string Vacuum = (H2Y.HexToInt(Ident[12], Ident[13]) / 10).ToString();
            NeoVI.Debug_Message("2.Vacuum Sensor range : 0kpa ~ 100kpa (Only for system applied Vacuum sensor) :" + Vacuum);

            string Status = H2Y.HexToASCII(Ident[14]);
            NeoVI.Debug_Message("3.Status (00=Off, 01=On, 10, 11=Reserved) :" + Status);
        }
        public static void Identification_C103(string pData) //4.3 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 32) return;

            string PID = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) + H2Y.HexToASCII(Ident[6]) + H2Y.HexToASCII(Ident[7]);
            NeoVI.Debug_Message("0.Supported PID : " + PID);

            string Switch = H2Y.HexToBinary(Ident[8]).ToString();
            NeoVI.Debug_Message("1.Switch (00=Off, 01=On, 11=Reserved) : " + Switch);

            string Switch1 = H2Y.HexToBinary(Ident[9]).ToString();
            NeoVI.Debug_Message("2.Switch1 (00=Off, 01=On, 11=Reserved) : " + Switch1);

            string Lamp = H2Y.HexToBinary(Ident[10]).ToString();
            NeoVI.Debug_Message("3.Lamp (00=Off, 01=On, 10=blinking, 11=Reserved) : " + Lamp);

            string Direction = H2Y.HexToBinary(Ident[11]).ToString();
            NeoVI.Debug_Message("4.Actuator Direction (0:Not Active, 1:Applying, 2:Releasing) : " + Direction);

            string Status = H2Y.HexToBinary(Ident[12]).ToString();
            NeoVI.Debug_Message("5.Actuator Status (0:Unknown, 1:Applied, 2:Released) : " + Status);

            string Motor1 = ((H2Y.HexToInt(Ident[13], Ident[14]) - 32767) / 10).ToString();
            NeoVI.Debug_Message("6.Motor #1 current : " + Motor1);

            string Motor2 = ((H2Y.HexToInt(Ident[15], Ident[16]) - 32767) / 10).ToString();
            NeoVI.Debug_Message("7.Motor #2 current : " + Motor2);

            string Sensor = H2Y.HexToInt(Ident[17], Ident[18]).ToString();
            NeoVI.Debug_Message("8.Force sensor (EPB integration type only) : " + Sensor);

            string Clutch = H2Y.HexToASCII(Ident[19]);
            NeoVI.Debug_Message("9.Clutch Sig1 Duty (MT Vehicle Only) : " + Clutch);

            string Clutch1 = H2Y.HexToASCII(Ident[20]);
            NeoVI.Debug_Message("10.Clutch Sig2 Duty (MT Vehicle Only) : " + Clutch1);

            string counter = H2Y.HexToASCII(Ident[21]) + H2Y.HexToASCII(Ident[22]) + H2Y.HexToASCII(Ident[23]) + H2Y.HexToASCII(Ident[24]);
            NeoVI.Debug_Message("11.Static operation count : " + counter);

            string counter1 = H2Y.HexToASCII(Ident[25]) + H2Y.HexToASCII(Ident[26]);
            NeoVI.Debug_Message("12.Dynamic operation count : " + counter1);

            string  Hall_counter = H2Y.HexToBinary(Ident[27]).ToString();
            NeoVI.Debug_Message("13.RL-Hall Counter Direction (0:Not Active, 1:Applying, 2:Releasing) : " + Hall_counter);

            string Hall_counter1 = H2Y.HexToBinary(Ident[28]).ToString();
            NeoVI.Debug_Message("14.RR-Hall Counter Direction (0:Not Active, 1:Applying, 2:Releasing) : " + Hall_counter1);

            string Hall_counter2 = (H2Y.HexToInt(Ident[29], Ident[30]) - 32767).ToString();
            NeoVI.Debug_Message("15.RL-Hall Counter Position : " + Hall_counter2);

            string Hall_counter3 = (H2Y.HexToInt(Ident[31], Ident[32]) - 32767).ToString();
            NeoVI.Debug_Message("16.RR-Hall Counter Position : " + Hall_counter3);
        }
        public static void Identification_F101(string pData) //4.4 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 5) return;

            string Vehicle = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]);
            NeoVI.Debug_Message("0.Vehicle name (ASCII code) : " + Vehicle);
        }
        public static void Identification_F102(string pData) //4.5 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 10) return;

            string Vehicle = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) +
                             H2Y.HexToASCII(Ident[6]) + H2Y.HexToASCII(Ident[7]) +
                             H2Y.HexToASCII(Ident[8]) + H2Y.HexToASCII(Ident[9]) +
                             H2Y.HexToASCII(Ident[10]);
            NeoVI.Debug_Message("0.Vehicle name (ASCII code) : " + Vehicle);
        }
        public static void Identification_F103(string pData) //4.6 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 6) return;

            string Version = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) + H2Y.HexToASCII(Ident[6]);
            NeoVI.Debug_Message("0.ECU Version (ASCII code) : " + Version);
        }
        public static void Identification_F104(string pData) //4.7 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 15) return;

            string Version = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) + H2Y.HexToASCII(Ident[6]) + 
                             H2Y.HexToASCII(Ident[7]) + H2Y.HexToASCII(Ident[8]) + H2Y.HexToASCII(Ident[9]) + 
                             H2Y.HexToASCII(Ident[10]) + H2Y.HexToASCII(Ident[11]) + H2Y.HexToASCII(Ident[12]) +
                             H2Y.HexToASCII(Ident[13]) + H2Y.HexToASCII(Ident[14]) + H2Y.HexToASCII(Ident[15]);
            NeoVI.Debug_Message("0.Software Version (ASCII code) : " + Version);
        }
        public static void Identification_F105(string pData) //4.8 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 7) return;

            string Release = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) +
                             H2Y.HexToASCII(Ident[6]) + H2Y.HexToASCII(Ident[7]);
            NeoVI.Debug_Message("0.Software Release Date(ASCII code) : " + Release);
        }

        public static bool Read__DTC()              //5 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("19 02 08");

            if (Ret) { ECUs.DTC_Read = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Clear_DTC()              //6 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("14 FF FF FF");
            if (Ret) { ECUs.DTCClear = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Check_Signals()          //7 
        {
            //return NeoVI.Ret_SendMsgs("21 02");

            bool Ret = true;

            Pedal = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("21 C2"); }  //Brake Pedal Position* 00:off, 01:On
            if (Ret)
            {
                Pedal = NeoVI.Get_Data;
                ECUs.SigPedal = Ret_OnOff(Pedal);
                NeoVI.Debug_Message("Brake Pedal Position : " + ECUs.SigPedal);
            }

            return Ret;
        }
        public static string Ret_OnOff(string pData)
        {
            string[] Hex = pData.Split(' ');
            if (Hex.Length < 4) { return "Err"; }

            string strValue = Hex[4];
            string RetValue = "None";

            switch (strValue)
            {
                case "00": RetValue = "OFF"; break;
                case "01": RetValue = "ON"; break;
            }

            return RetValue;
        }
        public static bool WSS_Test()               //8 
        {
            bool Ret = true;

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 C1 01"); }  
            if (Ret)
            {
                Ret_WSS_Speed(NeoVI.Get_Data);

                NeoVI.Debug_Message("WSS FL : " + ECUs.WSS_FL);
                NeoVI.Debug_Message("WSS FR : " + ECUs.WSS_FR);
                NeoVI.Debug_Message("WSS RL : " + ECUs.WSS_RL);
                NeoVI.Debug_Message("WSS RR : " + ECUs.WSS_RR);
            }

            return Ret;
        }
        private static void Ret_WSS_Speed(string pData) //8.1 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 42) return;

            ECUs.WSS_FL = H2Y.HexTobyte(Ident[15]);
            ECUs.WSS_FR = H2Y.HexTobyte(Ident[16]);
            ECUs.WSS_RL = H2Y.HexTobyte(Ident[17]);
            ECUs.WSS_RR = H2Y.HexTobyte(Ident[18]);
        }
        
        //연결 유지
        public static bool Tester_Present()         //TesterPresent(3E hex) Service 테스터 존재
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("3E 00");      //(00:Response required, 80:Response not required)
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Message_Falg()           //EnableNormalMessageTransmission (29 hex) service
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("29 01");      //(01:ResponseRequired, 02:NoResponseRequired)
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        
        public static bool Dynamic_Step(int idx)    //Dynamic ABS Test
        {
            bool Ret = true;

            switch (idx)
            {
                case 0: Ret = Start_Communication(); break;    
                case 1: Ret = NeoVI.Ret_SendMsgs("2F F0 6F 03 3C 11 01"); break;    //1. ABS Pressure release FL (600ms)    0001 0001 - 0000 0001
                case 2: Ret = NeoVI.Ret_SendMsgs("2F F0 6F 03 3C 22 01"); break;    //2. ABS Pressure release FR (600ms)    0010 0010 - 0000 0001
                case 3: Ret = NeoVI.Ret_SendMsgs("2F F0 6F 03 3C 44 01"); break;    //3. ABS Pressure release RL (600ms)    0100 0100 - 0000 0001 
                case 4: Ret = NeoVI.Ret_SendMsgs("2F F0 6F 03 3C 88 01"); break;    //4. ABS Pressure release RR (600ms)    1000 1000 - 0000 0001

                case 5: Ret = NeoVI.Ret_SendMsgs("2F F0 11 03"); break;             //5. ABS Pump Motor On for 2 Seconds
            }
            
            return Ret;
        }
        public static bool Dynamic_Auto()           //Dynamic Test
        {
            bool Ret = true;

            float T2 = 600;
            float T3 = 2000;

            double Vlv_Time = 0;
            double Old_Time = 0;
            double Off_Time = DateTime.Now.Ticks;
            bool ECU_Flag = true;
            byte ECU_Setp = 0;

            while (true)
            {
                if (!ECU_Flag) { ECU_Flag = true; }

                Vlv_Time = TSet.ABSv_Time + ((DateTime.Now.Ticks - Off_Time) / H2Y.tick_Dvd);

                if (ECU_Flag && ECU_Setp == 0)
                {
                    Ret = Start_Communication();                    ECU_Setp = 1; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 1)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 2; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 2)
                {
                    Ret = Dynamic_Step(1);                          ECU_Setp = 3; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 3)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 4; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 4)
                {
                    Ret = Dynamic_Step(2);                          ECU_Setp = 5; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 5)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 6; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 6)
                {
                    Ret = Dynamic_Step(3);                          ECU_Setp = 7; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 7)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 8; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 8)
                {
                    Ret = Dynamic_Step(4);                          ECU_Setp = 9; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 9)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) {  ECU_Setp = 10; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 10)
                {
                    Ret = Dynamic_Step(5);                          ECU_Setp = 11; ECU_Flag = false; Old_Time = Vlv_Time; 
                }

                if (ECU_Flag && ECU_Setp == 11)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T3, 1000)) {  ECU_Setp = 12; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 12)
                {
                    ECUs.ABS_Step = 5;
                    break;
                }
            }

            return Ret;
        }
        public static bool ESP_Step(int idx)        //Dynamic ESP Test
        {
            bool Ret = true;

            switch (idx)
            {
                case 1: Ret = true; break;          
                case 2: Ret = true; break;          
            }

            return Ret;
        }

        public static bool ESS_LampTest()           //10 ESS Lamp(Option) 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("2F F0 24 03");
            if (Ret) { ECUs.ESS_Lamp = NeoVI.Get_Data; }

            return Ret;
        }

        public static bool Single_IO_Idx(string pHex, int idx) //13. DataIdentifier list for Single I/O control for ABS/ESC system
        {
            bool Ret = true;

            switch (idx)
            {
                case 0: Ret = NeoVI.Ret_SendMsgs(pHex + " 00"); break;
                case 1: Ret = NeoVI.Ret_SendMsgs(pHex + " 03"); break;
            }

            return Ret;
        }
        #endregion
        
        public static string ret_DTCs(string pCode)
        {
            string ret_Msgs = "";

            switch (pCode)
            {
                case "C110101": ret_Msgs = "Battery voltage high"; break;
                case "C110201": ret_Msgs = "Battery Voltage Low"; break;
                case "C111301": ret_Msgs = "5V Sensor Power Fail"; break;
                case "C111401": ret_Msgs = "12V Sensor Power Fail"; break;
                case "C222707": ret_Msgs = "Excessive temperature of brake disc"; break;
                case "C120001": ret_Msgs = "Wheel Speed Sensor Front-LH Open / Short"; break;
                case "C120102": ret_Msgs = "Wheel Speed Sensor Front-LH Range / Performance / Intermittent"; break;
                case "C120202": ret_Msgs = "Wheel Speed Sensor Front-LH Invalid / No Signal"; break;
                case "C120301": ret_Msgs = "Wheel Speed Sensor Front-RH Open / Short"; break;
                case "C120402": ret_Msgs = "Wheel Speed sensor Front-RH Range / Performance / Intermittent"; break;
                case "C120502": ret_Msgs = "Wheel Speed Sensor Front-RH Invalid / No Signal"; break;
                case "C120601": ret_Msgs = "Wheel Speed Sensor Rear-LH Open / Short"; break;
                case "C120702": ret_Msgs = "Wheel Speed Sensor Rear-LH Range / Performance / Intermittent"; break;
                case "C120802": ret_Msgs = "Wheel Speed Sensor Rear-LH Invalid / No Signal"; break;
                case "C120901": ret_Msgs = "Wheel Speed Sensor Rear-RH Open / Short"; break;
                case "C121002": ret_Msgs = "Wheel Speed Sensor Rear-RH Range / Performance / Intermittent"; break;
                case "C121102": ret_Msgs = "Wheel Speed Sensor Rear-RH Invalid / No Signal"; break;
                case "C160404": ret_Msgs = "ECU hardware error"; break;
                case "C211201": ret_Msgs = "Valve relay error"; break;
                case "C238001": ret_Msgs = "ABS/TCS/ESP Valve Error"; break;
                case "C240201": ret_Msgs = "Motor electrical"; break;
                case "C123501": ret_Msgs = "Pressure Sensor (Primary) – Electrical"; break;
                case "C123702": ret_Msgs = "Pressure sensor – other"; break;
                case "C162308": ret_Msgs = "CAN time-out Steering angle sensor"; break;
                case "C126002": ret_Msgs = "Steering angle sensor circuit – signal"; break;
                case "C126104": ret_Msgs = "Steering angle sensor is not calibrated"; break;
                case "C126402": ret_Msgs = "SAS Offset Error, Noisy signal Stick"; break;
                case "C164308": ret_Msgs = "CAN Time-Out Yaw & G Sensor"; break;
                case "C128302": ret_Msgs = "Lateral G Sensor / Longitudinal G Sensor / Yaw Rate Sensor - Signal Error"; break;
                case "C127401": ret_Msgs = "G sensor error"; break;
                case "C127502": ret_Msgs = "G Sensor Range / Performance Error"; break;
                case "C128208": ret_Msgs = "Yaw Rate & Lateral G Sensor – Electrical "; break;
                case "C128504": ret_Msgs = "Uncalibrated Ax Signal"; break;
                case "C128601": ret_Msgs = "YAW RATE&2G Sensor Fail-Replace SRSCM"; break;

                case "C137801": ret_Msgs = "Pedal Signal Open/Short Error"; break;
                case "C137902": ret_Msgs = "Invalid Pedal Signal Error"; break;
                case "C138001": ret_Msgs = "Pedal Signal Not Calibrated"; break;
                case "C138501": ret_Msgs = "Vacuum Sensor - Electrical"; break;
                case "C138602": ret_Msgs = "Vacuum Sensor vehicle -Signal"; break;
                case "C212601": ret_Msgs = "Vacuum Pump Relay Drive Pin Open/Short"; break;
                case "C223102": ret_Msgs = "Vacuum Pump System Fail"; break;
                case "C135801": ret_Msgs = "AVH switch Error"; break;
                case "C150301": ret_Msgs = "TCS switch Error"; break;
                case "C152001": ret_Msgs = "Clutch switch Error"; break;
                case "C152601": ret_Msgs = "DBC switch Error"; break;
                case "C152701": ret_Msgs = "Reverse Gear Signal Error"; break;
                case "C151301": ret_Msgs = "Brake switch Circuit Error"; break;
                case "C154201": ret_Msgs = "Brake Circuit Error"; break;
                case "C213001": ret_Msgs = "Brake Lamp Relay Error"; break;
                case "C213101": ret_Msgs = "ESS Brake Lamp Relay error"; break;
                case "C170204": ret_Msgs = "Auto Coding Error"; break;
                case "C110001": ret_Msgs = "Battery Voltage"; break;
                case "C110301": ret_Msgs = "Ignition Voltage"; break;
                case "C150101": ret_Msgs = "Switch Failure"; break;
                case "C153901": ret_Msgs = "In Gear Switch Error"; break;
                case "C171004": ret_Msgs = "Assembly-Line Setup is Still Active"; break;
                case "C222001": ret_Msgs = "Rear-LH actuator"; break;
                case "C222401": ret_Msgs = "Rear-RH (or Rear) actuator"; break;
                case "C241601": ret_Msgs = "Motor Short or Open - LH"; break;
                case "C241701": ret_Msgs = "Motor Short or Open - RH"; break;
                case "C138201": ret_Msgs = "EPB Hall Sensor failure - LH"; break;
                case "C138301": ret_Msgs = "EPB Hall Sensor failure - RH"; break;
                case "C220201": ret_Msgs = "EPB Reclamp repetition"; break;
                case "C171304": ret_Msgs = "Factory Mode Not Disabled"; break;

                case "C161108": ret_Msgs = "CAN time-out EMS"; break;
                case "C161208": ret_Msgs = "CAN time-out TCU"; break;
                case "C161308": ret_Msgs = "CAN signal error EMS"; break;
                case "C161608": ret_Msgs = "C-CAN bus off"; break;
                case "C160E08": ret_Msgs = "P-CAN Bus Off"; break;
                case "C162708": ret_Msgs = "CAN time-out 4WD"; break;
                case "C222208": ret_Msgs = "Actuator Failure – 4WD"; break;
                case "C168708": ret_Msgs = "CAN time-out VSM2 (MDPS)"; break;
                case "C168808": ret_Msgs = "VSM2 (MDPS) Signal Error"; break;
                case "C165108": ret_Msgs = "CAN time-out EPB"; break;
                case "C165208": ret_Msgs = "CAN Signal Error EPB "; break;
                case "C182E08": ret_Msgs = "CAN Time-out FCS"; break;
                case "C163808": ret_Msgs = "ACC communication Error"; break;
                case "C165008": ret_Msgs = "CAN Signal Error ACC"; break;
                case "C164908": ret_Msgs = "CAN Time-Out EMS for ACC"; break;
                case "C16B808": ret_Msgs = "AEB Communication Error"; break;
                case "C164808": ret_Msgs = "CAN Signal Error EMS for ACC"; break;
                case "C222808": ret_Msgs = "TCU Signal Fault"; break;
                case "C222908": ret_Msgs = "Actuator Failure – EMS"; break;
                case "C224308": ret_Msgs = "Gear Fault"; break;
                case "C162808": ret_Msgs = "CAN Time-Out Cluster"; break;
                case "C165608": ret_Msgs = "CAN Signal Error-Cluster"; break;
                case "C181208": ret_Msgs = "CAN time-out Gateway"; break;
                case "C181708": ret_Msgs = "CAN Signal Error Gateway"; break;
                case "C162A08": ret_Msgs = "CAN Time-Out Cluster for Driving Mode (CLU13)"; break;
                case "C162B08": ret_Msgs = "CAN Time-out ECS_IVSS11"; break;
                case "C162C02": ret_Msgs = "CAN Signal Failure ECS_IVSS11"; break;
                case "C162E04": ret_Msgs = "IVSS SW Execution Time Failure"; break;
                case "C162F02": ret_Msgs = "CAN Signal Failure 4WD11"; break;
            }
            return ret_Msgs;
        }
        public static string ret_Errs(string pErr)
        {
            string ret_Msgs = "";

            switch (pErr)
            {
                case "10": ret_Msgs = "GeneralReject"; break;
                case "12": ret_Msgs = "SubFunctionNotSupported-invalidFormat"; break;
                case "13": ret_Msgs = "incorrectMessageLengthOrInvalidFormat"; break;
                case "22": ret_Msgs = "ConditionsNotCorrected"; break;
                case "24": ret_Msgs = "requestSequenceError"; break;
                case "31": ret_Msgs = "requestOutOfRange"; break;
                case "35": ret_Msgs = "invalidKey"; break;
                case "36": ret_Msgs = "exceededNumberOfAttempts"; break;
                case "37": ret_Msgs = "requiredTimeDelayNotExpired"; break;
                case "78": ret_Msgs = "requestCorrectlyReceived-ResponsePending"; break;
                case "7F": ret_Msgs = "serviceNotSupportedInActiveSession"; break;
            }

            return ret_Msgs;
        }
    }

    public static class MANDO__TM
    {
        #region Variable declaration
        public static string Part_No;
        public static string Diagnosis;
        public static string Supplier;
        public static string week;
        public static string Year;
        public static string Plant;
        public static string Line;
        public static string Shift;
        public static string DOW;
        public static string counter;
        public static string SW_ALG_1;
        public static string SW_ALG_2;
        public static string ALG_Edition_1;
        public static string ALG_Edition_2;

        public static string Reserved;
        public static string SW_Sys_1;
        public static string SW_Sys_2;
        public static string Edition_No_1;
        public static string Edition_No_2;

        public static string Pedal;
        #endregion

        #region Standard CAN
        public static bool SecurityAccess()         //0 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("27 03");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            string[] seed_key = NeoVI.Get_Data.Split(' ');

            if (seed_key[3] == "00" && seed_key[4] == "00") return true;

            string KeyString = Aloirithm(seed_key[3] + " " + seed_key[4]).ToString("X4");
            string Key1 = KeyString.Substring(0, 2);
            string Key2 = KeyString.Substring(2, 2);

            Ret = NeoVI.Ret_SendMsgs("27 02 " + Key1 + " " + Key2);
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static UInt32 Aloirithm(string HexSeed)
        {
            string[] HexD = HexSeed.Split(' ');
            uint Seed = H2Y.HexToUInt(HexD[0], HexD[1]);
            uint Ret = 0;
            uint DIAG_CRC_GEN_Code = 0x0D;
            uint GEN_CRC_KEY = (Seed << 3) & 0xFFFF;
            uint FCS_KEY = GEN_CRC_KEY;

            if (GEN_CRC_KEY >= 1024) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 128;
            if (512 <= GEN_CRC_KEY && GEN_CRC_KEY < 1024) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 64;
            if (256 <= GEN_CRC_KEY && GEN_CRC_KEY < 512) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 32;
            if (128 <= GEN_CRC_KEY && GEN_CRC_KEY < 256) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 16;
            if (64 <= GEN_CRC_KEY && GEN_CRC_KEY < 128) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 8;
            if (32 <= GEN_CRC_KEY && GEN_CRC_KEY < 64) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 4;
            if (16 <= GEN_CRC_KEY && GEN_CRC_KEY < 32) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 2;
            if (8 <= GEN_CRC_KEY && GEN_CRC_KEY < 16) GEN_CRC_KEY ^= DIAG_CRC_GEN_Code * 1;

            Ret = (FCS_KEY + GEN_CRC_KEY) & 0xFFFF;

            return Ret;
        }

        public static bool Start_Communication()    //1. 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("10 03");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Stop_Communication()     //2. 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("20");

            if (Ret) { ECUs.End_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Reset()              //3. 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("11 01");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Identification()     //4. 
        {
            bool Ret = true;

            #region Identification Clear
            Part_No = "";
            Diagnosis = "";
            Supplier = "";
            week = "";
            Year = "";
            Plant = "";
            Line = "";
            Shift = "";
            DOW = "";
            counter = "";
            SW_ALG_1 = "";
            SW_ALG_2 = "";
            ALG_Edition_1 = "";
            ALG_Edition_2 = "";

            Reserved = "";
            SW_Sys_1 = "";
            SW_Sys_2 = "";
            Edition_No_1 = "";
            Edition_No_2 = "";
            #endregion

            //0xC101 - 39
            //0xC102 - 11
            //0xC103 - 29(Integrated EPB system Only)
            //0xF101 - 2
            //0xF102 - 7
            //0xF103 - 3
            //0xF103 - 9
            //0xF104 - 12
            //0xF105 - 4
            //0xF107 - 4
            //0xF108 - 19
            //0xF18C - 15
            //0x2000 - 1

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 C1 01"); }
            if (Ret) { Identification_C101(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 C1 02"); }
            if (Ret) { Identification_C102(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 C1 03"); }
            if (Ret) { Identification_C103(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 01"); }
            if (Ret) { Identification_F101(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 02"); }
            if (Ret) { Identification_F102(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 03"); }
            if (Ret) { Identification_F103(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 04"); }
            if (Ret) { Identification_F104(NeoVI.Get_Data); }

            NeoVI.Debug_Message(" ******* ");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 05"); }
            if (Ret) { Identification_F105(NeoVI.Get_Data); }

            return Ret;
        }
        public static void Identification_C101(string pData) //4.1 Standard service data set
        {
            string[] Ident = pData.Split(' ');
            if (Ident.Length < 42) return;

            string PID = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) + H2Y.HexToASCII(Ident[6]) + H2Y.HexToASCII(Ident[7]);
            NeoVI.Debug_Message("0x00.Supported PID : " + PID);

            string RPM = (H2Y.HexToInt(Ident[8], Ident[9]) * 0.25).ToString();
            NeoVI.Debug_Message("0x01.Engine RPM : " + RPM);

            string speed = H2Y.HexTobyte(Ident[10]).ToString();
            NeoVI.Debug_Message("0x02.Vehicle speed : " + speed);

            string TPS = H2Y.HexTobyte(Ident[11]).ToString();
            NeoVI.Debug_Message("0x03.TPS : " + TPS);

            string Gear = H2Y.HexToBinary(Ident[12]).ToString();
            NeoVI.Debug_Message("0x04.Gear Position : " + Gear);

            string Battery = (H2Y.HexTobyte(Ident[13]) * 16 / 255).ToString();
            NeoVI.Debug_Message("0x05.Battery voltage : " + Battery);

            string G_sensor = (H2Y.HexTobyte(Ident[14]) * 6 / 255).ToString();
            NeoVI.Debug_Message("0x06.5 Volt reference (G sensor installed system Only) : " + G_sensor);

            string WSS_FL = H2Y.HexTobyte(Ident[15]).ToString();
            NeoVI.Debug_Message("0x07.Wheel speed sensor – Front Left : " + WSS_FL);

            string WSS_FR = H2Y.HexTobyte(Ident[16]).ToString();
            NeoVI.Debug_Message("0x08.Wheel speed sensor – Front Right : " + WSS_FR);

            string WSS_RL = H2Y.HexTobyte(Ident[17]).ToString();
            NeoVI.Debug_Message("0x09.Wheel speed sensor – Rear Left : " + WSS_RL);

            string WSS_RR = H2Y.HexTobyte(Ident[18]).ToString();
            NeoVI.Debug_Message("0x0A.Wheel speed sensor – Rear Right : " + WSS_RR);

            string Steering = H2Y.HexToBinary(Ident[19]).ToString();
            NeoVI.Debug_Message("0x0B.Reserved : " + Steering);

            string Longituinal = ((H2Y.HexTobyte(Ident[20]) - H2Y.HexTobyte("7F")) * 4 / 255).ToString();
            NeoVI.Debug_Message("0x0C.G Sensor – Longituinal (G sensor installed system Only) : " + Longituinal);

            string Lateral = H2Y.HexTobyte(Ident[21]).ToString();
            NeoVI.Debug_Message("0x0D.Reserved : " + Lateral);

            string Lamp = H2Y.HexToBinary(Ident[22]).ToString();
            NeoVI.Debug_Message("0x0E.Lamp (00=Off, 01=On, 11=Reserved) : " + Lamp);

            string Switch0 = H2Y.HexToBinary(Ident[23]).ToString();
            NeoVI.Debug_Message("0x0F.Switch 0 (00=Off, 01=On, 11=Reserved) : " + Switch0);

            string Relay = H2Y.HexToBinary(Ident[24]).ToString();
            NeoVI.Debug_Message("0x10.Relay (00=Off, 01=On, 11=Reserved) : " + Relay);

            string Motor = H2Y.HexToBinary(Ident[25]).ToString();
            NeoVI.Debug_Message("0x11.Motor (xxh : 00h=Off, 01h=On) : " + Motor);

            string Inlet = H2Y.HexToBinary(Ident[26]).ToString();
            NeoVI.Debug_Message("0x12.ABS inlet valve(00=Off, 01=On, 11=Reserved) : " + Inlet);

            string Outlet = H2Y.HexToBinary(Ident[27]).ToString();
            NeoVI.Debug_Message("0x13.ABS outlet valve (00=Off, 01=On, 11=Reserved) : " + Outlet);

            string ESC = H2Y.HexToBinary(Ident[28]).ToString();
            NeoVI.Debug_Message("0x14.ESC valve (00=Off, 01=On, 11=Reserved) : " + ESC);

            string Switch1 = H2Y.HexToBinary(Ident[29]).ToString();
            NeoVI.Debug_Message("0x15.Switch 1 (00=Off, 01=On, 11=Reserved) : " + Switch1);

            string Steering1 = ((H2Y.HexToInt(Ident[30], Ident[31]) - H2Y.HexToInt("8000")) / 10).ToString();
            NeoVI.Debug_Message("0x16.Steering(CAN type) Sensor range : -780˚ ~ 780˚ (ESC Only) : " + Steering1);

            string Yaw = ((H2Y.HexToInt(Ident[32], Ident[33]) - H2Y.HexToInt("0800")) / 630).ToString();
            NeoVI.Debug_Message("0x17.Yaw Sensor range : -1.5g ~ 1.5g (ESC Only) : " + Yaw);

            string Yaw1 = ((H2Y.HexToInt(Ident[34], Ident[35]) - H2Y.HexToInt("0800")) / 16).ToString();
            NeoVI.Debug_Message("0x18.Yaw Sensor range : -75˚/s ~ 75˚/s (ESC Only) : " + Yaw1);

            string Pressure = (H2Y.HexToInt(Ident[36], Ident[37])).ToString();
            NeoVI.Debug_Message("0x19.Pressure Sensor range : 0bar ~ 200bar (ESC Only) : " + Pressure);

            string Pedal_PDT = (H2Y.HexTobyte(Ident[38])).ToString();
            NeoVI.Debug_Message("0x1A.Sensor range : 0mm ~ 135mm (Only for system applied pedal travel sensor) : " + Pedal_PDT);

            string Pedal_PDF = (H2Y.HexTobyte(Ident[39])).ToString();
            NeoVI.Debug_Message("0x1B.Sensor range : 0mm ~ 135mm (Only for system applied pedal travel sensor) : " + Pedal_PDF);

            string Lamp_Status = H2Y.HexToBinary(Ident[40]).ToString();
            NeoVI.Debug_Message("0x1C.DBC Lamp & Switch (Only for system applied DBC) : " + Lamp_Status);

            string Sensor = Ident[41] + " " + Ident[42];
            NeoVI.Debug_Message("0x1D.Reserved : " + Sensor);
        }
        public static void Identification_C102(string pData) //4.2 Extension service data set
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 14) return;

            string PID = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) + H2Y.HexToASCII(Ident[6]) + H2Y.HexToASCII(Ident[7]);
            NeoVI.Debug_Message("0.Supported PID : " + PID);

            string FL = H2Y.HexToASCII(Ident[8]);
            string FR = H2Y.HexToASCII(Ident[9]);
            string RL = H2Y.HexToASCII(Ident[10]);
            string RR = H2Y.HexToASCII(Ident[11]);
            NeoVI.Debug_Message("1.Pressure range : 0bar ~ 200bar : FL:" + FL + ", FR:" + FR + ", RL:" + RL + ", RR:" + RR);

            string Vacuum = (H2Y.HexToInt(Ident[12], Ident[13]) / 10).ToString();
            NeoVI.Debug_Message("2.Vacuum Sensor range : 0kpa ~ 100kpa (Only for system applied Vacuum sensor) :" + Vacuum);

            string Status = H2Y.HexToASCII(Ident[14]);
            NeoVI.Debug_Message("3.Status (00=Off, 01=On, 10, 11=Reserved) :" + Status);
        }
        public static void Identification_C103(string pData) //4.3 EPB Standard service data set
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 32) return;

            string PID = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) + H2Y.HexToASCII(Ident[6]) + H2Y.HexToASCII(Ident[7]);
            NeoVI.Debug_Message("0.Supported PID : " + PID);

            string Switch = H2Y.HexToBinary(Ident[8]).ToString();
            NeoVI.Debug_Message("1.Switch (00=Off, 01=On, 11=Reserved) : " + Switch);

            string Switch1 = H2Y.HexToBinary(Ident[9]).ToString();
            NeoVI.Debug_Message("2.Switch1 (00=Off, 01=On, 11=Reserved) : " + Switch1);

            string Lamp = H2Y.HexToBinary(Ident[10]).ToString();
            NeoVI.Debug_Message("3.Lamp (00=Off, 01=On, 10=blinking, 11=Reserved) : " + Lamp);

            string Direction = H2Y.HexToBinary(Ident[11]).ToString();
            NeoVI.Debug_Message("4.Actuator Direction (0:Not Active, 1:Applying, 2:Releasing) : " + Direction);

            string Status = H2Y.HexToBinary(Ident[12]).ToString();
            NeoVI.Debug_Message("5.Actuator Status (0:Unknown, 1:Applied, 2:Released) : " + Status);

            string Motor1 = ((H2Y.HexToInt(Ident[13], Ident[14]) - 32767) / 10).ToString();
            NeoVI.Debug_Message("6.Motor #1 current : " + Motor1);

            string Motor2 = ((H2Y.HexToInt(Ident[15], Ident[16]) - 32767) / 10).ToString();
            NeoVI.Debug_Message("7.Motor #2 current : " + Motor2);

            string Sensor = H2Y.HexToInt(Ident[17], Ident[18]).ToString();
            NeoVI.Debug_Message("8.Force sensor (EPB integration type only) : " + Sensor);

            string Clutch = H2Y.HexToASCII(Ident[19]);
            NeoVI.Debug_Message("9.Clutch Sig1 Duty (MT Vehicle Only) : " + Clutch);

            string Clutch1 = H2Y.HexToASCII(Ident[20]);
            NeoVI.Debug_Message("10.Clutch Sig2 Duty (MT Vehicle Only) : " + Clutch1);

            string counter = H2Y.HexToASCII(Ident[21]) + H2Y.HexToASCII(Ident[22]) + H2Y.HexToASCII(Ident[23]) + H2Y.HexToASCII(Ident[24]);
            NeoVI.Debug_Message("11.Static operation count : " + counter);

            string counter1 = H2Y.HexToASCII(Ident[25]) + H2Y.HexToASCII(Ident[26]);
            NeoVI.Debug_Message("12.Dynamic operation count : " + counter1);

            string Hall_counter = H2Y.HexToBinary(Ident[27]).ToString();
            NeoVI.Debug_Message("13.RL-Hall Counter Direction (0:Not Active, 1:Applying, 2:Releasing) : " + Hall_counter);

            string Hall_counter1 = H2Y.HexToBinary(Ident[28]).ToString();
            NeoVI.Debug_Message("14.RR-Hall Counter Direction (0:Not Active, 1:Applying, 2:Releasing) : " + Hall_counter1);

            string Hall_counter2 = (H2Y.HexToInt(Ident[29], Ident[30]) - 32767).ToString();
            NeoVI.Debug_Message("15.RL-Hall Counter Position : " + Hall_counter2);

            string Hall_counter3 = (H2Y.HexToInt(Ident[31], Ident[32]) - 32767).ToString();
            NeoVI.Debug_Message("16.RR-Hall Counter Position : " + Hall_counter3);
        }
        public static void Identification_F101(string pData) //4.4 Vehicle Name
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 5) return;

            string Vehicle = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]);
            NeoVI.Debug_Message("0.Vehicle name (ASCII code) : " + Vehicle);
        }
        public static void Identification_F102(string pData) //4.5 Vehicle Function
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 10) return;

            string Vehicle = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) +
                             H2Y.HexToASCII(Ident[6]) + H2Y.HexToASCII(Ident[7]) +
                             H2Y.HexToASCII(Ident[8]) + H2Y.HexToASCII(Ident[9]) +
                             H2Y.HexToASCII(Ident[10]);
            NeoVI.Debug_Message("0.Vehicle name (ASCII code) : " + Vehicle);
        }
        public static void Identification_F103(string pData) //4.6 ECU Version 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 6) return;

            string Version = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) + H2Y.HexToASCII(Ident[6]);
            NeoVI.Debug_Message("0.ECU Version (ASCII code) : " + Version);
        }
        public static void Identification_F104(string pData) //4.7 Software Version 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 15) return;

            string Version = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) + H2Y.HexToASCII(Ident[6]) +
                             H2Y.HexToASCII(Ident[7]) + H2Y.HexToASCII(Ident[8]) + H2Y.HexToASCII(Ident[9]) +
                             H2Y.HexToASCII(Ident[10]) + H2Y.HexToASCII(Ident[11]) + H2Y.HexToASCII(Ident[12]) +
                             H2Y.HexToASCII(Ident[13]) + H2Y.HexToASCII(Ident[14]) + H2Y.HexToASCII(Ident[15]);
            NeoVI.Debug_Message("0.Software Version (ASCII code) : " + Version);
        }
        public static void Identification_F105(string pData) //4.8 Software Release Date
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 7) return;

            string Release = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) +
                             H2Y.HexToASCII(Ident[6]) + H2Y.HexToASCII(Ident[7]);
            NeoVI.Debug_Message("0.Software Release Date(ASCII code) : " + Release);
        }

        public static bool Read__DTC()              //5 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("19 02 08");

            if (Ret) { ECUs.DTC_Read = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Clear_DTC()              //6 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("14 FF FF FF");
            if (Ret) { ECUs.DTCClear = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Check_Signals()          //7 
        {
            //return NeoVI.Ret_SendMsgs("21 02");

            bool Ret = true;

            Pedal = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 C1 01"); }
            if (Ret)
            {
                Pedal = NeoVI.Get_Data;
                ECUs.SigPedal = Ret_OnOff(Pedal);
                NeoVI.Debug_Message("Brake Pedal Position : " + ECUs.SigPedal);
            }

            return Ret;
        }
        public static string Ret_OnOff(string pData)
        {
            string[] Hex = pData.Split(' ');
            if (Hex.Length < 42) { return "Err"; }

            string strValue = H2Y.HexToBinary(Hex[23], 5, 2);  //Hex[4];
            string RetValue = "None";

            NeoVI.Debug_Message("0x0F.Switch 0 (00=Off, 01=On, 11=Reserved) : " + strValue);

            switch (strValue)
            {
                case "00": RetValue = "OFF"; break;
                case "01": RetValue = "ON"; break;
            }

            return RetValue;
        }
        public static bool WSS_Test()               //8 
        {
            bool Ret = true;

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 C1 01"); }
            if (Ret)
            {
                Ret_WSS_Speed(NeoVI.Get_Data);

                NeoVI.Debug_Message("WSS FL : " + ECUs.WSS_FL);
                NeoVI.Debug_Message("WSS FR : " + ECUs.WSS_FR);
                NeoVI.Debug_Message("WSS RL : " + ECUs.WSS_RL);
                NeoVI.Debug_Message("WSS RR : " + ECUs.WSS_RR);
            }

            return Ret;
        }
        private static void Ret_WSS_Speed(string pData) //8.1 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 42) return;

            ECUs.WSS_FL = H2Y.HexTobyte(Ident[15]);
            ECUs.WSS_FR = H2Y.HexTobyte(Ident[16]);
            ECUs.WSS_RL = H2Y.HexTobyte(Ident[17]);
            ECUs.WSS_RR = H2Y.HexTobyte(Ident[18]);
        }

        //연결 유지
        public static bool Tester_Present()         //TesterPresent(3E hex) Service 테스터 존재
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("3E 00");      //(00:Response required, 80:Response not required)
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Message_Falg()           //EnableNormalMessageTransmission (29 hex) service
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("29 01");      //(01:ResponseRequired, 02:NoResponseRequired)
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }

        public static bool Dynamic_Step(int idx)    //Dynamic ABS Test
        {
            bool Ret = true;
            //100km Over Rock
            switch (idx)
            {
                case 0: Ret = Start_Communication(); break;
                case 1: Ret = NeoVI.Ret_SendMsgs("2F F0 6F 03 3C 11 00 00"); break; //1. ABS Pressure release FL (3C:600, 28:400ms)    
                case 2: Ret = NeoVI.Ret_SendMsgs("2F F0 6F 03 3C 22 00 00"); break; //2. ABS Pressure release FR (3C:600, 28:400ms)    
                case 3: Ret = NeoVI.Ret_SendMsgs("2F F0 6F 03 3C 44 00 01"); break; //3. ABS Pressure release RL (3C:600, 28:400ms)    
                case 4: Ret = NeoVI.Ret_SendMsgs("2F F0 6F 03 28 88 00 01"); break; //4. ABS Pressure release RR (3C:600, 28:400ms)    

                case 5: Ret = NeoVI.Ret_SendMsgs("2F F0 6F 03 00 00 00 00"); break; //5. Stop
            }
            //
            return Ret;
        }
        public static bool Dynamic_Auto()           //Dynamic Test
        {
            bool Ret = true;

            float T2 = 400;
            float T3 = 2000;

            double Vlv_Time = 0;
            double Old_Time = 0;
            double Off_Time = DateTime.Now.Ticks;
            bool ECU_Flag = true;
            byte ECU_Setp = 0;

            while (true)
            {
                if (!ECU_Flag) { ECU_Flag = true; }

                Vlv_Time = TSet.ABSv_Time + ((DateTime.Now.Ticks - Off_Time) / H2Y.tick_Dvd);

                if (ECU_Flag && ECU_Setp == 0)
                {
                    Ret = Start_Communication(); ECU_Setp = 1; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 1)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 2; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 2)
                {
                    Ret = Dynamic_Step(1); ECU_Setp = 3; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 3)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 4; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 4)
                {
                    Ret = Dynamic_Step(2); ECU_Setp = 5; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 5)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 6; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 6)
                {
                    Ret = Dynamic_Step(3); ECU_Setp = 7; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 7)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 8; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 8)
                {
                    Ret = Dynamic_Step(4); ECU_Setp = 9; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 9)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 10; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 10)
                {
                    Ret = Dynamic_Step(5); ECU_Setp = 11; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 11)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T3, 1000)) { ECU_Setp = 12; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 12)
                {
                    ECUs.ABS_Step = 5;
                    break;
                }
            }

            return Ret;
        }
        public static bool ESP_Step(int idx)        //Dynamic ESP Test
        {
            bool Ret = true;

            switch (idx)
            {
                case 1: Ret = true; break;
                case 2: Ret = true; break;
            }

            return Ret;
        }

        public static bool ESS_LampTest()           //10 ESS Lamp(Option) 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("2F F0 24 03");
            if (Ret) { ECUs.ESS_Lamp = NeoVI.Get_Data; }

            return Ret;
        }

        public static bool Single_IO_Idx(string pHex, int idx) //13. DataIdentifier list for Single I/O control for ABS/ESC system
        {
            bool Ret = true;

            switch (idx)
            {
                case 0: Ret = NeoVI.Ret_SendMsgs(pHex + " 00"); break;
                case 1: Ret = NeoVI.Ret_SendMsgs(pHex + " 03"); break;
            }

            return Ret;
        }
        #endregion

        public static string ret_DTCs(string pCode)
        {
            string ret_Msgs = "";

            switch (pCode)
            {
                case "C110101": ret_Msgs = "Battery voltage high"; break;
                case "C110201": ret_Msgs = "Battery Voltage Low"; break;
                case "C111301": ret_Msgs = "5V Sensor Power Fail"; break;
                case "C111401": ret_Msgs = "12V Sensor Power Fail"; break;
                case "C222707": ret_Msgs = "Excessive temperature of brake disc"; break;
                case "C120001": ret_Msgs = "Wheel Speed Sensor Front-LH Open / Short"; break;
                case "C120102": ret_Msgs = "Wheel Speed Sensor Front-LH Range / Performance / Intermittent"; break;
                case "C120202": ret_Msgs = "Wheel Speed Sensor Front-LH Invalid / No Signal"; break;
                case "C120301": ret_Msgs = "Wheel Speed Sensor Front-RH Open / Short"; break;
                case "C120402": ret_Msgs = "Wheel Speed sensor Front-RH Range / Performance / Intermittent"; break;
                case "C120502": ret_Msgs = "Wheel Speed Sensor Front-RH Invalid / No Signal"; break;
                case "C120601": ret_Msgs = "Wheel Speed Sensor Rear-LH Open / Short"; break;
                case "C120702": ret_Msgs = "Wheel Speed Sensor Rear-LH Range / Performance / Intermittent"; break;
                case "C120802": ret_Msgs = "Wheel Speed Sensor Rear-LH Invalid / No Signal"; break;
                case "C120901": ret_Msgs = "Wheel Speed Sensor Rear-RH Open / Short"; break;
                case "C121002": ret_Msgs = "Wheel Speed Sensor Rear-RH Range / Performance / Intermittent"; break;
                case "C121102": ret_Msgs = "Wheel Speed Sensor Rear-RH Invalid / No Signal"; break;
                case "C160404": ret_Msgs = "ECU hardware error"; break;
                case "C211201": ret_Msgs = "Valve relay error"; break;
                case "C238001": ret_Msgs = "ABS/TCS/ESP Valve Error"; break;
                case "C240201": ret_Msgs = "Motor electrical"; break;
                case "C123501": ret_Msgs = "Pressure Sensor (Primary) – Electrical"; break;
                case "C123702": ret_Msgs = "Pressure sensor – other"; break;
                case "C162308": ret_Msgs = "CAN time-out Steering angle sensor"; break;
                case "C126002": ret_Msgs = "Steering angle sensor circuit – signal"; break;
                case "C126104": ret_Msgs = "Steering angle sensor is not calibrated"; break;
                case "C126402": ret_Msgs = "SAS Offset Error, Noisy signal Stick"; break;
                case "C164308": ret_Msgs = "CAN Time-Out Yaw & G Sensor"; break;
                case "C128302": ret_Msgs = "Lateral G Sensor / Longitudinal G Sensor / Yaw Rate Sensor - Signal Error"; break;
                case "C127401": ret_Msgs = "G sensor error"; break;
                case "C127502": ret_Msgs = "G Sensor Range / Performance Error"; break;
                case "C128208": ret_Msgs = "Yaw Rate & Lateral G Sensor – Electrical "; break;
                case "C128504": ret_Msgs = "Uncalibrated Ax Signal"; break;
                case "C128601": ret_Msgs = "YAW RATE&2G Sensor Fail-Replace SRSCM"; break;

                case "C137801": ret_Msgs = "Pedal Signal Open/Short Error"; break;
                case "C137902": ret_Msgs = "Invalid Pedal Signal Error"; break;
                case "C138001": ret_Msgs = "Pedal Signal Not Calibrated"; break;
                case "C138501": ret_Msgs = "Vacuum Sensor - Electrical"; break;
                case "C138602": ret_Msgs = "Vacuum Sensor vehicle -Signal"; break;
                case "C212601": ret_Msgs = "Vacuum Pump Relay Drive Pin Open/Short"; break;
                case "C223102": ret_Msgs = "Vacuum Pump System Fail"; break;
                case "C135801": ret_Msgs = "AVH switch Error"; break;
                case "C150301": ret_Msgs = "TCS switch Error"; break;
                case "C152001": ret_Msgs = "Clutch switch Error"; break;
                case "C152601": ret_Msgs = "DBC switch Error"; break;
                case "C152701": ret_Msgs = "Reverse Gear Signal Error"; break;
                case "C151301": ret_Msgs = "Brake switch Circuit Error"; break;
                case "C154201": ret_Msgs = "Brake Circuit Error"; break;
                case "C213001": ret_Msgs = "Brake Lamp Relay Error"; break;
                case "C213101": ret_Msgs = "ESS Brake Lamp Relay error"; break;
                case "C170204": ret_Msgs = "Auto Coding Error"; break;
                case "C110001": ret_Msgs = "Battery Voltage"; break;
                case "C110301": ret_Msgs = "Ignition Voltage"; break;
                case "C150101": ret_Msgs = "Switch Failure"; break;
                case "C153901": ret_Msgs = "In Gear Switch Error"; break;
                case "C171004": ret_Msgs = "Assembly-Line Setup is Still Active"; break;
                case "C222001": ret_Msgs = "Rear-LH actuator"; break;
                case "C222401": ret_Msgs = "Rear-RH (or Rear) actuator"; break;
                case "C241601": ret_Msgs = "Motor Short or Open - LH"; break;
                case "C241701": ret_Msgs = "Motor Short or Open - RH"; break;
                case "C138201": ret_Msgs = "EPB Hall Sensor failure - LH"; break;
                case "C138301": ret_Msgs = "EPB Hall Sensor failure - RH"; break;
                case "C220201": ret_Msgs = "EPB Reclamp repetition"; break;
                case "C171304": ret_Msgs = "Factory Mode Not Disabled"; break;

                case "C161108": ret_Msgs = "CAN time-out EMS"; break;
                case "C161208": ret_Msgs = "CAN time-out TCU"; break;
                case "C161308": ret_Msgs = "CAN signal error EMS"; break;
                case "C161608": ret_Msgs = "C-CAN bus off"; break;
                case "C160E08": ret_Msgs = "P-CAN Bus Off"; break;
                case "C162708": ret_Msgs = "CAN time-out 4WD"; break;
                case "C222208": ret_Msgs = "Actuator Failure – 4WD"; break;
                case "C168708": ret_Msgs = "CAN time-out VSM2 (MDPS)"; break;
                case "C168808": ret_Msgs = "VSM2 (MDPS) Signal Error"; break;
                case "C165108": ret_Msgs = "CAN time-out EPB"; break;
                case "C165208": ret_Msgs = "CAN Signal Error EPB "; break;
                case "C182E08": ret_Msgs = "CAN Time-out FCS"; break;
                case "C163808": ret_Msgs = "ACC communication Error"; break;
                case "C165008": ret_Msgs = "CAN Signal Error ACC"; break;
                case "C164908": ret_Msgs = "CAN Time-Out EMS for ACC"; break;
                case "C16B808": ret_Msgs = "AEB Communication Error"; break;
                case "C164808": ret_Msgs = "CAN Signal Error EMS for ACC"; break;
                case "C222808": ret_Msgs = "TCU Signal Fault"; break;
                case "C222908": ret_Msgs = "Actuator Failure – EMS"; break;
                case "C224308": ret_Msgs = "Gear Fault"; break;
                case "C162808": ret_Msgs = "CAN Time-Out Cluster"; break;
                case "C165608": ret_Msgs = "CAN Signal Error-Cluster"; break;
                case "C181208": ret_Msgs = "CAN time-out Gateway"; break;
                case "C181708": ret_Msgs = "CAN Signal Error Gateway"; break;
                case "C162A08": ret_Msgs = "CAN Time-Out Cluster for Driving Mode (CLU13)"; break;
                case "C162B08": ret_Msgs = "CAN Time-out ECS_IVSS11"; break;
                case "C162C02": ret_Msgs = "CAN Signal Failure ECS_IVSS11"; break;
                case "C162E04": ret_Msgs = "IVSS SW Execution Time Failure"; break;
                case "C162F02": ret_Msgs = "CAN Signal Failure 4WD11"; break;
            }
            return ret_Msgs;
        }
        public static string ret_Errs(string pErr)
        {
            string ret_Msgs = "";

            switch (pErr)
            {
                case "10": ret_Msgs = "GeneralReject"; break;
                case "12": ret_Msgs = "SubFunctionNotSupported-invalidFormat"; break;
                case "13": ret_Msgs = "incorrectMessageLengthOrInvalidFormat"; break;
                case "22": ret_Msgs = "ConditionsNotCorrected"; break;
                case "24": ret_Msgs = "requestSequenceError"; break;
                case "31": ret_Msgs = "requestOutOfRange"; break;
                case "35": ret_Msgs = "invalidKey"; break;
                case "36": ret_Msgs = "exceededNumberOfAttempts"; break;
                case "37": ret_Msgs = "requiredTimeDelayNotExpired"; break;
                case "78": ret_Msgs = "requestCorrectlyReceived-ResponsePending"; break;
                case "7F": ret_Msgs = "serviceNotSupportedInActiveSession"; break;
            }

            return ret_Msgs;
        }
    }

    public static class CHERY1BOX
    {
        #region Variable declaration
        public static string Part_No;
        public static string SW_No;
        public static string Voltage;
        public static string Angle;
        public static string Lateral;
        public static string Longitudinal;
        public static string YawRate;
        public static string E_F_Byte;
        public static string Pulse;
        public static string PulseResult;
        public static string Plunger;
        public static string PlungerResult;
        public static string Conditioning;
        public static string ConditioningResult;
        public static string TMC_X_PFS;
        public static string TMC_X_PFS_Result;
        public static string TMC_O_PFS;
        public static string TMC_O_PFS_Result;
        public static string Cylinder;
        public static string Cylinder_Result;
        public static string Deactivation;
        public static string Activation;
        #endregion

        #region Standard CAN
        public static bool SecurityAccess()         //0 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("27 03");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            string[] seed_key = NeoVI.Get_Data.Split(' ');

            if (seed_key[3] == "00" && seed_key[4] == "00" && seed_key[5] == "00" && seed_key[6] == "00") return true;

            string KeyString = Aloirithm(seed_key[3] + " " + seed_key[4] + " " + seed_key[5] + " " + seed_key[6]).ToString("X8");
            string Key1 = KeyString.Substring(0, 2);
            string Key2 = KeyString.Substring(2, 2);
            string Key3 = KeyString.Substring(4, 2);
            string Key4 = KeyString.Substring(6, 2);

            Ret = NeoVI.Ret_SendMsgs("27 04 " + Key1 + " " + Key2 + " " + Key3 + " " + Key4);
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static UInt32 Aloirithm(string HexSeed)
        {
            string[] HexD = HexSeed.Split(' ');
            uint Seed = H2Y.HexToUInt(HexD[0], HexD[1], HexD[2], HexD[3]);
            uint key = (((Seed >> 2) ^ Seed) << 3) ^ Seed;
            
            return key;
        }

        public static bool Start_Communication()    //1. 
        {
            bool Ret = true;

            //Ret = NeoVI.Ret_SendMsgs("10 01");
            //if (Ret) 
            //{ 
                ECUs.Stt_Comm = NeoVI.Get_Data;
                Ret = NeoVI.Ret_SendMsgs("10 03");
                Thread.Sleep(100);
                Ret = SecurityAccess();
            //}

            return Ret;
        }
        public static bool Start_Secceon()    //1. 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("10 03");
            if (Ret)
            {
                ECUs.Stt_Comm = NeoVI.Get_Data;
             

                // Ret = NeoVI.Ret_SendMsgs("10 03");

                //  Ret = SecurityAccess();
            }

            return Ret;
        }
        public static bool Stop_Communication()     //2. 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("10 01");

            if (Ret) { ECUs.End_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Reset()              //3. 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("11 03");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool SendFlow()     //4. 
        {
            NeoVI.CAN_Transmit("30 00 0A");
            return true;
        }
        public static bool ECU_Identification()     //4. 
        {
            bool Ret = true;

            #region Identification Clear
            Part_No = "";
            SW_No = "";
            #endregion

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 87"); }
            if (Ret) { Identification_F187(NeoVI.Get_Data); }

            //NeoVI.Debug_Message(" ******* ");

            //if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 87"); }
            //if (Ret) { Identification_F187(NeoVI.Get_Data); }

            //NeoVI.Debug_Message(" ******* ");

            return Ret;
        }
        public static void Identification_F187(string pData) //4.1 Spare Part Numeber
        {
            //0x00000720 >03 22 f1 8c 00 00 00 00 ID로 데이터 읽기 - ECU 소프트웨어 번호 (DID F18C)
            //0x00000730 >10 23 62 f1 8c 39 36 32 ID로 데이터 읽기(F18C)에 대한 긍정 응답 - 파트 1
            //0x00000720 >30 00 0a 00 00 00 00 00 흐름 제어
            //0x00000730 >21 33 30 32 38 30 30 31 ID로 데이터 읽기(F18C)에 대한 긍정 응답 - 파트 2
            //0x00000730 >22 33 31 30 20 20 20 20 ID로 데이터 읽기(F18C)에 대한 긍정 응답 - 파트 3
            //0x00000730 >23 20 20 20 20 20 20 20 ID로 데이터 읽기(F18C)에 대한 긍정 응답 - 파트 4
            //0x00000730 >24 20 20 20 20 20 20 20 ID로 데이터 읽기(F18C)에 대한 긍정 응답 - 파트 5
            //0x00000730 >25 20 00 00 00 00 00 00 ID로 데이터 읽기(F18C)에 대한 긍정 응답 - 파트 6

            //>10 23 62 f1 8c 39 36 32 
            //>21 33 30 32 38 30 30 31 
            //>22 33 31 30 20 20 20 20 
            //>23 20 20 20 20 20 20 20 
            //>24 20 20 20 20 20 20 20 
            //>25 20 00 00 00 00 00 00 
            //        1  2  3  4  5  6  7 
            //>10 23 62 F1 87 39 36 32 33 30 32 38 30 30 31 33 31 30 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 00 00 00 00 00 00 
            //>      62 F1 87 39 36 32 33 30 32 38 30 30 31 33 31 30 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 


            string[] Ident = pData.Split(' ');
            if (Ident.Length < 38) return;

            Part_No = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) + H2Y.HexToASCII(Ident[6]) + H2Y.HexToASCII(Ident[7]) +
                      H2Y.HexToASCII(Ident[8]) + H2Y.HexToASCII(Ident[9]) + H2Y.HexToASCII(Ident[10]) + H2Y.HexToASCII(Ident[11]) +
                      H2Y.HexToASCII(Ident[12]) + H2Y.HexToASCII(Ident[13]) + H2Y.HexToASCII(Ident[14]) + H2Y.HexToASCII(Ident[15]) + H2Y.HexToASCII(Ident[16]);
            NeoVI.Debug_Message("Spare part Number : " + Part_No);
        }
        public static void Identification_F188(string pData) //4.2 ECU Software Number
        {
            string[] Ident = pData.Split(' ');

            //0x00000720 >03 22 f1 88 00 00 00 00	차량 제조사 스페어 파트 번호 (DID F188)
            //0x00000730 >10 0b 62 f1 88 30 31 2e	ID로 데이터 읽기(F188)에 대한 긍정 응답 - 파트 1
            //0x00000720 >30 00 0a 00 00 00 00 00	흐름 제어
            //0x00000730 >21 30 31 2e 30 30 00 00	ID로 데이터 읽기(F188)에 대한 긍정 응답 - 파트 2
            //10 0b 62 f1 88 30 31 2e 21 30 31 2e 30 30 00 00
            //62 f1 88 30 31 2e 30 31 2e 30 30
            //

            if (Ident.Length < 11) return;

            SW_No = H2Y.HexToASCII(Ident[4]) + H2Y.HexToASCII(Ident[5]) + H2Y.HexToASCII(Ident[6]) + H2Y.HexToASCII(Ident[7]) +
                    H2Y.HexToASCII(Ident[8]) + H2Y.HexToASCII(Ident[9]) + H2Y.HexToASCII(Ident[10]) + H2Y.HexToASCII(Ident[11]);
            NeoVI.Debug_Message("ECU Software Number : " + SW_No);
        }

        public static void Read_SensorData()
        {
            Read_BatteryVoltage();
            H2Y.Sleep(100);
            Read_SteeringAngle();
            H2Y.Sleep(100);
            Read_LateralAcceleration();
            H2Y.Sleep(100);
            Read_LongitudinalAcceleration();
            H2Y.Sleep(100);
            Read_YawRate();
            H2Y.Sleep(100);
            Check_ProcessByte();
        }
        public static bool Read_BatteryVoltage()     
        {
            bool Ret = true;

            Voltage = "";

            for (int cnt = 0; cnt < 3; cnt++)
            {
                Ret = NeoVI.Ret_SendMsgs("22 30 11");
                if (Ret) break;
            }
            if (Ret) { Voltage = Ret_Voltage(NeoVI.Get_Data); }

            NeoVI.Debug_Message("Read Battery Voltage : " + Voltage);

            return Ret;
        }
        private static string Ret_Voltage(string pData)
        {
            //04 62 30 11 8d 00 00 00
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 4) return "";

            return (H2Y.HexTobyte(Ident[4]) * 0.1).ToString();
        }

        public static bool Read_SteeringAngle()
        {
            bool Ret = true;

            Angle = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 30 0C"); }
            if (Ret) { Angle = Ret_Angle(NeoVI.Get_Data); }

            NeoVI.Debug_Message("Read Steering Angle : " + Angle);

            return Ret;
        }
        private static string Ret_Angle(string pData)
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 4) return "";

            return (H2Y.HexTobyte(Ident[4]) * 0.1).ToString();
        }
        public static bool Read_LateralAcceleration()
        {
            bool Ret = true;

            Lateral = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 30 0C"); }
            if (Ret) { Lateral = Ret_Lateral(NeoVI.Get_Data); }

            NeoVI.Debug_Message("Read Lateral Acceleration : " + Lateral);

            return Ret;
        }
        private static string Ret_Lateral(string pData)
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 4) return "";

            return (H2Y.HexTobyte(Ident[4]) * 0.027).ToString();
        }
        public static bool Read_LongitudinalAcceleration()
        {
            bool Ret = true;

            Longitudinal = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 30 0C"); }
            if (Ret) { Longitudinal = Ret_Longitudinal(NeoVI.Get_Data); }

            NeoVI.Debug_Message("Read Longitudinal Acceleration : " + Longitudinal);

            return Ret;
        }
        private static string Ret_Longitudinal(string pData)
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 4) return "";

            return (H2Y.HexTobyte(Ident[4]) * 0.027).ToString();
        }
        public static bool Read_YawRate()
        {
            bool Ret = true;

            YawRate = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 30 0C"); }
            if (Ret) { YawRate = Ret_YawRate(NeoVI.Get_Data); }

            NeoVI.Debug_Message("Read Yaw Rate : " + YawRate);

            return Ret;
        }
        private static string Ret_YawRate(string pData)
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 4) return "";

            return (H2Y.HexTobyte(Ident[4]) * 0.002).ToString();
        }
        public static bool Check_ProcessByte()
        {
            bool Ret = true;

            E_F_Byte = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 30 01"); }
            if (Ret) { E_F_Byte = Ret_ProcessByte(NeoVI.Get_Data); }

            switch (E_F_Byte)
            {
                case "00": NeoVI.Debug_Message("Check E&F Process Byte : " + E_F_Byte + " : Filling-in not completed"); break;
                case "01": NeoVI.Debug_Message("Check E&F Process Byte : " + E_F_Byte + " : Filling-in completed and OK"); break;
                case "02": NeoVI.Debug_Message("Check E&F Process Byte : " + E_F_Byte + " : Filling-in completed and NOK"); break;
                case "FF": NeoVI.Debug_Message("Check E&F Process Byte : " + E_F_Byte + " : The delivery state"); break;
            }

            return Ret;
        }
        private static string Ret_ProcessByte(string pData)
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 4) return "";

            return H2Y.HexTobyte(Ident[4]).ToString();
        }

        public static void Comfort_Pulse()
        {
            bool Ret = true;

            for (int cnt = 0; cnt < 3; cnt++)
            {
                Ret = Calibration();
                if (Ret) break;
            }
            
            if(Ret)
            {
                for (int cnt = 0; cnt < 20; cnt++)
                {
                    H2Y.Sleep(300);
                    if (Calibration_Result())
                    {
                        break;
                    }
                }
            }
        }
        public static bool Calibration()
        {
            bool Ret = true;
            //>04 31 01 30 3E       루틴 제어 - 루틴 시작 - VPC 컴포트 펄스 캘리브레이션 (303E)
            //>05 71 01 30 3E 01    루틴 제어에 대한 긍정 응답 - 루틴이 성공적으로 시작됨

            Pulse = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 01 30 3E"); }
            if (Ret) 
            {
                Pulse = NeoVI.Get_Data;
                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 5) return false;
                NeoVI.Debug_Message("Comfort Pulse Calibration : " + H2Y.HexTobyte(Ident[5]).ToString());
            }

            return Ret;
        }
        public static bool Calibration_Result()
        {
            bool Ret = true;
            //>04 31 03 30 3e                   루틴 제어 - 루틴 결과 요청 - VPC 컴포트 펄스 캘리브레이션 (303E)
            //>09 71 03 30 3e 02 00 00 00 01    루틴 제어에 대한 긍정 응답 - 결과 

            PulseResult = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 03 30 3E"); }
            if (Ret)
            {
                PulseResult = NeoVI.Get_Data;

                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 9) return false;
                NeoVI.Debug_Message("Comfort Pulse Calibration Result : " + H2Y.HexTobyte(Ident[5]).ToString() + H2Y.HexTobyte(Ident[9]).ToString());
                                                                            //02완료                              01성공

                if ("02" == H2Y.HexTobyte(Ident[5]).ToString() && "01" == H2Y.HexTobyte(Ident[9]).ToString())
                {
                    Ret = true;
                }
                else
                {
                    Ret = false;
                }
            }

            return Ret;
        }

        public static void LeakageAndAirTest()
        {
            bool Ret = true;

            for (int cnt = 0; cnt < 3; cnt++)
            {
                Ret = PlungerTest();
                if (Ret) break;
            }

            if (Ret)
            {
                for (int cnt = 0; cnt < 100; cnt++)
                {
                    H2Y.Sleep(300);
                    if (PlungerTest_Result())
                    {
                        break;
                    }
                }
            }
        }
        public static bool PlungerTest()
        {
            bool Ret = true;
            //>0B 31 01 30 3D 01 03 C0 01 00 02 06  플런저 요청 마라미터  /// c001: P1 14Bar, 0002: V1 4mm/s, 06: T1 6s, 00: T2 0s
            //>05 71 01 30 3d 01                    플런저 테스트에 대한 긍정 응답 (01)


            NeoVI.Debug_Message("PlungerTest");
            Plunger = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 01 30 3D 01 03 C0 01 00 02 06"); }
            if (Ret) 
            {
                Plunger = NeoVI.Get_Data;
                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 5) return false;
                NeoVI.Debug_Message("Plunger test : " + H2Y.HexTobyte(Ident[5]).ToString());
                

                if ("01" == Ident[5])
                {
                    
                    Ret = true;
                }
                else
                {
                    Ret = false;
                }
            }

            return Ret;
        }
        public static bool PlungerTest_Result()
        {
            bool Ret = true;
            //>04 31 03 30 3e                   루틴 제어 - 루틴 결과 요청 - VPC 컴포트 펄스 캘리브레이션 (303E)
            //>09 71 03 30 3e 02 00 00 00 01    루틴 제어에 대한 긍정 응답 - 결과

            PlungerResult = "";
            
            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 03 30 3D"); }
            if (Ret)
            {
                PlungerResult = NeoVI.Get_Data;

                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 9) return false;
                NeoVI.Debug_Message("Plunger test Result : " + Ident[5]);
                Console.WriteLine("Result : " + NeoVI.Get_Data);
                if ("02" == Ident[5])
                {
                    Ret = true;
                    NeoVI.Debug_Message("Plunger test Finish");

                }
                else
                {
                    Ret = false;
                }
            }

            return Ret;
        }

        public static void BrakeConditioningTest()
        {
            bool Ret = true;

            for (int cnt = 0; cnt < 3; cnt++)
            {
                Ret = BrakeConditioning();
                if (Ret) break;
            }

            if (Ret)
            {
                for (int cnt = 0; cnt < 100; cnt++)
                {
                    H2Y.Sleep(300);
                    if (BrakeConditioning_Result())
                    {
                        break;
                    }
                }
            }
        }
        public static bool BrakeConditioning()
        {
            bool Ret = true;

            //>0B 31 01 30 3D 04 0C 80 0A 00 01 00  800a: P1 84Bar, 0001: V1 2mm/s, 00: T1 0s, 00: T2 0s
            //>05 71 01 30 3D 01 

            Conditioning = "";

            NeoVI.Debug_Message("BrakeConditioning");

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 01 30 3D 04 0C 80 0A 00 01 00"); }
            if (Ret) 
            {
                Conditioning = NeoVI.Get_Data;
                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 5) return false;
                NeoVI.Debug_Message("Brake conditioning : " + H2Y.HexTobyte(Ident[5]).ToString());

                if ("01" == Ident[5])
                {
                    Ret = true;
                }
                else
                {
                    Ret = false;
                }
            }

            return Ret;
        }
        public static bool BrakeConditioning_Result()
        {
            bool Ret = true;
            //>04 31 03 30 3D 
            //>12 71 03 30 3D 02 00 03 C8 03 98 03 EA 03 9E 03 D8 03 9E 
            
            ConditioningResult = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 03 30 3D"); }
            if (Ret)
            {
                ConditioningResult = NeoVI.Get_Data;

                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 18) return false;
                NeoVI.Debug_Message("BrakeConditioning : " + H2Y.HexTobyte(Ident[5]).ToString());

                if ("02" == Ident[5])
                {
                    NeoVI.Debug_Message("BrakeConditioning Finish");
                    Ret = true;
                }
                else
                {
                    Ret = false;
                }
            }

            return Ret;
        }

        public static void TMC_Without_PFS_Test()
        {
            bool Ret = true;

            for (int cnt = 0; cnt < 3; cnt++)
            {
                Ret = TMC_Without_PFS();
                if (Ret) break;
            }

            if (Ret)
            {
                for (int cnt = 0; cnt < 100; cnt++)
                {
                    H2Y.Sleep(300);
                    if (TMC_Without_PFS_Result())
                    {
                        break;
                    }
                }
            }
        }
        public static bool TMC_Without_PFS()
        {
            bool Ret = true;

            //>0B 31 01 30 3D 02 03 C0 02 80 00 00  c002: P1 22bar, 8000: V1 1mm/s, 00: T1 0s, 00: T2 0s
            //>05 71 01 30 3D 01    

            TMC_X_PFS= "";
            NeoVI.Debug_Message("TMC_Without_PFS");
            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 01 30 3D 02 03 C0 02 80 00 00"); }
            if (Ret)
            {
                TMC_X_PFS = NeoVI.Get_Data;
                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 5) return false;
                NeoVI.Debug_Message("Brake test with TMC and without PFS : " + H2Y.HexTobyte(Ident[5]).ToString());

                if ("01" == Ident[5])
                {
                   
                    Ret = true;
                }
                else
                {
                    Ret = false;
                }
            }

            return Ret;
        }
        public static bool TMC_Without_PFS_Result()
        {
            bool Ret = true;

            //>04 31 03 30 3D 
            //>12 71 03 30 3D 02 00 03 C6 21 EC 03 C6 21 EC 03 C6 21 EC 

            TMC_X_PFS_Result = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 03 30 3D"); }
            if (Ret)
            {
                TMC_X_PFS_Result = NeoVI.Get_Data;

                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 18) return false;
                NeoVI.Debug_Message("Brake test with TMC and without PFS Result : " + H2Y.HexTobyte(Ident[5]).ToString());

                if ("02" == Ident[5])
                {
                    NeoVI.Debug_Message("TMC_Without_PFS Finish");
                    Ret = true;
                }
                else
                {
                    Ret = false;
                }
            }

            return Ret;
        }

        public static void TMC_With_PFS_Test()
        {
            bool Ret = true;

            for (int cnt = 0; cnt < 3; cnt++)
            {
                Ret = TMC_With_PFS();
                if (Ret) break;
            }

            if (Ret)
            {
                for (int cnt = 0; cnt < 100; cnt++)
                {
                    H2Y.Sleep(300);
                    if (TMC_With_PFS_Result())
                    {
                        break;
                    }
                }
            }
        }
        public static bool TMC_With_PFS()
        {
            bool Ret = true;

            //>0B 31 01 30 3D 03 03 C0 05 00 02 0A   // c005: P1 46bar, 0002: V1 4mm/s, 0a: T1 10s, 00: T2 0s
            //>05 71 01 30 3D 01 

            TMC_O_PFS = "";
            NeoVI.Debug_Message("TMC_With_PFS");
            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 01 30 3D 03 03 C0 05 00 02 0A"); }
            if (Ret)
            {
                TMC_O_PFS = NeoVI.Get_Data;
                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 5) return false;
                NeoVI.Debug_Message("Brake test with TMC and with PFS : " + H2Y.HexTobyte(Ident[5]).ToString());

                if ("01" == Ident[5])
                {
                    Ret = true;
                }
                else
                {
                    Ret = false;
                }
            }

            return Ret;
        }
        public static bool TMC_With_PFS_Result()
        {
            bool Ret = true;

            //>04 31 03 30 3D
            //>12 71 03 30 3D 02 00 03 C2 21 12 03 9E 21 7E 03 8C 21 7E 
            
            TMC_O_PFS_Result = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 03 30 3D"); }
            if (Ret)
            {
                TMC_O_PFS_Result = NeoVI.Get_Data;

                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 18) return false;
                NeoVI.Debug_Message("Brake test with TMC and with PFS Result : " + H2Y.HexTobyte(Ident[5]).ToString());

                if ("02" == Ident[5])
                {
                    NeoVI.Debug_Message("TMC_With_PFS Finish");
                    Ret = true;
                }
                else
                {
                    Ret = false;
                }
            }

            return Ret;
        }

        public static void MasterCylinder_Test()
        {
            bool Ret = true;

            for (int cnt = 0; cnt < 3; cnt++)
            {
                Ret = MasterCylinder();
                if (Ret) break;
            }

            if (Ret)
            {
                for (int cnt = 0; cnt < 20; cnt++)
                {
                    H2Y.Sleep(300);
                    if (MasterCylinder_Result())
                    {
                        break;
                    }
                }
            }
        }
        public static bool MasterCylinder()
        {
            bool Ret = true;

            //>09 31 01 30 3F 03 C0 0A 00 00    // 0a00: P1 0.3125bar, 00: T1 0s, 00: T2 0s, 00: Test Option 0 (SSV = off)
            //>05 71 01 30 3F 01 

            Cylinder = "";

            NeoVI.Debug_Message("MasterCylinder");

            //if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 01 30 3F 01 68 0A 00 00"); }
            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 01 30 3F 03 C0 0A 00 00"); }
            
            if (Ret)
            {
                Cylinder = NeoVI.Get_Data;
                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 5) return false;
                    

                if ("01" == Ident[5])
                {
                    Ret = true;
                }
                else
                {
                    Ret = false;
                }
            }

            return Ret;
        }
        public static bool MasterCylinder_Result()
        {
            bool Ret = true;

            //>04 31 03 30 3F   
            //>0E 71 03 30 3F 02 00 03 DC 04 A7 03 DC 04 A7 

            Cylinder_Result = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 03 30 3F"); }
            if (Ret)
            {
                Cylinder_Result = NeoVI.Get_Data;

                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 14) return false;
                NeoVI.Debug_Message("Onebox Tandem Master Cylinder Test Result : " + H2Y.HexTobyte(Ident[5]).ToString());

                if (Ident[5] == "02" )
                {
                    NeoVI.Debug_Message("Tandem Master Cylinder Test Success!");
                    Ret = true;
                }
                else if (Ident[5] == "05")
                {
                    NeoVI.Debug_Message("Tandem Master Cylinder Test Fail!");
                    Ret = true;
                }
                else
                {
                    Ret = false;
                }
            }

            return Ret;
        }

        public static void SpeedLimitedTest()
        {
            bool Ret = true;

            for (int cnt = 0; cnt < 3; cnt++)
            {
                Ret = SpeedLimitedDeactivation();
                if (Ret) break;
            }

            //if (Ret)
            //{
            //    for (int cnt = 0; cnt < 3; cnt++)
            //    {
            //        H2Y.Sleep(300);
            //        if (SpeedLimitedActivation())
            //        {
            //            break;
            //        }
            //    }
            //}
        }
        public static bool SpeedLimitedDeactivation()
        {
            bool Ret = true;

            //>05 31 01 31 18 00    루틴(ID: 3118) 시작 요청
            //>05 71 01 31 18 01    속도 제한 비활성화 시작됨 응답 (01)

            Deactivation = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 01 31 18 00"); }
            if (Ret)
            {
                Deactivation = NeoVI.Get_Data;
                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 5) return false;
                NeoVI.Debug_Message("Speed limited deactivation : " + H2Y.HexTobyte(Ident[5]).ToString());
            }

            return Ret;
        }
        public static bool SpeedLimitedActivation()
        {
            bool Ret = true;

            //>05 31 01 31 18 01    루틴(ID: 3118) 시작 요청
            //>05 71 01 31 18 01    속도 제한 비활성화 시작됨 응답 (01)

            Cylinder = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("31 01 31 18 01"); }
            if (Ret)
            {
                Cylinder = NeoVI.Get_Data;
                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 5) return false;
                NeoVI.Debug_Message("Speed limited activation : " + H2Y.HexTobyte(Ident[5]).ToString());
            }

            return Ret;
        }

        public static bool Read__DTC()              //5 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("19 02 08");

            if (Ret) { ECUs.DTC_Read = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Clear_DTC()              //6 
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("14 FF FF FF");
            if (Ret) { ECUs.DTCClear = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Check_BLS_Signal()
        {
            bool Ret = true;

            //>03 22 30 0b      브레이크 등 스위치 신호 확인 (DID 300b)
            //>04 62 30 0b 00   Not Actuated 응답 (00)


            string  Pedal = "";
            for (int cnt = 0; cnt < 3; cnt++)
            {
                Ret = NeoVI.Ret_SendMsgs("22 30 0B");
                if (Ret) break;
            }

            if (Ret)
            {
                Pedal = NeoVI.Get_Data;

                string[] Ident = NeoVI.Get_Data.Split(' ');

                if (Ident.Length < 4) return false;
                NeoVI.Debug_Message("Check BLS Signal : " + H2Y.HexTobyte(Ident[4]).ToString());

                if ("00" == H2Y.HexTobyte(Ident[4]).ToString())
                {
                    Ret = true;
                }
                else
                {
                    Ret = false;
                }
            }
            return Ret;
        }
        
        public static bool Start_WSS_Test()               //8 
        {
            bool Ret = true;

            //>06 31 01 31 16 01 F4     루틴(ID: 3116) 시작 요청
            //>05 71 01 31 16 01        성공 응답 (01)

            for (int cnt = 0; cnt < 3; cnt++)
            {
                Ret = NeoVI.Ret_SendMsgs("31 01 31 16 01 F4");
                if (Ret) break;
            }

            return Ret;
        }

        public static bool WSS_Test()               //8 
        {
            bool Ret = true;

            //>04 31 03 31 16 
            //>15 71 03 31 16 02 00 34 00 33 00 69 00 69 9D 00 9D 00 D2 00 D2 00 

            for (int cnt = 0; cnt < 3; cnt++)
            {
                Ret = NeoVI.Ret_SendMsgs("31 03 31 16");
                if (Ret) break;
            }

            string[] Ident = NeoVI.Get_Data.Split(' ');

            if (Ident.Length == 8)
            {
                Ret = false;
            }
            else
            {
                Ret = true;
            }

            if (Ret)
            {
                Ret_WSS_Speed(NeoVI.Get_Data);
                NeoVI.Debug_Message("WSS FL : " + ECUs.WSS_FL);
                NeoVI.Debug_Message("WSS FR : " + ECUs.WSS_FR);
                NeoVI.Debug_Message("WSS RL : " + ECUs.WSS_RL);
                NeoVI.Debug_Message("WSS RR : " + ECUs.WSS_RR);
            }
            else
            {
                NeoVI.Debug_Message("WSS Fail Error code" + Ident[5]);
            }

            return Ret;
        }
        private static void Ret_WSS_Speed(string pData) 
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 21) return;

            //ECUs.WSS_FL = (float)((H2Y.HexToInt(Ident[6], Ident[7]) + H2Y.HexToInt(Ident[8], Ident[9])) / 2 * 0.1);
            //ECUs.WSS_FR = (float)((H2Y.HexToInt(Ident[10], Ident[11]) + H2Y.HexToInt(Ident[12], Ident[13])) / 2 * 0.1);
            //ECUs.WSS_RL = (float)((H2Y.HexToInt(Ident[14], Ident[15]) + H2Y.HexToInt(Ident[16], Ident[17])) / 2 * 0.1);
            //ECUs.WSS_RR = (float)((H2Y.HexToInt(Ident[18], Ident[19]) + H2Y.HexToInt(Ident[20], Ident[21])) / 2 * 0.1);

            int nFL = H2Y.HexToInt(Ident[6], Ident[7]);
            int nFR = H2Y.HexToInt(Ident[10], Ident[11]);
            int nRL = H2Y.HexToInt(Ident[14], Ident[15]);
            int nRR = H2Y.HexToInt(Ident[18], Ident[19]);


            ECUs.WSS_FL = (float)(nFL * 0.055) ;
            ECUs.WSS_FR = (float)(nFR * 0.055) ;
            ECUs.WSS_RL = (float)(nRL * 0.055) ;
            ECUs.WSS_RR = (float)(nRR * 0.055) ;
            Console.WriteLine(pData);
        }

        //연결 유지
        public static bool Tester_Present()         //TesterPresent(3E hex) Service 테스터 존재
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("3E 00");      //(00:Response required, 80:Response not required)
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Message_Falg()           //EnableNormalMessageTransmission (29 hex) service
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("29 01");      //(01:ResponseRequired, 02:NoResponseRequired)
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }

        public static bool Dynamic_Step(int idx)    //Dynamic ABS Test
        {
            bool Ret = true;
            
            switch (idx)
            {
                case 0: Ret = Start_Communication(); break;

                //case 1: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 01 05 40 0A 00 01 F4 00 00"); break; //1. ABS Pressure FL (3C:600, 28:400ms)    
                //case 2: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 01 00 00 00 00 00 00 00 00"); break; //1. ABS Release  FL (3C:600, 28:400ms)    

                //case 3: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 02 05 40 0A 00 01 F4 00 00"); break; //2. ABS Pressure FR (3C:600, 28:400ms)    
                //case 4: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 02 00 00 00 00 00 00 00 00"); break; //2. ABS Release  FR (3C:600, 28:400ms)   

                //case 5: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 03 05 00 0A 00 01 F4 00 00"); break; //3. ABS Pressure RL (3C:600, 28:400ms)    
                //case 6: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 03 00 00 00 00 00 00 00 00"); break; //3. ABS Release  RL (3C:600, 28:400ms)

                //case 7: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 04 05 00 0A 00 01 F4 00 00"); break; //4. ABS Pressure RR (3C:600, 28:400ms)    
                //case 8: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 04 00 00 00 00 00 00 00 00"); break; //4. ABS Release  RR (3C:600, 28:400ms)    


                case 1: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 01 01 40 0A 00 01 F4 00 00"); break; //1. ABS Pressure FL (3C:600, 28:400ms)    
                case 2: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 01 00 00 00 00 00 00 00 00"); break; //1. ABS Release  FL (3C:600, 28:400ms)    

                case 3: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 02 01 40 0A 00 01 F4 00 00"); break; //2. ABS Pressure FR (3C:600, 28:400ms)    
                case 4: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 02 00 00 00 00 00 00 00 00"); break; //2. ABS Release  FR (3C:600, 28:400ms)   

                case 5: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 03 01 40 0A 00 01 F4 00 00"); break; //3. ABS Pressure RL (3C:600, 28:400ms)    
                case 6: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 03 00 00 00 00 00 00 00 00"); break; //3. ABS Release  RL (3C:600, 28:400ms)

                case 7: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 04 01 40 0A 00 01 F4 00 00"); break; //4. ABS Pressure RR (3C:600, 28:400ms)    
                case 8: Ret = NeoVI.Ret_SendMsgs("31 01 30 3C 04 00 00 00 00 00 00 00 00"); break; //4. ABS Release  RR (3C:600, 28:400ms)    

            }
            //
            return Ret;
        }
        public static bool Dynamic_Auto()           //Dynamic Test
        {
            bool Ret = true;

            float T2 = 400;
            float T3 = 2000;

            double Vlv_Time = 0;
            double Old_Time = 0;
            double Off_Time = DateTime.Now.Ticks;
            bool ECU_Flag = true;
            byte ECU_Setp = 0;

            while (true)
            {
                if (!ECU_Flag) { ECU_Flag = true; }

                Vlv_Time = TSet.ABSv_Time + ((DateTime.Now.Ticks - Off_Time) / H2Y.tick_Dvd);

                if (ECU_Flag && ECU_Setp == 0)
                {
                    Ret = Start_Communication();
                    if (Ret)
                    {
                        //Ret = SecurityAccess();
                        if (Ret)
                        {
                            ECU_Setp = 1; ECU_Flag = false; Old_Time = Vlv_Time;
                        }
                    }
                }

                if (ECU_Flag && ECU_Setp == 1)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 2; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 2)
                {
                    Ret = Dynamic_Step(1); ECU_Setp = 3; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 3)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 4; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 4)
                {
                    Ret = Dynamic_Step(2); ECU_Setp = 5; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 5)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 6; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 6)
                {
                    Ret = Dynamic_Step(3); ECU_Setp = 7; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 7)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 8; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 8)
                {
                    Ret = Dynamic_Step(4); ECU_Setp = 9; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 9)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 10; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 10)
                {
                    Ret = Dynamic_Step(5); ECU_Setp = 11; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 11)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 12; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 12)
                {
                    Ret = Dynamic_Step(6); ECU_Setp = 13; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 13)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 14; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 14)
                {
                    Ret = Dynamic_Step(7); ECU_Setp = 15; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 15)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 16; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 16)
                {
                    Ret = Dynamic_Step(8); ECU_Setp = 17; ECU_Flag = false; Old_Time = Vlv_Time;
                }

                if (ECU_Flag && ECU_Setp == 17)
                {
                    if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 18; ECU_Flag = false; Old_Time = Vlv_Time; }
                }

                if (ECU_Flag && ECU_Setp == 18)
                {
                    ECUs.ABS_Step = 5;
                    break;
                }
            }

            return Ret;
        }
        public static bool ESP_Step(int idx)        //Dynamic ESP Test
        {
            bool Ret = true;

            switch (idx)
            {
                case 1: Ret = true; break;
                case 2: Ret = true; break;
            }

            return Ret;
        }

        public static bool ESS_LampTest()           //10 ESS Lamp(Option) 
        {
            return true;
        }

        public static bool Write_EOLProcessByte()
        {
            bool Ret = true;

            //>04 2E 30 02 01   EOL 공정 제어 바이트 쓰기 완료 and OK 쓰기 (01)
            //>03 6E 30 02      EOL 쓰기 응답           

            ECUs.ProcessB = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("2E 30 02 01"); }
            if (Ret) { ECUs.ProcessB = Ret_ProcessByte(NeoVI.Get_Data); }

            //if (Ret)
            //{
            //    Cylinder = NeoVI.Get_Data;
            //    string[] Ident = NeoVI.Get_Data.Split(' ');

            //    if (Ident.Length < 5) return false;
            //    NeoVI.Debug_Message("Write EOL Process Byte : " + H2Y.HexTobyte(Ident[5]).ToString());
            //}

            return Ret;
        }

      

        #endregion

        //public static string ret_DTCs(string pCode)
        //{
        //    string ret_Msgs = "";

        //    switch (pCode)
        //    {
        //        case "C110101": ret_Msgs = "Battery voltage high"; break;
        //        case "C110201": ret_Msgs = "Battery Voltage Low"; break;
        //        case "C111301": ret_Msgs = "5V Sensor Power Fail"; break;
        //    }
        //    return ret_Msgs;
        //}
        //public static string ret_Errs(string pErr)
        //{
        //    string ret_Msgs = "";

        //    switch (pErr)
        //    {
        //        case "10": ret_Msgs = "GeneralReject"; break;
        //        case "12": ret_Msgs = "SubFunctionNotSupported-invalidFormat"; break;
        //        case "13": ret_Msgs = "incorrectMessageLengthOrInvalidFormat"; break;
        //        case "22": ret_Msgs = "ConditionsNotCorrected"; break;
        //        case "24": ret_Msgs = "requestSequenceError"; break;
        //        case "31": ret_Msgs = "requestOutOfRange"; break;
        //        case "35": ret_Msgs = "invalidKey"; break;
        //        case "36": ret_Msgs = "exceededNumberOfAttempts"; break;
        //        case "37": ret_Msgs = "requiredTimeDelayNotExpired"; break;
        //        case "78": ret_Msgs = "requestCorrectlyReceived-ResponsePending"; break;
        //        case "7F": ret_Msgs = "serviceNotSupportedInActiveSession"; break;
        //    }

        //    return ret_Msgs;
        //}
    }

    public static class MOBIS_LX3H      //LX3 HEV iMEB2  HYUNDAI MOBIS (250710)
    {
        #region Variable declaration
        public static string ECU_ID;
        public static string Pedal;
        #endregion

        #region Standard CAN
        public static bool Start_Communication()    //1. Extended Diagnostic Session
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("10 03");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Stop_Communication()     //2. Default Session
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("10 01");
            if (Ret) { ECUs.End_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Reset()              //3.
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("11 01");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Identification()     //4.
        {
            bool Ret = true;

            ECU_ID = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 00"); }
            if (Ret)
            {
                ECU_ID = NeoVI.Get_Data;
                NeoVI.Debug_Message("ECU Identification (F100) : " + ECU_ID);
            }

            return Ret;
        }
        public static bool Read__DTC()              //5
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("19 02 FF");
            if (Ret) { ECUs.DTC_Read = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Clear_DTC()              //6
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("14 FF FF FF");
            if (Ret) { ECUs.DTCClear = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Check_Signals()          //7
        {
            bool Ret = true;

            Pedal = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 01 04"); }
            if (Ret)
            {
                Pedal = NeoVI.Get_Data;
                ECUs.SigPedal = Ret_OnOff(Pedal);
                NeoVI.Debug_Message("Brake Pedal Position : " + ECUs.SigPedal);
            }

            return Ret;
        }
        public static string Ret_OnOff(string pData)
        {
            string[] Hex = pData.Split(' ');
            if (Hex.Length < 42) { return "Err"; }

            string strValue = H2Y.HexToBinary(Hex[27], 3, 2);  //DID0104h byte27 bit3,2 (Normal close)
            string RetValue = "None";

            NeoVI.Debug_Message("PID0x10.BrakePedalSwitch bit3,2 (00=Off, 01=On) : " + strValue);

            switch (strValue)
            {
                case "00": RetValue = "OFF"; break;
                case "01": RetValue = "ON"; break;
            }

            return RetValue;
        }
        public static bool WSS_Test()               //8
        {
            bool Ret = true;

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 01 04"); }
            if (Ret)
            {
                Ret_WSS_Speed(NeoVI.Get_Data);

                NeoVI.Debug_Message("WSS FL : " + ECUs.WSS_FL);
                NeoVI.Debug_Message("WSS FR : " + ECUs.WSS_FR);
                NeoVI.Debug_Message("WSS RL : " + ECUs.WSS_RL);
                NeoVI.Debug_Message("WSS RR : " + ECUs.WSS_RR);
            }

            return Ret;
        }
        private static void Ret_WSS_Speed(string pData)
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 42) return;

            ECUs.WSS_FL = H2Y.HexTobyte(Ident[15]);
            ECUs.WSS_FR = H2Y.HexTobyte(Ident[16]);
            ECUs.WSS_RL = H2Y.HexTobyte(Ident[17]);
            ECUs.WSS_RR = H2Y.HexTobyte(Ident[18]);
        }
        public static bool Message_Falg()           //CommunicationControl (0x28)
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("28 00 01");  //EnableRxAndTx, CommunicationType=01h
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }

        public static bool Tester_Present()         //TesterPresent
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("3E 00");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }

        public static bool Dynamic_Step(int idx)    //Dynamic ABS Test (HEV)
        {
            bool Ret = true;

            switch (idx)
            {
                case 0:  Ret = Start_Communication(); break;
                case 1:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 3C 11 00 00"); break; //1. FR+RL Valves(3C), PSV+MCV#5(11)
                case 2:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 3C 22 00 00"); break; //2. FR+RL Valves(3C), MCV#2+MCV#6(22)
                case 3:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 3C 44 00 01"); break; //3. FR+RL Valves(3C), WSV+LSV(44)
                case 4:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 3C 88 00 01"); break; //4. FR+RL Valves(3C), RCV(88)
                case 5:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 C3 11 00 00"); break; //5. FL+RR Valves(C3), PSV+MCV#5(11)
                case 6:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 C3 22 00 00"); break; //6. FL+RR Valves(C3), MCV#2+MCV#6(22)
                case 7:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 C3 44 00 01"); break; //7. FL+RR Valves(C3), WSV+LSV(44)
                case 8:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 C3 88 00 01"); break; //8. FL+RR Valves(C3), RCV(88)
                case 9:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 00 01 00 00"); break; //9. HEV Pump on (PSV)
                case 10: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 00 00 00 00"); break; //10. Stop
            }

            return Ret;
        }
        public static bool Dynamic_Auto()           //Dynamic Test (HEV)
        {
            bool Ret = true;

            float T2 = 400;
            float T3 = 2000;

            double Vlv_Time = 0;
            double Old_Time = 0;
            double Off_Time = DateTime.Now.Ticks;
            bool ECU_Flag = true;
            byte ECU_Setp = 0;

            while (true)
            {
                if (!ECU_Flag) { ECU_Flag = true; }

                Vlv_Time = TSet.ABSv_Time + ((DateTime.Now.Ticks - Off_Time) / H2Y.tick_Dvd);

                if (ECU_Flag && ECU_Setp == 0)  { Ret = Start_Communication(); ECU_Setp = 1; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 1)  { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 2; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 2)  { Ret = Dynamic_Step(1); ECU_Setp = 3; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 3)  { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 4; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 4)  { Ret = Dynamic_Step(2); ECU_Setp = 5; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 5)  { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 6; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 6)  { Ret = Dynamic_Step(3); ECU_Setp = 7; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 7)  { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 8; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 8)  { Ret = Dynamic_Step(4); ECU_Setp = 9; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 9)  { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 10; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 10) { Ret = Dynamic_Step(5); ECU_Setp = 11; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 11) { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 12; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 12) { Ret = Dynamic_Step(6); ECU_Setp = 13; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 13) { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 14; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 14) { Ret = Dynamic_Step(7); ECU_Setp = 15; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 15) { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 16; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 16) { Ret = Dynamic_Step(8); ECU_Setp = 17; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 17) { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 18; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 18) { Ret = Dynamic_Step(9); ECU_Setp = 19; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 19) { if (Vlv_Time - Old_Time > H2Y.DVD(T3, 1000)) { ECU_Setp = 20; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 20) { ECUs.ABS_Step = 5; break; }
            }

            return Ret;
        }
        public static bool ESP_Step(int idx)        //Dynamic ESP Test
        {
            bool Ret = true;

            switch (idx)
            {
                case 1: Ret = true; break;
                case 2: Ret = true; break;
            }

            return Ret;
        }

        public static bool ESS_LampTest()           //10 ESS Lamp (F024)
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("2F F0 24 03");
            if (Ret) { ECUs.ESS_Lamp = NeoVI.Get_Data; }

            return Ret;
        }
        #endregion
    }

    public static class MOBIS_LX3I      //LX3 ICE MEB5_1  HYUNDAI MOBIS (250710)
    {
        #region Variable declaration
        public static string ECU_ID;
        public static string Pedal;
        #endregion

        #region Standard CAN
        public static bool Start_Communication()    //1. Extended Diagnostic Session
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("10 03");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Stop_Communication()     //2. Default Session
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("10 01");
            if (Ret) { ECUs.End_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Reset()              //3.
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("11 01");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool ECU_Identification()     //4.
        {
            bool Ret = true;

            ECU_ID = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 F1 00"); }
            if (Ret)
            {
                ECU_ID = NeoVI.Get_Data;
                NeoVI.Debug_Message("ECU Identification (F100) : " + ECU_ID);
            }

            return Ret;
        }
        public static bool Read__DTC()              //5
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("19 02 FF");
            if (Ret) { ECUs.DTC_Read = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Clear_DTC()              //6
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("14 FF FF FF");
            if (Ret) { ECUs.DTCClear = NeoVI.Get_Data; }

            return Ret;
        }
        public static bool Check_Signals()          //7
        {
            bool Ret = true;

            Pedal = "";

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 01 04"); }
            if (Ret)
            {
                Pedal = NeoVI.Get_Data;
                ECUs.SigPedal = Ret_OnOff(Pedal);
                NeoVI.Debug_Message("Brake Pedal Position : " + ECUs.SigPedal);
            }

            return Ret;
        }
        public static string Ret_OnOff(string pData)
        {
            string[] Hex = pData.Split(' ');
            if (Hex.Length < 42) { return "Err"; }

            string strValue = H2Y.HexToBinary(Hex[24], 3, 2);  //DID0104h byte24 bit3,2 (Normal open)
            string RetValue = "None";

            NeoVI.Debug_Message("PID0x10.BrakePedalSwitch bit3,2 (00=Off, 01=On) : " + strValue);

            switch (strValue)
            {
                case "00": RetValue = "OFF"; break;
                case "01": RetValue = "ON"; break;
            }

            return RetValue;
        }
        public static bool WSS_Test()               //8
        {
            bool Ret = true;

            if (Ret) { Ret = NeoVI.Ret_SendMsgs("22 01 04"); }
            if (Ret)
            {
                Ret_WSS_Speed(NeoVI.Get_Data);

                NeoVI.Debug_Message("WSS FL : " + ECUs.WSS_FL);
                NeoVI.Debug_Message("WSS FR : " + ECUs.WSS_FR);
                NeoVI.Debug_Message("WSS RL : " + ECUs.WSS_RL);
                NeoVI.Debug_Message("WSS RR : " + ECUs.WSS_RR);
            }

            return Ret;
        }
        private static void Ret_WSS_Speed(string pData)
        {
            string[] Ident = pData.Split(' ');

            if (Ident.Length < 42) return;

            ECUs.WSS_FL = H2Y.HexTobyte(Ident[15]);
            ECUs.WSS_FR = H2Y.HexTobyte(Ident[16]);
            ECUs.WSS_RL = H2Y.HexTobyte(Ident[17]);
            ECUs.WSS_RR = H2Y.HexTobyte(Ident[18]);
        }
        public static bool Message_Falg()           //CommunicationControl (0x28)
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("28 00 01");  //EnableRxAndTx, CommunicationType=01h
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }

        public static bool Tester_Present()         //TesterPresent
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("3E 00");
            if (Ret) { ECUs.Stt_Comm = NeoVI.Get_Data; }

            return Ret;
        }

        public static bool Dynamic_Step(int idx)    //Dynamic ABS Test (ICE)
        {
            bool Ret = true;

            switch (idx)
            {
                case 0:  Ret = Start_Communication(); break;
                case 1:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 3C 81 1E"); break; //1. FR+RL Valves(3C), TCV1+Motor(81), 600ms
                case 2:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 3C 82 1E"); break; //2. FR+RL Valves(3C), TCV2+Motor(82), 600ms
                case 3:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 3C 84 1E"); break; //3. FR+RL Valves(3C), HSV1+Motor(84), 600ms
                case 4:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 3C 88 1E"); break; //4. FR+RL Valves(3C), HSV2+Motor(88), 600ms
                case 5:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 C3 81 1E"); break; //5. FL+RR Valves(C3), TCV1+Motor(81), 600ms
                case 6:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 C3 82 1E"); break; //6. FL+RR Valves(C3), TCV2+Motor(82), 600ms
                case 7:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 C3 84 1E"); break; //7. FL+RR Valves(C3), HSV1+Motor(84), 600ms
                case 8:  Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 C3 88 1E"); break; //8. FL+RR Valves(C3), HSV2+Motor(88), 600ms
                case 9:  Ret = NeoVI.Ret_SendMsgs("2F F0 11 03");           break; //9. ICE Pump on (F011)
                case 10: Ret = NeoVI.Ret_SendMsgs("2F F0 1E 03 00 00 00 00"); break; //10. Stop
            }

            return Ret;
        }
        public static bool Dynamic_Auto()           //Dynamic Test (ICE)
        {
            bool Ret = true;

            float T2 = 400;
            float T3 = 2000;

            double Vlv_Time = 0;
            double Old_Time = 0;
            double Off_Time = DateTime.Now.Ticks;
            bool ECU_Flag = true;
            byte ECU_Setp = 0;

            while (true)
            {
                if (!ECU_Flag) { ECU_Flag = true; }

                Vlv_Time = TSet.ABSv_Time + ((DateTime.Now.Ticks - Off_Time) / H2Y.tick_Dvd);

                if (ECU_Flag && ECU_Setp == 0)  { Ret = Start_Communication(); ECU_Setp = 1; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 1)  { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 2; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 2)  { Ret = Dynamic_Step(1); ECU_Setp = 3; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 3)  { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 4; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 4)  { Ret = Dynamic_Step(2); ECU_Setp = 5; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 5)  { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 6; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 6)  { Ret = Dynamic_Step(3); ECU_Setp = 7; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 7)  { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 8; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 8)  { Ret = Dynamic_Step(4); ECU_Setp = 9; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 9)  { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 10; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 10) { Ret = Dynamic_Step(5); ECU_Setp = 11; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 11) { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 12; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 12) { Ret = Dynamic_Step(6); ECU_Setp = 13; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 13) { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 14; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 14) { Ret = Dynamic_Step(7); ECU_Setp = 15; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 15) { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 16; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 16) { Ret = Dynamic_Step(8); ECU_Setp = 17; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 17) { if (Vlv_Time - Old_Time > H2Y.DVD(T2, 1000)) { ECU_Setp = 18; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 18) { Ret = Dynamic_Step(9); ECU_Setp = 19; ECU_Flag = false; Old_Time = Vlv_Time; }
                if (ECU_Flag && ECU_Setp == 19) { if (Vlv_Time - Old_Time > H2Y.DVD(T3, 1000)) { ECU_Setp = 20; ECU_Flag = false; Old_Time = Vlv_Time; } }
                if (ECU_Flag && ECU_Setp == 20) { ECUs.ABS_Step = 5; break; }
            }

            return Ret;
        }
        public static bool ESP_Step(int idx)        //Dynamic ESP Test
        {
            bool Ret = true;

            switch (idx)
            {
                case 1: Ret = true; break;
                case 2: Ret = true; break;
            }

            return Ret;
        }

        public static bool ESS_LampTest()           //10 ESS Lamp (F024)
        {
            bool Ret = true;

            Ret = NeoVI.Ret_SendMsgs("2F F0 24 03");
            if (Ret) { ECUs.ESS_Lamp = NeoVI.Get_Data; }

            return Ret;
        }
        #endregion
    }
}
