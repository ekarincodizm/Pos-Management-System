using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class StockCardListForm : Form
    {
        public StockCardListForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectedProductPopup obj = new SelectedProductPopup(this);
            obj.ShowDialog();
        }
        /// <summary>
        /// fk pro dtl
        /// </summary>
        /// <param name="id"></param>
        int _ProId;
        public void BinddingProductChoose(int fkProductDtl)
        {
            var data = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Enable == true && w.Id == fkProductDtl);
            _ProId = data.FKProduct;
            textBox1.Text = data.Code;
            textBox2.Text = data.Products.ThaiName;

        }
        /// <summary>
        /// ค้นหา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<int> ids = new List<int>();
                ids = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true && w.FKProduct == _ProId).Select(w => w.Id).Distinct().ToList<int>();
                var data = db.PosStockDetails
                    .Where(w => w.Enable == true && ids.Contains(w.FKProductDetails) &&
                    EntityFunctions.TruncateTime(w.CreateDate) >= EntityFunctions.TruncateTime(dateS) &&
                    EntityFunctions.TruncateTime(w.CreateDate) >= EntityFunctions.TruncateTime(dateS)
                    ).OrderBy(w => w.CreateDate).ToList();

                //List<PosStockDetails> list = new List<PosStockDetails>();
                //list = data.PosStockDetails.Where(w => w.Enable == true && 
                //ids.Contains(w.FKProductDetails) &&
                //DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS)
                //).OrderBy(w => w.CreateDate).ToList();
                //list = data.PosStockDetails
                //    .Where(w => (w.Enable == true) && (ids.Contains(w.FKProductDetails)) &&
                //    (EntityFunctions.TruncateTime(w.CreateDate) >= EntityFunctions.TruncateTime(dateS)) &&
                //    (EntityFunctions.TruncateTime(w.CreateDate) <= EntityFunctions.TruncateTime(dateE)))
                //    .OrderBy(w => w.CreateDate).ToList();

                //List<PosStockDetails> before = new List<PosStockDetails>();
                //before = data.PosStockDetails
                //    .Where(w => ids.Contains(w.FKProductDetails) && w.Enable == true &&
                //    EntityFunctions.TruncateTime(w.CreateDate) < EntityFunctions.TruncateTime(dateS))
                //    .OrderByDescending(w => w.CreateDate).ToList();
                decimal sumAll = 0;
                //foreach (var item in ids)
                //{
                //    var getLastStock = before.FirstOrDefault(w => w.FKProductDetails == item);
                //    sumBefore += getLastStock.ResultQty;
                //}
                // Add ยกมา
                //DateTime dateBefore = dateS.AddDays(-1)
                //dataGridView1.Rows.Add
                //       (
                //       Library.ConvertDateToThaiDate(dateS.AddDays(-1)),
                //     "ยอดยกมา",
                //       Library.ConvertDecimalToStringForm(sumBefore),
                //       "-",
                //       "1",
                //        Library.ConvertDecimalToStringForm(sumBefore) // get max ของแต่ละ barcode
                //       );
                foreach (var item in data)
                {
                    dataGridView1.Rows.Add
                        (
                        Library.ConvertDateToThaiDate(item.CreateDate, true),
                        item.TransactionType.Name,
                        Library.ConvertDecimalToStringForm(item.ActionQty),
                        item.ProductDetails.Code,
                        item.PackSize,
                         Library.ConvertDecimalToStringForm(item.ResultQty) // get max ของแต่ละ barcode
                        );

                }
                // Add ยกไป
                if (data.Count > 0)
                {
                    textBox3.Text = data.LastOrDefault().ResultQty.ToString();
                }
                else
                {
                    textBox3.Text = "0";
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
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

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string code = textBox1.Text.Trim();
                    var data = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Enable == true && w.Code == code);
                    if (data != null)
                    {
                        BinddingProductChoose(data.Id);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
