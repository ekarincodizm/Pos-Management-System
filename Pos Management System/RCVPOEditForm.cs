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
    public partial class RCVPOEditForm : Form
    {
        public RCVPOEditForm()
        {
            InitializeComponent();
        }

        public RCVPOEditForm(RCVPODetailForm rCVPODetailForm, string code)
        {
            InitializeComponent();
            this.rCVPODetailForm = rCVPODetailForm;
            this.code = code;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonBranch_Click(object sender, EventArgs e)
        {
            POListNotCompleteForm obj = new POListNotCompleteForm(this);
            obj.ShowDialog();
        }
        POHeader _po = new POHeader();
        private RCVPODetailForm rCVPODetailForm;
        private string code;
        /// <summary>
        /// หลังจาก เลือก PO มา
        /// </summary>
        /// <param name="po"></param>
        public void BinddingSelected(POHeader po)
        {
            _po = po;
            textBoxPONo.Text = po.PONo;
            // set summary
            textBoxDiscountKey.Text = "0.00";
            textBoxDiscountBath.Text = "0.00";
            textBoxTotalUnVat.Text = "0.00";
            textBoxTotalHasVat.Text = "0.00";
            textBoxTotalVat.Text = "0.00";
            textBoxTotalBalance.Text = "0.00";

            int i = 1;
            //dataGridView1.Rows.Clear();
            //dataGridView1.Refresh();
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
            CalculateSummary();
        }
        /// <summary>
        /// bindding data ลงไปก่อน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RCVPOEditForm_Load(object sender, EventArgs e)
        {
            int i = 1;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            string rcvcodeChoose = code;

            using (SSLsEntities db = new SSLsEntities())
            {
                var dataInRcvNo = db.PORcv.SingleOrDefault(w => w.Enable == true && w.Code == rcvcodeChoose);
                textBoxDesc.Text = dataInRcvNo.Description;
                textBoxRcvNo.Text = code;
                textBoxDiscountKey.Text = dataInRcvNo.DiscountKey;
                textBoxDiscountBath.Text = Library.ConvertDecimalToStringForm(dataInRcvNo.DiscountBath);
                foreach (var item in dataInRcvNo.PORcvDetails.Where(w => w.Enable == true).OrderBy(w => w.SequenceNumber).ToList())
                {
                    // จำนวนรับ default is 0 ก่อน
                    //decimal getcost = item.CostOnly;
                    //if (po.Vendor.FKPOCostType == MyConstant.POCostType.CostAndVat)
                    //{
                    //    getcost = item.CostAndVat;
                    //}
                    //decimal gift = 0;
                    //if (po.FKPOStatus == MyConstant.POStatus.RCVNotEnd)
                    //{
                    //    // แสดงว่าเคยทำงานรับเข้าแล้ว
                    //    gift = item.RcvGiftQty;
                    //}
                    dataGridView1.Rows.Add(
                        0,
                        item.SequenceNumber,
                        item.ProductDetails.Code,
                        item.ProductDetails.Products.ThaiName,
                        item.ProductDetails.ProductUnit.Name,
                        Library.ConvertDecimalToStringForm(item.QtyOnPO),
                        (item.RcvQuantity + item.GiftQty),
                        Library.ConvertDecimalToStringForm(item.CurrentCost),
                        (item.RcvQuantity + item.GiftQty),
                        item.DiscountKey,
                        Library.ConvertDecimalToStringForm(item.DiscountBath),
                        Library.ConvertDecimalToStringForm(item.TotalPrice),
                        0.00,
                        item.GiftOnPo);
                    i++;
                }
            }
            CalculateSummary();
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {

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
                    case 7: // ราคาทุน
                        CalculateRowDiscount();
                        CalculateRowTotal();
                        CalculateSummary();
                        DiscountBill();
                        break;
                    case 8: // จำนวนรับ
                        CalculateRowDiscount();
                        CalculateRowTotal();
                        CalculateSummary();
                        DiscountBill();
                        break;
                    case 9: // discount
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
                else
                {
                    totalHasVat += gettotal;
                    //if (radioButton1.Checked == true) // ราคาไม่รวมภาษี ต้องไป + vat
                    //{
                    //    totalHasVat += gettotal;
                    //}
                    //else
                    //{
                    //    totalHasVat += gettotal;
                    //}
                }
                //else if (pro.Products.FKProductVatType == MyConstant.ProductVatType.HasVat)
                //{
                //    totalHasVat += gettotal;
                //}
                //else
                //{
                //    totalHasVat += gettotal;
                //}

                decimal gift = decimal.Parse(dataGridView1.Rows[i].Cells[colGift].Value.ToString());
                totalGift += gift;
            }
            _totalUnvat = totalUnvat;
            _totalHasvat = totalHasVat;
            _totalGift = totalGift;
            // มูลค่ายกเว้น
            textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(totalUnvat);
            //if (_po.Vendor.FKPOCostType == MyConstant.POCostType.CostOnly) // ถ้าเป็นทุนเปล่า แปลว่าต้อง + vat7%
            //{


            if (radioButton1.Checked == true) // ราคาไม่รวมภาษี ต้องไป + vat
            {
                decimal totalVat = totalHasVat * MyConstant.MyVat.Vat / 100;
                textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(totalHasVat);
                textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(totalVat);
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(totalUnvat + totalHasVat + totalVat);
                _totalVat = totalVat;
            }
            else
            {
                decimal totalVat = totalHasVat * MyConstant.MyVat.Vat / MyConstant.MyVat.VatRemove;
                textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(totalHasVat - totalVat);
                textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(totalVat);
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(totalUnvat + totalHasVat);
                _totalVat = totalVat;
            }

            //}
            //else
            //{
            //    textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(total);
            //    // ต้องถอด Vat 
            //    decimal removeVat = Library.CalVatFromValue(totalHasVat);
            //    _totalVat = removeVat;
            //    textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(removeVat);
            //    textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(totalHasVat - removeVat);
            //}
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
        /// click ราคา ไม่รวมภาษี
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            CalculateSummary();
        }
        /// <summary>
        /// click ราคา รวมภาษีแล้ว
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            CalculateSummary();
        }

        private void button2_Click(object sender, EventArgs e)
        {
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

        private void SaveCommit()
        {
            SSLsEntities db = new SSLsEntities();
            try
            {
                var rcvOld = db.PORcv.SingleOrDefault(w => w.Enable == true && w.Code == textBoxRcvNo.Text);

                rcvOld.PORefer = rcvOld.PORefer + "," + textBoxPONo.Text;
                rcvOld.Description = textBoxDesc.Text;

                rcvOld.UpdateDate = DateTime.Now;
                rcvOld.UpdateBy = SingletonAuthen.Instance().Id;
                rcvOld.DiscountKey = textBoxDiscountKey.Text;
                rcvOld.DiscountBath = decimal.Parse(textBoxDiscountBath.Text);
                rcvOld.TotalBUnVat = decimal.Parse(textBoxTotalUnVat.Text);
                rcvOld.TotalBHasVat = decimal.Parse(textBoxTotalHasVat.Text);
                rcvOld.TotalVat = decimal.Parse(textBoxTotalVat.Text);
                rcvOld.TotalGift = _totalGift;

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
                    detail.SequenceNumber = int.Parse(dataGridView1.Rows[i].Cells[colNumber].Value.ToString());
                    int idPO = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    // ยอดรับเข้าครั้งนี้ = ยอดรับเข้า+ของแถม 
                    rcvAndGift = detail.GiftQty + detail.RcvQuantity;
                    if (idPO != 0)
                    {
                        rcvOld.PORcvDetails.Add(detail);

                        var getProductRcv = db.PODetail.FirstOrDefault(w => w.Id == idPO);
                        getProductRcv.RcvQty = detail.GiftQty;
                        getProductRcv.RcvGiftQty = rcvAndGift;
                        db.Entry(getProductRcv).State = EntityState.Modified;
                    }

                    decimal rcvComplet = decimal.Parse(dataGridView1.Rows[i].Cells[colQtyRcvComplete].Value.ToString());
                }
                db.Entry(rcvOld).State = EntityState.Modified;

                //List<PORcvDetails> rcvDtl = rcv.PORcvDetails.ToList();
                //Library.MakeValueForUpdateStockWms(rcvDtl);
                // update po status = 3 เคยมีการรับเข้าแล้ว เชคว่าครบแล้วหรือไม่
                var po = db.POHeader.SingleOrDefault(w => w.Id == _po.Id);
                decimal checkRcvQty = 0;
                decimal checkRcvGiftQty = 0;
                //foreach (var item in po.PODetail.Where(w => w.Enable == true).ToList())
                //{
                //    // ดึง
                //    var getProductRcv = rcvOld.PORcvDetails.FirstOrDefault(w => w.FKProductDtl == item.FKProductDetail);
                //    item.RcvQty = item.RcvQty + getProductRcv.RcvQuantity;
                //    item.RcvGiftQty = item.RcvGiftQty + getProductRcv.GiftQty;
                //    db.Entry(item).State = EntityState.Modified;

                //    checkRcvQty = checkRcvQty + item.RcvQty;
                //    checkRcvGiftQty = checkRcvGiftQty + item.RcvGiftQty;
                //}
                //if ((checkRcvQty + checkRcvGiftQty) == (po.TotalQty + po.TotalGift))
                //{
                //    // ถ้ารับเข้าครบ แปลว่า Complete
                //    po.FKPOStatus = MyConstant.POStatus.RCVComplete;
                //}
                //else
                //{
                //    po.FKPOStatus = MyConstant.POStatus.RCVNotEnd;
                //}

                po.FKPOStatus = MyConstant.POStatus.RCVNotEnd;
                db.Entry(po).State = EntityState.Modified;
                db.SaveChanges();
                // reset form

                frmMainReport mr = new frmMainReport(this, textBoxRcvNo.Text);
                mr.Show();

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
    }
}
