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
    public partial class SelectedUserPopup : Form
    {
        private ISS2FrontListForm iSS2FrontListForm;
        private string _FromForm;
        private ISS2FrontManageForm iSS2FrontManageForm;
        private SaleOrderWarehouseListForm saleOrderWarehouseListForm;
        private CheckFrontStockReport checkFrontStockReport;
        private CheckFrontStockForm checkFrontStockForm;

        public SelectedUserPopup()
        {
            InitializeComponent();
        }

        public SelectedUserPopup(ISS2FrontListForm iSS2FrontListForm)
        {
            InitializeComponent();
            this.iSS2FrontListForm = iSS2FrontListForm;
            _FromForm = "ISS2FrontListForm";
        }

        public SelectedUserPopup(ISS2FrontManageForm iSS2FrontManageForm)
        {
            InitializeComponent();
            this.iSS2FrontManageForm = iSS2FrontManageForm;
            _FromForm = "ISS2FrontManageForm";
        }

        public SelectedUserPopup(SaleOrderWarehouseListForm saleOrderWarehouseListForm)
        {
            InitializeComponent();
            _FromForm = "SaleOrderWarehouseListForm";
            this.saleOrderWarehouseListForm = saleOrderWarehouseListForm;
        }

        public SelectedUserPopup(CheckFrontStockReport checkFrontStockReport)
        {
            InitializeComponent();
            _FromForm = "CheckFrontStockReport";
            this.checkFrontStockReport = checkFrontStockReport;
        }

        public SelectedUserPopup(CheckFrontStockForm checkFrontStockForm)
        {
            InitializeComponent();
            _FromForm = "CheckFrontStockForm";
            this.checkFrontStockForm = checkFrontStockForm;
        }

        private void SelectedUserPopup_Load(object sender, EventArgs e)
        {
            foreach (var item in Singleton.SingletonPriority1.Instance().Users.Where(w=>w.Enable == true).ToList())
            {
                dataGridView1.Rows.Add(item.Id, item.Name, item.Description);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// เลือก user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string id = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
            Console.WriteLine(id);
            SendDataChoose(id);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string id = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
            Console.WriteLine(id);
            SendDataChoose(id);
        }
        private void SendDataChoose(string id)
        {

            var data = Singleton.SingletonPriority1.Instance().Users.SingleOrDefault(w => w.Id == id);
            //
            switch (_FromForm)
            {
                case "ISS2FrontListForm":
                    this.iSS2FrontListForm.BinddingUser(data);
                    break;

                case "ISS2FrontManageForm":
                    this.iSS2FrontManageForm.BinddingUser(data);
                    break;

                case "SaleOrderWarehouseListForm":
                    this.saleOrderWarehouseListForm.BinddingUser(data);
                    break;
                case "CheckFrontStockReport":
                    this.checkFrontStockReport.BinddingUser(data);
                    break;
                case "CheckFrontStockForm":
                    this.checkFrontStockForm.BinddingUser(data);
                    break;
                    
                default:
                    break;
            }
            this.Dispose();
        }

        private void textBoxSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                    string key = textBoxSearchKey.Text.Trim();
                    if (radioButtonCode.Checked == true)
                    {
                        var data = Singleton.SingletonPriority1.Instance().Users.Where(w => w.Enable == true && w.Id.Contains(key)).OrderByDescending(w => w.CreateDate).Take(200).ToList();
                        foreach (var item in data)
                        {
                            dataGridView1.Rows.Add(item.Id, item.Name, item.Description);
                        }
                    }
                    else
                    {
                        var data = Singleton.SingletonPriority1.Instance().Users.Where(w => w.Enable == true && w.Name.Contains(key)).OrderByDescending(w => w.CreateDate).Take(200).ToList();
                        foreach (var item in data)
                        {
                            dataGridView1.Rows.Add(item.Id, item.Name, item.Description);
                        }
                    }
                    break;
                default:
                    break;
            }
            
        }
    }
}
