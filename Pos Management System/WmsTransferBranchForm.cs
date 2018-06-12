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
    public partial class WmsTransferBranchForm : Form
    {
        public WmsTransferBranchForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// เลือก Branch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SelectedBranchPopup obj = new SelectedBranchPopup(this);
            obj.ShowDialog();
        }
        /// <summary>
        /// Bindding สาขาที่เลือก
        /// </summary>
        int _BranchId;
        public void BinddingBranchSelected(Branch send)
        {
            textBoxBrachCode.Text = send.Code;
            textBoxBranchName.Text = send.Name;
            _BranchId = send.Id;
        }

        private void WmsTransferBranchForm_Load(object sender, EventArgs e)
        {
            string code = "";
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var wto = db.WmsTransferOut.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                BudgetYear budget = Singleton.SingletonThisBudgetYear.Instance().ThisYear;
                //Branch branch = Singleton.SingletonP
                code = MyConstant.PrefixForGenerateCode.WmsTransferOut + "" + budget.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(wto, 4);
                textBoxCode.Text = code;
                textBoxTODate.Text = Library.ConvertDateToThaiDate(DateTime.Now);
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
        /// click cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            dataGridView1.Rows[currentRow].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostOnly);
            dataGridView1.Rows[currentRow].Cells[colAllPrice].Value = Library.ConvertDecimalToStringForm(product.CostOnly);
            dataGridView1.Rows[currentRow].Cells[colVatType].Value = product.Products.ProductVatType.Name;
            dataGridView1.Rows[currentRow].Cells[colDescription].Value = "-";
            //dataGridView1.Rows[currentRow].Cells[colQty].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[colQty];
            dataGridView1.BeginEdit(true);
        }
        /// <summary>
        /// เมื่อคีย์ cell qty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                switch (e.ColumnIndex)
                {
                    case 5:
                        decimal qty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
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
        /// <summary>
        /// เลือก Employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            SelectedEmployeePopup obj = new SelectedEmployeePopup(this);
            obj.ShowDialog();
        }
        int _EmployeeId;
        public void BinddingEmployee(Employee send)
        {
            _EmployeeId = send.Id;
            textBoxEmpCode.Text = send.Code;
            textBoxEmpName.Text = send.Name;
        }

        private void textBoxEmpCode_KeyUp(object sender, KeyEventArgs e)
        {
            string key = textBoxEmpCode.Text.Trim();

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    var emp = Singleton.SingletonEmployee.Instance().Employee.FirstOrDefault(w => w.Enable == true && w.Code == key);
                    if (emp != null)
                    {
                        BinddingEmployee(emp);
                    }
                    else
                    {
                        MessageBox.Show("ไม่พบข้อมูล");
                    }
                    break;
                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// บันทึก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
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
                WmsTransferOut wto = new WmsTransferOut();
                wto.Code = textBoxCode.Text;
                wto.Enable = true;
                wto.Description = textBoxDesc.Text;
                wto.CreateDate = DateTime.Now;
                wto.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                wto.UpdateDate = DateTime.Now;
                wto.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                wto.FK2Branch = _BranchId;
                decimal qtyPiece = 0;
                List<WmsTransferOutDetails> details = new List<WmsTransferOutDetails>();
                WmsTransferOutDetails detail;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    detail = new WmsTransferOutDetails();
                    qtyPiece += decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString()) * decimal.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString());
                    detail.Enable = true;
                    detail.Description = dataGridView1.Rows[i].Cells[colDescription].Value.ToString();
                    detail.CreateDate = DateTime.Now;
                    detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.UpdateDate = DateTime.Now;
                    detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.FKProductDetails = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    detail.Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    detail.PricePerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                    detail.BeforeVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString()) * detail.Qty;
                    detail.Vat = ((detail.BeforeVat * detail.Qty) * MyConstant.MyVat.Vat) / 100;
                    detail.TotalPrice = detail.BeforeVat + detail.Vat;
                    details.Add(detail);
                    if (i >= dataGridView1.Rows.Count - 2) break;
                }

                wto.TotalQty = qtyPiece;
                wto.TotalQtyUnit = decimal.Parse(textBoxQtyUnit.Text);
                wto.TotalUnVat = decimal.Parse(textBoxTotalUnVat.Text);
                wto.TotalBeforeVat = decimal.Parse(textBoxTotalBeforeVat.Text);
                wto.TotalVat = decimal.Parse(textBoxTotalVat.Text);
                wto.TotalBalance = decimal.Parse(textBoxTotalBalance.Text);
                wto.WmsTransferOutDetails = details;
                wto.FKEmployee = _EmployeeId;
                using (SSLsEntities db = new SSLsEntities())
                {
                    db.WmsTransferOut.Add(wto);
                    db.SaveChanges();
                    // ManageStock Wms
                    Library.MakeValueForUpdateStockWms(details);
                    MessageBox.Show("บันทึกเรียบร้อย " + wto.Code);
                    ResetForm();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("พบข้อมผิดพลาด");
            }
        }

        private void ResetForm()
        {
            string code = "";
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var wto = db.WmsTransferOut.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                BudgetYear budget = Singleton.SingletonThisBudgetYear.Instance().ThisYear;
                //Branch branch = Singleton.SingletonP
                code = MyConstant.PrefixForGenerateCode.WmsTransferOut + "" + budget.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(wto, 4);
                textBoxCode.Text = code;
                textBoxTODate.Text = Library.ConvertDateToThaiDate(DateTime.Now);
            }
        }
    }
}
