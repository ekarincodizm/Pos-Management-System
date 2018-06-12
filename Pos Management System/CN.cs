using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System.Repository
{
    public static class CN
    {
        /// <summary>
        /// รายละเอียดส่วน header ชื่อ สมาชิก ลูกหนี้ ชื่อร้าน เลขที่เอกสาร
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<c_CN_Header> getHead(string code)
        {
            try
            {
                List<c_CN_Header> ls = new List<c_CN_Header>();
                using (SSLsEntities db = new SSLsEntities())
                {
                    var getShop = Singleton.SingletonAuthen.Instance().MyBranch;
                    c_CN_Header cn = new c_CN_Header();
                    // ชื่อร้านชื่อสาขา
                    cn.NameShop = getShop.Name + " " + getShop.BranchNo;
                    cn.AddressShop = getShop.Address;
                    cn.TelShop = getShop.Tel;
                    cn.FaxShop = getShop.Fax;
                    cn.Email = getShop.Email;
                    cn.TaxNoShop = getShop.TaxNo;
                    var getCN = db.CNHeader
                        .Include("PosHeader.Member")
                        .Include("PosHeader.Debtor")
                        .FirstOrDefault(w => w.No == code && w.Enable == true);
                    // ชื่อ สมาชิกและลูกหนี้
                    cn.NoMember = getCN.PosHeader.Member.Code;
                    cn.NameMember = getCN.PosHeader.Member.Name;
                    if (getCN.PosHeader.FKDebtor == null)
                    {
                        cn.NoDebtor = "-";
                        cn.NameDebtor = "-";
                        cn.AddressDebtor = "-";
                    }
                    else
                    {
                        cn.NoDebtor = getCN.PosHeader.Debtor.Code;
                        cn.NameDebtor = getCN.PosHeader.Debtor.Name;
                        cn.AddressDebtor = getCN.PosHeader.Debtor.Address;
                    }

                    // เลขที่เอกสาร อ้างอิง วันที่เอกสาร
                    cn.InvoiceNo = getCN.No;
                    cn.InvoiceDate = Library.ConvertDateToThaiDate(getCN.CreateDate, true);
                    cn.Refer = Library.GetFullNameUserById(getCN.CreateBy); // อ้างอิง
                    ls.Add(cn);
                }
                return ls;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">FKPOsHeader</param>
        /// <returns></returns>
        public static List<c_CN_Details> getList(string cnNo)
        {
            try
            {
                List<c_CN_Details> ls = new List<c_CN_Details>();
                List<c_CN_Details> lsGroup = new List<c_CN_Details>();
                using (SSLsEntities db = new SSLsEntities())
                {
                    var data = db.CNHeader.SingleOrDefault(w => w.Enable == true && w.No == cnNo);
                    foreach (var dtl in data.CNDetails.Where(w => w.Enable == true).ToList())
                    {
                        c_CN_Details cn = new c_CN_Details();
                        cn.ProductNo = dtl.PosDetails.ProductDetails.Code;
                        cn.ProductDetail = dtl.PosDetails.ProductDetails.Products.ThaiName;
                        cn.ProductUnit = dtl.PosDetails.ProductDetails.ProductUnit.Name;
                        cn.QTY = dtl.CNQty;
                        cn.DiscountCoupon = dtl.CNDisShop; // ส่วนลดร้าน
                        cn.PricePerUnit = dtl.PricePerUnit;
                        cn.TotalPrice = dtl.CNTotalPrice;
                        cn.Tax = dtl.CNHeader.TotalVat; // ภาษี
                        cn.NoTax = dtl.CNHeader.TotaNoVat; // ยกเว้นภาษี
                        ls.Add(cn);
                    }

                    var groupData = ls.GroupBy(a => new { a.ProductNo })
                    .Select(g => new { Key = g.Key, Count = g.Count() });
                    foreach (var dt in groupData)
                    {
                        c_CN_Details cn = new c_CN_Details();
                        cn.ProductNo = ls.FirstOrDefault(w => w.ProductNo == dt.Key.ProductNo).ProductNo;
                        cn.ProductDetail = ls.FirstOrDefault(w => w.ProductNo == dt.Key.ProductNo).ProductDetail;
                        cn.ProductUnit = ls.FirstOrDefault(w => w.ProductNo == dt.Key.ProductNo).ProductUnit;
                        cn.QTY = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.QTY);
                        cn.DiscountCoupon = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.DiscountCoupon); // ส่วนลดร้าน
                        cn.PricePerUnit = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.PricePerUnit);
                        cn.PricePerUnitString = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.PricePerUnit).ToString();
                        cn.TotalPrice = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.TotalPrice);
                        cn.Tax = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.Tax); // ภาษี
                        cn.NoTax = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.NoTax); // ยกเว้นภาษี
                        lsGroup.Add(cn);
                    }
                }
                return lsGroup;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
        /// <summary>
        /// ผลรวมทั้งหมด
        /// </summary>
        /// <returns></returns>
        public static List<c_CN_Details> getTotal(List<c_CN_Details> lsdtl)
        {
            try
            {
                List<c_CN_Details> ls = new List<c_CN_Details>();
                using (SSLsEntities db = new SSLsEntities())
                {
                    c_CN_Details cn = new c_CN_Details();
                    cn.TotalPiece = lsdtl.Sum(w => w.QTY); // รวมชิ้น
                    cn.TotalList = lsdtl.Count(); // รวมจำนวนรายการ
                    cn.TotalPriceList = lsdtl.Sum(w => w.TotalPrice); // รวมราคา
                    cn.TotalDiscount = lsdtl.Sum(w => w.DiscountCoupon); // รวมส่วนลด
                    cn.AfterDelDiscount = lsdtl.Sum(w => w.TotalPrice) - lsdtl.Sum(w => w.DiscountCoupon); // หักส่วนลด
                    cn.TotalTax = (lsdtl.Sum(w => w.TotalPrice) * 7) / 100; // ภาษี
                    cn.ExceptTax = lsdtl.Sum(w => w.NoTax); // รวมยกเว้นภาษี
                                                            // ราคาสุทธิ  = ราคารวม - (รวมยกเว้นภาษี + รวมส่วดลด + ภาษี) 
                    cn.NetValue = lsdtl.Sum(w => w.TotalPrice) - (lsdtl.Sum(w => w.NoTax) + lsdtl.Sum(w => w.TotalDiscount) + ((lsdtl.Sum(w => w.TotalPrice) * 7)) / 100);
                    cn.ThaiBath = Library.ThaiBaht((lsdtl.Sum(w => w.TotalPrice) - (lsdtl.Sum(w => w.NoTax) + lsdtl.Sum(w => w.TotalDiscount) + ((lsdtl.Sum(w => w.TotalPrice) * 7)) / 100)).ToString());
                    ls.Add(cn);
                }
                return ls;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}
