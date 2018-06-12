using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Repository
{
    public class c_CN_Details
    {
        #region รายละเอียดสินค้า
        public string ProductNo { get; set; }
        public string ProductDetail { get; set; }
        public string ProductUnit { get; set; }
        /// <summary>
        /// จำนวน
        /// </summary>
        public decimal QTY { get; set; }
        /// <summary>
        /// ส่วนลด
        /// </summary>
        public decimal DiscountCoupon { get; set; }
        /// <summary>
        /// ราคาต่อหน่วย
        /// </summary>
        public decimal PricePerUnit { get; set; }
        public string PricePerUnitString { get; set; }
        /// <summary>
        /// รวมราคา
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// ของแถม
        /// </summary>
        public decimal Giveaway { get; set; }
        #endregion

        #region สรุปยอด
        /// <summary>
        /// รวมจำนวนชิ้น
        /// </summary>
        public decimal TotalPiece { get; set; }
        /// <summary>
        /// รวมทั้งหมดมีกี่รายการ
        /// </summary>
        public decimal TotalList { get; set; }
        /// <summary>
        /// ราคารวมทุกรายการ
        /// </summary>
        public decimal TotalPriceList { get; set; }
        /// <summary>
        /// รวมส่วนลด
        /// </summary>
        public decimal TotalDiscount { get; set; }
        /// <summary>
        /// หักมัดจำ
        /// </summary>
        public decimal DelDeposit { get; set; }
        /// <summary>
        /// มูลค่ายกเว้นภาษี
        /// </summary>
        public decimal ExceptTax { get; set; }
        /// <summary>
        /// ค่าภาษีมูลค่าเพิ่ม
        /// </summary>
        public decimal TotalTax { get; set; }
        /// <summary>
        /// มูลค่าสินค้าหลังหักส่วนลด
        /// </summary>
        public decimal AfterDelDiscount { get; set; }
        /// <summary>
        /// ภาษี
        /// </summary>
        public decimal Tax { get; set; }
        /// <summary>
        /// ยกเว้นภาษี
        /// </summary>
        public decimal NoTax { get; set; }
        /// <summary>
        /// มูลค่าสุทธิ
        /// </summary>
        public decimal NetValue { get; set; }
        /// <summary>
        /// จำนวนเงินเป็นบาท เป็นภาษาไทย
        /// </summary>
        public string ThaiBath { get; set; }
        #endregion
    }
}
