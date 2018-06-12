
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Repository
{
    public class Adjust
    {
        public static DataTable getData(string code)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var ad = db.AdjustStoreFrontDetail.Where(w => w.Enable == true &&
                w.AdjustStoreFront.Enable == true && w.AdjustStoreFront.Code == code).ToList();

                var convertInvoiceData = Library.ConvertDateToThaiDate(ad.FirstOrDefault().CreateDate, true);
                var TotalList = ad.Count();
                var TotalPiece = ad.Sum(s => s.Qty);
                var TotalCostPerUnit = ad.Sum(s => s.Qty * s.CostPerUnit);
                var TotalSellPricePerUnit = ad.Sum(s => s.Qty * s.SellPricePerUnit);
                var createBy = Library.GetFullNameUserById(ad.FirstOrDefault().CreateBy);
                var selectData = ad.Select(s =>
                new
                {
                    InvoiceNo = s.AdjustStoreFront.Code,
                    InvoiceDate = convertInvoiceData,
                    CreateBy = createBy,

                    ProductNo = s.ProductDetails.Code,
                    ProductName = s.ProductDetails.Products.ThaiName,
                    ProductUnit = s.ProductDetails.ProductUnit.Name + "x" + s.ProductDetails.PackSize,
                    QTY = s.Qty,
                    Packsize = s.ProductDetails.PackSize,
                    CostPerUnit = s.CostPerUnit,
                    SellPricePerUnit = s.SellPricePerUnit,
                    Warehouse = "หน้าร้าน",

                    TotalCostPerUnit = TotalCostPerUnit,
                    TotalSellPricePerUnit = TotalSellPricePerUnit,

                    TotalPiece = TotalPiece,
                    TotalList = TotalList,

                    Description = s.AdjustStoreFront.Description,
                    ThaiBath = Library.ThaiBaht(s.AdjustStoreFront.TotalBalance.ToString())

                }).ToList();
                var dt = Library.ConvertToDataTable(selectData);
                return dt;
            }
        }
    }
}
