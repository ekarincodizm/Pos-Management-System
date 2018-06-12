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
    /// <summary>
    /// โอนสินค้าออกจากหน้าร้าน ไปยังสาขาอื่น
    /// </summary>
    public partial class TransferOutForm : Form
    {
        public TransferOutForm()
        {
            InitializeComponent();
        }
        StoreFrontTransferOut hd;
        /// <summary>
        /// จากการแก้ไข
        /// </summary>
        /// <param name="transferOutListForm"></param>
        /// <param name="code"></param>
        public TransferOutForm(TransferOutListForm transferOutListForm, string code)
        {
            InitializeComponent();
            using (SSLsEntities db = new SSLsEntities())
            {
                hd = db.StoreFrontTransferOut.FirstOrDefault(w => w.Code == code);
                if (hd.Enable == false)
                {
                    button2.Enabled = false;
                }
                else if (hd.ConfirmDate != null)
                {
                    button2.Enabled = false;
                }
                // bindding
                BinddingBranchSelected(hd.Branch);
                textBoxTOCode.Text = hd.Code;
                textBoxTODate.Text = Library.ConvertDateToThaiDate(hd.CreateDate);
                textBoxDesc.Text = hd.Remark;
                textBoxQtyUnit.Text = Library.ConvertDecimalToStringForm(hd.TotalQtyUnit);
                textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(hd.TotalBalance);
                this.transferOutListForm = transferOutListForm;
                this.code = code;
                foreach (var item in hd.StoreFrontTransferOutDtl.Where(w => w.Enable == true).ToList())
                {
                    dataGridView1.Rows.Add
                        (
                            item.FKProductDetails,
                            item.ProductDetails.Code,
                            "",
                            item.ProductDetails.Products.ThaiName,
                            item.ProductDetails.PackSize,
                            item.ProductDetails.ProductUnit.Name,
                            Library.ConvertDecimalToStringForm(item.Qty),
                            Library.ConvertDecimalToStringForm(item.CostPerUnit),
                            Library.ConvertDecimalToStringForm(item.TotalPrice),
                            item.ProductDetails.Products.ProductVatType.Name,
                            item.Description
                        );
                }
                //dataGridView1.Rows.Add("");
            }
            
        }

        int colId = 0;
        int colCode = 1;
        int colSearch = 2;
        int colName = 3;
        int colUnit = 4;
        int colpz = 5;
        int colQty = 6;
        int colCostPerUnit = 7;
        int colTotalCost = 8;
        int colVatType = 9;
        int colLocation = 10;
        private void TransferOutForm_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(1);
            //var branchs = Singleton.SingletonBranch.Instance().Branchs;
            //// สาขา
            //var bList = new List<TransformBranch>();
            //foreach (var item in branchs)
            //{
            //    bList.Add(new TransformBranch()
            //    {
            //        Id = item.Id,
            //        Name = item.Name + " " + item.BranchNo
            //    });
            //}

            //textBoxBranchName.Text = branchs.FirstOrDefault().Address + " โทร." + branchs.FirstOrDefault().Tel;
            using (SSLsEntities db = new SSLsEntities())
            {
                int countTO = db.StoreFrontTransferOut.Where(w => w.CreateDate.Year == DateTime.Now.Year && w.CreateDate.Month == DateTime.Now.Month).Count() + 1;
                string toCode = MyConstant.PrefixForGenerateCode.TransferOut + Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(countTO, 4);
                textBoxTOCode.Text = toCode;
                textBoxTODate.Text = Library.ConvertDateToThaiDate(DateTime.Now);
                var branch = db.Branch.SingleOrDefault(w => w.Id == MyConstant.MyBranch._01);
                BinddingBranchSelected(branch);
            }

        }
        class TransformBranch
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        /// <summary>
        /// สรุปยอด การทำโอน
        /// </summary>
        void BinddingTotal()
        {
            decimal totalQty = 0;
            //decimal totalUnVat = 0;
            //decimal totalHasVat = 0;
            decimal total = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var code = dataGridView1.Rows[i].Cells[colCode].Value;
                if (code == null)
                {
                    continue;
                }
                code = code.ToString();
                //ProductDetails getProd = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == code.ToString());
                totalQty += decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                decimal price = decimal.Parse(dataGridView1.Rows[i].Cells[colTotalCost].Value.ToString());
                total += price;
                //if (getProd.Products.FKProductVatType == MyConstant.ProductVatType.UnVat)
                //{
                //    // ถ้าเป็น สินค้า ยกเว้นภาษี 
                //    totalUnVat += price;
                //}
                //else
                //{
                //    totalHasVat += price;
                //}

                //if (i >= dataGridView1.Rows.Count - 2) break;
            }
            // ถอด vat
            //decimal vat = Library.CalVatFromValue(totalHasVat);
            textBoxQtyUnit.Text = Library.ConvertDecimalToStringForm(totalQty);
            //textBoxTotalUnVat.Text = Library.ConvertDecimalToStringForm(totalUnVat);
            //textBoxTotalBeforeVat.Text = Library.ConvertDecimalToStringForm(totalUnVat);
            //textBoxTotalBeforeVat.Text = Library.ConvertDecimalToStringForm(totalHasVat - vat);
            //textBoxTotalVat.Text = Library.ConvertDecimalToStringForm(vat);
            textBoxTotalBalance.Text = Library.ConvertDecimalToStringForm(total);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// บันทึก ทำโอนสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?", "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
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
                if (hd != null)
                {
                    // พบการแก้ไข
                    StoreFrontTransferOut sf = db.StoreFrontTransferOut.SingleOrDefault(w => w.Code == hd.Code);
                    sf.Remark = textBoxDesc.Text;
                    sf.FK2Branch = _BranchId;
                    foreach (var item in sf.StoreFrontTransferOutDtl.Where(w=>w.Enable == true).ToList())
                    {
                        item.Enable = false;
                        db.Entry(item).State = EntityState.Modified;
                    }
                   
                    List<StoreFrontTransferOutDtl> details = new List<StoreFrontTransferOutDtl>();
                    StoreFrontTransferOutDtl detail;
                    decimal totalPiece = 0;
                    decimal totalUnit = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        var code = dataGridView1.Rows[i].Cells[colCode].Value;
                        if (code == null)
                        {
                            continue;
                        }
                        code = code.ToString();

                        detail = new StoreFrontTransferOutDtl();
                        detail.Enable = true;
                        detail.Description = dataGridView1.Rows[i].Cells[colLocation].Value.ToString();
                        detail.CreateDate = DateTime.Now;
                        detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        detail.UpdateDate = DateTime.Now;
                        detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        detail.FKProductDetails = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                        var prodDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == detail.FKProductDetails);

                        detail.Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                        detail.CostPerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                        detail.BeforeVat = 0;
                        detail.Vat = 0;
                        detail.TotalPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colTotalCost].Value.ToString());
                        detail.SellPricePerUnit = prodDtl.SellPrice;
                        details.Add(detail);
                        totalPiece += (detail.Qty * prodDtl.PackSize);
                        totalUnit += detail.Qty;
                        sf.StoreFrontTransferOutDtl.Add(detail);
                    }
                    sf.TotalQty = totalPiece;
                    sf.TotalQtyUnit = totalUnit;
                    sf.TotalUnVat = 0;
                    sf.TotalBeforeVat = 0;
                    sf.TotalVat = 0;
                    sf.FKEmployee = _EmployeeId;
           
                    sf.TotalBalance = decimal.Parse(textBoxTotalBalance.Text);
                    db.Entry(sf).State = EntityState.Modified;

                    db.SaveChanges();
                    transferOutListForm.ReloadGrid();
                    this.Dispose();
                }
                else
                {
                    StoreFrontTransferOut sf = new StoreFrontTransferOut();
                    sf.Code = textBoxTOCode.Text;
                    sf.Enable = true;
                    sf.Remark = textBoxDesc.Text;
                    sf.CreateDate = DateTime.Now;
                    sf.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    sf.UpdateDate = DateTime.Now;
                    sf.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    sf.FK2Branch = _BranchId;
                    List<StoreFrontTransferOutDtl> details = new List<StoreFrontTransferOutDtl>();
                    StoreFrontTransferOutDtl detail;
                    decimal totalPiece = 0;
                    decimal totalUnit = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        var code = dataGridView1.Rows[i].Cells[colCode].Value;
                        if (code == null)
                        {
                            continue;
                        }
                        code = code.ToString();

                        detail = new StoreFrontTransferOutDtl();
                        detail.Enable = true;
                        detail.Description = dataGridView1.Rows[i].Cells[colLocation].Value.ToString();
                        detail.CreateDate = DateTime.Now;
                        detail.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                        detail.UpdateDate = DateTime.Now;
                        detail.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                        detail.FKProductDetails = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                        var prodDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == detail.FKProductDetails);

                        detail.Qty = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                        detail.CostPerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colCostPerUnit].Value.ToString());
                        detail.BeforeVat = 0;
                        detail.Vat = 0;
                        detail.TotalPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colTotalCost].Value.ToString());
                        detail.SellPricePerUnit = prodDtl.SellPrice;
                        details.Add(detail);
                        totalPiece += (detail.Qty * prodDtl.PackSize);
                        totalUnit += detail.Qty;
                    }

                    sf.TotalQty = totalPiece;
                    sf.TotalQtyUnit = totalUnit;
                    sf.TotalUnVat = 0;
                    sf.TotalBeforeVat = 0;
                    sf.TotalVat = 0;
                    sf.FKEmployee = _EmployeeId;
                    sf.StoreFrontTransferOutDtl = details;
                    sf.TotalBalance = decimal.Parse(textBoxTotalBalance.Text);
                    db.StoreFrontTransferOut.Add(sf);
                    db.SaveChanges();
                    textBoxTotalBalance.Text = "0.00";
                    textBoxDesc.Text = "";
                    textBoxQtyUnit.Text = "0.00";
                    int countTO = db.StoreFrontTransferOut.Where(w => w.CreateDate.Year == DateTime.Now.Year && w.CreateDate.Month == DateTime.Now.Month).Count() + 1;
                    string toCode = MyConstant.PrefixForGenerateCode.TransferOut + Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(countTO, 4);
                    textBoxTOCode.Text = toCode;
                    textBoxTODate.Text = Library.ConvertDateToThaiDate(DateTime.Now);

                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                    dataGridView1.Rows.Add(1);
                }

            }
        }

        /// <summary>
        /// ค้นหา Branch ที่จะโอนไป
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SelectedBranchPopup obj = new SelectedBranchPopup(this);
            obj.ShowDialog();
        }
        /// <summary>
        /// Bindding สาขาที่เลือก
        /// </summary>
        int _BranchId;
        public void BinddingBranchSelected(Branch send)
        {
            textBoxBrachCode.Text = send.Code;
            textBoxBranchName.Text = send.Name;
            _BranchId = send.Id;
        }

        /// <summary>
        /// Bindding สินค้า เมื่อเลือกสินค้า
        /// </summary>
        /// <param name="id"></param>
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
            dataGridView1.Rows[currentRow].Cells[colpz].Value = product.PackSize;
            dataGridView1.Rows[currentRow].Cells[colQty].Value = 1.00;

            dataGridView1.Rows[currentRow].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(product.CostOnly);
            dataGridView1.Rows[currentRow].Cells[colTotalCost].Value = Library.ConvertDecimalToStringForm(product.CostOnly);
            dataGridView1.Rows[currentRow].Cells[colVatType].Value = product.Products.ProductVatType.Name;
            dataGridView1.Rows[currentRow].Cells[colLocation].Value = "หน้าร้าน";
            //dataGridView1.Rows[currentRow].Cells[colQty].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[colQty];
            dataGridView1.BeginEdit(true);

        }
        /// <summary>
        /// เลือกคนหยิบ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            SelectedEmployeePopup obj = new SelectedEmployeePopup(this);
            obj.ShowDialog();
        }
        int? _EmployeeId = null;
        private TransferOutListForm transferOutListForm;
        private string code;

        public void BinddingEmployee(Employee send)
        {
            _EmployeeId = send.Id;
            textBoxEmpCode.Text = send.Code;
            textBoxEmpName.Text = send.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// click ค้นหาสินค้า ใน grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.row = e.RowIndex;
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
        int row;
        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                int col = dataGridView1.CurrentCell.ColumnIndex;
                var code = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
                ProductDetails getProd = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == code);
                switch (e.KeyCode)
                {
                    case Keys.Enter:

                        if (col == colCode)
                        {
                            if (getProd != null)
                            {
                                dataGridView1.Rows[row].Cells[colId].Value = getProd.Id;
                                dataGridView1.Rows[row].Cells[colName].Value = getProd.Products.ThaiName;
                                dataGridView1.Rows[row].Cells[colUnit].Value = getProd.ProductUnit.Name;
                                dataGridView1.Rows[row].Cells[colpz].Value = getProd.PackSize;
                                dataGridView1.Rows[row].Cells[colQty].Value = "1.00";
                                dataGridView1.Rows[row].Cells[colCostPerUnit].Value = Library.ConvertDecimalToStringForm(getProd.CostOnly);
                                dataGridView1.Rows[row].Cells[colTotalCost].Value = Library.ConvertDecimalToStringForm(getProd.CostOnly);
                                dataGridView1.Rows[row].Cells[colVatType].Value = getProd.Products.ProductVatType.Name;
                                dataGridView1.Rows[row].Cells[colLocation].Value = "หน้าร้าน";
                                BinddingTotal();
                                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colQty];
                                dataGridView1.BeginEdit(true);
                            }
                            else
                            {
                                MessageBox.Show("ไม่พบ");
                                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colCode];
                                dataGridView1.BeginEdit(true);
                            }


                        }
                        else if (col == colQty)
                        {

                            if (row == dataGridView1.Rows.Count - 1)
                            {
                                //row = row - 1;
                                dataGridView1.Rows.Add(1);
                                decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                                decimal costOnly = decimal.Parse(dataGridView1.Rows[row].Cells[colCostPerUnit].Value.ToString());
                                dataGridView1.Rows[row].Cells[colTotalCost].Value = Library.ConvertDecimalToStringForm(qty * costOnly);

                                dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colCode];
                                dataGridView1.BeginEdit(true);

                            }
                            else
                            {
                                decimal qty = decimal.Parse(dataGridView1.Rows[row].Cells[colQty].Value.ToString());
                                decimal costOnly = decimal.Parse(dataGridView1.Rows[row].Cells[colCostPerUnit].Value.ToString());
                                dataGridView1.Rows[row].Cells[colTotalCost].Value = Library.ConvertDecimalToStringForm(qty * costOnly);
                            }

                        }

                        break;

                }

                BinddingTotal();
            }
            catch (Exception)
            {

            }

        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.row = e.RowIndex;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.row = e.RowIndex;
        }
    }
}
