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
    public partial class SearchEditDiscountDay : Form
    {
        private int id;

        public SearchEditDiscountDay()
        {
            InitializeComponent();
        }

        public SearchEditDiscountDay(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void SearchEditDiscountDay_Load(object sender, EventArgs e)
        {
            Reload();
        }

        private void Reload()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.PriceSchedule.SingleOrDefault(w => w.Id == id);
                // bindding grid
                foreach (var item in data.SellingPrice.Where(w => w.Enable == true).ToList())
                {
                    decimal diffBath = item.ProductDetails.SellPrice - item.SpecialPrice;
                    decimal percent = diffBath * 100 / item.ProductDetails.SellPrice;
                    dataGridView1.Rows.Add(item.ProductDetails.Code, item.ProductDetails.Products.ThaiName, item.ProductDetails.ProductUnit.Name, Library.ConvertDecimalToStringForm(item.ProductDetails.SellPrice), Library.ConvertDecimalToStringForm(item.SpecialPrice), Library.ConvertDecimalToStringForm(percent) + " %", Library.ConvertDecimalToStringForm(diffBath), item.Description, item.Id);
                }
                RowColor();
                /// check หมดเขต หรือ disable
                if (data.Enable == false)
                {
                    // ยกเลิกแล้ว
                    button3.Enabled = false;
                    label1.Text = "* ยกเลิกแล้ว";
                    label1.Visible = true;
                }
                else if (data.IsStop == true)
                {
                    // หมดเขตแล้ว
                    button3.Enabled = false;
                    label1.Text = "* หมดเขตแล้ว";
                    label1.Visible = true;
                }
            }
        }

        private void RowColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((i % 2) == 0)
                {
                    // คู่
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    // คี่
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.AliceBlue;
                }
            }
        }
        /// <summary>
        /// Disable รายการนั้นทิ้งจาก แคมเปญ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    int rowCurrent = dataGridView1.CurrentRow.Index;
                    int idSelling = int.Parse(dataGridView1.Rows[rowCurrent].Cells[8].Value.ToString());
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        var data = db.SellingPrice.SingleOrDefault(w => w.Id == idSelling);
                        data.Enable = false;
                        data.UpdateDate = DateTime.Now;
                        data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        db.Entry(data).State = EntityState.Modified;
                        db.SaveChanges();
                        dataGridView1.Rows.Clear();
                        dataGridView1.Refresh();
                        Reload();
                    }
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
