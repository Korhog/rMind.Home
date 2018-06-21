using System;
using Windows.UI.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Types
{
    public struct Vector2
    {
        public double X;
        public double Y;

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2(PointerPoint point)
        {
            X = point.Position.X;
            Y = point.Position.Y;
        }

        public static Vector2 operator + (Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator *(Vector2 v, double x)
        {
            return new Vector2(v.X * x, v.Y * x);
        }

        public static Vector2 operator /(Vector2 v, double x)
        {
            return new Vector2(v.X / x, v.Y / x);
        }    

        public static double Length(Vector2 vector)
        {
            return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }        
    }
}
