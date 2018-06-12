using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Repository
{
    public static class CheckStock
    {
        /// <summary>
        /// เลขที่เอกสาร
        /// </summary>
        /// <param name="DocNo"></param>
        public static DataTable getData(string DocNo)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var getDoc = db.StoreFrontValueDoc.SingleOrDefault(w => w.DocNo == DocNo);
                var sumQty1 = Library.ConvertDecimalToStringForm(getDoc.StoreFrontValueSet.Where(w => w.Enable == true).Sum(a => a.QtyUnit1));
                var sumQty2 = Library.ConvertDecimalToStringForm(getDoc.StoreFrontValueSet.Where(w => w.Enable == true).Sum(a => a.QtyUnit2));
                var sumPrice = Library.ConvertDecimalToStringForm(getDoc.StoreFrontValueSet.Where(w => w.Enable == true).Sum(a => a.Total));
                var conThaiBath = Library.ThaiBaht(sumPrice.ToString());
                var con = getDoc.StoreFrontValueSet.FirstOrDefault(w => w.Enable == true);
                var conDocDate = Library.ConvertDateToThaiDate(con.StoreFrontValueDoc.DocDate, true);
                var conNameCreateby = Library.GetFullNameUserById(con.StoreFrontValueDoc.CreateBy);
                var getBranch = Singleton.SingletonAuthen.Instance().MyBranch;
                var getName = Singleton.SingletonAuthen.Instance().Name;
                var PrintDate = Library.ConvertDateToThaiDate(DateTime.Now, true);
                var countList = getDoc.StoreFrontValueSet.Where(w => w.Enable == true).ToList().Count();
                var conCountList = Convert.ToInt32(countList);
                if (getDoc.ConfirmCheck1Date == null)
                {
                    var data = getDoc.StoreFrontValueSet.Where(w => w.Enable == true)
                    .Select(a => new
                    {
                        NameShop = getBranch.Name + " " + getBranch.BranchNo,
                        AddressShop = getBranch.Address,
                        TelShop = getBranch.Tel,
                        FaxShop = getBranch.Fax,
                        Email = getBranch.Email,
                        TaxNoShop = getBranch.TaxNo,
                        // ชื่อผู้พิพม์
                        NamePrint = getName,
                        // วันที่พิมพ์
                        PrintDate = PrintDate,
                        // ชื่อผู้บันทึก 
                        NameRecode = conNameCreateby,
                        // ปริ้นครั้งที่
                        PrintNumber = a.StoreFrontValueDoc.PrintNumber,

                        DocNo = a.StoreFrontValueDoc.DocNo,
                        DocDate = conDocDate,
                        ProductNo = a.ProductDetails.Code,
                        ProductName = a.ProductDetails.Products.ThaiName,
                        ProductUnti = a.ProductDetails.ProductUnit.Name + "x" + a.ProductDetails.PackSize,
                        Number = a.Number,
                        Packsize = a.Packsize,
                        Qty1 = a.QtyUnit1, // จำนวนนับครั้งที่ 1
                        Qty2 = "", // จำนวนนับครั้งที่ 2
                        CostOnlyPerUnit = a.CostOnlyPerUnit,
                        SellPricePerUnit = a.SellPricePerUnit,
                        Total = a.Total,
                        Warehouse = "หน้าร้าน",
                        Description = a.StoreFrontValueDoc.Description,
                        TotalQty1 = sumQty1,
                        TotalQty2 = "",
                        TotalPrice = sumPrice,
                        TotalList = conCountList,
                        ThaiBath = conThaiBath

                    }).OrderBy(a => a.Number).ToList();

                    var dt = Library.ConvertToDataTable(data);
                    return dt;
                }
                //else if (getDoc.ConfirmCheck1Date != null && getDoc.ConfirmCheck2Date == null)
                //{
                //    var data = getDoc.StoreFrontValueSet.Where(w => w.Enable == true)
                //    .Select(a => new
                //    {
                //        NameShop = getBranch.Name + " " + getBranch.BranchNo,
                //        AddressShop = getBranch.Address,
                //        TelShop = getBranch.Tel,
                //        FaxShop = getBranch.Fax,
                //        Email = getBranch.Email,
                //        TaxNoShop = getBranch.TaxNo,
                //        // ชื่อผู้พิพม์
                //        NamePrint = getName,
                //        // วันที่พิมพ์
                //        PrintDate = PrintDate,
                //        // ชื่อผู้บันทึก 
                //        NameRecode = conNameCreateby,
                //        // ปริ้นครั้งที่
                //        PrintNumber = a.StoreFrontValueDoc.PrintNumber,

                //        DocNo = a.StoreFrontValueDoc.DocNo,
                //        DocDate = conDocDate,
                //        ProductNo = a.ProductDetails.Code,
                //        ProductName = a.ProductDetails.Products.ThaiName,
                //        ProductUnti = a.ProductDetails.ProductUnit.Name + "x" + a.ProductDetails.PackSize,
                //        Number = a.Number,
                //        Packsize = a.Packsize,
                //        Qty1 = a.QtyUnit1, // จำนวนนับครั้งที่ 1
                //        Qty2 = , // จำนวนนับครั้งที่ 2
                //        CostOnlyPerUnit = a.CostOnlyPerUnit,
                //        SellPricePerUnit = a.SellPricePerUnit,
                //        Total = a.Total,
                //        Warehouse = "หน้าร้าน",
                //        Description = a.StoreFrontValueDoc.Description,
                //        TotalQty1 = sumQty1,
                //        TotalQty2 = sumQty2,
                //        TotalPrice = sumPrice,
                //        TotalList = conCountList,
                //        ThaiBath = conThaiBath

                //    }).OrderBy(a => a.Number).ToList();

                //    var dt = Library.ConvertToDataTable(data);
                //    return dt;
                //}
                else
                {
                    var data = getDoc.StoreFrontValueSet.Where(w => w.Enable == true)
                    .Select(a => new
                    {
                        NameShop = getBranch.Name + " " + getBranch.BranchNo,
                        AddressShop = getBranch.Address,
                        TelShop = getBranch.Tel,
                        FaxShop = getBranch.Fax,
                        Email = getBranch.Email,
                        TaxNoShop = getBranch.TaxNo,
                        // ชื่อผู้พิพม์
                        NamePrint = getName,
                        // วันที่พิมพ์
                        PrintDate = PrintDate,
                        // ชื่อผู้บันทึก 
                        NameRecode = conNameCreateby,
                        // ปริ้นครั้งที่
                        PrintNumber = a.StoreFrontValueDoc.PrintNumber,

                        DocNo = a.StoreFrontValueDoc.DocNo,
                        DocDate = conDocDate,
                        ProductNo = a.ProductDetails.Code,
                        ProductName = a.ProductDetails.Products.ThaiName,
                        ProductUnti = a.ProductDetails.ProductUnit.Name + "x" + a.ProductDetails.PackSize,
                        Number = a.Number,
                        Packsize = a.Packsize,
                        Qty1 = a.QtyUnit1, // จำนวนนับครั้งที่ 1
                        Qty2 = a.QtyUnit2, // จำนวนนับครั้งที่ 2
                        CostOnlyPerUnit = a.CostOnlyPerUnit,
                        SellPricePerUnit = a.SellPricePerUnit,
                        Total = a.Total,
                        Warehouse = "หน้าร้าน",
                        Description = a.StoreFrontValueDoc.Description,
                        TotalQty1 = sumQty1,
                        TotalQty2 = sumQty2,
                        TotalPrice = sumPrice,
                        TotalList = conCountList,
                        ThaiBath = conThaiBath

                    }).OrderBy(a => a.Number).ToList();

                    var dt = Library.ConvertToDataTable(data);
                    return dt;
                }

            }
        }
    }
}
