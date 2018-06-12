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
    public partial class GoodsReturnVendorStatusForm : Form
    {
        public GoodsReturnVendorStatusForm()
        {
            InitializeComponent();
        }

        private void GoodsReturnVendorStatusForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedVendorPopup dwv = new SelectedVendorPopup(this);
            dwv.ShowDialog();
        }

        int _VendorId;
        public void BinddingVendor(int id)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            _VendorId = id;
            List<Vendor> vendors = SingletonVender.Instance().Vendors;
            var data = vendors.SingleOrDefault(w => w.Id == id);
            textBoxVendorCode.Text = data.Code;
            textBoxVendorName.Text = data.Name;
            /// Get สินค้าห้องของเสีย
            IEnumerable<int> allProductInVendor = Singleton.SingletonProduct.Instance().Products.Where(w => w.Enable == true && w.FKVender == _VendorId).Select(w => w.Id).Distinct().ToList<int>();
            using (SSLsEntities db = new SSLsEntities())
            {
                var waste = db.WasteWarehouse.Where(w => w.Enable == true && allProductInVendor.Contains(w.FKProduct) && w.QtyUnit > 0).ToList();
                /// ตรวจพบสินค้า vendor ในห้องของเสีย
                foreach (var item in waste)
                {
                    IEnumerable<int> fkProductDtls = item.WasteWarehouseDetails.Where(w => w.Enable == true && w.LastResultUnit > 0).Select(w => w.FKProductDetails).Distinct().ToList<int>();
                    /// Bindding Data Grid
                    foreach (var last in fkProductDtls)
                    {
                        var getLast = item.WasteWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == last).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        // get ค่าจากใบทำคืนที่ ยังไม่คอนเฟิม
                        decimal qtyCNNotComplete = 0;
                        var cnData = db.CNWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == last && w.CNWarehouse.ConfirmCNDate == null && w.CNWarehouse.Enable == true).ToList();
                        if (cnData != null)
                        {
                            qtyCNNotComplete = cnData.Sum(w => w.Qty);
                        }
                        /// from ห้องของเสีย
                        dataGridView1.Rows.Add
                             (
                       getLast.FKProductDetails,
                       getLast.ProductDetails.Code,
                       getLast.ProductDetails.Products.ThaiName,
                       getLast.ProductDetails.ProductUnit.Name,
                       Library.ConvertDecimalToStringForm(getLast.LastResultUnit),
                       Library.ConvertDecimalToStringForm(qtyCNNotComplete),// ยอดที่รอการคอนเฟิม หลังจากส่งคืน
                       getLast.Packsize,
                       Library.ConvertDecimalToStringForm(getLast.ProductDetails.CostAndVat),
                       Library.ConvertDecimalToStringForm(getLast.ProductDetails.CostAndVat * getLast.QtyUnit),
                       "-"
                            );
                    }
                }
            }
            TotalSummary();
        }
        /// <summary>
        /// ค้นหาด้วยรหัสสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            string code = textBoxCode.Text.Trim();
            /// Get สินค้าห้องของเสีย            
            using (SSLsEntities db = new SSLsEntities())
            {
                /// ตรวจพบสินค้า vendor ในห้องของเสีย
                var product = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Enable == true && w.Code == code);
                if (product == null)
                {
                    MessageBox.Show("ไม่พบข้อมูล");
                    return;
                }
                /// Bindding Data Grid
                var getAllInWate = db.WasteWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == product.Id).ToList();
                decimal inWaste = getAllInWate.Where(w => w.Enable == true && w.IsInOrOut == true).Sum(w => w.QtyUnit);
                decimal outWaste = getAllInWate.Where(w => w.Enable == true && w.IsInOrOut == false).Sum(w => w.QtyUnit);
                var getCN = db.CNWarehouseDetails.Where(w => w.Enable == true && w.CNWarehouse.Enable == true && w.CNWarehouse.ConfirmCNDate == null && w.FKProductDetails == product.Id).ToList().Sum(w=>w.Qty);

                dataGridView1.Rows.Add
                         (
                   product.Id,
                   code,
                   product.Products.ThaiName,
                  product.ProductUnit.Name,
                   Library.ConvertDecimalToStringForm(inWaste - outWaste),
                   Library.ConvertDecimalToStringForm(getCN),// ยอดที่รอการคอนเฟิม หลังจากส่งคืน
                   product.PackSize,
                   Library.ConvertDecimalToStringForm(Library.GetResult(product.Id, DateTime.Now)),
                   Library.ConvertDecimalToStringForm(Library.GetResult(product.Id, DateTime.Now) * (inWaste - outWaste)),
                   "-");

                //foreach (var last in getAllInWate)
                //{
                // get ค่าจากใบทำคืนที่ ยังไม่คอนเฟิม
                //decimal qtyCNNotComplete = 0;
                //var cnData = db.CNWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == last.FKProductDetails && w.CNWarehouse.ConfirmCNDate == null && w.CNWarehouse.Enable == true).ToList();
                //if (cnData != null)
                //{
                //    qtyCNNotComplete = cnData.Sum(w => w.Qty);
                //}

                /// from ห้องของเสีย
                // dataGridView1.Rows.Add
                //      (
                //last.FKProductDetails,
                //last.ProductDetails.Code,
                //last.ProductDetails.Products.ThaiName,
                //last.ProductDetails.ProductUnit.Name,
                //Library.ConvertDecimalToStringForm(last.LastResultUnit),
                //Library.ConvertDecimalToStringForm(qtyCNNotComplete),// ยอดที่รอการคอนเฟิม หลังจากส่งคืน
                //last.Packsize,
                //Library.ConvertDecimalToStringForm(last.ProductDetails.CostAndVat),
                //Library.ConvertDecimalToStringForm(last.ProductDetails.CostAndVat * last.QtyUnit),
                //"-");
                //}

            }
            TotalSummary();
        }
        /// <summary>
        /// ค้นหาทั้งหมด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            /// Get สินค้าห้องของเสีย
            IEnumerable<int> allProductInVendor = Singleton.SingletonProduct.Instance().Products.Where(w => w.Enable == true).Select(w => w.Id).Distinct().ToList<int>();
            using (SSLsEntities db = new SSLsEntities())
            {
                var waste = db.WasteWarehouse.Where(w => w.Enable == true && allProductInVendor.Contains(w.FKProduct) && w.QtyUnit > 0).ToList();
                /// ตรวจพบสินค้า vendor ในห้องของเสีย
                foreach (var item in waste)
                {
                    IEnumerable<int> fkProductDtls = item.WasteWarehouseDetails.Where(w => w.Enable == true && w.LastResultUnit > 0).Select(w => w.FKProductDetails).Distinct().ToList<int>();
                    /// Bindding Data Grid
                    foreach (var last in fkProductDtls)
                    {
                        var getLast = item.WasteWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == last).OrderByDescending(w => w.CreateDate).FirstOrDefault();
                        // get ค่าจากใบทำคืนที่ ยังไม่คอนเฟิม
                        decimal qtyCNNotComplete = 0;
                        var cnData = db.CNWarehouseDetails.Where(w => w.Enable == true && w.FKProductDetails == getLast.FKProductDetails && w.CNWarehouse.ConfirmCNDate == null && w.CNWarehouse.Enable == true).ToList();
                        if (cnData != null)
                        {
                            qtyCNNotComplete = cnData.Sum(w => w.Qty);
                        }
                        /// from ห้องของเสีย
                        dataGridView1.Rows.Add
                             (
                       getLast.FKProductDetails,
                       getLast.ProductDetails.Code,
                       getLast.ProductDetails.Products.ThaiName,
                       getLast.ProductDetails.ProductUnit.Name,
                       Library.ConvertDecimalToStringForm(getLast.LastResultUnit),
                       Library.ConvertDecimalToStringForm(qtyCNNotComplete),// ยอดที่รอการคอนเฟิม หลังจากส่งคืน
                       getLast.Packsize,
                       Library.ConvertDecimalToStringForm(getLast.ProductDetails.CostOnly),
                       Library.ConvertDecimalToStringForm(getLast.ProductDetails.CostOnly * getLast.QtyUnit),
                       "-"
                            );
                    }
                }
                TotalSummary();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void TotalSummary()
        {
            decimal totalPiece = 0;
            decimal totalUnit = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                decimal unit = decimal.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                decimal packsize = decimal.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
                decimal piece = unit * packsize;
                totalPiece += piece;
                totalUnit += unit;
            }
            textBoxTotalUnit.Text = Library.ConvertDecimalToStringForm(totalUnit);
            textBoxTotalPiece.Text = Library.ConvertDecimalToStringForm(totalPiece);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
