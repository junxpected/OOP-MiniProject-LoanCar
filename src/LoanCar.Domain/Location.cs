using System;

namespace LoanCar.Domain
{
    public class Location
    {
        public double X { get; }
        public double Y { get; }

        public Location(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double CalculateDistanceTo(Location other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }
    }
}