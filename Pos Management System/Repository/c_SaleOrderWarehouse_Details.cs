using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Repository
{
    public partial class c_SaleOrderWarehouse_Details
    {
        /// <summary>
        /// รหัสสินค้า
        /// </summary>
        public string ProductNo { get; set; }
        /// <summary>
        /// รายละเอียดสินค้า
        /// </summary>
        public string ProductDetails { get; set; }
        /// <summary>
        /// หน่วยนับ
        /// </summary>
        public string ProductUnit { get; set; }
        /// <summary>
        /// จำนวน
        /// </summary>
        public decimal QTY { get; set; }
        /// <summary>
        /// จำนวน สติง
        /// </summary>
        public string QTYString { get; set; }
        /// <summary>
        /// ราคา
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// จำนวนเงิน
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// จำนวนเงิน สติง
        /// </summary>
        public string AmountString { get; set; }
        /// <summary>
        /// ส่วนลด
        /// </summary>
        public decimal Discount { get; set; }
        public string DiscountString { get; set; }
        /// <summary>
        /// countList
        /// </summary>
        public int countList { get; set; }

        #region คำนวนมูลค่าสุทธิ ต่างๆ
        /// <summary>
        /// จำนวนชิ้น
        /// </summary>
        public string Piece { get; set; }
        /// <summary>
        /// จำนวนรายการ
        /// </summary>
        public string ListReport { get; set; }
        /// <summary>
        /// จำนวนเงินรวมทั้งหมด
        /// </summary>
        public string TotalAmount { get; set; }
        /// <summary>
        /// ส่วนลด
        /// </summary>
        public string TotalDiscount { get; set; }
        /// <summary>
        /// มูลค่าหลังหักส่วนลด
        /// </summary>
        public string BeforeDiscount { get; set; }
        /// <summary>
        /// ภาษี
        /// </summary>
        public string Tax { get; set; }
        /// <summary>
        /// มูลค่าสุทธิ
        /// </summary>
        public string NetValue { get; set; }
        /// <summary>
        /// มูลค่าสุทธิเป็นภาษาไทย
        /// </summary>
        public string Thaibath { get; set; }
        #endregion
    }

}
