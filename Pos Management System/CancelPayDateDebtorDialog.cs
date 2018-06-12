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
    public partial class CancelPayDateDebtorDialog : Form
    {
        private DebtorPayDateForm debtorPayDateForm;
        private List<DebtorPayDateForm.StackSelected> stackDebtor;

        public CancelPayDateDebtorDialog()
        {
            InitializeComponent();
        }

        public CancelPayDateDebtorDialog(DebtorPayDateForm debtorPayDateForm, List<DebtorPayDateForm.StackSelected> stackDebtor)
        {
            this.debtorPayDateForm = debtorPayDateForm;
            this.stackDebtor = stackDebtor;
            InitializeComponent();
            groupBox1.Text = "ยืนยันยกเลิกชำระหนี้ " + stackDebtor.Count() + " รายการ";
            foreach (var item in stackDebtor)
            {
                listBox1.Items.Add("ลูกหนี้ " + item.DebtorCode + " : " + item.DebtorName + " บิล : " + item.InvoiceNo);
            }
        }

        private void CancelPayDateDebtorDialog_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
