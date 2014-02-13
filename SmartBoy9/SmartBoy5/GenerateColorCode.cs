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
        public string CalculateDominantColor(BitmapSource source)
        {
            Console.WriteLine("GenerateColorCode | CalculateDominantColor | Initializing...");
            if (source.Format.BitsPerPixel != 32)
                throw new ApplicationException("expected 32bit image");

            Dictionary<Color, double> colorDist = new Dictionary<Color, double>();

            System.Windows.Size sz = new System.Windows.Size(source.PixelWidth, source.PixelHeight);

            //read bitmap 
            int pixelsSz = (int)sz.Width * (int)sz.Height * (source.Format.BitsPerPixel / 8);
            int stride = ((int)sz.Width * source.Format.BitsPerPixel + 7) / 8;
            int pixelBytes = (source.Format.BitsPerPixel / 8);

            byte[] pixels = new byte[pixelsSz];
            source.CopyPixels(pixels, stride, 0);

            const int alphaThershold = 10;
            UInt64 pixelCount = 0;
            UInt64 avgAlpha = 0;

            for (int y = 0; y < sz.Height; y++)
            {
                Console.WriteLine("Working on Color code Y: " + y);
                for (int x = 0; x < sz.Width; x++)
                {
                    int index = (int)((y * sz.Width) + x) * (pixelBytes);
                    byte r1, g1, b1, a1; r1 = g1 = b1 = a1 = 0;
                    a1 = pixels[index + 3];
                    r1 = pixels[index + 2];
                    g1 = pixels[index + 1];
                    b1 = pixels[index];

                    if (a1 <= alphaThershold)
                        continue; //ignore

                    pixelCount++;
                    avgAlpha += (UInt64)a1;

                    Color cl = Color.FromArgb(0, r1, g1, b1);
                    double dist = 0;
                    if (!colorDist.ContainsKey(cl))
                    {
                        colorDist.Add(cl, 0);

                        for (int y2 = 0; y2 < sz.Height; y2++)
                        {
                            for (int x2 = 0; x2 < sz.Width; x2++)
                            {
                                int index2 = (int)(y2 * sz.Width) + x2;
                                byte r2, g2, b2, a2; r2 = g2 = b2 = a2 = 0;
                                a2 = pixels[index2 + 3];
                                r2 = pixels[index2 + 2];
                                g2 = pixels[index2 + 1];
                                b2 = pixels[index2];

                                if (a2 <= alphaThershold)
                                    continue; //ignore

                                dist += Math.Sqrt(Math.Pow(r2 - r1, 2) +
                                                  Math.Pow(g2 - g1, 2) +
                                                  Math.Pow(b2 - b1, 2));
                            }
                        }

                        colorDist[cl] = dist;
                    }
                }
            }

            //clamp alpha
            try
            {
                avgAlpha = avgAlpha / pixelCount;
                if (avgAlpha >= (255 - alphaThershold))
                    avgAlpha = 255;
            }
            catch {
                avgAlpha = 150;
            }
            //take weighted average of top 2% of colors         
            var clrs = (from entry in colorDist
                        orderby entry.Value ascending
                        select new { Color = entry.Key, Dist = 1.0 / Math.Max(1, entry.Value) }).ToList().Take(Math.Max(1, (int)(colorDist.Count * 0.02)));

            double sumDist = clrs.Sum(x => x.Dist);
            Color result = Color.FromArgb((byte)avgAlpha,
                                          (byte)(clrs.Sum(x => x.Color.R * x.Dist) / sumDist),
                                          (byte)(clrs.Sum(x => x.Color.G * x.Dist) / sumDist),
                                          (byte)(clrs.Sum(x => x.Color.B * x.Dist) / sumDist));


            Color c = result;
            Console.WriteLine("GenerateColorCode | CalculateDominantColor | Finalizing...");
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        /// <summary>
        /// find dominat color
        /// </summary>
        public string ContrastColor(Color color)
        {
            int d = 0;

            // Counting the perceptive luminance - human eye favors green color... 
            double a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            if (a < 0.5)
                d = 0; // bright colors - black font
            else
                d = 255; // dark colors - white font

            string x = "#" + d + d + d;
            return x;
        }


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
