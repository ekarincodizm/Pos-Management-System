using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Repository
{
    public class c_SaleOrderWarehouse_Header
    {
        #region องค์กร
        public string NameShop { get; set; }
        public string AddressShop { get; set; }
        public string TelShop { get; set; }
        public string FaxShop { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// เลขประจำตัวผู้เสียภาษี
        /// </summary>
        public string TaxNoShop { get; set; }
        public string GenBarcode { get; set; }
        #endregion

        #region ลูกค้า
        public string NameMember { get; set; }
        /// <summary>
        /// รหัสลูกค้า
        /// </summary>
        public string NoMember { get; set; }
        #endregion

        #region ลูกหนี้
        public string NameDebtor { get; set; }
        /// <summary>
        /// รหัสลูกหนี้
        /// </summary>
        public string NoDebtor { get; set; }
        public string AddressDebtor { get; set; }
        #endregion

        #region รายละเอียดหน้าอินวอย
        /// <summary>
        /// เลขที่เอกสาร เก็บ ฟิล Code
        /// </summary>
        public string InvoiceNo { get; set; }
        /// <summary>
        /// วันที่เอกสาร
        /// </summary>
        public string InvoicDate { get; set; }
        /// <summary>
        /// เลขที่อ้างอิง เก็บ ฟิล CodeRefer
        /// </summary>
        public string CodeRefer { get; set; }
        /// <summary>
        /// วิธีจัดส่ง
        /// </summary>
        public string DeliveryType { get; set; }
        /// <summary>
        /// ผู้สร้าง
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// วันที่ปริ้น
        /// </summary>
        public string PrintDate { get; set; }
        /// <summary>
        /// เลขที่ใบกํากับภาษี เก็บ ฟิล InoviceNo
        /// </summary>
        public string TaxInvoice { get; set; }

        #endregion

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
        public string Price { get; set; }
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
        #endregion
    }
}
