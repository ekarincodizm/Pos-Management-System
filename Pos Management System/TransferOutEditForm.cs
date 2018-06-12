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
    public partial class TransferOutEditForm : Form
    {
        private string code;
        private TransferOutListForm transferOutListForm;

        public TransferOutEditForm()
        {
            InitializeComponent();
        }

        public TransferOutEditForm(TransferOutListForm transferOutListForm, string code)
        {
            InitializeComponent();
            this.transferOutListForm = transferOutListForm;
            this.code = code;
        }
        int? _EmployeeId;
        public void BinddingEmployee(Employee send)
        {
            _EmployeeId = send.Id;
            textBoxEmpCode.Text = send.Code;
            textBoxEmpName.Text = send.Name;
        }

        /// <summary>
        ///  Bindding datagrid from  StoreFrontTransferOutDtl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TransferOutEditForm_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.StoreFrontTransferOut.SingleOrDefault(w => w.Code == code);
                textBoxCode.Text = data.Code;
                textBoxTODate.Text = Library.ConvertDateToThaiDate(data.CreateDate);

                if (data.FKEmployee != null)
                {
                    textBoxEmpCode.Text = data.Employee.Code;
                    textBoxEmpName.Text = data.Employee.Name;
                    _EmployeeId = data.FKEmployee;
                }

                if (data.Enable == true && data.ConfirmDate == null)
                {

                }
                else
                {
                    button2.Enabled = false;
                }
                // binddinfg grid1
                foreach (var item in data.StoreFrontTransferOutDtl.Where(w => w.Enable == true).ToList())
                {
                    dataGridView1.Rows.Add(item.FKProductDetails, item.ProductDetails.Code, "",
                        item.ProductDetails.Products.ThaiName,
                        item.ProductDetails.ProductUnit.Name,
                        Library.ConvertDecimalToStringForm(item.Qty),
                        item.ProductDetails.PackSize,
                        Library.ConvertDecimalToStringForm(item.ProductDetails.CostAndVat),
                        Library.ConvertDecimalToStringForm(item.TotalPrice),
                        item.ProductDetails.Products.ProductVatType.Name,
                        item.Description
                        );
                }
            }
            CalSummary();
        }
        int colId = 0;
        int colCode = 1;
        int colSearch = 2;
        int colName = 3;
        int colUnit = 4;
        int colQty = 5;
        int colPZ = 6;
        int colCostPerUnit = 7;
        int colAllPrice = 8;
        int colVatType = 9;
        int colDescription = 10;
        private void CalSummary()
        {
            try
            {
                decimal qty = 0;
                decimal hasvat = 0;
                decimal unvat = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    qty += decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    string code = dataGridView1.Rows[i].Cells[colCode].Value.ToString();
                    //int fkProdDtl = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    var prodDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Code == code && w.Enable == true);
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
                    if (i >= dataGridView1.Rows.Count - 2) break;
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// ยืนยัน บันทีก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
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
        /// <summary>
        /// การบันทึก disable อันเดิมทิ้ง
        /// </summary>
        private void SaveCommit()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.StoreFrontTransferOut.SingleOrDefault(w => w.Code == code);
                foreach (var item in data.StoreFrontTransferOutDtl.Where(w=>w.Enable == true).ToList())
                {
                    item.Enable = false;
                    item.UpdateDate = DateTime.Now;
                    item.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    db.Entry(item).State = EntityState.Modified;
                }
                /// Update 
                data.UpdateDate = DateTime.Now;
                data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
               
                List<StoreFrontTransferOutDtl> details = new List<StoreFrontTransferOutDtl>();
                StoreFrontTransferOutDtl detail;
                decimal totalPiece = 0;
                decimal totalUnit = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    detail = new StoreFrontTransferOutDtl();
                    detail.FKTransferOut = data.Code;
                    detail.Enable = true;
                    detail.Description = dataGridView1.Rows[i].Cells[colDescription].Value.ToString();
                    detail.CreateDate = DateTime.Now;
                    detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.UpdateDate = DateTime.Now;
                    detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.FKProductDetails = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    var prodDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == detail.FKProductDetails);

                    detail.Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    detail.CostPerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                    detail.BeforeVat = 0;
                    detail.Vat = 0;
                    detail.TotalPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colAllPrice].Value.ToString());
                    details.Add(detail);
                    totalPiece += (detail.Qty * prodDtl.PackSize);
                    totalUnit += detail.Qty;

                    if (i >= dataGridView1.Rows.Count - 2) break;
                }
                db.StoreFrontTransferOutDtl.AddRange(details);
                data.TotalQty = totalPiece;
                data.TotalQtyUnit = totalUnit;
                data.TotalUnVat = decimal.Parse(textBoxTotalUnVat.Text);
                data.TotalBeforeVat = decimal.Parse(textBoxTotalBeforeVat.Text);
                data.TotalVat = decimal.Parse(textBoxTotalVat.Text);
                data.FKEmployee = _EmployeeId;      
                data.TotalBalance = decimal.Parse(textBoxTotalBalance.Text);
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                this.transferOutListForm.ReloadGrid();
                this.Dispose();
            }
        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var code = dataGridView1.Rows[e.RowIndex].Cells[colCode].Value.ToString();
                ProductDetails getProd = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == code);
                switch (e.ColumnIndex)
                {
                    case 5:
                        decimal qty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                        decimal cost = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value.ToString());
                        dataGridView1.Rows[e.RowIndex].Cells[colAllPrice].Value = Library.ConvertDecimalToStringForm(qty * cost);
                        using (SSLsEntities db = new SSLsEntities())
                        {
                            var posStock = db.PosStockDetails.Where(w => w.Enable == true && w.FKProductDetails == getProd.Id).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                            if (qty > posStock.ResultQtyUnit)
                            {
                                MessageBox.Show("พบสินค้าใน Stock หน้าร้านจำวน : " + posStock.ResultQtyUnit);
                            }
                        }
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

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// เลือกพนักงานหยิบ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            SelectedEmployeePopup obj = new SelectedEmployeePopup(this);
            obj.ShowDialog();
        }
        /// <summary>
        /// คลิ๊ก เลือกสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 2:
                    SelectedProductPopup obj = new SelectedProductPopup(this);
                    obj.ShowDialog();
                    break;
                default:
                    break;
            }
        }
        public void BinddingProductChoose(int id)
        {
            List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
            var product = products.SingleOrDefault(w => w.Id == id);
            // start bind ui
            int currentRow = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows[currentRow].Cells[colId].Value = product.Id;
            dataGridView1.Rows[currentRow].Cells[colCode].Value = product.Code;
            dataGridView1.Rows[currentRow].Cells[colName].Value = product.Products.ThaiName;
            dataGridView1.Rows[currentRow].Cells[colUnit].Value = product.ProductUnit.Name;
            dataGridView1.Rows[currentRow].Cells[colQty].Value = 1.00;
            dataGridView1.Rows[currentRow].Cells[colPZ].Value = product.PackSize;
            dataGridView1.Rows[currentRow].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostAndVat);
            dataGridView1.Rows[currentRow].Cells[colAllPrice].Value = Library.ConvertDecimalToStringForm(product.CostAndVat);
            dataGridView1.Rows[currentRow].Cells[colVatType].Value = product.Products.ProductVatType.Name;
            dataGridView1.Rows[currentRow].Cells[colDescription].Value = "-";
            //dataGridView1.Rows[currentRow].Cells[colQty].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[colQty];
            dataGridView1.BeginEdit(true);
            CalSummary();
        }
    }
}
