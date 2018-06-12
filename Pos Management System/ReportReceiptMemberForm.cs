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
    public partial class ReportReceiptMemberForm : Form
    {
        public ReportReceiptMemberForm()
        {
            InitializeComponent();
        }

        private void ReportReceiptMemberForm_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            mrs = new List<Model.MemberReceipt>();
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.MemberChangeMoney25601.Where(w => EntityFunctions.TruncateTime(w.PrintDate) >= EntityFunctions.TruncateTime(dateTimePickerStart.Value) &&
                EntityFunctions.TruncateTime(w.PrintDate) <= EntityFunctions.TruncateTime(dateTimePickerEnd.Value) && w.Enable == true && w.PrintDate != null)
                .OrderBy(w => w.ReceiptNo).ToList();

                decimal คปอง = 0;
                string sumFormem = "";
                int i = 1;
                string cash = "";
                string cupon = "";
                foreach (var item in data)
                {
                    cash = "0";
                    cupon = "0";
                    
                    if (item.GetType == 1) //สด
                    {
                        คปอง = 0;
                        cash = Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + decimal.Parse(item.จำนวนเง_น1) + (decimal)item.ค_างจ_าย1 + (decimal)item.ป_601 + คปอง);
                    }
                    else // 2 คูปอง
                    {
                        คปอง = (decimal)item.ค_ปอง;
                        cupon = Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + decimal.Parse(item.จำนวนเง_น1) + (decimal)item.ค_างจ_าย1 + (decimal)item.ป_601 + คปอง);
                    }
                    //dataGridView1.Rows.Add("เงินปันผลยกมา", Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย)); // 1
                    //dataGridView1.Rows.Add("เงินปันผลปีปัจจุบัน", Library.ConvertDecimalToStringForm((decimal)item.จำนวนเง_น)); // 2
                    //dataGridView1.Rows.Add("เงินปันผลระหว่างปี", Library.ConvertDecimalToStringForm(decimal.Parse(item.จำนวนเง_น1))); // 3
                    //dataGridView1.Rows.Add("ยอดซื้อหักภาษี", Library.ConvertDecimalToStringForm((decimal)item.ยอดซ__อห_ก_7_));// 4
                    //dataGridView1.Rows.Add("เงินเฉลี่ยคืนยกมา", Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย1)); // 5
                    //dataGridView1.Rows.Add("เงินเฉลี่ยคืนปีปัจจุบัน", Library.ConvertDecimalToStringForm((decimal)item.ป_601)); // 6
                    //dataGridView1.Rows.Add("ส่วนเพิ่มคูปอง", Library.ConvertDecimalToStringForm((decimal)item.ค_ปอง)); // 7
                    dataGridView1.Rows.Add
                        (
                        item.หมายเลข,
                        item.ช__อ_สก_ล,
                        item.ReceiptNo,
                        Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + decimal.Parse(item.จำนวนเง_น1)),
                    Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย1 + (decimal)item.ป_601),
                    Library.ConvertDecimalToStringForm(คปอง),
                    Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + decimal.Parse(item.จำนวนเง_น1) + (decimal)item.ค_างจ_าย1 + (decimal)item.ป_601 + คปอง));

                    string date = Library.ConvertDateToThaiDate(item.PrintDate);
                    mrs.Add(new Model.MemberReceipt()
                    {
                        PrintDate = date,
                        Number = i + "",
                        No = item.หมายเลข,
                        Name = item.ช__อ_สก_ล,
                        ReceiptNo = item.ReceiptNo,
                        Money = Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + decimal.Parse(item.จำนวนเง_น1)),
                        MoneyAvg = Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย1 + (decimal)item.ป_601),
                        Cupon = Library.ConvertDecimalToStringForm(คปอง),
                        Total = Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + decimal.Parse(item.จำนวนเง_น1) + (decimal)item.ค_างจ_าย1 + (decimal)item.ป_601 + คปอง),
                        PageNumber = 0,
                        MoneyCash = cash,
                        MoneyCupon = cupon,
                    });
                    i++;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        List<Model.MemberReceipt> mrs = new List<Model.MemberReceipt>();
        private void button2_Click(object sender, EventArgs e)
        {
            var dt = Library.ConvertToDataTable(mrs);
            frmMainReport mr = new frmMainReport(this, dt, MyConstant.TypeReport.ReportMemberChange);
            mr.Show();
        }
    }
}
