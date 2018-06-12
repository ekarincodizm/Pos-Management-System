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
    public partial class GoodsAutoReturnWmsForm : Form
    {
        public GoodsAutoReturnWmsForm()
        {
            InitializeComponent();
        }
        int colId = 0;
        int colCode = 1;
        int colName = 2;
        int colUnit = 3;
        int colQty = 4;
        int colQtyCN = 5;
        int colPZ = 6;
        int colCostPerUnit = 7;
        int colAllPrice = 8;
        int colDescription = 9;
        private void CalSummary()
        {
            try
            {
                decimal qty = 0;
                decimal hasvat = 0;
                decimal unvat = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    qty += decimal.Parse(dataGridView1.Rows[i].Cells[colQtyCN].Value.ToString());

                    int fkProdDtl = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    var prodDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == fkProdDtl);
                    if (prodDtl.Products.ProductVatType.Id == MyConstant.ProductVatType.HasVat)
                    {
                        // ถ้าเป็นสินค้ามีภาษี
                        hasvat += decimal.Parse(dataGridView1.Rows[i].Cells[colAllPrice].Value.ToString());
                    }
                    else
                    {
                        // ถ้าเป็นสินค้า ยกเว้นภาษี
                        unvat += decimal.Parse(dataGridView1.Rows[i].Cells[colAllPrice].Value.ToString());
                    }
                    //if (i >= dataGridView1.Rows.Count - 2) break;
                }
                decimal vat = hasvat * MyConstant.MyVat.Vat / 100;
                decimal total = vat + hasvat + unvat;
                textBoxQtyUnit.Text = Library.ConvertDecimalToStringForm(qty);
                textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(unvat);
                textBoxTotalBeforeVat.Text = Library.ConvertDecimalToStringForm(hasvat);
                textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(vat);
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(total);
            }
            catch (Exception)
            {

            }
        }

        private void ResetForm()
        {
            string code = "";
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var cnw = db.CNWarehouse.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                BudgetYear budget = Singleton.SingletonThisBudgetYear.Instance().ThisYear;
                //Branch branch = Singleton.SingletonP
                code = MyConstant.PrefixForGenerateCode.GoodsReturnCN + "" + budget.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(cnw, 4);
                textBoxCode.Text = code;

                textBoxQtyUnit.Text = "0.00";
                textBoxTotalUnVat.Text = "0.00";
                textBoxTotalBeforeVat.Text = "0.00";
                textBoxTotalVat.Text = "0.00";
                textBoxTotalBalance.Text = "0.00";
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
            }
        }

        private void GoodsAutoReturnWmsForm_Load(object sender, EventArgs e)
        {
            textBoxDate.Text = Library.ConvertDateToThaiDate(DateTime.Now);
            string code = "";
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var cnw = db.CNWarehouse.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                BudgetYear budget = Singleton.SingletonThisBudgetYear.Instance().ThisYear;
                //Branch branch = Singleton.SingletonP
                code = MyConstant.PrefixForGenerateCode.GoodsReturnCN + "" + budget.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(cnw, 4);
                textBoxCode.Text = code;

            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedVendorPopup dwv = new SelectedVendorPopup(this);
            dwv.ShowDialog();
        }
        int _VendorId = 0;
        public void BinddingVendor(int id)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            _VendorId = id;
            List<Vendor> vendors = SingletonVender.Instance().Vendors;
            var data = vendors.SingleOrDefault(w => w.Id == id);
            textBoxVendorCode.Text = data.Code;
            textBoxVendorName.Text = data.Name;
            /// Get สินค้าห้องของเสีย
            IEnumerable<int> allProductInVendor = Singleton.SingletonProduct.Instance().Products.Where(w => w.Enable == true && w.FKVender == _VendorId).Select(w => w.Id).Distinct().ToList<int>();
            using (SSLsEntities db = new SSLsEntities())
            {
                var waste = db.WasteWarehouse.Where(w => w.Enable == true && allProductInVendor.Contains(w.FKProduct)).ToList();
                /// ตรวจพบสินค้า vendor ในห้องของเสีย
                foreach (var item in waste)
                {
                    IEnumerable<int> fkProductDtls = item.WasteWarehouseDetails.Where(w => w.Enable == true).Select(w => w.FKProductDetails).Distinct().ToList<int>();
                    /// Bindding Data Grid
                    foreach (int idProDtl in fkProductDtls)
                    {
                        decimal inQty = 0;
                        decimal outQty = 0;
                        try
                        {
                            inQty = db.WasteWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == idProDtl && w.IsInOrOut == true).Sum(w => w.QtyUnit);
                            outQty = db.WasteWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == idProDtl && w.IsInOrOut == false).Sum(w => w.QtyUnit);
                        }
                        catch (Exception)
                        {
                            outQty = 0;
                        }
                        if (inQty - outQty == 0)
                        {
                            continue;
                        }
                        /// from ห้องของเสีย
                        var getProdDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == idProDtl);
                        dataGridView1.Rows.Add
                             (
                       idProDtl,
                       getProdDtl.Code,
                       getProdDtl.Products.ThaiName,
                       getProdDtl.ProductUnit.Name,
                       Library.ConvertDecimalToStringForm(inQty - outQty),
                       Library.ConvertDecimalToStringForm(inQty - outQty),// ยอดที่ต้องคืน
                       getProdDtl.PackSize,
                       Library.ConvertDecimalToStringForm(getProdDtl.CostOnly),
                       Library.ConvertDecimalToStringForm(getProdDtl.CostOnly * (inQty - outQty)),
                       "-");
                    }
                }
                // คำนวน
                CalSummary();
            }
        }

        /// <summary>
        /// Enter Vendor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxVendorCode_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    List<Vendor> vendors = SingletonVender.Instance().Vendors;
                    string code = textBoxVendorCode.Text.Trim();
                    Vendor data = vendors.FirstOrDefault(w => w.Enable == true && w.Code == code);
                    if (data != null)
                    {
                        BinddingVendor(data.Id);
                    }
                    else
                    {
                        dataGridView1.Rows.Clear();
                        dataGridView1.Refresh();

                        textBoxVendorCode.Text = "";
                        textBoxVendorName.Text = "";
                        _VendorId = 0;
                    }
                    break;
                default:
                    break;
            }

        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                switch (e.ColumnIndex)
                {
                    case 5:
                        decimal qty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQtyCN].Value.ToString());
                        decimal cost = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value.ToString());
                        dataGridView1.Rows[e.RowIndex].Cells[colAllPrice].Value = Library.ConvertDecimalToStringForm(qty * cost);
                        break;
                    default:
                        break;
                }
                // cal summary
                CalSummary();
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// เปิดเหตุผลทำคืน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            SelectedWasteReasonPopup obj = new SelectedWasteReasonPopup(this);
            obj.ShowDialog();
        }
        int _WasteReason;
        private string code;

        public void BinddingWasteReasonSelected(WasteReason send)
        {
            _WasteReason = send.Id;
            textBoxWRCode.Text = send.Code;
            textBoxWRName.Text = send.Name;
        }
        /// <summary>
        /// บันทึก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            // cal summary
            CalSummary();
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
            try
            {
                CNWarehouse cnw = new CNWarehouse();
                cnw.Code = textBoxCode.Text;
                cnw.Enable = true;
                cnw.Description = textBoxDesc.Text;
                cnw.CreateDate = DateTime.Now;
                cnw.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                cnw.UpdateDate = DateTime.Now;
                cnw.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                cnw.FKWasteReason = _WasteReason;
                decimal qtyPiece = 0;
                List<CNWarehouseDetails> details = new List<CNWarehouseDetails>();
                CNWarehouseDetails detail;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    detail = new CNWarehouseDetails();

                    detail.Enable = true;
                    detail.Description = dataGridView1.Rows[i].Cells[colDescription].Value.ToString();
                    detail.CreateDate = DateTime.Now;
                    detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.UpdateDate = DateTime.Now;
                    detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.FKProductDetails = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    detail.Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQtyCN].Value.ToString());
                    if (detail.Qty < 1)
                    {
                        continue;
                    }
                    qtyPiece += decimal.Parse(dataGridView1.Rows[i].Cells[colQtyCN].Value.ToString()) * decimal.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString());
                    detail.PricePerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                    detail.BeforeVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString()) * detail.Qty;
                    detail.Vat = ((detail.BeforeVat * detail.Qty) * MyConstant.MyVat.Vat) / 100;
                    detail.TotalPrice = detail.BeforeVat + detail.Vat;
                    details.Add(detail);
                    //if (i >= dataGridView1.Rows.Count - 2) break;
                }
                cnw.FKVendor = _VendorId;
                cnw.DocDate = DateTime.Now;
                cnw.DocRefer = textBoxDocRefer.Text;
                cnw.TotalQty = qtyPiece;
                cnw.TotalQtyUnit = decimal.Parse(textBoxQtyUnit.Text);
                cnw.TotalUnVat = decimal.Parse(textBoxTotalUnVat.Text);
                cnw.TotalBeforeVat = decimal.Parse(textBoxTotalBeforeVat.Text);
                cnw.TotalVat = decimal.Parse(textBoxTotalVat.Text);
                cnw.TotalBalance = cnw.TotalUnVat + cnw.TotalBeforeVat;
                cnw.CNWarehouseDetails = details;

                using (SSLsEntities db = new SSLsEntities())
                {
                    db.CNWarehouse.Add(cnw);
                    db.SaveChanges();
                    // ManageStock Wms
                    //Library.MakeValueForUpdateStockWms(details);
                    MessageBox.Show("บันทึกเรียบร้อย " + cnw.Code);
                    // Add To Details ก่อน
                    // WasteWarehouse ห้องของเสียจัดเก็บ
                    // WasteWarehouseDetails
                    foreach (var item in details)
                    {
                        WasteWarehouseDetails dtl = new WasteWarehouseDetails();
                        dtl.ConfirmCNBy = null;
                        dtl.ConfirmCNDate = null;
                        var getProdDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails);
                        dtl.CostOnly = getProdDtl.CostOnly;
                        dtl.CreateBy = item.CreateBy;
                        dtl.CreateDate = DateTime.Now;
                        dtl.Description = "ส่งคืนห้องของเสีย ให้ผู้จำหน่าย";
                        dtl.DocReference = cnw.Code;
                        dtl.Enable = true;
                        dtl.FKProductDetails = item.FKProductDetails;
                        var lastTrans = db.WasteWarehouseDetails.Where(w => w.FKProductDetails == item.FKProductDetails && w.Enable == true)
                            .OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        dtl.FKWasteWarehouse = lastTrans.FKWasteWarehouse;
                        dtl.IsInOrOut = false;
                        dtl.LastResultPiece = 0;
                        dtl.LastResultUnit = 0;
                        dtl.Packsize = getProdDtl.PackSize;
                        dtl.QtyPiece = item.Qty * dtl.Packsize;
                        dtl.QtyUnit = item.Qty;
                        dtl.SellPrice = getProdDtl.SellPrice;
                        dtl.UpdateBy = item.CreateBy;
                        dtl.UpdateDate = DateTime.Now;
                        db.WasteWarehouseDetails.Add(dtl);
                        db.SaveChanges();

                        // Update HD
                        var wasteWarehouseHD = db.WasteWarehouse.SingleOrDefault(w => w.Id == lastTrans.FKWasteWarehouse);
                        wasteWarehouseHD.QtyPiece = wasteWarehouseHD.QtyPiece - dtl.QtyPiece;
                        wasteWarehouseHD.QtyUnit = wasteWarehouseHD.QtyUnit - dtl.QtyUnit;
                        db.Entry(wasteWarehouseHD).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
                // Update ห้องของเสีย
                //Library.AddWasteWarehouse(details, cnw.Code);

                // Open Paper WasteCN
                //PaperCNWasteViewer obj = new PaperCNWasteViewer(this, cnw.Code);
                //obj.ShowDialog();
                frmMainReport mr = new frmMainReport(this, cnw.Code);
                mr.Show();
                ResetForm();
            }
            catch (Exception)
            {
                MessageBox.Show("พบข้อมผิดพลาด");
            }
        }

    }
}
