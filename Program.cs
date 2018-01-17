using System;
using tui_netcore.TUI;
namespace tui_netcore
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.CursorVisible = false;

            Tui t = new Tui(100,25,5,3){
                Title = "Main Menu",
                Body = "Welcome To the first Version of the TUI, made by Fernando Crozetta."
            };
            Tui.ColorSchema schema =Tui.ColorSchema.Info;
            t.DrawOk(schema);
            
            t.Title = "Creating a Simple character";
            t.Body = "Please Type the name of your character";
            string charName = t.DrawInput(schema);

            t.Body = "Type your race";
            string charRace = t.DrawInput(schema);
            // Console.ReadLine();

            t.Title = "Confirmation Screen";
            t.Body = $"Are you sure that you are {charName}, the {charRace}?";

            if(t.DrawYesNo(Tui.ColorSchema.Warning)){
                t.Title = " The End ";
                t.Body = "Congratulations! the Test was a success";
                t.DrawOk(Tui.ColorSchema.System);
            }else
            {
                t.Title = "You have Failed";
                t.Body = "How could you fail this simples quiz ? I'm really upset. You let me Down.";
                t.DrawOk(Tui.ColorSchema.Danger);
            }


            //! Going back to default terminal settings
            Console.ResetColor();
            Console.CursorVisible = true;
        }
    }
}
