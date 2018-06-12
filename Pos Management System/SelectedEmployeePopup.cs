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
    public partial class SelectedEmployeePopup : Form
    {
        private WmsTransferBranchForm wmsTransferBranchForm;
        private string _FromForm;
        private StoreFrontAddForm storeFrontAddForm;
        private ISS2FrontListForm iSS2FrontListForm;
        private ISS2FrontAllowConfirmForm iSS2FrontAllowConfirmForm;
        private TransferOutForm transferOutForm;
        private TransferOutEditForm transferOutEditForm;
       

        public SelectedEmployeePopup()
        {
            InitializeComponent();
        }

        public SelectedEmployeePopup(WmsTransferBranchForm wmsTransferBranchForm)
        {
            InitializeComponent();
            this.wmsTransferBranchForm = wmsTransferBranchForm;
            _FromForm = "WmsTransferBranchForm";
        }

        public SelectedEmployeePopup(StoreFrontAddForm storeFrontAddForm)
        {
            InitializeComponent();
            this.storeFrontAddForm = storeFrontAddForm;
            _FromForm = "StoreFrontAddForm";
        }

        public SelectedEmployeePopup(ISS2FrontListForm iSS2FrontListForm)
        {
            InitializeComponent();
            this.iSS2FrontListForm = iSS2FrontListForm;
            _FromForm = "ISS2FrontListForm";
        }

        public SelectedEmployeePopup(ISS2FrontAllowConfirmForm iSS2FrontAllowConfirmForm)
        {
            InitializeComponent();
            this.iSS2FrontAllowConfirmForm = iSS2FrontAllowConfirmForm;
            _FromForm = "ISS2FrontAllowConfirmForm";
        }

        public SelectedEmployeePopup(TransferOutForm transferOutForm)
        {
            InitializeComponent();
            this.transferOutForm = transferOutForm;
            _FromForm = "TransferOutForm";
        }

        public SelectedEmployeePopup(TransferOutEditForm transferOutEditForm)
        {
            InitializeComponent();
            this.transferOutEditForm = transferOutEditForm;
            _FromForm = "TransferOutEditForm";
        }


        private void SelectedEmployeePopup_Load(object sender, EventArgs e)
        {
            foreach (var item in Singleton.SingletonEmployee.Instance().Employee)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Description);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            Console.WriteLine(id);
            SendDataChoose(id);
        }

        private void SendDataChoose(int id)
        {

            var data = Singleton.SingletonEmployee.Instance().Employee.SingleOrDefault(w => w.Id == id);
            //
            switch (_FromForm)
            {
                case "WmsTransferBranchForm":
                    this.wmsTransferBranchForm.BinddingEmployee(data);
                    break;
                case "StoreFrontAddForm":
                    this.storeFrontAddForm.BinddingEmployee(data);
                    break;
                case "ISS2FrontAllowConfirmForm":
                    this.iSS2FrontAllowConfirmForm.BinddingEmployee(data);
                    break;
                case "TransferOutForm":
                    this.transferOutForm.BinddingEmployee(data);
                    break;
                case "TransferOutEditForm":
                    this.transferOutEditForm.BinddingEmployee(data);
                    break;
              
                default:
                    break;
            }
            this.Dispose();
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            Console.WriteLine(id);
            SendDataChoose(id);
        }
    }
}
