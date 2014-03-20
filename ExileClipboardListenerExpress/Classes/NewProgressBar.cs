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
            if (Value * 100 / Width < Properties.Settings.Default.ToleranceAverageFrom)
            {
                backColor = Color.IndianRed;
                foreColor = Color.DarkRed;
            }
            else if (Value * 100 / Width < Properties.Settings.Default.ToleranceGoodFrom)
            {
                backColor = Color.Orange;
                foreColor = Color.Chocolate;
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
                        var rectFill = new Rectangle(0, 0, threshold.RangeHigh - threshold.RangeLow - 1, Height);
                        if (rectFill.Width < 1)
                            rectFill.Width = 1;
                        var brushFillRange = new LinearGradientBrush(rectFill, BackColor,  (Value >= threshold.RangeLow && Value <= threshold.RangeHigh) ? backColor : Color.Gray, LinearGradientMode.Vertical);
                        offscreen.FillRectangle(brushFillRange, threshold.RangeLow + 1, 1, rectFill.Width, rectFill.Height);

                        //Draw a thin rectangle to show the value
                        if (Value >= threshold.RangeLow && Value <= threshold.RangeHigh)
                        {
                            var rectValue = new Rectangle(0, 0, 3, Height - 2);
                            var brushValue = new LinearGradientBrush(rect, BackColor, foreColor, LinearGradientMode.Vertical);
                            var x = Value;
                            if (x < 0)
                                x = 0;
                            if (x > Width - 4)
                                x = Width - 4;
                            if (x <= threshold.RangeLow + 2)
                                x = threshold.RangeLow + 2;
                            if (x >= threshold.RangeHigh - 2)
                                x = threshold.RangeHigh - 2;
                            offscreen.FillRectangle(brushValue, x - 1, 1, rectValue.Width, rectValue.Height);
                        }

                        //Paint the boundaries
                        var rectBoundary = new Rectangle(0, 0, 1, Height - 1);
                        var brushRange = new LinearGradientBrush(rectBoundary, BackColor, Color.Black, LinearGradientMode.Vertical);
                        offscreen.FillRectangle(brushRange, threshold.RangeLow, 1, rectBoundary.Width, rectBoundary.Height);
                        offscreen.FillRectangle(brushRange, threshold.RangeHigh, 1, rectBoundary.Width, rectBoundary.Height);
                    }
                    e.Graphics.DrawImage(offscreenImage, 0, 0);
                    offscreenImage.Dispose();
                }
            }
        }
    }
}
