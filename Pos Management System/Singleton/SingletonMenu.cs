using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
   public class SingletonMenu
    {
        private static SingletonMenu _instance;
        public List<Menu> Menu { get; set; }
        public static SingletonMenu Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {              
                    _instance = new SingletonMenu();
                    var data = db.Menu.Where(w => w.Enable == true).ToList();
                    _instance.Menu = data;
                }
            }
            return _instance;
        }
        public static SingletonMenu SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
