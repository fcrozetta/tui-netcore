using System;

namespace tui_netcore.TUI
{
    public class Tui
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public char HorizontalChar { get; set; }
        public char VerticalChar { get; set; }
        public char TopLeftChar { get; set; }
        public char TopRightChar { get; set; }
        public char BottomLeftChar { get; set; }
        public char BottomRightChar { get; set; }
        public char IntersectionChar { get; set; }
        public enum ColorSchema
        {
            Regular,
            Info,
            Warning,
            Danger,
            System

        }

        public Tui (int width = 100, int height = 80)
        {
            Width = width;
            Height = height;
            HorizontalChar = '-';
            VerticalChar = '|';
            TopLeftChar = '+';
            TopRightChar = '+';
            BottomLeftChar = '+';
            BottomRightChar = '+';
            IntersectionChar = '+';
            Console.Clear();
        }

        private void setColorSchema(ColorSchema color)
        {
            switch (color)
            {
                case ColorSchema.Regular:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ColorSchema.Info:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ColorSchema.Warning:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ColorSchema.Danger:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ColorSchema.System:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
            }
        }

        /// <summary>
        /// Print the entire Screen, Use this to test your terminal
        /// </summary>
        public void TestSize(ColorSchema schema= ColorSchema.Info){
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Console.SetCursorPosition(j,i);
                    System.Console.Write("+");
                }
                Console.SetCursorPosition((Width/2)-6,i);
                setColorSchema(schema);
                System.Console.Write($"Line {i}");
                Console.ResetColor();
            }
        }

        public void MainFrame(ColorSchema schema = ColorSchema.Regular){
            setColorSchema(schema);
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if ((i == 0) || (i == Height-1) )
                    {
                        Console.SetCursorPosition(j,i);
                    }
                }
            }
        }
    }
}