using System;
using tui_netcore.TUI;

namespace tui_netcore
{
    class Program
    {
        static void Main(string[] args)
        {
            Tui t = new Tui();
            t.TestSize(Tui.ColorSchema.System); 
        }
    }
}
