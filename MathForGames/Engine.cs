using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    class Engine
    {
        private static bool _applicationShouldClose = false;
        private static int _currentSceneIndex;
        private Scene[] _scenes = new Scene[0];
        private Stopwatch _stopWatch = new Stopwatch();

        /// <summary>
        /// Called to begin the application.
        /// </summary>
        public void Run()
        {
            // Initalizes important variables for the application.
            Start();

            float currentTime = 0;
            float lastTime = 0;
            float deltaTime = 0;

            // Loop until the application is told to close.
            while (!(Raylib.WindowShouldClose() || _applicationShouldClose))
            {
                // Get how much time has passed since the application started.
                currentTime = _stopWatch.ElapsedMilliseconds / 1000.0f;

                // Set delta time to be the difference in time from the last time recorded.
                deltaTime = currentTime - lastTime;

                // Update the application.
                Update(deltaTime);
                // Draw all items.
                Draw();

                // Set the last time recorded to be the current time.
                lastTime = currentTime;
            }

            End();
        }

        /// <summary>
        /// Initalizes important variables for the application.
        /// </summary>
        private void Start()
        {
            _stopWatch.Start();

            // Creates a window using Raylib.
            Raylib.InitWindow(800, 450, "Math For Games");
            Raylib.SetTargetFPS(60);

            Scene openingScene = new Scene();
            Player player = new Player('@', 10, 10, 150, Color.RED, "Player");
            Actor actor = new Actor('E', 5, 5, Color.BLUE, "Actor");

            AddScene(openingScene);
            openingScene.AddActor(player);
            openingScene.AddActor(actor);

            _scenes[_currentSceneIndex].Start();
        }

        /// <summary>
        /// Called each time the game loops.
        /// </summary>
        private void Update(float deltaTime)
        {
            _scenes[_currentSceneIndex].Update(deltaTime);
            _scenes[_currentSceneIndex].UpdateUI(deltaTime);

            // Keeps inputs from piling up, allowing one input per update.
            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }

        /// <summary>
        /// Draws all necessary information to the screen.
        /// </summary>
        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.ORANGE);

            _scenes[_currentSceneIndex].Draw();
            _scenes[_currentSceneIndex].DrawUI();

            Raylib.EndDrawing();
        }

        /// <summary>
        /// Is called when the application is about to close.
        /// </summary>
        private void End()
        {
            _scenes[_currentSceneIndex].End();
            Raylib.CloseWindow();
        }

        /// <summary>
        /// Adds a new scene to the _scenes array.
        /// </summary>
        /// <param name="scene"> The new scene being added to the array. </param>
        /// <returns> The index that the new scene was added to. </returns>
        public int AddScene(Scene scene)
        {
            // Creates a temporary array.
            Scene[] tempArray = new Scene[_scenes.Length + 1];

            // Copies all of the old values from the array and adds them to the new array.
            for(int i = 0; i < _scenes.Length; i++)
                tempArray[i] = _scenes[i];

            // Sets the last index to be a new scene.
            tempArray[_scenes.Length] = scene;

            // Set the old array to the new array.
            _scenes = tempArray;

            // Return the last index.
            return _scenes.Length - 1;
        }

        /// <summary>
        /// Gets the next key in the input stream.
        /// </summary>
        /// <returns> Whether or not a key was pressed. </returns>
        public static ConsoleKey GetNextKey()
        {
            // If there is no key being pressed...
            if (!Console.KeyAvailable)
                // ...return.
                return 0;

            // Return the current key being pressed.
            return Console.ReadKey(true).Key;
        }

        /// <summary>
        /// Sets _applicationShouldClose to true.
        /// </summary>
        public static void CloseApplication()
        {
            _applicationShouldClose = true;
        }
    }
}
