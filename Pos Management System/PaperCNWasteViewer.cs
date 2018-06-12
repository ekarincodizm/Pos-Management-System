using Omu.ValueInjecter;
using Pos_Management_System.ModelForPaper;
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
    public partial class PaperCNWasteViewer : Form
    {
        private string code;
        private GoodsAutoReturnWmsForm goodsAutoReturnWmsForm;
        private GoodsReturnVendorConfirmForm goodsReturnVendorConfirmForm;
        private GoodsReturnWmsForm goodsReturnWmsForm;

        public PaperCNWasteViewer()
        {
            InitializeComponent();
        }

        public PaperCNWasteViewer(GoodsReturnWmsForm goodsReturnWmsForm, string code)
        {
            InitializeComponent();
            this.goodsReturnWmsForm = goodsReturnWmsForm;
            this.code = code;
        }

        public PaperCNWasteViewer(GoodsReturnVendorConfirmForm goodsReturnVendorConfirmForm, string code)
        {
            InitializeComponent();
            this.goodsReturnVendorConfirmForm = goodsReturnVendorConfirmForm;
            this.code = code;
        }

        public PaperCNWasteViewer(GoodsAutoReturnWmsForm goodsAutoReturnWmsForm, string code)
        {
            InitializeComponent();
            this.goodsAutoReturnWmsForm = goodsAutoReturnWmsForm;
            this.code = code;
        }

        private void PaperCNWasteViewer_Load(object sender, EventArgs e)
        {
            var thisBranch = SingletonAuthen.Instance().MyBranch;
            BranchValue bv = new BranchValue();

            bv.BranchName = thisBranch.Name + " สาขา " + thisBranch.BranchNo;
            bv.BranchAddress = thisBranch.Address;
            bv.BranchTax = "เลขประจำตัวผู้เสียภาษี " + thisBranch.TaxNo;
            bv.BranchTelAndFax = "โทรศัพท์ " + thisBranch.Tel + " แฟกซ์ " + thisBranch.Fax;
           

            WasteDocValue wv = new WasteDocValue();
            wv.DocNo = code;
            List<WasteDocDetails> wds = new List<WasteDocDetails>();
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.CNWarehouse.SingleOrDefault(w => w.Code == code);
                wv.DocDate = Library.ConvertDateToThaiDate(data.DocDate);
                wv.DocReference = data.DocRefer;
                wv.PrintDate = Library.ConvertDateToThaiDate(DateTime.Now);

                wv.VendorCode = data.Vendor.Code;
                wv.VendorName = data.Vendor.Name;
                wv.VendorAddress = data.Vendor.Address;
                int i = 1;
                decimal totalCost = 0;
                foreach (var item in data.CNWarehouseDetails.Where(w => w.Enable == true).ToList())
                {
                    wds.Add(new WasteDocDetails()
                    {
                        Number = i+"",
                        ProductCode = item.ProductDetails.Code,
                        ProductName = item.ProductDetails.Products.ThaiName,
                        ProductUnit = item.ProductDetails.ProductUnit.Name,
                        Qty = Library.ConvertDecimalToStringForm(item.Qty),
                        CostPerUnit = Library.ConvertDecimalToStringForm(item.PricePerUnit),
                        TotalCost = Library.ConvertDecimalToStringForm(item.TotalPrice)
                    });
                    i++;
                    totalCost += item.PricePerUnit;
                }
                wv.TotalUnit = Library.ConvertDecimalToStringForm(data.TotalQtyUnit);
                wv.TotalCost = Library.ConvertDecimalToStringForm(data.TotalBalance);
                wv.TotalBalance = Library.ConvertDecimalToStringForm(data.TotalBalance);
            }
            BranchValueBindingSource.DataSource = bv;
            WasteDocValueBindingSource.DataSource = wv;
            WasteDocDetailsBindingSource.DataSource = wds;
            this.reportViewer1.RefreshReport();
        }
    }
}
