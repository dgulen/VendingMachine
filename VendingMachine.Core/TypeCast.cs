using System;
using System.Configuration;

namespace VendingMachine.Core
{
    public class TypeCast
    {
        public static int ErrorCode = Convert.ToInt32(ConfigurationManager.AppSettings["ErrorCode"]); 

        public static int ConsoleToInt(string consoleInput)
        {
            int result;

            if (!Int32.TryParse(consoleInput, out result))
            {
                Console.WriteLine("Error converting to int ");
                return ErrorCode;
            }
            else
            {
                return result;
            }
        }
    }
}
