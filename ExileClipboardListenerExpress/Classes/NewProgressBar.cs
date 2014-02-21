using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ExileClipboardListener.Classes
{
    class NewProgressBar : ProgressBar
    {
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
                backColor = Color.Orange;
                foreColor = Color.Brown;
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
                        //Paint the range
                        var rectFill = new Rectangle(0, 0, Width - 1, Height);
                        rectFill.Inflate(new Size(-2, -1));
                        rectFill.Width = (int)(rectFill.Width * ((double)(threshold.RangeHigh - threshold.RangeLow) / Maximum));
                        var brushFillRange = new LinearGradientBrush(rectFill, BackColor,  (Value >= threshold.RangeLow && Value <= threshold.RangeHigh) ? backColor : Color.Gray, LinearGradientMode.Vertical);
                        offscreen.FillRectangle(brushFillRange, (Width - 1) * ((float)threshold.RangeLow / Maximum) + 1, 2, rectFill.Width, rectFill.Height);

                        //Paint the boundaries
                        var rectBoundary = new Rectangle(0, 0, 1, Height - 2);
                        var brushRange = new LinearGradientBrush(rectBoundary, BackColor, Color.Black, LinearGradientMode.Vertical);
                        offscreen.FillRectangle(brushRange, (Width - 1) * ((float)threshold.RangeLow / Maximum), 2, rectBoundary.Width, rectBoundary.Height);
                        offscreen.FillRectangle(brushRange, (Width - 1) * ((float)threshold.RangeHigh / Maximum), 2, rectBoundary.Width, rectBoundary.Height);
                    }

                    //Then a thin rectangle to show the value
                    var rectValue = new Rectangle(0, 0, 3, Height - 2);
                    //rectValue.Inflate(new Size(0, -1));
                    var brushValue = new LinearGradientBrush(rect, BackColor, foreColor, LinearGradientMode.Vertical);
                    var x = (Width - 1) * ((float)Value / Maximum) + 1;
                    if (x < 0)
                        x = 0;
                    if (x > Width - 4)
                        x = Width - 4;
                    offscreen.FillRectangle(brushValue, x, 2, rectValue.Width, rectValue.Height);
                    e.Graphics.DrawImage(offscreenImage, 0, 0);
                    offscreenImage.Dispose();
                }
            }
        }
    }
}
