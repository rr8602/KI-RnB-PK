using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace KI_RnB
{
    public class cls_Gage
    {
        private PaintEventArgs g;
        private Pen p1;
        private Graphics graphic;
        private Rectangle rectGreen, rectYellow, rectRed;
        private GraphicsPath gPathGreen, gPathYellow, gPathRed;
        private LinearGradientBrush GreenBrush, YellowBrush, RedBrush;
        private PointF Pt1 = new Point();
        private PointF Pt2 = new Point();
        private Control Ctrl;

        private int Width, Half, Length, Max; // 이미지의 반지름  // 게이지 바늘의 길이
        private double OneAngle; // 이미지의 원둘레//움직이는각도(눈금당)
        private double d_Angle = 110; // 0위치의 각도
        private double d_Rad;
        private double minZone, maxZone;
        private float minZoneAngle, maxZoneAngle;
        
        private string Caption;
        private string Unit;

        public float FontSize { get; set; }

        public double d_value;
        public double Value
        {
            get
            {
                return d_value;
            }
            set
            {
                d_value = value;

                if (value >= minZone && value <= maxZone)
                {
                    //p1.Color = Color.Red;
                    p1.Color = Color.Lime;
                }
                else
                {
                    p1.Color = Color.Lime;
                    //p1.Color = Color.FromArgb(255, 255, 255);
                }

                Gage_Set(); Ctrl.Invalidate();
            }
        }

        public cls_Gage(Control c, int width, int max, double oneAngle, string Caption, string Unit)
        {
            try
            {
                this.Ctrl = c; 
                this.Width = width;
                this.Max = max;
                this.OneAngle = oneAngle;
                this.Caption = Caption;
                this.Unit = Unit;

                FontSize = 20;

                Half = width / 2;
                p1 = new Pen(Color.Red, 7);
                rectGreen = new Rectangle(19, 19, c.Width - 40, c.Height - 40);
                rectYellow = new Rectangle(19, 19, c.Width - 40, c.Height - 40);
                rectRed = new Rectangle(19, 19, c.Width - 40, c.Height - 40);

                gPathGreen = new GraphicsPath();
                gPathYellow = new GraphicsPath();
                gPathRed = new GraphicsPath();

                GreenBrush = new LinearGradientBrush(rectGreen, Color.FromArgb(100, 0, 255, 0), Color.FromArgb(100, 0, 255, 0), LinearGradientMode.Vertical);
                YellowBrush = new LinearGradientBrush(rectYellow, Color.FromArgb(100, 255, 255, 0), Color.FromArgb(100, 255, 255, 0), LinearGradientMode.Vertical);
                RedBrush = new LinearGradientBrush(rectRed, Color.FromArgb(100, 255, 0, 0), Color.FromArgb(100, 255, 0, 0), LinearGradientMode.Vertical);

                p1.StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                p1.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;

                Length = Half - 60;

                Pt1.X = Half;
                Pt1.Y = Half;

                Value = 0;
            }
            catch (Exception e)
            { MessageBox.Show(e.Message, "ClsGage Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void Gage_Set()
        {
            try
            {
                if (Max < Value) Value = Max;

                d_Rad = (110 + (Value * OneAngle)) * (Math.PI / 180);
                Pt2.X = (float)(Length * Math.Cos(d_Rad) + Half);
                Pt2.Y = (float)(Length * Math.Sin(d_Rad) + Half);
            }
            catch (Exception e)
            { 
                MessageBox.Show(e.Message, "GageSet Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        public void Gage_Show(Graphics grfx)
        {
            graphic = grfx;
            graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.FillPath(GreenBrush, gPathGreen);
            graphic.FillPath(YellowBrush, gPathYellow);
            graphic.FillPath(RedBrush, gPathRed);

            Draw_Caption();

            graphic.DrawLine(p1, Pt1, Pt2);
        }

        public void Draw_Caption()
        {
            float x = Half - 100;
            float y = Width * (0.25f);
            float width = 200.0F;
            float height = 25.0F;

            Font drawFont = new Font("Arial", 14, FontStyle.Bold);            
            SolidBrush drawBrush = new SolidBrush(Color.White);
            StringFormat drawFormat = new StringFormat();
            RectangleF drawRect = new RectangleF(x, y, width, height);

            drawFormat.Alignment = StringAlignment.Center;
            graphic.DrawString(Caption, drawFont, drawBrush, drawRect, drawFormat);

            x = Half - 100;
            y = Width * (0.73f);
            width = 200.0F;
            height = 25.0F;
            drawBrush = new SolidBrush(Color.White);
            drawRect = new RectangleF(x, y, width, height);
            drawFont = new Font("Arial", 14, FontStyle.Bold);
            graphic.DrawString(Unit, drawFont, drawBrush, drawRect, drawFormat);

            x = Half - 150;
            y = Width * (0.65f);
            width = 300.0F;
            height = FontSize * 1.3f;
            drawBrush = new SolidBrush(Color.Lime);
            drawRect = new RectangleF(x, y, width, height);
            drawFont = new Font("Arial", FontSize, FontStyle.Bold);
            graphic.DrawString(d_value.ToString("0.0"), drawFont, drawBrush, drawRect, drawFormat);
        }

        public void Red_Zone(float Min, float Max)
        {
            try
            {
                minZone = Min; 
                maxZone = Max;
                
                minZoneAngle = ((float)d_Angle + (Min * (float)OneAngle));
                maxZoneAngle = ((float)d_Angle + (Max * (float)OneAngle));
                gPathRed.Dispose();
                gPathRed = new GraphicsPath();
                gPathRed.AddPie(rectGreen, minZoneAngle, Math.Abs(maxZoneAngle - minZoneAngle));
            }
            catch (Exception e)
            { { MessageBox.Show(e.Message, "DrawZone Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
        }

        public void Yellow_Zone(float Min, float Max)
        {
            try
            {
                minZoneAngle = ((float)d_Angle + (Min * (float)OneAngle));
                maxZoneAngle = ((float)d_Angle + (Max * (float)OneAngle));
                gPathYellow.Dispose();
                gPathYellow = new GraphicsPath();
                gPathYellow.AddPie(rectGreen, minZoneAngle, Math.Abs(maxZoneAngle - minZoneAngle));
            }
            catch (Exception e)
            { { MessageBox.Show(e.Message, "DrawZone Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
        }

        public void Green_Zone(float Min, float Max)
        {
            try
            {
                minZoneAngle = ((float)d_Angle + (Min * (float)OneAngle));
                maxZoneAngle = ((float)d_Angle + (Max * (float)OneAngle));
                gPathGreen.Dispose();
                gPathGreen = new GraphicsPath();
                gPathGreen.AddPie(rectGreen, minZoneAngle, Math.Abs(maxZoneAngle - minZoneAngle));
            }
            catch (Exception e)
            { { MessageBox.Show(e.Message, "DrawZone Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
        }

    }
}