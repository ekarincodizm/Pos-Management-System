using Pos_Management_System.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class SaleOrderWarehouseForm : Form
    {
        public SaleOrderWarehouseForm()
        {
            InitializeComponent();
        }

        private void SaleOrderWarehouseForm_Load(object sender, EventArgs e)
        {
            Singleton.SingletonBranch.Instance();
            Singleton.SingletonDebtor.Instance();
            Singleton.SingletonPayment.Instance();
            Singleton.SingletonProduct.Instance();
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var count = db.SaleOrderWarehouse.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                string code = SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(count, 4);
                textBoxCode.Text = MyConstant.PrefixForGenerateCode.SaleOrderFromWarehouse + code;
            }
            textBoxOrderDate.Text = Library.ConvertDateToThaiDate(DateTime.Now);
            dataGridView1.Rows.Add(1);

            // default branch
            var branch = SingletonAuthen.Instance().MyBranch;
            BinddingBranchSelected(branch);

            // default payment
            var payment = SingletonPayment.Instance().PaymentTypes.SingleOrDefault(w => w.Id == MyConstant.PaymentType.CREDIT);
            BinddingPaytmentSelected(payment);

            // default delivery
            var delvery = SingletonPriority1.Instance().DeliveryTypes.SingleOrDefault(w => w.Id == MyConstant.DeliveryType.DeliverService);
            BinddingDeliverSelected(delvery);
        }
        /// <summary>
        /// ปุ่มเลือก สาขา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBranch_Click(object sender, EventArgs e)
        {
            var branch = Singleton.SingletonBranch.Instance().Branchs;
            SelectedBranchPopup obj = new SelectedBranchPopup(this, branch);
            obj.ShowDialog();

        }
        /// <summary>
        /// Bindding สาขาที่เลือก
        /// </summary>
        int _BranchId;
        public void BinddingBranchSelected(Branch send)
        {
            _BranchId = send.Id;
            textBoxBranchCode.Text = send.Code;
            textBoxBranchName.Text = send.Name;
            textBoxBranchNo.Text = send.BranchNo;
            textBoxBranchDesc.Text = send.Description;
        }
        /// <summary>
        /// ปุ่มเลือกลูกหนี้
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDebtor_Click(object sender, EventArgs e)
        {
            var data = Singleton.SingletonDebtor.Instance().Debtors;
            SelectedDebtorPopup obj = new SelectedDebtorPopup(this, data);
            obj.ShowDialog();
        }
        /// <summary>
        /// bindding ลูกหนี้
        /// </summary>
        /// <param name="send"></param>
        int _DebtorId = 0;
        public void BinddingDebtorSelected(Debtor send)
        {
            _DebtorId = send.Id;
            textBoxDebtorCode.Text = send.Code;
            textBoxDebtorName.Text = send.Name;
            textBoxDebtorAddress.Text = send.Address;
            textBoxDebtorTel.Text = send.Tel;
            textBoxDebtorFax.Text = send.Fax;
            textBoxDebtorMail.Text = send.Email;
            textBoxDebtorTax.Text = send.TaxNo;
        }
        private void textBoxDebtorCode_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string debtorCode = textBoxDebtorCode.Text.Trim();
                    Debtor getDeb = SingletonDebtor.Instance().Debtors.SingleOrDefault(w => w.Enable == true && w.Code == debtorCode);
                    if (getDeb == null)
                    {
                        _DebtorId = 0;
                        textBoxDebtorCode.Text = "";
                        textBoxDebtorName.Text = "";
                        textBoxDebtorAddress.Text = "";
                        textBoxDebtorTel.Text = "";
                        textBoxDebtorFax.Text = "";
                        textBoxDebtorMail.Text = "";
                        textBoxDebtorTax.Text = "";
                    }
                    else
                    {
                        BinddingDebtorSelected(getDeb);
                    }
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// Line Number datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// ปุ่มเลือก สมาชิก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMember_Click(object sender, EventArgs e)
        {
            var data = Singleton.SingletonMember.Instance().Members;
            SelectedMemberPopup obj = new SelectedMemberPopup(this, data);
            obj.ShowDialog();
        }
        /// <summary>
        /// Bindding สมาชิก
        /// </summary>
        /// <param name="send"></param>
        int _MemberId = 0;
        public void BinddingMemberSelected(Member send)
        {
            _MemberId = send.Id;
            textBoxMemberCode.Text = send.Code;
            textBoxMemberName.Text = send.Name;
        }
        private void textBoxMemberCode_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string memberCode = textBoxMemberCode.Text.Trim();
                    Member mem = SingletonMember.Instance().Members.SingleOrDefault(w => w.Enable == true && w.Code == memberCode);
                    if (mem == null)
                    {
                        _MemberId = 0;
                        textBoxMemberCode.Text = "";
                        textBoxMemberName.Text = "";
                    }
                    else
                    {
                        BinddingMemberSelected(mem);
                    }
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// ปุ่มค้นหา การชำระ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPayment_Click(object sender, EventArgs e)
        {
            var data = Singleton.SingletonPayment.Instance().PaymentTypes;
            SelectedPaymentTypePopup obj = new SelectedPaymentTypePopup(this, data);
            obj.ShowDialog();
        }
        /// <summary>
        /// Bindding Paytment
        /// </summary>
        /// <param name="send"></param>
        int _PaymentId;
        public void BinddingPaytmentSelected(PaymentType send)
        {
            _PaymentId = send.Id;
            textBoxPaymentCode.Text = send.Code;
            textBoxPaymentName.Text = send.Name;
        }
        /// <summary>
        /// ปุ่ม วิธี ขนส่ง
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelivery_Click(object sender, EventArgs e)
        {
            var data = Singleton.SingletonPriority1.Instance().DeliveryTypes;
            SelectedDeliveryPopup obj = new SelectedDeliveryPopup(this, data);
            obj.ShowDialog();
        }
        /// <summary>
        /// Bindding Delivery
        /// </summary>
        /// <param name="send"></param>
        int _DeliverId;
        public void BinddingDeliverSelected(DeliveryType send)
        {
            _DeliverId = send.Id;
            textBoxDeliverCode.Text = send.Code;
            textBoxDeliverName.Text = send.Name;
        }
        /// <summary>
        /// สหกรณ์บอก ไม่ต้องใช้
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDiscountInput_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string disInput = textBoxDiscountInput.Text.Trim();
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        // Is Auto Calculate Working Now
                        var checkBathOrPercent = Library.GetCheckBathOrPercent(disInput);
                        //Console.WriteLine(checkBathOrPercent[0] + " " + checkBathOrPercent[1]);
                        decimal value = decimal.Parse(checkBathOrPercent[0]);
                        string bath = checkBathOrPercent[1];
                        if (bath == "บ")
                        {
                            // ถ้าเป็น Bath ก็แสดงตรงๆ เลย
                            textBoxDiscountBath.Text = Library.ConvertDecimalToStringForm(value);
                            textBoxDisRemark.Select();
                            textBoxDisRemark.SelectAll();
                        }
                        else
                        {
                            textBoxDiscountBath.Text = "0.00";
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("พบข้อผิดพลาด " + ex.ToString());
            }

        }
        int colId = 0;
        int colCode = 1;
        int colSearch = 2;
        int colName = 3;
        int colUnit = 4;
        int colQty = 5;
        int colPricePerUnit = 6;
        int colDiscount = 7;
        int colTotalPrice = 8;
        int colVatType = 9;
        int colRemark = 10;

        private decimal ConvertDecimalNull(float? value)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                return (decimal)value;
            }
        }
        /// <summary>
        /// cell click Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colSearch && e.RowIndex > -1)
            {
                SelectedProductPopup obj = new SelectedProductPopup(this, Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList());
                obj.ShowDialog();
            }
        }
        /// <summary>
        /// Bindding data main product
        /// </summary>
        /// <param name="send"></param>
        public void BinddingSelectedProduct(ProductDetails send)
        {
            int row = dataGridView1.CurrentRow.Index;

            var checkQty = dataGridView1.Rows[row].Cells[colQty].Value ?? "0";
            decimal qty = decimal.Parse(checkQty.ToString());
            dataGridView1.Rows[row].Cells[colId].Value = send.Id;
            dataGridView1.Rows[row].Cells[colCode].Value = send.Code;
            dataGridView1.Rows[row].Cells[colName].Value = send.Products.ThaiName;
            dataGridView1.Rows[row].Cells[colUnit].Value = send.ProductUnit.Name;
            dataGridView1.Rows[row].Cells[colPricePerUnit].Value = Library.ConvertDecimalToStringForm(send.SellPrice);
            dataGridView1.Rows[row].Cells[colVatType].Value = send.Products.ProductVatType.Name;
            dataGridView1.Rows[row].Cells[colDiscount].Value = "0.00";


            if (qty == 0)
            {
                dataGridView1.Rows[row].Cells[colQty].Value = "1";
                dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(send.SellPrice);
            }
            else
            {
                dataGridView1.Rows[row].Cells[colQty].Value = qty;
                dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * send.SellPrice);
            }
            //dataGridView1.Rows.Add();
            // set focus qty 
            //dataGridView1.Rows[row].Cells[colQty].begi
            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
            dataGridView1.BeginEdit(true);
            TotalSummary();
        }
        /// <summary>
        /// ยอดสรุปรวม ซื้อรถใหญ่
        /// </summary>
        void TotalSummary()
        {
            try
            {
                decimal totalQty = 0;
                decimal totalUnVat = 0;
                decimal totalHasVat = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    int id = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    ProductDetails product = SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == id);
                    decimal qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    totalQty += qty;

                    decimal totalPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colTotalPrice].Value.ToString());
                    if (product.Products.ProductVatType.Id == MyConstant.ProductVatType.UnVat)
                    {
                        // ถ้าเป็น ยกเว้นภาษี                       
                        totalUnVat += totalPrice;
                    }
                    else
                    {
                        totalHasVat += totalPrice;
                    }
                    if (i >= dataGridView1.Rows.Count - 2) break;
                }
                textBoxQtyUnit.Text = Library.ConvertDecimalToStringForm(totalQty);
                textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(totalUnVat);
                decimal vat = Library.CalVatFromValue(totalHasVat);
                decimal beforeVat = totalHasVat - vat;
                textBoxTotalBeforeVat.Text = Library.ConvertDecimalToStringForm(beforeVat);
                textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(vat);
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(totalUnVat + totalHasVat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TotalSummary();
        }
        /// <summary>
        /// บันทึก ออร์เดอร์ รถใหญ่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
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
        /// <summary>
        /// ยืนยันบันทึก
        /// </summary>
        private void SaveCommit()
        {
            SaleOrderWarehouse sow = new SaleOrderWarehouse();
            sow.Enable = true;
            sow.Code = textBoxCode.Text;
            sow.CodeRefer = textBoxCodeRefer.Text;
            sow.Description = textBoxRemark.Text;
            sow.CreateDate = DateTime.Now;
            sow.CreateBy = Singleton.SingletonAuthen.Instance().Id;
            sow.UpdateDate = DateTime.Now;
            sow.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
            sow.FKBranch = _BranchId;
            sow.FKDebtor = _DebtorId;
            sow.FKMember = _MemberId;
            sow.FKDelivery = _DeliverId;
            sow.FKPayment = _PaymentId;
            sow.TotalDiscount = 0;

            List<SaleOrderWarehouseDtl> details = new List<SaleOrderWarehouseDtl>();
            SaleOrderWarehouseDtl detail;
            decimal discountList = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[colCode].Value == null)
                {
                    continue;
                }
                else if (dataGridView1.Rows[i].Cells[colCode].Value.ToString() == "")
                {
                    continue;
                }
                detail = new SaleOrderWarehouseDtl();
                detail.Enable = true;

                detail.CreateDate = DateTime.Now;
                detail.CreateBy = SingletonAuthen.Instance().Id;
                detail.UpdateDate = DateTime.Now;
                detail.UpdateBy = SingletonAuthen.Instance().Id;
                detail.FKProductDtl = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                detail.Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString()); // จำนวนหน่วย
                detail.PricePerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colPricePerUnit].Value.ToString()); // จำนวนหน่วย
                detail.BathDiscount = decimal.Parse(dataGridView1.Rows[i].Cells[colDiscount].Value.ToString()); // จำนวนเงินส่วนลด
                detail.TotalPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colTotalPrice].Value.ToString()); // จำนวนเงิน หลังหักส่วนลด
                details.Add(detail);
                discountList += decimal.Parse(dataGridView1.Rows[i].Cells[colDiscount].Value.ToString());
                //if (i >= dataGridView1.Rows.Count - 2) break;
            }
            sow.TotalDiscountInList = discountList; // วนใน ลิส
            sow.DiscountRemark = "-";
            sow.SaleOrderWarehouseDtl = details;
            sow.ProjectName = textBoxProject.Text.Trim();
            sow.FKSaleOrderWarehouseStatus = MyConstant.SaleOrderWarehouseStatus.CreateOrder;
            sow.TotalBalance = decimal.Parse(textBoxTotalBalance.Text);
            sow.TotalVat = decimal.Parse(textBoxTotalVat.Text);
            sow.TotaNoVat = decimal.Parse(textBoxTotalUnVat.Text);
            sow.TotalBeforeVat = decimal.Parse(textBoxTotalBeforeVat.Text);
            sow.Total = sow.TotalVat + sow.TotaNoVat + sow.TotalBeforeVat;

            using (SSLsEntities db = new SSLsEntities())
            {
                db.SaleOrderWarehouse.Add(sow);
                db.SaveChanges();

                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var count = db.SaleOrderWarehouse.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                string code = SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(count, 4);
                textBoxCode.Text = MyConstant.PrefixForGenerateCode.SaleOrderFromWarehouse + code;
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                dataGridView1.Rows.Add(1);
                TotalSummary();
            }
        }
        /// <summary>
        /// แก้ไข cell ใน datagrid---------------------------------------------- main input product---------------------
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    string code = dataGridView1.Rows[e.RowIndex].Cells[colCode].Value.ToString();
            //    var product = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).FirstOrDefault(w => w.Code == code);
            //    var checkQty = dataGridView1.Rows[e.RowIndex].Cells[colQty].Value ?? "0";
            //    decimal qty = decimal.Parse(checkQty.ToString());
            //    switch (e.ColumnIndex)
            //    {
            //        /// Code                
            //        case 1:
            //            if (product != null)
            //            {
            //                dataGridView1.Rows[e.RowIndex].Cells[colId].Value = product.Id;
            //                dataGridView1.Rows[e.RowIndex].Cells[colName].Value = product.Products.ThaiName;
            //                dataGridView1.Rows[e.RowIndex].Cells[colUnit].Value = product.ProductUnit.Name;
            //                dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
            //                dataGridView1.Rows[e.RowIndex].Cells[colVatType].Value = product.Products.ProductVatType.Name;
            //                dataGridView1.Rows[e.RowIndex].Cells[colRemark].Value = "-";
            //                if (dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value == null)
            //                {
            //                    dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value = "0.00";
            //                }

            //                if (qty == 0)
            //                {
            //                    dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = "0";
            //                    dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = "0";
            //                }
            //                else
            //                {
            //                    dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = qty;
            //                    dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * product.SellPrice);
            //                }
            //                TotalSummary();
            //            }
            //            else
            //            {
            //                MessageBox.Show("ไม่พบข้อมูล");
            //                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[1];
            //                dataGridView1.BeginEdit(true);
            //            }
            //            break;
            //        case 7: // คีย์ ส่วนลด
            //            //decimal total = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value.ToString());
            //            decimal pricePerUnit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value.ToString());
            //            decimal total = pricePerUnit * qty;
            //            decimal discount = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value.ToString());
            //            dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(total - discount);
            //            TotalSummary();
            //            break;
            //        case 5: // จำนวน
            //            if (qty == 0)
            //            {
            //                dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = "1";
            //                dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
            //            }
            //            else
            //            {
            //                dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = qty;
            //                dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * product.SellPrice);
            //            }
            //            /// คีย์จำนวน ต้องไปเชค Stock 
            //            int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[colId].Value.ToString());
            //            decimal qtyUnit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
            //            using (WH_TRATEntities db = new WH_TRATEntities())
            //            {
            //                var locStk = db.WH_LOCSTK.Where(w => w.PRODUCT_NO == code);
            //                if (locStk.Count() > 0)
            //                {
            //                    decimal sumQty = ConvertDecimalNull(locStk.Sum(w => w.QTY));
            //                    decimal sumbook = ConvertDecimalNull(locStk.Sum(w => w.BOOK_QTY));
            //                    decimal packsize = ConvertDecimalNull(locStk.FirstOrDefault().PACK_SIZE);
            //                    if (qty > ((sumQty - sumbook) / packsize))
            //                    {
            //                        MessageBox.Show("Stock ไม่พอ พบ : " + ((sumQty - sumbook) / packsize) + " " + product.ProductUnit.Name);
            //                    }
            //                }
            //                else
            //                {
            //                    MessageBox.Show("ไม่พบ Stock");
            //                }
            //                //var stockWms = db.WmsStockDetail.Where(w => w.Enable == true && w.FKProductDetail == id).OrderByDescending(w => w.CreateDate).FirstOrDefault();
            //                //Console.WriteLine("row " + e.RowIndex + " Want: " + qtyUnit + " stock: " + stockWms.ResultQtyUnit);
            //                //if (qtyUnit > stockWms.ResultQtyUnit)
            //                //{
            //                //    MessageBox.Show("Stock คงเหลือ " + stockWms.ResultQtyUnit);
            //                //    //dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[colQty];
            //                //    //dataGridView1.CurrentCell.Selected = true;
            //                //}
            //                //else
            //                //{

            //                //}
            //            }
            //            TotalSummary();
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            int col = dataGridView1.CurrentCell.ColumnIndex;
            int row = dataGridView1.CurrentRow.Index;
            try
            {
                string code = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
                var product = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).FirstOrDefault(w => w.Code == code);
                var checkQty = dataGridView1.Rows[row].Cells[colQty].Value ?? "0";
                decimal qty = decimal.Parse(checkQty.ToString());


                if (col == colCode)
                {
                    if (product != null)
                    {
                        dataGridView1.Rows[row].Cells[colId].Value = product.Id;
                        dataGridView1.Rows[row].Cells[colName].Value = product.Products.ThaiName;
                        dataGridView1.Rows[row].Cells[colUnit].Value = product.ProductUnit.Name;
                        dataGridView1.Rows[row].Cells[colPricePerUnit].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
                        dataGridView1.Rows[row].Cells[colVatType].Value = product.Products.ProductVatType.Name;

                        if (dataGridView1.Rows[row].Cells[colDiscount].Value == null)
                        {
                            dataGridView1.Rows[row].Cells[colDiscount].Value = "0.00";
                        }

                        if (qty == 0)
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = "0";
                            dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = "0";
                        }
                        else
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = qty;
                            dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * product.SellPrice);

                            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colPricePerUnit];
                            dataGridView1.BeginEdit(true);
                            return;
                        }

                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                        dataGridView1.BeginEdit(true);
                    }
                    else
                    {
                        MessageBox.Show("ไม่พบข้อมูล");
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colCode];
                        dataGridView1.BeginEdit(true);
                    }

                    TotalSummary();
                }
                else if (col == colQty)
                {
                    decimal pricePerunit = decimal.Parse(dataGridView1.Rows[row].Cells[colPricePerUnit].Value.ToString());
                    decimal total = qty * pricePerunit;

                    if (qty == 0)
                    {
                        dataGridView1.Rows[row].Cells[colQty].Value = "0";
                        dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                        dataGridView1.BeginEdit(true);
                        return;
                    }
                    else
                    {
                        dataGridView1.Rows[row].Cells[colQty].Value = qty;
                        dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * pricePerunit);
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                        dataGridView1.BeginEdit(true);
                        return;
                    }
                    /// คีย์จำนวน ต้องไปเชค Stock 
                    int id = int.Parse(dataGridView1.Rows[row].Cells[colId].Value.ToString());
                    decimal qtyUnit = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                    using (WH_TRATEntities db = new WH_TRATEntities())
                    {
                        var locStk = db.WH_LOCSTK.Where(w => w.PRODUCT_NO == code);
                        if (locStk.Count() > 0)
                        {
                            decimal sumQty = ConvertDecimalNull(locStk.Sum(w => w.QTY));
                            decimal sumbook = ConvertDecimalNull(locStk.Sum(w => w.BOOK_QTY));
                            decimal packsize = ConvertDecimalNull(locStk.FirstOrDefault().PACK_SIZE);
                            if (qty > ((sumQty - sumbook) / packsize))
                            {
                                MessageBox.Show("Stock ไม่พอ พบ : " + ((sumQty - sumbook) / packsize) + " " + product.ProductUnit.Name);
                                TotalSummary();

                                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                                dataGridView1.BeginEdit(true);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("ไม่พบ Stock");
                            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colCode];
                            dataGridView1.BeginEdit(true);
                            return;
                        }

                    }
                    TotalSummary();

                    dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colPricePerUnit];
                    dataGridView1.BeginEdit(true);
                }
                else if (col == colPricePerUnit)
                {
                    // calculate toal
                    decimal pricePerunit = decimal.Parse(dataGridView1.Rows[row].Cells[colPricePerUnit].Value.ToString());
                    decimal total = qty * pricePerunit;
                    dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(total);
                    dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colDiscount];
                    dataGridView1.BeginEdit(true);
                    TotalSummary();
                }
                else if (col == colDiscount)
                {
                    //decimal total = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value.ToString());
                    decimal pricePerUnit = decimal.Parse(dataGridView1.Rows[row].Cells[colPricePerUnit].Value.ToString());
                    decimal total = pricePerUnit * qty;
                    decimal discount = decimal.Parse(dataGridView1.Rows[row].Cells[colDiscount].Value.ToString());
                    dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(total - discount);


                    if (row == dataGridView1.RowCount - 1)
                    {
                        dataGridView1.Rows.Add(1);
                        dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[1];
                        dataGridView1.BeginEdit(true);
                    }
                    else
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colDiscount];
                        dataGridView1.BeginEdit(true);
                    }
                    TotalSummary();
                }
            }
            catch (Exception)
            {

            }


        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (Char)Keys.Enter)
            //{

            //    int col = dataGridView1.CurrentCell.ColumnIndex;
            //    int row = dataGridView1.CurrentRow.Index;
            //    try
            //    {
            //        string code = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
            //        var product = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).FirstOrDefault(w => w.Code == code);
            //        var checkQty = dataGridView1.Rows[row].Cells[colQty].Value ?? "0";
            //        decimal qty = decimal.Parse(checkQty.ToString());


            //        if (col == colCode)
            //        {
            //            if (product != null)
            //            {
            //                dataGridView1.Rows[row].Cells[colId].Value = product.Id;
            //                dataGridView1.Rows[row].Cells[colName].Value = product.Products.ThaiName;
            //                dataGridView1.Rows[row].Cells[colUnit].Value = product.ProductUnit.Name;
            //                dataGridView1.Rows[row].Cells[colPricePerUnit].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
            //                dataGridView1.Rows[row].Cells[colVatType].Value = product.Products.ProductVatType.Name;

            //                if (dataGridView1.Rows[row].Cells[colDiscount].Value == null)
            //                {
            //                    dataGridView1.Rows[row].Cells[colDiscount].Value = "0.00";
            //                }

            //                //if (qty == 0)
            //                //{
            //                //    dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = "0";
            //                //    dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = "0";
            //                //}
            //                //else
            //                //{
            //                //    dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = qty;
            //                //    dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * product.SellPrice);
            //                //}
            //                dataGridView1.Rows[row].Cells[colQty].Value = "0";
            //                dataGridView1.Rows[row].Cells[colTotalPrice].Value = "0";
            //                TotalSummary();
            //                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
            //                dataGridView1.BeginEdit(true);
            //            }
            //            else
            //            {
            //                MessageBox.Show("ไม่พบข้อมูล");
            //                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colCode];
            //                dataGridView1.BeginEdit(true);
            //            }


            //        }
            //        else if (col == colQty)
            //        {
            //            decimal pricePerunit = decimal.Parse(dataGridView1.Rows[row].Cells[colPricePerUnit].Value.ToString());
            //            decimal total = qty * pricePerunit;

            //            if (qty == 0)
            //            {
            //                dataGridView1.Rows[row].Cells[colQty].Value = "1";
            //                dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
            //            }
            //            else
            //            {
            //                dataGridView1.Rows[row].Cells[colQty].Value = qty;
            //                dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * pricePerunit);
            //            }
            //            /// คีย์จำนวน ต้องไปเชค Stock 
            //            int id = int.Parse(dataGridView1.Rows[row].Cells[colId].Value.ToString());
            //            decimal qtyUnit = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
            //            using (WH_TRATEntities db = new WH_TRATEntities())
            //            {
            //                var locStk = db.WH_LOCSTK.Where(w => w.PRODUCT_NO == code);
            //                if (locStk.Count() > 0)
            //                {
            //                    decimal sumQty = ConvertDecimalNull(locStk.Sum(w => w.QTY));
            //                    decimal sumbook = ConvertDecimalNull(locStk.Sum(w => w.BOOK_QTY));
            //                    decimal packsize = ConvertDecimalNull(locStk.FirstOrDefault().PACK_SIZE);
            //                    if (qty > ((sumQty - sumbook) / packsize))
            //                    {
            //                        MessageBox.Show("Stock ไม่พอ พบ : " + ((sumQty - sumbook) / packsize) + " " + product.ProductUnit.Name);
            //                        TotalSummary();

            //                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
            //                        dataGridView1.BeginEdit(true);
            //                        return;
            //                    }
            //                }
            //                else
            //                {
            //                    MessageBox.Show("ไม่พบ Stock");
            //                    dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colCode];
            //                    dataGridView1.BeginEdit(true);
            //                    return;
            //                }

            //            }
            //            TotalSummary();

            //            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colPricePerUnit];
            //            dataGridView1.BeginEdit(true);
            //        }
            //        else if (col == colPricePerUnit)
            //        {
            //            // calculate toal
            //            decimal pricePerunit = decimal.Parse(dataGridView1.Rows[row].Cells[colPricePerUnit].Value.ToString());
            //            decimal total = qty * pricePerunit;
            //            dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(total);
            //            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colDiscount];
            //            dataGridView1.BeginEdit(true);
            //            TotalSummary();
            //        }
            //        else if (col == colDiscount)
            //        {
            //            //decimal total = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value.ToString());
            //            decimal pricePerUnit = decimal.Parse(dataGridView1.Rows[row].Cells[colPricePerUnit].Value.ToString());
            //            decimal total = pricePerUnit * qty;
            //            decimal discount = decimal.Parse(dataGridView1.Rows[row].Cells[colDiscount].Value.ToString());
            //            dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(total - discount);


            //            if (row == dataGridView1.RowCount - 1)
            //            {
            //                dataGridView1.Rows.Add(1);
            //                dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[1];
            //                dataGridView1.BeginEdit(true);
            //            }
            //            else
            //            {
            //                dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colDiscount];
            //                dataGridView1.BeginEdit(true);
            //            }
            //            TotalSummary();
            //        }
            //    }
            //    catch (Exception)
            //    {

            //    }
            //}
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    int col = dataGridView1.CurrentCell.ColumnIndex;
            //    int row = dataGridView1.CurrentRow.Index;
            //    try
            //    {
            //        string code = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
            //        var product = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).FirstOrDefault(w => w.Code == code);
            //        var checkQty = dataGridView1.Rows[row].Cells[colQty].Value ?? "0";
            //        decimal qty = decimal.Parse(checkQty.ToString());


            //        if (col == colCode)
            //        {
            //            if (product != null)
            //            {
            //                dataGridView1.Rows[row].Cells[colId].Value = product.Id;
            //                dataGridView1.Rows[row].Cells[colName].Value = product.Products.ThaiName;
            //                dataGridView1.Rows[row].Cells[colUnit].Value = product.ProductUnit.Name;
            //                dataGridView1.Rows[row].Cells[colPricePerUnit].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
            //                dataGridView1.Rows[row].Cells[colVatType].Value = product.Products.ProductVatType.Name;

            //                if (dataGridView1.Rows[row].Cells[colDiscount].Value == null)
            //                {
            //                    dataGridView1.Rows[row].Cells[colDiscount].Value = "0.00";
            //                }

            //                //if (qty == 0)
            //                //{
            //                //    dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = "0";
            //                //    dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = "0";
            //                //}
            //                //else
            //                //{
            //                //    dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = qty;
            //                //    dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * product.SellPrice);
            //                //}
            //                dataGridView1.Rows[row].Cells[colQty].Value = "0";
            //                dataGridView1.Rows[row].Cells[colTotalPrice].Value = "0";
            //                TotalSummary();
            //                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
            //                dataGridView1.BeginEdit(true);
            //            }
            //            else
            //            {
            //                MessageBox.Show("ไม่พบข้อมูล");
            //                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colCode];
            //                dataGridView1.BeginEdit(true);
            //            }


            //        }
            //        else if (col == colQty)
            //        {
            //            decimal pricePerunit = decimal.Parse(dataGridView1.Rows[row].Cells[colPricePerUnit].Value.ToString());
            //            decimal total = qty * pricePerunit;

            //            if (qty == 0)
            //            {
            //                dataGridView1.Rows[row].Cells[colQty].Value = "1";
            //                dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
            //            }
            //            else
            //            {
            //                dataGridView1.Rows[row].Cells[colQty].Value = qty;
            //                dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(qty * pricePerunit);
            //            }
            //            /// คีย์จำนวน ต้องไปเชค Stock 
            //            int id = int.Parse(dataGridView1.Rows[row].Cells[colId].Value.ToString());
            //            decimal qtyUnit = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
            //            using (WH_TRATEntities db = new WH_TRATEntities())
            //            {
            //                var locStk = db.WH_LOCSTK.Where(w => w.PRODUCT_NO == code);
            //                if (locStk.Count() > 0)
            //                {
            //                    decimal sumQty = ConvertDecimalNull(locStk.Sum(w => w.QTY));
            //                    decimal sumbook = ConvertDecimalNull(locStk.Sum(w => w.BOOK_QTY));
            //                    decimal packsize = ConvertDecimalNull(locStk.FirstOrDefault().PACK_SIZE);
            //                    if (qty > ((sumQty - sumbook) / packsize))
            //                    {
            //                        MessageBox.Show("Stock ไม่พอ พบ : " + ((sumQty - sumbook) / packsize) + " " + product.ProductUnit.Name);
            //                        TotalSummary();

            //                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
            //                        dataGridView1.BeginEdit(true);
            //                        return;
            //                    }
            //                }
            //                else
            //                {
            //                    MessageBox.Show("ไม่พบ Stock");
            //                    dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colCode];
            //                    dataGridView1.BeginEdit(true);
            //                    return;
            //                }

            //            }
            //            TotalSummary();

            //            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colPricePerUnit];
            //            dataGridView1.BeginEdit(true);
            //        }
            //        else if (col == colPricePerUnit)
            //        {
            //            // calculate toal
            //            decimal pricePerunit = decimal.Parse(dataGridView1.Rows[row].Cells[colPricePerUnit].Value.ToString());
            //            decimal total = qty * pricePerunit;
            //            dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(total);
            //            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colDiscount];
            //            dataGridView1.BeginEdit(true);
            //            TotalSummary();
            //        }
            //        else if (col == colDiscount)
            //        {
            //            //decimal total = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colTotalPrice].Value.ToString());
            //            decimal pricePerUnit = decimal.Parse(dataGridView1.Rows[row].Cells[colPricePerUnit].Value.ToString());
            //            decimal total = pricePerUnit * qty;
            //            decimal discount = decimal.Parse(dataGridView1.Rows[row].Cells[colDiscount].Value.ToString());
            //            dataGridView1.Rows[row].Cells[colTotalPrice].Value = Library.ConvertDecimalToStringForm(total - discount);


            //            if (row == dataGridView1.RowCount - 1)
            //            {
            //                dataGridView1.Rows.Add(1);
            //                dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[1];
            //                dataGridView1.BeginEdit(true);
            //            }
            //            else
            //            {
            //                dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colDiscount];
            //                dataGridView1.BeginEdit(true);
            //            }
            //            TotalSummary();
            //        }
            //    }
            //    catch (Exception)
            //    {

            //    }
            //}
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {


        }

        private void dataGridView1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }
    }
}

