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
    public partial class CheckStockFront1ListForm : Form
    {
        public CheckStockFront1ListForm()
        {
            InitializeComponent();
        }
        List<classCheckStock> ls = new List<classCheckStock>();
        public class classCheckStock
        {
            public string DocDate { get; set; }
            public string DocNo { get; set; }
            public string CreateBy { get; set; }
            public int SaveNumber { get; set; }
            public string check { get; set; }
            public int StoreFrontValueSet { get; set; }
            public decimal TotalQtyUnit { get; set; }
            public decimal TotalCostOnly { get; set; }
            public string Description { get; set; }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            int i = 1;
            var dateS = dateTimePickerStart.Value;
            var dateE = dateTimePickerEnd.Value;
            var check = "";
            List<StoreFrontValueDoc> list = new List<StoreFrontValueDoc>();
            decimal totalBath = 0;
            decimal totalUnit = 0;
            using (SSLsEntities db = new SSLsEntities())
            {
                var selectType = comboBox1.SelectedItem.ToString();
                //var dataList = db.StoreFrontValueDoc.Where(w => w.Enable == true).ToList();

                if (selectType == MyConstant.TypeCheck.NoConfirmCountOne)
                {
                    list = db.StoreFrontValueDoc.Where(w => w.Enable == true && w.ConfirmCheck1Date == null && w.ConfirmCheck2Date == null &&
                    DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                    DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                }
                else if (selectType == MyConstant.TypeCheck.ConfirmCountOne)
                {
                    list = db.StoreFrontValueDoc.Where(w => w.Enable == true && w.ConfirmCheck1Date != null && w.ConfirmCheck2Date == null &&
                    DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                    DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                }
                else if (selectType == MyConstant.TypeCheck.ConfirmCountTwo)
                {
                    list = db.StoreFrontValueDoc.Where(w => w.Enable == true && w.ConfirmCheck1Date != null && w.ConfirmCheck2Date != null &&
                    DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                    DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                }
                else
                {
                    list = db.StoreFrontValueDoc.Where(w => w.Enable == true &&
                    DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                    DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();
                }

                foreach (var item in list.OrderBy(w => w.DocNo).ToList())
                {
                    if (item.ConfirmCheck1Date == null)
                    {
                        check = MyConstant.TypeCheck.NoConfirmCountOne;
                    }
                    if (item.ConfirmCheck1Date != null && item.ConfirmCheck2Date == null)
                    {
                        check = MyConstant.TypeCheck.ConfirmCountOne;
                    }
                    else if (item.ConfirmCheck2Date != null)
                    {
                        check = MyConstant.TypeCheck.ConfirmCountTwo;
                    }

                    classCheckStock cs = new classCheckStock();
                    cs.DocDate = Library.ConvertDateToThaiDate(item.DocDate);
                    cs.DocNo = item.DocNo;
                    cs.CreateBy = Library.GetFullNameUserById(item.CreateBy);
                    cs.SaveNumber = item.SaveNumber;
                    cs.check = check;
                    cs.StoreFrontValueSet = item.StoreFrontValueSet.Where(w => w.Enable == true).ToList().Count();
                    cs.TotalQtyUnit = item.TotalQtyUnit;

                    cs.TotalCostOnly = item.TotalCostOnly;

                    totalBath += item.TotalCostOnly;
                    // หาจำนวนชิ้น
                    totalUnit += item.StoreFrontValueSet.Where(w => w.Enable == true).Sum(w => (w.QtyUnit2 * w.Packsize));
                    //foreach (var dtl in item.StoreFrontValueSet.Where(w => w.Enable == true))
                    //{
                    //    totalUnit += dtl.QtyUnit2 * dtl.Packsize;
                    //}

                    cs.Description = item.Description;
                    ls.Add(cs);
                    string print = "พิมพ์แล้ว";
                    if (item.PrintNumber == 0)
                    {
                        print = "ยังไม่พิมพ์";
                    }
                    dataGridView1.Rows.Add(i,
                        Library.ConvertDateToThaiDate(item.DocDate),
                        item.DocNo,
                        Library.GetFullNameUserById(item.CreateBy),
                        item.SaveNumber,
                        check, // ยืนยันการตรวจนับ
                        item.StoreFrontValueSet.Where(w => w.Enable == true).ToList().Count(),
                       Library.ConvertDecimalToStringForm(item.TotalQtyUnit),
                         Library.ConvertDecimalToStringForm(item.TotalCostOnly),
                        item.Description,
                        print);
                    i++;
                }
                textBoxTotalBath.Text = Library.ConvertDecimalToStringForm(totalBath);
                textBoxTotalUnit.Text = Library.ConvertDecimalToStringForm(totalUnit);
                //dataGridView1.AutoResizeColumns();
                //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        private void CheckStockFront1ListForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = "ทั้งหมด";
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        int NoCN = 2;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                /// รหัสใบออเดอร์
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[NoCN].Value.ToString();
                CheckStockFront1Form mr = new CheckStockFront1Form(this, code);
                mr.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("จำนวนเอกสารผิดพลาด");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                /// รหัสใบออเดอร์
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[NoCN].Value.ToString();
                CheckStockFront1Form mr = new CheckStockFront1Form(this, code);
                mr.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("จำนวนเอกสารผิดพลาด");
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                frmMainReport mr = new frmMainReport(this, ls);
                mr.Show();
                //string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[NoCN].Value.ToString();
                //frmMainReport mr = new frmMainReport(this, code);
                //mr.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("ไม่ถูกต้อง");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานเอกสารตรวจนับ " + Library.ConvertDateToThaiDate(dateTimePickerStart.Value) + " - " + 
                Library.ConvertDateToThaiDate(dateTimePickerEnd.Value), "");
        }
    }
}

