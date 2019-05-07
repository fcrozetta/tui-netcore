using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace tui_netcore
{
    public class Tui
    {
        // Box Width
        public int Width { get; set; }
        // Box Height
        public int Height { get; set; }

        // Position (left) of the first character of the box inside the terminal
        public int PosLeft { get; set; }
        // Position (top) of the first character of the box inside the terminal
        public int PosTop { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        //Character Used to navigate, answer strings and select an option 
        public char AnswerChar { get; set; }
        public char SelectedChar { get; set; }


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

        // * This is used to draw multiple lines below the body (checkbox/radio)
        public int LastBodyHeight { get; set; }

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
        public struct CheckBoxOption
        {
            public bool IsSelected { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public Tui (int width = 100, int height = 80,int posLeft=0,int posTop=0)
        {
            Width = width;
            Height = height;
            PosTop = posTop;
            PosLeft = posLeft;
            AnswerChar= '>';
            SelectedChar = '*';
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
            LastBodyHeight = 0;
        }

        public Tui(){
            Width = Console.WindowWidth;
            Height = Console.WindowHeight;
            PosTop = 0;
            PosLeft = 0;
            AnswerChar= '>';
            SelectedChar = '*';
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
            LastBodyHeight = 0;
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
            //* TODO: Improve the way the split is done to stay inside the box
            //! TODO: "\n" Breaks the layout. Find a way to make this work correctly
            
            int tmpLine = 0;
            StringBuilder tmpText = new StringBuilder(usableSpace, usableSpace);
            string[] tmpBody = Body.Split(' ');
            foreach (string s in tmpBody)
            {
                try
                {
                    if (s == "\n")
                    {
                        throw new ArgumentOutOfRangeException();
                    }
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
            LastBodyHeight = PosTop + MarginTop + tmpLine;
            System.Console.Write(tmpText);
        }

        private void DrawCorners()
        {
            Console.SetCursorPosition(PosLeft,PosTop);
            Console.Write(TopLeftChar);

            Console.SetCursorPosition(PosLeft + Width -1, PosTop);
            System.Console.Write(TopRightChar);

            Console.SetCursorPosition(PosLeft, PosTop + Height -1);
            System.Console.Write(BottomLeftChar);

            Console.SetCursorPosition(PosLeft + Width -1 , PosTop + Height -1);
            System.Console.Write(BottomRightChar);
        }


        /// <summary>
        /// This Frame wraps the entire screen, printing spaces inside.
        /// </summary>
        /// <param name="schema">Color Scheme to be used</param>
        public void Draw(ColorSchema schema = ColorSchema.Regular){
            setColorSchema(schema);
            for (int i = PosTop; i < Height+PosTop; i++)
            {
                for (int j = PosLeft; j < Width+PosLeft; j++)
                {
                    char ToPrint = ' ';

                    if ((i == 0 + PosTop) || (i == PosTop + Height-1))
                    {
                        ToPrint = HorizontalChar;
                    }else if((j == 0 + PosLeft) || (j==PosLeft + Width-1)){
                        ToPrint = VerticalChar;
                    }else
                    {
                        ToPrint = EmptyChar;
                    }
                    Console.SetCursorPosition(j,i);
                    Console.Write(ToPrint);
                }
            }
            DrawCorners();
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

        public List<CheckBoxOption> DrawCheckBox(List<CheckBoxOption> options, ColorSchema schema = ColorSchema.Regular, bool onlyChecked = true){
            Draw(schema);
            setColorSchema(schema);
            int Line = LastBodyHeight + MarginTop;
            int tmpCursor = 0;
            List<CheckBoxOption> TmpOptions = options;
            //Continue Here
            foreach (CheckBoxOption o in TmpOptions)
            {
                char tmpSelected = o.IsSelected ? SelectedChar : EmptyChar;
                Console.SetCursorPosition(MarginLeft + PosLeft, Line);
                System.Console.Write($" [{tmpSelected}] {o.Name} - {o.Description}");
                Line++;
            }

            Line -= options.Count;

            ConsoleKeyInfo keypress;
            do
            {
                
                Console.SetCursorPosition(MarginLeft + PosLeft, Line + tmpCursor);
                Console.Write(AnswerChar);
                keypress = Console.ReadKey(true);
                if (keypress.Key == ConsoleKey.UpArrow)
                {
                    if (tmpCursor > 0)
                    {
                        Console.SetCursorPosition(MarginLeft + PosLeft, Line + tmpCursor);
                        Console.Write(EmptyChar);
                        tmpCursor--;
                    }
                }
                if (keypress.Key == ConsoleKey.DownArrow)
                {
                    if (tmpCursor < TmpOptions.Count-1)
                    {
                        Console.SetCursorPosition(MarginLeft + PosLeft, Line + tmpCursor);
                        Console.Write(EmptyChar);
                        tmpCursor++;
                    }
                }
                if (keypress.Key == ConsoleKey.Spacebar)
                {
                    CheckBoxOption c = TmpOptions[tmpCursor];
                    c.IsSelected = !c.IsSelected;
                    char ch = c.IsSelected?SelectedChar:EmptyChar;
                    Console.SetCursorPosition(MarginLeft + 2 + PosLeft, Line + tmpCursor);
                    TmpOptions[tmpCursor] = c;
                    Console.Write(ch);

                }

            } while (keypress.Key != ConsoleKey.Enter);

            if (onlyChecked)
            {
                TmpOptions = (from t in TmpOptions where t.IsSelected select t).ToList();
            }

            setColorSchema(ColorSchema.Regular);
            return TmpOptions;

        }

        public string DrawList(List<String> options, ColorSchema schema = ColorSchema.Regular)
        {
            Draw(schema);
            setColorSchema(schema);
            int Line = LastBodyHeight + MarginTop;
            int tmpCursor = 0;
            foreach (string s in options)
            {
                Console.SetCursorPosition(MarginLeft + PosLeft, Line);
                System.Console.Write($" {s}");
                Line++;
            }

            Line -= options.Count;

            ConsoleKeyInfo keypress;
            do
            {

                Console.SetCursorPosition(MarginLeft + PosLeft, Line + tmpCursor);
                Console.Write(AnswerChar);
                keypress = Console.ReadKey(true);
                if (keypress.Key == ConsoleKey.UpArrow)
                {
                    if (tmpCursor > 0)
                    {
                        Console.SetCursorPosition(MarginLeft + PosLeft, Line + tmpCursor);
                        Console.Write(EmptyChar);
                        tmpCursor--;
                    }
                }
                if (keypress.Key == ConsoleKey.DownArrow)
                {
                    if (tmpCursor < options.Count - 1)
                    {
                        Console.SetCursorPosition(MarginLeft + PosLeft, Line + tmpCursor);
                        Console.Write(EmptyChar);
                        tmpCursor++;
                    }
                }

            } while (keypress.Key != ConsoleKey.Enter);
            setColorSchema(ColorSchema.Regular);
            return options[tmpCursor];
        }


    }
}