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
    public partial class WasteWarehouseManageLocationList : Form
    {
        private ProductDetails product;
        private WasteWarehouseManageForm wasteWarehouseManageForm;

        public WasteWarehouseManageLocationList()
        {
            InitializeComponent();
        }

        public WasteWarehouseManageLocationList(WasteWarehouseManageForm wasteWarehouseManageForm, ProductDetails product)
        {
            InitializeComponent();
            this.wasteWarehouseManageForm = wasteWarehouseManageForm;
            this.product = product;
        }

        private decimal ConvertDecimalNull(float? value)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                return (decimal)value;
            }
        }
        private void WasteWarehouseManageLocationList_Load(object sender, EventArgs e)
        {
            // ตรวจสอบ stock warehouse ในระบบเก่า
            using (WH_TRATEntities db = new WH_TRATEntities())
            {
                var locstk = db.WH_LOCSTK.Where(w => w.PRODUCT_NO == product.Code).ToList();
                foreach (var item in locstk)
                {
                    decimal qty = ConvertDecimalNull(item.QTY) / ConvertDecimalNull(item.PACK_SIZE);
                    decimal bookRaw = ConvertDecimalNull(item.BOOK_QTY);
                    decimal book = bookRaw / ConvertDecimalNull(item.PACK_SIZE);
                    decimal allow = (ConvertDecimalNull(item.QTY) - ConvertDecimalNull(item.BOOK_QTY)) / ConvertDecimalNull(item.PACK_SIZE);
                    dataGridView1.Rows.Add
                        (
                        item.LOCATION_NO,
                        item.DOOR_NO,
                       Library.ConvertDecimalToStringForm(qty),
                        Library.ConvertDecimalToStringForm(book),
                         Library.ConvertDecimalToStringForm(allow),
                          0,
                          item.ROWID
                        );
                }
                //    if (locstk != null)
                //    {
                //        decimal qtyLoc = 0;
                //        decimal bookqtyLoc = 0;
                //        if (locstk.QTY != null)
                //        {
                //            qtyLoc = (decimal)locstk.QTY;
                //        }
                //        if (locstk.BOOK_QTY != null)
                //        {
                //            bookqtyLoc = (decimal)locstk.BOOK_QTY;
                //        }
                //        if ((qtyLoc - bookqtyLoc) <= 0)
                //        {
                //            MessageBox.Show("ไม่มีสินค้าคงเหลือ");
                //        }
                //    }
                //    else
                //    {
                //        MessageBox.Show("ไม่มีสินค้าคงเหลือ");
                //    }
                //}

                //else
                //{
                //    MessageBox.Show("ไม่พบรหัสบาร์โค้ด");
                //}
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// เลือก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string location = dataGridView1.Rows[i].Cells[0].Value.ToString();
                decimal qty = decimal.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                int id = int.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
                if (qty > 0)
                {
                    wasteWarehouseManageForm.BinddingProductLocation(qty, location, id);
                    this.Dispose();
                    return;
                }

            }

        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 5:
                    try
                    {

                        string location = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
                        decimal qty = decimal.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[5].Value.ToString());
                        decimal qtyAllow = decimal.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[4].Value.ToString());
                        if (qty > qtyAllow)
                        {
                            MessageBox.Show("มากไป");
                            dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[5].Value = "0";
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("ผิดพลาด");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
