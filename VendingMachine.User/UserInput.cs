using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.User
{
    public class UserInput
    {
        public static bool ValidateUserInput(string userInput)
        {
            int choice;
            if (!Int32.TryParse(userInput, out choice))
            {
                return false;
            }
            else if (choice == 1 || choice == 2)
                return true;
            else return false;
        }
    }
}
