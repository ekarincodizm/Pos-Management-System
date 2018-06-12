using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System.Repository
{
    public class GetGoodsStorefrontUse
    {
        public static DataTable getHead(string code)
        {
            try
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    var getListData = db.GetGoodsStoreFrontDetails
                        .Where(w => w.Enable == true && w.GetGoodsStoreFront.Enable == true & w.GetGoodsStoreFront.Code == code).ToList();

                    var getShop = Singleton.SingletonAuthen.Instance().MyBranch;
                    var getWhoPrint = Singleton.SingletonAuthen.Instance().Name;

                    var TotalCostPerUnit = getListData.Sum(w => w.CostPerUnit);
                    var TotalQTY = getListData.Sum(w => w.Qty);
                    var TotalSalePrice = getListData.Sum(w => w.SellPricePerUnit * w.Qty);

                    var TotalCountList = getListData.Count();
                    var PrintDate = Library.ConvertDateToThaiDate(DateTime.Now, true);
                    string InvoiceDate = "";
                    string userCreate = "";
                    if (getListData.Count() > 0)
                    {
                        InvoiceDate = Library.ConvertDateToThaiDate(getListData.FirstOrDefault().GetGoodsStoreFront.CreateDate, true);
                        userCreate = Library.GetFullNameUserById(getListData.FirstOrDefault().CreateBy);
                    }
                    else
                    {
                        InvoiceDate = "";
                        userCreate = "";
                    }
                    var getData = getListData
                        .Select(w => new
                        {
                            NameShop = getShop.Name + " " + getShop.BranchNo,
                            AddressShop = getShop.Address,
                            TelShop = getShop.Tel,
                            Email = getShop.Email,
                            TaxNoShop = getShop.TaxNo,
                            FaxShop = getShop.Fax,
                            BranchSource = getShop.Name + " " + getShop.BranchNo,
                            BranchDestination = "",
                            CreateBy = userCreate,
                            InvoiceNo = w.GetGoodsStoreFront.Code,
                            InvoiceDate = InvoiceDate,
                            PrintDate = PrintDate,
                            PrintBy = getWhoPrint,
                            ProductNo = w.ProductDetails.Code,
                            ProductName = w.ProductDetails.Products.ThaiName,
                            ProductUnti = w.ProductDetails.ProductUnit.Name + "x" + w.ProductDetails.PackSize,
                            QTY = w.Qty,
                            Packsize = w.ProductDetails.PackSize,
                            CostPerUnit = w.CostPerUnit,
                            TypeTax = w.ProductDetails.Products.ProductVatType.Name,
                            SalePrice = w.SellPricePerUnit,
                            TotalSalePrice = TotalSalePrice,
                            TotalQTY = TotalQTY, // จำนวนรวม
                            TotalCostPerUnit = w.GetGoodsStoreFront.TotalBalance, // ราคารวม
                            TotalCountList = TotalCountList,
                            ThaiBath = Library.ThaiBaht(w.GetGoodsStoreFront.TotalBalance.ToString()),
                            Warehouse = "หน้าร้าน",
                            Description = w.GetGoodsStoreFront.Description
                        }).ToList();
                    var dt = Library.ConvertToDataTable(getData);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
    }
}
