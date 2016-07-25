using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Machine
{
    public class MainMenu
    {
        public static void PrintMainMenu()
        {
            Console.WriteLine("Welcome to Vending Machine Program \n");
            Console.WriteLine("\nSelect which type of operation you want to perform");
            Console.WriteLine("1) Admin");
            Console.WriteLine("2) User");
        }

        public static bool ValidateMainInput(string userInput)
        {
            int choice;
            if (!Int32.TryParse(userInput, out choice))
            {
                return false;
            }
            else if (choice == (int)Core.ENUM.MainMenuOptions.USER || choice == (int)Core.ENUM.MainMenuOptions.ADMIN)
                return true;
            else return false;
        }

    }
}
