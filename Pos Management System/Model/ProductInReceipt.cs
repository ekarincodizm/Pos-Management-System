using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Model
{
    /// <summary>
    /// สินค้า ใบเสร็จ
    /// </summary>
    public class ProductInReceipt
    {
        public Products Product { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Qty { get; set; }
        public string SalePrice { get; set; }
        public string Total { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// I Or #
        /// </summary>
        public string ProductVatType { get; set; }
    }
}
