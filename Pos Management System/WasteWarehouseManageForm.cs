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
    public partial class WasteWarehouseManageForm : Form
    {
        public WasteWarehouseManageForm()
        {
            //Library.InitialProductWasteWarehouse();
            InitializeComponent();
        }

        private void WasteWarehouseManageForm_Load(object sender, EventArgs e)
        {

            dataGridView1.Rows.Add(1);
            textBoxRemark.Select();
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[1];
            dataGridView1.BeginEdit(true);


            Warehouse warehouse = Singleton.SingletonWarehouse.Instance().Warehouses.SingleOrDefault(w => w.Id == MyConstant.WareHouse.MainWarehouse);
            textBoxWHCode.Text = warehouse.Code;
            textBoxWHDesc.Text = warehouse.Description;
            _WarehouseId = warehouse.Id;

            // Gen DocNo
            using (SSLsEntities db = new SSLsEntities())
            {
                int count = db.WarehouseToWaste.Where(w => w.CreateDate.Year == DateTime.Now.Year && w.CreateDate.Month == DateTime.Now.Month).Count() + 1;
                string code = MyConstant.PrefixForGenerateCode.WHToWaste + Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(count, 4);
                textBoxCode.Text = code;
                textBoxDate.Text = Library.ConvertDateToThaiDate(DateTime.Now);
            }

        }
        int _WarehouseId;
        private void button4_Click(object sender, EventArgs e)
        {
            SelectedWarehousePopup obj = new SelectedWarehousePopup(this);
            obj.ShowDialog();
        }
        public void BinddingWarehouseSelected(Warehouse send)
        {
            textBoxWHCode.Text = send.Code;
            textBoxWHDesc.Text = send.Description;
            _WarehouseId = send.Id;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SelectedWasteReasonPopup obj = new SelectedWasteReasonPopup(this);
            obj.ShowDialog();
        }
        int _WasteReason = 0;
        public void BinddingWasteReasonSelected(WasteReason send)
        {
            _WasteReason = send.Id;
            textBoxWRCode.Text = send.Code;
            textBoxWRName.Text = send.Name;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// click ค้นหา สินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
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
        int colId = 0;
        int colCode = 1;
        int colSearch = 2;
        int colName = 3;
        int colUnit = 4;
        int colPZ = 5;
        int colQty = 6;
        int colCostPerUnit = 7;
        int colTotal = 8;
        int colDescription = 9;
        int colLocation = 10;
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
            dataGridView1.Rows[currentRow].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostAndVat);
            dataGridView1.Rows[currentRow].Cells[colDescription].Value = "-";

            dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[colQty];
            dataGridView1.BeginEdit(true);

        }
        /// <summary>
        /// Bindding Grid
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="Location"></param>
        /// <param name="id"></param>
        public void BinddingProductLocation(decimal qty, string Location, int id)
        {

            int currentRow = dataGridView1.CurrentRow.Index;

            dataGridView1.Rows[currentRow].Cells[colQty].Value = qty;

            dataGridView1.Rows[currentRow].Cells[colLocation].Value = "" + Location;
            dataGridView1.Rows[currentRow].Cells[colLocation + 1].Value = id;

        }
        /// <summary>
        /// enter ongrid
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
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(product.CostAndVat);
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[colDescription].Value = "-";

                        WasteWarehouseManageLocationList obj = new WasteWarehouseManageLocationList(this, product);
                        obj.ShowDialog();

                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                        dataGridView1.BeginEdit(true);
                        BinddingTotal();
                    }
                    else if (col == colQty)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colDescription];
                        dataGridView1.BeginEdit(true);
                    }
                    else if (col == colDescription)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colLocation];
                        dataGridView1.Rows.Add(1);
                        dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[1];
                        dataGridView1.BeginEdit(true);
                    }
                    //else if (col == dataGridView1.ColumnCount - 1)
                    //{
                    //    dataGridView1.Rows.Add(1);
                    //    dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[1];
                    //    dataGridView1.BeginEdit(true);
                    //}
                    break;
                default:
                    break;
            }
        }

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
                            //dataGridView1.Rows[currentRow].Cells[colId].Value = product.Id;
                            //dataGridView1.Rows[currentRow].Cells[colCode].Value = product.Code;
                            //dataGridView1.Rows[currentRow].Cells[colName].Value = product.Products.ThaiName;
                            //dataGridView1.Rows[currentRow].Cells[colUnit].Value = product.ProductUnit.Name;
                            //dataGridView1.Rows[currentRow].Cells[colPZ].Value = product.PackSize;
                            //dataGridView1.Rows[currentRow].Cells[colQty].Value = 1.00;
                            //dataGridView1.Rows[currentRow].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostAndVat);
                            //dataGridView1.Rows[currentRow].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(product.CostAndVat);
                            //dataGridView1.Rows[currentRow].Cells[colDescription].Value = "-";

                            ////dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[colQty];
                            ////dataGridView1.BeginEdit(true);

                            //BinddingTotal();

                            // ตรวจสอบ stock warehouse ในระบบเก่า
                            //using (WH_TRATEntities db = new WH_TRATEntities())
                            //{
                            //    var locstk = db.WH_LOCSTK.Where(w => w.PRODUCT_NO == code);
                            //    if (locstk != null)
                            //    {
                            //        decimal qtyLoc = 0;
                            //        decimal bookqtyLoc = 0;
                            //        if (locstk.QTY != null)
                            //        {
                            //            qtyLoc = (decimal)locstk.QTY;
                            //        }
                            //        if (locstk.BOOK_QTY != null)
                            //        {
                            //            bookqtyLoc = (decimal)locstk.BOOK_QTY;
                            //        }
                            //        if ((qtyLoc-bookqtyLoc) <= 0)
                            //        {
                            //            MessageBox.Show("ไม่มีสินค้าคงเหลือ");
                            //        }
                            //    }
                            //    else
                            //    {
                            //        MessageBox.Show("ไม่มีสินค้าคงเหลือ");
                            //    }
                            //}
                            //}
                            //else
                            //{
                            //    MessageBox.Show("ไม่พบรหัสบาร์โค้ด");
                            //}
                        }
                        break;
                    case 6: // จำนวน
                        // ราคา/หน่วย * จำนวน
                        var qty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                        decimal cost = decimal.Parse(dataGridView1.Rows[currentRow].Cells[colCostPerUnit].Value.ToString());
                        dataGridView1.Rows[currentRow].Cells[colTotal].Value = Library.ConvertDecimalToStringForm(qty * cost);
                        BinddingTotal();

                        //using (WH_TRATEntities db = new WH_TRATEntities())
                        //{
                        //    var locstk = db.WH_LOCSTK.FirstOrDefault(w => w.PRODUCT_NO == code);
                        //    if (locstk != null)
                        //    {
                        //        decimal qtyLoc = 0;
                        //        decimal bookqtyLoc = 0;
                        //        if (locstk.QTY != null)
                        //        {
                        //            qtyLoc = (decimal)locstk.QTY;
                        //        }
                        //        if (locstk.BOOK_QTY != null)
                        //        {
                        //            bookqtyLoc = (decimal)locstk.BOOK_QTY;
                        //        }
                        //        if ((qtyLoc- bookqtyLoc) <= 0)
                        //        {
                        //            MessageBox.Show("ไม่มีสินค้าคงเหลือ");
                        //        }
                        //        else if (qty > (qtyLoc-bookqtyLoc))
                        //        {
                        //            MessageBox.Show("ไม่สามารถคืนจำนวนนี้ได้ Stock เหลือเพียง " + (qtyLoc-bookqtyLoc) + " " + product.ProductUnit.Name);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        MessageBox.Show("ไม่มีสินค้าคงเหลือ");
                        //    }
                        //}

                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }

        private void BinddingTotal()
        {
            decimal totalBalance = 0;
            decimal totalQty = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                decimal qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                totalQty += qty;
                decimal cost = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                decimal sum = qty * cost;
                totalBalance += sum;
                //if (i >= dataGridView1.Rows.Count - 2) break;
            }
            textBoxTotalQty.Text = Library.ConvertDecimalToStringForm(totalQty);
            textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(totalBalance);
        }

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
            try
            {
                WarehouseToWaste wt = new WarehouseToWaste();
                wt.Enable = true;
                wt.Description = textBoxRemark.Text;
                wt.CreateDate = DateTime.Now;
                wt.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                wt.UpdateDate = DateTime.Now;
                wt.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                wt.DocNo = textBoxCode.Text;
                wt.FKWarehouse = _WarehouseId;
                if (_WasteReason == 0)
                {
                    MessageBox.Show("กรุณาเลือกเหตุผล");
                    return;
                }
                wt.FKWasteReason = _WasteReason;
                List<WarehouseToWasteDetails> details = new List<WarehouseToWasteDetails>();
                WarehouseToWasteDetails d;
                using (WH_TRATEntities whEN = new WH_TRATEntities())
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        var code = dataGridView1.Rows[i].Cells[colCode].Value;
                        if (code == null)
                        {
                            continue;
                        }
                        d = new WarehouseToWasteDetails();
                        d.Enable = true;
                        d.Description = textBoxRemark.Text;
                        d.CreateDate = DateTime.Now;
                        d.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        d.UpdateDate = DateTime.Now;
                        d.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        d.FKProductDetails = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                        d.QtyUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                        d.Packsize = decimal.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString());
                        d.PricePerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());

                        decimal qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                        decimal cost = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                        decimal sum = qty * cost;
                        Console.WriteLine(sum);

                        int locId = int.Parse(dataGridView1.Rows[i].Cells[colLocation + 1].Value.ToString());
                        WH_LOCSTK locStk = whEN.WH_LOCSTK.FirstOrDefault(w => w.ROWID == locId);

                        d.Location = locStk.LOCATION_NO;
                        details.Add(d);
                        locStk.UPDATE_DATE = DateTime.Now;
                        locStk.QTY = locStk.QTY - ((float)qty * (float)locStk.PACK_SIZE);
                        if (locStk.QTY <= 0)
                        {
                            // ลบ row ทิ้ง
                            whEN.WH_LOCSTK.Remove(locStk);
                            whEN.SaveChanges();

                            // check WH_LOCSTK เพื่อ set book flag master
                            var locationList = whEN.WH_LOCSTK.Where(w => w.LOCATION_NO == locStk.LOCATION_NO && (w.QTY > 0 || w.BOOK_QTY > 0)).ToList();
                            if (locationList.Count() > 0) // ถ้าบ้านนี้ ยัง
                            {
                                // not set
                            }
                            else
                            {
                                // set book flag
                                // ต้องไปอัพเดท location mast ให้ book flag = null

                                //var locationMast = whEN.WH_LOCATION_MAST.SingleOrDefault(w => w.LOCATION_NO == locStk.LOCATION_NO);
                                //locationMast.BOOK_FLAG = null;
                                //whEN.Entry(locationMast).State = EntityState.Modified;
                                //whEN.SaveChanges();
                                whEN.Database.ExecuteSqlCommand("UPDATE dbo.WH_LOCATION_MAST SET BOOK_FLAG = NULL WHERE LOCATION_NO = '" + locStk.LOCATION_NO + "'");
                            }
                        }
                        else
                        {
                            whEN.Entry(locStk).State = EntityState.Modified;
                            whEN.SaveChanges();
                        }

                    }

                }
                wt.WarehouseToWasteDetails = details;
                using (SSLsEntities db = new SSLsEntities())
                {
                    db.WarehouseToWaste.Add(wt);
                    db.SaveChanges();
                    //reset form
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();

                    int count = db.WarehouseToWaste.Where(w => w.CreateDate.Year == DateTime.Now.Year && w.CreateDate.Month == DateTime.Now.Month).Count() + 1;
                    string code = MyConstant.PrefixForGenerateCode.WHToWaste + Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(count, 4);
                    textBoxCode.Text = code;
                    textBoxDate.Text = Library.ConvertDateToThaiDate(DateTime.Now);

                    textBoxTotalBalance.Text = "0.00";
                    textBoxTotalQty.Text = "0.00";
                    // Update Stock Wms
                    //Library.MakeValueForUpdateStockWms(details);
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
                    Library.AddWasteWarehouse(details, wt.DocNo);
                    /// ต้องไปตัด stock ระบบจารกอต โดยใช้ บาร์โค้ด

                    dataGridView1.Rows.Add(1);
                    textBoxRemark.Text = "";
                    textBoxRemark.Select();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("พบข้อผิดพลาด " + ex.ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
