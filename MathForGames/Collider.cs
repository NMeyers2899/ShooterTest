﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MathForGames
{
    public enum ColliderType
    {
        CIRCLE,
        AABB
    }

    abstract class Collider
    {
        private Actor _owner;
        private ColliderType _colliderType;

        public Actor Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public ColliderType ColliderType
        {
            get { return _colliderType; }
        }

        public Collider(Actor owner, ColliderType colliderType)
        {
            _owner = owner;
            _colliderType = colliderType;
        }

        public bool CheckForCollision(Actor other)
        {
            if (other.Collider.ColliderType == ColliderType.CIRCLE)
                return CheckCircleCollision((CircleCollider)other.Collider);

            return false;
        }

        public virtual bool CheckCircleCollision(CircleCollider other) { return false; }

        //public virtual bool CheckCollisionAABB(AABBCollider other) { return false; }
    }
}
