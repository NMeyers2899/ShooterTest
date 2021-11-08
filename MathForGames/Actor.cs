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
        private Color _color;

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

        public Color ShapeColor
        {
            get { return _color; }
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

            System.Numerics.Vector3 startPos = new System.Numerics.Vector3(WorldPosition.X,
                WorldPosition.Y, WorldPosition.Z);
            System.Numerics.Vector3 endPos = new System.Numerics.Vector3(WorldPosition.X + Forward.X * 10,
                WorldPosition.Y + Forward.Y * 10, WorldPosition.Z + Forward.Z * 10);

            switch (_shape)
            {
                case Shape.CUBE:
                    Raylib.DrawCube(position, Size.X, Size.Y, Size.Z, ShapeColor);
                    break;
                case Shape.SPHERE:
                    Raylib.DrawSphere(position, Size.X, ShapeColor);
                    break;
            }

            Raylib.DrawLine3D(startPos, endPos, Color.RED);
        }

        public virtual void End()
        {

        }

        public virtual void OnCollision(Actor actor, Scene currentScene)
        {
            
        }

        /// <summary>
        /// Sets the color of the actor.
        /// </summary>
        /// <param name="color"> The color the actor is being set to. </param>
        public void SetColor(Color color)
        {
            _color = color;
        }

        /// <summary>
        /// Sets the color of the actor in a more modifiable way.
        /// </summary>
        /// <param name="colorValue"> The x is red, the y is green, the z is blue, and the w
        ///  is transparency. </param>
        public void SetColor(Vector4 colorValue)
        {
            _color = new Color((int)colorValue.X, (int)colorValue.Y, (int)colorValue.Z,
                (int)colorValue.W);
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
        /// <param name="translationZ"> The new z position. </param>
        public void SetTranslation(float translationX, float translationY, float translationZ)
        {
            _translation = Matrix4.CreateTranslation(translationX, translationY, translationZ);
        }

        /// <summary>
        /// Applies the given values to the current translation.
        /// </summary>
        /// <param name="translationX"> The amount to move on the x. </param>
        /// <param name="translationY"> The amount to move on the y. </param>
        /// <param name="translationZ"> The amount to move on the z. </param>
        public void Translate(float translationX, float translationY, float translationZ)
        {
            _translation *= Matrix4.CreateTranslation(translationX, translationY, translationZ);
        }

        /// <summary>
        /// Set the rotation of the actor.
        /// </summary>
        /// <param name="radiansX"> The angle in radians to turn on the x-axis. </param>
        /// <param name="radiansY"> The angle in radians to turn on the y-axis. </param>
        /// <param name="radiansZ"> The angle in radians to turn on the z-axis. </param>
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
        /// <param name="radiansX"> The angle in radians to turn on the x-axis. </param>
        /// <param name="radiansY"> The angle in radians to turn on the y-axis. </param>
        /// <param name="radiansZ"> The angle in radians to turn on the z-axis. </param>
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
        /// <param name="x"> The value to scale on the x-axis. </param>
        /// <param name="y"> The value to scale on the y-axis. </param>
        /// <param name="z"> The value to scale on the z-axis. </param>
        public void Scale(float x, float y, float z)
        {
            _scale *= Matrix4.CreateScale(x, y, z);
        }

        /// <summary>
        /// Rotates the actor to face the given position.
        /// </summary>
        /// <param name="position"> The position the actor should be looking towards. </param>
        public void LookAt(Vector3 position)
        {
            // Find the direction that the actor should look in.
            Vector3 direction = (position - WorldPosition).Normalized;

            // If the direction has a length of 0...
            if (direction.Magnitude == 0)
                // ...set it to be the default foward.
                direction = new Vector3(0, 0, 1);

            // Create a vector that points directly upwards.
            Vector3 alignAxis = new Vector3(0, 1, 0);

            // Creates two new vectors that will be the new x and y-axis.
            Vector3 newYAxis = new Vector3(0, 1, 0);
            Vector3 newXAxis = new Vector3(1, 0, 0);

            // If the direction vector is parallel to the alignAxis vector...
            if(Math.Abs(direction.Y) > 0 && direction.X == 0 && direction.Z == 0)
            {
                // ...set the alignAxis vector to point to the right.
                alignAxis = new Vector3(1, 0, 0);

                // Get the cross product of the direction and the right to find the new y-axis.
                newYAxis = Vector3.CrossProduct(direction, alignAxis);
                // Normalize the new y-axis to prevent the matrix from being scaled.
                newYAxis.Normalize();

                // Get the cross product of the new x-axis and find the direction to find the new 
                // x-axis.
                newXAxis = Vector3.CrossProduct(newYAxis, direction);
                // Normalize the new x-axis to prevent the matrix from being scaled.
                newXAxis.Normalize();
            }
            // If the direction vector is not parallel...
            else
            {
                // ...get the cross product of the alignAxis and the direction vector.
                newXAxis = Vector3.CrossProduct(alignAxis, direction);
                // Normalize the x-axis to prevent the matrix from being scaled.
                newXAxis.Normalize();

                // Get the cross product of the alignAxis and the direction Vector.
                newYAxis = Vector3.CrossProduct(direction, newXAxis);
                // Normalize the new y-axis to prevent the matrix from being scaled.
                newYAxis.Normalize();
            }

            // Create a new matrix with the new axis.
            _rotation = new Matrix4(newXAxis.X, newYAxis.X, direction.X, 0,
                                    newXAxis.Y, newYAxis.Y, direction.Y, 0,
                                    newXAxis.Z, newYAxis.Z, direction.Z, 0,
                                    0, 0, 0, 1);
        }
    }
}
