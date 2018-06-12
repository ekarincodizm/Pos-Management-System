using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonProduct
    {
        private static SingletonProduct _instance;

        public List<ProductDetails> ProductDetails { get; set; }
        public List<Products> Products { get; set; }
        public static SingletonProduct Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                _instance = new SingletonProduct();
                using (SSLsEntities db = new SSLsEntities())
                {
                    List<Products> prod = db.Products
                   .Include("CostProductChangeLog")
                   .Include("Vendor")
                   .Include("ProductVatType")
                   //.Include("ProductGroups")
                   .Include("ProductDetails")
                   .Where(w => w.Enable == true)
                   .ToList();
                    _instance.Products = prod;

                    List<ProductDetails> data = db.ProductDetails
                        .Include("ProductUnit")
                        //.Include("Products.CostType")               
                        .Include("Products.Supplier")
                        .Include("Products.ProductVatType")
                        .Include("SellPriceChangeLog")
                        .Include("Products.CostProductChangeLog")
                        .Include("Products.ProductBrands")
                        .Where(w => w.Enable == true)
                        .ToList();
                    _instance.ProductDetails = data;
                    
                }
            }
            return _instance;
        }
        public static SingletonProduct SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
