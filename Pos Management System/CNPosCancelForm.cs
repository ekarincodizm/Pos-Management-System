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
    public partial class CNPosCancelForm : Form
    {
        public CNPosCancelForm()
        {
            InitializeComponent();
        }

        private void CNPosCancelForm_Load(object sender, EventArgs e)
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
        /// ค้นหาเลขที่ ทำคืน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlus_Click(object sender, EventArgs e)
        {
            MainBindding();
        }

        private void MainBindding()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            string cnNo = textBoxCNNo.Text.Trim();
            if (cnNo == "")
            {
                MessageBox.Show("กรุณาคีย์เลขที่ทำคืน");
                return;
            }
            // bindding
            using (SSLsEntities db = new SSLsEntities())
            {
                var cn = db.CNHeader.SingleOrDefault(w => w.Enable == true && w.No == cnNo);
                textBoxCNDate.Text = Library.ConvertDateToThaiDate(cn.CreateDate);
                textBoxCreateBy.Text = Library.GetFullNameUserById(cn.CreateBy);
                textBoxMemberNo.Text = cn.PosHeader.Member.Code;
                textBoxMemberName.Text = cn.PosHeader.Member.Name;
                if (cn.PosHeader.FKDebtor != null)
                {
                    textBoxDebtNo.Text = cn.PosHeader.Debtor.Code;
                    textBoxDebtName.Text = cn.PosHeader.Debtor.Name;
                }

                if (cn.PosHeader.PayDate != null)
                {
                    textBoxPayDate.Text = Library.ConvertDateToThaiDate((DateTime)cn.PosHeader.PayDate);
                }
                textBoxInvoiceNo.Text = cn.PosHeader.InvoiceNo;
                textBoxInvoiceDate.Text = Library.ConvertDateToThaiDate(cn.PosHeader.CreateDate);
                textBoxSequnce.Text = cn.SequenceNo + "";
                textBoxCNType.Text = cn.CNType.Name;

                decimal totalCNPrice = 0;
                decimal totalCNQty = 0;
                decimal totalCNDiscount = 0;
                foreach (var item in cn.CNDetails.Where(w => w.Enable == true).ToList())
                {
                    totalCNPrice += item.CNTotalPrice;
                    totalCNDiscount += item.CNDisShop + item.CNDisCoupon;
                    totalCNQty += item.CNQty;
                    dataGridView1.Rows.Add(
                        item.Id,
                        item.PosDetails.ProductDetails.Code,
                        item.PosDetails.ProductDetails.Products.ThaiName,
                        item.PosDetails.ProductDetails.Products.ProductVatType.Name,
                        Library.ConvertDecimalToStringForm(item.PosDetails.Qty),
                        item.PosDetails.ProductDetails.ProductUnit.Name,
                        Library.ConvertDecimalToStringForm(item.CNDisCoupon),
                        Library.ConvertDecimalToStringForm(item.CNDisShop),
                        Library.ConvertDecimalToStringForm(item.PricePerUnit),
                        Library.ConvertDecimalToStringForm(item.CNTotalPrice),
                        Library.ConvertDecimalToStringForm(item.CNQty)
                        );
                }
                textBoxTotalCNQty.Text = Library.ConvertDecimalToStringForm(totalCNQty);
                textBoxTotalCNPrice.Text = Library.ConvertDecimalToStringForm(totalCNPrice);
                textBoxTotalCNDiscount.Text = Library.ConvertDecimalToStringForm(totalCNDiscount);
            }

        }

        private void textBoxCNNo_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    MainBindding();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// บันทึก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBoxDescription.Text.Trim() == "")
            {
                MessageBox.Show("กรุณาใส่หมายเหตุ");
                return;
            }
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
        /// <summary>
        ///  ยกเลิก ต้องเอาลง CNCancel ด้วย
        /// </summary>
        private void SaveCommit()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                
                var data = db.CNHeader.SingleOrDefault(w => w.No == textBoxCNNo.Text.Trim());
                data.UpdateDate = DateTime.Now;
                data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                data.Enable = false;

                CNCancel cancel = new CNCancel();
                cancel.FKCN = textBoxCNNo.Text.Trim();
                cancel.Enable = true;
                cancel.Description = textBoxDescription.Text.Trim();
                cancel.CreateDate = DateTime.Now;
                cancel.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                cancel.UpdateDate = DateTime.Now;
                cancel.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                db.CNCancel.Add(cancel);
                // update -stock pos
                Library.MakeValueForUpdateStockPos(data.CNDetails.Where(w => w.Enable == true).ToList(), "ยกเลิกทำคืน");
                foreach (var item in data.CNDetails.Where(w => w.Enable == true).ToList())
                {
                    item.Enable = false;
                    item.UpdateDate = DateTime.Now;
                    item.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                }
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();

                this.Dispose();
            }
        }
    }
}
