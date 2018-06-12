using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Model
{
    public class SaleOrderWHForLibrary
    {
        /// <summary>
        /// FK
        /// </summary>
        public string PRODUCT_ID { get; set; }
        /// <summary>
        /// barcode
        /// </summary>
        public string PRODUCT_NO { get; set; }
        /// <summary>
        /// ชื่อ
        /// </summary>
        public string PRODUCT_NAME { get; set; }
        /// <summary>
        /// หน่วย
        /// </summary>
        public string UNIT_NAME { get; set; }
        /// <summary>
        /// จำนวน
        /// </summary>
        public string QTY { get; set; }
        /// <summary>
        /// ของแถม
        /// </summary>
        public string GIVEAWAY { get; set; }
        /// <summary>
        /// ราคา / หน่วย
        /// </summary>
        public string COST { get; set; }
        /// <summary>
        /// ส่วนลด
        /// </summary>
        public string DISCOUNT { get; set; }
        /// <summary>
        /// ยอดรวม
        /// </summary>
        public string COST_TOTAL { get; set; }

    }
}
