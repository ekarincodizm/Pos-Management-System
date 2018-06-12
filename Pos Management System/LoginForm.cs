using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            //textBox1.Text = "admin";
            //textBox2.Text = "admin12345";
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    textBox2.Select();
                    textBox2.SelectAll();
                    break;
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    LoginSuccess();
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginSuccess();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        void LoginSuccess()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                string user = textBox1.Text.Trim();
                string pass = textBox2.Text.Trim();
                var data = db.Users.SingleOrDefault(w => w.Id == user && w.Password == pass);
                if (data != null) // success
                {
                    this.Hide();
                    // set authen
                    Singleton.SingletonAuthen.SetInstance(data.Id, data.Name);
                    Form1 frm1 = new Form1();
                    frm1.Show();
                 
                }
                else // not success
                {
                    MessageBox.Show("ไม่ถูกต้อง กรุณาลองใหม่");
                    textBox2.Select();
                    textBox2.SelectAll();
                }
            } 
        }
    }
}
