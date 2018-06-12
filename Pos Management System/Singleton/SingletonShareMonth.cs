using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonShareMonth
    {
        private static SingletonShareMonth _instance;
        public List<ShareMonth> ShareMonth { get; set; }
        public static SingletonShareMonth Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonShareMonth();
                    var data = db.ShareMonth.ToList();
                    _instance.ShareMonth = data;
                }
            }
            return _instance;
        }
        public static SingletonShareMonth SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
