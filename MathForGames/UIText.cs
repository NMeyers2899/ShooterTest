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
        private int _fontSize;
        private Font _font;
        public Color FontColor;

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
        /// The height of the text box. If the cursor is outside the max y position the text is truncated.
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
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
        public UIText(float x, float y, string name, Color color, int width, int height, int fontSize, 
            string text = "") : base(x, y, name, "")
        {
            _text = text;
            _width = width;
            _height = height;

            _font = Raylib.LoadFont("resources/fonts/alagard.png");
            _fontSize = fontSize;
            FontColor = color;
        }

        /// <summary>
        /// Displays the text box to the screen.
        /// </summary>
        public override void Draw()
        {
            // Creates a new rectangle that will act as the borders for the text.
            Rectangle textBox = new Rectangle(LocalPosition.X, LocalPosition.Y, Width, Height);
            // Draws the text box for the text.
            Raylib.DrawTextRec(_font, Text, textBox, FontSize, 1, true, Color.WHITE);
        }
    }
}
