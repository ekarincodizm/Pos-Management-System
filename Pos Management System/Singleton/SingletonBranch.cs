using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonBranch
    {
        private static SingletonBranch _instance;
        public List<Branch> Branchs { get; set; }
        public static SingletonBranch Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonBranch();
                    var data = db.Branch.Where(w => w.Enable == true).OrderBy(w=>w.Id).ToList();
                    _instance.Branchs = data;
                }
            }
            return _instance;
        }
        public static SingletonBranch SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
