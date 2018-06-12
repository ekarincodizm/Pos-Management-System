using Pos_Management_System.Singleton;
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
    public partial class PORcvInvoiceForm : Form
    {
        public PORcvInvoiceForm()
        {
            InitializeComponent();
        }

        private void buttonBranch_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// เลือก PO ลิส
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            POListNotCompleteForm obj = new POListNotCompleteForm(this);
            obj.ShowDialog();
        }
        /// <summary>
        /// ส่งใบ po มา ตัวดึง PO
        /// </summary>
        /// <param name="poNo"></param>
        public void SendPONo(string poNo)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var getPo = db.POHeader.SingleOrDefault(w => w.Enable == true && w.PONo == poNo);
                // add to po list
                List<string> getPOInList = listBoxPolist.Items.OfType<string>().ToList();
                if (!getPOInList.Contains(getPo.PONo))
                {
                    listBoxPolist.Items.Add(getPo.PONo);
                    // bindding grid
                    foreach (var item in getPo.PODetail.Where(w => w.Enable == true).ToList())
                    {
                        dataGridView1.Rows.Add
                            (
                            getPo.Id,
                            item.FKProductDetail,
                            0,
                            item.ProductDetails.Code,
                            Library.ConvertDecimalToStringForm(item.RcvQty + item.RcvGiftQty), // รวมรับแล้ว
                            item.ProductDetails.Products.ThaiName,
                            item.ProductDetails.PackSize, // pz
                            item.ProductDetails.ProductUnit.Name,
                            item.Qty, // จำนวน
                            item.GiftQty, // แถม
                            0, // รับ
                            0, // รับแถม
                            Library.ConvertDecimalToStringForm(item.CostOnly), // ราคา
                            Library.ConvertDecimalToStringForm(0), // รวมสุทธิ
                            item.Id
                            );
                    }
                }

            }
        }
        /// <summary>
        /// ตัวดึง RCV
        /// </summary>
        /// <param name="code"></param>
        public void BinddingRcv(string code)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.PORcv.FirstOrDefault(w => w.Enable == true && w.Code == code);

                textBoxInvoiceNo.Text = data.InvoiceNo;
                textBoxInvoiceDate.Text = Library.ConvertDateToThaiDate(data.InvoiceDate);
                textBoxRcvNo.Text = data.Code;
                if (data.Vendor != null)
                {
                    BinddingVendor((int)data.FKVendor);
                }
                string[] listPO = data.PORefer.Split(',');
                listBoxPolist.Items.AddRange(listPO);
                textBoxDesc.Text = data.Description;

                foreach (var item in data.PORcvDetails.Where(w => w.Enable == true).OrderBy(w => w.SequenceNumber).ToList())
                {
                    dataGridView1.Rows.Add
                           (
                           item.FKPOHeader,
                           item.FKProductDtl,
                           item.SequenceNumber,
                           item.ProductDetails.Code,
                           Library.ConvertDecimalToStringForm(item.RcvComplete), // รวมรับแล้ว
                           item.ProductDetails.Products.ThaiName,
                           item.ProductDetails.PackSize, // pz
                           item.ProductDetails.ProductUnit.Name, // 
                           item.QtyOnPO, // จำนวน
                           item.GiftOnPo, // แถม
                           0, // รับ
                           0, // รับแถม
                           0, // ราคา
                           0, // รวมสุทธิ
                           item.FKPoDetails
                           );
                }
                CalSummary();
            }
        }
        private void listBoxPolist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string poInList = "";
            if (listBoxPolist.Items.Count == 1)
            {
                poInList = listBoxPolist.Items[0].ToString();
            }
            else
            {
                poInList = listBoxPolist.GetItemText(listBoxPolist.SelectedItem);
            }

            //MessageBox.Show("poInList "+ poInList);
            using (SSLsEntities db = new SSLsEntities())
            {
                try
                {
                    var getPo = db.POHeader.SingleOrDefault(w => w.Enable == true && w.PONo == poInList);
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        int idPo = int.Parse(dataGridView1.Rows[i].Cells[colIdPo].Value.ToString());
                        if (idPo == getPo.Id)
                        {
                            DataGridViewRow row = dataGridView1.Rows[i];
                            row.DefaultCellStyle.BackColor = Color.Azure;
                        }
                        else
                        {
                            DataGridViewRow row = dataGridView1.Rows[i];
                            row.DefaultCellStyle.BackColor = Color.White;
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        /// <summary>
        /// คีย์บน Grid1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int colIdPo = 0;
        int colIdProd = 1;
        int colNumber = 2;
        int colCode = 3;
        int colRcvComplete = 4;
        int colName = 5;
        int colPz = 6;
        int colUnit = 7;
        int colQty = 8;
        int colGift = 9;
        int colRcvQty = 10; // จำนวนรับ
        int colRcvGift = 11;
        int colPrice = 12;
        //int colDiscount = 13;
        int colTotal = 13;
        int colPODtlId = 14;
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            int row = e.RowIndex;
            int cell = e.ColumnIndex;
            int keyRow = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[colNumber].Value.ToString()) - 1;
            try
            {
                #region colum
                int idpo = int.Parse(dataGridView1.Rows[row].Cells[colIdPo].Value.ToString());
                int idProd = int.Parse(dataGridView1.Rows[row].Cells[colIdProd].Value.ToString());
                int number = int.Parse(dataGridView1.Rows[row].Cells[colNumber].Value.ToString());
                string code = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
                decimal rcvComplete = decimal.Parse(dataGridView1.Rows[row].Cells[colRcvComplete].Value.ToString());
                string name = dataGridView1.Rows[row].Cells[colName].Value.ToString();
                decimal pz = decimal.Parse(dataGridView1.Rows[row].Cells[colPz].Value.ToString());
                string unit = dataGridView1.Rows[row].Cells[colUnit].Value.ToString();
                decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                decimal gift = decimal.Parse(dataGridView1.Rows[row].Cells[colGift].Value.ToString());
                decimal rcvQty = decimal.Parse(dataGridView1.Rows[row].Cells[colRcvQty].Value.ToString());
                decimal rcvGift = decimal.Parse(dataGridView1.Rows[row].Cells[colRcvGift].Value.ToString());
                decimal price = decimal.Parse(dataGridView1.Rows[row].Cells[colPrice].Value.ToString());
                //string discount = dataGridView1.Rows[row].Cells[colDiscount].Value.ToString();
                decimal total = decimal.Parse(dataGridView1.Rows[row].Cells[colTotal].Value.ToString());
                int poDtlId = int.Parse(dataGridView1.Rows[row].Cells[colPODtlId].Value.ToString());
                #endregion

                switch (cell)
                {
                    case 2: // คีย์ แถว
                        if (keyRow >= 0 && keyRow < dataGridView1.Rows.Count)
                        {
                            dataGridView1.Rows.RemoveAt(row);
                            dataGridView1.Rows.Insert(keyRow, // index to insert
                             idpo,
                             idProd,
                            number,
                             code,
                             rcvComplete,
                             name,
                             pz,
                             unit,
                             qty,
                             gift,
                             rcvQty,
                             rcvGift,
                             price,
                            Library.ConvertDecimalToStringForm(rcvQty * price),
                            poDtlId
                             );
                        }
                        break;
                    case 10: // จำนวนรับ จะไปดีด กับ total ราคา/หน่วยจะ default
                        if (rcvQty + rcvComplete + rcvGift > qty+gift)
                        {
                            MessageBox.Show("ห้ามรับเกินจำนวนจริง");
                            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colRcvQty];
                            dataGridView1.BeginEdit(true);
                            return;
                        }
                        dataGridView1.Rows[row].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(rcvQty * price);
                        break;
                    case 11: // จำนวนรับ จะไปดีด กับ total ราคา/หน่วยจะ default
                        if (rcvQty + rcvComplete + rcvGift > qty + gift)
                        {
                            MessageBox.Show("ห้ามรับเกินจำนวนจริง");
                            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colRcvGift];
                            dataGridView1.BeginEdit(true);
                            return;
                        }
                        dataGridView1.Rows[row].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(rcvQty * price);
                        break;
                    //case 12: // ราคา/หน่วย จะไปดีด กับ total
                    //    dataGridView1.Rows[row].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(rcvQty * price);
                    //    break;
                    case 13: // total จะไปดีด กับ ราคา/หน่วย
                        //dataGridView1.Rows[row].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(rcvQty * price);
                        if (rcvQty <= 0)
                        {
                            dataGridView1.Rows[row].Cells[colPrice].Value = Library.ConvertDecimalToStringForm(0);
                            dataGridView1.Rows[row].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(0);
                            break;
                        }
                        else
                        {
                            decimal calNewPrice = total / rcvQty;
                            dataGridView1.Rows[row].Cells[colPrice].Value = Library.ConvertDecimalToStringForm(calNewPrice);
                            break;
                        }

                }
                CalSummary();
            }
            catch (Exception)
            {

            }

        }
        /// <summary>
        /// คำนวน total
        /// </summary>
        void CalSummary()
        {
            try
            {
                decimal totalUnVat = 0;
                decimal totalHasVat = 0;
                using (SSLsEntities db = new SSLsEntities())
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        int idProDtl = int.Parse(dataGridView1.Rows[i].Cells[colIdProd].Value.ToString());
                        var prod = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Enable == true && w.Id == idProDtl);

                        decimal total = decimal.Parse(dataGridView1.Rows[i].Cells[colTotal].Value.ToString());
                        if (prod.Products.ProductVatType.Id == MyConstant.ProductVatType.UnVat)
                        {
                            totalUnVat += total;
                        }
                        else
                        {
                            totalHasVat += total;
                        }
                    }
                }
                textBoxUnVat.Text = Library.ConvertDecimalToStringForm(totalUnVat);
                if (radioButton1.Checked == true) // รวมภาษี
                {
                    // ต้องถอดภาษี
                    decimal beforeVat = totalHasVat * 100 / MyConstant.MyVat.VatRemove;
                    textBoxHasVat.Text = Library.ConvertDecimalToStringForm(beforeVat);
                    textBoxVat.Text = Library.ConvertDecimalToStringForm(totalHasVat - beforeVat);
                    textBoxTotal.Text = Library.ConvertDecimalToStringForm(totalUnVat + totalHasVat);
                }
                else // แยกภาษี
                {
                    // ต้องคิด ภาษี
                    textBoxHasVat.Text = Library.ConvertDecimalToStringForm(totalHasVat);
                    decimal vat = totalHasVat * MyConstant.MyVat.Vat / 100;
                    textBoxVat.Text = Library.ConvertDecimalToStringForm(vat);
                    textBoxTotal.Text = Library.ConvertDecimalToStringForm(totalUnVat + totalHasVat + vat);
                }

                /// call discount อิกชั้น นึง
                decimal discountBath = decimal.Parse(textBoxDis.Text.Trim());

                if (discountBath > 0)
                {
                    decimal sumValue = decimal.Parse(textBoxHasVat.Text) + decimal.Parse(textBoxUnVat.Text);
                    decimal getPercent = discountBath * 100 / sumValue; // ได้เปอร์เซ็น
                    // ยกเว้นภาษี - ส่วนลด %
                    decimal getunvat = decimal.Parse(textBoxUnVat.Text);
                    textBoxUnVat.Text = Library.ConvertDecimalToStringForm(getunvat - ((getunvat * getPercent) / 100));

                    decimal gethasvat = decimal.Parse(textBoxHasVat.Text);
                    textBoxHasVat.Text = Library.ConvertDecimalToStringForm(gethasvat - ((gethasvat * getPercent) / 100));

                    // cal new vat 
                    decimal lastHasVat = decimal.Parse(textBoxHasVat.Text);
                    decimal vat = lastHasVat * MyConstant.MyVat.Vat / 100;
                    textBoxVat.Text = Library.ConvertDecimalToStringForm(vat);

                    textBoxTotal.Text = Library.ConvertDecimalToStringForm(decimal.Parse(textBoxUnVat.Text) + decimal.Parse(textBoxHasVat.Text) + decimal.Parse(textBoxVat.Text));
                }
            }
            catch (Exception)
            {

            }
        }

        private void PORcvInvoiceForm_Load(object sender, EventArgs e)
        {
            Singleton.SingletonProduct.Instance();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            CalSummary();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            CalSummary();
        }
        /// <summary>
        /// 1. check invoice no
        /// 2. check invoice date****
        /// 3. check vendor****
        /// 4. ต้องเลือก อย่างน้อย 1 PO เพื่อ save 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string invoice = textBoxInvoiceNo.Text.Trim();
            if (invoice == "")
            {
                MessageBox.Show("กรุณากรอก ใบกำกับภาษี/ใบส่ง");
                return;
            }
            try
            {
                DateTime? invoiceDate = Library.ConvertTHToENDate(textBoxInvoiceDate.Text);
                if (invoiceDate == null)
                {
                    MessageBox.Show("วันที่ Invoice ไม่ถูกต้อง (Need Date English United Kingdom)");
                    return;
                }
                else
                {

                }
                Console.WriteLine(invoiceDate);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                return;
            }
            //if (_VendorId == 0)
            //{
            //    MessageBox.Show("กรุณาเลือก ผู้จำหน่าย");
            //    return;
            //}

            if (listBoxPolist.Items.Count == 0)
            {
                MessageBox.Show("กรุณาเลือก อย่างน้อย 1 PO");
                return;
            }
            string poInList = listBoxPolist.Items[0].ToString();

            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    string rcvNo = textBoxRcvNo.Text;

                    #region กรณีแก้ไข
                    if (rcvNo != "")
                    {
                        using (SSLsEntities db = new SSLsEntities())
                        {
                            var rcv = db.PORcv.SingleOrDefault(w => w.Enable == true && w.Code == rcvNo);
                            rcv.InvoiceNo = textBoxInvoiceNo.Text;
                            rcv.InvoiceDate = (DateTime)Library.ConvertTHToENDate(textBoxInvoiceDate.Text);
                            rcv.FKVendor = _VendorId;
                            string[] listPO = listBoxPolist.Items.OfType<string>().ToArray();
                            rcv.PORefer = string.Join(",", listPO);
                            rcv.Description = textBoxDesc.Text;

                            rcv.UpdateDate = DateTime.Now;
                            rcv.UpdateBy = SingletonAuthen.Instance().Id;
                            rcv.DiscountKey = textBoxDis.Text;
                            rcv.DiscountBath = decimal.Parse(textBoxDis.Text);
                            rcv.TotalBUnVat = decimal.Parse(textBoxUnVat.Text);
                            rcv.TotalBHasVat = decimal.Parse(textBoxHasVat.Text);
                            rcv.TotalVat = decimal.Parse(textBoxVat.Text);
                            // details
                            List<PORcvDetails> details = new List<PORcvDetails>();
                            PORcvDetails detail;
                            for (int row = 0; row < dataGridView1.Rows.Count; row++)
                            {
                                int idpo = int.Parse(dataGridView1.Rows[row].Cells[colIdPo].Value.ToString());
                                int idProd = int.Parse(dataGridView1.Rows[row].Cells[colIdProd].Value.ToString());
                                int number = int.Parse(dataGridView1.Rows[row].Cells[colNumber].Value.ToString());
                                string barcode = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
                                decimal rcvComplete = decimal.Parse(dataGridView1.Rows[row].Cells[colRcvComplete].Value.ToString());
                                string name = dataGridView1.Rows[row].Cells[colName].Value.ToString();
                                decimal pz = decimal.Parse(dataGridView1.Rows[row].Cells[colPz].Value.ToString());
                                string unit = dataGridView1.Rows[row].Cells[colUnit].Value.ToString();
                                decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                                decimal gift = decimal.Parse(dataGridView1.Rows[row].Cells[colGift].Value.ToString());
                                decimal rcvQty = decimal.Parse(dataGridView1.Rows[row].Cells[colRcvQty].Value.ToString());
                                decimal rcvGift = decimal.Parse(dataGridView1.Rows[row].Cells[colRcvGift].Value.ToString());
                                decimal price = decimal.Parse(dataGridView1.Rows[row].Cells[colPrice].Value.ToString());
                                decimal total = decimal.Parse(dataGridView1.Rows[row].Cells[colTotal].Value.ToString());
                                int poDtlId = int.Parse(dataGridView1.Rows[row].Cells[colPODtlId].Value.ToString());
                                // ถ้ามีแล้ว ก็อัพเดท
                                var getDtlRcv = db.PORcvDetails.SingleOrDefault(w => w.Enable == true && w.FKPoDetails == poDtlId);
                                if (getDtlRcv != null)
                                {
                                    getDtlRcv.FKProductDtl = idProd;
                                    getDtlRcv.SequenceNumber = number;
                                    getDtlRcv.RcvQuantity = rcvQty;
                                    getDtlRcv.GiftQty = rcvGift;
                                    getDtlRcv.NewCost = price;
                                    getDtlRcv.CurrentCost = price;
                                    getDtlRcv.TotalPrice = total;
                                    if ((rcvGift + rcvQty + rcvComplete) == qty + gift)
                                    {
                                        getDtlRcv.IsComplete = true;
                                    }
                                    else
                                    {
                                        getDtlRcv.IsComplete = false;
                                    }
                                    getDtlRcv.RcvComplete = rcvGift + rcvQty + rcvComplete;
                                    db.Entry(getDtlRcv).State = EntityState.Modified;
                                }
                                else // ถ้าไมีมี
                                {
                                    detail = new PORcvDetails();
                                    detail.FKPORcv = rcv.Id;
                                    detail.CreateDate = DateTime.Now;
                                    detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                                    detail.UpdateDate = DateTime.Now;
                                    detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                                    detail.Description = "-";
                                    detail.Enable = true;
                                    detail.FKProductDtl = idProd;
                                    detail.SequenceNumber = number;
                                    detail.QtyOnPO = qty;
                                    detail.RcvQuantity = rcvQty;
                                    detail.CurrentCost = price;
                                    detail.NewCost = price;
                                    detail.DiscountKey = 0 + "";
                                    detail.DiscountBath = 0;
                                    detail.TotalPrice = total;
                                    detail.GiftOnPo = gift;
                                    detail.GiftQty = rcvGift; // รับเข้าของแถม
                                    detail.FKPoDetails = poDtlId;
                                    if ((rcvGift + rcvQty + rcvComplete) == qty + gift)
                                    {
                                        detail.IsComplete = true;
                                    }
                                    else
                                    {
                                        detail.IsComplete = false;
                                    }
                                    detail.RcvComplete = detail.RcvComplete + rcvGift + rcvQty;
                                    detail.FKPOHeader = idpo;
                                    //if (rcvGift + rcvQty > 0)
                                    //{
                                    //    details.Add(detail);                                    
                                    //}
                                    db.PORcvDetails.Add(detail);
                                }
                            }
                            db.SaveChanges();
                        }
                        return;
                    }
                    #endregion
                    ///////////////////////////////////////////////////////////////////////////////////////////////////
                    #region กรณีเพิ่ม
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        int currentYear = DateTime.Now.Year;
                        int currentMonth = DateTime.Now.Month;
                        var running = db.PORcv.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                        string code = SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(running, 4);

                        //string poInList = listBoxPolist.Items[0].ToString();
                        var getPo = db.POHeader.SingleOrDefault(w => w.Enable == true && w.PONo == poInList);
                        PORcv rcv = new PORcv();
                        rcv.FKPOHeader = getPo.Id;
                        rcv.Code = MyConstant.PrefixForGenerateCode.RCVPOS + code;
                        rcv.Description = textBoxDesc.Text;
                        rcv.Enable = true;
                        rcv.CreateDate = DateTime.Now;
                        rcv.CreateBy = SingletonAuthen.Instance().Id;
                        rcv.UpdateDate = DateTime.Now;
                        rcv.UpdateBy = SingletonAuthen.Instance().Id;
                        rcv.DiscountKey = textBoxDis.Text;
                        rcv.DiscountBath = decimal.Parse(textBoxDis.Text);
                        rcv.TotalBUnVat = decimal.Parse(textBoxUnVat.Text);
                        rcv.TotalBHasVat = decimal.Parse(textBoxHasVat.Text);
                        rcv.TotalVat = decimal.Parse(textBoxVat.Text);
                        rcv.TotalGift = 0;
                        rcv.InvoiceNo = textBoxInvoiceNo.Text;
                        rcv.InvoiceDate = (DateTime)Library.ConvertTHToENDate(textBoxInvoiceDate.Text);
                        string[] listPO = listBoxPolist.Items.OfType<string>().ToArray();
                        rcv.PORefer = string.Join(",", listPO);
                        rcv.FKTransport = MyConstant.Transport.NotChoose;
                        rcv.PrintNumber = 0;
                        rcv.FKVendor = _VendorId;
                        if (_VendorId == 0)
                        {
                            rcv.FKVendor = null;
                        }

                        // details
                        List<PORcvDetails> details = new List<PORcvDetails>();
                        PORcvDetails detail;
                        for (int row = 0; row < dataGridView1.Rows.Count; row++)
                        {
                            detail = new PORcvDetails();
                            int idpo = int.Parse(dataGridView1.Rows[row].Cells[colIdPo].Value.ToString());
                            int idProd = int.Parse(dataGridView1.Rows[row].Cells[colIdProd].Value.ToString());
                            int number = int.Parse(dataGridView1.Rows[row].Cells[colNumber].Value.ToString());
                            string barcode = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
                            decimal rcvComplete = decimal.Parse(dataGridView1.Rows[row].Cells[colRcvComplete].Value.ToString());
                            string name = dataGridView1.Rows[row].Cells[colName].Value.ToString();
                            decimal pz = decimal.Parse(dataGridView1.Rows[row].Cells[colPz].Value.ToString());
                            string unit = dataGridView1.Rows[row].Cells[colUnit].Value.ToString();
                            decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                            decimal gift = decimal.Parse(dataGridView1.Rows[row].Cells[colGift].Value.ToString());
                            decimal rcvQty = decimal.Parse(dataGridView1.Rows[row].Cells[colRcvQty].Value.ToString());
                            decimal rcvGift = decimal.Parse(dataGridView1.Rows[row].Cells[colRcvGift].Value.ToString());
                            decimal price = decimal.Parse(dataGridView1.Rows[row].Cells[colPrice].Value.ToString());
                            decimal total = decimal.Parse(dataGridView1.Rows[row].Cells[colTotal].Value.ToString());
                            int poDtlId = int.Parse(dataGridView1.Rows[row].Cells[colPODtlId].Value.ToString());

                            detail.CreateDate = DateTime.Now;
                            detail.CreateBy = SingletonAuthen.Instance().Id;
                            detail.UpdateDate = DateTime.Now;
                            detail.UpdateBy = SingletonAuthen.Instance().Id;
                            detail.Description = "-";
                            detail.Enable = true;
                            detail.FKProductDtl = idProd;
                            detail.SequenceNumber = number;
                            detail.QtyOnPO = qty;
                            detail.RcvQuantity = rcvQty;
                            detail.CurrentCost = price;
                            detail.NewCost = price;
                            detail.DiscountKey = 0 + "";
                            detail.DiscountBath = 0;
                            detail.TotalPrice = total;
                            detail.GiftOnPo = gift;
                            detail.GiftQty = rcvGift; // รับเข้าของแถม
                            detail.FKPoDetails = poDtlId;
                            if ((rcvGift + rcvQty + rcvComplete) == qty)
                            {
                                detail.IsComplete = true;
                            }
                            else
                            {
                                detail.IsComplete = false;
                            }
                            detail.RcvComplete = rcvGift + rcvQty;
                            detail.FKPOHeader = idpo;
                            details.Add(detail);
                            //if (rcvGift + rcvQty > 0)
                            //{
                            //    details.Add(detail);                            
                            //}
                        }
                        rcv.PORcvDetails = details;
                        db.PORcv.Add(rcv);
                        db.SaveChanges();
                        MessageBox.Show("บันทึก " + rcv.Code + "เรียบร้อย");
                    }
                    #endregion

                    break;
                case DialogResult.No:
                    break;
            }
        }
        /// <summary>
        /// บันทึกลง PORcv อาจ save เพื่อจอง
        /// </summary>
        private void SaveToPORcv()
        {
            //using (SSLsEntities db = new SSLsEntities())
            //{
            //    int currentYear = DateTime.Now.Year;
            //    int currentMonth = DateTime.Now.Month;
            //    var running = db.PORcv.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
            //    string code = SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(running, 4);

            //    string poInList = listBoxPolist.Items[0].ToString();
            //    var getPo = db.POHeader.SingleOrDefault(w => w.Enable == true && w.PONo == poInList);
            //    PORcv rcv = new PORcv();
            //    rcv.FKPOHeader = getPo.Id;
            //    rcv.Code = MyConstant.PrefixForGenerateCode.RCVPOS + code;
            //    rcv.PORefer = "" + rcv.Code;
            //    rcv.Description = textBoxDesc.Text;
            //    rcv.Enable = true;
            //    rcv.CreateDate = DateTime.Now;
            //    rcv.CreateBy = SingletonAuthen.Instance().Id;
            //    rcv.UpdateDate = DateTime.Now;
            //    rcv.UpdateBy = SingletonAuthen.Instance().Id;
            //    rcv.DiscountKey = textBoxDis.Text;
            //    rcv.DiscountBath = decimal.Parse(textBoxDis.Text);
            //    rcv.TotalBUnVat = decimal.Parse(textBoxUnVat.Text);
            //    rcv.TotalBHasVat = decimal.Parse(textBoxHasVat.Text);
            //    rcv.TotalVat = decimal.Parse(textBoxVat.Text);
            //    rcv.TotalGift = 0;
            //    //rcv.DriverName = textBoxDriverName.Text;
            //    rcv.InvoiceNo = textBoxInvoiceNo.Text;
            //    rcv.InvoiceDate = (DateTime)Library.ConvertTHToENDate(textBoxInvoiceDate.Text);
            //    rcv.PORefer = string.Join(",", listBoxPolist.Items);
            //    MessageBox.Show("บันทึก " + rcv.Code + "เรียบร้อย");
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectedVendorPopup obj = new SelectedVendorPopup(this);
            obj.ShowDialog();
        }
        int _VendorId = 0;
        public void BinddingVendor(int id)
        {
            _VendorId = id;
            List<Vendor> vendors = SingletonVender.Instance().Vendors;
            var data = vendors.SingleOrDefault(w => w.Id == id);
            textBoxVendorCode.Text = data.Code;
            textBoxVendorName.Text = data.Name;
        }
        /// <summary>
        /// Enter vedor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxVendorCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string code = textBoxVendorCode.Text.Trim();
                var vendor = Singleton.SingletonVender.Instance().Vendors.FirstOrDefault(w => w.Enable == true && w.Code == code);
                if (vendor == null)
                {
                    _VendorId = 0;
                    textBoxVendorCode.Text = "";
                    textBoxVendorName.Text = "";
                }
                else
                {
                    _VendorId = vendor.Id;

                    textBoxVendorCode.Text = vendor.Code;
                    textBoxVendorName.Text = vendor.Name;
                }
            }
        }
        /// <summary>
        /// Enter InvoiceNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxInvoiceNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //string code = textBoxVendorCode.Text.Trim();
                //var vendor = Singleton.SingletonVender.Instance().Vendors.FirstOrDefault(w => w.Enable == true && w.Code == code);
                //if (vendor == null)
                //{
                //    _VendorId = 0;
                //    textBoxVendorCode.Text = "";
                //    textBoxVendorName.Text = "";
                //}
                //else
                //{
                //    _VendorId = vendor.Id;

                //    textBoxVendorCode.Text = vendor.Code;
                //    textBoxVendorName.Text = vendor.Name;
                //}
            }
        }

        private void textBoxDis_Validated(object sender, EventArgs e)
        {
            //CalSummary();
        }

        private void textBoxDis_Leave(object sender, EventArgs e)
        {
            CalSummary();
        }

        private void textBoxDis_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    CalSummary();
                    break;
                default:
                    break;
            }
        }

        private void listBoxPolist_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Delete)
            {
                string poInList = "";
                if (listBoxPolist.Items.Count == 1)
                {
                    poInList = listBoxPolist.Items[0].ToString();
                }
                else
                {
                    poInList = listBoxPolist.GetItemText(listBoxPolist.SelectedItem);
                }
                using (SSLsEntities db = new SSLsEntities())
                {
                    var getPo = db.POHeader.SingleOrDefault(w => w.Enable == true && w.PONo == poInList);

                    dataGridView1.Rows.Cast<DataGridViewRow>()
    .Where(row => (int)row.Cells[colIdPo].Value == getPo.Id)
    .ToList().ForEach(row =>
    {
        dataGridView1.Rows.Remove(row);
    });
                    listBoxPolist.Items.Remove(poInList);
                }
            }
        }
        /// <summary>
        /// double click cell code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colCode)
            {
                int idProDtl = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[colIdProd].Value.ToString());
                PopupProductBaseForm obj = new PopupProductBaseForm(this, idProDtl);
                obj.ShowDialog();

            }
        }
        /// <summary>
        /// product id dtl
        /// </summary>
        /// <param name="id"></param>
        public void BinddingProductDtChoose(int id)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var getNewProduct = db.ProductDetails.SingleOrDefault(w => w.Id == id);
                int row = dataGridView1.CurrentRow.Index;
                //int idpo = int.Parse(dataGridView1.Rows[row].Cells[colIdPo].Value.ToString());
                //int idProd = int.Parse(dataGridView1.Rows[row].Cells[colIdProd].Value.ToString());
                dataGridView1.Rows[row].Cells[colIdProd].Value = id;
                //int number = int.Parse(dataGridView1.Rows[row].Cells[colNumber].Value.ToString());
                //string code = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
                dataGridView1.Rows[row].Cells[colCode].Value = getNewProduct.Code;
                decimal pz = decimal.Parse(dataGridView1.Rows[row].Cells[colPz].Value.ToString());
                decimal rcvComplete = decimal.Parse(dataGridView1.Rows[row].Cells[colRcvComplete].Value.ToString());
                decimal pieceRcvComplete = rcvComplete * pz;
                dataGridView1.Rows[row].Cells[colRcvComplete].Value = Library.ConvertDecimalToStringForm(pieceRcvComplete / getNewProduct.PackSize);

                decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                decimal piece = qty * pz;

                dataGridView1.Rows[row].Cells[colPz].Value = getNewProduct.PackSize;
                //string unit = dataGridView1.Rows[row].Cells[colUnit].Value.ToString();
                dataGridView1.Rows[row].Cells[colUnit].Value = getNewProduct.ProductUnit.Name;

                dataGridView1.Rows[row].Cells[colQty].Value = Library.ConvertDecimalToStringForm(piece / getNewProduct.PackSize);

                decimal gift = decimal.Parse(dataGridView1.Rows[row].Cells[colGift].Value.ToString());

                dataGridView1.Rows[row].Cells[colGift].Value = Library.ConvertDecimalToStringForm((gift * pz) / getNewProduct.PackSize);

                decimal rcvQty = decimal.Parse(dataGridView1.Rows[row].Cells[colRcvQty].Value.ToString());
                dataGridView1.Rows[row].Cells[colRcvQty].Value = Library.ConvertDecimalToStringForm(0);

                decimal rcvGift = decimal.Parse(dataGridView1.Rows[row].Cells[colRcvGift].Value.ToString());
                dataGridView1.Rows[row].Cells[colRcvGift].Value = Library.ConvertDecimalToStringForm(0);

                //decimal price = decimal.Parse(dataGridView1.Rows[row].Cells[colPrice].Value.ToString());
                dataGridView1.Rows[row].Cells[colPrice].Value = Library.ConvertDecimalToStringForm(0);

                //decimal total = decimal.Parse(dataGridView1.Rows[row].Cells[colTotal].Value.ToString());
                qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                dataGridView1.Rows[row].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(0);

                int poDtlId = int.Parse(dataGridView1.Rows[row].Cells[colPODtlId].Value.ToString());

            }
        }
        /// <summary>
        /// get ใบรับเข้า มา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            PopupPORcvList obj = new PopupPORcvList(this);
            obj.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                /// รหัสใบออเดอร์
                string code = textBoxRcvNo.Text;
                if (code == "")
                {
                    MessageBox.Show("ไม่พบเลขที่รับเข้า");
                    return;
                }
                frmMainReport mr = new frmMainReport(this, code);
                mr.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("จำนวนเอกสารผิดพลาด");
            }
        }
    }
}
