using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System
{
    public class POsModel
    {
        public int Sequence { get; set; }
        public string ProductNo { get; set; }
        public bool VatType { get; set; }
        public decimal Quatity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal PriceTotal { get; set; }


    }
}
