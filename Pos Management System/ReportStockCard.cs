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
    public partial class ReportStockCard : Form
    {
        public ReportStockCard()
        {
            InitializeComponent();
        }

        public void BillingData()
        {
            Cursor.Current = Cursors.WaitCursor;
            using (SSLsEntities db = new SSLsEntities())
            {
                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                int checkCombo1 = 0;
                int checkCombo2 = 0;
                List<int> fkProHd = new List<int>();
                string code1 = textBox1.Text.Trim();
                string code2 = textBox2.Text.Trim();
                if (code1 == "")
                {
                    fkProHd = db.StoreFrontStock.Where(w => w.Enable == true).Select(w => w.FKProduct).Distinct().ToList();
                }
                else
                {
                    fkProHd = Library.GetAllProDtlId(code1, code2);
                }
                //if (comboBox1.Text != "ทั้งหมด")
                //{
                //    checkCombo1 = int.Parse(comboBox1.SelectedValue.ToString());
                //    checkCombo2 = int.Parse(comboBox2.SelectedValue.ToString());
                //    fkProHd = db.StoreFrontStock.Where(w => w.Enable == true).Select(w => w.FKProduct).Distinct().ToList();
                //}
                //else
                //{
                //    fkProHd = Library.GetAllProDtlId(code1, code2);
                //}
                //var txt1 = Singleton.SingletonProduct.Instance().ProductDetails.Where(ww => ww.Code == textBox1.Text & ww.Enable == true).Select(s => s.FKProduct).FirstOrDefault();

                //var txt2 = Singleton.SingletonProduct.Instance().ProductDetails.Where(ww => ww.Code == textBox2.Text & ww.Enable == true).Select(s => s.FKProduct).FirstOrDefault();
                // var listData = db.StoreFrontStockDetails.Where(w => w.Enable == true && 
                // fkProHd.Contains(w.StoreFrontStock.FKProduct) &&
                // DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                // DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE) &&
                //(checkCombo1 == 0 && checkCombo2 == 0 ?
                //(w.StoreFrontStock.Products.FKProductGroup > 0) :
                //(w.StoreFrontStock.Products.FKProductGroup >= checkCombo1 &&
                //w.StoreFrontStock.Products.FKProductGroup <= checkCombo2))).ToList();
                var listData = db.StoreFrontStockDetails.Where(w => w.Enable == true &&
                fkProHd.Contains(w.StoreFrontStock.FKProduct) &&
                DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE)
               ).ToList();
                var list1 = listData
                .GroupBy(g => new
                {
                    g.StoreFrontStock.Products.ProductDetails.OrderBy(w => w.PackSize).FirstOrDefault().Code,
                    g.StoreFrontStock.Products.ThaiName,
                    g.StoreFrontStock.FKProduct,
                    g.FKStoreFrontStock,
                    g.FKTransactionType,
                    UnitName = g.StoreFrontStock.Products.ProductDetails.Where(w => w.PackSize == 1).Select(ss => (ss.ProductUnit.Name == null ? "ไม่มี" : ss.ProductUnit.Name)).FirstOrDefault(),
                    ยอดยกมา = 0,
                    ยอดยกไป = 0,
                    Price = 0
                })
                .Select(s => new { ProductId = s.Key.FKProduct, Product = s.Key.Code + " " + s.Key.ThaiName, FKS = s.Key.FKStoreFrontStock, Id = s.Key.FKTransactionType, s.Key.UnitName, s.Key.ยอดยกมา, s.Key.ยอดยกไป, ActionQty = (s.Sum(ss => ss.ActionQty) == null ? 0 : s.Sum(ss => ss.ActionQty)), s.Key.Price }).GroupBy(g => new { g.ProductId, g.Product, g.UnitName, g.ยอดยกมา, g.ยอดยกไป, g.Price, g.Id })
                  .Select(s => new
                  {
                      ProuctId = s.Key.ProductId,
                      Product = s.Key.Product,
                      UnitName = s.Key.UnitName,
                      Summit = s.Key.ยอดยกมา,
                      LiftUp = s.Key.ยอดยกไป,
                      Price = s.Key.Price,
                      Buy = (s.Key.Id == 19 ? s.Sum(m => m.ActionQty) : 0),
                      SendVender = (s.Key.Id == 3 ? s.Sum(m => m.ActionQty) : 0),
                      Sell = (s.Key.Id == 2 ? s.Sum(m => m.ActionQty) : 0),
                      SellGive = (s.Key.Id == 11 ? s.Sum(m => m.ActionQty) : 0),
                      Cn = (s.Key.Id == 4 || s.Key.Id == 22 ? s.Sum(m => m.ActionQty) : 0),
                      CnGive = (s.Key.Id == 12 ? s.Sum(m => m.ActionQty) : 0),
                      CnCancel = (s.Key.Id == 5 ? s.Sum(m => m.ActionQty) : 0),
                      CnGiveCancel = (s.Key.Id == 17 ? s.Sum(m => m.ActionQty) : 0),
                      Rcv = (s.Key.Id == 1 ? s.Sum(m => m.ActionQty) : 0),
                      RcvPo = (s.Key.Id == 10 ? s.Sum(m => m.ActionQty) : 0),
                      OutUse = (s.Key.Id == 14 ? s.Sum(m => m.ActionQty) : 0),
                      Out = (s.Key.Id == 8 ? s.Sum(m => m.ActionQty) : 0),
                      AdjustCost = (s.Key.Id == 20 ? s.Sum(m => m.ActionQty) : 0),
                      Tran = (s.Key.Id == 9 ? s.Sum(m => m.ActionQty) : 0)
                  }).GroupBy(g => new { g.ProuctId, g.Product, g.UnitName, g.Summit, g.LiftUp, g.Price })
                   .Select(s => new
                   {
                       ProuctId = s.Key.ProuctId,
                       Product = s.Key.Product,
                       UnitName = s.Key.UnitName,
                       Summit = s.Key.Summit,
                       LiftUp = s.Key.LiftUp,
                       Price = s.Key.Price,
                       Buy = s.Sum(ss => ss.Buy),
                       SendVender = s.Sum(ss => ss.SendVender),
                       Sell = s.Sum(ss => ss.Sell),
                       SellGive = s.Sum(ss => ss.SellGive),
                       Cn = s.Sum(ss => ss.Cn),
                       CnGive = s.Sum(ss => ss.CnGive),
                       CnCancel = s.Sum(ss => ss.CnCancel),
                       CnGiveCancel = s.Sum(ss => ss.CnGiveCancel),
                       Rcv = s.Sum(ss => ss.Rcv),
                       RcvPo = s.Sum(ss => ss.RcvPo),
                       OutUse = s.Sum(ss => ss.OutUse),
                       Out = s.Sum(ss => ss.Out),
                       AdjustCost = s.Sum(ss => ss.AdjustCost),
                       Tran = s.Sum(ss => ss.Tran)
                   }).ToList();
                List<TransValue> ls = new List<TransValue>();
                int i = 0;
                int countListpg = list1.Count();
                progressBar1.Minimum = 0;
                progressBar1.Maximum = countListpg;
                foreach (var item in list1)
                {
                    i++;
                    TransValue c = new TransValue();
                    c.ProuctId = item.ProuctId.ToString();
                    c.Product = item.Product.ToString();
                    c.UnitName = item.UnitName;
                    c.Summit = GetResult(item.ProuctId, dateS);
                    c.LiftUp = GetBalance(item.ProuctId, dateS, dateE);
                    c.Price = Library.GetAverage(item.ProuctId);
                    c.Buy = item.Buy;
                    c.SendVender = item.SendVender;
                    c.Sell = item.Sell;
                    c.SellGive = item.SellGive;
                    c.Cn = item.Cn;
                    c.CnCancel = item.CnCancel;
                    c.CnGiveCancel = item.CnGiveCancel;
                    c.Rcv = item.Rcv;
                    c.RcvPo = item.RcvPo;
                    c.OutUse = item.OutUse;
                    c.Out = item.Out;
                    c.AdjustCost = item.AdjustCost;
                    c.Tran = item.Tran;
                    ls.Add(c);
                    progressBar1.Value = i;
                    progressBar1.Refresh();
                }
                if (list1.Count > 0)
                {
                    string User, WhereDate, WhereProduct, WhereGroupP;
                    //User = Singleton.SingletonAuthen.Instance().Name; 
                    User = Singleton.SingletonAuthen.Instance().Name;
                    WhereDate = Library.ConvertDateToThaiDate(dateS) + "  ถึง  " + Library.ConvertDateToThaiDate(dateE);
                    WhereProduct = (textBox1.Text == "" ? "ค้นหา รหัส Barcode สินค้าทั้งหมด" : "ค้นหา รหัส Barcode สินค้า : " + textBox1.Text + " ถึง " + textBox2.Text);
                    WhereGroupP = (comboBox1.Text == "ทั้งหมด" ? "ค้นหา หมวดสินค้าทั้งหมด" : "ค้นหา หมวดสินค้า : " + comboBox1.Text + " ถึง " + comboBox2.Text);
                    DataTable dt = CodeFileDLL.ConvertToDataTable(ls);
                    frmMainReport frm = new frmMainReport(this, dt, User, WhereDate, WhereProduct, WhereGroupP);
                    Cursor.Current = Cursors.Default;
                    frm.Show();
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("ไม่พบข้อมูล");
                }

            }
        }
        class TransValue
        {
            public decimal RcvPo { get; set; }
            public decimal OutUse { get; set; }
            public decimal Out { get; set; }
            public decimal AdjustCost { get; set; }

            public decimal Tran { get; set; }
            public string ProuctId { get; set; }
            public string Product { get; set; }
            public string UnitName { get; set; }
            public decimal Summit { get; set; }
            public decimal LiftUp { get; set; }
            public decimal Price { get; set; }

            public decimal Buy { get; set; }
            public decimal SendVender { get; set; }
            public decimal Sell { get; set; }
            public decimal SellGive { get; set; }
            public decimal Cn { get; set; }
            public decimal CnGive { get; set; }
            public decimal CnCancel { get; set; }
            public decimal CnGiveCancel { get; set; }
            public decimal Rcv { get; set; }


        }
        private void ReportStockCard_Load(object sender, EventArgs e)
        {

            button3.Enabled = false;
            button4.Enabled = false;
            checkBoxProduct.Checked = true;
            lb_msg.Visible = false;

            var getPdtl = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
            AutoCompleteStringCollection colPdtl = new AutoCompleteStringCollection();
            foreach (var item in getPdtl)
            {
                colPdtl.Add(item.Code);
            }
            textBox1.AutoCompleteCustomSource = colPdtl;

            var getPdg = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
            AutoCompleteStringCollection colPdg = new AutoCompleteStringCollection();
            foreach (var item in getPdg)
            {
                colPdg.Add(item.Code);
            }
            textBox2.AutoCompleteCustomSource = colPdg;

            SSLsEntities db = new SSLsEntities();
            var combo = db.ProductGroups.Where(w => w.Enable == true).Select(s => new { s.Id, s.Name }).OrderBy(o => o.Id).ToList();

            DataTable dt_summary = new DataTable();
            DataTable dt_summary2 = new DataTable();
            dt_summary = CodeFileDLL.ConvertToDataTable(combo);
            dt_summary2 = CodeFileDLL.ConvertToDataTable(combo);
            dt_summary.Rows.Add(0, "ทั้งหมด");
            comboBox1.DataSource = dt_summary;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedIndex = dt_summary.Rows.Count - 1;

            dt_summary2.Rows.Add(0, "ทั้งหมด");
            comboBox2.DataSource = dt_summary2;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "Id";
            comboBox2.SelectedIndex = dt_summary2.Rows.Count - 1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox1.Text;
        }

        private void ComboboxChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedValue = comboBox1.SelectedValue;
        }
        /// <summary>
        /// get ยอดยกมา
        /// </summary>
        /// <param name="fkProHD"></param>
        /// <param name="startdDate"></param>
        /// <returns></returns>
        public decimal GetResult(int fkProHD, DateTime startdDate)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                //return 0;
                List<StoreFrontStockDetails> detils = db.StoreFrontStockDetails
                    .Where(w => w.StoreFrontStock.Enable == true && w.StoreFrontStock.FKProduct == fkProHD && w.Enable == true &&
                    DbFunctions.TruncateTime(w.CreateDate) < DbFunctions.TruncateTime(startdDate)
                    ).ToList();

                decimal plus = 0;
                decimal minus = 0;
                decimal result = 0;
                if (detils.Count() > 0)
                {
                    plus = detils.Where(w => w.TransactionType.IsPlus == true).Sum(w => w.ActionQty);
                    minus = detils.Where(w => w.TransactionType.IsPlus == false).Sum(w => w.ActionQty);
                    result = plus - minus;
                    return result;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// ยอดยกไป
        /// </summary>
        /// <param name="fkProHD"></param>
        /// <param name="startdDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal GetBalance(int fkProHD, DateTime startdDate, DateTime endDate)
        {
            decimal resultForProDtl = GetResult(fkProHD, startdDate);
            try
            {

                using (SSLsEntities db = new SSLsEntities())
                {
                    List<StoreFrontStockDetails> details = db.StoreFrontStock.FirstOrDefault(w => w.FKProduct == fkProHD && w.Enable == true)
                      .StoreFrontStockDetails.Where(w => w.Enable == true &&
                      int.Parse(w.CreateDate.ToString("yyyyMMdd")) >= int.Parse(startdDate.ToString("yyyyMMdd")) &&
                      int.Parse(w.CreateDate.ToString("yyyyMMdd")) <= int.Parse(endDate.ToString("yyyyMMdd"))
                      ).OrderBy(w => w.CreateDate).ToList();

                    if (details.Count() == 0)
                    {
                        return 0;
                    }
                    else
                    {

                        foreach (var trans in details)
                        {
                            decimal plus = 0;
                            decimal minus = 0;

                            if (trans.TransactionType.IsPlus == true)
                            {
                                plus = trans.ActionQty;
                                resultForProDtl = resultForProDtl + plus;
                            }
                            else
                            {
                                minus = trans.ActionQty;
                                resultForProDtl = resultForProDtl - minus;
                            }
                        }
                    }
                    return resultForProDtl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lb_msg.Text = "โปรแกรมกำลังทำงาน . . ";
            lb_msg.Visible = true;
            this.Refresh();
            this.BillingData();
        }
        bool changeTxt;
        private void button3_Click(object sender, EventArgs e)
        {
            changeTxt = true;
            SelectedProductPopup obj = new SelectedProductPopup(this);
            obj.ShowDialog();
        }
        public void BinddingProduct(int id)
        {
            var product = SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == id);
            if (changeTxt == true)
            {
                textBox1.Text = product.Code;
            }
            else
            {
                textBox2.Text = product.Code;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            changeTxt = false;
            SelectedProductPopup obj = new SelectedProductPopup(this);
            obj.ShowDialog();
        }

        private void checkBoxProduct_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxProduct.Checked == true)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                textBox1.Text = null;
                textBox2.Text = null;
            }
            else
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                textBox1.Text = null;
                textBox2.Text = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lb_msg.Visible = false;
            checkBoxProduct.Checked = true;
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            comboBox2.SelectedIndex = comboBox2.Items.Count - 1;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox1.Text = null;
            textBox2.Text = null;
            dateTimePickerStart.Value = DateTime.Now;
            dateTimePickerEnd.Value = DateTime.Now;
            progressBar1.Value = 0;
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            textBox2.Text = textBox1.Text;
        }
    }
}
