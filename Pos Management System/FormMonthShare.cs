using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class FormMonthShare : Form
    {
        public FormMonthShare()
        {
            InitializeComponent();
        }
        CultureInfo ThaiCulture = new CultureInfo("th-TH");
        int RowId;
        void clearfrom()
        {
            CbAgeM.Text = "เลือก";
            CbD.Text = "เลือก";
            CbM.Text = "เลือก";
            CbAgeM.Text = "เลือก";
            CbM2.Text = "เลือก";
            CbD2.Text = "เลือก";
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
            var Bg = Singleton.SingletonShareMonth.Instance().ShareMonth.Where(w => w.Id > 0 & (comboBox1.SelectedIndex == 0 ? w.Enable == true : (comboBox1.SelectedIndex == 1 ? w.Enable == false : w.Id > 0))).OrderByDescending(o => o.AgeMonth).ThenByDescending(t=>t.CreateDate).ToList();
            if (Bg == null)
            {
                return;
            }
            foreach (var item in Bg)
            {
                dataGridView1.Rows.Add(item.Id, item.AgeMonth, item.Description, item.TermMonthStart.ToString("วัน : dd   เดือน : MM ", ThaiCulture), item.TermMonthEnd.ToString("วัน: dd  เดือน : MM ", ThaiCulture), item.CreateBy, item.CreateDate.ToString("dd /MM/yyyy", ThaiCulture), (item.Enable == true ? "ใช้งาน" : "ไม่ได้ใช้งาน"));

            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Status"].Value.ToString() == "ไม่ได้ใช้งาน")
                {
                    dataGridView1["Status", row.Index].Style.BackColor = Color.Red;
                }
            }
        }

        private void FormMonthShare_Load(object sender, EventArgs e)
        {

            comboBox1.SelectedIndex = 0;
        }

        private void DataGrid1RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (CbAgeM.Text == "เลือก" || CbM.Text == "เลือก" || CbD.Text == "เลือก" || CbM2.Text == "เลือก" || CbD2.Text == "เลือก")
                {
                    MessageBox.Show("กรุณาเลือกข้อมูลให้ครบ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult dialogResult = MessageBox.Show("ยืนยันทำรายการใช่หรือไม่", "Some Title", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DateTime dat = Library.DateTimeServer();
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        var ds = db.ShareMonth.Where(w => w.Enable == true).ToList();

                        if (ds.Where(w => w.TermMonthStart <= DateTime.Parse("2018-" + CbM.Text + "-" + CbD.Text + " 00:00:00") & w.TermMonthEnd >= DateTime.Parse("2018-" + CbM.Text + "-" + CbD.Text + " 00:00:00")).ToList().Count > 0)
                        {
                            MessageBox.Show("ไม่สามารถใช้วันที่เริ่มต้นนี้ได้ เนื่องจากมีอยุ่อายุเดือนอื่นเเล้ว", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (ds.Where(w => w.TermMonthStart <= DateTime.Parse("2018-" + CbM2.Text + "-" + CbD2.Text + " 00:00:00") & w.TermMonthEnd >= DateTime.Parse("2018-" + CbM2.Text + "-" + CbD2.Text + " 00:00:00")).ToList().Count > 0)
                        {
                            MessageBox.Show("ไม่สามารถใช้วันที่สิ้นสุดนี้ได้ เนื่องจากมีอยุ่อายุเดือนอื่นเเล้ว", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (ds.Where(w => w.AgeMonth == int.Parse(CbAgeM.Text.Trim())).ToList().Count > 0)
                        {
                            MessageBox.Show("อายุเดือนซ้ำ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        ShareMonth by = new ShareMonth();
                        by.AgeMonth = int.Parse(CbAgeM.Text.Trim());
                        by.Description = (TbDes.Text == "" ? null : TbDes.Text);
                        by.TermMonthStart = DateTime.Parse("2018-" + CbM.Text + "-" + CbD.Text + " 00:00:00");
                        by.TermMonthEnd = DateTime.Parse("2018-" + CbM2.Text + "-" + CbD2.Text + " 00:00:00");
                        by.Enable = true;
                        by.CreateDate = dat;
                        by.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        db.ShareMonth.Add(by);
                        db.SaveChanges();
                    }
                    MessageBox.Show("เพิ่มข้อมูลสำเร็๋จ", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Singleton.SingletonShareMonth.SetInstance();
                    comboBox1_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
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
                            var ds = db.ShareMonth.Where(w => w.Id == RowId).FirstOrDefault();
                            if (ds != null)
                            {
                                ds.Description = "ยกเลิกโดย " + Singleton.SingletonAuthen.Instance().Id.ToString() + "Date " + (Library.DateTimeServer()).ToString();
                                ds.Enable = false;
                            }
                            db.SaveChanges();
                        }
                        MessageBox.Show("สำเร็๋จ", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Singleton.SingletonShareMonth.SetInstance();
                        comboBox1_SelectedIndexChanged(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgClick(object sender, DataGridViewCellEventArgs e)
        {
            TbBudgetYear2.Text = "";
            TbDes2.Text = "";
            int index = dataGridView1.CurrentRow.Index;
            if (dataGridView1.Rows[index].Cells["Status"].Value.ToString() == "ใช้งาน")
            {
                int id = int.Parse(dataGridView1.Rows[index].Cells[0].Value.ToString());
                var member = Singleton.SingletonShareMonth.Instance().ShareMonth.SingleOrDefault(w => w.Id == id);
                RowId = member.Id;
                TbBudgetYear2.Text = member.AgeMonth.ToString();
                TbDes2.Text = member.Description;
            }
        }
    }
}
