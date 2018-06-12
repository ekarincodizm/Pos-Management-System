using CrystalDecisions.Shared;
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
    public partial class ReportOrderWarehouseForm : Form
    {
        private SaleOrderWarehouseListForm saleOrderWarehouseListForm;
        private string docNos;

        public ReportOrderWarehouseForm()
        {
            InitializeComponent();
        }

        public ReportOrderWarehouseForm(SaleOrderWarehouseListForm saleOrderWarehouseListForm, string docNos)
        {
            InitializeComponent();
            this.saleOrderWarehouseListForm = saleOrderWarehouseListForm;
            this.docNos = docNos;
        }

        private void ReportOrderWarehouseForm_Load(object sender, EventArgs e)
        {

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
        
            Cursor = Cursors.WaitCursor;
            ParameterFields pFields = new ParameterFields();
            ParameterField pJobNo = new ParameterField();
            ParameterDiscreteValue dJobNo = new ParameterDiscreteValue();

            this.Text = "พิมพ์ ใบรถใหญ่";
            pJobNo.ParameterFieldName = "DocNo";
            dJobNo.Value = docNos;
            pJobNo.CurrentValues.Add(dJobNo);
            pFields.Add(pJobNo);

            crystalReportViewer1.ParameterFieldInfo = pFields;

            ConnectionInfo conInfo = new ConnectionInfo();
            var _with1 = conInfo;
            _with1.ServerName = MyConstant._ServerIP;
            _with1.DatabaseName = MyConstant._DBName;
            _with1.UserID = MyConstant._UserID;
            _with1.Password = MyConstant._Password;
            foreach (TableLogOnInfo cnInfo in this.crystalReportViewer1.LogOnInfo)
            {
                cnInfo.ConnectionInfo = conInfo;
            }
            Cursor = Cursors.Default;
        }
    }
}
