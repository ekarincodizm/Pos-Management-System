using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonPromotionActive
    {
        private static SingletonPromotionActive _instance;
        public List<PriceSchedule> PriceScheduleDiscountDay { get; set; }
        public static SingletonPromotionActive Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {                    
                    _instance = new SingletonPromotionActive();
                    var data = db.PriceSchedule.Include("SellingPrice").Where(w => w.Enable == true && w.IsStop == false && 
                    w.FKCampaign == MyConstant.CampaignType.DiscountDay).OrderBy(w => w.StartDate).ToList();
                    _instance.PriceScheduleDiscountDay = data;
                }
            }
            return _instance;
        }
        public static SingletonPromotionActive SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
