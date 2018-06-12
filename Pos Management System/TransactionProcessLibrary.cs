using Pos_Management_System.Model;
using Pos_Management_System.Singleton;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System
{
    public static class TransactionProcessLibrary
    {
        /// <summary>
        /// ความเคลื่อนไหว ของ Store Front All Trancsaction
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            SingletonPOsTransaction.Instance();
            using (SSLsEntities db = new SSLsEntities())
            {
                string input = "";
                List<ObjectPOsTransaction> objs = new List<ObjectPOsTransaction>();
                do
                {
                    input = Console.ReadLine();
                    string[] splitInput = input.Split('-');
                    if (splitInput.Count() == 3)
                    {
                        // pass 
                        string code = splitInput[0];
                        ProductDetails productDtl = db.ProductDetails.Include("Products").FirstOrDefault(w => w.Enable == true && w.Code == code);
                        decimal qty = decimal.Parse(splitInput[1]);
                        int fkTransaction = int.Parse(splitInput[2]);
                        objs.Add(new ObjectPOsTransaction()
                        {
                            ProductDetail = productDtl,
                            ActionQty = qty,
                            FKTransaction = fkTransaction
                        });
                    }
                } while (input != "*");
                Console.WriteLine(" ****************** End Input *************** ");
                foreach (var item in objs)
                {
                    Console.WriteLine("Product: " + item.ProductDetail.Code + " Qty*" + item.ProductDetail.PackSize + ": " + item.ActionQty + " Small Unit: " + (item.ActionQty * item.ProductDetail.PackSize));
                }
                Console.WriteLine(" ****************** End Process *************** ");
                POsTransactionProcess(objs);
                Console.Read();
            }
        }

        /// <summary>
        /// Main Process Store Front Transaction
        /// </summary>
        /// <param name="list">map มา และกรุ๊ปมา ให้เรียบร้อย</param>
        public static void POsTransactionProcess(List<ObjectPOsTransaction> list)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                decimal actionQty = 0;
                decimal actionQtyUnit = 0;
                foreach (var item in list)
                {
                    PosStock headerStock = db.PosStock.FirstOrDefault(w => w.Enable == true && w.FKProduct == item.ProductDetail.FKProduct);
                    TransactionType transaction = SingletonPOsTransaction.Instance().TransactionTypes.SingleOrDefault(w => w.Id == item.FKTransaction);
                    if (transaction.IsPlus)
                    {
                        // แสดงว่า ค่า +
                        actionQtyUnit = item.ActionQty; // จำนวนหน่วย
                        actionQty = item.ActionQty * item.ProductDetail.PackSize; // จำนวน เล็กสุด ชิ้น
                    }
                    else
                    {
                        actionQtyUnit = -item.ActionQty; // จำนวนหน่วย
                        actionQty = -item.ActionQty * item.ProductDetail.PackSize; // จำนวน เล็กสุด ชิ้น
                    }
                    // ถ้าไม่มีใน pos store front
                    if (headerStock == null)
                    {
                        // is add
                        headerStock = new PosStock();
                        headerStock.Name = item.ProductDetail.Name;
                        headerStock.Description = "Auto Add";
                        headerStock.CreateDate = DateTime.Now;
                        headerStock.CreateBy = SingletonAuthen.Instance().Id;
                        headerStock.UpdateDate = DateTime.Now;
                        headerStock.UpdateBy = SingletonAuthen.Instance().Id;
                        headerStock.Enable = true;
                        headerStock.CurrentQty = actionQty;
                        headerStock.FKProduct = item.ProductDetail.FKProduct;

                        // details 
                        PosStockDetails details = new PosStockDetails();
                        details.Description = transaction.Name;
                        details.CreateDate = DateTime.Now;
                        details.CreateBy = SingletonAuthen.Instance().Id;
                        details.UpdateBy = SingletonAuthen.Instance().Id;
                        details.UpdateDate = DateTime.Now;
                        details.Enable = true;
                        details.FKTransactionType = item.FKTransaction;
                        details.ActionQty = actionQty;
                        details.ActionQtyUnit = actionQtyUnit;
                        details.FKProductDetails = item.ProductDetail.Id;

                        headerStock.PosStockDetails.Add(details);
                        db.PosStock.Add(headerStock);
                    }
                    else // ถ้ามีแล้วใน store front
                    {
                        // add transaction ก่อน
                        PosStockDetails details = new PosStockDetails();
                        details.Description = transaction.Name;
                        details.CreateDate = DateTime.Now;
                        details.CreateBy = SingletonAuthen.Instance().Id;
                        details.UpdateBy = SingletonAuthen.Instance().Id;
                        details.UpdateDate = DateTime.Now;
                        details.Enable = true;
                        details.FKTransactionType = item.FKTransaction;
                        details.ActionQty = actionQty;
                        details.ActionQtyUnit = actionQtyUnit;
                        details.FKProductDetails = item.ProductDetail.Id;
                        details.FKPosStock = headerStock.Id;
                        db.PosStockDetails.Add(details);
                        // is Update header
                        headerStock.UpdateDate = DateTime.Now;
                        headerStock.UpdateBy = SingletonAuthen.Instance().Id;
                        headerStock.CurrentQty = headerStock.CurrentQty + actionQty;
                        db.Entry(headerStock).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
