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
    public partial class MemberChange2560Dialog : Form
    {
        private MemberChange2560Form memberChange2560Form;
        private string memno;

        public MemberChange2560Dialog()
        {
            InitializeComponent();
        }

        public MemberChange2560Dialog(MemberChange2560Form memberChange2560Form, string memno)
        {
            InitializeComponent();
            this.memberChange2560Form = memberChange2560Form;
            this.memno = memno;
        }

        private void MemberChange2560Dialog_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.MemberChangeMoney25601.FirstOrDefault(w => w.หมายเลข == this.memno);
                nolabel.Text = data.หมายเลข;
                namelabel.Text = data.ช__อ_สก_ล;
                // set grid
                dataGridView1.Rows.Add("เงินปันผลยกมา", Library.ConvertDecimalToStringForm((decimal)data.ค_างจ_าย)); // 1
                dataGridView1.Rows.Add("เงินปันผลปีปัจจุบัน", Library.ConvertDecimalToStringForm((decimal)data.จำนวนเง_น)); // 2
                dataGridView1.Rows.Add("เงินปันผลระหว่างปี", Library.ConvertDecimalToStringForm(decimal.Parse(data.จำนวนเง_น1))); // 3
                dataGridView1.Rows.Add("ยอดซื้อหักภาษี", Library.ConvertDecimalToStringForm((decimal)data.ยอดซ__อห_ก_7_));// 4
                dataGridView1.Rows.Add("เงินเฉลี่ยคืนยกมา", Library.ConvertDecimalToStringForm((decimal)data.ค_างจ_าย1)); // 5
                dataGridView1.Rows.Add("เงินเฉลี่ยคืนปีปัจจุบัน", Library.ConvertDecimalToStringForm((decimal)data.ป_601)); // 6
                dataGridView1.Rows.Add("ส่วนเพิ่มคูปอง", Library.ConvertDecimalToStringForm((decimal)data.ค_ปอง)); // 7
                label7.Text = Library.ConvertDateToThaiDate(DateTime.Now);

                //decimal sum = (decimal)dataGridView1.Rows[0].Cells[1].Value; // 1
                //sum = sum + (decimal)dataGridView1.Rows[1].Cells[1].Value; // 2
                //sum = sum + (decimal)dataGridView1.Rows[2].Cells[1].Value; //3
                ////sum = sum + (decimal)dataGridView1.Rows[3].Cells[1].Value; //4
                //sum = sum + (decimal)dataGridView1.Rows[4].Cells[1].Value; //5
                //sum = sum + (decimal)dataGridView1.Rows[5].Cells[1].Value; //6

                decimal sum = (decimal)data.ค_างจ_าย + // 1
                    (decimal)data.จำนวนเง_น + // 2
                    decimal.Parse(data.จำนวนเง_น1) + // 3 
                    (decimal)data.ค_างจ_าย1 + // 5
                    (decimal)data.ป_601; //6
                label10.Text = Library.ConvertDecimalToStringForm(sum);
                if (data.ReceiptNo != null)
                {
                    invlabel.Text = data.ReceiptNo;
                }
                else
                {
                    invlabel.Text = "-";
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

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                // คูปอง+
                decimal sum = decimal.Parse(dataGridView1.Rows[0].Cells[1].Value.ToString()); // 1
                sum = sum + decimal.Parse(dataGridView1.Rows[1].Cells[1].Value.ToString()); // 2
                sum = sum + decimal.Parse(dataGridView1.Rows[2].Cells[1].Value.ToString()); //3
                //sum = sum + (decimal)dataGridView1.Rows[3].Cells[1].Value; //4
                sum = sum + decimal.Parse(dataGridView1.Rows[4].Cells[1].Value.ToString()); //5
                sum = sum + decimal.Parse(dataGridView1.Rows[5].Cells[1].Value.ToString()); //6
                sum = sum + decimal.Parse(dataGridView1.Rows[6].Cells[1].Value.ToString()); //7
                label11.Text = Library.ConvertDecimalToStringForm(sum);
                label10.Text = "0.00";
            }
            else
            {
                // เงินสด
                decimal sum = decimal.Parse(dataGridView1.Rows[0].Cells[1].Value.ToString()); // 1
                sum = sum + decimal.Parse(dataGridView1.Rows[1].Cells[1].Value.ToString()); // 2
                sum = sum + decimal.Parse(dataGridView1.Rows[2].Cells[1].Value.ToString()); //3
                //sum = sum + (decimal)dataGridView1.Rows[3].Cells[1].Value; //4
                sum = sum + decimal.Parse(dataGridView1.Rows[4].Cells[1].Value.ToString()); //5
                sum = sum + decimal.Parse(dataGridView1.Rows[5].Cells[1].Value.ToString()); //6
                label11.Text = "0.00";
                label10.Text = Library.ConvertDecimalToStringForm(sum);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        // พิมพ์ใบเสร็จ yymmdd000001
        private void button2_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                string memberNo = nolabel.Text;
                var getMem = db.MemberChangeMoney25601.SingleOrDefault(w => w.หมายเลข == memberNo);
                decimal upcupon = 0;
                if (invlabel.Text == "-")// ยังไม่พิมพ์
                {
                    string userId = Singleton.SingletonAuthen.Instance().Id;
                    int qtyReceipt = db.MemberChangeMoney25601.Where(w => w.PrintDate != null && w.PrintBy == userId).Count();
                    string running = (qtyReceipt + 1).ToString("D5");
                    string receiptno = "61";

                    getMem.PrintDate = DateTime.Now;
                    getMem.PrintBy = Singleton.SingletonAuthen.Instance().Id;
                    getMem.PrintNumber = 1;
                    //ปี เช่น 61
                    //ประเภท ใบเสร็จ สด = 1 คูปอง = 2
                    //เดือน เช่น 03
                    //วัน เช่น 29
                    //รหัสพนักงาน เช่น 112
                    //ลำดับเฉพาะพนักงาน 0001

                    //ตัวอย่าง จ่ายสด 61 1 03 29 112 00001
                    //ตัวอย่าง จ่ายคูปอง 61 2 03 29 11 2 00002

                    if (radioButton1.Checked == true)
                    {
                        // สด
                        getMem.GetType = 1;
                        getMem.GetAmount = decimal.Parse(label10.Text);
                        upcupon = 0;
                        receiptno = receiptno + "1";
                    }
                    else
                    {
                        // คูปอง
                        getMem.GetType = 2;
                        getMem.GetAmount = decimal.Parse(label11.Text);
                        upcupon = decimal.Parse(dataGridView1.Rows[6].Cells[1].Value.ToString());
                        receiptno = receiptno + "2";
                    }

                    receiptno = receiptno + "" + DateTime.Now.ToString("MMdd") + userId + running;
                    getMem.ReceiptNo = receiptno;
                }
                else
                {
                    DialogResult dr = MessageBox.Show("ใบเสร็จนี้พิมพ์ไปแล้ว คุณต้องการพิมพ์ซ้ำ ใช่หรือไม่ ?",
                        "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:

                            break;
                        case DialogResult.No:
                            return;
                    }
                    getMem.PrintNumber = getMem.PrintNumber + 1;
                }
                db.Entry(getMem).State = EntityState.Modified;
                db.SaveChanges();

                // print 
                MemberChange2560 receiptPaper = new MemberChange2560();
                receiptPaper.No = getMem.ReceiptNo;
                receiptPaper.NameMember = getMem.ช__อ_สก_ล;
                receiptPaper.NoMemeber = getMem.หมายเลข;
                receiptPaper.Date = Library.ConvertDateToThaiDate(DateTime.Now, true);
                receiptPaper.User = Library.GetFullNameUserById(getMem.PrintBy);
                receiptPaper.PrintDate = Library.ConvertDateToThaiDate(getMem.PrintDate, true);
                receiptPaper.UserPrint = Library.GetFullNameUserById(Singleton.SingletonAuthen.Instance().Id);

                receiptPaper.BroughtForward = Library.ConvertDecimalToStringForm(decimal.Parse(dataGridView1.Rows[0].Cells[1].Value.ToString()));
                receiptPaper.Current = Library.ConvertDecimalToStringForm(decimal.Parse(dataGridView1.Rows[1].Cells[1].Value.ToString()));
                receiptPaper.BetweenYear = Library.ConvertDecimalToStringForm(decimal.Parse(dataGridView1.Rows[2].Cells[1].Value.ToString()));
                receiptPaper.AverageBroughtForward = Library.ConvertDecimalToStringForm(decimal.Parse(dataGridView1.Rows[3].Cells[1].Value.ToString()));
                receiptPaper.AverageCurrent = Library.ConvertDecimalToStringForm(decimal.Parse(dataGridView1.Rows[4].Cells[1].Value.ToString()));
                receiptPaper.AverageCoupon = Library.ConvertDecimalToStringForm(decimal.Parse(dataGridView1.Rows[5].Cells[1].Value.ToString()));
                receiptPaper.UpCoupon = Library.ConvertDecimalToStringForm(upcupon);
                if (getMem.GetType == 1)
                {
                    receiptPaper.NameAverageCoupon = "เฉลี่ยคืนเป็น เงินสด";
                    receiptPaper.RecipientCoupon = "ผู้รับเงินสด";
                }
                else
                {
                    receiptPaper.NameAverageCoupon = "เฉลี่ยคืนเป็น คูปอง";
                    receiptPaper.RecipientCoupon = "ผู้รับคูปอง";
                }
                receiptPaper.TotalAmount = Library.ConvertDecimalToStringForm(getMem.GetAmount);

                List<MemberChange2560> dividends = new List<MemberChange2560>();
                dividends.Add(receiptPaper);
                frmMainReport mr = new frmMainReport(this, dividends);
                mr.Show();
            }
        }
        public class MemberChange2560
        {
            public string No { get; set; }
            public string NameMember { get; set; }
            public string NoMemeber { get; set; }
            public string Date { get; set; }
            public string User { get; set; }
            public string PrintDate { get; set; }
            public string UserPrint { get; set; }

            public string BroughtForward { get; set; }
            public string Current { get; set; }
            public string BetweenYear { get; set; }
            public string AverageBroughtForward { get; set; }
            public string AverageCurrent { get; set; }
            public string AverageCoupon { get; set; }
            public string UpCoupon { get; set; }

            public string NameAverageCoupon { get; set; }
            public string RecipientCoupon { get; set; }
            public string TotalAmount { get; set; }
        }
    }
}
