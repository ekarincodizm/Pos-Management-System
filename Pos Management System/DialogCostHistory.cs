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
    public partial class DialogCostHistory : Form
    {
        private List<CostProductChangeLog> _CostProductChangeLog;

        public DialogCostHistory()
        {
            InitializeComponent();
        }

        public DialogCostHistory(List<CostProductChangeLog> _CostProductChangeLog)
        {
            InitializeComponent();
            this._CostProductChangeLog = _CostProductChangeLog;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void DialogCostHistory_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            foreach (var item in _CostProductChangeLog)
            {
                dataGridView1.Rows.Add(Library.ConvertDateToThaiDate(item.CreateDate, true),
                    Library.GetFullNameUserById(item.CreateBy), 
                    Library.ConvertDecimalToStringForm(item.OldCostAndVat), 
                    Library.ConvertDecimalToStringForm(item.CurrentCostAndVat),
                    item.Description
                    );
            }
            button1.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

    }
}
