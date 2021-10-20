using System;
using System.Collections.Generic;
using System.Text;

namespace MathForGames
{
    class Scene
    {
        /// <summary>
        ///  Array that contains all actors in the scene.
        /// </summary>
        private Actor[] _actors;
        private Actor[] _UIElements;

        public Scene()
        {
            _actors = new Actor[0];
        }

        /// <summary>
        /// Calls the start for every actor within the scene.
        /// </summary>
        public virtual void Start()
        {
            // Loops through the _actors array and gets all actors in it to Start.
            for(int i = 0; i < _actors.Length; i++)
                _actors[i].Start();
        }

        /// <summary>
        /// Calls the update for every actor within the scene.
        /// </summary>
        public virtual void Update()
        {
            // Loops through the array to get each character to Update.
            for(int i = 0; i < _actors.Length; i++)
            {
                // If the actor's start function hasn't been called yet...
                if (!_actors[i].Started)
                    // ...the current actor calls its Start function.
                    _actors[i].Start();

                _actors[i].Update();

                // Goes through the actors list to see if any actor has collided.
                for (int j = 0; j < _actors.Length; j++)
                {
                    // If they have collided...
                    if(_actors[i].Position == _actors[j].Position && j != i)
                        // ...calls the OnCollision function for the actor.
                        _actors[i].OnCollision(_actors[j]);
                }
            }
        }

        public virtual void UpdateUI()
        {
            for (int i = 0; i < _UIElements.Length; i++)
            {
                if (!_UIElements[i].Started)
                {
                    _UIElements[i].Start();
                }

                _UIElements[i].Update();
            }
        }

        /// <summary>
        /// Calls the draw for every actor within the scene.
        /// </summary>
        public virtual void Draw()
        {
            // Loops through the array to get each character to Draw.
            for (int i = 0; i < _actors.Length; i++)
            {
                // If the actor's start function hasn't been called yet...
                if (!_actors[i].Started)
                    // ...the current actor calls its Start function.
                    _actors[i].Start();

                _actors[i].Draw();
            }
        }

        public virtual void DrawUI()
        {
            for (int i = 0; i < _UIElements.Length; i++)
            {
                _UIElements[i].Draw();
            }
        }

        /// <summary>
        /// Calls the end for every actor within the scene.
        /// </summary>
        public virtual void End()
        {
            // Loops through the array to get each character to End.
            for (int i = 0; i < _actors.Length; i++)
            {
                // If the actor's start function hasn't been called yet...
                if (!_actors[i].Started)
                    // ...the current actor calls its Start function.
                    _actors[i].Start();

                _actors[i].End();
            }
        }

        /// <summary>
        /// Appends a new actor to the scenes array of actors.
        /// </summary>
        /// <param name="actor"> The actor being added to the scene. </param>
        public void AddActor(Actor actor)
        {
            // Creates a temporary array.
            Actor[] tempArray = new Actor[_actors.Length + 1];

            // Copies all of the old values from the array and adds them to the new array.
            for (int i = 0; i < _actors.Length; i++)
                tempArray[i] = _actors[i];

            // Sets the last index to be a new scene.
            tempArray[_actors.Length] = actor;

            // Set the old array to the new array.
            _actors = tempArray;
        }

        /// <summary>
        /// Appends a new actor to the scenes array of actors.
        /// </summary>
        /// <param name="actor"> The actor being added to the scene. </param>
        public void AddUIElement(UIText UI)
        {
            // Creates a temporary array.
            Actor[] tempArray = new Actor[_UIElements.Length + 1];

            // Copies all of the old values from the array and adds them to the new array.
            for (int i = 0; i < _UIElements.Length; i++)
                tempArray[i] = _UIElements[i];

            // Sets the last index to be a new scene.
            tempArray[_UIElements.Length] = UI;

            // Set the old array to the new array.
            _UIElements = tempArray;
        }

        /// <summary>
        /// Checks to see if a given actor can be removed from the array of actors.
        /// </summary>
        /// <param name="actor"> The actor being removed. </param>
        /// <returns> If the actor could be removed. </returns>
        public bool TryRemoveActor(Actor actor)
        {
            // Creates a variable that helps keep track of when an actor is removed from the _actors array.
            bool actorRemoved = false;

            // Creates a temporary array.
            Actor[] tempArray = new Actor[_actors.Length - 1];

            int j = 0;

            // Copies all of the old values from the array, except for the one being removed.
            for (int i = 0; i < _actors.Length; i++)
            {
                // If the current actor in _actors is not equal to actor...
                if(_actors[i] != actor)
                {
                    // ...set the tempArray at j to the actor at i in _actors and increment j.
                    tempArray[j] = _actors[i];
                    j++;
                }
                // If the current actor in _actors is equal to actor...
                else
                    // ...set actorRemoved to true.
                    actorRemoved = true;
            }

            // If an actor has been removed...
            if (actorRemoved)
                // ...set the old array to the new array.
                _actors = tempArray;

            return actorRemoved;
        }
    }
}
