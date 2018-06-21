using System;
using System.Collections.Generic;
using Windows.UI;

namespace rMind.ColorForge
{
    /*
    public class ColorHelper
    {
        
         
        /// <summary>
        /// Получение оттентов цвета
        /// </summary>
        /// <param name="baseColor">базовый цвет</param>
        /// <param name="count">количество оттенов</param>
        /// <returns></returns>
        public static List<Color> GetColorShades(Color baseColor, int count)
        {
            List<Color> colorShades = new List<Color>();

            HSVColor hsv = RGBtoHSV(baseColor);
            hsv.V = 255.0;
            double v = hsv.V / count; // shade step

            for (int i = 0; i < count; i++)
            {
                hsv.V = v * i;
                if (hsv.V > 255.0)
                    hsv.V = 255.0;

                colorShades.Add(HSVtoRGB(hsv));
            }

            return colorShades;
        }

        public static HSVColor RGBtoHSV(Color rgb)
        {
            double max, min, chroma;
            HSVColor hsv = new HSVColor();

            min = Math.Min(Math.Min(rgb.R, rgb.B), rgb.G);
            max = Math.Max(Math.Max(rgb.R, rgb.B), rgb.G);

            chroma = max - min;

            if (chroma != 0)
            {
                if (rgb.R == max)
                {
                    hsv.H = (rgb.G - rgb.B) / chroma;
                    if (hsv.H < 0.0)
                        hsv.H += 6.0;
                }
                else if (rgb.G == max)
                {
                    hsv.H = ((rgb.B - rgb.R) / chroma) + 2.0;
                }
                else 
                {
                    hsv.H = ((rgb.R - rgb.G) / chroma) + 4.0;
                }

                hsv.H *= 60.0;
                hsv.S = chroma / max;
            }

            hsv.V = max;
            hsv.A = rgb.A;

            return hsv;
        }

        public static Color HSVtoRGB(HSVColor hsv)
        {
            double min, chroma, hdash, x;
            Color rgb = new Color();

            chroma = hsv.S * hsv.V;
            hdash = hsv.H / 60.0;

            x = chroma / (1.0 - Math.Abs((hdash % 2.0) - 1.0));

            if (hdash < 1.0)
            {
                rgb.R = (byte)chroma;
                rgb.G = (byte)x;
            }
            else if (hdash < 2.0)
            {
                rgb.R = (byte)x;
                rgb.G = (byte)chroma;
            }
            else if (hdash < 3.0)
            {
                rgb.G = (byte)chroma;
                rgb.B = (byte)x;
            }
            else if (hdash < 4.0)
            {
                rgb.G = (byte)x;
                rgb.B = (byte)chroma;
            }
            else if (hdash < 5.0)
            {
                rgb.R = (byte)x;
                rgb.B = (byte)chroma;
            }
            else if (hdash < 3.0)
            {
                rgb.R = (byte)chroma;
                rgb.B = (byte)x;
            }

            min = hsv.V - chroma;

            rgb.R += (byte)min;
            rgb.G += (byte)min;
            rgb.B += (byte)min;
            rgb.A = (byte)hsv.A;

            return rgb;
        }
    }

    public class HSVColor
    {
        public double H { get; set; }
        public double S { get; set; }
        public double V { get; set; }
        public double A { get; set; }

        public HSVColor()
        {
            H = S = V = A = 1.0;
        }
    }
    */

        public class ColorHelper
        {
            public static List<Color> GetColorShades(Color baseColor, int max)
            {
                // fill color shades list
                List<Color> colorShades = new List<Color>();
                HSVColor hsv = ColorHelper.RGBtoHSV(baseColor);
                // hsv.V = 255; 
                double v = 255 / max;
                for (int i = 0; i < max; i++)
                {                    
                    hsv.V = 255 - v * i;
                    if (hsv.V > 255) hsv.V = 255;
                    if (hsv.V < 0) hsv.V = 0;
                    
                    colorShades.Add(ColorHelper.HSVtoRGB(hsv));
                }
                return colorShades;
            }

            public static HSVColor RGBtoHSV(Windows.UI.Color rgb)
            {
                HSVColor hsv = new HSVColor();
                double max, min, chroma;
            
                min = Math.Min(Math.Min(rgb.R, rgb.G), rgb.B);
                max = Math.Max(Math.Max(rgb.R, rgb.G), rgb.B);
            
                hsv.V = max;
                chroma = max - min;

                if (chroma < 0.00001)
                {
                    hsv.S = 0;
                    hsv.H = 0;
                    return hsv;
                }

                if (max > 0.0)
                { 
                    hsv.S = (chroma / max);   
                }
                else
                {
                    hsv.S = 0.0;
                    hsv.H = 0.0;   
                    return hsv;
                }

                if (rgb.R >= max)    
                    hsv.H = ( rgb.G - rgb.B) / chroma;       
                else if ( rgb.G >= max )
                    hsv.H = 2.0 + (rgb.B - rgb.R) / chroma; 
                else
                    hsv.H = 4.0 + (rgb.R - rgb.G) / chroma;
                hsv.H *= 60.0;

                if (hsv.H < 0.0)
                    hsv.H += 360.0;

                return hsv;
            }

            public static Windows.UI.Color HSVtoRGB(HSVColor hsv)
            {
                double min, chroma, hdash, x;
                Windows.UI.Color rgb = new Windows.UI.Color();

                chroma = hsv.S * hsv.V;
                hdash = hsv.H / 60.0;
                x = chroma * (1.0 - Math.Abs((hdash % 2.0) - 1.0));

                if (hdash < 1.0)
                {
                    rgb.R = (byte)chroma;
                    rgb.G = (byte)x;
                }
                else if (hdash < 2.0)
                {
                    rgb.R = (byte)x;
                    rgb.G = (byte)chroma;
                }
                else if (hdash < 3.0)
                {
                    rgb.G = (byte)chroma;
                    rgb.B = (byte)x;
                }
                else if (hdash < 4.0)
                {
                    rgb.G = (byte)x;
                    rgb.B = (byte)chroma;
                }
                else if (hdash < 5.0)
                {
                    rgb.R = (byte)x;
                    rgb.B = (byte)chroma;
                }
                else if (hdash < 6.0)
                {
                    rgb.R = (byte)chroma;
                    rgb.B = (byte)x;
                }

                min = hsv.V - chroma;

                rgb.R += (byte)min;
                rgb.G += (byte)min;
                rgb.B += (byte)min;
                rgb.A = (byte)(hsv.A * 255);

                return rgb;
            }
        }

        public class HSVColor
        {
            public double H { get; set; }
            public double S { get; set; }
            public double V { get; set; }
            public double A { get; set; }

            public HSVColor()
            {
                H = S = V = A = 1.0;
            }
        }
    }
