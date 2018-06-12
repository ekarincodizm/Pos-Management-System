using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System.Repository
{
    public static class TransferOut
    {
        public static DataTable getHead(string code, string brachDestination)
        {
            try
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    var getListData = db.StoreFrontTransferOutDtl
                        .Where(w => w.Enable == true && w.StoreFrontTransferOut.Enable == true & w.StoreFrontTransferOut.Code == code).ToList();

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
                        InvoiceDate = Library.ConvertDateToThaiDate(getListData.FirstOrDefault().StoreFrontTransferOut.CreateDate, true);
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
                            BranchDestination = brachDestination,
                            CreateBy = userCreate,
                            InvoiceNo = w.StoreFrontTransferOut.Code,
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
                            TotalCostPerUnit = w.StoreFrontTransferOut.TotalBalance, // ราคารวม
                            TotalCountList = TotalCountList,
                            ThaiBath = Library.ThaiBaht(w.StoreFrontTransferOut.TotalBalance.ToString()),
                            Warehouse = "หน้าร้าน",
                            Description = w.StoreFrontTransferOut.Remark
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
