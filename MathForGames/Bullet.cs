﻿using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    class Bullet : Actor
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

        public Bullet(char icon, float x, float y, float speed, float velocityX, float velocityY,
            Color color, string name = "Bullet") : base(icon, x, y, color, name)
        {
            _speed = speed;
            _velocity.X = velocityX;
            _velocity.Y = velocityY;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update(float deltaTime, Scene currentScene)
        {
            base.Update(deltaTime, currentScene);

            // Create a vector that stores the move input.
            Vector2 moveDirection = new Vector2(_velocity.X, _velocity.Y);

            Velocity = moveDirection.Normalized * Speed * deltaTime;

            Position += Velocity;
        }
    }
}