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
    public partial class WasteManageForm : Form
    {
        public WasteManageForm(Form1 frm)
        {
            InitializeComponent();
        }

        private void WasteManageForm_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(1);
            textBoxRemark.Select();
            textBoxDate.Text = Library.ConvertDateToThaiDate(DateTime.Now);

            //comboBoxWarehouse.DataSource = SingletonWarehouse.Instance().Warehouses;
            //comboBoxWarehouse.DisplayMember = "Name";
            //comboBoxWarehouse.ValueMember = "Id";
            //// first active data
            //textBoxDescWH.Text = SingletonWarehouse.Instance().Warehouses.FirstOrDefault().Description;

            //comboBoxReason.DataSource = SingletonWasteReason.Instance().WasteReasons;
            //comboBoxReason.DisplayMember = "Name";
            //comboBoxReason.ValueMember = "Id";

            using (SSLsEntities db = new SSLsEntities())
            {
                Singleton.SingletonWasteReason.Instance();
                Singleton.SingletonWarehouse.Instance();

                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var tw = db.StoreFrontTransferWaste.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth).Count() + 1;
                string code = MyConstant.PrefixForGenerateCode.TransferWaste + SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(tw, 3);
                textBoxCode.Text = code;
                textBoxDate.Text = Library.ConvertDateToThaiDate(DateTime.Now);

            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// เปิดเลือก เหตุผลการทำคืน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SelectedWasteReasonPopup obj = new SelectedWasteReasonPopup(this);
            obj.ShowDialog();
        }
        int _WasteReason;
        public void BinddingWasteReasonSelected(WasteReason send)
        {
            _WasteReason = send.Id;
            textBoxWRCode.Text = send.Code;
            textBoxWRName.Text = send.Name;
        }
        /// <summary>
        /// click เลือก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                switch (e.ColumnIndex)
                {
                    case 2:
                        SelectedProductPopup obj = new SelectedProductPopup(this);
                        obj.ShowDialog();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }
        int colId = 0;
        int colCode = 1;
        int colSearch = 2;
        int colName = 3;
        int colUnit = 4;
        int colPZ = 5;
        int colQty = 6;
        int colCostPerUnit = 7;
        int colDescription = 8;
        public void BinddingProductChoose(int id)
        {
            List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
            var product = products.SingleOrDefault(w => w.Id == id);
            // start bind ui
            int currentRow = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows[currentRow].Cells[colId].Value = product.Id;
            dataGridView1.Rows[currentRow].Cells[colCode].Value = product.Code;
            dataGridView1.Rows[currentRow].Cells[colName].Value = product.Products.ThaiName;
            dataGridView1.Rows[currentRow].Cells[colUnit].Value = product.ProductUnit.Name;
            dataGridView1.Rows[currentRow].Cells[colPZ].Value = product.PackSize;
            dataGridView1.Rows[currentRow].Cells[colQty].Value = 1.00;
            dataGridView1.Rows[currentRow].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostOnly);
            dataGridView1.Rows[currentRow].Cells[colDescription].Value = "-";
            //dataGridView1.Rows[currentRow].Cells[colQty].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[colQty];
            dataGridView1.BeginEdit(true);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// บันทึก พร้อม
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
                StoreFrontTransferWaste sft = new StoreFrontTransferWaste();
                sft.Enable = true;
                sft.Code = textBoxCode.Text;
                sft.CreateDate = DateTime.Now;
                sft.CreateBy = SingletonAuthen.Instance().Id;
                sft.Description = textBoxRemark.Text;
                sft.UpdateDate = DateTime.Now;
                sft.UpdateBy = SingletonAuthen.Instance().Id;
                decimal qtyPiece = 0;
                List<StoreFrontTransferWasteDtl> details = new List<StoreFrontTransferWasteDtl>();
                StoreFrontTransferWasteDtl detail;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var code = dataGridView1.Rows[i].Cells[colCode].Value;
                    if (code == null)
                    {
                        continue;
                    }
                    detail = new StoreFrontTransferWasteDtl();
                    qtyPiece += decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString()) * decimal.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString());
                    detail.Enable = true;
                    detail.Description = dataGridView1.Rows[i].Cells[colDescription].Value.ToString();
                    detail.CreateDate = DateTime.Now;
                    detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.UpdateDate = DateTime.Now;
                    detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.FKProductDetails = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                    detail.Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    detail.CostPerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                    details.Add(detail);
                    //if (i >= dataGridView1.Rows.Count - 2) break;
                }
                sft.TotalQty = qtyPiece;
                sft.TotalQtyUnit = details.Sum(w => w.Qty);

                sft.FKWarehouse = MyConstant.WareHouse.StoreFront;
                sft.FKWasteReason = _WasteReason;
                sft.StoreFrontTransferWasteDtl = details;
                db.StoreFrontTransferWaste.Add(sft);
                db.SaveChanges();

                // + ห้องของเสีย
                // Initisl waste warehouse ก่อน
                List<int> fkProDtl = details.Select(w => w.FKProductDetails).Distinct().ToList<int>();
                List<int> fkPro = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => fkProDtl.Contains(w.Id)).Select(w => w.FKProduct).Distinct().ToList<int>();
                List<WasteWarehouse> wss = new List<WasteWarehouse>();
                foreach (var item in fkPro)
                {
                    var data = db.WasteWarehouse.FirstOrDefault(w => w.FKProduct == item && w.Enable == true);
                    if (data == null)
                    {
                        WasteWarehouse ws = new WasteWarehouse();
                        ws.FKProduct = item;
                        ws.FKWarehouse = MyConstant.WareHouse.WasteWarehouse;
                        ws.QtyPiece = 0;
                        ws.QtyUnit = 0;
                        ws.Description = "Auto Detect";
                        ws.Enable = true;
                        ws.CreateDate = DateTime.Now;
                        ws.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        ws.UpdateDate = DateTime.Now;
                        ws.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        wss.Add(ws);
                    }
                }
                db.WasteWarehouse.AddRange(wss);
                db.SaveChanges();
                /// จัดการ stock
                /// -storefront
                //Library.MakeValueForUpdateStockPos(details); // อันเดิม ยกเลิก
                /// + warehouse
                ///Library.MakeValueForUpdateStockWms(details);
                /// + ห้องของเสีย
                Library.AddWasteWarehouse(details, sft.Code);
                // print ใบ ของเสียหน้าร้าน สู่ ห้องของเสีย
                // Initisl หน้าร้าน ก่อน เผื่อยังไม่มี
                List<StoreFrontStock> stocks = new List<StoreFrontStock>();
                foreach (var item in fkPro)
                {
                    var data = db.StoreFrontStock.FirstOrDefault(w => w.Enable == true && w.FKProduct == item);
                    if (data == null)
                    {
                        stocks.Add(new StoreFrontStock()
                        {
                            CreateDate = DateTime.Now,
                            CreateBy = SingletonAuthen.Instance().Id,
                            UpdateDate = DateTime.Now,
                            UpdateBy = SingletonAuthen.Instance().Id,
                            Enable = true,
                            CurrentQty = 0,
                            FKProduct = item,
                            Description = "พบของเสียห้านร้าน"
                        });
                    }
                }
                db.StoreFrontStock.AddRange(stocks);
                db.SaveChanges();
                // - หน้าร้าน              
                int number = 1;
                foreach (var item in details)
                {
                    var proDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails && w.Enable == true);
                    var stockHD = db.StoreFrontStock.FirstOrDefault(w => w.FKProduct == proDtl.FKProduct && w.Enable == true);
                    StoreFrontStockDetails addDtl = new StoreFrontStockDetails();
                    addDtl.DocNo = sft.Code;
                    addDtl.DocDtlNumber = number;
                    addDtl.Description = "ของเสียหน้าร้าน";
                    addDtl.CreateDate = DateTime.Now;
                    addDtl.CreateBy = SingletonAuthen.Instance().Id;
                    addDtl.UpdateDate = DateTime.Now;
                    addDtl.UpdateBy = SingletonAuthen.Instance().Id;
                    addDtl.Enable = true;
                    addDtl.ActionQty = item.Qty * proDtl.PackSize;
                    addDtl.FKStoreFrontStock = stockHD.Id;
                    addDtl.FKTransactionType = MyConstant.PosTransaction.CNToWarehouse;
                    addDtl.Barcode = proDtl.Code;
                    addDtl.Name = proDtl.Products.ThaiName;
                    addDtl.FKProductDetails = item.FKProductDetails;
                    addDtl.ResultQty = addDtl.ActionQty;
                    var lastAction = db.StoreFrontStockDetails.OrderByDescending(w => w.CreateDate).FirstOrDefault(w => w.FKProductDetails == item.FKProductDetails && w.Enable == true);
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

                try
                {
                    /// ใบของเสียหน้าร้าน         
                    frmMainReport mr = new frmMainReport(this, sft.Code);
                    mr.Show();
                }
                catch (Exception)
                {
                    MessageBox.Show("จำนวนเอกสารผิดพลาด");
                }

                this.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var code = dataGridView1.Rows[e.RowIndex].Cells[colCode].Value.ToString();
                ProductDetails product = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == code);
                int currentRow = e.RowIndex;
                switch (e.ColumnIndex)
                {
                    case 1:
                        if (product != null)
                        {

                        }
                        break;
                    case 6: // จำนวน
                        // ราคา/หน่วย * จำนวน
                        //var qty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                        //decimal cost = decimal.Parse(dataGridView1.Rows[currentRow].Cells[colCostPerUnit].Value.ToString());
                        //dataGridView1.Rows[currentRow].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(qty * cost);
                        //BinddingTotal();

                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// enter grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    int col = dataGridView1.CurrentCell.ColumnIndex;
                    int row = dataGridView1.CurrentRow.Index;
                    if (col == colCode)
                    {
                        var code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colCode].Value.ToString();
                        ProductDetails product = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == code);

                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colId].Value = product.Id;
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colCode].Value = product.Code;
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colName].Value = product.Products.ThaiName;
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colUnit].Value = product.ProductUnit.Name;
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colPZ].Value = product.PackSize;
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colQty].Value = 1.00;
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostAndVat);

                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colDescription].Value = "-";


                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                        dataGridView1.BeginEdit(true);
                        //BinddingTotal();
                    }
                    else if (col == colQty)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colDescription];
                        dataGridView1.BeginEdit(true);
                    }
                    else if (col == colDescription)
                    {
                        dataGridView1.Rows.Add(1);
                        dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[1];
                        dataGridView1.BeginEdit(true);
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
