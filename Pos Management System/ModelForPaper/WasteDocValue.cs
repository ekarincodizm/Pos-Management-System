using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.ModelForPaper
{
    public class WasteDocValue
    {
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string BudgetYear { get; set; }
        public string DocReference { get; set; }
        public string PrintDate { get; set; }

        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }

        public string TotalUnit { get; set; }
        public string TotalCost { get; set; }
        public string TotalBalance { get; set; }

    }
}
