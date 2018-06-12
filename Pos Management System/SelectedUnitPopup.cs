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
    public partial class SelectedUnitPopup : Form
    {
        private ManageProductForm manageProductForm;
        private string _ReturnForm;
        public SelectedUnitPopup()
        {
            InitializeComponent();
        }

        public SelectedUnitPopup(ManageProductForm manageProductForm)
        {
            InitializeComponent();
            _ReturnForm = "ManageProductForm";
            this.manageProductForm = manageProductForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void SelectedUnitPopup_Load(object sender, EventArgs e)
        {
            foreach (var item in Singleton.SingletonPUnit.Instance().Units.Where(w => w.Enable == true).ToList())
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Selected();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Selected();
        }
        void Selected()
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            var data = Singleton.SingletonPUnit.Instance().Units.SingleOrDefault(w => w.Id == id);
            switch (_ReturnForm)
            {
                case "ManageProductForm":
                    manageProductForm.BinddingUnit(data);
                    break;
                default:
                    break;
            }
            this.Dispose();
        }

        private void textBoxSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            string key = textBoxSearchKey.Text.Trim();
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();


                    foreach (var item in Singleton.SingletonPUnit.Instance().Units.Where(w => w.Enable == true && w.Name.Contains(key)).ToList())
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
