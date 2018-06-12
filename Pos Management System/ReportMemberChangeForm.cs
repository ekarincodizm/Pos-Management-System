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
    public partial class ReportMemberChangeForm : Form
    {
        public ReportMemberChangeForm()
        {
            InitializeComponent();
        }

        private void ReportMemberChangeForm_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        List<Model.MemberReceipt> mrs = new List<Model.MemberReceipt>();
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
                decimal sumFormem = 0;
                int i = 1;
                string cash = "0";
                string cupon = "0";
                string state = "รับแล้ว";
                foreach (var item in data)
                {
                    cash = "0";
                    cupon = "0";
                    sumFormem = (decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + (decimal)item.ค_างจ_าย1 + (decimal)item.ป_601 + คปอง;
                    if (item.GetType == 1) //สด
                    {
                        คปอง = 0;
                        cash = Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + decimal.Parse(item.จำนวนเง_น1) + (decimal)item.ค_างจ_าย1 + (decimal)item.ป_601) + คปอง; ;
                    }
                    else // 2 คูปอง
                    {
                        คปอง = (decimal)item.ค_ปอง;
                        cupon = Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + decimal.Parse(item.จำนวนเง_น1) + (decimal)item.ค_างจ_าย1 + (decimal)item.ป_601) + คปอง; ;
                    }
                    dataGridView1.Rows.Add
                        (
                        item.หมายเลข,
                        item.ช__อ_สก_ล,
                        item.ReceiptNo,
                        Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น),
                    Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย1 + (decimal)item.ป_601),
                    Library.ConvertDecimalToStringForm(คปอง),
                    Library.ConvertDecimalToStringForm(sumFormem), state);
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
                        Total = Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + decimal.Parse(item.จำนวนเง_น1) + (decimal)item.ค_างจ_าย1 + (decimal)item.ป_601) + คปอง,
                        PageNumber = 0,
                        MoneyCash = cash,
                        MoneyCupon = cupon,
                    });
                    i++;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            mrs = new List<Model.MemberReceipt>();
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = new List<MemberChangeMoney25601>();
                string state = "";
                if (radioButtonAll.Checked == true)
                {
                    data = db.MemberChangeMoney25601.Where(w => w.Enable == true).OrderBy(w => w.ReceiptNo).ToList();
                }
                else if (radioButtonNot.Checked == true)
                {
                    data = db.MemberChangeMoney25601.Where(w => w.Enable == true && w.PrintDate == null).OrderBy(w => w.ReceiptNo).ToList();
                }

                decimal คปอง = 0;
                decimal sumFormem = 0;
                int i = 1;
                string cash = "";
                string cupon = "";
                foreach (var item in data)
                {
                    cash = "0";
                    cupon = "0";
                    //sumFormem = (decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + (decimal)item.ค_างจ_าย1 + (decimal)item.ป_601 + คปอง;
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
                    sumFormem = (decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + (decimal)item.ค_างจ_าย1 + (decimal)item.ป_601 + คปอง;
                    if (item.PrintDate == null)
                    {
                        state = "ยังไม่รับ";
                    }
                    else
                    {
                        state = "รับแล้ว";
                    }
                    dataGridView1.Rows.Add
                        (
                        item.หมายเลข,
                        item.ช__อ_สก_ล,
                        item.ReceiptNo,
                        Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + decimal.Parse(item.จำนวนเง_น1)),
                    Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย1 + (decimal)item.ป_601),
                    Library.ConvertDecimalToStringForm(คปอง),
                    Library.ConvertDecimalToStringForm((decimal)item.ค_างจ_าย + (decimal)item.จำนวนเง_น + decimal.Parse(item.จำนวนเง_น1) + (decimal)item.ค_างจ_าย1 + (decimal)item.ป_601 + คปอง), 
                    state);
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

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //string dateS = Library.ConvertDateToThaiDate(dateTimePickerStart.Value);
            //string dateE = Library.ConvertDateToThaiDate(dateTimePickerEnd.Value);
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานสมาชิกปันผล - เฉลี่ยคืน", "");
        }
    }
}
