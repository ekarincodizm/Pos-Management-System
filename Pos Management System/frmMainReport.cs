using Microsoft.Reporting.WinForms;
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
using static Pos_Management_System.CheckStockFront1ListForm;

namespace Pos_Management_System
{
    public partial class frmMainReport : Form
    {
        public frmMainReport()
        {
            InitializeComponent();
        }
        public frmMainReport(SaleOrderWarehouseListForm saleOrderWarehouseListForm, string code)
        {
            InitializeComponent();
            this.saleOrderWarehouseListForm = saleOrderWarehouseListForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.OrderWarehouse;
        }

        public frmMainReport(TransferOutListForm transferOutListForm, string code, string brachDestination)
        {
            InitializeComponent();
            this.transferOutListForm = transferOutListForm;
            this.code = code;
            this.brachDestination = brachDestination;
            this.TypeReport = MyConstant.TypeReport.TransferOut;
        }

        public frmMainReport(WastManageListForm wastManageListForm, string code)
        {
            InitializeComponent();
            this.wastManageListForm = wastManageListForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.WastManage;
        }

        public frmMainReport(CNPosListForm cNPosListForm, string code)
        {
            InitializeComponent();
            this.cNPosListForm = cNPosListForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.CN;
        }

        public frmMainReport(GoodsReturnWmsListForm goodsReturnWmsListForm, string code)
        {
            InitializeComponent();
            this.goodsReturnWmsListForm = goodsReturnWmsListForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.GoodsReturnWms;
        }

        public frmMainReport(RCVPODetailForm rCVPODetailForm, string code)
        {
            InitializeComponent();
            this.rCVPODetailForm = rCVPODetailForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.RCVPO;
        }

        public frmMainReport(CheckStockFront1Form checkStockFront1Form, string code)
        {
            InitializeComponent();
            this.checkStockFront1Form = checkStockFront1Form;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.CheckStock;
        }
        public frmMainReport(CheckStockFront1ListForm checkStockFront1ListForm, string code)
        {
            InitializeComponent();
            this.checkStockFront1ListForm = checkStockFront1ListForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.CheckStock;
        }

        public frmMainReport(RCVPOEditForm rCVPOEditForm, string code)
        {
            InitializeComponent();
            this.rCVPOEditForm = rCVPOEditForm;
            this.code = code;
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.PORcv.SingleOrDefault(w => w.Enable == true && w.Code == code);
                data.PrintNumber = data.PrintNumber + 1;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
            }
            this.TypeReport = MyConstant.TypeReport.RCVPO;
        }

        public frmMainReport(CheckStockFront1ListForm checkStockFront1ListForm, List<classCheckStock> ls)
        {
            this.checkStockFront1ListForm = checkStockFront1ListForm;
            this.ls = ls;

            InitializeComponent();
            this.checkStockFront1ListForm = checkStockFront1ListForm;

            this.TypeReport = MyConstant.TypeReport.CheckStockDoc;

        }

        public frmMainReport(PORcvInvoiceForm pORcvInvoiceForm, string code)
        {
            InitializeComponent();
            this.pORcvInvoiceForm = pORcvInvoiceForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.RCVPO;
        }

        public frmMainReport(WasteManageForm wasteManageForm, string code)
        {
            InitializeComponent();
            this.wasteManageForm = wasteManageForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.WastManage;
        }

        public frmMainReport(ProductRawInOut productRawInOut, DataTable dt, bool reportValue)
        {
            InitializeComponent();
            this.productRawInOut = productRawInOut;
            this.dt = dt;
            if (reportValue)
            {
                this.TypeReport = MyConstant.TypeReport.InOutBalValueReport;
            }
            else // แสดงแค่จำนวน
            {
                this.TypeReport = MyConstant.TypeReport.InOutBalReport;
            }
        }

