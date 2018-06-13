using Pos_Management_System.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    /// <summary>
    /// หน้าจอหลัก
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            Thread t = new Thread(new ThreadStart(LoadStart));
            t.Start();
            InitializeComponent();

            while (Library.checkLoad)
            {
                System.Threading.Thread.Sleep(1200);//10 seconds               
            }
            t.Abort();
        }
        void LoadStart()
        {
            Application.Run(new LoadingForm());
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            toolStripStatusLabel2.Text = "ผู้ใช้งาน : " + SingletonAuthen.Instance().Name + " |";
            //เวลาเข้าใช้งาน: 23.00 น. |
            toolStripStatusLabel1.Text = "เข้าใช้เมื่อ: " + Library.ConvertDateToThaiDate(DateTime.Now, true) + " น. |";
            using (SSLsEntities db = new SSLsEntities())
            {
                //toolStripStatusLabel5.Text = db.Database.Connection.ConnectionString;
                System.Data.SqlClient.SqlConnectionStringBuilder connBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder();

                connBuilder.ConnectionString = db.Database.Connection.ConnectionString;

                string server = connBuilder.DataSource;
                toolStripStatusLabel5.Text = "| SERVER: " + server;//-> this gives you the Server name.
                string database = connBuilder.InitialCatalog;
            }
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            LibraryUI.AddNewMenuAndDisable(menuStrip1);
            try
            {
                toolStripStatusVersion.Text = "Version Name : " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString() + " |";
            }
            catch
            {
            }


            //CheckPromorionstop();

            //int[] values = { 85, 30, 45, 96, 20, 0, 74, 60, 45, 101 };

        }
        /// <summary>
        /// ตรวจสอบ Promotion หยุดทำงาน
        /// </summary>
        private void CheckPromorionstop()
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var dateQuery = db.Database.SqlQuery<DateTime>("SELECT getdate()");
                DateTime serverDate = dateQuery.AsEnumerable().First();
                var dateCheck = db.PriceScheduleCheck.FirstOrDefault(w => DbFunctions.TruncateTime(w.DateCheck) == DbFunctions.TruncateTime(serverDate));
                if (dateCheck.IsCheck == false)
                {
                    // ถ้าวันนี้ ยังไม่เชค ก็เชค
                    dateCheck.IsCheck = true;
                    dateCheck.CheckDate = serverDate;
                    dateCheck.CheckBy = SingletonAuthen.Instance().Id;
                    db.Entry(dateCheck).State = EntityState.Modified;
                    var priceSchedule = db.PriceSchedule.Where(w => w.Enable == true && w.IsStop == false);
                    foreach (var item in priceSchedule)
                    {
                        if (serverDate > item.EndDate)
                        {
                            // ถ้าวันนี้ มากกว่า วันสิ้นสุด
                            // โปรโมชั่น ควร Stop ลง
                            item.IsStop = true;
                            item.StopReason = "Stop By Code";
                            item.UpdateDate = DateTime.Now;
                            item.UpdateBy = SingletonAuthen.Instance().Id;
                            db.Entry(item).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();
                }
                else
                {
                    // ถ้าวันนี้ เชคแล้ว ปล่อยผ่าน                    
                }
                //int days = 1000;
                //for (int i = 0; i < days; i++)
                //{
                //    PriceScheduleCheck f = new PriceScheduleCheck();
                //    f.DateCheck = DateTime.Now.AddDays(i + 1);
                //    f.IsCheck = false;
                //    db.PriceScheduleCheck.Add(f);
                //}
                //db.SaveChanges();
            }

        }

        private void ขายสนคาหนารานToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// หน้าต่างสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void สนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageProductForm mpf = new ManageProductForm();
            mpf.MdiParent = this;
            mpf.Show();
        }
        /// <summary>
        /// หน้าต่างใบสั่งซื้อ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void สรางใบสงซอPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreatePOForm cpf = new CreatePOForm();
            cpf.MdiParent = this;
            cpf.Show();
        }
        /// <summary>
        /// ซื้อครบจำนวน *คละ ได้รับสิทธิ์แลกซื้อ ในราคา พิเศษ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ไดรบสทธแลกซอในราคาพเศษToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CampaignFullyQtyAndSaleForm obj = new CampaignFullyQtyAndSaleForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// ซื้อช่วงนาทีทอง สบายๆ ง่ายๆ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private CampaignDiscountDayForm _campaignDiscountDayForm;
        private void ซอชวงนาททองToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CampaignDiscountDayForm cd = new CampaignDiscountDayForm();
            cd.MdiParent = this;
            cd.Show();
            //if (_campaignDiscountDayForm == null)
            //{
            //    CampaignDiscountDayForm cd = new CampaignDiscountDayForm();
            //    cd.MdiParent = this;
            //    _campaignDiscountDayForm = cd;
            //    cd.Show();
            //}
            //else
            //{
            //    _campaignDiscountDayForm.Activate();
            //}

        }
        /// <summary>
        /// ซื้อครบจำนวน *คละ ได้รับสิทธิ์เงินส่วนลด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ไดรบสทธเงนสวนลดToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CampiagnFullyQtyAndDiscount cd = new CampiagnFullyQtyAndDiscount();
            cd.MdiParent = this;
            cd.Show();
        }
        /// <summary>
        /// ซื้อครบจำนวน (คละได้) ได้รับข้อความพิเศษ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ไดรบขอความพเศษToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// ซื้อครบยอดเงิน (คละได้) ได้รับสิทธิ์แลกซื้อในราคาพิเศษ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ไดรบสทธแลกซอในราคาพเศษToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CampiagnFullyAmountAndSaleForm cd = new CampiagnFullyAmountAndSaleForm();
            cd.MdiParent = this;
            cd.Show();
        }
        /// <summary>
        /// ซื้อครบยอดเงิน (คละได้) ได้รับสิทธิ์เงินส่วนลด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ไดรบสทธเงนสวนลดToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CampaignFullyAmountAndDisForm cd = new CampaignFullyAmountAndDisForm();
            cd.MdiParent = this;
            cd.Show();
        }
        /// <summary>
        /// เปิดหน้าต่าง สมาชิก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void จดการขอมลสมาชกToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageMemberForm cd = new ManageMemberForm();
            cd.MdiParent = this;
            cd.Show();
        }
        /// <summary>
        /// รับคืนสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ทำคนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CNPosForm cd = new CNPosForm();
            cd.MdiParent = this;
            cd.Show();
        }
        /// <summary>
        /// สินค้าของเสียไปคลัง
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void โอนสนคาของเสยหนารานToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WasteManageForm wm = new WasteManageForm(this);
            wm.MdiParent = this;
            wm.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ตดชำระหนToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// TransferOutForm (โอนสินค้าหน้าร้าน ไปยังสาขาอื่น)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void โอนสนคาหนารานไปสาขาอนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransferOutForm obj = new TransferOutForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// แคมเปญ ซื้อครบ แถมฟรี
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ซอครบจำนวนไดแถมฟรToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CampaignFullyQtyAndGiftForm obj = new CampaignFullyQtyAndGiftForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// ใบกำกับภาษี ฉะบับเต็ม
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ใบกำกบภาษแบบเตมรปToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintFullInvoiceForm obj = new PrintFullInvoiceForm();
            obj.MdiParent = this;
            obj.Show();

        }
        /// <summary>
        /// พิมพ์ใบเสร็จรับเงิน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ใบเสรจรบเงนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintInvoiceForm obj = new PrintInvoiceForm();
            obj.MdiParent = this;
            obj.Show();

        }
        /// <summary>
        /// ไม่ใช้
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void เตมสนคาหนารานToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// เติมสินค้าหน้าร้าน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void เตมสนคาหนารานToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StoreFrontAddForm obj = new StoreFrontAddForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// สั่งสินค้ารถใหญ่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ออเดอรสงสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void จดการสนคาToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        void LoadingThread()
        {
            //code goes here  
            LoadingForm obj = new LoadingForm();
            //obj.MdiParent = this;
            obj.ShowDialog();
        }

        private void สมาชกรานToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void สรางใบสงซอToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreatePOForm cpf = new CreatePOForm();
            cpf.MdiParent = this;
            cpf.Show();
        }
        /// <summary>
        /// แก้ไขโปรโมชั่น
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void แกใขโปรโมชนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchEditPromotionForm obj = new SearchEditPromotionForm();
            obj.MdiParent = this;
            obj.Show();
        }


        /// <summary>
        /// หน่วยสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void จดการหนวยสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearMdiChildren();

            ProductUnitForm obj = new ProductUnitForm();
            obj.MdiParent = this;
            maxminForm(obj,"min");
            obj.Show();
        }

        private void หมวดหสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearMdiChildren();

            ProductCategoryForm obj = new ProductCategoryForm();
            obj.MdiParent = this;
            maxminForm(obj, "max");
            obj.Show();
        }

        private void ประเภทสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearMdiChildren();

            ProductGroupForm obj = new ProductGroupForm();
            obj.MdiParent = this;
            maxminForm(obj, "max");
            obj.Show();
        }

        private void เจาของผลตภณฑSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearMdiChildren();

            SupplierForm obj = new SupplierForm();
            obj.MdiParent = this;
            maxminForm(obj, "max");
            obj.Show();
        }

        private void ผจำหนายสนคาSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearMdiChildren();

            VendorForm obj = new VendorForm();
            obj.MdiParent = this;
            obj.WindowState = FormWindowState.Maximized;
            obj.Show();
        }

        private void ยหอสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrandForm obj = new BrandForm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void ขนาดสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductSizeForm obj = new ProductSizeForm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void สสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductColorForm obj = new ProductColorForm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void โซนเกบสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductZoneForm obj = new ProductZoneForm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void ชนวางสนคาShelfหนารานToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductSelfForm obj = new ProductSelfForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// รายการประจำวัน รถใหญ่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void รายการออเดอรประจำวนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaleOrderWarehouseListForm obj = new SaleOrderWarehouseListForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// สร้างรถใหญ่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void สรางออเดอรToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaleOrderWarehouseForm obj = new SaleOrderWarehouseForm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            DialogResult dr = MessageBox.Show("คุณต้องการออกจากระบบ ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    //Application.Exit();
                    Environment.Exit(0);
                    break;
                case DialogResult.No:
                    e.Cancel = true;
                    return;
                    break;
            }
        }
        private void ออกจากระบบToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("คุณต้องการออกจากระบบ ใช่หรือไม่ ?", "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    //Application.Exit();
                    Environment.Exit(0);
                    break;
                case DialogResult.No:
                    return;
                    break;
            }
        }
        void OpenLoading()
        {
            LoadingForm obj = new LoadingForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// ฟื้นฟูข้อมูลระบบ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ฟนฟระบบToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการฟื้นฟูระบบ ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    Singleton.SingletonAgeOfShare.SetInstance();
                    Singleton.SingletonBranch.SetInstance();
                    Singleton.SingletonDebtor.SetInstance();
                    Singleton.SingletonMember.SetInstance();
                    Singleton.SingletonPayment.SetInstance();
                    Singleton.SingletonPOsTransaction.SetInstance();
                    Singleton.SingletonPriceSchedule.SetInstance();
                    Singleton.SingletonPriority1.SetInstance();
                    Singleton.SingletonProduct.SetInstance();
                    Singleton.SingletonPromotionActive.SetInstance();
                    Singleton.SingletonPUnit.SetInstance();
                    Singleton.SingletonShared.SetInstance();
                    Singleton.SingletonThisBudgetYear.SetInstance();
                    Singleton.SingletonVender.SetInstance();
                    Singleton.SingletonWarehouse.SetInstance();
                    Singleton.SingletonWasteReason.SetInstance();
                    Singleton.SingletonShareMonth.SetInstance();
                    Singleton.SingletonBudgetYearNew.SetInstance();

                    Thread t = new Thread(new ThreadStart(LoadStart));
                    t.Start();
                    Library.checkLoad = true;
                    while (Library.checkLoad)
                    {
                        System.Threading.Thread.Sleep(1200); // 10 seconds
                    }
                    t.Abort();

                    break;
                case DialogResult.No:
                    break;
            }

        }

        private void ทำรบเขาดวยPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RCVPOForm obj = new RCVPOForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// จัดการสถานะ PO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void จดการสถานะPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POStatusManageForm obj = new POStatusManageForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// อนุมัติ PO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void อนมตPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POApproveManage obj = new POApproveManage();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// ตัดชำระลูกหนี้
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ตดชำระลกหนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebtorPayDateForm wm = new DebtorPayDateForm(this);
            wm.MdiParent = this;
            wm.Show();
        }
        /// <summary>
        /// CRUD ลูกหนี้
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void จดการลกหนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageDebtorForm obj = new ManageDebtorForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// แก้ไข PO * ที่ยังไม่อนุมัติเท่านั้น เท่านั้นจริงๆ แน๊แน่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void แกไขPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PONonApproveEditForm obj = new PONonApproveEditForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// เปลี่ยน รหัสผ่าน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void เปลยนรหสผานToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetPasswordForm obj = new ResetPasswordForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// บทบาทการทำงานของ Role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void บทบาทการทำงานToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManageForm obj = new RoleManageForm();
            obj.MdiParent = this;
            obj.Show();

        }
        /// <summary>
        /// ข้อมูลส่วนตัว
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ขอมลสวนตวToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditMyProfileForm obj = new EditMyProfileForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// แบน user จัดการ Role ให้
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void จดการผใชงานToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserRoleManageForm obj = new UserRoleManageForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// ทำคืนของในคลัง
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ทำคนของเสยToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoodsReturnWmsForm obj = new GoodsReturnWmsForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// โอนสินค้าในคลัง ไปสาขา 01 *ไม่มีการออกใบกำกับภาษี ซึ่งจะต่างจาก เบิกออร์เดอร์รถใหญ่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void โอนสนคาหลงรานToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WmsTransferBranchForm obj = new WmsTransferBranchForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// รายการที่เบิก จะรวมอยู่หน้านี้ทั้งหมด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void รายการเบกเตมหนารานToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISS2FrontListForm obj = new ISS2FrontListForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// จัดการใบเบิก โดยคลังสินค้า ยืนยันยอดที่จะหยิบให้
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void จดการใบเบกคลงToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISS2FrontManageForm obj = new ISS2FrontManageForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// รายการยืนยันออเดอร์ สำหรับ คลัง take job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void รายการยนยนออรเดอรคลงToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaleOrderWarehouseTakeForm obj = new SaleOrderWarehouseTakeForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// จัดการใบโอนสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void รายการโอนสนคาประจำวนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransferOutListForm obj = new TransferOutListForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// รายการคืนคลัง ประจำเป็น
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void รายการใบโอนของเสยToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WastManageListForm obj = new WastManageListForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// ยกเลิก รับคืน จากลูกค้า -stock pos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ยกเลกรบคนสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CNPosCancelForm obj = new CNPosCancelForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// ดูรายงานทำคืน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void รายการทำคนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CNPosListForm obj = new CNPosListForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// ดูรายงานทำคืนที่ยกเลิกไป
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void รายงานยกเลกใบทำคนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CNPosListCancelForm obj = new CNPosListCancelForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// รายงานส่งคืนสินค้า ให้ Vendor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void รายงานใบสงคนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoodsReturnWmsListForm obj = new GoodsReturnWmsListForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// รายงานใบโอนสินค้า ไปสาขาอื่น
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void รายงานใบโอนสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WmsTransferBranchListForm obj = new WmsTransferBranchListForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// จัดการสมาชิก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void จดการสมาชกToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageMemberForm cd = new ManageMemberForm();
            cd.MdiParent = this;
            cd.Show();
        }

        private void รายToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberActiveAndResignReport cd = new MemberActiveAndResignReport();
            cd.MdiParent = this;
            cd.Show();
        }
        /// <summary>
        /// จัดการ ตั้งหัวข้อ วันเวลา สถานที่ และสมาชิกต้องเข้าร่วม
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void จดการประชมสมาชกToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// เมื่อตรวจพบสินค้าในคลัง ชำรุด ต้องสร้างเอกสารเพื่อเข้าห้องของเสีย Waste Warehouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void เกบสนคาคลงเขาหองของเสยToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WasteWarehouseManageForm obj = new WasteWarehouseManageForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// ส่งคืนสินค้า ให้ผู้จำน่าย แบบ auto เลือก Vendor ระบบ detect Goods Auto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void จดการหองของเสยToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoodsAutoReturnWmsForm obj = new GoodsAutoReturnWmsForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// รายงานและ จัดการสถานะใบคืนของเสีย
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void รายงานใบคนของเสยToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoodsReturnVendorConfirmForm obj = new GoodsReturnVendorConfirmForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// ตรวจดูสินค้า ในห้องของเสีย
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void หองของเสยToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoodsReturnVendorStatusForm obj = new GoodsReturnVendorStatusForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        ///  สินค้าคงเหลือหน้าร้าน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void สนคาคงเหลอหนารานToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StoreFronStockForm obj = new StoreFronStockForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ตรวจนบสตอกToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckFrontStockForm obj = new CheckFrontStockForm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void รายการตรวจนบแลวToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckFrontStockReport obj = new CheckFrontStockReport();
            obj.MdiParent = this;
            obj.Show();

        }
        /// <summary>
        /// ความเคลื่อนไหว รายตัว
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ความเคลอนไหวรายตวToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StockCardListForm obj = new StockCardListForm();
            obj.MdiParent = this;
            obj.Show();
        }
        /// <summary>
        /// ยกยอดสด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ยกยอดสดToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void รายงานยนยนเบกสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaleOrderWarehouseNoHaveProduct obj = new SaleOrderWarehouseNoHaveProduct();
            obj.MdiParent = this;
            obj.Show();
        }

        private void รายการทำรบเขาPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RCVPODetailForm obj = new RCVPODetailForm();
            obj.MdiParent = this;
            obj.Show();

        }

        private void นบสตอกหนารานToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckStockFront1Form obj = new CheckStockFront1Form();
            obj.MdiParent = this;
            obj.Show();
        }

        private void รายการเอกสารตรวจนบToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckStockFront1ListForm obj = new CheckStockFront1ListForm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void ทำรบเขาPOหลายใบToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PORcvInvoiceForm obj = new PORcvInvoiceForm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void รายงานรบจายคงเหลอToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductRawInOut obj = new ProductRawInOut();
            obj.MdiParent = this;
            obj.Show();
        }

        private void รายงานสนคาคงเหลอToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeftInStock obj = new LeftInStock();
            obj.MdiParent = this;
            obj.Show();
        }

        private void รายงานความเคลอนไหวสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportStockCard obj = new ReportStockCard();
            obj.MdiParent = this;
            obj.Show();
        }

        private void รายงานสนคาคงเหลอราคาทนราคาขายToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeftInStock obj = new LeftInStock("ราคาทุน-ราคาขาย");
            obj.MdiParent = this;
            obj.Show();
        }

        private void รายงานการตรวจนบสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CountProduct obj = new CountProduct();
            obj.MdiParent = this;
            obj.Show();
        }

        private void ทะเบยนคมสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void adjustสนคาหนารานToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StoreFrontAdjustForm obj = new StoreFrontAdjustForm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void ราคาทนราคาขายในหองของเสยToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WasteValueDateForm obj = new WasteValueDateForm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void เบกสนคาหนารานใชเองToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetGoodsForUseForm obj = new GetGoodsForUseForm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void เพมแกไขฐานสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Thread t = new Thread(LoadingThread);
            //t.Start();
            //FormCollection fc = Application.OpenForms;

            //foreach (Form frm in fc)
            //{
            //    //iterate through
            //    if (frm.Name == "ManageProductForm")
            //    {
            //        MessageBox.Show("คุณเปิดหน้าต่างนี้อยู่");
            //        return;
            //    }
            //}

            ManageProductForm mpf = new ManageProductForm();
            mpf.MdiParent = this;
            mpf.Show();
        }

        private void ยายฐานสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MergeProductForm mpf = new MergeProductForm();
            mpf.MdiParent = this;
            mpf.Show();
        }

        private void รายงานเบกใชเองToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetGoodsUsedListForm mpf = new GetGoodsUsedListForm();
            mpf.MdiParent = this;
            mpf.Show();
        }

        private void ยอดหองของเสยประจำวนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WasteWarehouseStockDateForm mpf = new WasteWarehouseStockDateForm();
            mpf.MdiParent = this;
            mpf.Show();
        }

        private void คนหาสนคาตามใบคนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchProdWasteWarehouseForm mpf = new SearchProdWasteWarehouseForm();
            mpf.MdiParent = this;
            mpf.Show();
        }

        private void เงนปนผล2560ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberChange2560Form mpf = new MemberChange2560Form();
            mpf.MdiParent = this;
            mpf.Show();
        }

        private void รายงานทอยสมาชกToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberAddressForm mpf = new MemberAddressForm();
            mpf.MdiParent = this;
            mpf.Show();
        }

        private void สรางสนคาแปรรปToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PGoodsForm mpf = new PGoodsForm();
            mpf.MdiParent = this;
            mpf.Show();
        }

        private void รายงานรบจายคงเหลอหองของเสยToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WasteInOutStockForm mpf = new WasteInOutStockForm();
            mpf.MdiParent = this;
            mpf.Show();

        }

        private void ปรบปรงหองของเสยToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdjustWasteForm form = new AdjustWasteForm();
            form.MdiParent = this;
            form.Show();
        }

        private void รายงานสรปการจายปนผลเฉลยคนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportReceiptMemberForm form = new ReportReceiptMemberForm();
            form.MdiParent = this;
            form.Show();
        }

        private void รายToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReportMemberChangeForm form = new ReportMemberChangeForm();
            form.MdiParent = this;
            form.Show();
        }

        private void จดการปToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBudgetYear form = new FormBudgetYear();
            form.MdiParent = this;
            form.Show();
        }

        private void จดการหนไมครบปToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMonthShare form = new FormMonthShare();
            form.MdiParent = this;
            form.Show();
        }

        private void รายงานToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormReportMember form = new FormReportMember();
            form.MdiParent = this;
            form.Show();
        }

        public void maxminForm(Form obj,string valueMinMax)
        {
            if (valueMinMax == "max")
            {
                obj.WindowState = FormWindowState.Maximized;
            }
            else if(valueMinMax == "min")
            {
                obj.WindowState = FormWindowState.Minimized;
            }
        }
        public void clearMdiChildren()
        {
            foreach (Form c in this.MdiChildren)
            {
                c.Close();
            }
        }
    }
}
