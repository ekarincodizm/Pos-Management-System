using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class MemberChange2560Form : Form
    {
        public MemberChange2560Form()
        {
            InitializeComponent();
        }

        private void MemberChange2560Form_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (SSLsEntities db = new SSLsEntities())
            {
                List<MemberChangeMoney25601> data = new List<MemberChangeMoney25601>();
                if (checkBox1.Checked == true && checkBox2.Checked == false) // เฉพาะ พิมพ์แล้ว
                {
                    data = db.MemberChangeMoney25601.Where(w => w.Enable == true && w.PrintDate != null).OrderBy(w => w.หมายเลข).ToList();
                }
                else if (checkBox1.Checked == false && checkBox2.Checked == true)
                {
                    data = db.MemberChangeMoney25601.Where(w => w.Enable == true && w.PrintDate == null).OrderBy(w => w.หมายเลข).ToList();
                }
                else
                {
                    data = db.MemberChangeMoney25601.Where(w => w.Enable == true).OrderBy(w => w.หมายเลข).ToList();
                }

                foreach (var item in data)
                {
                    dataGridView1.Rows.Add
                        (
                        item.หมายเลข,
                        item.ช__อ_สก_ล,
                        Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย),
                        Library.ConvertDecimalToStringForm((decimal)item.ป_60),
                       Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย1),
                       Library.ConvertDecimalToStringForm((decimal)item.ยอดซ__อห_ก_7_),
                       Library.ConvertDecimalToStringForm((decimal)item.ป_601),
                       Library.ConvertDecimalToStringForm((decimal)item.เฉล__ยค_นป_60),
                       Library.ConvertDecimalToStringForm((decimal)item.ค_ปอง),
                       Library.ConvertDecimalToStringForm((decimal)item.เฉล__ยค_นค_ปองป_60),
                       Library.ConvertDateToThaiDate(item.PrintDate, true)
                        );
                }
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานปันผลสมาชิก", "");
        }
        /// <summary>
        /// ค้นหารายสมาชิก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            string memno = textBoxMemno.Text.Trim();
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.MemberChangeMoney25601.Where(w => w.หมายเลข  == memno);
                foreach (var item in data)
                {
                    dataGridView1.Rows.Add
                        (
                        item.หมายเลข,
                        item.ช__อ_สก_ล,
                        Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย),
                        Library.ConvertDecimalToStringForm((decimal)item.ป_60),
                       Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย1),
                       Library.ConvertDecimalToStringForm((decimal)item.ยอดซ__อห_ก_7_),
                       Library.ConvertDecimalToStringForm((decimal)item.ป_601),
                       Library.ConvertDecimalToStringForm((decimal)item.เฉล__ยค_นป_60),
                       Library.ConvertDecimalToStringForm((decimal)item.ค_ปอง),
                       Library.ConvertDecimalToStringForm((decimal)item.เฉล__ยค_นค_ปองป_60),
                       Library.ConvertDateToThaiDate(item.PrintDate, true)
                        );
                }
            }
        }
        /// <summary>
        /// เลือกสมาชิก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            string memno = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
            MemberChange2560Dialog obj = new MemberChange2560Dialog(this, memno);
            obj.ShowDialog();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string memno = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
            MemberChange2560Dialog obj = new MemberChange2560Dialog(this, memno);
            obj.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.MemberChangeMoney25601.Where(w => EntityFunctions.TruncateTime(w.PrintDate) >= EntityFunctions.TruncateTime(dateTimePickerStart.Value) &&
                EntityFunctions.TruncateTime(w.PrintDate) <= EntityFunctions.TruncateTime(dateTimePickerEnd.Value)).ToList();
               
                foreach (var item in data)
                {
                    dataGridView1.Rows.Add
                        (
                        item.หมายเลข,
                        item.ช__อ_สก_ล,
                        Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย),
                        Library.ConvertDecimalToStringForm((decimal)item.ป_60),
                       Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย1),
                       Library.ConvertDecimalToStringForm((decimal)item.ยอดซ__อห_ก_7_),
                       Library.ConvertDecimalToStringForm((decimal)item.ป_601),
                       Library.ConvertDecimalToStringForm((decimal)item.เฉล__ยค_นป_60),
                       Library.ConvertDecimalToStringForm((decimal)item.ค_ปอง),
                       Library.ConvertDecimalToStringForm((decimal)item.เฉล__ยค_นค_ปองป_60),
                       Library.ConvertDateToThaiDate(item.PrintDate, true)
                        );
                }
            }
        }
    }
}
