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
            int yDirection = Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_SPACE));

            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
                Speed *= 2;

            if (Convert.ToBoolean(Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT)))
            {
                Bullet bullet = new Bullet(LocalPosition.X, LocalPosition.Y, LocalPosition.Z,
                    20, -1, 0, "Bullet");
                bullet.SetScale(0.3f, 0.2f, 0.3f);
                currentScene.AddActor(bullet);
            }
            if (Convert.ToBoolean(Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT)))
            {
                Bullet bullet = new Bullet(LocalPosition.X, LocalPosition.Y, LocalPosition.Z,
                    20, 1, 0, "Bullet");
                bullet.SetScale(0.3f, 0.2f, 0.3f);
                currentScene.AddActor(bullet);
            }
            if (Convert.ToBoolean(Raylib.IsKeyPressed(KeyboardKey.KEY_UP)))
            {
                Bullet bullet = new Bullet(LocalPosition.X, LocalPosition.Y, LocalPosition.Z,
                    20, 0, -1, "Bullet");
                bullet.SetScale(0.3f, 0.2f, 0.3f);
                currentScene.AddActor(bullet);
            }
            if (Convert.ToBoolean(Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN)))
            {
                Bullet bullet = new Bullet(LocalPosition.X, LocalPosition.Y, LocalPosition.Z,
                    20, 0, 1, "Bullet");
                bullet.SetScale(0.3f, 0.2f, 0.3f);
                currentScene.AddActor(bullet);
            }

            // Create a vector that stores the move input.
            Vector3 moveDirection = new Vector3(xDirection, yDirection, zDirection);

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
            if (actor is Enemy)
                currentScene.TryRemoveActor(this);
        }
    }
}
