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
    public partial class CheckFrontStockReport : Form
    {
        public CheckFrontStockReport()
        {
            InitializeComponent();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        int colSelect = 12;
        int colId = 0;
        private System.Windows.Forms.DataGridView songsDataGridView = new System.Windows.Forms.DataGridView();
        CheckBox ckBox = new CheckBox();
        int width_columcheckbox = 50;
        private void CheckFrontStockReport_Load(object sender, EventArgs e)
        {
            DataGridViewCheckBoxColumn ColumnCheckBox = new DataGridViewCheckBoxColumn();
            ColumnCheckBox.Width = width_columcheckbox;
            ColumnCheckBox.DataPropertyName = "Select";
            //dataGridView1.Columns.Add(ColumnCheckBox);
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(colSelect, -1, true);
            ckBox.Size = new Size(14, 14);
            rect.X = rect.Location.X + (rect.Width / 2) - (ckBox.Width / 2);
            rect.Y += 3;
            ckBox.Location = rect.Location;
            this.ckBox.CheckedChanged += new EventHandler(this.ckBox_CheckedChanged);
            dataGridView1.Controls.Add(ckBox);
            dataGridView1.Columns[colSelect].Frozen = false;
        }
        private void ckBox_CheckedChanged(object sender, EventArgs e)
        {
            int i = 0;
            if (ckBox.Checked == true)
            {
                for (int j = 0; j <= this.dataGridView1.RowCount - 1; j++)
                {
                    this.dataGridView1[colSelect, j].Value = true;
                }
            }
            else
            {
                for (int j = 0; j <= this.dataGridView1.RowCount - 1; j++)
                {
                    this.dataGridView1[colSelect, j].Value = false;
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colSelect)
            {                
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                }
                else if (bool.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == false)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                }
                else
                {
                    // true
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                }
            }
        }
        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(colSelect, -1, true);
            ckBox.Size = new Size(14, 14);
            rect.X = rect.Location.X + (rect.Width / 2) - (ckBox.Width / 2);
            rect.Y += 3;
            ckBox.Location = rect.Location;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<CheckStockDetails> list = new List<CheckStockDetails>();
                if (_UserId == null)
                {
                    list = db.CheckStockDetails.Where(w => w.Enable == true &&
       DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
       DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                }
                else
                {
                    list = db.CheckStockDetails.Where(w => w.Enable == true && w.CreateBy == _UserId &&
       DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
       DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                }

                foreach (var item in list)
                {
                    string stat = "ตรวจนับ";
                    if (item.ConfirmDate != null)
                    {
                        stat = "ยืนยัน";
                    }
                    decimal total = (item.QtyUnitCheck - item.QtyUnitOnHand) * item.ProductDetails.CostAndVat;
                    dataGridView1.Rows.Add
                        (
                        item.Id,
                            Library.ConvertDateToThaiDate(item.CreateDate),
                            Library.GetFullNameUserById(item.CreateBy),
                            item.ProductDetails.Code,
                            item.ProductDetails.Products.ThaiName,
                            item.ProductDetails.ProductUnit.Name,
                            Library.ConvertDecimalToStringForm(item.QtyUnitOnHand),
                            Library.ConvertDecimalToStringForm(item.QtyUnitCheck),
                            Library.ConvertDecimalToStringForm(item.QtyUnitCheck - item.QtyUnitOnHand),
                                      Library.ConvertDecimalToStringForm(item.ProductDetails.CostAndVat),
                                      Library.ConvertDecimalToStringForm(total),
                                      stat
                        );
                }
            }

        }
        /// <summary>
        /// ค้นหาผู้นับ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            SelectedUserPopup obj = new SelectedUserPopup(this);
            obj.ShowDialog();
        }
        string _UserId;
        public void BinddingUser(Users send)
        {
            _UserId = send.Id;
            textBoxEmpCode.Text = send.Id;
            textBoxEmpName.Text = send.Name;
        }

        private void textBoxEmpCode_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string userCheck = textBoxEmpCode.Text.Trim();
                    Users user = Singleton.SingletonPriority1.Instance().Users.SingleOrDefault(w => w.Id == userCheck);
                    if (user == null)
                    {
                        _UserId = null;
                        textBoxEmpCode.Text = "";
                        textBoxEmpName.Text = "";
                        return;
                    }
                    BinddingUser(user);
                    break;
                default:
                    break;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _UserId = null;
            textBoxEmpCode.Text = "";
            textBoxEmpName.Text = "";
            return;
        }
        /// <summary>
        /// confirm ยอด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    ConfirmCommit();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void ConfirmCommit()
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int id = int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());
                string check = "";
                if (dataGridView1.Rows[i].Cells[12].Value == null)
                {
                    check = "FALSE";
                }
                else
                {
                    check = dataGridView1.Rows[i].Cells[12].Value.ToString();
                }

     
                Console.WriteLine(id + " " + id + " --------- " + check);
                if (check.ToUpper() == "TRUE")
                {
                    ids.Add(id);
                }
            }
            using (SSLsEntities db = new SSLsEntities())
            {
                var getCheckStock = db.CheckStockDetails.Where(w => ids.Contains(w.Id));
                foreach (var item in getCheckStock)
                {
                    
                }
            }
        }
    }
}
