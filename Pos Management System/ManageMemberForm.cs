using Pos_Management_System.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pos_Management_System;

namespace Pos_Management_System
{
    public partial class ManageMemberForm : Form
    {
        Member memberNew;
        int RowId = 0;
        //private DateTime GetYearServer;
        DataTable DtShareMonth, DtBudgetYear, DtBudgetYearEnable;

        public ManageMemberForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// ปุ่มเพิ่มหุ้น 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlus_Click(object sender, EventArgs e)
        {
            DialogAddShred obj = new DialogAddShred(this);
            obj.ShowDialog();
        }
        /// <summary>
        ///  bindding เพิ่มหุ้น
        /// </summary>
        int col2Id = 0;
        // ไว้ใช้ตอน save
        decimal value;
        int fkAgeOfShared;
        // ไว้ใช้ตอน เพิ่มค่า หุ้น
        decimal old = 0;
        public void BinddingAddShared(decimal value)
        {
            DateTime dateEnd = Singleton.SingletonThisBudgetYear.Instance().ThisYear.EndDate;
            textBoxTotalShared.Text = (value + old) + "";
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                int id = int.Parse(dataGridView2.Rows[i].Cells[col2Id].Value.ToString());

                if (id == 0)
                {
                    dataGridView2.Rows.RemoveAt(i);
                    continue;
                }
            }
            // add dataGridView2
            // แปลง วันที่ thai เป็น EN
            string dateShared = maskedTextBoxCreateDate.Text;
            var split = dateShared.Split('/');
            var yyyy = int.Parse(split[2]) - 543;
            DateTime dateshared = new DateTime(yyyy, int.Parse(split[1]), int.Parse(split[0]));
            var ageOfShared = SingletonAgeOfShare.Instance().AgeOfShare.ToList();
            int c = int.Parse(dateshared.ToString("yyyyMMdd"));
            int f = int.Parse(ageOfShared.FirstOrDefault().TermStart.ToString("yyyyMMdd"));
            string sharedAge = "";
            if (c < f)
            {
                // แปล ว่า ครบปี แน่ๆ
                sharedAge = "ครบปี";
                fkAgeOfShared = ageOfShared.FirstOrDefault().Id;
            }
            else
            {
                bool isTerm = false;
                foreach (var item in ageOfShared)
                {
                    if (Library.CheckAgeOfShared(dateshared, item.TermStart, item.TermEnd))
                    {
                        isTerm = true;
                        sharedAge = item.ShareAge + " เดือน";
                        if (item.ShareAge == 12)
                        {
                            sharedAge = "ครบปี";
                        }
                        fkAgeOfShared = item.Id;
                        break;
                    }
                }
            }
            ///////////////////Edit 1
            //DateTime dateshared = DateTime.Parse(localDate.ToString("en-GB"));

            //int yearShared = localDate.Year;

            //var diffMonths = (dateEnd.Month + dateEnd.Year) - (localDate.Month + localDate.Year);
            //DateDiff dateDiff = new DateDiff(date1, date2);
            ///////////////////Edit 2
            //int diffMonths = Library.MonthDiff(dateshared, dateEnd);

            //if (diffMonths >= 12)
            //{
            //    sharedAge = "ครบปี";
            //}
            //else
            //{
            //    sharedAge = diffMonths + " เดือน";
            //}
            //Console.WriteLine(diffMonths + "");

