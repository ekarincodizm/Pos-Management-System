using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Model
{
    public class ObjectPOsTransaction
    {
        public int FKTransaction { get; set; }
        public decimal ActionQty { get; set; }
        public ProductDetails ProductDetail { get; set; }
    }
}
