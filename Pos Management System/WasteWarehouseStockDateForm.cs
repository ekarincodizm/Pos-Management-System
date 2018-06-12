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
    public partial class WasteWarehouseStockDateForm : Form
    {
        public WasteWarehouseStockDateForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// ค้นหาสินค้าตามวันที่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            Cursor.Current = Cursors.WaitCursor;
            var date = dateTimePickerStart.Value.AddDays(1);
            using (SSLsEntities db = new SSLsEntities())
            {
                var getData = db.WasteWarehouseDetails.Where(w => w.Enable == true && DbFunctions.TruncateTime(w.CreateDate) < date).OrderBy(w => w.CreateDate).ToList();
                List<int> getAllFKProduct = getData.Select(w => w.ProductDetails.FKProduct).Distinct().ToList();
                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = getAllFKProduct.Count();
                int i = 1;
                decimal qty = 0;
                decimal value = 0;
                foreach (var item in getAllFKProduct)
                {
                    progressBar1.Value = progressBar1.Value + 1;

                    var getSKU = getData.Where(w => w.ProductDetails.FKProduct == item).OrderBy(w => w.CreateDate).ToList();
                    decimal plus = getSKU.Where(w => w.IsInOrOut == true).Sum(w => w.QtyPiece);
                    decimal minus = getSKU.Where(w => w.IsInOrOut == false).Sum(w => w.QtyPiece);
                    if (getSKU.Count() > 0)
                    {
                        var cost = Library.GetAverage(item);
                        var getProd = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.FKProduct == item).OrderBy(w => w.PackSize).FirstOrDefault();
                        dataGridView1.Rows.Add(getProd.Code, getProd.Products.ThaiName,
                            getProd.PackSize,
                            getProd.ProductUnit.Name,
                           Library.ConvertDecimalToStringForm(plus - minus),
                           Library.ConvertDecimalToStringForm(cost),
                            getProd.SellPrice,
                            Library.ConvertDecimalToStringForm(cost * (plus - minus)));

                        if (plus - minus < 0)
                        {
                            Console.WriteLine("*******plus - minus*****" + (plus - minus));
                        }
                        else
                        {
                            Console.WriteLine(" ** " + item);
                        }
                        qty += (plus - minus);
                        value+= cost * (plus - minus);
                    }
                    i++;
                }
                labelQty.Text = Library.ConvertDecimalToStringForm(qty);
                labelValue.Text = Library.ConvertDecimalToStringForm(value);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานของเสียคงคลัง ณ วันที่ " + Library.ConvertDateToThaiDate(dateTimePickerStart.Value), "");
        }

        private void button1_Click(object sender, EventArgs e)
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
    }
}
