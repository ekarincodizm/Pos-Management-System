using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Pos_Management_System.DebtorPayDateForm;

namespace Pos_Management_System
{
    public partial class ConfirmPayDateDebtorDialog : Form
    {
        private DebtorPayDateForm debtorPayDateForm;
        private List<StackSelected> stackDebtor;
        public ConfirmPayDateDebtorDialog(DebtorPayDateForm debtorPayDateForm, List<StackSelected> stackDebtor)
        {
            InitializeComponent();
            this.debtorPayDateForm = debtorPayDateForm;
            this.stackDebtor = stackDebtor;
           
        }

        private void ConfirmPayDateDebtorDialog_Load(object sender, EventArgs e)
        {
            groupBox1.Text = "ยืนยันการชำระลูกหนี้ " + stackDebtor.Count() + " รายการ";
            foreach (var item in stackDebtor)
            {
                listBox1.Items.Add("ลูกหนี้ " + item.DebtorCode + " : " + item.DebtorName + " บิล : " + item.InvoiceNo);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// ยืนยันตัดชำระลูกหนี้
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    SaveCommit();
                    break;
                case DialogResult.No:
                    break;
            }
        }
        /// <summary>
        /// ยืนยันชำระหนี้
        /// </summary>
        private void SaveCommit()
        {
            SSLsEntities db = new SSLsEntities();
            try
            {
                foreach (var item in stackDebtor)
                {
                    
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                db.Dispose();
            }
            

        }
    }
}
