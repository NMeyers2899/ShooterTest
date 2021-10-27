using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    class Actor
    {
        private string _name;
        private bool _started;
        private Collider _collider;
        private Vector2 _forward = new Vector2(1, 0);
        private Matrix3 _transform = Matrix3.Identity;
        private Matrix3 _translation = Matrix3.Identity;
        private Matrix3 _rotation = Matrix3.Identity;
        private Matrix3 _scale = Matrix3.Identity;
        private Sprite _sprite;

        /// <summary>
        /// True if the start function has been called for this actor.
        /// </summary>
        public bool Started
        {
            get { return _started; }
        }

        public Vector2 Position
        {
            get { return new Vector2(_transform.M02, _transform.M12); }
            set
            {
                _transform.M02 = value.X;
                _transform.M12 = value.Y;
            }
        }

        public string Name
        {
            get { return _name; }
        }

        public Collider Collider
        {
            get { return _collider; }
            set { _collider = value; }
        }

        public Sprite Sprite
        {
            get { return _sprite; }
            set { _sprite = value; }
        }

        public Actor(Vector2 position, string name = "Actor", string path = "")
        {
            Position = position;
            _name = name;

            if (path != "")
                _sprite = new Sprite(path);
        }

        public Actor(float x, float y, string name = "Actor", string path = "")
            : this(new Vector2 { X = x, Y = y }, name, path) { }

        public Actor() { }

        public virtual void Start()
        {
            _started = true;
        }

        public virtual void Update(float deltaTime, Scene currentScene)
        {
            _transform = _translation * _rotation * _scale;
            Console.WriteLine(Name + Position.X + " , " + Position.Y);
        }

        public virtual void Draw()
        {
            if(_sprite != null)
                _sprite.Draw(_transform);
        }

        public virtual void End()
        {

        }

        public virtual void OnCollision(Actor actor, Scene currentScene)
        {

        }

        /// <summary>
        /// Checks if this actor has collided with another actor.
        /// </summary>
        /// <param name="other"> The actor being collided with. </param>
        /// <returns> Whether or not the distance between the two is less than the combined radii. </returns>
        public virtual bool CheckForCollision(Actor other)
        {
            // If this actor or the other actor do not have a collider...
            if (Collider == null || other.Collider == null)
                // ...return false.
                return false;

            return Collider.CheckForCollision(other);
        }

        public void SetScale(float x, float y)
        {
            _transform.M00 = x;
            _transform.M11 = y;
        }
    }
}
