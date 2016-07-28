using System;
using VendingMachine.Core.ENUM;

namespace VendingMachine.Machine
{
    public class MoneyHolder
    {
        public static bool DecreaseMoney(int[] changeAmount)
        {
            // 1 yeterli miktarda var mı 
            for (int i = 0; i < Machine.moneyTypeCount; i++)
            {
                if (Convert.ToDouble(Machine.VendingMachineMoneyStock[i, 0]) == 1)
                {
                    double tempDouble = Convert.ToDouble(Machine.VendingMachineMoneyStock[i, 1]);
                    tempDouble = tempDouble - changeAmount[0];
                    if (!(tempDouble < 0))
                    {
                        // 0.5 yeterli miktarda var mı
                        for (int j = 0; j < Machine.moneyTypeCount; j++)
                        {
                            if (Convert.ToDouble(Machine.VendingMachineMoneyStock[j, 0]) == 0.5)
                            {
                                double tempDouble2 = Convert.ToDouble(Machine.VendingMachineMoneyStock[j, 1]);
                                tempDouble2 = tempDouble2 - changeAmount[1];
                                if (!(tempDouble2 < 0))
                                {
                                    // 0.25 yeterli miktarda var mı 
                                    for (int k = 0; k < Machine.moneyTypeCount; k++)
                                    {
                                        if (Convert.ToDouble(Machine.VendingMachineMoneyStock[k, 0]) == 0.25)
                                        {
                                            double tempDouble3 = Convert.ToDouble(Machine.VendingMachineMoneyStock[k, 1]);
                                            tempDouble3 = tempDouble3 - changeAmount[2];
                                            if (!(tempDouble3 < 0))
                                            {
                                                Machine.VendingMachineMoneyStock[k, 1] = Convert.ToString(tempDouble3);
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                    }

                                    Machine.VendingMachineMoneyStock[j, 1] = Convert.ToString(tempDouble2);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        Machine.VendingMachineMoneyStock[i, 1] = Convert.ToString(tempDouble);
                        Core.DBConnection.VendingMachineMoneyDB.UpdateMoneyDatabase(Machine.VendingMachineMoneyStock);
                        //Core.DatabaseConnection.UpdateMoneyDatabase(Machine.VendingMachineMoneyStock);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }           
            return true;
        }

        public static bool IncreaseMoney(double inputMoney)
        {
            for (int i = 0; i < Machine.moneyTypeCount; i++)
            {
                if (Convert.ToDouble(Machine.VendingMachineMoneyStock[i, 0]) == inputMoney)
                {
                    double tempDouble = Convert.ToDouble(Machine.VendingMachineMoneyStock[i, 1]);
                    tempDouble++;
                    Machine.VendingMachineMoneyStock[i, 1] = Convert.ToString(tempDouble);
                    Core.DBConnection.VendingMachineMoneyDB.UpdateMoneyDatabase(Machine.VendingMachineMoneyStock);
                   // Core.DatabaseConnection.UpdateMoneyDatabase(Machine.VendingMachineMoneyStock);
                    return true;
                }
            }
            return false;
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
