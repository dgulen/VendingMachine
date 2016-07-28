using System;
using VendingMachine.Core;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Threading;

namespace VendingMachine.User
{
    public static class User
    {
        public static int ErrorCode = Convert.ToInt32(ConfigurationManager.AppSettings["ErrorCode"]);
        static StateHandler _stateHandler = new StateHandler();

        public static void Main()
        {
            _stateHandler.UpdateState(State.ALLOW_RUNNING);

            SetConsoleCtrlHandler(new ConsoleEventDelegate(ConsoleEventCallback), true);
            bool loop = true;
            while (loop)
            {
                if (_stateHandler.GetState() != State.ALLOW_RUNNING)
                {
                    while (true)
                    {
                        if (_stateHandler.GetState() == State.ASKING_USER)
                        {
                            Thread.Sleep(100);
                        }
                        else if (_stateHandler.GetState() == State.ALLOW_RUNNING)
                        {
                            break;
                        }
                        else if (_stateHandler.GetState() == State.USER_CANCELLED)
                        {
                            return;
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }

                UserMenu.PrintUserMenu();
                string userInput = Console.ReadLine();
                if (UserInput.ValidateUserInput(userInput))
                {
                    switch(Convert.ToInt32(userInput))
                    {
                        case (int)Core.ENUM.UserOptions.LIST_ITEMS:
                            Console.WriteLine("Item List...\n");
                            //Machine.ProductHolder.ListVendingMachineProducts();
                            Core.DBConnection.VendingMachineProductsDB.ListProductsFromDatabase();
                            //DatabaseConnection.ListProductsFromDatabase();
                            break;

                        case (int)Core.ENUM.UserOptions.PURCHASE:
                            Console.WriteLine("Purchase Item Page...\n");
                            Console.WriteLine("Enter slot id for the product you want to purchase:");
                            int ProductID = TypeCast.ConsoleToInt(Console.ReadLine());

                            while (ProductID == ErrorCode)
                            {
                                Console.WriteLine("Invalid input. Enter slot id for the product you want to purchase:");
                                ProductID = TypeCast.ConsoleToInt(Console.ReadLine());
                            }

                            if (Purchase.PurchaseProduct(ProductID))
                            {
                                string[] productInfo = Core.DBConnection.VendingMachineProductsDB.GetProductInfo(ProductID);
                             // string[] productInfo = DatabaseConnection.GetProductInfo(ProductID);
                                Console.WriteLine("{0} Purchase successful." , Machine.Machine.VendingMachineProducts[ProductID-1,0]);
                                Console.WriteLine("{0} Purchase successful.", productInfo[0]);
                            }
                            else
                            {
                                Console.WriteLine("Purchase not successful.");
                            }
                            break;
                    }
                    break;
                }
            }
        }
        private delegate bool ConsoleEventDelegate(int eventType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 0)
            {
                _stateHandler.UpdateState(State.ASKING_USER);

                Console.WriteLine("Continue? [Y]es or [N]o");

                while (Console.KeyAvailable)
                    Console.ReadKey();

                var keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.N)
                {
                    _stateHandler.UpdateState(State.USER_CANCELLED);
                }
                else
                {
                    _stateHandler.UpdateState(State.ALLOW_RUNNING);
                }
            }
            while (Console.KeyAvailable)
                Console.ReadKey();

            return true;
        }
    }
}