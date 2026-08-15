using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace KI_RnB
{
    class cls_SSTs
    {
        private PaintEventArgs g;
        private Pen p1;
        private Graphics graphic;
        private Rectangle rectGreen, rectYellow, rectRed;
        private GraphicsPath gPathGreen, gPathYellow, gPathRed, gPathCenter;
        private LinearGradientBrush GreenBrush, YellowBrush, RedBrush;
        private PointF Center = new Point();
        private PointF Pt1 = new Point();
        private PointF Pt2 = new Point();
        private Control Ctrl;

        private double One_Point = 0;           // 0위치의 각도
        private double Zero_Point = 0;        // 0위치의 각도
        private double minRed, maxRed;
        private double minGreen, maxGreen;
        private double minYellow, maxYellow;
        private float minAngle, maxAngle;

        public bool Value_Show { get; set; }
        public bool Center_Show { get; set; }
        public bool Red_Show { get; set; }
        public bool Yellow_Show { get; set; }
        public bool Green_Show { get; set; }
        public bool Niddle_Show { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public string CarNo { get; set; }


        public float d_value;
        public float Value
        {
            get
            {
                return d_value;
            }
            set
            {
                d_value = value;

                if (PSet.SST_Type == 1)     //0:측정 않음, 1:막대 그래프, 2:숫자만
                {
                    if ((minGreen <= value) && (value <= maxGreen))
                    {
                        p1.Color = Color.Lime;
                    }
                    else if ((minYellow <= value) && (value <= maxYellow))
                    {
                        p1.Color = Color.Yellow;
                    }
                    else
                    {
                        p1.Color = Color.Red;
                    }
                }

                Gage_Set(); Ctrl.Invalidate();
            }
        }

        public cls_SSTs(Control c)
        {
            try
            {
                this.Ctrl = c;
                int Length = 430;

                Center.X = c.Width / 2; // Half - 2;
                Center.Y = c.Height / 2 + 43; // Half + 50;
                Zero_Point = Center.X - 1;
                One_Point = 36.5;

                rectGreen = new Rectangle((int)Center.X - Length, (int)Center.Y - Length, Length * 2, Length * 2);
                rectYellow = new Rectangle((int)Center.X - Length, (int)Center.Y - Length, Length * 2, Length * 2);
                rectRed = new Rectangle((int)Center.X - Length, (int)Center.Y - Length, Length * 2, Length * 2);

                gPathGreen = new GraphicsPath();
                gPathYellow = new GraphicsPath();
                gPathRed = new GraphicsPath();
                gPathCenter = new GraphicsPath();

                GreenBrush = new LinearGradientBrush(rectGreen, Color.Green, Color.Green, LinearGradientMode.Vertical);
                YellowBrush = new LinearGradientBrush(rectYellow, Color.Yellow, Color.Yellow, LinearGradientMode.Vertical);
                RedBrush = new LinearGradientBrush(rectRed, Color.Red, Color.Red, LinearGradientMode.Vertical);

                p1 = new Pen(Color.Red, 20);

                p1.StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                //p1.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;
                p1.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                Value = 0;
            }
            catch (Exception e)
            { MessageBox.Show(e.Message, "ClsGage Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void Gage_Set()
        {
            try
            {
                if (10 < Value) Value = 10;

                Pt1.Y = Center.Y; Pt1.X = (float)(Zero_Point + (Value * One_Point));
                Pt2.Y = Center.Y - 100 ; Pt2.X = (float)(Zero_Point + (Value * One_Point));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "GageSet Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Gage_Show(Graphics g)
        {
            graphic = g;

            graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;

            if (PSet.SST_Type == 1) //0:측정 않음, 1:막대 그래프, 2:숫자만
            {
                if (Red_Show) { graphic.FillPath(RedBrush, gPathRed); }
                if (Yellow_Show) { graphic.FillPath(YellowBrush, gPathYellow); }
                if (Green_Show) { graphic.FillPath(GreenBrush, gPathGreen); }
                if (Center_Show) { Center_Zone(); }
                Center_Line();

                if (Niddle_Show)
                {
                    graphic.DrawLine(p1, Pt1, Pt2);
                }
            }

            if (Value_Show)
            {
                Draw_Value();
            }

            Draw_Head();
            Draw_Title();
            Draw_Message();
        }

        public void Draw_Head()
        {
            float width = 800.0F;
            float height = 200.0F;
            float x = Center.X - (width / 2);
            float y = 20;

            Font drawFont = new Font("굴림", 30, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            StringFormat drawFormat = new StringFormat();
            RectangleF drawRect = new RectangleF(x, y, width, height);

            drawFormat.Alignment = StringAlignment.Center;
            graphic.DrawString(CarNo, drawFont, drawBrush, drawRect, drawFormat);
        }

        public void Draw_Title()
        {
            float width = 800.0F;
            float height = 200.0F;
            float x = Center.X - (width / 2);
            float y = 120;

            Font drawFont = new Font("굴림", 45, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            StringFormat drawFormat = new StringFormat();
            RectangleF drawRect = new RectangleF(x, y, width, height);

            drawFormat.Alignment = StringAlignment.Center;
            graphic.DrawString(Title, drawFont, drawBrush, drawRect, drawFormat);
        }

        public void Draw_Message()
        {
            float width = Ctrl.Width;
            float height = 200.0F;
            float x = Center.X - (width / 2);
            float y = 640;

            Font drawFont = new Font("굴림", 70, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.Yellow);
            StringFormat drawFormat = new StringFormat();
            RectangleF drawRect = new RectangleF(x, y, width, height);

            drawFormat.Alignment = StringAlignment.Center;
            graphic.DrawString(Message, drawFont, drawBrush, drawRect, drawFormat);
        }

        public void Draw_Value()
        {
            float width = 400.0F;
            float height = 100.0F;
            float x = Center.X - (width / 2);
            float y = Center.Y + 40;

            //Image ValueArea = Image.FromFile(@"Image\ValueArea-1.gif");
            //graphic.DrawImage(ValueArea, Pt1.X - ValueArea.Width / 2, Pt1.Y - 170);

            Font drawFont = new Font("굴림", 50, FontStyle.Bold);

            if (PSet.SST_Type == 2) //0:측정 않음, 1:막대 그래프, 2:숫자만
            {
                width = 800.0F;
                height = 200.0F;
                x = Center.X - (width / 2);
                y = Center.Y - (height / 2); 

                drawFont = new Font("굴림", 100, FontStyle.Bold);
            }

            SolidBrush drawBrush;
            StringFormat drawFormat = new StringFormat();
            RectangleF drawRect = new RectangleF(x, y, width, height);

            drawFormat.Alignment = StringAlignment.Center;

            if ((minGreen <= Value) && (Value <= maxGreen))
            {
                drawBrush = new SolidBrush(Color.Lime);
            }
            else if ((minYellow <= Value) && (Value <= maxYellow))
            {
                drawBrush = new SolidBrush(Color.Yellow);
            }
            else
            {
                drawBrush = new SolidBrush(Color.Red);
            }

            if (Value == 0)
            {
                graphic.DrawString(" " + Value.ToString("#0.0"), drawFont, drawBrush, drawRect, drawFormat);
            }
            else if (Value > 0)
            {
                graphic.DrawString("OUT " + Value.ToString("#0.0"), drawFont, drawBrush, drawRect, drawFormat);
            }
            else
            {
                graphic.DrawString("IN " + Math.Abs(Value).ToString("#0.0"), drawFont, drawBrush, drawRect, drawFormat);
            }
        }

        public void Center_Zone()
        {
            graphic.DrawLine(new Pen(Color.Red, 1), 0, Center.Y, Ctrl.Width, Center.Y);
            graphic.DrawLine(new Pen(Color.Red, 1), Center.X, 0, Center.X, Ctrl.Height);
        }

        public void Red_Zone(float Min, float Max)
        {
            try
            {
                minRed = Min;
                maxRed = Max;

                minAngle = (float)(Zero_Point + (Min * One_Point));
                maxAngle = (float)(Zero_Point + (Max * One_Point));
                gPathRed.Dispose();
                gPathRed = new GraphicsPath();
                RectangleF rect = new RectangleF(minAngle, Center.Y - 10, maxAngle - minAngle, 20);
                gPathRed.AddRectangle(rect);
            }
            catch (Exception e)
            { { MessageBox.Show(e.Message, "DrawZone Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
        }

        public void Yellow_Zone(float Min, float Max)
        {
            try
            {
                minYellow = Min;
                maxYellow = Max;

                minAngle = (float)(Zero_Point + (Min * One_Point));
                maxAngle = (float)(Zero_Point + (Max * One_Point));
                gPathYellow.Dispose();
                gPathYellow = new GraphicsPath();
                RectangleF rect = new RectangleF(minAngle, Center.Y - 10, maxAngle - minAngle, 20);
                gPathYellow.AddRectangle(rect);
            }
            catch (Exception e)
            { { MessageBox.Show(e.Message, "DrawZone Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
        }

        public void Green_Zone(float Min, float Max)
        {
            try
            {
                minGreen = Min;
                maxGreen = Max;

                minAngle = (float)(Zero_Point + (Min * One_Point));
                maxAngle = (float)(Zero_Point + (Max * One_Point));
                gPathGreen.Dispose();
                gPathGreen = new GraphicsPath();
                RectangleF rect = new RectangleF(minAngle, Center.Y - 10, maxAngle - minAngle, 20);
                gPathGreen.AddRectangle(rect);
            }
            catch (Exception e)
            { { MessageBox.Show(e.Message, "DrawZone Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
        }

        public void Center_Line()
        {
            graphic.DrawLine(new Pen(Color.Red, 2), Center.X, Center.Y - 10, Center.X, Center.Y + 10);
        }
    }

    class cls__SST
    {
        private PaintEventArgs g;
        private Pen p1;
        private Graphics graphic;
        private PointF Center = new Point();
        private PointF Pt1 = new Point();
        private PointF Pt2 = new Point();
        private Control Ctrl;

        private double One_Point = 0;           // 0위치의 각도
        private double Zero_Point = 0;        // 0위치의 각도
        
        public string Title { get; set; }
        public string CarNo { get; set; }
        
        public float d_value;
        public float Value
        {
            get
            {
                return d_value;
            }
            set
            {
                d_value = value;

                p1.Color = Color.Lime;
                Gage_Set(); Ctrl.Invalidate();
            }
        }

        public cls__SST(Control c)
        {
            try
            {
                this.Ctrl = c;
                
                Center.X = c.Width / 2; // Half - 2;
                Center.Y = c.Height / 2 + 105; // Half + 50;
                Zero_Point = Center.X - 1;
                One_Point = 36.5;

                p1 = new Pen(Color.Red, 20);

                p1.StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                p1.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                Value = 0;
            }
            catch (Exception e)
            { MessageBox.Show(e.Message, "ClsGage Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void Gage_Set()
        {
            try
            {
                if (10 < Value) Value = 10;

                Pt1.Y = Center.Y;       Pt1.X = (float)(Zero_Point + (Value * One_Point));
                Pt2.Y = Center.Y - 100; Pt2.X = (float)(Zero_Point + (Value * One_Point));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "GageSet Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Gage_Show(Graphics g)
        {
            graphic = g;

            graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            
            graphic.DrawLine(p1, Pt1, Pt2);

            Draw_Value();
            Draw_Title();
        }

        public void Draw_Title()
        {
            float width = 800.0F;
            float height = 70.0F;
            float x = Center.X - (width / 2);
            float y = 30;

            Font drawFont = new Font("굴림", 45, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            StringFormat drawFormat = new StringFormat();
            RectangleF drawRect = new RectangleF(x, y, width, height);

            drawFormat.Alignment = StringAlignment.Center;
            graphic.DrawString(Title, drawFont, drawBrush, drawRect, drawFormat);
        }

        public void Draw_Value()
        {
            float width = 400.0F;
            float height = 130.0F;
            float x = Center.X - (width / 2);
            float y = Center.Y + 40;

            //Image ValueArea = Image.FromFile(@"Image\ValueArea-1.gif");
            //graphic.DrawImage(ValueArea, Pt1.X - ValueArea.Width / 2, Pt1.Y - 170);

            Font drawFont = new Font("굴림", 50, FontStyle.Bold);
            SolidBrush drawBrush;
            StringFormat drawFormat = new StringFormat();
            RectangleF drawRect = new RectangleF(x, y, width, height);

            drawFormat.Alignment = StringAlignment.Center;

            drawBrush = new SolidBrush(Color.Lime);

            if (Value == 0)
            {
                graphic.DrawString(" " + Value.ToString("#0.0"), drawFont, drawBrush, drawRect, drawFormat);
            }
            else if (Value > 0)
            {
                graphic.DrawString("OUT " + Value.ToString("#0.0"), drawFont, drawBrush, drawRect, drawFormat);
            }
            else
            {
                graphic.DrawString("IN " + Math.Abs(Value).ToString("#0.0"), drawFont, drawBrush, drawRect, drawFormat);
            }
        }
    }
}
