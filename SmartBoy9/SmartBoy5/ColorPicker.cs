using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;



namespace SmartBoy
{
    class ColorPicker
    {
        public Color GetDominantColor(BitmapSource x)
        {
            return CalculateDominantColor(x);
        }


        public static Color CalculateDominantColor(BitmapSource source)
        {
            if (source.Format.BitsPerPixel != 32 || source.Format != System.Windows.Media.PixelFormats.Bgra32)
                throw new ApplicationException("expected 32bit image");


            Dictionary<Color, double> colorDist = new Dictionary<Color, double>();

            System.Windows.Size sz = new System.Windows.Size(source.PixelWidth, source.PixelHeight);

            //read bitmap 
            int pixelsSz = (int)sz.Width * (int)sz.Height * (source.Format.BitsPerPixel / 8);
            int stride = ((int)sz.Width * source.Format.BitsPerPixel + 7) / 8;
            int pixelBytes = (source.Format.BitsPerPixel / 8);

            byte[] pixels = new byte[pixelsSz];
            source.CopyPixels(pixels, stride, 0);

            const int alphaThershold = 70;
            UInt64 pixelCount = 0;
            UInt64 avgAlpha = 0;
            try
            {
                for (int y = 0; y < sz.Height; y++)
                {
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
                avgAlpha = avgAlpha / pixelCount;
                if (avgAlpha >= (255 - alphaThershold))
                    avgAlpha = 255;

                //take weighted average of top 2% of colors         
                var clrs = (from entry in colorDist
                            orderby entry.Value ascending
                            select new { Color = entry.Key, Dist = 1.0 / Math.Max(1, entry.Value) }).ToList().Take(Math.Max(1, (int)(colorDist.Count * 0.02)));

                double sumDist = clrs.Sum(x => x.Dist);
                Color result = Color.FromArgb((byte)avgAlpha,
                                              (byte)(clrs.Sum(x => x.Color.R * x.Dist) / sumDist),
                                              (byte)(clrs.Sum(x => x.Color.G * x.Dist) / sumDist),
                                              (byte)(clrs.Sum(x => x.Color.B * x.Dist) / sumDist));


                return result;
            }
            catch { }
            return new Color();
        }

        public Color Getcolor(BitmapImage b)
        {
            return AverageColor(b);
        }
        System.Drawing.Bitmap bmp;
        static Color AverageColor(BitmapImage b)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(b));
                enc.Save(outStream);
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(outStream);

                int width = bmp.Width;
                int height = bmp.Height;
                int red = 0;
                int green = 0;
                int blue = 0;
                int alpha = 0;
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        var pixel = bmp.GetPixel(x, y);
                        red += pixel.R;
                        green += pixel.G;
                        blue += pixel.B;
                        alpha += pixel.A;
                    }

                Func<int, int> avg = c => c / (width * height);

                red = avg(red);
                green = avg(green);
                blue = avg(blue);
                alpha = avg(alpha);

                var color = Color.FromArgb(alpha, red, green, blue);

                return color;
            }
        }
    }
}
