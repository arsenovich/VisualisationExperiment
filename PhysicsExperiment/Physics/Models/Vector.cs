using System;

namespace PhysicsExperiment.Physics.Models
{
    public struct Vector
    {
        public double X { get; }

        public double Y { get; }

        public double R => Math.Sqrt(X * X + Y * Y);

        public static Vector operator + (Vector a, Vector b) => new Vector(a.X + b.X, a.Y + b.Y);
        
        public static Vector operator * (Vector a, double scalar) => new Vector(a.X * scalar, a.Y * scalar);

        public static Vector operator - (Vector a, Vector b) => a + b * (-1.0);

        public static Vector operator / (Vector a, double scalar) => a * (1.0 / scalar);

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
