using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
   public class SingletonTransport
    {
        private static SingletonTransport _instance;
        public List<Transport> Transport { get; set; }
        public static SingletonTransport Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonTransport();
                    var data = db.Transport.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.Transport = data;
                }
            }
            return _instance;
        }
        public static SingletonTransport SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
