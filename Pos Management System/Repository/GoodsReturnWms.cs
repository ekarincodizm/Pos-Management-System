using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System.Repository
{
    // ใบส่งของคืนให้ vendor
    public static class GoodsReturnWms
    {
        public static DataTable getData(string code)
        {
            try
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    var getShop = Singleton.SingletonAuthen.Instance().MyBranch;

                    var getData = db.CNWarehouseDetails
                        .Where(w => w.CNWarehouse.Enable == true && w.CNWarehouse.Code == code).ToList();

                    //var _TotalQty = getData.Sum(w => w.Qty);
                    var _CountList = getData.Count();
                    var _TotalPriceList = getData.FirstOrDefault().CNWarehouse.TotalBalance;
                    var _TotalVat = getData.Sum(w => w.Vat);
                    var _TotalBeforeVat = getData.Sum(w => w.BeforeVat);
                    var GetThaiBath = Library.ThaiBaht(_TotalPriceList.ToString());
                    var InvoiceDate = Library.ConvertDateToThaiDate(getData.FirstOrDefault().CreateDate, true);
                    var PrintDate = Library.ConvertDateToThaiDate(DateTime.Now, true);

                    string name = Library.GetFullNameUserById(getData.FirstOrDefault().CreateBy);
                    var list = getData.Select(w => new
                    {
                        NameShop = getShop.Name + " " + getShop.BranchNo,
                        AddressShop = getShop.Address,
                        TelShop = getShop.Tel,
                        FaxShop = getShop.Fax,
                        EmailShop = getShop.Email,
                        TaxNoShop = getShop.TaxNo,

                        NoVendor = w.CNWarehouse.Vendor.Code,
                        NameVendor = w.CNWarehouse.Vendor.Name,
                        AddressVendor = w.CNWarehouse.Vendor.Address,

                        InvoiceNo = w.CNWarehouse.Code,
                        InvoiceDate = InvoiceDate,
                        CreateBy = name,
                        PrintDate = PrintDate,
                        Description = w.CNWarehouse.WasteReason.Name + " - " + w.CNWarehouse.Description,
                        ProductNo = w.ProductDetails.Code,
                        ProductName = w.ProductDetails.Products.ThaiName,
                        ProductUnit = w.ProductDetails.ProductUnit.Name + "x" + w.ProductDetails.PackSize,
                        Packsize = w.ProductDetails.PackSize,
                        PricePerUnit = w.PricePerUnit,
                        PriceBeforeVat = w.BeforeVat,
                        Vat = w.Vat,
                        QTY = w.Qty,
                        TotalPrice = w.TotalPrice, // ราคารวม
                        TotalQTY = w.CNWarehouse.TotalQtyUnit, // ผลรวมจำนวน
                        CountList = _CountList, // จำนวนรายการ
                        TotalPriceList = _TotalPriceList, // ราคารวม
                        TotalVat = _TotalVat, // รวมภาษี
                        TotalPriceBeforeVat = _TotalBeforeVat, // รวมก่อนภาษี
                        TotalNetValues = _TotalPriceList + ((_TotalPriceList * 7) / 100), //ราคาสุทธิ
                        ThaiBath = GetThaiBath
                    }).ToList();

                    var dt = Library.ConvertToDataTable(list);
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
