using Pos_Management_System.Model;
using Pos_Management_System.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class ManageProductForm : Form
    {
        public ManageProductForm()
        {
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }


        private void ManageProductForm_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(1);

            SingletonProduct.Instance();
            List<PriceSchedule> priceSchedules = SingletonPriceSchedule.Instance().PriceSchedules;
            SingletonPromotionActive.Instance();

            SingletonPriority1.Instance();
            List<ProductUnit> productUnit = SingletonPUnit.Instance().Units.Where(w => w.Enable).ToList();
            // Data grid *** dataGridViewProductPrice
            ProductUnit pu = new ProductUnit();
            //pu.Id = 0;
            //pu.Name = "-- เพิ่มใหม่ --";
            //productUnit.Insert(0, pu);
            productUnitBindingSource.DataSource = productUnit;
            productUnitBindingSource.EndEdit();

            // หน่วยนับ        
            List<ProductUnit> units = new List<ProductUnit>();
            units = productUnit;
            ProductUnit[] unitCounts = units.ToArray();
            comboBoxUnitCount.DataSource = unitCounts;
            comboBoxUnitCount.DisplayMember = "Name";
            comboBoxUnitCount.ValueMember = "Id";

            // หน่วยเติม
            List<ProductUnit> fills = new List<ProductUnit>();
            fills = productUnit;
            ProductUnit[] unitFills = fills.ToArray();
            comboBoxUnitFill.DataSource = unitFills;
            comboBoxUnitFill.DisplayMember = "Name";
            comboBoxUnitFill.ValueMember = "Id";

            // หน่วยเติม
            List<ProductUnit> buys = new List<ProductUnit>();
            buys = productUnit;
            ProductUnit[] unitBuys = buys.ToArray();
            comboBoxUnitBuy.DataSource = unitBuys;
            comboBoxUnitBuy.DisplayMember = "Name";
            comboBoxUnitBuy.ValueMember = "Id";

            // ประเภทภาษี
            //var productVatType = SingletonPriority1.Instance().ProductVatType.Where(w => w.Enable == true).ToList();
            //comboBoxVatType.DataSource = productVatType;
            //comboBoxVatType.DisplayMember = "Name";
            //comboBoxVatType.ValueMember = "Id";
            // วิธีคิด ต้นทุน
            var costType = SingletonPriority1.Instance().CostType.Where(w => w.Enable == true).ToList();
            comboBoxCostType.DataSource = costType;
            comboBoxCostType.DisplayMember = "Name";
            comboBoxCostType.ValueMember = "Id";
            // ภาษี
            //textBoxVatValue.Text = productVatType.FirstOrDefault().Value.ToString();
            // หมวดหมู่สินค้า
            var productCategory = SingletonProductCategory.Instance().ProductCategories.Where(w => w.Enable == true).ToList();
            comboBoxProductCategory.DataSource = productCategory;
            comboBoxProductCategory.DisplayMember = "Name";
            comboBoxProductCategory.ValueMember = "Id";

            // ประเภทสินค้า
            //var productGroup = SingletonProductGroup.Instance().ProductGroups.Where(w => w.Enable == true).ToList();
            //comboBoxProductGroup.DataSource = productGroup;
            //comboBoxProductGroup.DisplayMember = "Name";
            //comboBoxProductGroup.ValueMember = "Id";

            // เจ้าของผลิตภัณฑ์
            var supplier = SingletonPriority1.Instance().Supplier.Where(w => w.Enable == true).ToList();
            comboBoxSupplier.DataSource = supplier;
            comboBoxSupplier.DisplayMember = "Name";
            comboBoxSupplier.ValueMember = "Id";

            // ผู้จัดจำหน่าย
            //var vendor = SingletonVender.Instance().Vendors.Where(w => w.Enable == true).ToList();
            //comboBoxVendor.DataSource = vendor;
            //comboBoxVendor.DisplayMember = "Name";
            //comboBoxVendor.ValueMember = "Id";

            // ยี่ห้อสินค้า
            //var brand = SingletonProductBrand.Instance().ProductBrands.Where(w => w.Enable == true).ToList();
            //comboBoxBrand.DataSource = brand;
            //comboBoxBrand.DisplayMember = "Name";
            //comboBoxBrand.ValueMember = "Id";

            // ขนาดสินค้า
            var size = SingletonPriority1.Instance().ProductSize.Where(w => w.Enable == true).ToList();
            comboBoxSize.DataSource = size;
            comboBoxSize.DisplayMember = "Name";
            comboBoxSize.ValueMember = "Id";

            // สี
            var color = SingletonPriority1.Instance().ProductColor.Where(w => w.Enable == true).ToList();
            comboBoxColor.DataSource = color;
            comboBoxColor.DisplayMember = "Name";
            comboBoxColor.ValueMember = "Id";

            // โซนเก็บสินค้า
            var zone = SingletonPriority1.Instance().Zone.Where(w => w.Enable == true).ToList();
            comboBoxZone.DataSource = zone;
            comboBoxZone.DisplayMember = "Name";
            comboBoxZone.ValueMember = "Id";

            //Thread t = new Thread(LoadingThread);
            //t.Start();
            //LoadingForm obj = new LoadingForm(this);
        }
        /// <summary>
        /// Bindding Unit Name
        /// </summary>
        /// <param name="data"></param>
        public void BinddingUnit(ProductUnit data)
        {
            int currentRow = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows[currentRow].Cells[colUnitName].Value = data.Name;
            dataGridView1.Rows[currentRow].Cells[colIdUnit].Value = data.Id;
        }

        //void LoadingThread()
        //{
        //    //code goes here  
        //    LoadingForm obj = new LoadingForm(this);
        //    //obj.MdiParent = this;
        //    //obj.CloseMe();
        //}
        /// <summary>
        /// บันทึกสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
                      "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    SaveCommit();
                    //_ProductIdSelected
                    Library._fkGoodsActive.Remove(_ProductIdSelected);
                    /// Initial Store Front หน้าร้าน                    
                    break;
                case DialogResult.No:
                    break;
            }
        }
        int colCode = 0;
        int colPZ = 1;
        int colUnitName = 2; // หน่วย
        int colUnitButton = 3; // ปุ่มค้นหา
        int colCost = 4;
        int colCostVat = 5; // ทุนรวมภาษี
        int colVat = 6;
        int colPrint = 7;
        int colPricePerUnit = 8; // ราคาขาย/หน่วย
        int colOldPrice = 9; // ราคาขายเก่า เอามาจาก log
        int colProfit = 10;
        int colDesc = 11;
        int colIdProductDtl = 12; // id product dtl
        int colIdUnit = 13;
        /// <summary>
        /// save น่ะ
        /// </summary>
        void SaveCommit()
        {
            try
            {
                #region Edit Product
                /// เป็นการเลือก เพื่อดูรายละเอียด และแก้ไข
                if (_ProductIdSelected != 0)
                {
                    string oldCode = "";
                    bool isChangeCost = false;
                    decimal oldCostAndVat = decimal.Parse(textBoxCostOld.Text);
                    decimal newCostAndVat = decimal.Parse(textBoxCostNew.Text);
                    if (newCostAndVat != oldCostAndVat)
                    {
                        isChangeCost = true;
                    }
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        // แปลว่า การ Update Product แน๊ๆ
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            // check ถ้า ตัวแรก packsize เป็น 1 หรือไม่
                            if (i == 0)
                            {
                                if (decimal.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString()) != 1)
                                {
                                    MessageBox.Show("กรุณาสร้าง Packsize 1 ขึ้นก่อนเสมอ");
                                    return;
                                }
                                //else if (decimal.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString()) != 1) // ห้ามเปลี่ยน packsize 1 
                                //{
                                //    MessageBox.Show("ไม่สามารถเปลี่ยน packsize 1 ได้");
                                //    return;
                                //}
                            }
                            //------------------------------------------------------------
                            if (dataGridView1.Rows[i].Cells[colCode].Value == null)
                            {
                                continue;
                            }
                            var check = dataGridView1.Rows[i].Cells[colIdProductDtl].Value ?? "0";
                            if (check != "0")
                            {
                                decimal costOnly = decimal.Parse(dataGridView1.Rows[i].Cells[colCost].Value.ToString());
                                decimal costAndVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostVat].Value.ToString());
                                decimal costVat = decimal.Parse(dataGridView1.Rows[i].Cells[colVat].Value.ToString());
                                decimal profit = decimal.Parse(dataGridView1.Rows[i].Cells[colProfit].Value.ToString().Replace("%", "").Trim());

                                string code = dataGridView1.Rows[i].Cells[colCode].Value.ToString();
                                decimal pz = decimal.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString());
                                int fkUnit = int.Parse(dataGridView1.Rows[i].Cells[colIdUnit].Value.ToString());
                                int id = int.Parse(check.ToString()); // id product details
                                decimal sellPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colPricePerUnit].Value.ToString());
                                // check ราคาขาย ปัจจุบัน ว่าเปลี่ยนหรือไม่
                                ProductDetails getProduct = Singleton.SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == id);
                                // get จาก singleton update ช้า
                                ProductDetails getNewFromDB = db.ProductDetails.SingleOrDefault(w => w.Id == id);
                                // ------------------- check packsize เปลี่ยน
                                if (pz != getProduct.PackSize)
                                {
                                    // check หลายจุด
                                    // 1. หน้าร้าน
                                    // 2. ห้องของเสีย
                                    // 3. ในคลัง
                                    decimal value = Library.GetResult(getProduct.FKProduct, DateTime.Now.AddDays(1));
                                    if (value > 0)
                                    {

                                    }
                                    else // stock อาจเปน 0 หรือ ติด - สามารถเปลี่ยน pz ได้ จะไม่กระทบ transactions ภายได้ชิ้นเล็กที่สุด คือ 1
                                    {

                                    }
                                }
                                oldCode = getNewFromDB.Code;
                                // check barcode เปลี่ยนแปลง
                                //if (getNewFromDB.Code != code)
                                //{
                                //    getNewFromDB.Code = code;
                                //}
                                getNewFromDB.Code = code;
                                getNewFromDB.PackSize = pz;
                                getNewFromDB.FKUnit = fkUnit;
                                // compare ราคา
                                SellPriceChangeLog sellLog = new SellPriceChangeLog();
                                sellLog.CreateDate = DateTime.Now;
                                sellLog.CreateBy = SingletonAuthen.Instance().Id;
                                sellLog.UpdateDate = DateTime.Now;
                                sellLog.UpdateBy = SingletonAuthen.Instance().Id;
                                sellLog.Enable = true;
                                sellLog.FKProductDetails = id;
                                sellLog.CurrentPrice = sellPrice;
                                sellLog.OldPrice = getProduct.SellPrice;
                                sellLog.PackSize = getProduct.PackSize;
                                sellLog.CostAndVat = costAndVat;
                                decimal oldProfit = decimal.Parse(Library.CalMonneyPercent(getProduct.SellPrice, getProduct.CostAndVat).Replace("%", "").Trim());
                                sellLog.Profit = oldProfit;
                                getNewFromDB.UpdateDate = DateTime.Now;
                                getNewFromDB.UpdateBy = SingletonAuthen.Instance().Id;

                                if (sellPrice > getProduct.SellPrice)
                                {
                                    // ถ้าเปลี่ยนราคา > ราคาขายปัจจุบัน * ต้อง save log
                                    sellLog.Description = "ขึ้นราคาขายปกติ";
                                    sellLog.IsHigher = true;
                                    db.SellPriceChangeLog.Add(sellLog);
                                    // Update Current Price
                                    getNewFromDB.SellPrice = sellPrice;
                                    //db.Entry(getNewFromDB).State = EntityState.Modified;
                                }
                                else if (sellPrice < getProduct.SellPrice)
                                {
                                    // ถ้าเปลี่ยนราคา < ราคาขายปัจจุบัน * ต้อง save log
                                    sellLog.Description = "ลดราคาขายปกติ";
                                    sellLog.IsHigher = false;
                                    db.SellPriceChangeLog.Add(sellLog);
                                    // Update Current Price
                                    getNewFromDB.SellPrice = sellPrice;
                                    //db.Entry(getNewFromDB).State = EntityState.Modified;
                                }
                                else
                                {
                                    // ไม่มีการเปลี่ยนแปลง ราคาขายใดๆ
                                }
                                ////////////////// ส่วนของการ check การเปลี่ยนแปลงทุน //////////////////////--------------------
                                if (isChangeCost)
                                {

                                    getNewFromDB.CostOnly = costOnly;
                                    getNewFromDB.CostAndVat = costAndVat;
                                    getNewFromDB.CostVat = costVat;
                                }
                                //decimal costOnly = decimal.Parse(dataGridView1.Rows[i].Cells[colCost].Value.ToString());
                                //decimal costAndVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostVat].Value.ToString());
                                //decimal costVat = decimal.Parse(dataGridView1.Rows[i].Cells[colVat].Value.ToString());
                                //getNewFromDB.CostOnly = costOnly;
                                //getNewFromDB.CostAndVat = costAndVat;
                                //getNewFromDB.CostVat = costVat;
                                db.Entry(getNewFromDB).State = EntityState.Modified;
                                db.SaveChanges();
                                // update cost บน WH
                                using (WH_TRATEntities wh = new WH_TRATEntities())
                                {
                                    var getProductDtl = wh.WH_PRODUCT_MAST_DTL.FirstOrDefault(w => w.PRODUCT_NO == oldCode);
                                    pz = decimal.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString());
                                    sellPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colPricePerUnit].Value.ToString());
                                    getProductDtl.SELL_PRICE = sellPrice;
                                    getProductDtl.PRODUCT_NO = code;
                                    getProductDtl.COST = costOnly;
                                    getProductDtl.PACK_SIZE = (float)pz;
                                    getProductDtl.UPDATE_DATE = DateTime.Now;
                                    getProductDtl.UNIT_NAME = Singleton.SingletonPUnit.Instance().Units.SingleOrDefault(w => w.Id == fkUnit).Name;
                                    wh.Entry(getProductDtl).State = EntityState.Modified;
                                    wh.SaveChanges();
                                }
                            }

                            else
                            {
                                ///////////////////// พบสินค้าใหม่ บาร์ใหม่
                                bool isPrint = true;
                                List<ProductDetails> details = new List<ProductDetails>();
                                // Add In SSL
                                if (dataGridView1.Rows[i].Cells[colPrint].Value == null)
                                {
                                    isPrint = false;
                                }
                                else
                                {
                                    isPrint = (bool)dataGridView1.Rows[i].Cells[colPrint].Value;
                                }
                                details.Add(new ProductDetails()
                                {
                                    FKProduct = _ProductIdSelected,
                                    Code = dataGridView1.Rows[i].Cells[colCode].Value.ToString(),
                                    Name = "-",
                                    PackSize = int.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString()),
                                    CostOnly = decimal.Parse(dataGridView1.Rows[i].Cells[colCost].Value.ToString()),
                                    CostAndVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostVat].Value.ToString()),
                                    CostVat = decimal.Parse(dataGridView1.Rows[i].Cells[colVat].Value.ToString()),
                                    CreateBy = SingletonAuthen.Instance().Id,
                                    CreateDate = DateTime.Now,
                                    Enable = true,
                                    FKUnit = (int)dataGridView1.Rows[i].Cells[colIdUnit].Value,
                                    IsPrintLabel = isPrint,
                                    UpdateDate = DateTime.Now,
                                    UpdateBy = SingletonAuthen.Instance().Id,
                                    SellPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colPricePerUnit].Value.ToString())
                                });
                                db.ProductDetails.AddRange(details);

                                /// Add In Wh
                                using (WH_TRATEntities wh = new WH_TRATEntities())
                                {
                                    var getProductOnSSl = SingletonProduct.Instance().Products.SingleOrDefault(w => w.Id == _ProductIdSelected);

                                    string code = dataGridView1.Rows[i].Cells[colCode].Value.ToString();
                                    float pz = float.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString());
                                    string unitName = dataGridView1.Rows[i].Cells[colUnitName].Value.ToString();
                                    decimal costOnly = decimal.Parse(dataGridView1.Rows[i].Cells[colCost].Value.ToString());
                                    decimal sellprice = decimal.Parse(dataGridView1.Rows[i].Cells[colPricePerUnit].Value.ToString());
                                    WH_PRODUCT_MAST_DTL dtl;
                                    dtl = new WH_PRODUCT_MAST_DTL();
                                    dtl.HD_ROWID = getProductOnSSl.FKRowID;
                                    dtl.PRODUCT_NO = code;
                                    dtl.UNIT_NAME = unitName;
                                    dtl.PACK_SIZE = pz;
                                    dtl.COST = costOnly;
                                    dtl.SELL_TYPE = 0;
                                    dtl.CREATE_DATE = DateTime.Now;
                                    dtl.UPDATE_DATE = null;
                                    dtl.USER_ID = SingletonAuthen.Instance().Id;
                                    dtl.SKU_KEY = "0";
                                    dtl.LAST_ACTION = null;
                                    dtl.SELL_PRICE = sellprice;
                                    dtl.OLD_COST = 0;
                                    dtl.OLD_PRICE = 0;
                                    dtl.ENABLE = true;
                                    wh.WH_PRODUCT_MAST_DTL.Add(dtl);
                                    wh.SaveChanges();
                                }
                            }
                            //if (i >= dataGridView1.Rows.Count - 2) break;
                        }

                        // Check Cost Change Log
                        CostProductChangeLog logCost = new CostProductChangeLog();
                        logCost.CreateDate = DateTime.Now;
                        logCost.CreateBy = SingletonAuthen.Instance().Id;
                        logCost.UpdateDate = DateTime.Now;
                        logCost.UpdateBy = SingletonAuthen.Instance().Id;
                        logCost.Enable = true;
                        logCost.FKProduct = _ProductIdSelected;
                        logCost.CurrentCostOnly = decimal.Parse(textBoxCostNewNoVat.Text);
                        logCost.CurrentCostAndVat = newCostAndVat;

                        var getProductUpdate = db.Products.SingleOrDefault(w => w.Id == _ProductIdSelected);
                        var getProducts = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.FKProduct == _ProductIdSelected).ToList().OrderBy(w => w.PackSize).ToList();
                        // cost ของ packsize 1 
                        logCost.OldCostOnly = getProducts.FirstOrDefault().CostOnly;
                        logCost.OldCostAndVat = getProducts.FirstOrDefault().CostAndVat;
                        var costLogChange = getProductUpdate.CostProductChangeLog.Where(w => w.Enable == true && w.FKProduct == _ProductIdSelected).OrderByDescending(w => w.Id).ToList();
                        if (costLogChange.Count == 0)
                        {
                            // ถ้าไม่มี add ทุนตั้งต้น
                            logCost.IsHigher = true;
                            logCost.Description = "ทุนตั้งต้น";
                            db.CostProductChangeLog.Add(logCost);
                        }
                        else
                        {
                            if (newCostAndVat > costLogChange.FirstOrDefault().CurrentCostAndVat)
                            {
                                // ทุนใหม่ > ทุนเก่า
                                logCost.IsHigher = true;
                                logCost.Description = "ปรับทุนขึ้น";
                                db.CostProductChangeLog.Add(logCost);
                            }
                            else if (newCostAndVat < costLogChange.FirstOrDefault().CurrentCostAndVat)
                            {
                                // ทุนใหม่ < ทุนเก่า
                                logCost.IsHigher = false;
                                logCost.Description = "ปรับทุนลง";
                                db.CostProductChangeLog.Add(logCost);
                            }
                            else
                            {
                                // no add cost
                            }
                        }


                        getProductUpdate.UpdateDate = DateTime.Now;
                        getProductUpdate.UpdateBy = SingletonAuthen.Instance().Id;
                        getProductUpdate.ThaiName = textBoxProductName.Text;
                        getProductUpdate.EnglishName = textBoxOtherName.Text;
                        getProductUpdate.Description = textBoxRemark.Text;

                        ProductUnit comboBoxUnitCountSelect = (ProductUnit)comboBoxUnitCount.SelectedItem;
                        ProductUnit comboBoxUnitFillSelect = (ProductUnit)comboBoxUnitFill.SelectedItem;
                        ProductUnit comboBoxUnitBuySelect = (ProductUnit)comboBoxUnitBuy.SelectedItem;
                        getProductUpdate.FKUnitCheckStock = comboBoxUnitCountSelect.Id;
                        getProductUpdate.FKUnitAddStock = comboBoxUnitFillSelect.Id;
                        getProductUpdate.FKUnitBuy = comboBoxUnitBuySelect.Id;
                        if (getProductUpdate.FKProductVatType != int.Parse(textBoxVatTypeId.Text))
                        {
                            getProductUpdate.FKProductVatType = int.Parse(textBoxVatTypeId.Text);
                        }

                        int collect = MyConstant.PosProductCollect.UnCollect;
                        if (radioButtonPosCollect.Checked)
                        {
                            collect = MyConstant.PosProductCollect.Collect;
                        }
                        // checkBoxProcessingGoods
                        if (checkBoxProcessingGoods.Checked == true)
                        {
                            getProductUpdate.IsProcessingGoods = true;
                        }
                        else
                        {
                            getProductUpdate.IsProcessingGoods = false;
                        }
                        getProductUpdate.FKProductPosCollect = collect;
                        CostType comboBoxCostTypeSelect = (CostType)comboBoxCostType.SelectedItem;
                        getProductUpdate.FKCostType = comboBoxCostTypeSelect.Id;
                        // วิธีคิด ทุน
                        getProductUpdate.FKCostType = comboBoxCostTypeSelect.Id;
                        // คำอธิบาย
                        getProductUpdate.ProductRemerk = textBoxDescription.Text;
                        getProductUpdate.FKVender = _VendorId;

                        ProductCategory comboBoxProductCategorySelect = (ProductCategory)comboBoxProductCategory.SelectedItem;
                        getProductUpdate.FKProductCategory = comboBoxProductCategorySelect.Id;
                        //ProductGroups comboBoxProductGroupSelect = (ProductGroups)comboBoxProductGroup.SelectedItem;
                        getProductUpdate.FKProductGroup = _GroupId;
                        Supplier comboBoxSupplierSelect = (Supplier)comboBoxSupplier.SelectedItem;
                        getProductUpdate.FKSupplier = comboBoxSupplierSelect.Id;
                        //Vendor comboBoxVenderSelect = (Vendor)comboBoxVendor.SelectedItem;
                        //ProductBrands comboBoxProductBrandSelect = (ProductBrands)comboBoxBrand.SelectedItem;
                        getProductUpdate.FKProductBrand = _BrandId;
                        ProductSize comboBoxSizeSelect = (ProductSize)comboBoxSize.SelectedItem;
                        getProductUpdate.FKProductSize = comboBoxSizeSelect.Id;
                        ProductColor comboBoxColorSelect = (ProductColor)comboBoxColor.SelectedItem;
                        getProductUpdate.FKProductColor = comboBoxColorSelect.Id;
                        Zone comboBoxZoneSelect = (Zone)comboBoxZone.SelectedItem;
                        getProductUpdate.FKZone = comboBoxZoneSelect.Id;

                        string cheerMsg1 = textBoxCheer1.Text;
                        string cheerMsg2 = textBoxCheer2.Text;
                        string cheerMsg3 = textBoxCheer3.Text;
                        string labelMsg = textBoxMsgLabel.Text;
                        getProductUpdate.CheerMessage1 = cheerMsg1;
                        getProductUpdate.CheerMessage2 = cheerMsg2;
                        getProductUpdate.CheerMessage3 = cheerMsg3;
                        getProductUpdate.LabelMessage = labelMsg;
                        getProductUpdate.Description1 = textBoxDescription1.Text;

                        getProductUpdate.Description1 = textBoxDescription1.Text;
                        db.Entry(getProductUpdate).State = EntityState.Modified;

                        using (WH_TRATEntities wh1 = new WH_TRATEntities())
                        {
                            var proWH = wh1.WH_PRODUCT_MAST_HD.SingleOrDefault(w => w.ROWID == getProductUpdate.FKRowID);
                            proWH.PRODUCT_NAME = getProductUpdate.ThaiName;
                            if (getProductUpdate.FKProductVatType == MyConstant.ProductVatType.HasVat)
                            {
                                proWH.VAT_TYPE = true;
                            }
                            else
                            {
                                proWH.VAT_TYPE = false;
                            }
                            proWH.BONUS_TYPE = false;
                            if (getProductUpdate.FKProductPosCollect == MyConstant.PosProductCollect.Collect)
                            {
                                proWH.BONUS_TYPE = true;
                            }
                            proWH.UPDATE_DATE = DateTime.Now;
                            proWH.GROUP_NO = int.Parse(Singleton.SingletonProductGroup.Instance().ProductGroups.SingleOrDefault(w => w.Id == getProductUpdate.FKProductGroup).Code);
                            proWH.BRAND_NO = int.Parse(Singleton.SingletonProductBrand.Instance().ProductBrands.SingleOrDefault(w => w.Id == getProductUpdate.FKProductBrand).Code);
                            wh1.Entry(proWH).State = EntityState.Modified;
                            wh1.SaveChanges();
                        }

                        db.SaveChanges();
                        /// add to posstockdetails 
                        LibraryInitialAllStock.DetectNewBarcode(_ProductIdSelected);
                        // set instance
                        //SingletonProduct.SetInstance();
                        var getProductRemove = SingletonProduct.Instance().Products.SingleOrDefault(w => w.Id == _ProductIdSelected);
                        SingletonProduct.Instance().Products.Remove(getProductRemove);
                        var prod = db.Products
                  //.Include("CostProductChangeLog")
                  .Include("Vendor")
                  .Include("ProductVatType")
                  //.Include("ProductGroups")
                  .Include("ProductDetails")
                  .SingleOrDefault(w => w.Enable == true && w.Id == _ProductIdSelected);
                        SingletonProduct.Instance().Products.Add(prod);

                        foreach (var item in prod.ProductDetails.Where(w => w.Enable == true).ToList())
                        {
                            var dtl = SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == item.Id);
                            SingletonProduct.Instance().ProductDetails.Remove(dtl);
                        }
                        var prodDtl = db.ProductDetails
                    .Include("Products.CostType")
                    .Include("ProductUnit")
                    //.Include("Products.Supplier")
                    .Include("Products.ProductVatType")
                    .Include("SellPriceChangeLog")
                    .Include("Products.CostProductChangeLog")
                    //.Include("Products.ProductBrands")
                    .Where(w => w.Enable == true && w.FKProduct == _ProductIdSelected).ToList();
                        foreach (var item in prodDtl)
                        {
                            SingletonProduct.Instance().ProductDetails.Add(item);
                        }
                        this.Dispose();
                    }
                }
                #endregion

                #region Add New Product
                else
                {
                    // Products
                    Products prod = new Products();
                    /// tab 1
                    string productNo = textBoxProductNo.Text.Trim();
                    string productName = textBoxProductName.Text.Trim();
                    string otherName = textBoxOtherName.Text.Trim();
                    string desc = textBoxRemark.Text.Trim();

                    ProductUnit comboBoxUnitCountSelect = (ProductUnit)comboBoxUnitCount.SelectedItem;
                    ProductUnit comboBoxUnitFillSelect = (ProductUnit)comboBoxUnitFill.SelectedItem;
                    ProductUnit comboBoxUnitBuySelect = (ProductUnit)comboBoxUnitBuy.SelectedItem;

                    int collect = MyConstant.PosProductCollect.UnCollect;
                    if (radioButtonPosCollect.Checked)
                    {
                        collect = MyConstant.PosProductCollect.Collect;
                    }
                    //ProductVatType comboBoxVatTypeSelect = (ProductVatType)comboBoxVatType.SelectedItem;

                    //prod.IsSale = checkBoxSale.Checked;
                    //prod.IsBuy = checkBoxBuy.Checked;
                    //prod.Enable = !checkBoxDisable.Checked;

                    CostType comboBoxCostTypeSelect = (CostType)comboBoxCostType.SelectedItem;
                    string productRemark = textBoxRemark.Text;

                    //string saleCheck = checkBoxSale.Checked.ToString();
                    //string buyCheck = checkBoxBuy.Checked.ToString();
                    //string disable = checkBoxDisable.Checked.ToString();

                    //decimal vatChange = decimal.Parse(textBoxVatValue.Text);
                    /// tab3
                    //  หมวดหมู่
                    ProductCategory comboBoxProductCategorySelect = (ProductCategory)comboBoxProductCategory.SelectedItem;
                    //ProductGroups comboBoxProductGroupSelect = (ProductGroups)comboBoxProductGroup.SelectedItem;
                    Supplier comboBoxSupplierSelect = (Supplier)comboBoxSupplier.SelectedItem;
                    //Vendor comboBoxVenderSelect = (Vendor)comboBoxVendor.SelectedItem;
                    //ProductBrands comboBoxProductBrandSelect = (ProductBrands)comboBoxBrand.SelectedItem;

                    ProductSize comboBoxSizeSelect = (ProductSize)comboBoxSize.SelectedItem;
                    ProductColor comboBoxColorSelect = (ProductColor)comboBoxColor.SelectedItem;
                    Zone comboBoxZoneSelect = (Zone)comboBoxZone.SelectedItem;

                    string cheerMsg1 = textBoxCheer1.Text;
                    string cheerMsg2 = textBoxCheer2.Text;
                    string cheerMsg3 = textBoxCheer3.Text;
                    string labelMsg = textBoxMsgLabel.Text;

                    prod.Code = productNo;
                    prod.ThaiName = productName;
                    prod.EnglishName = otherName;
                    prod.Description = productRemark; // หมายเหตุ
                    prod.Description1 = textBoxDescription1.Text; // หมายเหตุ

                    prod.FKUnitCheckStock = comboBoxUnitCountSelect.Id;
                    prod.FKUnitAddStock = comboBoxUnitFillSelect.Id;
                    prod.FKUnitBuy = comboBoxUnitBuySelect.Id;
                    // checkBoxProcessingGoods
                    if (checkBoxProcessingGoods.Checked == true)
                    {
                        prod.IsProcessingGoods = true;
                    }
                    else
                    {
                        prod.IsProcessingGoods = false;
                    }

                    prod.FKProductPosCollect = collect;

                    prod.FKProductVatType = int.Parse(textBoxVatTypeId.Text);

                    prod.IsSale = checkBoxSale.Checked;
                    prod.IsBuy = checkBoxBuy.Checked;
                    prod.Enable = !checkBoxDisable.Checked;

                    // วิธีคิด ทุน
                    prod.FKCostType = comboBoxCostTypeSelect.Id;
                    // คำอธิบาย
                    prod.ProductRemerk = textBoxDescription.Text;

                    prod.FKProductCategory = comboBoxProductCategorySelect.Id;
                    prod.FKProductGroup = _GroupId;
                    prod.FKSupplier = comboBoxSupplierSelect.Id;
                    prod.FKVender = _VendorId;
                    prod.FKProductBrand = _BrandId;

                    prod.FKProductSize = comboBoxSizeSelect.Id;
                    prod.FKProductColor = comboBoxColorSelect.Id;
                    prod.FKZone = comboBoxZoneSelect.Id;

                    prod.CheerMessage1 = cheerMsg1;
                    prod.CheerMessage2 = cheerMsg2;
                    prod.CheerMessage3 = cheerMsg3;
                    prod.LabelMessage = textBoxMsgLabel.Text;



                    prod.CreateDate = DateTime.Now;
                    prod.CreateBy = SingletonAuthen.Instance().Id;
                    prod.UpdateDate = DateTime.Now;
                    prod.UpdateBy = SingletonAuthen.Instance().Id;

                    /// Details  
                    bool isPrint = true;
                    List<ProductDetails> details = new List<ProductDetails>();
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        // check ถ้า ตัวแรก packsize เป็น 1 หรือไม่
                        if (i == 0)
                        {
                            if (decimal.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString()) != 1)
                            {
                                MessageBox.Show("กรุณาสร้าง Packsize 1 ขึ้นก่อนเสมอ");
                                return;
                            }
                        }
                        if (dataGridView1.Rows[i].Cells[colCode].Value == null)
                        {
                            continue;
                        }
                        if (dataGridView1.Rows[i].Cells[colPrint].Value == null)
                        {
                            isPrint = false;
                        }
                        else
                        {
                            isPrint = (bool)dataGridView1.Rows[i].Cells[colPrint].Value;
                        }
                        details.Add(new ProductDetails()
                        {

                            Code = dataGridView1.Rows[i].Cells[colCode].Value.ToString(),
                            Name = productName,
                            PackSize = int.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString()),
                            CostOnly = decimal.Parse(dataGridView1.Rows[i].Cells[colCost].Value.ToString()),
                            CostAndVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostVat].Value.ToString()),
                            CostVat = decimal.Parse(dataGridView1.Rows[i].Cells[colVat].Value.ToString()),
                            CreateBy = SingletonAuthen.Instance().Id,
                            CreateDate = DateTime.Now,
                            Enable = true,
                            FKUnit = (int)dataGridView1.Rows[i].Cells[colIdUnit].Value,
                            IsPrintLabel = isPrint,
                            UpdateDate = DateTime.Now,
                            UpdateBy = SingletonAuthen.Instance().Id,
                            SellPrice = decimal.Parse(dataGridView1.Rows[i].Cells[colPricePerUnit].Value.ToString())
                        });

                        //if (i >= dataGridView1.Rows.Count - 2) break;
                    }
                    prod.ProductDetails = details;
                    /// add to WH_TRAD
                    // check barcode
                    using (WH_TRATEntities whEn = new WH_TRATEntities())
                    {
                        foreach (var item in details)
                        {
                            var checkOnWHTrad = whEn.WH_PRODUCT_MAST_DTL.FirstOrDefault(w => w.PRODUCT_NO == item.Code && w.ENABLE == true);
                            if (checkOnWHTrad != null)
                            {
                                MessageBox.Show("พบบาร์โค้ดซ้ำ " + item.Code);
                                return;
                            }
                        }
                        // add to wh 
                        WH_PRODUCT_MAST_HD wh = new WH_PRODUCT_MAST_HD();
                        wh.PRODUCT_NAME = prod.ThaiName;

                        wh.GROUP_NO = int.Parse(Singleton.SingletonProductGroup.Instance().ProductGroups.SingleOrDefault(w => w.Id == prod.FKProductGroup).Code);
                        wh.BRAND_NO = int.Parse(Singleton.SingletonProductBrand.Instance().ProductBrands.SingleOrDefault(w => w.Id == prod.FKProductBrand).Code);
                        wh.COST_PIECE = 0;
                        wh.DOOR_NO = "00";
                        wh.CREATE_DATE = DateTime.Now;
                        wh.UPDATE_DATE = null;
                        wh.USER_ID = prod.CreateBy;
                        wh.SKU_KEY = "0";
                        wh.ENABLE = true;
                        wh.VAT_TYPE = false;
                        // vat type true มีภาษี
                        if (prod.FKProductVatType == MyConstant.ProductVatType.HasVat)
                        {
                            wh.VAT_TYPE = true;
                        }
                        wh.BONUS_TYPE = false;
                        if (prod.FKProductPosCollect == MyConstant.PosProductCollect.Collect)
                        {
                            wh.BONUS_TYPE = true;
                        }
                        wh.CAL_QTY = null;
                        wh.CAL_COST = null;
                        whEn.WH_PRODUCT_MAST_HD.Add(wh);
                        whEn.SaveChanges();
                        // get last 
                        var last = whEn.WH_PRODUCT_MAST_HD.Where(w => w.USER_ID == prod.CreateBy).OrderByDescending(w => w.ROWID).FirstOrDefault();
                        // insert dtl
                        prod.FKRowID = last.ROWID;
                        WH_PRODUCT_MAST_DTL dtl;
                        foreach (var item in details)
                        {
                            dtl = new WH_PRODUCT_MAST_DTL();
                            dtl.HD_ROWID = last.ROWID;
                            dtl.PRODUCT_NO = item.Code;
                            dtl.UNIT_NAME = Singleton.SingletonPUnit.Instance().Units.SingleOrDefault(w => w.Id == item.FKUnit).Name;
                            dtl.PACK_SIZE = (float)item.PackSize;
                            dtl.COST = item.CostOnly;
                            dtl.SELL_TYPE = 0;
                            dtl.CREATE_DATE = DateTime.Now;
                            dtl.UPDATE_DATE = null;
                            dtl.USER_ID = prod.CreateBy;
                            dtl.SKU_KEY = "0";
                            dtl.LAST_ACTION = null;
                            dtl.SELL_PRICE = item.SellPrice;
                            dtl.OLD_COST = 0;
                            dtl.OLD_PRICE = 0;
                            dtl.ENABLE = true;
                            whEn.WH_PRODUCT_MAST_DTL.Add(dtl);
                        }
                        whEn.SaveChanges();
                    }
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        // Add new Initial Stock

                        CostProductChangeLog logCost = new CostProductChangeLog();
                        logCost.CreateDate = DateTime.Now;
                        logCost.CreateBy = SingletonAuthen.Instance().Id;
                        logCost.UpdateDate = DateTime.Now;
                        logCost.UpdateBy = SingletonAuthen.Instance().Id;
                        logCost.Enable = true;

                        logCost.CurrentCostOnly = decimal.Parse(textBoxCostNewNoVat.Text);
                        decimal newCostAndVat = decimal.Parse(textBoxCostNew.Text);
                        logCost.CurrentCostAndVat = newCostAndVat;

                        // cost ของ packsize 1 
                        logCost.OldCostOnly = decimal.Parse(textBoxCostNewNoVat.Text.Trim());
                        logCost.OldCostAndVat = decimal.Parse(textBoxCostNew.Text);

                        // ถ้าไม่มี add ทุนตั้งต้น
                        logCost.IsHigher = true;
                        logCost.Description = "ทุนตั้งต้น";
                        //db.CostProductChangeLog.Add(logCost);
                        prod.CostProductChangeLog.Add(logCost);

                        db.Products.Add(prod);
                        db.SaveChanges();
                        var data = db.Products.OrderByDescending(w => w.Id).FirstOrDefault(w => w.Enable == true && w.CreateBy == prod.CreateBy);
                        // set instance
                        LibraryInitialAllStock.DetectNewBarcode(data.Id);
                        //SingletonProduct.SetInstance();
                        var prodAdd = db.Products
          //.Include("CostProductChangeLog")
          .Include("Vendor")
          .Include("ProductVatType")
          //.Include("ProductGroups")
          .Include("ProductDetails")
          .SingleOrDefault(w => w.Enable == true && w.Id == data.Id);
                        SingletonProduct.Instance().Products.Add(prodAdd);
                        var prodDtl = db.ProductDetails
                    .Include("Products.CostType")
                    .Include("ProductUnit")
                    //.Include("Products.Supplier")
                    .Include("Products.ProductVatType")
                    .Include("SellPriceChangeLog")
                    .Include("Products.CostProductChangeLog")
                    //.Include("Products.ProductBrands")
                    .Where(w => w.Enable == true && w.FKProduct == data.Id).ToList();
                        foreach (var item in prodDtl)
                        {
                            SingletonProduct.Instance().ProductDetails.Add(item);
                        }
                        this.Dispose();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine("พบข้อผิดพลาด");
            }

            //#region สินค้าใน Data Grid
            //int i = 1;
            //List<ProductInGrid> prodInGrids = new List<ProductInGrid>();
            //string pl = "True";
            //foreach (DataGridViewRow item in dataGridViewProductPrice.Rows)
            //{
            //    if (item.Cells[5].Value != null)
            //    {
            //        pl = item.Cells[5].Value.ToString();
            //    }
            //    else
            //    {
            //        pl = "False";
            //    }
            //    prodInGrids.Add(new ProductInGrid()
            //    {
            //        Number = i,
            //        ProductNo = item.Cells[0].Value.ToString(),
            //        PackSize = decimal.Parse(item.Cells[1].Value.ToString()),
            //        Unit = int.Parse(item.Cells[2].Value.ToString()),
            //        Cost = decimal.Parse(item.Cells[3].Value.ToString()),
            //        CostAndVat = decimal.Parse(item.Cells[4].Value.ToString()),
            //        PrintLabel = bool.Parse(pl)
            //    });

            //    i++;
            //    if (i == dataGridViewProductPrice.Rows.Count) break;
            //}
            //#endregion
            //// 
            //#region ตารางราคาทั้งหมด
            //i = 1;
            //List<PriceProductInGrid> priceProductInGrids = new List<PriceProductInGrid>();
            //foreach (DataGridViewRow item in dataGridViewPriceTable.Rows)
            //{
            //    priceProductInGrids.Add(new PriceProductInGrid()
            //    {
            //        Number = i,
            //        PriceNo = item.Cells[0].Value.ToString(),
            //        PriceName = item.Cells[1].Value.ToString(),
            //        PriceUnit = item.Cells[2].Value.ToString(),
            //        PriceLabel = item.Cells[3].Value.ToString(),
            //        PriceTotal = item.Cells[4].Value.ToString(),
            //        PrieProfit = item.Cells[5].Value.ToString()
            //    });

            //    i++;
            //    if (i == dataGridViewPriceTable.Rows.Count) break;
            //}
            //#endregion

        }
        /// <summary>
        /// ช่องภาษี
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxVatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ProductVatType productVatTypeSelect = (ProductVatType)comboBoxVatType.SelectedItem;
            //// เปลี่ยน Percent
            //textBoxVatValue.Text = productVatTypeSelect.Value.ToString();
        }

        private void productUnitBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //int colPZ = 1;
            //int colCost = 3;
            //int colCostVat = 4;
            //int colVat = 5;
            //int colPricePerUnit = 8;
            //int colTotal = 9;
            //int colPercent = 10;

            //decimal cost = 0;
            //decimal costAndVat = 0;
            //decimal vat = 0;
            //decimal pz = 0;

            //decimal costNew = decimal.Parse(textBoxCostNew.Text); // ทุนรวมภาษี
            //decimal costNewNoVat = decimal.Parse(textBoxCostNewNoVat.Text); // ทุนเปล่า
            //decimal costvat = costNew - costNewNoVat;
            if (e.RowIndex < 0)
                return;
            try
            {
                switch (e.ColumnIndex)
                {
                    case 0:
                        var productDtl = Singleton.SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
                        string code = dataGridView1.Rows[e.RowIndex].Cells[colCode].Value.ToString();
                        // ถ้าเป็นการ add ใหม่ โดย Id จะเป็น 0
                        var id = dataGridView1.Rows[e.RowIndex].Cells[colIdProductDtl].Value ?? "0";
                        if (id == "0")
                        {
                            if (productDtl.FirstOrDefault(w => w.Code == code) != null) // ซ้ำครับ
                            {
                                MessageBox.Show("รหัสบาร์โค้ดซ้ำ");
                            }
                        }

                        break;
                    case 1:
                        ManageGridProduct();
                        var units = SingletonPUnit.Instance().Units.Where(w => w.Enable == true).ToList();
                        DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[e.RowIndex].Cells[colUnitName]);
                        cell.DataSource = units;
                        cell.DisplayMember = "Name";
                        cell.ValueMember = "Id";
                        dataGridView1.Rows[e.RowIndex].Cells[colUnitName].Value = units[0].Id;

                        //pz = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colPZ].Value.ToString()); // PZ
                        //cost = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCost].Value.ToString());
                        //costAndVat = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostVat].Value.ToString());
                        //vat = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colVat].Value.ToString());
                        ////set data
                        //dataGridView1.Rows[e.RowIndex].Cells[colCost].Value = Library.ConvertDecimalToStringForm(costNewNoVat * pz);
                        //dataGridView1.Rows[e.RowIndex].Cells[colCostVat].Value = Library.ConvertDecimalToStringForm(costNew * pz);
                        //dataGridView1.Rows[e.RowIndex].Cells[colVat].Value = Library.ConvertDecimalToStringForm(costvat * pz);
                        break;
                    case 2:
                        //Console.WriteLine(dataGridView1.Rows[e.RowIndex].Cells[colUnit].Value.ToString());
                        break;
                    //case 5:
                    //    string costAndVat = dataGridView1.Rows[e.RowIndex].Cells[colCostVat].Value.ToString();// ทุนเปล่า + ภาษี
                    //    var vat = Library.CalVatFromValue(decimal.Parse(costAndVat));
                    //    dataGridView1.Rows[e.RowIndex].Cells[colCost].Value = decimal.Parse(costAndVat) - vat;// ทุนเปล่า
                    //    dataGridView1.Rows[e.RowIndex].Cells[colVat].Value = vat;// ภาษี
                    //    break;
                    case 7:
                        decimal costAndVat = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostVat].Value.ToString());
                        decimal price = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value.ToString());
                        dataGridView1.Rows[e.RowIndex].Cells[colProfit].Value = Library.CalMonneyPercent(price, costAndVat);
                        dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value = Library.ConvertDecimalToStringForm(decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value.ToString()));
                        break;
                    case 8:
                        //decimal pz = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colPZ].Value.ToString()); // PZ
                        //decimal pricePerUnit = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value.ToString()); // ราคา / หน่วย
                        //decimal totalPrice = pricePerUnit * pz;
                        //dataGridView1.Rows[e.RowIndex].Cells[colTotal].Value = totalPrice;// ภาษี
                        ManageGridProduct();
                        break;
                        //case 9:
                        //    string price = dataGridView1.Rows[e.RowIndex].Cells[colTotal].Value.ToString(); // ราคาทั้งหมด
                        //    costAndVat = dataGridView1.Rows[e.RowIndex].Cells[colCostVat].Value.ToString();// ทุนเปล่า + ภาษี
                        //    var percent = Library.CalMonneyPercent(decimal.Parse(price), decimal.Parse(costAndVat));
                        //    pz = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colPZ].Value.ToString()); // PZ
                        //    dataGridView1.Rows[e.RowIndex].Cells[colPercent].Value = percent; // Precent
                        //    break;
                }
            }


            catch (Exception)
            {
                //MessageBox.Show("พบสิ่งไม่ถูกต้อง");
            }
        }

        /// <summary>
        /// เมื่อมีการเพิ่มแถว
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //Console.WriteLine("Add Rows");
            ManageGridProduct();
        }
        /// <summary>
        /// เปลี่ยนแปลงทุนใน กริดสินค้า
        /// </summary>
        void ManageGridProduct()
        {
            try
            {
                decimal costNew = decimal.Parse(textBoxCostNew.Text);// ทุนรวมภาษี
                decimal costNewNoVat = decimal.Parse(textBoxCostNewNoVat.Text); // ทุนเปล่า
                Console.WriteLine(costNew + " " + costNewNoVat);

                decimal costvat = costNew - costNewNoVat; // ภาษี
                //ProductVatType comboBoxVatTypeSelect = (ProductVatType)comboBoxVatType.SelectedItem;
                int vatId = int.Parse(textBoxVatTypeId.Text.Trim());
                if (vatId == MyConstant.ProductVatType.UnVat) // ถ้าเป็น ยกเว้นภาษี
                {
                    costvat = 0;
                }
                decimal pz = 1;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        pz = decimal.Parse(dataGridView1.Rows[i].Cells[colPZ].Value.ToString());
                    }
                    catch (Exception)
                    {
                        colPZ = 1;
                    }
                    dataGridView1.Rows[i].Cells[colCost].Value = Library.ConvertDecimalToStringForm(costNewNoVat * pz);// ทุนเปล่า   
                    decimal costNewVatByPZ = Library.CalCostVat(costNewNoVat * pz);// หา ทุนรวมภาษี
                    if (vatId == MyConstant.ProductVatType.UnVat)
                        costNewVatByPZ = 0;
                    dataGridView1.Rows[i].Cells[colCostVat].Value = Library.ConvertDecimalToStringForm(costNewVatByPZ + costNewNoVat * pz); // ทุนรวมภาษี
                    dataGridView1.Rows[i].Cells[colVat].Value = Library.ConvertDecimalToStringForm(costNewVatByPZ); // ภาษี+             

                    try
                    {
                        decimal costAndVat = decimal.Parse(dataGridView1.Rows[i].Cells[colCostVat].Value.ToString());
                        decimal price = decimal.Parse(dataGridView1.Rows[i].Cells[colPricePerUnit].Value.ToString());
                        dataGridView1.Rows[i].Cells[colProfit].Value = Library.CalMonneyPercent(price, costAndVat); // กำไรขั้นต้น
                    }
                    catch (Exception)
                    {
                        dataGridView1.Rows[i].Cells[colProfit].Value = Library.CalMonneyPercent(0, 0); // กำไรขั้นต้น   
                    }

                    //if (i >= dataGridView1.Rows.Count - 2) break;
                }
            }
            catch (Exception)
            {

            }


        }
        /// <summary>
        /// ขยับเม้าออกจาก ช่องเปลี่ยนคอสใหม่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxCostNew_Validated(object sender, EventArgs e)
        {
            CostAction();
            ManageGridProduct();
        }
        /// <summary>
        /// ช่อง ทุนเปล่า เมื่อ Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxCostNewNoVat_Validated(object sender, EventArgs e)
        {
            CostNewNoVatAction();
            ManageGridProduct();
        }
        /// <summary>
        /// การเปลี่ยนแปลงช่องราคา ทุนรวมภาษี
        /// </summary>
        void CostAction()
        {
            try
            {
                decimal newCost = decimal.Parse(textBoxCostNew.Text.Trim()); // ทุนรวมภาษี
                //ProductVatType comboBoxVatTypeSelect = (ProductVatType)comboBoxVatType.SelectedItem;
                int vatId = int.Parse(textBoxVatTypeId.Text.Trim());
                if (vatId == MyConstant.ProductVatType.UnVat) // ถ้าเป็น ยกเว้นภาษี
                {
                    textBoxCostNewNoVat.Text = Library.ConvertDecimalToStringForm(newCost);
                }
                else
                {
                    decimal vatFromCostVat = Library.CalVatFromValue(newCost); // ภาษี
                    textBoxCostNewNoVat.Text = Library.ConvertDecimalToStringForm(newCost - vatFromCostVat);
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// การเปลี่ยนแปลงช่อง ราคาทุนเปล่า
        /// </summary>
        void CostNewNoVatAction()
        {
            try
            {
                decimal costNoVat = decimal.Parse(textBoxCostNewNoVat.Text.Trim());
                //ProductVatType comboBoxVatTypeSelect = (ProductVatType)comboBoxVatType.SelectedItem;
                int vatId = int.Parse(textBoxVatTypeId.Text.Trim());
                if (vatId == MyConstant.ProductVatType.UnVat) // ถ้าเป็น ยกเว้นภาษี
                {
                    textBoxCostNew.Text = Library.ConvertDecimalToStringForm(costNoVat);
                }
                else
                {
                    decimal vat = Library.CalCostVat(costNoVat);
                    textBoxCostNew.Text = Library.ConvertDecimalToStringForm(costNoVat + vat);
                }

            }
            catch (Exception)
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
             "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    SaveCommit();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่ ?",
             "คำเตือนจากระบบ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    SaveCommit();
                    break;
                case DialogResult.No:
                    break;
            }
        }
        /// <summary>
        /// เปิด Dialog Product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSearchProduct_Click(object sender, EventArgs e)
        {
            //DialogWindowsProducts obj = new DialogWindowsProducts(this);
            //obj.ShowDialog();
            if (_ProductIdSelected != 0)
            {
                Library._fkGoodsActive.Remove(_ProductIdSelected);
            }
            List<ProductDetails> list = SingletonProduct.Instance().ProductDetails.Where(w => w.Enable == true).ToList();
            SelectedProductPopup obj = new SelectedProductPopup(this, list);
            obj.ShowDialog();
        }

        /// <summary>
        /// หลังจากเลือก product แล้ว มา binding data grid
        /// </summary>
        /// <param name="id"></param>
        int _ProductDtlIdSelected = 0;
        int _ProductIdSelected = 0; // FKProductId
        List<CostProductChangeLog> _CostProductChangeLog = new List<CostProductChangeLog>();        
        public void BinddingProduct(int id)
        {
            List<ProductDetails> prodDtl = SingletonProduct.Instance().ProductDetails;
            var product = prodDtl.FirstOrDefault(w => w.Enable == true && w.Id == id);
            // check goods is active

            if (Library._fkGoodsActive.Contains(product.FKProduct))
            {
                MessageBox.Show("คุณเลือก SKU นี้ไปแล้ว");
                return;
            }
            else
            {
                Library._fkGoodsActive.Add(product.FKProduct);
            }
            buttonSave.Enabled = false;
            _ProductDtlIdSelected = id;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
           
            // จะเก็บหน่วยที่เล็กที่สุด บนลงล่าง
            List<ProductDetails> getProducts = new List<ProductDetails>();           
            _ProductIdSelected = product.FKProduct;
            Products getProduct = Singleton.SingletonProduct.Instance().Products.FirstOrDefault(w => w.Id == _ProductIdSelected);
            _CostProductChangeLog.AddRange(getProduct.CostProductChangeLog.OrderByDescending(w => w.CreateDate).ToList());// Show History dialog

            if (SingletonProduct.Instance().Products.SingleOrDefault(w => w.Id == _ProductIdSelected).CostProductChangeLog.OrderByDescending(w => w.CreateDate).FirstOrDefault() != null)
            {
                textBoxCostOld.Text = Library.ConvertDecimalToStringForm(SingletonProduct.Instance().Products.SingleOrDefault(w => w.Id == _ProductIdSelected).CostProductChangeLog.OrderByDescending(w => w.CreateDate).FirstOrDefault().OldCostAndVat);
            }
            else
            {
                textBoxCostOld.Text = "0";
            }
            getProducts = prodDtl.Where(w => w.FKProduct == product.FKProduct).ToList().OrderBy(w => w.PackSize).ToList();

            textBoxProductNo.Text = product.Products.Code;
            textBoxProductName.Text = product.Products.ThaiName;
            textBoxOtherName.Text = product.Products.EnglishName;
            textBoxRemark.Text = product.Products.Description;

            comboBoxUnitCount.SelectedValue = getProduct.FKUnitCheckStock;
            comboBoxUnitFill.SelectedValue = getProduct.FKUnitAddStock;
            comboBoxUnitBuy.SelectedValue = getProduct.FKUnitBuy;

            if (getProduct.FKProductPosCollect == MyConstant.PosProductCollect.Collect)
            {
                // สะสมแต้ม
                radioButtonPosCollect.Checked = true;
            }
            else
            {
                radioButtonPosUnCollect.Checked = true;
            }
            // set ProductVatType
            var prodVatType = Singleton.SingletonPriority1.Instance().ProductVatType.SingleOrDefault(w => w.Id == getProduct.FKProductVatType);
            //comboBoxVatType.SelectedValue = getProduct.FKProductVatType;
            textBoxVatTypeId.Text = prodVatType.Id + "";
            textBoxVatTypeName.Text = prodVatType.Name;
            textBoxVatValue.Text = Library.ConvertDecimalToStringForm(prodVatType.Value);

            if (getProduct.IsSale)
            {
                checkBoxSale.Checked = true;
            }
            else
            {
                checkBoxSale.Checked = false;
            }

            if (getProduct.IsBuy)
            {
                checkBoxBuy.Checked = true;
            }
            else
            {
                checkBoxBuy.Checked = false;
            }

            if (!getProduct.Enable)
            {
                checkBoxDisable.Checked = true;
            }
            else
            {
                checkBoxDisable.Checked = false;
            }

            comboBoxCostType.SelectedValue = getProduct.FKCostType;
            textBoxDescription.Text = getProduct.ProductRemerk;
            textBoxDescription1.Text = getProduct.Description1;

            if (getProduct.IsProcessingGoods)
            {
                checkBoxProcessingGoods.Checked = true;
            }
            else
            {
                checkBoxProcessingGoods.Checked = false;
            }
            // cost ของ packsize 1 
            textBoxCostNewNoVat.Text = Library.ConvertDecimalToStringForm(getProducts.FirstOrDefault().CostOnly);
            textBoxCostNew.Text = Library.ConvertDecimalToStringForm(getProducts.FirstOrDefault().CostAndVat);

            //productUnitBindingSource.DataSource = productUnit;
            //productUnitBindingSource.EndEdit();
            textBoxCreateBy.Text = Library.GetFullNameUserById(getProduct.CreateBy);
            for (int i = 0; i < getProducts.Count; i++)
            {
                dataGridView1.Rows.Add();
                List<ProductUnit> units = SingletonPUnit.Instance().Units.Where(w => w.Enable == true).ToList();
                //var unit = units.SingleOrDefault(w => w.Id == getProducts[i].FKUnit);
                //units.Remove(unit);
                //units.Insert(0, unit);
                dataGridView1.Rows[i].Cells[colCode].Value = getProducts[i].Code;
                dataGridView1.Rows[i].Cells[colPZ].Value = getProducts[i].PackSize;
                dataGridView1.Rows[i].Cells[colPricePerUnit].Value = getProducts[i].SellPrice;
                if (getProducts[i].SellPriceChangeLog.Where(w => w.Enable == true).OrderByDescending(w => w.CreateDate).Count() > 0)
                {
                    dataGridView1.Rows[i].Cells[colOldPrice].Value = Library.ConvertDecimalToStringForm(getProducts[i].SellPriceChangeLog.Where(w => w.Enable == true).OrderByDescending(w => w.CreateDate).FirstOrDefault().OldPrice);
                }
                else
                {
                    dataGridView1.Rows[i].Cells[colOldPrice].Value = 0;
                }

                dataGridView1.Rows[i].Cells[colCost].Value = getProducts[i].CostOnly;
                dataGridView1.Rows[i].Cells[colCostVat].Value = getProducts[i].CostAndVat;
                dataGridView1.Rows[i].Cells[colVat].Value = getProducts[i].CostVat;
                dataGridView1.Rows[i].Cells[colPrint].Value = getProducts[i].IsPrintLabel;
                dataGridView1.Rows[i].Cells[colIdProductDtl].Value = getProducts[i].Id;
                dataGridView1.Rows[i].Cells[colIdUnit].Value = getProducts[i].FKUnit;
                dataGridView1.Rows[i].Cells[colUnitName].Value = getProducts[i].ProductUnit.Name + "";
                /// กำไรข้างต้น
                dataGridView1.Rows[i].Cells[colProfit].Value = Library.CalMonneyPercent(getProducts[i].SellPrice, getProducts[i].CostAndVat);

                //DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[i].Cells[colUnitName]);
                //cell.DataSource = units;
                //cell.DisplayMember = "Name";
                //cell.ValueMember = "Id";
                dataGridView1.Rows[i].Cells[colDesc].Value = getProducts[i].Description;
            }
            comboBoxProductCategory.SelectedValue = getProduct.FKProductCategory;
            //comboBoxProductGroup.SelectedValue = getProduct.FKProductGroup;
            Singleton.SingletonProductGroup.Instance().ProductGroups.SingleOrDefault(w => w.Id == getProduct.FKProductGroup);
            BinddingProGroupChoose(Singleton.SingletonProductGroup.Instance().ProductGroups.SingleOrDefault(w => w.Id == getProduct.FKProductGroup));
            comboBoxSupplier.SelectedValue = getProduct.FKSupplier;
            //comboBoxVendor.SelectedValue = getProduct.FKVender;
            BinddingVendor(getProduct.FKVender);
            //comboBoxBrand.SelectedValue = getProduct.FKProductBrand;
            BinddingProBrandChoose(Singleton.SingletonProductBrand.Instance().ProductBrands.SingleOrDefault(w => w.Id == getProduct.FKProductBrand));
            comboBoxSize.SelectedValue = getProduct.FKProductSize;
            comboBoxColor.SelectedValue = getProduct.FKProductColor;
            comboBoxZone.SelectedValue = getProduct.FKZone;

            // message cheer and label
            textBoxCheer1.Text = getProduct.CheerMessage1;
            textBoxCheer2.Text = getProduct.CheerMessage2;
            textBoxCheer3.Text = getProduct.CheerMessage2;
            textBoxMsgLabel.Text = getProduct.LabelMessage;

            //comboBoxVatType.DataSource = prodVatType;
            //comboBoxVatType.DisplayMember = "Name";
            //comboBoxVatType.ValueMember = "Id";
            //comboBoxVatType.SelectedValue = product.Products.ProductVatType.Id;
            //comboBoxCostType.SelectedValue = product.Products.CostType.Id;
        }

        /// <summary>
        /// คลิกสินค้า แต่ละตัว
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int col2Code = 0;
        int col2Name = 1;
        int col2DateLength = 2;
        int col2Remark = 3;
        int col2LabelType = 4;
        int col2TotalPrice = 5;
        int col2Profit = 6;
        int col2CreateBy = 7;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            try
            {
                /// ถ้าคลิก ค้นหาหน่วย
                if (e.ColumnIndex == colUnitName)
                {
                    SelectedUnitPopup obj = new SelectedUnitPopup(this);
                    obj.ShowDialog();
                    return;
                }

                int idDtl = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[colIdProductDtl].Value.ToString());
                decimal costAntVat = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostVat].Value.ToString());

                // Check Campaign นาทีทอง
                //var priceSchedule = SingletonPriceSchedule.Instance().PriceSchedules;
                // ช่วงเวลาที่ แคมเปญใช้อยู่ นาทีทองเท่านั้น
                List<PriceSchedule> ps = new List<PriceSchedule>();
                ps = Singleton.SingletonPromotionActive.Instance().PriceScheduleDiscountDay;
                //ps = SingletonPriceSchedule.Instance().DiscountDayActice;
                var product = SingletonProduct.Instance().ProductDetails;
                //var getProduct = product.FirstOrDefault(w => w.Enable == true && w.Id == idDtl);
                int i = 1;
                foreach (var item in ps)
                {
                    // ถ้า โปรนี้ถูกเล่นรายการ จะมีเพียง 1 record ใน SellingPrice
                    var sellingPrice = item.SellingPrice.FirstOrDefault(w => w.Enable == true && w.FKProduct == idDtl);
                    if (sellingPrice != null) // is detect product in promotion
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[i].Cells[col2Code].Value = item.Code;
                        dataGridView2.Rows[i].Cells[col2Name].Value = item.Name;
                        dataGridView2.Rows[i].Cells[col2DateLength].Value = Library.ConvertDateToThaiDate(item.StartDate) + " - " + Library.ConvertDateToThaiDate(item.EndDate);
                        dataGridView2.Rows[i].Cells[col2Remark].Value = item.Description;
                        dataGridView2.Rows[i].Cells[col2LabelType].Value = "ป้ายขายหน้าร้าน";
                        dataGridView2.Rows[i].Cells[col2TotalPrice].Value = sellingPrice.SpecialPrice;
                        dataGridView2.Rows[i].Cells[col2Profit].Value = Library.CalMonneyPercent(sellingPrice.SpecialPrice, costAntVat);
                        dataGridView2.Rows[i].Cells[col2CreateBy].Value = Library.GetFullNameUserById(sellingPrice.CreateBy);
                        i++;
                    }
                    // หาก ตรวจสอบพบแคมเปญ
                    //var detectCampiagn = sellingPrice.FirstOrDefault(w => w.FKProduct == getProduct.Id);
                    //if (detectCampiagn != null)
                    //{
                    //    dataGridView2.Rows.Add();
                    //    dataGridView2.Rows[1].Cells[col2Code].Value = item.Code;
                    //    dataGridView2.Rows[1].Cells[col2Name].Value = item.Name;
                    //    dataGridView2.Rows[1].Cells[col2Remark].Value = item.Description;
                    //    dataGridView2.Rows[1].Cells[col2LabelType].Value = "ป้ายขายหน้าร้าน";
                    //    dataGridView2.Rows[1].Cells[col2TotalPrice].Value = detectCampiagn.SpecialPrice;
                    //    dataGridView2.Rows[1].Cells[col2Profit].Value = "undefine";
                    //}
                }


            }
            catch (Exception)
            {

            }

        }
        /// <summary>
        /// เลขที่แถว datagrid 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// เลขที่แถว datagrid 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        /// <summary>
        /// เมื่อมีการเปลี่ยน row selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            try
            {
                int checkid = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[colIdProductDtl].Value.ToString());
                if (checkid == 0)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
            var code = dataGridView1.Rows[e.RowIndex].Cells[colCode].Value.ToString();
            var profit = dataGridView1.Rows[e.RowIndex].Cells[colProfit].Value.ToString();
            var price = dataGridView1.Rows[e.RowIndex].Cells[colPricePerUnit].Value.ToString();
            int idDtl = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[colIdProductDtl].Value.ToString());
            decimal costAntVat = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[colCostVat].Value.ToString());
            /// Binding ราคาปกติ ตั้งต้น
            groupBox14.Text = "ราคาอื่นๆ ของสินค้า " + code;
            //dataGridView2.Rows.Add();
            //dataGridView2.Rows[0].Cells[col2Code].Value = "0";
            //dataGridView2.Rows[0].Cells[col2Name].Value = "ราคาขายตั้งต้น";
            //dataGridView2.Rows[0].Cells[col2DateLength].Value = "ปัจจุบัน";
            //dataGridView2.Rows[0].Cells[col2Remark].Value = "ราคาขายตั้งต้น";
            //dataGridView2.Rows[0].Cells[col2LabelType].Value = "ป้ายขายหน้าร้าน";
            //dataGridView2.Rows[0].Cells[col2TotalPrice].Value = price;
            //dataGridView2.Rows[0].Cells[col2Profit].Value = profit;
            //dataGridView2.Rows[0].Cells[col2CreateBy].Value = "-";

            /// ใส่ประวัติ ราคาขาย
            var sellLog = SingletonProduct.Instance().ProductDetails.SingleOrDefault(w => w.Id == idDtl);
            foreach (var item in sellLog.SellPriceChangeLog.Where(w => w.Enable == true).OrderByDescending(w => w.CreateDate).ToList())
            {
                dataGridView2.Rows.Add("0", "ราคาขายตั้งต้น", "ก่อน " + Library.ConvertDateToThaiDate(item.CreateDate),
                    "ราคาขายตั้งต้น", "ป้ายขายหน้าร้าน", Library.ConvertDecimalToStringForm(item.OldPrice),
                    item.Profit + " %", Library.GetFullNameUserById(item.CreateBy));
            }
        }

        private void textBoxCostNewNoVat_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    buttonCostHistory.Select();
                    break;
                default:
                    break;
            }
        }

        private void textBoxCostNew_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    buttonCostHistory.Select();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ปุ่มดูประวัติทุน จากปัจจุบัน ไปหา - อดีต
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCostHistory_Click(object sender, EventArgs e)
        {
            DialogCostHistory obj = new DialogCostHistory(_CostProductChangeLog);
            obj.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Library._fkGoodsActive.Remove(_ProductIdSelected);
            this.Dispose();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    int col = dataGridView1.CurrentCell.ColumnIndex;
                    int row = dataGridView1.CurrentRow.Index;
                    if (col == colCode)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colPZ];
                        dataGridView1.BeginEdit(true);
                    }
                    else if (col == colPZ)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colUnitName];
                        dataGridView1.BeginEdit(true);
                    }
                    else if (col == colUnitName)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colPricePerUnit];
                        dataGridView1.BeginEdit(true);
                    }
                    else if (col == colPricePerUnit)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[colDesc];
                        dataGridView1.BeginEdit(true);
                    }
                    else if (col == colDesc)
                    {
                        if (row == dataGridView1.Rows.Count - 1)
                        {
                            dataGridView1.Rows.Add(1);
                            dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[colCode];
                            dataGridView1.BeginEdit(true);
                        }
                        //dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[1];
                        //dataGridView1.BeginEdit(true);
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// open product vat type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Enter vat type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxVatTypeId_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        int vatId = int.Parse(textBoxVatTypeId.Text.Trim());
                        var getVat = Singleton.SingletonPriority1.Instance().ProductVatType.SingleOrDefault(w => w.Id == vatId);
                        if (getVat == null)
                        {
                            MessageBox.Show("ไม่ถูกต้อง");
                        }
                        else
                        {
                            textBoxVatTypeName.Text = getVat.Name;
                            textBoxVatValue.Text = getVat.Value + "";
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("ไม่ถูกต้อง");
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ลบแถว
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //MessageBox.Show("" + e.Row.Index);
            if (dataGridView1.Rows.Count == 0)
            {
                dataGridView1.Rows.Add("", 1, "-", "", 0, 0, 0, true, 0, 0, 0);
            }

        }
        /// <summary>
        /// เลือกผุ้จัดจำหน่าย
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            SelectedVendorPopup obj = new SelectedVendorPopup(this);
            obj.ShowDialog();
        }
        int _VendorId;
        public void BinddingVendor(int id)
        {
            _VendorId = id;
            List<Vendor> vendors = SingletonVender.Instance().Vendors;
            var data = vendors.SingleOrDefault(w => w.Id == id);
            textBoxVendorCode.Text = data.Code;
            textBoxVendorName.Text = data.Name;
        }
        /// <summary>
        /// คีย์ vendor แล้ว Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxVendorCode_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        //string code = textBoxVendorCode.Text.Trim();
                        //var getVendor = SingletonVender.Instance().Vendors.FirstOrDefault(w => w.Enable == true && w.Code == code);
                        //if (getVendor != null)
                        //{
                        //    BinddingVendor(getVendor.Id);
                        //}
                        //else
                        //{
                        //    _VendorId = 0;
                        //    textBoxVendorCode.Text = "";
                        //    textBoxVendorName.Text = "";
                        //}

                    }
                    catch (Exception)
                    {
                        _VendorId = 0;
                        textBoxVendorCode.Text = "";
                        textBoxVendorName.Text = "";
                    }

                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///  ประเภทสินค้า ProductGroup ต้องตรงกับ ระบบเก่า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            SelectedProductGroupPopup obj = new SelectedProductGroupPopup(this);
            obj.ShowDialog();
        }
        int _GroupId;
        public void BinddingProGroupChoose(ProductGroups send)
        {
            _GroupId = send.Id;
            textBoxProGroupCode.Text = send.Code;
            textBoxProGroupName.Text = send.Name;
        }
        /// <summary>
        /// เลือก ProductBrands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            SelectedProductBrandPopup obj = new SelectedProductBrandPopup(this);
            obj.ShowDialog();
        }
        int _BrandId;
        public void BinddingProBrandChoose(ProductBrands send)
        {
            _BrandId = send.Id;
            textBoxProBrandCode.Text = send.Code;
            textBoxProBrandName.Text = send.Name;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonNeedEdit_Click(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        /// <summary>
        /// ปิดหน้าต่าง จัดการสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManageProductForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Library._fkGoodsActive.Remove(_ProductIdSelected);
        }
    }
}
