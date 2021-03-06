﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Singleton
{
    public class SingletonProductCategory
    {
        private static SingletonProductCategory _instance;
        public List<ProductCategory> ProductCategories { get; set; }
        public static SingletonProductCategory Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            //if (_instance == null)
            //{
                using (SSLsEntities db = new SSLsEntities())
                {
                    _instance = new SingletonProductCategory();
                    //var data = db.ProductCategory.OrderBy(w => w.Id).ToList();
                    var data = db.ProductCategory.Where(w => w.Enable == true).OrderBy(w => w.Id).ToList();
                    _instance.ProductCategories = data;
            }
            //}
            return _instance;
        }
        public static SingletonProductCategory SetInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            _instance = null;
            return _instance;
        }
    }
}
