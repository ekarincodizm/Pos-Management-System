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
    public partial class SelectedBranchPopup : Form
    {
        private List<Branch> branch;
        private SaleOrderWarehouseForm saleOrderWarehouseForm;
        private WmsTransferBranchForm wmsTransferBranchForm;
        string _FromForm = "";
        private TransferOutForm transferOutForm;
        private ManageProductForm manageProductForm;

        public SelectedBranchPopup()
        {
            InitializeComponent();
        }

        public SelectedBranchPopup(SaleOrderWarehouseForm saleOrderWarehouseForm, List<Branch> branch)
        {
            InitializeComponent();
            this.saleOrderWarehouseForm = saleOrderWarehouseForm;
            this.branch = branch;
            _FromForm = "SaleOrderWarehouseForm";
        }

        public SelectedBranchPopup(WmsTransferBranchForm wmsTransferBranchForm)
        {
            InitializeComponent();
            this.wmsTransferBranchForm = wmsTransferBranchForm;
            _FromForm = "WmsTransferBranchForm";
        }

        public SelectedBranchPopup(TransferOutForm transferOutForm)
        {
            InitializeComponent();
            this.transferOutForm = transferOutForm;
            _FromForm = "TransferOutForm";
        }

        public SelectedBranchPopup(ManageProductForm manageProductForm)
        {
            InitializeComponent();
            _FromForm = "ManageProductForm";
            this.manageProductForm = manageProductForm;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void SelectedBranchPopup_Load(object sender, EventArgs e)
        {
            foreach (var item in Singleton.SingletonBranch.Instance().Branchs)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, "สาขา " + item.BranchNo, item.Description);
            }
        }
        /// <summary>
        /// เลือก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            Console.WriteLine(id);
            SendDataChoose(id);
        }
        /// <summary>
        /// เลือก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            Console.WriteLine(id);
            SendDataChoose(id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        void SendDataChoose(int id)
        {
            var data = Singleton.SingletonBranch.Instance().Branchs.SingleOrDefault(w => w.Id == id);
            switch (_FromForm)
            {
                case "SaleOrderWarehouseForm":
                    this.saleOrderWarehouseForm.BinddingBranchSelected(data);
                    break;
                case "WmsTransferBranchForm":
                    this.wmsTransferBranchForm.BinddingBranchSelected(data);
                    break;
                case "TransferOutForm":
                    this.transferOutForm.BinddingBranchSelected(data);
                    break;
                case "ManageProductForm":
                   
                    break;
                    
            }           
            this.Dispose();
        }
    }
}
