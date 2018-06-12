using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonVender
    {
        private static SingletonVender _instance;
        public List<Vendor> Vendors { get; set; }

        public static SingletonVender SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }

        public static SingletonVender Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonVender();
                    var data = db.Vendor.Include("POCostType").Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.Vendors = data;
                }
            }
            return _instance;
        }
    }
}
