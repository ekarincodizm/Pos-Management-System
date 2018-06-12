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
    public partial class CNPosForm : Form
    {
        public CNPosForm()
        {
            InitializeComponent();
        }

        private void CNPosForm_Load(object sender, EventArgs e)
        {
            //textBoxDescription.Select();
        }
        /// <summary>
        /// row number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// Search ใบเสร็จ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int colId = 0; // Is Hide
        int colCode = 1;
        int colName = 2;
        int colProductvatType = 3;
        int colQty = 4;
        int colUnit = 5;
        int colDisCoupon = 6;
        int colCNDisCoupon = 7;
        int colDisShop = 8;
        int colCNDisShop = 9;
        int colPricePerUnit = 10;
        int colTotalPrice = 11;
        int colOnceQty = 12;
        int colCnQty = 13;
        int colCnAll = 14;
        int _fkPos;
        private void buttonPlus_Click(object sender, EventArgs e)
        {
            MainBindding();
        }
        /// <summary>
        /// บันทัก การรับคืนสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBoxDescription.Text.Trim() == "")
            {
                MessageBox.Show("กรุณาป้อนหมายเหตุ");
                textBoxDescription.Select();
                return;
            }
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        string invoiceNo = textBoxInvoiceNo.Text;
                        var data = db.PosHeader.FirstOrDefault(w => w.InvoiceNo == invoiceNo && w.Enable == true);
                        string cnNo = "";
                        int countCN = db.CNHeader.Where(w => w.CreateDate.Year == DateTime.Now.Year && w.CreateDate.Month == DateTime.Now.Month).Count() + 1;
                        int fkCnType;
                        if (data.PayDate == null) // แสดงว่า ยังไม่จ้าย
                        {
                            // เชื่อ
                            cnNo = "ISB" + Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(countCN, 3);
                            fkCnType = MyConstant.CNType.Creadit;
                        }
                        else
                        {
                            // สด
                            cnNo = "ISCB" + Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(countCN, 3);
                            fkCnType = MyConstant.CNType.Cash;
                        }
                        try
                        {
                            // check value from checkbox and CN qty 
                            List<CNDetails> details = new List<CNDetails>();
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                var check = dataGridView1.Rows[i].Cells[colCnAll].Value ?? "0";
                                int fkPosDetails = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                                // แปลว่า เอาทั้งหมด
                                decimal qty;
                                //if (check == "1")
                                //{
                                //    // ยอดที่เหลือ ต้องคืนทั้งหมด
                                //    decimal qtyAll = decimal.Parse(dataGridView1.Rows[i].Cells[colQty].Value.ToString());
                                //    // ยอดทั้งหมด - ยอดที่เคยคืนไปแล้ว
                                //    decimal qtyOnce = decimal.Parse(dataGridView1.Rows[i].Cells[colOnceQty].Value.ToString());
                                //    qty = qtyAll - qtyOnce;
                                //}
                                //else
                                //{
                                //    // ยอดที่ต้องการคืน
                                //    qty = decimal.Parse(dataGridView1.Rows[i].Cells[colCnQty].Value.ToString());
                                //}
                                qty = decimal.Parse(dataGridView1.Rows[i].Cells[colCnQty].Value.ToString());
                                decimal cnCNDisCoupon = decimal.Parse(dataGridView1.Rows[i].Cells[colCNDisCoupon].Value.ToString());
                                decimal cnCNDisShop = decimal.Parse(dataGridView1.Rows[i].Cells[colCNDisShop].Value.ToString());
                                decimal pricePerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colPricePerUnit].Value.ToString());
                                details.Add(new CNDetails()
                                {
                                    Enable = true,
                                    CreateDate = DateTime.Now,
                                    CreateBy = Singleton.SingletonAuthen.Instance().Id,
                                    UpdateDate = DateTime.Now,
                                    UpdateBy = Singleton.SingletonAuthen.Instance().Id,
                                    FKPosDetails = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString()),
                                    CNQty = qty,
                                    CNDisCoupon = cnCNDisCoupon,
                                    CNDisShop = cnCNDisShop,
                                    PricePerUnit = pricePerUnit,
                                    CNTotalPrice = pricePerUnit * qty,
                                    Description = textBoxDescription.Text
                                });
                            }
                            CNHeader header = new CNHeader();
                            header.No = cnNo;
                            header.Enable = true;
                            header.Description = textBoxDescription.Text;
                            header.CreateDate = DateTime.Now;
                            header.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                            header.UpdateDate = DateTime.Now;
                            header.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                            header.FKPosHeader = this._fkPos;
                            header.TotalVat = 0; // ยอดคืนที่ ภาษี
                            header.TotaNoVat = 0; // ยอดคืน ไม่มีภาษี
                            header.Total = details.Sum(w => w.CNTotalPrice);
                            header.TotalBalance = details.Sum(w => w.CNTotalPrice + w.CNDisCoupon + w.CNDisShop);
                            header.QtyList = details.Count();
                            header.Qty = details.Sum(w => w.CNQty);
                            header.FKCNType = fkCnType;
                            header.SequenceNo = countCN;
                            header.CNDetails = details;
                            db.CNHeader.Add(header);
                            db.SaveChanges();

                            /// Update Stock +StoreFront
                            details = new List<CNDetails>();
                            details = db.CNDetails.Include("PosDetails").Where(w => w.FKCN == cnNo && w.CNQty > 0).ToList();
                            Library.MakeValueForUpdateStockPos(details);
                            MainBindding();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("พบข้อผิดพลาด " + ex.ToString());
                        }

                    }
                    break;
                case DialogResult.No:
                    break;
            }



        }
        /// <summary>
        ///  Enter invoice no
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxInvoiceNo_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    MainBindding();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Bindding UI
        /// </summary>
        void MainBindding()
        {
            string invoiceNo = textBoxInvoiceNo.Text.Trim();
            if (invoiceNo == "")
            {
                return;
            }
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                var data = db.PosHeader.FirstOrDefault(w => w.InvoiceNo == invoiceNo && w.Enable == true);
               var cns = data.CNHeader.Where(w => w.Enable == true).ToList();
                if (cns.Count() > 0)
                {
                    MessageBox.Show("เคยทำคืนแล้ว ");
                }
                if (data == null)
                {
                    MessageBox.Show("ไม่พบข้อมูล กรุณาลองใหม่");
                    return;
                }
                this._fkPos = data.Id;
                string cnNo = "";
                int countCN = db.CNHeader.Where(w => w.CreateDate.Year == DateTime.Now.Year && w.CreateDate.Month == DateTime.Now.Month).Count() + 1;
                if (data.PayDate == null) // แสดงว่า ยังไม่จ้าย
                {
                    // เชื่อ
                    cnNo = MyConstant.PrefixForGenerateCode.CustomerCNCredit + Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(countCN, 3);
                    textBoxDebtNo.Text = data.Debtor.Code;
                    textBoxDebtName.Text = data.Debtor.Name;
                    textBoxCNType.Text = "คืนเชื่อ";
                }
                else
                {
                    textBoxCNType.Text = "คืนสด";
                    // สด
                    cnNo = MyConstant.PrefixForGenerateCode.CustomerCNCash + Singleton.SingletonThisBudgetYear.Instance().ThisYear.CodeYear + DateTime.Now.ToString("MM") + Library.GenerateCodeFormCount(countCN, 3);
                    textBoxPayDate.Text = Library.ConvertDateToThaiDate(data.PayDate.Value);
                }
                textBoxInvDate.Text = Library.ConvertDateToThaiDate(data.CreateDate);
                textBoxCashier.Text = data.Users.Name;
                textBoxMemberNo.Text = data.Member.Code;
                textBoxMemberName.Text = data.Member.Name;
                textBoxSequnce.Text = "" + (data.CNHeader.ToList().Count() + 1);
                textBoxCnCode.Text = cnNo;
                textBoxDateNow.Text = Library.ConvertDateToThaiDate(DateTime.Now);
                // bindding
                foreach (var item in data.PosDetails.Where(w => w.Enable == true).ToList())
                {
                    // ยอดรวมทั้งหมด ที่เคยทำ CN
                    decimal cnQty = item.CNDetails.Where(w => w.Enable == true).Sum(w => w.CNQty);
                    // ยอดรวมคูปองทั้งหมด ที่เคยทำ CN
                    decimal cnCNDisCoupon = item.CNDetails.Where(w => w.Enable == true).Sum(w => w.CNDisCoupon);
                    // ยอดรวมร้านทั้งหมด ที่เคยทำ CN
                    decimal cnCNDisShop = item.CNDetails.Where(w => w.Enable == true).Sum(w => w.CNDisShop);

                    dataGridView1.Rows.Add(
                        item.Id,
                        item.ProductDetails.Code,
                        item.ProductDetails.Products.ThaiName,
                        item.ProductDetails.Products.ProductVatType.Name, // สวยงาม 4 มิติ
                        item.Qty - cnQty,
                        item.ProductDetails.ProductUnit.Name + " x " + item.ProductDetails.PackSize, // สวยงาม 3 มิติ
                        Library.ConvertDecimalToStringForm(item.DiscountCoupon - cnCNDisCoupon),
                        "0",
                        Library.ConvertDecimalToStringForm(item.DiscountShop - cnCNDisShop),
                        "0",
                        item.PricePerUnit, // ราคาต่อหน่วย / ณ ตอนขาย
                        Library.ConvertDecimalToStringForm(item.PricePerUnit * (item.Qty - cnQty)),
                        cnQty, // ฝาก 0 ไว้ก่อน
                        "0" // เริ่มต้นที่ 0
                        );
                }
            }
        }
        /// <summary>
        ///มีการคีย์ บน datagrid1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 13:
                    /// check การใส่ ค่าเกินจริง จาก ยอดครั้งนี้ + ยอดที่เคยคืนไปแล้ว
                    //decimal qtyUnit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                    //decimal qtyOnceUnit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colOnceQty].Value.ToString());
                    //decimal thisCNQty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCnQty].Value.ToString());
                    //decimal allQty = thisCNQty + qtyOnceUnit;

                    //// ยอดครั้งนี้ + ยอดที่เคยคืนไปแล้ว > จำนวนจริง หรือไม่
                    //if (allQty > qtyUnit)
                    //{
                    //    MessageBox.Show("คีย์เกินจำนวนจริง");
                    //    dataGridView1.Rows[e.RowIndex].Cells[colCnQty].Value = "0";
                    //    //return;
                    //}  
                    /// จำนวนที่เหลือ    
                    decimal qtyUnit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colQty].Value.ToString());
                    /// จำนวนที่ คีย์
                    decimal thisCNQty = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCnQty].Value.ToString());
                    if (thisCNQty > qtyUnit)
                    {
                        MessageBox.Show("คีย์เกินจำนวนจริง");
                        dataGridView1.Rows[e.RowIndex].Cells[colCnQty].Value = "0";
                    }
                    break;
                case 7: // คูปอง
                    /// ยอดลดคูปองที่เหลือ
                    decimal couponDis = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colDisCoupon].Value.ToString());
                    /// ยอดคีย์ ลดครั้งนี้
                    decimal couponDisCN = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCNDisCoupon].Value.ToString());
                    if (couponDisCN > couponDis)
                    {
                        MessageBox.Show("คีย์เกินจำนวนจริง");
                        dataGridView1.Rows[e.RowIndex].Cells[colCNDisCoupon].Value = "0";
                    }
                    break;
                case 9: // ร้าน
                    /// ยอดลดร้านที่เหลือ
                    decimal shopDis = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colDisShop].Value.ToString());
                    /// ยอดคีย์ ลดครั้งนี้
                    decimal shopDisCN = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCNDisShop].Value.ToString());
                    if (shopDisCN > shopDis)
                    {
                        MessageBox.Show("คีย์เกินจำนวนจริง");
                        dataGridView1.Rows[e.RowIndex].Cells[colCNDisShop].Value = "0";
                    }
                    break;
                default:
                    break;
            }
            CalSummary();
        }

        private void CalSummary()
        {
            decimal totalUnitCN = 0;
            decimal totalPriceCN = 0;
            decimal totalDiscount = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                decimal qtyUnitCN = decimal.Parse(dataGridView1.Rows[i].Cells[colCnQty].Value.ToString());
                decimal pricePerUnit = decimal.Parse(dataGridView1.Rows[i].Cells[colPricePerUnit].Value.ToString());
                decimal discountShop = decimal.Parse(dataGridView1.Rows[i].Cells[colCNDisShop].Value.ToString());
                decimal discountCoupon = decimal.Parse(dataGridView1.Rows[i].Cells[colCNDisCoupon].Value.ToString());
                totalDiscount += discountShop + discountCoupon;
                totalPriceCN += pricePerUnit * qtyUnitCN;
                totalUnitCN += qtyUnitCN;

            }
            textBoxQtyUnit.Text = Library.ConvertDecimalToStringForm(totalUnitCN);
            textBoxTotalCN.Text = Library.ConvertDecimalToStringForm(totalPriceCN);
            textBoxDiscountCN.Text = Library.ConvertDecimalToStringForm(totalDiscount);
        }


    }
}
