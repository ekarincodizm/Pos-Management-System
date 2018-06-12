using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System
{
    public static class LibraryInitialAllStock
    {
        /// <summary>
        ///  not use
        /// </summary>
        /// <param name="fkProduct"></param>
        static void InitialPosStock(int fkProduct)
        {
            SSLsEntities db = new SSLsEntities();
            //using (SSLsEntities db = new SSLsEntities())
            //{
            var products = db.PosStock.Where(w => w.Enable == true && w.FKProduct == fkProduct).ToList();
            if (products.Count() == 0)
            {
                List<PosStock> posStocks = new List<PosStock>();
                PosStock pos;
                int i = 1;
                foreach (var item in products)
                {
                    pos = new PosStock();
                    pos.CreateBy = "admin";
                    pos.CreateDate = DateTime.Now;
                    pos.CurrentQty = 0;
                    pos.Description = "Initial Stock ";
                    pos.Enable = true;
                    pos.FKProduct = item.Id;
                    pos.Name = "-";
                    pos.OldQty = 0;
                    pos.UpdateBy = "admin";
                    pos.UpdateDate = DateTime.Now;
                    pos.FKShelf = 21;
                    posStocks.Add(pos);
                }
                db.BulkInsert(posStocks);
                db.SaveChanges();
            }
        }

        public static void DetectNewBarcode(int fkProduct)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var posStock = db.PosStock.FirstOrDefault(w => w.FKProduct == fkProduct);
                List<PosStockDetails> dtlsNew = new List<PosStockDetails>();
                PosStockDetails dtlNewBar;
                if (posStock != null)
                {
                    /// แปลว่ามีฐานสินค้าแล้ว ใน stock หน้าร้าน
                    var getPro = db.Products.SingleOrDefault(w => w.Id == fkProduct);
                    foreach (var item in getPro.ProductDetails.Where(w => w.Enable == true))
                    {
                        var posStockDetails = posStock.PosStockDetails.Where(w => w.Enable == true && w.FKProductDetails == item.Id).ToList();
                        if (posStockDetails.Count > 0)
                        {
                            // แปลว่า บาร์นี้ เคย add เข้าแล้ว
                            continue;
                        }
                        dtlNewBar = new PosStockDetails();
                        dtlNewBar.FKPosStock = posStock.Id;
                        dtlNewBar.Description = "initial stock new barcode";
                        dtlNewBar.CreateDate = DateTime.Now;
                        dtlNewBar.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        dtlNewBar.UpdateDate = DateTime.Now;
                        dtlNewBar.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        dtlNewBar.Enable = true;
                        dtlNewBar.FKTransactionType = MyConstant.PosTransaction.SetStock;
                        dtlNewBar.ActionQty = 0;
                        dtlNewBar.ActionQtyUnit = 0;
                        dtlNewBar.FKProductDetails = item.Id;
                        dtlNewBar.OldQty = 0;
                        dtlNewBar.ResultQty = 0;
                        dtlNewBar.ResultQtyUnit = 0;
                        dtlNewBar.PackSize = item.PackSize;
                        dtlsNew.Add(dtlNewBar);
                    }
                    db.PosStockDetails.AddRange(dtlsNew);
                    db.SaveChanges();
                }
                else
                {
                    var getPro = db.Products.SingleOrDefault(w => w.Id == fkProduct);
                    // ยังไม่มี
                    PosStock pos = new PosStock();
                    List<PosStockDetails> dtls = new List<PosStockDetails>();
                    PosStockDetails dtl;
                    var detailsPro = getPro.ProductDetails.Where(w => w.Enable == true).OrderBy(w => w.PackSize);
                    pos.Name = detailsPro.FirstOrDefault().Code;
                    pos.Description = "initial stock new product";
                    pos.CreateDate = DateTime.Now;
                    pos.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    pos.UpdateDate = DateTime.Now;
                    pos.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    pos.Enable = true;
                    pos.CurrentQty = 0;
                    pos.OldQty = 0;
                    pos.FKProduct = fkProduct;
                    pos.FKShelf = MyConstant.Shelf.ShelfStart;

                    foreach (var item in detailsPro)
                    {
                        dtl = new PosStockDetails();
                        dtl.Description = "initial stock new product";
                        dtl.CreateDate = DateTime.Now;
                        dtl.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        dtl.UpdateDate = DateTime.Now;
                        dtl.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        dtl.Enable = true;
                        dtl.FKTransactionType = MyConstant.PosTransaction.SetStock;
                        dtl.ActionQty = 0;
                        dtl.ActionQtyUnit = 0;
                        dtl.FKProductDetails = item.Id;
                        dtl.OldQty = 0;
                        dtl.ResultQty = 0;
                        dtl.ResultQtyUnit = 0;
                        dtl.PackSize = item.PackSize;
                        dtls.Add(dtl);
                    }
                    pos.PosStockDetails = dtls;
                    db.PosStock.Add(pos);
                    db.SaveChanges();
                }
            }
        }
    }
}
