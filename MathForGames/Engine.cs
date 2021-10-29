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

            Scene levelOne = new Scene();
            Scene levelTwo = new Scene();

            Player player = new Player(700, 300, 150, "Player", "Images/player.png");
            AABBCollider playerBoxCollider = new AABBCollider(48, 48, player);
            player.Collider = playerBoxCollider;
            player.SetScale(50, 50);

            Enemy enemy1 = new Enemy(100, 100, 120, 100, 1, player, "Enemy", "Images/enemy.png");
            AABBCollider enemy1BoxCollider = new AABBCollider(50, 50, enemy1);
            enemy1.Collider = enemy1BoxCollider;
            enemy1.SetScale(50, 50);

            Enemy enemy2 = new Enemy(200, 300, 120, 100, 1, player, "Enemy", "Images/enemy.png");
            AABBCollider enemy2BoxCollider = new AABBCollider(50, 50, enemy2);
            enemy2.Collider = enemy2BoxCollider;
            enemy2.SetScale(50, 50);

            Enemy enemy3 = new Enemy(350, 160, 120, 100, 1, player, "Enemy", "Images/enemy.png");
            AABBCollider enemy3BoxCollider = new AABBCollider(50, 50, enemy3);
            enemy3.Collider = enemy3BoxCollider;
            enemy3.SetScale(50, 50);

            Enemy enemy4 = new Enemy(10, 10, 120, 100, 1, player, "Enemy", "Images/enemy.png");
            AABBCollider enemy4BoxCollider = new AABBCollider(50, 50, enemy4);
            enemy4.Collider = enemy4BoxCollider;
            enemy4.SetScale(50, 50);

            Enemy enemy5 = new Enemy(200, 10, 120, 100, 1, player, "Enemy", "Images/enemy.png");
            AABBCollider enemy5BoxCollider = new AABBCollider(50, 50, enemy5);
            enemy5.Collider = enemy5BoxCollider;
            enemy5.SetScale(50, 50);

            AddScene(levelOne);
            levelOne.AddActor(player);
            levelOne.AddActor(enemy1);
            levelOne.AddActor(enemy2);
            levelOne.AddActor(enemy3);
            levelOne.AddActor(enemy4);
            levelOne.AddActor(enemy5);

            _scenes[_currentSceneIndex].Start();
        }

        /// <summary>
        /// Called each time the game loops.
        /// </summary>
        private void Update(float deltaTime)
        {
            _scenes[_currentSceneIndex].Update(deltaTime, _scenes[_currentSceneIndex]);
            _scenes[_currentSceneIndex].UpdateUI(deltaTime, _scenes[_currentSceneIndex]);

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
            Raylib.ClearBackground(Color.BLACK);

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

        public static void MoveToNextLevel()
        {
            _currentSceneIndex++;
        }
    }
}
