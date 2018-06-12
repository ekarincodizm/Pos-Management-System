using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System.Repository
{
    public static class WastManage
    {
        /// <summary>
        /// ชื่อบริษัท สาเหตุต้นคลัง หมายเหตุ เลขที่เอกสาร
        /// </summary>
        /// <param name="code">รหัสใบของเสีย</param>
        /// <returns></returns>
        public static List<c_WastManage_Header> getHead(string code)
        {
            try
            {
                List<c_WastManage_Header> ls = new List<c_WastManage_Header>();
                using (SSLsEntities db = new SSLsEntities())
                {
                    var getShop = db.StoreFrontTransferWaste.FirstOrDefault(w => w.Code == code && w.Enable == true);
                    c_WastManage_Header wh = new c_WastManage_Header();
                    // branchShop
                    wh.NameShop = getShop.Warehouse.Branch.Name + " " + getShop.Warehouse.Branch.BranchNo;
                    wh.AddressShop = getShop.Warehouse.Branch.Address;
                    wh.TelShop = getShop.Warehouse.Branch.Tel;
                    wh.FaxShop = getShop.Warehouse.Branch.Fax;
                    wh.Email = getShop.Warehouse.Branch.Email;
                    wh.TaxNoShop = getShop.Warehouse.Branch.TaxNo;
                    // สาเหตุต้นคลัง หมายเหตุ
                    wh.Reson = getShop.WasteReason.Name;
                    wh.Description = getShop.Description;
                    // เลขที่เอกสาร
                    wh.InvoiceNo = getShop.Code;
                    wh.InvoiceDate = getShop.CreateDate.ToString("dd/MM/yyyy");
                    wh.PrintDate = DateTime.Now.ToString("dd/MM/yyyy");
                    ls.Add(wh);
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
        /// รายการของเสีย
        /// </summary>
        /// <param name="code">รหัสใบของเสีย</param>
        /// <returns></returns>
        public static List<c_WastManage_Details> getList(string code)
        {
            try
            {
                List<c_WastManage_Details> ls = new List<c_WastManage_Details>();
                List<c_WastManage_Details> lsGroup = new List<c_WastManage_Details>();
                using (SSLsEntities db = new SSLsEntities())
                {
                    var getHead = db.StoreFrontTransferWaste.FirstOrDefault(w => w.Enable == true && w.Code == code);
            
                    foreach (var sf in getHead.StoreFrontTransferWasteDtl.Where(w=>w.Enable == true).ToList())
                    {
                        c_WastManage_Details wm = new c_WastManage_Details();
                        wm.ProductNo = sf.ProductDetails.Code; // รหัสสินค้า
                        wm.ProductDetail = sf.ProductDetails.Products.ThaiName;
                        wm.ProductUnit = sf.ProductDetails.ProductUnit.Name;
                        wm.QTY = sf.Qty; // ชิ้น
                        wm.Price = sf.CostPerUnit; // ราคาต่อหน่วย
                        wm.Packsize = sf.ProductDetails.PackSize;
                        // หมายเหตุ
                        wm.DescriptionList = sf.Description;
                        ls.Add(wm);
                    }
                    var groupData = ls.GroupBy(a => new { a.ProductNo })
                    .Select(g => new { Key = g.Key, Count = g.Count() });
                    foreach (var dt in groupData)
                    {
                        c_WastManage_Details wm = new c_WastManage_Details();
                        wm.ProductNo = ls.FirstOrDefault(w => w.ProductNo == dt.Key.ProductNo).ProductNo;
                        wm.ProductDetail = ls.FirstOrDefault(w => w.ProductNo == dt.Key.ProductNo).ProductDetail;
                        wm.ProductUnit = ls.FirstOrDefault(w => w.ProductNo == dt.Key.ProductNo).ProductUnit;
                        wm.QTY = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.QTY); // ชิ้น
                        wm.Price = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.Price); // ราคาต่อหน่วย
                        wm.Packsize = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.Packsize);
                        // หมายเหตุ
                        wm.DescriptionList = ls.FirstOrDefault(w => w.ProductNo == dt.Key.ProductNo).DescriptionList;
                        lsGroup.Add(wm);
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
        /// ผลรวมข้อมูลทั้งหมด
        /// </summary>
        /// <param name="lsData"></param>
        /// <returns></returns>
        public static List<c_WastManage_Details> getTotal(List<c_WastManage_Details> lsData)
        {
            try
            {
                List<c_WastManage_Details> ls = new List<c_WastManage_Details>();
                using (SSLsEntities db = new SSLsEntities())
                {
                    var sumPiece = lsData.Sum(w => w.QTY); // จำนวนชิ้น
                    var sumList = lsData.Count(); // รายการทั้งหมด
                    var sumPrice = lsData.Sum(w => w.Price); // ราคารวมทั้งหมด
                    c_WastManage_Details wm = new c_WastManage_Details();
                    wm.TotalPiece = sumPiece;
                    wm.TotalList = sumList;
                    wm.TotalPrice = sumPrice;
                    wm.ThaiBath = Library.ThaiBaht(sumPrice.ToString());
                    ls.Add(wm);
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
