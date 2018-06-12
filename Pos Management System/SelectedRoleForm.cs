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
    public partial class SelectedRoleForm : Form
    {
        private UserRoleManageForm userRoleManageForm;

        public SelectedRoleForm()
        {
            InitializeComponent();
        }

        public SelectedRoleForm(UserRoleManageForm userRoleManageForm)
        {
            InitializeComponent();
            this.userRoleManageForm = userRoleManageForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// /Choose Role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            ChooseRole();
        }
        /// <summary>
        /// double click choose role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ChooseRole();
        }

        private void ChooseRole()
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            this.userRoleManageForm.BinddingRole(id);
            this.Dispose();
        }

        private void SelectedRoleForm_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.Role.Where(w => w.Enable == true).ToList();
                foreach (var item in data)
                {
                    dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Description, Library.ConvertDateToThaiDate(item.UpdateDate), Library.GetFullNameUserById(item.UpdateBy));
                }
            }
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
