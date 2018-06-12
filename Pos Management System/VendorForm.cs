using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class VendorForm : Form
    {
        public VendorForm()
        {
            InitializeComponent();
        }

        private void VendorForm_Load(object sender, EventArgs e)
        {
            dataGrid();
        }
        private void RowColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((i % 2) == 0)
                {
                    // คู่
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    // คี่
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.AliceBlue;
                }
            }
        }
        int colId = 0;
        int colCode = 1;
        int colName = 2;

        int _Id = 0;
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            BinddingSelect();
        }
        void BinddingSelect()
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[colId].Value.ToString());
            _Id = id;
            string code = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
            string name = dataGridView1.Rows[row].Cells[colName].Value.ToString();

            var vendor = Singleton.SingletonVender.Instance().Vendors.SingleOrDefault(w => w.Id == id);
            if (vendor.Enable)
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }

            if (vendor.FKPOCostType == MyConstant.POCostType.CostOnly)
            {
                radioButtonCostOnly.Checked = true;
                radioButtonCostAndVat.Checked = false;
            }
            else
            {
                radioButtonCostAndVat.Checked = true;
                radioButtonCostOnly.Checked = false;
            }
            textBoxCode.Text = code;
            textBoxName.Text = name;
            textBoxAddress.Text = vendor.Address;
            textBoxFax.Text = vendor.Fax;
            textBoxTel.Text = vendor.Tel;
            textBoxDesc.Text = vendor.Description;
            textBoxTax.Text = vendor.TaxNo;
        }
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // add new 
            _Id = 0;
            textBoxCode.Text = "";
            textBoxName.Text = "";
            textBoxAddress.Text = "";
            textBoxTel.Text = "";
            textBoxFax.Text = "";
            textBoxDesc.Text = "";
            radioButton1.Checked = true;
            radioButtonCostOnly.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                if (_Id == 0)
                {
                    // add new 
                    Vendor obj = new Vendor();
                    obj.Code = textBoxCode.Text;
                    obj.Name = textBoxName.Text;
                    obj.Address = textBoxAddress.Text;
                    obj.Tel = textBoxTel.Text;
                    obj.Fax = textBoxFax.Text;
                    obj.TaxNo = textBoxTax.Text.Trim();
                    obj.Description = textBoxDesc.Text;
                    obj.CreateDate = DateTime.Now;
                    obj.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    obj.Enable = true;
                    obj.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    obj.UpdateDate = DateTime.Now;
                    if (radioButtonCostOnly.Checked == true)
                    {
                        obj.FKPOCostType = MyConstant.POCostType.CostOnly;
                    }
                    else
                    {
                        obj.FKPOCostType = MyConstant.POCostType.CostAndVat;
                    }
                    db.Vendor.Add(obj);
                    var vendorsOnSingleton = Singleton.SingletonVender.Instance().Vendors;
                    if (vendorsOnSingleton.FirstOrDefault(w => w.Code == obj.Code) != null) // ถ้า code นี้ เคยมีแล้ว
                    {
                        MessageBox.Show("รหัสซ้ำบนระบบใหม่ (pos.Vendor) " + vendorsOnSingleton.FirstOrDefault(w => w.Code == obj.Code).Name);
                        return;
                    }
                    // add to wh customer and brand
                    using (WH_TRATEntities wh = new WH_TRATEntities())
                    {
                        var getCust = wh.WH_CUSTOMER_MAST.FirstOrDefault(w => (w.CUSTOMER_NO + "") == obj.Code);
                        if (getCust != null) // แปลว่าซ้ำยุ่ในระบบเก่า
                        {
                            MessageBox.Show("รหัสซ้ำบนระบบเก่า (dbo.WH_CUSTOMER_MAST) " + getCust.CUSTOMER_NAME);
                            return;
                        }
                        // check in brand
                        var getBrand = wh.WH_BRAND_MAST.FirstOrDefault(w => (w.BRAND_NO + "") == obj.Code);
                        if (getBrand != null) // ซ้ำ ยี่ห้อในระบบเก่า
                        {
                            MessageBox.Show("รหัสซ้ำบนระบบเก่า (dbo.WH_BRAND_MAST) " + getBrand.BRAND_NAME);
                            return;
                        }
                        // pass customer and brand to add 2 table
                        WH_CUSTOMER_MAST customer = new WH_CUSTOMER_MAST();
                        customer.CUSTOMER_NO = int.Parse(obj.Code);
                        customer.CUSTOMER_NAME = obj.Name;
                        customer.CUSTOMER_BRANCH = "-";
                        customer.CUSTOMER_TAXID = obj.TaxNo;
                        customer.CUSTOMER_ADDRESS = obj.Address;
                        customer.CUSTOMER_PROVINCE = "";
                        customer.CUSTOMER_POSTNO = null;
                        customer.CUSTOMER_PHONE = obj.Tel;
                        customer.CUSTOMER_FAX = obj.Fax;
                        customer.CUSTOMER_EMAIL = "";
                        customer.CREATE_DATE = DateTime.Now;
                        customer.UPDATE_DATE = null;
                        customer.USER_ID = obj.CreateBy;
                        wh.WH_CUSTOMER_MAST.Add(customer);
                        // brand 
                        WH_BRAND_MAST brand = new WH_BRAND_MAST();
                        brand.BRAND_NO = int.Parse(obj.Code);
                        brand.BRAND_NAME = obj.Name;
                        brand.CREATE_DATE = DateTime.Now;
                        brand.UPDATE_DATE = null;
                        brand.USER_ID = obj.CreateBy;
                        wh.WH_BRAND_MAST.Add(brand);
                        wh.SaveChanges();
                    }
                    db.SaveChanges();
                }
                else
                {
                    // edit
                    var obj = db.Vendor.SingleOrDefault(w => w.Id == _Id);
                    var vendorSingle = Singleton.SingletonVender.Instance().Vendors.SingleOrDefault(w => w.Id == _Id);
                    Singleton.SingletonVender.Instance().Vendors.Remove(vendorSingle);
                    obj.UpdateDate = DateTime.Now;
                    obj.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    //obj.Code = textBoxCode.Text;
                    obj.Name = textBoxName.Text;
                    obj.Address = textBoxAddress.Text;
                    obj.Tel = textBoxTel.Text;
                    obj.Fax = textBoxFax.Text;
                    obj.Description = textBoxDesc.Text;
                    if (radioButton1.Checked)
                    {
                        obj.Enable = true;
                    }
                    else
                    {
                        obj.Enable = false;
                    }
                    if (radioButtonCostOnly.Checked == true)
                    {
                        obj.FKPOCostType = MyConstant.POCostType.CostOnly;
                    }
                    else
                    {
                        obj.FKPOCostType = MyConstant.POCostType.CostAndVat;
                    }
                    db.Entry(obj).State = EntityState.Modified;
                }
                db.SaveChanges();

                string user = Singleton.SingletonAuthen.Instance().Id;
                var v = db.Vendor.Include("POCostType").OrderByDescending(w => w.UpdateDate).Where(w => w.UpdateBy == user).FirstOrDefault();
                Singleton.SingletonVender.Instance().Vendors.Add(v);

                dataGrid();
                //this.Dispose();
                //_Id = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void textBoxSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    Reload(textBoxSearchKey.Text.Trim());
                    break;
                default:
                    break;
            }
        }

        private void Reload(string v)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            List<Vendor> vendors = new List<Vendor>();

            if (radioButtonCode.Checked == true)
            {
                vendors = Singleton.SingletonVender.Instance().Vendors.Where(w => w.Code.Contains(v)).OrderByDescending(w => w.CreateDate).ToList();
            }
            else
            {
                vendors = Singleton.SingletonVender.Instance().Vendors.Where(w => w.Name.Contains(v)).OrderByDescending(w => w.CreateDate).ToList();
            }

            if (checkBoxActive.Checked == true && checkBoxNonActive.Checked == true) // แสดง enable
            {
                // not doing
            }
            else if (checkBoxActive.Checked == true && checkBoxNonActive.Checked == false)
            {
                vendors = vendors.Where(w => w.Enable == true).ToList();
            }
            else if (checkBoxActive.Checked == false && checkBoxNonActive.Checked == true)
            {
                vendors = vendors.Where(w => w.Enable == false).ToList();
            }
            foreach (var item in vendors)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Address, item.Tel, item.Fax, item.POCostType.Name, Library.GetFullNameUserById(item.CreateBy),
                   Library.ConvertBoolToStr(item.Enable), item.Description);
            }
            /// row color
            RowColor();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var obj = db.Vendor.SingleOrDefault(w => w.Id == _Id);
                var vendorSingle = Singleton.SingletonVender.Instance().Vendors.SingleOrDefault(w => w.Id == _Id);
                Singleton.SingletonVender.Instance().Vendors.Remove(vendorSingle);
                obj.UpdateDate = DateTime.Now;
                obj.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                obj.Code = textBoxCode.Text;
                obj.Name = textBoxName.Text;
                obj.Address = textBoxAddress.Text;
                obj.Tel = textBoxTel.Text;
                obj.Fax = textBoxFax.Text;
                obj.Description = textBoxDesc.Text;
                if (radioButton1.Checked)
                {
                    obj.Enable = true;
                }
                else
                {
                    obj.Enable = false;
                }
                if (radioButtonCostOnly.Checked == true)
                {
                    obj.FKPOCostType = MyConstant.POCostType.CostOnly;
                }
                else
                {
                    obj.FKPOCostType = MyConstant.POCostType.CostAndVat;
                }
                db.Entry(obj).State = EntityState.Modified;

                db.SaveChanges();

                string user = Singleton.SingletonAuthen.Instance().Id;
                var v = db.Vendor.Include("POCostType").OrderByDescending(w => w.UpdateDate).Where(w => w.UpdateBy == user).FirstOrDefault();
                Singleton.SingletonVender.Instance().Vendors.Add(v);

                dataGrid();
                //_Id = 0;
                Reload(textBoxSearchKey.Text.Trim());
            }
        }

        public void dataGrid()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            var data = Singleton.SingletonVender.Instance().Vendors;
            foreach (var item in data)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Address, item.Tel, item.Fax, item.POCostType.Name, Library.GetFullNameUserById(item.CreateBy),
                    Library.ConvertBoolToStr(item.Enable), item.Description);
            }
            /// row color
            RowColor();
        }

    }
}