        public frmMainReport(ReportStockCard reportStockCard, DataTable dt, string user, string whereDate, string whereProduct, string whereGroupP)
        {
            InitializeComponent();
            this.reportStockCard = reportStockCard;
            this.dt = dt;
            this.user = user;
            this.whereDate = whereDate;
            this.whereProduct = whereProduct;
            this.whereGroupP = whereGroupP;

            this.TypeReport = MyConstant.TypeReport.StockCard;
        }
        public frmMainReport(LeftInStock leftInStock, DataTable dt)
        {
            InitializeComponent();
            this.leftInStock = leftInStock;
            this.dt = dt;

            this.TypeReport = MyConstant.TypeReport.LeftInStock;
        }
        public frmMainReport(LeftInStock leftInStock, DataTable dt, int leftInStockVat)
        {
            InitializeComponent();
            this.leftInStockVat = leftInStockVat;
            this.leftInStock = leftInStock;
            this.dt = dt;
            this.TypeReport = leftInStockVat;
        }

        public frmMainReport(GoodsReturnWmsForm goodsReturnWmsForm, string code)
        {
            InitializeComponent();
            this.goodsReturnWmsForm = goodsReturnWmsForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.GoodsReturnWms;
        }

        public frmMainReport(GoodsReturnVendorConfirmForm goodsReturnVendorConfirmForm, string code)
        {
            InitializeComponent();
            this.goodsReturnVendorConfirmForm = goodsReturnVendorConfirmForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.GoodsReturnWms;
        }

        public frmMainReport(GoodsAutoReturnWmsForm goodsAutoReturnWmsForm, string code)
        {
            InitializeComponent();
            this.goodsAutoReturnWmsForm = goodsAutoReturnWmsForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.GoodsReturnWms;
        }

        public frmMainReport(CountProduct countProduct, DataTable dt)
        {
            InitializeComponent();
            this.countProduct = countProduct;
            this.dt = dt;
            this.TypeReport = MyConstant.TypeReport.CountProduct;
        }

        public frmMainReport(MemberActiveAndResignReport memberActiveAndResignReport, DataTable dt)
        {
            InitializeComponent();
            this.memberActiveAndResignReport = memberActiveAndResignReport;
            this.dt = dt;
            this.TypeReport = MyConstant.TypeReport.MemberActive;
        }

        public frmMainReport(StoreFrontAdjustForm storeFrontAdjustForm, string code)
        {
            InitializeComponent();
            this.storeFrontAdjustForm = storeFrontAdjustForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.Adjust;
        }

        public frmMainReport(GetGoodsForUseForm getGoodsForUseForm, string code)
        {
            InitializeComponent();
            this.getGoodsForUseForm = getGoodsForUseForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.GetGoodsFrontUse;
        }
        public frmMainReport(GetGoodsUsedListForm getGoodsUsedListForm, string code)
        {
            InitializeComponent();
            this.getGoodsUsedListForm = getGoodsUsedListForm;
            this.code = code;
            this.TypeReport = MyConstant.TypeReport.GetGoodsFrontUse;
        }
        /// <summary>
        /// ปันผล สมาชิก
        /// </summary>
        /// <param name="memberChange2560Dialog"></param>
        /// <param name="receiptPaper"></param>
        public frmMainReport(MemberChange2560Dialog memberChange2560Dialog, List<MemberChange2560Dialog.MemberChange2560> receiptPaper)
        {
            InitializeComponent();
            this.memberChange2560Dialog = memberChange2560Dialog;
            this.receiptPaper = receiptPaper;
            this.TypeReport = MyConstant.TypeReport.MemberChange2560;
        }
        /// <summary>
        /// รายงานสรุปการจ่ายปันผล-เฉลี่ยคืน
        /// </summary>
        /// <param name="reportReceiptMemberForm"></param>
        /// <param name="dt"></param>
        /// <param name="reportMemberChange"></param>
        public frmMainReport(ReportReceiptMemberForm reportReceiptMemberForm, DataTable dt, int reportMemberChange)
        {
            InitializeComponent();
            this.reportReceiptMemberForm = reportReceiptMemberForm;
            this.dt = dt;
            this.reportMemberChange = reportMemberChange;
            this.TypeReport = MyConstant.TypeReport.ReportMemberChange;
        }

