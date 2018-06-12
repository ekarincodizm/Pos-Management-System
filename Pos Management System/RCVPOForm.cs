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
    public partial class RCVPOForm : Form
    {
        POHeader _po = new POHeader();
        public RCVPOForm()
        {
            InitializeComponent();
        }

        private void RCVPOForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// หลังจากเลือก po ในรายการบน Dialog
        /// </summary>
        /// <param name="po"></param>
        public void BinddingSelected(POHeader po)
        {
            _po = po;
            textBoxPONo.Text = po.PONo;
            textBoxPODate.Text = Library.ConvertDateToThaiDate(po.CreateDate);
            textBoxPayType.Text = po.PaymentType.Name;
            textBoxReferNo.Text = po.ReferenceNo;
            textBoxExpireDate.Text = Library.ConvertDateToThaiDate(po.POExpire);
            textBoxVendorCode.Text = po.Vendor.Code;
            textBoxVendorName.Text = po.Vendor.Name;
            textBoxVendorCostType.Text = po.Vendor.POCostType.Name;
            // set summary
            textBoxDiscountKey.Text = "0.00";
            textBoxDiscountBath.Text = "0.00";
            textBoxTotalUnVat.Text = "0.00";
            textBoxTotalHasVat.Text = "0.00";
            textBoxTotalVat.Text = "0.00";
            textBoxTotalBalance.Text = "0.00";

            int i = 1;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            foreach (var item in po.PODetail.Where(w => w.Enable == true).ToList())
            {
                // จำนวนรับ default is 0 ก่อน
                decimal getcost = item.CostOnly;
                if (po.Vendor.FKPOCostType == MyConstant.POCostType.CostAndVat)
                {
                    getcost = item.CostAndVat;
                }
                decimal gift = 0;
                if (po.FKPOStatus == MyConstant.POStatus.RCVNotEnd)
                {
                    // แสดงว่าเคยทำงานรับเข้าแล้ว
                    gift = item.RcvGiftQty;
                }
                dataGridView1.Rows.Add(
                    item.Id,
                    0,
                    item.ProductDetails.Code,
                    item.ProductDetails.Products.ThaiName,
                    item.ProductDetails.ProductUnit.Name,
                    (item.Qty + item.GiftQty),
                    (item.RcvGiftQty + item.RcvQty),
                    Library.ConvertDecimalToStringForm(getcost),
                    0,
                    item.DiscountInput,
                    Library.ConvertDecimalToStringForm(item.DiscountBath),
                    0.00,
                    0.00,
                    item.GiftQty);
                i++;
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    BinddingDataGrid();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// from selected
        /// </summary>
        void BinddingDataGrid(POHeader po)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            using (SSLsEntities db = new SSLsEntities())
            {
                int i = 1;
                var data = po;
                if (data == null)
                {
                    MessageBox.Show("ไม่พบ PO นี้");
                    return;
                }
                foreach (var item in data.PODetail.Where(w => w.Enable == true).ToList())
                {
                    // จำนวนรับ default is 0 ก่อน
                    decimal getcost = item.CostOnly;
                    if (data.Vendor.FKPOCostType == MyConstant.POCostType.CostAndVat)
                    {
                        getcost = item.CostAndVat;
                    }
                    // check ว่าเคยมีการรับเข้า
                    decimal gift = 0;
                    if (data.FKPOStatus == MyConstant.POStatus.RCVNotEnd)
                    {
                        // แสดงว่าเคยทำงานรับเข้าแล้ว
                        gift = item.RcvGiftQty;
                    }
                    dataGridView1.Rows.Add(item.Id, 0, item.ProductDetails.Code, item.ProductDetails.Products.ThaiName, item.ProductDetails.ProductUnit.Name, (item.Qty + item.GiftQty), (item.RcvGiftQty + item.RcvQty), Library.ConvertDecimalToStringForm(getcost), 0, item.DiscountInput, Library.ConvertDecimalToStringForm(item.DiscountBath), 0.00, 0.00);
                    i++;
                }
                // Bindding Details
                textBoxPONo.Text = po.PONo;
                textBoxPODate.Text = Library.ConvertDateToThaiDate(po.CreateDate);
                textBoxPayType.Text = po.PaymentType.Name;
                textBoxReferNo.Text = po.ReferenceNo;
                textBoxExpireDate.Text = Library.ConvertDateToThaiDate(po.POExpire);
                textBoxVendorCode.Text = po.Vendor.Code;
                textBoxVendorName.Text = po.Vendor.Name;
                textBoxVendorCostType.Text = data.Vendor.POCostType.Name;
                // set summary
                textBoxDiscountKey.Text = "0.00";
                textBoxDiscountBath.Text = "0.00";
                textBoxTotalUnVat.Text = "0.00";
                textBoxTotalHasVat.Text = "0.00";
                textBoxTotalVat.Text = "0.00";
                textBoxTotalBalance.Text = "0.00";
            }
        }
        /// <summary>
        /// from search
        /// </summary>
        void BinddingDataGrid()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            string keyPO = textBoxPONo.Text.Trim();
            using (SSLsEntities db = new SSLsEntities())
            {
                int i = 1;
                var data = db.POHeader.Include("PaymentType").Include("POStatus").Include("PODetail.ProductDetails.Products").Include("PODetail.ProductDetails.ProductUnit").Include("Vendor.POCostType").Include("PORcv.PORcvDetails").Include("PORcv.PORcvDetails").FirstOrDefault(w => w.Enable == true && w.ApproveDate != null && (w.FKPOStatus == MyConstant.POStatus.NotRCV || w.FKPOStatus == MyConstant.POStatus.RCVNotEnd) && w.PONo == keyPO);
                if (data == null)
                {
                    MessageBox.Show("ไม่พบ PO นี้");
                    return;
                }
                _po = data;
                foreach (var item in data.PODetail.Where(w => w.Enable == true).ToList())
                {
                    // จำนวนรับ default is 0 ก่อน
                    decimal getcost = item.CostOnly;
                    if (data.Vendor.FKPOCostType == MyConstant.POCostType.CostAndVat)
                    {
                        getcost = item.CostAndVat;
                    }
                    // check ถ้ามีการรับเข้าแล้ว
                    decimal gift = 0;
                    if (data.FKPOStatus == MyConstant.POStatus.RCVNotEnd)
                    {
                        // แสดงว่าเคยทำงานรับเข้าแล้ว
                        gift = item.RcvGiftQty;
                    }
                    dataGridView1.Rows.Add(item.Id, 
                        0, 
                        item.ProductDetails.Code, 
                        item.ProductDetails.Products.ThaiName, 
                        item.ProductDetails.ProductUnit.Name, 
                        (item.Qty + item.GiftQty),
                        (item.RcvGiftQty + item.RcvQty),
                        Library.ConvertDecimalToStringForm(getcost), 
                        0, 
                        item.DiscountInput, 
                        Library.ConvertDecimalToStringForm(item.DiscountBath), 
                        0.00,
                        0.00,
                        item.GiftQty);
                    i++;
                }
                // Bindding Details
                textBoxPONo.Text = data.PONo;
                textBoxPODate.Text = Library.ConvertDateToThaiDate(data.CreateDate);
                textBoxPayType.Text = data.PaymentType.Name;
                textBoxReferNo.Text = data.ReferenceNo;
                textBoxExpireDate.Text = Library.ConvertDateToThaiDate(data.POExpire);
                textBoxVendorCode.Text = data.Vendor.Code;
                textBoxVendorName.Text = data.Vendor.Name;
                textBoxVendorCostType.Text = data.Vendor.POCostType.Name;
                // set summary
                textBoxDiscountKey.Text = "0.00";
                textBoxDiscountBath.Text = "0.00";
                textBoxTotalUnVat.Text = "0.00";
                textBoxTotalHasVat.Text = "0.00";
                textBoxTotalVat.Text = "0.00";
                textBoxTotalBalance.Text = "0.00";
            }
        }

        int colId = 0;
        int colNumber = 1;
        int colCode = 2;
        int colName = 3;
        int colUnit = 4;
        int colQty = 5;
        int colQtyRcvComplete = 6;
        int colCostPerUnit = 7;
        int colQtyRcv = 8;
        int colDiscountKey = 9;
        int colDiscountBath = 10;
        int colTotal = 11;
        int colGift = 12;
        int colGiftOnPo = 13;
        /// <summary>
        /// หลังจากคีย์เสร็จ ทำอีเว้น
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            int rowCurrent = e.RowIndex;
            Console.WriteLine(rowCurrent + "");
            //dataGridView1.Rows.Clear();
            //dataGridView1.Refresh();
            int keyRow = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[colNumber].Value.ToString()) - 1;
            //dataGridView1.Rows.InsertCopy(rowCurrent, keyRow);       
            try
            {
                switch (e.ColumnIndex)
                {
                    case 1: // key number
                            //if (row != keyRow)
                            //  SwapDatagridRows(e.RowIndex, keyRow);
                        int id = int.Parse(dataGridView1.Rows[rowCurrent].Cells[colId].Value.ToString());
                        int number = int.Parse(dataGridView1.Rows[rowCurrent].Cells[colNumber].Value.ToString());
                        string code = dataGridView1.Rows[rowCurrent].Cells[colCode].Value.ToString();
                        string name = dataGridView1.Rows[rowCurrent].Cells[colName].Value.ToString();
                        string unit = dataGridView1.Rows[rowCurrent].Cells[colUnit].Value.ToString();
                        string qty = dataGridView1.Rows[rowCurrent].Cells[colQty].Value.ToString();
                        string cost = dataGridView1.Rows[rowCurrent].Cells[colCostPerUnit].Value.ToString();
                        string qtyRcv = dataGridView1.Rows[rowCurrent].Cells[colQtyRcv].Value.ToString();
                        string qtyRcvCom = dataGridView1.Rows[rowCurrent].Cells[colQtyRcvComplete].Value.ToString();
                        string disKey = dataGridView1.Rows[rowCurrent].Cells[colDiscountKey].Value.ToString();
                        string disBath = dataGridView1.Rows[rowCurrent].Cells[colDiscountBath].Value.ToString();
                        string total = dataGridView1.Rows[rowCurrent].Cells[colTotal].Value.ToString();
                        string gift = dataGridView1.Rows[rowCurrent].Cells[colGift].Value.ToString();
                        string giftOnPo = dataGridView1.Rows[rowCurrent].Cells[colGiftOnPo].Value.ToString();
                        if (keyRow >= 0 && keyRow < dataGridView1.Rows.Count)
                        {
                            //MessageBox.Show(keyRow + "");
                            // ถ้า key เลขอะไร ให้อยู่ลำดับนั้น
                            dataGridView1.Rows.RemoveAt(rowCurrent);
                            dataGridView1.Rows.Insert(keyRow, id, number, code, name, unit, qty, qtyRcvCom, cost, qtyRcv, disKey, disBath, total, gift, giftOnPo);
                        }
                        break;
                    case 6: // ราคาทุน
                        CalculateRowDiscount();
                        CalculateRowTotal();
                        CalculateSummary();
                        DiscountBill();
                        break;
                    case 7: // จำนวนรับ
                        CalculateRowDiscount();
                        CalculateRowTotal();
                        CalculateSummary();
                        DiscountBill();
                        break;
                    case 8: // discount
                        CalculateRowDiscount();
                        CalculateRowTotal();
                        CalculateSummary();
                        DiscountBill();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("พบข้อผิดพลาด");
            }
        }
        /// <summary>
        /// คำนวนยอดสรุป
        /// </summary>
        decimal _totalUnvat = 0;
        decimal _totalHasvat = 0;
        decimal _totalVat = 0;
        decimal _totalGift = 0;
        void CalculateSummary()
        {
            decimal total = 0;
            decimal totalUnvat = 0;
            decimal totalHasVat = 0;
            decimal totalGift = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                // get total
                decimal gettotal = decimal.Parse(dataGridView1.Rows[i].Cells[colTotal].Value.ToString());
                total += gettotal;
                // check product unvat
                string code = dataGridView1.Rows[i].Cells[colCode].Value.ToString();
                var pro = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Code == code);
                if (pro.Products.FKProductVatType == MyConstant.ProductVatType.UnVat) // ถ้าเป็นสินค้า ยกเว้นภาษี
                {
                    totalUnvat += gettotal;
                }
                else if (pro.Products.FKProductVatType == MyConstant.ProductVatType.HasVat)
                {
                    totalHasVat += gettotal;
                }
                else
                {
                    totalHasVat += gettotal;
                }

                decimal gift = decimal.Parse(dataGridView1.Rows[i].Cells[colGift].Value.ToString());
                totalGift += gift;
            }
            _totalUnvat = totalUnvat;
            _totalHasvat = totalHasVat;
            _totalGift = totalGift;
            // มูลค่ายกเว้น
            textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(totalUnvat);
            if (_po.Vendor.FKPOCostType == MyConstant.POCostType.CostOnly) // ถ้าเป็นทุนเปล่า แปลว่าต้อง + vat7%
            {
                decimal totalVat = totalHasVat * MyConstant.MyVat.Vat / 100;
                textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(totalHasVat);
                textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(totalVat);
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(totalUnvat + totalHasVat + totalVat);
                _totalVat = totalVat;
            }
            else
            {
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(total);
                // ต้องถอด Vat 
                decimal removeVat = Library.CalVatFromValue(totalHasVat);
                _totalVat = removeVat;
                textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(removeVat);
                textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(totalHasVat - removeVat);
            }
        }
        /// <summary>
        /// ยอดเงิน
        /// </summary>
        void CalculateRowTotal()
        {
            decimal rcvQty = decimal.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colQtyRcv].Value.ToString());
            decimal getCost = decimal.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colCostPerUnit].Value.ToString());
            decimal totalCost = rcvQty * getCost;
            decimal discountBath = decimal.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colDiscountBath].Value.ToString());
            dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(totalCost - discountBath);
            //decimal rcvQty;
            //decimal discountBath;
            //decimal cost;

        }
        /// <summary>
        /// ส่วนลด
        /// </summary>
        void CalculateRowDiscount()
        {
            decimal rcvQty = decimal.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colQtyRcv].Value.ToString());
            decimal getCost = decimal.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colCostPerUnit].Value.ToString());
            decimal totalCost = rcvQty * getCost;
            string disKey = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colDiscountKey].Value.ToString();
            string[] dis = Library.GetCheckBathOrPercent(disKey);
            if (dis[1] == "บ") // แปลว่า เป็นบาท
            {
                dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colDiscountBath].Value = Library.ConvertDecimalToStringForm(decimal.Parse(dis[0]));
            }
            else if (dis[1] == "")
            {
                // หา %
                decimal discountBath = Library.CalPercentByValue(totalCost, decimal.Parse(dis[0]));
                dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colDiscountBath].Value = Library.ConvertDecimalToStringForm(discountBath);
            }
        }
        /// <summary>
        /// ปุ่ม PO ยังไม่ปิดสถานะ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBranch_Click(object sender, EventArgs e)
        {
            POListNotCompleteForm obj = new POListNotCompleteForm(this);
            obj.ShowDialog();
        }
        /// <summary>
        /// บันทึก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? invoiceDate = Library.ConvertTHToENDate(textBoxInvoiceDate.Text);
                if (invoiceDate == null)
                {
                    MessageBox.Show("วันที่ Invoice ไม่ถูกต้อง");
                    return;
                }
                Console.WriteLine(invoiceDate);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                return;
            }

            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    SaveCommit();
                    break;
                case DialogResult.No:
                    break;
            }
        }
        /// <summary>
        /// บันทึกรับเข้า และ เพิ่ม Stock Card Wms
        /// </summary>
        private void SaveCommit()
        {
            SSLsEntities db = new SSLsEntities();
            try
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var running = db.PORcv.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                string code = SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(running, 4);

                PORcv rcv = new PORcv();
                rcv.FKPOHeader = _po.Id;
                rcv.Code = MyConstant.PrefixForGenerateCode.RCVPOS + code;
                rcv.PORefer = "" + rcv.Code;
                rcv.Description = textBoxRemark.Text;
                rcv.Enable = true;
                rcv.CreateDate = DateTime.Now;
                rcv.CreateBy = SingletonAuthen.Instance().Id;
                rcv.UpdateDate = DateTime.Now;
                rcv.UpdateBy = SingletonAuthen.Instance().Id;
                rcv.DiscountKey = textBoxDiscountKey.Text;
                rcv.DiscountBath = decimal.Parse(textBoxDiscountBath.Text);
                rcv.TotalBUnVat = decimal.Parse(textBoxTotalUnVat.Text);
                rcv.TotalBHasVat = decimal.Parse(textBoxTotalHasVat.Text);
                rcv.TotalVat = decimal.Parse(textBoxTotalVat.Text);
                rcv.TotalGift = _totalGift;
                rcv.DriverName = textBoxDriverName.Text;
                rcv.InvoiceNo = textBoxInvoice.Text;
                rcv.InvoiceDate = (DateTime)Library.ConvertTHToENDate(textBoxInvoiceDate.Text);
                if (_idTransport == 0)
                {
                    _idTransport = MyConstant.Transport.NotChoose;
                    rcv.TransportRemark = "ไม่เลือกบริษัทขนส่ง";
                }
                rcv.FKTransport = _idTransport;
                // details
                //List<PORcvDetails> details = new List<PORcvDetails>();
                PORcvDetails detail;
                decimal rcvAndGift = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    detail = new PORcvDetails();
                    detail.Enable = true;
                    detail.CreateDate = DateTime.Now;
                    detail.CreateBy = SingletonAuthen.Instance().Id;
                    detail.UpdateDate = DateTime.Now;
                    detail.UpdateBy = SingletonAuthen.Instance().Id;
                    string proCode = dataGridView1.Rows[i].Cells[colCode].Value.ToString();
                    var product = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Code == proCode);
                    detail.FKProductDtl = product.Id;
                    detail.SequenceNumber = int.Parse(dataGridView1.Rows[i].Cells[colNumber].Value.ToString());
                    // รับเข้าในครั้งนี้
                    detail.RcvQuantity = decimal.Parse(dataGridView1.Rows[i].Cells[colQtyRcv].Value.ToString());

                    if (_po.Vendor.FKPOCostType == MyConstant.POCostType.CostOnly) // ตรวจสอบการ ยึดราคาทุน
                    {
                        detail.CurrentCost = product.CostOnly;
                    }
                    else if (_po.Vendor.FKPOCostType == MyConstant.POCostType.CostAndVat)
                    {
                        detail.CurrentCost = product.CostAndVat;
                    }
                    else
                    {
                        detail.CurrentCost = product.CostAndVat;
                    }
                    detail.QtyOnPO = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    detail.NewCost = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                    detail.DiscountKey = dataGridView1.Rows[i].Cells[colDiscountKey].Value.ToString();
                    detail.DiscountBath = decimal.Parse(dataGridView1.Rows[i].Cells[colDiscountBath].Value.ToString());
                    detail.TotalPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colTotal].Value.ToString());
                    detail.GiftOnPo = decimal.Parse(dataGridView1.Rows[i].Cells[colGiftOnPo].Value.ToString());
                    detail.GiftQty = decimal.Parse(dataGridView1.Rows[i].Cells[colGift].Value.ToString());
                    rcv.PORcvDetails.Add(detail);

                    decimal rcvComplet = decimal.Parse(dataGridView1.Rows[i].Cells[colQtyRcvComplete].Value.ToString());
                    // ยอดรับเข้าครั้งนี้ = ยอดรับเข้า+ของแถม 
                    rcvAndGift = detail.GiftQty + detail.RcvQuantity;

                    #region Manage WmsStock //////////////////////////////// ใช้ Library ManageStock แทนละ///////////////////////////////////////////////////////
                    /// จัดการ stock card
                    //int productId = product.FKProduct;
                    ///// get wmsStock
                    //var wmsStock = db.WmsStock.FirstOrDefault(w => w.Enable == true && w.FKProduct == productId);
                    //if (wmsStock != null) // ถ้าเคยตั้ง stock แล้ว
                    //{
                    //    ProductDetails productDtl = product;
                    //    /// check wmsStockDetail เอาตัวล่าสุด
                    //    WmsStockDetail wmsDtl = wmsStock.WmsStockDetail.OrderByDescending(w => w.CreateDate).FirstOrDefault(w => w.Enable == true && w.FKProductDetail == productDtl.Id);
                    //    decimal thisResultQty = 0;
                    //    if (wmsDtl != null)
                    //    {
                    //        // ถ้ามีใน wms stock detail แล้ว
                    //        // modify WmsStockDetail
                    //        WmsStockDetail wmsStockDetail = new WmsStockDetail();
                    //        wmsStockDetail.FKItemRemark = MyConstant.ItemRemark.Nornal;
                    //        wmsStockDetail.FKProductDetail = productDtl.Id;
                    //        wmsStockDetail.FKTransaction = MyConstant.WmsTransaction.RCV;
                    //        wmsStockDetail.FKWmsStock = wmsStock.Id;
                    //        wmsStockDetail.CreateDate = DateTime.Now;
                    //        wmsStockDetail.CreateBy = SingletonAuthen.Instance().Id;
                    //        wmsStockDetail.UpdateDate = DateTime.Now;
                    //        wmsStockDetail.UpdateBy = SingletonAuthen.Instance().Id;
                    //        wmsStockDetail.Enable = true;
                    //        wmsStockDetail.Description = "รับเข้าด้วย PO";
                    //        wmsStockDetail.PackSize = productDtl.PackSize;
                    //        wmsStockDetail.ActionQtyUnit = rcvAndGift;
                    //        wmsStockDetail.ActionQty = rcvAndGift * productDtl.PackSize;
                    //        /// Result จะได้ ของยกยอดมา + ของรับเข้าล่าสุด
                    //        wmsStockDetail.ResultQtyUnit = wmsDtl.ResultQtyUnit + rcvAndGift;
                    //        wmsStockDetail.ResultQty = (wmsDtl.ResultQtyUnit + rcvAndGift) * productDtl.PackSize;
                    //        thisResultQty = wmsStockDetail.ResultQty;
                    //        db.WmsStockDetail.Add(wmsStockDetail);
                    //    }
                    //    else // ถ้าไม่มีใน 
                    //    {
                    //        // add new WmsStock
                    //        WmsStockDetail wmsStockDetail = new WmsStockDetail();
                    //        wmsStockDetail.FKItemRemark = MyConstant.ItemRemark.Nornal;
                    //        wmsStockDetail.FKProductDetail = productDtl.Id;
                    //        wmsStockDetail.FKTransaction = MyConstant.WmsTransaction.RCV;
                    //        wmsStockDetail.FKWmsStock = wmsStock.Id;
                    //        wmsStockDetail.CreateDate = DateTime.Now;
                    //        wmsStockDetail.CreateBy = SingletonAuthen.Instance().Id;
                    //        wmsStockDetail.UpdateDate = DateTime.Now;
                    //        wmsStockDetail.UpdateBy = SingletonAuthen.Instance().Id;
                    //        wmsStockDetail.Enable = true;
                    //        wmsStockDetail.Description = "รับเข้าด้วย PO";
                    //        wmsStockDetail.PackSize = productDtl.PackSize;
                    //        wmsStockDetail.ActionQtyUnit = rcvAndGift;
                    //        wmsStockDetail.ActionQty = rcvAndGift * productDtl.PackSize;
                    //        /// Result จะได้ ของยกยอดมา + ของรับเข้าล่าสุด
                    //        wmsStockDetail.ResultQtyUnit =  rcvAndGift;
                    //        wmsStockDetail.ResultQty = rcvAndGift * productDtl.PackSize;
                    //        thisResultQty = wmsStockDetail.ResultQty;
                    //        db.WmsStockDetail.Add(wmsStockDetail);
                    //    }

                    //    // Update WmsStock
                    //    wmsStock.OldQty = wmsStock.CurrentQty;
                    //    wmsStock.CurrentQty = thisResultQty;
                    //    wmsStock.UpdateDate = DateTime.Now;
                    //    wmsStock.UpdateBy = SingletonAuthen.Instance().Id;
                    //    db.Entry(wmsStock).State = EntityState.Modified;

                    //}
                    //else // กรณีไม่เคยมีใน Stock
                    //{
                    //    /// ต้อง new stock ใหม่
                    //    // ต้องดัก การ initial Stock ตั้งแต่ new Product
                    //}
                    #endregion

                }
                db.PORcv.Add(rcv);
                List<PORcvDetails> rcvDtl = rcv.PORcvDetails.ToList();
                //Library.MakeValueForUpdateStockWms(rcvDtl);
                // update po status = 3 เคยมีการรับเข้าแล้ว เชคว่าครบแล้วหรือไม่
                var po = db.POHeader.SingleOrDefault(w => w.Id == _po.Id);
                decimal checkRcvQty = 0;
                decimal checkRcvGiftQty = 0;
                foreach (var item in po.PODetail.Where(w => w.Enable == true).ToList())
                {
                    // ดึง
                    var getProductRcv = rcv.PORcvDetails.FirstOrDefault(w => w.FKProductDtl == item.FKProductDetail);
                    item.RcvQty = item.RcvQty + getProductRcv.RcvQuantity;
                    item.RcvGiftQty = item.RcvGiftQty + getProductRcv.GiftQty;
                    db.Entry(item).State = EntityState.Modified;

                    checkRcvQty = checkRcvQty + item.RcvQty;
                    checkRcvGiftQty = checkRcvGiftQty + item.RcvGiftQty;
                }
                if ((checkRcvQty + checkRcvGiftQty) == (po.TotalQty + po.TotalGift))
                {
                    // ถ้ารับเข้าครบ แปลว่า Complete
                    po.FKPOStatus = MyConstant.POStatus.RCVComplete;
                }
                else
                {
                    po.FKPOStatus = MyConstant.POStatus.RCVNotEnd;
                }
                db.Entry(po).State = EntityState.Modified;

                db.SaveChanges();
                // reset form
                ResetRcv();
            }
            catch (Exception)
            {
                MessageBox.Show("ไม่ถูกต้อง พบข้อผิดพลาด กรุณาติดต่อ admin");
            }
            finally
            {
                db.Dispose();
            }
        }
        /// <summary>
        /// หลัง save เสร็จ reset ใหม่
        /// </summary>
        private void ResetRcv()
        {
            textBoxPONo.Text = "";
            textBoxPODate.Text = "";
            textBoxPayType.Text = "";
            textBoxReferNo.Text = "";
            textBoxExpireDate.Text = "";
            _po = null;
            textBoxVendorCode.Text = "";
            textBoxVendorName.Text = "";
            textBoxVendorCostType.Text = "";

            textBoxDiscountKey.Text = "0.00";
            textBoxDiscountBath.Text = "0.00";
            textBoxTotalUnVat.Text = "0.00";
            textBoxTotalHasVat.Text = "0.00";
            textBoxTotalVat.Text = "0.00";
            textBoxTotalBalance.Text = "0.00";
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }
        /// <summary>
        /// key ส่วนลดท้ายบิล
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDiscountKey_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    DiscountBill();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ส่วนลดท้ายบิล
        /// </summary>
        void DiscountBill()
        {
            string key = textBoxDiscountKey.Text.Trim();
            string[] discount = Library.GetCheckBathOrPercent(key);
            decimal discountBath = 0;
            if (discount[1] == "บ")////////////////////////////////////////// ใส่ส่วนลด บาท *ต้องหาออกมาเป็น %
            {
                discountBath = decimal.Parse(discount[0]);// เป็น บาท
                                                          // จำนวนเงินของ ยกเว้นภาษี
                decimal unvat = _totalUnvat;
                // เอาบาท คิดเป็น % 
                decimal sumValue = _totalHasvat + unvat; // เอายอด ยกเว้น+ยอดมีภาษี
                // 1200บ หา% จาก 5400 = 22.22 %
                decimal percent = discountBath * 100 / sumValue;

                decimal discountUnvat = Library.CalPercentByValue(unvat, percent);
                textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(unvat - discountUnvat);

                decimal hasvat = _totalHasvat;
                decimal discountHasvat = Library.CalPercentByValue(hasvat, percent);
                textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(hasvat - discountHasvat);
                //decimal unvatAndhasvat = decimal.Parse(textBoxTotalUnVat.Text) + decimal.Parse(textBoxTotalHasVat.Text);
                discountBath = discountUnvat + discountHasvat; // เอาลดของ มูลค่ายกเว้น + มีVat

                decimal newHasvat = hasvat - discountHasvat; // เอามาเพื่อหา vat
                decimal newvat = newHasvat * MyConstant.MyVat.Vat / 100;
                textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(newvat);

            }
            else if (discount[1] == "") ////////////////////////////////////////// ใส่ส่วนลด เปอร์เซ็น
            {
                decimal percent = decimal.Parse(discount[0]); // key percent
                // จำนวนเงินของ ยกเว้นภาษี
                decimal unvat = _totalUnvat;
                // เช่น 10% ของ 400 = 40
                decimal discountUnvat = Library.CalPercentByValue(unvat, percent);
                textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(unvat - discountUnvat);

                // จำนวนเงินของ ยกเว้นภาษี
                decimal hasvat = _totalHasvat;
                decimal discountHasvat = Library.CalPercentByValue(hasvat, percent);
                textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(hasvat - discountHasvat);
                //decimal unvatAndhasvat = decimal.Parse(textBoxTotalUnVat.Text) + decimal.Parse(textBoxTotalHasVat.Text);
                discountBath = discountUnvat + discountHasvat; // เอาลดของ มูลค่ายกเว้น + มีVat

                decimal newHasvat = hasvat - discountHasvat; // เอามาเพื่อหา vat
                decimal newvat = newHasvat * MyConstant.MyVat.Vat / 100;
                textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(newvat);
            }
            else
            {
                MessageBox.Show("ไม่ถูกต้อง " + discount[1]);
                return;
            }
            textBoxDiscountBath.Text = Library.ConvertDecimalToStringForm(discountBath);
            // สินค้ามีภาษี และยกเว้น ต้องอัพเดท โดยแบ่งส่วนลด คนละครึ่ง
            //decimal newUnvat = decimal.Parse(textBoxTotalUnVat.Text) - (discountBath / 2);
            //textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(newUnvat);
            //decimal newHasvat = decimal.Parse(textBoxTotalHasVat.Text) - (discountBath / 2);
            //textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(newHasvat);
            //decimal vat = decimal.Parse(textBoxTotalHasVat.Text) * MyConstant.MyVat.Vat / 100;
            //textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(vat);
            //textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(decimal.Parse(textBoxTotalUnVat.Text) + decimal.Parse(textBoxTotalHasVat.Text) + decimal.Parse(textBoxTotalVat.Text));
            textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(decimal.Parse(textBoxTotalUnVat.Text) + decimal.Parse(textBoxTotalHasVat.Text) + decimal.Parse(textBoxTotalVat.Text));
        }
        /// <summary>
        /// เลื่อน cursor จาก คีย์ส่วนลด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDiscountKey_Validated(object sender, EventArgs e)
        {
            Console.WriteLine("Discount Cal");
            DiscountBill();
        }
        /// <summary>
        /// เปิดบริษัทขนส่ง
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SelectedTransportPopup obj = new SelectedTransportPopup(this);
            obj.ShowDialog();
        }
        int _idTransport = 0;
        public void BinddingTransport(Transport data)
        {
            _idTransport = data.Id;
            textBoxTransportName.Text = data.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
