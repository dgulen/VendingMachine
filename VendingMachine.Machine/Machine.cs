using System;
using System.Configuration;
using VendingMachine.Core.ENUM;
using System.Data.SQLite;

namespace VendingMachine.Machine
{
    public class Machine
    {
        public static String [,] VendingMachineProducts ;
        public static String[,] VendingMachineMoneyStock;

        public static String[] VendingMachineProductStock;
        public static String[] VendingMachineProductPrice;

        public static int rowSize;
        public static int columnSize;

        public static int moneyTypeCount;
        public static int moneyTableColumnCount;

        public static double TotalCurrency;
        public static double ProductStockCurrency;
        public static double MoneyStockCurrency;

        public static string VendingMachineFile = (ConfigurationManager.AppSettings["VendingMachineFile"]);
        public static string VendingMachineMoneyFile = (ConfigurationManager.AppSettings["VendingMachineMoneyFile"]);

        public static int ProductNameIndex = Convert.ToInt32(ConfigurationManager.AppSettings["ProductNameIndex"]);
        public static int ProductStockIndex = Convert.ToInt32(ConfigurationManager.AppSettings["ProductStockIndex"]);
        public static int ProductPriceIndex = Convert.ToInt32(ConfigurationManager.AppSettings["ProductPriceIndex"]);
        public static int ProductSlotIdIndex = Convert.ToInt32(ConfigurationManager.AppSettings["ProductSlotIdIndex"]);

        public static int MaxCapacity = Convert.ToInt32(ConfigurationManager.AppSettings["MaxCapacity"]);

        public static void InitializeVendingMachine()
        {
            RetriewData.GetData(VendingMachineFile, ref rowSize, ref columnSize);
            VendingMachineProducts = new string[rowSize, columnSize];
            VendingMachineProducts = RetriewData.GetData(VendingMachineFile, ref rowSize, ref columnSize);
            if (VendingMachineProducts[0, 0] == Convert.ToString(ErrorOutput.UNSUCCESSFUL))
            {
                Console.WriteLine("Error in openning Vending Machine Products file.");
                return;
            }
            else
            {
                for(int i = 0; i <rowSize; i++)
                {
                    if(Convert.ToInt32(VendingMachineProducts[i, 1]) > MaxCapacity)
                    {
                        Console.WriteLine("Too much {0}. Products need to be less than {1}.", VendingMachineProducts[i,0],MaxCapacity);
                        return;
                    }
                }
            }

            InitializeVendingMachineProductPrice();
            InitializeVendingMachineProductStock();
            InitializeVendingMachineMoneyStock(VendingMachineMoneyFile);
        }

        public static bool UpdateVendingMachine(string ProductFile)
        {
            if (ProductHolder.AddFromFileToVendingMachine(ProductFile))
            {
                InitializeVendingMachineProductStock();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UpdateVendingMachineMoneyStock(string MoneyFile)
        {
            if (!MoneyHolder.AddMoneyFromFileToVendingMachine(MoneyFile))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void InitializeVendingMachineProductPrice()
        {
            VendingMachineProductPrice = new string[rowSize];
            for (int i = 0; i < rowSize; i++)
            {
                VendingMachineProductPrice[i] = VendingMachineProducts[i, ProductPriceIndex];
            }
        }

        public static void InitializeVendingMachineProductStock()
        {
            VendingMachineProductStock = new string[rowSize];
            for (int i = 0; i < rowSize; i++)
            {
                VendingMachineProductStock[i] = VendingMachineProducts[i, ProductStockIndex];
            }
        }

        public static void InitializeVendingMachineMoneyStock(string MoneyFile)
        {
            RetriewData.GetData(MoneyFile, ref moneyTypeCount, ref moneyTableColumnCount);
            VendingMachineMoneyStock = new string[moneyTypeCount, columnSize];
            VendingMachineMoneyStock = RetriewData.GetData(MoneyFile, ref moneyTypeCount, ref moneyTableColumnCount);
            if(VendingMachineMoneyStock[0,0] == Convert.ToString(ErrorOutput.UNSUCCESSFUL))
            {
                Console.WriteLine("Error in openning money stock file.");
            }
        }

        public static bool ValidateMoneyInput(double inputMoney)
        {
            //enum yap veya app.config
            if (inputMoney == 0.25 || inputMoney == 0.5 || inputMoney == 1 || inputMoney == 5 || inputMoney == 10 || inputMoney == 20)
                return true;
            else
                return false;
        }
    }
}
