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
    public partial class SelectedWarehousePopup : Form
    {
        private WasteWarehouseManageForm wasteWarehouseManageForm;

        public SelectedWarehousePopup()
        {
            InitializeComponent();
        }

        public SelectedWarehousePopup(WasteWarehouseManageForm wasteWarehouseManageForm)
        {
            InitializeComponent();
            this.wasteWarehouseManageForm = wasteWarehouseManageForm;
            _FromForm = "WasteWarehouseManageForm";
        }

        private void SelectedWarehousePopup_Load(object sender, EventArgs e)
        {
            foreach (var item in Singleton.SingletonWarehouse.Instance().Warehouses)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Description);
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            Console.WriteLine(id);
            SendDataChoose(id);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            Console.WriteLine(id);
            SendDataChoose(id);
        }
        private string _FromForm;
        private void SendDataChoose(int id)
        {
            var data = Singleton.SingletonWarehouse.Instance().Warehouses.SingleOrDefault(w => w.Id == id);
            switch (_FromForm)
            {
                case "WasteWarehouseManageForm":
                    this.wasteWarehouseManageForm.BinddingWarehouseSelected(data);
                    break;
            }
            this.Dispose();
        }
    }
}
