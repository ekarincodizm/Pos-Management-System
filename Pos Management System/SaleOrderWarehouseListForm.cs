using Pos_Management_System.Model;
using Pos_Management_System.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class SaleOrderWarehouseListForm : Form
    {
        public SaleOrderWarehouseListForm()
        {
            InitializeComponent();
        }
        int colDocNo = 2;
        int colCheckAll = 11;
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
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
                    this.dataGridView1[colCheckAll, j].Value = true;
                }
            }
            else
            {
                for (int j = 0; j <= this.dataGridView1.RowCount - 1; j++)
                {
                    this.dataGridView1[colCheckAll, j].Value = false;
                }
            }
        }
        PhysicalAddressPC _pc;
        private void SaleOrderWarehouseListForm_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                this._pc = PhysicalAddressPOS.ShowNetworkInterfaces();
                var b = SingletonAuthen.Instance().MyBranch.Id;
                var pcs = db.PosMachine.Where(w => w.Enable == true && w.FKBranch == b).ToList();
                //var checkPC = pcs.FirstOrDefault(w => w.EthernetPhysicalAddress == pc.EthernetAddress || w.WirelessPhysicalAddress == pc.WirelessAddress && w.Enable == true && w.FKBranch == b);
                var checkPC = pcs.FirstOrDefault(w => w.ComputerName == _pc.ComputerName && w.Enable == true && w.FKBranch == b);
                if (checkPC != null)
                {
                    // แสดงว่า เครื่อง เคยบันทึกการตั้งค่าแล้ว
                    _posMachine = checkPC;
                }
                else
                {
                    MessageBox.Show("ไม่พบการตั้งค่าหมายเลขเครื่อง กรุณาติดต่อ Admin");
                }
            }

            DataGridViewCheckBoxColumn ColumnCheckBox = new DataGridViewCheckBoxColumn();
            ColumnCheckBox.Width = width_columcheckbox;
            ColumnCheckBox.DataPropertyName = "Select";
            //dataGridView1.Columns.Add(ColumnCheckBox);
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(colCheckAll, -1, true);
            ckBox.Size = new Size(14, 14);
            rect.X = rect.Location.X + (rect.Width / 2) - (ckBox.Width / 2);
            rect.Y += 3;
            ckBox.Location = rect.Location;
            this.ckBox.CheckedChanged += new EventHandler(this.ckBox_CheckedChanged);
            dataGridView1.Controls.Add(ckBox);
            dataGridView1.Columns[colCheckAll].Frozen = false;

            LoadGrid();
        }
        /// <summary>
        /// โหลด Grid
        /// </summary>
        public void LoadGrid()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                List<SaleOrderWarehouse> list = new List<SaleOrderWarehouse>();
                list = db.SaleOrderWarehouse.Where(w =>
                DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.CreateDate).ToList();

                // check filter
                if (checkBoxConfirm.Checked == false)
                {
                    // ตัดออก
                    list = list.Where(w => w.FKSaleOrderWarehouseStatus != MyConstant.SaleOrderWarehouseStatus.ConfirmOrder).ToList();
                }
                if (checkBoxCreate.Checked == false)
                {
                    // รดยืนยัน
                    list = list.Where(w => w.FKSaleOrderWarehouseStatus != MyConstant.SaleOrderWarehouseStatus.CreateOrder).ToList();
                }
                if (checkBoxDisable.Checked == false)
                {
                    // ยกเลิกแล้ว
                    list = list.Where(w => w.FKSaleOrderWarehouseStatus != MyConstant.SaleOrderWarehouseStatus.CancelOrder).ToList();
                }
                decimal total = 0;
                foreach (var item in list)
                {
                    if (_User != null)
                    {
                        if (item.CreateBy != _User)
                        {
                            continue;
                        }
                    }
                    string status = "";
                    if (item.Enable == false)
                    {
                        status = "ยกเลิกออร์เดอร์";
                    }
                    else
                    {
                        status = item.SaleOrderWarehouseStatus.Name;
                    }
                    dataGridView1.Rows.Add(
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Code,
                        item.Debtor.Name,
                        item.Member.Name,
                        item.PaymentType.Name,
                        item.DeliveryType.Name,
                        item.SaleOrderWarehouseDtl.Where(w => w.Enable == true).ToList().Count(),
                        Library.ConvertDecimalToStringForm(item.TotalBalance),
                        status,
                        item.Description
                        );
                    total += item.TotalBalance;
                }
                textBoxQtyList.Text = list.Count() + "";
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(total);

            }
        }
        /// <summary>
        /// ค้นหา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// ค้นหา 1 ออร์เดอร์
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlus_Click(object sender, EventArgs e)
        {
            SearchOrder();
        }

        private void SearchOrder()
        {
            string code = textBoxKeySearch.Text.Trim();
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                List<SaleOrderWarehouse> list = new List<SaleOrderWarehouse>();
                list = db.SaleOrderWarehouse.Where(w => w.Code == code).OrderBy(w => w.CreateDate).ToList();

                decimal total = 0;
                foreach (var item in list)
                {
                    string status = "";
                    if (item.Enable == false)
                    {
                        status = "ยกเลิกออร์เดอร์";
                    }
                    else
                    {
                        status = item.SaleOrderWarehouseStatus.Name;
                    }
                    dataGridView1.Rows.Add(
                        Library.ConvertDateToThaiDate(item.CreateDate),
                        Library.GetFullNameUserById(item.CreateBy),
                        item.Code,
                        item.Debtor.Name,
                        item.Member.Name,
                        item.PaymentType.Name,
                        item.DeliveryType.Name,
                        item.SaleOrderWarehouseDtl.Where(w => w.Enable == true).ToList().Count(),
                        Library.ConvertDecimalToStringForm(item.TotalBalance),
                        status,
                        item.Description
                        );
                    total += item.TotalBalance;
                }
                textBoxQtyList.Text = list.Count() + "";
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(total);
            }
        }

        /// <summary>
        /// แก้ไข ออร์เดอร์  Click 1 Row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
            SaleOrderWarehouseEditForm obj = new SaleOrderWarehouseEditForm(this, code);
            obj.ShowDialog();
        }
        /// <summary>
        /// ยืนยัน หลังจากยืนยันออร์เดอร์ จะห้ามแก้ไข รายการ คลังจะมองเห็น ออร์เดอร์ เพื่อ TakeOrder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            var stackDocNo = GetCodeOrderSelected();
            using (SSLsEntities db = new SSLsEntities())
            {
                foreach (var item in stackDocNo)
                {
                    var order = db.SaleOrderWarehouse.SingleOrDefault(w => w.Code == item);
                    if (order.FKSaleOrderWarehouseStatus == MyConstant.SaleOrderWarehouseStatus.CreateOrder && order.Enable == true)
                    {

                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถยืนยันรายการนี้ได้ " + item + " กรุณาติ๊กออก");
                        return;
                    }
                }
            }

            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?", "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    ConfirmOrder(stackDocNo);
                    break;
                case DialogResult.No:
                    break;
            }
        }
        PosMachine _posMachine;
        /// <summary>
        /// หลังจากยืนยัน จะต้อง add เข้า pos ด้วย เพราะเป็นการจบการขาย เชื่อ
        /// </summary>
        /// <param name="stackDocNo"></param>
        private void ConfirmOrder(IEnumerable<string> stackDocNo)
        {
            SSLsEntities dbS = new SSLsEntities();
            try
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    PosHeader pos;
                    List<PosDetails> details;
                    foreach (var item in stackDocNo)
                    {
                        pos = new PosHeader();
                        details = new List<PosDetails>();

                        var order = db.SaleOrderWarehouse.Include("SaleOrderWarehouseDtl.ProductDetails.Products.ProductVatType").SingleOrDefault(w => w.Code == item && w.Enable == true && w.InvoiceNo == null && w.ConfirmOrderDate == null);
                        // call library tin tin
                        DataTable dataMap = Library.ConvertToDataTable(MapSaleOrderWHForLibrary(order));
                        //TradPickBigCar(string tb_cusno, string tb_sentno, string tb_po_distcount, string tb_statusno, int UserId, string tb_costall, string detail, DataTable dt, string invoice)
                        //TradPickBigCar(order.FKMember.ToString(), order.FKBranch.ToString(), 0 + "", "ยืนยัน", int.Parse(order.CreateBy), order.TotalBalance.ToString(), order.Description, dataMap, order.Code);

                        var ds = db.SaleOrderWarehouseDtl.Where(w => w.Enable == true & w.SaleOrderWarehouse.Enable == true &
                        w.ProductDetails.Enable == true & w.ProductDetails.Products.Enable == true &
                        w.SaleOrderWarehouse.InvoiceNoWH == null & w.SaleOrderWarehouse.Code == item)
                        .Select(s => new { PRODUCT_ID = s.ProductDetails.Products.FKRowID, PRODUCT_NO = s.ProductDetails.Code, PRODUCT_NAME = s.ProductDetails.Name, UNIT_NAME = s.ProductDetails.ProductUnit.Name, QTY = s.Qty, GIVEAWAY = "0", COST = s.PricePerUnit, DISCOUNT = (s.PercentDiscount + s.BathDiscount), COST_TOTAL = s.TotalPrice }).ToList();
                        DataTable dt = Library.ConvertToDataTable(ds);

                        foreach (DataRow row in dt.Rows)
                        {
                            Console.WriteLine(row["PRODUCT_NO"].ToString());
                        }
                        if (dt.Rows.Count > 0)
                        {
                            var dshd = db.SaleOrderWarehouseDtl.Where(w => w.Enable == true & w.SaleOrderWarehouse.Enable == true & w.ProductDetails.Enable == true & w.ProductDetails.Products.Enable == true & w.SaleOrderWarehouse.InvoiceNoWH == null & w.SaleOrderWarehouse.Code == item).FirstOrDefault();
                            if (dshd != null)
                            {
                                var check = Library.TradPickBigCar(dshd.SaleOrderWarehouse.Member.Code.ToString(), "1", "0", "1", int.Parse(dshd.SaleOrderWarehouse.CreateBy), dshd.SaleOrderWarehouse.TotalBeforeVat.ToString(), dshd.SaleOrderWarehouse.Description.ToString(), dt, dshd.SaleOrderWarehouse.Code.ToString());

                                if (check == "OK")
                                {
                                    Console.WriteLine(check);
                                }
                                else
                                {
                                    Console.WriteLine(check);
                                }
                            }
                        }

                        // call เสด ReCal
                        Recal(order.Code);
                        /// ถ้า ไป เชคกะ จารตินแล้ว ไม่ได้ของ จิงๆ ก็ คอนเฟิมไปนะแหละ    
                        List<SaleOrderWarehouseDtl> getNewList = dbS.SaleOrderWarehouseDtl.Where(w => w.Enable == true && w.FKSaleOrderWarehouse == order.Id).ToList();
                        if (getNewList.Sum(w => w.QtyAllow) == 0)
                        {
                            order.FKSaleOrderWarehouseStatus = MyConstant.SaleOrderWarehouseStatus.ConfirmOrder;
                            order.UpdateDate = DateTime.Now;
                            order.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            order.ConfirmOrderDate = DateTime.Now;
                            order.ConfirmOrderBy = Singleton.SingletonAuthen.Instance().Id;
                            db.Entry(order).State = EntityState.Modified;
                            db.SaveChanges();
                            return;
                        }
                        // Add To Pos   
                        pos.Qty = getNewList.Sum(w => w.QtyAllow);
                        pos.QtyList = getNewList.Count();
                        pos.TotalCost = 0;
                        pos.FKPosType = MyConstant.POsType.Creadit;
                        pos.FKPosMachine = _posMachine.Id;
                        pos.FKBudgetYear = Singleton.SingletonThisBudgetYear.Instance().ThisYear.Id;
                        pos.PrintSequence = 0;
                        foreach (var dtl in getNewList)
                        {
                            PosDetails dd = new PosDetails();
                            dd.Enable = true;
                            dd.Description = "รถใหญ่";
                            dd.CreateDate = DateTime.Now;
                            dd.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                            dd.UpdateDate = DateTime.Now;
                            dd.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            dd.FKProductDetails = dtl.FKProductDtl;
                            dd.Qty = dtl.QtyAllow;
                            dd.Cost = dtl.ProductDetails.CostAndVat;
                            dd.PricePerUnit = dtl.PricePerUnit;
                            dd.DiscountCoupon = 0;
                            dd.DiscountShop = dtl.BathDiscount;
                            if (dtl.QtyAllow < dd.Qty)
                            {
                                // ถ้าไม่ได้ตามเป้า 
                                dd.DiscountShop = 0;
                            }
                            dd.TotalPrice = dtl.TotalPrice;
                            if (dtl.ProductDetails.Products.ProductVatType.Id == MyConstant.ProductVatType.HasVat)
                            {
                                decimal removeVat = dd.TotalPrice * MyConstant.MyVat.Vat / MyConstant.MyVat.VatRemove;
                                dd.TotalBeforeVat = dd.TotalPrice - removeVat;
                                dd.TotalVat = removeVat;
                            }
                            else
                            {
                                dd.TotalBeforeVat = dd.TotalPrice;
                                dd.TotalVat = 0;
                            }
                            dd.IsPromotion = false;
                            details.Add(dd);
                        }
                        var posCode = Library.GetGeneratePosNo(this._posMachine);
                        pos.InvoiceNo = posCode;
                        pos.Enable = true;
                        pos.Description = order.Description;
                        pos.CreateDate = DateTime.Now;
                        pos.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        pos.UpdateDate = DateTime.Now;
                        pos.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        pos.FKMember = order.FKMember;
                        pos.FKDebtor = order.FKDebtor;
                        pos.TotalBalance = order.TotalBalance;
                        pos.PayDate = null;
                        pos.TotalVat = order.TotalVat;
                        pos.TotaNoVat = order.TotaNoVat;
                        pos.Total = order.Total;
                        pos.TotalDiscountCoupon = 0;
                        pos.TotalDiscountShop = 0;
                        pos.TotalBeforeVat = order.TotalBeforeVat;
                        pos.Discount = 0;
                        pos.Cash = order.TotalBalance;
                        pos.Change = 0;
                        pos.TotalDiscountShop = details.Sum(w => w.DiscountShop);
                        pos.PosDetails = details;
                        //pos.Total = getNewList.Sum(w => w.QtyAllow);
                        order.FKSaleOrderWarehouseStatus = MyConstant.SaleOrderWarehouseStatus.ConfirmOrder;
                        order.UpdateDate = DateTime.Now;
                        order.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        order.ConfirmOrderDate = DateTime.Now;
                        order.ConfirmOrderBy = Singleton.SingletonAuthen.Instance().Id;
                        order.InvoiceNo = posCode;

                        // add to post เพื่อขาย
                        db.PosHeader.Add(pos);
                        // อีพเดท สถาน
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    LoadGrid();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                dbS.Dispose();
            }

        }

        //private void TradPickBigCar(string v1, string v2, string v3, string v4, string createBy, decimal totalBalance, List<SaleOrderWHForLibrary> dataMap, string code)
        //{
        //    throw new NotImplementedException();
        //}

        private void TradPickBigCar(string tb_cusno, string tb_sentno, string tb_po_distcount, string tb_statusno, int UserId, string tb_costall, string detail, DataTable dt, string invoice)
        {

        }
        /// <summary>
        /// คำนวน Order ใหม่
        /// </summary>
        /// <param name="code"></param>
        private void Recal(string code)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.SaleOrderWarehouse.SingleOrDefault(w => w.Code == code);
                decimal totalHasVat = 0; // ยอดสินค้ามีภาษี
                decimal totalUnVat = 0; // ยอดสินค้ายกเว้นภาษี
                decimal totalDisInList = 0;
                foreach (var item in data.SaleOrderWarehouseDtl.Where(w => w.Enable == true).ToList())
                {
                    if (item.QtyAllow < item.Qty)
                    {
                        // ได้ยอดไม่ตามเป้า
                        item.QtyOriginal = item.Qty;
                        item.Qty = item.QtyAllow;
                        // clear ส่วนลด
                        item.PercentDiscount = 0;
                        item.BathDiscount = 0;
                        item.TotalPrice = item.QtyAllow * item.PricePerUnit;
                        db.Entry(item).State = EntityState.Modified;

                    }
                    if (item.ProductDetails.Products.ProductVatType.Id == MyConstant.ProductVatType.HasVat)
                    {
                        totalHasVat += item.TotalPrice;
                    }
                    else
                    {
                        totalUnVat += item.TotalPrice;
                    }
                    totalDisInList += item.BathDiscount;

                }
                decimal total = totalHasVat + totalUnVat; // ยอดรวมก่อนหักลด
                decimal totalBalance = total - data.TotalDiscount;

                data.TotalVat = totalHasVat * MyConstant.MyVat.Vat / MyConstant.MyVat.VatRemove;
                data.TotaNoVat = totalUnVat;
                data.TotalBeforeVat = totalHasVat - data.TotalVat;
                data.Total = total;
                data.TotalDiscountInList = totalDisInList;
                data.TotalBalance = totalBalance;
                db.SaveChanges();
            }
        }

        private List<SaleOrderWHForLibrary> MapSaleOrderWHForLibrary(SaleOrderWarehouse order)
        {
            List<SaleOrderWHForLibrary> sales = new List<SaleOrderWHForLibrary>();
            foreach (var item in order.SaleOrderWarehouseDtl.Where(w => w.Enable == true).ToList())
            {
                sales.Add(new SaleOrderWHForLibrary()
                {
                    PRODUCT_ID = item.FKProductDtl + "",
                    PRODUCT_NO = item.ProductDetails.Code,
                    PRODUCT_NAME = item.ProductDetails.Products.ThaiName,
                    UNIT_NAME = item.ProductDetails.ProductUnit.Name,
                    QTY = item.Qty + "",
                    GIVEAWAY = 0 + "",
                    COST = item.PricePerUnit + "",
                    DISCOUNT = item.BathDiscount + "",
                    COST_TOTAL = item.TotalPrice + ""

                });
            }
            return sales;
        }

        //private decimal CalVatFromDetails(List<SaleOrderWarehouseDtl> list)
        //{

        //    decimal sumProHasVat = 0;
        //    foreach (var item in list)
        //    {
        //        if (item.ProductDetails.Products.ProductVatType.Id == MyConstant.ProductVatType.HasVat)
        //        {
        //            // ถ้ามี vat 

        //        }
        //    }

        //}

        /// <summary>
        /// ยกเลิก ออร์เดอร์ โดย Disable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            List<string> stackDocNo = new List<string>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string docNo = dataGridView1.Rows[i].Cells[colDocNo].Value.ToString();
                string check = "";
                if (dataGridView1.Rows[i].Cells[colCheckAll].Value == null)
                {
                    check = "FALSE";
                }
                else
                {
                    check = dataGridView1.Rows[i].Cells[colCheckAll].Value.ToString();
                }
                //Console.WriteLine(id + " " + po + " --------- " + check);
                if (check.ToUpper() == "TRUE")
                {
                    stackDocNo.Add(docNo);
                    Console.WriteLine((i + 1) + " " + docNo + " --------- " + check);
                }

            }

            using (SSLsEntities db = new SSLsEntities())
            {
                foreach (var item in stackDocNo)
                {
                    var order = db.SaleOrderWarehouse.SingleOrDefault(w => w.Code == item);
                    if (order.FKSaleOrderWarehouseStatus == MyConstant.SaleOrderWarehouseStatus.CreateOrder && order.Enable)
                    {

                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถยืนยันรายการนี้ได้ " + item);
                        return;
                    }
                }
            }

            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?", "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    CancelOrder(stackDocNo);
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void CancelOrder(IEnumerable<string> stackDocNo)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                foreach (var item in stackDocNo)
                {
                    var order = db.SaleOrderWarehouse.SingleOrDefault(w => w.Code == item);
                    order.UpdateDate = DateTime.Now;
                    order.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    order.Enable = false;
                    order.FKSaleOrderWarehouseStatus = MyConstant.SaleOrderWarehouseStatus.CancelOrder;
                    db.Entry(order).State = EntityState.Modified;
                }
                db.SaveChanges();
                LoadGrid();
            }
        }
        /// <summary>
        /// Enter TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxKeySearch_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    SearchOrder();
                    break;
                default:
                    break;
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(colCheckAll, -1, true);
            ckBox.Size = new Size(14, 14);
            rect.X = rect.Location.X + (rect.Width / 2) - (ckBox.Width / 2);
            rect.Y += 3;
            ckBox.Location = rect.Location;
        }

        private void buttonBranch_Click(object sender, EventArgs e)
        {
            SelectedUserPopup obj = new SelectedUserPopup(this);
            obj.ShowDialog();
        }
        string _User = null;
        public void BinddingUser(Users send)
        {
            _User = send.Id;
            textBoxUserCode.Text = send.Id;
            textBoxUserName.Text = send.Name;
        }

        private void textBoxUserCode_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string userCode = textBoxUserCode.Text.Trim();
                    var user = SingletonPriority1.Instance().Users.Where(w => w.Enable == true).ToList();
                    var getUser = user.SingleOrDefault(w => w.Id == userCode);
                    if (getUser == null)
                    {
                        _User = null;
                        textBoxUserCode.Text = "";
                        textBoxUserName.Text = "";
                        return;
                    }
                    _User = getUser.Id;
                    textBoxUserCode.Text = getUser.Id;
                    textBoxUserName.Text = getUser.Name;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// พิมพ์ใบ A4 ออเดอร์ ที่เลือก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                /// รหัสใบออเดอร์
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
                frmMainReport mr = new frmMainReport(this, code);
                mr.Show();
                //int docQty = int.Parse(textBoxDocQty.Text.Trim());
                //var codeSelected = GetCodeOrderSelected();
                //PaperSaleOrderWarehouseViewer obj = new PaperSaleOrderWarehouseViewer(this, docQty, codeSelected);
                //obj.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("จำนวนเอกสารผิดพลาด");
            }

        }
        /// <summary>
        /// พิมพ์ใบเสร็จ ต้องเป็น ออเดอร์ที่ยืนยันแล้ว เท่านั้น
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            var codeSelected = GetCodeOrderSelected();
            using (SSLsEntities db = new SSLsEntities())
            {
                foreach (var item in codeSelected)
                {
                    var order = db.SaleOrderWarehouse.SingleOrDefault(w => w.Code == item && w.Enable == true);
                    //if ((order.FKSaleOrderWarehouseStatus == MyConstant.SaleOrderWarehouseStatus.WarehouseISS ||
                    //    order.FKSaleOrderWarehouseStatus == MyConstant.SaleOrderWarehouseStatus.WarehouseISSSuccess ||
                    //    order.FKSaleOrderWarehouseStatus == MyConstant.SaleOrderWarehouseStatus.CreateOrder) && order.Enable)
                    //{

                    //}
                    if (order.InvoiceNo != null)
                    {

                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถพิมพ์รายการนี้ได้ " + item + " กรุณาติ๊กออก");
                        return;
                    }
                }
            }

            PrintOrderOrder(codeSelected);
        }
        /// <summary>
        /// ปริ้นออเดอร์
        /// </summary>
        /// <param name="codeSelected"></param>
        private void PrintOrderOrder(IEnumerable<string> codeSelected)
        {

            using (SSLsEntities db = new SSLsEntities())
            {
                List<string> invs = new List<string>();
                foreach (var item in codeSelected)
                {
                    var data = db.SaleOrderWarehouse.FirstOrDefault(w => w.Enable == true && w.Code == item);
                    invs.Add(data.InvoiceNo);
                }
                _Inv = invs;
            }

            Thread print = new Thread(ThreadPrint);
            print.Start();
        }
        List<string> _Inv;
        void ThreadPrint()
        {
            int number = int.Parse(textBoxDocQty.Text.Trim());
            using (SSLsEntities db = new SSLsEntities())
            {
                foreach (var inv in _Inv)
                {
                    for (int i = 0; i < number; i++)
                    {
                        #region Print Invoice From Number
                        var createBy = Singleton.SingletonAuthen.Instance().Id;
                        var branch = Singleton.SingletonAuthen.Instance().MyBranch;
                        PosHeader data = db.PosHeader.FirstOrDefault(w => w.InvoiceNo == inv);
                        //  get lass order In data base by CreateBy
                        Model.ReceiptPrint rp = new Model.ReceiptPrint();
                        //rp.CompanyName = "ร้านสหกรณ์จังหวัดตราด จำกัด";
                        //rp.CompanyBranch = "สาขา 00002";
                        //rp.CompanyAddress = "99/9 หมู่ที่ 2 ตำบลเนินทราย อำเภอเมือง";
                        //rp.CompanyTel = "โทร. 039-513666-9";
                        //rp.CompanyFax = "แฟกซ์ 039-513664, 039-513665";
                        //rp.CompanyLineId = "ID Line: 0618648686";
                        //rp.CompanyTaxId = "Tax ID :0994000272383";
                        //rp.CompanyRegId = "Reg ID :E561655613561";
                        rp.CompanyName = branch.Name;
                        rp.CompanyBranch = "สาขา " + branch.BranchNo;
                        rp.CompanyAddress = branch.Address;
                        rp.CompanyTel = "โทร. " + branch.Tel;
                        rp.CompanyFax = "แฟกซ์ " + branch.Fax;
                        rp.CompanyLineId = "ID Line: " + branch.LineId;
                        rp.CompanyTaxId = "Tax ID :" + branch.TaxNo;
                        rp.CompanyRegId = "Reg ID :E561655613561";
                        List<Model.ProductInReceipt> pir = new List<Model.ProductInReceipt>();
                        string vatType = "";
                        string isPro = "";

                        foreach (var item in data.PosDetails.Where(w => w.Enable == true).ToList())
                        {
                            if (item.ProductDetails.Products.FKProductVatType == MyConstant.ProductVatType.HasVat)
                            {
                                vatType = "I";
                            }
                            else
                            {
                                vatType = "#";
                            }
                            if (item.IsPromotion == true)
                            {
                                isPro = "#C";
                            }
                            else
                            {
                                isPro = "";
                            }
                            pir.Add(new ProductInReceipt()
                            {
                                Code = isPro + item.ProductDetails.Code,
                                Qty = Library.ConvertDecimalToStringForm(item.Qty),
                                Name = item.ProductDetails.Products.ThaiName,
                                SalePrice = Library.ConvertDecimalToStringForm(item.PricePerUnit),
                                ProductVatType = vatType,
                                Total = Library.ConvertDecimalToStringForm(item.TotalPrice)
                            });
                            // แสดงว่ามี คูปอง
                            if (item.DiscountCoupon > 0)
                            {
                                pir.Add(new ProductInReceipt()
                                {
                                    Code = item.ProductDetails.Code,
                                    Qty = "1.00",
                                    Name = "[ลดคูปอง]" + item.Description,
                                    SalePrice = Library.ConvertDecimalToStringForm(-item.DiscountCoupon),
                                    ProductVatType = vatType,
                                    Total = Library.ConvertDecimalToStringForm(-item.DiscountCoupon)
                                });
                            }
                            // แสดงว่ามี ลดทางร้าน
                            if (item.DiscountShop > 0)
                            {
                                pir.Add(new ProductInReceipt()
                                {
                                    Code = item.ProductDetails.Code,
                                    Qty = "1.00",
                                    Name = "[ส่วนลด]" + item.Description,
                                    SalePrice = Library.ConvertDecimalToStringForm(-item.DiscountShop),
                                    ProductVatType = vatType,
                                    Total = Library.ConvertDecimalToStringForm(-item.DiscountShop)
                                });
                            }
                        }

                        rp.ProductInReceipts = pir;
                        rp.TotalBalance = Library.ConvertDecimalToStringForm(data.TotalBalance);
                        rp.TotalCash = Library.ConvertDecimalToStringForm(data.Cash);
                        rp.TotalChange = Library.ConvertDecimalToStringForm(data.Change);
                        rp.MemberId = data.Member.Code;
                        rp.MemberName = data.Member.Name;
                        rp.OrderType = data.PosType.Name;
                        rp.TotalUnVat = Library.ConvertDecimalToStringForm(data.TotaNoVat);
                        rp.TotalHasVat = Library.ConvertDecimalToStringForm(data.TotalBeforeVat);
                        rp.TotalVat = Library.ConvertDecimalToStringForm(data.TotalVat);
                        rp.TotalList = Library.ConvertDecimalToStringForm(pir.Count);
                        //decimal sumQty = data.PosDetails.Where(w => w.Enable == true).ToList()
                        rp.TotalUnit = Library.ConvertDecimalToStringForm(data.PosDetails.Where(w => w.Enable == true).Sum(w => w.Qty));
                        rp.CashierName = Singleton.SingletonAuthen.Instance().Name;
                        rp.OrderNo = data.InvoiceNo;

                        rp.OrderDate = Library.ConvertDateToThaiDate(data.CreateDate, true);


                        if (data.PrintSequence == 0)
                        {
                            Repository.ReceiptPrint.PrintNow(rp);
                            // update
                            data.PrintSequence += 1;
                            db.Entry(data).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            Repository.ReceiptPrint.PrintNow(rp, true, data.PrintSequence);
                            // update
                            data.PrintSequence += 1;
                            db.Entry(data).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        #endregion

                    }
                }

            }


        }
        /// <summary>
        /// geg code ที่เลือกทั้งหมด
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetCodeOrderSelected()
        {
            //string docNos = "SOR60090005,SOR60090006,SOR60090008,SOR60090009";
            List<string> stackDocNo = new List<string>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string docNo = dataGridView1.Rows[i].Cells[colDocNo].Value.ToString();
                string check = "";
                if (dataGridView1.Rows[i].Cells[colCheckAll].Value == null)
                {
                    check = "FALSE";
                }
                else
                {
                    check = dataGridView1.Rows[i].Cells[colCheckAll].Value.ToString();
                }
                //Console.WriteLine(id + " " + po + " --------- " + check);
                if (check.ToUpper() == "TRUE")
                {
                    stackDocNo.Add(docNo);

                    Console.WriteLine((i + 1) + " " + docNo + " --------- " + check);
                }
            }
            return stackDocNo;
        }
    }
}
