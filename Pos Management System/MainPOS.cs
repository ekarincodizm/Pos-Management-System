using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public partial class MainPOS : Form
    {
        //List<WH_PRODUCT_MAST_HD> _WH_PRODUCT_MAST_HD = new List<WH_PRODUCT_MAST_HD>();
        //List<WH_PRODUCT_MAST_DTL> _WH_PRODUCT_MAST_DTL = new List<WH_PRODUCT_MAST_DTL>();
        //List<PS_CAMPAIGN_HD> _PS_CAMPAIGN_HD = new List<PS_CAMPAIGN_HD>();
        //List<PS_CAMPAIGN_DTL> _PS_CAMPAIGN_DTL = new List<PS_CAMPAIGN_DTL>();
        decimal _Quantity = 1;
        List<POsModel> pos = new List<POsModel>();

        public MainPOS()
        {
            InitializeComponent();
            //using (WH_TRATEntities db = new WH_TRATEntities())
            //{
            //    _WH_PRODUCT_MAST_HD = db.WH_PRODUCT_MAST_HD.Where(w => w.ENABLE == true).ToList();
            //    _WH_PRODUCT_MAST_DTL = db.WH_PRODUCT_MAST_DTL.Where(w => w.ENABLE == true).ToList();
            //    _PS_CAMPAIGN_HD = db.PS_CAMPAIGN_HD.Where(w => w.ENABLE == true && w.USING == true &&
            //    EntityFunctions.TruncateTime(w.START_DATE) <= EntityFunctions.TruncateTime(DateTime.Now) &&
            //    EntityFunctions.TruncateTime(w.END_DATE) >= EntityFunctions.TruncateTime(DateTime.Now)).ToList();

            //    List<string> campNos = new List<string>();
            //    campNos = _PS_CAMPAIGN_HD.Select(d => d.CAMPAIGN_NO).ToList<string>().ToList();
            //    _PS_CAMPAIGN_DTL = db.PS_CAMPAIGN_DTL.Where(w => campNos.Contains(w.CAMPAIGN_NO)).ToList();

            //}
        }
        /// <summary>
        /// ดึงราคาขาย
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        //private decimal GetPriceByProductNo(string productNo)
        //{
        //    var data = _WH_PRODUCT_MAST_DTL.SingleOrDefault(w => w.PRODUCT_NO == productNo);
        //    return (decimal)data.SELL_PRICE;
        //}
        /// <summary>
        /// get ชื่อสินค้า
        /// </summary>
        /// <param name="rowId"></param>
        /// <returns></returns>
        //private string GetProductName(int rowId)
        //{
        //    return _WH_PRODUCT_MAST_HD.SingleOrDefault(w => w.ROWID == rowId).PRODUCT_NAME;
        //}
        /// <summary>
        /// get ชื่อหน่วย
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        //private string GetProductUnit(string barcode)
        //{
        //    return _WH_PRODUCT_MAST_DTL.SingleOrDefault(w => w.PRODUCT_NO == barcode).UNIT_NAME;
        //}
        /// <summary>
        /// ประเภทถษี
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //private string GetVatType(int id)
        //{
        //    var vat = _WH_PRODUCT_MAST_HD.SingleOrDefault(w => w.ROWID == id);
        //    if (vat.VAT_TYPE) // ภาษี
        //    {
        //        return "I";
        //    }
        //    else
        //    {
        //        return "#";
        //    }

        //}
        /// <summary>
        /// ประเภทถษี
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //private bool GetVatType(int id, string c)
        //{
        //    var vat = _WH_PRODUCT_MAST_HD.SingleOrDefault(w => w.ROWID == id);
        //    return vat.VAT_TYPE;

        //}
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ClearTextMainScan()
        {
            textBox1.Text = "";
            textBox1.Select();

        }
        private void MainPOS_Load(object sender, EventArgs e)
        {
            //dataGridView1.ColumnCount
            //dataGridView1.Columns[0].HeaderCell.Value = "รหัสสินค้า";
            //dataGridView1.Columns[1].HeaderCell.Value = "รายการสินค้า";
            //dataGridView1.Columns[2].HeaderCell.Value = "จำนวน";
            //dataGridView1.Columns[3].HeaderCell.Value = "ราคา/หน่วย";
            //dataGridView1.Columns[4].HeaderCell.Value = "ชื่อหน่วย";
            //dataGridView1.Columns[5].HeaderCell.Value = "ภาษี";
            //dataGridView1.Columns[6].HeaderCell.Value = "ราคารวม";
            textBox1.Select();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            #region POS Main
            if (e.KeyCode == Keys.Enter)
            {
                //try
                //{
                //    _Quantity = decimal.Parse(textBox2.Text);
                //    //enter key is down
                //    Console.WriteLine(textBox1.Text);
                //    string barcdode = textBox1.Text;
                //    var getProduct = _WH_PRODUCT_MAST_DTL.SingleOrDefault(w => w.PRODUCT_NO == barcdode);
                //    // add to data grid
                //    string name = GetProductName(getProduct.HD_ROWID);
                //    // 1. รหัสสินค้า 2. รายการสินค้า 3.จำนวน 4.ราคา/หน่วย 5.ชื่อหน่วย 6. ภาษี 7.ราคารวม
                //    dataGridView1.Rows.Add
                //        (
                //        getProduct.PRODUCT_NO, //1
                //        name, //2
                //        _Quantity, //3
                //        Library.ConvertDecimalToStringForm((decimal)getProduct.SELL_PRICE), //4
                //        GetProductUnit(getProduct.PRODUCT_NO),//5
                //       GetVatType(getProduct.HD_ROWID),//6
                //        Library.ConvertDecimalToStringForm(_Quantity * (decimal)getProduct.SELL_PRICE)//7
                //        );

                //    this.pos.Add(new POsModel()
                //    {
                //        Sequence = this.pos.Count() + 1,
                //        ProductNo = getProduct.PRODUCT_NO,
                //        PricePerUnit = (decimal)getProduct.SELL_PRICE,
                //        VatType = GetVatType(getProduct.HD_ROWID, ""),
                //        PriceTotal = _Quantity * (decimal)getProduct.SELL_PRICE,
                //        Quatity = _Quantity
                //    });

                //    // add success
                //    textBox2.Text = "1";
                //    _Quantity = 1;
                //    ClearTextMainScan();
                //    textBox3.Text = "เพิ่มสำเร็จ";
                //}
                //catch (Exception)
                //{

                //    //MessageBox.Show("เกิดข้อผิดพลาด");
                //    textBox3.Text = "เกิดข้อผิดพลาด";
                //}
            }
            else if (e.KeyCode == Keys.End)
            {
                // Check Campaign 
                textBox3.Text = "ยืนยันการขาย";
                CheckCampaignGift(this.pos);

            }
            else if (e.KeyCode == Keys.Multiply)
            {
                // Add Quantity

                textBox2.Text = textBox1.Text;
                textBox2.Text = textBox2.Text.Replace("*", "");
                _Quantity = decimal.Parse(textBox2.Text);
                ClearTextMainScan();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                textBox2.Text = "1";
                _Quantity = 1;
                ClearTextMainScan();
                textBox3.Text = "เคลียร์หน้าจอ";

                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                this.pos = new List<POsModel>();
            }
            #endregion
        }
        private decimal SpliterGift(decimal qty, decimal condition, decimal gift)
        {
            // ดูว่า จำนวนนี้ จะได้ของแถมกี่ชิ้น

            //while (qty > condition)
            //{
            //    qty = qty - (condition + gift);
            //    if (qty >= 0)
            //    {
            //        i += gift;
            //    }
            //    else
            //    {
            //        i++;
            //    }
            //}
            decimal count = 0;
            bool getGift = false;
            decimal countGift = 0;
            decimal stackGift = 0;
            for (int i = 1; i <= qty; i++)
            {
                countGift++;
                if (countGift > condition)
                {
                    getGift = true;
                }

                if (getGift) // ถ้าได้แล้ว
                {
                    count++;
                    stackGift++;
                    if (count == gift)
                    {
                        getGift = false;
                        countGift = 0;
                        // ฝาก count               
                        count = 0;
                    }
                }
            }
            return stackGift;
            //var avg = qty / condition;
            //var mod = qty % condition;

            //var giftQty = avg * gift;
            //if (mod > 0)
            //{
            //    giftQty++;
            //}

        }
        /// <summary>
        /// ของแถม
        /// </summary>
        /// <param name="models"></param>
        private void CheckCampaignGift(List<POsModel> models)
        {
            //foreach (var item in _PS_CAMPAIGN_HD)
            //{
            //    var campNo = item.CAMPAIGN_NO;
            //    var details = _PS_CAMPAIGN_DTL.Where(w => w.CAMPAIGN_NO == campNo).ToList();
            //    var conditionQty = item.CONDITION_QTY;
            //    //var conditionStack = 0;
            //    // check product
            //    foreach (var productInCamp in details.Where(w => w.MAIN_PRODUCT == true).ToList()) // เอาเฉพาะ main product
            //    {
            //        var mainProduct = productInCamp.PRODUCT_NO;
            //        var getProdPos = models.Where(w => w.ProductNo == mainProduct).ToList(); // ดูใน pos ว่ามีสินค้าที่เป็น main มั้ย

            //        // ดูสิ ตรงตามเงื่อไขหรือไม่
            //        decimal sumQty = getProdPos.Sum(w => w.Quatity); // จำนวนของ main product ตรงตามเงื่อนไข
            //        if (sumQty > conditionQty) // ถ้าจำนวนตรงตามเงื่อนไขละก้อ
            //        {
            //            // หาสิ่งที่จะลดให้ อยู่ใน models
            //            foreach (var gift in details.Where(w => w.MAIN_PRODUCT == false).ToList()) // วนเชคว่า ในการซื้อทั้งหมดนี้ มีอะไรบ้างที่เป็น เกส
            //            {
            //                // มีอะไรใน pos บ้าง ที่เป็น gift
            //                var foundGift = models.Where(w => w.ProductNo == gift.PRODUCT_NO);
            //                var splitGift = SpliterGift(sumQty, conditionQty, gift.QTY);

            //                // 1. รหัสสินค้า 2. รายการสินค้า 3.จำนวน 4.ราคา/หน่วย 5.ชื่อหน่วย 6. ภาษี 7.ราคารวม                                                               
            //                dataGridView1.Rows.Add
            //                    (
            //                    "#C " + gift.PRODUCT_NO, //1
            //                    "[แคมเปญแถม] " + GetProductName(gift.PRODUCT_ID), //2
            //                    splitGift, //3
            //                    Library.ConvertDecimalToStringForm(GetPriceByProductNo(gift.PRODUCT_NO)), //4
            //                    GetProductUnit(mainProduct),//5
            //                    GetVatType(gift.PRODUCT_ID), //6
            //                    Library.ConvertDecimalToStringForm(-splitGift * GetPriceByProductNo(gift.PRODUCT_NO))//7
            //                );

            //            }
            //        }
            //        else
            //        {

            //        }

            //    }
            //}
        }
        /// <summary>
        /// นาทีทอง
        /// </summary>
        /// <param name="models"></param>
        private void CheckCampaignTimeDiscount(List<POsModel> models)
        {

        }
    }
}