        public DataTable d1;
        public DataTable d2;
        public DataTable d3;
        private string code;
        private SaleOrderWarehouseListForm saleOrderWarehouseListForm;
        private int TypeReport;
        private TransferOutListForm transferOutListForm;
        private string brachDestination;
        private WastManageListForm wastManageListForm;
        private CNPosListForm cNPosListForm;
        private GoodsReturnWmsListForm goodsReturnWmsListForm;
        private RCVPODetailForm rCVPODetailForm;
        private CheckStockFront1Form checkStockFront1Form;
        private RCVPOEditForm rCVPOEditForm;
        private CheckStockFront1ListForm checkStockFront1ListForm;
        private List<classCheckStock> ls;
        private PORcvInvoiceForm pORcvInvoiceForm;
        private WasteManageForm wasteManageForm;
        private ProductRawInOut productRawInOut;
        private DataTable dt;
        private LeftInStock leftInStock;
        private ReportStockCard reportStockCard;
        private string user;
        private string whereDate;
        private string whereProduct;
        private string whereGroupP;
        private int leftInStockVat;
        private GoodsReturnWmsForm goodsReturnWmsForm;
        private GoodsReturnVendorConfirmForm goodsReturnVendorConfirmForm;
        private GoodsAutoReturnWmsForm goodsAutoReturnWmsForm;
        private CountProduct countProduct;
        private bool v;
        private MemberActiveAndResignReport memberActiveAndResignReport;
        private StoreFrontAdjustForm storeFrontAdjustForm;
        private GetGoodsForUseForm getGoodsForUseForm;
        private GetGoodsUsedListForm getGoodsUsedListForm;
        private MemberChange2560Dialog memberChange2560Dialog;
        private List<MemberChange2560Dialog.MemberChange2560> receiptPaper;
        private ReportReceiptMemberForm reportReceiptMemberForm;
        private int reportMemberChange;

        private void frmMainReport_Load(object sender, EventArgs e)
        {
            try
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    ReportParameter User = new ReportParameter("User", Singleton.SingletonAuthen.Instance().Name);
                    ReportParameter Tel = new ReportParameter("Tel", Singleton.SingletonAuthen.Instance().MyBranch.Tel);
                    ReportParameter Address = new ReportParameter("Address", Singleton.SingletonAuthen.Instance().MyBranch.Address);
                    ReportParameter Fax = new ReportParameter("Fax", Singleton.SingletonAuthen.Instance().MyBranch.Fax);
                    ReportParameter TaxNo = new ReportParameter("TaxNo", Singleton.SingletonAuthen.Instance().MyBranch.TaxNo);
                    ReportParameter Email = new ReportParameter("Email", Singleton.SingletonAuthen.Instance().MyBranch.Email);
                    ReportParameter BranchName = new ReportParameter("BranchName", Singleton.SingletonAuthen.Instance().MyBranch.Name + " " + Singleton.SingletonAuthen.Instance().MyBranch.BranchNo);
                    ReportParameter PrintDate = new ReportParameter("PrintDate", Library.ConvertDateToThaiDate(DateTime.Now, true));

                    ReportParameter[] HeaderParams = { User, Tel, Fax, TaxNo, Email, BranchName, Address, PrintDate };

                    switch (this.TypeReport)
                    {
                        case MyConstant.TypeReport.ReportMemberChange: /// รายงานสรุปการจ่ายปันผล-เฉลี่ยคืน

                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();

                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", this.dt));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_MemberReceiptReport.rdlc";
                            foreach (ReportParameter param in HeaderParams)
                            {
                                this.reportViewer1.LocalReport.SetParameters(param);
                            }
                            this.reportViewer1.RefreshReport();
                            ///
                            break;

                        case MyConstant.TypeReport.MemberChange2560: /// ใบเสร็จปันผล

                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                   
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", receiptPaper));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_Dividend.rdlc";
                            foreach (ReportParameter param in HeaderParams)
                            {
                                this.reportViewer1.LocalReport.SetParameters(param);
                            }
                            this.reportViewer1.RefreshReport();
                            ///
                            break;
                        case MyConstant.TypeReport.GetGoodsFrontUse: /// ใบเบิกใช้เอง
                            var headPaper = Repository.GetGoodsStorefrontUse.getHead(this.code);
                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", headPaper));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_GetGoodsUse.rdlc";
                            this.reportViewer1.RefreshReport();
                            ///
                            break;
                        case MyConstant.TypeReport.Adjust:
                            var dtAdjust = (Repository.Adjust.getData(this.code));
                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dtAdjust));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_Adjust.rdlc";
                            foreach (ReportParameter param in HeaderParams)
                            {
                                this.reportViewer1.LocalReport.SetParameters(param);
                            }
                            this.reportViewer1.RefreshReport();
                            break;
                        case MyConstant.TypeReport.MemberActive: // สมาชิก คงอยู่ - ลาออก
                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_MemberActive.rdlc";
                            foreach (ReportParameter param in HeaderParams)
                            {
                                this.reportViewer1.LocalReport.SetParameters(param);
                            }
                            this.reportViewer1.RefreshReport();
                            break;
                        case MyConstant.TypeReport.InOutBalValueReport: // ราคาทุน - ราคาขาย มูลค่า
                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_ProductRawInOutDtl.rdlc";
                            foreach (ReportParameter param in HeaderParams)
                            {
                                this.reportViewer1.LocalReport.SetParameters(param);
                            }
                            this.reportViewer1.RefreshReport();
                            break;
                        //this.TypeReport = MyConstant.TypeReport.InOutBalValueReport;
                        case MyConstant.TypeReport.CountProduct:
                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_CountProduct.rdlc";
                            foreach (ReportParameter param in HeaderParams)
                            {
                                this.reportViewer1.LocalReport.SetParameters(param);
                            }
                            this.reportViewer1.RefreshReport();
                            break;

