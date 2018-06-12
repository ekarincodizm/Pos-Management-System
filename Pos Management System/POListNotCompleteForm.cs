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
    public partial class POListNotCompleteForm : Form
    {
        private RCVPOForm rCVPOForm;
        string _Fromform = "";
        public POListNotCompleteForm()
        {
            InitializeComponent();
        }
        private static List<POHeader> _poList;
        private RCVPOEditForm rCVPOEditForm;
        private PORcvInvoiceForm pORcvInvoiceForm;

        public POListNotCompleteForm(RCVPOForm rCVPOForm)
        {
            InitializeComponent();
            this.rCVPOForm = rCVPOForm;
            _Fromform = "RCVPOForm";
        }

        public POListNotCompleteForm(RCVPOEditForm rCVPOEditForm)
        {
            InitializeComponent();
            _Fromform = "RCVPOEditForm";
            this.rCVPOEditForm = rCVPOEditForm;
        }

        public POListNotCompleteForm(PORcvInvoiceForm pORcvInvoiceForm)
        {
            InitializeComponent();
            _Fromform = "PORcvInvoiceForm";
            this.pORcvInvoiceForm = pORcvInvoiceForm;
        }

        private void POListNotCompleteForm_Load(object sender, EventArgs e)
        {
            _poList = new List<POHeader>();
            using (SSLsEntities db = new SSLsEntities())
            {
                _poList = db.POHeader.Include("PaymentType").Include("POStatus").Include("PODetail.ProductDetails.Products").Include("PODetail.ProductDetails.ProductUnit").Include("Vendor.POCostType").Include("PORcv.PORcvDetails").Include("PORcv.PORcvDetails")
                    .Where(w => w.Enable == true && w.ApproveDate != null && (w.FKPOStatus == MyConstant.POStatus.NotRCV || w.FKPOStatus == MyConstant.POStatus.RCVNotEnd)).ToList();
            }
            foreach (var item in _poList)
            {
                // จำนวนหน่วยทั้งหมดของ PO ไม่นับของแถม
                decimal getQtyInPO = item.PODetail.Where(w => w.Enable == true).ToList().Sum(w => (w.Qty + w.GiftQty));
                decimal getRcvQtyInPO = item.PODetail.Where(w => w.Enable == true).ToList().Sum(w => (w.RcvQty + w.RcvGiftQty));

                decimal percent = Library.GetPercentFromValue(getRcvQtyInPO, getQtyInPO);
                dataGridView1.Rows.Add(item.Id, Library.ConvertDateToThaiDate(item.CreateDate), item.PONo, Library.ConvertDateToThaiDate(item.POExpire), item.POStatus.Name, item.Vendor.Name, Library.ConvertDecimalToStringForm(percent) + " %");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// เลือก PO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            GetPOChoose();
            this.Dispose();
        }

        private void GetPOChoose()
        {
            int row = dataGridView1.CurrentRow.Index;
            int idPOHD = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            var data = _poList.SingleOrDefault(w => w.Id == idPOHD);
            switch (_Fromform)
            {
                case "RCVPOForm":
                    this.rCVPOForm.BinddingSelected(data);
                    break;

                case "RCVPOEditForm":
                    this.rCVPOEditForm.BinddingSelected(data);
                    break;
                case "PORcvInvoiceForm":
                    this.pORcvInvoiceForm.SendPONo(data.PONo);
                    break;
                    
                default:
                    break;
            }
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
        /// ปุ่มค้นหา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                bool checkNotRcv = checkBoxNotRcv.Checked;
                bool checkRcv = checkBoxRcv.Checked;
                //var data = _poList.Where(w =>
                //DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                //DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)
                //).ToList();
                var data = _poList.Where(w =>
            int.Parse(w.CreateDate.ToString("yyyyMMdd")) >= int.Parse(dateS.ToString("yyyyMMdd")) &&
            int.Parse(w.CreateDate.ToString("yyyyMMdd")) <= int.Parse(dateE.ToString("yyyyMMdd"))
            ).ToList();
                if (checkNotRcv == true && checkRcv == true)
                {

                }
                else if (checkNotRcv == true && checkRcv == false) // เอาเฉพาะ ไม่เคยรับเข้า
                {
                    data = data.Where(w => w.FKPOStatus == MyConstant.POStatus.NotRCV).ToList();
                }
                else if (checkNotRcv == false && checkRcv == true) // เอาเฉพาะ เคยรับเข้าแล้ว
                {
                    data = data.Where(w => w.FKPOStatus == MyConstant.POStatus.RCVNotEnd).ToList();
                }
                else
                {
                    //data = data.Where(w => w.FKPOStatus == MyConstant.POStatus.RCVNotEnd).ToList();
                    MessageBox.Show("เงื่อนไขค้นหาไม่ถูกต้อง");
                    return;
                }
                foreach (var item in data)
                {
                    // จำนวนหน่วยทั้งหมดของ PO ไม่นับของแถม
                    decimal getQtyInPO = item.PODetail.Where(w => w.Enable == true).ToList().Sum(w => (w.Qty + w.GiftQty));
                    decimal getRcvQtyInPO = item.PODetail.Where(w => w.Enable == true).ToList().Sum(w => (w.RcvQty + w.RcvGiftQty));

                    decimal percent = Library.GetPercentFromValue(getRcvQtyInPO, getQtyInPO);
                    dataGridView1.Rows.Add(item.Id, Library.ConvertDateToThaiDate(item.CreateDate), item.PONo, Library.ConvertDateToThaiDate(item.POExpire), item.POStatus.Name, item.Vendor.Name, Library.ConvertDecimalToStringForm(percent) + " %");
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }
            GetPOChoose();
        }
    }
}
