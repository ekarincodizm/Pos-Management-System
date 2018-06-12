using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonPayment
    {
        private static SingletonPayment _instance;
        public List<PaymentType> PaymentTypes { get; set; }
        public static SingletonPayment Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonPayment();
                    var data = db.PaymentType.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.PaymentTypes = data;
                }
            }
            return _instance;
        }
        public static SingletonPayment SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
