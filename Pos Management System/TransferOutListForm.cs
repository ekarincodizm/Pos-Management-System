using Pos_Management_System.Singleton;
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
    public partial class TransferOutListForm : Form
    {
        public TransferOutListForm()
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
                    this.dataGridView1[0, j].Value = true;
                }
            }
            else
            {
                for (int j = 0; j <= this.dataGridView1.RowCount - 1; j++)
                {
                    this.dataGridView1[0, j].Value = false;
                }
            }
        }
        private void TransferOutListForm_Load(object sender, EventArgs e)
        {
            DataGridViewCheckBoxColumn ColumnCheckBox = new DataGridViewCheckBoxColumn();
            ColumnCheckBox.Width = width_columcheckbox;
            ColumnCheckBox.DataPropertyName = "Select";
            //dataGridView1.Columns.Add(ColumnCheckBox);
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(0, -1, true);
            ckBox.Size = new Size(14, 14);
            rect.X = rect.Location.X + (rect.Width / 2) - (ckBox.Width / 2);
            rect.Y += 3;
            ckBox.Location = rect.Location;
            this.ckBox.CheckedChanged += new EventHandler(this.ckBox_CheckedChanged);
            dataGridView1.Controls.Add(ckBox);
            dataGridView1.Columns[0].Frozen = false;

            ReloadGrid();


        }

        public void ReloadGrid()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<StoreFrontTransferOut> list = new List<StoreFrontTransferOut>();
                list = db.StoreFrontTransferOut.Where(w =>
                DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();

                decimal total = 0;
                foreach (var item in list)
                {
                    string status = "";
                    if (item.Enable == false)
                    {
                        status = "ยกเลิกใบโอน";
                    }
                    else if (item.ConfirmDate != null)
                    {
                        status = "ยืนยันใบโอน";
                    }
                    else
                    {
                        status = "สร้างใบโอน";
                    }

                    dataGridView1.Rows.Add(
                        false,
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Code,
                        item.StoreFrontTransferOutDtl.Where(w => w.Enable == true).ToList().Count(),
                        Library.ConvertDecimalToStringForm(item.TotalQtyUnit),
                        Library.ConvertDecimalToStringForm(item.TotalBalance),
                        status,
                        item.Branch.Name + " " + item.Branch.BranchNo,
                        item.Remark
                        );
                    total += item.TotalBalance;
                }
                textBoxQtyList.Text = list.Count() + "";
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(total);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReloadGrid();
        }
        /// <summary>
        /// แก้ไขใบฌอน จะต้องยังไม่ยืนยัน และ enable อยุ่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString();
            TransferOutForm obj = new TransferOutForm(this, code);
            obj.Show();
            //using (SSLsEntities db = new SSLsEntities())
            //{
            //    var data = db.StoreFrontTransferOut.SingleOrDefault(w => w.Code == code);

            //    TransferOutEditForm obj = new TransferOutEditForm(this, code);
            //    obj.ShowDialog();

            //}
        }
        List<string> GetAllCodeForAction()
        {
            List<string> ids = new List<string>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var code = dataGridView1.Rows[i].Cells[3].Value.ToString();
                string check = "";
                if (dataGridView1.Rows[i].Cells[0].Value == null)
                {
                    check = "FALSE";
                }
                else
                {
                    check = dataGridView1.Rows[i].Cells[0].Value.ToString();
                }

                Console.WriteLine(code + " --------- " + check);
                if (check.ToUpper() == "TRUE")
                {
                    ids.Add(code);
                }
            }
            return ids;
        }
        /// <summary>
        /// ยกเลิกใบโอน ต้อง enable อยู่ และยังไม่ confirm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            var stackCode = GetAllCodeForAction();
            using (SSLsEntities db = new SSLsEntities())
            {
                foreach (var item in stackCode)
                {
                    var data = db.StoreFrontTransferOut.SingleOrDefault(w => w.Code == item);
                    if (data.Enable == false)
                    {
                        MessageBox.Show(item + " ถูกยกเลิกไปแล้ว");
                        return;
                    }
                    else if (data.ConfirmDate != null)
                    {
                        MessageBox.Show(item + " ยืนยันไปแล้ว");
                        return;
                    }
                }
                DialogResult dr = MessageBox.Show("ยืนยันทำรายการ ?",
                  "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
                switch (dr)
                {
                    case DialogResult.Yes:
                        foreach (var item in stackCode)
                        {
                            var data = db.StoreFrontTransferOut.SingleOrDefault(w => w.Code == item);
                            data.Enable = false;
                            data.UpdateDate = DateTime.Now;
                            data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            db.Entry(data).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                        ReloadGrid();
                        break;
                    case DialogResult.No:
                        break;
                }
            }
        }
        /// <summary>
        /// ยืนยัน จะตัด stock หน้าร้าน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            var stackCode = GetAllCodeForAction();
            using (SSLsEntities db = new SSLsEntities())
            {
                foreach (var item in stackCode)
                {
                    var data = db.StoreFrontTransferOut.SingleOrDefault(w => w.Code == item);
                    if (data.Enable == false)
                    {
                        MessageBox.Show(item + " ถูกยกเลิกไปแล้ว");
                        return;
                    }
                    else if (data.ConfirmDate != null)
                    {
                        MessageBox.Show(item + " ยืนยันไปแล้ว");
                        return;
                    }
                }
                DialogResult dr = MessageBox.Show("ยืนยันทำรายการ ?",
                  "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
                switch (dr)
                {
                    case DialogResult.Yes:
                        foreach (var item in stackCode)
                        {
                            var data = db.StoreFrontTransferOut.SingleOrDefault(w => w.Code == item);                        
                            data.ConfirmDate = DateTime.Now;
                            data.ConfirmBy = Singleton.SingletonAuthen.Instance().Id;
                            db.Entry(data).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                        // ตัดสต๊อก                      
                        // Initisl หน้าร้าน ก่อน เผื่อยังไม่มี
                        List<StoreFrontStock> stocks = new List<StoreFrontStock>();
                        foreach (var item in stackCode)
                        {
                            var data = db.StoreFrontTransferOut.SingleOrDefault(w => w.Code == item);
                            List<int> fkPro = data.StoreFrontTransferOutDtl.Select(w => w.ProductDetails.FKProduct).ToList().Distinct().ToList<int>();
                            foreach (var fk in fkPro)
                            {
                                var stock = db.StoreFrontStock.FirstOrDefault(w => w.Enable == true && w.FKProduct == fk);
                                if (stock == null)
                                {
                                    stocks.Add(new StoreFrontStock()
                                    {
                                        CreateDate = DateTime.Now,
                                        CreateBy = SingletonAuthen.Instance().Id,
                                        UpdateDate = DateTime.Now,
                                        UpdateBy = SingletonAuthen.Instance().Id,
                                        Enable = true,
                                        CurrentQty = 0,
                                        FKProduct = fk,
                                        Description = "พบการโอนสาขา"
                                    });
                                }
                            }
                            db.StoreFrontStock.AddRange(stocks);
                            db.SaveChanges();
                        }
                       
                        // - หน้าร้าน
                        //List<StoreFrontStockDetails> addDtl = new List<StoreFrontStockDetails>();
                        
                        foreach (var item in stackCode)
                        {
                            var data = db.StoreFrontTransferOut.SingleOrDefault(w => w.Code == item);
                            int number = 1;
                            foreach (var getDtl in data.StoreFrontTransferOutDtl.Where(w=>w.Enable == true).ToList())
                            {
                                var proDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == getDtl.FKProductDetails && w.Enable == true);
                                var stockHD = db.StoreFrontStock.FirstOrDefault(w => w.FKProduct == proDtl.FKProduct && w.Enable == true);
                                StoreFrontStockDetails addDtl = new StoreFrontStockDetails();
                                addDtl.DocNo = data.Code;
                                addDtl.DocDtlNumber = number;
                                addDtl.Description = "โอนไปสาขาอื่น";
                                addDtl.CreateDate = DateTime.Now;
                                addDtl.CreateBy = SingletonAuthen.Instance().Id;
                                addDtl.UpdateDate = DateTime.Now;
                                addDtl.UpdateBy = SingletonAuthen.Instance().Id;
                                addDtl.Enable = true;
                                addDtl.ActionQty = getDtl.Qty * proDtl.PackSize;
                                addDtl.FKStoreFrontStock = stockHD.Id;
                                addDtl.FKTransactionType = MyConstant.PosTransaction.TransferStoreFrontToBranch;
                                addDtl.Barcode = proDtl.Code;
                                addDtl.Name = proDtl.Products.ThaiName;
                                addDtl.FKProductDetails = getDtl.FKProductDetails;
                                addDtl.ResultQty = addDtl.ActionQty;
                                var lastAction = db.StoreFrontStockDetails.OrderByDescending(w => w.CreateDate).FirstOrDefault(w => w.FKProductDetails == getDtl.FKProductDetails && w.Enable == true);
                                if (lastAction != null)
                                {
                                    addDtl.ResultQty = lastAction.ResultQty - addDtl.ActionQty;
                                }
                                addDtl.PackSize = proDtl.PackSize;
                                addDtl.DocRefer = "-";
                                addDtl.DocReferDtlNumber = 0;
                                addDtl.CostOnlyPerUnit = proDtl.CostOnly;
                                addDtl.SellPricePerUnit = proDtl.SellPrice;

                                stockHD.CurrentQty = stockHD.CurrentQty - addDtl.ActionQty;
                                db.Entry(stockHD).State = EntityState.Modified;

                                db.StoreFrontStockDetails.Add(addDtl);
                                db.SaveChanges();
                                number++;
                            }
                        }
                        
                        ReloadGrid();
                        break;
                    case DialogResult.No:
                        break;
                }
            }

            //string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
            //using (SSLsEntities db = new SSLsEntities())
            //{
            //    var data = db.StoreFrontTransferOut.SingleOrDefault(w => w.Code == code);
            //    if (data.Enable == true && data.ConfirmDate == null)
            //    {
            //        DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?", "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            //        switch (dr)
            //        {
            //            case DialogResult.Yes:
            //                /// ยืนยัน 
            //                data.ConfirmDate = DateTime.Now;
            //                data.ConfirmBy = Singleton.SingletonAuthen.Instance().Id;
            //                data.UpdateDate = DateTime.Now;
            //                data.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
            //                db.Entry(data).State = EntityState.Modified;
            //                db.SaveChanges();
            //                /// Update Stock Version เก่า
            //                //Library.MakeValueForUpdateStockPos(data.StoreFrontTransferOutDtl.Where(w => w.Enable == true).ToList());

            //                ///// StoreFrontStock
            //                foreach (var item in data.StoreFrontTransferOutDtl.Where(w => w.Enable == true).ToList())
            //                {
            //                    // add to StoreFrontStockDetails
            //                    var prodtl = Singleton.SingletonProduct.Instance().ProductDetails
            //                        .SingleOrDefault(w => w.Enable == true && w.Id == item.FKProductDetails);
            //                    // check in header
            //                    var storeFrontStock = db.StoreFrontStock.FirstOrDefault(w => w.Enable == true && w.FKProduct == prodtl.FKProduct);

            //                }
            //                ReloadGrid();
            //                break;
            //            case DialogResult.No:
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("ไม่สามารถยกเลิกได้");
            //    }
            //}
        }
        int keyCode = 2;
        int brachToe = 7;
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                /// รหัสใบออเดอร์
                string brachDestination = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[8].Value.ToString();
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString();
                frmMainReport mr = new frmMainReport(this, code, brachDestination);
                mr.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("จำนวนเอกสารผิดพลาด");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
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
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(0, -1, true);
            ckBox.Size = new Size(14, 14);
            rect.X = rect.Location.X + (rect.Width / 2) - (ckBox.Width / 2);
            rect.Y += 3;
            ckBox.Location = rect.Location;
        }
    }
}
