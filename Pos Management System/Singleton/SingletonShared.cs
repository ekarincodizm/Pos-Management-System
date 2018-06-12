using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonShared
    {
        private static SingletonShared _instance;
        public Share Share { get; set; }
        public static SingletonShared Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonShared();
                    var share = db.Share.FirstOrDefault(w => w.Enable == true);
                    _instance.Share = share;
                }
            }
            return _instance;
        }
        public static SingletonShared SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
