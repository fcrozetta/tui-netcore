using System;
using tui_netcore.TUI;

namespace tui_netcore
{
    class Program
    {
        static void Main(string[] args)
        {
            Tui t = new Tui(50,25,5,3);
            t.Title = "Sample Window";
            t.Body = "This is a really long description that should break lines somewhere in the middle of the text. Maybe here, maybe there";
            // t.TestSize(Tui.ColorSchema.System);
            t.DrawYesNo(Tui.ColorSchema.System);
            Console.ReadLine();
        }
    }
}
