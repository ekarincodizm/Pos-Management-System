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
    public partial class ProductUnitForm : Form
    {
        public ProductUnitForm()
        {
            InitializeComponent();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void ProductUnitForm_Load(object sender, EventArgs e)
        {
            dataGrid();
        }

        int colId = 0;
        int colCode = 1;
        int colName = 2;
        int colDesc = 5;
        int _Id = 0;
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[colId].Value.ToString());
            _Id = id;
            string code = dataGridView1.Rows[row].Cells[colCode].Value.ToString();
            string name = dataGridView1.Rows[row].Cells[colName].Value.ToString();
            var desc = dataGridView1.Rows[row].Cells[colDesc].Value ?? "-";


            //bool enable = Singleton.SingletonPUnit.Instance().Units.SingleOrDefault(w => w.Id == id).Enable;
            //if (enable)
            //{
                radioButton1.Checked = true;
            //}
            //else
            //{
            //    radioButton2.Checked = true;
            //}
            textBoxCode.Text = code;
            textBoxName.Text = name;
            textBoxDesc.Text = desc.ToString();
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// save or edit 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                if (_Id == 0)
                {
                    // add new 
                    ProductUnit obj = new ProductUnit();
                    obj.Code = textBoxCode.Text;
                    obj.Name = textBoxName.Text;
                    obj.Description = textBoxDesc.Text;
                    obj.CreateDate = DateTime.Now;
                    obj.CreateBy = Singleton.SingletonAuthen.Instance().Id;
                    obj.Enable = true;
                    obj.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    obj.UpdateDate = DateTime.Now;
                    db.ProductUnit.Add(obj);
                }
                else
                {
                    // edit
                    var unit = db.ProductUnit.SingleOrDefault(w => w.Id == _Id);
                    unit.UpdateDate = DateTime.Now;
                    unit.UpdateBy = Singleton.SingletonAuthen.Instance().Id;
                    unit.Code = textBoxCode.Text;
                    unit.Name = textBoxName.Text;
                    unit.Description = textBoxDesc.Text;
                    if (radioButton1.Checked)
                    {
                        unit.Enable = true;
                    }
                    else
                    {
                        unit.Enable = false;
                    }
                    db.Entry(unit).State = EntityState.Modified;
                }
                db.SaveChanges();
                Singleton.SingletonPUnit.SetInstance();

                dataGrid();
                //this.Dispose();
                //_Id = 0;
            }
        }

        public void dataGrid()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            var data = Singleton.SingletonPUnit.Instance().Units;
            foreach (var item in data)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, Library.GetFullNameUserById(item.CreateBy),
                    Library.ConvertBoolToStr(item.Enable), item.Description);

            }
        }
    }
}
