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
    public partial class StoreFrontAddForm : Form
    {
        public StoreFrontAddForm()
        {
            InitializeComponent();
        }

        private void StoreFrontAddForm_Load(object sender, EventArgs e)
        {
            textBoxDate.Text = Library.ConvertDateToThaiDate(DateTime.Now);
            textBoxUserId.Text = Singleton.SingletonAuthen.Instance().Id;
            textBoxUserName.Text = Singleton.SingletonAuthen.Instance().Name;
            string code = "";
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var iss = db.ISS2Front.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                BudgetYear budget = Singleton.SingletonThisBudgetYear.Instance().ThisYear;
                //Branch branch = Singleton.SingletonP
                code = MyConstant.PrefixForGenerateCode.StoreFrontAdd + "" + budget.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(iss, 4);
                textBoxCode.Text = code;
            }
        }
        /// <summary>
        /// บันทึก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            try
            {
                decimal qty = 0;
                decimal qtyPiece = 0;
                decimal hasvat = 0;
                decimal unvat = 0;
                List<ISS2FrontDetails> details = new List<ISS2FrontDetails>();
                ISS2FrontDetails detail;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    detail = new ISS2FrontDetails();
                    qty += decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());

                    int fkProdDtl = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    var prodDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == fkProdDtl);
                    qtyPiece += prodDtl.PackSize * decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    //if (prodDtl.Products.ProductVatType.Id == MyConstant.ProductVatType.HasVat)
                    //{
                    //    // ถ้าเป็นสินค้ามีภาษี
                    //    //hasvat += decimal.Parse(dataGridView1.Rows[i].Cells[colAllPrice].Value.ToString());
                    //}
                    //else
                    //{
                    //    // ถ้าเป็นสินค้า ยกเว้นภาษี
                    //    //unvat += decimal.Parse(dataGridView1.Rows[i].Cells[colAllPrice].Value.ToString());
                    //}
                    string desc = dataGridView1.Rows[i].Cells[colDescription].Value.ToString();
                    detail.Enable = true;
                    detail.Description = desc;
                    detail.CreateDate = DateTime.Now;
                    detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.UpdateDate = DateTime.Now;
                    detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.FKProductDetails = fkProdDtl;
                    detail.Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    detail.QtyAllow = 0;
                    detail.PricePerUnit = prodDtl.SellPrice;
                    detail.BeforeVat = 0;
                    detail.Vat = 0;
                    detail.TotalPrice = detail.Qty * prodDtl.SellPrice;
                    detail.FKShelf  = int.Parse(dataGridView1.Rows[i].Cells[colShelfId].Value.ToString());
                    details.Add(detail);
                    if (i >= dataGridView1.Rows.Count - 2) break;
                }
                ISS2Front iss = new ISS2Front();
                iss.Code = textBoxCode.Text;
                iss.Enable = true;
                iss.Description = textBoxDesc.Text;
                iss.CreateDate = DateTime.Now;
                iss.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                iss.UpdateDate = DateTime.Now;
                iss.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                iss.FKEmployee = _EmployeeId;
                iss.TotalQty = decimal.Parse(textBoxTotalQty.Text);
                iss.TotalQtyUnit = decimal.Parse(textBoxQtyUnit.Text);
                iss.TotalUnVat = 0;
                iss.TotalBeforeVat = 0;
                iss.TotalVat = 0;
                iss.TotalBalance = 0;
                iss.FKISS2FrontStatus = MyConstant.ISS2FrontStatus.CreateOrder;
                iss.ISS2FrontDetails = details;
                using (SSLsEntities db = new SSLsEntities())
                {
                    db.ISS2Front.Add(iss);
                    db.SaveChanges();
                    MessageBox.Show("บันทึก " + iss.Code + " เรียบร้อย");

                    ResetForm();

                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("พบข้อมูลผิดพลาด");
            }
        }

        private void ResetForm()
        {
            string code = "";
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var iss = db.ISS2Front.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                BudgetYear budget = Singleton.SingletonThisBudgetYear.Instance().ThisYear;
                //Branch branch = Singleton.SingletonP
                code = MyConstant.PrefixForGenerateCode.StoreFrontAdd + "" + budget.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(iss, 4);
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
        /// <summary>
        /// พนักงานหยิบ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SelectedEmployeePopup obj = new SelectedEmployeePopup(this);
            obj.ShowDialog();
        }
        int? _EmployeeId = null;
        public void BinddingEmployee(Employee send)
        {
            _EmployeeId = send.Id;
            textBoxEmpCode.Text = send.Code;
            textBoxEmpName.Text = send.Name;
        }
        /// <summary>
        /// Click DataGrid1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            switch (e.ColumnIndex)
            {
                case 2:
                    SelectedProductPopup obj = new SelectedProductPopup(this);
                    obj.ShowDialog();
                    break;
                case 9:
                    SelectedShelfPopup shelf = new SelectedShelfPopup(this);
                    shelf.ShowDialog();
                    break;
                default:
                    break;
            }
        }
        int colId = 0;
        int colCode = 1;
        int colSearch = 2;
        int colName = 3;
        int colUnit = 4;
        int colQty = 5;
        int colPricePerUnit = 6;
        int colPZ = 7;
        int colDescription = 8;
        int colShelf = 9;
        int colShelfName = 10;
        int colShelfId = 11;
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
            dataGridView1.Rows[currentRow].Cells[colPricePerUnit].Value = Library.ConvertDecimalToStringForm(product.SellPrice); // ราคาขาย / หน่วย
            dataGridView1.Rows[currentRow].Cells[colPZ].Value = product.PackSize;
            dataGridView1.Rows[currentRow].Cells[colDescription].Value = "-";
            //dataGridView1.Rows[currentRow].Cells[colQty].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[colQty];
            //dataGridView1.CurrentCell.Selected = true;
            dataGridView1.BeginEdit(true);
        }
        public void BinddingShelfChoose(Shelf send)
        {
            int currentRow = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows[currentRow].Cells[colShelfName].Value = send.Name;
            dataGridView1.Rows[currentRow].Cells[colShelfId].Value = send.Id;
        }
        /// <summary>
        /// คีย์ Cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                switch (e.ColumnIndex)
                {
                    case 5: // คีย์จำนวน
                            // เชค Stock Wms ก่อน ว่าของมีแค่ไหน

                        int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[colId].Value.ToString());
                        decimal qtyUnit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                        using (SSLsEntities db = new SSLsEntities())
                        {
                            var stockWms = db.WmsStockDetail.Where(w => w.Enable == true && w.FKProductDetail == id).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                            Console.WriteLine("row " + e.RowIndex + " Want: " + qtyUnit + " stock: " + stockWms.ResultQtyUnit);
                            if (qtyUnit > stockWms.ResultQtyUnit)
                            {
                                MessageBox.Show("Stock คงเหลือ " + stockWms.ResultQtyUnit);
                                //dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[colQty];
                                //dataGridView1.CurrentCell.Selected = true;
                            }
                            else
                            {

                            }
                        }
                        CalSummary();
                        break;
                    default:
                        break;
                }
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
                decimal qtyPiece = 0;
                decimal hasvat = 0;
                decimal unvat = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    qty += decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());

                    int fkProdDtl = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    var prodDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == fkProdDtl);
                    qtyPiece += prodDtl.PackSize * decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    //if (prodDtl.Products.ProductVatType.Id == MyConstant.ProductVatType.HasVat)
                    //{
                    //    // ถ้าเป็นสินค้ามีภาษี
                    //    //hasvat += decimal.Parse(dataGridView1.Rows[i].Cells[colAllPrice].Value.ToString());
                    //}
                    //else
                    //{
                    //    // ถ้าเป็นสินค้า ยกเว้นภาษี
                    //    //unvat += decimal.Parse(dataGridView1.Rows[i].Cells[colAllPrice].Value.ToString());
                    //}
                    if (i >= dataGridView1.Rows.Count - 2) break;
                }
                //decimal vat = hasvat * MyConstant.MyVat.Vat / 100;
                //decimal total = vat + hasvat + unvat;
                //textBoxQtyUnit.Text = Library.ConvertDecimalToStringForm(qty);
                //textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(unvat);
                //textBoxTotalBeforeVat.Text = Library.ConvertDecimalToStringForm(hasvat);
                //textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(vat);
                //textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(total);
                textBoxTotalQty.Text = Library.ConvertDecimalToStringForm(qtyPiece);
                textBoxQtyUnit.Text = Library.ConvertDecimalToStringForm(qty);
            }
            catch (Exception)
            {

            }
        }
    }
}
