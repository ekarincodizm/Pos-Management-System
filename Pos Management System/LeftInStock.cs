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
    public partial class LeftInStock : Form
    {
        public LeftInStock() // สินค้าคงเหลือ
        {
            InitializeComponent();
        }

        public LeftInStock(string v) // สินค้าคงเหลือ ราคาทุน-ราคาขาย
        {
            InitializeComponent();
            this.v = v;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lb_msg.Text = "โปรแกรมกำลังทำงาน . . ";
            lb_msg.Visible = true;
            this.Refresh();
            if (v == "ราคาทุน-ราคาขาย")
            {
                bildingDataCostAndSellPrice(txt_p_S.Text.Trim(), txt_p_E.Text.Trim());
            }
            else
            {
                bildingData(txt_p_S.Text.Trim(), txt_p_E.Text.Trim());
            }

        }
        public class LefClass
        {
            public string WareHouse { get; set; }
            public string ProductCode { get; set; }
            public string ProductBarCode { get; set; }
            public string ProductName { get; set; }
            public decimal CurrentQty { get; set; }
            public string Unit { get; set; }
            public decimal Price { get; set; }
            public decimal ResidualValue { get; set; }
            public string CurrUnit { get; set; }

            public string SellPricePerUnit { get; set; }
            public string SellPriceTotal { get; set; }

            public int FKProduct { get; set; }
            public string Description { get; set; }
        }
        void bildingData(string Code_S, string Code_E)
        {
            //try
            //{
            var dateRun = DateTime.Now;
            DateTime dateS = dateTimePicker_S.Value;
            var aaa = comboBox_S.Text;
            Cursor.Current = Cursors.WaitCursor;
            int i = 0;
            List<LefClass> lssfs = new List<LefClass>();
            using (SSLsEntities db = new SSLsEntities())
            {
                int comBo_S = int.Parse(comboBox_S.SelectedValue.ToString());
                int comBo_E = int.Parse(comboBox_E.SelectedValue.ToString());
                //List<int> fkPro = Library.GetAllProDtlId(Code_S, Code_E);
                //IQueryable<fn_GetResultDate_Result4> getVal;
                if (checkBoxProduct.Checked == true)
                {
                    // เลือกทั้งหมด          
                    //fkPro = db.StoreFrontStock.Where(w => w.Enable == true).Select(w => w.FKProduct).Distinct().ToList();
                    Code_S = "0";
                    Code_E = "999999999999999";
                }
                else
                {
                    Code_S = txt_p_S.Text.Trim();
                    Code_E = txt_p_E.Text.Trim();
                    //pid_S = getIdProduct_S(Code_S);
                    //pid_E = getIdProduct_E(Code_E);                    
                }
                //List<StoreFrontStock> sfs = new List<StoreFrontStock>();
                //sfs = db.StoreFrontStock.Where(w => w.Enable == true && fkPro.Contains(w.FKProduct)).ToList();

                //List<StoreFrontStock> sfs = db.StoreFrontStock.Where(w => w.Enable == true && fkPro.Contains(w.FKProduct) &&
                //(comBo_S <= comBo_E ?  // เงื่อนไขนี้เพื่อ บางครั้ง กลุ่มสินค้า ที่หามา id เริ่มต้นค้นหาน้อยกว่า ทำให้หาไม่เจอ เลยสับกัน
                //((comBo_S == 0 ? w.Products.FKProductGroup > 0 : w.Products.FKProductGroup >= comBo_S) && (comBo_E == 0 ? w.Products.FKProductGroup > 0 : w.Products.FKProductGroup <= comBo_E)) :
                //((comBo_E == 0 ? w.Products.FKProductGroup > 0 : w.Products.FKProductGroup >= comBo_E) && (comBo_S == 0 ? w.Products.FKProductGroup > 0 : w.Products.FKProductGroup <= comBo_S)))).ToList();
                var dateString = dateS.AddDays(1).ToString("yyyyMMdd");
                //var aa = db.fn_GetResultDate(dateString, Code_S, Code_E).ToList();
                var aa = Library.GetQueryยอดยกมา(dateString, Code_S, Code_E).ToList();
                int countListpg = aa.Count();
                progressBar1.Minimum = 0;
                progressBar1.Maximum = countListpg;
                LefClass c;
                //List<fn_GetResultDate> getValueResult = new List<fn_GetResultDate>();
                var checkFront = db.View_CheckFront_01_11_17.ToList();
                //var getVal = ggr.Select(s => new { s.Id, s.ThaiName, s.qty, s.Cost }).ToList();
                foreach (var item in aa)
                {
                    i++;
                    c = new LefClass();
                    try
                    {
                        //var getFirst = item.Products.ProductDetails.OrderBy(od => od.PackSize).FirstOrDefault();
                        c.WareHouse = "หน้าร้าน";
                        c.ProductBarCode = item.Code;
                        c.ProductCode = item.Code;
                        c.ProductName = item.Thainame;
                        //if (item.Cost == 0)
                        //{
                        //    //c.Price = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.FKProduct == item.Id).OrderBy(w => w.PackSize).FirstOrDefault().CostOnly;
                        //    c.Price = item.Cost;
                        //}
                        //else
                        //{
                        //    c.Price = (decimal)item.Cost;
                        //}
                        c.Price = decimal.Parse(item.Cost);
                        c.CurrentQty = decimal.Parse(item.qty);
                        c.Unit = item.Unit;

                        //c.Unit = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.FKProduct == item.Id).OrderBy(w => w.PackSize).FirstOrDefault().ProductUnit.Name;
                        ////c.CurrentQty = Library.GetResult(item.FKProduct, dateS.AddDays(1));
                        //var getr = getVal.FirstOrDefault(w => w.Id == item.FKProduct);
                        //getVal.Remove(getr);
                        //decimal res = 0;
                        //if (getr == null)
                        //{
                        //    res = 0;
                        //    c.Price = getFirst.CostOnly;
                        //}
                        //else
                        //{
                        //    res = (decimal)getr.qty;
                        //    if (getr.Cost == 0)
                        //    {
                        //        c.Price = getFirst.CostOnly;
                        //    }
                        //    else
                        //    {
                        //        c.Price = getr.Cost;
                        //    }
                        //}
                        //c.CurrentQty = res;
                        //c.Unit = getFirst.ProductUnit.Name;
                        //c.CurrUnit = Library.ConvertDecimalToStringForm(c.CurrentQty) + " " + c.Unit;
                        c.CurrUnit = Library.ConvertDecimalToStringForm(c.CurrentQty) + " " + c.Unit;
                        ////c.Price = Library.GetAverage(item.FKProduct);

                        c.ResidualValue = c.CurrentQty * c.Price;
                        c.FKProduct = (int)item.FKProduct;
                        c.SellPricePerUnit = "0";
                        c.SellPriceTotal = "0";
                        var checkDoc = checkFront.Where(w => w.FKProduct == item.FKProduct).ToList();
                        string docStr = "";
                        foreach (var doc in checkDoc)
                        {
                            docStr = doc.DocNo + " (" + doc.Number + ") " + docStr;
                        }
                        c.Description = docStr;
                        lssfs.Add(c);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("*************** Error ");
                    }
                    progressBar1.Value = i;
                    progressBar1.Refresh();
                    Console.WriteLine("*************** " + item.FKProduct);
                }
            }

            dataGridView1.DataSource = lssfs;
            dataGridView1.Refresh();

            var sumQty = lssfs.Sum(w => w.CurrentQty);
            var sumPrice = lssfs.Sum(w => w.Price);
            var sumResidualValue = lssfs.Sum(w => w.ResidualValue);
            var countList = lssfs.Count();

            var bilding2Report = lssfs.Select(a =>
            new
            {
                WareHouse = "หน้าร้าน",
                ProductCode = a.ProductBarCode,
                ProductName = a.ProductName,
                CurrentQty = a.CurrentQty,
                CurrUnit = a.CurrUnit,
                Unit = a.Unit,
                Price = a.Price,
                ResidualValue = a.ResidualValue,
                sumQty = sumQty,
                sumPrice = sumPrice,
                sumResidualValue = sumResidualValue,
                SellPricePerUit = 0,
                SellPriceTotal = 0,
                countList = countList,
                TimeNow = Library.ConvertDateToThaiDate(dateS),
                Product_S = Code_S == "" || Code_S == null ? "ทั้งหมด" : Code_S,
                Product_E = Code_E == "" || Code_E == null ? "ทั้งหมด" : Code_E,
                ProductGroup_S = comboBox_S.Text,
                ProductGroup_E = comboBox_E.Text
            }).ToList();

            MessageBox.Show((DateTime.Now - dateRun).Minutes + " นาที");

            var dt = Library.ConvertToDataTable(bilding2Report);
            frmMainReport mr = new frmMainReport(this, dt);
            mr.Show();

            lb_msg.Text = "ระบบประมวลผลสำเร็จ";
            this.Refresh();
            Cursor.Current = Cursors.Default;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    throw;
            //}
        }

        void bildingDataCostAndSellPrice(string Code_S, string Code_E)
        {
            //try
            //{
            DateTime dateS = dateTimePicker_S.Value;
            var aaa = comboBox_S.Text;
            Cursor.Current = Cursors.WaitCursor;
            int i = 0;
            List<LefClass> lssfs = new List<LefClass>();
            using (SSLsEntities db = new SSLsEntities())
            {
                int pid_S;
                int pid_E;
                int comBo_S = int.Parse(comboBox_S.SelectedValue.ToString());
                int comBo_E = int.Parse(comboBox_E.SelectedValue.ToString());
                List<int> fkPro = new List<int>();
                if (checkBoxProduct.Checked == true)
                {
                    // เลือกทั้งหมด
                    pid_S = 0;
                    pid_E = 0;
                    fkPro = db.StoreFrontStock.Where(w => w.Enable == true).Select(w => w.FKProduct).Distinct().ToList();
                }
                else
                {
                    pid_S = getIdProduct_S(Code_S);
                    pid_E = getIdProduct_E(Code_E);
                    fkPro = Library.GetAllProDtlId(Code_S, Code_E);
                }


                List<StoreFrontStock> sfs = db.StoreFrontStock.Where(w => w.Enable == true && fkPro.Contains(w.FKProduct) &&
                (comBo_S <= comBo_E ?  // เงื่อนไขนี้เพื่อ บางครั้ง กลุ่มสินค้า ที่หามา id เริ่มต้นค้นหาน้อยกว่า ทำให้หาไม่เจอ เลยสับกัน
                ((comBo_S == 0 ? w.Products.FKProductGroup > 0 : w.Products.FKProductGroup >= comBo_S) && (comBo_E == 0 ? w.Products.FKProductGroup > 0 : w.Products.FKProductGroup <= comBo_E)) :
                ((comBo_E == 0 ? w.Products.FKProductGroup > 0 : w.Products.FKProductGroup >= comBo_E) && (comBo_S == 0 ? w.Products.FKProductGroup > 0 : w.Products.FKProductGroup <= comBo_S)))).ToList();
                int countListpg = sfs.Count();
                progressBar1.Minimum = 0;
                progressBar1.Maximum = countListpg;
                var dateString = dateS.AddDays(1).ToString("yyyyMMdd");
                //var aa = db.fn_GetResultDate(dateString, "0", "999999999999999").ToList();
                var aa = Library.GetQueryยอดยกมา(dateString, "0", "999999999999999");
                foreach (var item in sfs)
                {
                    i++;
                    LefClass c = new LefClass();
                    c.WareHouse = "หน้าร้าน";
                    if (item.Products.Enable == false)
                    {
                        progressBar1.Value = i;
                        progressBar1.Refresh();
                        continue;
                    }
                    if (item.Products.ProductDetails.OrderBy(od => od.PackSize).FirstOrDefault() == null)
                    {
                        progressBar1.Value = i;
                        progressBar1.Refresh();
                        continue;
                    }
                    c.ProductBarCode = item.Products.ProductDetails.OrderBy(od => od.PackSize).FirstOrDefault().Code;
                    c.ProductCode = item.Products.Code;
                    c.ProductName = item.Products.ThaiName;
                    //c.CurrentQty = Library.GetResult(item.FKProduct, dateS.AddDays(1));
                    var getProd = aa.SingleOrDefault(w => w.FKProduct == item.FKProduct);
                    if (getProd == null)
                    {
                        progressBar1.Value = i;
                        progressBar1.Refresh();
                        continue;
                    }
                    c.CurrentQty = decimal.Parse(getProd.qty);
                    c.Unit = item.Products.ProductDetails.OrderBy(od => od.PackSize).Select(ss => (ss.ProductUnit.Name == null ? "ไม่มี" : ss.ProductUnit.Name)).FirstOrDefault();
                    c.CurrUnit = Library.ConvertDecimalToStringForm(c.CurrentQty) + " " + c.Unit;
                    //c.Price = Library.GetAverage(item.FKProduct);
                    c.Price = decimal.Parse(getProd.Cost);
                    c.ResidualValue = c.CurrentQty * c.Price;
                    c.FKProduct = item.Products.Id;
                    c.SellPricePerUnit = Library.ConvertDecimalToStringForm(Library.GetSellPriceOnly(item.FKProduct, dateS));
                    //c.SellPricePerUnit = "1";
                    c.SellPriceTotal = Library.ConvertDecimalToStringForm(decimal.Parse(c.SellPricePerUnit) * c.CurrentQty);
                    lssfs.Add(c);
                    progressBar1.Value = i;
                    progressBar1.Refresh();
                    //if (item.FKProduct == 60018)
                    //{
                    //    break;
                    //}
                }
            }

            var sumQty = lssfs.Sum(w => w.CurrentQty);
            var sumPrice = lssfs.Sum(w => w.Price);
            var sumResidualValue = lssfs.Sum(w => w.ResidualValue);
            var countList = lssfs.Count();
            var sumTotal = lssfs.Sum(w => decimal.Parse(w.SellPriceTotal));
            var bilding2Report = lssfs.Select(a =>
            new
            {
                WareHouse = "หน้าร้าน",
                ProductCode = a.ProductBarCode,
                ProductName = a.ProductName,
                CurrentQty = a.CurrentQty,
                CurrUnit = a.CurrUnit,
                Unit = a.Unit,
                Price = a.Price,
                ResidualValue = a.ResidualValue,
                sumQty = sumQty,
                sumPrice = sumPrice,
                sumResidualValue = sumResidualValue,
                SellPricePerUnit = a.SellPricePerUnit,
                SellPriceTotal = a.SellPriceTotal,
                countList = countList,
                TimeNow = Library.ConvertDateToThaiDate(dateS),
                Product_S = Code_S == "" || Code_S == null ? "ทั้งหมด" : Code_S,
                Product_E = Code_E == "" || Code_E == null ? "ทั้งหมด" : Code_E,
                ProductGroup_S = comboBox_S.Text,
                ProductGroup_E = comboBox_E.Text,
                sumSellPriceTotal = sumTotal
            }).ToList();

            var dt = Library.ConvertToDataTable(bilding2Report);
            frmMainReport mr = new frmMainReport(this, dt, MyConstant.TypeReport.LeftInStockVat);
            mr.Show();

            lb_msg.Text = "ระบบประมวลผลสำเร็จ";
            this.Refresh();
            Cursor.Current = Cursors.Default;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    throw;
            //}
        }
        public int getIdProduct_S(string Key_S)
        {
            try
            {
                var data_S = Singleton.SingletonProduct.Instance().ProductDetails.OrderBy(dm => dm.PackSize).FirstOrDefault(w => w.Code == Key_S.Replace(" ", ""));
                if (data_S != null)
                {
                    return data_S.FKProduct;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        public int getIdProduct_E(string Key_S)
        {
            try
            {
                var data_E = Singleton.SingletonProduct.Instance().ProductDetails.OrderBy(dm => dm.PackSize).FirstOrDefault(w => w.Code == Key_S.Replace(" ", ""));
                if (data_E != null)
                {
                    return data_E.FKProduct;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        public void setProductGroup(ComboBox cb)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var pgp = db.ProductGroups.Where(w => w.Enable == true).ToList();
                var locationtype = pgp.Select(w => new { w.Id, w.Name }).ToList();
                DataTable dt_summary = new DataTable();
                dt_summary = Library.ConvertToDataTable(locationtype);
                dt_summary.Rows.Add(0, "ทั้งหมด");
                cb.DataSource = dt_summary;
                cb.DisplayMember = "Name";
                cb.ValueMember = "Id";
                cb.SelectedIndex = dt_summary.Rows.Count - 1;
            }
        }

        private void LeftInStock_Load(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button4.Enabled = false;
            setProductGroup(comboBox_S);
            setProductGroup(comboBox_E);
            checkBoxProduct.Checked = true;
            lb_msg.Visible = false;

            var getPdtl = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
            AutoCompleteStringCollection colPdtl = new AutoCompleteStringCollection();
            foreach (var item in getPdtl)
            {
                colPdtl.Add(item.Code);
            }
            txt_p_S.AutoCompleteCustomSource = colPdtl;

            var getPdg = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
            AutoCompleteStringCollection colPdg = new AutoCompleteStringCollection();
            foreach (var item in getPdg)
            {
                colPdg.Add(item.Code);
            }
            txt_p_E.AutoCompleteCustomSource = colPdg;
        }

        private void txt_p_S_TextChanged(object sender, EventArgs e)
        {
            txt_p_E.Text = txt_p_S.Text;
        }

        private void checkBoxProduct_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxProduct.Checked == true)
            {
                txt_p_S.Enabled = false;
                txt_p_E.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                txt_p_S.Text = null;
                txt_p_E.Text = null;
            }
            else
            {
                txt_p_S.Enabled = true;
                txt_p_E.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                txt_p_S.Text = null;
                txt_p_E.Text = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lb_msg.Visible = false;
            checkBoxProduct.Checked = true;
            comboBox_S.SelectedIndex = comboBox_S.Items.Count - 1;
            comboBox_E.SelectedIndex = comboBox_E.Items.Count - 1;
            txt_p_S.Enabled = false;
            txt_p_E.Enabled = false;
            txt_p_S.Text = null;
            txt_p_E.Text = null;
        }
        bool changeTxt;
        private string v;

        private void button3_Click(object sender, EventArgs e)
        {
            changeTxt = true;
            SelectedProductPopup obj = new SelectedProductPopup(this);
            obj.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            changeTxt = false;
            SelectedProductPopup obj = new SelectedProductPopup(this);
            obj.ShowDialog();
        }
        public void BinddingProduct(int id)
        {
            var product = SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == id);
            if (changeTxt == true)
            {
                txt_p_S.Text = product.Code;
            }
            else
            {
                txt_p_E.Text = product.Code;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            decimal[,] array =
      {
                { 7, 200 },
                { 9, 500}
            };
            CodeFileDLL.ExcelReport(dataGridView1, "รายงานสินค้าคงเหลือ "
                + Library.ConvertDateToThaiDate(dateTimePicker_S.Value), "สินค้า : " + txt_p_S.Text + " - " + txt_p_E.Text);
        }
    }
}
