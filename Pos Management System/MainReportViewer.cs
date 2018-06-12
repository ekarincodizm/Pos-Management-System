using Microsoft.Reporting.WinForms;
using Pos_Management_System.Repository;
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
    public partial class MainReportViewer : Form
    {
        private int id;
        private POApproveManage pOApproveManage;
        private POEditForm pOEditForm;
        private PONonApproveEditForm pONonApproveEditForm;
        private POStatusManageForm pOStatusManageForm;

        public MainReportViewer(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        public MainReportViewer(CreatePOForm cp)
        {
            InitializeComponent();
        }

        public MainReportViewer(POApproveManage pOApproveManage, int id)
        {
            InitializeComponent();
            this.pOApproveManage = pOApproveManage;
            this.id = id;
        }

        public MainReportViewer(PONonApproveEditForm pONonApproveEditForm, int id)
        {
            InitializeComponent();
            this.pONonApproveEditForm = pONonApproveEditForm;
            this.id = id;
        }

        public MainReportViewer(POStatusManageForm pOStatusManageForm, int id)
        {
            InitializeComponent();
            this.pOStatusManageForm = pOStatusManageForm;
            this.id = id;
        }

        public MainReportViewer(POEditForm pOEditForm, int id)
        {
            InitializeComponent();
            this.pOEditForm = pOEditForm;
            this.id = id;
        }

        public MainReportViewer(CreatePOForm cp, int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void MainReportViewer_Load(object sender, EventArgs e)
        {
            var data = PORepository.GetPOFormPrint(this.id);
            POHeaderBindingSource.DataSource = data;
            POProductDetailsBindingSource.DataSource = data.PODetails;
            this.reportViewer1.RefreshReport();
        }


    }
}