                        case MyConstant.TypeReport.LeftInStockVat: // ราคาทุน - ราคาขาย
                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_LeftInStockVat.rdlc";
                            foreach (ReportParameter param in HeaderParams)
                            {
                                this.reportViewer1.LocalReport.SetParameters(param);
                            }
                            this.reportViewer1.RefreshReport();
                            break;
                        case MyConstant.TypeReport.StockCard: // รายงาน StockCard
                            /// ใบโอนของข้ามสาขา
                            //var head_fmo = Repository.TransferOut.getHead(this.code, brachDestination);
                            ReportParameter user = new ReportParameter("User", this.user);
                            ReportParameter WhereDate = new ReportParameter("WhereDate", this.whereDate);
                            ReportParameter WhereProduct = new ReportParameter("WhereProduct", this.whereProduct);
                            ReportParameter WhereGroupP = new ReportParameter("WhereGroupP", this.whereGroupP);
                            ReportParameter[] HeaderParams1 = { user, WhereDate, WhereProduct, WhereGroupP };


                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_StockCard.rdlc";
                            foreach (ReportParameter param in HeaderParams1)
                            {
                                this.reportViewer1.LocalReport.SetParameters(param);
                            }
                            this.reportViewer1.RefreshReport();
                            ///
                            break;
                        case MyConstant.TypeReport.LeftInStock: // รายงานสินค้าคงเหลือ
                            /// 
                            //var head_cskdoc = Repository.CheckStockDoc.getData(this.ls);
                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_LeftInStock.rdlc";
                            foreach (ReportParameter param in HeaderParams)
                            {
                                this.reportViewer1.LocalReport.SetParameters(param);
                            }
                            this.reportViewer1.RefreshReport();
                            ///
                            break;
                        case MyConstant.TypeReport.InOutBalReport: // รายงาน รับ-จ่าย-คงเหลือ

                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_ProductRawInOut.rdlc";
                            foreach (ReportParameter param in HeaderParams)
                            {
                                this.reportViewer1.LocalReport.SetParameters(param);
                            }
                            this.reportViewer1.RefreshReport();
                            break;
                        case MyConstant.TypeReport.CheckStockDoc:
                            /// 
                            var head_cskdoc = Repository.CheckStockDoc.getData(this.ls);
                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", head_cskdoc));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_CheckStockDoc.rdlc";
                            this.reportViewer1.RefreshReport();
                            ///
                            break;
                        case MyConstant.TypeReport.CheckStock:
                            /// 
                            var head_csk = Repository.CheckStock.getData(this.code);
                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", head_csk));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_CheckStock.rdlc";
                            this.reportViewer1.RefreshReport();
                            ///
                            break;
                        case MyConstant.TypeReport.RCVPO:
                            /// 
                            var head_rcvpo = Repository.RCVPO.getData(this.code);
                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", head_rcvpo));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_RCVPO.rdlc";
                            this.reportViewer1.RefreshReport();
                            ///
                            break;
                        case MyConstant.TypeReport.GoodsReturnWms:
                            /// ใบส่งของคืนให้ผู้จัดจำหน่าย
                            var head_grw = Repository.GoodsReturnWms.getData(this.code);
                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", head_grw));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_GoodsReturnWms.rdlc";
                            this.reportViewer1.RefreshReport();
                            ///
                            break;
                        case MyConstant.TypeReport.CN:
                            // ใบรับคืน
                            var head_cn = (Repository.CN.getHead(this.code));
                            var list_cn = Repository.CN.getList(this.code);
                            var footer_cn = Repository.CN.getTotal(list_cn);

