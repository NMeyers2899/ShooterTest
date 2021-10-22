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

        public Player(char icon, float x, float y, float speed, Color color, string name = "Player") 
            : base(icon, x, y, color, name)
        {
            _speed = speed;
            CollisionRadius = 5;
        }

        public override void Update(float deltaTime, Scene currentScene)
        {
            base.Update(deltaTime, currentScene);

            // Get the player input direction.
            int xDirection = -Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_A)) 
                + Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_D));
            int yDirection = -Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_W))
                + Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_S));

            if (Convert.ToBoolean(Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT)))
            {
                Bullet bullet = new Bullet('.', Position.X, Position.Y, 200, -1, 0, Color.WHITE);
                currentScene.AddActor(bullet);
            }
            else if (Convert.ToBoolean(Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT)))
            {
                Bullet bullet = new Bullet('.', Position.X, Position.Y, 200, 1, 0, Color.WHITE);
                currentScene.AddActor(bullet);
            }
            else if (Convert.ToBoolean(Raylib.IsKeyPressed(KeyboardKey.KEY_UP)))
            {
                Bullet bullet = new Bullet('.', Position.X, Position.Y, 200, 0, -1, Color.WHITE);
                currentScene.AddActor(bullet);
            }
            else if (Convert.ToBoolean(Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN)))
            {
                Bullet bullet = new Bullet('.', Position.X, Position.Y, 200, 0, 1, Color.WHITE);
                currentScene.AddActor(bullet);
            }

            // Create a vector that stores the move input.
            Vector2 moveDirection = new Vector2(xDirection, yDirection);

            Velocity = moveDirection.Normalized * Speed * deltaTime;

            Position += Velocity;
        }

        public override void OnCollision(Actor actor, Scene currentScene)
        {
            Console.WriteLine("Collision Occured");
        }
    }
}
