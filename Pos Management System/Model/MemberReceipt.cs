using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Model
{
    public class MemberReceipt
    {
        public string PrintDate { get; set; }
        public string Number { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string ReceiptNo { get; set; }
        public string Money { get; set; }
        public string MoneyAvg { get; set; }
        public string Cupon { get; set; }
        public string Total { get; set; }
        public string MoneyCash { get; set; }
        public string MoneyCupon { get; set; }
        public int PageNumber { get; set; }

    }
}
