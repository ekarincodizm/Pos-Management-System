using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Model
{
    /// <summary>
    /// map Query View_GetFrontStockByProductDate
    /// </summary>
    public class Map_GetFrontStockByProductDate
    {
        public int FKProduct { get; set; }
        public DateTime CreateDate { get; set; }
        public string DateCreate { get; set; }
        public string Barcode { get; set; }
        public int FKProductDetails { get; set; }
        public decimal ActionQty { get; set; }
        public decimal PackSize { get; set; }
        public string DocNo { get; set; }
        public int? DocDtlNumber { get; set; }
        public string DocRefer { get; set; }
        public int DocReferDtlNumber { get; set; }

        public string Code { get; set; }
        public string Unit { get; set; }
        public bool IsPlus { get; set; }
        public string TransName { get; set; }
    }
}
