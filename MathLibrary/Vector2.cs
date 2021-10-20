using System;

namespace MathLibrary
{
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        public float Magnitude
        {
            get { return (float)Math.Sqrt(X * X + Y * Y); }
        }

        /// <summary>
        /// Gets the normalized version of this vector without changing it.
        /// </summary>
        public Vector2 Normalized
        {
            get 
            {
                Vector2 value = this;
                return value.Normalize();
            }
        }

        /// <summary>
        /// Changes this vector to have a magnitude that is equal to one.
        /// </summary>
        /// <returns> The result of the normalization. Returns an empty vector if magnitude is zero. </returns>
        public Vector2 Normalize()
        {
            if (Magnitude == 0)
                return new Vector2();

            return this / Magnitude;
        }

        /// <summary>
        /// Adds the X of the lhs and rhs together and the Y of the lhs and rhs together.
        /// </summary>
        /// <param name="lhs"> The Vector2 on the left hand side. </param>
        /// <param name="rhs"> The Vector2 on the right hand side. </param>
        /// <returns> The sum of the two vectors. </returns>
        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2 { X = lhs.X + rhs.X, Y = lhs.Y + rhs.Y };
        }

        /// <summary>
        /// Subtracts the X of the lhs from the rhs and the Y of the lhs from the rhs.
        /// </summary>
        /// <param name="lhs"> The Vector2 on the left hand side. </param>
        /// <param name="rhs"> The Vector2 on the right hand side. </param>
        /// <returns> The difference between the two vectors. </returns>
        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2 { X = lhs.X - rhs.X, Y = lhs.Y - rhs.Y };
        }

        /// <summary>
        /// Finds the inverse of the vector.
        /// </summary>
        /// <param name="vector"> The vector that will be inversed. </param>
        /// <returns> The inverse of the vector. </returns>
        public static Vector2 operator -(Vector2 vector)
        {
            return new Vector2 { X = -vector.X, Y = -vector.Y };
        }

        /// <summary>
        ///  Multiplies the Vector2 by the scalar.
        /// </summary>
        /// <param name="lhs"> The Vector2 on the left hand side. </param>
        /// <param name="rhs"> The scalar on the right hand side. </param>
        /// <returns> The product of the vector and the scalar. </returns>
        public static Vector2 operator *(Vector2 lhs, float rhs)
        {
            return new Vector2 { X = lhs.X * rhs, Y = lhs.Y * rhs };
        }

        /// <summary>
        /// Divides the Vector2 by the scalar.
        /// </summary>
        /// <param name="lhs"> The Vector2 on the left hand side. </param>
        /// <param name="rhs"> The scalar on the right hand side. </param>
        /// <returns> The quotient of the vector and the scalar. </returns>
        public static Vector2 operator /(Vector2 lhs, float rhs)
        {
            return new Vector2 { X = lhs.X / rhs, Y = lhs.Y / rhs };
        }

        /// <summary>
        /// Finds whether or not the two vectors are equal.
        /// </summary>
        /// <param name="lhs"> The Vector2 on the left hand side. </param>
        /// <param name="rhs"> The Vector2 on the right hand side. </param>
        /// <returns> If the two vectors are equal or not. </returns>
        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        /// <summary>
        /// Finds whether or not the two vectors are not equal.
        /// </summary>
        /// <param name="lhs"> The Vector2 on the left hand side. </param>
        /// <param name="rhs"> The Vector2 on the right hand side. </param>
        /// <returns> If the two vectors are or are not equal. </returns>
        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !(lhs.X == rhs.X && lhs.Y == rhs.Y);
        }
    }
}
