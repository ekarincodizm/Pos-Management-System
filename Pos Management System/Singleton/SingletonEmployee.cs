using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonEmployee
    {
        private static SingletonEmployee _instance;
        public List<Employee> Employee { get; set; }
        public static SingletonEmployee Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonEmployee();
                    var data = db.Employee.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.Employee = data;
                }
            }
            return _instance;
        }
        public static SingletonEmployee SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
