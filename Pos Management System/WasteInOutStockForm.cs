using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class WasteInOutStockForm : Form
    {
        public WasteInOutStockForm()
        {
            InitializeComponent();
        }

        private void WasteInOutStockForm_Load(object sender, EventArgs e)
        {

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
            SelectedProductPopup obj = new SelectedProductPopup(this);
            obj.ShowDialog();
        }

        int _FKProduct = 0;
        int _IDDtl = 0;
        public void BinddingProduct(int id)
        {
            List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
            var product = products.SingleOrDefault(w => w.Id == id);
            _IDDtl = id;
            _FKProduct = product.FKProduct;
            textBoxBarcode.Text = product.Code;
            textBoxName.Text = product.Products.ThaiName;
        }
        /// <summary>
        /// ค้นหาหลัก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            if (_VId != 0)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    var prodByVendor = Singleton.SingletonProduct.Instance().Products.Where(w => w.FKVender == _VId).ToList();
                    var getResult = Getยกมา();
                    List<int> fkDtls = new List<int>();
                    fkDtls = getResult.Select(w => w.FKProductDetails).ToList<int>();
                    var getLog = db.WasteWarehouseDetails.Where(w => fkDtls.Contains(w.FKProductDetails) && w.Enable == true &&
                     EntityFunctions.TruncateTime(w.CreateDate) >= EntityFunctions.TruncateTime(dateTimePickerStart.Value) &&
                EntityFunctions.TruncateTime(w.CreateDate) <= EntityFunctions.TruncateTime(dateTimePickerEnd.Value)
                    ).OrderBy(w => w.CreateDate).ToList();
                    foreach (var item in prodByVendor)
                    {
                        decimal plus = getResult.Where(w => w.FKProduct == item.Id && w.IsInOrOut == "True").Sum(w => w.QtyPiece);
                        decimal minus = getResult.Where(w => w.FKProduct == item.Id && w.IsInOrOut == "False").Sum(w => w.QtyPiece);
                        decimal result = plus - minus;
                        var sku = item.ProductDetails.Where(w => w.Enable == true).OrderBy(w => w.PackSize).FirstOrDefault();
                        var logs = getLog.Where(w => w.ProductDetails.FKProduct == item.Id).OrderBy(w => w.CreateDate).ToList();
                        if (logs.Count() == 0)
                        {
                            continue;
                        }
                        dataGridView1.Rows.Add("ยอดยกมา", "-", sku.Code, item.ThaiName, "-", "0", "0", Library.ConvertDecimalToStringForm(result));
                        foreach (var log in logs)
                        {
                            if (log.IsInOrOut == true)
                            {
                                result = result + log.QtyPiece;
                                dataGridView1.Rows.Add(Library.ConvertDateToThaiDate(log.CreateDate, true), log.DocReference, sku.Code,
                                    log.ProductDetails.Code + " " + item.ThaiName,
                                    log.ProductDetails.ProductUnit.Name + " x " + log.Packsize,
                                    Library.ConvertDecimalToStringForm(log.QtyPiece),
                                    "0", 
                                    Library.ConvertDecimalToStringForm(result));
                            }
                            else
                            {
                                result = result - log.QtyPiece;
                                dataGridView1.Rows.Add(Library.ConvertDateToThaiDate(log.CreateDate, true), log.DocReference, sku.Code,
                                    log.ProductDetails.Code + " " + item.ThaiName,
                                    log.ProductDetails.ProductUnit.Name + " x " + log.Packsize,
                                   "0", 
                                   Library.ConvertDecimalToStringForm(log.QtyPiece),
                                   Library.ConvertDecimalToStringForm(result));
                            }
                        }
                        dataGridView1.Rows.Add("ยอดยกไป", "-", sku.Code, item.ThaiName, "-", "0", "0", Library.ConvertDecimalToStringForm(result));
                    }
                }
                return;
            }
            List<ProductDetails> products = Singleton.SingletonProduct.Instance().ProductDetails;
            var product = products.SingleOrDefault(w => w.Id == _IDDtl);

            List<int> listFKDtls = new List<int>();
            listFKDtls = products.Where(w => w.FKProduct == _FKProduct).Select(w => w.Id).ToList<int>();
            var getSku = products.Where(w => w.FKProduct == _FKProduct).OrderBy(w => w.PackSize).FirstOrDefault();
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.WasteWarehouseDetails.Where(w => listFKDtls.Contains(w.FKProductDetails) && w.Enable == true &&
                EntityFunctions.TruncateTime(w.CreateDate) >= EntityFunctions.TruncateTime(dateTimePickerStart.Value) &&
                EntityFunctions.TruncateTime(w.CreateDate) <= EntityFunctions.TruncateTime(dateTimePickerEnd.Value)
                ).OrderBy(w => w.CreateDate).ToList();
                DateTime start = dateTimePickerStart.Value;
                //if (data.Count > 0)
                //{
                //    start = data.FirstOrDefault().CreateDate;
                //}
                decimal plus = 0;
                decimal minus = 0;
                var plusGet = db.WasteWarehouseDetails.Where(w => listFKDtls.Contains(w.FKProductDetails) && w.Enable == true &&
              w.IsInOrOut == true &&
              EntityFunctions.TruncateTime(w.CreateDate) < EntityFunctions.TruncateTime(dateTimePickerStart.Value));
                if (plusGet.Count() != 0)
                {
                    plus = plusGet.Sum(w => w.QtyPiece);
                }
                var minusGet = db.WasteWarehouseDetails.Where(w => listFKDtls.Contains(w.FKProductDetails) && w.Enable == true &&
                w.IsInOrOut == false &&
                EntityFunctions.TruncateTime(w.CreateDate) < EntityFunctions.TruncateTime(dateTimePickerStart.Value));
                if (minusGet.Count() != 0)
                {
                    minus = plusGet.Sum(w => w.QtyPiece);
                }
                //var dateString = dateTimePickerStart.Value.ToString("yyyyMMdd");
                //var aa = Library.GetQueryยอดยกมา(dateString, product.Code, product.Code).ToList();
                decimal getResult = plus - minus;

                dataGridView1.Rows.Add("ยอดยกมา", "", getSku.Code, getSku.Products.ThaiName, "-", "0", "0", Library.ConvertDecimalToStringForm(getResult));
                foreach (var item in data)
                {
                    if (item.IsInOrOut == true)
                    {
                        getResult = getResult + item.QtyPiece;
                        dataGridView1.Rows.Add(Library.ConvertDateToThaiDate(item.CreateDate, true), item.DocReference, getSku.Code,
                            item.ProductDetails.Code + " " + getSku.Products.ThaiName,
                            item.ProductDetails.ProductUnit.Name + " x " + item.Packsize,
                            Library.ConvertDecimalToStringForm(item.QtyPiece),
                            "0", Library.ConvertDecimalToStringForm(getResult));
                    }
                    else
                    {
                        getResult = getResult - item.QtyPiece;
                        dataGridView1.Rows.Add(Library.ConvertDateToThaiDate(item.CreateDate, true), item.DocReference, getSku.Code,
                            item.ProductDetails.Code + " " + getSku.Products.ThaiName,
                            item.ProductDetails.ProductUnit.Name + " x " + item.Packsize,
                            "0",
                           Library.ConvertDecimalToStringForm(item.QtyPiece), Library.ConvertDecimalToStringForm(getResult));
                    }
                }
                dataGridView1.Rows.Add("ยอดยกไป", "-", getSku.Code, getSku.Products.ThaiName, "-", "0", "0", Library.ConvertDecimalToStringForm(getResult));
            }
        }
        class ยกมาของเสย
        {
            public string DateStr { get; set; }
            public int FKProduct { get; set; }
            public int FKProductDetails { get; set; }
            public decimal QtyPiece { get; set; }
            public string IsInOrOut { get; set; }

        }
        private List<ยกมาของเสย> Getยกมา()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                string date = dateTimePickerStart.Value.ToString("yyyyMMdd");
                string query = @"SELECT        TOP (100) PERCENT wms.WasteWarehouseDetails.CreateDate, CONVERT(varchar, wms.WasteWarehouseDetails.CreateDate, 112) AS DateStr, wms.WasteWarehouseDetails.CreateBy, wms.ProductDetails.FKProduct, 
                         wms.WasteWarehouseDetails.FKProductDetails, wms.WasteWarehouseDetails.QtyPiece, wms.WasteWarehouseDetails.IsInOrOut, wms.Products.FKVender
                        FROM            wms.WasteWarehouseDetails INNER JOIN
                                                    wms.ProductDetails ON wms.WasteWarehouseDetails.FKProductDetails = wms.ProductDetails.Id INNER JOIN
                                                    wms.Products ON wms.ProductDetails.FKProduct = wms.Products.Id
                        WHERE        (wms.WasteWarehouseDetails.Enable = 1) AND (wms.ProductDetails.Enable = 1) AND (CONVERT(varchar, wms.WasteWarehouseDetails.CreateDate, 112) < '{0}') AND (wms.Products.Enable = 1) AND 
                                                    (wms.Products.FKVender = {1})
                        ORDER BY wms.WasteWarehouseDetails.CreateDate DESC";
                String s = String.Format(query, date, _VId);
                var data = db.Database.ExecuteEntities<ยกมาของเสย>(s).ToList();
                return data;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CodeFileDLL.ExcelReport(dataGridView1, "รายงาน รับ-จ่าย-คงเหลือ ห้องของเสีย",
                Library.ConvertDateToThaiDate(dateTimePickerStart.Value) + " - " +
                Library.ConvertDateToThaiDate(dateTimePickerEnd.Value));
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _FKProduct = 0;
            _IDDtl = 0;
            textBoxBarcode.Text = "";
            textBoxName.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedVendorPopup dwv = new SelectedVendorPopup(this);
            dwv.ShowDialog();
        }
        int _VId = 0;
        public void BinddingVendor(int id)
        {
            var data = Singleton.SingletonProduct.Instance().Products.Where(w => w.FKVender == id).ToList();
            if (data.Count == 0)
            {
                MessageBox.Show("ไม่พบข้อมูล");
                return;
            }
            textBoxVCode.Text = data.FirstOrDefault().Vendor.Code;
            textBoxVName.Text = data.FirstOrDefault().Vendor.Name;
            _VId = id;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _VId = 0;
            textBoxVCode.Text = "";
            textBoxVName.Text = "";
        }
    }
}
