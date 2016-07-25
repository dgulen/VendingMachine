using System;

namespace VendingMachine.Admin
{
    public class AdminInput
    {
        public static bool ValidateAdminInput(string adminInput)
        {
            int choice;
            if (!Int32.TryParse(adminInput, out choice))
            {
                return false;
            }
            else if (choice == (int)Core.ENUM.AdminOptions.UPLOAD_PRODUCTS || choice == (int)Core.ENUM.AdminOptions.UPLOAD_MONEY || choice == (int)Core.ENUM.AdminOptions.REPORT_MODULE)
            {
                return true;
            }
            else return false;
        }
    }
}
