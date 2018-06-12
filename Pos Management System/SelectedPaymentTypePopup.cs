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
    public partial class SelectedPaymentTypePopup : Form
    {
        private List<PaymentType> data;
        private SaleOrderWarehouseForm saleOrderWarehouseForm;

        public SelectedPaymentTypePopup()
        {
            InitializeComponent();
        }

        public SelectedPaymentTypePopup(SaleOrderWarehouseForm saleOrderWarehouseForm, List<PaymentType> data)
        {
            InitializeComponent();
            this.saleOrderWarehouseForm = saleOrderWarehouseForm;
            this.data = data;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void SelectedPaymentTypePopup_Load(object sender, EventArgs e)
        {
            foreach (var item in data)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Description);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
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
        /// <summary>
        /// เลือก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            SendDataChoose(id);
        }
        /// <summary>
        /// Ready Bindding Payment
        /// </summary>
        /// <param name="id"></param>
        void SendDataChoose(int id)
        {
            var data = this.data.SingleOrDefault(w => w.Id == id);
            this.saleOrderWarehouseForm.BinddingPaytmentSelected(data);
            this.Dispose();
        }
    }
}
