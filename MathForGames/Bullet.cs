using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    class Bullet : Actor
    {
        private float _speed;
        private Vector3 _velocity;
        private Vector3 _basePosition;
        private int _timer;

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

        public Bullet(float x, float y, float z, float speed, float velocityX, float velocityZ, string name = "Bullet", 
             Shape shape = Shape.SPHERE, Actor parent = null) : base(x, y, z, name, shape, parent)
        {
            _speed = speed;
            _velocity.X = velocityX;
            _velocity.Z = velocityZ;
            Collider = new SphereCollider(0.3f, this);
            _basePosition = LocalPosition;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update(float deltaTime, Scene currentScene)
        {
            if (_timer >= 50)
                currentScene.TryRemoveActor(this);

            base.Update(deltaTime, currentScene);

            // Create a vector that stores the move input.
            Vector3 moveDirection = new Vector3(_velocity.X, _velocity.Y, _velocity.Z);

            Velocity = moveDirection.Normalized * Speed * deltaTime;

            Translate(Velocity.X, Velocity.Y, Velocity.Z);

            _timer++;
        }

        public override void Draw()
        {
            base.Draw();

            if(Collider != null)
                Collider.Draw();
        }
    }
}
