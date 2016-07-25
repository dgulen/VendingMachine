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
            for (int i = 0; i < Machine.rowSize; i++)
            {
                subtotal += Convert.ToDouble(Machine.VendingMachineProducts[i, 1]) * Convert.ToDouble(Machine.VendingMachineProducts[i, 2]);
            }

            Machine.ProductStockCurrency = subtotal;

            for (int i = 0; i < Machine.moneyTypeCount; i++)
            {
                subtotal += Convert.ToDouble(Machine.VendingMachineMoneyStock[i, 0]) * Convert.ToDouble(Machine.VendingMachineMoneyStock[i, 1]);
            }

            Machine.MoneyStockCurrency = subtotal - Machine.ProductStockCurrency;

            Machine.TotalCurrency = subtotal;
        }

        public static void GetCurrentSituation()
        {
            ProductHolder.ListVendingMachineProducts();
            MoneyHolder.ListVendingMachineMoneyStock();
            CalculateCurrentCurrencies();
            ShowReport();
        }
    }
}
