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
    public partial class SelectedProductPopup : Form
    {
        private SaleOrderWarehouseForm saleOrderWarehouseForm;
        private List<ProductDetails> list;
        private ManageProductForm manageProductForm;

        public SelectedProductPopup()
        {
            InitializeComponent();
        }

        public SelectedProductPopup(SaleOrderWarehouseForm saleOrderWarehouseForm, List<ProductDetails> list)
        {
            InitializeComponent();
            this.saleOrderWarehouseForm = saleOrderWarehouseForm;
            this.list = list;
            _FromForm = "SaleOrderWarehouseForm";
        }

        public SelectedProductPopup(ManageProductForm manageProductForm, List<ProductDetails> list)
        {
            InitializeComponent();
            this.manageProductForm = manageProductForm;
            this.list = list;
            _FromForm = "ManageProductForm";
        }
        /// <summary>
        /// มาจากโปรโมชั่น นาทีทอง
        /// </summary>
        /// <param name="campaignDiscountDayForm"></param>
        public SelectedProductPopup(CampaignDiscountDayForm campaignDiscountDayForm)
        {
            InitializeComponent();
            this.campaignDiscountDayForm = campaignDiscountDayForm;
            _FromForm = "CampaignDiscountDayForm";
        }
        /// <summary>
        /// มาจากซื้อครบจำนวน (คละ) ได้แลกซื้อ ราคาพิเศษ
        /// </summary>
        /// <param name="campaignFullyQtyAndSaleForm"></param>
        private CampaignFullyQtyAndSaleForm campaignFullyQtyAndSaleForm;

        public SelectedProductPopup(CampaignFullyQtyAndSaleForm campaignFullyQtyAndSaleForm, int gridNumber)
        {
            InitializeComponent();
            this.campaignFullyQtyAndSaleForm = campaignFullyQtyAndSaleForm;
            _FromForm = "CampaignFullyQtyAndSaleForm" + gridNumber;
        }
        /// <summary>
        /// มาจาก ซื้อครบ คละ ได้รับเงินส่วนลด
        /// </summary>
        /// <param name="campiagnFullyQtyAndDiscount"></param>
        public SelectedProductPopup(CampiagnFullyQtyAndDiscount campiagnFullyQtyAndDiscount)
        {
            InitializeComponent();
            this.campiagnFullyQtyAndDiscount = campiagnFullyQtyAndDiscount;
            _FromForm = "CampiagnFullyQtyAndDiscount";
        }
        /// <summary>
        /// ซื้อครบ เงิน ได้ สิทธิ์แลกซื้อ
        /// </summary>
        /// <param name="campiagnFullyAmountAndSaleForm"></param>
        public SelectedProductPopup(CampiagnFullyAmountAndSaleForm campiagnFullyAmountAndSaleForm, int gridNumber)
        {
            InitializeComponent();
            this.campiagnFullyAmountAndSaleForm = campiagnFullyAmountAndSaleForm;
            _FromForm = "CampiagnFullyAmountAndSaleForm" + gridNumber;
        }
        /// <summary>
        /// ซื้อครบ เงิน ได้ ส่วนลด
        /// </summary>
        /// <param name="campaignFullyAmountAndDisForm"></param>
        public SelectedProductPopup(CampaignFullyAmountAndDisForm campaignFullyAmountAndDisForm)
        {
            InitializeComponent();
            this.campaignFullyAmountAndDisForm = campaignFullyAmountAndDisForm;
            _FromForm = "CampaignFullyAmountAndDisForm";
        }
        /// <summary>
        /// สร้าง PO
        /// </summary>
        /// <param name="createPOForm"></param>
        public SelectedProductPopup(CreatePOForm createPOForm)
        {
            InitializeComponent();
            this.createPOForm = createPOForm;
            _FromForm = "CreatePOForm";
        }
        /// <summary>
        /// ซื้อครบได้แถม
        /// </summary>
        /// <param name="campaignFullyQtyAndGiftForm"></param>
        public SelectedProductPopup(CampaignFullyQtyAndGiftForm campaignFullyQtyAndGiftForm, int gridNumber)
        {
            InitializeComponent();
            this.campaignFullyQtyAndGiftForm = campaignFullyQtyAndGiftForm;
            _FromForm = "CampaignFullyQtyAndGiftForm" + gridNumber;
        }

        public SelectedProductPopup(POEditForm pOEditForm)
        {
            InitializeComponent();
            this.pOEditForm = pOEditForm;
            _FromForm = "POEditForm";
        }

        public SelectedProductPopup(WmsTransferBranchForm wmsTransferBranchForm)
        {
            InitializeComponent();
            this.wmsTransferBranchForm = wmsTransferBranchForm;
            _FromForm = "WmsTransferBranchForm";
        }
        /// <summary>
        /// มาจาก คืนของเสียให้ผู้จำหน่าย
        /// </summary>
        /// <param name="goodsReturnWmsForm"></param>
        public SelectedProductPopup(GoodsReturnWmsForm goodsReturnWmsForm)
        {
            InitializeComponent();
            this.goodsReturnWmsForm = goodsReturnWmsForm;
            _FromForm = "GoodsReturnWmsForm";
        }

        public SelectedProductPopup(StoreFrontAddForm storeFrontAddForm)
        {
            InitializeComponent();
            this.storeFrontAddForm = storeFrontAddForm;
            _FromForm = "StoreFrontAddForm";
        }

        public SelectedProductPopup(SaleOrderWarehouseEditForm saleOrderWarehouseEditForm)
        {
            InitializeComponent();
            this.saleOrderWarehouseEditForm = saleOrderWarehouseEditForm;
            _FromForm = "SaleOrderWarehouseEditForm";
        }

        public SelectedProductPopup(TransferOutForm transferOutForm)
        {
            InitializeComponent();
            this.transferOutForm = transferOutForm;
            _FromForm = "TransferOutForm";
        }

        public SelectedProductPopup(TransferOutEditForm transferOutEditForm)
        {
            InitializeComponent();
            this.transferOutEditForm = transferOutEditForm;
            _FromForm = "TransferOutEditForm";
        }

        public SelectedProductPopup(WasteManageForm wasteManageForm)
        {
            InitializeComponent();
            this.wasteManageForm = wasteManageForm;
            _FromForm = "WasteManageForm";
        }

        public SelectedProductPopup(WasteWarehouseManageForm wasteWarehouseManageForm)
        {
            InitializeComponent();
            this.wasteWarehouseManageForm = wasteWarehouseManageForm;
            _FromForm = "WasteWarehouseManageForm";
        }

        public SelectedProductPopup(StockCardListForm stockCardListForm)
        {
            InitializeComponent();
            _FromForm = "StockCardListForm";
            this.stockCardListForm = stockCardListForm;
        }

        public SelectedProductPopup(CheckStockFront1Form checkStockFront1Form)
        {
            InitializeComponent();
            _FromForm = "CheckStockFront1Form";
            this.checkStockFront1Form = checkStockFront1Form;
            button2.Visible = false;
        }

        public SelectedProductPopup(LeftInStock leftInStock)
        {
            InitializeComponent();
            _FromForm = "LeftInStock";
            this.leftInStock = leftInStock;

        }

        public SelectedProductPopup(ProductRawInOut productRawInOut)
        {
            InitializeComponent();
            _FromForm = "ProductRawInOut";
            this.productRawInOut = productRawInOut;
        }

        public SelectedProductPopup(ReportStockCard reportStockCard)
        {
            InitializeComponent();
            _FromForm = "ReportStockCard";
            this.reportStockCard = reportStockCard;
        }

        public SelectedProductPopup(StoreFrontAdjustForm storeFrontAdjustForm, List<ProductDetails> list)
        {
            InitializeComponent();
            this.storeFrontAdjustForm = storeFrontAdjustForm;
            this.list = list;
            _FromForm = "StoreFrontAdjustForm";
        }

        public SelectedProductPopup(GetGoodsForUseForm getGoodsForUseForm)
        {
            InitializeComponent();
            this.getGoodsForUseForm = getGoodsForUseForm;
            _FromForm = "GetGoodsForUseForm";
        }
        int gvNumber;
        public SelectedProductPopup(MergeProductForm mergeProductForm, List<ProductDetails> list, int gvNumber)
        {
            InitializeComponent();
            this.mergeProductForm = mergeProductForm;
            this.list = list;
            this.gvNumber = gvNumber;
            _FromForm = "MergeProductForm";
        }

        public SelectedProductPopup(PGoodsForm pGoodsForm)
        {
            InitializeComponent();
            this.pGoodsForm = pGoodsForm;
            _FromForm = "PGoodsForm";
        }
        /// <summary>
        /// รับ - จ่าย - คงเหลือ ห้องของเสีย
        /// </summary>
        /// <param name="wasteInOutStockForm"></param>
        public SelectedProductPopup(WasteInOutStockForm wasteInOutStockForm)
        {
            InitializeComponent();
            this.wasteInOutStockForm = wasteInOutStockForm;
            _FromForm = "WasteInOutStockForm";
        }

        public SelectedProductPopup(AdjustWasteForm adjustWasteForm)
        {
            InitializeComponent();
            this.adjustWasteForm = adjustWasteForm;
            _FromForm = "AdjustWasteForm";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// เลือกสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            Selected(id);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            Selected(id);
        }
        string _FromForm = "";
        private CampaignDiscountDayForm campaignDiscountDayForm;
        private CampiagnFullyQtyAndDiscount campiagnFullyQtyAndDiscount;
        private CampiagnFullyAmountAndSaleForm campiagnFullyAmountAndSaleForm;
        private CampaignFullyAmountAndDisForm campaignFullyAmountAndDisForm;
        private CreatePOForm createPOForm;
        private CampaignFullyQtyAndGiftForm campaignFullyQtyAndGiftForm;
        private POEditForm pOEditForm;
        private WmsTransferBranchForm wmsTransferBranchForm;
        private GoodsReturnWmsForm goodsReturnWmsForm;
        private StoreFrontAddForm storeFrontAddForm;
        private SaleOrderWarehouseEditForm saleOrderWarehouseEditForm;
        private TransferOutForm transferOutForm;
        private TransferOutEditForm transferOutEditForm;
        private WasteManageForm wasteManageForm;
        private WasteWarehouseManageForm wasteWarehouseManageForm;
        private StockCardListForm stockCardListForm;
        private CheckStockFront1Form checkStockFront1Form;
        private LeftInStock leftInStock;
        private ProductRawInOut productRawInOut;
        private ReportStockCard reportStockCard;
        private StoreFrontAdjustForm storeFrontAdjustForm;
        private GetGoodsForUseForm getGoodsForUseForm;
        private MergeProductForm mergeProductForm;
        private PGoodsForm pGoodsForm;
        private WasteInOutStockForm wasteInOutStockForm;
        private AdjustWasteForm adjustWasteForm;

        void Selected(int id)
        {
            var product = SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == id);
            switch (_FromForm)
            {

                case "AdjustWasteForm":
                    this.adjustWasteForm.BinddingProduct(id); // adjust ห่้องของเสีย
                    break;
                case "WasteInOutStockForm":
                    this.wasteInOutStockForm.BinddingProduct(id); // สินค้า แปรรูป
                    break;
                case "PGoodsForm":
                    this.pGoodsForm.BinddingProduct(id); // สินค้า แปรรูป
                    break;
                case "MergeProductForm":
                    this.mergeProductForm.BinddingProduct(id, gvNumber); // ย้ายฐานสินค้า
                    break;
                case "GetGoodsForUseForm": // เบิกใช้เอง
                    this.getGoodsForUseForm.BinddingProductChoose(id);
                    break;
                case "StoreFrontAdjustForm": // รายงาน 
                    this.storeFrontAdjustForm.BinddingProduct(id);
                    break;
                case "ReportStockCard": // รายงาน 
                    this.reportStockCard.BinddingProduct(id);
                    break;
                case "ProductRawInOut": // รายงาน รับ-จ่าย- คงเหลือ
                    this.productRawInOut.BinddingProduct(id);
                    break;
                case "LeftInStock": // รายงาน สินค้าคงเหลือ
                    this.leftInStock.BinddingProduct(id);
                    break;
                case "ManageProductForm":
                    this.manageProductForm.BinddingProduct(id);
                    break;
                case "SaleOrderWarehouseForm":
                    this.saleOrderWarehouseForm.BinddingSelectedProduct(product);
                    break;
                case "CampaignDiscountDayForm":
                    this.campaignDiscountDayForm.BinddingSelectedProduct(product);
                    break;
                case "CampaignFullyQtyAndSaleForm1": // datagrid1
                    this.campaignFullyQtyAndSaleForm.BinddingSelectedProduct(product);
                    break;
                case "CampaignFullyQtyAndSaleForm2": // datagrid2
                    this.campaignFullyQtyAndSaleForm.BinddingSelectedProduct(product, true);
                    break;
                case "CampiagnFullyQtyAndDiscount":
                    this.campiagnFullyQtyAndDiscount.BinddingSelectedProduct(product);
                    break;
                case "CampiagnFullyAmountAndSaleForm1": // datagrid1
                    this.campiagnFullyAmountAndSaleForm.BinddingSelectedProduct(product);
                    break;
                case "CampiagnFullyAmountAndSaleForm2": // datagrid2
                    this.campiagnFullyAmountAndSaleForm.BinddingSelectedProduct(product, true);
                    break;
                case "CampaignFullyAmountAndDisForm":
                    this.campaignFullyAmountAndDisForm.BinddingSelectedProduct(product);
                    break;
                case "CreatePOForm":
                    this.createPOForm.BinddingProductChoose(id);
                    break;
                case "CampaignFullyQtyAndGiftForm1":
                    this.campaignFullyQtyAndGiftForm.BinddingGrid1(id);
                    break;
                case "CampaignFullyQtyAndGiftForm2":
                    this.campaignFullyQtyAndGiftForm.BinddingGrid2(id);
                    break;
                case "POEditForm":
                    this.pOEditForm.BinddingProductChoose(id);
                    break;
                case "WmsTransferBranchForm":
                    this.wmsTransferBranchForm.BinddingProductChoose(id);
                    break;
                case "GoodsReturnWmsForm":
                    this.goodsReturnWmsForm.BinddingProductChoose(id);
                    break;
                case "StoreFrontAddForm":
                    this.storeFrontAddForm.BinddingProductChoose(id);
                    break;
                case "SaleOrderWarehouseEditForm":
                    this.saleOrderWarehouseEditForm.BinddingProductChoose(id);
                    break;
                case "TransferOutForm":
                    this.transferOutForm.BinddingProductChoose(id);
                    break;
                case "TransferOutEditForm":
                    this.transferOutEditForm.BinddingProductChoose(id);
                    break;
                case "WasteManageForm":
                    this.wasteManageForm.BinddingProductChoose(id);
                    break;
                case "WasteWarehouseManageForm":
                    this.wasteWarehouseManageForm.BinddingProductChoose(id);
                    break;
                case "StockCardListForm":
                    this.stockCardListForm.BinddingProductChoose(id);
                    break;
                case "CheckStockFront1Form":
                    this.checkStockFront1Form.BinddingProductChoose(id);
                    break;
            }
            this.Dispose();
        }
        private void SelectedProductPopup_Load(object sender, EventArgs e)
        {
            if (_FromForm == "GoodsReturnWmsForm")
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    List<int> fkProDtlInWaste = db.WasteWarehouseDetails.Where(w => w.Enable == true).Select(w => w.FKProductDetails).Distinct().ToList<int>();
                    var getPdtl = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true && fkProDtlInWaste.Contains(w.Id)).ToList();
                    AutoCompleteStringCollection colPdtl = new AutoCompleteStringCollection();
                    foreach (var item in getPdtl)
                    {
                        colPdtl.Add(item.Products.ThaiName);
                    }
                    textBoxSearchKey.AutoCompleteCustomSource = colPdtl;

                    foreach (var item in getPdtl)
                    {
                        dataGridView1.Rows.Add(item.Id, item.Code, item.Products.ThaiName, item.ProductUnit.Name, Library.ConvertDecimalToStringForm(item.PackSize), Library.ConvertDecimalToStringForm(item.SellPrice), item.Description);
                    }
                }

            }
            else
            {
                var getPdtl = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
                AutoCompleteStringCollection colPdtl = new AutoCompleteStringCollection();
                foreach (var item in getPdtl)
                {
                    colPdtl.Add(item.Products.ThaiName);
                }
                textBoxSearchKey.AutoCompleteCustomSource = colPdtl;

                foreach (var item in SingletonProduct.Instance().ProductDetails.Where(w => w.Enable).Take(MyConstant.SelectTopRow.Product))
                {
                    dataGridView1.Rows.Add(item.Id, item.Code, item.Products.ThaiName, item.ProductUnit.Name, Library.ConvertDecimalToStringForm(item.PackSize), Library.ConvertDecimalToStringForm(item.SellPrice), item.Description);
                }
            }

            textBoxSearchKey.Select();
            textBoxSearchKey.SelectAll();
        }
        private void textBoxSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string key = textBoxSearchKey.Text.Trim();
                    var result = new List<ProductDetails>();
                    if (_FromForm == "GoodsReturnWmsForm")
                    {
                        using (SSLsEntities db = new SSLsEntities())
                        {
                            List<int> fkProDtlInWaste = db.WasteWarehouseDetails.Where(w => w.Enable == true).Select(w => w.FKProductDetails).Distinct().ToList<int>();
                            var getPdtl = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true && fkProDtlInWaste.Contains(w.Id)).ToList();

                            if (radioButtonCode.Checked)
                            {
                                // ถ้าหาด้วย code
                                var getFirst = getPdtl.FirstOrDefault(w => w.Code == key && w.Enable == true);
                                if (getFirst != null)
                                {
                                    result = getPdtl.Where(w => w.FKProduct == getFirst.FKProduct && w.Enable == true).Take(MyConstant.SelectTopRow.Product).ToList();
                                }
                                else
                                {
                                    result = getPdtl.Where(w => w.Code.Contains(key) && w.Enable == true).Take(MyConstant.SelectTopRow.Product).ToList();
                                }
                            }
                            else
                            {
                                string[] words = key.Split(' ');
                                // ถ้าหาด้วยชื่อ

                                //result = SingletonProduct.Instance().ProductDetails.Where(w => w.Products.ThaiName.Contains(key) && w.Enable == true).ToList();
                                switch (words.Count())
                                {
                                    case 1:
                                        result = getPdtl
                                            .Where(w => w.Products.ThaiName.Contains(words[0]) && w.Enable == true).Take(MyConstant.SelectTopRow.Product).ToList();
                                        break;
                                    case 2:
                                        result = getPdtl
                                            .Where(w => w.Products.ThaiName.Contains(words[0]) && w.Products.ThaiName.Contains(words[1]) &&
                                            w.Enable == true).ToList();
                                        break;
                                    case 3:
                                        result = getPdtl
                                            .Where(w => w.Products.ThaiName.Contains(words[0]) &&
                                            w.Products.ThaiName.Contains(words[1]) &&
                                            w.Products.ThaiName.Contains(words[2]) &&
                                            w.Enable == true).ToList();
                                        break;
                                    case 4:
                                        result = getPdtl
                                         .Where(w => w.Products.ThaiName.Contains(words[0]) &&
                                         w.Products.ThaiName.Contains(words[1]) &&
                                         w.Products.ThaiName.Contains(words[2]) &&
                                          w.Products.ThaiName.Contains(words[3]) &&
                                         w.Enable == true).ToList();
                                        break;
                                    case 5:
                                        result = getPdtl
                                        .Where(w => w.Products.ThaiName.Contains(words[0]) &&
                                        w.Products.ThaiName.Contains(words[1]) &&
                                        w.Products.ThaiName.Contains(words[2]) &&
                                         w.Products.ThaiName.Contains(words[3]) &&
                                             w.Products.ThaiName.Contains(words[4]) &&
                                        w.Enable == true).ToList();
                                        break;
                                    default:
                                        break;
                                }
                            }

                            dataGridView1.Rows.Clear();
                            dataGridView1.Refresh();
                            foreach (var item in result)
                            {
                                dataGridView1.Rows.Add(item.Id, item.Code, item.Products.ThaiName, item.ProductUnit.Name, Library.ConvertDecimalToStringForm(item.PackSize), Library.ConvertDecimalToStringForm(item.SellPrice), item.Description);
                            }
                        }
                    }
                    else
                    {
                        if (radioButtonCode.Checked)
                        {
                            // ถ้าหาด้วย code
                            var getFirst = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Code == key && w.Enable == true);
                            if (getFirst != null)
                            {
                                result = SingletonProduct.Instance().ProductDetails.Where(w => w.FKProduct == getFirst.FKProduct && w.Enable == true).Take(MyConstant.SelectTopRow.Product).ToList();
                            }
                            else
                            {
                                result = SingletonProduct.Instance().ProductDetails.Where(w => w.Code.Contains(key) && w.Enable == true).Take(MyConstant.SelectTopRow.Product).ToList();
                            }
                        }
                        else
                        {
                            string[] words = key.Split(' ');
                            // ถ้าหาด้วยชื่อ

                            //result = SingletonProduct.Instance().ProductDetails.Where(w => w.Products.ThaiName.Contains(key) && w.Enable == true).ToList();
                            switch (words.Count())
                            {
                                case 1:
                                    result = SingletonProduct.Instance().ProductDetails
                                        .Where(w => w.Products.ThaiName.Contains(words[0]) && w.Enable == true).Take(MyConstant.SelectTopRow.Product).ToList();
                                    break;
                                case 2:
                                    result = SingletonProduct.Instance().ProductDetails
                                        .Where(w => w.Products.ThaiName.Contains(words[0]) && w.Products.ThaiName.Contains(words[1]) &&
                                        w.Enable == true).ToList();
                                    break;
                                case 3:
                                    result = SingletonProduct.Instance().ProductDetails
                                        .Where(w => w.Products.ThaiName.Contains(words[0]) &&
                                        w.Products.ThaiName.Contains(words[1]) &&
                                        w.Products.ThaiName.Contains(words[2]) &&
                                        w.Enable == true).ToList();
                                    break;
                                case 4:
                                    result = SingletonProduct.Instance().ProductDetails
                                     .Where(w => w.Products.ThaiName.Contains(words[0]) &&
                                     w.Products.ThaiName.Contains(words[1]) &&
                                     w.Products.ThaiName.Contains(words[2]) &&
                                      w.Products.ThaiName.Contains(words[3]) &&
                                     w.Enable == true).ToList();
                                    break;
                                case 5:
                                    result = SingletonProduct.Instance().ProductDetails
                                    .Where(w => w.Products.ThaiName.Contains(words[0]) &&
                                    w.Products.ThaiName.Contains(words[1]) &&
                                    w.Products.ThaiName.Contains(words[2]) &&
                                     w.Products.ThaiName.Contains(words[3]) &&
                                         w.Products.ThaiName.Contains(words[4]) &&
                                    w.Enable == true).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }

                        dataGridView1.Rows.Clear();
                        dataGridView1.Refresh();
                        foreach (var item in result)
                        {
                            dataGridView1.Rows.Add(item.Id, item.Code, item.Products.ThaiName, item.ProductUnit.Name, Library.ConvertDecimalToStringForm(item.PackSize), Library.ConvertDecimalToStringForm(item.SellPrice), item.Description);
                        }
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
