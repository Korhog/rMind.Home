using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
    
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace rMind.ColorContainer
{
    public class rMindColors
    {
        Dictionary<Color, SolidColorBrush> m_solid_brushes;

        private Random m_random;
        private static rMindColors instance;
        private static readonly object sync = new object();

        public static rMindColors Current()
        {
            lock(sync)
            {
                if (instance == null)
                {
                    instance = new rMindColors();
                }
                return instance;
            }
        }

        rMindColors()
        {
            m_solid_brushes = new Dictionary<Color, SolidColorBrush>();
            m_random = new Random();
        }
        
        public SolidColorBrush GetSolidBrush(Color color)
        {
            if (m_solid_brushes.ContainsKey(color))
                return m_solid_brushes[color];

            var brush = new SolidColorBrush(color);
            m_solid_brushes[color] = brush;
            return brush;
        }

        public AcrylicBrush GetAcrylicBrush(Color color)
        {

            var brush = new AcrylicBrush
            {
                TintColor = color,
                BackgroundSource = AcrylicBackgroundSource.Backdrop,
                TintOpacity = 0.5,
            };
            return brush;
        }

        public static Color GetSelectorColor()
        {
            return Colors.LightSkyBlue;
            //return ColorHelper.FromArgb(120, 255, 0, 0);
        }

        public static Brush GetSelectorBrush()
        {
            /*
            var brush = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops = new GradientStopCollection {
                    new GradientStop() {Color = Colors.BlueViolet, Offset = 0},
                    new GradientStop() {Color = Colors.SkyBlue, Offset = 1}
                }
            };
            */

            var brush = new SolidColorBrush(Colors.LimeGreen);
            return brush;
        }

        static byte Clamp(byte value, byte min, byte max)
        {
            if (value < min) return (byte)min;
            if (value > max) return (byte)max;
            return (byte)value;
        }

        static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        static byte ChannelBrigness(byte colorChannel, float brigness, bool light)
        {
            return Clamp(                
                (byte)(colorChannel * (light ? (1 + brigness) * 2 : brigness)), 
                Byte.MinValue, 
                Byte.MaxValue
            );
        }

        /// <summary>
        /// Set color brigness
        /// </summary>
        /// <param name="color"></param>
        /// <param name="brigness">range -100 : 100</param>
        /// <returns></returns>
        public static Color ColorBrigness(Color color, int brigness, bool ligth = false)
        {
            var A = color.A;

            var brig = brigness / 100.0f;

            var R = ChannelBrigness(color.R, brig, true);
            var G = ChannelBrigness(color.G, brig, true);
            var B = ChannelBrigness(color.B, brig, true);

            return ColorHelper.FromArgb(A, R, G, B);
        }

        public static double Brigness(Color color)
        {
            var R = (double)color.R;
            var G = (double)color.G;
            var B = (double)color.B;

            return Math.Sqrt(R * R * .241 + G * G * .691 + B * B * 0.068);
        }

        public static Color ColorRandom(byte min, byte max)
        {
            var colors = new rMindColors();
            var rand = colors.m_random;

            var R = (byte)rand.Next(min, max);
            var G = (byte)rand.Next(min, max);
            var B = (byte)rand.Next(min, max);

            return ColorHelper.FromArgb(255, R, G, B);
        }

        public static Color ColorRandom()
        {
            return ColorRandom(0, 255);
        }

        public static Color Deserialize(string colorString, bool randomIfEmpty = false)
        {
            if (string.IsNullOrEmpty(colorString))
            {
                if (randomIfEmpty)
                    return ColorRandom(100, 200);
                return Colors.Aquamarine;
            }

            return GetColorFromHex(colorString);
        }


        public static Color GetColorFromHex(string hex)
        {
            hex = hex.Replace("#", string.Empty);

            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));

            Color c = Color.FromArgb(a, r, g, b);
            return c;
        }
    }
}
