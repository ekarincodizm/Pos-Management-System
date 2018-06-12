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
    public partial class MemberAddressForm : Form
    {
        public MemberAddressForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (SSLsEntities db = new SSLsEntities())
            {
                List<Member> members = new List<Member>();
                if (radioButton1.Checked == true)
                {
                    members = db.Member.Where(w => w.Enable == true && w.IsRemoveShared == false).ToList();
                }
                else
                {
                    members = db.Member.Where(w => w.Enable == true && w.IsRemoveShared == true).ToList();
                }
                string date = "";
                foreach (var item in members)
                {
                    date = "คงอยู่";
                    if (item.ResignDate != null)
                    {
                        date = Library.ConvertDateToThaiDate(item.ResignDate);
                    }

                    dataGridView1.Rows.Add(item.Code.Trim(), item.Name.Trim(), item.Address.Trim(), item.TaxId.Trim(), item.Tel.Trim(), date);
                }
            }
        }
        /// <summary>
        /// Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string getName = "";
            if (radioButton1.Checked == true)
            {
                getName = "คงอยู่";
            }
            else
            {
                getName = "ลาออก";
            }
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานสมาชิก " + getName, "");
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
    }
}
