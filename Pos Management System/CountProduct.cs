using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class CountProduct : Form
    {
        public CountProduct()
        {
            InitializeComponent();
        }

        private void checkBoxProduct_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxProduct.Checked == true)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox1.Text = null;
                textBox2.Text = null;
            }
            else
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox1.Text = null;
                textBox2.Text = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lb_msg.Visible = false;
            checkBoxProduct.Checked = true;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox1.Text = null;
            textBox2.Text = null;
            dateTimePickerStart.Value = DateTime.Now;
            dateTimePickerEnd.Value = DateTime.Now;
            progressBar1.Value = 0;
        }

        private void CountProduct_Load(object sender, EventArgs e)
        {
            checkBoxProduct.Checked = true;
            lb_msg.Visible = false;
            using (SSLsEntities db = new SSLsEntities())
            {
                var getValueDoc = db.StoreFrontValueDoc.Where(w => w.Enable.Equals(true)).ToList();
                AutoCompleteStringCollection doc_S = new AutoCompleteStringCollection();
                foreach (var item in getValueDoc)
                {
                    doc_S.Add(item.DocNo);
                }
                textBox1.AutoCompleteCustomSource = doc_S; // จาก
                // - -------------------- - //
                AutoCompleteStringCollection doc_E = new AutoCompleteStringCollection();
                foreach (var item in getValueDoc)
                {
                    doc_E.Add(item.DocNo);
                }
                textBox2.AutoCompleteCustomSource = doc_E; // ถึง
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lb_msg.Text = "โปรแกรมกำลังทำงาน . . ";
            lb_msg.Visible = true;
            this.Refresh();
            this.BillingData();
        }

        public static List<string> GetAllDocId(string Code_S, string Code_E)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                List<string> fkProHd = new List<string>();
                try
                {
                    var allDoc = db.StoreFrontValueDoc.Where(w => w.Enable == true).OrderBy(w => w.DocNo);

                    if (Code_S == "")
                    {
                        fkProHd.AddRange(allDoc.Select(w => w.DocNo).Distinct().ToList<string>());
                        return fkProHd;
                    }
                    else if (Code_S == Code_E)
                    {
                        fkProHd.Add(Code_S);
                        return fkProHd;
                    }
                    bool found = false;
                    foreach (var item in allDoc)
                    {
                        if (found == true && item.DocNo != Code_E)
                        {
                            fkProHd.Add(item.DocNo);
                        }
                        else if (item.DocNo == Code_S)
                        {
                            found = true;
                            fkProHd.Add(item.DocNo);
                        }
                        else if (item.DocNo == Code_E)
                        {
                            fkProHd.Add(item.DocNo);
                            break;
                        }
                    }
                    return fkProHd.Distinct().ToList<string>(); ;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return fkProHd;
                }
            }
        }
        class ObjItemList
        {
            public string Number { get; set; }
            public int SKU { get; set; }
            public string Code { get; set; }
            public string ItemName { get; set; }
            public string CheckStock { get; set; }
            public string TotalStock { get; set; }
            public string DiffQty { get; set; }
            public string CostPerUnit { get; set; }
            public string TotalCost { get; set; }
            public string SellPerUnit { get; set; }
            public string TotalSell { get; set; }
            public decimal decimal_TotalCost { get; set; }
            public decimal decimal_TotalSell { get; set; }
            public decimal decimal_TotalResult { get; set; }
            public decimal decimal_TotalCheck { get; set; }
            public string DiffPrice { get; set; }
            public string Description { get; set; }
        }
        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                using (_timeoutTimer)
                    MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }
        class GetCountStock
        {
            public int SKU { get; set; }
            public string Code { get; set; }
            public string ThaiName { get; set; }
            public decimal CheckQtyPiece { get; set; }
            public decimal CostAvg { get; set; }
            public decimal SellAvg { get; set; }
        }
        class GetResultFromDate
        {
            public int Id { get; set; }
            public string createStr { get; set; }
            public DateTime CreateDate { get; set; }
            public decimal ActionQty { get; set; }
            public int FKTransactionType { get; set; }
            public decimal PackSize { get; set; }
            public int FKProductDetails { get; set; }
            public int FKProduct { get; set; }
        }
        public void BillingData()
        {
            //try
            //{
            Cursor.Current = Cursors.WaitCursor;
            List<ObjItemList> objs = new List<ObjItemList>();
            DateTime dateS = dateTimePickerStart.Value;
            DateTime dateE = dateTimePickerEnd.Value;
            DateTime dateRun = DateTime.Now;
            using (SSLsEntities db = new SSLsEntities())
            {
                string queryString = @"SELECT        dbo.View_SumSKUCheck_01_11_17.SKU, wms.Products.Code, wms.Products.ThaiName, dbo.View_SumSKUCheck_01_11_17.CheckQtyPiece, 
                         dbo.View_SumSKUCheck_01_11_17.CostValue / dbo.View_SumSKUCheck_01_11_17.CheckQtyPiece AS CostAvg, 
                         dbo.View_SumSKUCheck_01_11_17.SellValue / dbo.View_SumSKUCheck_01_11_17.CheckQtyPiece AS SellAvg
                            FROM            dbo.View_SumSKUCheck_01_11_17 LEFT OUTER JOIN
                         wms.Products ON dbo.View_SumSKUCheck_01_11_17.SKU = wms.Products.Id";
                var getdata = db.Database.ExecuteEntities<GetCountStock>(queryString);
                var checkFront = db.View_CheckFront_01_11_17.ToList();

                string docS = textBox1.Text;
                string docE = textBox2.Text;

                var vDon = db.V_DifStockCount02.ToList();
                ObjItemList obj;
                int countListpg = getdata.Count();
                Console.WriteLine("count = " + countListpg);

                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = getdata.Count();
                int i = 1;
                var dateString = dateS.ToString("yyyyMMdd");
                var aa = Library.GetQueryยอดยกมา(dateString, "0", "999999999999999");
                List<TransactionType> transType = new List<TransactionType>();
                transType = db.TransactionType.Where(w => w.Enable == true).ToList();
                List<int> fktransP = new List<int>();
                fktransP = transType.Where(w => w.IsPlus == true).Select(w => w.Id).ToList<int>();
                List<int> fktransM = new List<int>();
                fktransP = transType.Where(w => w.IsPlus == false).Select(w => w.Id).ToList<int>();
                foreach (var item in getdata)
                {
                    progressBar1.Value = progressBar1.Value + 1;
                 
                    var prodGet = Singleton.SingletonProduct.Instance().Products.SingleOrDefault(w => w.Id == item.SKU);
                    obj = new ObjItemList();
                    var getResultDate = aa.SingleOrDefault(w => w.FKProduct == item.SKU);
                    obj.Number = i + "";
                    obj.ItemName = item.ThaiName;
                    obj.SKU = item.SKU;
                    obj.Code = item.Code;
                    obj.CheckStock = Library.ConvertDecimalToStringForm(item.CheckQtyPiece);
                    var don = vDon.SingleOrDefault(w => w.FKProduct == item.SKU);
                    obj.TotalStock = Library.ConvertDecimalToStringForm(decimal.Parse(getResultDate.qty));
                    obj.DiffQty = Library.ConvertDecimalToStringForm(item.CheckQtyPiece - decimal.Parse(obj.TotalStock));
                    obj.CostPerUnit = Library.ConvertDecimalToStringForm(item.CostAvg);
                    obj.TotalCost = Library.ConvertDecimalToStringForm(item.CostAvg * (item.CheckQtyPiece - decimal.Parse(obj.TotalStock)));
                    obj.SellPerUnit = Library.ConvertDecimalToStringForm(item.SellAvg);
                    obj.TotalSell = Library.ConvertDecimalToStringForm(decimal.Parse(obj.SellPerUnit) * decimal.Parse(obj.DiffQty));
                    obj.DiffPrice = Library.ConvertDecimalToStringForm(decimal.Parse(obj.SellPerUnit) - decimal.Parse(obj.CostPerUnit)); 
                    obj.decimal_TotalCost = item.CostAvg * decimal.Parse(obj.DiffQty);
                    obj.decimal_TotalSell = decimal.Parse(obj.TotalSell);
                    obj.decimal_TotalCheck += decimal.Parse(obj.CheckStock);
                    obj.decimal_TotalResult += decimal.Parse(obj.TotalStock);
                    var checkDoc = checkFront.Where(w => w.FKProduct == item.SKU).ToList();
                    string docStr = "";
                    foreach (var doc in checkDoc)
                    {
                        docStr = doc.DocNo + " (" + doc.Number + ") " + docStr;
                    }
                    obj.Description = docStr;
                    objs.Add(obj);
                    i++;

                    // ********** ปรับปรุง ***************************************************
                //    StoreFrontStockDetails detail = new StoreFrontStockDetails();
                //    detail.DocNo = "-";
                //    detail.DocDtlNumber = 0;
                //    detail.Description = "ปรับปรุงหน้าร้าน28/04/2018";
                //    detail.CreateDate = DateTime.Parse("2018-04-27 23:58:00");
                //    detail.CreateBy = "admin";
                //    detail.UpdateDate = DateTime.Now;
                //    detail.UpdateBy = "admin";
                //    detail.Enable = true;
                //    var fkStore = db.StoreFrontStock.FirstOrDefault(w => w.Enable == true && w.FKProduct == item.SKU);
                //    if (fkStore == null)
                //    {
                //        using (SSLsEntities db1 = new SSLsEntities())
                //        {

                //            // add to 
                //            StoreFrontStock storefrontHD = new StoreFrontStock();
                //            storefrontHD = new StoreFrontStock();
                //            storefrontHD.Description = "add new";
                //            storefrontHD.CreateDate = DateTime.Now;
                //            storefrontHD.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                //            storefrontHD.UpdateDate = DateTime.Now;
                //            storefrontHD.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                //            storefrontHD.Enable = true;
                //            storefrontHD.CurrentQty = 0;
                //            storefrontHD.FKProduct = item.SKU;
                //            db1.StoreFrontStock.Add(storefrontHD);
                //            db1.SaveChanges();
                //            fkStore = db.StoreFrontStock.FirstOrDefault(w => w.Enable == true && w.FKProduct == item.SKU);
                //        }

                //    }
                //    if (decimal.Parse(obj.DiffQty) > 0) // ผลต่างเป็น + ต้องทำปรับเข้า =25 
                //    {
                //        detail.ActionQty = decimal.Parse(obj.DiffQty);
                //        detail.FKTransactionType = 25;
                //    }
                //    else if (decimal.Parse(obj.DiffQty) < 0)// ผลต่างเป็น - ต้องทำจ่ายออก  =24
                //    {
                //        detail.ActionQty = decimal.Parse(obj.DiffQty) * -1;
                //        detail.FKTransactionType = 24;
                //    }
                //    else // 0 ไม่ทำไร
                //    {
                //        continue;
                //    }
                //    detail.FKStoreFrontStock = fkStore.Id;
                //    detail.Barcode = obj.Code;
                //    detail.Name = "-";
                //    detail.FKProductDetails = prodGet.ProductDetails.OrderBy(w => w.PackSize).FirstOrDefault(w => w.Enable == true).Id;
                //    detail.ResultQty = 0;
                //    detail.PackSize = prodGet.ProductDetails.OrderBy(w => w.PackSize).FirstOrDefault(w => w.Enable == true).PackSize;
                //    detail.CostOnlyPerUnit = decimal.Parse(obj.CostPerUnit);
                //    detail.SellPricePerUnit = decimal.Parse(obj.SellPerUnit);
                //    db.StoreFrontStockDetails.Add(detail);
                //    Console.WriteLine("fkProHD " + item.SKU + " " + detail.ActionQty + " type=" + detail.FKTransactionType);
                }
                //db.SaveChanges();
                // ********** ปรับปรุง
            }
            dataGridView1.DataSource = objs;
            dataGridView1.Refresh();
            MessageBox.Show("ใช้เวลาโปรเซส " + Library.ConvertDateToThaiDate(dateRun, true) + " ถึง " + Library.ConvertDateToThaiDate(DateTime.Now, true));
            var Date_S = Library.ConvertDateToThaiDate(dateS);
            var Date_E = Library.ConvertDateToThaiDate(dateE);
            var countList = objs.Where(w => w.Number != null).Count();
            var sumTotalCost = Library.ConvertDecimalToStringForm(objs.Sum(s => s.decimal_TotalCost));
            var sumTotalSell = Library.ConvertDecimalToStringForm(objs.Sum(s => s.decimal_TotalSell));
            var sumTotalResult = Library.ConvertDecimalToStringForm(objs.Sum(s => s.decimal_TotalResult));
            var sumTotalCheck = Library.ConvertDecimalToStringForm(objs.Sum(s => s.decimal_TotalCheck));
            var sumTotalDiff = Library.ConvertDecimalToStringForm(objs.Sum(s => decimal.Parse(s.DiffQty)));


            var getReport = objs.Select(w => new
            {
                Number = w.Number,
                ItemName = w.ItemName,
                Code = w.Code,
                CheckStock = w.CheckStock,
                TotalStock = w.TotalStock,
                DiffQty = w.DiffQty,
                CostPerUnit = w.CostPerUnit,
                TotalCost = w.TotalCost,
                Date_S = Date_S,
                Date_E = Date_E,
                Doc_S = textBox1.Text,
                Doc_E = textBox2.Text,
                countList = countList,
                sumTotalCost = sumTotalCost,
                SellPerUnit = w.SellPerUnit,
                TotalSell = w.TotalSell,
                sumTotalSell = sumTotalSell,
                SumTotalCheck = sumTotalCheck,
                SumTotalResult = sumTotalResult,
                SumTotalDiff = sumTotalDiff,
                DiffPrice = w.DiffPrice
            });

            var dt = Library.ConvertToDataTable(getReport.ToList());
            frmMainReport mr = new frmMainReport(this, dt);
            mr.Show();

            lb_msg.Text = "ระบบประมวลผลสำเร็จ";
            this.Refresh();

            Cursor.Current = Cursors.Default;
            //}
            //catch (Exception ex)
            //{
            //    Cursor.Current = Cursors.Default;
            //    MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    throw;
            //}
        }
        /// <summary>
        /// ///////////// Back Up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //public void BillingData()
        //{
        //    try
        //    {
        //        Cursor.Current = Cursors.WaitCursor;
        //        List<ObjItemList> objs = new List<ObjItemList>();
        //        DateTime dateS = dateTimePickerStart.Value;
        //        DateTime dateE = dateTimePickerEnd.Value;
        //        string dateRun = DateTime.Now.ToString();
        //        using (SSLsEntities db = new SSLsEntities())
        //        {
        //            string docS = textBox1.Text;
        //            string docE = textBox2.Text;

        //            var docCheck = db.StoreFrontValueDoc.Where(w => w.Enable == true &&
        //            DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
        //            DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE))
        //            .ToList();
        //            ObjItemList obj;

        //            int countListpg = docCheck.Count();
        //            //List<int> fkProdStack = new List<int>();
        //            //progressBar2.Value = 0;
        //            //progressBar2.Minimum = 0;
        //            //progressBar2.Maximum = countListpg;
        //            Console.WriteLine("count = " + countListpg);
        //            List<string> docStack = docCheck.Select(w => w.DocNo).ToList();
        //            List<int> getId = db.StoreFrontValueSet.Where(w => w.Enable == true && docStack.Contains(w.FKDocNo)).Select(w => w.ProductDetails.FKProduct).Distinct().ToList<int>();
        //            //foreach (var item in docCheck)
        //            //{
        //            //    progressBar2.Value = progressBar2.Value + 1;
        //            //    var getId = item.StoreFrontValueSet.Where(w => w.Enable == true).Select(w => w.ProductDetails.FKProduct).Distinct().ToList<int>();
        //            //    fkProdStack.AddRange(getId);
        //            //    Console.WriteLine("value = " + progressBar2.Value);
        //            //}
        //            //AutoClosingMessageBox.Show("กรุณารอสักครู่", "คำเตือน", 1000);
        //            List<int> fkProdStack1 = new List<int>();
        //            fkProdStack1.AddRange(getId);
        //            progressBar1.Value = 0;
        //            progressBar1.Minimum = 0;
        //            progressBar1.Maximum = fkProdStack1.Count();
        //            int i = 1;
        //            var valueSet = db.StoreFrontValueSet.Where(w => w.Enable == true && docStack.Contains(w.FKDocNo)).ToList();
        //            foreach (var fkProHD in fkProdStack1)
        //            {
        //                progressBar1.Value = progressBar1.Value + 1;
        //                //progressBar1.Refresh();
        //                decimal sumProductCheck = 0;
        //                var getDtlId = Library.GetProdDtlInHD(fkProHD);
        //                decimal getDoc = valueSet.Where(w => docStack.Contains(w.FKDocNo) && getDtlId.Contains(w.FKProductDetails) && w.Enable == true).ToList().Sum(w => w.QtyUnit2 * w.Packsize);
        //                sumProductCheck += getDoc;
        //                //foreach (var doc in docCheck)
        //                //{
        //                //    // หายอดนับได้
        //                //    var getDtlId = Library.GetProdDtlInHD(fkProHD);
        //                //    decimal getDoc = doc.StoreFrontValueSet.Where(w => getDtlId.Contains(w.FKProductDetails) && w.Enable == true).ToList().Sum(w=>w.QtyUnit2 * w.Packsize);

        //                //    sumProductCheck += getDoc;
        //                //}
        //                var prodGet = Singleton.SingletonProduct.Instance().Products.SingleOrDefault(w => w.Id == fkProHD);
        //                obj = new ObjItemList();
        //                obj.Number = i + "";
        //                obj.ItemName = prodGet.ThaiName;
        //                obj.Code = prodGet.ProductDetails.OrderBy(w => w.PackSize).FirstOrDefault(w => w.Enable == true).Code;
        //                obj.CheckStock = Library.ConvertDecimalToStringForm(sumProductCheck);
        //                obj.TotalStock = Library.ConvertDecimalToStringForm(Library.GetResult(fkProHD, dateS.AddDays(1)));
        //                obj.DiffQty = Library.ConvertDecimalToStringForm(decimal.Parse(obj.CheckStock) - decimal.Parse(obj.TotalStock));
        //                decimal costAvg = Library.GetAverage(fkProHD);
        //                obj.CostPerUnit = Library.ConvertDecimalToStringForm(costAvg);
        //                obj.TotalCost = Library.ConvertDecimalToStringForm(costAvg * decimal.Parse(obj.DiffQty));
        //                obj.SellPerUnit = Library.ConvertDecimalToStringForm(prodGet.ProductDetails.Where(w => w.Enable == true).OrderBy(w => w.PackSize).FirstOrDefault().SellPrice);
        //                obj.TotalSell = Library.ConvertDecimalToStringForm(decimal.Parse(obj.SellPerUnit) * decimal.Parse(obj.DiffQty));
        //                obj.decimal_TotalCost = costAvg * decimal.Parse(obj.DiffQty);
        //                obj.decimal_TotalSell = decimal.Parse(obj.TotalSell);

        //                obj.decimal_TotalCheck += decimal.Parse(obj.CheckStock);
        //                obj.decimal_TotalResult += decimal.Parse(obj.TotalStock);

        //                objs.Add(obj);
        //                i++;
        //                Console.WriteLine("fkProHD " + fkProHD);
        //            }
        //        }
        //        MessageBox.Show(DateTime.Now.ToString() + " " + dateRun);
        //        var Date_S = Library.ConvertDateToThaiDate(dateS);
        //        var Date_E = Library.ConvertDateToThaiDate(dateE);
        //        var countList = objs.Where(w => w.Number != null).Count();
        //        var sumTotalCost = Library.ConvertDecimalToStringForm(objs.Sum(s => s.decimal_TotalCost));
        //        var sumTotalSell = Library.ConvertDecimalToStringForm(objs.Sum(s => s.decimal_TotalSell));
        //        var sumTotalResult = Library.ConvertDecimalToStringForm(objs.Sum(s => s.decimal_TotalResult));
        //        var sumTotalCheck = Library.ConvertDecimalToStringForm(objs.Sum(s => s.decimal_TotalCheck));
        //        var getReport = objs.Select(w => new
        //        {
        //            Number = w.Number,
        //            ItemName = w.ItemName,
        //            Code = w.Code,
        //            CheckStock = w.CheckStock,
        //            TotalStock = w.TotalStock,
        //            DiffQty = w.DiffQty,
        //            CostPerUnit = w.CostPerUnit,
        //            TotalCost = w.TotalCost,
        //            Date_S = Date_S,
        //            Date_E = Date_E,
        //            Doc_S = textBox1.Text,
        //            Doc_E = textBox2.Text,
        //            countList = countList,
        //            sumTotalCost = sumTotalCost,
        //            SellPerUnit = w.SellPerUnit,
        //            TotalSell = w.TotalSell,
        //            sumTotalSell = sumTotalSell,
        //            SumTotalCheck = sumTotalCheck,
        //            SumTotalResult = sumTotalResult
        //        });

        //        var dt = Library.ConvertToDataTable(getReport.ToList());
        //        frmMainReport mr = new frmMainReport(this, dt);
        //        mr.Show();

        //        lb_msg.Text = "ระบบประมวลผลสำเร็จ";
        //        this.Refresh();

        //        Cursor.Current = Cursors.Default;
        //    }
        //    catch (Exception ex)
        //    {
        //        Cursor.Current = Cursors.Default;
        //        MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        throw;
        //    }
        //}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox1.Text;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานผลต่าง 28/04/2561", "");
        }
    }
}
