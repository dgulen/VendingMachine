using System;
using VendingMachine.Core;

namespace VendingMachine.Admin
{
    public static class Admin
    {
        public static void Main()
        {
            AdminMenu.PrintAdminMenu();
            string adminInput = Console.ReadLine();
            if (AdminInput.ValidateAdminInput(adminInput))
            {
                switch (Convert.ToInt32(adminInput))
                {
                    case (int)Core.ENUM.AdminOptions.UPLOAD_PRODUCTS:
                        Console.WriteLine("Enter new products' file address: ");
                        if (Machine.Machine.UpdateVendingMachine(Console.ReadLine()))
                        {
                            Console.WriteLine("New products are uploaded to the Vending Machine.");
                        }
                        else
                        {
                            Console.WriteLine("Error in uploading new products to the Vending Machine");
                        }
                        break;

                    case (int)Core.ENUM.AdminOptions.UPLOAD_MONEY:
                        Console.WriteLine("Enter new money stock's file address: ");
                        if (Machine.Machine.UpdateVendingMachineMoneyStock(Console.ReadLine()))
                        {
                            Console.WriteLine("Money storage updated.");
                        }
                        else
                        {
                            Console.WriteLine("Error in updating money storage.");
                        }
                        break;

                    case (int)Core.ENUM.AdminOptions.REPORT_MODULE:
                        Console.WriteLine("Connecting to the Report Module");
                        Machine.ReportModule.GetCurrentSituation();
                        break;

                    case (int)Core.ENUM.AdminOptions.RESET:
                        Console.WriteLine("Resetting... Uploading initial data... ");
                        Core.DBConnection.VendingMachineProductsDB.InitializeProductDatabase("testtest.txt");
                        Core.DBConnection.VendingMachineMoneyDB.InitializeMoneyDatabase();
                        //DatabaseConnection.InitializeDatabase("testtest.txt"); 
                        break;
                }
            }
        }    
    }
}
