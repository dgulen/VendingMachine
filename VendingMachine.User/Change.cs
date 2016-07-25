using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.User
{
    public class Change
    {
        public static int[] CalculateChange(double userInputMoneyTotal, double currentProductPrice)
        {
            int count;
            int[] changeAmount = new int[3];
            Array.Clear(changeAmount, 0, 3);
            double change = userInputMoneyTotal - currentProductPrice;

            while (change != 0)
            {
                if (change >= 1)
                {
                    count = Convert.ToInt32(Math.Floor(change)) / 1;
                    change = change - count;
                    changeAmount[0] = count;
                }
                else
                {
                    count = Convert.ToInt32(change / 0.5);

                    if (count < 1)
                    {
                        count = Convert.ToInt32(change / 0.25);
                        changeAmount[2] = count;
                        change = change - (count * 0.25);
                    }
                    else
                    {
                        changeAmount[1] = count;
                        change = change - (count * 0.5);
                    }
                }
            }
            return changeAmount;
        }
    }
}