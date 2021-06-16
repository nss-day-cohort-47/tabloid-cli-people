using System;
using TabloidCLI.UserInterfaceManagers;

namespace TabloidCLI
{
    class Program
    {
        static void Main(string[] args)
        {
          

            Console.WriteLine("--------------------------");
            Console.WriteLine("| Hi! Welcome to People! |");
            Console.WriteLine("--------------------------");
            Console.WriteLine("What background color would you like?");
            Console.WriteLine("1) Blue background with black text");
            Console.WriteLine("2) White background with black text");
            Console.WriteLine("3) Gray background with black text");
            Console.WriteLine("4) Default");
            int answer = int.Parse(Console.ReadLine());
            if (answer ==1)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else if (answer ==2)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Black; 
            }
            else if (answer == 3)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else if (answer == 4)
            {
               
            }



            // MainMenuManager implements the IUserInterfaceManager interface
            IUserInterfaceManager ui = new MainMenuManager();
            while (ui != null)
            {
                // Each call to Execute will return the next IUserInterfaceManager we should execute
                // When it returns null, we should exit the program;
                ui = ui.Execute();
            }
          
        }
    }
}
