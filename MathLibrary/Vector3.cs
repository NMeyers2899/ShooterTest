using System;
using System.Collections.Generic;
using System.Text;

namespace MathLibrary
{
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float Magnitude
        {
            get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        public Vector3 Normalized
        {
            get
            {
                Vector3 value = this;
                return value.Normalize();
            }
        }

        public Vector3 Normalize()
        {
            if (Magnitude == 0)
                return new Vector3();

            return this /= Magnitude;
        }

        public static float DotProduct(Vector3 lhs, Vector3 rhs)
        {
            return (lhs.X * rhs.X) + (lhs.Y * rhs.Y) + (lhs.Z * rhs.Z);
        }

        public static float Distance(Vector3 lhs, Vector3 rhs)
        {
            return (rhs - lhs).Magnitude;
        }

        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3 { X = lhs.X + rhs.X, Y = lhs.Y + rhs.Y, Z = lhs.Z + rhs.Z };
        }

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3 { X = lhs.X - rhs.X, Y = lhs.Y - rhs.Y, Z = lhs.Z - rhs.Z };
        }

        public static Vector3 operator -(Vector3 vector)
        {
            return new Vector3 { X = -vector.X, Y = -vector.Y, Z = -vector.Z };
        }

        public static Vector3 operator *(Vector3 lhs, float rhs)
        {
            return new Vector3 { X = lhs.X * rhs, Y = lhs.Y * rhs, Z = lhs.Z * rhs };
        }

        public static Vector3 operator /(Vector3 lhs, float rhs)
        {
            return new Vector3 { X = lhs.X / rhs, Y = lhs.Y / rhs, Z = lhs.Z / rhs };
        }

        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;
        }

        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return !(lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z);
        }

        public static bool operator >=(Vector3 lhs, Vector3 rhs)
        {
            return lhs.X >= rhs.X && lhs.Y >= rhs.Y && lhs.Z >= rhs.Z;
        }

        public static bool operator <=(Vector3 lhs, Vector3 rhs)
        {
            return lhs.X <= rhs.X && lhs.Y <= rhs.Y && lhs.Z <= rhs.Z;
        }

        public static bool operator <(Vector3 lhs, float rhs)
        {
            return lhs.X < rhs && lhs.Y < rhs && lhs.Z < rhs;
        }

        public static bool operator >(Vector3 lhs, float rhs)
        {
            return lhs.X > rhs && lhs.Y > rhs && lhs.Z > rhs;
        }
    }
}
