using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    class UIText : Actor
    {
        private string _text;
        private int _width;
        private int _height;

        /// <summary>
        /// The text that will appear in the text box.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// The width of the text box. If the cursor is outside the max x position the text will wrap around.
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// The height of the text box. If the cursor is outside the max y position the text is truncated.
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Sets the starting values for the text box.
        /// </summary>
        /// <param name="x"> The x position of the text box. </param>
        /// <param name="y"> The y position of the text box. </param>
        /// <param name="name"> The name of the text box. </param>
        /// <param name="color"> The color of the text box. </param>
        /// <param name="width"> The width of the text box. </param>
        /// <param name="height"> The height of the text box. </param>
        /// <param name="text"> The text being displayed. </param>
        public UIText(float x, float y, string name, Color color, int width, int height, string text = "")
            : base('\0', x, y, color, name)
        {
            _text = text;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Displays the text box to the screen.
        /// </summary>
        public override void Draw()
        {
            // Store the position of the cursor.
            int cursorPosX = (int)Position.X;
            int cursorPosY = (int)Position.Y;

            // Create a new icon to store the current character and color.
            Icon currentLetter = new Icon { Color = Icon.Color };

            // Conver the string to text into a character array.
            char[] textChars = Text.ToCharArray();

            // Iterates through all characters in the string.
            for (int i = 0; i < textChars.Length; i++)
            {
                // Sets the icon symbol to tbe the current character in the array.
                currentLetter.Symbol = textChars[i];

                if (currentLetter.Symbol == '\n')
                {
                    cursorPosX = (int)Position.X;
                    cursorPosY++;
                    continue;
                }

                // Increment the cursor position so the letters are set side by side.
                cursorPosX++;

                // If the cursor has reached the max x position...
                if (cursorPosX > (int)Position.X + Width)
                {
                    // ...reset the cursor x position and increase the y position.
                    cursorPosX = (int)Position.X;
                    cursorPosY++;
                }

                // If the cursor has reached the maximum height...
                if (cursorPosY > (int)Position.Y + Height)
                {
                    // ...leave the loop.
                    break;
                }
            }
        }
    }
}
