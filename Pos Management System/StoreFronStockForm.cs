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
    public partial class StoreFronStockForm : Form
    {
        public StoreFronStockForm()
        {
            InitializeComponent();
        }

        private void StoreFronStockForm_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// เปิดประเภทสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SelectedProductGroupPopup obj = new SelectedProductGroupPopup(this);
            obj.ShowDialog();
        }
        int _GroupId;
        public void BinddingProGroupChoose(ProductGroups send)
        {
            _GroupId = send.Id;
            textBoxProGroupCode.Text = send.Code;
            textBoxProGroupName.Text = send.Name;
        }

        private void textBoxProGroupCode_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string code = textBoxProGroupCode.Text.Trim();
                    var data = Singleton.SingletonProductGroup.Instance().ProductGroups.FirstOrDefault(w => w.Code == code);
                    BinddingProGroupChoose(data);
                    break;
                default:
                    break;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadGrid(_GroupId);
        }

        private void LoadGrid(int group)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (SSLsEntities db = new SSLsEntities())
            {
                decimal unit = 0;
                decimal piece = 0;
                decimal total = 0;
                List<int> fkPro = db.Products.Where(w => w.Enable == true && w.FKProductGroup == group).Select(w => w.Id).ToList<int>();
                var data = db.PosStock.Where(w => w.Enable == true && fkPro.Contains(w.FKProduct));
                foreach (var item in data)
                {
                    List<int> getFK = item.PosStockDetails.Where(w => w.Enable == true).Select(w => w.FKProductDetails).Distinct().ToList<int>();
                    foreach (var dtl in getFK)
                    {
                        var getDtl = item.PosStockDetails.OrderByDescending(w => w.Id).FirstOrDefault(w => w.Enable == true && w.FKProductDetails == dtl);
                        dataGridView1.Rows.Add(getDtl.ProductDetails.Code,
                            getDtl.ProductDetails.Products.ThaiName,
                            Library.ConvertDecimalToStringForm(getDtl.ResultQtyUnit),
                            getDtl.ProductDetails.ProductUnit.Name,
                            getDtl.ProductDetails.PackSize,
                          Library.ConvertDecimalToStringForm(getDtl.ProductDetails.SellPrice),
                           Library.ConvertDecimalToStringForm(getDtl.ProductDetails.CostOnly),
                            Library.ConvertDecimalToStringForm(getDtl.ProductDetails.CostAndVat),
                            item.Products.ProductVatType.Name,
                             Library.ConvertDecimalToStringForm(getDtl.ProductDetails.SellPrice * getDtl.ResultQtyUnit));
                        unit += getDtl.ResultQtyUnit;
                        piece += getDtl.ResultQty;
                        total += getDtl.ProductDetails.SellPrice * getDtl.ResultQtyUnit;
                    }
                }
                textBoxUnit.Text = Library.ConvertDecimalToStringForm(unit);
                textBoxPiece.Text = Library.ConvertDecimalToStringForm(piece);
                textBoxValue.Text = Library.ConvertDecimalToStringForm(total);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string code1 = textBox1.Text.Trim();
         
                    break;
                default:
                    break;
            }
        }

        private void LoadGrid(string code1)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string code1 = textBox1.Text.Trim();
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                using (SSLsEntities db = new SSLsEntities())
                {
                    decimal unit = 0;
                    decimal piece = 0;
                    decimal total = 0;
                    var pro = db.ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == code1);
                    if (pro == null)
                    {
                        MessageBox.Show("ไม่ถูกต้อง");
                        return;
                    }
                    var data = db.PosStock.Where(w => w.Enable == true && w.FKProduct == pro.FKProduct);
                    foreach (var item in data)
                    {
                        List<int> getFK = item.PosStockDetails.Where(w => w.Enable == true).Select(w => w.FKProductDetails).Distinct().ToList<int>();
                        foreach (var dtl in getFK)
                        {
                            var getDtl = item.PosStockDetails.OrderByDescending(w => w.Id).FirstOrDefault(w => w.Enable == true && w.FKProductDetails == dtl);
                            dataGridView1.Rows.Add(getDtl.ProductDetails.Code,
                                getDtl.ProductDetails.Products.ThaiName,
                                Library.ConvertDecimalToStringForm(getDtl.ResultQtyUnit),
                                getDtl.ProductDetails.ProductUnit.Name,
                                getDtl.ProductDetails.PackSize,
                              Library.ConvertDecimalToStringForm(getDtl.ProductDetails.SellPrice),
                               Library.ConvertDecimalToStringForm(getDtl.ProductDetails.CostOnly),
                                Library.ConvertDecimalToStringForm(getDtl.ProductDetails.CostAndVat),
                                item.Products.ProductVatType.Name,
                                 Library.ConvertDecimalToStringForm(getDtl.ProductDetails.SellPrice * getDtl.ResultQtyUnit));
                            unit += getDtl.ResultQtyUnit;
                            piece += getDtl.ResultQty;
                            total += getDtl.ProductDetails.SellPrice * getDtl.ResultQtyUnit;
                        }
                        textBoxUnit.Text = Library.ConvertDecimalToStringForm(unit);
                        textBoxPiece.Text = Library.ConvertDecimalToStringForm(piece);
                        textBoxValue.Text = Library.ConvertDecimalToStringForm(total);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
