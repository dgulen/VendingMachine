using System;
using VendingMachine.Core.ENUM;

namespace VendingMachine.Machine
{
    public class ProductHolder
    {
        
        public static void AddProductToTheVendingMachine(string [,] newVendingMachineProducts)
        {
            for (int i = 0; i < Machine.rowSize; i++)
            {
                Machine.VendingMachineProducts[i, 1] = Convert.ToString(Convert.ToInt32(newVendingMachineProducts[i, 1]) + Convert.ToInt32(Machine.VendingMachineProducts[i, 1]));
            }
        }

        public static void TakeProductFromTheVendingMachine(int ProductID)
        {
            Machine.VendingMachineProducts[ProductID-1, 1] = Convert.ToString(Convert.ToInt32(Machine.VendingMachineProducts[ProductID-1, 1]) - 1) ;
            Machine.InitializeVendingMachineProductStock();
            Core.DBConnection.VendingMachineProductsDB.UpdateProductDatabase(Machine.VendingMachineProducts);
            //Core.DatabaseConnection.UpdateProductDatabase(Machine.VendingMachineProducts);
        }

        public static bool AddFromFileToVendingMachine(string inFile)
        {
            int _rowSize = Machine.rowSize;
            int _columnSize = Machine.columnSize;
            RetriewData.GetData(inFile, ref _rowSize, ref _columnSize);
            String[,] newVendingMachineProducts = new string[_rowSize, _columnSize];
            newVendingMachineProducts = RetriewData.GetData(inFile, ref _rowSize, ref _columnSize);
            if (newVendingMachineProducts[0, 0] == Convert.ToString(ErrorOutput.UNSUCCESSFUL))
                return false;
            for (int i = 0; i < Machine.rowSize; i++)
            {
                if (Convert.ToInt32(newVendingMachineProducts[i, 1]) > Machine.MaxCapacity)
                {
                    Console.WriteLine("Too much {0}. Products need to be less than {1}.", newVendingMachineProducts[i, 0], Machine.MaxCapacity);
                    return false;
                }
            }
            bool fileFormat = RetriewData.CheckFileFormat(newVendingMachineProducts, Machine.VendingMachineProducts, Machine.rowSize, Machine.columnSize);

            if (!fileFormat)
            {
                return false;
            }
            else
            {
                ProductHolder.AddProductToTheVendingMachine(newVendingMachineProducts);
                return true;
            }
        }

        public static void ListVendingMachineProducts()
        {
            for (int i = 0; i < Machine.rowSize; i++)
            {
                for (int j = 0; j < Machine.columnSize; j++)
                {
                    Console.Write("{0, -20} ", Machine.VendingMachineProducts[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
