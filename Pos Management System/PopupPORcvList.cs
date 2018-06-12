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
    public partial class PopupPORcvList : Form
    {
        private PORcvInvoiceForm pORcvInvoiceForm;
        private string _FromForm;
        public PopupPORcvList()
        {
            InitializeComponent();
        }

        public PopupPORcvList(PORcvInvoiceForm pORcvInvoiceForm)
        {
            _FromForm = "PORcvInvoiceForm";
            InitializeComponent();            
            this.pORcvInvoiceForm = pORcvInvoiceForm;
        }

        private void PopupPORcvList_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.PORcv.Where(w => w.Enable == true).OrderByDescending(w => w.CreateDate).ToList();
                foreach (var item in data)
                {
                    string vendor = "";
                    if (item.Vendor != null)
                    {
                        vendor = item.Vendor.Name;
                    }
                    else
                    {
                        vendor = "ยังไม่เลือกในการทำรับ";
                    }
                    dataGridView1.Rows.Add(item.Code, item.InvoiceNo, item.PORefer, vendor);
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            string rcvNo = dataGridView1.Rows[row].Cells[0].Value.ToString();
            switch (_FromForm)
            {
                case "PORcvInvoiceForm":
                    pORcvInvoiceForm.BinddingRcv(rcvNo);
                    break;
                default:
                    break;
            }
            this.Dispose();
        }
    }
}
