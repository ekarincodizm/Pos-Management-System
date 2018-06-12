using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonProductBrand
    {
        private static SingletonProductBrand _instance;
        public List<ProductBrands> ProductBrands { get; set; }
        public static SingletonProductBrand Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonProductBrand();
                    var data = db.ProductBrands.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.ProductBrands = data;
                }
            }
            return _instance;
        }
        public static SingletonProductBrand SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
