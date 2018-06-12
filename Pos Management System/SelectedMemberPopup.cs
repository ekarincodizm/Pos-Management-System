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
    public partial class SelectedMemberPopup : Form
    {
        private SaleOrderWarehouseForm saleOrderWarehouseForm;
        private List<Member> data;

        public SelectedMemberPopup()
        {
            InitializeComponent();
        }

        public SelectedMemberPopup(SaleOrderWarehouseForm saleOrderWarehouseForm, List<Member> data)
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
        /// <summary>
        /// เลือก สมาชิก
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
        /// double click สวมาชิก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            Console.WriteLine(id);
            SendDataChoose(id);
        }
        void SendDataChoose(int id)
        {
            var data = this.data.SingleOrDefault(w => w.Id == id);
            this.saleOrderWarehouseForm.BinddingMemberSelected(data);
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// Bindding grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedMemberPopup_Load(object sender, EventArgs e)
        {
            foreach (var item in data)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Description);
            }
        }
        /// <summary>
        /// ค้นหา Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string key = textBoxSearchKey.Text.Trim();
                    var result = new List<Member>();
                    if (radioButtonCode.Checked)
                    {
                        // ถ้าหาด้วย code
                        result = data.Where(w => w.Code.Contains(key)).ToList();
                    }
                    else
                    {
                        // ถ้าหาด้วยชื่อ
                        result = data.Where(w => w.Name.Contains(key)).ToList();
                    }
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                    foreach (var item in result)
                    {
                        dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Description);
                    }
                    break;
                default:
                    break;
            }

        }
    }
}
