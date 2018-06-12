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
    public partial class SearchEditPromotionForm : Form
    {
        public SearchEditPromotionForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reload();

            RowColor();
        }
        /// <summary>
        /// โหลด datagrid ใหม่ ตามเงื่อนไข
        /// </summary>
        public void Reload()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            DateTime date1 = dateTimePickerStart.Value;
            DateTime date2 = dateTimePickerEnd.Value;
            using (SSLsEntities db = new SSLsEntities())
            {
                List<PriceSchedule> stack = new List<PriceSchedule>();

                var priceSchedule = db.PriceSchedule.Where(w => (DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(date1)) && (DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(date2))).ToList();
                stack.AddRange(priceSchedule);
                foreach (var item in priceSchedule)
                {
                    /// ถ้าเลือก กำลังเล่นรายการ
                    /// ตัดที่กำลังเล่นรายการออก
                    if (checkBoxInPro.Checked == false)
                    {
                        Console.WriteLine(item.Code + " " + int.Parse(DateTime.Now.ToString("yyyyMMdd")) + " " + int.Parse(item.StartDate.ToString("yyyyMMdd")));
                        if ((int.Parse(DateTime.Now.ToString("yyyyMMdd")) >= int.Parse(item.StartDate.ToString("yyyyMMdd"))) && (item.IsStop == false) && (item.Enable == true))
                        {
                            Console.WriteLine(item.Code);
                            stack.Remove(item);
                        }
                    }
                    /// เลือก หมดเขตแล้ว
                    if (checkBoxEndPro.Checked == false)
                    {
                        if (item.IsStop == true && item.Enable == true)
                        {
                            //priceSchedule.RemoveAt(i);
                            stack.Remove(item);
                        }
                    }
                    /// ตัดยังไม่เริ่มออก
                    if (checkBoxNotStart.Checked == false && item.Enable == true)
                    {
                        if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) < int.Parse(item.StartDate.ToString("yyyyMMdd")))
                        {
                            //priceSchedule.RemoveAt(i);
                            stack.Remove(item);
                        }
                    }
                    /// ยกเลิก
                    if (checkBoxCancel.Checked == false)
                    {
                        if (item.Enable == false)
                        {
                            //priceSchedule.RemoveAt(i);
                            stack.Remove(item);
                        }
                    }
                }

                foreach (var item in stack)
                {

                    string status = "-";
                    if (item.Enable == false)
                    {
                        status = "ยกเลิก";
                    }
                    else if (item.IsStop == true)
                    {
                        status = "หมดเขต";
                    }
                    else
                    {
                        if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) >= int.Parse(item.StartDate.ToString("yyyyMMdd")))
                        {
                            status = "กำลังเล่นรายการ";
                        }
                        else
                        {
                            status = "ยังไม่เริ่ม";
                        }
                    }
                    dataGridView1.Rows.Add
                        (
                            item.Id,
                            item.Code,
                            item.Name,
                            item.Campaign.Name,
                            Library.ConvertDateToThaiDate(item.CreateDate, true),
                            item.CreateBy,
                            item.Notice,
                            item.Description,
                         Library.ConvertDateToThaiDate(item.StartDate),
                         Library.ConvertDateToThaiDate(item.EndDate),
                         status
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

        private void RowColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((i % 2) == 0)
                {
                    // คู่
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    // คี่
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.AliceBlue;
                }
            }
        }
        /// <summary>
        /// แก้ไขแคมเปญ แก้โดยการเพิ่ม ลบ สินค้าออก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            int rowCurrent = dataGridView1.CurrentRow.Index;
            /// id = PriceSchedule
            int id = int.Parse(dataGridView1.Rows[rowCurrent].Cells[0].Value.ToString());
            /// check ประเภทแคมเปญ 

            using (SSLsEntities db = new SSLsEntities())
            {
                var promotion = db.PriceSchedule.SingleOrDefault(w => w.Id == id);
                if (promotion.FKCampaign == MyConstant.CampaignType.DiscountDay)
                {
                    // นาทีทอง
                    SearchEditDiscountDay obj = new SearchEditDiscountDay(id);
                    obj.ShowDialog();
                }
                else if (promotion.FKCampaign == MyConstant.CampaignType.FullyQtyAndGift)
                {
                    // ซื้อครบได้ แถม
                    SearchEditQtyAndGift obj = new SearchEditQtyAndGift(id);
                    obj.ShowDialog();
                }
                else if (promotion.FKCampaign == MyConstant.CampaignType.FullyAmountAndDiscount)
                {
                    // ซื้อครบเงิน ได้รับส่วนลด
                    SearchEditAmountAndDis obj = new SearchEditAmountAndDis(id);
                    obj.ShowDialog();
                }
                else if (promotion.FKCampaign == MyConstant.CampaignType.FullyQtyAndDiscount)
                {
                    // ซื้อครบจำนวน ได้รับส่วนลด
                    SearchEditQtyAndDis obj = new SearchEditQtyAndDis(id);
                    obj.ShowDialog();
                }
                else if (promotion.FKCampaign == MyConstant.CampaignType.FullyQtyAndPrice)
                {
                    // ซื้อครบจำนวน ได้รับสิทธิ์แลกซื้อ
                    SearchEditQtyAndSale obj = new SearchEditQtyAndSale(id);
                    obj.ShowDialog();
                }
                else if (promotion.FKCampaign == MyConstant.CampaignType.FullyAmountAndPrice)
                {
                    // ซื้อครบเงิน ได้รับสิทธิ์แลกซื้อ
                    SearchEditAmountAndSale obj = new SearchEditAmountAndSale(id);
                    obj.ShowDialog();
                }
                else
                {
                    MessageBox.Show("ยังไม่พร้อมใช้งาน");
                }
            }

        }
        /// <summary>
        /// ยกเลิกทั้งแคมเปญทิ้ง
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    int row = dataGridView1.CurrentRow.Index;
                    int id = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        var priceSchedule = db.PriceSchedule.SingleOrDefault(w => w.Id == id);
                        priceSchedule.Enable = false;
                        priceSchedule.UpdateDate = DateTime.Now;
                        priceSchedule.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        foreach (var item in priceSchedule.SellingPrice)
                        {
                            item.Enable = false;
                            item.UpdateDate = DateTime.Now;
                            item.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            foreach (var dtl in item.SellingPriceDetails)
                            {
                                dtl.Enable = false;
                                dtl.UpdateDate = DateTime.Now;
                                dtl.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            }
                        }
                        db.Entry(priceSchedule).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    Reload();
                    break;
                case DialogResult.No:
                    break;
            }
        }
        /// <summary>
        /// ยืดอายุการเล่น โปรไปอิก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            using (SSLsEntities db = new SSLsEntities())
            {
                var priceSchedule = db.PriceSchedule.SingleOrDefault(w => w.Id == id);
                SearchEditLongTimeDialog obj = new SearchEditLongTimeDialog(this, priceSchedule);
                obj.ShowDialog();
            }

        }
        /// <summary>
        /// โหลด จัดการโปรโมชั่น ** แจ้งเตือนโปรโมชั่น
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchEditPromotionForm_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("แจ้งเตือนโปรโมชั่น");
        }
    }
}
