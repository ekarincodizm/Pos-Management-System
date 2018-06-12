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
    public partial class SearchEditLongTimeDialog : Form
    {
        private SearchEditPromotionForm searchEditPromotionForm;
        private PriceSchedule priceSchedule;

        public SearchEditLongTimeDialog()
        {
            InitializeComponent();
        }

        public SearchEditLongTimeDialog(SearchEditPromotionForm searchEditPromotionForm, PriceSchedule priceSchedule)
        {
            InitializeComponent();
            this.searchEditPromotionForm = searchEditPromotionForm;
            this.priceSchedule = priceSchedule;
        }
        /// <summary>
        /// โหลดฟอม set ui
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchEditLongTimeDialog_Load(object sender, EventArgs e)
        {
            /// check promotion 
            /// ถ้ายังไม่เริ่ม เท่านั้น ถึงจะแก้ไขเวลาเริ่มได้

            if ((int.Parse(DateTime.Now.ToString("yyyyMMdd")) >= int.Parse(priceSchedule.StartDate.ToString("yyyyMMdd"))) && (priceSchedule.IsStop == false) && (priceSchedule.Enable == true))
            {
                /// กำลังเล่นรายการ อนุญาติให้ แก้ไขวันสิ้นสุดได้
                dateTimePickerStart.Value = priceSchedule.StartDate;
                dateTimePickerStart.Enabled = false;

                dateTimePickerEnd.MinDate = DateTime.Now;
            }
            else if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) < int.Parse(priceSchedule.StartDate.ToString("yyyyMMdd")))
            {
                /// ยังไม่เริ่ม
                dateTimePickerStart.MinDate = DateTime.Now;
                dateTimePickerEnd.MinDate = DateTime.Now;

                dateTimePickerStart.Value = priceSchedule.StartDate;
                dateTimePickerEnd.Value = priceSchedule.EndDate;
            }
            else
            {
                dateTimePickerStart.Value = priceSchedule.StartDate;
                dateTimePickerEnd.Value = priceSchedule.EndDate;

                dateTimePickerStart.Enabled = false;
                dateTimePickerEnd.Enabled = false;
                button2.Enabled = false;
            }

            //textBox1.Text = priceSchedule.StartDate.ToString("dd/MM/yyyy");

        }
        /// <summary>
        /// ตกลง ต่อเวลา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                priceSchedule = db.PriceSchedule.SingleOrDefault(w => w.Id == priceSchedule.Id);
                priceSchedule.StartDate = dateTimePickerStart.Value;
                priceSchedule.EndDate = dateTimePickerEnd.Value;
                priceSchedule.UpdateDate = DateTime.Now;
                priceSchedule.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                db.Entry(priceSchedule).State = EntityState.Modified;
                db.SaveChanges();
                this.searchEditPromotionForm.Reload();
                this.Dispose();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
