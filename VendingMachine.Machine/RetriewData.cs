using System;
using System.IO;
using VendingMachine.Core.ENUM;

namespace VendingMachine.Machine
{
    public class RetriewData
    {
        public static string[,] GetData(string FileName, ref int rowSize, ref int columnSize)
        {
            string[,] retriewedData;

            StreamReader readStock;
            try
            {
                readStock = new StreamReader(FileName);
                // TODO: file path kontrol et
                String line;

                if (readStock == null)
                {
                    Console.WriteLine("Error, file operation is not created. ");
                    retriewedData = new string[1, 1];
                    retriewedData[0, 0] = Convert.ToString(ErrorOutput.UNSUCCESSFUL);
                    return retriewedData;
                }

                var lines = File.ReadAllLines(FileName);
                rowSize = lines.Length;
                columnSize = lines[0].Split(';').Length;
                retriewedData = new String[rowSize, columnSize];

                int rowIndex = 0;
                while ((line = readStock.ReadLine()) != null)
                {
                    string[] values = line.Split(';');
                    int columnIndex = 0;
                    foreach (var value in values)
                    {
                        retriewedData[rowIndex, columnIndex] = value;
                        columnIndex++;
                    }
                    rowIndex++;
                }
                return retriewedData;
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                retriewedData = new string[1,1];
                retriewedData[0,0] = Convert.ToString(ErrorOutput.UNSUCCESSFUL);
                return retriewedData;
            }

            
        }

        public static bool CheckFileFormat(string[,] newArray, string[,] originalArray, int rowSize, int columnSize)
        {
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < columnSize; j++)
                {
                    if (j != 1)
                    {
                        if (newArray[i, 0] != originalArray[i, 0])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
