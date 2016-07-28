using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Machine
{
    public class ReportModule
    {
        public static void ShowReport()
        {
            Console.WriteLine("Total Currency: {0}", Machine.TotalCurrency);
            Console.WriteLine("Total Money Stock Currency: {0}", Machine.MoneyStockCurrency);
            Console.WriteLine("Total Product Stock Currency: {0}", Machine.ProductStockCurrency);
        }

        public static void CalculateCurrentCurrencies()
        {
            double subtotal = 0;
            string []productInfo = new string[4];
            for (int i = 0; i < Machine.rowSize; i++)
            {
                productInfo = Core.DBConnection.VendingMachineProductsDB.GetProductInfo(i);
               // productInfo = Core.DatabaseConnection.GetProductInfo(i + 1);
                subtotal += Convert.ToDouble(productInfo[1]) * Convert.ToDouble(productInfo[2]);
               // subtotal += Convert.ToDouble(Machine.VendingMachineProducts[i, 1]) * Convert.ToDouble(Machine.VendingMachineProducts[i, 2]);
            }

            Machine.ProductStockCurrency = subtotal;
            string[] moneyInfo = new string[2];
            for (int i = 0; i < Machine.moneyTypeCount; i++)
            {
                moneyInfo = Core.DBConnection.VendingMachineMoneyDB.GetMoneyInfo(i);
                //moneyInfo = Core.DatabaseConnection.GetMoneyInfo(i);
                subtotal += Convert.ToDouble(moneyInfo[0]) * Convert.ToDouble(moneyInfo[1]); 
                //subtotal += Convert.ToDouble(Machine.VendingMachineMoneyStock[i, 0]) * Convert.ToDouble(Machine.VendingMachineMoneyStock[i, 1]);
            }

            Machine.MoneyStockCurrency = subtotal - Machine.ProductStockCurrency;

            Machine.TotalCurrency = subtotal;
        }

        public static void GetCurrentSituation()
        {
            Core.DBConnection.VendingMachineProductsDB.ListProductsFromDatabase();
            Core.DBConnection.VendingMachineMoneyDB.ListMoneyFromDatabase();
//            Core.DatabaseConnection.ListProductsFromDatabase();
  //          Core.DatabaseConnection.ListMoneyFromDatabase();

            CalculateCurrentCurrencies();
            ShowReport();
        }
    }
}
