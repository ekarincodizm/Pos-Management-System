using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonWasteReason
    {
        private static SingletonWasteReason _instance;
        public List<WasteReason> WasteReasons { get; set; }
        public static SingletonWasteReason Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonWasteReason();
                    var data = db.WasteReason.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.WasteReasons = data;
                }
            }
            return _instance;
        }
        public static SingletonWasteReason SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
