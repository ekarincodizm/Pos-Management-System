using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Repository
{
    public class c_CN_Header
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
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        /// <summary>
        /// อ้างอิง
        /// </summary>
        public string Refer { get; set; }
        #endregion
    }
}
