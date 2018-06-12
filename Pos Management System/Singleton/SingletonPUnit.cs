using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonPUnit
    {
        private static SingletonPUnit _instance;
        public List<ProductUnit> Units { get; set; }
        public static SingletonPUnit Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonPUnit();
                    var data = db.ProductUnit.Where(w=>w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.Units = data;
                }
            }
            return _instance;
        }
        public static SingletonPUnit SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
