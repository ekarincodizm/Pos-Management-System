using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonWarehouse
    {
        private static SingletonWarehouse _instance;
        public List<Warehouse> Warehouses { get; set; }
        public static SingletonWarehouse Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonWarehouse();
                    var data = db.Warehouse.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.Warehouses = data;
                }
            }
            return _instance;
        }
        public static SingletonWarehouse SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
