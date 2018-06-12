using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonThisBudgetYear
    {
        private static SingletonThisBudgetYear _instance;
        public BudgetYear ThisYear { get; set; }
        public static SingletonThisBudgetYear SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
        public static SingletonThisBudgetYear Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonThisBudgetYear();
                    BudgetYear data = db.BudgetYear.FirstOrDefault(w => w.Enable == true && w.IsCurrent == true);
                    _instance.ThisYear = data;
                }
            }
            return _instance;
        }
    }
}
