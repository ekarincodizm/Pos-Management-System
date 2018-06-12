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
    public partial class SelectedShelfPopup : Form
    {
        private StoreFrontAddForm storeFrontAddForm;
        private string _FromForm;
        public SelectedShelfPopup()
        {
            InitializeComponent();
        }

        public SelectedShelfPopup(StoreFrontAddForm storeFrontAddForm)
        {
            InitializeComponent();
            this.storeFrontAddForm = storeFrontAddForm;
            _FromForm = "StoreFrontAddForm";
        }

        private void SelectedShelfPopup_Load(object sender, EventArgs e)
        {
            foreach (var item in Singleton.SingletonShlf.Instance().Shelfs)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Description);
            }
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
        void SendDataChoose(int id)
        {
            var data = Singleton.SingletonShlf.Instance().Shelfs.SingleOrDefault(w => w.Id == id);
            switch (_FromForm)
            {
                case "StoreFrontAddForm":
                    this.storeFrontAddForm.BinddingShelfChoose(data);
                    break;
                default:
                    break;
            } 
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            Console.WriteLine(id);
            SendDataChoose(id);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            Console.WriteLine(id);
            SendDataChoose(id);
        }
    }
}
