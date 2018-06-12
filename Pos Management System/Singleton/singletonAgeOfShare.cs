using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonAgeOfShare
    {
        private static SingletonAgeOfShare _instance;
        public List<AgeOfShare> AgeOfShare { get; set; }
        public static SingletonAgeOfShare Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    int thisBudget = SingletonThisBudgetYear.Instance().ThisYear.Id;
                    _instance = new SingletonAgeOfShare();
                    var data = db.AgeOfShare.Where(w => w.Enable == true)
                        .OrderBy(w => w.TermStart).ToList();
                    _instance.AgeOfShare = data;
                }
            }
            return _instance;
        }
        public static SingletonAgeOfShare SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
