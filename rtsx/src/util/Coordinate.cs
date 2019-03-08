
using System;

namespace rtsx.src.util
{
    class Coordinate
    {
        public double X { get; }
        public double Y { get; }

        public double Abs => Math.Sqrt(X * X + Y * Y);

        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Coordinate operator +(Coordinate A, Coordinate B)
        {
            var newX = A.X + B.X;
            var newY = A.Y + B.Y;

            return new Coordinate(newX, newY);
        }

        public static Coordinate operator -(Coordinate A, Coordinate B)
        {
            var newX = A.X - B.X;
            var newY = A.Y - B.Y;

            return new Coordinate(newX, newY);
        }
    }
}
