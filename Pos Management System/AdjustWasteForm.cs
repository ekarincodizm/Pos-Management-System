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
    public partial class AdjustWasteForm : Form
    {
        public AdjustWasteForm()
        {
            InitializeComponent();
        }

        private void AdjustWasteForm_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
            DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[0].Cells["Column9"]);
            cell.DataSource = new string[] { "รับเข้า", "จ่ายออก" };

        }
        int row;
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.row = e.RowIndex;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.row = e.RowIndex;
                switch (e.ColumnIndex)
                {
                    case 1:
                        SelectedProductPopup obj = new SelectedProductPopup(this);
                        obj.ShowDialog();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// bindding สินค้าที่เลือก
        /// </summary>
        /// <param name="id">product dtl</param>
        public void BinddingProduct(int id)
        {
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.row = e.RowIndex;
        }
        int colCode = 0;
        int colSearch = 1;
        int colName = 2;
        int colPZ = 3;
        int colUnit = 4;
        int colVatType = 5;
        int colRemaining = 6;
        int colEditQty = 7;
        int colAction = 8;
        int colRemark = 9;
        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            int col = dataGridView1.CurrentCell.ColumnIndex;
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (col == colSearch)
                    {
                        //SelectedProductPopup obj = new SelectedProductPopup(this);
                        //obj.ShowDialog();
                    }
                    else if (col == colRemark)
                    {
                        //TotalSummary();
                        if (row == dataGridView1.Rows.Count - 1)
                        {
                            //var getBarcode = dataGridView1.Rows[row].Cells[colCode].Value;
                            //if (getBarcode == null)
                            //{
                            //    return;
                            //}
                            //else if (getBarcode.ToString() == "")
                            //{
                            //    return;
                            //}
                            dataGridView1.Rows.Add();
                            DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[row].Cells["Column9"]);
                            cell.DataSource = new string[] { "รับเข้า", "จ่ายออก" };

                            dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colCode];
                            dataGridView1.BeginEdit(true);
                        }
                        else
                        {
                            return;
                        }
                    }
                    break;
            }
        }
    }
}
