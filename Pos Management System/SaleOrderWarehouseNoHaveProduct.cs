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
    public partial class SaleOrderWarehouseNoHaveProduct : Form
    {
        public SaleOrderWarehouseNoHaveProduct()
        {
            InitializeComponent();
        }
        int checkload = 1;
        private void textBoxKeySearch_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    LoadGrid();
                    break;
                default:
                    break;
            }
        }
        public void LoadGrid()
        {
            string code = textBoxKeySearch.Text.Trim();
            using (SSLsEntities db = new SSLsEntities())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                DateTime dateS = dateTimePickerStart.Value;
                DateTime dateE = dateTimePickerEnd.Value;
                var list = db.SaleOrderWarehouseDtl.Where(w => w.SaleOrderWarehouse.ConfirmOrderDate != null && (code == "" ? w.SaleOrderWarehouse.Code != null : w.SaleOrderWarehouse.Code == code) &&
                DbFunctions.TruncateTime(w.SaleOrderWarehouse.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
                DbFunctions.TruncateTime(w.SaleOrderWarehouse.CreateDate) <= DbFunctions.TruncateTime(dateE)).OrderBy(w => w.SaleOrderWarehouse.CreateDate).ThenBy(w => w.SaleOrderWarehouse.Code).ThenBy(w => w.ProductDetails.Code).ToList();
                foreach (var item in list)
                {
                    if (checkBox1.Checked == true)
                    {

                    }
                    else
                    {
                        if (item.QtyAllow == item.Qty) // ไม่แสดง เบิกได้
                        {
                            continue;
                        }
                    }

                    if (checkBox2.Checked == true)
                    {

                    }else
                    {
                        if (item.QtyAllow < item.Qty) // ไม่แสดง สต็อกไม่พอ
                        {
                            continue;
                        }
                    }
                    dataGridView1.Rows.Add(
                        1 + dataGridView1.Rows.Count,
                        Library.ConvertDateToThaiDate(item.SaleOrderWarehouse.CreateDate),
                        Library.GetFullNameUserById(item.SaleOrderWarehouse.CreateBy),
                        item.SaleOrderWarehouse.Code,
                        item.SaleOrderWarehouse.InvoiceNo,
                        item.ProductDetails.Code,
                        item.ProductDetails.Products.ThaiName,
                        item.ProductDetails.ProductUnit.Name,
                        item.ProductDetails.PackSize,
                        item.Qty,
                        item.QtyAllow,
                        (item.Qty == item.QtyAllow ? "เบิกได้" : "สต็อกไม่พอ")
                        );
                }
                if (dataGridView1.Rows.Count == 0)
                {
                    if (checkload == 1)
                    {
                        MessageBox.Show("ไม่พบข้อมูล", "คำเตือนจากระบบ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

            }
        }

        private void buttonPlus_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void BtExcel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            string KeyWord = "";
            DateTime dateS = dateTimePickerStart.Value;
            DateTime dateE = dateTimePickerEnd.Value;
            if (textBoxKeySearch.Text != "")
            {
                KeyWord = ("ค้าหาโดย DocNo : " + textBoxKeySearch.Text + "  วันที่ : " + dateS + " ถึง " + dateE);
            }
            else
            {
                KeyWord = ("ค้าหาโดย วันที่ : " + dateS + " ถึง " + dateE);
            }

            CodeFileDLL.ExcelReport(dataGridView1, this.Text.ToString(), KeyWord);
            Cursor.Current = Cursors.Default;
        }
    }
}
