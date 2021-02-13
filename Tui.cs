using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

#nullable enable

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

        public BorderStyleBrush BorderStyle { get; set; } = BorderStyles.Text;

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

        public sealed class BorderStyleBrush
        {
            public char TopLeftChar { get; }
            public char HorizontalChar { get; }
            public char TopRightChar { get; }
            public char VerticalChar { get; }
            public char BottomLeftChar { get; }
            public char BottomRightChar { get; }

            public BorderStyleBrush(char topLeft, char horizontal, char topRight, char vertical, char bottomLeft, char bottomRight)
            {
                TopLeftChar = topLeft;
                HorizontalChar = horizontal;
                TopRightChar = topRight;
                VerticalChar = vertical;
                BottomLeftChar = bottomLeft;
                BottomRightChar = bottomRight;
            }

            public void Deconstruct(out char topLeft, out char horizontal, out char topRight, out char vertical, out char bottomLeft, out char bottomRight)
            {
                topLeft = TopLeftChar;
                horizontal = HorizontalChar;
                topRight = TopRightChar;
                vertical = VerticalChar;
                bottomLeft = BottomLeftChar;
                bottomRight = BottomRightChar;
            }
        }

        public static class BorderStyles
        {
            public static readonly BorderStyleBrush Text = new BorderStyleBrush('+', '-', '+', '|', '+', '+');
            public static readonly BorderStyleBrush SingleLine = new BorderStyleBrush('┌', '─', '┐', '│', '└', '┘');
            public static readonly BorderStyleBrush DoubleLine = new BorderStyleBrush('╔', '═', '╗', '║', '╚', '╝');
        }

        public Tui(int width = 100, int height = 80, int posLeft = 0, int posTop = 0)
        {
            Width = width;
            Height = height;
            PosTop = posTop;
            PosLeft = posLeft;
            AnswerChar = '>';
            SelectedChar = '*';
            IntersectionChar = '+';

            EmptyChar = ' ';
            Title = String.Empty;
            Body = String.Empty;
            MarginLeft = 4;
            MarginTop = 2;
            LastBodyHeight = 0;
        }

        public Tui() : this(Console.WindowWidth, Console.WindowHeight, 0, 0)
        {
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
        public void TestSize(ColorSchema schema = ColorSchema.Info)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Console.SetCursorPosition(j, i);
                    System.Console.Write("+");
                }
                Console.SetCursorPosition((Width / 2) - 6, i);
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

        /// <summary>
        /// Roughly break body text in pages, considering no line breaks.
        /// If there are line breaks, the window will not be complete.
        /// words may break abruptely
        /// </summary>
        /// <returns></returns>
        private List<string> separatePages()
        {
            var pages = new List<string>();
            int index = -1;
            int totalSpace = (Width - (2 * MarginLeft)) * (Height - (2 * MarginTop) - 2);

            foreach (var c in Body)
            {
                if (index == -1)
                {
                    pages.Add(c.ToString());
                    index++;
                }
                else if (pages.Last().ToString().Count() - 1 < totalSpace)
                {
                    pages[index] += c.ToString();
                }
                else
                {
                    pages.Add(c.ToString());
                    index++;
                }
            }

            return pages;
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
            Console.SetCursorPosition(PosLeft, PosTop);
            Console.Write(BorderStyle.TopLeftChar);

            Console.SetCursorPosition(PosLeft + Width - 1, PosTop);
            System.Console.Write(BorderStyle.TopRightChar);

            Console.SetCursorPosition(PosLeft, PosTop + Height - 1);
            System.Console.Write(BorderStyle.BottomLeftChar);

            Console.SetCursorPosition(PosLeft + Width - 1, PosTop + Height - 1);
            System.Console.Write(BorderStyle.BottomRightChar);
        }

        /// <summary>
        /// This Frame wraps the entire screen, printing spaces inside.
        /// </summary>
        /// <param name="schema">Color Scheme to be used</param>
        public void Draw(ColorSchema schema = ColorSchema.Regular)
        {
            setColorSchema(schema);

            var headLine = new Char[Width];
            var bodyLine = new Char[Width];

            for (var i = 0; i < Width; ++i)
            {
                headLine[i] = i == 0 || i == Width - 1 ? BorderStyle.VerticalChar : BorderStyle.HorizontalChar;
                bodyLine[i] = i == 0 || i == Width - 1 ? BorderStyle.VerticalChar : EmptyChar;
            }

            for (int i = PosTop; i < Height + PosTop; i++)
            {
                Console.SetCursorPosition(PosLeft, i);
                Console.Write(i == PosTop || i == Height + PosTop - 1 ? headLine : bodyLine);
            }

            DrawCorners();
            if (!String.IsNullOrEmpty(Title)) drawTitle();
            if (!String.IsNullOrEmpty(Body)) drawBody();

            setColorSchema(ColorSchema.Regular);
        }

        /// <summary>
        /// This method Creates an input so the user can answer things
        /// </summary>
        /// <param name="schema"></param>
        /// <returns>string containing the user answer</returns>
        public string? DrawInput(ColorSchema schema = ColorSchema.Regular)
        {
            Draw(schema);
            setColorSchema(schema);
            Console.SetCursorPosition((PosLeft + MarginLeft), (PosTop + Height - MarginTop));
            Console.Write($"{AnswerChar} ");
            string? answer = Console.ReadLine();
            setColorSchema(ColorSchema.Regular);
            return answer;
        }

        /// <summary>
        /// Prints message as book pages, based on window size.
        /// Has three button options (next, previous, done) that are used to go forward, backward and exit scree
        /// </summary>
        /// <param name="schema">Color Schema</param>
        /// <param name="txtNext">Text for Next option</param>
        /// <param name="txtPrev">Text for Previous option</param>
        /// <param name="txtDone">Text for Quit option</param>
        /// <param name="defaultAnswer">0=Prev / 1=Next / 2=Done</param>
        public void DrawBook(ColorSchema schema = ColorSchema.Regular, string txtNext = "next", string txtPrev = "previous", string txtDone = "Done", int defaultAnswer = 1)
        {
            setColorSchema(schema);
            int answer = defaultAnswer;
            int CursorPrev = PosLeft + MarginLeft;
            int CursorDone = PosLeft + Width - (2 * MarginLeft);
            int CursorNext = (PosLeft + Width) / 2;
            int Line = (PosTop + Height - MarginTop);
            int index = 0;
            string oldTitle = Title;

            List<string> pages = separatePages();
            int totalPages = pages.Count();
            Body = pages[0];

            ConsoleKeyInfo keypress;
            bool keepGoing = true;
            while (keepGoing)
            {
                Title = oldTitle + $@" [ {index + 1}/{totalPages} ]";
                Draw(schema);
                Console.SetCursorPosition(CursorPrev + 1, Line);
                Console.Write(txtPrev);
                Console.SetCursorPosition(CursorNext + 1, Line);
                Console.Write(txtNext);
                Console.SetCursorPosition(CursorDone + 1, Line);
                Console.Write(txtDone);

                Console.SetCursorPosition(CursorPrev, Line);
                Console.Write(answer == 0 ? AnswerChar : EmptyChar);
                Console.SetCursorPosition(CursorNext, Line);
                Console.Write(answer == 1 ? AnswerChar : EmptyChar);
                Console.SetCursorPosition(CursorDone, Line);
                Console.Write(answer == 2 ? AnswerChar : EmptyChar);

                keypress = Console.ReadKey(false);

                switch (keypress.Key)
                {
                    case ConsoleKey.LeftArrow:
                        answer = answer - 1 >= 0 ? answer - 1 : 2;
                        break;
                    case ConsoleKey.RightArrow:
                        answer = answer + 1 <= 2 ? answer + 1 : 0;
                        break;
                    case ConsoleKey.Enter:
                        switch (answer)
                        {
                            case 0:
                                index = index - 1 >= 0 ? index - 1 : pages.Count() - 1;
                                break;
                            case 1:
                                index = index + 1 <= pages.Count() - 1 ? index + 1 : 0;
                                break;
                            case 2:
                                keepGoing = false;
                                break;

                        }
                        break;
                }
                Body = pages[index];
            }

            setColorSchema(ColorSchema.Regular);
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
        public bool DrawYesNo(ColorSchema schema = ColorSchema.Regular, string txtYes = "Yes", string txtNo = "No", bool defaultAnswer = false)
        {
            Draw(schema);
            setColorSchema(schema);
            bool answer = defaultAnswer;
            int CursorYes = PosLeft + MarginLeft;
            int CursorNo = PosLeft + Width - (2 * MarginLeft);
            int Line = (PosTop + Height - MarginTop);

            ConsoleKeyInfo keypress;
            do
            {
                Console.SetCursorPosition(CursorYes + 1, Line);
                Console.Write(txtYes);
                Console.SetCursorPosition(CursorNo + 1, Line);
                Console.Write(txtNo);

                Console.SetCursorPosition(CursorYes, Line);
                Console.Write(answer ? AnswerChar : EmptyChar);
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
        public void DrawOk(ColorSchema schema = ColorSchema.Regular, string txtOk = "Press any key to continue...")
        {
            Draw(schema);
            setColorSchema(schema);
            int CursorOk = PosLeft + MarginLeft;
            int Line = (PosTop + Height - MarginTop);
            Console.SetCursorPosition(CursorOk, Line);
            Console.Write($"{AnswerChar}{txtOk}");
            Console.ReadKey();
        }

        public List<CheckBoxOption> DrawCheckBox(List<CheckBoxOption> options, ColorSchema schema = ColorSchema.Regular, bool onlyChecked = true)
        {
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
                    if (tmpCursor < TmpOptions.Count - 1)
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
                    char ch = c.IsSelected ? SelectedChar : EmptyChar;
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