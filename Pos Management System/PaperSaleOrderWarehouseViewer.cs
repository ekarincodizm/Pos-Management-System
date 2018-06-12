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
    public partial class PaperSaleOrderWarehouseViewer : Form
    {
        private IEnumerable<string> codeSelected;
        private int docQty;
        private SaleOrderWarehouseListForm saleOrderWarehouseListForm;

        public PaperSaleOrderWarehouseViewer()
        {
            InitializeComponent();
        }

        public PaperSaleOrderWarehouseViewer(SaleOrderWarehouseListForm saleOrderWarehouseListForm, int docQty, IEnumerable<string> codeSelected)
        {
            InitializeComponent();
            this.saleOrderWarehouseListForm = saleOrderWarehouseListForm;
            this.docQty = docQty;
            this.codeSelected = codeSelected;
        }

        private void PaperSaleOrderWarehouseViewer_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var thisBranch = SingletonAuthen.Instance().MyBranch;
                List<SaleOrderWarehouseList> objs = new List<SaleOrderWarehouseList>();
                SaleOrderWarehouseList obj;
                var data = db.SaleOrderWarehouse.Where(w => codeSelected.Contains(w.Code)).ToList();
                foreach (var item in data)
                {
                    obj = new SaleOrderWarehouseList();
                    obj.BranchName = thisBranch.Name + " สาขา " + thisBranch.BranchNo;
                    obj.DebtorCode = item.Debtor.Code;

                    obj.MemberCode = item.Member.Code;
                    obj.DocNo = item.Code;
                    List<SaleOrderWarehouseList.Details> details = new List<SaleOrderWarehouseList.Details>();
                    int i = 1;
                    foreach (var dtl in item.SaleOrderWarehouseDtl.Where(w => w.Enable == true))
                    {
                        details.Add(new SaleOrderWarehouseList.Details()
                        {
                            Number = i + "",
                            Code = dtl.ProductDetails.Code
                        });
                        i++;
                    }
                    obj.DataDetails = details;
                    objs.Add(obj);
                }  
                SaleOrderWarehouseListBindingSource.DataSource = objs;
                this.reportViewer1.RefreshReport();
            }
            
            //bv.BranchName = thisBranch.Name + " สาขา " + thisBranch.BranchNo;
            //bv.BranchAddress = thisBranch.Address;
            //bv.BranchTax = "เลขประจำตัวผู้เสียภาษี " + thisBranch.TaxNo;
            //bv.BranchTelAndFax = "โทรศัพท์ " + thisBranch.Tel + " แฟกซ์ " + thisBranch.Fax;

            //BranchValueBindingSource.DataSource = branch;
            //this.reportViewer1.RefreshReport();
        }
    }
}
