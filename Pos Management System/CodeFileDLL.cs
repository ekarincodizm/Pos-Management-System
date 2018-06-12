using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public static class CodeFileDLL
    {
        private static string cal_abc(int i)
        {
            string str = "";
            if (i == 1)
                str = "A";
            else if (i == 2)
                str = "B";
            else if (i == 3)
                str = "C";
            else if (i == 4)
                str = "D";
            else if (i == 5)
                str = "E";
            else if (i == 6)
                str = "F";
            else if (i == 7)
                str = "G";
            else if (i == 8)
                str = "H";
            else if (i == 9)
                str = "I";
            else if (i == 10)
                str = "J";
            else if (i == 11)
                str = "K";
            else if (i == 12)
                str = "L";
            else if (i == 13)
                str = "M";
            else if (i == 14)
                str = "N";
            else if (i == 15)
                str = "O";
            else if (i == 16)
                str = "P";
            else if (i == 17)
                str = "Q";
            else if (i == 18)
                str = "R";
            else if (i == 19)
                str = "S";
            else if (i == 20)
                str = "T";
            else if (i == 21)
                str = "U";
            else if (i == 22)
                str = "V";
            else if (i == 23)
                str = "W";
            else if (i == 24)
                str = "X";
            else if (i == 25)
                str = "Y";
            else if (i == 26)
                str = "Z";
            return str;
        }
        public static SSLsEntities db = new SSLsEntities();
        /// <summary>
        /// รับ สรุปรวมมา 2 แบบ
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="Head"></param>
        /// <param name="Keyword"></param>
        /// <param name="summary"></param>
        public static void ExcelReport(DataGridView dg, string Head, string Keyword, decimal[,] summary)
        {
            if (dg.Rows.Count == 0)
                return;
            try
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Worksheet xlSheet = default(Microsoft.Office.Interop.Excel.Worksheet);
                Microsoft.Office.Interop.Excel.Workbook xlBook = default(Microsoft.Office.Interop.Excel.Workbook);
                xlBook = xlApp.Workbooks.Add();
                xlSheet = xlBook.Worksheets[1];
                xlApp.ActiveSheet.Cells[1, 1] = "รายงานสรุป " + Head;
                xlApp.ActiveSheet.Range("A1:" + cal_abc(dg.Columns.Count) + "1").Merge();
                xlApp.ActiveSheet.Range("A1").HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
                xlApp.ActiveSheet.Cells[2, 1] = Keyword;
                xlApp.ActiveSheet.Range("A2:" + cal_abc(dg.Columns.Count) + "2").Merge();
                xlApp.ActiveSheet.Range("A2").HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
                xlApp.ActiveSheet.Range("A1:" + cal_abc(dg.Columns.Count) + "2").Interior.ColorIndex = 13;
                xlApp.ActiveSheet.Range("A1:" + cal_abc(dg.Columns.Count) + "2").Font.ColorIndex = 6;
                int k = 0;
                int i = 0;
                int j = 0;
                for (k = 0; k <= dg.Columns.Count - 1; k += 1)
                {
                    xlApp.ActiveSheet.Cells(3, k + 1).Value = dg.Columns[k].HeaderText;
                }
                for (i = 0; i <= dg.Rows.Count - 1; i += 1)
                {
                    for (j = 0; j <= dg.Columns.Count - 1; j += 1)
                    {
                        xlApp.ActiveSheet.Cells(i + 4, j + 1).Value = "'" + dg.Rows[i].Cells[j].EditedFormattedValue + "";
                    }
                }
                xlApp.ActiveSheet.Range("A3:" + cal_abc(dg.Columns.Count) + "3").FONT.Bold = true;
                xlApp.ActiveSheet.Range("A3:" + cal_abc(dg.Columns.Count) + (dg.Rows.Count + 3)).BORDERS.Weight = 2;

                foreach (DataGridViewColumn dc in dg.Columns)
                {
                    if (dc.ValueType == typeof(Decimal) || dc.ValueType == typeof(int))
                    {
                        int a = dc.Index + 1;
                        xlApp.ActiveSheet.Range(cal_abc(a) + "3:" + cal_abc(a) + (dg.Rows.Count + 3)).HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlRight;
                    }
                }
                int next = (i + 4);
                // set last
                for (int s = 0; s < summary.GetLength(summary.Rank); s++)
                {
                    // แถว / คอลัม / value
                    xlApp.ActiveSheet.Cells(next, summary[s, 0]).Value = "" + summary[s, 1];
                }

                xlApp.ActiveSheet.Columns("A:" + cal_abc(dg.Columns.Count)).AutoFit();
                MessageBox.Show("ออกรายงานเรียบร้อย", "Report Finish", MessageBoxButtons.OK, MessageBoxIcon.Information);
                xlApp.Application.Visible = true;
                xlSheet = null;
                xlBook = null;
                xlApp = null;
            }
            catch
            {
                MessageBox.Show("ลองใหม่อีกครั้ง", "เเจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        /// <summary>
        /// รายงานออกเอกเซล
        /// </summary>
        /// <param name="dg">datgrid ที่ต้องการ</param>
        /// <param name="Head">หัวรายงาน</param>
        /// <param name="Keyword">คำค้น</param>
        public static void ExcelReport(DataGridView dg, string Head, string Keyword)
        {
            if (dg.Rows.Count == 0)
                return;
            try
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Worksheet xlSheet = default(Microsoft.Office.Interop.Excel.Worksheet);
                Microsoft.Office.Interop.Excel.Workbook xlBook = default(Microsoft.Office.Interop.Excel.Workbook);
                xlBook = xlApp.Workbooks.Add();
                xlSheet = xlBook.Worksheets[1];
                xlApp.ActiveSheet.Cells[1, 1] = "รายงานสรุป " + Head;
                xlApp.ActiveSheet.Range("A1:" + cal_abc(dg.Columns.Count) + "1").Merge();
                xlApp.ActiveSheet.Range("A1").HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
                xlApp.ActiveSheet.Cells[2, 1] = Keyword;
                xlApp.ActiveSheet.Range("A2:" + cal_abc(dg.Columns.Count) + "2").Merge();
                xlApp.ActiveSheet.Range("A2").HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
                xlApp.ActiveSheet.Range("A1:" + cal_abc(dg.Columns.Count) + "2").Interior.ColorIndex = 13;
                xlApp.ActiveSheet.Range("A1:" + cal_abc(dg.Columns.Count) + "2").Font.ColorIndex = 6;
                int k = 0;
                int i = 0;
                int j = 0;
                for (k = 0; k <= dg.Columns.Count - 1; k += 1)
                {
                    xlApp.ActiveSheet.Cells(3, k + 1).Value = dg.Columns[k].HeaderText;
                }
                for (i = 0; i <= dg.Rows.Count - 1; i += 1)
                {
                    for (j = 0; j <= dg.Columns.Count - 1; j += 1)
                    {
                        xlApp.ActiveSheet.Cells(i + 4, j + 1).Value = "'" + dg.Rows[i].Cells[j].EditedFormattedValue + "";
                    }
                }
                xlApp.ActiveSheet.Range("A3:" + cal_abc(dg.Columns.Count) + "3").FONT.Bold = true;
                xlApp.ActiveSheet.Range("A3:" + cal_abc(dg.Columns.Count) + (dg.Rows.Count + 3)).BORDERS.Weight = 2;

                foreach (DataGridViewColumn dc in dg.Columns)
                {
                    if (dc.ValueType == typeof(Decimal) || dc.ValueType == typeof(int))
                    {
                        int a = dc.Index + 1;
                        xlApp.ActiveSheet.Range(cal_abc(a) + "3:" + cal_abc(a) + (dg.Rows.Count + 3)).HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlRight;
                    }
                }

                xlApp.ActiveSheet.Columns("A:" + cal_abc(dg.Columns.Count)).AutoFit();
                MessageBox.Show("ออกรายงานเรียบร้อย", "Report Finish", MessageBoxButtons.OK, MessageBoxIcon.Information);
                xlApp.Application.Visible = true;
                xlSheet = null;
                xlBook = null;
                xlApp = null;
            }
            catch
            {
                MessageBox.Show("ลองใหม่อีกครั้ง", "เเจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public static string s = "";
        public static string Strcon = "data source=203.150.102.7;initial catalog=WH_TRAT;persist security info=True;user id=sa;password=*Adm@sslog";
        public static DataTable selectsql(string sql)
        {
            SqlDataAdapter dtAdapter = new SqlDataAdapter();
            SqlConnection objConn = new SqlConnection();
            DataTable dt = new DataTable();
            try
            {
                //objConn.ConnectionString = db.Database.Connection.ConnectionString;
                objConn.ConnectionString = Strcon;
                objConn.Open();
                dtAdapter = new SqlDataAdapter(sql, objConn);
                dtAdapter.Fill(dt);
                objConn.Close();
                objConn = null;
                return dt;
            }
            catch (Exception exx)
            {
                Console.Write(exx.ToString());
                dt = null;
            }
            return dt;
        }
        public static Boolean excelsql(string sql)
        {
            SqlDataAdapter dtAdapter = new SqlDataAdapter();
            SqlConnection objConn = new SqlConnection();
            DataTable dt = new DataTable();
            bool check;
            try
            {
                objConn.ConnectionString = Strcon;
                objConn.Open();
                dtAdapter = new SqlDataAdapter(sql, objConn);
                dtAdapter.Fill(dt);
                objConn.Close();
                objConn = null;
                check = true;
            }
            catch (Exception exx)
            {
                Console.Write(exx.ToString());
                check = false;
            }
            return check;
        }
        public static double caltax(double data)
        {
            return (data * 7) / 100;
        }
    }
}
