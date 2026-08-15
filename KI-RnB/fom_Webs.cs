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
    public partial class fom_Webs : Form
    {
        fom_Main main;

        public fom_Webs()
        {
            InitializeComponent();
        }
        public fom_Webs(fom_Main main)
            : this()
        {
            this.main = main;
        }

        private void fom_Webs_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new System.Uri("http://939.co.kr/kimc", System.UriKind.Absolute);

            this.Text = "Remote control";
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
