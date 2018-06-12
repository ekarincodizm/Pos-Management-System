using Pos_Management_System.Model;
using Pos_Management_System.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public static class Library
    {
        public static List<int> _fkGoodsActive = new List<int>();
        /// <summary>
        /// ดึงยอดยกมา จะเอาสินค้า คงเหลือของวันก่อนหน้าทั้งหมด (ไม่รวมวันที่ค้นหา) yyyyMMdd
        /// </summary>
        /// <returns></returns>
        public static List<ยอดยกมา> GetQueryยอดยกมา(string date, string code1, string code2)
        {
            string query = @"with tb as (select a.FKProductDetails,case when t.IsPlus = 1 then a.ActionQty else a.ActionQty*-1 end Qty
                                FROM   pos.StoreFrontStockDetails a inner join pos.TransactionType t on a.FKTransactionType = t.Id WHERE (a.Enable = 1 and CONVERT(varchar, a.CreateDate, 112) < '{0}')),
                                tb2 as (select FKProductDetails,sum(qty) qty from tb group by FKProductDetails),
                                tb3 as (select p.Id,p.ThaiName,sum(tb2.qty) qty from tb2 inner join wms.ProductDetails d on tb2.FKProductDetails = d.Id
                                inner join wms.Products p on d.FKProduct = p.Id group by p.Id,p.ThaiName),
                                t4 as (select FKProduct,sum(CurrentCostOnly) Cost,COUNT(*) cnt from wms.CostProductChangeLog group by FKProduct),
                                t5 as (select FKProduct,cost,cnt,cost/cnt RCost from t4),
                                t6 as (select tb3.*,round(isnull(t5.RCost,0),2) Cost from tb3 left join t5 on tb3.Id = t5.FKProduct),
                                t7 as (select FKProduct,Code,CostOnly,FKUnit from wms.ProductDetails a where Enable = 1 
                                and PackSize = (select min(PackSize) from wms.ProductDetails where Enable = 1 and FKProduct = a.FKProduct) and FKProduct in (
                                select distinct FKProduct from wms.ProductDetails where Enable = 1 and Code between '{1}' and '{2}')),
                                t8 as (select * from t7 where Code = (select min(Code) from t7 tt where tt.FKProduct = t7.FKProduct)),
                                t9 as (select t8.*,t6.* from t8 left join t6 on t8.FKProduct = t6.Id),
                                p1 as (select t9.FKProduct,t9.Code,u.Name Unit,ThaiName,isnull(qty,0) qty,case when isnull(Cost,0) = 0 then CostOnly else Cost end Cost 
                                from t9 inner join wms.ProductUnit u on t9.FKUnit = u.Id),
                                p2 as (select * from p1 where ThaiName is null),
                                p3 as (select p2.FKProduct,p2.Code,p2.Unit,pp.ThaiName,p2.qty,p2.Cost from p2 inner join wms.Products pp on p2.FKProduct = pp.Id),
                                p4 as (select * from p1 where ThaiName is not null)
                                select * from p3 
                                union all 
                                select * from p4";
            using (SSLsEntities db = new SSLsEntities())
            {
                String s = String.Format(query, date, code1, code2);
                var data = db.Database.ExecuteEntities<ยอดยกมา>(s);
                return data.ToList();
            }

        }
        /// <summary>
        /// หา id product dtl ทั้งหมด ของ HD
        /// </summary>
        /// <param name="prodHD"></param>
        /// <returns></returns>
        public static List<int> GetProdDtlInHD(int prodHD)
        {
            var getProd = Singleton.SingletonProduct.Instance().Products.SingleOrDefault(w => w.Id == prodHD)
                .ProductDetails.Where(w => w.Enable == true).Select(w => w.Id).ToList<int>();
            return getProd;
        }
        /// <summary>
        /// หายอดก่อนภาษี
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double GetBeforeVat(double value)
        {
            if (value > 0)
            {
                double result = value / 1.07;
                return result;
            }
            else
            {
                return 0;
            }

        }
        /// <summary>
        /// Get ราคาขาย ณ วันที่ หน่วยเลกสุด
        /// </summary>
        public static decimal GetSellPriceOnly(int fkProHD, DateTime date)
        {
            try
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    date = date.AddDays(1);
                    var product = db.Products.SingleOrDefault(w => w.Id == fkProHD && w.Enable == true);
                    var proDtl = product.ProductDetails.Where(w => w.Enable == true).OrderBy(w => w.PackSize).FirstOrDefault();

                    var sellPriceLog = proDtl.SellPriceChangeLog.OrderByDescending(w => w.Enable == true).ToList();

                    if (sellPriceLog.Count() > 0)
                    {
                        //var getSellPriceFromDate = sellPriceLog.Where(w => w.Enable == true && 
                        //DbFunctions.TruncateTime(w.CreateDate) < DbFunctions.TruncateTime(date)).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        var getSellPriceFromDate = sellPriceLog.Where(w => w.Enable == true &&
                        int.Parse(w.CreateDate.ToString("yyyyMMdd")) < int.Parse(date.ToString("yyyyMMdd"))).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        if (getSellPriceFromDate != null)
                        {
                            return (decimal)GetBeforeVat((double)getSellPriceFromDate.CurrentPrice);
                        }
                        else
                        {
                            return (decimal)GetBeforeVat((double)proDtl.SellPrice);
                        }

                    }
                    else
                    {
                        return (decimal)GetBeforeVat((double)proDtl.SellPrice);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }
        /// <summary>
        /// Get ราคาทุนถัวเฉลี่ย ของชิ้น
        /// </summary>
        /// <param name="fkProHd"></param>
        /// <returns></returns>
        public static decimal GetAverage(int fkProHd)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                try
                {
                    var costChangeLog = db.CostProductChangeLog.Where(w => w.Enable == true && w.FKProduct == fkProHd).ToList();
                    if (costChangeLog.Count() == 0)
                    {
                        var costFromMaster = db.Products.SingleOrDefault(w => w.Id == fkProHd && w.Enable == true);
                        decimal costPz1 = costFromMaster.ProductDetails.Where(w => w.Enable == true).OrderBy(w => w.PackSize).FirstOrDefault().CostOnly;
                        return costPz1;
                    }
                    else
                    {
                        decimal sum = costChangeLog.Where(w => w.Enable == true).Sum(w => w.CurrentCostOnly);
                        decimal result = sum / costChangeLog.Count();
                        return result;
                    }
                }
                catch (Exception)
                {
                    return 0;
                }

            }
            return 0;
        }
        /// <summary>
        /// ยอดยกไป
        /// </summary>
        /// <param name="fkProHD"></param>
        /// <param name="startdDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static decimal GetBalance(int fkProHD, DateTime startdDate, DateTime endDate)
        {
            decimal resultForProDtl = GetResult(fkProHD, startdDate);
            try
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    List<StoreFrontStockDetails> details = db.StoreFrontStock.FirstOrDefault(w => w.FKProduct == fkProHD && w.Enable == true)
                      .StoreFrontStockDetails.Where(w => w.Enable == true &&
                      int.Parse(w.CreateDate.ToString("yyyyMMdd")) >= int.Parse(startdDate.ToString("yyyyMMdd")) &&
                      int.Parse(w.CreateDate.ToString("yyyyMMdd")) <= int.Parse(endDate.ToString("yyyyMMdd"))
                      ).OrderBy(w => w.CreateDate).ToList();

                    if (details.Count() == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        foreach (var trans in details)
                        {
                            decimal plus = 0;
                            decimal minus = 0;

                            if (trans.TransactionType.IsPlus == true)
                            {
                                plus = trans.ActionQty;
                                resultForProDtl = resultForProDtl + plus;
                            }
                            else
                            {
                                minus = trans.ActionQty;
                                resultForProDtl = resultForProDtl - minus;
                            }
                        }
                    }
                    return resultForProDtl;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// get ยอดยกมา หน่วยเลกสุด (DateTime Add Days 1 = get ทั้งหมดปัจจุบัน)
        /// </summary>
        /// <param name="fkProHD"></param>
        /// <param name="startdDate"></param>
        /// <returns></returns>
        public static decimal GetResult(int fkProHD, DateTime startdDate)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                List<StoreFrontStockDetails> detils = db.StoreFrontStockDetails
                    .Where(w => w.StoreFrontStock.Enable == true && w.StoreFrontStock.FKProduct == fkProHD && w.Enable == true &&
                    DbFunctions.TruncateTime(w.CreateDate) < DbFunctions.TruncateTime(startdDate)
                    ).ToList();

                decimal plus = 0;
                decimal minus = 0;
                decimal result = 0;
                if (detils.Count() > 0)
                {
                    plus = detils.Where(w => w.TransactionType.IsPlus == true).Sum(w => w.ActionQty);
                    minus = detils.Where(w => w.TransactionType.IsPlus == false).Sum(w => w.ActionQty);
                    result = plus - minus;
                    return result;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// get ยอดยกมา หน่วยเลกสุด ราย barcode
        /// </summary>
        /// <param name="fkProDtl"></param>
        /// <param name="startdDate"></param>
        /// <param name="docCheck"></param>
        /// <returns></returns>
        public static decimal GetResult(int fkProDtl, DateTime startdDate, string docCheck)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                List<StoreFrontStockDetails> detils = db.StoreFrontStockDetails
                    .Where(w => w.FKProductDetails == fkProDtl && w.Enable == true && w.DocNo == docCheck &&
                    DbFunctions.TruncateTime(w.CreateDate) < DbFunctions.TruncateTime(startdDate)
                    ).ToList();

                decimal plus = 0;
                decimal minus = 0;
                decimal result = 0;
                if (detils.Count() > 0)
                {
                    plus = detils.Where(w => w.TransactionType.IsPlus == true).Sum(w => w.ActionQty);
                    minus = detils.Where(w => w.TransactionType.IsPlus == false).Sum(w => w.ActionQty);
                    result = plus - minus;
                    return result;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// สินค้า จาก-ถึง
        /// </summary>
        /// <param name="Code_S"></param>
        /// <param name="Code_E"></param>
        /// <returns></returns>
        public static List<int> GetAllProDtlId(string Code_S, string Code_E)
        {
            List<int> fkProHd = new List<int>();
            try
            {

                var allPro = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).OrderBy(w => w.Code);

                if (Code_S == "")
                {
                    fkProHd.AddRange(allPro.Select(w => w.FKProduct).Distinct().ToList<int>());
                    return fkProHd;
                }
                else if (Code_S == Code_E)
                {

                    int id = allPro.FirstOrDefault(w => w.Code == Code_S).FKProduct;
                    fkProHd.Add(id);
                    return fkProHd;
                }
                bool found = false;
                foreach (var item in allPro)
                {
                    if (found == true && item.Code != Code_E)
                    {
                        fkProHd.Add(item.FKProduct);
                    }
                    else if (item.Code == Code_S)
                    {
                        found = true;
                        fkProHd.Add(item.FKProduct);
                    }
                    else if (item.Code == Code_E)
                    {
                        fkProHd.Add(item.FKProduct);
                        break;
                    }
                }
                return fkProHd.Distinct().ToList<int>(); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return fkProHd;
            }
        }
        /// <summary>
        /// main stock card value อัลกอรึทึมหลักของ ทุก transaction หน้าร้าน
        /// </summary>
        private static void ManagePosStock(List<WarehouseStockValue> values)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                PosStock pss;
                foreach (var item in values)
                {
                    pss = new PosStock();
                    pss = db.PosStock.FirstOrDefault(w => w.Enable == true && w.FKProduct == item.FKProduct); // ซึ่งจะต้องเจอยู่แล้ว
                    //decimal oldQty = pss.CurrentQty; //จำนวนชิ้น
                    List<PosStockDetails> posDtls = new List<PosStockDetails>();
                    PosStockDetails posDtl;
                    foreach (var dtl in item.ValueDetails)
                    {
                        var detail = pss.PosStockDetails.Where(w => w.Enable == true && w.FKProductDetails == dtl.FKProductDetail).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        var transaction = db.TransactionType.SingleOrDefault(w => w.Id == dtl.FKTransaction);
                        decimal packsize = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == dtl.FKProductDetail).PackSize;
                        // ได้ log ตัวล่าสุดมา เพื่อทำ Result
                        posDtl = new PosStockDetails();
                        posDtl.FKProductDetails = dtl.FKProductDetail;
                        posDtl.FKTransactionType = dtl.FKTransaction;
                        posDtl.FKPosStock = pss.Id;
                        posDtl.CreateDate = DateTime.Now;
                        posDtl.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        posDtl.UpdateDate = DateTime.Now;
                        posDtl.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        posDtl.Enable = true;
                        posDtl.Description = transaction.Name;
                        posDtl.PackSize = packsize;
                        posDtl.ActionQtyUnit = dtl.ActionQtyUnit;
                        posDtl.ActionQty = dtl.ActionQtyUnit * packsize;

                        /// จัดการ Result โดยเชค transaction
                        if (transaction.IsPlus)
                        {
                            posDtl.ResultQty = detail.ResultQty + (dtl.ActionQtyUnit * packsize); // เอาของเดิม มา + ของใหม่
                            posDtl.ResultQtyUnit = detail.ResultQtyUnit + dtl.ActionQtyUnit;
                        }
                        else
                        {
                            posDtl.ResultQty = detail.ResultQty - (dtl.ActionQtyUnit * packsize); // เอาของเดิม มา + ของใหม่
                            posDtl.ResultQtyUnit = detail.ResultQtyUnit - dtl.ActionQtyUnit;
                        }
                        posDtl.OldQty = detail.ResultQty;
                        posDtls.Add(posDtl);
                    }
                    db.PosStockDetails.AddRange(posDtls);
                }
                db.SaveChanges();
                // Update Header
                IEnumerable<int> fkproducts = values.Select(w => w.FKProduct).ToList<int>();
                foreach (var item in fkproducts)
                {
                    // ไล่อัพเดท
                    var data = db.PosStock.FirstOrDefault(w => w.Enable == true && w.FKProduct == item);
                    data.UpdateDate = DateTime.Now;
                    data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    data.OldQty = data.CurrentQty;
                    //data.CurrentQty = 0; // ค่าใหม่
                    decimal currentQty = 0;
                    IEnumerable<int> fkProductDtl = data.PosStockDetails.Where(w => w.Enable == true).Select(w => w.FKProductDetails).Distinct().ToList<int>();
                    foreach (var id in fkProductDtl)
                    {
                        // จะ Get Top ของแต่ล่ะ บาร์ยังไง ค่อยว่ากัน ทำ SD ก่อน ***
                        // get from data
                        var lastLog = data.PosStockDetails.Where(w => w.Enable == true && w.FKProductDetails == id).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        if (lastLog != null)
                        {
                            // ได้ตัวล่าสุดมา
                            currentQty += lastLog.ResultQty;
                        }
                    }
                    data.CurrentQty = currentQty; // ค่าใหม่
                }
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Make CNDetails สำหรับ ยกเลิกคืนของจากลูกค้า -StoreFront
        /// </summary>
        /// <param name="details"></param>
        /// <param name="cnType"></param>
        public static void MakeValueForUpdateStockPos(List<CNDetails> details, string cnType)
        {
            List<MakeForStockValue> values = new List<MakeForStockValue>();
            foreach (var item in details)
            {
                int fkProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.PosDetails.FKProductDetails).FKProduct;
                values.Add(new MakeForStockValue()
                {
                    FKProduct = fkProduct,
                    FKProductDetail = item.PosDetails.FKProductDetails
                });
            }
            // Group FKProduct
            IEnumerable<int> fkProd = values.Select(w => w.FKProduct).ToList().Distinct();
            List<WarehouseStockValue> warehouseValues = new List<WarehouseStockValue>();
            //WarehouseStockValue warehouseValue;
            foreach (var item in fkProd)
            {
                // หา ProductDetail ใน Values
                var productDtls = values.Where(w => w.FKProduct == item);
                //warehouseValue = new WarehouseStockValue();
                List<WarehouseStockValue.Details> valueDetail = new List<WarehouseStockValue.Details>();
                foreach (var dtlId in productDtls)
                {
                    decimal actionQtyUnit = details.FirstOrDefault(w => w.PosDetails.FKProductDetails == dtlId.FKProductDetail).CNQty;
                    //if (actionQtyUnit == 0)
                    //{
                    //    continue;
                    //}
                    valueDetail.Add(new WarehouseStockValue.Details()
                    {
                        ActionQtyUnit = actionQtyUnit,
                        FKItemRemark = MyConstant.ItemRemark.Nornal,
                        FKProductDetail = dtlId.FKProductDetail,
                        FKTransaction = MyConstant.PosTransaction.CustomerReturnCancel
                    });
                }
                // add rows
                warehouseValues.Add(new WarehouseStockValue()
                {
                    FKProduct = item,
                    ValueDetails = valueDetail
                });
            }
            // post warehouseValues to dll
            ManagePosStock(warehouseValues);
        }
        /// <summary>
        /// Make CNDetails สำหรับคืนของจากลูกค้า +StoreFront
        /// </summary>
        /// <param name="details"></param>
        public static void MakeValueForUpdateStockPos(List<CNDetails> details)
        {
            List<MakeForStockValue> values = new List<MakeForStockValue>();
            foreach (var item in details)
            {
                int fkProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.PosDetails.FKProductDetails).FKProduct;
                values.Add(new MakeForStockValue()
                {
                    FKProduct = fkProduct,
                    FKProductDetail = item.PosDetails.FKProductDetails
                });
            }
            // Group FKProduct
            IEnumerable<int> fkProd = values.Select(w => w.FKProduct).ToList().Distinct();
            List<WarehouseStockValue> warehouseValues = new List<WarehouseStockValue>();
            //WarehouseStockValue warehouseValue;
            foreach (var item in fkProd)
            {
                // หา ProductDetail ใน Values
                var productDtls = values.Where(w => w.FKProduct == item);
                //warehouseValue = new WarehouseStockValue();
                List<WarehouseStockValue.Details> valueDetail = new List<WarehouseStockValue.Details>();
                foreach (var dtlId in productDtls)
                {
                    decimal actionQtyUnit = details.FirstOrDefault(w => w.PosDetails.FKProductDetails == dtlId.FKProductDetail).CNQty;
                    //if (actionQtyUnit == 0)
                    //{
                    //    continue;
                    //}
                    valueDetail.Add(new WarehouseStockValue.Details()
                    {
                        ActionQtyUnit = actionQtyUnit,
                        FKItemRemark = MyConstant.ItemRemark.Nornal,
                        FKProductDetail = dtlId.FKProductDetail,
                        FKTransaction = MyConstant.PosTransaction.CustomerReturn
                    });
                }
                // add rows
                warehouseValues.Add(new WarehouseStockValue()
                {
                    FKProduct = item,
                    ValueDetails = valueDetail
                });
            }
            // post warehouseValues to dll
            ManagePosStock(warehouseValues);
        }
        /// <summary>
        /// Make StoreFrontTransferWasteDtl สำหรับคืนของเสียเข้าคลัง -StoreFront
        /// </summary>
        /// <param name="details"></param>
        public static void MakeValueForUpdateStockPos(List<StoreFrontTransferWasteDtl> details)
        {
            List<MakeForStockValue> values = new List<MakeForStockValue>();
            foreach (var item in details)
            {
                int fkProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails).FKProduct;
                values.Add(new MakeForStockValue()
                {
                    FKProduct = fkProduct,
                    FKProductDetail = item.FKProductDetails
                });
            }
            // Group FKProduct
            IEnumerable<int> fkProd = values.Select(w => w.FKProduct).ToList().Distinct();
            List<WarehouseStockValue> warehouseValues = new List<WarehouseStockValue>();
            //WarehouseStockValue warehouseValue;
            foreach (var item in fkProd)
            {
                // หา ProductDetail ใน Values
                var productDtls = values.Where(w => w.FKProduct == item);
                //warehouseValue = new WarehouseStockValue();
                List<WarehouseStockValue.Details> valueDetail = new List<WarehouseStockValue.Details>();
                foreach (var dtlId in productDtls)
                {
                    decimal actionQtyUnit = details.FirstOrDefault(w => w.FKProductDetails == dtlId.FKProductDetail).Qty;
                    valueDetail.Add(new WarehouseStockValue.Details()
                    {
                        ActionQtyUnit = actionQtyUnit,
                        FKItemRemark = MyConstant.ItemRemark.Nornal,
                        FKProductDetail = dtlId.FKProductDetail,
                        FKTransaction = MyConstant.PosTransaction.CNToWarehouse
                    });
                }
                // add rows
                warehouseValues.Add(new WarehouseStockValue()
                {
                    FKProduct = item,
                    ValueDetails = valueDetail
                });
            }
            // post warehouseValues to dll
            ManagePosStock(warehouseValues);
        }
        /// <summary>
        /// Make StoreFrontTransferOutDtl สำหรับโอนสินค้าหน้าร้าน -StoreFront
        /// </summary>
        /// <param name="details"></param>
        public static void MakeValueForUpdateStockPos(List<StoreFrontTransferOutDtl> details)
        {
            List<MakeForStockValue> values = new List<MakeForStockValue>();
            foreach (var item in details)
            {
                int fkProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails).FKProduct;
                values.Add(new MakeForStockValue()
                {
                    FKProduct = fkProduct,
                    FKProductDetail = item.FKProductDetails
                });
            }
            // Group FKProduct
            IEnumerable<int> fkProd = values.Select(w => w.FKProduct).ToList().Distinct();
            List<WarehouseStockValue> warehouseValues = new List<WarehouseStockValue>();
            //WarehouseStockValue warehouseValue;
            foreach (var item in fkProd)
            {
                // หา ProductDetail ใน Values
                var productDtls = values.Where(w => w.FKProduct == item);
                //warehouseValue = new WarehouseStockValue();
                List<WarehouseStockValue.Details> valueDetail = new List<WarehouseStockValue.Details>();
                foreach (var dtlId in productDtls)
                {
                    decimal actionQtyUnit = details.FirstOrDefault(w => w.FKProductDetails == dtlId.FKProductDetail).Qty;
                    valueDetail.Add(new WarehouseStockValue.Details()
                    {
                        ActionQtyUnit = actionQtyUnit,
                        FKItemRemark = MyConstant.ItemRemark.Nornal,
                        FKProductDetail = dtlId.FKProductDetail,
                        FKTransaction = MyConstant.PosTransaction.TransferStoreFrontToBranch
                    });
                }
                // add rows
                warehouseValues.Add(new WarehouseStockValue()
                {
                    FKProduct = item,
                    ValueDetails = valueDetail
                });
            }
            // post warehouseValues to dll
            ManagePosStock(warehouseValues);
        }
        /// <summary>
        /// Make ISS2FrontDetails สำหรับเติมสินค้าหน้าร้าน +ร้าน หลังจาก -คลัง
        /// </summary>
        public static void MakeValueForUpdateStockPos(List<ISS2FrontDetails> details)
        {
            List<MakeForStockValue> values = new List<MakeForStockValue>();
            foreach (var item in details)
            {
                int fkProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails).FKProduct;
                values.Add(new MakeForStockValue()
                {
                    FKProduct = fkProduct,
                    FKProductDetail = item.FKProductDetails
                });
            }
            // Group FKProduct
            IEnumerable<int> fkProd = values.Select(w => w.FKProduct).ToList().Distinct();
            List<WarehouseStockValue> warehouseValues = new List<WarehouseStockValue>();
            //WarehouseStockValue warehouseValue;
            foreach (var item in fkProd)
            {
                // หา ProductDetail ใน Values
                var productDtls = values.Where(w => w.FKProduct == item);
                //warehouseValue = new WarehouseStockValue();
                List<WarehouseStockValue.Details> valueDetail = new List<WarehouseStockValue.Details>();
                foreach (var dtlId in productDtls)
                {
                    decimal actionQtyUnit = details.FirstOrDefault(w => w.FKProductDetails == dtlId.FKProductDetail).QtyAllow;
                    valueDetail.Add(new WarehouseStockValue.Details()
                    {
                        ActionQtyUnit = actionQtyUnit,
                        FKItemRemark = MyConstant.ItemRemark.Nornal,
                        FKProductDetail = dtlId.FKProductDetail,
                        FKTransaction = MyConstant.PosTransaction.ISS
                    });
                }
                // add rows
                warehouseValues.Add(new WarehouseStockValue()
                {
                    FKProduct = item,
                    ValueDetails = valueDetail
                });
            }
            // post warehouseValues to dll
            ManagePosStock(warehouseValues);
        }
        /// <summary>
        /// จัดการ Stock Wms ******************************************************************************************************************
        /// </summary>
        /// <param name="values"></param>
        private static void ManageWmsStock(List<WarehouseStockValue> values)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                WmsStock wms;
                foreach (var item in values)
                {
                    wms = new WmsStock();
                    wms = db.WmsStock.FirstOrDefault(w => w.Enable == true && w.FKProduct == item.FKProduct); // ซึ่งจะต้องเจอยู่แล้ว
                    decimal oldQty = wms.CurrentQty; //จำนวนชิ้น
                    List<WmsStockDetail> wmsDtls = new List<WmsStockDetail>();
                    WmsStockDetail wmsDtl;
                    foreach (var dtl in item.ValueDetails)
                    {
                        var detail = wms.WmsStockDetail.Where(w => w.Enable == true && w.FKProductDetail == dtl.FKProductDetail).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        var transaction = db.TransactionWms.SingleOrDefault(w => w.Id == dtl.FKTransaction);
                        decimal packsize = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == dtl.FKProductDetail).PackSize;
                        // ได้ log ตัวล่าสุดมา เพื่อทำ Result
                        wmsDtl = new WmsStockDetail();
                        wmsDtl.FKItemRemark = dtl.FKItemRemark;
                        wmsDtl.FKProductDetail = dtl.FKProductDetail;
                        wmsDtl.FKTransaction = dtl.FKTransaction;
                        wmsDtl.FKWmsStock = wms.Id;
                        wmsDtl.CreateDate = DateTime.Now;
                        wmsDtl.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        wmsDtl.UpdateDate = DateTime.Now;
                        wmsDtl.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        wmsDtl.Enable = true;
                        wmsDtl.Description = transaction.Name;
                        wmsDtl.PackSize = packsize;
                        wmsDtl.ActionQtyUnit = dtl.ActionQtyUnit;
                        wmsDtl.ActionQty = dtl.ActionQtyUnit * packsize;

                        /// จัดการ Result โดยเชค transaction
                        if (transaction.IsPlus)
                        {
                            wmsDtl.ResultQty = detail.ResultQty + (dtl.ActionQtyUnit * packsize); // เอาของเดิม มา + ของใหม่
                            wmsDtl.ResultQtyUnit = detail.ResultQtyUnit + dtl.ActionQtyUnit;
                        }
                        else
                        {
                            wmsDtl.ResultQty = detail.ResultQty - (dtl.ActionQtyUnit * packsize); // เอาของเดิม มา + ของใหม่
                            wmsDtl.ResultQtyUnit = detail.ResultQtyUnit - dtl.ActionQtyUnit;
                        }
                        wmsDtl.OldQty = detail.ResultQty;
                        wmsDtls.Add(wmsDtl);
                    }
                    db.WmsStockDetail.AddRange(wmsDtls);
                }
                db.SaveChanges();
                // Update Header
                IEnumerable<int> fkproducts = values.Select(w => w.FKProduct).ToList<int>();
                foreach (var item in fkproducts)
                {
                    // ไล่อัพเดท
                    var data = db.WmsStock.FirstOrDefault(w => w.Enable == true && w.FKProduct == item);
                    data.UpdateDate = DateTime.Now;
                    data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    data.OldQty = data.CurrentQty;
                    //data.CurrentQty = 0; // ค่าใหม่
                    decimal currentQty = 0;
                    IEnumerable<int> fkProductDtl = data.WmsStockDetail.Where(w => w.Enable == true).Select(w => w.FKProductDetail).Distinct().ToList<int>();
                    foreach (var id in fkProductDtl)
                    {
                        // จะ Get Top ของแต่ล่ะ บาร์ยังไง ค่อยว่ากัน ทำ SD ก่อน ***
                        // get from data
                        var lastLog = data.WmsStockDetail.Where(w => w.Enable == true && w.FKProductDetail == id).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        if (lastLog != null)
                        {
                            // ได้ตัวล่าสุดมา
                            currentQty += lastLog.ResultQty;
                        }
                    }
                    data.CurrentQty = currentQty; // ค่าใหม่
                }
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Make WarehouseToWasteDetails สำหรับโอนสินค้าคลัง เข้าห้องของเสีย
        /// </summary>
        /// <param name="details"></param>
        public static void MakeValueForUpdateStockWms(List<WarehouseToWasteDetails> details)
        {
            List<MakeForStockValue> values = new List<MakeForStockValue>();
            foreach (var item in details)
            {
                int fkProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails).FKProduct;
                values.Add(new MakeForStockValue()
                {
                    FKProduct = fkProduct,
                    FKProductDetail = item.FKProductDetails
                });
            }
            // Group FKProduct
            IEnumerable<int> fkProd = values.Select(w => w.FKProduct).ToList().Distinct();
            List<WarehouseStockValue> warehouseValues = new List<WarehouseStockValue>();
            //WarehouseStockValue warehouseValue;
            foreach (var item in fkProd)
            {
                // หา ProductDetail ใน Values
                var productDtls = values.Where(w => w.FKProduct == item);
                //warehouseValue = new WarehouseStockValue();
                List<WarehouseStockValue.Details> valueDetail = new List<WarehouseStockValue.Details>();
                foreach (var dtlId in productDtls)
                {
                    decimal actionQtyUnit = details.FirstOrDefault(w => w.FKProductDetails == dtlId.FKProductDetail).QtyUnit;
                    valueDetail.Add(new WarehouseStockValue.Details()
                    {
                        ActionQtyUnit = actionQtyUnit,
                        FKItemRemark = MyConstant.ItemRemark.Nornal,
                        FKProductDetail = dtlId.FKProductDetail,
                        FKTransaction = MyConstant.WmsTransaction.DetectWaste
                    });
                }
                // add rows
                warehouseValues.Add(new WarehouseStockValue()
                {
                    FKProduct = item,
                    ValueDetails = valueDetail
                });
            }
            // post warehouseValues to dll
            ManageWmsStock(warehouseValues);
        }
        /// <summary>
        /// Make StoreFrontTransferWasteDtl สำหรับโอนสินค้าหน้าร้าน เข้าคลัง +Wms Stock
        /// </summary>
        /// <param name="details"></param>
        public static void MakeValueForUpdateStockWms(List<StoreFrontTransferWasteDtl> details)
        {
            List<MakeForStockValue> values = new List<MakeForStockValue>();
            foreach (var item in details)
            {
                int fkProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails).FKProduct;
                values.Add(new MakeForStockValue()
                {
                    FKProduct = fkProduct,
                    FKProductDetail = item.FKProductDetails
                });
            }
            // Group FKProduct
            IEnumerable<int> fkProd = values.Select(w => w.FKProduct).ToList().Distinct();
            List<WarehouseStockValue> warehouseValues = new List<WarehouseStockValue>();
            //WarehouseStockValue warehouseValue;
            foreach (var item in fkProd)
            {
                // หา ProductDetail ใน Values
                var productDtls = values.Where(w => w.FKProduct == item);
                //warehouseValue = new WarehouseStockValue();
                List<WarehouseStockValue.Details> valueDetail = new List<WarehouseStockValue.Details>();
                foreach (var dtlId in productDtls)
                {
                    decimal actionQtyUnit = details.FirstOrDefault(w => w.FKProductDetails == dtlId.FKProductDetail).Qty;
                    valueDetail.Add(new WarehouseStockValue.Details()
                    {
                        ActionQtyUnit = actionQtyUnit,
                        FKItemRemark = MyConstant.ItemRemark.Nornal,
                        FKProductDetail = dtlId.FKProductDetail,
                        FKTransaction = MyConstant.WmsTransaction.StoreFrontToWarehouse
                    });
                }
                // add rows
                warehouseValues.Add(new WarehouseStockValue()
                {
                    FKProduct = item,
                    ValueDetails = valueDetail
                });
            }
            // post warehouseValues to dll
            ManageWmsStock(warehouseValues);
        }
        /// <summary>
        /// Make SaleOrderWarehouseDtl สำหรับการออเดอร์ คลังสินค้า -Wms Stock
        /// </summary>
        /// <param name="details"></param>
        public static void MakeValueForUpdateStockWms(List<SaleOrderWarehouseDtl> details)
        {
            List<MakeForStockValue> values = new List<MakeForStockValue>();
            foreach (var item in details)
            {
                int fkProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDtl).FKProduct;
                values.Add(new MakeForStockValue()
                {
                    FKProduct = fkProduct,
                    FKProductDetail = item.FKProductDtl
                });
            }
            // Group FKProduct
            IEnumerable<int> fkProd = values.Select(w => w.FKProduct).ToList().Distinct();
            List<WarehouseStockValue> warehouseValues = new List<WarehouseStockValue>();
            //WarehouseStockValue warehouseValue;
            foreach (var item in fkProd)
            {
                // หา ProductDetail ใน Values
                var productDtls = values.Where(w => w.FKProduct == item);
                //warehouseValue = new WarehouseStockValue();
                List<WarehouseStockValue.Details> valueDetail = new List<WarehouseStockValue.Details>();
                foreach (var dtlId in productDtls)
                {
                    decimal actionQtyUnit = details.FirstOrDefault(w => w.FKProductDtl == dtlId.FKProductDetail).QtyAllow;
                    valueDetail.Add(new WarehouseStockValue.Details()
                    {
                        ActionQtyUnit = actionQtyUnit,
                        FKItemRemark = MyConstant.ItemRemark.Nornal,
                        FKProductDetail = dtlId.FKProductDetail,
                        FKTransaction = MyConstant.WmsTransaction.OrderWarehouse
                    });
                }
                // add rows
                warehouseValues.Add(new WarehouseStockValue()
                {
                    FKProduct = item,
                    ValueDetails = valueDetail
                });
            }
            // post warehouseValues to dll
            ManageWmsStock(warehouseValues);
        }
        /// <summary>
        /// Make ISS2FrontDetails สำหรับการเบิกเพื่อเติมหน้าร้าน -คลัง ไป +ร้าน
        /// </summary>
        /// <param name="details"></param>
        public static void MakeValueForUpdateStockWms(List<ISS2FrontDetails> details)
        {
            List<MakeForStockValue> values = new List<MakeForStockValue>();
            foreach (var item in details)
            {
                int fkProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails).FKProduct;
                values.Add(new MakeForStockValue()
                {
                    FKProduct = fkProduct,
                    FKProductDetail = item.FKProductDetails
                });
            }
            // Group FKProduct
            IEnumerable<int> fkProd = values.Select(w => w.FKProduct).ToList().Distinct();
            List<WarehouseStockValue> warehouseValues = new List<WarehouseStockValue>();
            //WarehouseStockValue warehouseValue;
            foreach (var item in fkProd)
            {
                // หา ProductDetail ใน Values
                var productDtls = values.Where(w => w.FKProduct == item);
                //warehouseValue = new WarehouseStockValue();
                List<WarehouseStockValue.Details> valueDetail = new List<WarehouseStockValue.Details>();
                foreach (var dtlId in productDtls)
                {
                    decimal actionQtyUnit = details.FirstOrDefault(w => w.FKProductDetails == dtlId.FKProductDetail).QtyAllow;
                    valueDetail.Add(new WarehouseStockValue.Details()
                    {
                        ActionQtyUnit = actionQtyUnit,
                        FKItemRemark = MyConstant.ItemRemark.Nornal,
                        FKProductDetail = dtlId.FKProductDetail,
                        FKTransaction = MyConstant.WmsTransaction.ISS
                    });
                }
                // add rows
                warehouseValues.Add(new WarehouseStockValue()
                {
                    FKProduct = item,
                    ValueDetails = valueDetail
                });
            }
            // post warehouseValues to dll
            ManageWmsStock(warehouseValues);
        }
        /// <summary>
        /// Make CNWarehouseDetails สำหรับการทำส่งคืน -
        /// </summary>
        /// <param name="details"></param>
        public static void MakeValueForUpdateStockWms(List<CNWarehouseDetails> details)
        {
            List<MakeForStockValue> values = new List<MakeForStockValue>();
            foreach (var item in details)
            {
                int fkProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails).FKProduct;
                values.Add(new MakeForStockValue()
                {
                    FKProduct = fkProduct,
                    FKProductDetail = item.FKProductDetails
                });
            }
            // Group FKProduct
            IEnumerable<int> fkProd = values.Select(w => w.FKProduct).ToList().Distinct();
            List<WarehouseStockValue> warehouseValues = new List<WarehouseStockValue>();
            //WarehouseStockValue warehouseValue;
            foreach (var item in fkProd)
            {
                // หา ProductDetail ใน Values
                var productDtls = values.Where(w => w.FKProduct == item);
                //warehouseValue = new WarehouseStockValue();
                List<WarehouseStockValue.Details> valueDetail = new List<WarehouseStockValue.Details>();
                foreach (var dtlId in productDtls)
                {
                    /// กระทำจำนวนรับเข้า ทั้งของแถม + รับเข้าปกติ
                    decimal actionQtyUnit = details.FirstOrDefault(w => w.FKProductDetails == dtlId.FKProductDetail).Qty;
                    valueDetail.Add(new WarehouseStockValue.Details()
                    {
                        ActionQtyUnit = actionQtyUnit,
                        FKItemRemark = MyConstant.ItemRemark.Nornal,
                        FKProductDetail = dtlId.FKProductDetail,
                        FKTransaction = MyConstant.WmsTransaction.CN
                    });
                }
                // add rows
                warehouseValues.Add(new WarehouseStockValue()
                {
                    FKProduct = item,
                    ValueDetails = valueDetail
                });
            }
            // post warehouseValues to dll
            ManageWmsStock(warehouseValues);
        }
        /// <summary>
        /// Make WmsTransferOutDetails สำหรับการทำโอนไปสาขาอื่น -
        /// </summary>
        /// <param name="details"></param>
        public static void MakeValueForUpdateStockWms(List<WmsTransferOutDetails> details)
        {
            List<MakeForStockValue> values = new List<MakeForStockValue>();
            foreach (var item in details)
            {
                int fkProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails).FKProduct;
                values.Add(new MakeForStockValue()
                {
                    FKProduct = fkProduct,
                    FKProductDetail = item.FKProductDetails
                });
            }
            // Group FKProduct
            IEnumerable<int> fkProd = values.Select(w => w.FKProduct).ToList().Distinct();
            List<WarehouseStockValue> warehouseValues = new List<WarehouseStockValue>();
            //WarehouseStockValue warehouseValue;
            foreach (var item in fkProd)
            {
                // หา ProductDetail ใน Values
                var productDtls = values.Where(w => w.FKProduct == item);
                //warehouseValue = new WarehouseStockValue();
                List<WarehouseStockValue.Details> valueDetail = new List<WarehouseStockValue.Details>();
                foreach (var dtlId in productDtls)
                {
                    /// กระทำจำนวนรับเข้า ทั้งของแถม + รับเข้าปกติ
                    decimal actionQtyUnit = details.FirstOrDefault(w => w.FKProductDetails == dtlId.FKProductDetail).Qty;
                    valueDetail.Add(new WarehouseStockValue.Details()
                    {
                        ActionQtyUnit = actionQtyUnit,
                        FKItemRemark = MyConstant.ItemRemark.Nornal,
                        FKProductDetail = dtlId.FKProductDetail,
                        FKTransaction = MyConstant.WmsTransaction.TransferBranch
                    });
                }
                // add rows
                warehouseValues.Add(new WarehouseStockValue()
                {
                    FKProduct = item,
                    ValueDetails = valueDetail
                });
            }
            // post warehouseValues to dll
            ManageWmsStock(warehouseValues);
        }
        /// <summary>
        /// Make PORcvDetails สำหรับการทำรับเข้า +
        /// </summary>
        public static void MakeValueForUpdateStockWms(List<PORcvDetails> details)
        {
            List<MakeForStockValue> values = new List<MakeForStockValue>();
            foreach (var item in details)
            {
                int fkProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDtl).FKProduct;
                values.Add(new MakeForStockValue()
                {
                    FKProduct = fkProduct,
                    FKProductDetail = item.FKProductDtl
                });
            }
            // Group FKProduct
            IEnumerable<int> fkProd = values.Select(w => w.FKProduct).ToList().Distinct();
            List<WarehouseStockValue> warehouseValues = new List<WarehouseStockValue>();
            //WarehouseStockValue warehouseValue;
            foreach (var item in fkProd)
            {
                // หา ProductDetail ใน Values
                var productDtls = values.Where(w => w.FKProduct == item);
                //warehouseValue = new WarehouseStockValue();
                List<WarehouseStockValue.Details> valueDetail = new List<WarehouseStockValue.Details>();
                foreach (var dtlId in productDtls)
                {
                    /// กระทำจำนวนรับเข้า ทั้งของแถม + รับเข้าปกติ
                    decimal actionQtyUnit = details.FirstOrDefault(w => w.FKProductDtl == dtlId.FKProductDetail).RcvQuantity +
                        details.FirstOrDefault(w => w.FKProductDtl == dtlId.FKProductDetail).GiftQty;
                    valueDetail.Add(new WarehouseStockValue.Details()
                    {
                        ActionQtyUnit = actionQtyUnit,
                        FKItemRemark = MyConstant.ItemRemark.Nornal,
                        FKProductDetail = dtlId.FKProductDetail,
                        FKTransaction = MyConstant.WmsTransaction.RCV
                    });
                }
                // add rows
                warehouseValues.Add(new WarehouseStockValue()
                {
                    FKProduct = item,
                    ValueDetails = valueDetail
                });
            }
            // post warehouseValues to dll
            ManageWmsStock(warehouseValues);
        }

        private class MakeForStockValue
        {
            public int FKProduct { get; set; }
            public int FKProductDetail { get; set; }
        }
        /// <summary>
        /// สำหรับ เชคว่า สินค้าตัวไหน ยังไม่มีข้อมูล ใน Stock หรือยังไม่ตั้งต้น Stock
        /// </summary>
        public static void CheckHasStockStoreFrontAndWmsStock()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var products = db.Products.Where(w => w.Enable == true && w.ProductDetails.Where(a => a.Enable == true).Count() > 0).ToList();

                foreach (var item in products)
                {
                    // check in PosStock
                    if (item.PosStock == null)
                    {
                        // ถ้าไม่มีใน Stock หน้าร้าน
                        var details = item.ProductDetails.Where(w => w.Enable == true).ToList();
                        // add PosStock
                        PosStock ps = new PosStock();
                        ps.Name = item.ThaiName;
                        ps.Description = "initial stock";
                        ps.CreateDate = DateTime.Now;
                        ps.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        ps.UpdateDate = DateTime.Now;
                        ps.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        ps.FKShelf = MyConstant.Shelf.ShelfStart;
                        ps.Enable = true;
                        ps.CurrentQty = 0;
                        ps.OldQty = 0;
                        ps.FKProduct = item.Id;
                        PosStockDetails detailStock;
                        foreach (var dtlProd in details)
                        {
                            detailStock = new PosStockDetails();
                            detailStock.PackSize = dtlProd.PackSize;
                            detailStock.ResultQty = 0;
                            detailStock.ResultQtyUnit = 0;
                            detailStock.ActionQty = 0;
                            detailStock.ActionQtyUnit = 0;
                            detailStock.CreateDate = DateTime.Now;
                            detailStock.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                            detailStock.UpdateDate = DateTime.Now;
                            detailStock.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            detailStock.Description = "initial stock";
                            detailStock.Enable = true;
                            detailStock.FKProductDetails = dtlProd.Id;
                            detailStock.OldQty = 0;
                            detailStock.FKTransactionType = MyConstant.PosTransaction.SetStock;
                            ps.PosStockDetails.Add(detailStock);
                        }
                        db.PosStock.Add(ps);
                    }
                    // check in WmsStock
                    if (item.WmsStock == null)
                    {
                        // ถ้าไม่มีใน Stock หลังร้าน
                        var details = item.ProductDetails.Where(w => w.Enable == true).ToList();
                        // add WmsStock
                        WmsStock ws = new WmsStock();
                        ws.CreateDate = DateTime.Now;
                        ws.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        ws.UpdateDate = DateTime.Now;
                        ws.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        ws.Description = "initial stock";
                        ws.Enable = true;
                        ws.FKProduct = item.Id;
                        ws.FKWarehouse = MyConstant.WareHouse.MainWarehouse;
                        ws.FKZone = MyConstant.Zone.MainZone;
                        ws.OldQty = 0;
                        ws.CurrentQty = 0;
                        WmsStockDetail detailStock;
                        foreach (var dtlProd in details)
                        {
                            detailStock = new WmsStockDetail();
                            detailStock.FKItemRemark = MyConstant.ItemRemark.Nornal;
                            detailStock.FKProductDetail = dtlProd.Id;
                            detailStock.FKTransaction = MyConstant.WmsTransaction.SetStock;
                            detailStock.CreateDate = DateTime.Now;
                            detailStock.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                            detailStock.UpdateDate = DateTime.Now;
                            detailStock.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            detailStock.Description = "initial stock";
                            detailStock.Enable = true;
                            detailStock.PackSize = dtlProd.PackSize;
                            detailStock.ResultQty = 0;
                            detailStock.ResultQtyUnit = 0;
                            detailStock.ActionQty = 0;
                            detailStock.ActionQtyUnit = 0;
                            ws.WmsStockDetail.Add(detailStock);
                        }
                        db.WmsStock.Add(ws);
                    }
                }
                db.SaveChanges();
            }
        }

        public static bool checkLoad = true;
        /// <summary>
        /// หา % เช่น 20 ของ 160 ได้ 12.50 %
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allValue"></param>
        /// <returns></returns>
        public static decimal GetPercentFromValue(decimal value, decimal allValue)
        {
            decimal result = value * 100 / allValue;
            return result;
        }
        public static string ConvertBoolToStr(bool enable)
        {
            if (enable)
            {
                return "ใช้งาน";
            }
            else
            {
                return "ยกเลิก";
            }
        }
        public static string GetFullNameUserById(string id)
        {
            if (id == null)
            {
                return "";
            }
            var data = Singleton.SingletonPriority1.Instance().Users;
            string name = data.SingleOrDefault(w => w.Id == id).Name;
            return name;
        }
        /// <summary>
        /// Check วันที่เพิ่มหุ้น ยุ่ในช่วง อายุหุ้นกี่เดือน
        /// </summary>
        /// <param name="myDate">วันเช็ค</param>
        /// <param name="term1">ช่วงเริ่ม</param>
        /// <param name="term2">ช่วงจบ</param>
        /// <returns></returns>
        public static bool CheckAgeOfShared(DateTime myDate, DateTime term1, DateTime term2)
        {
            if (
                int.Parse(myDate.ToString("yyyyMMdd")) >= int.Parse(term1.ToString("yyyyMMdd")) &&
                int.Parse(myDate.ToString("yyyyMMdd")) <= int.Parse(term2.ToString("yyyyMMdd"))
                )
            {
                return true; // ยุ่ใน length
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// Month Diff
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static int MonthDiff(DateTime date1, DateTime date2)
        {
            if (date1.Month < date2.Month)
            {
                return (date2.Year - date1.Year) * 12 + date2.Month - date1.Month;
            }
            else
            {
                return (date2.Year - date1.Year - 1) * 12 + date2.Month - date1.Month + 12;
            }
        }

        /// <summary>
        /// แปลงวันที่ string เช่น 30/03/2533 เป็น DateTime En
        /// </summary>
        /// <param name="thDate"></param>
        /// <returns></returns>
        public static DateTime? ConvertTHToENDate(string thDate)
        {
            try
            {
                string[] dateArray = thDate.Split('/');
                int yyyy = int.Parse(dateArray[2]);
                yyyy = yyyy - 543;
                string date = dateArray[1] + "/" + dateArray[0] + "/" + yyyy;
                return DateTime.Parse(date);
            }
            catch (Exception)
            {
                return null;
            }

        }
        /// <summary>
        /// แปลงวันที่ เป็นไทย + เวลา
        /// </summary>
        /// <param name="engDate"></param>
        /// <returns></returns>
        public static string ConvertDateToThaiDate(DateTime? engDate, bool time)
        {
            if (engDate == null)
            {
                return "";
            }
            else
            {
                DateTime engDate1 = (DateTime)engDate;
                int thaiYear = engDate1.Year + 543;
                return engDate1.ToString("dd/MM/") + thaiYear + " " + engDate1.ToString("HH:mm");
            }

        }
        /// <summary>
        /// แปลงวันที่ เป็นไทย
        /// </summary>
        /// <param name="engDate"></param>
        /// <returns></returns>
        public static string ConvertDateToThaiDate(DateTime? engDate)
        {
            if (engDate == null)
            {
                return "";
            }
            else
            {
                DateTime engDate1 = (DateTime)engDate;
                int thaiYear = engDate1.Year + 543;
                return engDate1.ToString("dd/MM/") + thaiYear;
            }
        }
        /// <summary>
        /// ต่อ int ให้เป็น format 00001
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GenerateCodeFormCount(int count, int length)
        {
            string number = "" + count;
            while (number.Length < length)
            {
                number = "0" + number;
            }
            return number;
        }
        public static DateTime ConvertDateTime(string dd, string mm, string yyyy)
        {
            try
            {
                string dateString;
                CultureInfo culture;
                DateTimeStyles styles;
                DateTime result;

                dateString = mm + "/" + dd + "/" + yyyy;
                culture = CultureInfo.CreateSpecificCulture("en-US");
                styles = DateTimeStyles.None;
                result = DateTime.Parse(dateString, culture, styles);

                DateTime date = DateTime.Parse(mm + "/" + dd + "/" + yyyy);
                return date;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
        /// <summary>
        ///  เลขที่ PO gen auto
        /// </summary>
        /// <returns></returns>
        public static string GetPONoGenerateForLoadForm(string firstTerm, int fkBranch, int fkBudgetYear)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                int count = db.POGenerate.Where(w => w.FKBranch == fkBranch && w.FKBudgetYear == fkBudgetYear && w.Enable == true).
                    Count();
                count = count + 1;
                string number = "" + count;
                while (number.Length < 4)
                {
                    number = "0" + number;
                }
                firstTerm = firstTerm.Replace("-", "");
                return firstTerm + number;
            }
        }
        /// <summary>
        /// หา % เช่น 5 บาท คิดเป็น % ได้เท่าไหร่ของ total 276
        /// </summary>
        /// <param name="discount">ส่วนลด</param>
        /// <param name="total">ยอดรวมทั้งหมด ไม่รวมภาษี</param>
        /// <returns></returns>
        public static decimal GetPercentFromDiscountBath(decimal discount, decimal total)
        {
            decimal result = discount * 100 / total;
            return Convert.ToDecimal(String.Format("{0:0.00}", result).ToString());
        }
        /// <summary>
        /// หาค่า เช่น 10% ของ 950 คือเท่าไหร่? 95
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal CalPercentByValue(decimal value, decimal percent)
        {
            try
            {
                return value * percent / 100;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// เช็คว่า คีย์ 120บ หรือ 120 | [0] == 120, [1] == บ
        /// ถ้าเป็น %              | [0] == 120, [1] == ""
        /// </summary>
        /// <param name="value"></param>
        /// <returns> [0] == 120, [1] == บ</returns>
        public static string[] GetCheckBathOrPercent(string value)
        {
            string first = value.Substring(0, value.Length - 1); // 120
            string last = value.Substring(value.Length - 1, 1); // บ
            if (last != "บ")
            {
                last = "";
            }
            if (last == "")
            {
                first = value;
            }
            string[] subString = { first, last };
            return subString;
        }
        /// <summary>
        /// หากำไรข้างต้น
        /// </summary>
        /// <param name="value">ราคาขาย/หนวย</param>
        /// <param name="costAndVat">ราคาทุนบวกภาษี</param>
        /// <returns></returns>
        public static string CalMonneyPercent(decimal value, decimal costAndVat)
        {
            try
            {
                value = (value - costAndVat) * 100 / value;
                var vatValue = Convert.ToDecimal(String.Format("{0:0.00}", value).ToString());
                return vatValue + " %";
            }
            catch (Exception)
            {

                return "Error";
            }

        }
        /// <summary>
        /// หาภาษี จากยอดที่รวมภาษีมาแล้ว หรือถอด Vat
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal CalVatFromValue(decimal value)
        {
            decimal vat = value * (decimal)MyConstant.MyVat.Vat / MyConstant.MyVat.VatRemove;
            var vatValue = Convert.ToDecimal(String.Format("{0:0.00}", vat).ToString());
            return vatValue;
        }
        /// <summary>
        /// หา ภาษี จากทุนเปล่า
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal CalCostVat(decimal value)
        {
            decimal vat = value * (decimal)0.07;
            var vatValue = Convert.ToDecimal(String.Format("{0:0.00}", vat).ToString());
            return vatValue;
        }
        /// <summary>
        /// แปลง "" null is 0
        /// </summary>
        /// <returns></returns>
        public static decimal ConvertStringToDecimalZero(string value)
        {
            switch (value)
            {
                case "":
                    return 0;
                case null:
                    return 0;
                default:
                    return decimal.Parse(value);
            }
        }
        /// <summary>
        /// แปลง format ใหม่เปน #,###,###.#0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertDecimalToStringForm(decimal? value)
        {

            if (value == null || value == 0)
            {
                return "0.00";
            }
            else if (Math.Abs((decimal)value) > 0 && Math.Abs((decimal)value) < 1)
            {
                return "0" + string.Format("{0:#,###,###.#0}", value);
            }
            //string returnString = string.Format("{0:#,###,###.#0}", value);
            return string.Format("{0:#,###,###.#0}", value);
        }
        public static string ConvertDecimalToStringForm(decimal value, string type)
        {
            //else if (value < 1000)
            //{
            //    return value.ToString();
            //}
            //string returnString = string.Format("{0:#,###,###.#0}", value);
            return string.Format("{0:#,###,###}", value);
        }
        /// <summary>
        /// แปลงพวก 1,000.21;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ConvertStringToDecimal(string value)
        {
            string a = value.Replace(",", "");
            return decimal.Parse(value);
        }
        /// <summary>
        /// จำนวนเงินเปน สตริง
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string ThaiBaht(string txt)
        {
            try
            {
                string bahtTxt, n, bahtTH = "";
                double amount;
                try { amount = Convert.ToDouble(txt); }
                catch { amount = 0; }
                bahtTxt = amount.ToString("####.00");
                string[] num = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
                string[] rank = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };
                string[] temp = bahtTxt.Split('.');
                string intVal = temp[0];
                string decVal = temp[1];
                if (Convert.ToDouble(bahtTxt) == 0)
                    bahtTH = "ศูนย์บาทถ้วน";
                else
                {
                    for (int i = 0; i < intVal.Length; i++)
                    {
                        n = intVal.Substring(i, 1);
                        if (n != "0")
                        {
                            if ((i == (intVal.Length - 1)) && (n == "1"))
                                bahtTH += "เอ็ด";
                            else if ((i == (intVal.Length - 2)) && (n == "2"))
                                bahtTH += "ยี่";
                            else if ((i == (intVal.Length - 2)) && (n == "1"))
                                bahtTH += "";
                            else
                                bahtTH += num[Convert.ToInt32(n)];
                            bahtTH += rank[(intVal.Length - i) - 1];
                        }
                    }
                    bahtTH += "บาท";
                    if (decVal == "00")
                        bahtTH += "ถ้วน";
                    else
                    {
                        for (int i = 0; i < decVal.Length; i++)
                        {
                            n = decVal.Substring(i, 1);
                            if (n != "0")
                            {
                                if ((i == decVal.Length - 1) && (n == "1"))
                                    bahtTH += "เอ็ด";
                                else if ((i == (decVal.Length - 2)) && (n == "2"))
                                    bahtTH += "ยี่";
                                else if ((i == (decVal.Length - 2)) && (n == "1"))
                                    bahtTH += "";
                                else
                                    bahtTH += num[Convert.ToInt32(n)];
                                bahtTH += rank[(decVal.Length - i) - 1];
                            }
                        }
                        bahtTH += "สตางค์";
                    }
                }
                return bahtTH;
            }
            catch (Exception)
            {
                MessageBox.Show("จำนวนเงินเกินสิบล้าน");
            }
            finally
            {

            }
            return "";
        }
        /// <summary>
        /// ทำคืนของในห้องของเสียให้ vendor
        /// </summary>
        /// <param name="details"></param>
        /// <param name="docReference"></param>
        public static void AddWasteWarehouse(List<CNWarehouseDetails> details, string docReference)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                List<WasteWarehouseDetails> wds = new List<WasteWarehouseDetails>();
                WasteWarehouseDetails wd;
                List<int> stackFKProducts = new List<int>();
                foreach (var item in details)
                {
                    var getProdict = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails);
                    // get WasteWarehouse
                    var wasteWarehouse = db.WasteWarehouse.FirstOrDefault(w => w.Enable == true && w.FKProduct == getProdict.FKProduct);
                    stackFKProducts.Add(wasteWarehouse.Id);
                    wd = new WasteWarehouseDetails();
                    wd.DocReference = docReference;
                    wd.Description = item.Description;
                    wd.Enable = true;
                    wd.CreateDate = DateTime.Now;
                    wd.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    wd.UpdateDate = DateTime.Now;
                    wd.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    wd.FKWasteWarehouse = wasteWarehouse.Id;
                    wd.FKProductDetails = item.FKProductDetails;
                    wd.QtyUnit = item.Qty;
                    wd.Packsize = getProdict.PackSize;
                    wd.QtyPiece = item.Qty * getProdict.PackSize;
                    wd.IsInOrOut = false; // false -
                    var lastWasteStock = db.WasteWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == item.FKProductDetails).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                    wd.LastResultUnit = wd.QtyUnit;
                    wd.LastResultPiece = wd.QtyPiece;

                    if (lastWasteStock != null)
                    {
                        wd.LastResultUnit = lastWasteStock.LastResultUnit - wd.QtyUnit;
                        wd.LastResultPiece = lastWasteStock.LastResultPiece - wd.QtyPiece;
                    }
                    wds.Add(wd);
                    //wasteWarehouse.QtyUnit += wd.QtyUnit;
                    //wasteWarehouse.QtyPiece += wd.QtyPiece;
                    //wasteWarehouse.UpdateDate = DateTime.Now;
                    //wasteWarehouse.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    //db.Entry(wasteWarehouse).State = EntityState.Modified;
                }
                db.WasteWarehouseDetails.AddRange(wds);
                db.SaveChanges();
                /// Update Header
                var wasteWarehouseList = db.WasteWarehouse.Where(w => stackFKProducts.Contains(w.Id)).ToList();
                foreach (var item in wasteWarehouseList)
                {
                    //decimal resultUnitLast = 0;
                    //decimal resultPieceLast = 0;
                    item.QtyUnit = 0;
                    item.QtyPiece = 0;
                    /// get last for details
                    IEnumerable<int> groupFKProducts = item.WasteWarehouseDetails.Select(w => w.FKProductDetails).Distinct().ToList<int>();
                    foreach (var lastFKProDtl in groupFKProducts)
                    {
                        var lastWasteStock = db.WasteWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == lastFKProDtl).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        item.QtyUnit += lastWasteStock.LastResultUnit;
                        item.QtyPiece += lastWasteStock.LastResultPiece;
                    }
                    //item.QtyUnit = resultUnitLast;
                    //item.QtyPiece = resultPieceLast;
                    item.UpdateDate = DateTime.Now;
                    item.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }
        /// <summary>
        /// เพิ่มยอดใน ห้องของเสีย +true จากคลัง 
        /// </summary>
        /// <param name="details"></param>
        public static void AddWasteWarehouse(List<WarehouseToWasteDetails> details, string docReference)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                List<WasteWarehouseDetails> wds = new List<WasteWarehouseDetails>();
                WasteWarehouseDetails wd;
                List<int> stackFKProducts = new List<int>();
                foreach (var item in details)
                {
                    var getProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails);
                    // get WasteWarehouse
                    var wasteWarehouse = db.WasteWarehouse.FirstOrDefault(w => w.Enable == true && w.FKProduct == getProduct.FKProduct);
                    stackFKProducts.Add(wasteWarehouse.Id);
                    wd = new WasteWarehouseDetails();
                    wd.DocReference = docReference;
                    wd.Description = item.Description;
                    wd.Enable = true;
                    wd.CreateDate = DateTime.Now;
                    wd.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    wd.UpdateDate = DateTime.Now;
                    wd.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    wd.FKWasteWarehouse = wasteWarehouse.Id;
                    wd.FKProductDetails = item.FKProductDetails;
                    wd.QtyUnit = item.QtyUnit;
                    wd.Packsize = item.Packsize;
                    wd.QtyPiece = item.QtyUnit * item.Packsize;
                    wd.IsInOrOut = true;
                    var lastWasteStock = db.WasteWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == item.FKProductDetails).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                    wd.LastResultUnit = wd.QtyUnit;
                    wd.LastResultPiece = wd.QtyPiece;
                    wd.CostOnly = getProduct.CostOnly;
                    wd.SellPrice = getProduct.SellPrice;
                    if (lastWasteStock != null)
                    {
                        wd.LastResultUnit = wd.QtyUnit + lastWasteStock.LastResultUnit;
                        wd.LastResultPiece = wd.QtyPiece + lastWasteStock.LastResultPiece;
                    }
                    wds.Add(wd);
                    //wasteWarehouse.QtyUnit += wd.QtyUnit;
                    //wasteWarehouse.QtyPiece += wd.QtyPiece;
                    //wasteWarehouse.UpdateDate = DateTime.Now;
                    //wasteWarehouse.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    //db.Entry(wasteWarehouse).State = EntityState.Modified;
                }
                db.WasteWarehouseDetails.AddRange(wds);
                db.SaveChanges();
                /// Update Header
                var wasteWarehouseList = db.WasteWarehouse.Where(w => stackFKProducts.Contains(w.Id)).ToList();
                foreach (var item in wasteWarehouseList)
                {
                    decimal resultUnitLast = 0;
                    decimal resultPieceLast = 0;
                    /// get last for details
                    IEnumerable<int> groupFKProducts = item.WasteWarehouseDetails.Select(w => w.FKProductDetails).Distinct().ToList<int>();
                    foreach (var lastFKProDtl in groupFKProducts)
                    {
                        var lastWasteStock = db.WasteWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == lastFKProDtl).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        resultUnitLast += lastWasteStock.LastResultUnit;
                        resultPieceLast += lastWasteStock.LastResultPiece;
                    }
                    item.QtyUnit = resultUnitLast;
                    item.QtyPiece = resultPieceLast;
                    item.UpdateDate = DateTime.Now;
                    item.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }
        /// <summary>
        /// เพิ่มยอดใน ห้องของเสีย +true จากหน้าร้าน
        /// </summary>
        /// <param name="details"></param>
        public static void AddWasteWarehouse(List<StoreFrontTransferWasteDtl> details, string docReference)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                List<WasteWarehouseDetails> wds = new List<WasteWarehouseDetails>();
                WasteWarehouseDetails wd;
                List<int> stackFKProducts = new List<int>();
                foreach (var item in details)
                {
                    var getProdict = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails);
                    // get WasteWarehouse
                    var wasteWarehouse = db.WasteWarehouse.FirstOrDefault(w => w.Enable == true && w.FKProduct == getProdict.FKProduct);
                    stackFKProducts.Add(wasteWarehouse.Id);
                    wd = new WasteWarehouseDetails();
                    wd.DocReference = docReference;
                    wd.Description = item.Description;
                    wd.Enable = true;
                    wd.CreateDate = DateTime.Now;
                    wd.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    wd.UpdateDate = DateTime.Now;
                    wd.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    wd.FKWasteWarehouse = wasteWarehouse.Id;
                    wd.FKProductDetails = item.FKProductDetails;
                    wd.QtyUnit = item.Qty;
                    wd.Packsize = getProdict.PackSize;
                    wd.QtyPiece = item.Qty * getProdict.PackSize;
                    wd.IsInOrOut = true;
                    var lastWasteStock = db.WasteWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == item.FKProductDetails).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                    wd.LastResultUnit = wd.QtyUnit;
                    wd.LastResultPiece = wd.QtyPiece;
                    wd.CostOnly = getProdict.CostOnly;
                    wd.SellPrice = getProdict.SellPrice;
                    if (lastWasteStock != null)
                    {
                        wd.LastResultUnit = wd.QtyUnit + lastWasteStock.LastResultUnit;
                        wd.LastResultPiece = wd.QtyPiece + lastWasteStock.LastResultPiece;
                    }
                    wds.Add(wd);
                    //wasteWarehouse.QtyUnit += wd.QtyUnit;
                    //wasteWarehouse.QtyPiece += wd.QtyPiece;
                    //wasteWarehouse.UpdateDate = DateTime.Now;
                    //wasteWarehouse.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    //db.Entry(wasteWarehouse).State = EntityState.Modified;
                }
                db.WasteWarehouseDetails.AddRange(wds);
                db.SaveChanges();
                /// Update Header
                var wasteWarehouseList = db.WasteWarehouse.Where(w => stackFKProducts.Contains(w.Id)).ToList();
                foreach (var item in wasteWarehouseList)
                {
                    decimal resultUnitLast = 0;
                    decimal resultPieceLast = 0;
                    /// get last for details
                    IEnumerable<int> groupFKProducts = item.WasteWarehouseDetails.Select(w => w.FKProductDetails).Distinct().ToList<int>();
                    foreach (var lastFKProDtl in groupFKProducts)
                    {
                        var lastWasteStock = db.WasteWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == lastFKProDtl).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        if (lastWasteStock == null)
                        {
                            continue;
                        }
                        resultUnitLast += lastWasteStock.LastResultUnit;
                        resultPieceLast += lastWasteStock.LastResultPiece;
                    }
                    item.QtyUnit = resultUnitLast;
                    item.QtyPiece = resultPieceLast;
                    item.UpdateDate = DateTime.Now;
                    item.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }
        /// initial product not in WasteWarehouse
        public static void InitialProductWasteWarehouse()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = Singleton.SingletonProduct.Instance().Products.Where(w => w.Enable == true).ToList();
                WasteWarehouse ww;
                List<WasteWarehouse> ws = new List<WasteWarehouse>();
                List<int> ids = db.WasteWarehouse.Where(w => w.Enable == true).Select(w => w.FKProduct).ToList<int>();
                int i = 1;
                foreach (var item in data)
                {
                    if (ids.Contains(item.Id))
                    {
                        continue;
                    }
                    ww = new WasteWarehouse();
                    ww.FKProduct = item.Id;
                    ww.FKWarehouse = MyConstant.WareHouse.WasteWarehouse;
                    ww.QtyUnit = 0;
                    ww.QtyPiece = 0;
                    ww.Description = "Initial Stock";
                    ww.Enable = true;
                    ww.CreateDate = DateTime.Now;
                    ww.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    ww.UpdateDate = DateTime.Now;
                    ww.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    ws.Add(ww);
                    Console.WriteLine(i + " " + item.ThaiName);
                    i++;
                }
                if (ws.Count == 0)
                {
                    return;
                }
                db.WasteWarehouse.AddRange(ws);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Generate Pos No เมื่อขาย แต่ละเครื่อง *ยกเลิก โดยพี่หลิน
        /// bb ccc xxxxxxx
        /// </summary>
        /// <param name="posMachine"></param>
        /// <returns></returns>
        public static string GetGeneratePosNo(PosMachine posMachine)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                //int count = db.PosGenerateCode.Where(w => w.Enable == true &&
                //DbFunctions.TruncateTime(DateTime.Now) == DbFunctions.TruncateTime(w.CreateDate) && w.FKPosMachine == posMachine.Id).Count();
                //string firstTerm = Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear;
                //firstTerm = firstTerm + DateTime.Now.ToString("MMdd") + posMachine.PosNo;
                //count = count + 1;
                //string number = "" + count;
                //while (number.Length < 4)
                //{
                //    number = "0" + number;
                //}
                //return firstTerm + number;
                var code = Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear;
                int id = Singleton.SingletonThisBudgetYear.Instance().ThisYear.Id;
                var count = db.PosHeader.Where(w => w.FKBudgetYear == id).Count();
                string number = "" + count;
                while (number.Length < 7)
                {
                    number = "0" + number;
                }
                return code + posMachine.PosNo + number;
            }
        }
        public static DateTime DateTimeServer()

        {

            using (SSLsEntities context = new SSLsEntities())

            {

                dynamic Query = context.Users.Select(c => DateTime.Now).FirstOrDefault();

                return Query;

            }

        }
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public static string TradPickBigCar(string tb_cusno, string tb_sentno, string tb_po_distcount, string tb_statusno, int UserId, string tb_costall, string detail, DataTable dt, string invoice)
        {
            string check = "";
            DataTable ds_poid = new DataTable();
            ds_poid = CodeFileDLL.selectsql("select yyyymm,yymm,max(cnt_inv) cnt_inv from( " + " select left(convert(varchar,getdate(),112),6) yyyymm,left(convert(varchar,getdate(),12),4) yymm,case when charindex('/',max(invoice_no)) > 0 then MAX(CONVERT(INT,substring(invoice_no,9,len(invoice_no)))) else 0 end cnt_inv " + " FROM ps_iv_order_temp " + " where left(convert(varchar,create_date,12),4) = left(convert(varchar,getdate(),12),4) " + " union all " + " select left(convert(varchar,getdate(),112),6) yyyymm,left(convert(varchar,getdate(),12),4) yymm,case when charindex('/',max(invoice_no)) > 0 then MAX(CONVERT(INT,substring(invoice_no,9,len(invoice_no)))) else 0 end cnt_inv " + " FROM ps_iv_order " + " where left(convert(varchar,create_date,12),4) = left(convert(varchar,getdate(),12),4) " + " )tb1 group by yyyymm,yymm ");

            int run_no = int.Parse(ds_poid.Rows[0]["cnt_inv"].ToString());
            string yymm = ds_poid.Rows[0]["yymm"].ToString();
            string yyyymm = ds_poid.Rows[0]["yyyymm"].ToString();
            string inv_no = "INV" + yymm + "/" + (run_no + 1);
            string po_insert = "";
            string po_insert1 = "";
            double s1 = 0;

            foreach (DataRow Row in dt.Rows)
            {
                po_insert = "('" + inv_no + "'," + Row["PRODUCT_ID"].ToString() + ",'" + Row["PRODUCT_NO"].ToString() + "' " + ",'" + Row["PRODUCT_NAME"].ToString().Trim() + "','" + Row["UNIT_NAME"].ToString().Trim() + "','" + Row["QTY"].ToString().Trim() + "' " + ",'" + Row["GIVEAWAY"].ToString().Trim() + "','" + Row["COST"].ToString().Trim() + "','" + Row["DISCOUNT"].ToString().Trim() + "','" + Row["COST_TOTAL"].ToString().Trim() + "'),";
                po_insert1 += po_insert;

                Console.WriteLine(Row["QTY"]);
                s1 += double.Parse(Row["QTY"].ToString());
            }
            po_insert1 = po_insert1.Substring(0, po_insert1.Length - 1).ToString();
            if (CodeFileDLL.excelsql("INSERT INTO [dbo].[PS_IV_HD_TEMP] ([INVOICE_NO],[MEMBER_ID],[COMPANY_NO],[IV_DISCOUNT],[PAYMENT_TYPE],[CREATE_DATE],[UPDATE_DATE], " + "[USER_NO],[ENABLE],[ENABLE_USER],[TOTAL_QTY],[SUBTOTAL],[TAX],[PS_NOTE]) VALUES ('" + inv_no + "','" + tb_cusno.Trim() + "', " + "'" + tb_sentno.Trim() + "','" + tb_po_distcount.Trim() + "','" + tb_statusno.Trim() + "',GETDATE(),GETDATE(),'" + UserId + "',1,'" + UserId + "', " + "" + s1 + ",'" + (double.Parse(tb_costall.Trim()) - double.Parse(tb_po_distcount.Trim())) + "','" + CodeFileDLL.caltax(double.Parse(tb_costall.Trim()) - double.Parse(tb_po_distcount.Trim())) + "','" + detail.Trim() + "')" + "INSERT INTO [dbo].[PS_IV_DTL_TEMP] ([INVOICE_NO],[PRODUCT_ID],[PRODUCT_NO],[PRODUCT_NAME],[UNIT_NAME],[QTY],[GIVEAWAY],[COST], " + "[DISCOUNT],[COST_TOTAL]) VALUES " + po_insert1 + " " + "INSERT INTO [dbo].[PS_IV_ORDER_TEMP] (INVOICE_ID, INVOICE_NO, OBJ_ID, OBJ_NAME, CREATE_DATE, USER_ID, ENABLE) VALUES " + "((select IDENT_CURRENT('[dbo].[PS_IV_HD_TEMP]')),'" + inv_no + "',OBJECT_ID('PS_IV_HD" + yyyymm + "'),'PS_IV_HD" + yyyymm + "',getdate(),'" + UserId + "',1) ") == false)
            {
                check = "บันทึกลง PS_IV ไม่ผ่าน";
            }
            else
            {
                if (CodeFileDLL.excelsql(" exec SP_INVOICE_TEMP '" + inv_no + "' ") == false)
                {
                    check = "stored procedure (SP_INVOICE_TEMP) ไม่ผ่าน";
                }
                else
                {
                    if (CodeFileDLL.excelsql(" exec sp_sel_iss 'ISS2CUST'," + UserId + "  ") == false)
                    {
                        check = "stored procedure (sp_sel_iss) ไม่ผ่าน";
                    }
                    else
                    {

                        SSLsEntities db = new SSLsEntities();
                        WH_TRATEntities wh = new WH_TRATEntities();
                        try
                        {
                            var ds = db.SaleOrderWarehouse.Where(x => x.Enable == true & x.Code == invoice).FirstOrDefault();
                            if (ds == null)
                            {
                                check = "ไม่พบข้อมูล SOR : " + invoice;
                            }
                            else
                            {
                                ds.InvoiceNoWH = inv_no;
                                db.SaveChanges();




                                var dtget = "select convert(varchar,iv.PS_DOC) PSDOC,substring(convert(varchar,ps.OBJ_NAME),5,6) OBJNAME from PS_IV_ORDER iv left join PS_ORDER ps on iv.PS_DOC = ps.INVOICE_NO where iv.INVOICE_NO = '" + inv_no + "'";

                                SqlConnection sqlConn = new SqlConnection(wh.Database.Connection.ConnectionString);
                                sqlConn.Open();
                                SqlCommand cmd = new SqlCommand(dtget, sqlConn);
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                DataTable dt1 = new DataTable();
                                da.Fill(dt1);



                                if (dt1.Rows.Count > 0)
                                {

                                    var PSDOC = dt1.Rows[0][0];
                                    var OBJNAME = dt1.Rows[0][1];

                                    if (dt1.Rows[0]["PSDOC"] == null)
                                    {
                                        db.SaleOrderWarehouseDtl.Where(x => x.Enable == true & x.SaleOrderWarehouse.Code == invoice & x.SaleOrderWarehouse.Enable == true).ToList().ForEach(u =>
                                        {
                                            u.QtyAllow = 0;
                                        });
                                        db.SaveChanges();
                                    }
                                    else
                                    {

                                        var uu = db.SaleOrderWarehouseDtl.Where(x => x.Enable == true & x.SaleOrderWarehouse.Code == invoice & x.SaleOrderWarehouse.Enable == true).ToList();

                                        foreach (var item in uu)
                                        {
                                            var dtcheck = "select product_no,qty from ps_d" + OBJNAME + " where invoice_no = '" + PSDOC + "' and product_no = '" + item.ProductDetails.Code + "'";

                                            SqlConnection sqlConn2 = new SqlConnection(wh.Database.Connection.ConnectionString);
                                            sqlConn2.Open();
                                            SqlCommand cmd2 = new SqlCommand(dtcheck, sqlConn2);
                                            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                                            DataTable dt2 = new DataTable();
                                            da2.Fill(dt2);


                                            if (dt2.Rows.Count > 0)
                                            {
                                                decimal qty = decimal.Parse(dt2.Rows[0][1].ToString());
                                                item.QtyAllow = qty;
                                            }
                                            else
                                            {
                                                item.QtyAllow = 0;
                                            }
                                            db.Entry(item).State = EntityState.Modified;
                                        }

                                        db.SaveChanges();
                                    }
                                }
                                else
                                {
                                    check = "ไม่พบบิล : " + inv_no + " ใน PS_IV_ORDER";
                                }
                            }
                        }
                        catch
                        {
                            check = "UPDATE InvoiceNoWH ใน SaleOrderWarehouse ไม่สำเร็จ ";
                        }
                        finally
                        {
                            db.Dispose();
                            wh.Dispose();
                        }


                    }
                }
            }
            return check;
        }
    }
}
