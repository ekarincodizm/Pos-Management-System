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
    public partial class FormReportViewer : Form
    {
        public FormReportViewer()
        {
            InitializeComponent();
        }
        private FormReportMember formReportMember;
        public FormReportViewer(FormReportMember formReportMember)
        {
            InitializeComponent();
            this.formReportMember = formReportMember;
        }

        private void FormReportViewer_Load(object sender, EventArgs e)
        {

         
        }
    }
}
