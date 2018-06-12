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
    public partial class StoreFrontAdjustForm : Form
    {
        public StoreFrontAdjustForm()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<ProductDetails> list = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
            SelectedProductPopup obj = new SelectedProductPopup(this, list);
            obj.ShowDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StoreFrontAdjustForm_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.AdjustStoreFront.OrderBy(w => w.CreateDate).Where(w => w.Enable == true).ToList();
                foreach (var item in data)
                {
                    dataGridView1.Rows.Add
                        (
                        item.Code,
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.TotalQtyUnit,
                        item.TotalBalance,
                        item.Description                        
                        );
                }
            }
        }
        /// <summary>
        /// Enter Barcode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxCode_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    var code = textBoxCode.Text.Trim();
                    var getProd = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Code == code);
                    dtl = getProd;
                    textBoxName.Text = getProd.Products.ThaiName;
                    textBoxPZ.Text = getProd.PackSize + "";
                    textBoxUnit.Text = getProd.ProductUnit.Name;
                    textBoxQty.Text = Library.ConvertDecimalToStringForm(Library.GetResult(getProd.FKProduct, DateTime.Now));
                    textBoxQtyAd.Select();
                    textBoxQtyAd.SelectAll();
                    break;
                default:
                    break;
            }

        }
        ProductDetails dtl;
        public void BinddingProduct(int idDtl)
        {
            var getProd = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Id == idDtl);
            dtl = getProd;

            textBoxCode.Text = getProd.Code;
            textBoxName.Text = getProd.Products.ThaiName;
            textBoxPZ.Text = getProd.PackSize + "";
            textBoxUnit.Text = getProd.ProductUnit.Name;
            textBoxQty.Text = Library.ConvertDecimalToStringForm(Library.GetResult(getProd.FKProduct, DateTime.Now));
            textBoxQtyAd.Focus();
            textBoxQtyAd.SelectAll();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBoxQtyAd_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dtl == null)
            {
                MessageBox.Show("ข้อมูลไม่ถูกต้อง");
                return;
            }
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    SaveCommit();
                    dtl = null;
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void SaveCommit()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var adjust = db.AdjustStoreFront.Where(w => w.UpdateDate.Year == currentYear && w.UpdateDate.Month == currentMonth).Count() + 1;

                //Branch branch = Singleton.SingletonP
                string code = MyConstant.PrefixForGenerateCode.AdjustStoreFront + DateTime.Now.ToString("yy") + "" + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(adjust, 4);

                // adjust
                AdjustStoreFront obj = new AdjustStoreFront();
                obj.Code = code;
                obj.Enable = true;
                obj.Description = textBoxDesc.Text;
                obj.CreateDate = DateTime.Now;
                obj.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                obj.UpdateDate = DateTime.Now;
                obj.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                obj.TotalBalance = dtl.CostOnly * decimal.Parse(textBoxQtyAd.Text);
                obj.TotalQtyUnit = decimal.Parse(textBoxQtyAd.Text);

                List<AdjustStoreFrontDetail> adDtl = new List<AdjustStoreFrontDetail>();
                adDtl.Add(new AdjustStoreFrontDetail()
                {
                    Enable = true,
                    Description = "",
                    CreateDate = DateTime.Now,
                    CreateBy = obj.CreateBy,
                    UpdateDate = DateTime.Now,
                    UpdateBy = obj.CreateBy,
                    FKProductDetails = dtl.Id,
                    Qty = decimal.Parse(textBoxQtyAd.Text),
                    CostPerUnit = dtl.CostOnly,
                    SellPricePerUnit = dtl.SellPrice
                });
                obj.AdjustStoreFrontDetail = adDtl;
                db.AdjustStoreFront.Add(obj);
       
                // stock 
                StoreFrontStockDetails addDtl = new StoreFrontStockDetails();
                addDtl.DocNo = code;
                addDtl.DocDtlNumber = 1;
                addDtl.Description = "Adjust หน้าร้าน";
                addDtl.CreateDate = DateTime.Now;
                addDtl.CreateBy = SingletonAuthen.Instance().Id;
                addDtl.UpdateDate = DateTime.Now;
                addDtl.UpdateBy = SingletonAuthen.Instance().Id;
                addDtl.Enable = true;
                addDtl.ActionQty = decimal.Parse(textBoxQtyAd.Text) * dtl.PackSize;
                var stockHD = db.StoreFrontStock.FirstOrDefault(w => w.FKProduct == dtl.FKProduct && w.Enable == true);
                addDtl.FKStoreFrontStock = stockHD.Id;
                addDtl.FKTransactionType = MyConstant.PosTransaction.ADJ;
                addDtl.Barcode = textBoxCode.Text;
                addDtl.Name = dtl.Products.ThaiName;
                addDtl.FKProductDetails = dtl.Id;
                addDtl.ResultQty = addDtl.ActionQty;

                addDtl.PackSize = dtl.PackSize;
                addDtl.DocRefer = "-";
                addDtl.DocReferDtlNumber = 0;
                addDtl.CostOnlyPerUnit = dtl.CostOnly;
                addDtl.SellPricePerUnit = dtl.SellPrice;

                db.StoreFrontStockDetails.Add(addDtl);
                db.SaveChanges();

                // open paper               
                frmMainReport report = new frmMainReport(this, code);
                report.Show();
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                var data = db.AdjustStoreFront.OrderBy(w => w.CreateDate).Where(w => w.Enable == true).ToList();
                foreach (var item in data)
                {
                    dataGridView1.Rows.Add
                        (
                        item.Code,
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.TotalQtyUnit,
                        item.TotalBalance,
                        item.Description
                        );
                }
            }
        }
        /// <summary>
        /// พิมพ์ใบ Adjust
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
            frmMainReport report = new frmMainReport(this, code);
            report.Show();
        }
    }
}
