using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Repository
{
    public class c_WastManage_Header
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
        #endregion

        #region สาเหตุต้นคลัง หมายเหตุ
        /// <summary>
        /// สาเหตุต้นคลัง
        /// </summary>
        public string Reson { get; set; }
        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region รายละเอียดหน้าอินวอย
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        /// <summary>
        /// วันที่พิมพ์
        /// </summary>
        public string PrintDate { get; set; }
        public string CreateBy { get; set; }
        #endregion
    }
}
