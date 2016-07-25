using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.User
{
    public class UserMenu
    {
        public static void PrintUserMenu()
        {
            Console.WriteLine("\nWelcome to User Menu");
            Console.WriteLine("\nPlease select one of the operations below");
            Console.WriteLine("1) List items in the vending machine");
            Console.WriteLine("2) Purchase item");
        }
    }
}
