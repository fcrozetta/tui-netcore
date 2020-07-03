using System;
using tui_netcore;
using System.Collections.Generic;
namespace tui_netcore
{
    class Program
    {
        static void Main(string[] args)
        {

            // ? This is set outside the class for now. this may be set inside the class, in another commit
            Console.CursorVisible = false;

            // The empty Constructor build a fullscreen, based on Console size at instance time
            Tui fullscreen = new Tui();
            fullscreen.DrawYesNo();

            //The construction of a box. Set Size and position
            Tui t = new Tui(100,25,10,15){
                Title = "Tui Demo",
                Body = "This is a demo test for Tui netcore. This is the first version, and bugs may appear" +
                " \n Do not be afraid. \n This Contains a sequence of screens that will show you how to create and"+
                " manage screens. \n You can create your own screens to play with."
            };

            // This sets a color schema for the windows
            Tui.ColorSchema schema =Tui.ColorSchema.Info;
            // Draw ok is a screen that will continue on any key pressed
            t.DrawOk(schema);
            
            // text with scroll
            t.Title = "Long screen with scroll";
            // t.Body = "Occaecat labore est Lorem pariatur deserunt. Esse sint aliquip sit culpa sit exercitation. Quis ad pariatur officia exercitation dolore dolor ad quis. Esse incididunt in nulla ullamco deserunt sunt tempor incididunt mollit est laborum. \n Esse pariatur nostrud dolor excepteur consectetur Lorem minim minim. Commodo do ex anim aliquip. Nostrud elit aliqua nostrud sunt nostrud ipsum est elit est. Ex magna magna occaecat occaecat quis eiusmod elit anim voluptate. Pariatur sint aute proident Lorem in quis fugiat. \n Magna adipisicing occaecat pariatur et ad. Enim pariatur esse sint deserunt irure dolor Lorem anim est laboris voluptate ut voluptate ut. Aliquip sunt id ad ea sunt culpa mollit ipsum et laboris eu exercitation proident. Enim duis quis et voluptate nulla quis quis Lorem dolor labore sit excepteur aliqua aute. Fugiat elit veniam esse sit laboris aliqua commodo ad consequat incididunt eiusmod minim esse aliqua. Deserunt laboris nostrud sunt sunt veniam officia culpa duis.Occaecat labore est Lorem pariatur deserunt. Esse sint aliquip sit culpa sit exercitation. Quis ad pariatur officia exercitation dolore dolor ad quis. Esse incididunt in nulla ullamco deserunt sunt tempor incididunt mollit est laborum. \n Esse pariatur nostrud dolor excepteur consectetur Lorem minim minim. Commodo do ex anim aliquip. Nostrud elit aliqua nostrud sunt nostrud ipsum est elit est. Ex magna magna occaecat occaecat quis eiusmod elit anim voluptate. Pariatur sint aute proident Lorem in quis fugiat. \n Magna adipisicing occaecat pariatur et ad. Enim pariatur esse sint deserunt irure dolor Lorem anim est laboris voluptate ut voluptate ut. Aliquip sunt id ad ea sunt culpa mollit ipsum et laboris eu exercitation proident. Enim duis quis et voluptate nulla quis quis Lorem dolor labore sit excepteur aliqua aute. Fugiat elit veniam esse sit laboris aliqua commodo ad consequat incididunt eiusmod minim esse aliqua. Deserunt laboris nostrud sunt sunt veniam officia culpa duis.Occaecat labore est Lorem pariatur deserunt. Esse sint aliquip sit culpa sit exercitation. Quis ad pariatur officia exercitation dolore dolor ad quis. Esse incididunt in nulla ullamco deserunt sunt tempor incididunt mollit est laborum. \n Esse pariatur nostrud dolor excepteur consectetur Lorem minim minim. Commodo do ex anim aliquip. Nostrud elit aliqua nostrud sunt nostrud ipsum est elit est. Ex magna magna occaecat occaecat quis eiusmod elit anim voluptate. Pariatur sint aute proident Lorem in quis fugiat. \n Magna adipisicing occaecat pariatur et ad. Enim pariatur esse sint deserunt irure dolor Lorem anim est laboris voluptate ut voluptate ut. Aliquip sunt id ad ea sunt culpa mollit ipsum et laboris eu exercitation proident. Enim duis quis et voluptate nulla quis quis Lorem dolor labore sit excepteur aliqua aute. Fugiat elit veniam esse sit laboris aliqua commodo ad consequat incididunt eiusmod minim esse aliqua. Deserunt laboris nostrud sunt sunt veniam officia culpa duis.Occaecat labore est Lorem pariatur deserunt. Esse sint aliquip sit culpa sit exercitation. Quis ad pariatur officia exercitation dolore dolor ad quis. Esse incididunt in nulla ullamco deserunt sunt tempor incididunt mollit est laborum. \n Esse pariatur nostrud dolor excepteur consectetur Lorem minim minim. Commodo do ex anim aliquip. Nostrud elit aliqua nostrud sunt nostrud ipsum est elit est. Ex magna magna occaecat occaecat quis eiusmod elit anim voluptate. Pariatur sint aute proident Lorem in quis fugiat. \n Magna adipisicing occaecat pariatur et ad. Enim pariatur esse sint deserunt irure dolor Lorem anim est laboris voluptate ut voluptate ut. Aliquip sunt id ad ea sunt culpa mollit ipsum et laboris eu exercitation proident. Enim duis quis et voluptate nulla quis quis Lorem dolor labore sit excepteur aliqua aute. Fugiat elit veniam esse sit laboris aliqua commodo ad consequat incididunt eiusmod minim esse aliqua. Deserunt laboris nostrud sunt sunt veniam officia culpa duis.";
            t.Body = "testing";
            t.DrawBook();

            // User answer
            t.Title = "Screen to get user answer";
            t.Body = "This screen contains a text, which is a description of what the user whould do. \n Use this to let the user type the answer."+
            " \n For the sake of this tutorial, type your name, and press <enter>.";
            string username = t.DrawInput(schema);

            t.Title = "CheckBox Screen";
            t.Body = "Check Box Screens allows the user to choose multiple options. A list of the chosen options will be returned." +
            " \n The user can select using <space> and continue to the next screen using <enter>";

            List<Tui.CheckBoxOption> options = t.DrawCheckBox(new List<Tui.CheckBoxOption>(){
                new Tui.CheckBoxOption(){
                    IsSelected = false,
                    Name = "option 1",
                    Description = "Description of option 1"
                },
                new Tui.CheckBoxOption(){
                    IsSelected = true,
                    Name = "option 2",
                    Description = "Description of option 2"
                },
                new Tui.CheckBoxOption(){
                    IsSelected = false,
                    Name = "option 3",
                    Description = "Description of option 3"
                }
            },schema);


            t.Title = "Lists";
            t.Body = "Lists were created to be used as 'radio buttons' and simple menus."+
            " \n Use the Lists as a single choice menu. \n Press <enter> to select.";
            string TestList = t.DrawList(new List<string>() {
                "Banana",
                "Apple",
                "Orange"
        }, schema);

            t.Title = "Confirmation Screen";
            t.Body = $"This Screens let the user choose between Yes and No options.You can set one options as default."+
            " \n <enter> selects an option";
            t.DrawYesNo(Tui.ColorSchema.Danger);

            t.Title = "Finish";
            t.Body = "You have Completed the example program";
            t.DrawOk();


            t.Title = "End Program";
            t.Body = "This does not require user interaction";
            t.Draw();

            //! Going back to default terminal settings
            Console.ResetColor();
            Console.CursorVisible = true;
        }
    }
}
