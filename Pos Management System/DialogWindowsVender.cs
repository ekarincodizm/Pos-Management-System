using Pos_Management_System.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class DialogWindowsVender : Form
    {
        public DialogWindowsVender()
        {
            InitializeComponent();
        }

        CreatePOForm cpf = new CreatePOForm();

        public DialogWindowsVender(CreatePOForm cpf)
        {
            this.cpf = cpf;
            InitializeComponent();
        }

        private void DialogWindowsVender_Load(object sender, EventArgs e)
        {     
            List<Vendor> vendors = SingletonVender.Instance().Vendors;
            //dataGridView1.DataSource = vendors;

            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Id";
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns[1].Name = "รหัส";
            dataGridView1.Columns[2].Name = "ชื่อ";
            dataGridView1.Columns[3].Name = "ที่อยู่";
            dataGridView1.Columns[4].Name = "เบอร์โทร";
            dataGridView1.Columns[5].Name = "ราคาสินค้า";
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        
            var columns = (from t in vendors
                           where t.Enable == true
                           select new
                           {
                               Id = t.Id,
                               Code = t.Code,
                               Name = t.Name,
                               Address = t.Address,
                               Tel = t.Tel,
                               CostType = t.POCostType.Name                              
                           }).ToList();

            foreach (var item in columns)
            {
                dataGridView1.Rows.Add(item.Id, item.Code, item.Name, item.Address, item.Tel, item.CostType);
            }
        }

        private void buttoCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //DataGridViewRow rows = dataGridView1.CurrentRow;
            //rows.cu
            int iRow = dataGridView1.CurrentRow.Index;
            if (iRow < 0)
            {
                return;
            }

            int id = int.Parse(dataGridView1.Rows[iRow].Cells[0].Value.ToString());
 
            // instance call widget
            this.cpf.BinddingVendor(id);
            this.Dispose();           
        }
    }
}
