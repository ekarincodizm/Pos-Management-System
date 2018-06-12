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
    public partial class PrintInvoiceForm : Form
    {
        public PrintInvoiceForm()
        {
            InitializeComponent();
        }
        int colId = 0;
        int colNo = 1;
        int colDate = 2;
        int colMemberNo = 3;
        int colMemberName = 4;
        int colDebtNo = 5;
        int colDebtName = 6;
        int colSumList = 7;
        int colUnvate = 8;
        int colHasvat = 9;
        int colTotal = 10;
        int colChoose = 11;
        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerBox = ((CheckBox)dataGridView1.Controls.Find("checkboxHeader", true)[0]);
            int index = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[colChoose].Value = headerBox.Checked;
            }
        }
        void ControlsSetColumnHeaderCheckbox()
        {
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(colChoose, -1, true);
            // set checkbox header to center of header cell. +1 pixel to position 
            rect.Y = 3;
            rect.X = rect.Location.X + (rect.Width / 4) + 2;
            CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            //datagridview[0, 0].ToolTipText = "sdfsdf";
            checkboxHeader.Size = new Size(18, 18);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);
            dataGridView1.Controls.Add(checkboxHeader);

        }
        private void PrintInvoiceForm_Load(object sender, EventArgs e)
        {
            ControlsSetColumnHeaderCheckbox();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// ค้นหา ตามรายละเอียด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                dataGridView1.Rows.Add(i, "aa" + i, "asdf", "sdfdf");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
