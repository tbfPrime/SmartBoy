using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace SmartBoy
{
    class GenerateColorCode
    {
        public string AvgColorCode(Bitmap bmp)
        {
            //Used for tally
            int r = 0;
            int g = 0;
            int b = 0;

            int total = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);

                    r += clr.R;
                    g += clr.G;
                    b += clr.B;

                    total++;
                }
            }

            //Calculate average
            r /= total;
            g /= total;
            b /= total;

            //return Color.FromArgb(r, g, b);
            return "#" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
        }

        public string DarkerColor(string col) {
            char[] c = col.ToCharArray();
            StringBuilder red = new StringBuilder();
            StringBuilder blue = new StringBuilder();
            StringBuilder green = new StringBuilder();

            for (int i = 1; i < c.Length; i++)
            {
                if (i < 3) {
                    red.Append(c[i]);
                }
                else if (i < 5)
                {
                    green.Append(c[i]);
                }
                else {
                    blue.Append(c[i]);
                }
            }

            int r = Convert.ToInt32(red.ToString(), 16);
            int b = Convert.ToInt32(blue.ToString(), 16);
            int g = Convert.ToInt32(green.ToString(), 16);

            r = ((r - 50) <= 0) ? 0 : (r - 50);
            b = ((b - 50) <= 0) ? 0 : (b - 50);
            g = ((g - 50) <= 0) ? 0 : (g - 50);

            return "#" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
        }
    }
}
