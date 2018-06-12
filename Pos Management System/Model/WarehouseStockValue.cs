using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Model
{
   public class WarehouseStockValue
    {
        public int FKProduct { get; set; }
        public List<Details> ValueDetails { get; set; }
        public class Details
        {
            public int FKItemRemark { get; set; }
            public int FKProductDetail { get; set; }
            public int FKTransaction { get; set; }
            public decimal ActionQtyUnit { get; set; }
        }
    }
}
