using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KI_RnB
{
    public partial class fom_Stop : Form
    {
        fom_Main main;

        public fom_Stop()
        {
            InitializeComponent();

            if (PSet.OwnerS00 > PSet.Def_Lang)
            {
                lblTitle.Text = PSet.LangStop[0]; //정지
                lbl_PLCs.Text = PSet.LangStop[1]; //PLC
                lblReady.Text = PSet.LangStop[2]; //운전 준비
                lblWBMin.Text = PSet.LangStop[3]; //최소
                lblWBase.Text = PSet.LangStop[4]; //휠베이스
                lblWBMax.Text = PSet.LangStop[5]; //최대
                lblLPost.Text = PSet.LangStop[6]; //안전 포스트
                lblRPost.Text = PSet.LangStop[7]; //안전 포스트
                lblFLSRF.Text = PSet.LangStop[8]; //안전 롤러
                lblFRSRF.Text = PSet.LangStop[9]; //안전 롤러
                lblFLMot.Text = PSet.LangStop[10]; //모터
                lblFRMot.Text = PSet.LangStop[11]; //모터
                lblFLSRR.Text = PSet.LangStop[12]; //안전 롤러
                lblFRSRR.Text = PSet.LangStop[13]; //안전 롤러
                lblFSens.Text = PSet.LangStop[14]; //광전 센서
                lblRLSR_.Text = PSet.LangStop[15]; //안전 롤러
                lblRRSR_.Text = PSet.LangStop[16]; //안전 롤러
                lblRLMot.Text = PSet.LangStop[17]; //모터
                lblRRMot.Text = PSet.LangStop[18]; //모터
                lblRSens.Text = PSet.LangStop[19]; //광전 센서
                lbl_EMGs.Text = PSet.LangStop[20]; //비상 정지 스위치
            }
        }
        public fom_Stop(fom_Main main)
            : this()
        {
            this.main = main;
        }

        private void fom_Stop_Load(object sender, EventArgs e)
        {
            this.Top = PSet.siz__Sub.Top;
            this.Left = PSet.siz__Sub.Left;
            this.Width = 1024;
            this.Height = 768;

            picEnter.Location = new Point(12, 95); 
            picEnter.Size = new Size(1000, 664);
            picEnter.Visible = false;

            PSet.Onf_Stop = true;
            tmr_Stop.Enabled = true;
        }

        private void tmr_Stop_Tick(object sender, EventArgs e)
        {

            //return;

            tmr_Stop.Enabled = false;

            long Gap_Time = DateTime.Now.AddSeconds(0.1).Ticks;
            int Err_flag = 1;

            do
            {
                if (PSet.OnfOwner) { break; }
                if (PSet.OnfSetup) { break; }
                if (PSet.Onf_PsWd) { break; }
                if (!PSet.Onf_Prog) { break; }

                if ((DateTime.Now.Ticks - Gap_Time) > 0)
                {
                    this.BringToFront();

                    Gap_Time = DateTime.Now.AddSeconds(0.1).Ticks;

                    Err_flag = 0;

                    if (!PLC.DO.MD_FirstH)
                    {
                        lblWBase.Text = "Homeposition";
                        Err_flag += Onf_ErrorMsg(lblWBase, !PLC.DO.MD_FirstH);   //WB Homeposition
                    }
                    else
                    {
                        if (PLC.DI.WB_L__Min || PLC.DI.WB_L__Max)
                        {
                            lblWBase.Text = PSet.LangStop[4]; //휠베이스
                            Err_flag += Onf_ErrorMsg(lblWBMin, PLC.DI.WB_L__Min);      //WB Min.
                            Err_flag += Onf_ErrorMsg(lblWBMax, PLC.DI.WB_L__Max);      //WB Max.
                        }
                    }

                    Err_flag += Onf_ErrorMsg(lblLPost, !PLC.DI.L_Post_Dn);     //L Post
                    Err_flag += Onf_ErrorMsg(lblRPost, !PLC.DI.R_Post_Dn);     //R Post

                    Err_flag += Onf_ErrorMsg(lblFLSRF, !PLC.DI.FLF_SR_Dn);     //FL_Front Safty Roll
                    Err_flag += Onf_ErrorMsg(lblFRSRF, !PLC.DI.FRF_SR_Dn);     //FR_Front Safty Roll
                    Err_flag += Onf_ErrorMsg(lblFLMot, PLC.DI.FL_MotErr);      //FL   Motor
                    Err_flag += Onf_ErrorMsg(lblFRMot, PLC.DI.FR_MotErr);      //FR   Motor
                    Err_flag += Onf_ErrorMsg(lblFLSRR, !PLC.DI.FLR_SR_Dn);     //FL_Rear Safty Roll
                    Err_flag += Onf_ErrorMsg(lblFRSRR, !PLC.DI.FRR_SR_Dn);     //FR_Rear Safty Roll

                    Err_flag += Onf_ErrorMsg(lblRLSR_, !PLC.DI.RLR_SR_Dn);     //RL       Safty Roll
                    Err_flag += Onf_ErrorMsg(lblRRSR_, !PLC.DI.RRR_SR_Dn);     //RR       Safty Roll
                    Err_flag += Onf_ErrorMsg(lblRLMot, PLC.DI.RL_MotErr);      //RL   Motor
                    Err_flag += Onf_ErrorMsg(lblRRMot, PLC.DI.RR_MotErr);      //RR   Motor

                    Err_flag += Onf_ErrorMsg(lblFSens, PLC.DI.PHO_Front);      //Front Photo Sensor
                    Err_flag += Onf_ErrorMsg(lblRSens, PLC.DI.PHO__Rear);      //Rear  Photo Sensor

                    Err_flag += Onf_ErrorMsg(lbl_Flap, PLC.DI.R_Flap_Up);      //Exhaust gas flap
                    Err_flag += Onf_ErrorMsg(lbl_EMGs, PLC.DO.MD_Emerge);      //EMERGENCY

                    Err_flag += Onf_ErrorMsg(lblReady, !PLC.DO.MD__Ready);     //Ready Hold PLC

                    //if (!PLC.DO.m_Redy)
                    //{
                    //    lblReady.Text = PSet.LangStop[2];
                    //    Err_flag += Onf_ErrorMsg(lblReady, !PLC.DO.m_Redy);           //Ready Hold PLC
                    //}
                    //else
                    //{
                    //    lblReady.Text = "Remote";
                    //    Err_flag += Onf_ErrorMsg(lblReady, !PLC.DI.Rmt_ST);           //Ready Remote Controller
                    //}

                    Err_flag += Onf_ErrorMsg(lblTitle, !PLC.DO.MD___Auto);              //Auto PLC
                    Err_flag += Onf_ErrorMsg(lbl_PLCs, !PLC.IsRedy);                    //Stop PLC
                    Err_flag += Onf_ErrorMsg(lbl_kimc, main.ABSBoard.BHerz == 0);       //ABS Board Error

                    if (!PLC.DO.MD___Auto)
                    {
                        picEnter.Visible = false;
                        if (PLC.DO.MD_Manual) { lblTitle.Text = PSet.LangStop[21]; }   //"Manual mode"
                        if (PLC.DI.PBCalMode) { lblTitle.Text = PSet.LangStop[22]; }   //"Calibration mode"
                    }
                    {
                        if (!PLC.DO.Pos_Enter)
                        {
                            if (Err_flag == 0) { picEnter.Visible = true; }
                            lblTitle.Text = "Enter Position";
                            Err_flag += Onf_ErrorMsg(lblTitle, !PLC.DO.Pos_Enter);   //Enter Position
                        }
                        else 
                        { 
                            picEnter.Visible = false; 
                        }
                    }
                }

                Application.DoEvents(); 

            } while (Err_flag > 0);

            PSet.Onf_Stop = false;
            this.Close();
        }

        private int Onf_ErrorMsg(Label lbl, bool Onf)
        {
            if (Onf)
            {
                lbl.BackColor = Color.Black;
                lbl.ForeColor = Color.Red;
                return 1;
            }
            else
            {
                lbl.BackColor = Color.LightGray;
                lbl.ForeColor = Color.Black;
                return 0;
            }
        }
    }
}
