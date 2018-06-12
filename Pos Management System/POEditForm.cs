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
    /// <summary>
    /// แก้ไข PO
    /// </summary>
    public partial class POEditForm : Form
    {
        /// <summary>
        /// id PO Header
        /// </summary>
        private int _id;
        private PONonApproveEditForm pONonApproveEditForm;

        public POEditForm()
        {
            InitializeComponent();
        }

        public POEditForm(PONonApproveEditForm pONonApproveEditForm, int id)
        {
            InitializeComponent();
            this.pONonApproveEditForm = pONonApproveEditForm;
            this._id = id;
        }
        /// <summary>
        /// มาจาก หน้า po approve
        /// </summary>
        /// <param name="pOApproveManage"></param>
        /// <param name="id"></param>
        public POEditForm(POApproveManage pOApproveManage, int id)
        {
            InitializeComponent();
            this.pOApproveManage = pOApproveManage;
            this._id = id;
            buttonOK.Visible = false;
        }

        private void POEditForm_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var po = db.POHeader.SingleOrDefault(w => w.Id == _id);
                _poCostType = po.Vendor.FKPOCostType;

                textBoxShopName.Text = po.Branch.Name;
                textBoxShopBranch.Text = po.Branch.Code;

                textBoxVendorCode.Text = po.Vendor.Code;
                textBoxVendorName.Text = po.Vendor.Name;

                textBoxPONo.Text = po.PONo;
                textBoxPODate.Text = Library.ConvertDateToThaiDate(po.PODate);
                textBoxReferNo.Text = po.ReferenceNo;
                textBoxDueDate.Text = Library.ConvertDateToThaiDate(po.DueDate);
                textBoxExpireDate.Text = Library.ConvertDateToThaiDate(po.POExpire);
                textBoxPayType.Text = po.PaymentType.Name;

                textBoxDiscountInvoice.Text = po.DiscountInput;
                groupBoxPOProducts.Text = "แก้ไขรายการสินค้า (" + po.Vendor.POCostType.Name + ")";
                // details po
                foreach (var item in po.PODetail.Where(w => w.Enable == true).ToList())
                {
                    dataGridView1.Rows.Add
                        (
                        item.ProductDetails.Code,
                        "",
                        item.ProductDetails.Products.ThaiName,
                        item.Qty,
                        item.ProductDetails.ProductUnit.Name,
                        item.GiftQty,
                        Library.ConvertDecimalToStringForm(item.CostOnly),
                        Library.ConvertDecimalToStringForm(item.CostAndVat),
                        item.DiscountInput,
                        Library.ConvertDecimalToStringForm(item.DiscountBath),
                        Library.ConvertDecimalToStringForm(item.TotalCost),
                        item.FKProductDetail,
                        item.Id,
                        item.InterfaceDate
                        );
                }
                dataGridView1.Rows.Add(1);
                /// หลังจาก Bindding data เสร็จ ก็สรุป
                RefreshConclude();
                RefreshConclude();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        int colNo = 0;
        int colName = 2;
        int colQty = 3;
        int colUnit = 4;
        int colGift = 5;
        int colCostPerUnit = 6;
        int colCostVatPerUnit = 7;
        int colDiscount = 8;
        int colDiscountBath = 9;
        int colTotalBalance = 10;
        int colFKProDtlId = 11;
        int colId = 12;
        int colInterface = 13;

        bool discountPercent = true;
        int _poCostType = 0;
        private POApproveManage pOApproveManage;

        private void RefreshConclude()
        {
            decimal total = decimal.Parse(textBoxTotal.Text); // รวม
            decimal totalDiscountInvoice = decimal.Parse(textBoxTotalDiscountInvoice.Text); // ลดท้ายบิล
            decimal totalAfterDis = decimal.Parse(textBoxTotalAfterDis.Text); // หลังหักลด
            decimal totalVat = decimal.Parse(textBoxTotalVat.Text); // ภาษี
            decimal totalBalance = decimal.Parse(textBoxTotalBalance.Text); // สุทธิ

            decimal totalUnVat = 0; // ยอดรวมสินค้าที่ยกเว้นภาษี
            decimal totalHasVat = 0; // ยอดรวมสินค้าที่มีภาษี

            decimal discountBath = 0;
            string discountInvoice = textBoxDiscountInvoice.Text; // ส่วนลดท้ายบิล

            total = 0; // รวมทั้งหมด
            decimal cost = 0;
            decimal costAndVat = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {
                    cost = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                    costAndVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostVatPerUnit].Value.ToString());
                }
                catch (Exception)
                {
                    cost = 0;
                    costAndVat = 0;
                }

                if (dataGridView1.Rows[i].Cells[colTotalBalance].Value == null)
                {
                    total += 0;
                }
                else
                {
                    total += decimal.Parse(dataGridView1.Rows[i].Cells[colTotalBalance].Value.ToString());
                    if (cost == costAndVat) // แสดงว่าเป็นสินค้ายกเว้นภาษี
                    {
                        totalUnVat += decimal.Parse(dataGridView1.Rows[i].Cells[colTotalBalance].Value.ToString());
                    }
                    else
                    {
                        totalHasVat += decimal.Parse(dataGridView1.Rows[i].Cells[colTotalBalance].Value.ToString());
                    }
                }
                if (i >= dataGridView1.Rows.Count - 2) break;
            }
            // binding ui
            // textBoxTotal.Text = Library.ConvertDecimalToStringForm(total);
            // bind discount
            // รวมทั้งหมด
            string[] subStr = Library.GetCheckBathOrPercent(discountInvoice);

            decimal percent = 0;

            if (subStr[1] == "บ")
            {
                discountPercent = false;
                discountBath = decimal.Parse(subStr[0]);
                // หา บาท ส่วนลด ของยอดมีภาษี
                decimal totalBeforeVat = totalUnVat + (totalHasVat - (totalHasVat * MyConstant.MyVat.Vat / MyConstant.MyVat.VatRemove));
                percent = Library.GetPercentFromDiscountBath(discountBath, totalBeforeVat);
            }
            else
            {
                discountPercent = true;
                discountBath = Library.CalPercentByValue(totalUnVat + decimal.Parse(textBoxTotalHasVat.Text), decimal.Parse(subStr[0]));
                // หา % ส่วนลด ของยอดมีภาษี
                percent = decimal.Parse(subStr[0]);
            }
            switch (_poCostType)
            {

                case 1: // ถ้าเป็นทุนเปล่า
                    // ยอดรวมทั้งหมด
                    textBoxTotal.Text = Library.ConvertDecimalToStringForm(total);
                    // ยอดรวมภาษี
                    textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(totalHasVat);
                    // ยอดรวมยกเว้นภาษี
                    textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(totalUnVat);
                    // ยอดส่วนลดท้ายบิล
                    textBoxTotalDiscountInvoice.Text = Library.ConvertDecimalToStringForm(discountBath);
                    // ยอดหลังหักส่วนลด
                    textBoxTotalAfterDis.Text = Library.ConvertDecimalToStringForm(total - discountBath);
                    decimal forVat = 0;
                    // ยอดมีภาษี - ยอดส่วนลดจากคิดเป็น % 
                    forVat = decimal.Parse(textBoxTotalHasVat.Text) - Library.CalPercentByValue(decimal.Parse(textBoxTotalHasVat.Text), percent);
                    // ภาษีมูลค่าเพิ่ม                    
                    textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(forVat * MyConstant.MyVat.Vat / 100);
                    // ยอดสุทธิ หลังหักลด + ภาษีมูลค่าเพิ่ม
                    textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(decimal.Parse(textBoxTotalAfterDis.Text) + decimal.Parse(textBoxTotalVat.Text));
                    break;
                case 2: // ถ้าเป็นทุนรวมภาษี
                    // ลดแบบ %
                    if (discountPercent)
                    {

                    }
                    decimal removeVat = Library.CalVatFromValue(totalHasVat);
                    // ยอดรวมทั้งหมด                
                    textBoxTotal.Text = Library.ConvertDecimalToStringForm(total);
                    // ยอดรวมภาษี
                    textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(totalHasVat - removeVat);
                    forVat = 0;

                    // ยอดมีภาษี - ยอดส่วนลดจากคิดเป็น % 
                    forVat = decimal.Parse(textBoxTotalHasVat.Text) - Library.CalPercentByValue(decimal.Parse(textBoxTotalHasVat.Text), percent);
                    //// ยอดรวมยกเว้นภาษี
                    textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(totalUnVat);
                    //// ยอดส่วนลดท้ายบิล
                    textBoxTotalDiscountInvoice.Text = Library.ConvertDecimalToStringForm(discountBath);
                    //// ยอดหลังหักส่วนลด
                    textBoxTotalAfterDis.Text = Library.ConvertDecimalToStringForm(decimal.Parse(textBoxTotalHasVat.Text) + decimal.Parse(textBoxTotalUnVat.Text) - discountBath);
                    // ภาษีมูลค่าเพิ่ม                    
                    textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(forVat * MyConstant.MyVat.Vat / 100);
                    // ยอดสุทธิ หลังหักลด + ภาษีมูลค่าเพิ่ม
                    textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(decimal.Parse(textBoxTotalAfterDis.Text) + decimal.Parse(textBoxTotalVat.Text));
                    break;
            }
        }
        /// <summary>
        /// Enter Discount Input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDiscountInvoice_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    RefreshConclude();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ยก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDiscountInvoice_Leave(object sender, EventArgs e)
        {
            RefreshConclude();
        }
        /// <summary>
        /// มีการเปลี่ยนแปลงใน datagrid1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }
        /// <summary>
        /// บันทึกแก้ไข *แก้ไขได้เฉพาะ สินค้า และส่วนลด เท่านั้น
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            RefreshConclude();
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
        /// แก้ไข PO ที่ยังไม่ approve เราจะ Disable  details ทอ้งทั้งหมด และ add new details ใหม่
        /// </summary>
        private void SaveCommit()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var hd = db.POHeader.SingleOrDefault(w => w.Id == _id);
                hd.UpdateDate = DateTime.Now;
                hd.UpdateBy = SingletonAuthen.Instance().Id;
                if (discountPercent)
                {
                    hd.DiscountBath = Library.ConvertDecimalToStringForm(Library.CalPercentByValue(hd.TotalHasVat + hd.TotalUnVat, decimal.Parse(textBoxDiscountInvoice.Text))); // แปลงจากคีย์ % เป็น บาท
                    hd.DiscountPercent = textBoxDiscountInvoice.Text + " %";
                }
                else
                {
                    hd.DiscountBath = textBoxDiscountInvoice.Text.Replace("บ", "");
                    //hd.DiscountPercent = hd.TotalDiscount + ""; // แปลงเงินบาทเป็น % 
                    hd.DiscountPercent = Library.GetPercentFromDiscountBath(decimal.Parse(hd.DiscountBath), hd.TotalHasVat + hd.TotalUnVat) + " %";
                }
                hd.DiscountInput = textBoxDiscountInvoice.Text;
                hd.TotalPrice = decimal.Parse(textBoxTotal.Text);
                hd.TotalHasVat = decimal.Parse(textBoxTotalHasVat.Text);
                hd.TotalUnVat = decimal.Parse(textBoxTotalUnVat.Text);
                hd.TotalDiscount = decimal.Parse(textBoxTotalDiscountInvoice.Text);
                hd.TotalPriceDiscount = decimal.Parse(textBoxTotalAfterDis.Text);
                hd.TotalVat = decimal.Parse(textBoxTotalVat.Text);
                hd.TotalBalance = decimal.Parse(textBoxTotalBalance.Text);
                /// disable old
                foreach (var item in hd.PODetail)
                {
                    if (item.Enable == true)
                    {
                        item.Enable = false;
                        item.UpdateDate = DateTime.Now;
                        item.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        db.Entry(item).State = EntityState.Modified;
                    }
                }
                /// add new
                //List<PODetail> details = new List<PODetail>();
                PODetail detail;
                decimal costOnly = 0;
                decimal costAndVat = 0;
                decimal totalCost = 0;
                decimal totalQty = 0;
                decimal totalGift = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    detail = new PODetail();
                    costOnly = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                    costAndVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostVatPerUnit].Value.ToString());
                    if (_poCostType == MyConstant.POCostType.CostOnly)
                    {
                        totalCost = costOnly * decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    }
                    else
                    {
                        totalCost = costAndVat * decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    }

                    detail.Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    totalQty += detail.Qty;

                    detail.GiftQty = decimal.Parse(dataGridView1.Rows[i].Cells[colGift].Value.ToString());
                    totalGift += detail.GiftQty;

                    detail.CostOnly = costOnly;
                    detail.CostAndVat = costAndVat;
                    detail.DiscountInput = dataGridView1.Rows[i].Cells[colDiscount].Value.ToString();
                    detail.DiscountBath = decimal.Parse(dataGridView1.Rows[i].Cells[colDiscountBath].Value.ToString());
                    detail.TotalCost = totalCost - decimal.Parse(dataGridView1.Rows[i].Cells[colDiscountBath].Value.ToString());
                    detail.CreateDate = DateTime.Now;
                    detail.CreateBy = SingletonAuthen.Instance().Id;
                    detail.UpdateDate = DateTime.Now;
                    detail.UpdateBy = SingletonAuthen.Instance().Id;
                    detail.Enable = true;
                    //detail.RcvQty = 0;
                    detail.FKProductDetail = int.Parse(dataGridView1.Rows[i].Cells[colFKProDtlId].Value.ToString());
                    if (dataGridView1.Rows[i].Cells[colInterface].Value != null)
                    {
                        string interfaceDate = dataGridView1.Rows[i].Cells[colInterface].Value.ToString();
                        detail.InterfaceDate = DateTime.Parse(interfaceDate);
                    }
                    //int id = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    if (dataGridView1.Rows[i].Cells[colId].Value != null)
                    {
                        int id = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                        detail.RcvQty = hd.PODetail.SingleOrDefault(w => w.Id == id).RcvQty;
                        detail.RcvGiftQty = hd.PODetail.SingleOrDefault(w => w.Id == id).RcvGiftQty;
                    }
                    detail.Sequence = hd.SequenceEdit;
                    hd.PODetail.Add(detail);
                    if (i >= dataGridView1.Rows.Count - 2) break;
                }
                hd.TotalQty = totalQty;
                hd.TotalGift = totalGift;
                db.SaveChanges();
                MainReportViewer mr = new MainReportViewer(this, _id);
                mr.ShowDialog();

            }
        }



        /// <summary>
        /// Click Search Product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            switch (e.ColumnIndex)
            {
                case 1:
                    // show dialog products
                    //DialogWindowsProducts objInstance = new DialogWindowsProducts(this);
                    //objInstance.ShowDialog();
                    SelectedProductPopup obj = new SelectedProductPopup(this);
                    obj.ShowDialog();
                    break;
            }
        }
        /// <summary>
        /// Bindding After Choose Product
        /// </summary>
        /// <param name="id">Pro Dtl</param>
        public void BinddingProductChoose(int id)
        {
            List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
            var product = products.SingleOrDefault(w => w.Id == id);
            // start bind ui
            int currentRow = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows[currentRow].Cells[colFKProDtlId].Value = product.Id;
            dataGridView1.Rows[currentRow].Cells[colNo].Value = product.Code;
            dataGridView1.Rows[currentRow].Cells[colName].Value = product.Products.ThaiName;
            dataGridView1.Rows[currentRow].Cells[colQty].Value = 1.00;
            dataGridView1.Rows[currentRow].Cells[colUnit].Value = product.ProductUnit.Name;
            dataGridView1.Rows[currentRow].Cells[colGift].Value = 0.00;
            dataGridView1.Rows[currentRow].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostOnly);
            dataGridView1.Rows[currentRow].Cells[colCostVatPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostAndVat);
            dataGridView1.Rows[currentRow].Cells[colDiscount].Value = 0.00;
            dataGridView1.Rows[currentRow].Cells[colDiscountBath].Value = "0.00";
            if (_poCostType == MyConstant.POCostType.CostOnly)
            {
                dataGridView1.Rows[currentRow].Cells[colTotalBalance].Value = product.CostOnly * 1;
            }
            else
            {
                dataGridView1.Rows[currentRow].Cells[colTotalBalance].Value = product.CostAndVat * 1;
            }
            //dataGridView1.Rows[currentRow].Cells[colQty].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[colQty];
            dataGridView1.CurrentCell.Selected = true;
            //dataGridView1.BeginEdit(true);
        }
        /// <summary>
        /// เปลี่ยนแปลงค่าใน Grid1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                List<ProductDetails> products = new List<ProductDetails>();
                products = SingletonProduct.Instance().ProductDetails;
                decimal costCal = 0;
                if (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2)
                {

                }
                else if (e.ColumnIndex == colQty)
                {
                    var pNo = dataGridView1.Rows[e.RowIndex].Cells[colNo].Value.ToString();
                    var p = products.FirstOrDefault(w => w.Code == pNo);
                    decimal qty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                    //dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(p.CostOnly * qty); // ทุนเปล่า
                    //dataGridView1.Rows[e.RowIndex].Cells[colCostVatPerUnit].Value = Library.ConvertDecimalToStringForm(p.CostAndVat * qty); // ทุนรวมภาษี

                    costCal = 0;
                    if (_poCostType == MyConstant.POCostType.CostAndVat)
                    {
                        costCal = p.CostAndVat;
                    }
                    else if (_poCostType == MyConstant.POCostType.CostOnly)
                    {
                        costCal = p.CostOnly;
                    }
                    else
                    {
                        costCal = p.CostOnly;
                    }
                    dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value = "0.00";
                    dataGridView1.Rows[e.RowIndex].Cells[colDiscountBath].Value = "0.00"; // ส่วนลดเมื่อแปลงเป็นเงิน จาก บ หรือ %
                    dataGridView1.Rows[e.RowIndex].Cells[colTotalBalance].Value = Library.ConvertDecimalToStringForm(costCal * qty); // ราคาสุทธิ
                    RefreshConclude();
                }
                else if (e.ColumnIndex == colDiscount) // ถ้าคีย์ และะออกจากช่อง ส่วนลด
                {
                    string discountTxt = dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value.ToString();
                    string[] subString = Library.GetCheckBathOrPercent(discountTxt);
                    decimal discountBath = 0;
                    //decimal costOnly = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value.ToString());

                    var pNo = dataGridView1.Rows[e.RowIndex].Cells[colNo].Value.ToString();
                    var p = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Code == pNo);

                    if (_poCostType == MyConstant.POCostType.CostAndVat) // ทุนรวมภาษี
                    {
                        costCal = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostVatPerUnit].Value.ToString());
                    }
                    else if (_poCostType == MyConstant.POCostType.CostOnly) // ทุนเปล่า
                    {
                        costCal = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value.ToString());
                    }
                    //else
                    //{
                    //    costCal = p.CostOnly;
                    //}
                    /// คำนวนส่วนลด
                    decimal qty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString()); // จำนวน
                    if (subString[1] == "บ") // ต้องการลดเป็น บาท
                    {
                        discountBath = decimal.Parse(subString[0]);

                    }
                    else if (subString[1] == "") // ต้องการลด %
                    {
                        decimal percent = decimal.Parse(subString[0]);
                        if (percent > 99)
                            MessageBox.Show("ส่วนลด " + percent + " % คุณแน่ใจหรือไม่ ?");
                        discountBath = Library.CalPercentByValue(costCal * qty, decimal.Parse(subString[0]));
                    }
                    dataGridView1.Rows[e.RowIndex].Cells[colDiscountBath].Value = Library.ConvertDecimalToStringForm(discountBath); // ส่วนลดเมื่อแปลงเป็นเงิน จาก บ หรือ %
                    dataGridView1.Rows[e.RowIndex].Cells[colTotalBalance].Value = Library.ConvertDecimalToStringForm(qty * costCal - discountBath); // total balance
                    RefreshConclude();
                }
                else
                {
                    RefreshConclude();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    int col = dataGridView1.CurrentCell.ColumnIndex;
                    int row = dataGridView1.CurrentRow.Index;
                    if (col == colNo)
                    {
                        var code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colNo].Value.ToString();
                        ProductDetails product = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == code);
                        if (product == null)
                        {
                            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colNo];
                            dataGridView1.BeginEdit(true);
                        }
                        BinddingProductChoose(product.Id);
                        //dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colId].Value = product.Id;
                        //dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colCode].Value = product.Code;
                        //dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colName].Value = product.Products.ThaiName;
                        //dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colUnit].Value = product.ProductUnit.Name;
                        //dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colPZ].Value = product.PackSize;
                        //dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colQty].Value = 1.00;
                        //dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostAndVat);

                        //dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colDescription].Value = "-";


                        //dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                        //dataGridView1.BeginEdit(true);

                    }
                    else if (col == colQty)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colGift];
                        dataGridView1.BeginEdit(true);
                    }
                    else if (col == colGift)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colDiscount];
                        dataGridView1.BeginEdit(true);
                    }
                    else if (col == colDiscount)
                    {
                        dataGridView1.Rows.Add(1);
                        dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colNo];
                        dataGridView1.BeginEdit(true);
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
