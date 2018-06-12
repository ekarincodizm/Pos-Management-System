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
    public partial class MemberActiveAndResignReport : Form
    {

        public MemberActiveAndResignReport()
        {
            InitializeComponent();
        }

        private void MemberActiveAndResignReport_Load(object sender, EventArgs e)
        {
            CbDateNow.Checked = false;
            CbDateNow_CheckedChanged(sender, e);
            checkBoxYear.Checked = true;
            //var Bg = Singleton.SingletonBudgetYearNew.Instance().BudgetYear.Where(w => w.Enable == true).Select(s => new { ThaiYear = s.ThaiYear, IsCurrent = s.IsCurrent }).OrderByDescending(o => o.IsCurrent).ToList();
            //if (Bg == null)
            //{
            //    MessageBox.Show("Not have BudgetYear");
            //    return;
            //}
            //DataTable dtbg = Library.ConvertToDataTable(Bg);
            //textBoxYear.DataSource = dtbg;
            //textBoxYear.DisplayMember = dtbg.Columns["ThaiYear"].ToString();
            //textBoxYear.ValueMember = dtbg.Columns["IsCurrent"].ToString();
            //textBoxYear.SelectedIndex = 0;
            //comboBox1.DataSource = dtbg;
            //comboBox1.DisplayMember = dtbg.Columns["ThaiYear"].ToString();
            //comboBox1.ValueMember = dtbg.Columns["IsCurrent"].ToString();
            //comboBox1.SelectedIndex = 0;
            LoadMember();
        }
        /// <summary>
        /// ค้นหา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (CbDateNow.Checked == true)
            {
                var Bg = Singleton.SingletonBudgetYearNew.Instance().BudgetYear.Where(w => w.Enable == true & w.StartDate <= DtDateNow.Value & w.EndDate >= DtDateNow.Value).FirstOrDefault();
                if (Bg == null)
                {
                    MessageBox.Show("ไม่มีปีงบประมาณ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {

                    foreach (DataRowView item in comboBox1.Items)
                    {
                        if (item.Row[0].ToString() == Bg.ThaiYear.ToString())
                        {
                            //MessageBox.Show(item.Row[0].ToString());
                            int a = comboBox1.Items.IndexOf(item); ;
                            comboBox1.SelectedIndex = a;
                            textBoxYear.SelectedIndex = a;
                        }
                      
                    }

                    //foreach (object item  in comboBox1.i)
                    //{
                    //    if () == Bg.ThaiYear.ToString())
                    //    {
                    //        comboBox1.SelectedIndex = comboBox1.Items.IndexOf(item);
                    //    }
                    //}
                }

            }

            LoadMember();
        }


        public class MemberActive
        {
            public int ID { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Sex { get; set; }
            public decimal d_getFromBeforeBudget { get; set; }
            public decimal d_getAddThisYear { get; set; }
            public decimal d_removeValue { get; set; }
            public decimal d_MemberShare { get; set; }
            public string getFromBeforeBudget { get; set; }
            public string getAddThisYear { get; set; }
            public string removeValue { get; set; }
            public string removeDate { get; set; }
            public string MemberShare { get; set; }
            public int TotalList { get; set; }
            public int Group { get; set; }
            public int Number { get; set; }
            public int sumMale { get; set; }
            public int sumFemale { get; set; }
        }
        List<MemberActive> lsReport = new List<MemberActive>();
        int TotalList;
        private void LoadMember()
        {
            if (lsReport.Count() > 0)
            {
                lsReport = new List<MemberActive>();
            }
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            decimal totalMale = 0;
            decimal totalFemale = 0;
            decimal totalFromBeforeBudget = 0;
            decimal totalAddThisBudget = 0;
            decimal totalResign = 0;
            decimal totalBalance = 0;
            List<Member> members = new List<Member>();
            List<Member> mb = new List<Member>();
            if (textBoxStartNo.Text.Trim() == "")
            {
                members = Singleton.SingletonMember.Instance().Members.Where(w => w.Enable == true).ToList();
            }
            else
            {
                int sNo = int.Parse(textBoxStartNo.Text.Trim());
                int eNo = int.Parse(textBoxEndNo.Text.Trim());

                members = Singleton.SingletonMember.Instance().Members.Where(w => w.Enable == true && int.Parse(w.Code) >= sNo && int.Parse(w.Code) <= eNo).ToList();
            }
            int rows = 46;
            int chk = 1;
            int Male = 0;
            int Female = 0;
            int Number = 1;
            int countSum = 0;


            var Bg = Singleton.SingletonBudgetYearNew.Instance().BudgetYear.Where(w => w.Enable == true & w.ThaiYear == textBoxYear.Text.Trim()).Select(s => new { ThaiYear = s.ThaiYear, IsCurrent = s.IsCurrent, s.StartDate, s.EndDate }).OrderByDescending(o => o.IsCurrent).FirstOrDefault();
            var YearCheckS = Bg.StartDate;
            var YearCheckE = Bg.EndDate;


            if (checkBoxActive.Checked == false && checkBoxResign.Checked == true)
            {
                mb = members.Where(w => w.IsRemoveShared == true & (checkBoxYear.Checked != true ? w.Enable == true : (w.ResignDate == null ? YearCheckS : w.ResignDate.Value) >= YearCheckS & (w.ResignDate == null ? YearCheckE : w.ResignDate.Value) <= YearCheckE) & (CbDateNow.Checked == true ? w.CreateDate <= DtDateNow.Value & (w.ResignDate == null ? DtDateNow.Value : w.ResignDate.Value) <= DtDateNow.Value : w.Enable == true)).ToList();
            }
            else if (checkBoxResign.Checked == false && checkBoxActive.Checked == true)
            {
                mb = members.Where(w => w.IsRemoveShared == false & (CbDateNow.Checked == true ? w.CreateDate <= DtDateNow.Value & (w.ResignDate == null ? DtDateNow.Value : w.ResignDate.Value) <= DtDateNow.Value : w.Enable == true)).ToList();
            }
            else if (checkBoxActive.Checked == true && checkBoxResign.Checked == true)
            {
                mb = members.Where(w => w.Enable == true & (checkBoxYear.Checked != true ? w.Enable == true : (w.ResignDate == null ? YearCheckS : w.ResignDate.Value) >= YearCheckS & (w.ResignDate == null ? YearCheckE : w.ResignDate.Value) <= YearCheckE) & (CbDateNow.Checked == true ? w.CreateDate <= DtDateNow.Value & (w.ResignDate == null ? DtDateNow.Value : w.ResignDate.Value) <= DtDateNow.Value : w.Enable == true)).ToList();
            }
            else if (checkBoxActive.Checked == false && checkBoxResign.Checked == false)
            {
                mb = new List<Member>();
            }
            TotalList = mb.Count();

            decimal sumPageBeforeBudget = 0;
            decimal sumPageThisYear = 0;
            decimal sumPageRemove = 0;
            decimal sumPageTotal = 0;

            foreach (var item in mb.OrderBy(w => w.Code).ToList())
            {
                if (item.FKSex == MyConstant.Sex.Female)
                {
                    totalFemale += 1;
                }
                else
                {
                    totalMale += 1;
                }
                decimal getFromBeforeBudget = GetTotalFromBeforeBudget(item);
                decimal getAddThisYear = GetTotalFromBeforeBudget(item, "onThisYear");

                totalFromBeforeBudget += getFromBeforeBudget;
                totalAddThisBudget += getAddThisYear;

                string removeDate = "";
                decimal removeValue = 0;
                if (item.IsRemoveShared == true)
                {
                    removeDate = Library.ConvertDateToThaiDate(item.ResignDate);
                    totalResign += item.MemberShare.Where(w => w.Enable == true).Sum(w => w.Qty) * MyConstant.SharedValue._100;
                    removeValue = item.MemberShare.Where(w => w.Enable == true).Sum(w => w.Qty) * MyConstant.SharedValue._100;
                }
                totalBalance += getFromBeforeBudget + getAddThisYear - removeValue;
                dataGridView1.Rows.Add
                    (
                    item.Id,
                    item.Code,
                    item.Name,
                    item.Sex.Name,
                    Library.ConvertDecimalToStringForm(getFromBeforeBudget), // ยกมาก
                    Library.ConvertDecimalToStringForm(getAddThisYear), // เพิ่มหุ้น
                    Library.ConvertDecimalToStringForm(removeValue), // ลาออก
                    removeDate, // วันที่ลาออก
                    Library.ConvertDecimalToStringForm((item.MemberShare.Where(w => w.Enable == true).Sum(w => w.Qty) * MyConstant.SharedValue._100 - removeValue))
                    );

                if (chk == 45)
                {
                    if (item.FKSex == MyConstant.Sex.Female)
                    {
                        Female += 1;
                    }
                    else
                    {
                        Male += 1;
                    }
                    sumPageBeforeBudget += getFromBeforeBudget;
                    sumPageThisYear += getAddThisYear;
                    sumPageRemove += removeValue;
                    sumPageTotal += (item.MemberShare.Where(w => w.Enable == true).Sum(w => w.Qty) * MyConstant.SharedValue._100 - removeValue);
                    lsReport.Add(new MemberActive
                    {
                        Number = Number,
                        Code = item.Code,
                        Name = item.Name,
                        Sex = item.Sex.Name,
                        d_getFromBeforeBudget = getFromBeforeBudget,
                        getFromBeforeBudget = Library.ConvertDecimalToStringForm(getFromBeforeBudget),
                        d_getAddThisYear = getAddThisYear,
                        getAddThisYear = Library.ConvertDecimalToStringForm(getAddThisYear),
                        d_removeValue = removeValue,
                        removeValue = Library.ConvertDecimalToStringForm(removeValue),
                        removeDate = removeDate,
                        d_MemberShare = (item.MemberShare.Where(w => w.Enable == true).Sum(w => w.Qty) * MyConstant.SharedValue._100 - removeValue),
                        MemberShare = Library.ConvertDecimalToStringForm((item.MemberShare.Where(w => w.Enable == true).Sum(w => w.Qty) * MyConstant.SharedValue._100 - removeValue))
                    });
                    Number++;
                    chk++;
                    // add summary
                    lsReport.Add(new MemberActive
                    {
                        Code = "",
                        Name = "รวม " + "ชาย " + Male + " คน " + "หญิง " + Female + " คน ",
                        Sex = "",
                        getFromBeforeBudget = Library.ConvertDecimalToStringForm(sumPageBeforeBudget),
                        getAddThisYear = Library.ConvertDecimalToStringForm(sumPageThisYear),
                        removeValue = Library.ConvertDecimalToStringForm(sumPageRemove),
                        removeDate = "",
                        MemberShare = Library.ConvertDecimalToStringForm(sumPageTotal)
                    });
                    Female = 0;
                    Male = 0;
                    sumPageBeforeBudget = 0;
                    sumPageThisYear = 0;
                    sumPageRemove = 0;
                    sumPageTotal = 0;
                    chk = 0;
                    Console.WriteLine(chk + " " + Number);
                }
                else
                {
                    if (item.FKSex == MyConstant.Sex.Female)
                    {
                        Female += 1;
                    }
                    else
                    {
                        Male += 1;
                    }
                    sumPageBeforeBudget += getFromBeforeBudget;
                    sumPageThisYear += getAddThisYear;
                    sumPageRemove += removeValue;
                    sumPageTotal += (item.MemberShare.Where(w => w.Enable == true).Sum(w => w.Qty) * MyConstant.SharedValue._100 - removeValue);
                    lsReport.Add(new MemberActive
                    {
                        Number = Number,
                        Code = item.Code,
                        Name = item.Name,
                        Sex = item.Sex.Name,
                        d_getFromBeforeBudget = getFromBeforeBudget,
                        getFromBeforeBudget = Library.ConvertDecimalToStringForm(getFromBeforeBudget),
                        d_getAddThisYear = getAddThisYear,
                        getAddThisYear = Library.ConvertDecimalToStringForm(getAddThisYear),
                        d_removeValue = removeValue,
                        removeValue = Library.ConvertDecimalToStringForm(removeValue),
                        removeDate = removeDate,
                        d_MemberShare = (item.MemberShare.Where(w => w.Enable == true).Sum(w => w.Qty) * MyConstant.SharedValue._100 - removeValue),
                        MemberShare = Library.ConvertDecimalToStringForm((item.MemberShare.Where(w => w.Enable == true).Sum(w => w.Qty) * MyConstant.SharedValue._100 - removeValue))
                    });
                    Number++;
                    chk++;
                }

                // check is last
                if (Number - 1 == TotalList)
                {
                    Console.WriteLine(TotalList);
                    lsReport.Add(new MemberActive
                    {
                        Code = "",
                        Name = "รวม " + "ชาย " + Male + " คน " + "หญิง " + Female + " คน ",
                        Sex = "",
                        getFromBeforeBudget = Library.ConvertDecimalToStringForm(sumPageBeforeBudget),
                        getAddThisYear = Library.ConvertDecimalToStringForm(sumPageThisYear),
                        removeValue = Library.ConvertDecimalToStringForm(sumPageRemove),
                        removeDate = "",
                        MemberShare = Library.ConvertDecimalToStringForm(sumPageTotal)
                    });
                }
            }
            textBoxTotalFromBefore.Text = Library.ConvertDecimalToStringForm(totalFromBeforeBudget);
            textBoxTotalAddThisYear.Text = Library.ConvertDecimalToStringForm(totalAddThisBudget);
            textBoxTotalResign.Text = Library.ConvertDecimalToStringForm(totalResign);
            textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(totalBalance);

            textBoxMale.Text = Library.ConvertDecimalToStringForm(totalMale, "int");
            textBoxFemale.Text = Library.ConvertDecimalToStringForm(totalFemale, "int");
        }
        decimal GetTotalFromBeforeBudget(Member member)
        {
            //BudgetYear thisBudgetYear = Singleton.SingletonThisBudgetYear.Instance().ThisYear;
            //int startThisYear = int.Parse(thisBudgetYear.StartDate.ToString("yyyyMMdd"));
            //decimal sum = 0;
            //foreach (var item in member.MemberShare.Where(w => w.Enable == true).ToList())
            //{
            //    // 
            //    int thisItem = int.Parse(item.CreateDate.ToString("yyyyMMdd"));
            //    if (thisItem < startThisYear) // ถ้าสมัครเก่ากว่าวันที่เริ่มของ ปีงบปัจจุบัน
            //    {
            //        sum += item.Qty;
            //    }
            //}
            //return sum * MyConstant.SharedValue._100;


            //Edit By Tin Date 14/05/2018

            var Bg = Singleton.SingletonBudgetYearNew.Instance().BudgetYear.Where(w => w.Enable == true & w.ThaiYear == comboBox1.Text.Trim()).Select(s => new { ThaiYear = s.ThaiYear, IsCurrent = s.IsCurrent, s.StartDate, s.EndDate }).OrderByDescending(o => o.IsCurrent).FirstOrDefault();
            decimal sum = 0;
            foreach (var item in member.MemberShare.Where(w => w.Enable == true).ToList())
            {
                if (item.CreateDate < Bg.StartDate)
                {
                    sum += item.Qty;
                }
            }
            return sum * MyConstant.SharedValue._100;
            //End Edit
        }
        /// <summary>
        /// เชคหุ้นที่ เพิ่มในปีนี้ 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        decimal GetTotalFromBeforeBudget(Member member, string thisYear)
        {

            var Bg = Singleton.SingletonBudgetYearNew.Instance().BudgetYear.Where(w => w.Enable == true & w.ThaiYear == comboBox1.Text.Trim()).Select(s => new { ThaiYear = s.ThaiYear, IsCurrent = s.IsCurrent, s.StartDate, s.EndDate }).OrderByDescending(o => o.IsCurrent).FirstOrDefault();
            decimal sum = 0;
            foreach (var item in member.MemberShare.Where(w => w.Enable == true).ToList())
            {
                if (item.CreateDate >= Bg.StartDate & item.CreateDate <= Bg.EndDate)
                {
                    sum += item.Qty;
                }
            }
            return sum * MyConstant.SharedValue._100;
        }
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void textBoxStartNo_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    textBoxEndNo.Text = textBoxStartNo.Text.Trim();
                    break;
                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานสมาชิกคงอยู่-ลาออก", "");
        }
        /// <summary>
        /// ออกรายงานเป็น Paper
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                var dt = Library.ConvertToDataTable(lsReport.
                    Select(s => new
                    {
                        TotalMale = textBoxMale.Text == "" || textBoxMale.Text == null ? "0" : textBoxMale.Text,
                        TotalFemale = textBoxFemale.Text == "" || textBoxFemale.Text == null ? "0" : textBoxFemale.Text,
                        TotalgetFromBeforeBudget = textBoxTotalFromBefore.Text == "" || textBoxTotalFromBefore.Text == null ? "0" : textBoxTotalFromBefore.Text,
                        TotalgetAddThisYear = textBoxTotalAddThisYear.Text == "" || textBoxTotalAddThisYear.Text == null ? "0" : textBoxTotalAddThisYear.Text,
                        TotalMemberShare = textBoxTotalBalance.Text == "" || textBoxTotalBalance.Text == null ? "0" : textBoxTotalBalance.Text,
                        TotalremoveValue = textBoxTotalResign.Text == "" || textBoxTotalResign.Text == null ? "0" : textBoxTotalResign.Text,
                        Number = s.Number,
                        Group = s.Group,
                        Code = s.Code,
                        Name = s.Name,
                        Sex = s.Sex,
                        getFromBeforeBudget = s.getFromBeforeBudget,
                        getAddThisYear = s.getAddThisYear,
                        removeValue = s.removeValue,
                        removeDate = s.removeDate,
                        MemberShare = s.MemberShare,
                        TotalList = TotalList
                    }).ToList());
                frmMainReport mr = new frmMainReport(this, dt);
                mr.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("จำนวนเอกสารผิดพลาด");
            }

            //int row = 45;
            //int number = 1;
            //List<MemberReportPaper> objs = new List<MemberReportPaper>();
            //int countMale = 0;
            //int countFemale = 0;
            //int totalMale = 0;
            //int totalFemale = 0;
            //decimal sumAfterYear = 0;
            //decimal sumAddPoint = 0;
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    objs.Add(new MemberReportPaper()
            //    {
            //        Number = (i + 1) + "",
            //        Code = dataGridView1.Rows[i].Cells[colCode].Value.ToString(),
            //        Name = dataGridView1.Rows[i].Cells[colName].Value.ToString(),
            //        Sex = dataGridView1.Rows[i].Cells[colSex].Value.ToString(),
            //        GetAfterYear = dataGridView1.Rows[i].Cells[colGetAfterYear].Value.ToString(),
            //        AddPoint = dataGridView1.Rows[i].Cells[colAddPoint].Value.ToString(),
            //        Resign = dataGridView1.Rows[i].Cells[colResign].Value.ToString(),
            //        ResignDate = dataGridView1.Rows[i].Cells[colResignDate].Value.ToString(),
            //        Total = dataGridView1.Rows[i].Cells[colTotal].Value.ToString(),
            //    });
            //    if (dataGridView1.Rows[i].Cells[colSex].Value.ToString() == "ชาย")
            //    {
            //        countMale++;
            //    }
            //    else
            //    {
            //        countFemale++;
            //    }
            //    sumAfterYear += decimal.Parse(dataGridView1.Rows[i].Cells[colGetAfterYear].Value.ToString());
            //    sumAddPoint += decimal.Parse(dataGridView1.Rows[i].Cells[colAddPoint].Value.ToString());

            //    if (number == row)
            //    {
            //        totalMale = totalMale + countMale;
            //        totalFemale = totalMale + countFemale;
            //        number = 1;
            //        // add summary
            //        objs.Add(new MemberReportPaper()
            //        {
            //            Number = "----",
            //            Code = "รวม",
            //            Name = "หญิง = " + countFemale + " ชาย = " + countFemale,
            //            Sex = "-",
            //            GetAfterYear = Library.ConvertDecimalToStringForm(sumAfterYear),
            //            AddPoint = Library.ConvertDecimalToStringForm(sumAddPoint),
            //            Resign = dataGridView1.Rows[i].Cells[colResign].Value.ToString(),
            //            ResignDate = "-----------",
            //            Total = dataGridView1.Rows[i].Cells[colTotal].Value.ToString(),
            //        });
            //    }
            //    number++;
            //}
        }
        int colId = 0;
        int colCode = 1;
        int colName = 2;
        int colSex = 3;
        int colGetAfterYear = 4;
        int colAddPoint = 5;
        int colResign = 6;
        int colResignDate = 7;
        int colTotal = 8;
        class MemberReportPaper
        {
            public string Number { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Sex { get; set; }
            public string GetAfterYear { get; set; }
            public string AddPoint { get; set; }
            public string Resign { get; set; }
            public string ResignDate { get; set; }
            public string Total { get; set; }


        }

        private void checkBoxYear_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CbDateNow_CheckedChanged(object sender, EventArgs e)
        {
            if (CbDateNow.Checked == true)
            {
                DtDateNow.Enabled = true;
            }
            else
            {
                DtDateNow.Enabled = false;
                var Bg = Singleton.SingletonBudgetYearNew.Instance().BudgetYear.Where(w => w.Enable == true).Select(s => new { ThaiYear = s.ThaiYear, IsCurrent = s.IsCurrent }).OrderByDescending(o => o.IsCurrent).ToList();
                if (Bg == null)
                {
                    MessageBox.Show("Not have BudgetYear");
                    return;
                }
                DataTable dtbg = Library.ConvertToDataTable(Bg);
                textBoxYear.DataSource = dtbg;
                textBoxYear.DisplayMember = dtbg.Columns["ThaiYear"].ToString();
                textBoxYear.ValueMember = dtbg.Columns["IsCurrent"].ToString();
                textBoxYear.SelectedIndex = 0;
                comboBox1.DataSource = dtbg;
                comboBox1.DisplayMember = dtbg.Columns["ThaiYear"].ToString();
                comboBox1.ValueMember = dtbg.Columns["IsCurrent"].ToString();
                comboBox1.SelectedIndex = 0;
            }
        }
    }
}
