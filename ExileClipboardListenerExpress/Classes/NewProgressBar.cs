using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ExileClipboardListener.Classes
{
    class NewProgressBar : ProgressBar
    {
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
            using (Image offscreenImage = new Bitmap(Width, Height))
            {
                using (Graphics offscreen = Graphics.FromImage(offscreenImage))
                {
                    var rect = new Rectangle(0, 0, Width, Height);

                    if (ProgressBarRenderer.IsSupported)
                        ProgressBarRenderer.DrawHorizontalBar(offscreen, rect);

                    //Deflate inner rect
                    rect.Inflate(new Size(-inset, -inset)); 
                    rect.Width = (int)(rect.Width * ((double)Value / Maximum));
                    
                    //Can't draw rec with width of 0
                    if (rect.Width == 0) 
                        rect.Width = 1; 
                    var brush = new LinearGradientBrush(rect, BackColor, ForeColor, LinearGradientMode.Vertical);
                    offscreen.FillRectangle(brush, inset, inset, rect.Width, rect.Height);
                    e.Graphics.DrawImage(offscreenImage, 0, 0);
                    offscreenImage.Dispose();
                }
            }
        }
    }
}