            dataGridView2.Rows.Add
            (0,
            value,
            maskedTextBoxCreateDate.Text,
            Library.ConvertDecimalToStringForm(SingletonShared.Instance().Share.Value * value),
            Singleton.SingletonAuthen.Instance().Name
            //,sharedAge
            );
            this.value = value;
        }
        /// <summary>
        /// Add Member
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
          "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    // แปลว่า add new
                    if (textBoxId.Text == "")
                    {
                        SaveCommit();
                        reload();
                    }
                    else
                    {
                        // edit
                        using (SSLsEntities db = new SSLsEntities())
                        {
                            int id = int.Parse(textBoxId.Text.ToString());
                            Member member = new Member();
                            //member = Singleton.SingletonMember.Instance().Members.SingleOrDefault(w => w.Id == id);
                            member = db.Member.SingleOrDefault(w => w.Id == id);
                            if (radioButtonRemove.Checked == true)
                            {
                                // ถ้าถอนหุ้น วิ่งไปอัพเดท 
                                member.IsRemoveShared = true;
                                member.ResignDate = DateTime.Now;
                                //foreach (var item in member.MemberShare.Where(w => w.Enable == true))
                                //{
                                //    item.UpdateDate = DateTime.Now;
                                //    item.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                                //    item.Enable = false;
                                //    db.Entry(item).State = EntityState.Modified;
                                //}
                            }
                            else
                            {
                                member.Name = textBoxName.Text;
                                member.Code = textBoxCode.Text.Trim();
                                member.CreateDate = (DateTime)Library.ConvertTHToENDate(maskedTextBoxMemberCreate.Text);

                                if (radioButtonFemale.Checked == true)
                                {
                                    member.FKSex = MyConstant.Sex.Female;
                                }
                                else
                                {
                                    member.FKSex = MyConstant.Sex.male;
                                }
                                member.TaxId = textBoxTax.Text;
                                var date = Library.ConvertTHToENDate(textBoxBirthDate.Text);
                                if (date == null)
                                {
                                    MessageBox.Show("วันเกิดไม่ถูกต้อง");
                                    return;
                                }
                                else
                                {
                                    member.BirthDate = date.Value;
                                }
                                member.Tel = textBoxTel.Text;
                                member.Age = decimal.Parse(textBoxAge.Text);
                                member.Address = textBoxAddress.Text;
                                // หุ้นมากกว่า 0 ถึงเพิ่ม
                                if (this.value > 0)
                                {
                                    MemberShare ms = new MemberShare();
                                    ms.Enable = true;
                                    var dateC = Library.ConvertTHToENDate(maskedTextBoxCreateDate.Text);
                                    if (dateC == null)
                                    {
                                        MessageBox.Show("วันที่เพิ่มหุ้นไม่ถูกต้อง");
                                        return;
                                    }
                                    ms.CreateDate = dateC.Value;
                                    ms.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                                    ms.UpdateDate = DateTime.Now;
                                    ms.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                                    ms.FKShare = MyConstant.Shared.General;
                                    ms.FKBudgetYear = Singleton.SingletonThisBudgetYear.Instance().ThisYear.Id;
                                    ms.Qty = value;
                                    ms.FKMember = member.Id;
                                    //ms.FKAgeOfShare = this.fkAgeOfShared;
                                    ms.FKAgeOfShare = CheckAgeOfShared(ms.CreateDate);
                                    db.MemberShare.Add(ms);
                                    //db.SaveChanges();
                                }
                                // check edit share
                                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                                {
                                    int idShared = int.Parse(dataGridView2.Rows[i].Cells[0].Value.ToString());
                                    if (idShared != 0)
                                    {
                                        // หุ้น
                                        decimal shareVal = decimal.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString());
                                        MemberShare shareEdit = member.MemberShare.SingleOrDefault(w => w.Id == idShared);
                                        if (shareVal < 1) // ใส่ 0 เพื่อต้องการ ลบ หุ้นออก *Disable
                                        {
                                            shareEdit.Enable = false;
                                            shareEdit.UpdateDate = DateTime.Now;
                                            shareEdit.UpdateBy = SingletonAuthen.Instance().Id;
                                        }
                                        else if (shareEdit.Qty != shareVal)
                                        {
                                            // แปลว่าแก้ไข จำนวนหุ้น ให้อัพเดท
                                            shareEdit.Qty = shareVal;
                                            shareEdit.UpdateDate = DateTime.Now;
                                            shareEdit.UpdateBy = SingletonAuthen.Instance().Id;
                                        }
                                        db.Entry(shareEdit).State = EntityState.Modified;
                                    }
                                }
                            }
                            //db.Member.Attach(member);
                            member.UpdateDate = DateTime.Now;
                            member.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            db.Entry(member).State = EntityState.Modified;

                            db.SaveChanges();
                            var members = Singleton.SingletonMember.Instance().Members;
                            member = members.SingleOrDefault(w => w.Id == id);
                            members.Remove(member);
                            var lastEditMember = db.Member.Include("Sex").Include("MemberShare.Share").Include("MemberShare.AgeOfShare")
                                .SingleOrDefault(w => w.Id == member.Id);
                            members.Add(lastEditMember);
                            this.value = 0;
                            this.fkAgeOfShared = 0;
                            reload();
                        }
                    }
                    break;
                case DialogResult.No:
                    break;
            }

        }
        /// <summary>
        /// save สร้างใหม่ กรณี ถอนหุ้น Disable เฉพาะ remove sheard
        /// </summary>
        void SaveCommit()
        {
            Member member = new Member();
            member.Code = textBoxCode.Text;
            //if (Singleton.SingletonMember.Instance().Members.FirstOrDefault(w => w.Code == member.Code) != null)
            //{
            //    textBoxCode.SelectAll();
            //    label10.Text = "รหัสสมาชิกซ้ำ";
            //    label10.Visible = true;
            //    return;
            //}
            //label10.Visible = false;
            member.Enable = true;
            member.Name = textBoxName.Text;
            member.Address = textBoxAddress.Text;
            member.CreateDate = DateTime.Now;
            member.CreateBy = Singleton.SingletonAuthen.Instance().Id;
            member.UpdateDate = DateTime.Now;
            member.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
            if (radioButtonFemale.Checked == true) // 
            {
                member.FKSex = MyConstant.Sex.Female;
            }
            else
            {
                member.FKSex = MyConstant.Sex.male;
            }
            member.TaxId = textBoxTax.Text.Replace("-", "");
            var date = Library.ConvertTHToENDate(textBoxBirthDate.Text);
            if (date == null)
            {
                MessageBox.Show("วันเกิดไม่ถูกต้อง");
                return;
            }
            else
            {
                member.BirthDate = date.Value;
            }
            member.Age = decimal.Parse(textBoxAge.Text);

            if (radioButtonRemove.Checked == true)
            {
                member.IsRemoveShared = true;
            }
            else
            {
                member.IsRemoveShared = false;
            }
            member.Tel = textBoxTel.Text;
            /// มากกว่า 0 ถึง เพิ่ม
            if (this.value > 0)
            {
                MemberShare ms = new MemberShare();
                ms.Enable = true;
                var dateC = Library.ConvertTHToENDate(maskedTextBoxCreateDate.Text);
                if (dateC == null)
                {
                    MessageBox.Show("วันที่เพิ่มหุ้นไม่ถูกต้อง");
                    return;
                }
                ms.CreateDate = (DateTime)dateC;
                ms.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                ms.UpdateDate = DateTime.Now;
                ms.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                ms.FKShare = MyConstant.Shared.General;
                ms.FKBudgetYear = Singleton.SingletonThisBudgetYear.Instance().ThisYear.Id;
                ms.Qty = value;
                ms.FKAgeOfShare = CheckAgeOfShared(ms.CreateDate);
                // เชค อายุหุ้น
                CheckAgeOfShared(ms.CreateDate);
                member.MemberShare.Add(ms);
            }
            using (SSLsEntities db = new SSLsEntities())
            {
                db.Member.Add(member);
                db.SaveChanges();
                memberNew = new Member();
                string myName = Singleton.SingletonAuthen.Instance().Id;
                memberNew = db.Member.Include("Sex").Include("MemberShare.Share").Include("MemberShare.AgeOfShare")
                    .OrderByDescending(w => w.CreateDate).FirstOrDefault(w => w.CreateBy == myName);
                label10.ForeColor = Color.Red;
                label10.Text = "* Enter";
                this.value = 0;
            }
        }
        /// <summary>
        /// เชคอายุ หุ้น
        /// </summary>
        /// <param name="createDate"></param>
        private int CheckAgeOfShared(DateTime createDate)
        {
            foreach (var item in SingletonAgeOfShare.Instance().AgeOfShare)
            {
                if (int.Parse(createDate.ToString("yyyyMMdd")) < int.Parse(item.TermStart.ToString("yyyyMMdd")) && int.Parse(createDate.ToString("yyyyMMdd")) < int.Parse(item.TermEnd.ToString("yyyyMMdd")))
                {
                    return item.Id;
                }
                else
                {
                    if (int.Parse(createDate.ToString("yyyyMMdd")) > int.Parse(item.TermStart.ToString("yyyyMMdd")) && int.Parse(createDate.ToString("yyyyMMdd")) <= int.Parse(item.TermEnd.ToString("yyyyMMdd")))
                    {
                        return item.Id;
                    }
                }
            }
            return 2;
        }

        private void ManageMemberForm_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = " dd / MM / yyyy";
            comboBox2.SelectedIndex = 0;
            Singleton.SingletonShared.Instance();
            SingletonAgeOfShare.Instance();
            reload();
            var Bg = Singleton.SingletonBudgetYearNew.Instance().BudgetYear.Where(w => w.Enable == true).Select(s => new { ThaiYear = s.ThaiYear, IsCurrent = s.IsCurrent }).OrderByDescending(o => o.IsCurrent).ToList();
            if (Bg == null)
            {
                MessageBox.Show("Not have BudgetYear");
                return;
            }
            DataTable dtbg = Library.ConvertToDataTable(Bg);
            comboBox1.DataSource = dtbg;
            comboBox1.DisplayMember = dtbg.Columns["ThaiYear"].ToString();
            comboBox1.ValueMember = dtbg.Columns["IsCurrent"].ToString();
            comboBox1.SelectedIndex = 0;


        }
        int col1Id = 0;
        void CheckCurrentRowAndBindding()
        {
            RowId = 0;
            GbEditRemove.Visible = false;
            int index = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[index].Cells[col1Id].Value.ToString());
            var member = Singleton.SingletonMember.Instance().Members.SingleOrDefault(w => w.Id == id);

            // bindding now
            textBoxId.Text = member.Id + "";
            textBoxCode.Text = member.Code;
            textBoxName.Text = member.Name;
            maskedTextBoxMemberCreate.Text = Library.ConvertDateToThaiDate(member.CreateDate);
            if (member.FKSex == MyConstant.Sex.Female)
            {
                radioButtonFemale.Checked = true;
            }
            else
            {
                radioButtonMale.Checked = true;
            }
            /// ส่วนของ ถอนหุ้น
            if (member.IsRemoveShared == true)
            {
                radioButtonRemove.Checked = true;
                dateTimePicker1.Value = member.ResignDate.Value;
                GbEditRemove.Visible = true;
                RowId = member.Id;
            }
            else
            {
                radioButtonNotRemove.Checked = true;
            }

            textBoxTax.Text = member.TaxId;
            textBoxBirthDate.Text = Library.ConvertDateToThaiDate(member.BirthDate);
            textBoxAge.Text = member.Age + "";
            textBoxAddress.Text = member.Address;
            textBoxTotalShared.Text = member.MemberShare.Where(w => w.Enable == true).Sum(w => w.Qty) + "";
            this.old = member.MemberShare.Where(w => w.Enable == true).Sum(w => w.Qty);
            textBoxTel.Text = member.Tel;
            // add dataGridView2
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            DateTime dateEnd = Singleton.SingletonThisBudgetYear.Instance().ThisYear.EndDate;


            //Edit By tin  Calculat new MemberShare  DateEdit 14/05/2018
            dataGridView3.Rows.Clear();
            dataGridView3.Refresh();

            //var ds = (from MemberS in member.MemberShare.Where(w => w.Enable == true)
            //         .Select(s => new
            //         {
            //             Id = s.Id,
            //             Qty = s.Qty,
            //             CreateDate = s.CreateDate,
            //             CreateBy = s.CreateBy,
            //             bath = s.Share.Value * s.Qty
            //         })
            //          join Pdtl in db.ProductDetails.Include("ProductUnit") on new { Pro = dtl.FKProduct, Issueuit = true, en = true } equals new { Pro = Pdtl.FKProduct, Issueuit = Pdtl.IssueUnit == true, en = Pdtl.Enable == true } into ps from p in ps.DefaultIfEmpty()
            var d = comboBox1.Text.Trim();

            using (SSLsEntities db = new SSLsEntities())
            {
                string queryString = ";with GetBudGetY as (select top(1) StartDate,EndDate from pos.BudgetYear where Enable = 1 and ThaiYear = '" + d + "' ) " +
                        "select  Code,Name,sum(Qty)Qty,sum(Bath)Bath,ThaiYear,Status from ( " +
                        "select  t.Code,t.Name,t.CreateDate,t.Qty,t.Bath,b.ThaiYear,case when t.CreateDate < (select StartDate from GetBudGetY) then 'ครบปี' " +
                        "when t.CreateDate >= (select StartDate from GetBudGetY) and t.CreateDate <= (select EndDate from GetBudGetY) then  " +
                        "isnull((select convert(varchar,isnull(AgeMonth,'')) + ' เดือน' " +
                        "from mem.ShareMonth where Enable = 1 and FORMAT(t.CreateDate,'MMdd') >= FORMAT(TermMonthStart,'MMdd') and FORMAT(t.CreateDate,'MMdd') <= FORMAT(TermMonthEnd,'MMdd')),'ไม่คิด') " +
                        "else '0' end Status from ( " +
                        "select m.Code,m.Name,s.CreateDate,Qty,(Qty * e.Value) Bath from mem.Member m left join mem.MemberShare s on m.Id = s.FKMember left join mem.Share e on s.FKShare = e.Id  " +
                        "where m.Enable = 1 and s.Enable = 1 and m.Code = '" + member.MemberShare.Where(w => w.Enable == true).Select(s => s.Member.Code).FirstOrDefault() + "' " +
                        "and s.CreateDate <= (select EndDate from GetBudGetY) " +
                        ")t left join pos.BudgetYear b on t.CreateDate >= b.StartDate and t.CreateDate <= b.EndDate and b.Enable = 1 " +
                        ")tt group by Code,Name,ThaiYear,Status";
                var getdata = db.Database.ExecuteEntities<GetToDataGrid3>(queryString);
                if (getdata.Count() > 0)
                {
                    foreach (var item in getdata)
                    {
                        dataGridView3.Rows.Add((int)item.Qty, item.ThaiYear, item.Bath, item.Status);
                    }
                }

            }







            //End Edit By tin
            foreach (MemberShare item in member.MemberShare.Where(w => w.Enable == true).OrderBy(w => w.CreateDate).ToList())
            {
                //string sharedAge = "";
                string bath = Library.ConvertDecimalToStringForm(item.Share.Value * item.Qty);
                //sharedAge = item.AgeOfShare.ShareAge + " เดือน";
                //if (item.AgeOfShare.ShareAge == 12)
                //{
                //    sharedAge = "ครบปี";
                //}
                dataGridView2.Rows.Add
                (item.Id,
                (int)item.Qty,
                Library.ConvertDateToThaiDate(item.CreateDate),
                bath,
                Library.GetFullNameUserById(item.CreateBy)
                //sharedAge
                );
            }
        }
        void reload()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();


            Singleton.SingletonMember.Instance();
            List<Member> members = new List<Member>();
            members = Singleton.SingletonMember.Instance().Members.Where(w => w.Id > 0 & (comboBox2.SelectedIndex == 1 ? w.IsRemoveShared == false : (comboBox2.SelectedIndex == 2 ? w.IsRemoveShared == true : w.Id > 0))).ToList(); ;
            if (memberNew != null)
            {
                members.Insert(0, memberNew);
                memberNew = null;
            }
            foreach (var item in members.OrderByDescending(w => w.CreateDate).ToList())
            {
                dataGridView1.Rows.Add
                    (
                    item.Id,
                    item.Code,
                    item.Name,
                    item.TaxId,
                    (int)item.MemberShare.Where(w => w.Enable == true && w.Member.IsRemoveShared == false).Sum(w => w.Qty),
                    0,
                    0,
                    0,
                    item.IsRemoveShared
                    );
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Remove"].Value.ToString() == "True")
                {
                    row.DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Red, BackColor = Color.White };
                }
              
            }
            dataGridView1.ClearSelection();
            ClearUI();
        }
        class GetToDataGrid3
        {
            public string Code { get; set; }
            public string ThaiName { get; set; }
            public decimal Qty { get; set; }
            public decimal Bath { get; set; }
            public string ThaiYear { get; set; }
            public string Status { get; set; }
        }
        void reload(string key)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();


            Singleton.SingletonMember.Instance();
            List<Member> members = new List<Member>();
            members = Singleton.SingletonMember.Instance().Members.Where(w=>w.Id > 0 & (comboBox2.SelectedIndex == 1 ? w.IsRemoveShared == false : (comboBox2.SelectedIndex == 2 ? w.IsRemoveShared == true : w.Id > 0) )).ToList();
            if (memberNew != null)
            {
                members.Insert(0, memberNew);
                memberNew = null;
            }
            var fromSearch = new List<Member>();
            if (radioButtonCode.Checked == true)
            {
                fromSearch = members.Where(w => w.Code.Contains(key)).OrderByDescending(w => w.CreateDate).ToList();
            }
            else
            {
                fromSearch = members.Where(w => w.Name.Contains(key)).OrderByDescending(w => w.CreateDate).ToList();
            }
            foreach (var item in fromSearch)
            {
                dataGridView1.Rows.Add
                    (
                    item.Id,
                    item.Code,
                    item.Name,
                    item.TaxId,
                    (int)item.MemberShare.Where(w => w.Enable == true && w.Member.IsRemoveShared == false).Sum(w => w.Qty),
                    0,
                    0,
                    0,
                    item.IsRemoveShared
                    );
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Remove"].Value.ToString() == "True")
                {
                    row.DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Red, BackColor = Color.White };
                }

            }
            dataGridView1.ClearSelection();
            ClearUI();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //string date = textBoxBirthDate.Text.Replace("/", "");
            //string dateStr = "";
            //switch (date.Count())
            //{
            //    case 2:
            //        dateStr = date.Substring(0, 2) + "/";
            //        break;
            //    case 4:
            //        dateStr = date.Substring(3, 2) + "/";
            //        break;
            //    //case 6:
            //    //    dateStr = date.Substring(6, 2) + "/";
            //    //    break;
            //    default:
            //        dateStr = date;
            //        break;
            //}
            //textBoxBirthDate.Text = dateStr;
            //for (int i = 0; i < date.Count(); i++)
            //{
            //    switch (i)
            //    {
            //        case 0:
            //            string dd = date[i];
            //            if (true)
            //            {

            //            }
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }
        /// <summary>
        /// row number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 10);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //int index = dataGridView1.CurrentRow.Index;
            //CheckCurrentRowAndBindding();
        }
        /// <summary>
        /// clear ui for add new 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddNew_Click(object sender, EventArgs e)
        {
            ClearUI();
        }

        private void ClearUI()
        {
            textBoxId.Text = "";
            textBoxCode.Text = "";
            textBoxName.Text = "";
            maskedTextBoxMemberCreate.Text = "";
            maskedTextBoxCreateDate.Text = "";
            // sex
            textBoxTax.Text = "";
            textBoxBirthDate.Text = "";
            textBoxAge.Text = "";
            textBoxAddress.Text = "";
            textBoxTel.Text = "";
            textBoxTotalShared.Text = "0";
            radioButtonNotRemove.Checked = true;
            old = 0;
            // ไว้ใช้ตอน save
            this.value = 0;
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            dataGridView3.Rows.Clear();
            dataGridView3.Refresh();
        }

        /// <summary>
        /// Enter BirthDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxBirthDate_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    //var date = Library.ConvertTHToENDate(textBoxBirthDate.Text);
                    //DateTime date = DateTime.Parse(textBoxBirthDate.Text);    
                    try
                    {
                        var date = textBoxBirthDate.Text;
                        var split = date.Split('/');
                        var yyyy = int.Parse(split[2]);
                        DateTime localDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                        int yearNow = localDate.Year;
                        int yearB = yyyy - 543;
                        int age = yearNow - yearB;
                        textBoxAge.Text = age + "";
                        textBoxTel.Select();
                        textBoxTel.SelectAll();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("วันที่ไม่ถูกต้อง");
                    }
                    break;
                default:
                    break;
            }

        }

        private void textBoxCode_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    var members = Singleton.SingletonMember.Instance().Members;
                    var code = textBoxCode.Text.Trim();
                    if (members.FirstOrDefault(w => w.Code == code) != null)
                    {
                        // แปลว่า ซ้ำ
                        textBoxCode.Select();
                        textBoxCode.SelectAll();
                        label10.ForeColor = Color.Red;
                        label10.Text = "สมาชิกซ้ำ";
                    }
                    else
                    {
                        textBoxName.Select();
                        textBoxName.SelectAll();
                        label10.ForeColor = Color.Green;
                        label10.Text = "ไม่ซ้ำ";
                    }
                    break;
                default:
                    break;
            }
        }

        private void textBoxName_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    textBoxTax.Select();
                    textBoxTax.SelectAll();
                    break;
                default:
                    break;
            }
        }

        private void textBoxTax_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    textBoxBirthDate.Select();
                    textBoxBirthDate.SelectAll();
                    break;
                default:
                    break;
            }
        }

        private void textBoxTel_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    textBoxAddress.Select();
                    textBoxAddress.SelectAll();
                    break;
                default:
                    break;
            }

        }

        private void textBoxAddress_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    maskedTextBoxCreateDate.Select();
                    maskedTextBoxCreateDate.SelectAll();
                    break;
                default:
                    break;
            }
        }

        private void maskedTextBoxCreateDate_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    buttonPlus.Select();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ช่องค้นหา แล้วกด Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    reload(textBoxSearchKey.Text.Trim());
                    break;
                default:
                    break;
            }
        }

        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// ลบสมาชิก ออกจากระบบ Disable ทั้ง HD Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    int index = dataGridView1.CurrentRow.Index;
                    int id = int.Parse(dataGridView1.Rows[index].Cells[col1Id].Value.ToString());
                    var member = Singleton.SingletonMember.Instance().Members.SingleOrDefault(w => w.Id == id);
                    Singleton.SingletonMember.Instance().Members.Remove(member);
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        var data = db.Member.SingleOrDefault(w => w.Id == id);
                        data.UpdateDate = DateTime.Now;
                        data.UpdateBy = SingletonAuthen.Instance().Id;
                        data.Enable = false;
                        db.Entry(data).State = EntityState.Modified;

                        foreach (var item in data.MemberShare.Where(w => w.Enable == true))
                        {
                            item.UpdateDate = DateTime.Now;
                            item.UpdateBy = SingletonAuthen.Instance().Id;
                            item.Enable = false;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    reload();
                    break;
                case DialogResult.No:
                    break;
            }


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            reload();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("ยืนยันทำรายการใช่หรือไม่", "Some Title", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        var data = db.Member.Where(w => w.Id == RowId & w.Enable == true & w.ResignDate != null).FirstOrDefault();
                        if (data != null)
                        {
                            data.UpdateDate = Library.DateTimeServer();
                            data.UpdateBy = SingletonAuthen.Instance().Id;
                            data.ResignDate = dateTimePicker1.Value;
                            db.SaveChanges();
                        }
                    }
                    Singleton.SingletonMember.SetInstance();
                    MessageBox.Show("Complete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CheckCurrentRowAndBindding();
        }

        private void dataGridView3_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
    }
}
