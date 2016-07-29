using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Core;
using VendingMachine.Machine;

namespace VendingMachine.User
{
    public class Purchase
    {
        static StateHandler _stateHandler = new StateHandler();

        public static bool PurchaseProduct(int ProductID)
        {
            _stateHandler.UpdateState(State.ALLOW_RUNNING);

            SetConsoleCtrlHandler(new ConsoleEventDelegate(ConsoleEventCallback), true);

            double currentProductPrice;
            if (ProductID > 10) //Machine.Machine.rowSize) TODO
            {
                Console.WriteLine("There is no {0}. slot!", ProductID);
                return false;
            }
            else if (!(Convert.ToDouble( Machine.Machine.VendingMachineProducts[ProductID-1, 1]) > 0))
            {
                Console.WriteLine("There is no product {0} left.", ProductID);
                return false;
            }

            string[] productInfo = Core.DBConnection.VendingMachineProductsDB.GetProductInfo(ProductID);
            currentProductPrice = (Convert.ToDouble(productInfo[2]));
            Console.WriteLine("Please insert: {0} TL", currentProductPrice);

            List<double> inputMoneyList = new List<double>();

            double userInputMoneyTotal = 0;
            int emptyInputcount = 0;

            while (userInputMoneyTotal < currentProductPrice)
            {
                SetConsoleCtrlHandler(new ConsoleEventDelegate(ConsoleEventCallback), true);

                if (emptyInputcount > 3)
                {
                    Console.WriteLine("Time Out!");
                    break;
                }

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
                            Console.WriteLine("\nPlease take your {0} TL back.", userInputMoneyTotal);
                            return false;
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }
                var userInputMoneyCurrent = Console.ReadLine();

                if (userInputMoneyCurrent == "" )
                {
                    Console.WriteLine("Please enter money amount.");
                    emptyInputcount++;
                }
                else
                {
                    emptyInputcount = 0;
                    if (Machine.Machine.ValidateMoneyInput(Convert.ToDouble(userInputMoneyCurrent)))
                    {
                        userInputMoneyTotal = userInputMoneyTotal + Convert.ToDouble(userInputMoneyCurrent);
                        inputMoneyList.Add(Convert.ToDouble(userInputMoneyCurrent));
                    }
                    else
                    {
                        Console.WriteLine("Unexceptable amount of money. Please pay with different bills/coins.");
                    }
                }
            }

            //payment diye ayır burayı
            if (userInputMoneyTotal < currentProductPrice)
            {
                Console.WriteLine("Not enough money, take the money back from the holder.");
                return false;
            }
            else if (userInputMoneyTotal == currentProductPrice)
            {
                bool result = true;
                foreach (var money in inputMoneyList)
                {
                    if (!MoneyHolder.IncreaseMoney(money))
                    {
                        Console.WriteLine("Error in updating money stock.");
                        result = false;
                    }
                }
                if (result)
                {
                    ProductHolder.TakeProductFromTheVendingMachine(ProductID);
                }
                return result;
            }
            else
            {
                int[] changeAmount = Change.CalculateChange(userInputMoneyTotal, currentProductPrice);

                if (MoneyHolder.DecreaseMoney(changeAmount))
                {
                    bool result = true;
                    foreach (var money in inputMoneyList)
                    {
                        if (!MoneyHolder.IncreaseMoney(money))
                        {
                            Console.WriteLine("Error in updating money stock.");
                            result = false;
                        }
                    }
                    if (result)
                    {
                        ProductHolder.TakeProductFromTheVendingMachine(ProductID);
                        Console.WriteLine("Your change: {0}", userInputMoneyTotal - currentProductPrice);
                        Console.WriteLine("Take your change and product from the holders.");
                        Console.WriteLine("{0} 1 TL ", changeAmount[0]);
                        Console.WriteLine("{0} 0.5 TL ", changeAmount[1]);
                        Console.WriteLine("{0} 0.25 TL ", changeAmount[2]);
                    }
                    return result;
                }
                else
                {
                    Console.WriteLine("tThere is not enough money for change. Take your money back.");
                    return false;
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
