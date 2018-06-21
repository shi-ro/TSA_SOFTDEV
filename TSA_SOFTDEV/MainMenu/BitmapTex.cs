using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
    public class BitmapTex
    {
        public string Text = "";
        public Bitmap Image;
        public int Buffer = 10;

        private Bitmap _default = new Bitmap(596, 107);
        private char[] _kE = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
        private char[] _sP = new char[] { '⁰', '¹', '²', '³', '⁴', '⁵', '⁶', '⁷', '⁸', '⁹'};

        public BitmapTex(string text)
        {
            Text = text;
            UpdateTex();
        }
        public void UpdateTex()
        {
            string F = Text;
            F = F.Replace("sqrt", "√");
            if (F.Contains("indefinite integral of"))
            {
                F = F.Replace("indefinite integral of", "⌠\n⎮");
                F += "\n⌡";
            }

            Image = (Bitmap)_default.Clone();
            Graphics g = Graphics.FromImage(Image);
            RectangleF rectf = new RectangleF(Buffer, Buffer, _default.Width-Buffer*2, _default.Height-Buffer*2);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(F, new Font("Terminal", 16), Brushes.Black, rectf);
            g.Flush();
        }
    }
}
