using Pos_Management_System.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class CreatePOForm : Form
    {
        int _poCostType = 0;
        public CreatePOForm()
        {
            InitializeComponent();
        }
        class ObjForComboBox
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }
        private void CreatePOForm_Load(object sender, EventArgs e)
        {
            textBoxPODate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            listBox1.Items.Add("ผู้การ");

            List<Branch> branchs = new List<Branch>();
            branchs = SingletonBranch.Instance().Branchs.Where(w => w.Enable == true).OrderByDescending(w => w.Id).ToList();
            //List<ObjForComboBox> branchList = new List<ObjForComboBox>();
            //foreach (var item in branchs)
            //{
            //    branchList.Add(new ObjForComboBox()
            //    {
            //        Id = item.Id,
            //        Name = item.Name + " สาขา"+item.BranchNo
            //    });
            //}
            comboBoxBranch.DataSource = branchs;
            comboBoxBranch.DisplayMember = "Name";
            comboBoxBranch.ValueMember = "Id";
            textBoxBranchCode.Text = branchs.FirstOrDefault().BranchNo;

            List<BudgetYear> budgetYears = new List<BudgetYear>();
            budgetYears = SingletonPriority1.Instance().BudgetYears.Where(w => w.Enable == true).ToList();
            comboBoxBudgetYear.DataSource = budgetYears;
            comboBoxBudgetYear.DisplayMember = "Name";
            comboBoxBudgetYear.ValueMember = "Id";

            List<PaymentType> paymentTypes = new List<PaymentType>();
            paymentTypes = SingletonPriority1.Instance().PaymentTypes.Where(w => w.Enable == true).ToList();
            comboBoxPaymentType.DataSource = paymentTypes;
            comboBoxPaymentType.DisplayMember = "Name";
            comboBoxPaymentType.ValueMember = "Id";

            Branch branch = (Branch)comboBoxBranch.SelectedItem;
            BudgetYear budget = (BudgetYear)comboBoxBudgetYear.SelectedItem;
            string code = "";
            using (SSLsEntities db = new SSLsEntities())
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                var po = db.POHeader.Where(w => w.FKPaymentType != MyConstant.PaymentType.CashNow && w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth && w.FKBranch == branch.Id).Count() + 1;

                code = MyConstant.PrefixForGenerateCode.PurchaseOrder + "" + branch.Code + budget.CodeYear + DateTime.Now.ToString("MM") + MyConstant.POCreditOrCash.Credit + "/" + Library.GenerateCodeFormCount(po, 3);
                textBoxPONo.Text = code;
            }
            /// add row 1 datagrid
            dataGridView1.Rows.Add(1);
        }
        int colNo = 0;
        int colName = 2;
        int colQty = 3;
        int colUnit = 4;
        int colGift = 5;
        int colCostPerUnit = 6;
        int colCostVatPerUnit = 7;
        int colDiscount = 8;
        int colDiscountBath = 9;
        int colTotalBalance = 10;
        int colId = 11;
        /// <summary>
        /// เกิดการคีย์ ใน Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            List<ProductDetails> products = new List<ProductDetails>();
            products = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
            if (e.RowIndex < 0)
                return;
            try
            {
                ProductDetails p = new ProductDetails();
                string pNo = "0";
                decimal costCal = 0;
                switch (e.ColumnIndex)
                {
                    case 0:    // Tab  รหัสสินค้า
                        if (dataGridView1.Rows[e.RowIndex].Cells[colNo].Value != null)
                        {
                            pNo = dataGridView1.Rows[e.RowIndex].Cells[colNo].Value.ToString();
                        }

                        p = products.FirstOrDefault(w => w.Code == pNo);
                        if (p != null) // ถ้ามี
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[colId].Value = p.Id; // Id
                            dataGridView1.Rows[e.RowIndex].Cells[colName].Value = p.Products.ThaiName; // ชื่อ
                            dataGridView1.Rows[e.RowIndex].Cells[colUnit].Value = p.ProductUnit.Name; // หน่วย
                            dataGridView1.Rows[e.RowIndex].Cells[colQty].Value = "1"; //  จำนวน
                            dataGridView1.Rows[e.RowIndex].Cells[colGift].Value = "0"; // ของแถม
                            dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value = "0"; // ส่วนลด
                            dataGridView1.Rows[e.RowIndex].Cells[colDiscountBath].Value = "0.00"; // ส่วนลด บาท
                            dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(p.CostOnly); // ทุนเปล่า
                            dataGridView1.Rows[e.RowIndex].Cells[colCostVatPerUnit].Value = Library.ConvertDecimalToStringForm(p.CostAndVat); // ทุนรวมภาษี

                            if (_poCostType == MyConstant.POCostType.CostAndVat)
                            {
                                costCal = p.CostAndVat;
                            }
                            else if (_poCostType == MyConstant.POCostType.CostOnly)
                            {
                                costCal = p.CostOnly;
                            }
                            else
                            {
                                costCal = p.CostOnly;
                            }

                            dataGridView1.Rows[e.RowIndex].Cells[colTotalBalance].Value = Library.ConvertDecimalToStringForm(costCal); // ราคาสุทธิ
                            // set Focus index
                            //dataGridView1.Rows[e.RowIndex].Cells[colQty].Selected = true;               
                        }
                        else
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[colName].Value = "ไม่พบ";
                            dataGridView1.Rows[e.RowIndex].Cells[colUnit].Value = "-";
                        }
                        break;
                    case 3: // Tab จำนวน
                        pNo = dataGridView1.Rows[e.RowIndex].Cells[colNo].Value.ToString();
                        p = products.FirstOrDefault(w => w.Code == pNo);
                        decimal qty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                        //dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(p.CostOnly * qty); // ทุนเปล่า
                        //dataGridView1.Rows[e.RowIndex].Cells[colCostVatPerUnit].Value = Library.ConvertDecimalToStringForm(p.CostAndVat * qty); // ทุนรวมภาษี

                        costCal = 0;
                        if (_poCostType == MyConstant.POCostType.CostAndVat)
                        {
                            costCal = p.CostAndVat;
                        }
                        else if (_poCostType == MyConstant.POCostType.CostOnly)
                        {
                            costCal = p.CostOnly;
                        }
                        else
                        {
                            costCal = p.CostOnly;
                        }

                        dataGridView1.Rows[e.RowIndex].Cells[colTotalBalance].Value = Library.ConvertDecimalToStringForm(costCal * qty); // ราคาสุทธิ
                        RefreshConclude();
                        break;
                    case 6: // Tab ราคาทุนเปล่า
                        if (_poCostType == MyConstant.POCostType.CostAndVat)
                        {
                            costCal = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostVatPerUnit].Value.ToString());
                        }
                        else if (_poCostType == MyConstant.POCostType.CostOnly)
                        {
                            costCal = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value.ToString());
                        }
                        qty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                        // clear ส่วนลด และ เงินส่วนลด
                        dataGridView1.Rows[e.RowIndex].Cells[colDiscountBath].Value = "0";
                        dataGridView1.Rows[e.RowIndex].Cells[colDiscountBath].Value = "0.00";

                        dataGridView1.Rows[e.RowIndex].Cells[colTotalBalance].Value = Library.ConvertDecimalToStringForm(costCal * qty); // ราคาสุทธิ

                        //decimal costOnly = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value.ToString());
                        // เอาทุนนี้ไป หา vat
                        decimal costAndVat = (costCal * MyConstant.MyVat.Vat / 100) + costCal;
                        dataGridView1.Rows[e.RowIndex].Cells[colCostVatPerUnit].Value = Library.ConvertDecimalToStringForm(costAndVat);
                        //dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value = 0;
                        break;
                    case 7: // Tab ราคาทุนเปล่า + ภาษี
                        if (_poCostType == MyConstant.POCostType.CostAndVat)
                        {
                            costCal = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostVatPerUnit].Value.ToString());
                        }
                        else if (_poCostType == MyConstant.POCostType.CostOnly)
                        {
                            costCal = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value.ToString());
                        }
                        qty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                        // clear ส่วนลด และ เงินส่วนลด
                        dataGridView1.Rows[e.RowIndex].Cells[colDiscountBath].Value = "0";
                        dataGridView1.Rows[e.RowIndex].Cells[colDiscountBath].Value = "0.00";
                        dataGridView1.Rows[e.RowIndex].Cells[colTotalBalance].Value = Library.ConvertDecimalToStringForm(costCal * qty); // ราคาสุทธิ

                        //decimal costOnly = costCal - (costCal * MyConstant.MyVat.Vat / MyConstant.MyVat.VatRemove);
                        //dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(costOnly);
                        break;
                    case 8: // Tab ส่วนลด
                        string discountTxt = dataGridView1.Rows[e.RowIndex].Cells[colDiscount].Value.ToString();
                        string[] subString = Library.GetCheckBathOrPercent(discountTxt);
                        decimal discountBath = 0;
                        //decimal costOnly = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value.ToString());

                        pNo = dataGridView1.Rows[e.RowIndex].Cells[colNo].Value.ToString();
                        p = products.FirstOrDefault(w => w.Code == pNo);
                        costCal = 0;
                        if (_poCostType == MyConstant.POCostType.CostAndVat) // ทุนรวมภาษี
                        {
                            costCal = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostVatPerUnit].Value.ToString());
                        }
                        else if (_poCostType == MyConstant.POCostType.CostOnly) // ทุนเปล่า
                        {
                            costCal = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostPerUnit].Value.ToString());
                        }
                        //else
                        //{
                        //    costCal = p.CostOnly;
                        //}
                        /// คำนวนส่วนลด
                        qty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString()); // จำนวน
                        if (subString[1] == "บ") // ต้องการลดเป็น บาท
                        {
                            discountBath = decimal.Parse(subString[0]);

                        }
                        else if (subString[1] == "") // ต้องการลด %
                        {
                            decimal percent = decimal.Parse(subString[0]);
                            if (percent > 99)
                                MessageBox.Show("ส่วนลด " + percent + " % คุณแน่ใจหรือไม่ ?");
                            discountBath = Library.CalPercentByValue(costCal * qty, decimal.Parse(subString[0]));
                        }

                        // bind data
                        dataGridView1.Rows[e.RowIndex].Cells[colDiscountBath].Value = Library.ConvertDecimalToStringForm(discountBath); // ส่วนลดเมื่อแปลงเป็นเงิน จาก บ หรือ %
                        dataGridView1.Rows[e.RowIndex].Cells[colTotalBalance].Value = Library.ConvertDecimalToStringForm(qty * costCal - discountBath); // total balance
                        RefreshConclude();
                        break;
                    case 9:
                        break;
                    case 10:
                        RefreshConclude();
                        break;
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("พบบางสิ่งไม่ถูกต้อง");
            }
        }

        bool discountPercent = true;
        /// <summary>
        /// สรุปกรอบด้านล่าง (ยอดสรุป)
        /// </summary>
        private void RefreshConclude()
        {
            decimal total = decimal.Parse(textBoxTotal.Text); // รวม
            decimal totalDiscountInvoice = decimal.Parse(textBoxTotalDiscountInvoice.Text); // ลดท้ายบิล
            decimal totalAfterDis = decimal.Parse(textBoxTotalAfterDis.Text); // หลังหักลด
            decimal totalVat = decimal.Parse(textBoxTotalVat.Text); // ภาษี
            decimal totalBalance = decimal.Parse(textBoxTotalBalance.Text); // สุทธิ

            decimal totalUnVat = 0; // ยอดรวมสินค้าที่ยกเว้นภาษี
            decimal totalHasVat = 0; // ยอดรวมสินค้าที่มีภาษี

            decimal discountBath = 0;
            string discountInvoice = textBoxDiscountInvoice.Text; // ส่วนลดท้ายบิล

            total = 0; // รวมทั้งหมด
            decimal cost = 0;
            decimal costAndVat = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {
                    cost = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                    costAndVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostVatPerUnit].Value.ToString());
                }
                catch (Exception)
                {
                    cost = 0;
                    costAndVat = 0;
                }

                if (dataGridView1.Rows[i].Cells[colTotalBalance].Value == null)
                {
                    total += 0;
                }
                else
                {
                    total += decimal.Parse(dataGridView1.Rows[i].Cells[colTotalBalance].Value.ToString());
                    if (cost == costAndVat) // แสดงว่าเป็นสินค้ายกเว้นภาษี
                    {
                        totalUnVat += decimal.Parse(dataGridView1.Rows[i].Cells[colTotalBalance].Value.ToString());
                    }
                    else
                    {
                        totalHasVat += decimal.Parse(dataGridView1.Rows[i].Cells[colTotalBalance].Value.ToString());
                    }
                }
                if (i >= dataGridView1.Rows.Count - 2) break;
            }
            // binding ui
            // textBoxTotal.Text = Library.ConvertDecimalToStringForm(total);
            // bind discount
            // รวมทั้งหมด
            string[] subStr = Library.GetCheckBathOrPercent(discountInvoice);

            decimal percent = 0;

            if (subStr[1] == "บ")
            {
                discountPercent = false;
                discountBath = decimal.Parse(subStr[0]);
                // หา บาท ส่วนลด ของยอดมีภาษี
                //vatAfterDiscount = totalHasVat - discountBath;
                decimal totalBeforeVat = totalUnVat + (totalHasVat - (totalHasVat * MyConstant.MyVat.Vat / MyConstant.MyVat.VatRemove));
                percent = Library.GetPercentFromDiscountBath(discountBath, totalBeforeVat);
            }
            else
            {
                discountPercent = true;
                discountBath = Library.CalPercentByValue(totalUnVat + decimal.Parse(textBoxTotalHasVat.Text), decimal.Parse(subStr[0]));
                // หา % ส่วนลด ของยอดมีภาษี
                //vatAfterDiscount = totalHasVat - (totalHasVat * decimal.Parse(subStr[0]) / 100);
                percent = decimal.Parse(subStr[0]);
            }
            switch (_poCostType)
            {

                case 1: // ถ้าเป็นทุนเปล่า
                    // ยอดรวมทั้งหมด
                    textBoxTotal.Text = Library.ConvertDecimalToStringForm(total);
                    // ยอดรวมภาษี
                    textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(totalHasVat);
                    // ยอดรวมยกเว้นภาษี
                    textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(totalUnVat);
                    // ยอดส่วนลดท้ายบิล
                    textBoxTotalDiscountInvoice.Text = Library.ConvertDecimalToStringForm(discountBath);
                    // ยอดหลังหักส่วนลด
                    textBoxTotalAfterDis.Text = Library.ConvertDecimalToStringForm(total - discountBath);
                    decimal forVat = 0;
                    //if (percent != 0)
                    //{
                    //    forVat = decimal.Parse(textBoxTotalHasVat.Text) - Library.CalPercentByValue(decimal.Parse(textBoxTotalHasVat.Text), percent);
                    //}
                    //else
                    //{
                    //    forVat = decimal.Parse(textBoxTotalHasVat.Text) - discountBath;
                    //}
                    // ยอดมีภาษี - ยอดส่วนลดจากคิดเป็น % 
                    forVat = decimal.Parse(textBoxTotalHasVat.Text) - Library.CalPercentByValue(decimal.Parse(textBoxTotalHasVat.Text), percent);
                    // ภาษีมูลค่าเพิ่ม                    
                    textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(forVat * MyConstant.MyVat.Vat / 100);
                    // ยอดสุทธิ หลังหักลด + ภาษีมูลค่าเพิ่ม
                    textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(decimal.Parse(textBoxTotalAfterDis.Text) + decimal.Parse(textBoxTotalVat.Text));
                    break;
                case 2: // ถ้าเป็นทุนรวมภาษี
                    // ลดแบบ %
                    if (discountPercent)
                    {

                    }
                    decimal removeVat = Library.CalVatFromValue(totalHasVat);
                    // ยอดรวมทั้งหมด                
                    //textBoxTotal.Text = Library.ConvertDecimalToStringForm(totalUnVat + (totalHasVat - removeVat));
                    textBoxTotal.Text = Library.ConvertDecimalToStringForm(total);
                    // ยอดรวมภาษี
                    textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(totalHasVat - removeVat);
                    forVat = 0;
                    //if (percent != 0)
                    //{
                    //    forVat = decimal.Parse(textBoxTotalHasVat.Text) - Library.CalPercentByValue(decimal.Parse(textBoxTotalHasVat.Text), percent);
                    //}
                    //else
                    //{
                    //    forVat = decimal.Parse(textBoxTotalHasVat.Text) - discountBath;
                    //}
                    // ยอดมีภาษี - ยอดส่วนลดจากคิดเป็น % 
                    forVat = decimal.Parse(textBoxTotalHasVat.Text) - Library.CalPercentByValue(decimal.Parse(textBoxTotalHasVat.Text), percent);
                    //// ยอดรวมยกเว้นภาษี
                    textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(totalUnVat);
                    //// ยอดส่วนลดท้ายบิล
                    textBoxTotalDiscountInvoice.Text = Library.ConvertDecimalToStringForm(discountBath);
                    //// ยอดหลังหักส่วนลด
                    textBoxTotalAfterDis.Text = Library.ConvertDecimalToStringForm(decimal.Parse(textBoxTotalHasVat.Text) + decimal.Parse(textBoxTotalUnVat.Text) - discountBath);
                    // ภาษีมูลค่าเพิ่ม                    
                    textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(forVat * MyConstant.MyVat.Vat / 100);
                    // ยอดสุทธิ หลังหักลด + ภาษีมูลค่าเพิ่ม
                    textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(decimal.Parse(textBoxTotalAfterDis.Text) + decimal.Parse(textBoxTotalVat.Text));
                    break;
            }
            //textBoxTotalDiscountInvoice.Text = Library.ConvertDecimalToStringForm(discountBath);
            //totalAfterDis = total - discountBath;
            //textBoxTotalAfterDis.Text = Library.ConvertDecimalToStringForm(totalAfterDis);

            //VatType productVatTypeSelect = (VatType)comboBoxVatType.SelectedItem;
            //int vatType = productVatTypeSelect.Id;
            //decimal vat = 0;
            //if (vatType == MyConstant.VatType.NoVat) // ก่อน Vat
            //{
            //    vat = ((totalAfterDis - totalUnVat) * MyConstant.MyVat.Vat) / 100;
            //    textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(vat);
            //}
            //else if (vatType == MyConstant.VatType.HasVat) // รวม Vat
            //{
            // ยอดรวมทั้งบิล
            //textBoxTotal.Text = Library.ConvertDecimalToStringForm(totalHasVat + totalUnVat);
            //step:1. ยอดสุทธิ ได้จาก total - ส่วนลดท้ายบิล
            //textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(totalUnVat + totalHasVat);
            ////step:2. หายอดมีภาษีจาก totalHasVat ถอด Vat
            //decimal removeVat = Library.CalVatFromValue(totalHasVat);
            //textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(totalHasVat - removeVat);
            ////step:3. หายอดมีภาษี แล้วถอด vat
            //textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(removeVat);
            ////step:4. หายอด ยกเว้นภาษี
            //textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(totalUnVat);
            ////step:5. ยอดรวมหลังหักลด ได้จาก ยอดสินค้ามีภาษี + ยอดสินค้ายกเว้นภาษี 
            //textBoxTotalAfterDis.Text = Library.ConvertDecimalToStringForm(decimal.Parse(textBoxTotalHasVat.Text) + decimal.Parse(textBoxTotalUnVat.Text) - discountBath);
            // เอาหลังหักลด-ยอดยกเว้นภาษี มา หา vat 
            //decimal vatFromDis = (totalAfterDis - totalUnVat) * MyConstant.MyVat.Vat / MyConstant.MyVat.VatRemove;

            //decimal valueNoVat = totalAfterDis - vatFromDis; // ยอด - ภาษี

            //}
            //else
            //{
            //    textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(0);
            //}

            //totalAfterDis = decimal.Parse(textBoxTotalAfterDis.Text); // หลังหักลด
            //totalVat = decimal.Parse(textBoxTotalVat.Text); // ภาษี
            //textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(totalAfterDis + totalVat); // ยอดสุทธิ
            //textBoxTotalHasVat.Text = Library.ConvertDecimalToStringForm(totalHasVat); // ยอดมีภาษี
            //textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(totalUnVat); // ยกเว้นภาษี
        }
        /// <summary>
        /// Tab ส่วนลดท้ายบิล
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDiscountInvoice_Validated(object sender, EventArgs e)
        {
            RefreshConclude();
        }

        /// <summary>
        /// เลือก ประเภทภาษี VatType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxVatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshConclude();
        }

        private void buttonSearchVendor_Click(object sender, EventArgs e)
        {
            SelectedVendorPopup dwv = new SelectedVendorPopup(this);
            dwv.ShowDialog();
        }
        int _VendorId = 0;
        /// <summary>
        /// เลือกเจ้าหนี้ / ผู้จัดจำหน่าย
        /// </summary>
        /// <param name="id"></param>
        public void BinddingVendor(int id)
        {
            _VendorId = id;
            List<Vendor> vendors = SingletonVender.Instance().Vendors;
            var data = vendors.SingleOrDefault(w => w.Id == id);
            textBoxVendorCode.Text = data.Code;
            textBoxVendorName.Text = data.Name;
            textBoxVendorAddress.Text = data.Address;
            textBoxVendorTel.Text = data.Tel;
            textBoxVendorFax.Text = data.Fax;
            groupBoxPOProducts.Text = "เพิ่มรายการสินค้า (" + data.POCostType.Name + ")";
            this._poCostType = data.FKPOCostType;
        }
        /// <summary>
        /// Enter VendorCode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxVendorCode_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string vendorCode = textBoxVendorCode.Text.Trim();
                    Vendor data = SingletonVender.Instance().Vendors.SingleOrDefault(w => w.Enable == true && w.Code == vendorCode);
                    if (data == null)
                    {
                        _VendorId = 0;

                        textBoxVendorCode.Text = "";
                        textBoxVendorName.Text = "";
                        textBoxVendorAddress.Text = "";
                        textBoxVendorTel.Text = "";
                        textBoxVendorFax.Text = "";
                        groupBoxPOProducts.Text = "เพิ่มรายการสินค้า (กรุณาเลือกผู้จัดจำหน่าย)";
                        this._poCostType = 0;
                    }
                    else
                    {
                        BinddingVendor(data.Id);
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Click ค้นหา
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            switch (e.ColumnIndex)
            {
                case 1:
                    // show dialog products
                    //DialogWindowsProducts objInstance = new DialogWindowsProducts(this);
                    //objInstance.ShowDialog();
                    SelectedProductPopup obj = new SelectedProductPopup(this);
                    obj.ShowDialog();
                    break;
            }
        }
        /// <summary>
        /// Bindding After Choose Product
        /// </summary>
        /// <param name="id"></param>
        public void BinddingProductChoose(int id)
        {
            List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
            var product = products.SingleOrDefault(w => w.Id == id);
            // start bind ui
            int currentRow = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows[currentRow].Cells[colId].Value = product.Id;
            dataGridView1.Rows[currentRow].Cells[colNo].Value = product.Code;
            dataGridView1.Rows[currentRow].Cells[colName].Value = product.Products.ThaiName;
            dataGridView1.Rows[currentRow].Cells[colQty].Value = 1.00;
            dataGridView1.Rows[currentRow].Cells[colUnit].Value = product.ProductUnit.Name;
            dataGridView1.Rows[currentRow].Cells[colGift].Value = 0.00;
            dataGridView1.Rows[currentRow].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostOnly);
            dataGridView1.Rows[currentRow].Cells[colCostVatPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostAndVat);
            dataGridView1.Rows[currentRow].Cells[colDiscount].Value = 0.00;
            dataGridView1.Rows[currentRow].Cells[colDiscountBath].Value = "0.00";
            if (_poCostType == MyConstant.POCostType.CostOnly)
            {
                dataGridView1.Rows[currentRow].Cells[colTotalBalance].Value = product.CostOnly * 1;
            }
            else
            {
                dataGridView1.Rows[currentRow].Cells[colTotalBalance].Value = product.CostAndVat * 1;
            }
            //dataGridView1.Rows[currentRow].Cells[colQty].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[colQty];
            dataGridView1.CurrentCell.Selected = true;
            //dataGridView1.BeginEdit(true);
        }
        /// <summary>
        /// บันทึก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            RefreshConclude();
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
        // ถ้าลด percent จะเป็น true
        void SaveCommit()
        {
            try
            {
                PaymentType payment = (PaymentType)comboBoxPaymentType.SelectedItem;
                Branch branch = (Branch)comboBoxBranch.SelectedItem;
                BudgetYear budget = (BudgetYear)comboBoxBudgetYear.SelectedItem;
                string code = "";
                using (SSLsEntities db = new SSLsEntities())
                {
                    int currentYear = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    int po;
                    if (payment.Id == MyConstant.PaymentType.CashNow) // ถ้าเอา
                    {
                        po = db.POHeader.Where(w => w.FKPaymentType == MyConstant.PaymentType.CashNow && w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth && w.FKBranch == branch.Id).Count() + 1;
                        code = MyConstant.PrefixForGenerateCode.PurchaseOrder + "" + branch.Code + budget.CodeYear + DateTime.Now.ToString("MM") + MyConstant.POCreditOrCash.CashNow + "/" + Library.GenerateCodeFormCount(po, 3);
                    }
                    else
                    {
                        po = db.POHeader.Where(w => w.FKPaymentType != MyConstant.PaymentType.CashNow && w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth && w.FKBranch == branch.Id).Count() + 1;
                        code = MyConstant.PrefixForGenerateCode.PurchaseOrder + "" + branch.Code + budget.CodeYear + DateTime.Now.ToString("MM") + MyConstant.POCreditOrCash.Credit + "/" + Library.GenerateCodeFormCount(po, 3);
                    }

                    POHeader hd = new POHeader();
                    hd.FKBranch = branch.Id;
                    hd.FKVender = _VendorId;
                    hd.PONo = code;
                    hd.PODate = DateTime.Now;
                    hd.FKBudgetYear = budget.Id;
                    hd.ReferenceNo = textBoxReferNo.Text;
                    hd.FKPOStatus = MyConstant.POStatus.NotRCV;
                    hd.DueDate = dateTimePickerDeliExp.Value;
                    hd.POExpire = dateTimePickerPOExp.Value;

                    hd.FKPaymentType = payment.Id;
                    hd.CreateBy = SingletonAuthen.Instance().Id;
                    hd.CreateDate = DateTime.Now;
                    hd.DiscountInput = textBoxDiscountInvoice.Text;

                    hd.Description = textBoxRemark.Text;
                    //hd.ApproveBy = "admin";
                    hd.TotalPrice = decimal.Parse(textBoxTotal.Text);
                    hd.TotalHasVat = decimal.Parse(textBoxTotalHasVat.Text);
                    hd.TotalUnVat = decimal.Parse(textBoxTotalUnVat.Text);
                    hd.TotalDiscount = decimal.Parse(textBoxTotalDiscountInvoice.Text);
                    hd.TotalPriceDiscount = decimal.Parse(textBoxTotalAfterDis.Text);
                    hd.TotalVat = decimal.Parse(textBoxTotalVat.Text);
                    hd.TotalBalance = decimal.Parse(textBoxTotalBalance.Text);

                    if (discountPercent)
                    {
                        hd.DiscountBath = Library.ConvertDecimalToStringForm(Library.CalPercentByValue(hd.TotalHasVat + hd.TotalUnVat, decimal.Parse(textBoxDiscountInvoice.Text))); // แปลงจากคีย์ % เป็น บาท
                        hd.DiscountPercent = textBoxDiscountInvoice.Text + " %";
                    }
                    else
                    {
                        hd.DiscountBath = textBoxDiscountInvoice.Text.Replace("บ", "");
                        //hd.DiscountPercent = hd.TotalDiscount + ""; // แปลงเงินบาทเป็น % 
                        hd.DiscountPercent = Library.GetPercentFromDiscountBath(decimal.Parse(hd.DiscountBath), hd.TotalHasVat + hd.TotalUnVat) + " %";
                    }

                    hd.Enable = true;
                    hd.UpdateBy = SingletonAuthen.Instance().Id;
                    hd.UpdateDate = DateTime.Now;
                    List<PODetail> details = new List<PODetail>();
                    decimal costOnly = 0;
                    decimal costAndVat = 0;
                    decimal totalCost = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        costOnly = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                        costAndVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostVatPerUnit].Value.ToString());
                        if (_poCostType == MyConstant.POCostType.CostOnly)
                        {
                            totalCost = costOnly * decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                        }
                        else
                        {
                            totalCost = costAndVat * decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                        }
                        details.Add(new PODetail()
                        {
                            Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString()),
                            GiftQty = decimal.Parse(dataGridView1.Rows[i].Cells[colGift].Value.ToString()),
                            CostOnly = costOnly,
                            CostAndVat = costAndVat,
                            DiscountInput = dataGridView1.Rows[i].Cells[colDiscount].Value.ToString(),
                            DiscountBath = decimal.Parse(dataGridView1.Rows[i].Cells[colDiscountBath].Value.ToString()),
                            TotalCost = totalCost - decimal.Parse(dataGridView1.Rows[i].Cells[colDiscountBath].Value.ToString()),
                            CreateDate = DateTime.Now,
                            CreateBy = SingletonAuthen.Instance().Id,
                            UpdateDate = DateTime.Now,
                            UpdateBy = SingletonAuthen.Instance().Id,
                            Enable = true,
                            FKProductDetail = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString())
                        });

                        if (i >= dataGridView1.Rows.Count - 2) break;
                    }
                    hd.TotalQty = details.Sum(w => w.Qty);
                    hd.TotalGift = details.Sum(w => w.GiftQty);
                    hd.PODetail = details;

                    //POGenerate pGen = new POGenerate();
                    //pGen.Used = true;
                    //pGen.Code = hd.PONo;
                    //pGen.Description = "Auto Gen";
                    //pGen.CreateDate = DateTime.Now;
                    //pGen.CreateBy = SingletonAuthen.Instance().Id;
                    //pGen.UpdateDate = DateTime.Now;
                    //pGen.UpdateBy = SingletonAuthen.Instance().Id;
                    //pGen.Enable = true;
                    //pGen.FKBranch = branch.Id;
                    //pGen.FKBudgetYear = budget.Id;

                    //db.POGenerate.Add(pGen);

                    db.POHeader.Add(hd);
                    db.SaveChanges();
                    // get last po for get Id
                    int lastPO = db.POHeader.Where(w => w.Enable == true && w.CreateBy == hd.CreateBy).OrderByDescending(w => w.CreateDate).FirstOrDefault().Id;
                    MainReportViewer mr = new MainReportViewer(this, lastPO);
                    mr.ShowDialog();
                }

                ResetNewCreatePO();
            }
            catch (Exception)
            {
                MessageBox.Show("เกิดข้อผิดพลาด กรุณาลองใหม่");
            }
        }
        /// <summary>
        /// Reset หลังจากสร้างเสร็จ
        /// </summary>
        void ResetNewCreatePO()
        {
            PaymentType paytype = (PaymentType)comboBoxPaymentType.SelectedItem;
            if (paytype.Id == MyConstant.PaymentType.CashNow)
            {
                Branch branch = (Branch)comboBoxBranch.SelectedItem;
                BudgetYear budget = (BudgetYear)comboBoxBudgetYear.SelectedItem;
                string code = "";
                using (SSLsEntities db = new SSLsEntities())
                {
                    int currentYear = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    var po = db.POHeader.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth && w.FKBranch == branch.Id && w.FKPaymentType == MyConstant.PaymentType.CashNow).Count() + 1;

                    code = MyConstant.PrefixForGenerateCode.PurchaseOrder + "" + branch.Code + budget.CodeYear + DateTime.Now.ToString("MM") + MyConstant.POCreditOrCash.CashNow + "/" + Library.GenerateCodeFormCount(po, 3);
                    textBoxPONo.Text = code;
                }
            }
            else
            {
                Branch branch = (Branch)comboBoxBranch.SelectedItem;
                BudgetYear budget = (BudgetYear)comboBoxBudgetYear.SelectedItem;
                string code = "";
                using (SSLsEntities db = new SSLsEntities())
                {
                    int currentYear = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    var po = db.POHeader.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth && w.FKBranch == branch.Id && w.FKPaymentType != MyConstant.PaymentType.CashNow).Count() + 1;

                    code = MyConstant.PrefixForGenerateCode.PurchaseOrder + "" + branch.Code + budget.CodeYear + DateTime.Now.ToString("MM") + MyConstant.POCreditOrCash.Credit + "/" + Library.GenerateCodeFormCount(po, 3);
                    textBoxPONo.Text = code;
                }
            }

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            textBoxDiscountInvoice.Text = "0.00";
            textBoxRemark.Text = "";
            textBoxReferNo.Text = "";

            textBoxTotal.Text = "0.00";
            textBoxTotalHasVat.Text = "0.00";
            textBoxTotalUnVat.Text = "0.00";
            textBoxTotalDiscountInvoice.Text = "0.00";
            textBoxTotalAfterDis.Text = "0.00";
            textBoxTotalVat.Text = "0.00";
            textBoxTotalBalance.Text = "0.00";
            /// add row 1 datagrid
            dataGridView1.Rows.Add(1);

        }
        /// <summary>
        /// เลือก สาขาเสร็จ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxBranch_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Branch branch = (Branch)comboBoxBranch.SelectedItem;
            //BudgetYear budget = (BudgetYear)comboBoxBudgetYear.SelectedItem;
            //string code = "";
            //using (SSLsEntities db = new SSLsEntities())
            //{
            //    int currentYear = DateTime.Now.Year;
            //    int currentMonth = DateTime.Now.Month;
            //    var po = db.POHeader.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth && w.FKBranch == branch.Id).Count() + 1;

            //    code = MyConstant.PrefixForGenerateCode.PurchaseOrder + "" + branch.Code + budget.CodeYear + DateTime.Now.ToString("MM") + "/" + Library.GenerateCodeFormCount(po, 3);
            //    textBoxPONo.Text = code;
            //    textBoxBranchCode.Text = branch.BranchNo;
            //}
            PaymentType paytype = (PaymentType)comboBoxPaymentType.SelectedItem;
            Branch branch = (Branch)comboBoxBranch.SelectedItem;
            BudgetYear budget = (BudgetYear)comboBoxBudgetYear.SelectedItem;
            if (paytype.Id == MyConstant.PaymentType.CashNow)
            {

                string code = "";
                using (SSLsEntities db = new SSLsEntities())
                {
                    int currentYear = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    var po = db.POHeader.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth && w.FKBranch == branch.Id && w.FKPaymentType == MyConstant.PaymentType.CashNow).Count() + 1;

                    code = MyConstant.PrefixForGenerateCode.PurchaseOrder + "" + branch.Code + budget.CodeYear + DateTime.Now.ToString("MM") + MyConstant.POCreditOrCash.CashNow + "/" + Library.GenerateCodeFormCount(po, 3);
                    textBoxPONo.Text = code;
                }
            }
            else
            {

                string code = "";
                using (SSLsEntities db = new SSLsEntities())
                {
                    int currentYear = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    var po = db.POHeader.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth && w.FKBranch == branch.Id && w.FKPaymentType != MyConstant.PaymentType.CashNow).Count() + 1;

                    code = MyConstant.PrefixForGenerateCode.PurchaseOrder + "" + branch.Code + budget.CodeYear + DateTime.Now.ToString("MM") + MyConstant.POCreditOrCash.Credit + "/" + Library.GenerateCodeFormCount(po, 3);
                    textBoxPONo.Text = code;
                }
            }
            textBoxBranchCode.Text = branch.BranchNo;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// Enter ส่วนลด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDiscountInvoice_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    RefreshConclude();
                    break;
                default:
                    break;
            }
        }

        private void comboBoxBudgetYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PaymentType paytype = (PaymentType)comboBoxPaymentType.SelectedItem;
            if (paytype.Id == MyConstant.PaymentType.CashNow)
            {
                Branch branch = (Branch)comboBoxBranch.SelectedItem;
                BudgetYear budget = (BudgetYear)comboBoxBudgetYear.SelectedItem;
                string code = "";
                using (SSLsEntities db = new SSLsEntities())
                {
                    int currentYear = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    var po = db.POHeader.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth && w.FKBranch == branch.Id && w.FKPaymentType == MyConstant.PaymentType.CashNow).Count() + 1;

                    code = MyConstant.PrefixForGenerateCode.PurchaseOrder + "" + branch.Code + budget.CodeYear + DateTime.Now.ToString("MM") + MyConstant.POCreditOrCash.CashNow + "/" + Library.GenerateCodeFormCount(po, 3);
                    textBoxPONo.Text = code;
                }
            }
            else
            {
                Branch branch = (Branch)comboBoxBranch.SelectedItem;
                BudgetYear budget = (BudgetYear)comboBoxBudgetYear.SelectedItem;
                string code = "";
                using (SSLsEntities db = new SSLsEntities())
                {
                    int currentYear = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    var po = db.POHeader.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth && w.FKBranch == branch.Id && w.FKPaymentType != MyConstant.PaymentType.CashNow).Count() + 1;

                    code = MyConstant.PrefixForGenerateCode.PurchaseOrder + "" + branch.Code + budget.CodeYear + DateTime.Now.ToString("MM") + MyConstant.POCreditOrCash.Credit + "/" + Library.GenerateCodeFormCount(po, 3);
                    textBoxPONo.Text = code;
                }
            }
        }
        /// <summary>
        /// Enter Cell
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
                    if (col == colNo)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                        dataGridView1.BeginEdit(true);
                    }
                    else if (col == colQty)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colGift];
                        dataGridView1.BeginEdit(true);
                    }
                    else if (col == colGift)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colCostPerUnit];
                        dataGridView1.BeginEdit(true);
                    }
                    else if (col == colCostPerUnit)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colDiscount];
                        dataGridView1.BeginEdit(true);
                    }
                    //else if (col == colCostVatPerUnit)
                    //{
                    //    dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colDiscount];
                    //    dataGridView1.BeginEdit(true);
                    //}
                    else if (col == colDiscount)
                    {
                        dataGridView1.Rows.Add(1);
                        dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colNo];
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
        /// <summary>
        /// click cell datagrid เชคการเลือกผู้จำหน่าย
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // check
            if (_VendorId == 0)
            {
                MessageBox.Show("กรุณาเลือกผู้จำหน่าย");
            }
        }
        /// <summary>
        /// เลือก วิธีชำระเงิน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxPaymentType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PaymentType paytype = (PaymentType)comboBoxPaymentType.SelectedItem;
            if (paytype.Id == MyConstant.PaymentType.CashNow)
            {
                Branch branch = (Branch)comboBoxBranch.SelectedItem;
                BudgetYear budget = (BudgetYear)comboBoxBudgetYear.SelectedItem;
                string code = "";
                using (SSLsEntities db = new SSLsEntities())
                {
                    int currentYear = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    var po = db.POHeader.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth && w.FKBranch == branch.Id && w.FKPaymentType == MyConstant.PaymentType.CashNow).Count() + 1;

                    code = MyConstant.PrefixForGenerateCode.PurchaseOrder + "" + branch.Code + budget.CodeYear + DateTime.Now.ToString("MM") + MyConstant.POCreditOrCash.CashNow + "/" + Library.GenerateCodeFormCount(po, 3);
                    textBoxPONo.Text = code;
                }
            }
            else
            {
                Branch branch = (Branch)comboBoxBranch.SelectedItem;
                BudgetYear budget = (BudgetYear)comboBoxBudgetYear.SelectedItem;
                string code = "";
                using (SSLsEntities db = new SSLsEntities())
                {
                    int currentYear = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    var po = db.POHeader.Where(w => w.CreateDate.Year == currentYear && w.CreateDate.Month == currentMonth && w.FKBranch == branch.Id && w.FKPaymentType != MyConstant.PaymentType.CashNow).Count() + 1;

                    code = MyConstant.PrefixForGenerateCode.PurchaseOrder + "" + branch.Code + budget.CodeYear + DateTime.Now.ToString("MM") + MyConstant.POCreditOrCash.Credit + "/" + Library.GenerateCodeFormCount(po, 3);
                    textBoxPONo.Text = code;
                }
            }
        }
    }
}
