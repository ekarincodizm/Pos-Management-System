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
    public partial class POStatusManageForm : Form
    {
        public POStatusManageForm()
        {
            InitializeComponent();
        }

        private void POStatusManageForm_Load(object sender, EventArgs e)
        {

            //using (SSLsEntities db = new SSLsEntities())
            //{
            //    List<POHeader> poList = new List<POHeader>();
            //    poList = db.POHeader.Include("PaymentType").Include("POStatus").Include("PODetail.ProductDetails.Products").Include("PODetail.ProductDetails.ProductUnit").Include("Vendor.POCostType").Include("PORcv.PORcvDetails").Include("PORcv.PORcvDetails")
            //        .Where(w => w.Enable == true && w.ApproveDate != null && DbFunctions.TruncateTime(DateTime.Now) == DbFunctions.TruncateTime(w.CreateDate))
            //        .OrderBy(w => w.CreateDate)
            //        .ToList();
            //    foreach (var item in poList)
            //    {
            //        // จำนวนหน่วยทั้งหมดของ PO ไม่นับของแถม
            //        decimal getQtyInPO = item.PODetail.Where(w => w.Enable == true).ToList().Sum(w => (w.Qty));
            //        decimal getQtyInRcv = 0;
            //        foreach (var rcv in item.PORcv.Where(w => w.Enable == true).ToList())
            //        {
            //            decimal rcvQty = rcv.PORcvDetails.Where(w => w.Enable == true).Sum(w => w.RcvQuantity);
            //            getQtyInRcv = getQtyInRcv + rcvQty;
            //        }
            //        decimal percent = Library.GetPercentFromValue(getQtyInRcv, getQtyInPO);

            //        dataGridView1.Rows.Add(item.Id, Library.ConvertDateToThaiDate(item.CreateDate), Library.GetFullNameUserById(item.CreateBy), item.PONo, Library.ConvertDateToThaiDate(item.POExpire), item.POStatus.Name, Library.ConvertDecimalToStringForm(percent) + " %", item.Vendor.Name);
            //    }
            //    RowColor();
            //}
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
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// ปุ่มค้นหา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            Search();
        }
        void Search()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<POHeader> poList = new List<POHeader>();
                poList = db.POHeader.Include("PaymentType").Include("POStatus").Include("PODetail.ProductDetails.Products").Include("PODetail.ProductDetails.ProductUnit").Include("Vendor.POCostType").Include("PORcv.PORcvDetails").Include("PORcv.PORcvDetails")
                    .Where(w =>w.Enable == true && w.ApproveDate != null && DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) && DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE))
                    .OrderBy(w => w.CreateDate)
                    .ToList();
                if (checkBoxEndStatus.Checked == true && checkBoxNotEndStatus.Checked == false)
                {
                    // ถ้าปิดแล้ว
                    poList = poList.Where(w => w.FKPOStatus == MyConstant.POStatus.RCVComplete || w.FKPOStatus == MyConstant.POStatus.RCVNotComplete_ButEnd).ToList();
                }
                else if (checkBoxEndStatus.Checked == false && checkBoxNotEndStatus.Checked == true)
                {
                    // ถ้า ยังไม่ปิด
                    poList = poList.Where(w => w.FKPOStatus == MyConstant.POStatus.NotRCV || w.FKPOStatus == MyConstant.POStatus.RCVNotEnd).ToList();
                }
                foreach (var item in poList)
                {
                    // จำนวนหน่วยทั้งหมดของ PO ไม่นับของแถม
                    decimal getQtyInPO = item.PODetail.Where(w => w.Enable == true).ToList().Sum(w => (w.Qty + w.GiftQty));
                    decimal getRcvQtyInPO = item.PODetail.Where(w => w.Enable == true).ToList().Sum(w => (w.RcvQty + w.RcvGiftQty));

                    decimal percent = Library.GetPercentFromValue(getRcvQtyInPO, getQtyInPO);
                    string enable = "";
                    if (item.Enable == false)
                    {
                        enable = "(ยกเลิก)";
                        //continue;
                    }
                    dataGridView1.Rows.Add(item.Id, Library.ConvertDateToThaiDate(item.CreateDate), Library.GetFullNameUserById(item.CreateBy), item.PONo, Library.ConvertDateToThaiDate(item.POExpire), item.POStatus.Name + " " + enable, Library.ConvertDecimalToStringForm(percent) + " %", item.Vendor.Name);
                }
                RowColor();
            }
        }
        /// <summary>
        /// ยกเลิก PO ทีละ 1 PO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            SSLsEntities db = new SSLsEntities();
            try
            {
                int currentRow = dataGridView1.CurrentRow.Index;
                int currentId = int.Parse(dataGridView1.Rows[currentRow].Cells[0].Value.ToString());
                var data = db.POHeader.SingleOrDefault(w => w.Id == currentId);
                if (data.Enable == false)
                {
                    MessageBox.Show("PO นี้ยกเลิกไปแล้ว");
                }
                else if (data.FKPOStatus == MyConstant.POStatus.RCVComplete || data.FKPOStatus == MyConstant.POStatus.RCVNotComplete_ButEnd)
                {
                    MessageBox.Show("PO นี้ ปิดสถานะแล้ว");
                }
                else if (data.FKPOStatus == MyConstant.POStatus.RCVNotEnd)
                {
                    MessageBox.Show("PO นี้ อยู่ในระหว่างรับเข้า");
                }
                else
                {
                    // 
                    DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            data.UpdateDate = DateTime.Now;
                            data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            data.Enable = false;
                            db.Entry(data).State = EntityState.Modified;
                            db.SaveChanges();
                            Search();
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("พบข้อผิดพลาด ในการเลือกรายการ");
            }
            finally
            {
                db.Dispose();
            }
        }
        /// <summary>
        /// ปิดสถานะ จะปิดได้ ต้อง enable == true และต้องมีการรับเข้า > 0%
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            SSLsEntities db = new SSLsEntities();
            WH_TRATEntities wh = new WH_TRATEntities();
            try
            {
                int currentRow = dataGridView1.CurrentRow.Index;
                int currentId = int.Parse(dataGridView1.Rows[currentRow].Cells[0].Value.ToString());
                var data = db.POHeader.SingleOrDefault(w => w.Id == currentId);
                if (data.Enable == false)
                {
                    MessageBox.Show("PO นี้ยกเลิกไปแล้ว");
                }
                else if (data.FKPOStatus == MyConstant.POStatus.RCVComplete || data.FKPOStatus == MyConstant.POStatus.RCVNotComplete_ButEnd)
                {
                    MessageBox.Show("PO นี้ ปิดสถานะแล้ว");
                }
                else if (data.FKPOStatus == MyConstant.POStatus.NotRCV)
                {
                    MessageBox.Show("PO นี้ ไม่เคยรับเข้ามาก่อน ไม่สามารถปิดได้");
                }
                else
                {
                    // 
                    DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            data.UpdateDate = DateTime.Now;
                            data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            data.FKPOStatus = MyConstant.POStatus.RCVNotComplete_ButEnd;
                            db.Entry(data).State = EntityState.Modified;

                            var poOrder = wh.PS_PO_ORDER.FirstOrDefault(w => w.ENABLE == true && w.PO_NO == data.PONo);
                            poOrder.CONFIRMRCV = DateTime.Now;
                            wh.Entry(poOrder).State = EntityState.Modified;

                            wh.SaveChanges();
                            db.SaveChanges();
                            Search();
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("พบข้อผิดพลาด ในการเลือกรายการ");
            }
            finally
            {
                db.Dispose();
                wh.Dispose();
            }
        }
        /// <summary>
        /// double click เลือกรายการ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }
            SSLsEntities db = new SSLsEntities();
            try
            {
                int currentRow = dataGridView1.CurrentRow.Index;
                int currentId = int.Parse(dataGridView1.Rows[currentRow].Cells[0].Value.ToString());
                var data = db.POHeader.Include("PODetail.ProductDetails.Products.ProductVatType").Include("Vendor.POCostType").SingleOrDefault(w => w.Id == currentId);

                PODetailsDialog obj = new PODetailsDialog(data);
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                db.Dispose();
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;

            int id = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());

            MainReportViewer mr = new MainReportViewer(this, id);
            mr.ShowDialog();
        }
    }
}
