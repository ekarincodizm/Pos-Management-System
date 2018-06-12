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
    public partial class SearchProdWasteWarehouseForm : Form
    {
        public SearchProdWasteWarehouseForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานสินค้าเข้าห้องของเสีย " + textBoxBarcode.Text, "");
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
