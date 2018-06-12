using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonDebtor
    {
        private static SingletonDebtor _instance;
        public List<Debtor> Debtors { get; set; }
        public static SingletonDebtor Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonDebtor();
                    var data = db.Debtor.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.Debtors = data;
                }
            }
            return _instance;
        }
        public static SingletonDebtor SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
