using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Repository
{
    public static class PORepository
    {
        /// <summary>
        /// ปริ้นใบ PO 1 ใบ
        /// </summary>
        /// <param name="poNo"></param>
        public static POHeader GetPOFormPrint(int id)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                try
                {
                    List<POProductDetails> details = new List<POProductDetails>();

                    var data = db.POHeader.FirstOrDefault(w => w.Id == id);
                    int i = 1;
                    decimal sumPricePerUnit = 0;
                    decimal sumTotal = 0;
                    decimal sumDiscount = 0;
                    decimal TotalList = 0;
                    foreach (var item in data.PODetail.Where(w => w.Enable == true).ToList())
                    {
                        decimal price = 0;
                        if (item.POHeader.Vendor.POCostType.Id == MyConstant.POCostType.CostOnly)
                        {
                            // หาก Vendor ยึดราคา ทุนเปล่า ใช้ ราคา ทุนเปล่า
                            price = item.CostOnly;
                        }
                        else
                        {
                            price = item.CostAndVat;
                        }
                        details.Add(new Repository.POProductDetails()
                        {
                            Number = i,
                            Code = item.ProductDetails.Code,
                            Name = item.ProductDetails.Products.ThaiName,
                            Discount = Library.ConvertDecimalToStringForm(item.DiscountBath),
                            Gift = Library.ConvertDecimalToStringForm(item.GiftQty),
                            Price = Library.ConvertDecimalToStringForm(price),
                            Qty = Library.ConvertDecimalToStringForm(item.Qty),
                            Total = Library.ConvertDecimalToStringForm(item.TotalCost),
                            Unit = item.ProductDetails.ProductUnit.Name
                        });
                        sumPricePerUnit += price;
                        sumTotal += item.TotalCost;
                        sumDiscount += item.DiscountBath;
                        TotalList = data.PODetail.Where(w => w.Enable == true).ToList().Count();
                        i++;
                    }

                    Repository.POHeader hd = new Repository.POHeader();
                    hd.PODetails = details;
                    hd.CompName = data.Branch.Name + " สาขา " + data.Branch.BranchNo;
                    hd.CompAddress = data.Branch.Address;
                    hd.CompTax = data.Branch.TaxNo;
                    hd.CompTel = data.Branch.Tel;
                    hd.CompEmail = data.Branch.Email;
                    hd.CompFax = data.Branch.Fax;
                    hd.CompDescription = data.Branch.Description;

                    hd.VendorCode = data.Vendor.Code;
                    hd.VendorName = data.Vendor.Name;
                    hd.VendorAddress = data.Vendor.Address;
                    hd.TargetName = hd.CompName;
                    hd.TargetAddress = hd.CompAddress;

                    hd.PONo = data.PONo;
                    hd.PODate = Library.ConvertDateToThaiDate(DateTime.Now, true);
                    hd.BudgetYear = data.BudgetYear.Name;
                    hd.ReferNo = data.ReferenceNo;
                    hd.DuaDate = Library.ConvertDateToThaiDate(data.DueDate);
                    hd.POExpire = Library.ConvertDateToThaiDate(data.POExpire);
                    hd.PaymentType = data.PaymentType.Name;

                    hd.CreateBy = Library.GetFullNameUserById(data.CreateBy);
                    if (MyConstant.CostType.Vat == 1)
                    {
                        hd.VatType = "ไม่มีภาษี";
                    }
                    hd.VatType = "มีภาษี";
                    hd.SumQty = Library.ConvertDecimalToStringForm(data.TotalQty);
                    // ใช้ชื่อ SumGift ขี้เกียจรีเครื่อง ใช้เป็นตัวนับรายการทั้งหมด
                    hd.SumGift = TotalList.ToString(); /*Library.ConvertDecimalToStringForm(data.TotalGift);*/
                    hd.SumPrice = Library.ConvertDecimalToStringForm(sumPricePerUnit);
                    hd.SumDiscount = Library.ConvertDecimalToStringForm(sumDiscount);
                    hd.SumTotal = Library.ConvertDecimalToStringForm(sumTotal);
                    hd.Remark = data.Description;
                    hd.ThaiBath = "(" + Library.ThaiBaht(data.TotalBalance + "") + ")";

                    hd.TotalAmount = Library.ConvertDecimalToStringForm(data.TotalPrice);
                    hd.DiscountPercent = Library.ConvertDecimalToStringForm(data.TotalDiscount * 100 / (data.TotalUnVat + data.TotalHasVat)) + "%";
                    hd.TotalDiscount = Library.ConvertDecimalToStringForm(data.TotalDiscount);
                    hd.TotalAfterDis = Library.ConvertDecimalToStringForm(data.TotalPriceDiscount);
                    hd.TotalVat = Library.ConvertDecimalToStringForm(data.TotalVat);
                    hd.TotalBalance = Library.ConvertDecimalToStringForm(data.TotalBalance);
                    return hd;
                }
                catch (Exception)
                {
                    return null;
                }

            }
        }
    }
}
