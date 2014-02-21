using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ExileClipboardListener.Classes
{
    class NewProgressBar : ProgressBar
    {
        //public int RangeLow;
        //public int RangeHigh;
        public class Tuple
        {
            public int RangeLow;
            public int RangeHigh;
        }
        public List<Tuple> Thresholds = new List<Tuple>();

        public NewProgressBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //Helps control the flicker
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //A single inset value to control the sizing of the inner rect
            const int inset = 2;

            //Determine the "theme", i.e. what colour scheme to use for this control
            Color backColor;
            Color foreColor;
            if (Value < Properties.Settings.Default.ToleranceAverageFrom)
            {
                backColor = Color.IndianRed;
                foreColor = Color.DarkRed;
            }
            else if (Value < Properties.Settings.Default.ToleranceGoodFrom)
            {
                backColor = Color.Yellow;
                foreColor = Color.Orange;
            }
            else
            {
                backColor = Color.ForestGreen;
                foreColor = Color.DarkGreen;
            }

            using (Image offscreenImage = new Bitmap(Width, Height))
            {
                using (Graphics offscreen = Graphics.FromImage(offscreenImage))
                {
                    var rect = new Rectangle(0, 0, Width, Height);
                    if (ProgressBarRenderer.IsSupported)
                        ProgressBarRenderer.DrawHorizontalBar(offscreen, rect);

                    //We draw a rectangle to show the threshold ranges
                    foreach (var threshold in Thresholds)
                    {
                        //Paint the range if it is selected
                        if (Value >= threshold.RangeLow && Value <= threshold.RangeHigh)
                        {
                            var rectFill = new Rectangle(0, 0, Width, Height);
                            rectFill.Inflate(new Size(-inset, -inset));
                            rectFill.Width = (int)(rectFill.Width * ((double)(threshold.RangeHigh - threshold.RangeLow) / Maximum));
                            var brushFillRange = new LinearGradientBrush(rectFill, BackColor, backColor, LinearGradientMode.Vertical);
                            offscreen.FillRectangle(brushFillRange, Width * ((float)threshold.RangeLow / Maximum) + inset, inset, rectFill.Width, rectFill.Height);
                        }

                        //Paint the boundaries
                        var rectBoundary = new Rectangle(0, 0, 1, Height);
                        var brushRange = new LinearGradientBrush(rectBoundary, BackColor, Color.Black, LinearGradientMode.Vertical);
                        offscreen.FillRectangle(brushRange, Width * ((float)threshold.RangeLow / Maximum), 0, rectBoundary.Width, rectBoundary.Height);
                        offscreen.FillRectangle(brushRange, Width * ((float)threshold.RangeHigh / Maximum), 0, rectBoundary.Width, rectBoundary.Height);
                    }

                    //Then a thin rectangle to show the value
                    var rectValue = new Rectangle(0, 0, 3, Height);
                    rectValue.Inflate(new Size(0, -inset));
                    var brushValue = new LinearGradientBrush(rect, BackColor, foreColor, LinearGradientMode.Vertical);
                    offscreen.FillRectangle(brushValue, Width * ((float)Value / Maximum) + inset, inset, rectValue.Width, rectValue.Height);
                    e.Graphics.DrawImage(offscreenImage, 0, 0);
                    offscreenImage.Dispose();
                }
            }
        }
    }
}
