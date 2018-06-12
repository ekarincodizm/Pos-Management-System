using Pos_Management_System.Model;
using Pos_Management_System.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class ProductRawInOut : Form
    {
        public ProductRawInOut()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            lb_msg.Text = "โปรแกรมกำลังทำงาน . . ";
            lb_msg.Visible = true;
            this.Refresh();
            if (radioButton1.Checked == true)
            {
                bbildingDatauilding();
            }
            else
            {
                BindingInOutValue();
            }

        }

        public class C_ProductRawInOut
        {
            public string CreateDate { get; set; }
            public string DocNo { get; set; }
            public string DocDtlNumber { get; set; }
            public string DocDescription { get; set; }
            public string ActionQty_P { get; set; }
            public string ActionQty_D { get; set; }
            public string BalanceQty { get; set; }
            public string ReferDoc { get; set; }
            public string DocReferDtlNumber { get; set; }
            public string ProductNo { get; set; }
            public string ProductName { get; set; }
            public string ProductUnit { get; set; }
            public string PackSize { get; set; }
            public int FKProductDetails { get; set; }
        }

        public void bbildingDatauilding()
        {
            //try
            //{
            Cursor.Current = Cursors.WaitCursor;
            var time_S = dateTimePicker_S.Value;
            var time_E = dateTimePicker_E.Value;
            string dateStart = time_S.ToString("yyyyMMdd");
            List<C_ProductRawInOut> ls = new List<C_ProductRawInOut>();
            List<C_ProductRawInOut> lsGetResult = new List<C_ProductRawInOut>();
            using (SSLsEntities db = new SSLsEntities())
            {
                //int fkPro = db.ProductDetails.Where(w => w.Code == _Code && w.Enable == true)
                //    .ToList().Select(sl => sl.FKProduct).Distinct().FirstOrDefault();
                List<int> fkPro = new List<int>();
                if (checkBoxProduct.Checked == true)
                {
                    // ถ้าเลือกทั้งหมด
                    fkPro = Library.GetAllProDtlId("", "");
                }
                else
                {
                    fkPro = Library.GetAllProDtlId(txt_p_S.Text.Trim(), txt_p_E.Text.Trim());
                }
                //fkPro.Add(41550);
                //fkPro.Add(41557);
                //string fkProductConcat = string.Join(",", fkPro);
                var getOnView = db.View_GetFrontStockByProductDate.Where(w => DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(time_S) &&
                    DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(time_E)).OrderBy(w => w.CreateDate).ToList();
                var getForProduct = getOnView.Where(w => fkPro.Contains(w.FKProduct)).OrderBy(w => w.FKProduct).ToList();
                //List<C_ProductRawInOut> stackDetails = new List<C_ProductRawInOut>();
                /// อ้างอิง วิว View_GetFrontStockByProductDate สามารถเชคได้
                //                string query = @"SELECT TOP (100) PERCENT pos.StoreFrontStock.FKProduct, pos.StoreFrontStockDetails.CreateDate, CONVERT(varchar, pos.StoreFrontStockDetails.CreateDate, 112) AS DateCreate, pos.StoreFrontStockDetails.Barcode,  
                //                         pos.StoreFrontStockDetails.FKProductDetails, pos.StoreFrontStockDetails.ActionQty, pos.StoreFrontStockDetails.PackSize, pos.StoreFrontStockDetails.DocNo, pos.StoreFrontStockDetails.DocDtlNumber,  
                //                         pos.StoreFrontStockDetails.DocRefer, pos.StoreFrontStockDetails.DocReferDtlNumber, wms.ProductDetails.Code, wms.ProductUnit.Name AS Unit, pos.TransactionType.IsPlus, pos.TransactionType.Name AS TransName 
                //FROM            pos.StoreFrontStockDetails INNER JOIN 
                //                         pos.StoreFrontStock ON pos.StoreFrontStockDetails.FKStoreFrontStock = pos.StoreFrontStock.Id INNER JOIN 
                //                         wms.ProductDetails ON pos.StoreFrontStockDetails.FKProductDetails = wms.ProductDetails.Id INNER JOIN 
                //                         wms.ProductUnit ON wms.ProductDetails.FKUnit = wms.ProductUnit.Id AND wms.ProductDetails.FKUnit = wms.ProductUnit.Id INNER JOIN 
                //                         pos.TransactionType ON pos.StoreFrontStockDetails.FKTransactionType = pos.TransactionType.Id AND pos.StoreFrontStockDetails.FKTransactionType = pos.TransactionType.Id 
                //WHERE        (pos.StoreFrontStockDetails.Enable = 1) AND (pos.StoreFrontStock.Enable = 1) AND (CONVERT(varchar, pos.StoreFrontStockDetails.CreateDate, 112) BETWEEN '@dateStart' AND '20171201') AND 
                //                         (pos.StoreFrontStock.FKProduct IN (41550, 41557))";

                //                SqlConnection sqlConn = new SqlConnection(db.Database.Connection.ConnectionString);
                //                sqlConn.Open();
                //                SqlCommand cmd = new SqlCommand(query, sqlConn);
                //                // pass param
                //                cmd.Parameters.AddWithValue("@dateStart", "dateStart");
                //cmd.Parameters.Add("@DateS", SqlDbType.VarChar);
                //cmd.Parameters["@DateS"].Value = time_S.ToString("yyyyMMdd");

                //cmd.Parameters.Add("@DateE", SqlDbType.VarChar);
                //cmd.Parameters["@DateE"].Value = time_E.ToString("yyyyMMdd");

                //cmd.Parameters.Add("@fkProductConcat", SqlDbType.Int);
                //cmd.Parameters["@fkProductConcat"].Value = fkPro;
                //Console.WriteLine(fkProductConcat);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable datatable = new DataTable();
                //da.Fill(datatable);
                //sqlConn.Close();
                //List<Map_GetFrontStockByProductDate> models = new List<Map_GetFrontStockByProductDate>();
                //foreach (DataRow row in datatable.Rows)
                //{
                //    var values = row.ItemArray;
                //    var map = new Map_GetFrontStockByProductDate();
                //    //string a = values[10].ToString();
                //    map.FKProduct = int.Parse(values[0].ToString());
                //    map.CreateDate = DateTime.Parse(values[1].ToString());
                //    map.DateCreate = values[2].ToString();
                //    map.Barcode = values[3].ToString();
                //    map.FKProductDetails = int.Parse(values[4].ToString());
                //    map.ActionQty = decimal.Parse(values[5].ToString());
                //    map.PackSize = decimal.Parse(values[6].ToString());
                //    map.DocNo = values[7].ToString();
                //    map.DocDtlNumber = values[8] == null ? 0 : int.Parse(values[8].ToString());
                //    map.DocRefer = values[9].ToString();
                //    map.DocReferDtlNumber = values[10].ToString() == "" ? 0 : int.Parse(values[10].ToString());
                //    map.Code = values[11].ToString();
                //    map.Unit = values[12].ToString();
                //    map.IsPlus = bool.Parse(values[13].ToString());
                //    map.TransName = values[14].ToString();
                //    models.Add(map);
                //}

                int countListpg = getForProduct.Count();
                progressBar1.Minimum = 0;
                progressBar1.Maximum = fkPro.Count();
                int i = 0;
                //var aa = db.fn_GetResultDate(dateStart, "0", "999999999999999").ToList();
                var aa = Library.GetQueryยอดยกมา(dateStart, "0", "999999999999999");
                DateTime startPro = DateTime.Now;
                //var sortNew = getForProduct.OrderBy(w => w.DateCreate).ToList();
                foreach (var item in fkPro)
                {
                    i++;
                    List<View_GetFrontStockByProductDate> dataGet = getForProduct.Where(w => w.FKProduct == item).OrderBy(w => w.CreateDate).ToList();
                    if (dataGet.Count == 0)
                    {
                        progressBar1.Value = i;
                        progressBar1.Refresh();
                        Console.WriteLine("******* " + fkPro.Count + " " + item + " " + i + " " + getForProduct.Count());
                        continue;
                    }
                    //decimal resultForProHD = Library.GetResult(item, time_S);
                    decimal resultForProHD = decimal.Parse(aa.FirstOrDefault(w => w.FKProduct == item).qty);
                    lsGetResult.Add(new C_ProductRawInOut() // ยอดยกมา
                    {
                        CreateDate = "",
                        ActionQty_P = "",
                        ActionQty_D = "",
                        BalanceQty = Library.ConvertDecimalToStringForm(resultForProHD),
                        DocDescription = "",
                        DocDtlNumber = "",
                        DocNo = "ยอดยกมา",
                        DocReferDtlNumber = "",
                        PackSize = "",
                        ProductName = "",
                        ProductNo = "",
                        ProductUnit = "",
                        ReferDoc = ""
                    });

                    decimal sumPlus = 0;
                    decimal sumMinus = 0;

                    foreach (var trans in dataGet) // transactions
                    {
                        decimal plus = 0;
                        decimal minus = 0;

                        if (trans.IsPlus == true)
                        {
                            plus = trans.ActionQty;
                            resultForProHD = resultForProHD + plus;
                        }
                        else
                        {
                            minus = trans.ActionQty;
                            resultForProHD = resultForProHD - minus;
                        }
                        sumPlus += plus;
                        sumMinus += minus;
                        lsGetResult.Add(new C_ProductRawInOut() // transactions
                        {
                            CreateDate = Library.ConvertDateToThaiDate(trans.CreateDate, true),
                            ActionQty_P = Library.ConvertDecimalToStringForm(plus),
                            ActionQty_D = Library.ConvertDecimalToStringForm(minus),
                            BalanceQty = Library.ConvertDecimalToStringForm(resultForProHD),
                            DocDescription = "",
                            DocDtlNumber = trans.DocDtlNumber + "",
                            DocNo = trans.DocNo,
                            DocReferDtlNumber = trans.DocReferDtlNumber + "",
                            PackSize = trans.PackSize + "",
                            ProductNo = trans.Code,
                            ProductName = trans.ThaiName,
                            ProductUnit = trans.Unit,
                            ReferDoc = trans.DocRefer
                        });
                        getForProduct.Remove(trans);
                    }
                    lsGetResult.Add(new C_ProductRawInOut() // ยอดยกไป
                    {
                        CreateDate = "",
                        ActionQty_P = Library.ConvertDecimalToStringForm(sumPlus),
                        ActionQty_D = Library.ConvertDecimalToStringForm(sumMinus),
                        BalanceQty = Library.ConvertDecimalToStringForm(resultForProHD),
                        DocDescription = "",
                        DocDtlNumber = "",
                        DocNo = "สรุปรวม",
                        DocReferDtlNumber = "",
                        PackSize = "",
                        ProductName = "",
                        ProductNo = "",
                        ProductUnit = "",
                        ReferDoc = ""
                    });

                    progressBar1.Value = i;
                    progressBar1.Refresh();
                    Console.WriteLine("******* " + fkPro.Count + " " + item + " " + i + " " + getForProduct.Count());
                }
                Console.WriteLine(startPro + " " + DateTime.Now);
                //foreach (var item in fkPro)
                //{
                //    i++;
                //    List<StoreFrontStockDetails> details = db.StoreFrontStockDetails
                //    .Where(w => w.StoreFrontStock.FKProduct == item && w.StoreFrontStock.Enable == true && w.Enable == true &&
                //  DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(time_S) &&
                //    DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(time_E)
                //  ).OrderBy(w => w.CreateDate).ToList();

                //    if (details.Count == 0)
                //    {
                //        progressBar1.Value = i;
                //        progressBar1.Refresh();
                //        continue;
                //    }
                //    decimal resultForProHD = Library.GetResult(item, time_S);
                //    lsGetResult.Add(new C_ProductRawInOut() // ยอดยกมา
                //    {
                //        CreateDate = "",
                //        ActionQty_P = "",
                //        ActionQty_D = "",
                //        BalanceQty = Library.ConvertDecimalToStringForm(resultForProHD),
                //        DocDescription = "",
                //        DocDtlNumber = "",
                //        DocNo = "ยอดยกมา",
                //        DocReferDtlNumber = "",
                //        PackSize = "",
                //        ProductName = "",
                //        ProductNo = "",
                //        ProductUnit = "",
                //        ReferDoc = ""
                //    });

                //    decimal sumPlus = 0;
                //    decimal sumMinus = 0;

                //    foreach (var trans in details) // transactions
                //    {
                //        decimal plus = 0;
                //        decimal minus = 0;

                //        if (trans.TransactionType.IsPlus == true)
                //        {
                //            plus = trans.ActionQty;
                //            resultForProHD = resultForProHD + plus;
                //        }
                //        else
                //        {
                //            minus = trans.ActionQty;
                //            resultForProHD = resultForProHD - minus;
                //        }
                //        sumPlus += plus;
                //        sumMinus += minus;
                //        lsGetResult.Add(new C_ProductRawInOut() // transactions
                //        {
                //            CreateDate = Library.ConvertDateToThaiDate(trans.CreateDate, true),
                //            ActionQty_P = Library.ConvertDecimalToStringForm(plus),
                //            ActionQty_D = Library.ConvertDecimalToStringForm(minus),
                //            BalanceQty = Library.ConvertDecimalToStringForm(resultForProHD),
                //            DocDescription = "",
                //            DocDtlNumber = trans.DocDtlNumber + "",
                //            DocNo = trans.DocNo,
                //            DocReferDtlNumber = trans.DocReferDtlNumber + "",
                //            PackSize = trans.PackSize + "",
                //            ProductNo = trans.ProductDetails.Code,
                //            ProductName = trans.ProductDetails.Products.ThaiName,
                //            ProductUnit = trans.ProductDetails.ProductUnit.Name,
                //            ReferDoc = trans.DocRefer
                //        });

                //    }
                //    lsGetResult.Add(new C_ProductRawInOut() // ยอดยกไป
                //    {
                //        CreateDate = "",
                //        ActionQty_P = Library.ConvertDecimalToStringForm(sumPlus),
                //        ActionQty_D = Library.ConvertDecimalToStringForm(sumMinus),
                //        BalanceQty = Library.ConvertDecimalToStringForm(resultForProHD),
                //        DocDescription = "",
                //        DocDtlNumber = "",
                //        DocNo = "สรุปรวม",
                //        DocReferDtlNumber = "",
                //        PackSize = "",
                //        ProductName = "",
                //        ProductNo = "",
                //        ProductUnit = "",
                //        ReferDoc = ""
                //    });

                //    progressBar1.Value = i;
                //    progressBar1.Refresh();
                //}

                var countList = lsGetResult.Where(w => w.DocNo != "ยอดยกมา" && w.DocNo != "สรุปรวม").Count();

                dataGridView1.DataSource = lsGetResult;
                dataGridView1.Refresh();

                var dt = Library.ConvertToDataTable(lsGetResult.Select(s =>
                new
                {
                    CreateDate = s.CreateDate,
                    DocNo = s.DocNo == "ยอดยกมา" || s.DocNo == "สรุปรวม" ? s.DocNo : s.DocNo + "- " + s.DocDtlNumber,
                    DocName = s.DocNo == "ยอดยกมา" || s.DocNo == "สรุปรวม" ? "" : s.ProductNo + " " + s.ProductName + " " + s.ProductUnit + " x " + s.PackSize,
                    ActionQty_P = s.ActionQty_P,
                    ActionQty_D = s.ActionQty_D,
                    BalanceQty = s.BalanceQty,
                    ReferDoc = s.ReferDoc == "" || s.ReferDoc == null ? "" : s.ReferDoc + "- " + s.DocReferDtlNumber,
                    countList = countList,
                    Date_S = Library.ConvertDateToThaiDate(dateTimePicker_S.Value),
                    Date_E = Library.ConvertDateToThaiDate(dateTimePicker_E.Value),
                    Product_S = txt_p_S.Text == "" || txt_p_S.Text == null ? "ทั้งหมด" : txt_p_S.Text,
                    Product_E = txt_p_E.Text == "" || txt_p_E.Text == null ? "ทั้งหมด" : txt_p_E.Text
                }).ToList());
                lb_msg.Text = "ระบบประมวลผลสำเร็จ";
                this.Refresh();
                Cursor.Current = Cursors.Default;
                frmMainReport mr = new frmMainReport(this, dt, false);
                mr.Show();
            }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
        public class InOutValue
        {
            public string CreateDate { get; set; }
            public string DocNo { get; set; }
            public string DocDtlNumber { get; set; }

            public string ProductName { get; set; }
            public string Pz { get; set; }

            public string InQty { get; set; }
            public string InCost { get; set; }
            public string InValue { get; set; }

            public string OutQty { get; set; }
            public string OutCost { get; set; }
            public string OutValue { get; set; }

            public string BalQty { get; set; }
            public string BalCost { get; set; }
            public string BalValue { get; set; }

            public string DocDesc { get; set; }

            public int FKTrans { get; set; }
        }
        /// <summary>
        /// แบบมีมูลค่า
        /// </summary>
        public void BindingInOutValue()
        {
            //try
            //{
            Cursor.Current = Cursors.WaitCursor;
            var time_S = dateTimePicker_S.Value;
            var time_E = dateTimePicker_E.Value;
            string dateStart = time_S.ToString("yyyyMMdd");
            List<InOutValue> lsGetResult = new List<InOutValue>();
            using (SSLsEntities db = new SSLsEntities())
            {
                List<int> fkPro = new List<int>();
                if (checkBoxProduct.Checked == true)
                {
                    // ถ้าเลือกทั้งหมด
                    fkPro = Library.GetAllProDtlId("", "");
                }
                else
                {
                    fkPro = Library.GetAllProDtlId(txt_p_S.Text.Trim(), txt_p_E.Text.Trim());
                }

                var getOnView = db.View_GetFrontStockByProductDate.Where(w => DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(time_S) &&
                    DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(time_E)).OrderBy(w => w.CreateDate).ToList();
                var getForProduct = getOnView.Where(w => fkPro.Contains(w.FKProduct)).OrderBy(w => w.FKProduct).ToList();

                List<C_ProductRawInOut> stackDetails = new List<C_ProductRawInOut>();
                int countListpg = fkPro.Count();
                progressBar1.Minimum = 0;
                progressBar1.Maximum = countListpg;
                int i = 0;
                DateTime startPro = DateTime.Now;
                var aa = Library.GetQueryยอดยกมา(dateStart, "0", "999999999999999");
                foreach (var item in fkPro)
                {
                    List<View_GetFrontStockByProductDate> dataGet = getForProduct.Where(w => w.FKProduct == item).OrderBy(w => w.CreateDate).ToList();
                    decimal movementAvg = 0;
                    decimal balance = 0;
                    i++;
                    if (dataGet.Count == 0)
                    {
                        progressBar1.Value = i;
                        progressBar1.Refresh();
                        Console.WriteLine("******* " + fkPro.Count + " " + item + " " + i + " " + getForProduct.Count());
                        continue;
                    }
                    //decimal resultForProHD = Library.GetResult(item, time_S);
                    decimal resultForProHD = decimal.Parse(aa.SingleOrDefault(w => w.FKProduct == item).qty);
                    movementAvg = Library.GetAverage(item);

                    lsGetResult.Add(new InOutValue() // ยอดยกมา
                    {
                        CreateDate = "",
                        DocNo = "ยอดยกมา",
                        DocDtlNumber = "",
                        ProductName = "",
                        Pz = "",
                        InQty = "",
                        InCost = "",
                        InValue = "",
                        OutQty = "",
                        OutCost = "",
                        OutValue = "",
                        BalQty = Library.ConvertDecimalToStringForm(resultForProHD),
                        BalCost = Library.ConvertDecimalToStringForm(movementAvg),
                        BalValue = Library.ConvertDecimalToStringForm(movementAvg * resultForProHD),
                        DocDesc = "ยกมาจาก " + Library.ConvertDateToThaiDate(time_S)
                    });
                    balance = movementAvg * resultForProHD;

                    decimal sumPlus = 0;
                    decimal sumMinus = 0;
                    decimal sumTotalInCost = 0;
                    decimal sumTotalOutCost = 0;
                    foreach (var trans in dataGet.OrderBy(w => w.Barcode).ToList()) // transactions
                    {
                        decimal plus = 0;
                        decimal minus = 0;
                        sumTotalInCost = 0;
                        sumTotalOutCost = 0;
                        if (trans.IsPlus == true)
                        {
                            plus = trans.ActionQty;
                            resultForProHD = resultForProHD + plus;
                            // bal value in 1 transation
                            //movementAvg = trans.CostOnlyPerUnit;
                            sumTotalInCost = movementAvg * plus;
                            balance = balance + sumTotalInCost;
                        }
                        else
                        {
                            minus = trans.ActionQty;
                            resultForProHD = resultForProHD - minus;
                            // bal value in 1 transation                       
                            sumTotalOutCost = movementAvg * minus; // มูลค่าจ่าย
                            balance = balance - sumTotalOutCost;
                        }
                        sumPlus += plus;
                        sumMinus += minus;
                        //balance = movementAvg * resultForProHD;
                        ///////// use movement average
                        lsGetResult.Add(new InOutValue()
                        {
                            CreateDate = Library.ConvertDateToThaiDate(trans.CreateDate, true),
                            DocNo = trans.DocNo,
                            DocDtlNumber = trans.DocDtlNumber + "",
                            ProductName = trans.Code + " " + trans.ThaiName + " " + trans.Unit + "x" + ((int)trans.PackSize),
                            Pz = ((int)trans.PackSize) + "",
                            InQty = Library.ConvertDecimalToStringForm(plus),
                            InCost = Library.ConvertDecimalToStringForm(movementAvg),
                            InValue = Library.ConvertDecimalToStringForm(plus * movementAvg),
                            OutQty = Library.ConvertDecimalToStringForm(minus),
                            OutCost = Library.ConvertDecimalToStringForm(movementAvg),
                            OutValue = Library.ConvertDecimalToStringForm(minus * movementAvg),
                            BalQty = Library.ConvertDecimalToStringForm(resultForProHD),
                            BalCost = Library.ConvertDecimalToStringForm(movementAvg),
                            BalValue = Library.ConvertDecimalToStringForm(balance),
                            DocDesc = trans.TransName,
                            FKTrans = trans.FKTransactionType
                        });
                        /////////////////// old logic
                        //lsGetResult.Add(new InOutValue()
                        //{
                        //    CreateDate = Library.ConvertDateToThaiDate(trans.CreateDate, true),
                        //    DocNo = trans.DocNo,
                        //    DocDtlNumber = trans.DocDtlNumber + "",
                        //    ProductName = trans.ProductDetails.Code + " " + trans.ProductDetails.Products.ThaiName + " " + trans.ProductDetails.ProductUnit.Name + "x" + ((int)trans.PackSize),
                        //    Pz = ((int)trans.PackSize) + "",
                        //    InQty = Library.ConvertDecimalToStringForm(plus),
                        //    InCost = Library.ConvertDecimalToStringForm(trans.CostOnlyPerUnit),
                        //    InValue = Library.ConvertDecimalToStringForm((trans.CostOnlyPerUnit / trans.PackSize) * plus),
                        //    OutQty = Library.ConvertDecimalToStringForm(minus),
                        //    OutCost = Library.ConvertDecimalToStringForm(trans.CostOnlyPerUnit),
                        //    OutValue = Library.ConvertDecimalToStringForm((trans.CostOnlyPerUnit / trans.PackSize) * minus),
                        //    BalQty = Library.ConvertDecimalToStringForm(resultForProHD),
                        //    BalCost = Library.ConvertDecimalToStringForm(Library.GetAverage(item)),
                        //    BalValue = Library.ConvertDecimalToStringForm(Library.GetAverage(item) * resultForProHD),
                        //    DocDesc = trans.TransactionType.Name,
                        //    FKTrans = trans.FKTransactionType
                        //});
                        sumTotalInCost += trans.CostOnlyPerUnit * plus;
                        sumTotalOutCost += trans.CostOnlyPerUnit * minus;
                        getForProduct.Remove(trans);
                    }

                    lsGetResult.Add(new InOutValue() // ยอดยกไป
                    {
                        CreateDate = "",
                        DocNo = "ยอดยกไป",
                        DocDtlNumber = "",
                        ProductName = "",
                        Pz = "",
                        InQty = "",
                        InCost = "",
                        InValue = "",
                        OutQty = "",
                        OutCost = "",
                        OutValue = "",
                        BalQty = Library.ConvertDecimalToStringForm(resultForProHD),
                        BalCost = Library.ConvertDecimalToStringForm(Library.GetAverage(item)),
                        BalValue = Library.ConvertDecimalToStringForm(Library.GetAverage(item) * resultForProHD),
                        DocDesc = "ยกไป " + Library.ConvertDateToThaiDate(time_E)
                    });
                    lsGetResult.Add(new InOutValue() // ยอดยกไป
                    {
                        CreateDate = "",
                        DocNo = "",
                        DocDtlNumber = "",
                        ProductName = "",
                        Pz = "",
                        InQty = "",
                        InCost = "",
                        InValue = "",
                        OutQty = "",
                        OutCost = "",
                        OutValue = "",
                        BalQty = "",
                        BalCost = "",
                        BalValue = "",
                        DocDesc = "",
                    });
                    progressBar1.Value = i;
                    progressBar1.Refresh();

                    Console.WriteLine("******* " + fkPro.Count + " " + item + " " + i + " " + getForProduct.Count());
                }
                Console.WriteLine(startPro + " " + DateTime.Now);
                //foreach (var item in fkPro)
                //{
                //    decimal movementAvg = 0;
                //    decimal balance = 0;
                //    i++;
                //    List<StoreFrontStockDetails> details = db.StoreFrontStockDetails
                //    .Where(w => w.StoreFrontStock.FKProduct == item && w.StoreFrontStock.Enable == true && w.Enable == true &&
                //  DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(time_S) &&
                //    DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(time_E)
                //  ).OrderBy(w => w.CreateDate).ToList();

                //    if (details.Count == 0)
                //    {
                //        progressBar1.Value = i;
                //        progressBar1.Refresh();
                //        continue;
                //    }
                //    decimal resultForProHD = Library.GetResult(item, time_S);
                //    movementAvg = Library.GetAverage(item);

                //    lsGetResult.Add(new InOutValue() // ยอดยกมา
                //    {
                //        CreateDate = "",
                //        DocNo = "ยอดยกมา",
                //        DocDtlNumber = "",
                //        ProductName = "",
                //        Pz = "",
                //        InQty = "",
                //        InCost = "",
                //        InValue = "",
                //        OutQty = "",
                //        OutCost = "",
                //        OutValue = "",
                //        BalQty = Library.ConvertDecimalToStringForm(resultForProHD),
                //        BalCost = Library.ConvertDecimalToStringForm(movementAvg),
                //        BalValue = Library.ConvertDecimalToStringForm(movementAvg * resultForProHD),
                //        DocDesc = "ยกมาจาก " + Library.ConvertDateToThaiDate(time_S)
                //    });

                //    balance = movementAvg * resultForProHD;

                //    decimal sumPlus = 0;
                //    decimal sumMinus = 0;
                //    decimal sumTotalInCost = 0;
                //    decimal sumTotalOutCost = 0;
                //    foreach (var trans in details.OrderBy(w => w.Barcode).ToList()) // transactions
                //    {
                //        decimal plus = 0;
                //        decimal minus = 0;
                //        sumTotalInCost = 0;
                //        sumTotalOutCost = 0;
                //        if (trans.TransactionType.IsPlus == true)
                //        {
                //            plus = trans.ActionQty;
                //            resultForProHD = resultForProHD + plus;
                //            // bal value in 1 transation
                //            movementAvg = trans.CostOnlyPerUnit;
                //            sumTotalInCost = movementAvg * plus;
                //            balance = balance + sumTotalInCost;
                //        }
                //        else
                //        {
                //            minus = trans.ActionQty;
                //            resultForProHD = resultForProHD - minus;
                //            // bal value in 1 transation                       
                //            sumTotalOutCost = movementAvg * minus; // มูลค่าจ่าย
                //            balance = balance - sumTotalOutCost;
                //        }
                //        sumPlus += plus;
                //        sumMinus += minus;
                //        //balance = movementAvg * resultForProHD;
                //        ///////// use movement average
                //        lsGetResult.Add(new InOutValue()
                //        {
                //            CreateDate = Library.ConvertDateToThaiDate(trans.CreateDate, true),
                //            DocNo = trans.DocNo,
                //            DocDtlNumber = trans.DocDtlNumber + "",
                //            ProductName = trans.ProductDetails.Code + " " + trans.ProductDetails.Products.ThaiName + " " + trans.ProductDetails.ProductUnit.Name + "x" + ((int)trans.PackSize),
                //            Pz = ((int)trans.PackSize) + "",
                //            InQty = Library.ConvertDecimalToStringForm(plus),
                //            InCost = Library.ConvertDecimalToStringForm(movementAvg),
                //            InValue = Library.ConvertDecimalToStringForm(plus * movementAvg),
                //            OutQty = Library.ConvertDecimalToStringForm(minus),
                //            OutCost = Library.ConvertDecimalToStringForm(movementAvg),
                //            OutValue = Library.ConvertDecimalToStringForm(minus * movementAvg),
                //            BalQty = Library.ConvertDecimalToStringForm(resultForProHD),
                //            BalCost = Library.ConvertDecimalToStringForm(movementAvg),
                //            BalValue = Library.ConvertDecimalToStringForm(balance),
                //            DocDesc = trans.TransactionType.Name,
                //            FKTrans = trans.FKTransactionType
                //        });
                //        /////////////////// old logic
                //        //lsGetResult.Add(new InOutValue()
                //        //{
                //        //    CreateDate = Library.ConvertDateToThaiDate(trans.CreateDate, true),
                //        //    DocNo = trans.DocNo,
                //        //    DocDtlNumber = trans.DocDtlNumber + "",
                //        //    ProductName = trans.ProductDetails.Code + " " + trans.ProductDetails.Products.ThaiName + " " + trans.ProductDetails.ProductUnit.Name + "x" + ((int)trans.PackSize),
                //        //    Pz = ((int)trans.PackSize) + "",
                //        //    InQty = Library.ConvertDecimalToStringForm(plus),
                //        //    InCost = Library.ConvertDecimalToStringForm(trans.CostOnlyPerUnit),
                //        //    InValue = Library.ConvertDecimalToStringForm((trans.CostOnlyPerUnit / trans.PackSize) * plus),
                //        //    OutQty = Library.ConvertDecimalToStringForm(minus),
                //        //    OutCost = Library.ConvertDecimalToStringForm(trans.CostOnlyPerUnit),
                //        //    OutValue = Library.ConvertDecimalToStringForm((trans.CostOnlyPerUnit / trans.PackSize) * minus),
                //        //    BalQty = Library.ConvertDecimalToStringForm(resultForProHD),
                //        //    BalCost = Library.ConvertDecimalToStringForm(Library.GetAverage(item)),
                //        //    BalValue = Library.ConvertDecimalToStringForm(Library.GetAverage(item) * resultForProHD),
                //        //    DocDesc = trans.TransactionType.Name,
                //        //    FKTrans = trans.FKTransactionType
                //        //});
                //        sumTotalInCost += trans.CostOnlyPerUnit * plus;
                //        sumTotalOutCost += trans.CostOnlyPerUnit * minus;
                //    }

                //    lsGetResult.Add(new InOutValue() // ยอดยกไป
                //    {
                //        CreateDate = "",
                //        DocNo = "ยอดยกไป",
                //        DocDtlNumber = "",
                //        ProductName = "",
                //        Pz = "",
                //        InQty = "",
                //        InCost = "",
                //        InValue = "",
                //        OutQty = "",
                //        OutCost = "",
                //        OutValue = "",
                //        BalQty = Library.ConvertDecimalToStringForm(resultForProHD),
                //        BalCost = Library.ConvertDecimalToStringForm(Library.GetAverage(item)),
                //        BalValue = Library.ConvertDecimalToStringForm(Library.GetAverage(item) * resultForProHD),
                //        DocDesc = "ยกไป " + Library.ConvertDateToThaiDate(time_E)
                //    });

                //    lsGetResult.Add(new InOutValue() // ยอดยกไป
                //    {
                //        CreateDate = "",
                //        DocNo = "",
                //        DocDtlNumber = "",
                //        ProductName = "",
                //        Pz = "",
                //        InQty = "",
                //        InCost = "",
                //        InValue = "",
                //        OutQty = "",
                //        OutCost = "",
                //        OutValue = "",
                //        BalQty = "",
                //        BalCost = "",
                //        BalValue = "",
                //        DocDesc = "",
                //    });
                //    progressBar1.Value = i;
                //    progressBar1.Refresh();
                //}

                var countList = lsGetResult.Where(w => w.DocNo != "ยอดยกมา" && w.DocNo != "สรุปรวม").Count();

                dataGridView1.DataSource = lsGetResult;
                dataGridView1.Refresh();

                var dt = Library.ConvertToDataTable(lsGetResult.Select(s =>
                new
                {
                    CreateDate = s.CreateDate,
                    DocNo = s.DocNo == "ยอดยกมา" || s.DocNo == "สรุปรวม" ? s.DocNo : s.DocNo + "- " + s.DocDtlNumber,
                    DocName = s.DocNo == "ยอดยกมา" || s.DocNo == "สรุปรวม" ? "" : s.ProductName,
                    InQty = s.InQty == "0.00" ? "" : s.InQty,
                    InCost = s.InQty == "0.00" ? "" : s.InCost,
                    InValue = s.InQty == "0.00" ? "" : s.InValue,
                    OutQty = s.OutQty == "0.00" ? "" : s.OutQty,
                    OutCost = s.OutQty == "0.00" ? "" : s.OutCost,
                    OutValue = s.OutQty == "0.00" ? "" : s.OutValue,
                    BalQty = s.BalQty == "0.00" ? "" : s.BalQty,
                    BalCost = s.BalQty == "0.00" ? "" : s.BalCost,
                    BalValue = s.BalQty == "0.00" ? "" : s.BalValue,
                    ReferDoc = s.FKTrans == MyConstant.PosTransaction.Selling ? "" : s.DocDesc,
                    countList = countList,
                    Date_S = Library.ConvertDateToThaiDate(time_S),
                    Date_E = Library.ConvertDateToThaiDate(time_E),
                    Product_S = txt_p_S.Text == "" || txt_p_S.Text == null ? "ทั้งหมด" : txt_p_S.Text,
                    Product_E = txt_p_E.Text == "" || txt_p_E.Text == null ? "ทั้งหมด" : txt_p_E.Text,

                }).ToList());
                lb_msg.Text = "ระบบประมวลผลสำเร็จ";
                this.Refresh();
                Cursor.Current = Cursors.Default;
                frmMainReport mr = new frmMainReport(this, dt, true);
                mr.Show();
            }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void ProductRawInOut_Load(object sender, EventArgs e)
        {
            lb_msg.Visible = false;
            button3.Enabled = false;
            button4.Enabled = false;
            checkBoxProduct.Checked = true;

            var getPdtl = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
            AutoCompleteStringCollection colPdtl = new AutoCompleteStringCollection();
            foreach (var item in getPdtl)
            {
                colPdtl.Add(item.Code);
            }
            txt_p_S.AutoCompleteCustomSource = colPdtl;

            var getPdg = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
            AutoCompleteStringCollection colPdg = new AutoCompleteStringCollection();
            foreach (var item in getPdg)
            {
                colPdg.Add(item.Code);
            }
            txt_p_E.AutoCompleteCustomSource = colPdg;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            txt_p_S.Text = null;
            dateTimePicker_S.Value = DateTime.Now;
            dateTimePicker_E.Value = DateTime.Now;
        }

        private void checkBoxProduct_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxProduct.Checked == true)
            {
                txt_p_S.Enabled = false;
                txt_p_E.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                txt_p_S.Text = null;
                txt_p_E.Text = null;
            }
            else
            {
                txt_p_S.Enabled = true;
                txt_p_E.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                txt_p_S.Text = null;
                txt_p_E.Text = null;
            }
        }
        bool changeTxt;
        private void button3_Click(object sender, EventArgs e)
        {
            changeTxt = true;
            SelectedProductPopup obj = new SelectedProductPopup(this);
            obj.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            changeTxt = false;
            SelectedProductPopup obj = new SelectedProductPopup(this);
            obj.ShowDialog();
        }
        public void BinddingProduct(int id)
        {
            var product = SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == id);
            if (changeTxt == true)
            {
                txt_p_S.Text = product.Code;
            }
            else
            {
                txt_p_E.Text = product.Code;
            }
        }

        private void txt_p_S_TextChanged(object sender, EventArgs e)
        {
            txt_p_E.Text = txt_p_S.Text;
        }

        private void lb_msg_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
      
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานรับ - จ่าย - คงเหลือ ณ "
                + Library.ConvertDateToThaiDate(dateTimePicker_S.Value) + " - "
                + Library.ConvertDateToThaiDate(dateTimePicker_E.Value), "สินค้า : " + txt_p_S.Text + " - " + txt_p_E.Text);
        }
    }
}
