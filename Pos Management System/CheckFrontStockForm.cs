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
    public partial class CheckFrontStockForm : Form
    {
        public CheckFrontStockForm()
        {
            InitializeComponent();
        }

        private void CheckFrontStockForm_Load(object sender, EventArgs e)
        {
            BinddingUser(Singleton.SingletonAuthen.Instance().Users);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:

                        using (SSLsEntities db = new SSLsEntities())
                        {
                            string barcodescan = textBoxScanner.Text.Trim();
                            var getProductDetails = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == barcodescan);
                            if (getProductDetails != null)
                            {
                                var inShelf = db.PosStockDetails.OrderByDescending(w => w.CreateDate).FirstOrDefault(w => w.Enable == true && w.FKProductDetails == getProductDetails.Id);

                                textBoxCount.Text = "0";
                                textBoxCount.Select();
                                textBoxCount.SelectAll();

                                textBoxQty.Text = inShelf.ResultQtyUnit + "";
                                textBoxDiff.Text = inShelf.ResultQtyUnit + "";

                                textBoxName.Text = getProductDetails.Products.ThaiName;
                                textBoxPZ.Text = getProductDetails.PackSize + "";
                                labelUnit.Text = getProductDetails.ProductUnit.Name;
                                textBoxCostOnly.Text = Library.ConvertDecimalToStringForm(getProductDetails.CostOnly);
                                textBoxPrice.Text = Library.ConvertDecimalToStringForm(getProductDetails.SellPrice);
                                string stat = "";
                                var storeFront = db.CheckStockDetails.FirstOrDefault(w => w.Enable == true && w.FKProductDetails == getProductDetails.Id);
                                if (storeFront != null)
                                {
                                    stat = "นับแล้ว";
                                    if (storeFront.ConfirmDate != null)
                                    {
                                        stat = "ยืนยันแล้ว";
                                        textBoxScanner.SelectAll();
                                    }
                                    buttonReCount.Select();
                                    buttonConfirm.Enabled = false;
                                    textBoxCount.Text = storeFront.QtyUnitCheck + "";
                                    textBoxQty.Text = storeFront.QtyUnitOnHand + "";
                                    textBoxDiff.Text = (storeFront.QtyUnitCheck - storeFront.QtyUnitOnHand).ToString();
                                }
                                else
                                {
                                    stat = "ยังไม่นับ";
                                    buttonConfirm.Enabled = true;
                                }
                                textBoxStat.Text = stat;

                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ผิดพลาด");
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// Reset Count
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReCount_Click(object sender, EventArgs e)
        {
            // check ถ้ายืนยันแล้ว
            using (SSLsEntities db = new SSLsEntities())
            {
                string barcodescan = textBoxScanner.Text.Trim();
                var getProductDetails = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == barcodescan);
                var storeFront = db.CheckStockDetails.FirstOrDefault(w => w.Enable == true && w.FKProductDetails == getProductDetails.Id);
                if (storeFront.ConfirmDate != null)
                {
                    textBoxScanner.Select();
                    textBoxScanner.SelectAll();
                    return;
                }
            }
            DialogResult dr = MessageBox.Show("ยืนยันนับใหม่ ใช่หรือไม่ ?",
                        "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    Recount();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void Recount()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                string barcodescan = textBoxScanner.Text.Trim();
                var getProductDetails = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == barcodescan);
                var checkFKPro = db.CheckStockDetails.FirstOrDefault(w => w.Enable == true && w.FKProductDetails == getProductDetails.Id);
                checkFKPro.Enable = false;
                checkFKPro.UpdateDate = DateTime.Now;
                checkFKPro.UpdateBy = _UserId;
                db.Entry(checkFKPro).State = EntityState.Modified;
                db.SaveChanges();
            }
            ResetNew();
        }

        /// <summary>
        /// Enter จำนวที่นับได้
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxCount_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        decimal count = decimal.Parse(textBoxCount.Text);
                        decimal qty = decimal.Parse(textBoxQty.Text);
                        decimal result = count - qty;
                        textBoxDiff.Text = result.ToString();
                        buttonConfirm.Select();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                MessageBox.Show("ผิดพลาด");
            }

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ResetNew();

        }

        private void ResetNew()
        {
            buttonConfirm.Enabled = true;
            textBoxScanner.Text = "";
            textBoxScanner.Select();
            textBoxCount.Text = "0";
            textBoxQty.Text = "0";
            textBoxDiff.Text = "0";
            textBoxName.Text = "";
            textBoxPZ.Text = "0";
            textBoxStat.Text = "";
            labelUnit.Text = "";
            textBoxCostOnly.Text = "";
        }

        /// <summary>
        ///  ยืนยันนับเลย
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    decimal qty = decimal.Parse(textBoxQty.Text);
                    decimal count = decimal.Parse(textBoxCount.Text);
                    string barcodescan = textBoxScanner.Text.Trim();
                    var getProductDetails = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Enable == true && w.Code == barcodescan);
                    var checkFKPro = db.CheckStockPos.FirstOrDefault(w => w.Enable == true && w.FKProduct == getProductDetails.FKProduct);
                    if (checkFKPro != null)
                    {
                        CheckStockDetails dtl = new CheckStockDetails();
                        dtl.Enable = true;
                        dtl.Description = "ขึ้นระบบ ตุลา 60";
                        dtl.CreateDate = DateTime.Now;
                        dtl.CreateBy = _UserId;
                        dtl.UpdateDate = DateTime.Now;
                        dtl.UpdateBy = _UserId;
                        dtl.FKCheckStockPos = checkFKPro.Id;
                        dtl.FKProductDetails = getProductDetails.Id;
                        dtl.QtyUnitOnHand = qty;
                        dtl.QtyUnitCheck = count;
                        dtl.Packsize = getProductDetails.PackSize;
                        db.CheckStockDetails.Add(dtl);
                        db.SaveChanges();
                    }
                    else
                    {
                        CheckStockPos pos = new CheckStockPos();
                        pos.Enable = true;
                        pos.Name = getProductDetails.Products.ThaiName;
                        pos.Description = "ขึ้นระบบ ตุลา 60";
                        pos.CreateDate = DateTime.Now;
                        pos.CreateBy = _UserId;
                        pos.UpdateDate = DateTime.Now;
                        pos.UpdateBy = _UserId;
                        pos.FKProduct = getProductDetails.FKProduct;

                        CheckStockDetails dtl = new CheckStockDetails();
                        dtl.Enable = true;
                        dtl.Description = "ขึ้นระบบ ตุลา 60";
                        dtl.CreateDate = DateTime.Now;
                        dtl.CreateBy = _UserId;
                        dtl.UpdateDate = DateTime.Now;
                        dtl.UpdateBy = _UserId;

                        dtl.FKProductDetails = getProductDetails.Id;
                        dtl.QtyUnitOnHand = qty;
                        dtl.QtyUnitCheck = count;
                        dtl.Packsize = getProductDetails.PackSize;

                        pos.CheckStockDetails.Add(dtl);

                        db.CheckStockPos.Add(pos);
                        db.SaveChanges();
                    }
                    ResetNew();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ผิดพลาด");
            }

        }
        /// <summary>
        /// เปิดเลือก พนักงาน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            SelectedUserPopup obj = new SelectedUserPopup(this);
            obj.ShowDialog();
        }
        string _UserId;
        public void BinddingUser(Users send)
        {
            _UserId = send.Id;
            textBoxEmpCode.Text = send.Id;
            textBoxEmpName.Text = send.Name;
        }

        private void textBoxEmpCode_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string userId = textBoxEmpCode.Text.Trim();
                    var data = Singleton.SingletonPriority1.Instance().Users.FirstOrDefault(w => w.Enable == true && w.Id == userId);
                    if (data != null)
                    {
                        BinddingUser(data);
                    }
                    else
                    {
                        _UserId = null;
                        textBoxEmpCode.Text = "";
                        textBoxEmpName.Text = "";
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
