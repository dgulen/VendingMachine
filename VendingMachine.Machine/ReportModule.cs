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
                subtotal += Convert.ToDouble(productInfo[1]) * Convert.ToDouble(productInfo[2]);
            }

            string[] moneyTypeArray = new string[6] { "0,25", "0,50", "1", "5", "10", "20" };

            Machine.ProductStockCurrency = subtotal;
            string[] moneyInfo = new string[2];
            for (int i = 0; i < Machine.moneyTypeCount; i++)
            {
                moneyInfo = Core.DBConnection.VendingMachineMoneyDB.GetMoneyInfo(Convert.ToDouble( moneyTypeArray[i]));
                subtotal += Convert.ToDouble(moneyInfo[0]) * Convert.ToDouble(moneyInfo[1]); 
            }

            Machine.MoneyStockCurrency = subtotal - Machine.ProductStockCurrency;

            Machine.TotalCurrency = subtotal;
        }

        public static void GetCurrentSituation()
        {
            Core.DBConnection.VendingMachineProductsDB.ListProductsFromDatabase();
            Core.DBConnection.VendingMachineMoneyDB.ListMoneyFromDatabase();

            CalculateCurrentCurrencies();
            ShowReport();
        }
    }
}
