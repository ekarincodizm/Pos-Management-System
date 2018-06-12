using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Repository
{
    public class c_WastManage_Details
    {
        #region รายละเอียดสินค้า
        public string ProductNo { get; set; }
        public string ProductDetail { get; set; }
        public string ProductUnit { get; set; }
        /// <summary>
        /// จำนวน
        /// </summary>
        public decimal QTY { get; set; }
        public decimal Packsize { get; set; }
        /// <summary>
        /// ราคา
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// หมายเหตุรายการ
        /// </summary>
        public string DescriptionList { get; set; }
        #endregion

        #region สรุปรวม
        /// <summary>
        /// รวมชิ้น
        /// </summary>
        public decimal TotalPiece { get; set; }
        /// <summary>
        /// รวมรายการ
        /// </summary>
        public decimal TotalList { get; set; }
        /// <summary>
        /// ราคารวม
        /// </summary>
        public decimal TotalPrice { get; set; }
        public string ThaiBath { get; set; }
        #endregion
    }
}
