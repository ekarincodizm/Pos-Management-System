using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class FormBudgetYear : Form
    {
        public FormBudgetYear()
        {
            InitializeComponent();
        }
        CultureInfo ThaiCulture = new CultureInfo("th-TH");
        int RowId;
        private void FormBudgetYear_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = " dd / MM / yyyy";
            dateTimePicker2.CustomFormat = " dd / MM / yyyy";
            comboBox1.SelectedIndex = 0;
            TbbudgetYear.Focus();
        }
        private void DataGrid1RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        void clearfrom()
        {
            TbbudgetYear.Text = "";
            TbDes.Text = "";
            TbDes2.Text = "";
            TbBudgetYear2.Text = "";
            RowId = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            clearfrom();
            var Bg = Singleton.SingletonBudgetYearNew.Instance().BudgetYear.Where(w => w.Id > 0 & (comboBox1.SelectedIndex == 0 ? w.Enable == true : (comboBox1.SelectedIndex == 1 ? w.Enable == false : w.Id > 0))).OrderByDescending(o => o.IsCurrent).OrderByDescending(o => o.StartDate).ToList();
            if (Bg == null)
            {
                return;
            }
            foreach (var item in Bg)
            {
                dataGridView1.Rows.Add(item.Id, item.ThaiYear, item.Description, item.StartDate.ToString("dd/MM/yyyy", ThaiCulture), item.EndDate.ToString("dd/MM/yyyy", ThaiCulture), item.CreateBy, item.CreateDate.ToString("dd/MM/yyyy", ThaiCulture), (item.Enable == true ? "ใช้งาน" : "ไม่ได้ใช้งาน"), (item.IsCurrent == true ? "Default" : ""));

            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Status"].Value.ToString() == "ไม่ได้ใช้งาน")
                {
                    dataGridView1["Status", row.Index].Style.BackColor = Color.Red;
                }
                if (row.Cells["Used"].Value.ToString() == "Default")
                {
                    dataGridView1["Used", row.Index].Style.BackColor = Color.Lime;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (TbbudgetYear.Text == "")
                {
                    MessageBox.Show("กรุณากรอก ปีงบประมาณ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult dialogResult = MessageBox.Show("ยืนยันทำรายการใช่หรือไม่", "Some Title", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DateTime dat = Library.DateTimeServer();
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        var ds = db.BudgetYear.Where(w => w.Enable == true).ToList();

                        if (ds.Where(w => w.StartDate <= dateTimePicker1.Value & w.EndDate >= dateTimePicker1.Value).ToList().Count > 0)
                        {
                            MessageBox.Show("ไม่สามารถใช้วันที่เริ่มต้นนี้ได้ เนื่องจากมีอยุ่ในงบประมาณอื่นเเล้ว", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (ds.Where(w => w.StartDate <= dateTimePicker2.Value & w.EndDate >= dateTimePicker2.Value).ToList().Count > 0)
                        {
                            MessageBox.Show("ไม่สามารถใช้วันที่สิ้นสุดนี้ได้ เนื่องจากมีอยุ่ในงบประมาณอื่นเเล้ว", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (ds.Where(w => w.ThaiYear == TbbudgetYear.Text.Trim()).ToList().Count > 0)
                        {
                            MessageBox.Show("ปีงบประมาณซ้ำ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        BudgetYear by = new BudgetYear();
                        by.Name = TbbudgetYear.Text.Trim();
                        by.ThaiYear = TbbudgetYear.Text.Trim();
                        by.Description = (TbDes.Text == "" ? null: TbDes.Text);
                        by.EngYear = (int.Parse(TbbudgetYear.Text.Trim()) + 543).ToString();
                        by.StartDate = dateTimePicker1.Value;
                        by.EndDate = dateTimePicker2.Value;
                        by.CodeYear = TbbudgetYear.Text.Trim().Substring(2, 2);
                        by.IsCurrent = false;
                        by.Enable = true;
                        by.CreateDate = dat;
                        by.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        by.UpdateDate = dat;
                        by.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        db.BudgetYear.Add(by);
                        db.SaveChanges();
                    }
                    MessageBox.Show("เพิ่มข้อมูลสำเร็๋จ", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Singleton.SingletonBudgetYearNew.SetInstance();
                    comboBox1_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TbYKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TbBudgetYear2.Text = "";
            TbDes2.Text = "";
            int index = dataGridView1.CurrentRow.Index;
            if (dataGridView1.Rows[index].Cells["Status"].Value.ToString() == "ใช้งาน") 
            {
                int id = int.Parse(dataGridView1.Rows[index].Cells[0].Value.ToString());
                var member = Singleton.SingletonBudgetYearNew.Instance().BudgetYear.SingleOrDefault(w => w.Id == id);
                RowId = member.Id;
                TbBudgetYear2.Text = member.ThaiYear;
                TbDes2.Text = member.Description;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (TbBudgetYear2.Text == "")
                {
                    return;
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("ยืนยันทำรายการใช่หรือไม่", "Some Title", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        using (SSLsEntities db = new SSLsEntities())
                        {
                            var ds = db.BudgetYear.Where(w => w.Id == RowId & w.Enable == true).FirstOrDefault();
                            if (ds != null)
                            {
                                ds.UpdateDate = Library.DateTimeServer(); ;
                                ds.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                                ds.Enable = false;
                                db.SaveChanges();
                            }

                        }
                        MessageBox.Show("สำเร็๋จ", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Singleton.SingletonBudgetYearNew.SetInstance();
                        comboBox1_SelectedIndexChanged(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                if (TbBudgetYear2.Text == "")
                {
                    return;
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("ยืนยันทำรายการใช่หรือไม่", "Some Title", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        using (SSLsEntities db = new SSLsEntities())
                        {
                            var ds = db.BudgetYear.Where(w => w.Id == RowId & w.Enable == true).FirstOrDefault();
                            if (ds != null)
                            {

                                var dss = db.BudgetYear.Where(w => w.IsCurrent == true & w.Enable == true).FirstOrDefault();
                                if (dss != null)
                                {
                                    dss.IsCurrent = false;
                                    dss.UpdateDate = Library.DateTimeServer(); ;
                                    dss.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                                }
                                ds.IsCurrent = true;
                                ds.UpdateDate = DateTime.Now;
                                ds.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                                db.SaveChanges();
                            }
                        }
                        MessageBox.Show("สำเร็๋จ", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Singleton.SingletonBudgetYearNew.SetInstance();
                        comboBox1_SelectedIndexChanged(sender, e);
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
