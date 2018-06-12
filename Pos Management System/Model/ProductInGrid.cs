using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Model
{
    /// <summary>
    /// ใช้หน้าจัดการสินค้า ใน Grid
    /// </summary>
    public class ProductInGrid
    {
        public int Number { get; set; }
        public string ProductNo { get; set; }
        public decimal PackSize { get; set; }
        public int Unit { get; set; }
        public decimal Cost { get; set; }
        public decimal CostAndVat { get; set; }
        public bool PrintLabel { get; set; }


    }
}
