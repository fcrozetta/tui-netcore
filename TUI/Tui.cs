using System;
using System.Collections.Generic;
using System.Text;

namespace tui_netcore.TUI
{
    public class Tui
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int PosLeft { get; set; }
        public int PosTop { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public char AnswerChar { get; set; }
        public char HorizontalChar { get; set; }
        public char VerticalChar { get; set; }
        public char TopLeftChar { get; set; }
        public char TopRightChar { get; set; }
        public char BottomLeftChar { get; set; }
        public char BottomRightChar { get; set; }
        public char IntersectionChar { get; set; }
        public char EmptyChar { get; set; }
        public int MarginLeft { get; set; }
        public int MarginTop { get; set; }

        public enum ColorSchema
        {
            Regular,
            Info,
            Warning,
            Danger,
            System

        }


        /// <summary>
        /// This is the object used for each option in the checkbox and/or radio options
        /// </summary>
        public struct CheckRadioOption
        {
            bool isSelected;
            string name;
            string description;
        }

        public Tui (int width = 100, int height = 80,int posLeft=0,int posTop=0)
        {
            Width = width;
            Height = height;
            PosTop = 0;
            PosLeft = 0;
            AnswerChar= '>';
            HorizontalChar = '-';
            VerticalChar = '|';
            TopLeftChar = '+';
            TopRightChar = '+';
            BottomLeftChar = '+';
            BottomRightChar = '+';
            IntersectionChar = '+';
            EmptyChar = ' ';
            Title = null;
            Body = null;
            MarginLeft = 4;
            MarginTop = 2;
        }

        /// <summary>
        /// Defines a Color schema to the box
        /// </summary>
        /// <param name="color"></param>
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

        private void drawTitle()
        {
            Console.SetCursorPosition((Width / 2) - (Title.Length / 2), PosTop);
            Console.Write(Title);
        }

        private void drawBody()
        {
            int usableSpace = Width - (2 * MarginLeft);
            if (Body.Length <= usableSpace)
            {
                Console.SetCursorPosition((PosLeft + MarginLeft), (PosTop + MarginTop));
                System.Console.Write(Body);
            }
            else
            {
                //* TODO: Improve the way the split is done to stay inside the box
                //! TODO: "\n" Breaks the layout. Find a way to make this work correctly
                
                int tmpLine = 0;
                StringBuilder tmpText = new StringBuilder(usableSpace, usableSpace);
                string[] tmpBody = Body.Split(" ");
                foreach (string s in tmpBody)
                {
                    try
                    {
                        tmpText.Append(s);
                        tmpText.Append(" ");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.SetCursorPosition((PosLeft + MarginLeft), (PosTop + MarginTop + tmpLine));
                        System.Console.Write(tmpText);
                        tmpLine++;
                        tmpText.Clear();
                    }
                }
                Console.SetCursorPosition((PosLeft + MarginLeft), (PosTop + MarginTop + tmpLine));
                System.Console.Write(tmpText);
            }

        }

        /// <summary>
        /// This Frame wraps the entire screen, printing spaces inside.
        /// </summary>
        /// <param name="schema">Color Scheme to be used</param>
        public void Draw(ColorSchema schema = ColorSchema.Regular){
            setColorSchema(schema);
            for (int i = PosTop; i < Height; i++)
            {
                for (int j = PosLeft; j < Width; j++)
                {
                    char ToPrint = ' ';

                    if ((i == 0) || (i == Height-1))
                    {
                        ToPrint = HorizontalChar;
                    }else if((j == 0) || (j==Width-1)){
                        ToPrint = VerticalChar;
                    }else
                    {
                        ToPrint = EmptyChar;
                    }
                    Console.SetCursorPosition(j,i);
                    Console.Write(ToPrint);
                }
            }
            if (Title != null) drawTitle();
            if (Body != null) drawBody();
            setColorSchema(ColorSchema.Regular);
        }

        /// <summary>
        /// This method Creates an input so the user can answer things
        /// </summary>
        /// <param name="schema"></param>
        /// <returns>string containing the user answer</returns>
        public string DrawInput(ColorSchema schema = ColorSchema.Regular){
            Draw(schema);
            setColorSchema(schema);
            Console.SetCursorPosition((PosLeft + MarginLeft),(PosTop + Height - MarginTop));
            Console.Write($"{AnswerChar} ");
            string answer = Console.ReadLine();
            setColorSchema(ColorSchema.Regular);
            return answer;
        }

        /// <summary>
        /// Draws a box with two options to answer (Yes/No)
        /// Left/Right arrows to move between options
        /// Enter to select option
        /// </summary>
        /// <param name="schema">Color Schema</param>
        /// <param name="txtYes">Text to Yes option</param>
        /// <param name="txtNo">Text to No option</param>
        /// <param name="defaultAnswer">true=yes/false=no</param>
        /// <returns>true=yes/false=no</returns>
        public bool DrawYesNo(ColorSchema schema = ColorSchema.Regular,string txtYes = "Yes", string txtNo = "No", bool defaultAnswer = false){
            Draw(schema);
            setColorSchema(schema);
            bool answer = defaultAnswer;
            int CursorYes = PosLeft + MarginLeft;
            int CursorNo = PosLeft + Width - (2*MarginLeft) ;
            int Line = (PosTop + Height - MarginTop);

            ConsoleKeyInfo keypress;
            do
            {
                Console.SetCursorPosition(CursorYes + 1, Line);
                Console.Write(txtYes);
                Console.SetCursorPosition(CursorNo + 1, Line);
                Console.Write(txtNo);

                Console.SetCursorPosition(CursorYes, Line);
                Console.Write(answer?AnswerChar:EmptyChar);
                Console.SetCursorPosition(CursorNo, Line);
                Console.Write(!answer ? AnswerChar : EmptyChar);
                keypress = Console.ReadKey(false);

                if ((keypress.Key == ConsoleKey.LeftArrow) || (keypress.Key == ConsoleKey.RightArrow))
                {
                    answer = !answer;
                }
                
            } while (keypress.Key != ConsoleKey.Enter);
            setColorSchema(ColorSchema.Regular);
            return answer;

        }

        /// <summary>
        /// This method draws the box, and wait the user press any key to continue
        /// </summary>
        /// <param name="schema">Color schema</param>
        /// <param name="txtOk">Text to appear on the bottom of the box</param>
        public void DrawOk(ColorSchema schema = ColorSchema.Regular,string txtOk = "Press any key to continue..."){
            Draw(schema);
            setColorSchema(schema);
            int CursorOk = PosLeft + MarginLeft;
            int Line = (PosTop + Height - MarginTop);
            Console.SetCursorPosition(CursorOk, Line);
            Console.Write($"{AnswerChar}{txtOk}");
            Console.ReadKey();
        }

        public List<string> DrawCheckBox(List<CheckRadioOption> options, ColorSchema schema = ColorSchema.Regular){
            //Continue Here
        }




    }
}