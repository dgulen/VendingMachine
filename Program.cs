using System;
using VendingMachine.Machine;
using VendingMachine.Core.ENUM;

namespace VendingMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            Machine.Machine.InitializeVendingMachine();

            bool mainLoop = true;
            while (mainLoop)
            {
                MainMenu.PrintMainMenu();

                string userInput;

                while (Console.KeyAvailable)
                {
                    Console.ReadKey();
                }
                userInput = Console.ReadLine();

                if (MainMenu.ValidateMainInput(userInput))
                {
                    switch (Convert.ToInt32(userInput))
                    {
                        case (int)MainMenuOptions.ADMIN:
                            Admin.Admin.Main();
                            break;

                        case (int)MainMenuOptions.USER:
                            User.User.Main();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }

            }
        }
    }
}
