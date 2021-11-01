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
        private Vector2 _forward;
        private Matrix3 _localTransform = Matrix3.Identity;
        private Matrix3 _globalTransform = Matrix3.Identity;
        private Matrix3 _translation = Matrix3.Identity;
        private Matrix3 _rotation = Matrix3.Identity;
        private Matrix3 _scale = Matrix3.Identity;
        private Actor[] _children = new Actor[0];
        private Actor _parent;
        private Sprite _sprite;

        /// <summary>
        /// True if the start function has been called for this actor.
        /// </summary>
        public bool Started
        {
            get { return _started; }
        }

        public Vector2 LocalPosition
        {
            get { return new Vector2(_translation.M02, _translation.M12); }
            set { SetTranslation(value.X, value.Y); }
        }

        public Vector2 WorldPosition
        {
            get; set;
        }

        public Matrix3 GlobalTransform
        {
            get; set;
        }

        public Matrix3 LocalTransforms
        {
            get; set;
        }

        public Actor Parent
        {
            get; set;
        }

        public Actor[] Children
        {
            get;
        }

        public Vector2 Size
        {
            get { return new Vector2(_scale.M00, _scale.M11); }
            set { SetScale(value.X, value.Y); }
        }

        public string Name
        {
            get { return _name; }
        }

        public Vector2 Forward
        {
            get { return new Vector2(_rotation.M00, _rotation.M10); }
            set 
            { 
                Vector2 point = value.Normalized + LocalPosition;
                LookAt(point);
            }
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
            SetTranslation(position.X, position.Y);
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
            _localTransform = _translation * _rotation * _scale;
            Console.WriteLine(Name + LocalPosition.X + " , " + LocalPosition.Y);
        }

        public virtual void Draw()
        {
            if(_sprite != null)
                _sprite.Draw(_localTransform);
        }

        public virtual void End()
        {

        }

        public virtual void OnCollision(Actor actor, Scene currentScene)
        {

        }

        public void UpdateTransforms()
        {

        }

        public void AddChild(Actor child)
        {
            Actor[] tempArray = new Actor[_children.Length];

            for (int i = 0; i <= _children.Length; i++)
                tempArray[i] = _children[i];

            tempArray[_children.Length] = child;

            _children = tempArray;
        }

        public bool RemoveChild(Actor child)
        {
            Actor[] tempArray = new Actor[_children.Length - 1];

            int j = 0;
            bool childRemoved = false;

            for (int i = 0; i <= _children.Length; i++)
            {
                if (_children[i].Name != child.Name || childRemoved)
                {
                    tempArray[j] = _children[i];
                    j++;
                }
                else
                    childRemoved = true;
            }

            _children = tempArray;

            return childRemoved;
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

        /// <summary>
        /// Sets the position of the actor.
        /// </summary>
        /// <param name="translationX"> The new x position. </param>
        /// <param name="translationY"> The new y position. </param>
        public void SetTranslation(float translationX, float translationY)
        {
            _translation = Matrix3.CreateTranslation(translationX, translationY);
        }

        /// <summary>
        /// Applies the given values to the current translation.
        /// </summary>
        /// <param name="translationX"> The amount to move on the x. </param>
        /// <param name="translationY"> The amount to move on the y. </param>
        public void Translate(float translationX, float translationY)
        {
            _translation *= Matrix3.CreateTranslation(translationX, translationY);
        }

        /// <summary>
        /// Set the rotation of the actor.
        /// </summary>
        /// <param name="radians"> The angle of the new rotation in radians. </param>
        public void SetRotation(float radians)
        {
            _rotation = Matrix3.CreateRotation(radians);
        }

        /// <summary>
        /// Adds a roation to the current transform's rotation.
        /// </summary>
        /// <param name="radians"> The angle in radians to turn. </param>
        public void Rotate(float radians)
        {
            _rotation *= Matrix3.CreateRotation(radians);
        }

        public void SetScale(float x, float y)
        {
            _scale = Matrix3.CreateScale(x, y);
        }

        /// <summary>
        /// Scales the actor by the given amount.
        /// </summary>
        /// <param name="x"> The value to scale on the x axis. </param>
        /// <param name="y"> The value to scale on the y axis. </param>
        public void Scale(float x, float y)
        {
            _scale *= Matrix3.CreateScale(x, y);
        }

        /// <summary>
        /// Rotates the actor to face the given position.
        /// </summary>
        /// <param name="position"> The position the actor should be looking towards. </param>
        public void LookAt(Vector2 position)
        {
            // Find the direction that the actor should look in.
            Vector2 direction = (position - LocalPosition).Normalized;

            // Use the dot product to find the angle the actor needs to rotate.
            float dotProduct = Vector2.DotProduct(direction, Forward);

            if (dotProduct > 1)
                dotProduct = 1;
            
            float angle = (float)Math.Acos(dotProduct);

            // Find the perpindiculat vector to the direction.
            Vector2 perpDirection = new Vector2(direction.Y, -direction.X);

            // Find the dot product of the perpindicular vector and the current forward.
            float perpDot = Vector2.DotProduct(perpDirection, Forward);

            // If the result is not zero...
            if (perpDot != 0)
                // ...use it to change the sign of the angle to be either positive or negative.
                angle *= -perpDot / Math.Abs(perpDot);

            Rotate(angle);
        }
    }
}
