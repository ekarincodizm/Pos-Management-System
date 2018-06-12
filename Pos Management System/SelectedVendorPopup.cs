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
    public partial class SelectedVendorPopup : Form
    {
        private CreatePOForm createPOForm;
        private string _ReturnForm;
        private GoodsReturnWmsForm goodsReturnWmsForm;
        private GoodsAutoReturnWmsForm goodsAutoReturnWmsForm;
        private GoodsReturnVendorStatusForm goodsReturnVendorStatusForm;
        private GoodsReturnVendorConfirmForm goodsReturnVendorConfirmForm;
        private ManageProductForm manageProductForm;
        private PORcvInvoiceForm pORcvInvoiceForm;
        private WasteInOutStockForm wasteInOutStockForm;

        public SelectedVendorPopup()
        {
            InitializeComponent();
        }

        public SelectedVendorPopup(CreatePOForm createPOForm)
        {
            InitializeComponent();
            this.createPOForm = createPOForm;
            _ReturnForm = "CreatePOForm";
        }

        public SelectedVendorPopup(GoodsReturnWmsForm goodsReturnWmsForm)
        {
            InitializeComponent();
            this.goodsReturnWmsForm = goodsReturnWmsForm;
            _ReturnForm = "GoodsReturnWmsForm";
        }

        public SelectedVendorPopup(GoodsAutoReturnWmsForm goodsAutoReturnWmsForm)
        {
            InitializeComponent();
            this.goodsAutoReturnWmsForm = goodsAutoReturnWmsForm;
            _ReturnForm = "GoodsAutoReturnWmsForm";
        }

        public SelectedVendorPopup(GoodsReturnVendorStatusForm goodsReturnVendorStatusForm)
        {
            InitializeComponent();
            _ReturnForm = "GoodsReturnVendorStatusForm";
            this.goodsReturnVendorStatusForm = goodsReturnVendorStatusForm;
        }

        public SelectedVendorPopup(GoodsReturnVendorConfirmForm goodsReturnVendorConfirmForm)
        {
            InitializeComponent();
            _ReturnForm = "GoodsReturnVendorConfirmForm";
            this.goodsReturnVendorConfirmForm = goodsReturnVendorConfirmForm;
        }

        public SelectedVendorPopup(ManageProductForm manageProductForm)
        {
            InitializeComponent();
            _ReturnForm = "ManageProductForm";
            this.manageProductForm = manageProductForm;
        }

        public SelectedVendorPopup(PORcvInvoiceForm pORcvInvoiceForm)
        {
            InitializeComponent();
            _ReturnForm = "PORcvInvoiceForm";
            this.pORcvInvoiceForm = pORcvInvoiceForm;
        }

        public SelectedVendorPopup(WasteInOutStockForm wasteInOutStockForm)
        {
            InitializeComponent();
            _ReturnForm = "WasteInOutStockForm";
            this.wasteInOutStockForm = wasteInOutStockForm;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void SelectedVendorPopup_Load(object sender, EventArgs e)
        {
            foreach (var item in Singleton.SingletonVender.Instance().Vendors.Where(w => w.Enable == true).Take(200).ToList())
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Description);
            }
        }

        void Selected()
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            var data = Singleton.SingletonVender.Instance().Vendors.SingleOrDefault(w => w.Id == id);
            switch (_ReturnForm)
            {
                //WasteInOutStockForm
                case "WasteInOutStockForm": // หน้า รับ - จ่าย - คงเหลือ ห้องของเสีย
                    wasteInOutStockForm.BinddingVendor(id);
                    break;
                case "CreatePOForm":
                    createPOForm.BinddingVendor(id);
                    break;
                case "GoodsReturnWmsForm":
                    goodsReturnWmsForm.BinddingVendor(id);
                    break;
                case "GoodsAutoReturnWmsForm":
                    goodsAutoReturnWmsForm.BinddingVendor(id);
                    break;
                case "GoodsReturnVendorStatusForm":
                    goodsReturnVendorStatusForm.BinddingVendor(id);
                    break;
                case "GoodsReturnVendorConfirmForm":
                    goodsReturnVendorConfirmForm.BinddingVendor(id);
                    break;
                case "ManageProductForm":
                    manageProductForm.BinddingVendor(id);
                    break;
                case "PORcvInvoiceForm":
                    pORcvInvoiceForm.BinddingVendor(id);
                    break;
                    
                default:
                    break;
            }
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Selected();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Selected();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void textBoxSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    Reload(textBoxSearchKey.Text.Trim());
                    break;
                default:
                    break;
            }
        }

        private void Reload(string v)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            List<Vendor> vendors = new List<Vendor>();

            if (radioButtonCode.Checked == true)
            {
                vendors = Singleton.SingletonVender.Instance().Vendors.Where(w => w.Enable == true && w.Code.Contains(v)).OrderByDescending(w => w.CreateDate).Take(200).ToList();
            }
            else
            {
                vendors = Singleton.SingletonVender.Instance().Vendors.Where(w => w.Enable == true && w.Name.Contains(v)).OrderByDescending(w => w.CreateDate).Take(200).ToList();
            }
            
            foreach (var item in vendors)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Address, item.Tel, item.Fax, item.POCostType.Name, Library.GetFullNameUserById(item.CreateBy),
                   Library.ConvertBoolToStr(item.Enable), item.Description);
            }
        }
    }
}
