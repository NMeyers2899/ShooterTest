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
        private Vector2 _velocity;
        private Actor _target;
        private Vector2 _forward = new Vector2(1, 0);
        private float _maxViewAngle;
        private float _maxSightDistance;
        private int _health = 5;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public Vector2 Forward
        {
            get { return _forward; }
            set { _forward = value; }
        }
        public float Health
        {
            get { return _health; }
        }

        public Enemy(float x, float y, float speed, float maxSightDistance, float maxViewAngle,
            Actor target, string name = "Enemy", string path = "") : base(x, y, name, path)
        {
            _target = target;
            _speed = speed;
            _maxViewAngle = maxViewAngle;
            _maxSightDistance = maxSightDistance;
        }

        public override void Update(float deltaTime, Scene currentScene)
        {
            base.Update(deltaTime, currentScene);

            // Create a vector that stores the move input.
            Vector2 moveDirection = _target.LocalPosition - LocalPosition;

            Velocity = moveDirection.Normalized * Speed * deltaTime;

            if (GetTargetInSight())
            {
                LookAt(_target.LocalPosition);
                Translate(Velocity.X, Velocity.Y);
            }        
        }

        public override void Draw()
        {
            base.Draw();
            Collider.Draw();
        }

        public bool GetTargetInSight()
        {
            Vector2 directionOfTarget = (_target.LocalPosition - LocalPosition).Normalized;

            float dotProduct = Vector2.DotProduct(directionOfTarget, Forward);

            return Math.Acos(dotProduct) < _maxViewAngle;
        }

        public override void OnCollision(Actor actor, Scene currentScene)
        {
            if (actor is Bullet)
            {
                currentScene.TryRemoveActor(this);
                currentScene.TryRemoveActor(actor);
            }
        }
    }
}