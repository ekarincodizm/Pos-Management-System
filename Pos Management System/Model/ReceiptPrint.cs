using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Model
{
    public class ReceiptPrint
    {
        public string CompanyName { get; set; }
        public string CompanyBranch { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyTel { get; set; }
        public string CompanyFax { get; set; }
        public string CompanyLineId { get; set; }
        public string CompanyTaxId { get; set; }
        public string CompanyRegId { get; set; }

        public List<ProductInReceipt> ProductInReceipts { get; set; }

        public string TotalBalance { get; set; }
        public string TotalCash { get; set; }
        public string TotalChange { get; set; }
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        /// <summary>
        /// สด หรือ เชื่อ
        /// </summary>
        public string OrderType { get; set; }
        public string TotalUnVat { get; set; }
        public string TotalHasVat { get; set; }
        public string TotalVat { get; set; }
        public string TotalList { get; set; }
        public string TotalUnit { get; set; }
        public string CashierName { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }


    }
}
