using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    public enum Shape
    {
        CUBE,
        SPHERE,

    }

    class Actor
    {
        private string _name;
        private bool _started;
        private Collider _collider;
        private Vector3 _forward = new Vector3(0, 0, 1);
        private Matrix4 _localTransform = Matrix4.Identity;
        private Matrix4 _globalTransform = Matrix4.Identity;
        private Matrix4 _translation = Matrix4.Identity;
        private Matrix4 _rotation = Matrix4.Identity;
        private Matrix4 _scale = Matrix4.Identity;
        private Actor[] _children = new Actor[0];
        private Actor _parent;
        private Shape _shape;

        /// <summary>
        /// True if the start function has been called for this actor.
        /// </summary>
        public bool Started
        {
            get { return _started; }
        }

        public Vector3 LocalPosition
        {
            get { return new Vector3(_translation.M03, _translation.M13, _translation.M23); }
            set { SetTranslation(value.X, value.Y, value.Z); }
        }

        /// <summary>
        /// The position of this actor in the world.
        /// </summary>
        public Vector3 WorldPosition
        {
            // Return the global transform's T column.
            get { return new Vector3(_globalTransform.M03, _globalTransform.M13, _globalTransform.M23); }
            set {
                // If the actor has a parent...
                if (Parent != null)
                {
                    // ...convert the world coordinates into local coordinates and translate the actor.
                    float xOffset = (value.X - Parent.WorldPosition.X) / new Vector3(_globalTransform.M00, _globalTransform.M10, _globalTransform.M20).Magnitude;
                    float yOffset = (value.Y - Parent.WorldPosition.Y) / new Vector3(_globalTransform.M10, _globalTransform.M11, _globalTransform.M21).Magnitude;
                    float zOffset = (value.Z - Parent.WorldPosition.Z) / new Vector3(_globalTransform.M20, _globalTransform.M21, _globalTransform.M22).Magnitude;
                    SetTranslation(xOffset, yOffset, zOffset);
                }
                // If this actor does not have a parent...
                else
                    // ...set local position to be the given value.
                    LocalPosition = value;
            }
        }

        public Matrix4 GlobalTransform
        {
            get { return _globalTransform; }
            private set { _globalTransform = value; }
        }

        public Matrix4 LocalTransform
        {
            get { return _localTransform; }
            private set { _localTransform = value; }
        }

        public Actor Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public Actor[] Children
        {
            get { return _children; }
        }

        public Vector3 Size
        {
            get 
            {
                float xScale = new Vector3(_scale.M00, _scale.M10, _scale.M20).Magnitude;
                float yScale = new Vector3(_scale.M01, _scale.M11, _scale.M21).Magnitude;
                float zScale = new Vector3(_scale.M02, _scale.M12, _scale.M22).Magnitude;
                return new Vector3(xScale, yScale, zScale); 
            }
            set { SetScale(value.X, value.Y, value.Z); }
        }

        public string Name
        {
            get { return _name; }
        }

        public Vector3 Forward
        {
            get { return new Vector3(_rotation.M02, _rotation.M12, _rotation.M22); }
            set { _forward = value; }
        }

        public Collider Collider
        {
            get { return _collider; }
            set { _collider = value; }
        }

        public Actor(Vector3 position, string name = "Actor", Shape shape = Shape.CUBE, Actor parent = null)
        {
            SetTranslation(position.X, position.Y, position.Z);
            _name = name;
            _shape = shape;

            if (parent != null)
                _parent = parent;
        }

        public Actor(float x, float y, float z, string name = "Actor", Shape shape = Shape.CUBE,
            Actor parent = null) : this(new Vector3 { X = x, Y = y, Z = z }, name, shape, parent) { }

        public Actor() { }

        public virtual void Start()
        {
            _started = true;
        }

        public virtual void Update(float deltaTime, Scene currentScene)
        {
            Console.WriteLine(Name + LocalPosition.X + " , " + LocalPosition.Y);

            UpdateTransforms();
        }

        public virtual void Draw()
        {
            System.Numerics.Vector3 position = new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y,
                WorldPosition.Z);

            switch (_shape)
            {
                case Shape.CUBE:
                    Raylib.DrawCube(position, Size.X, Size.Y, Size.Z, Color.BLACK);
                    break;
                case Shape.SPHERE:
                    Raylib.DrawSphere(position, Size.X, Color.BLACK);
                    break;
            }
        }

        public virtual void End()
        {

        }

        public virtual void OnCollision(Actor actor, Scene currentScene)
        {

        }

        public void UpdateTransforms()
        {
            _localTransform = _translation * _rotation * _scale;

            if (_parent != null)
                GlobalTransform = _parent.GlobalTransform * LocalTransform;
            else
                GlobalTransform = LocalTransform;
        }

        public void AddChild(Actor child)
        {
            Actor[] tempArray = new Actor[_children.Length + 1];

            for (int i = 0; i < _children.Length; i++)
                tempArray[i] = _children[i];

            tempArray[_children.Length] = child;

            _children = tempArray;

            child.Parent = this;
        }

        public bool RemoveChild(Actor child)
        {
            Actor[] tempArray = new Actor[_children.Length - 1];

            int j = 0;
            bool childRemoved = false;

            for (int i = 0; i < _children.Length; i++)
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

            if (childRemoved)
                child.Parent = null;

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
        public void SetTranslation(float translationX, float translationY, float translationZ)
        {
            _translation = Matrix4.CreateTranslation(translationX, translationY, translationZ);
        }

        /// <summary>
        /// Applies the given values to the current translation.
        /// </summary>
        /// <param name="translationX"> The amount to move on the x. </param>
        /// <param name="translationY"> The amount to move on the y. </param>
        public void Translate(float translationX, float translationY, float translationZ)
        {
            _translation *= Matrix4.CreateTranslation(translationX, translationY, translationZ);
        }

        /// <summary>
        /// Set the rotation of the actor.
        /// </summary>
        /// <param name="radians"> The angle of the new rotation in radians. </param>
        public void SetRotation(float radiansX, float radiansY, float radiansZ)
        {
            Matrix4 rotationX = Matrix4.CreateRotationX(radiansX);
            Matrix4 rotationY = Matrix4.CreateRotationY(radiansY);
            Matrix4 rotationZ = Matrix4.CreateRotationZ(radiansZ);
            _rotation = rotationX * rotationY * rotationZ;
        }

        /// <summary>
        /// Adds a roation to the current transform's rotation.
        /// </summary>
        /// <param name="radians"> The angle in radians to turn. </param>
        public void Rotate(float radiansX, float radiansY, float radiansZ)
        {
            Matrix4 rotationX = Matrix4.CreateRotationX(radiansX);
            Matrix4 rotationY = Matrix4.CreateRotationY(radiansY);
            Matrix4 rotationZ = Matrix4.CreateRotationZ(radiansZ);
            _rotation *= rotationX * rotationY * rotationZ;
        }

        public void SetScale(float x, float y, float z)
        {
            _scale = Matrix4.CreateScale(x, y, z);
        }

        /// <summary>
        /// Scales the actor by the given amount.
        /// </summary>
        /// <param name="x"> The value to scale on the x axis. </param>
        /// <param name="y"> The value to scale on the y axis. </param>
        public void Scale(float x, float y, float z)
        {
            _scale *= Matrix4.CreateScale(x, y, z);
        }

        /// <summary>
        /// Rotates the actor to face the given position.
        /// </summary>
        /// <param name="position"> The position the actor should be looking towards. </param>
        //public void LookAt(Vector3 position)
        //{
        //    // Find the direction that the actor should look in.
        //    Vector3 direction = (position - LocalPosition).Normalized;

        //    // Use the dot product to find the angle the actor needs to rotate.
        //    float dotProduct = Vector3.DotProduct(direction, Forward);

        //    if (dotProduct > 1)
        //        dotProduct = 1;
            
        //    float angle = (float)Math.Acos(dotProduct);

        //    // Find the perpindicular vector to the direction.
        //    Vector3 perpDirection = new Vector3(direction.Y, -direction.X, direction.Z);

        //    // Find the dot product of the perpindicular vector and the current forward.
        //    float perpDot = Vector3.DotProduct(perpDirection, Forward);

        //    // If the result is not zero...
        //    if (perpDot != 0)
        //        // ...use it to change the sign of the angle to be either positive or negative.
        //        angle *= -perpDot / Math.Abs(perpDot);

        //    Rotate(angle, angle, angle);
        //}
    }
}
