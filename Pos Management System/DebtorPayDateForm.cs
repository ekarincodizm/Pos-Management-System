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
    /// <summary>
    /// ลูกหนี้ ตัดชำระเงิน *ต้องสนใจ การทำคืนด้วย
    /// </summary>
    public partial class DebtorPayDateForm : Form
    {
        public DebtorPayDateForm(Form1 frm)
        {
            InitializeComponent();
        }
        int colCheck = 13;
        private void DebtorPayDateForm_Load(object sender, EventArgs e)
        {
            DataGridViewCheckBoxColumn ColumnCheckBox = new DataGridViewCheckBoxColumn();
            ColumnCheckBox.Width = width_columcheckbox;
            ColumnCheckBox.DataPropertyName = "Select";
            //dataGridView1.Columns.Add(ColumnCheckBox);
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(colCheck, -1, true);
            ckBox.Size = new Size(14, 14);
            rect.X = rect.Location.X + (rect.Width / 2) - (ckBox.Width / 2);
            rect.Y += 3;
            ckBox.Location = rect.Location;
            ckBox.CheckedChanged += new EventHandler(ckBox_CheckedChanged);
            dataGridView1.Controls.Add(ckBox);
            dataGridView1.Columns[colCheck].Frozen = false;
        }

        private void ckBox_CheckedChanged(object sender, EventArgs e)
        {

            if (ckBox.Checked == true)
            {
                for (int j = 0; j <= dataGridView1.Rows.Count - 1; j++)
                {
                    dataGridView1[colCheck, j].Value = true;
                    Console.WriteLine(dataGridView1[colCheck, j].Value);
                }
            }
            else
            {
                for (int j = 0; j <= dataGridView1.Rows.Count - 1; j++)
                {
                    dataGridView1[colCheck, j].Value = false;
                }
            }
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
        private System.Windows.Forms.DataGridView songsDataGridView = new System.Windows.Forms.DataGridView();
        CheckBox ckBox = new CheckBox();
        int width_columcheckbox = 50;

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            DateTime dateS = dateTimePickerStart.Value;
            DateTime dateE = dateTimePickerEnd.Value;
            using (SSLsEntities db = new SSLsEntities())
            {
                string payDate = "";
                List<PosHeader> data = new List<PosHeader>();
                if (radioButtonSale.Checked == true) // ค้นหาจากวันที่ ขาย คือ createDate
                {
                    if (checkBoxShowPayList.Checked == true) // ถ้าต้องการแสดง ที่ชำระไปแล้วด้วย
                    {
                        data = db.PosHeader.Where(w => w.Enable == true &&
                DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
            DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE) &&
        w.FKDebtor != null && w.FKPosType == MyConstant.POsType.Creadit).ToList();
                    }
                    else // ต้องการค้นหาวันที่ขาย และบิลที่ชำระแล้ว ไม่ต้องแสดง
                    {
                        data = db.PosHeader.Where(w => w.Enable == true && w.PayDate == null &&
                DbFunctions.TruncateTime(w.CreateDate) >= DbFunctions.TruncateTime(dateS) &&
            DbFunctions.TruncateTime(w.CreateDate) <= DbFunctions.TruncateTime(dateE) &&
        w.FKDebtor != null && w.FKPosType == MyConstant.POsType.Creadit).ToList();
                    }

                }
                else // แสดงเฉพาะบิลทที่ ชำระแล้ว ค้นจากวันที่ ชำระ PayDate
                {
                    data = db.PosHeader.Where(w => w.Enable == true && 
               DbFunctions.TruncateTime(w.PayDate) >= DbFunctions.TruncateTime(dateS) &&
           DbFunctions.TruncateTime(w.PayDate) <= DbFunctions.TruncateTime(dateE) &&
       w.FKDebtor != null && w.FKPosType == MyConstant.POsType.Creadit).ToList();
                }


                foreach (var item in data)
                {
                    // เป็น null หรือไม่
                    if (item.PayDate.HasValue)
                    {
                        payDate = item.PayDate.Value.ToString("dd//MM/yyyy");
                    }
                    else
                    {
                        payDate = "ยังไม่ชำระ";
                    }
                    dataGridView1.Rows.Add
                        (
                            item.Id,
                            item.CreateDate.ToString("dd/MM/yyyy"),
                            item.InvoiceNo,
                              payDate,
                            item.Debtor.Code,
                            item.Debtor.Name,
                            //item.QtyList,
                            //item.Qty,
                            Library.ConvertDecimalToStringForm(item.TotaNoVat),
                            Library.ConvertDecimalToStringForm(item.TotalBeforeVat),
                            Library.ConvertDecimalToStringForm(item.TotalVat),
                            Library.ConvertDecimalToStringForm(item.TotalBalance),
                            item.Users.Name,
                            "0.00",
                            Library.ConvertDecimalToStringForm(item.TotalBalance),
                            false
                        );
                }
                //RowColor();
            }
        }
        int colId = 0;
        int colInvoiceDate = 1;
        int colInvoiceNo = 2;
        int colPayDate = 3;
        int colDebtorCode = 4;
        int colDebtorName = 5;
        //int colQtyList = 6;
        //int colQtyUnit = 7;
        int colTotalUnVat = 6;
        int colTotalBeforeVat = 7;
        int colTotalVat = 8;
        int colTotalBalance = 9;
        int colCashier = 10;
        int colTotalCN = 11;
        int colTotalPay = 12;
        int colSelected = 13;
        /// <summary>
        /// ตกลง ยืนยันการชำระลูกหนี้
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            List<StackSelected> stackDebtor = new List<StackSelected>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var check = dataGridView1.Rows[i].Cells[colSelected].Value ?? "false";
                int fkPos = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                //int id = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                string invoiceNo = dataGridView1.Rows[i].Cells[colInvoiceNo].Value.ToString();
                string debtorCode = dataGridView1.Rows[i].Cells[colDebtorCode].Value.ToString();
                string debtorName = dataGridView1.Rows[i].Cells[colDebtorName].Value.ToString();
                // แปลว่า เลือกแล้ว
                if (check.ToString().ToLower() == "true")
                {
                    stackDebtor.Add(new StackSelected()
                    {
                        FKPos = fkPos,
                        InvoiceNo = invoiceNo,
                        DebtorCode = debtorCode,
                        DebtorName = debtorName
                    });
                }
            }
            ConfirmPayDateDebtorDialog obj = new ConfirmPayDateDebtorDialog(this, stackDebtor);
            obj.ShowDialog();

        }
        /// <summary>
        /// stack ค่าที่เลือก
        /// </summary>
        public class StackSelected
        {
            public int FKPos { get; set; }
            public string InvoiceNo { get; set; }
            //public int FKDebtor { get; set; }
            public string DebtorCode { get; set; }
            public string DebtorName { get; set; }

        }
        /// <summary>
        /// ยกเลิกชำระหนี้
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            List<StackSelected> stackDebtor = new List<StackSelected>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var check = dataGridView1.Rows[i].Cells[colSelected].Value ?? "0";
                int fkPos = int.Parse(dataGridView1.Rows[i].Cells[colId].Value.ToString());
                string invoiceNo = dataGridView1.Rows[i].Cells[colInvoiceNo].Value.ToString();
                string debtorCode = dataGridView1.Rows[i].Cells[colDebtorCode].Value.ToString();
                string debtorName = dataGridView1.Rows[i].Cells[colDebtorName].Value.ToString();
                // แปลว่า เลือกแล้ว
                if (check == "1")
                {
                    stackDebtor.Add(new StackSelected()
                    {
                        FKPos = fkPos,
                        InvoiceNo = invoiceNo,
                        DebtorCode = debtorCode,
                        DebtorName = debtorName
                    });
                }
            }
            CancelPayDateDebtorDialog obj = new CancelPayDateDebtorDialog(this, stackDebtor);
            obj.ShowDialog();
        }
        /// <summary>
        /// check true false checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colCheck)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                }
                else if (bool.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == false)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                }
                else
                {
                    // true
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                }
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(colCheck, -1, true);
            ckBox.Size = new Size(14, 14);
            rect.X = rect.Location.X + (rect.Width / 2) - (ckBox.Width / 2);
            rect.Y += 3;
            ckBox.Location = rect.Location;
        }
    }
}
