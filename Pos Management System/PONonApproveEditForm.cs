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
    public partial class PONonApproveEditForm : Form
    {
        public PONonApproveEditForm()
        {
            InitializeComponent();
        }

        private void PONonApproveEditForm_Load(object sender, EventArgs e)
        {
            Search();
        }
        /// <summary>
        /// ค้นหา
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
                if (textBoxPOSearch.Text.Trim() != "")
                {
                    /// ถ้าไม่มีการค้นหา PO ยึดการค้นแบบวันที่
                    poList = db.POHeader.Include("PaymentType").Include("POStatus").Include("PODetail.ProductDetails.Products").Include("PODetail.ProductDetails.ProductUnit").Include("Vendor.POCostType").Include("PORcv.PORcvDetails").Include("PORcv.PORcvDetails")
                    .Where(w => w.Enable == true && w.ApproveDate == null && w.PONo == textBoxPOSearch.Text.Trim())
                    .OrderBy(w => w.CreateDate)
                    .ToList();
                }
                else
                {
                    poList = db.POHeader.Include("PaymentType").Include("POStatus").Include("PODetail.ProductDetails.Products").Include("PODetail.ProductDetails.ProductUnit").Include("Vendor.POCostType").Include("PORcv.PORcvDetails").Include("PORcv.PORcvDetails")
                      .Where(w => w.Enable == true && w.ApproveDate == null && DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) && DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE))
                      .OrderBy(w => w.CreateDate)
                      .ToList();
                }

                foreach (var item in poList)
                {
                    string status = "";
                    if (item.ApproveDate == null)
                    {
                        status = "ยังไม่อนุมัติ";
                    }
                    else
                    {
                        status = "อนุมัติแล้ว";
                    }
                    dataGridView1.Rows.Add(item.Id, Library.ConvertDateToThaiDate(item.CreateDate), Library.GetFullNameUserById(item.CreateBy), item.PONo, Library.ConvertDateToThaiDate(item.POExpire), status, Library.ConvertDecimalToStringForm(item.TotalDiscount), Library.ConvertDecimalToStringForm(item.TotalBalance), item.Vendor.Name, item.FKPOStatus);
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
        /// แก้ไข ที่เลือก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonApprove_Click(object sender, EventArgs e)
        {
            int rowCurrent = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[rowCurrent].Cells[0].Value.ToString());
            int poStatus = int.Parse(dataGridView1.Rows[rowCurrent].Cells[9].Value.ToString());
            if (poStatus == MyConstant.POStatus.RCVNotEnd || poStatus == MyConstant.POStatus.NotRCV)
            {
                // แก้ไขได้
                //MessageBox.Show("แหล่ม");
                POEditForm obj = new POEditForm(this, id);
                obj.ShowDialog();
            }
            else
            {
                MessageBox.Show("ไม่สามารถแก้ไขได้ กรุณาตวจสอบสถานะ PO");
            }
        }

        private void textBoxPOSearch_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    Search();
                    break;
                default:
                    break;
            }
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            string code = dataGridView1.Rows[row].Cells[3].Value.ToString();
            using (SSLsEntities db = new SSLsEntities())
            {
                //var polist = db.POHeader.Where(w => w.PONo == code).OrderBy(w => w.SequenceEdit).ToList();
                //if (polist.Count() > 1)
                //{
                //    //MessageBox.Show("พบ PO หลายฉบับ");                  
                //    POApproveManagePrintSequenceDialog obj = new POApproveManagePrintSequenceDialog(this, polist.Count(), code);
                //    obj.ShowDialog();
                //}
                //else
                //{
                //    MainReportViewer mr = new MainReportViewer(this, id);
                //    mr.ShowDialog();
                //}
                MainReportViewer mr = new MainReportViewer(this, id);
                mr.ShowDialog();
            }
        }
    }
}
