using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;

namespace MathForGames
{
    class AABBCollider : Collider
    {
        private float _width;
        private float _height;

        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public float Left
        {
            get { return Owner.Position.X - (Width / 2); }
        }

        public float Right
        {
            get  { return Owner.Position.X + (Width / 2); }
        }

        public float Top
        {
            get  {  return Owner.Position.Y - (Width / 2); }
        }

        public float Bottom
        {
            get  { return Owner.Position.Y - (Width / 2); }
        }

        public AABBCollider(float width, float height, Actor owner) : base(owner, ColliderType.AABB)
        {
            _width = width;
            _height = height;
        }
    }
}
