using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonProductGroup
    {
        private static SingletonProductGroup _instance;
        public List<ProductGroups> ProductGroups { get; set; }
        public static SingletonProductGroup Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            //if (_instance == null)
            //{
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonProductGroup();
                    //var data = db.ProductGroups.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    var data = db.ProductGroups.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.ProductGroups = data;
                }
            //}
            return _instance;
        }
        public static SingletonProductGroup SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
