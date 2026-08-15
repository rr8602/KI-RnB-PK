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
    public partial class fom_Keys : Form
    {
        public string RetValue;

        public fom_Keys()
        {
            InitializeComponent();

            if (PSet.OwnerS00 > PSet.Def_Lang)
            {
                this.Text = PSet.Lang_Key[0];       //입력 장치
                btnClose.Text = PSet.Lang_Key[1];   //닫기
                btnApend.Text = PSet.Lang_Key[2];   //적용
            }
        }

        public fom_Keys(int Style):this()
        {
            switch (Style)
            {
                case 0: this.Width = 310; break;    //입력기
                case 1: this.Width = 236; break;    //계산기
            }
        }


        private void fom_Keys_Load(object sender, EventArgs e)
        {
            txtValue.Text = RetValue;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApend_Click(object sender, EventArgs e)
        {
            RetValue = txtValue.Text;
            this.Close();
        }

        private void btn_Keys_Click(object sender, EventArgs e)
        {
            string keys = "";

            switch (((Button)sender).Name)
            {
                case "btn_0": keys = "0"; break;
                case "btn_1": keys = "1"; break;
                case "btn_2": keys = "2"; break;
                case "btn_3": keys = "3"; break;
                case "btn_4": keys = "4"; break;
                case "btn_5": keys = "5"; break;
                case "btn_6": keys = "6"; break;
                case "btn_7": keys = "7"; break;
                case "btn_8": keys = "8"; break;
                case "btn_9": keys = "9"; break;
                case "btn_A": keys = "."; break;
            }

            txtValue.Text += keys;
        }

        private void btn1Keys_Click(object sender, EventArgs e)
        {
            string keys = txtValue.Text;

            if (keys.Length == 0) return;

            try
            {
                switch (((Button)sender).Name)
                {
                    case "btn_B": break;    //(+)
                    case "btn_C": break;    //(-)
                    case "btn_D": break;    //(*)
                    case "btn_E": break;    //(/)
                    case "btn_F": break;    //(=)
                    case "btn_G": if (keys.Length > 0) keys = keys.Substring(0, keys.Length - 1); break;  //(<-)
                    case "btn_H": keys = ""; break;  //(cls)
                }

                txtValue.Text = keys;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
