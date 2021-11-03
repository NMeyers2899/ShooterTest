using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    class CircleCollider : Collider
    {
        private float _collisionRadius;

        public float CollisionRadius
        {
            get { return _collisionRadius; }
            set { _collisionRadius = value; }
        }

        public CircleCollider(float collisionRadius, Actor owner) : base(owner, ColliderType.CIRCLE)
        {
            _collisionRadius = collisionRadius;
        }

        public override void Draw()
        {
            base.Draw();
            Raylib.DrawCircleLines((int)Owner.LocalPosition.X, (int)Owner.LocalPosition.Y, CollisionRadius, Color.YELLOW);
        }

        public override bool CheckCircleCollision(CircleCollider other)
        {
            // If the owners are the same...
            if (other.Owner == Owner)
                // ...return false. 
                return false;

            // Finds the distance between the two actors.
            float distance = Vector3.Distance(other.Owner.LocalPosition, Owner.LocalPosition);
            // Finds the length of radii of the two actors combined.
            float combinedRadii = other.CollisionRadius + CollisionRadius;

            // Return whether or not the distance is less than the combined radii.
            return distance <= combinedRadii;
        }

        public override bool CheckCollisionAABB(AABBCollider other)
        {
            // If the owners are the same...
            if (other.Owner == Owner)
                // ...return false.
                return false;

            // Get the direction from this collider to the AABB.
            Vector3 direction = Owner.LocalPosition - other.Owner.LocalPosition;

            // Clamp the direction vector to be within the bounds of the AABB.
            direction.X = Math.Clamp(direction.X, -other.Width / 2, other.Width / 2);
            direction.Y = Math.Clamp(direction.Y, -other.Height / 2, other.Height / 2);

            // Add the direction vector to the AABB center to get the closest point to the circle.
            Vector3 closestPoint = other.Owner.LocalPosition + direction;

            // Find the distance from the circle's center to the closest point.
            float distanceFromClosestPoint = Vector3.Distance(Owner.LocalPosition, closestPoint);

            // Return whether or not the distance is less than the circle's radius.
            return distanceFromClosestPoint <= CollisionRadius;
        }
    }
}
