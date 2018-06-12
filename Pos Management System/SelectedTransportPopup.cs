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
    public partial class SelectedTransportPopup : Form
    {
        public SelectedTransportPopup()
        {
            InitializeComponent();
        }

        public SelectedTransportPopup(RCVPOForm rCVPOForm)
        {
            InitializeComponent();
            this.rCVPOForm = rCVPOForm;
            _ReturnForm = "RCVPOForm";
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

        private void SelectedTransportPopup_Load(object sender, EventArgs e)
        {
           var data = Singleton.SingletonTransport.Instance().Transport;
            foreach (var item in data)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Description);
            }
        }
        private string _ReturnForm;
        private RCVPOForm rCVPOForm;

        void Selected()
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            var data = Singleton.SingletonTransport.Instance().Transport.SingleOrDefault(w => w.Id == id);
            switch (_ReturnForm)
            {
                case "RCVPOForm":
                    this.rCVPOForm.BinddingTransport(data);
                    break;
                default:
                    break;
            }
            this.Dispose();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Selected();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Selected();
        }
    }
}
