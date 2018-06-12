using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Repository
{
    public class POHeader
    {
        #region Div 1 ชื่อองกรณ์
        public string CompName { get; set; }
        public string CompAddress { get; set; }
        public string CompTax { get; set; }
        public string CompTel { get; set; }
        public string CompEmail { get; set; }
        public string CompFax { get; set; }
        public string CompDescription { get; set; }
        public string CreateBy { get; set; }
        public string VatType { get; set; }
        #endregion

        #region Div 1.1 Vendor and Target
        public string VendorCode { get; set; }
        public string VendorName { get; set; }        
        public string VendorAddress { get; set; }
        public string TargetName { get; set; }
        public string TargetAddress { get; set; }
        #endregion

        #region Div 2 รายละเอียด PO
        public byte[] Barcode { get; set; }
        public string PONo { get; set; }
        public string PODate { get; set; }
        public string BudgetYear { get; set; }
        public string ReferNo { get; set; }
        public string DuaDate { get; set; }
        public string POExpire { get; set; }
        public string PaymentType { get; set; }
        #endregion

        #region Div 3 ยอดรวม    
        public string SumQty { get; set; }
        public string SumGift { get; set; }
        public string SumPrice { get; set; }
        public string SumDiscount { get; set; }
        public string SumTotal { get; set; }
        public string Remark { get; set; }
        public string ThaiBath { get; set; }
        #endregion

        #region Div 4 ยอดรวม Total
        public string TotalAmount { get; set; }
        public string DiscountPercent { get; set; }
        public string TotalDiscount { get; set; }
        public string TotalAfterDis { get; set; }
        public string TotalVat { get; set; }
        public string TotalBalance { get; set; }
        #endregion

        public List<POProductDetails> PODetails { get; set; }
    }
}
