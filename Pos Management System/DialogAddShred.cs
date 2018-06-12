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
    public partial class DialogAddShred : Form
    {
        ManageMemberForm manageMemberForm;
        public DialogAddShred(ManageMemberForm manageMemberForm)
        {
            InitializeComponent();
            this.manageMemberForm = manageMemberForm;
        }

        private void DialogAddShred_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        decimal value = decimal.Parse(textBox1.Text);
                        manageMemberForm.BinddingAddShared(value);
                        this.Dispose();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("ไม่ถูกต้อง");
                    }
                    break;
            }
        }
    }
}
