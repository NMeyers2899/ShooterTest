using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    class Player : Actor
    {
        private float _speed;
        private Vector3 _velocity;
        private int _health = 2;

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

        public int Health
        {
            get { return _health; }
        }

        public Player(float x, float y, float z, float speed, string name = "Player", 
            Shape shape = Shape.CUBE) : base(x, y, z, name, shape)
        {
            _speed = speed;
        }

        public override void Update(float deltaTime, Scene currentScene)
        {
            // Get the player input direction.
            int xDirection = -Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_A)) 
                + Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_D));
            int zDirection = -Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_W))
                + Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_S));

            // Create a vector that stores the move input.
            Vector3 moveDirection = new Vector3(xDirection, 0, zDirection);

            Velocity = moveDirection.Normalized * Speed * deltaTime;

            if (Velocity.Magnitude > 0)
                Forward = Velocity.Normalized;

            LocalPosition += Velocity;
            base.Update(deltaTime, currentScene);
        }

        public override void Draw()
        {
            base.Draw();

            if(Collider != null)
                Collider.Draw();
        }

        public override void OnCollision(Actor actor, Scene currentScene)
        {

        }
    }
}
