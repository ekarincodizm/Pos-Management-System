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
    public partial class CheckStockFront1Form : Form
    {
        public CheckStockFront1Form()
        {
            InitializeComponent();
        }
        private string _Fromform = "";
        public CheckStockFront1Form(CheckStockFront1ListForm checkStockFront1ListForm, string code)
        {
            InitializeComponent();
            this.checkStockFront1ListForm = checkStockFront1ListForm;
            this.code = code;

            textBoxDocNo.Text = code;
            _Fromform = "CheckStockFront1ListForm";
            //dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count + 1);
            //dataGridView1.Rows.RemoveAt(this.dataGridView1.SelectedRows[dataGridView1.Rows.Count].Index);
            //dataGridView1.Rows.Add("" + dataGridView1.Rows.Count);
            //dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            //dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            //dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
        }

        StoreFrontValueDoc _GetDocNo;
        private void CheckStockFront1Form_Load(object sender, EventArgs e)
        {
            if (_Fromform == "CheckStockFront1ListForm")
            {
                this.BinddingUI();
                //dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                if (_GetDocNo.ConfirmCheck1Date != null || _GetDocNo.ConfirmCheck2Date != null)
                {
                    //dataGridView1.AllowUserToAddRows = false;
                }
            }
            else
            {

            }
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[colCode];
        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //if (dataGridView1.Rows.Count == 0)
            //{
            //    dataGridView1.Rows.Add("1");
            //}
            //else
            //{
            //    int row = dataGridView1.CurrentCell.RowIndex;

            //    int number = int.Parse(dataGridView1.Rows[row].Cells[colNumber].Value.ToString());

            //    //MessageBox.Show("row " + row + " num " + number);
            //    //dataGridView1.Rows[row].Cells[colNumber].Value = number - 1;
            //    //number++;
            //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //    {
            //        //number++;
            //        dataGridView1.Rows[i].Cells[colNumber].Value = (i + 1);
            //    }
            //}
            //TotalSummary();
        }
        int colNumber = 0;
        int colCode = 1;
        int colSearckPro = 2;
        int colQty1 = 3;
        int colQty2 = 4;
        int colName = 5;
        int colUnit = 6;
        int colPZ = 7;

        int colCostOnly = 8;
        int colSellPrice = 9;
        int colTotal = 10;
        int colZone = 11;
        int colProDtl = 12;
        private CheckStockFront1ListForm checkStockFront1ListForm;
        private string code;

        void TotalSummary()
        {
            try
            {
                decimal sumUnit = 0;
                decimal sumCost = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {

                    if (dataGridView1.Rows[i].Cells[colCode].Value != null)
                    {
                        decimal unit1 = decimal.Parse(dataGridView1.Rows[i].Cells[colQty1].Value.ToString());
                        decimal unit2 = decimal.Parse(dataGridView1.Rows[i].Cells[colQty2].Value.ToString());

                        decimal cost = decimal.Parse(dataGridView1.Rows[i].Cells[colTotal].Value.ToString());
                        decimal qty = 0;
                        qty = unit1;
                        if (_GetDocNo != null)
                        {
                            if (_GetDocNo.ConfirmCheck1Date != null && _GetDocNo.ConfirmCheck2Date == null)
                            {
                                qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty2].Value.ToString());
                            }
                            else if (_GetDocNo.ConfirmCheck1Date != null && _GetDocNo.ConfirmCheck2Date != null)
                            {
                                qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty2].Value.ToString());
                            }

                        }

                        sumUnit += qty;
                        sumCost += cost;
                    }
                }
                textBoxUnit.Text = Library.ConvertDecimalToStringForm(sumUnit);
                textBoxValue.Text = Library.ConvertDecimalToStringForm(sumCost);
            }
            catch (Exception)
            {

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //int row = dataGridView1.CurrentCell.RowIndex;
            //if (e.ColumnIndex == colCode)
            //{
            //    dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty1];
            //    dataGridView1.BeginEdit(true);
            //}
            //else if (e.ColumnIndex == colQty1)
            //{
            //    dataGridView1.Rows.Add("" + (dataGridView1.Rows.Count + 1));
            //    dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colCode];
            //    dataGridView1.BeginEdit(true);
            //}


        }

        Boolean hasCellBeenEdited = false;
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            hasCellBeenEdited = true;
            //try
            //{
            //    int row = dataGridView1.CurrentCell.RowIndex;
            //    string code = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
            //    if (e.ColumnIndex == colCode)
            //    {
            //        var data = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == code);
            //        if (data != null)
            //        {
            //            dataGridView1.Rows[row].Cells[colNumber].Value = row + 1;
            //            dataGridView1.Rows[row].Cells[colCode].Value = data.Code;
            //            dataGridView1.Rows[row].Cells[colQty1].Value = "0";
            //            dataGridView1.Rows[row].Cells[colQty2].Value = "0";
            //            dataGridView1.Rows[row].Cells[colName].Value = data.Products.ThaiName;
            //            dataGridView1.Rows[row].Cells[colUnit].Value = data.ProductUnit.Name;
            //            dataGridView1.Rows[row].Cells[colPZ].Value = data.PackSize;

            //            dataGridView1.Rows[row].Cells[colCostOnly].Value = data.CostOnly;
            //            dataGridView1.Rows[row].Cells[colSellPrice].Value = Library.ConvertDecimalToStringForm(data.SellPrice);
            //            dataGridView1.Rows[row].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(data.CostOnly);
            //            dataGridView1.Rows[row].Cells[colZone].Value = "หน้าร้าน";
            //            dataGridView1.Rows[row].Cells[colProDtl].Value = data.Id;

            //            // get product

            //            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty1];
            //            dataGridView1.BeginEdit(true);
            //        }
            //        else
            //        {
            //            //MessageBox.Show("");
            //            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colCode];
            //            dataGridView1.BeginEdit(true);
            //        }

            //    }
            //    else if (e.ColumnIndex == colQty1)
            //    {
            //        decimal costOly = decimal.Parse(dataGridView1.Rows[row].Cells[colCostOnly].Value.ToString());
            //        decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty1].Value.ToString());
            //        dataGridView1.Rows[row].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(qty * costOly);
            //        if (row == dataGridView1.RowCount - 1)
            //        {
            //            //dataGridView1.Rows.Add(1);
            //            dataGridView1.Rows.Add("" + (dataGridView1.Rows.Count + 1));
            //            dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colCode];
            //            dataGridView1.BeginEdit(true);
            //        }
            //        else
            //        {
            //            dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colQty1];
            //            dataGridView1.BeginEdit(true);
            //        }
            //        TotalSummary();
            //    }
            //    else if (e.ColumnIndex == colQty2)
            //    {
            //        decimal costOly = decimal.Parse(dataGridView1.Rows[row].Cells[colCostOnly].Value.ToString());
            //        decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty2].Value.ToString());
            //        dataGridView1.Rows[row].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(qty * costOly);
            //        //if (row == dataGridView1.RowCount - 1)
            //        //{
            //        //    //dataGridView1.Rows.Add(1);
            //        //    dataGridView1.Rows.Add("" + (dataGridView1.Rows.Count + 1));
            //        //    dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colCode];
            //        //    dataGridView1.BeginEdit(true);
            //        //}
            //        //else
            //        //{
            //        //    dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colCode];
            //        //    dataGridView1.BeginEdit(true);
            //        //}
            //        TotalSummary();
            //    }

            //}
            //catch (Exception)
            //{

            //}
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    // ค้นหา ด้วยเลขที่เอกสาร
                    BinddingUI();
                    break;
                default:
                    break;
            }
        }

        private void BinddingUI()
        {
            string docno = textBoxDocNo.Text.Trim();
            using (SSLsEntities db = new SSLsEntities())
            {
                var getDocOlder = db.StoreFrontValueDoc.SingleOrDefault(w => w.DocNo == docno && w.Enable == true);

                if (getDocOlder != null)
                {
                    _GetDocNo = getDocOlder;
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                    // bindding data
                    textBoxDesc.Text = getDocOlder.Description;
                    textBoxDocdate.Text = Library.ConvertDateToThaiDate(getDocOlder.DocDate);
                    textBoxDocNo.Text = getDocOlder.DocNo;
                    textBoxUnit.Text = Library.ConvertDecimalToStringForm(getDocOlder.TotalQtyUnit);
                    textBoxValue.Text = Library.ConvertDecimalToStringForm(getDocOlder.TotalCostOnly);
                    decimal sumCount1 = 0;
                    decimal sumCount2 = 0;

                    decimal sumCoust = 0;
                    decimal sumValue = 0;
                    foreach (var item in getDocOlder.StoreFrontValueSet.Where(w => w.Enable == true).OrderBy(w => w.Number).ToList())
                    {
                        sumCount1 += item.QtyUnit1;
                        sumCount2 += item.QtyUnit2;
                        sumCoust += (item.CostOnlyPerUnit * item.QtyUnit2);
                        sumValue += (item.SellPricePerUnit * item.QtyUnit2);
                        dataGridView1.Rows.Add
                            (
                            item.Number,
                            item.ProductDetails.Code,
                            "",
                                 Library.ConvertDecimalToStringForm(item.QtyUnit1),
                             Library.ConvertDecimalToStringForm(item.QtyUnit2),
                            item.ProductDetails.Products.ThaiName,
                            item.ProductDetails.ProductUnit.Name,
                            Library.ConvertDecimalToStringForm(item.Packsize),
                             Library.ConvertDecimalToStringForm(item.CostOnlyPerUnit),
                             Library.ConvertDecimalToStringForm(item.SellPricePerUnit),
                             Library.ConvertDecimalToStringForm(item.Total),
                             "หน้าร้าน",
                             item.FKProductDetails
                            );
                        //Console.WriteLine(Library.ConvertDecimalToStringForm(item.Total));

                        dataGridView2.Rows.Add
                                (
                                item.Number,
                                item.ProductDetails.Code,
                                     Library.ConvertDecimalToStringForm(item.QtyUnit1),
                                 Library.ConvertDecimalToStringForm(item.QtyUnit2),
                                item.ProductDetails.Products.ThaiName,
                                item.ProductDetails.ProductUnit.Name,
                                Library.ConvertDecimalToStringForm(item.Packsize),
                                 Library.ConvertDecimalToStringForm(item.CostOnlyPerUnit),
                                 Library.ConvertDecimalToStringForm(item.CostOnlyPerUnit * item.QtyUnit2),
                                 Library.ConvertDecimalToStringForm(item.SellPricePerUnit),
                                     Library.ConvertDecimalToStringForm(item.SellPricePerUnit * item.QtyUnit2),              
                                 "หน้าร้าน"
                                );
                    }
                    dataGridView2.Rows.Add
                               (
                               "",
                               "รวม",
                                    Library.ConvertDecimalToStringForm(sumCount1),
                                Library.ConvertDecimalToStringForm(sumCount2),
                               "",
                               "",
                               "",
                                "",
                                Library.ConvertDecimalToStringForm(sumCoust),
                                "",
                                    Library.ConvertDecimalToStringForm(sumValue),
                                ""
                               );
                    _GetDocNo = getDocOlder;
                    if (_GetDocNo.ConfirmCheck1Date == null)
                    {
                        dataGridView1.Columns[colQty1].ReadOnly = false;
                        dataGridView1.Columns[colQty2].ReadOnly = true;
                        label8.Text = "ยังไม่ยืนยันตรวจนับ 1";
                        label8.Visible = true;
                    }
                    else if (_GetDocNo.ConfirmCheck1Date != null && _GetDocNo.ConfirmCheck2Date == null)
                    {
                        dataGridView1.Columns[colQty1].ReadOnly = true;
                        dataGridView1.Columns[colQty2].ReadOnly = false;
                        dataGridView1.Columns[colCode].ReadOnly = true;
                        label8.Text = "ยืนยันตรวจนับ 1 แล้ว";
                        label8.Visible = true;
                    }
                    else
                    {
                        dataGridView1.Columns[colCode].ReadOnly = true;
                        dataGridView1.Columns[colQty1].ReadOnly = true;
                        dataGridView1.Columns[colQty2].ReadOnly = true;
                        label8.Text = "ยืนยันตรวจนับ 2 แล้ว";
                        label8.Visible = true;
                    }

                }
                else
                {
                    label8.Visible = false;
                    MessageBox.Show("ไม่พบเอกสาร");
                }
            }
        }
        public void setGridView(int row, ProductDetails data)
        {
            dataGridView1.Rows[row].Cells[colNumber].Value = row + 1;
            dataGridView1.Rows[row].Cells[colCode].Value = data.Code;
            //dataGridView1.Rows[row].Cells[colQty1].Value = "0";
            //dataGridView1.Rows[row].Cells[colQty2].Value = "0";
            dataGridView1.Rows[row].Cells[colName].Value = data.Products.ThaiName;
            dataGridView1.Rows[row].Cells[colUnit].Value = data.ProductUnit.Name;
            dataGridView1.Rows[row].Cells[colPZ].Value = data.PackSize;

            dataGridView1.Rows[row].Cells[colCostOnly].Value = data.CostOnly;
            dataGridView1.Rows[row].Cells[colSellPrice].Value = Library.ConvertDecimalToStringForm(data.SellPrice);
            dataGridView1.Rows[row].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(data.CostOnly);
            dataGridView1.Rows[row].Cells[colZone].Value = "หน้าร้าน";
            dataGridView1.Rows[row].Cells[colProDtl].Value = data.Id;
            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty1];
            dataGridView1.BeginEdit(true);
        }
        /// <summary>
        /// insert row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            //switch (e.KeyCode)
            //{
            //    case Keys.Insert:
            //        int row = dataGridView1.CurrentCell.RowIndex;
            //        //dataGridView1.Rows[row].Cells[colNumber].Value
            //        dataGridView1.Rows.Insert(row, "");
            //        for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //        {
            //            //number++;
            //            dataGridView1.Rows[i].Cells[colNumber].Value = (i + 1);
            //        }
            //        break;
            //    default:
            //        break;
            //}
        }

        /// <summary>
        /// บันทึก รายการ ระหว่างเชค
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string getDocno = textBoxDocNo.Text.Trim();
                using (SSLsEntities db = new SSLsEntities())
                {
                    var getDocOlder = db.StoreFrontValueDoc.SingleOrDefault(w => w.DocNo == getDocno && w.Enable == true);
                    if (getDocOlder != null)
                    {
                        if (getDocOlder.ConfirmCheck1Date != null && getDocOlder.ConfirmCheck2Date != null)
                        {
                            MessageBox.Show("ไม่สามารถบันทึกได้ ยืนยันนับ 2 แล้ว");
                            return;
                        }
                    }
                }
                decimal sumUnit = 0;
                decimal sumCost = 0;
                List<StoreFrontValueSet> dtl = new List<StoreFrontValueSet>();
                StoreFrontValueSet d;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[colCode].Value != null)
                    {
                        d = new StoreFrontValueSet();
                        int number = int.Parse(dataGridView1.Rows[i].Cells[colNumber].Value.ToString());
                        string name = dataGridView1.Rows[i].Cells[colName].Value.ToString();
                        decimal qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty1].Value.ToString());
                        decimal qty2 = 0;
                        if (dataGridView1.Rows[i].Cells[colQty2].Value != null)
                        {
                            qty2 = decimal.Parse(dataGridView1.Rows[i].Cells[colQty2].Value.ToString());
                        }

                        decimal cost = decimal.Parse(dataGridView1.Rows[i].Cells[colCostOnly].Value.ToString());
                        decimal sellPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colSellPrice].Value.ToString());
                        decimal total = decimal.Parse(dataGridView1.Rows[i].Cells[colTotal].Value.ToString());
                        int fkProdtl = int.Parse(dataGridView1.Rows[i].Cells[colProDtl].Value.ToString());
                        decimal pz = decimal.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString());
                        d.Enable = true;
                        d.Name = name;
                        d.Number = number;
                        d.CreateDate = DateTime.Now;
                        d.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        d.UpdateDate = DateTime.Now;
                        d.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        d.FKProductDetails = fkProdtl;
                        d.QtyUnit1 = qty;
                        d.QtyUnit2 = qty2;
                        d.SellPricePerUnit = sellPrice;
                        d.CostOnlyPerUnit = cost;
                        d.Total = total;
                        d.Packsize = pz;

                        sumUnit = sumUnit + qty;
                        sumCost = sumCost + total;
                        dtl.Add(d);
                    }
                }
                textBoxUnit.Text = Library.ConvertDecimalToStringForm(sumUnit);
                textBoxValue.Text = Library.ConvertDecimalToStringForm(sumCost);
                using (SSLsEntities db = new SSLsEntities())
                {
                    StoreFrontValueDoc hd = new StoreFrontValueDoc();
                    hd.Enable = true;
                    hd.Used = true;
                    hd.Description = textBoxDesc.Text;
                    hd.CreateDate = DateTime.Now;
                    hd.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    hd.UpdateDate = DateTime.Now;
                    hd.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    int count = db.StoreFrontValueDoc.Count();
                    string docno = MyConstant.PrefixForGenerateCode.CheckStoreFront + DateTime.Now.ToString("yyyyMM") + Library.GenerateCodeFormCount(count, 3);
                    hd.DocDate = DateTime.Now;
                    hd.DocNo = docno;
                    hd.TotalQtyUnit = sumUnit;
                    hd.TotalCostOnly = sumCost;
                    hd.SaveNumber = 1;
                    hd.StoreFrontValueSet = dtl;

                    if (getDocno != "") // แปลว่า ไม่แอดใหม่
                    {

                        var getDocOlder = db.StoreFrontValueDoc.SingleOrDefault(w => w.DocNo == getDocno && w.Enable == true);
                        foreach (var item in getDocOlder.StoreFrontValueSet.Where(w => w.Enable == true).ToList())
                        {
                            item.UpdateDate = DateTime.Now;
                            item.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            item.Description = "ยกเลิกบันทึก " + getDocOlder.SaveNumber;
                            item.Enable = false;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        foreach (var item in dtl)
                        {
                            item.FKDocNo = getDocOlder.DocNo;
                            getDocOlder.StoreFrontValueSet.Add(item);
                        }
                        getDocOlder.Description = textBoxDesc.Text;
                        getDocOlder.SaveNumber = getDocOlder.SaveNumber + 1;
                        getDocOlder.UpdateDate = DateTime.Now;
                        getDocOlder.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        getDocOlder.TotalCostOnly = sumCost;
                        getDocOlder.TotalQtyUnit = sumUnit;
                        db.Entry(getDocOlder).State = EntityState.Modified;
                        db.SaveChanges();
                        MessageBox.Show("บันทึกเรียบร้อย ครั้งที่ " + getDocOlder.SaveNumber);
                        //textBoxDocdate.Text = Library.ConvertDateToThaiDate(hd.CreateDate);
                        //textBoxDocNo.Text = docno;
                    }
                    else
                    {
                        db.StoreFrontValueDoc.Add(hd);
                        db.SaveChanges();
                        MessageBox.Show("บันทึกเรียบร้อย ครั้งที่ " + hd.SaveNumber);
                        textBoxDocdate.Text = Library.ConvertDateToThaiDate(hd.CreateDate);
                        textBoxDocNo.Text = docno;
                    }
                    BinddingUI();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("พบข้อผิดพลาด");
            }
        }
        /// <summary>
        /// พิมพ์
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string code = textBoxDocNo.Text.Trim();
                using (SSLsEntities db = new SSLsEntities())
                {
                    var getDocOlder = db.StoreFrontValueDoc.SingleOrDefault(w => w.DocNo == code && w.Enable == true);
                    if (getDocOlder != null)
                    {
                        getDocOlder.PrintNumber = getDocOlder.PrintNumber + 1;
                        db.Entry(getDocOlder).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    else
                    {
                        MessageBox.Show("ไม่ถูกต้อง");
                    }
                }

                frmMainReport mr = new frmMainReport(this, code);
                mr.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("ไม่ถูกต้อง");
            }
        }
        /// <summary>
        /// ยืนยันตรวจนับ 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                string docno = textBoxDocNo.Text.Trim();
                var getDocOlder = db.StoreFrontValueDoc.SingleOrDefault(w => w.DocNo == docno && w.Enable == true);
                if (getDocOlder != null)
                {
                    if (getDocOlder.ConfirmCheck1Date != null)
                    {
                        MessageBox.Show("ยืนยันนับ 1 ไปแล้ว");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("ไม่สามาถบันทึกได้");
                    return;
                }

            }
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                  "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    CheckCount1();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void CheckCount1()
        {
            string docno = textBoxDocNo.Text.Trim();
            using (SSLsEntities db = new SSLsEntities())
            {
                var getDocOlder = db.StoreFrontValueDoc.SingleOrDefault(w => w.DocNo == docno && w.Enable == true);
                if (getDocOlder.ConfirmCheck1Date != null)
                {
                    return;
                }
                if (getDocOlder != null)
                {
                    _GetDocNo = getDocOlder;
                    if (_GetDocNo.ConfirmCheck1Date == null) // ยังไม่ยืนยัน 1
                    {
                        getDocOlder.ConfirmCheck1Date = DateTime.Now;
                        getDocOlder.ConfirmCheck1By = Singleton.SingletonAuthen.Instance().Id;
                        foreach (var item in getDocOlder.StoreFrontValueSet.Where(w => w.Enable == true).ToList())
                        {
                            item.QtyUnit2 = item.QtyUnit1;
                            item.Total = item.CostOnlyPerUnit * item.QtyUnit2;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        db.Entry(getDocOlder).State = EntityState.Modified;
                        db.SaveChanges();
                        MessageBox.Show("ยืนยันตรวจนับ 1 เรียบร้อย");
                        BinddingUI();
                    }

                    //else if (_GetDocNo.ConfirmCheck1Date != null && _GetDocNo.ConfirmCheck2Date == null)
                    //{
                    //    dataGridView1.Columns[colQty1].ReadOnly = true;
                    //    dataGridView1.Columns[colQty2].ReadOnly = false;
                    //    dataGridView1.Columns[colCode].ReadOnly = true;
                    //}
                    //else
                    //{
                    //    dataGridView1.Columns[colCode].ReadOnly = true;
                    //}
                }
            }
        }

        /// <summary>
        /// ยืนยันตรวจนับ 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                string docno = textBoxDocNo.Text.Trim();
                var getDocOlder = db.StoreFrontValueDoc.SingleOrDefault(w => w.DocNo == docno && w.Enable == true);
                if (getDocOlder != null)
                {
                    if (getDocOlder.ConfirmCheck2Date != null)
                    {
                        MessageBox.Show("ยืนยันนับ 2 ไปแล้ว");
                        return;
                    }
                    else if (getDocOlder.ConfirmCheck2Date == null && getDocOlder.ConfirmCheck1Date == null)
                    {
                        MessageBox.Show("ยังไม่ยืนยันนับ 1");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("ไม่สามาถบันทึกได้");
                    return;
                }

            }
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    CheckCount2();

                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void CheckCount2()
        {
            string docno = textBoxDocNo.Text.Trim();
            using (SSLsEntities db = new SSLsEntities())
            {
                var getDocOlder = db.StoreFrontValueDoc.SingleOrDefault(w => w.DocNo == docno && w.Enable == true);
                if (getDocOlder != null)
                {
                    _GetDocNo = getDocOlder;
                    if (_GetDocNo.ConfirmCheck1Date != null && _GetDocNo.ConfirmCheck2Date == null) // ยังไม่ยืนยัน 1
                    {
                        getDocOlder.ConfirmCheck2Date = DateTime.Now;
                        getDocOlder.ConfirmCheck2By = Singleton.SingletonAuthen.Instance().Id;
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[colCode].Value != null)
                            {
                                int number = int.Parse(dataGridView1.Rows[i].Cells[colNumber].Value.ToString());
                                decimal qty1 = decimal.Parse(dataGridView1.Rows[i].Cells[colQty1].Value.ToString());
                                decimal qty2 = decimal.Parse(dataGridView1.Rows[i].Cells[colQty2].Value.ToString());
                                decimal total = decimal.Parse(dataGridView1.Rows[i].Cells[colTotal].Value.ToString());
                                if (qty1 != qty2)
                                {
                                    // แปลว่ามีการนับ 2
                                    var edit = getDocOlder.StoreFrontValueSet.FirstOrDefault(w => w.Enable == true && w.Number == number);
                                    edit.QtyUnit2 = qty2;
                                    edit.Total = total;
                                    db.Entry(edit).State = EntityState.Modified;
                                }

                            }
                        }
                        getDocOlder.TotalQtyUnit = decimal.Parse(textBoxUnit.Text);
                        getDocOlder.TotalCostOnly = decimal.Parse(textBoxValue.Text);
                        db.Entry(getDocOlder).State = EntityState.Modified;
                        db.SaveChanges();
                        MessageBox.Show("ยืนยันตรวจนับ 2 เรียบร้อย");
                        BinddingUI();
                    }

                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            SSLsEntities db = new SSLsEntities();
            try
            {
                if (hasCellBeenEdited && dataGridView1.CurrentCell.ColumnIndex != dataGridView1.ColumnCount - 10)
                {
                    int desiredColumn = dataGridView1.CurrentCell.ColumnIndex + 2;
                    int desiredRow = dataGridView1.CurrentCell.RowIndex - 1;
                    string _Code = dataGridView1.Rows[desiredRow].Cells[colCode].Value.ToString().Replace(" ", "");
                    var data = db.ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == _Code);

                    if (desiredColumn == 3)
                    {
                        dataGridView1.CurrentCell = dataGridView1[desiredColumn, desiredRow];
                        hasCellBeenEdited = false;

                        if (_Code == null)
                        {

                        }
                        else
                        {

                            if (data == null)
                            {
                                MessageBox.Show("ไม่พบรหัสสินค้า " + _Code + " ในระบบกรุณาตรวจสอบอีกครั้ง", "คำเตือนจากระบบ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                dataGridView1.Rows[desiredRow].Cells[colCode].Value = null;
                                SendKeys.Send("{LEFT}");
                                SendKeys.Send("{LEFT}");
                                return;
                            }
                            else
                            {
                                setGridView(desiredRow, data);
                            }
                        }

                    }
                    else
                    {
                        SumCalRow();
                        return;
                        desiredRow++;
                        dataGridView1.CurrentCell = dataGridView1[desiredColumn, desiredRow];
                        hasCellBeenEdited = false;
                        setGridView(desiredRow, data);
                    }
                }
                else if (hasCellBeenEdited && dataGridView1.CurrentCell.ColumnIndex == dataGridView1.ColumnCount - 10)
                {
                    int desiredColumn = 1;
                    int desiredRow = dataGridView1.CurrentCell.RowIndex;

                    dataGridView1.CurrentCell = dataGridView1[desiredColumn, desiredRow];
                    hasCellBeenEdited = false;
                }
                // set value
                //decimal qty1 = decimal.Parse(dataGridView1.Rows[desiredRow].Cells[colQty1].Value.ToString());
                //return;
                SumCalRow();
            }
            catch (Exception)
            {

            }
            finally
            {
                db.Dispose();
            }
        }
        void SumCalRow()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string code = dataGridView1.Rows[i].Cells[colCode].Value.ToString();
                if (code == "")
                {
                    continue;
                }
                if (_GetDocNo != null)
                {
                    if (_GetDocNo.ConfirmCheck1Date != null && _GetDocNo.ConfirmCheck2Date == null)
                    {
                        decimal qty2 = decimal.Parse(dataGridView1.Rows[i].Cells[colQty2].Value.ToString());
                        decimal cost2 = decimal.Parse(dataGridView1.Rows[i].Cells[colCostOnly].Value.ToString());
                        dataGridView1.Rows[i].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(qty2 * cost2);
                        continue;
                    }
                    else if (_GetDocNo.ConfirmCheck1Date != null && _GetDocNo.ConfirmCheck2Date != null)
                    {
                        decimal qty2 = decimal.Parse(dataGridView1.Rows[i].Cells[colQty2].Value.ToString());
                        decimal cost2 = decimal.Parse(dataGridView1.Rows[i].Cells[colCostOnly].Value.ToString());
                        dataGridView1.Rows[i].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(qty2 * cost2);
                        continue;
                    }
                }
                decimal qty1 = decimal.Parse(dataGridView1.Rows[i].Cells[colQty1].Value.ToString());
                decimal cost = decimal.Parse(dataGridView1.Rows[i].Cells[colCostOnly].Value.ToString());
                dataGridView1.Rows[i].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(qty1 * cost);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            SelectedProductPopup obj = new SelectedProductPopup(this);
            obj.ShowDialog();
        }
        /// <summary>
        /// fk pro dtl
        /// </summary>
        /// <param name="id"></param>
        public void BinddingProductChoose(int fkProductDtl)
        {
            var data = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Enable == true && w.Id == fkProductDtl);
            int currentRow = dataGridView1.CurrentRow.Index;
            dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[colCode];
            dataGridView1.Rows[currentRow].Cells[colCode].Value = data.Code;
            dataGridView1.BeginEdit(true);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRow = dataGridView1.CurrentRow.Index + 1;
            if (dataGridView1.RowCount == currentRow)
            {
                //if (_GetDocNo != null)
                //{
                //    if (_GetDocNo.ConfirmCheck1Date != null)
                //    {
                //        return;
                //    }
                //}

                SendKeys.Send("{ENTER}");
                SendKeys.Send(" ");
            }
            else
            {

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            else if (e.ColumnIndex == colSearckPro)
            {
                SelectedProductPopup obj = new SelectedProductPopup(this);
                obj.ShowDialog();
                //var code = dataGridView1.Rows[e.RowIndex].Cells[colCode].Value;
                //if (code == null)
                //{
                //    return;
                //}
            }

            //switch (e.ColumnIndex)
            //{
            //    case 2:
            //        var code = dataGridView1.Rows[e.RowIndex].Cells[colCode].Value;
            //        if (code == null)
            //        {
            //            MessageBox.Show("กรุณาคีย์บาร์โค้ดก่อน");
            //            return;
            //        }
            //        //dataGridView1.Rows[e.RowIndex].Cells[colCode].Value = "11";

            //        SelectedProductPopup obj = new SelectedProductPopup(this);
            //        obj.ShowDialog();
            //        break;
            //}
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CodeFileDLL.ExcelReport(dataGridView2, "เอกสารตรวจนับ " + textBoxDocNo.Text + " วันที่ : " + textBoxDocdate.Text, "");
        }
    }
}
