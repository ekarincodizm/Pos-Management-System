using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    /// <summary>
    /// เก็บค่า Master ที่ ไม่ค่อยได้อัพเดท โดย ไม่มีเงื่อนไข Enable
    /// </summary>
    public class SingletonPriority1
    {
        private static SingletonPriority1 _instance;
        //public List<Branch> Branchs { get; set; }
        public List<BudgetYear> BudgetYears { get; set; }
        //public BudgetYear ThisBudgetYear { get; set; }
        public List<PaymentType> PaymentTypes { get; set; }

        public List<ProductVatType> ProductVatType { get; set; }
        public List<CostType> CostType { get; set; }
        //public List<ProductUnit> ProductUnit { get; set; }
        //public List<ProductCategory> ProductCategory { get; set; }
        //public List<ProductGroups> ProductGroups { get; set; }
        public List<Supplier> Supplier { get; set; }
        //public List<Vendor> Vendor { get; set; }
        //public List<ProductBrands> ProductBrands { get; set; }
        public List<ProductSize> ProductSize { get; set; }
        public List<ProductColor> ProductColor { get; set; }
        public List<Zone> Zone { get; set; }
        public List<DeliveryType> DeliveryTypes { get; set; }
        public List<Users> Users { get; set; }
        //public List<Shelf> Shelfs { get; set; }

        public static SingletonPriority1 Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonPriority1();
                    //var data = db.ProductDetails
                    //        .Include("Products")
                    //    .Include("Products.Supplier")
                    //        .Include("Products.ProductBrands")
                    //    .Include("Products.ProductVatType")
                    //    .Include("ProductUnit").ToList();

                    //_instance.Branchs = db.Branch.ToList();

                    _instance.BudgetYears = db.BudgetYear.ToList();
                    //_instance.ThisBudgetYear = budgetYears.FirstOrDefault(w => w.Enable == true && w.IsCurrent == true);

                    _instance.PaymentTypes = db.PaymentType.ToList();

                    _instance.ProductVatType = db.ProductVatType.Where(w => w.Enable == true).ToList();
                    _instance.CostType = db.CostType.ToList();
                    //_instance.ProductCategory = db.ProductCategory.ToList();
                    //_instance.ProductGroups = db.ProductGroups.ToList();
                    _instance.Supplier = db.Supplier.ToList();
                    //_instance.Vendor = db.Vendor.Include("POCostType").ToList();
                    //_instance.ProductBrands = db.ProductBrands.ToList();
                    _instance.ProductSize = db.ProductSize.ToList();
                    _instance.ProductColor = db.ProductColor.ToList();
                    _instance.Zone = db.Zone.ToList();
                    _instance.DeliveryTypes = db.DeliveryType.Where(w => w.Enable == true).ToList();
                    //_instance.ProductUnit = db.ProductUnit.ToList();
                    _instance.Users = db.Users.ToList();
                    //_instance.Shelfs = db.Shelf.ToList();
                }
            }
            return _instance;
        }
        public static SingletonPriority1 SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
