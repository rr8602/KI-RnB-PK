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
    public partial class fomFlash : Form
    {
        public bool IsOpen;
        private int old_List = -1;

        public fomFlash()
        {
            InitializeComponent();
        }

        private void fomFlash_Load(object sender, EventArgs e)
        {
            this.Top = PSet.siz__Sub.Top;
            this.Left = PSet.siz__Sub.Left;
            this.Width = 1024;
            this.Height = 768;

            btnModel.Visible = false; btnModel.Text = "";
            btnVinNo.Visible = false; btnVinNo.Text = ""; 

            axWindowsMediaPlayer1.Dock = DockStyle.Fill;
            axWindowsMediaPlayer1.uiMode = "none";
            picImage.Dock = DockStyle.Fill;

            IsOpen = true;
        }
        private void fomFlash_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsOpen = false;
        }

        public void Image1(Image img)
        {
            picImage.Image = img;
            //picImage.ImageLocation = "pic.jpg";
        }

        public bool Play(int pList)
        {
            VinNo_Hide();

            if (old_List == pList) { return false; }
            old_List = pList;
            string str_Play = "";

            if (pList == 100)
            {
                this.Visible = false;
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            }
            else if (pList == 99)
            {
                axWindowsMediaPlayer1.Visible = false;
                picImage.Visible = true;
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            }
            else
            {
                this.Visible = true;
                axWindowsMediaPlayer1.Visible = true;
                picImage.Visible = false;

                string file = PSet.FlashOnf == 0 ? ".mp4" : ".jpg";

                switch (pList)
                {
                    case 0: str_Play = Application.StartupPath + @"\Flash\00.Barcode" + file; break;
                    case 1: str_Play = Application.StartupPath + @"\Flash\01.Enter" + file; break;
                    case 2: str_Play = Application.StartupPath + @"\Flash\02.Safe-On" + file; break;
                    case 3: str_Play = Application.StartupPath + @"\Flash\03.Connect" + file; break;
                    case 4: str_Play = Application.StartupPath + @"\Flash\04.Testing" + file; break;
                    case 5: str_Play = Application.StartupPath + @"\Flash\05.Disconnect" + file; break;
                    case 6: str_Play = Application.StartupPath + @"\Flash\06.Safe-Off" + file; break;
                    case 7: str_Play = Application.StartupPath + @"\Flash\07.GoOut" + file; break;
                    case 8: str_Play = Application.StartupPath + @"\Flash\10.Main Start" + file; break;
                }

                if (PSet.FlashOnf == 0)
                {
                    axWindowsMediaPlayer1.stretchToFit = true;
                    axWindowsMediaPlayer1.settings.setMode("Loop", true);
                    axWindowsMediaPlayer1.URL = str_Play;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                else
                {
                    axWindowsMediaPlayer1.Visible = false;
                    picImage.Visible = true;
                    picImage.ImageLocation = str_Play;

                    //picImage.BackgroundImageLayout = ImageLayout.None;
                    //Message_Show(picImage, "Barcode scan", Color.Red);
                }
            }
            
            return true;
        }

        public void Message_Show(System.Windows.Forms.PictureBox pic, string msg, Color color)
        {
            try
            {
                Bitmap bmp = new Bitmap(pic.Width, pic.Height);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    RectangleF drawRect;
                    drawRect = new RectangleF(12f, 10f, 1000f, 95F);
                    H2Y.Draw__String(g, 70, msg, color, drawRect, StringAlignment.Center);

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

        public void VinNo_Show(string CarModel, string VinNo)
        {
            btnModel.Visible = true; btnModel.Text = CarModel;
            btnVinNo.Visible = true; btnVinNo.Text = VinNo;

            if (CarModel == "ERROR")
            {
                btnModel.ForeColor = Color.Red;
                btnVinNo.ForeColor = Color.Red;

                H2Y.Sleep(2000);
                VinNo_Hide();
            }
        }

        public void VinNo_Hide()
        {
            btnModel.Visible = false; btnModel.Text = ""; btnModel.ForeColor = Color.Black;
            btnVinNo.Visible = false; btnVinNo.Text = ""; btnVinNo.ForeColor = Color.Black;
        }
    }
}