                            this.d1 = Library.ConvertToDataTable(head_cn);
                            this.d2 = Library.ConvertToDataTable(list_cn);
                            this.d3 = Library.ConvertToDataTable(footer_cn);

                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", d1));
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet2", d2));
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet3", d3));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_CN.rdlc";
                            this.reportViewer1.RefreshReport();
                            break;
                        case MyConstant.TypeReport.WastManage:
                            /// ใบของเสีย
                            var head_wm = Repository.WastManage.getHead(this.code);
                            //var list_wm = Repository.WastManage.getList(this.code);
                           

                            var getList = db.StoreFrontTransferWasteDtl.Where(w => w.FKStoreFrontTransferWaste == code && w.Enable == true).ToList();
                            List<Repository.c_WastManage_Details> listData = new List<Repository.c_WastManage_Details>();
                            List<StoreFrontTransferWasteDtl> result = new List<StoreFrontTransferWasteDtl>();
                            result.AddRange(getList);
                            foreach (var item in getList)
                            {
                                // get 1 fkproductDtl
                                var data = result.Where(w => w.FKProductDetails == item.FKProductDetails).ToList();
                                if (data.Count() == 0)
                                {
                                    continue;
                                }
                                Repository.c_WastManage_Details stack = new Repository.c_WastManage_Details();
                                stack.DescriptionList = "-";
                                var getProDtl = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.FKProductDetails);
                                stack.Packsize = getProDtl.PackSize;
                                // ทุนไม่รวมภาษี 
                                stack.Price = data.Sum(w => w.Qty) * getProDtl.CostOnly;
                                stack.ProductDetail = getProDtl.Products.ThaiName;
                                stack.ProductNo = getProDtl.Code;
                                stack.ProductUnit = getProDtl.ProductUnit.Name;
                                stack.QTY = data.Sum(w => w.Qty);
                                stack.ThaiBath = "";
                                listData.Add(stack);
                                foreach (var getcode in data)
                                {
                                    result.Remove(getcode);
                                }
                            }
                            //GetValueByFKProduct(result);
                            var footer_wm = Repository.WastManage.getTotal(listData);
                            this.d1 = Library.ConvertToDataTable(head_wm);
                            this.d2 = Library.ConvertToDataTable(listData);
                            this.d3 = Library.ConvertToDataTable(footer_wm);

                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", d1));
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet2", d2));
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet3", d3));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_WastManage.rdlc";
                            this.reportViewer1.RefreshReport();
                            ///
                            break;
                        case MyConstant.TypeReport.TransferOut:
                            /// ใบโอนของข้ามสาขา
                            var head_fmo = Repository.TransferOut.getHead(this.code, brachDestination);
                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", head_fmo));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_TranferOut.rdlc";
                            this.reportViewer1.RefreshReport();
                            ///
                            break;
                        case MyConstant.TypeReport.OrderWarehouse:
                            var head = (Repository.SaleOrderWarehouse.getHead(this.code));
                            var checkHead = db.SaleOrderWarehouse.SingleOrDefault(w => w.Code == this.code).Id;
                            var list = Repository.SaleOrderWarehouse.getList(checkHead);
                            var footer = Repository.SaleOrderWarehouse.getTotal(list);

                            this.d1 = Library.ConvertToDataTable(head);
                            this.d2 = Library.ConvertToDataTable(list);
                            this.d3 = Library.ConvertToDataTable(footer);

                            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                            this.reportViewer1.LocalReport.DataSources.Clear();
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", d1));
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet2", d2));
                            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet3", d3));
                            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.rw_SaleOrderWarehouse.rdlc";
                            this.reportViewer1.RefreshReport();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือนจากระบบ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
