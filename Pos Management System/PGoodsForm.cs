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
    public partial class PGoodsForm : Form
    {
        public PGoodsForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        int colCode = 0;
        int colSearch = 1;
        int colName = 2;
        int colpz = 3;
        int colUnit = 4;
        int colPrice = 5;
        int colVatType = 6;
        int colCostOnly = 7;
        int colCostAndVat = 8;
        int colQty = 9;
        int colFKId = 10;
        private void PGoodsForm_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        int row;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.row = e.RowIndex;
                switch (e.ColumnIndex)
                {
                    case 1:
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
        int _FKProDtl = 0;
        public void BinddingProduct(int id)
        {
            //_FKProDtl = id;
            List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
            var product = products.SingleOrDefault(w => w.Id == id);
            // start bind ui
            int row = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows[row].Cells[colCode].Value = product.Code;
            dataGridView1.Rows[row].Cells[colName].Value = product.Products.ThaiName;
            dataGridView1.Rows[row].Cells[colUnit].Value = product.ProductUnit.Name;
            dataGridView1.Rows[row].Cells[colpz].Value = product.PackSize;
            dataGridView1.Rows[row].Cells[colQty].Value = 1.00;
            dataGridView1.Rows[row].Cells[colCostOnly].Value = Library.ConvertDecimalToStringForm(product.CostOnly);
            dataGridView1.Rows[row].Cells[colPrice].Value = Library.ConvertDecimalToStringForm(product.SellPrice);
            dataGridView1.Rows[row].Cells[colVatType].Value = product.Products.ProductVatType.Name;
            dataGridView1.Rows[row].Cells[colCostAndVat].Value = Library.ConvertDecimalToStringForm(product.CostAndVat);
            dataGridView1.Rows[row].Cells[colFKId].Value = product.Id;

            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
            dataGridView1.BeginEdit(true);
            TotalSummary();
        }
        void TotalSummary()
        {
            decimal sumCost = 0;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var getBarcode = dataGridView1.Rows[i].Cells[colCode].Value;
                if (getBarcode == null)
                {
                    continue;
                }
                else if (getBarcode.ToString() == "")
                {
                    continue;
                }
                decimal qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                decimal cost = decimal.Parse(dataGridView1.Rows[i].Cells[colCostAndVat].Value.ToString());
                sumCost = sumCost + (qty * cost);
            }
            decimal profitBath = decimal.Parse(textBoxSellPrice.Text) - sumCost;
            textBoxProfitBath.Text = Library.ConvertDecimalToStringForm(profitBath);
            textBoxTotalCost.Text = Library.ConvertDecimalToStringForm(sumCost);
            string percent = Library.CalMonneyPercent(decimal.Parse(textBoxSellPrice.Text), sumCost);
            textBoxProfit.Text = percent;
        }
        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            int col = dataGridView1.CurrentCell.ColumnIndex;
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (col == colSearch)
                    {
                        //SelectedProductPopup obj = new SelectedProductPopup(this);
                        //obj.ShowDialog();
                    }
                    else if (col == colQty)
                    {
                        TotalSummary();
                        if (row == dataGridView1.Rows.Count - 1)
                        {
                            var getBarcode = dataGridView1.Rows[row].Cells[colCode].Value;
                            if (getBarcode == null)
                            {
                                return;
                            }
                            else if (getBarcode.ToString() == "")
                            {
                                return;
                            }
                            //row = row - 1;
                            dataGridView1.Rows.Add(1);
                            //decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                            //decimal costOnly = decimal.Parse(dataGridView1.Rows[row].Cells[colCostPerUnit].Value.ToString());
                            //dataGridView1.Rows[row].Cells[colTotalCost].Value = Library.ConvertDecimalToStringForm(qty * costOnly);

                            dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colSearch];
                            dataGridView1.BeginEdit(true);

                        }
                        else
                        {
                            //decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                            //decimal costOnly = decimal.Parse(dataGridView1.Rows[row].Cells[colCostPerUnit].Value.ToString());
                            //dataGridView1.Rows[row].Cells[colTotalCost].Value = Library.ConvertDecimalToStringForm(qty * costOnly);
                        }

                    }
                    break;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.row = e.RowIndex;
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.row = e.RowIndex;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.row = e.RowIndex;
        }
        /// <summary>
        /// ลบสินค้าที่ไม่ต้องการ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            TotalSummary();
        }
        /// <summary>
        /// ค้นหาสินค้าแปรรูป
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            ProcessingGoodsPopup obj = new ProcessingGoodsPopup(this);
            obj.ShowDialog();
        }
        /// <summary>
        /// หลังจากเลือกสินค้าแปรรูป
        /// </summary>
        /// <param name="id"></param>
        int _QTY = 0;
        int _SaveChangeType = 0;
        public void BinddingProcessingGoods(int id)
        {
            _FKProDtl = id;
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.ProductDetails.SingleOrDefault(w => w.Id == id && w.Enable == true);
                textBoxGoodsCode.Text = data.Code;
                textBoxGoodsName.Text = data.Products.ThaiName;
                textBoxDocName.Text = MyConstant.PrefixForGenerateCode.ProcessGoods + data.Code;
                textBoxSellPrice.Text = Library.ConvertDecimalToStringForm(data.SellPrice);
                labelUnitName.Text = data.ProductUnit.Name;
            }
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            var getResult = Library.GetQueryยอดยกมา(DateTime.Now.AddDays(1).ToString("yyyyMMdd"), textBoxGoodsCode.Text, textBoxGoodsCode.Text);
            textBoxQty.Text = (int)decimal.Parse(getResult.FirstOrDefault().qty) + "";
            _QTY = (int)decimal.Parse(getResult.FirstOrDefault().qty);
            // binding when fat data
            using (SSLsEntities db = new SSLsEntities())
            {
                List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
                var getPGoods = db.ProcessedGoods.Where(w => w.FKProductDtl == id && w.Enable == true).ToList();
                if (getPGoods.Count() > 0)
                {
                    linkLabel1.Visible = true;
                    _SaveChangeType = 1; // ต้องอัพเดทยอดแปรรูป
                    // แปลว่า ตั้งค่าแล้ว
                    textBoxQty.Enabled = false;
                    textBoxDesc.Text = getPGoods.FirstOrDefault().Description;
                    foreach (var item in getPGoods)
                    {
                        var getBarcode = products.SingleOrDefault(w => w.Code == item.Barcode);
                        dataGridView1.Rows.Add
                            (
                            item.Barcode,
                            "",
                            getBarcode.Products.ThaiName,
                                   item.PackSize,
                            getBarcode.ProductUnit.Name,
                            Library.ConvertDecimalToStringForm(item.SellPrice),
                            getBarcode.Products.ProductVatType.Name,
                            Library.ConvertDecimalToStringForm(item.CostOnly),
                            Library.ConvertDecimalToStringForm(item.CostAndVat),
                            Library.ConvertDecimalToStringForm(item.Qty),
                            getBarcode.Id
                            );
                    }
                }
                else
                {
                    dataGridView1.Rows.Add();
                }
            }
            TotalSummary();
            //dataGridView1.Rows.Add();
        }
        /// <summary>
        /// ยืนยันบันทึก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    //Console.WriteLine(textBoxTotalCost.Text);
                    if (_SaveChangeType == 0)
                    {
                        SaveCommit();
                    }
                    else if (_SaveChangeType == 1)
                    {
                        // check ว่าใครหายไป
                        ReturnToStock();
                        // check การเพิ่ม ลด วัตถุดิบ ต้องไล่คืน ให้หมดก่อน
                        CheckAddNew();
                        // check ว่า เพิ่มเติมหรือไม่
                        int newQty = int.Parse(textBoxQty.Text) - _QTY;
                        if (newQty > 0)
                        {
                            // กระทำกับกับ _QTY
                            QtyEdit(MyConstant.PosTransaction.AddProcessingGoods, Math.Abs(newQty), MyConstant.PosTransaction.ProcessingGoods);
                        }
                        else if (newQty < 0)
                        {
                            QtyEdit(MyConstant.PosTransaction.MinusProcessingGoods, Math.Abs(newQty), MyConstant.PosTransaction.CancelProcessingGoods);
                        }
                    }
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        var data = db.ProductDetails.SingleOrDefault(w => w.Id == _FKProDtl);
                        
                        decimal totalcost = decimal.Parse(textBoxTotalCost.Text);
                        if (data.CostAndVat != totalcost)
                        {
                            // update now
                            data.CostAndVat = totalcost;
                            data.CostVat = Library.CalVatFromValue(data.CostAndVat);
                            data.CostOnly = data.CostAndVat - data.CostVat;
                            db.Entry(data).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    this.Dispose();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void ReturnToStock()
        {
            List<string> barcodeOnGrid = new List<string>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var getBarcode = dataGridView1.Rows[i].Cells[colCode].Value;
                if (getBarcode == null)
                {
                    continue;
                }
                else if (getBarcode.ToString() == "")
                {
                    continue;
                }
                barcodeOnGrid.Add(getBarcode.ToString());
            }
            using (SSLsEntities db = new SSLsEntities())
            {
                List<StoreFrontStockDetails> details = new List<StoreFrontStockDetails>();
                var getPGoods = db.ProcessedGoods.Where(w => w.FKProductDtl == _FKProDtl && w.Enable == true).ToList();
                foreach (var item in getPGoods)
                {
                    if (barcodeOnGrid.FirstOrDefault(w => w == item.Barcode) != null)
                    {
                        // ยังยุ่
                    }
                    else
                    {
                        string desc = textBoxDesc.Text;
                        string docno = textBoxDocName.Text;
                        List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
                        var getBarcode = products.FirstOrDefault(w => w.Code == item.Barcode);
                        var stockHD = db.StoreFrontStock.SingleOrDefault(w => w.FKProduct == getBarcode.FKProduct && w.Enable == true);
                        // ไม่ยุ่แล้ว
                        item.Enable = false;
                        // คืน stock ทั้งหมด
                        StoreFrontStockDetails detail = new StoreFrontStockDetails();
                        detail.DocNo = docno;
                        detail.DocDtlNumber = 0;
                        detail.Description = "";
                        detail.CreateDate = DateTime.Now;
                        detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        detail.UpdateDate = DateTime.Now;
                        detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        detail.Enable = true;
                        detail.ActionQty = (item.Qty * item.PackSize) * _QTY; // ยอดคงเหลือ
                        detail.FKStoreFrontStock = stockHD.Id;
                        detail.FKTransactionType = MyConstant.PosTransaction.CancelProcessingGoods;
                        detail.Barcode = item.Barcode;
                        detail.Name = products.SingleOrDefault(w => w.Id == getBarcode.Id).Products.ThaiName;
                        detail.FKProductDetails = getBarcode.Id;
                        detail.ResultQty = 0;
                        detail.PackSize = 1;
                        detail.CostOnlyPerUnit = products.SingleOrDefault(w => w.Id == getBarcode.Id).CostOnly;
                        detail.SellPricePerUnit = products.SingleOrDefault(w => w.Id == getBarcode.Id).SellPrice;
                        details.Add(detail);
                    }
                }
                db.StoreFrontStockDetails.AddRange(details);
                db.SaveChanges();
            }

        }
        /// <summary>
        /// คืน stock กรณีลด รายการ
        /// </summary>
        private void CheckAddNew()
        {
            List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
            using (SSLsEntities db = new SSLsEntities())
            {
                List<StoreFrontStockDetails> details = new List<StoreFrontStockDetails>();
                var getPGoods = db.ProcessedGoods.Where(w => w.FKProductDtl == _FKProDtl && w.Enable == true).ToList();
                List<ProcessedGoods> lists = new List<ProcessedGoods>();
                ProcessedGoods obj;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var getBarcode = dataGridView1.Rows[i].Cells[colCode].Value;
                    if (getBarcode == null)
                    {
                        continue;
                    }
                    else if (getBarcode.ToString() == "")
                    {
                        continue;
                    }
                    var detect = getPGoods.FirstOrDefault(w => w.Barcode == getBarcode.ToString());
                    if (detect != null)
                    {
                        // แสดงว่าเป็นบาร์โค้ดเดิม
                    }
                    else
                    {
                        // แสดงว่า เพิ่มมาใหม่
                        obj = new ProcessedGoods();
                        obj.FKProductDtl = _FKProDtl;
                        obj.Barcode = getBarcode.ToString();
                        obj.CreateDate = DateTime.Now;
                        obj.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        obj.UpdateDate = DateTime.Now;
                        obj.UpdateBy = Singleton.SingletonAuthen.Instance().Id;

                        decimal qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                        decimal costOnly = decimal.Parse(dataGridView1.Rows[i].Cells[colCostOnly].Value.ToString());
                        decimal costAndVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostAndVat].Value.ToString());
                        decimal packsize = decimal.Parse(dataGridView1.Rows[i].Cells[colpz].Value.ToString());
                        decimal sellprice = decimal.Parse(dataGridView1.Rows[i].Cells[colPrice].Value.ToString());
                        obj.CostOnly = costOnly;
                        obj.CostAndVat = costAndVat;
                        obj.CostVat = costAndVat - costOnly;
                        obj.PackSize = packsize;
                        obj.SellPrice = sellprice;
                        obj.Qty = qty;
                        obj.Description = textBoxDesc.Text;
                        obj.Enable = true;
                        obj.DocumentNo = textBoxDocName.Text;
                        lists.Add(obj);

                        string desc = textBoxDesc.Text;
                        string docno = textBoxDocName.Text;

                        var getGoodfromSing = products.FirstOrDefault(w => w.Code == obj.Barcode);
                        var stockHD = db.StoreFrontStock.SingleOrDefault(w => w.FKProduct == getGoodfromSing.FKProduct && w.Enable == true);

                        if (stockHD == null)
                        {
                            StoreFrontStock hd = new StoreFrontStock();
                            hd.Description = "-";
                            hd.CreateDate = DateTime.Now;
                            hd.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                            hd.UpdateDate = DateTime.Now;
                            hd.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            hd.Enable = true;
                            hd.CurrentQty = 0;
                            hd.FKProduct = getGoodfromSing.FKProduct;
                            db.StoreFrontStock.Add(hd);
                            db.SaveChanges();
                            stockHD = db.StoreFrontStock.SingleOrDefault(w => w.FKProduct == getGoodfromSing.FKProduct && w.Enable == true);
                        }
                        // ตัด stock
                        StoreFrontStockDetails detail = new StoreFrontStockDetails();
                        detail.DocNo = docno;
                        detail.DocDtlNumber = i + 1;
                        detail.Description = "";
                        detail.CreateDate = DateTime.Now;
                        detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        detail.UpdateDate = DateTime.Now;
                        detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        detail.Enable = true;
                        detail.ActionQty = (qty * packsize) * _QTY; // ยอดคงเหลือ
                        detail.FKStoreFrontStock = stockHD.Id;
                        detail.FKTransactionType = MyConstant.PosTransaction.ProcessingGoods;
                        detail.Barcode = obj.Barcode;
                        detail.Name = products.SingleOrDefault(w => w.Id == getGoodfromSing.Id).Products.ThaiName;
                        detail.FKProductDetails = getGoodfromSing.Id;
                        detail.ResultQty = 0;
                        detail.PackSize = 1;
                        detail.CostOnlyPerUnit = products.SingleOrDefault(w => w.Id == getGoodfromSing.Id).CostOnly;
                        detail.SellPricePerUnit = products.SingleOrDefault(w => w.Id == getGoodfromSing.Id).SellPrice;
                        details.Add(detail);
                    }
                }
                // add to process goods
                db.StoreFrontStockDetails.AddRange(details);
                db.ProcessedGoods.AddRange(lists);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// เพิ่ม = เพิ่มแปรรูป ลด stock รานการย่อย
        /// ลด = เพิ่ม stock รายการย่อย ลดแปรรูป
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="newQty"></param>
        /// <param name="transactions2"></param>
        void QtyEdit(int transactions, int newQty, int transactions2)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
                int idHD = products.SingleOrDefault(w => w.Id == _FKProDtl).Products.Id;
                var stockHD = db.StoreFrontStock.SingleOrDefault(w => w.FKProduct == idHD && w.Enable == true);

                List<StoreFrontStockDetails> details = new List<StoreFrontStockDetails>();
                StoreFrontStockDetails detail = new StoreFrontStockDetails();
                detail.DocNo = textBoxDocName.Text;
                detail.DocDtlNumber = 0;
                detail.Description = "";
                detail.CreateDate = DateTime.Now;
                detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                detail.UpdateDate = DateTime.Now;
                detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                detail.Enable = true;
                detail.ActionQty = newQty;
                detail.FKStoreFrontStock = stockHD.Id;
                detail.FKTransactionType = transactions;
                detail.Barcode = textBoxGoodsCode.Text;
                detail.Name = products.SingleOrDefault(w => w.Id == _FKProDtl).Products.ThaiName;
                detail.FKProductDetails = _FKProDtl;
                detail.ResultQty = 0;
                detail.PackSize = 1;
                detail.CostOnlyPerUnit = products.SingleOrDefault(w => w.Id == _FKProDtl).CostOnly;
                detail.SellPricePerUnit = products.SingleOrDefault(w => w.Id == _FKProDtl).SellPrice;
                details.Add(detail);

                //ProcessedGoods procGoods;
                //List<ProcessedGoods> procList = new List<ProcessedGoods>();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //procGoods = new ProcessedGoods();

                    detail = new StoreFrontStockDetails();
                    var getBarcode = dataGridView1.Rows[i].Cells[colCode].Value;
                    if (getBarcode == null)
                    {
                        continue;
                    }
                    else if (getBarcode.ToString() == "")
                    {
                        continue;
                    }
                    decimal qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    decimal costOnly = decimal.Parse(dataGridView1.Rows[i].Cells[colCostOnly].Value.ToString());
                    decimal costAndVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostAndVat].Value.ToString());
                    decimal packsize = decimal.Parse(dataGridView1.Rows[i].Cells[colpz].Value.ToString());
                    decimal sellprice = decimal.Parse(dataGridView1.Rows[i].Cells[colPrice].Value.ToString());
                    int id = int.Parse(dataGridView1.Rows[i].Cells[colFKId].Value.ToString());

                    // ตัด Stock
                    int idHDForProd = products.SingleOrDefault(w => w.Id == id).Products.Id; // fkproduct
                    var getStockHD = db.StoreFrontStock.SingleOrDefault(w => w.FKProduct == idHDForProd && w.Enable == true);
                    if (getStockHD == null)
                    {
                        AddStockHD(idHDForProd);
                        getStockHD = db.StoreFrontStock.SingleOrDefault(w => w.FKProduct == idHDForProd && w.Enable == true);
                    }
                    detail.DocNo = textBoxDocName.Text;
                    detail.DocDtlNumber = (i + 1);
                    detail.Description = "";
                    detail.CreateDate = DateTime.Now;
                    detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.UpdateDate = DateTime.Now;
                    detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.Enable = true;
                    detail.ActionQty = (qty * packsize) * newQty;
                    detail.FKStoreFrontStock = getStockHD.Id;
                    detail.FKTransactionType = transactions2;
                    detail.Barcode = getBarcode.ToString();
                    detail.Name = products.SingleOrDefault(w => w.Id == id).Products.ThaiName;
                    detail.FKProductDetails = id;
                    detail.ResultQty = 0;
                    detail.PackSize = packsize;
                    detail.CostOnlyPerUnit = costOnly;
                    detail.SellPricePerUnit = sellprice;
                    details.Add(detail);
                }

                // add to stock
                db.StoreFrontStockDetails.AddRange(details);
                db.SaveChanges();
            }

        }

        private void SaveCommit()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                //var ss = textBoxTotalCost.Text;
                // 1. เพิ่มใหม่
                List<ProcessedGoods> lists = new List<ProcessedGoods>();
                ProcessedGoods obj;
                string desc = textBoxDesc.Text;
                string docno = textBoxDocName.Text;
                List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
                int idHD = products.SingleOrDefault(w => w.Id == _FKProDtl).Products.Id;
                var stockHD = db.StoreFrontStock.SingleOrDefault(w => w.FKProduct == idHD && w.Enable == true);
                if (stockHD == null)
                {
                    StoreFrontStock hd = new StoreFrontStock();
                    hd.Description = "-";
                    hd.CreateDate = DateTime.Now;
                    hd.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    hd.UpdateDate = DateTime.Now;
                    hd.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    hd.Enable = true;
                    hd.CurrentQty = 0;
                    hd.FKProduct = idHD;
                    db.StoreFrontStock.Add(hd);
                    db.SaveChanges();
                    stockHD = db.StoreFrontStock.SingleOrDefault(w => w.FKProduct == idHD && w.Enable == true);
                }
                List<StoreFrontStockDetails> details = new List<StoreFrontStockDetails>();
                StoreFrontStockDetails detail = new StoreFrontStockDetails();
                detail.DocNo = docno;
                detail.DocDtlNumber = 0;
                detail.Description = "";
                detail.CreateDate = DateTime.Now;
                detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                detail.UpdateDate = DateTime.Now;
                detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                detail.Enable = true;
                detail.ActionQty = decimal.Parse(textBoxQty.Text);
                detail.FKStoreFrontStock = stockHD.Id;
                detail.FKTransactionType = MyConstant.PosTransaction.SetStock;
                detail.Barcode = textBoxGoodsCode.Text;
                detail.Name = products.SingleOrDefault(w => w.Id == _FKProDtl).Products.ThaiName;
                detail.FKProductDetails = _FKProDtl;
                detail.ResultQty = 0;
                detail.PackSize = 1;
                detail.CostOnlyPerUnit = products.SingleOrDefault(w => w.Id == _FKProDtl).CostOnly;
                detail.SellPricePerUnit = products.SingleOrDefault(w => w.Id == _FKProDtl).SellPrice;
                details.Add(detail);

                //ProcessedGoods procGoods;
                //List<ProcessedGoods> procList = new List<ProcessedGoods>();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //procGoods = new ProcessedGoods();
                    obj = new ProcessedGoods();
                    detail = new StoreFrontStockDetails();
                    var getBarcode = dataGridView1.Rows[i].Cells[colCode].Value;
                    if (getBarcode == null)
                    {
                        continue;
                    }
                    else if (getBarcode.ToString() == "")
                    {
                        continue;
                    }
                    decimal qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                    decimal costOnly = decimal.Parse(dataGridView1.Rows[i].Cells[colCostOnly].Value.ToString());
                    decimal costAndVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostAndVat].Value.ToString());
                    decimal packsize = decimal.Parse(dataGridView1.Rows[i].Cells[colpz].Value.ToString());
                    decimal sellprice = decimal.Parse(dataGridView1.Rows[i].Cells[colPrice].Value.ToString());
                    int id = int.Parse(dataGridView1.Rows[i].Cells[colFKId].Value.ToString());
                    obj.FKProductDtl = _FKProDtl;
                    obj.Barcode = getBarcode.ToString();
                    obj.CreateDate = DateTime.Now;
                    obj.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    obj.UpdateDate = DateTime.Now;
                    obj.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    obj.CostOnly = costOnly;
                    obj.CostAndVat = costAndVat;
                    obj.CostVat = costAndVat - costOnly;
                    obj.PackSize = packsize;
                    obj.SellPrice = sellprice;
                    obj.Qty = qty;
                    obj.Description = desc;
                    obj.Enable = true;
                    obj.DocumentNo = docno;
                    lists.Add(obj);
                    // ตัด Stock
                    int idHDForProd = products.SingleOrDefault(w => w.Id == id).Products.Id; // fkproduct
                    var getStockHD = db.StoreFrontStock.SingleOrDefault(w => w.FKProduct == idHDForProd && w.Enable == true);
                    if (getStockHD == null)
                    {
                        AddStockHD(idHDForProd);
                        getStockHD = db.StoreFrontStock.SingleOrDefault(w => w.FKProduct == idHDForProd && w.Enable == true);
                    }
                    detail.DocNo = docno;
                    detail.DocDtlNumber = (i + 1);
                    detail.Description = "";
                    detail.CreateDate = DateTime.Now;
                    detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.UpdateDate = DateTime.Now;
                    detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.Enable = true;
                    detail.ActionQty = (qty * packsize) * decimal.Parse(textBoxQty.Text);
                    detail.FKStoreFrontStock = getStockHD.Id;
                    detail.FKTransactionType = MyConstant.PosTransaction.ProcessingGoods;
                    detail.Barcode = getBarcode.ToString();
                    detail.Name = products.SingleOrDefault(w => w.Id == id).Products.ThaiName;
                    detail.FKProductDetails = id;
                    detail.ResultQty = 0;
                    detail.PackSize = packsize;
                    detail.CostOnlyPerUnit = costOnly;
                    detail.SellPricePerUnit = sellprice;
                    details.Add(detail);
                }
                // add to process goods
                db.ProcessedGoods.AddRange(lists);
                // add to stock
                db.StoreFrontStockDetails.AddRange(details);
                db.SaveChanges();    
            }
        }
        void AddStockHD(int id)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                StoreFrontStock hd = new StoreFrontStock();
                hd.Description = "-";
                hd.CreateDate = DateTime.Now;
                hd.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                hd.UpdateDate = DateTime.Now;
                hd.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                hd.Enable = true;
                hd.CurrentQty = 0;
                hd.FKProduct = id;
                db.StoreFrontStock.Add(hd);
                db.SaveChanges();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBoxQty.Enabled = true;
            textBoxQty.Select();
            textBoxQty.SelectAll();

        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            TotalSummary();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int row = this.row;

            DialogResult dr = MessageBox.Show("คุณต้องการลบรายการ ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    dataGridView1.Rows.RemoveAt(row);
                    //int id = int.Parse(dataGridView1.Rows[row].Cells[colFKId].Value.ToString());
                    //for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    //{
                    //    var getBarcode = dataGridView1.Rows[i].Cells[colCode].Value;
                    //    if (getBarcode == null)
                    //    {
                    //        continue;
                    //    }
                    //    else if (getBarcode.ToString() == "")
                    //    {
                    //        continue;
                    //    }

                    //    dataGridView1.Rows[row].
                    //}
                    TotalSummary();
                    if (dataGridView1.Rows.Count == 0)
                    {
                        dataGridView1.Rows.Add();
                    }
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการลบรายการ ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    CancelGoods();
                    this.Dispose();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void CancelGoods()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                StoreFrontStockDetails detail;
                List<StoreFrontStockDetails> details = new List<StoreFrontStockDetails>();
                var getPGoods = db.ProcessedGoods.Where(w => w.FKProductDtl == _FKProDtl && w.Enable == true).ToList();
                foreach (var item in getPGoods)
                {
       
                    string docno = textBoxDocName.Text;
                    List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
                    var getBarcode = products.FirstOrDefault(w => w.Code == item.Barcode);
                    var stockHD = db.StoreFrontStock.SingleOrDefault(w => w.FKProduct == getBarcode.FKProduct && w.Enable == true);
                    // ไม่ยุ่แล้ว
                    item.Enable = false;
                    // คืน stock ทั้งหมด
                    detail = new StoreFrontStockDetails();
                    detail.DocNo = docno;
                    detail.DocDtlNumber = 0;
                    detail.Description = "";
                    detail.CreateDate = DateTime.Now;
                    detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.UpdateDate = DateTime.Now;
                    detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    detail.Enable = true;
                    detail.ActionQty = (item.Qty * item.PackSize) * _QTY; // ยอดคงเหลือ
                    detail.FKStoreFrontStock = stockHD.Id;
                    detail.FKTransactionType = MyConstant.PosTransaction.CancelProcessingGoods;
                    detail.Barcode = item.Barcode;
                    detail.Name = products.SingleOrDefault(w => w.Id == getBarcode.Id).Products.ThaiName;
                    detail.FKProductDetails = getBarcode.Id;
                    detail.ResultQty = 0;
                    detail.PackSize = 1;
                    detail.CostOnlyPerUnit = products.SingleOrDefault(w => w.Id == getBarcode.Id).CostOnly;
                    detail.SellPricePerUnit = products.SingleOrDefault(w => w.Id == getBarcode.Id).SellPrice;
                    details.Add(detail);
                    db.Entry(item).State = EntityState.Modified;
                }
                List<ProductDetails> products1 = Singleton.SingletonProduct.Instance().ProductDetails;
                var getBarcode1 = products1.FirstOrDefault(w => w.Code == textBoxGoodsCode.Text);
                var stockHD1 = db.StoreFrontStock.SingleOrDefault(w => w.FKProduct == getBarcode1.FKProduct && w.Enable == true);
                detail = new StoreFrontStockDetails();
                detail.DocNo = textBoxDocName.Text;
                detail.DocDtlNumber = 0;
                detail.Description = "";
                detail.CreateDate = DateTime.Now;
                detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                detail.UpdateDate = DateTime.Now;
                detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                detail.Enable = true;
                detail.ActionQty =  _QTY; // ยอดคงเหลือ
                detail.FKStoreFrontStock = stockHD1.Id;
                detail.FKTransactionType = MyConstant.PosTransaction.CancelProcessingGoods;
                detail.Barcode = textBoxGoodsCode.Text;
                detail.Name = products1.SingleOrDefault(w => w.Id == getBarcode1.Id).Products.ThaiName;
                detail.FKProductDetails = getBarcode1.Id;
                detail.ResultQty = 0;
                detail.PackSize = 1;
                detail.CostOnlyPerUnit = products1.SingleOrDefault(w => w.Id == getBarcode1.Id).CostOnly;
                detail.SellPricePerUnit = products1.SingleOrDefault(w => w.Id == getBarcode1.Id).SellPrice;
                details.Add(detail);

                db.StoreFrontStockDetails.AddRange(details);
                db.SaveChanges();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("กรุณาเปิดเมนู สร้างสินค้าแปรรูปใหม่");
            this.Dispose();            
        }
    }
}
