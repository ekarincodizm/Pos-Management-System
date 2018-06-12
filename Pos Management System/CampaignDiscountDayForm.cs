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
    public partial class CampaignDiscountDayForm : Form
    {
        public CampaignDiscountDayForm()
        {
            InitializeComponent();
        }

        int colCode = 0;
        int colShearch = 1;
        int colName = 2;
        int colUnit = 3;
        int colCurrentPrice = 4;
        int colSpecialPrice = 5;
        int colDiffPercent = 6;
        int colDiffBath = 7;
        int colRemark = 8;
        int colId = 9;

        /// <summary>
        /// เมื่อมีการ Tab data grid
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
                            //dataGridView1.Rows[e.RowIndex].Cells[colCode].Value = p.Code; // code
                            dataGridView1.Rows[e.RowIndex].Cells[colName].Value = p.Products.ThaiName; // ชื่อ
                            dataGridView1.Rows[e.RowIndex].Cells[colUnit].Value = p.ProductUnit.Name; // หน่วย
                            dataGridView1.Rows[e.RowIndex].Cells[colCurrentPrice].Value = Library.ConvertDecimalToStringForm(p.SellPrice); // ราคาปกติ
                            dataGridView1.Rows[e.RowIndex].Cells[colSpecialPrice].Value = "0.00"; // ราคาพิเศษ
                            dataGridView1.Rows[e.RowIndex].Cells[colDiffPercent].Value = "0.00 %"; // ส่วนต่าง %
                            dataGridView1.Rows[e.RowIndex].Cells[colDiffBath].Value = "0.00"; // ส่วนต่าง บาท
                            dataGridView1.Rows[e.RowIndex].Cells[colRemark].Value = "-"; // หมายเหตุ
                        }
                        break;
                    case 5:
                        // ราคา ปกติ
                        decimal price = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCurrentPrice].Value.ToString());
                        // ส่วนต่าง ได้จาก ราคาปกติ - ราคาพิเศษ
                        decimal sPrice = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colSpecialPrice].Value.ToString());
                        if (sPrice >= price)
                        {
                            MessageBox.Show("คุณแน่ใจหรือไม่ ? ราคาโปรโมชั่นมากว่าหรือเท่ากันราคาปกติ");
                        }
                        // set diff bath
                        dataGridView1.Rows[e.RowIndex].Cells[colDiffBath].Value = Library.ConvertDecimalToStringForm(price - sPrice);
                        // เงินต่าง
                        decimal diffBath = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colDiffBath].Value.ToString());
                        decimal percent = diffBath * 100 / price;
                        dataGridView1.Rows[e.RowIndex].Cells[colDiffPercent].Value = Library.ConvertDecimalToStringForm(percent) + " %";
                        break;
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// บันทึก แคมเปญ
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
                    // check วันที่ย้อนหลัง
                    //DateTime dateStart = dateTimePickerStart.Value;
                    //if (int.Parse(dateStart.ToString("yyyyMMdd")) < int.Parse(DateTime.Now.ToString("yyyyMMdd"))) 
                    //{
                    //    // ถ้าตั้งค่าน้อยกว่าวันปัจจุบัน 
                    //    MessageBox.Show("ไม่สามารถตั้งค่าย้อนหลังได้");
                    //    return;
                    //}
                    SaveCommit();
                    break;
                case DialogResult.No:
                    break;
            }
        }
        void SaveCommit()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                PriceSchedule ps = new PriceSchedule();
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var pro = db.PriceSchedule.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                string code = SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(pro, 3);

                ps.Code = code;
                ps.Enable = true;
                ps.Name = textBoxProName.Text;
                ps.Description = textBoxRemark.Text;
                ps.CreateDate = DateTime.Now;
                ps.CreateBy = SingletonAuthen.Instance().Id;
                ps.UpdateDate = DateTime.Now;
                ps.UpdateBy = SingletonAuthen.Instance().Id;
                ps.FKCampaign = MyConstant.CampaignType.DiscountDay;
                // dd/MM/yyyy
                //string[] date = dateTimePickerStart.Text.Split('/');
                //ps.StartDate = Library.ConvertDateTime(date[0], date[1], date[2]);
                ps.StartDate = dateTimePickerStart.Value;
                //date = dateTimePickerEnd.Text.Split('/');
                //ps.EndDate = Library.ConvertDateTime(date[0], date[1], date[2]);
                ps.EndDate = dateTimePickerEnd.Value;
                ps.Limited = 0;
                ps.Notice = textBoxProNotice.Text;
                ps.FullPrice = 0;
                ps.FullQty = 0;
                ps.Discount = 0;
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
                        SpecialPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colSpecialPrice].Value.ToString()),
                        UpdateDate = DateTime.Now,
                        UpdateBy = SingletonAuthen.Instance().Id
                    });
                    if (i >= dataGridView1.Rows.Count - 2) break;
                }
                ps.SellingPrice = details;
                db.PriceSchedule.Add(ps);
                db.SaveChanges();
                // reset all
                ResetForm();
            }
        }
        void ResetForm()
        {
            textBoxProName.Text = "";
            textBoxProCode.Text = "";
            textBoxProNotice.Text = "";
            dateTimePickerStart.Value = DateTime.Now;
            dateTimePickerEnd.Value = DateTime.Now;
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
        private void CampaignDiscountDayForm_Load(object sender, EventArgs e)
        {
            dateTimePickerStart.MinDate = DateTime.Now;
            SingletonProduct.Instance();
            //using (SSLsEntities db = new SSLsEntities())
            //{
            //    string proCodeGen = SingletonPriority1.Instance().ThisBudgetYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(db.PriceSchedule.Count() + 1, 3);
            //    textBoxProCode.Text = proCodeGen;
            //}
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
        /// คลิ๊กปุ่ม ค้นหา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Bindding from selected
        /// </summary>
        /// <param name="details"></param>
        public void BinddingSelectedProduct(ProductDetails details)
        {
            int cuurentRow = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows[cuurentRow].Cells[colId].Value = details.Id; // Id
            dataGridView1.Rows[cuurentRow].Cells[colCode].Value = details.Code; // code
            dataGridView1.Rows[cuurentRow].Cells[colName].Value = details.Products.ThaiName; // ชื่อ
            dataGridView1.Rows[cuurentRow].Cells[colUnit].Value = details.ProductUnit.Name; // หน่วย
            dataGridView1.Rows[cuurentRow].Cells[colCurrentPrice].Value = Library.ConvertDecimalToStringForm(details.SellPrice); // ราคาปกติ
            dataGridView1.Rows[cuurentRow].Cells[colSpecialPrice].Value = "0.00"; // ราคาพิเศษ
            dataGridView1.Rows[cuurentRow].Cells[colDiffPercent].Value = "0.00 %"; // ส่วนต่าง %
            dataGridView1.Rows[cuurentRow].Cells[colDiffBath].Value = "0.00"; // ส่วนต่าง บาท
            dataGridView1.Rows[cuurentRow].Cells[colRemark].Value = "-"; // หมายเหตุ

            dataGridView1.CurrentCell = dataGridView1.Rows[cuurentRow].Cells[colSpecialPrice];
            dataGridView1.BeginEdit(true);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
    }
}
