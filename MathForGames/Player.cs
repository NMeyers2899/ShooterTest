﻿using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    class Player : Actor
    {
        private float _speed;
        private Vector2 _velocity;
        private int _health = 2;

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

        public int Health
        {
            get { return _health; }
        }

        public Player(float x, float y, float speed, string name = "Player", string path = "") 
            : base(x, y, name, path)
        {
            _speed = speed;
        }

        public override void Update(float deltaTime, Scene currentScene)
        {
            // Get the player input direction.
            int xDirection = -Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_A)) 
                + Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_D));
            int yDirection = -Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_W))
                + Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_S));

            if (Convert.ToBoolean(Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT)))
            {
                Bullet bullet = new Bullet(Position.X, Position.Y, 200, -1, 0, "Bullet", "Images/bullet.png");
                bullet.SetScale(20, 20);
                currentScene.AddActor(bullet);
            }
            if (Convert.ToBoolean(Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT)))
            {
                Bullet bullet = new Bullet(Position.X, Position.Y, 200, 1, 0, "Bullet", "Images/bullet.png");
                bullet.SetScale(20, 20);
                currentScene.AddActor(bullet);
            }
            if (Convert.ToBoolean(Raylib.IsKeyPressed(KeyboardKey.KEY_UP)))
            {
                Bullet bullet = new Bullet(Position.X, Position.Y, 200, 0, -1, "Bullet", "Images/bullet.png");
                bullet.SetScale(20, 20);
                currentScene.AddActor(bullet);
            }
            if (Convert.ToBoolean(Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN)))
            {
                Bullet bullet = new Bullet(Position.X, Position.Y, 200, 0, 1, "Bullet", "Images/bullet.png");
                bullet.SetScale(20, 20);
                currentScene.AddActor(bullet);
            }

            // Create a vector that stores the move input.
            Vector2 moveDirection = new Vector2(xDirection, yDirection);

            Velocity = moveDirection.Normalized * Speed * deltaTime;

            if (Velocity.Magnitude > 0)
                Forward = Velocity.Normalized;

            Position += Velocity;
            base.Update(deltaTime, currentScene);
        }

        public override void Draw()
        {
            base.Draw();
            Collider.Draw();
        }

        public override void OnCollision(Actor actor, Scene currentScene)
        {
            if (actor is Enemy && Health <= 0)
            {
                currentScene.TryRemoveActor(this);
                UIText deathMessage = new UIText(400, 200, "Death Message", Color.WHITE, 100, 100, 12, "You died.");
                currentScene.AddActor(deathMessage);
            }
            else if (actor is Enemy && Health > 0)
            {
                _health--;
                Position = new Vector2(700, 300);
            }
        }
    }
}
