using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    class Enemy : Actor
    {
        private float _speed;
        private Vector3 _velocity;
        private Actor _target;
        private float _maxViewAngle;
        private float _maxSightDistance;
        private int _health = 5;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public Vector3 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public float Health
        {
            get { return _health; }
        }

        public Enemy(float x, float y, float z, float speed, float maxSightDistance, float maxViewAngle,
            Actor target, string name = "Enemy") : base(x, y, z, name)
        {
            _target = target;
            _speed = speed;
            _maxViewAngle = maxViewAngle;
            _maxSightDistance = maxSightDistance;
            Collider = new SphereCollider(1, this);
        }

        public override void Update(float deltaTime, Scene currentScene)
        {
            base.Update(deltaTime, currentScene);

            // Create a vector that stores the move input.
            Vector3 moveDirection = _target.LocalPosition - LocalPosition;

            Velocity = moveDirection.Normalized * Speed * deltaTime;

            Translate(Velocity.X, 0, Velocity.Z);    
        }

        public override void Draw()
        {
            base.Draw();
            if(Collider != null)
                Collider.Draw();
        }

        public override void OnCollision(Actor actor, Scene currentScene)
        {
            if(actor is Bullet)
            {
                currentScene.TryRemoveActor(this);
                currentScene.TryRemoveActor(actor);
            }
        }
    }
}