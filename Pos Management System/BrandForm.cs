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
    public partial class BrandForm : Form
    {
        public BrandForm()
        {
            InitializeComponent();
        }

        private void BrandForm_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            var data = Singleton.SingletonProductBrand.Instance().ProductBrands;
            foreach (var item in data)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, Library.GetFullNameUserById(item.CreateBy),
                    Library.ConvertBoolToStr(item.Enable), item.Description);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // add new 
            _Id = 0;
            textBoxCode.Text = "";
            textBoxName.Text = "";
            textBoxDesc.Text = "";
            radioButton1.Checked = true;
        }
        int colId = 0;
        int colCode = 1;
        int colName = 2;
        int colDesc = 5;
        int _Id = 0;
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[colId].Value.ToString());
            _Id = id;
            string code = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
            string name = dataGridView1.Rows[row].Cells[colName].Value.ToString();
            var desc = dataGridView1.Rows[row].Cells[colDesc].Value ?? "-";

            bool enable = Singleton.SingletonProductBrand.Instance().ProductBrands.SingleOrDefault(w => w.Id == id).Enable;
            if (enable)
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
            textBoxCode.Text = code;
            textBoxName.Text = name;
            textBoxDesc.Text = desc.ToString();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                if (_Id == 0)
                {
                    // add new 
                    ProductBrands obj = new ProductBrands();
                    obj.Code = textBoxCode.Text;
                    obj.Name = textBoxName.Text;
                    obj.Description = textBoxDesc.Text;
                    obj.CreateDate = DateTime.Now;
                    obj.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    obj.Enable = true;
                    obj.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    obj.UpdateDate = DateTime.Now;
                    db.ProductBrands.Add(obj);

                    //add to wh
                    using (WH_TRATEntities wh = new WH_TRATEntities())
                    {
                        var getBrand = wh.WH_BRAND_MAST.FirstOrDefault(w => (w.BRAND_NO + "") == obj.Code);
                        if (getBrand == null)
                        {
                            // add now
                            WH_BRAND_MAST brand = new WH_BRAND_MAST();
                            brand.BRAND_NAME = obj.Name;
                            brand.BRAND_NO = int.Parse(obj.Code);
                            brand.CREATE_DATE = DateTime.Now;
                            brand.USER_ID = obj.UpdateBy;
                            wh.WH_BRAND_MAST.Add(brand);
                            wh.SaveChanges();
                        }
                        else
                        {
                            // not add
                        }
                    }
                }
                else
                {
                    // edit
                    var obj = db.ProductBrands.SingleOrDefault(w => w.Id == _Id);
                    obj.UpdateDate = DateTime.Now;
                    obj.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    //obj.Code = textBoxCode.Text;
                    obj.Name = textBoxName.Text;
                    obj.Description = textBoxDesc.Text;
                    if (radioButton1.Checked)
                    {
                        obj.Enable = true;
                    }
                    else
                    {
                        obj.Enable = false;
                    }
                    db.Entry(obj).State = EntityState.Modified;
                }
                db.SaveChanges();
                Singleton.SingletonPriority1.SetInstance();
                Singleton.SingletonPriority1.Instance();
                this.Dispose();
                _Id = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
