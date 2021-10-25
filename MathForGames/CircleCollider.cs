using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;

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

        public override bool CheckCircleCollision(CircleCollider other)
        {
            if (other.Owner == Owner)
                return false;

            // Finds the distance between the two actors.
            float distance = Vector2.Distance(other.Owner.Position, Owner.Position);
            // Finds the length of radii of the two actors combined.
            float combinedRadii = other.CollisionRadius + CollisionRadius;

            // Return whether or not the distance is less than the combined radii.
            return distance <= combinedRadii;
        }
    }
}
