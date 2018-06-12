using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class LoadingForm : Form
    {
        private ManageProductForm manageProductForm;
        private Form1 form1;

        public LoadingForm()
        {
            InitializeComponent();
        }
        private void LoadingForm_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(InnitialSingleton);
            t.Start();
            Thread t1 = new Thread(OpenLoadding);
            t1.Start();
        }

        void OpenLoadding()
        {
            decimal? a = null;
            Library.checkLoad = true;
            while (Library.checkLoad)
            {
                Console.WriteLine("do the InnitialSingleton " + checkObj);
                System.Threading.Thread.Sleep(1000);//10 seconds               
            }
        }
        int checkObj = 0;
        void InnitialSingleton()
        {
            Library.checkLoad = true;
            //Singleton.SingletonAgeOfShare.Instance();
            //Singleton.SingletonAuthen.Instance();
            //Singleton.SingletonTransactionWms.Instance();
            //Singleton.SingletonBranch.Instance();
            //checkObj = 1;
            //Singleton.SingletonDebtor.Instance();


            checkObj = 2;
            Singleton.SingletonMember.Instance();


            //Singleton.SingletonPayment.Instance();
            //Singleton.SingletonPOsTransaction.Instance();
            //Singleton.SingletonPriceSchedule.Instance();
            //Singleton.SingletonPriority1.Instance();
            //checkObj = 3;
            //Singleton.SingletonProduct.Instance();


            //Singleton.SingletonPromotionActive.Instance();
            //Singleton.SingletonPUnit.Instance();
            //Singleton.SingletonShared.Instance();
            //Singleton.SingletonThisBudgetYear.Instance();
            //checkObj = 4;
            //Singleton.SingletonVender.Instance();


            //Singleton.SingletonWarehouse.Instance();
            //Singleton.SingletonWasteReason.Instance();
            Library.checkLoad = false;
            Console.WriteLine("end the InnitialSingleton");

        }
    }
}
