using OposPOSPrinter_CCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System.Repository
{
    public static class ReceiptPrint
    {
        /// <summary>
        /// ปริ้นซ้ำ
        /// </summary>
        /// <param name="rp"></param>
        public static void PrintNow(Model.ReceiptPrint rp, bool reprint, int sequence)
        {
            OPOSPOSPrinter myPrinter = new OPOSPOSPrinter();
            myPrinter.Open("TM-T82MU");
            myPrinter.ClaimDevice(100);
            myPrinter.DeviceEnabled = true;

            if (myPrinter.DeviceEnabled == false)
            {
                Console.WriteLine(" ********* Printer Not Detect **********");
            }
            char ESC = (char)13;
            myPrinter.PrintNormal(2, rp.CompanyName + " " + rp.CompanyBranch + ESC);
            myPrinter.PrintNormal(2, rp.CompanyAddress + ESC);
            myPrinter.PrintNormal(2, rp.CompanyTel + " " + rp.CompanyFax + ESC);
            myPrinter.PrintNormal(2, rp.CompanyLineId + ESC);
            myPrinter.PrintNormal(2, "ใบกำกับภาษีอย่างย่อ" + ESC);
            myPrinter.PrintNormal(2, rp.CompanyTaxId + ESC);
            myPrinter.PrintNormal(2, rp.CompanyRegId + ESC);
            myPrinter.PrintNormal(2, "ราคารวมภาษีมูลค่าเพิ่มแล้ว          <<พิมพ์ซ้ำ " + sequence + ">> #ยกเว้น" + ESC);
            myPrinter.PrintNormal(2, "------------------------------------------------" + ESC);
            foreach (var item in rp.ProductInReceipts)
            {
                myPrinter.PrintNormal(2, item.Name + ESC);
                string space1 = GetSpace(23, item.Code);
                string space2 = GetSpace(27, item.Qty + " X " + item.SalePrice + item.Total + item.ProductVatType);
                myPrinter.PrintNormal(2, item.Code + space1 + item.Qty + " X " + item.SalePrice + space2 + item.Total + item.ProductVatType + ESC);
            }
            int termSpace = 49;
            myPrinter.PrintNormal(2, "------------------------------------------------" + ESC);

            string space3 = GetSpace(termSpace, rp.TotalBalance + "รวม");
            myPrinter.PrintNormal(2, "รวม" + space3 + rp.TotalBalance + ESC);

            string space4 = GetSpace(termSpace, rp.TotalCash + "รับเงิน", 2);
            myPrinter.PrintNormal(2, "รับเงิน" + space4 + rp.TotalCash + ESC);

            string space5 = GetSpace(termSpace, rp.TotalChange + "ทอน");
            myPrinter.PrintNormal(2, "ทอน" + space5 + rp.TotalChange + ESC);

            termSpace = 20;
            string space = GetSpace(termSpace, "รหัสสมาชิก", 2);
            myPrinter.PrintNormal(2, "รหัสสมาชิก" + space + rp.MemberId + ESC);

            space = GetSpace(termSpace, "ชื่อสมาชิก", 3);
            myPrinter.PrintNormal(2, "ชื่อสมาชิก" + space + rp.MemberName + ESC);

            space = GetSpace(termSpace, "สินค้าไม่มีภาษี", 5);
            myPrinter.PrintNormal(2, "สินค้าไม่มีภาษี" + space + rp.TotalUnVat + ESC);

            space = GetSpace(termSpace, "สินค้ามีภาษี", 4);
            myPrinter.PrintNormal(2, "สินค้ามีภาษี" + space + rp.TotalHasVat + ESC);

            space = GetSpace(termSpace, "ภาษี", 1);
            myPrinter.PrintNormal(2, "ภาษี" + space + rp.TotalVat + ESC);

            space = GetSpace(termSpace, "รวมรายการ", 0);
            myPrinter.PrintNormal(2, "รวมรายการ" + space + rp.TotalList + ESC);

            space = GetSpace(termSpace, "รวมชิ้น", 2);
            myPrinter.PrintNormal(2, "รวมชิ้น" + space + rp.TotalUnit + ESC);

            myPrinter.PrintNormal(2, "          Thank you for shopping with us" + ESC);

            space = GetSpace(termSpace, "พนักงานขาย", 1);
            myPrinter.PrintNormal(2, "พนักงานขาย " + space + rp.CashierName + ESC);
            myPrinter.PrintNormal(2, "เลขที่ใบกำกับภาษี " + " " + rp.OrderNo + " " + rp.OrderDate + ESC);
   
            myPrinter.PrintNormal(2, "                 ขอบคุณที่ใช้บริการ" + ESC);

            myPrinter.PrintNormal(2, (char)27 + "|fP");

            myPrinter.DeviceEnabled = false;
            myPrinter.ReleaseDevice();
            myPrinter.Close();
        }

        public static void PrintNow(Model.ReceiptPrint rp)
        {
            OPOSPOSPrinter myPrinter = new OPOSPOSPrinter();
            myPrinter.Open("TM-T82MU");
            myPrinter.ClaimDevice(100);
            myPrinter.DeviceEnabled = true;

            if (myPrinter.DeviceEnabled == false)
            {
                Console.WriteLine(" ********* Printer Not Detect **********");
            }
            char ESC = (char)13;
            myPrinter.PrintNormal(2, rp.CompanyName + " " + rp.CompanyBranch + ESC);
            myPrinter.PrintNormal(2, rp.CompanyAddress + ESC);
            myPrinter.PrintNormal(2, rp.CompanyTel + " " + rp.CompanyFax + ESC);
            myPrinter.PrintNormal(2, rp.CompanyLineId + ESC);
            myPrinter.PrintNormal(2, "ใบกำกับภาษีอย่างย่อ" + ESC);
            myPrinter.PrintNormal(2, rp.CompanyTaxId + ESC);
            myPrinter.PrintNormal(2, rp.CompanyRegId + ESC);
            myPrinter.PrintNormal(2, "ราคารวมภาษีมูลค่าเพิ่มแล้ว                     #ยกเว้น" + ESC);
            myPrinter.PrintNormal(2, "------------------------------------------------" + ESC);
            foreach (var item in rp.ProductInReceipts)
            {
                myPrinter.PrintNormal(2, item.Name + ESC);
                string space1 = GetSpace(23, item.Code);
                string space2 = GetSpace(27, item.Qty + " X " + item.SalePrice + item.Total + item.ProductVatType);
                myPrinter.PrintNormal(2, item.Code + space1 + item.Qty + " X " + item.SalePrice + space2 + item.Total + item.ProductVatType + ESC);
            }
            int termSpace = 49;
            myPrinter.PrintNormal(2, "------------------------------------------------" + ESC);

            string space3 = GetSpace(termSpace, rp.TotalBalance + "รวม");
            myPrinter.PrintNormal(2, "รวม" + space3 + rp.TotalBalance + ESC);

            string space4 = GetSpace(termSpace, rp.TotalCash + "รับเงิน", 2);
            myPrinter.PrintNormal(2, "รับเงิน" + space4 + rp.TotalCash + ESC);

            string space5 = GetSpace(termSpace, rp.TotalChange + "ทอน");
            myPrinter.PrintNormal(2, "ทอน" + space5 + rp.TotalChange + ESC);

            termSpace = 20;
            string space = GetSpace(termSpace, "รหัสสมาชิก", 2);
            myPrinter.PrintNormal(2, "รหัสสมาชิก" + space + rp.MemberId + ESC);

            space = GetSpace(termSpace, "ชื่อสมาชิก", 3);
            myPrinter.PrintNormal(2, "ชื่อสมาชิก" + space + rp.MemberName + ESC);

            space = GetSpace(termSpace, "สินค้าไม่มีภาษี", 5);
            myPrinter.PrintNormal(2, "สินค้าไม่มีภาษี" + space + rp.TotalUnVat + ESC);

            space = GetSpace(termSpace, "สินค้ามีภาษี", 4);
            myPrinter.PrintNormal(2, "สินค้ามีภาษี" + space + rp.TotalHasVat + ESC);

            space = GetSpace(termSpace, "ภาษี", 1);
            myPrinter.PrintNormal(2, "ภาษี" + space + rp.TotalVat + ESC);

            space = GetSpace(termSpace, "รวมรายการ", 0);
            myPrinter.PrintNormal(2, "รวมรายการ" + space + rp.TotalList + ESC);

            space = GetSpace(termSpace, "รวมชิ้น", 2);
            myPrinter.PrintNormal(2, "รวมชิ้น" + space + rp.TotalUnit + ESC);

            myPrinter.PrintNormal(2, "          Thank you for shopping with us" + ESC);

            space = GetSpace(termSpace, "พนักงานขาย", 1);
            myPrinter.PrintNormal(2, "พนักงานขาย " + space + rp.CashierName + ESC);
            myPrinter.PrintNormal(2, "เลขที่ใบกำกับภาษี " + " " + rp.OrderNo + " " + rp.OrderDate + ESC);
            
            myPrinter.PrintNormal(2, "                 ขอบคุณที่ใช้บริการ" + ESC);

            myPrinter.PrintNormal(2, (char)27 + "|fP");

            myPrinter.DeviceEnabled = false;
            myPrinter.ReleaseDevice();
            myPrinter.Close();
        }
        static string GetSpace(int s, string value)
        {
            int space = s - value.Length;
            string spaceStr = "";
            for (int i = 0; i < space - 1; i++)
            {
                spaceStr += " ";
            }
            return spaceStr;
        }
        static string GetSpace(int s, string value, int thai)
        {
            int space = s - value.Length + thai;
            string spaceStr = "";
            for (int i = 0; i < space - 1; i++)
            {
                spaceStr += " ";
            }
            return spaceStr;
        }

    }
}
