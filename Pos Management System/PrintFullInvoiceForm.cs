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
    public partial class PrintFullInvoiceForm : Form
    {
        public PrintFullInvoiceForm()
        {
            InitializeComponent();
        }

        private void PrintFullInvoiceForm_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }
    }
}
