using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System.Repository
{
    public static class SaleOrderWarehouse
    {
        /// <summary>
        /// ข้อมูลส่วน head // สามาชิก ลูกหนี้ เลขที่เอกสาร ชื่อร้าน
        /// </summary>
        /// <param name="code">code ที่เลือกจาก datagrid</param>
        /// <returns></returns>
        public static List<c_SaleOrderWarehouse_Header> getHead(string code)
        {
            try
            {
                List<c_SaleOrderWarehouse_Header> ls = new List<c_SaleOrderWarehouse_Header>();
                using (SSLsEntities db = new SSLsEntities())
                {
                    var getData = db.SaleOrderWarehouse
                        .Include("Branch")
                        .Include("Member")
                        .Include("Debtor")
                        .Include("DeliveryType")
                        .FirstOrDefault(w => w.Code.Equals(code) && w.Enable == true);
                    c_SaleOrderWarehouse_Header c = new c_SaleOrderWarehouse_Header();
                    // head //
                    c.NameShop = getData.Branch.Name;
                    c.AddressShop = getData.Branch.Address;
                    c.TelShop = getData.Branch.Tel;
                    c.FaxShop = getData.Branch.Fax;
                    c.TaxNoShop = getData.Branch.TaxNo;
                    c.Email = getData.Branch.Email;
                    //c.GenBarcode =
                    // member //
                    c.NameMember = getData.Member.Name;
                    c.NoMember = getData.Member.Code;
                    // Debtop //
                    c.NameDebtor = getData.Debtor.Name;
                    c.AddressDebtor = getData.Debtor.Address;
                    c.NoDebtor = getData.Debtor.Code;
                    // invoice no //
                    c.InvoiceNo = getData.Code;
                    c.InvoicDate = getData.CreateDate.ToString("dd/MM/yyyy");
                    c.TaxInvoice = getData.InvoiceNo;
                    c.CodeRefer = getData.CodeRefer;
                    c.DeliveryType = getData.DeliveryType.Name;
                    c.CreateBy = getData.CreateBy;
                    c.PrintDate = DateTime.Now.ToString("dd/MM/yyyy");
                    ls.Add(c);
                    return ls;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
        /// <summary>
        /// list ข้อมูลทั้งหมด ที่จะนำไปแสดงใน table
        /// </summary>
        /// <param name="id">เลขที่ SaleOrderWarehouse where มาจาก code ที่เลือกหน้า datagrid</param>
        /// <returns></returns>
        public static List<c_SaleOrderWarehouse_Details> getList(int id)
        {
            try
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    List<c_SaleOrderWarehouse_Details> ls = new List<c_SaleOrderWarehouse_Details>();
                    List<c_SaleOrderWarehouse_Details> lsGroup = new List<c_SaleOrderWarehouse_Details>();
                    // list data //
                    var getDetail = db.SaleOrderWarehouseDtl
                        .Include("ProductDetails.Products")
                        .Where(w => w.FKSaleOrderWarehouse == id && w.Enable == true).ToList();
                    foreach (var dt in getDetail)
                    {
                        c_SaleOrderWarehouse_Details c = new c_SaleOrderWarehouse_Details();
                        c.ProductNo = dt.ProductDetails.Code;
                        c.ProductDetails = dt.ProductDetails.Products.ThaiName;
                        c.ProductUnit = dt.ProductDetails.ProductUnit.Name;

                        c.QTY = dt.Qty; // สำหรับ sum
                        c.QTYString = dt.Qty.ToString(); // เอาไปแสดง report view

                        c.Price = dt.ProductDetails.SellPrice;

                        c.DiscountString = dt.BathDiscount.ToString(); // ส่วนลด
                        c.Discount = dt.BathDiscount;

                        c.AmountString = dt.TotalPrice.ToString();
                        c.Amount = dt.TotalPrice; // จำนวนเงิน
                        ls.Add(c);
                    }
                    var groupData = ls.GroupBy(a => new { a.ProductNo })
                    .Select(g => new { Key = g.Key, Count = g.Count() });
                    foreach (var dt in groupData)
                    {
                        c_SaleOrderWarehouse_Details c = new c_SaleOrderWarehouse_Details();
                        c.ProductNo = ls.FirstOrDefault(w => w.ProductNo == dt.Key.ProductNo).ProductNo;
                        c.ProductDetails = ls.FirstOrDefault(w => w.ProductNo == dt.Key.ProductNo).ProductDetails;
                        c.ProductUnit = ls.FirstOrDefault(w => w.ProductNo == dt.Key.ProductNo).ProductUnit;
                        c.QTY = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.QTY);
                        c.QTYString = Library.ConvertDecimalToStringForm(ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.QTY));
                        c.Price = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.Price);
                        c.DiscountString = Library.ConvertDecimalToStringForm(ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.Discount));
                        c.Discount = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.Discount);
                        c.AmountString = Library.ConvertDecimalToStringForm(ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.Amount));
                        c.Amount = ls.Where(w => w.ProductNo == dt.Key.ProductNo).Sum(w => w.Amount);
                        lsGroup.Add(c);
                    }
                    return lsGroup;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
        /// <summary>
        /// ผลรวมทั้งหมด เช่นรวมสุทธิ
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public static List<c_SaleOrderWarehouse_Details> getTotal(List<c_SaleOrderWarehouse_Details> ls)
        {
            try
            {
                List<c_SaleOrderWarehouse_Details> lss = new List<c_SaleOrderWarehouse_Details>();
                c_SaleOrderWarehouse_Details a = new c_SaleOrderWarehouse_Details();
                var CountList = ls.Count();
                var sumQTY = ls.Sum(w => w.QTY);
                var sumAmount = ls.Sum(w => w.Amount);
                var sumDiscount = ls.Sum(w => w.Discount);
                a.countList = CountList;
                a.Piece = Library.ConvertDecimalToStringForm(sumQTY);
                a.TotalAmount = Library.ConvertDecimalToStringForm(sumAmount);
                a.TotalDiscount = Library.ConvertDecimalToStringForm(sumDiscount);
                a.BeforeDiscount = Library.ConvertDecimalToStringForm(sumAmount - sumDiscount); // มูลค่าหลังหักส่วนลด
                a.Tax = Library.ConvertDecimalToStringForm(((sumAmount - sumDiscount) * 7) / 100); // ภาษี
                a.NetValue = Library.ConvertDecimalToStringForm((sumAmount - sumDiscount) - (((sumAmount - sumDiscount) * 7) / 100)); // มูลค่าสุทธิ
                a.Thaibath = Library.ThaiBaht(((sumAmount - sumDiscount) - (((sumAmount - sumDiscount) * 7) / 100)).ToString());
                
                lss.Add(a);
                return lss;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
    }
}
