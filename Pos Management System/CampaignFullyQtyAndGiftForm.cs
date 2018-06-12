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
    public partial class CampaignFullyQtyAndGiftForm : Form
    {
        public CampaignFullyQtyAndGiftForm()
        {
            InitializeComponent();
        }
        int col1Code = 0;
        int col1Search = 1;
        int col1Name = 2;
        int col1Unit = 3;
        int col1Price = 4;
        int col1Qty = 5;
        int col1Id = 6;

        int col2Code = 0;
        int col2Search = 1;
        int col2Name = 2;
        int col2Unit = 3;
        int col2Price = 4;
        int col2Qty = 5;
        int col2Id = 6;
        private void CampaignFullyQtyAndGiftForm_Load(object sender, EventArgs e)
        {
            SingletonProduct.Instance();
            dateTimePickerStart.MinDate = DateTime.Now;
            var thisBudget = SingletonThisBudgetYear.Instance().ThisYear;
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var pro = db.PriceSchedule.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                textBoxProCode.Text = thisBudget.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(pro, 3);
            }
        }
        /// <summary>
        /// กริด1 คีย์ สินค้า
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
                        if (dataGridView1.Rows[e.RowIndex].Cells[col1Code].Value != null)
                        {
                            pNo = dataGridView1.Rows[e.RowIndex].Cells[col1Code].Value.ToString();
                        }
                        p = products.FirstOrDefault(w => w.Code == pNo);
                        if (p != null)
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[col1Id].Value = p.Id; // Id
                            dataGridView1.Rows[e.RowIndex].Cells[col1Name].Value = p.Products.ThaiName; // ชื่อ
                            dataGridView1.Rows[e.RowIndex].Cells[col1Unit].Value = p.ProductUnit.Name; // หน่วย
                            dataGridView1.Rows[e.RowIndex].Cells[col1Price].Value = Library.ConvertDecimalToStringForm(p.SellPrice); // ราคา/หน่วย
                        }
                        break;
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// กริด2 คีย์ สินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellValidated(object sender, DataGridViewCellEventArgs e)
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
                        if (dataGridView2.Rows[e.RowIndex].Cells[col2Code].Value != null)
                        {
                            pNo = dataGridView2.Rows[e.RowIndex].Cells[col2Code].Value.ToString();
                        }
                        p = products.FirstOrDefault(w => w.Code == pNo);
                        if (p != null)
                        {
                            dataGridView2.Rows[e.RowIndex].Cells[col2Id].Value = p.Id; // Id
                            dataGridView2.Rows[e.RowIndex].Cells[col2Name].Value = p.Products.ThaiName; // ชื่อ
                            dataGridView2.Rows[e.RowIndex].Cells[col2Unit].Value = p.ProductUnit.Name; // หน่วย
                            dataGridView2.Rows[e.RowIndex].Cells[col2Price].Value = Library.ConvertDecimalToStringForm(p.SellPrice); // ราคา/หน่วย
                        }
                        break;
                }
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

        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// บันทึก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            var thisBudget = SingletonThisBudgetYear.Instance().ThisYear;
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var pro = db.PriceSchedule.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                string code = thisBudget.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(pro, 3);

                PriceSchedule header = new PriceSchedule();
                header.Code = code;
                header.Enable = true;
                header.Name = textBoxProName.Text;
                header.Description = textBoxRemark.Text;
                header.CreateDate = DateTime.Now;
                header.CreateBy = SingletonAuthen.Instance().Id;
                header.UpdateBy = SingletonAuthen.Instance().Id;
                header.UpdateDate = DateTime.Now;
                header.FKCampaign = MyConstant.CampaignType.FullyQtyAndGift;
                header.StartDate = dateTimePickerStart.Value;
                header.EndDate = dateTimePickerEnd.Value;
                header.Limited = 0;
                header.Notice = textBoxProNotice.Text;
                header.FullPrice = 0;
                header.FullQty = 0;
                header.Discount = 0;
                header.IsStop = false;
                header.StopReason = null;

                /// ต้อง Row เท่ากัน
                if (dataGridView1.Rows.Count == dataGridView2.Rows.Count)
                {
                    SellingPrice selling;
                    SellingPriceDetails details;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        selling = new SellingPrice();
                        selling.CreateDate = DateTime.Now;
                        selling.CreateBy = SingletonAuthen.Instance().Id;
                        selling.Enable = true;
                        selling.Name = header.Name;
                        selling.Description = null;
                        selling.UpdateDate = DateTime.Now;
                        selling.UpdateBy = SingletonAuthen.Instance().Id;
                        selling.FKProduct = int.Parse(dataGridView1.Rows[i].Cells[col1Id].Value.ToString());
                        selling.GetCurrentPrice = decimal.Parse(dataGridView1.Rows[i].Cells[col1Price].Value.ToString());
                        selling.SpecialPrice = 0;
                        selling.FullyQty = decimal.Parse(dataGridView1.Rows[i].Cells[col1Qty].Value.ToString());

                        /// ส่วนของแถม
                        details = new SellingPriceDetails();
                        details.Enable = true;
                        details.Name = header.Name;
                        details.CreateDate = DateTime.Now;
                        details.CreateBy = SingletonAuthen.Instance().Id;
                        details.UpdateDate = DateTime.Now;
                        details.UpdateBy = SingletonAuthen.Instance().Id;
                        details.FKProduct = int.Parse(dataGridView2.Rows[i].Cells[col2Id].Value.ToString());
                        details.GiftQty = decimal.Parse(dataGridView2.Rows[i].Cells[col2Qty].Value.ToString());
                        details.GetCurrentPrice = decimal.Parse(dataGridView2.Rows[i].Cells[col2Price].Value.ToString());
                        details.SpecialPrice = 0;

                        selling.SellingPriceDetails.Add(details);
                        header.SellingPrice.Add(selling);
                        if (i >= dataGridView1.Rows.Count - 2) break;
                    }
                }
                else
                {
                    MessageBox.Show("พบข้อผิดพลาดกรุณาตรวจสอบดูอีกครั้ง");
                }
                db.PriceSchedule.Add(header);
                db.SaveChanges();
                // reset all
                ResetForm();
            }
        }

        private void ResetForm()
        {
            textBoxProName.Text = "";
            textBoxProCode.Text = "";
            textBoxProNotice.Text = "";
            dateTimePickerStart.Value = DateTime.Now;
            dateTimePickerEnd.Value = DateTime.Now;

            textBoxRemark.Text = "";
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
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
        /// <summary>
        /// grid 1 click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 1)
            {
                SelectedProductPopup obj = new SelectedProductPopup(this, 1);
                obj.ShowDialog();
            }
        }
        /// <summary>
        /// grid 2 click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 1)
            {
                SelectedProductPopup obj = new SelectedProductPopup(this, 2);
                obj.ShowDialog();
            }
        }
        /// <summary>
        /// bindding grid 1
        /// </summary>
        /// <param name="id"></param>
        public void BinddingGrid1(int id)
        {
            var proDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == id);
            int rowCurrent = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows[rowCurrent].Cells[col1Id].Value = proDtl.Id; // Id
            dataGridView1.Rows[rowCurrent].Cells[col1Code].Value = proDtl.Code; // Code
            dataGridView1.Rows[rowCurrent].Cells[col1Name].Value = proDtl.Products.ThaiName; // ชื่อ
            dataGridView1.Rows[rowCurrent].Cells[col1Unit].Value = proDtl.ProductUnit.Name; // หน่วย
            dataGridView1.Rows[rowCurrent].Cells[col1Price].Value = Library.ConvertDecimalToStringForm(proDtl.SellPrice); // ราคา/หน่วย
            dataGridView1.CurrentCell = dataGridView1.Rows[rowCurrent].Cells[col1Qty];
            dataGridView1.BeginEdit(true);
        }
        /// <summary>
        /// bindding grid 2
        /// </summary>
        /// <param name="id"></param>
        public void BinddingGrid2(int id)
        {
            var proDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == id);
            int rowCurrent = dataGridView2.CurrentRow.Index;
            dataGridView2.Rows[rowCurrent].Cells[col2Id].Value = proDtl.Id; // Id
            dataGridView2.Rows[rowCurrent].Cells[col2Code].Value = proDtl.Code; // Code
            dataGridView2.Rows[rowCurrent].Cells[col2Name].Value = proDtl.Products.ThaiName; // ชื่อ
            dataGridView2.Rows[rowCurrent].Cells[col2Unit].Value = proDtl.ProductUnit.Name; // หน่วย
            dataGridView2.Rows[rowCurrent].Cells[col2Price].Value = Library.ConvertDecimalToStringForm(proDtl.SellPrice); // ราคา/หน่วย
            dataGridView2.CurrentCell = dataGridView2.Rows[rowCurrent].Cells[col2Qty];
            dataGridView2.BeginEdit(true);
        }
    }
}
