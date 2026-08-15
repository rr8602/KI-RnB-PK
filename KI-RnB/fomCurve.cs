using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace KI_RnB
{
    public partial class fomCurve : Form
    {
        public bool IsOpen;

        fom_Main main = null;

        byte crv_Mode;
        string crv_Name;
        clsCurve crv_Data = new clsCurve();
        Bitmap bmp;

        string str_sort = "";

        public fomCurve()
        {
            InitializeComponent();

            if (PSet.OwnerS00 > PSet.Def_Lang)
            {
                this.Text = PSet.Lang_Crv[0]; //주행 곡선 데이터
                btnI_Add.Text = PSet.Lang_Crv[1]; //추가
                btnIEdit.Text = PSet.Lang_Crv[2]; //변경
                btnI_Del.Text = PSet.Lang_Crv[3]; //삭제
                lbl___00.Text = PSet.Lang_Crv[4]; //구분
                lbl___01.Text = PSet.Lang_Crv[5]; //시간
                lbl___03.Text = PSet.Lang_Crv[7]; //차량
                lbl___04.Text = PSet.Lang_Crv[8]; //롤
                lbl___05.Text = PSet.Lang_Crv[9]; //속도
                lbl___06.Text = PSet.Lang_Crv[10]; //항목
                lbl___07.Text = PSet.Lang_Crv[11]; //설명
                btnIInit.Text = PSet.Lang_Crv[12]; //초기화
                btn_Save.Text = PSet.Lang_Crv[13]; //저장
                btnClose.Text = PSet.Lang_Crv[14]; //닫기
            }

            picGraph.Image = Init_Graph();
        }

        public fomCurve(fom_Main main, string pCurve, int Lock)  : this()
        {
            this.main = main;
            chk_Item.Visible = Lock == 1 ? true : false;

            if (PSet.OwnerCrv == 2)
            {
                lst_Item.Top = 87;
                lst_Item.Left = 12;
                lst_Item.Width = 205;
                lst_Item.Height = 308;

                cboMDB02.Items.Clear();
                cboMDB02.Items.Add("All items");
                cboMDB02.Items.Add("Basic items");
                cboMDB02.Items.Add("ECU items");
                cboMDB02.Text = "All items";
                str_sort = "All items";

                dgvCurve.Top = 87;
                dgvCurve.Left = 223;
                dgvCurve.Width = 781;
                dgvCurve.Height = 304;
            }
            else
            {
                dgvCurve.Top = 87;
                dgvCurve.Left = 12;
                dgvCurve.Width = 992;
                dgvCurve.Height = 304;
                dgvCurve.BringToFront();
            }

            if (Lock == 1)
            {
                MDB_ItemList();
            }
            
            crv_Name = pCurve;

            if (pCurve != "")
            {
                if (crv_Data.Get_DriveCurve(pCurve))
                {
                    crv_Mode = 0;   //수정
                    Read_Graph(pCurve);
                }
                else
                {
                    crv_Mode = 1;   //새로 만들기
                }
            }
        }

        private void fomCurve_Load(object sender, EventArgs e)
        {
            IsOpen = true;
        }
        private void fomCurve_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsOpen = false;
        }

        #region Drive Curve
        private void Read_Graph(string pCurve)
        {
            if (crv_Data.Get_DriveCurve(pCurve))
            {
                dgvCurve.DataSource = crv_Data.G_Data;
                dgvCurve.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvCurve.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvCurve.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                dgvCurve.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

                picGraph.Image = Data_Graph(-1);
                this.Text = PSet.Lang_Crv[0] +" [" + pCurve + "]";
            }
        }
        private Bitmap Init_Graph()
        {
            bmp = new Bitmap(picGraph.Width, picGraph.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float gap_l = 40;
                float gap_r = 20;
                float gap_t = 30;
                float gap_b = 40;
                float bmp_w = (bmp.Width - (gap_l + gap_r));
                float bmp_h = (bmp.Height - (gap_t + gap_b));

                float gap_w = bmp_w / 17;
                float gap_h = bmp_h / 8;

                float x;
                float y;
                float width;
                float height;
                RectangleF drawRect;

                Pen blackPen = new Pen(Color.Gray, 1);
                Font drawFont = new Font("굴림", 8);
                SolidBrush drawBrush = new SolidBrush(Color.Black);

                //Set format of string.
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;

                for (int i = 0; i <= 17; i++)
                {
                    x = gap_l + (gap_w * i) - 15.0F;
                    y = gap_t + bmp_h + 10.0F;
                    width = 30.0F;
                    height = 20.0F;
                    drawRect = new RectangleF(x, y, width, height);

                    g.DrawLine(blackPen, gap_l + (gap_w * i), gap_t, gap_l + (gap_w * i), gap_t + bmp_h);
                    g.DrawString((i * 10).ToString(), drawFont, drawBrush, drawRect, drawFormat);
                }

                drawFormat.Alignment = StringAlignment.Far;
                drawFormat.LineAlignment = StringAlignment.Center;

                for (int i = 0; i <= 8; i++)
                {
                    x = 0.0F;
                    y = gap_t + (gap_h * i) - 10.0F;
                    width = gap_l - 10.0F;
                    height = 20.0F;
                    drawRect = new RectangleF(x, y, width, height);

                    g.DrawLine(blackPen, gap_l, gap_t + (gap_h * i), gap_l + bmp_w, gap_t + (gap_h * i));
                    g.DrawString((160 - (i * 20)).ToString(), drawFont, drawBrush, drawRect, drawFormat);
                }

                x = gap_l + 10.0F;
                y = 10.0F;
                width = 200.0F;
                height = 20.0F;
                drawRect = new RectangleF(x, y, width, height);

                //Draw rectangle to screen
                //g.DrawRectangle(blackPen, x, y, width, height);

                drawFormat.Alignment = StringAlignment.Near;
                g.DrawString(PSet.Lang_Crv[16], drawFont, drawBrush, drawRect, drawFormat); //Vehicle Speed(km/h)

                x = (bmp.Width / 2) - 100.0F;
                y = gap_t + bmp_h + 20.0F;
                width = 200.0F;
                height = 20.0F;
                drawRect = new RectangleF(x, y, width, height);

                //Draw rectangle to screen
                //g.DrawRectangle(blackPen, x, y, width, height);

                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString(PSet.Lang_Crv[17], drawFont, drawBrush, drawRect, drawFormat); //시간(초)
            }

            return bmp;
        }
        private Bitmap Data_Graph(int Idx)
        {
            if (bmp == null) return null;

            Init_Graph();

            using (Graphics g = Graphics.FromImage(bmp))
            {
                float gap_l = 40;
                float gap_r = 20;
                float gap_t = 30;
                float gap_b = 40;
                float bmp_w = (bmp.Width - (gap_l + gap_r));
                float bmp_h = (bmp.Height - (gap_t + gap_b));

                float gap_w = bmp_w / 17;
                float gap_h = bmp_h / 8;

                float x, o_x = gap_l;
                float y, o_y = (gap_t + bmp_h);
                float scal_X = bmp_w / 170;
                float scal_Y = bmp_h / 160;

                int CNT = crv_Data.G_Data.Count - 1;
                Pen RedPen = new Pen(Color.Red, 2);
                Pen BluePen = new Pen(Color.Blue, 3);
                RectangleF drawRect;

                for (int i = 0; i <= CNT; i++)
                {
                    x = gap_l + (scal_X * crv_Data.G_Data[i].T_Time);
                    y = (gap_t + bmp_h) - (scal_Y * crv_Data.G_Data[i].Speed);

                    g.DrawLine(RedPen, o_x, o_y, x, y);

                    drawRect = new RectangleF(x - 1.5f, y - 1.5f, 3, 3);
                    g.DrawEllipse(BluePen, drawRect);

                    o_x = x; o_y = y;
                }

                if (Idx > -1 & crv_Data.G_Data.Count > 0)
                {
                    Pen PointPen = new Pen(Color.Green, 7);

                    x = gap_l + (scal_X * crv_Data.G_Data[Idx].T_Time);
                    y = (gap_t + bmp_h) - (scal_Y * crv_Data.G_Data[Idx].Speed);

                    drawRect = new RectangleF(x - 3.5f, y - 3.5f, 7, 7);
                    g.DrawEllipse(PointPen, drawRect);
                }
            }

            return bmp;
        }

        private void btnCurve_Click(object sender, EventArgs e)
        {
            string btn_Name = ((Button)sender).Name;

            this.Enabled = false;

            try
            {
                //int sped = int.Parse(txtSpeed.Text); //05.측정 속도(km/h)
                //int time = int.Parse(txt_Time.Text); //04.측정 시간(sec)
                
                if (chk_Item.Checked)
                {
                    switch (btn_Name)
                    {
                        case "btnIInit": CurveItemInit(); break;
                        case "btn_Save": CurveItemSave(); break;
                        case "btnClose": this.Close(); break;
                        case "btnI_Add": MDB_Item_Add(); break;
                        case "btnIEdit": MDB_ItemEdit(); break;
                        case "btnI_Del": MDB_Item_Del(); break;
                    }

                    MDB_ItemList();
                }
                else
                {
                    switch (btn_Name)
                    {
                        case "btnIInit": CurveItemInit(); break;
                        case "btn_Save": CurveItemSave(); break;
                        case "btnClose": this.Close(); break;
                        case "btnI_Add": CurveItem_Add(); break;
                        case "btnIEdit": CurveItemEdit(); break;
                        case "btnI_Del": CurveItem_Del(); break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("Input error");
            }

            this.Enabled = true;
        }

        private void chk_Item_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Item.Checked)
            {
                lst_Item.Top = 135;
                lst_Item.Left = 12;
                lst_Item.Width = 205;
                lst_Item.Height = 260;
            }
            else
            {
                lst_Item.Top = 87;
                lst_Item.Left = 12;
                lst_Item.Width = 205;
                lst_Item.Height = 308;
            }
        }

        private void lst_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lst_Item.SelectedItem == null) { return; }

            int cnt = main.DB_All.DB_Item.Select(lst_Item.SelectedItem.ToString());

            txtMDB00.Text = ""; //00.아이템 이름
            txtMDB01.Text = ""; //01.아이템 순서
            cboMDB02.Text = ""; //02.측정 구분(기본, ECU)

            if(cnt> 0)
            {
                if (chk_Item.Checked)
                {
                    txtMDB00.Text = main.DB_All.DB_Item.dbItemName; //00.아이템 이름
                    txtMDB01.Text = main.DB_All.DB_Item.dbItem_Idx; //01.아이템 순서
                    cboMDB02.Text = main.DB_All.DB_Item.dbRnB_Kind; //02.측정 구분(기본, ECU)
                }
                txt_Segm.Text = main.DB_All.DB_Item.dbSegments; //03.구분 명칭
                txt_Time.Text = main.DB_All.DB_Item.dbRnB_Time; //04.측정 시간(sec)
                txtSpeed.Text = main.DB_All.DB_Item.dbRnBSpeed; //05.측정 속도(km/h)
                txtItems.Text = main.DB_All.DB_Item.dbItem_Set; //06.표기 설명
                cbo_Vehi.Text = main.DB_All.DB_Item.dbVeh__Set; //07.차량 구동 여부
                cbo_Roll.Text = main.DB_All.DB_Item.dbRoll_Set; //08.롤러 구동 여부
                txt_Desc.Text = main.DB_All.DB_Item.dbDesc_Set; //09.내용 설명
            }
        }
        private void cboMDB02_SelectedIndexChanged(object sender, EventArgs e)
        {
            str_sort = cboMDB02.Text;

            MDB_ItemList();
        }
        private void MDB_ItemList()
        {
            DataTable dt = main.DB_All.DB_Item.Search(str_sort);

            lst_Item.Items.Clear();
            foreach(DataRow row in dt.Rows)
            {
                //lst_Item.Items.Add(row[1].ToString("000") + ":" + row[0]);
            }
        }
        private void MDB_Item_Add()
        {
            int cnt = main.DB_All.DB_Item.Select(txtMDB00.Text.Trim());

            main.DB_All.DB_Item.dbItemName = txtMDB00.Text; //00.아이템 이름
            main.DB_All.DB_Item.dbItem_Idx = txtMDB01.Text; //01.아이템 순서
            main.DB_All.DB_Item.dbRnB_Kind = cboMDB02.Text; //02.측정 구분(기본, ECU)
            main.DB_All.DB_Item.dbSegments = txt_Segm.Text; //03.구분 명칭
            main.DB_All.DB_Item.dbRnB_Time = txt_Time.Text; //04.측정 시간(sec)
            main.DB_All.DB_Item.dbRnBSpeed = txtSpeed.Text; //05.측정 속도(km/h)
            main.DB_All.DB_Item.dbItem_Set = txtItems.Text; //06.표기 설명
            main.DB_All.DB_Item.dbVeh__Set = cbo_Vehi.Text; //07.차량 구동 여부
            main.DB_All.DB_Item.dbRoll_Set = cbo_Roll.Text; //08.롤러 구동 여부
            main.DB_All.DB_Item.dbDesc_Set = txt_Desc.Text; //09.내용 설명

            if (cnt == 0)
            {
                main.DB_All.DB_Item.Insert();
            }
            else
            {
                main.DB_All.DB_Item.Update(txtMDB00.Text.Trim());
            }
        }
        private void MDB_ItemEdit()
        {
            int cnt = main.DB_All.DB_Item.Select(txtMDB00.Text.Trim());

            main.DB_All.DB_Item.dbItemName = txtMDB00.Text; //00.아이템 이름
            main.DB_All.DB_Item.dbItem_Idx = txtMDB01.Text; //01.아이템 순서
            main.DB_All.DB_Item.dbRnB_Kind = cboMDB02.Text; //02.측정 구분(기본, ECU)
            main.DB_All.DB_Item.dbSegments = txt_Segm.Text; //03.구분 명칭
            main.DB_All.DB_Item.dbRnB_Time = txt_Time.Text; //04.측정 시간(sec)
            main.DB_All.DB_Item.dbRnBSpeed = txtSpeed.Text; //05.측정 속도(km/h)
            main.DB_All.DB_Item.dbItem_Set = txtItems.Text; //06.표기 설명
            main.DB_All.DB_Item.dbVeh__Set = cbo_Vehi.Text; //07.차량 구동 여부
            main.DB_All.DB_Item.dbRoll_Set = cbo_Roll.Text; //08.롤러 구동 여부
            main.DB_All.DB_Item.dbDesc_Set = txt_Desc.Text; //09.내용 설명

            if (cnt == 0)
            {
                main.DB_All.DB_Item.Insert();
            }
            else
            {
                main.DB_All.DB_Item.Update(txtMDB00.Text.Trim());
            }
        }
        private void MDB_Item_Del()
        {
            int cnt = main.DB_All.DB_Item.Select(txtMDB00.Text.Trim());

            if (cnt > 0)
            {
                main.DB_All.DB_Item.Delete(txtMDB00.Text.Trim());
            }
        }

        private void CurveItemInit()
        {
            switch (crv_Mode)
            {
                case 0: Read_Graph(crv_Name);   
                        break;

                case 1: picGraph.Image = Init_Graph();
                        dgvCurve.DataSource = null;           
                        break;
            }
        }
        private void CurveItemSave()
        {
            if (crv_Data.Set_DriveCurve(crv_Name, crv_Data))
            {
                MessageBox.Show(PSet.Lang_Crv[18]); //저장되었습니다.
            }
        }
        private void CurveItem_Add()
        {
            int Idx;

            if (dgvCurve.CurrentRow == null)
            {
                Idx = 0;
                crv_Data.G_Data = new List<Curve_Data>();
            }
            else
            {
                Idx = dgvCurve.CurrentRow.Index;
            }

            try
            {
                int crvTime, crvSpeed;
                if (!int.TryParse(txt_Time.Text, out crvTime)) { MessageBoxEx.Show("Time 값이 올바르지 않습니다."); return; }
                if (!int.TryParse(txtSpeed.Text, out crvSpeed)) { MessageBoxEx.Show("Speed 값이 올바르지 않습니다."); return; }

                if (crv_Data.G_Data != null && crv_Data.G_Data.Count == Idx + 1)
                {
                    crv_Data.G_Data.Add(new Curve_Data
                    {
                        Segment = txt_Segm.Text.ToUpper(),
                        Time = crvTime,
                        T_Time = crvTime,
                        Speed = crvSpeed,
                        Items = txtItems.Text,
                        Vehicle = cbo_Vehi.Text,
                        Roll = cbo_Roll.Text,
                        Description = txt_Desc.Text
                    });
                }
                else
                {
                    crv_Data.G_Data.Insert(Idx, new Curve_Data
                    {
                        Segment = txt_Segm.Text.ToUpper(),
                        Time = crvTime,
                        T_Time = crvTime,
                        Speed = crvSpeed,
                        Items = txtItems.Text,
                        Vehicle = cbo_Vehi.Text,
                        Roll = cbo_Roll.Text,
                        Description = txt_Desc.Text
                    });
                }

                //MessageBox.Show(G_Data.Count.ToString());

                int ttime = 0;
                for (int cnt = 0; cnt < dgvCurve.Rows.Count; cnt++)
                {
                    ttime += Convert.ToInt32(dgvCurve.Rows[cnt].Cells["Time"].Value);
                    dgvCurve.Rows[cnt].Cells["T_Time"].Value = ttime.ToString();
                }

                dgvCurve.DataSource = null;
                dgvCurve.DataSource = crv_Data.G_Data;
                //dgvCurve.Rows.Add(txt_Segm.Text, txt_Time.Text, txtTTime.Text, txtSpeed.Text, txtItems.Text, txt_Vehi.Text, txt_Roll.Text, txt_Desc.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CurveItemEdit()
        {
            int ttime = 0;

            try
            {
                dgvCurve.CurrentRow.Cells["Segment"].Value = txt_Segm.Text.ToUpper();
                dgvCurve.CurrentRow.Cells["Time"].Value = txt_Time.Text;
                dgvCurve.CurrentRow.Cells["T_Time"].Value = txt_Time.Text;
                dgvCurve.CurrentRow.Cells["Speed"].Value = txtSpeed.Text;
                dgvCurve.CurrentRow.Cells["Items"].Value = txtItems.Text;
                dgvCurve.CurrentRow.Cells["Vehicle"].Value = cbo_Vehi.Text;
                dgvCurve.CurrentRow.Cells["Roll"].Value = cbo_Roll.Text;
                dgvCurve.CurrentRow.Cells["Description"].Value = txt_Desc.Text;

                for (int cnt = 0; cnt < dgvCurve.Rows.Count; cnt++)
                {
                    ttime += Convert.ToInt32(dgvCurve.Rows[cnt].Cells["Time"].Value);
                    dgvCurve.Rows[cnt].Cells["T_Time"].Value = ttime.ToString();
                }

                dgvCurve.DataSource = null;
                dgvCurve.DataSource = crv_Data.G_Data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CurveItem_Del()
        {
            int ttime = 0;
            int Idx = dgvCurve.CurrentRow.Index;

            crv_Data.G_Data.RemoveAt(Idx);

            for (int cnt = 0; cnt < dgvCurve.Rows.Count; cnt++)
            {
                ttime += Convert.ToInt32(dgvCurve.Rows[cnt].Cells["Time"].Value);
                dgvCurve.Rows[cnt].Cells["T_Time"].Value = ttime.ToString();
            }

            dgvCurve.DataSource = null;
            dgvCurve.DataSource = crv_Data.G_Data;
        }

        private void dgvCurve_Click(object sender, EventArgs e)
        {
            Select_Curve();
        }
        private void dgvCurve_CurrentCellChanged(object sender, EventArgs e)
        {
            Select_Curve();
        }
        private void Select_Curve()
        {
            if (dgvCurve.CurrentRow == null) return;

            try
            {
                int Idx = dgvCurve.CurrentRow.Index;

                picGraph.Image = Data_Graph(Idx);

                txt_Segm.Text = dgvCurve.CurrentRow.Cells["Segment"].Value.ToString();
                txt_Time.Text = dgvCurve.CurrentRow.Cells["Time"].Value.ToString();
              //txtTTime.Text = dgvCurve.CurrentRow.Cells["T_Time"].Value.ToString();
                txtSpeed.Text = dgvCurve.CurrentRow.Cells["Speed"].Value.ToString();
                txtItems.Text = dgvCurve.CurrentRow.Cells["Items"].Value.ToString();
                cbo_Vehi.Text = dgvCurve.CurrentRow.Cells["Vehicle"].Value.ToString();
                cbo_Roll.Text = dgvCurve.CurrentRow.Cells["Roll"].Value.ToString();
                txt_Desc.Text = dgvCurve.CurrentRow.Cells["Description"].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}