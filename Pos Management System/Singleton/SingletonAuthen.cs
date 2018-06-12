using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonAuthen
    {
        private static SingletonAuthen _instance;
        public string Id { get; set; }
        public string Name { get; set; }
        public Users Users { get; set; }
        public Branch MyBranch { get; set; }

        public static SingletonAuthen Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                _instance = new SingletonAuthen();
                _instance.Id = LoginId;
                _instance.Name = FullName;

                using (SSLsEntities db = new SSLsEntities())
                {
                    var user = db.Users.Include("Role.MenuAccess.Menu").SingleOrDefault(w => w.Id == LoginId);
                    _instance.Users = user;
                    _instance.MyBranch = user.Branch;
                }
            }
            return _instance;
        }
        private static string LoginId;
        private static string FullName;
        public static void SetInstance(string loginId, string fullName)
        {
            LoginId = loginId;
            FullName = fullName;
            Instance();
        }
    }
}
