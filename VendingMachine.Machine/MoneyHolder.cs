using System;
using VendingMachine.Core.ENUM;

namespace VendingMachine.Machine
{
    public class MoneyHolder
    {
        public static bool DecreaseMoney(int[] changeAmount)
        {
            // 1 yeterli miktarda var mı 
    
            string[] money1 = Core.DBConnection.VendingMachineMoneyDB.GetMoneyInfo(1);
            double tempDouble = Convert.ToDouble(money1[1]);
            tempDouble = tempDouble - changeAmount[0];
            if (!(tempDouble < 0))
            {
                // 0.5 yeterli miktarda var mı

                string[] money2 = Core.DBConnection.VendingMachineMoneyDB.GetMoneyInfo(0.5);
                double tempDouble2 = Convert.ToDouble(money2[1]);
                tempDouble2 = tempDouble2 - changeAmount[1];
                if (!(tempDouble2 < 0))
                {
                    // 0.25 yeterli miktarda var mı 

                    string[] money3 = Core.DBConnection.VendingMachineMoneyDB.GetMoneyInfo(0.25);
                    double tempDouble3 = Convert.ToDouble(money3[1]);
                    tempDouble3 = tempDouble3 - changeAmount[2];
                    if (!(tempDouble3 < 0))
                    {
                        Core.DBConnection.VendingMachineMoneyDB.IncreaseMoneyAmountInDB(0.25, -changeAmount[2]);
                    }
                    else
                    {
                        return false;
                    }
                    Core.DBConnection.VendingMachineMoneyDB.IncreaseMoneyAmountInDB(0.50, -changeAmount[1]);
                }
                else
                {
                    return false;
                }
                Core.DBConnection.VendingMachineMoneyDB.IncreaseMoneyAmountInDB(1, -changeAmount[0]);
            }
            else
            {
                return false;
            }

            Machine.InitializeVendingMachineMoneyStock();
            return true;
        }

        public static bool IncreaseMoney(double inputMoney)
        {
            string[] money = Core.DBConnection.VendingMachineMoneyDB.GetMoneyInfo(inputMoney);
            double moneyCount = Convert.ToDouble(money[1]);
            //moneyCount++;
            Core.DBConnection.VendingMachineMoneyDB.IncreaseMoneyAmountInDB(inputMoney, 1);
            Machine.InitializeVendingMachineMoneyStock();
            return true; 
        }

        public static bool AddMoneyFromFileToVendingMachine(string inFile)
        {
            int _rowSize = Machine.moneyTypeCount;
            int _columnSize = Machine.moneyTableColumnCount;
            RetriewData.GetData(inFile, ref _rowSize, ref _columnSize);
            String[,] newVendingMachineMoney = new string[_rowSize, _columnSize];
            newVendingMachineMoney = RetriewData.GetData(inFile, ref _rowSize, ref _columnSize);

            if(newVendingMachineMoney[0,0 ] == Convert.ToString(ErrorOutput.UNSUCCESSFUL))
            {
                return false;
            }

            bool fileFormat = RetriewData.CheckFileFormat(newVendingMachineMoney, Machine.VendingMachineMoneyStock, _rowSize, _columnSize);

            if (!fileFormat)
            {
                return false;
            }
            else
            {
                MoneyHolder.AddMoneyToTheVendingMachineMoneyStock(newVendingMachineMoney);
                return true;
            }
        }

        public static bool AddMoneyToTheVendingMachineMoneyStock(string[,] inputArray)
        {
            for (int i = 0; i < Machine.moneyTypeCount; i++)
            {
                Machine.VendingMachineMoneyStock[i, 1] = Convert.ToString(Convert.ToInt32(inputArray[i, 1]) + Convert.ToInt32(Machine.VendingMachineMoneyStock[i, 1]));
            }
            //Core.DBConnection.VendingMachineMoneyDB.UpdateMoneyDatabase(inputArray);
            return true;
        }

        public static void ListVendingMachineMoneyStock()
        {
            for (int i = 0; i < Machine.moneyTypeCount; i++)
            {
                for (int j = 0; j < Machine.moneyTableColumnCount; j++)
                {
                    Console.Write("{0, -20} ", Machine.VendingMachineMoneyStock[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
}
