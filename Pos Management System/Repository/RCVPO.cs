using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Repository
{
    public static class RCVPO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">PONo</param>
        public static DataTable getData(string code)
        {

            try
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    var PrintDate = Library.ConvertDateToThaiDate(DateTime.Now, true);

                    List<PORcvDetails> data = db.PORcvDetails
                        .Where(w => w.Enable == true && w.PORcv.Code == code && w.PORcv.Enable == true).ToList();
                    var getTotalQty = data
                        .Where(w => w.Enable == true && w.PORcv.Code == code && w.PORcv.Enable == true).Sum(w => w.RcvQuantity);
                    var getTotalGiftQty = data
                       .Where(w => w.Enable == true && w.PORcv.Code == code && w.PORcv.Enable == true).Sum(w => w.GiftQty);
                    var getTotalPrice = data
                        .Where(w => w.Enable == true && w.PORcv.Code == code && w.PORcv.Enable == true).Sum(w => w.TotalPrice);
                    var TotalCountList = data
                        .Where(w => w.Enable == true && w.PORcv.Code == code && w.PORcv.Enable == true &&
                        w.RcvQuantity > 0 || w.GiftQty > 0).GroupBy(a => a.ProductDetails.Code).Count();
                    var TotalDiscount = data
                        .Where(w => w.Enable == true && w.PORcv.Code == code && w.PORcv.Enable == true).Sum(w => w.DiscountBath);

                    var conCreateDate = Library.ConvertDateToThaiDate(data.FirstOrDefault().CreateDate, true);
                    var conInvoiceDate = Library.ConvertDateToThaiDate(data.FirstOrDefault().PORcv.InvoiceDate, true);
                    var conCreateBy = Library.GetFullNameUserById(data.FirstOrDefault().CreateBy);
                    var conThaiBath = Library.ThaiBaht((data.FirstOrDefault().PORcv.TotalBUnVat + data.FirstOrDefault().PORcv.TotalBHasVat + data.FirstOrDefault().PORcv.TotalVat).ToString());
                    var getData = data.Where(w => w.RcvQuantity > 0 || w.GiftQty > 0).OrderBy(w => w.SequenceNumber)
                    .Select(a => new
                    {
                        NameShop = a.PORcv.POHeader.Branch.Name + " " + a.PORcv.POHeader.Branch.BranchNo,
                        AddressShop = a.PORcv.POHeader.Branch.Address,
                        TelShop = a.PORcv.POHeader.Branch.Tel,
                        FaxShop = a.PORcv.POHeader.Branch.Fax,
                        TaxNoShop = a.PORcv.POHeader.Branch.TaxNo,
                        EmailShop = a.PORcv.POHeader.Branch.Email,

                        //เลขที่เอกสาร
                        PORcvNo = a.PORcv.Code,
                        // วันที่เอกสาร
                        PORcvCreateDate = conCreateDate,
                        // เลขที่ PO
                        PoNo = a.PORcv.PORefer,
                        // เลขที่ใบรับเข้า
                        PODate = a.PORcv.POHeader.PODate,
                        CreateBy = conCreateBy,
                        PrintDate = PrintDate, // วันที่ปริ้น

                        VenderName = a.PORcv.Vendor.Name,
                        VenderAddress = a.PORcv.Vendor.Address,
                        VenderTel = a.PORcv.Vendor.Tel,
                        VenderFax = a.PORcv.Vendor.Fax,

                        InvoiceNo = a.PORcv.InvoiceNo,
                        InvoiceDate = conInvoiceDate,
                        Number = a.SequenceNumber,
                        ProductNo = a.ProductDetails.Code,
                        ProductsName = a.ProductDetails.Products.ThaiName,
                        ProductUnit = a.ProductDetails.ProductUnit.Name,
                        GiftQty = a.GiftQty, // จำนวนแถม ใบรับเข้าสินค้า
                        Qty = a.RcvQuantity, // จำนวนที่รับเข้า ใบรับเข้าสินค้า

                        PricePerUnit = a.NewCost,
                        Discount = a.DiscountBath,
                        TotalPrice = a.TotalPrice,
                        TotalPriceList = getTotalPrice,
                        Discription = a.Description,
                        TotalQty = getTotalQty + getTotalGiftQty, // จำนวน + ของแถม
                        TotalCountList = TotalCountList,
                        TotalDiscount = TotalDiscount,
                        TotalVat = a.PORcv.TotalVat,
                        TotalBefDiscount = getTotalPrice - TotalDiscount,
                        TotalBUnVat = a.PORcv.TotalBUnVat,
                        TotalBHasVat = a.PORcv.TotalBHasVat,
                        Total = a.PORcv.TotalBUnVat + a.PORcv.TotalBHasVat + a.PORcv.TotalVat,
                        ThaiBath = conThaiBath
                    })
                    .ToList();
                    var dt = Library.ConvertToDataTable(getData);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex.ToString());
            }
            return null;
        }
    }
}
