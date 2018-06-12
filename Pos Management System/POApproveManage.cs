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
using AutoMapper.Mappers;
using Omu.ValueInjecter;
namespace Pos_Management_System
{
    public partial class POApproveManage : Form
    {
        public POApproveManage()
        {
            InitializeComponent();
        }

        private System.Windows.Forms.DataGridView songsDataGridView = new System.Windows.Forms.DataGridView();
        CheckBox ckBox = new CheckBox();
        int width_columcheckbox = 50;

        private void ckBox_CheckedChanged(object sender, EventArgs e)
        {
            int i = 0;
            if (ckBox.Checked == true)
            {
                for (int j = 0; j <= this.dataGridView1.RowCount - 1; j++)
                {
                    this.dataGridView1[9, j].Value = true;
                }
            }
            else
            {
                for (int j = 0; j <= this.dataGridView1.RowCount - 1; j++)
                {
                    this.dataGridView1[9, j].Value = false;
                }
            }
        }
        private void POApproveManage_Load(object sender, EventArgs e)
        {
            DataGridViewCheckBoxColumn ColumnCheckBox = new DataGridViewCheckBoxColumn();
            ColumnCheckBox.Width = width_columcheckbox;
            ColumnCheckBox.DataPropertyName = "Select";
            //dataGridView1.Columns.Add(ColumnCheckBox);
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(9, -1, true);
            ckBox.Size = new Size(14, 14);
            rect.X = rect.Location.X + (rect.Width / 2) - (ckBox.Width / 2);
            rect.Y += 3;
            ckBox.Location = rect.Location;
            this.ckBox.CheckedChanged += new EventHandler(this.ckBox_CheckedChanged);
            dataGridView1.Controls.Add(ckBox);
            dataGridView1.Columns[9].Frozen = false;
            Search();
            //using (SSLsEntities db = new SSLsEntities())
            //{
            //    List<POHeader> poList = new List<POHeader>();
            //    poList = db.POHeader.Include("PaymentType").Include("POStatus").Include("PODetail.ProductDetails.Products").Include("PODetail.ProductDetails.ProductUnit").Include("Vendor.POCostType").Include("PORcv.PORcvDetails").Include("PORcv.PORcvDetails")
            //        .Where(w => w.Enable == true && DbFunctions.TruncateTime(DateTime.Now) == DbFunctions.TruncateTime(w.CreateDate))
            //        .OrderBy(w => w.CreateDate)
            //        .ToList();
            //    foreach (var item in poList)
            //    {
            //        string status = "";
            //        if (item.ApproveDate == null)
            //        {
            //            status = "ยังไม่อนุมัติ";
            //        }
            //        else
            //        {
            //            status = "อนุมัติแล้ว";
            //        }
            //        dataGridView1.Rows.Add(item.Id, Library.ConvertDateToThaiDate(item.CreateDate), Library.GetFullNameUserById(item.CreateBy), item.PONo, Library.ConvertDateToThaiDate(item.POExpire), status, Library.ConvertDecimalToStringForm(item.TotalDiscount), Library.ConvertDecimalToStringForm(item.TotalBalance), item.Vendor.Name, false);
            //    }
            //    RowColor();
            //}
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
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Search();
        }
        void Search()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<POHeader> poList = new List<POHeader>();
                poList = db.POHeader.Include("PaymentType").Include("POStatus").Include("PODetail.ProductDetails.Products").Include("PODetail.ProductDetails.ProductUnit").Include("Vendor.POCostType").Include("PORcv.PORcvDetails").Include("PORcv.PORcvDetails")
                    .Where(w => w.Enable == true && 
                    DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) && 
                    DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE))
                    .OrderBy(w => w.CreateDate).ToList();
                //if (checkBoxApp.Checked == false) // ไม่ต้องการ po อนุมัติแล้ว
                //{
                //    poList = poList.Where(w => w.ApproveDate != null).ToList();
                //}
                //if (checkBoxApp.Checked == true && checkBoxNotApp.Checked == false)
                //{
                //    // ถ้า อนุมัติอย่างเดียว
                //    poList = poList.Where(w => w.ApproveDate != null).ToList();
                //}
                //else if (checkBoxApp.Checked == false && checkBoxNotApp.Checked == true)
                //{
                //    // ถ้า ไม่อนุมัติอย่างเดียว
                //    poList = poList.Where(w => w.ApproveDate == null).ToList();
                //}
                //else if (checkBoxApp.Checked == true && checkBoxNotApp.Checked == true)
                //{
                //    // เอาทั้งหมด ไมต้องมีเงื่อนไข
                //}
                //else
                //{
                //    //poList = poList.Where(w => w.ApproveDate != null).ToList();
                //}
                foreach (var item in poList)
                {
                    if (checkBoxApp.Checked == false)// ไม่ต้องการ po อนุมัติแล้ว
                    {
                        if (item.ApproveDate != null)
                        {
                            continue;
                        }
                    }
                    if (checkBoxWait.Checked == false)
                    {
                        if (item.ApproveDate == null && item.NotApproveDate == null)
                        {
                            continue;
                        }
                    }
                    if (checkBoxNotApp.Checked == false)
                    {
                        if (item.ApproveDate == null && item.NotApproveDate != null)
                        {
                            continue;
                        }
                    }

                    dataGridView1.Rows.Add(
                        item.Id,
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.PONo,
                        Library.ConvertDateToThaiDate(item.POExpire),
                        GetStatusPO(item),
                        Library.ConvertDecimalToStringForm(item.TotalDiscount),
                        Library.ConvertDecimalToStringForm(item.TotalBalance),
                        item.Vendor.Name);
                }
                //RowColor();
            }
        }

        private string GetStatusPO(POHeader item)
        {
            if (item.ApproveDate == null && item.NotApproveDate == null)
            {
                return "รออนุมัติ";
            }
            else if (item.ApproveDate != null)
            {
                return "อนุมัติแล้ว";
            }
            else if (item.ApproveDate == null && item.NotApproveDate != null)
            {
                return "ไม่อนุมัติ";
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// ก่อนพิมพ์ PO ต้องเชคการแก้ไข po ด้วย ว่าต้องพิมพ์รอบเท่าไหร่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            string code = dataGridView1.Rows[row].Cells[3].Value.ToString();
            using (SSLsEntities db = new SSLsEntities())
            {
                //var polist = db.POHeader.Where(w => w.PONo == code).OrderBy(w => w.SequenceEdit).ToList();
                //if (polist.Count() > 1)
                //{
                //    //MessageBox.Show("พบ PO หลายฉบับ");                  
                //    POApproveManagePrintSequenceDialog obj = new POApproveManagePrintSequenceDialog(this, polist.Count(), code);
                //    obj.ShowDialog();
                //}
                //else
                //{
                //    MainReportViewer mr = new MainReportViewer(this, id);
                //    mr.ShowDialog();
                //}
                MainReportViewer mr = new MainReportViewer(this, id);
                mr.ShowDialog();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 9)
            {
                //if (e.RowIndex == 0) // click header
                //{
                //    MessageBox.Show("asdfgasfgafdg");
                //    return;
                //}

                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                }
                else if (bool.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == false)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                }
                else
                {
                    // true
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                }
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(9, -1, true);
            ckBox.Size = new Size(14, 14);
            rect.X = rect.Location.X + (rect.Width / 2) - (ckBox.Width / 2);
            rect.Y += 3;
            ckBox.Location = rect.Location;
        }

        /// <summary>
        /// อนุมัติที่เลือก อนุมัติเฉพาะ po ที่ ApproveDate = null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonApprove_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    SaveCommit();
                    break;
                case DialogResult.No:
                    break;
            }

        }

        private void SaveCommit()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                List<int> idsPO = new List<int>();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var po = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    string check = "";
                    if (dataGridView1.Rows[i].Cells[9].Value == null)
                    {
                        check = "FALSE";
                    }
                    else
                    {
                        check = dataGridView1.Rows[i].Cells[9].Value.ToString();
                    }

                    var id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    Console.WriteLine(id + " " + po + " --------- " + check);
                    if (check.ToUpper() == "TRUE")
                    {
                        idsPO.Add(int.Parse(id));
                    }

                }
                var pos = db.POHeader.Where(w => idsPO.Contains(w.Id)).ToList();
                foreach (var item in pos)
                {
                    if (item.ApproveDate != null)
                    {
                        MessageBox.Show("พบ PO อนุมัติแล้ว " + item.PONo);
                        return;
                    }
                    item.ApproveDate = DateTime.Now;
                    item.ApproveBy = Singleton.SingletonAuthen.Instance().Id;
                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges();
                Search();
            }
        }
        /// <summary>
        /// ไม่อนุมัติ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNotApprove_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    NotApproveSave();
                    break;
                case DialogResult.No:
                    break;
            }
        }
        /// <summary>
        /// po ที่จะ ไม่อนุมัติ หรือเตะทิ้ง จะต้องยังไม่อนูมัติ
        /// </summary>
        private void NotApproveSave()
        {
            List<int> idsPO = new List<int>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var po = dataGridView1.Rows[i].Cells[3].Value.ToString();
                string check = "";
                if (dataGridView1.Rows[i].Cells[9].Value == null)
                {
                    check = "FALSE";
                }
                else
                {
                    check = dataGridView1.Rows[i].Cells[9].Value.ToString();
                }

                var id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                Console.WriteLine(id + " " + po + " --------- " + check);
                if (check.ToUpper() == "TRUE")
                {
                    idsPO.Add(int.Parse(id));
                }
            }
            Console.WriteLine(idsPO.Count());
            if (idsPO.Count() < 1)
            {
                MessageBox.Show("ไม่มีพบเลือกรายการ");
                return;
            }
            else if (idsPO.Count() > 0 && textBoxNotApproveRemark.Text == "")
            {
                MessageBox.Show("กรุณากรอกเหตุผล");
                return;
            }
            using (SSLsEntities db = new SSLsEntities())
            {
                var pos = db.POHeader.Where(w => idsPO.Contains(w.Id)).ToList();
                foreach (var item in pos)
                {
                    if (item.ApproveDate != null || item.NotApproveDate != null)
                    {
                        MessageBox.Show("พบ PO อนุมัติแล้ว " + item.PONo);
                        return;
                    }
                    item.NotApproveDate = DateTime.Now;
                    item.NotApproveBy = Singleton.SingletonAuthen.Instance().Id;
                    item.NotApproveRemark = textBoxNotApproveRemark.Text;
                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            Search();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    UnApproveSave();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void UnApproveSave()
        {
            List<int> idsPO = new List<int>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var po = dataGridView1.Rows[i].Cells[3].Value.ToString();
                string check = "";
                if (dataGridView1.Rows[i].Cells[9].Value == null)
                {
                    check = "FALSE";
                }
                else
                {
                    check = dataGridView1.Rows[i].Cells[9].Value.ToString();
                }

                var id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                Console.WriteLine(id + " " + po + " --------- " + check);
                if (check.ToUpper() == "TRUE")
                {
                    idsPO.Add(int.Parse(id));
                }
            }
            Console.WriteLine(idsPO.Count());
            if (idsPO.Count() < 1)
            {
                MessageBox.Show("ไม่มีพบเลือกรายการ");
                return;
            }
            else if (idsPO.Count() > 0 && textBoxNotApproveRemark.Text == "")
            {
                MessageBox.Show("กรุณากรอกเหตุผล");
                return;
            }
            using (SSLsEntities db = new SSLsEntities())
            {
                var pos = db.POHeader.Include("PODetail").Where(w => idsPO.Contains(w.Id)).ToList();
                List<POHeader> poAddNew = new List<POHeader>();
                foreach (var item in pos)
                {
                    if (item.ApproveDate != null)
                    {
                        // add new ก่อน
                        POHeader po = new POHeader();
                        po.FKBranch = item.FKBranch;
                        po.FKVender = item.FKVender;
                        po.PONo = item.PONo;
                        po.PODate = item.PODate;
                        po.FKBudgetYear = item.FKBudgetYear;
                        po.ReferenceNo = item.ReferenceNo;
                        po.DueDate = item.DueDate;
                        po.POExpire = item.POExpire;
                        po.FKPaymentType = item.FKPaymentType;
                        po.DiscountInput = item.DiscountInput;
                        po.DiscountBath = item.DiscountBath;
                        po.DiscountPercent = item.DiscountPercent;
                        po.Description = item.Description;
                        po.NotApproveDate = item.NotApproveDate;
                        po.NotApproveBy = item.NotApproveBy;
                        po.NotApproveRemark = item.NotApproveRemark;
                        po.ApproveBy = null;
                        po.ApproveDate = null;
                        po.TotalPrice = item.TotalPrice;
                        po.TotalHasVat = item.TotalHasVat;
                        po.TotalUnVat = item.TotalUnVat;
                        po.TotalDiscount = item.TotalDiscount;
                        po.TotalPriceDiscount = item.TotalPriceDiscount;
                        po.TotalVat = item.TotalVat;
                        po.TotalBalance = item.TotalBalance;
                        po.TotalQty = item.TotalQty;
                        po.TotalGift = item.TotalGift;
                        po.CreateDate = item.CreateDate;
                        po.CreateBy = item.CreateBy;
                        po.UpdateDate = item.UpdateDate;
                        po.UpdateBy = item.UpdateBy;
                        po.Enable = true;
                        po.FKPOStatus = item.FKPOStatus;
                        po.UnApproveDate = null;
                        po.UpApproveBy = null;
                        po.UpApproveRemark = null;
                        po.SequenceEdit = item.SequenceEdit + 1;

                        List<PODetail> details = new List<PODetail>();
                        PODetail dtl;
                        foreach (var d in item.PODetail.Where(w => w.Enable == true).ToList())
                        {
                            dtl = new PODetail();
                            dtl.FKProductDetail = d.FKProductDetail;
                            dtl.Qty = d.Qty;
                            dtl.GiftQty = d.GiftQty;
                            dtl.CostOnly = d.CostOnly;
                            dtl.CostAndVat = d.CostAndVat;
                            dtl.DiscountInput = d.DiscountInput;
                            dtl.DiscountBath = d.DiscountBath;
                            dtl.TotalCost = d.TotalCost;
                            dtl.CreateDate = d.CreateDate;
                            dtl.CreateBy = d.CreateBy;
                            dtl.UpdateDate = d.UpdateDate;
                            dtl.UpdateBy = d.UpdateBy;
                            dtl.Enable = d.Enable;
                            dtl.RcvQty = d.RcvQty;
                            dtl.RcvGiftQty = d.RcvGiftQty;
                            dtl.InterfaceDate = d.InterfaceDate;
                            dtl.Sequence = po.SequenceEdit;
                            po.PODetail.Add(dtl);
                        }
                        db.POHeader.Add(po);
                        ///////////////////////////////////// disableตัวเดิมทิ้ง
                        item.UnApproveDate = DateTime.Now;
                        item.UpApproveBy = Singleton.SingletonAuthen.Instance().Id;
                        item.UpApproveRemark = textBoxNotApproveRemark.Text;
                        item.Enable = false;
                        db.Entry(item).State = EntityState.Modified;
                        foreach (var dtlForDisable in item.PODetail.Where(w => w.Enable == true).ToList())
                        {
                            // disable รายละเอียด
                            dtlForDisable.Enable = false;
                            db.Entry(dtlForDisable).State = EntityState.Modified;
                        }
                        //SendAutoMapper(item);
                        //POHeader po = (POHeader)Mapper.Map<POHeader>(item);
                        //poAddNew.Add(po);                        
                        //db.POHeader.Attach(po);
                        //db.POHeader.Add(po);
                        //POHeader po = new POHeader();
                        //po = item;
                        //po.PODetail = null;
                        //po.ApproveBy = null;
                        //po.ApproveDate = null;
                        //po.UnApproveDate = null;
                        //po.UpApproveBy = null;
                        //po.UpApproveRemark = null;
                        //po.Enable = true;
                        //List<PODetail> details = new List<PODetail>();
                        //PODetail dtls;
                        //foreach (var dtl in item.PODetail.Where(w => w.Enable == true).ToList())
                        //{
                        //    dtls = new PODetail();
                        //    dtls = dtl;
                        //    details.Add(dtls);
                        //    //po.PODetail.Add(dtls);
                        //}
                        //po.PODetail = details;
                        //db.POHeader.Add(po);
                        //db.SaveChanges();

                       
                    }
                    else
                    {
                        MessageBox.Show("ต้องเป็น PO ที่อนุมัติแล้วเท่านั้น " + item.PONo);
                        return;
                    }
                }
                db.SaveChanges();
            }
            Search();
        }

        private void SendAutoMapper(POHeader item)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                POHeader po = new POHeader();

                po.ApproveBy = null;
                po.ApproveDate = null;
                po.UnApproveDate = null;
                po.UpApproveBy = null;
                po.UpApproveRemark = null;
                po.Enable = true;
                db.POHeader.Add(po);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// double click ดูรายการ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            POEditForm obj = new POEditForm(this, id);
            obj.ShowDialog();
        }
    }
}
