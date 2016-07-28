using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Admin
{
    public class AdminMenu
    {
        public static void PrintAdminMenu()
        {
            Console.WriteLine("\nWelcome to Admin Menu");
            Console.WriteLine("\nPlease select one of the operations below");
            Console.WriteLine("1) Update product stock");
            Console.WriteLine("2) Update money stock");
            Console.WriteLine("3) Report Module: View Current Situation of the Vending Machine");
            Console.WriteLine("4) Reset Vending Machine");
        }
    }
}
