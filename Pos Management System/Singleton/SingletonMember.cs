using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    /// <summary>
    /// Enable = true
    /// </summary>
    public class SingletonMember
    {
        private static SingletonMember _instance;
        public List<Member> Members { get; set; }
        public static SingletonMember Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonMember();
                    var data = db.Member.Include("Sex").Include("MemberShare.Share").Include("MemberShare.AgeOfShare")
                        .Where(w => w.Enable == true)
                         .ToList();
                    _instance.Members = data;
                }
            }
            return _instance;
        }
        public static SingletonMember SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
