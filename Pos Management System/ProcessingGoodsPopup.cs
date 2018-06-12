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
    public partial class ProcessingGoodsPopup : Form
    {
        private PGoodsForm pGoodsForm;

        public ProcessingGoodsPopup()
        {
            InitializeComponent();
        }

        public ProcessingGoodsPopup(PGoodsForm pGoodsForm)
        {
            InitializeComponent();
            this.pGoodsForm = pGoodsForm;
        }

        private void ProcessingGoodsPopup_Load(object sender, EventArgs e)
        {
            using (SSLsEntities db = new SSLsEntities())
            {
                var data = db.ProductDetails.Where(w => w.Enable == true && w.Products.IsProcessingGoods == true).ToList();
                foreach (var item in data)
                {
                    dataGridView1.Rows.Add(item.Id, item.Code, item.Products.ThaiName, item.ProductUnit.Name, Library.ConvertDecimalToStringForm(item.PackSize), Library.ConvertDecimalToStringForm(item.SellPrice), item.Description);
                }
            }

        }

        private void textBoxSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string key = textBoxSearchKey.Text.Trim();
                    var result = new List<ProductDetails>();

                    if (radioButtonCode.Checked)
                    {
                        // ถ้าหาด้วย code
                        var getFirst = Singleton.SingletonProduct.Instance().ProductDetails.FirstOrDefault(w => w.Code == key && w.Enable == true && w.Products.IsProcessingGoods == true);
                        if (getFirst != null)
                        {
                            result = SingletonProduct.Instance().ProductDetails.Where(w => w.FKProduct == getFirst.FKProduct && w.Enable == true && w.Products.IsProcessingGoods == true).Take(MyConstant.SelectTopRow.Product).ToList();
                        }
                        else
                        {
                            result = SingletonProduct.Instance().ProductDetails.Where(w => w.Code.Contains(key) && w.Enable == true && w.Products.IsProcessingGoods == true).Take(MyConstant.SelectTopRow.Product).ToList();
                        }
                    }
                    else
                    {
                        string[] words = key.Split(' ');
                        // ถ้าหาด้วยชื่อ

                        //result = SingletonProduct.Instance().ProductDetails.Where(w => w.Products.ThaiName.Contains(key) && w.Enable == true).ToList();
                        switch (words.Count())
                        {
                            case 1:
                                result = SingletonProduct.Instance().ProductDetails
                                    .Where(w => w.Products.ThaiName.Contains(words[0]) && w.Enable == true && w.Products.IsProcessingGoods == true).Take(MyConstant.SelectTopRow.Product).ToList();
                                break;
                            case 2:
                                result = SingletonProduct.Instance().ProductDetails
                                    .Where(w => w.Products.ThaiName.Contains(words[0]) && w.Products.ThaiName.Contains(words[1]) &&
                                    w.Enable == true && w.Products.IsProcessingGoods == true).ToList();
                                break;
                            case 3:
                                result = SingletonProduct.Instance().ProductDetails
                                    .Where(w => w.Products.ThaiName.Contains(words[0]) &&
                                    w.Products.ThaiName.Contains(words[1]) &&
                                    w.Products.ThaiName.Contains(words[2]) &&
                                    w.Enable == true && w.Products.IsProcessingGoods == true).ToList();
                                break;
                            case 4:
                                result = SingletonProduct.Instance().ProductDetails
                                 .Where(w => w.Products.ThaiName.Contains(words[0]) &&
                                 w.Products.ThaiName.Contains(words[1]) &&
                                 w.Products.ThaiName.Contains(words[2]) &&
                                  w.Products.ThaiName.Contains(words[3]) &&
                                 w.Enable == true && w.Products.IsProcessingGoods == true).ToList();
                                break;
                            case 5:
                                result = SingletonProduct.Instance().ProductDetails
                                .Where(w => w.Products.ThaiName.Contains(words[0]) &&
                                w.Products.ThaiName.Contains(words[1]) &&
                                w.Products.ThaiName.Contains(words[2]) &&
                                 w.Products.ThaiName.Contains(words[3]) &&
                                     w.Products.ThaiName.Contains(words[4]) &&
                                w.Enable == true && w.Products.IsProcessingGoods == true).ToList();
                                break;
                            default:
                                break;
                        }
                    }

                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                    foreach (var item in result)
                    {
                        dataGridView1.Rows.Add(item.Id, item.Code, item.Products.ThaiName, item.ProductUnit.Name, Library.ConvertDecimalToStringForm(item.PackSize), Library.ConvertDecimalToStringForm(item.SellPrice), item.Description);
                    }

                    break;
                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// เลือก
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            pGoodsForm.BinddingProcessingGoods(id);
            this.Dispose();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            pGoodsForm.BinddingProcessingGoods(id);
            this.Dispose();
        }
    }
}
