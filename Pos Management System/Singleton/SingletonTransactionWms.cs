using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonTransactionWms
    {
        private static SingletonTransactionWms _instance;
        public List<TransactionWms> TransactionWms { get; set; }
        public static SingletonTransactionWms Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonTransactionWms();
                    var data = db.TransactionWms.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.TransactionWms = data;
                }
            }
            return _instance;
        }
        public static SingletonTransactionWms SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
