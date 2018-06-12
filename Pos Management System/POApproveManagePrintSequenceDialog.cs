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
    public partial class POApproveManagePrintSequenceDialog : Form
    {
        private POApproveManage pOApproveManage;
        private int v;
        private string _code;

        public POApproveManagePrintSequenceDialog()
        {
            InitializeComponent();
        }

        public POApproveManagePrintSequenceDialog(POApproveManage pOApproveManage, int v, string code)
        {
            InitializeComponent();
            this.pOApproveManage = pOApproveManage;
            this.v = v;
            _code = code;

        }

        private void POApproveManagePrintSequenceDialog_Load(object sender, EventArgs e)
        {
            labelValue.Text = v-1 + "";
            textBoxKeyValue.Text = 0 + "";
        }

        private void textBoxKeyValue_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    PrintConfirm();
                    break;
                default:
                    break;
            }
        }

        private void PrintConfirm()
        {
            int seq = int.Parse(textBoxKeyValue.Text.Trim());
            if (seq > v)
            {
                MessageBox.Show("ไม่ถูกต้อง");
            }
            else
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    var data = db.POHeader.SingleOrDefault(w => w.SequenceEdit == seq && w.PONo == _code);
                    MainReportViewer mr = new MainReportViewer(data.Id);
                    mr.ShowDialog();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintConfirm();
        }
    }
}
