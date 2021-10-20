using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;

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

        public Player(char icon, float x, float y, float speed, string name = "Player", 
            ConsoleColor color = ConsoleColor.White) : base(icon, x, y, name, color)
        {
            _speed = speed;
        }

        public override void Update()
        {
            Vector2 moveDirection = new Vector2();

            // Gets the next key push.
            ConsoleKey keyPress = Engine.GetNextKey();

            // Determines which direction the player character moves in.
            if (keyPress == ConsoleKey.A || keyPress == ConsoleKey.LeftArrow)
                moveDirection = new Vector2 { X = -1 };
            else if (keyPress == ConsoleKey.D || keyPress == ConsoleKey.RightArrow)
                moveDirection = new Vector2 { X = 1 };
            else if (keyPress == ConsoleKey.W || keyPress == ConsoleKey.UpArrow)
                moveDirection = new Vector2 { Y = -1 };
            else if (keyPress == ConsoleKey.S || keyPress == ConsoleKey.DownArrow)
                moveDirection = new Vector2 { Y = 1 };

            Velocity = moveDirection * Speed;

            Position += Velocity;
        }

        public override void OnCollision(Actor actor)
        {
            
        }
    }
}
