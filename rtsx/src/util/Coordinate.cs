using System;

namespace rtsx.src.util
{
    class Coordinate
    {
        public static Coordinate ZeroVector => new Coordinate(0, 0);

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

        public static Coordinate operator *(Coordinate A, double multiplier)
        {
            var newX = A.X * multiplier;
            var newY = A.Y * multiplier;

            return new Coordinate(newX, newY);
        }

        public static Coordinate operator /(Coordinate A, double divisor)
        {
            var newX = A.X / divisor;
            var newY = A.Y / divisor;

            return new Coordinate(newX, newY);
        }

        public override string ToString()
        {
            return String.Format("{{ {0:N3} : {1:N3} }}", X, Y);
        }

        public Coordinate Clone()
        {
            return new Coordinate(X, Y);
        }
    }
}
