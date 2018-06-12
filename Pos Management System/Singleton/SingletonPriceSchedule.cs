using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    /// <summary>
    /// enable = 1 isStop = 0
    /// </summary>
    public class SingletonPriceSchedule
    {
        private static SingletonPriceSchedule _instance;
        public List<PriceSchedule> PriceSchedules { get; set; }
        public List<PriceSchedule> DiscountDayActice { get; set; }
        public static SingletonPriceSchedule SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
        public static SingletonPriceSchedule Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonPriceSchedule();
                    var data = db.PriceSchedule
                        .Include("SellingPrice.SellingPriceDetails.ProductDetails")
                        .Include("SellingPrice.ProductDetails")
                        .Where(w => w.Enable == true && w.IsStop == false).ToList();
                    _instance.PriceSchedules = data;

                    var ps = db.PriceSchedule.Where(w => w.Enable == true && w.IsStop == false &&
                    DbFunctions.TruncateTime(w.StartDate) <= DbFunctions.TruncateTime(DateTime.Now) &&
                    DbFunctions.TruncateTime(w.EndDate) >= DbFunctions.TruncateTime(DateTime.Now) &&
                    w.FKCampaign == MyConstant.CampaignType.DiscountDay
                ).ToList();
                    _instance.DiscountDayActice = ps;
                }
            }
            return _instance;
        }
    }
}
