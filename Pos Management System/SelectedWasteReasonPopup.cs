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
    public partial class SelectedWasteReasonPopup : Form
    {
        private GoodsReturnWmsForm goodsReturnWmsForm;
        private string _FromForm;
        private WasteManageForm wasteManageForm;
        private WasteWarehouseManageForm wasteWarehouseManageForm;
        private GoodsAutoReturnWmsForm goodsAutoReturnWmsForm;

        public SelectedWasteReasonPopup()
        {
            InitializeComponent();
        }

        public SelectedWasteReasonPopup(GoodsReturnWmsForm goodsReturnWmsForm)
        {
            InitializeComponent();
            this.goodsReturnWmsForm = goodsReturnWmsForm;
            _FromForm = "GoodsReturnWmsForm";
        }

        public SelectedWasteReasonPopup(WasteManageForm wasteManageForm)
        {
            InitializeComponent();
            this.wasteManageForm = wasteManageForm;
            _FromForm = "WasteManageForm";
        }

        public SelectedWasteReasonPopup(WasteWarehouseManageForm wasteWarehouseManageForm)
        {
            InitializeComponent();
            this.wasteWarehouseManageForm = wasteWarehouseManageForm;
            _FromForm = "WasteWarehouseManageForm";
        }

        public SelectedWasteReasonPopup(GoodsAutoReturnWmsForm goodsAutoReturnWmsForm)
        {
            InitializeComponent();
            this.goodsAutoReturnWmsForm = goodsAutoReturnWmsForm;
            _FromForm = "GoodsAutoReturnWmsForm";
        }

        private void SelectedWasteReasonPopup_Load(object sender, EventArgs e)
        {
            foreach (var item in Singleton.SingletonWasteReason.Instance().WasteReasons)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Description);
            }
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
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            Console.WriteLine(id);
            SendDataChoose(id);
        }
        void SendDataChoose(int id)
        {
            var data = Singleton.SingletonWasteReason.Instance().WasteReasons.SingleOrDefault(w => w.Id == id);
            switch (_FromForm)
            {
                case "GoodsReturnWmsForm":
                    this.goodsReturnWmsForm.BinddingWasteReasonSelected(data);
                    break;
                case "WasteManageForm":
                    this.wasteManageForm.BinddingWasteReasonSelected(data);
                    break;
                case "WasteWarehouseManageForm":
                    this.wasteWarehouseManageForm.BinddingWasteReasonSelected(data);
                    break;
                case "GoodsAutoReturnWmsForm":
                    this.goodsAutoReturnWmsForm.BinddingWasteReasonSelected(data);
                    break;
                    
                default:
                    break;
            }
            this.Dispose();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            Console.WriteLine(id);
            SendDataChoose(id);
        }
    }
}
