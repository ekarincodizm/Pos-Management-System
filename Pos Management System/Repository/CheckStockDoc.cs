using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using static Pos_Management_System.CheckStockFront1ListForm;

namespace Pos_Management_System.Repository
{
    public static class CheckStockDoc
    {
        public static System.Data.DataTable getData(List<classCheckStock> dataList)
        {
            var PrintDate = Library.ConvertDateToThaiDate(DateTime.Now, true);
            var getShop = Singleton.SingletonAuthen.Instance().MyBranch;
            var Name = Singleton.SingletonAuthen.Instance().Name;
            using (SSLsEntities db = new SSLsEntities())
            {
                List<StoreFrontValueDoc> list = new List<StoreFrontValueDoc>();
                var TotalQty = dataList.Sum(w => w.TotalQtyUnit);
                var TotalCost = dataList.Sum(w => w.TotalCostOnly);
                var ThaiBath = Library.ThaiBaht(TotalCost.ToString());
                var TotalCountList = dataList.Count();
                var getDate = dataList
                    .Select(a => new
                    {
                        NameShop = getShop.Name + " " + getShop.BranchNo,
                        NameAddress = getShop.Address,
                        TelShop = getShop.Tel,
                        FaxShop = getShop.Fax,
                        EmailShop = getShop.Email,
                        TaxNo = getShop.TaxNo,

                        PrintBy = Name, // ผู้พิมพ์
                        PrintDate = PrintDate, // วันที่พิมพ์
                        Check = a.check,
                        DocNo = a.DocNo,
                        DocDate = a.DocDate,
                        CreateBy = a.CreateBy,
                        SaveNumber = a.SaveNumber,
                        StoreFrontValueSet = a.StoreFrontValueSet,
                        TotalQtyUnit = a.TotalQtyUnit,
                        Total = a.TotalCostOnly,
                        Description = a.Description,
                        TotalQty = TotalQty, // รวมชิ้น
                        TotalCost = TotalCost, // รวมราคา
                        ThaiBath = ThaiBath,
                        TotalCountList = TotalCountList
                    }).ToList();
                var dt = Library.ConvertToDataTable(getDate);
                return dt;
            }
        }
    }
}
