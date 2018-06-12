using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
  public class SingletonBudgetYearNew
    {
        private static SingletonBudgetYearNew _instance;
        public List<BudgetYear> BudgetYear { get; set; }
        public static SingletonBudgetYearNew Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonBudgetYearNew();
                    var data = db.BudgetYear.ToList();
                    _instance.BudgetYear = data;
                }
            }
            return _instance;
        }
        public static SingletonBudgetYearNew SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
