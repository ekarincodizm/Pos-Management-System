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
    public partial class ISS2FrontAllowConfirmForm : Form
    {
        private ISS2FrontManageForm iSS2FrontManageForm;
        private string code;

        public ISS2FrontAllowConfirmForm()
        {
            InitializeComponent();
        }

        public ISS2FrontAllowConfirmForm(ISS2FrontManageForm iSS2FrontManageForm, string code)
        {
            InitializeComponent();
            this.iSS2FrontManageForm = iSS2FrontManageForm;
            this.code = code;
        }

        private void ISS2FrontAllowConfirmForm_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.ISS2Front.SingleOrDefault(w => w.Code == code);
                textBox1.Text = code;
                foreach (var item in data.ISS2FrontDetails.Where(w => w.Enable == true).ToList())
                {
                    decimal qtyAllow = 0;
                    if (data.FKISS2FrontStatus == MyConstant.ISS2FrontStatus.ConfirmOrder || data.FKISS2FrontStatus == MyConstant.ISS2FrontStatus.ISSSuccessValue || data.FKISS2FrontStatus == MyConstant.ISS2FrontStatus.ISSSucessNotValue)
                    {
                        qtyAllow = item.QtyAllow;
                    }
                    else
                    {
                        qtyAllow = item.Qty;
                    }
                    var stock = db.WmsStockDetail.Where(w => w.Enable == true && w.FKProductDetail == item.FKProductDetails).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                    dataGridView1.Rows.Add(item.Id, item.ProductDetails.Code,
                        item.ProductDetails.Products.ThaiName,
                        Library.ConvertDecimalToStringForm(item.Qty),
                        Library.ConvertDecimalToStringForm(qtyAllow),
                        Library.ConvertDecimalToStringForm(stock.ResultQtyUnit),
                        Library.ConvertDecimalToStringForm(item.PricePerUnit),
                        item.Shelf.Name, item.Description);
                }
                if (data.FKISS2FrontStatus == MyConstant.ISS2FrontStatus.CreateOrder)
                {
                    
                }
                else
                {
                    button2.Enabled = false;
                }
            }

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
        /// <summary>
        /// ยืนยันหยิบ จะมีการตัด stock  และบันทึกยอด AllowQty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int colAllowQty = 4;
        private void button2_Click(object sender, EventArgs e)
        {
            if (_EmployeeId == 0)
            {
                MessageBox.Show("กรุณาเลือกผู้หยิบ");
                return;
            }
            try
            {
                DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                    "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
                switch (dr)
                {
                    case DialogResult.Yes:
                        using (SSLsEntities db = new SSLsEntities())
                        {
                            var data = db.ISS2Front.SingleOrDefault(w => w.Code == code);
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                int id = int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());
                                decimal allow = decimal.Parse(dataGridView1.Rows[i].Cells[colAllowQty].Value.ToString());
                                var detail = data.ISS2FrontDetails.SingleOrDefault(w => w.Id == id);
                                detail.QtyAllow = allow;
                                detail.UpdateDate = DateTime.Now;
                                detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                                db.Entry(detail).State = EntityState.Modified;
                            }
                            data.FKISS2FrontStatus = MyConstant.ISS2FrontStatus.ConfirmOrder;
                            data.FKEmployee = _EmployeeId;
                            data.Takedate = DateTime.Now;
                            data.TakeBy = Singleton.SingletonAuthen.Instance().Id;
                            data.Remark = textBoxRemark.Text;
                            db.Entry(data).State = EntityState.Modified;
                            
                            db.SaveChanges();
                            /// ตัด stock wms แล้ว
                            Library.MakeValueForUpdateStockWms(data.ISS2FrontDetails.ToList());
                            /// เพิ่ม stock 
                            /// + store Front 
                            Library.MakeValueForUpdateStockPos(data.ISS2FrontDetails.ToList());
                            this.iSS2FrontManageForm.LoadGrid();
                            this.Dispose();
                        }
                        break;
                    case DialogResult.No:
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("พบสิ่งผิดพลาด");
            }
        }
        /// <summary>
        /// เลือก Employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            SelectedEmployeePopup obj = new SelectedEmployeePopup(this);
            obj.ShowDialog();
        }
        int _EmployeeId = 0;
        public void BinddingEmployee(Employee send)
        {
            _EmployeeId = send.Id;
            textBoxEmpCode.Text = send.Code;
            textBoxEmpName.Text = send.Name;
        }
    }
}
