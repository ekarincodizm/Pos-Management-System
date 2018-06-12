using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonShlf
    {
        private static SingletonShlf _instance;
        public List<Shelf> Shelfs { get; set; }
        public static SingletonShlf Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonShlf();
                    var data = db.Shelf.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.Shelfs = data;
                }
            }
            return _instance;
        }
        public static SingletonShlf SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
