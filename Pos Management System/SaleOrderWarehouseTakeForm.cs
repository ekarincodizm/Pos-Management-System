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
    public partial class SaleOrderWarehouseTakeForm : Form
    {
        public SaleOrderWarehouseTakeForm()
        {
            InitializeComponent();
        }

        private void SaleOrderWarehouseTakeForm_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        public void LoadGrid()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<SaleOrderWarehouse> list = new List<SaleOrderWarehouse>();
                list = db.SaleOrderWarehouse.Where(w =>
                w.FKSaleOrderWarehouseStatus != MyConstant.SaleOrderWarehouseStatus.CreateOrder &&
                w.FKSaleOrderWarehouseStatus != MyConstant.SaleOrderWarehouseStatus.CancelOrder &&
                DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();

                decimal total = 0;
                foreach (var item in list)
                {
                    dataGridView1.Rows.Add(
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Code,
                        item.Debtor.Name,
                        item.Member.Name,
                        item.PaymentType.Name,
                        item.DeliveryType.Name,
                        item.SaleOrderWarehouseDtl.Where(w => w.Enable == true).ToList().Count(),
                        Library.ConvertDecimalToStringForm(item.TotalBalance),
                        item.SaleOrderWarehouseStatus.Name,
                        item.Description
                        );
                    total += item.TotalBalance;
                }
                textBoxQtyList.Text = list.Count() + "";
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(total);

            }
        }
        /// <summary>
        /// ค้นหาข้อมูล จะแสดงเฉพาะ enable และ ยืนยันออเดอร์แล้วเท่านั้น
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }
        private void SearchOrder()
        {
            string code = textBoxKeySearch.Text.Trim();
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                List<SaleOrderWarehouse> list = new List<SaleOrderWarehouse>();
                list = db.SaleOrderWarehouse.Where(w => w.Code == code &&
                w.FKSaleOrderWarehouseStatus != MyConstant.SaleOrderWarehouseStatus.CreateOrder &&
                w.FKSaleOrderWarehouseStatus != MyConstant.SaleOrderWarehouseStatus.CancelOrder
                ).OrderBy(w => w.CreateDate).ToList();

                decimal total = 0;
                foreach (var item in list)
                {

                    dataGridView1.Rows.Add(
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Code,
                        item.Debtor.Name,
                        item.Member.Name,
                        item.PaymentType.Name,
                        item.DeliveryType.Name,
                        item.SaleOrderWarehouseDtl.Where(w => w.Enable == true).ToList().Count(),
                        Library.ConvertDecimalToStringForm(item.TotalBalance),
                        item.SaleOrderWarehouseStatus.Name,
                        item.Description
                        );
                    total += item.TotalBalance;
                }
                textBoxQtyList.Text = list.Count() + "";
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(total);
            }
        }
        /// <summary>
        /// ค้นหาราย order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlus_Click(object sender, EventArgs e)
        {
            SearchOrder();
        }

        private void textBoxKeySearch_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    SearchOrder();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ยืนยันหยิบ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?", "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            //switch (dr)
            //{
            //    case DialogResult.Yes:
            //        ConfirmISSOrder();
            //        break;
            //    case DialogResult.No:
            //        break;
            //}
            string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
            //using (SSLsEntities db = new SSLsEntities())
            //{
            //    var data = dvb
            //}
            SaleOrderWarehouseAllowForm obj = new SaleOrderWarehouseAllowForm(this, code);
            obj.ShowDialog();
        }

        private void ConfirmISSOrder()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// ปฏิเสธออเดอร์ หรือ Reject จะต้องเป็น ออเดอร์ ที่ถูกยืนยันมาแล้ว เท่านั้น
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
                var data = db.SaleOrderWarehouse.SingleOrDefault(w => w.Code == code);
                if (data.FKSaleOrderWarehouseStatus == MyConstant.SaleOrderWarehouseStatus.ConfirmOrder)
                {
                    if (textBoxRemark.Text.Trim() == "")
                    {
                        MessageBox.Show("กรุณากรอกหมายเหตุ");
                        return;
                    }
                    DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?", "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            data.FKSaleOrderWarehouseStatus = MyConstant.SaleOrderWarehouseStatus.WarehouseRejectOrder;
                            data.UpdateDate = DateTime.Now;
                            data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            db.Entry(data).State = EntityState.Modified;
                            db.SaveChanges();
                            LoadGrid();
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("ไม่สามารถปฏิเสธออเดอร์นี้ได้");
                }
            }
        }
    }
}
