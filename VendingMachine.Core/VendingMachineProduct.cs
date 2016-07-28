using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace VendingMachine.Core
{
    [DelimitedRecord(";")]
    public class VendingMachineProduct
    {
        public string Name;
        public string Count;
        public string Price;
        public string SlotNo;
    }

    [DelimitedRecord(";")]
    public class VendingMachineMoney
    {
        public string MoneyType;
        public string Count;
    }


}
