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

        public static int rowSize=40;
        public static int columnSize=4;

        public static int moneyTypeCount=6;
        public static int moneyTableColumnCount=2;

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
            VendingMachineProducts = new string[rowSize, columnSize];
            VendingMachineProducts = Core.DBConnection.VendingMachineProductsDB.GetDataFromProductsDB(ref rowSize, ref columnSize);
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
            InitializeVendingMachineMoneyStock(); 
        }

        
        public static bool UpdateVendingMachine(string ProductFile)
        {
            if (ProductHolder.AddFromFileToVendingMachine(ProductFile))
            {
                Core.DBConnection.VendingMachineProductsDB.UpdateProductDatabase(VendingMachineProducts);
                InitializeVendingMachine();
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
                Core.DBConnection.VendingMachineMoneyDB.UpdateMoneyDatabase(VendingMachineMoneyStock);
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

        public static void InitializeVendingMachineMoneyStock()
        {
            VendingMachineMoneyStock = new string[moneyTypeCount, moneyTableColumnCount]; 
            VendingMachineMoneyStock = Core.DBConnection.VendingMachineMoneyDB.GetDataFromMoneyDB();
            if(VendingMachineMoneyStock[0,0] == Convert.ToString(ErrorOutput.UNSUCCESSFUL))
            {
                Console.WriteLine("Error in openning money stock file.");
            }
        }

        public static bool ValidateMoneyInput(double inputMoney)
        {
            if (inputMoney == 0.25 || inputMoney == 0.5 || inputMoney == 1 || inputMoney == 5 || inputMoney == 10 || inputMoney == 20)
                return true;
            else
                return false;
        }
    }
}
