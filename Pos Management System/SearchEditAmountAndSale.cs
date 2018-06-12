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
    public partial class SearchEditAmountAndSale : Form
    {
        private int id;

        public SearchEditAmountAndSale()
        {
            InitializeComponent();
        }

        public SearchEditAmountAndSale(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void SearchEditAmountAndSale_Load(object sender, EventArgs e)
        {
            Reload();
        }

        private void Reload()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.PriceSchedule.SingleOrDefault(w => w.Id == id);
                var selling = data.SellingPrice.Where(w => w.Enable == true).ToList();
                foreach (var item in selling)
                {
                    dataGridView1.Rows.Add(item.Id, item.ProductDetails.Code, item.ProductDetails.Products.ThaiName, item.ProductDetails.ProductUnit.Name, Library.ConvertDecimalToStringForm(item.ProductDetails.SellPrice), item.Description);
                }
                textBoxProAmount.Text = Library.ConvertDecimalToStringForm(data.FullPrice);
                foreach (SellingPriceDetails item in data.SellingPrice.FirstOrDefault().SellingPriceDetails.Where(w => w.Enable == true).ToList())
                {
                    dataGridView2.Rows.Add(item.Id, item.ProductDetails.Code, item.ProductDetails.Products.ThaiName, item.ProductDetails.ProductUnit.Name, Library.ConvertDecimalToStringForm(item.ProductDetails.SellPrice), Library.ConvertDecimalToStringForm(item.SpecialPrice), item.Description, item.FKProduct);
                }
                /// check หมดเขต หรือ disable
                if (data.Enable == false)
                {
                    // ยกเลิกแล้ว
                    button3.Enabled = false;
                    label1.Text = "* ยกเลิกแล้ว";
                    label1.Visible = true;

                    button2.Enabled = false;
                    label2.Text = "* ยกเลิกแล้ว";
                    label2.Visible = true;
                }
                else if (data.IsStop == true)
                {
                    // หมดเขตแล้ว
                    button3.Enabled = false;
                    label1.Text = "* หมดเขตแล้ว";
                    label1.Visible = true;

                    button2.Enabled = false;
                    label2.Text = "* หมดเขตแล้ว";
                    label2.Visible = true;
                }
            }
            RowColor();
            RowColor(2);
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
        private void RowColor(int number)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if ((i % 2) == 0)
                {
                    // คู่
                    dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    // คี่
                    dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.AliceBlue;
                }
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
        /// ลบสินค้า ที่ selling price
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
                    int rowCurrent = dataGridView1.CurrentRow.Index;
                    int idSelling = int.Parse(dataGridView1.Rows[rowCurrent].Cells[0].Value.ToString());
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        var data = db.SellingPrice.SingleOrDefault(w => w.Id == idSelling);
                        data.Enable = false;
                        data.UpdateDate = DateTime.Now;
                        data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        db.Entry(data).State = EntityState.Modified;

                        db.SaveChanges();
                        Reload();
                    }
                    break;
                case DialogResult.No:
                    break;
            }
        }
        /// <summary>
        /// ลบสินค้าที่ SellingPriceDetails
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
                    int rowCurrent = dataGridView2.CurrentRow.Index;
                    int idSellingDtl = int.Parse(dataGridView2.Rows[rowCurrent].Cells[0].Value.ToString()); // id
                    int fkProdDtl = int.Parse(dataGridView2.Rows[rowCurrent].Cells[7].Value.ToString()); // id product dtl
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        var priceSched = db.PriceSchedule.SingleOrDefault(w => w.Id == id);
                        foreach (var selling in priceSched.SellingPrice)
                        {
                            foreach (var item in selling.SellingPriceDetails)
                            {
                                // Disable
                                if (fkProdDtl == item.FKProduct)
                                {
                                    item.Enable = false;
                                    item.UpdateDate = DateTime.Now;
                                    item.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                                    db.Entry(item).State = EntityState.Modified;
                                }
                            }
                        }
                        db.SaveChanges();
                        Reload();
                    }
                    break;
                case DialogResult.No:
                    break;
            }
        }
    }
}
