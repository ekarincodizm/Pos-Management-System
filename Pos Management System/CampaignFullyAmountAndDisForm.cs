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
    public partial class CampaignFullyAmountAndDisForm : Form
    {
        int colCode = 0;
        int colShearch = 1;
        int colName = 2;
        int colUnit = 3;
        int colCurrentPrice = 4;
        int colRemark = 5;
        int colId = 6;
        public CampaignFullyAmountAndDisForm()
        {
            InitializeComponent();
        }

        private void CampaignFullyAmountAndDisForm_Load(object sender, EventArgs e)
        {
            dateTimePickerStart.MinDate = DateTime.Now;
            SingletonProduct.Instance();
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var pro = db.PriceSchedule.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                string proCodeGen = SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(pro, 3);

                textBoxProCode.Text = proCodeGen;
            }
        }
        /// <summary>
        /// Tab data grid 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            List<ProductDetails> products = new List<ProductDetails>();
            products = SingletonProduct.Instance().ProductDetails;

            if (e.RowIndex < 0)
                return;
            try
            {
                ProductDetails p = new ProductDetails();
                string pNo = "0";
                switch (e.ColumnIndex)
                {
                    case 0:
                        if (dataGridView1.Rows[e.RowIndex].Cells[colCode].Value != null)
                        {
                            pNo = dataGridView1.Rows[e.RowIndex].Cells[colCode].Value.ToString();
                        }
                        p = products.FirstOrDefault(w => w.Code == pNo);
                        if (p != null)
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[colId].Value = p.Id; // Id
                            dataGridView1.Rows[e.RowIndex].Cells[colName].Value = p.Products.ThaiName; // ชื่อ
                            dataGridView1.Rows[e.RowIndex].Cells[colUnit].Value = p.ProductUnit.Name; // หน่วย
                            dataGridView1.Rows[e.RowIndex].Cells[colCurrentPrice].Value = Library.ConvertDecimalToStringForm(p.SellPrice); // ราคาปกติ
                            dataGridView1.Rows[e.RowIndex].Cells[colRemark].Value = "-"; // หมายเหตุ
                        }
                        break;
                    case 5:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
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
            using (SSLsEntities db = new SSLsEntities())
            {
                PriceSchedule ps = new PriceSchedule();
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var pro = db.PriceSchedule.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                string proCodeGen = SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(pro, 3);

                ps.Code = proCodeGen;
                ps.Enable = true;
                ps.Name = textBoxProName.Text;
                ps.Description = textBoxRemark.Text;
                ps.CreateDate = DateTime.Now;
                ps.CreateBy = SingletonAuthen.Instance().Id;
                ps.UpdateDate = DateTime.Now;
                ps.UpdateBy = SingletonAuthen.Instance().Id;
                ps.FKCampaign = MyConstant.CampaignType.FullyAmountAndDiscount;
                // dd/MM/yyyy
                string[] date = dateTimePickerStart.Text.Split('/');
                ps.StartDate = Library.ConvertDateTime(date[0], date[1], date[2]);
                //ps.StartDate = DateTime.Parse(dateTimePickerStart.Text);
                date = dateTimePickerEnd.Text.Split('/');
                ps.EndDate = Library.ConvertDateTime(date[0], date[1], date[2]);
                //ps.EndDate = DateTime.Parse(dateTimePickerEnd.Text);
                ps.Limited = 0;
                ps.Notice = textBoxProNotice.Text;
                ps.FullPrice = decimal.Parse(textBoxProAmount.Text);
                ps.FullQty = 0;
                ps.Discount = decimal.Parse(textBoxProDiscount.Text);
                ps.IsStop = false;
                List<SellingPrice> details = new List<SellingPrice>();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    details.Add(new SellingPrice()
                    {
                        CreateBy = SingletonAuthen.Instance().Id,
                        CreateDate = DateTime.Now,
                        Description = dataGridView1.Rows[i].Cells[colRemark].Value.ToString(),
                        Enable = true,
                        FKProduct = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString()),
                        GetCurrentPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colCurrentPrice].Value.ToString()),
                        Name = textBoxProName.Text,
                        SpecialPrice = 0,
                        UpdateDate = DateTime.Now,
                        UpdateBy = SingletonAuthen.Instance().Id
                    });
                    if (i >= dataGridView1.Rows.Count - 2) break;
                }
                ps.SellingPrice = details;
                db.PriceSchedule.Add(ps);
                db.SaveChanges();
            }
            // reset all
            ResetForm();
        }

        private void ResetForm()
        {
            textBoxProName.Text = "";
            textBoxProCode.Text = "";
            textBoxProNotice.Text = "";
            dateTimePickerStart.Value = DateTime.Now;
            dateTimePickerEnd.Value = DateTime.Now;
            textBoxProAmount.Text = "";
            textBoxProDiscount.Text = "";
            textBoxRemark.Text = "";
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            // Generate Code New
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var pro = db.PriceSchedule.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                string code = SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(pro, 3);
                textBoxProCode.Text = code;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex == colShearch)
            {
                SelectedProductPopup obj = new SelectedProductPopup(this);
                obj.ShowDialog();
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
        /// Bindding from selected
        /// </summary>
        /// <param name="details"></param>
        public void BinddingSelectedProduct(ProductDetails details)
        {
            int cuurentRow = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows.Add(details.Code, "", details.Products.ThaiName, details.ProductUnit.Name, Library.ConvertDecimalToStringForm(details.SellPrice), "-", details.Id);

            dataGridView1.CurrentCell = dataGridView1.Rows[cuurentRow].Cells[colRemark];
            dataGridView1.BeginEdit(true);
            // new row 
        }
    }
}
