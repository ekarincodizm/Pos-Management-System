using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.ModelForPaper
{
    public class SaleOrderWarehouseList
    {
        public string BranchName { get; set; }
        public string BranchNo { get; set; }
        public string BranchAddress { get; set; }
        public string BranchTax { get; set; }
        public string BranchTelAndFax { get; set; }

        public string DocNo { get; set; }
        public string DocDate { get; set; }        
        public string DocReference { get; set; }
        public string PaymentType { get; set; }
        public string DeliveryType { get; set; }
        public string PrintDate { get; set; }

        public string DebtorCode { get; set; }
        public string DebtorName { get; set; }
        public string DebtorAddress { get; set; }

        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        /// Summary
        public string TotalQty { get; set; }
        public string Total { get; set; }
        public string TotalDiscount { get; set; }
        public string TotalPrice { get; set; }

        public string Remark { get; set; }
        public string ThaiBath { get; set; }
        public string TotalAfterDis { get; set; }
        public string TotalVat { get; set; }
        public string TotalBalance { get; set; }

        public List<Details> DataDetails { get; set; }
        public class Details
        {
            public string Number { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Unit { get; set; }
            public string Qty { get; set; }
            public string PricePerUnit { get; set; }
            public string Discount { get; set; }
            public string AllPrice { get; set; }
        }


    }
}
