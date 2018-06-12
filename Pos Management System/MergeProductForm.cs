using Pos_Management_System.Singleton;
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
    public partial class MergeProductForm : Form
    {
        public MergeProductForm()
        {
            InitializeComponent();
        }

        private void MergeProductForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonSearchProduct_Click(object sender, EventArgs e)
        {
            List<ProductDetails> list = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
            SelectedProductPopup obj = new SelectedProductPopup(this, list, 1);
            obj.ShowDialog();
        }
        int fk1;
        int fk2;
        /// <summary>
        /// ส่ง fk prod dtl มา
        /// </summary>
        /// <param name="id"></param>
        public void BinddingProduct(int id, int gvNumber)
        {
            List<ProductDetails> list = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
            var fkProd = list.FirstOrDefault(w => w.Enable == true && w.Id == id).FKProduct;
            var getProdDetails = list.Where(w => w.Enable == true && w.FKProduct == fkProd).ToList();
            //using (SSLsEntities db = new SSLsEntities())
            //{
            //    var getStock = db.StoreFrontStock.FirstOrDefault(w => w.Enable == true && w.FKProduct == fkProd);
            //    if (getStock)
            //    {

            //    }
            //}
            if (gvNumber == 1) // bindding datagrid 1
            {
                fk1 = fkProd;
                label3.Text = getProdDetails.FirstOrDefault().Products.ThaiName;
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                foreach (var item in getProdDetails)
                {
                    dataGridView1.Rows.Add
                        (
                        item.Id,
                        item.Code,
                        item.PackSize,
                        item.ProductUnit.Name,
                        item.CostOnly,
                        item.CostAndVat,
                        item.SellPrice
                        );
                }
                qtyLb1.Text = Library.ConvertDecimalToStringForm(Library.GetResult(fkProd, DateTime.Now.AddDays(1)));
                /// clear grid2 กันพลาดจาก grid1 หลงเข้ามา
                fk2 = 0;
                label4.Text = "";
                qtyLb2.Text = "0.00";
                dataGridView2.Rows.Clear();
                dataGridView2.Refresh();
            }
            else
            {
                fk2 = fkProd;
                label4.Text = getProdDetails.FirstOrDefault().Products.ThaiName;
                dataGridView2.Rows.Clear();
                dataGridView2.Refresh();
                foreach (var item in getProdDetails)
                {
                    dataGridView2.Rows.Add
                        (
                        item.Id,
                        item.Code,
                        item.PackSize,
                        item.ProductUnit.Name,
                        item.CostOnly,
                        item.CostAndVat,
                        item.SellPrice
                        );
                }
                qtyLb2.Text = Library.ConvertDecimalToStringForm(Library.GetResult(fkProd, DateTime.Now.AddDays(1)));
            }
            if (fk1 == fk2)
            {
                MessageBox.Show("คุณเลือกสินค้าฐานเดียวกัน");
                fk2 = 0;
                label4.Text = "สินค้า 2";
                dataGridView2.Rows.Clear();
                dataGridView2.Refresh();
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<ProductDetails> list = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
            SelectedProductPopup obj = new SelectedProductPopup(this, list, 2);
            obj.ShowDialog();
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
        /// เลือกสินค้า เพื่อย้ายฐาน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            Console.WriteLine(id);
            if (fk2 == null || fk2 == 0)
            {
                MessageBox.Show("กรุณาเลือก สินค้า 2");
                return;
            }
            else // allow transfer
            {
                List<ProductDetails> list = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
                var data = list.FirstOrDefault(w => w.Enable == true && w.Id == id);
                dataGridView2.Rows.Add
                        (
                        data.Id,
                        data.Code,
                        data.PackSize,
                        data.ProductUnit.Name,
                        data.CostOnly,
                        data.CostAndVat,
                        data.SellPrice);
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
            }
        }

        private void button3_Click(object sender, EventArgs e)
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
                List<ProductDetails> list = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
                var getProdDetails = list.Where(w => w.Enable == true && w.FKProduct == fk2).ToList();
                List<int> disId = new List<int>();

                // ต้องมี pz 1 ยุ่ในฐาน
                if (dataGridView1.Rows.Count > 0)
                {
                    bool checkPz1 = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        decimal pz = decimal.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        if (pz == 1)
                        {
                            checkPz1 = true;
                        }
                    }
                    if (!checkPz1) // not allow
                    {
                        MessageBox.Show("ต้องเหลือ packsize 1 ไว้");
                        return;
                    }
                }               

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    int id = int.Parse(dataGridView2.Rows[i].Cells[0].Value.ToString());
                    string code = dataGridView2.Rows[i].Cells[1].Value.ToString();
                    decimal pz = decimal.Parse(dataGridView2.Rows[i].Cells[2].Value.ToString());
                    decimal costOnly = decimal.Parse(dataGridView2.Rows[i].Cells[4].Value.ToString());
                    // id check new
                    if (getProdDetails.SingleOrDefault(w => w.Id == id) != null)
                    {
                        continue;
                    }
                    else
                    {
                        // add new 
                        var oldProd = list.FirstOrDefault(w => w.Enable == true && w.Id == id);
                        ProductDetails details = new ProductDetails();
                        details.Code = code;
                        details.Name = "-";
                        details.Description = "ย้ายฐานจาก : " + fk1;
                        details.PackSize = pz;
                        details.CostOnly = costOnly;
                        details.CostAndVat = oldProd.CostAndVat;
                        details.CostVat = oldProd.CostVat;
                        details.IsPrintLabel = oldProd.IsPrintLabel;
                        details.SellPrice = oldProd.SellPrice;
                        details.FKProduct = fk2;
                        details.FKUnit = oldProd.FKUnit;
                        details.SpecialQtyPiece = oldProd.SpecialQtyPiece;
                        details.SetPallet = oldProd.SetPallet;
                        details.PalletRow = oldProd.PalletRow;
                        details.PalletLevel = oldProd.PalletLevel;
                        details.PalletTotal = oldProd.PalletTotal;
                        details.CreateDate = DateTime.Now;
                        details.CreateBy = SingletonAuthen.Instance().Id;
                        details.UpdateDate = DateTime.Now;
                        details.UpdateBy = SingletonAuthen.Instance().Id;
                        details.Enable = true;
                        db.ProductDetails.Add(details);
                        // disable old
                        var getOld = db.ProductDetails.SingleOrDefault(w => w.Id == id);
                        getOld.Enable = false;
                        getOld.Description = "ถูกย้ายไป : " + fk2;
                        db.Entry(getOld).State = EntityState.Modified;
                    }
                }

                // check การย้ายฐาน
                if (dataGridView1.Rows.Count == 0)
                {
                    // แปลว่ามีการย้ายทั้งฐาน ต้อง เพิ่ม transactions
                    decimal getResultProduct1 = decimal.Parse(qtyLb1.Text);
                    if (getResultProduct1 < 0)
                    {
                        // add transactions
                        getResultProduct1 = Math.Abs(getResultProduct1);
                    }
                    else if (getResultProduct1 > 0) // พบสินค้าหน้าร้าน ห้ามย้ายฐานทั้งหมด
                    {
                        MessageBox.Show("ไม่สามารถย้ายฐานทั้งหมดได้ ยังมีสินค้าค้างอยู่หน้าร้าน");
                        return;
                    }
                }
                else
                {

                }
                //db.SaveChanges();
            }
            this.Dispose();
        }
    }
}
