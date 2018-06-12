using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class ManageDebtorForm : Form
    {
        public ManageDebtorForm()
        {
            InitializeComponent();
        }

        private void ManageDebtorForm_Load(object sender, EventArgs e)
        {
            foreach (var item in Singleton.SingletonDebtor.Instance().Debtors.Take(200).ToList())
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.TaxNo, item.Address, item.Tel, item.LineId, item.Email);
            }
            /// row color
            RowColor();
        }

        private void RowColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((i % 2) == 0)
                {
                    // คู่
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    // คี่
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.AliceBlue;
                }
            }
        }

        int colId = 0;
        int colCode = 1;
        int colName = 2;
        int colTax = 3;
        int colAddress = 4;
        int colTel = 5;
        int colLine = 6;
        int colMail = 7;
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// เมื่อมีการเปลี่ยน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int _id = 0;
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // bindding ui
            int currentRow = dataGridView1.CurrentRow.Index;
            string id = dataGridView1.Rows[currentRow].Cells[colId].Value.ToString();
            string code = dataGridView1.Rows[currentRow].Cells[colCode].Value.ToString();
            string name = dataGridView1.Rows[currentRow].Cells[colName].Value.ToString();
            string tax = dataGridView1.Rows[currentRow].Cells[colTax].Value.ToString();
            string address = dataGridView1.Rows[currentRow].Cells[colAddress].Value.ToString();
            string tel = dataGridView1.Rows[currentRow].Cells[colTel].Value.ToString();
            string line = dataGridView1.Rows[currentRow].Cells[colLine].Value.ToString();
            string mail = dataGridView1.Rows[currentRow].Cells[colMail].Value.ToString();
            _id = int.Parse(id);
            textBoxCode.Text = code;
            textBoxName.Text = name;
            textBoxTax.Text = tax;
            textBoxAddress.Text = address;
            textBoxTel.Text = tel;
            textBoxLine.Text = line;
            textBoxEmail.Text = mail;
        }
        /// <summary>
        /// เพิ่มใหม่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddNew_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        /// <summary>
        /// 
        /// </summary>
        private void ResetForm()
        {
            _id = 0;
            textBoxCode.Text = "";
            textBoxName.Text = "";
            textBoxTax.Text = "";
            textBoxAddress.Text = "";
            textBoxTel.Text = "";
            textBoxLine.Text = "";
            textBoxEmail.Text = "";
        }
        /// <summary>
        /// คีย์ค้นหา 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    Reload(textBoxSearchKey.Text.Trim());
                    break;
                default:
                    break;
            }
        }
        void Reload(string key)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            List<Debtor> debtor = new List<Debtor>();
            debtor = Singleton.SingletonDebtor.Instance().Debtors;

            var fromSearch = new List<Debtor>();
            if (radioButtonCode.Checked == true)
            {
                fromSearch = debtor.Where(w => w.Code.Contains(key)).OrderByDescending(w => w.CreateDate).ToList();
            }
            else
            {
                fromSearch = debtor.Where(w => w.Name.Contains(key)).OrderByDescending(w => w.CreateDate).ToList();
            }
            foreach (var item in fromSearch.Take(200).ToList())
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.TaxNo, item.Address, item.Tel, item.LineId, item.Email);
            }
            /// row color
            RowColor();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
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
        /// 
        /// </summary>
        private void SaveCommit()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                if (_id == 0)
                {
                    // add new
                    Debtor d = new Debtor();
                    d.Code = textBoxCode.Text.Trim();
                    d.Name = textBoxName.Text.Trim();
                    d.Address = textBoxAddress.Text.Trim();
                    d.Tel = textBoxTel.Text.Trim();
                    d.Email = textBoxEmail.Text.Trim();
                    d.TaxNo = textBoxTax.Text.Trim();
                    d.LineId = textBoxLine.Text.Trim();
                    d.Fax = "-";
                    d.Description = null;
                    d.CreateDate = DateTime.Now;
                    d.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    d.UpdateDate = DateTime.Now;
                    d.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    d.Enable = true;
                    db.Debtor.Add(d);
                    db.SaveChanges();

                    Debtor data = db.Debtor.OrderByDescending(w => w.CreateDate).FirstOrDefault(w => w.CreateBy == d.CreateBy);
                    Singleton.SingletonDebtor.Instance().Debtors.Add(data);
                }
                else
                {
                    // Update
                    var d = db.Debtor.SingleOrDefault(w => w.Id == _id);
                    var oldDeb = Singleton.SingletonDebtor.Instance().Debtors.SingleOrDefault(w => w.Id == _id);
                    Singleton.SingletonDebtor.Instance().Debtors.Remove(oldDeb);

                    d.Code = textBoxCode.Text.Trim();
                    d.Name = textBoxName.Text.Trim();
                    d.Address = textBoxAddress.Text.Trim();
                    d.Tel = textBoxTel.Text.Trim();
                    d.Email = textBoxEmail.Text.Trim();
                    d.TaxNo = textBoxTax.Text.Trim();
                    d.LineId = textBoxLine.Text.Trim();
                    d.UpdateDate = DateTime.Now;
                    d.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    db.Entry(d).State = EntityState.Modified;

                    Debtor data = db.Debtor.OrderByDescending(w => w.UpdateDate).FirstOrDefault(w => w.UpdateBy == d.UpdateBy);
                    Singleton.SingletonDebtor.Instance().Debtors.Add(data);
                    db.SaveChanges();
                }
                Reload(textBoxSearchKey.Text.Trim());
            }
        }
    }
}
