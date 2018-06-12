using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    /// <summary>
    /// เก็บ Transaction ทั้งหมดที่เกิด
    /// </summary>
    public class SingletonPOsTransaction
    {
        private static SingletonPOsTransaction _instance;
        public List<TransactionType> TransactionTypes { get; set; }
        public static SingletonPOsTransaction Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonPOsTransaction();
                    var data = db.TransactionType.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.TransactionTypes = data;
                }
            }
            return _instance;
        }
        public static SingletonPOsTransaction SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
