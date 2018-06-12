using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System
{
    public static class MyConstant
    {
        public static string _ServerIP = "203.150.102.7";
        public static string _DBName = "SSLs";
        public static string _UserID = "sa";
        public static string _Password = "*Adm@sslog";
        /// <summary>
        /// ยืนยันการตรวจนับ
        /// </summary>
        public static class TypeCheck
        {
            public const string
                ConfirmCountOne = "ยืนยันการนับ 1",
                NoConfirmCountOne = "ยังไม่ยืนยันนับ 1",
                ConfirmCountTwo = "ยืนยันนับ 2";
        }
        /// <summary>
        ///  ประเภทรายงาน
        /// </summary>
        public static class TypeReport
        {
            public const int
                ReportMemberChange = 22, //  รายงานสรุปการจ่ายปันผล-เฉลี่ยคืน
                MemberChange2560 = 21, // ปันผลสมาชิก
                GetGoodsFrontUse = 20, // เบิกใช้เอง
                Adjust = 19, // adjust หน้าร้าน
                MemberActive = 18, // รายงานสมาชิก คงอยู่ - ลาออก
                InOutBalValueReport = 17, // รับ-จ่าย-คงเหลือ มูลค่า (ทะเบียนคุมสินค้า)
                CountProduct = 14, // รายงานผลต่าง
                LeftInStockVat = 16, // รายงานสินค้าคงเหลือ ราคาทุน-ราคาขาย
                StockCard = 15,
                InOutBalReport = 13, // รับ-จ่าย-คงเหลือ
                LeftInStock = 11, // รายงานสินค้าคงเหลือ
                  CheckStockDoc = 10, // รายการใบตรวจนับหน้าร้าน
                CheckStock = 9, // ใบตรวจนับหน้าร้าน
               PO = 1, // ใบ PO
                OrderWarehouse = 2, // ใบออเดอร์รถใหญ่
                CN = 3, // ใบรับคืน
                WastManage = 4, // ใบของเสีย หน้าร้าน
                TransferOut = 5,
                GoodsReturnWms = 6,
                ISS2Front = 7, // ใบเบิกหน้าร้าน
            RCVPO = 8; // ใบเบิกหน้าร้าน

        }
        /// <summary>
        /// หุ้น/บาท
        /// </summary>
        public static class SharedValue
        {
            public const decimal
            _100 = 100;
        }
        /// <summary>
        /// สถานะใบออร์เดอร์
        /// </summary>
        public static class SaleOrderWarehouseStatus
        {
            public const int
                   CancelOrder = 6, // ยกเลิกออร์เดอร์ ก่อนเข้าคลัง
                WarehouseRejectOrder = 5, // คลัง ปฏิเสธออร์เดอร์
                WarehouseISSSuccess = 4, //หยิบเรียบร้อย
                WarehouseISS = 3, // คลังยืนยันหยิบ
                ConfirmOrder = 2, // ยืนยันออเดอร์
            CreateOrder = 1;
        }
        /// <summary>
        /// สำหรับการ เบิกคลังเติมหน้าร้าน
        /// </summary>
        public static class ISS2FrontStatus
        {
            public const int
                ISSSucessNotValue = 6, // หยิบเรียบร้อย ไม่ตามยอด
                ISSSuccessValue = 5, //หยิบเรียบร้อยตามยอด
                CancelOrder = 4, //ปฏิเสธออร์เดอร์
                ConfirmOrder = 3,
            CreateOrder = 2;
        }
        public static class Zone
        {
            public const int
                MainZone = 1;
        }
        /// <summary>
        /// คลัง สินค้า ต่างๆ
        /// </summary>
        public static class WareHouse
        {
            public const int
                StoreFront = 2, // หน้าร้าน
                WasteWarehouse = 6, // คลังของเสีย
                MainWarehouse = 1; // คลังหลัก
        }
        public static class Shelf
        {
            public const int
                ShelfStart = 21;
        }
        /// <summary>
        /// สำหรับ Wms ดูสินค้ายุ่ในสถานใด
        /// </summary>
        public static class ItemRemark
        {
            public const int
                Nornal = 1,
                CN = 2;
        }
        /// <summary>
        /// สำหรับ StoreFront เกิดความเคลื่อนไหว
        /// </summary>
        public static class PosTransaction
        {
            public const int
                MinusProcessingGoods = 28, // ลดจำนวนแปรรูป
                AddProcessingGoods = 29, // เพิ่มจำนวนแปรรูป
                CancelProcessingGoods = 27, // ยกเลิกสินค้าแปรรูป
                ProcessingGoods = 26, // สร้างสินค้าแปรรูป
                GGF = 14, // เบิกใช้เอง
                ADJ = 23, // adjust หน้าร้าน
                ISS = 1, // เบิกสินค้าจากคลังเติม Store Front +
                Selling = 2, // ขายสดหน้าร้าน ขายลูกหนี้ -
                CNToWarehouse = 3, // จัดการสินค้าเสียหาย คืนคลัง -StoreFront +ห้องของเสีย
                CustomerReturn = 4, // ลูกค้านำของมาคืน ทำคืนสินค้า +
                CustomerReturnCancel = 5, // ยกเลิกทำคืนสินค้า -
                TransferStoreFrontToBranch = 8, // ทำโอนสินค้า Store Front ไปสาขาอื่นๆ -
                SetStock = 9; // ยกยอด +
        }
        /// <summary>
        /// เหตุการณ์ในคลังสินค้า
        /// </summary>
        public static class WmsTransaction
        {
            public const int
                StoreFrontToWarehouse = 2003, // รับของคืนจากหน้าร้าน +wms stock
                OrderWarehouse = 1003, // ออเดอร์คลังสินค้า รถใหญ่ รถเล็ก ขายแบบ เชื่อ -Wms
                TransferBranch = 5, //-Wms
                ISS = 4, //- // เบิกจากคลัง-Wms ไปเติมหน้าร้าน +StoreFront
                CN = 3, // -Wms // ส่งคืน Vendor 
                DetectWaste = 2004, // พบของเสียในคลัง ตัดออกมาเก็บห้องของเสีย -wms
                RCV = 1, //+Wms
                SetStock = 2; // +Wms ตั้งต้นยอด ยกยอด ตั้งระบบ
        }
        public static class MyBranch
        {
            public const int
                _01 = 2,
                _02 = 4;
        }

        public static class DeliveryType
        {
            public const int
                Subcontract = 8,//Subcontract
                       DeliverService = 7,//บริการส่ง
                CustomerDeliver = 6;//ลูกค้าขนส่ง
        }
        public static class Transport
        {
            public const int
                NotChoose = 2;
        }
        /// <summary>
        /// ใช้ในการเปิด PO
        /// </summary>
        public static class PaymentType
        {
            public const int
                CREDIT = 1006,
                CR90 = 1,
                CRUnlimit = 2,
                CR45 = 3,
                CR60 = 4,
                CashNow = 5; // ซื้อสด
        }
        /// <summary>
        /// เลขที่เปิด PO นำหน้าด้วย 1 หรือ 2
        /// </summary>
        public static class POCreditOrCash
        {
            public const int
                Credit = 1, // เชื่อ
                CashNow = 2; // ซื้อสด
        }
        public static class POStatus
        {
            public const int
                RCVComplete = 1, // รับเข้าเรียบร้อย
                RCVNotComplete_ButEnd = 2, // รับเข้ายังไม่ครบ แต่ปิดสถานะเรียบร้อย
                  RCVNotEnd = 3, // รับเข้ายังไม่ครบ ยังไม่ปิดสถานะ
            NotRCV = 4; // ยังไม่มีการรับเข้า
        }
        public static class SelectTopRow
        {
            public const int
               Product = 900, // สินค้า
                Default = 200; // ยึดทุนรวมภาษี
        }
        /// <summary>
        /// รหัส Id แคมเปญ
        /// </summary>
        public static class CampaignType
        {
            public const int
                FullyQtyAndGift = 17, // ซื้อครบจำนวน ได้แถมฟรี
                FullyAmountAndMsg = 16, // ซื้อครบยอดเงิน (คละได้) ได้รับข้อความพิเศษ
                FullyQtyAndMsg = 15, // ซื้อครบจำนวน (คละได้) ได้รับข้อความพิเศษ
                FullyAmountAndDiscount = 14, // ซื้อครบยอดเงิน (คละได้) ได้รับสิทธิ์เงินส่วนลด
                FullyAmountAndPrice = 13, // ซื้อครบยอดเงิน (คละได้) ได้รับสิทธิ์แลกซื้อในราคาพิเศษ
                FullyQtyAndDiscount = 12, // ซื้อครบจำนวน (คละได้) ได้รับสิทธิ์เงินส่วนลด
                FullyQtyAndPrice = 11, // ซื้อครบจำนวน (คละได้) ได้รับสิทธิ์แลกซื้อในราคาพิเศษ
                DiscountDay = 7, // ซื้อช่วงนาทีทอง
                Notice = 10; // แสดงข้อความแคมเปญ (แถมของนอกระบบ)
        }
        /// <summary>
        /// สะสมแต้มสมาชิก ในการซื้อสินค้าสมาชิก
        /// </summary>
        public static class PosProductCollect
        {
            public const int
               Collect = 1, // สะสมแต้ม
                UnCollect = 2; //ไม่ สะสมแต้ม
        }

        /// <summary>
        ///  PO ยึดราคาทุนเปล่า หรือ ทุนรวม Vat
        /// </summary>
        public static class POCostType
        {
            public const int
               CostOnly = 1, // ยึดทุนเปล่า
                CostAndVat = 2; // ยึดทุนรวมภาษี
        }

        public static class MyVat
        {
            public const decimal
               Vat = 7,
                VatRemove = 107;
        }
        public static class ProductVatType
        {
            public const int
                /// กำหนดเอง
                ManualVat = 3,
                /// ยกเว้น Vat
                UnVat = 2,
                /// มี Vat 
                HasVat = 1;
        }
        public static class VatType
        {
            public const int

                UnVat = 4,
                /// ยกเว้น Vat
                NoVat = 5,
                /// มี Vat 
                HasVat = 6;
        }
        /// <summary>
        ///  เพศ
        /// </summary>
        public static class Sex
        {
            public const int
               male = 2, // ชาย
                Female = 3; // หญิง
        }
        /// <summary>
        ///  เพศ
        /// </summary>
        public static class Shared
        {
            public const int
               General = 1, // ทั่วไป
                Premium = 2; // คนสำคัญ
        }
        /// <summary>
        ///  สด 1 เชื่อ 2
        /// </summary>
        public static class CNType
        {
            public const int
               Cash = 1, // สด
                Creadit = 2; // เชื่อ
        }
        /// <summary>
        ///  สด 1 เชื่อ 2
        /// </summary>
        public static class POsType
        {
            public const int
               Cash = 1, // สด
                Creadit = 2; // เชื่อ
        }
        public static class CostType
        {
            public const int
                Vat = 1,
            NoVat = 2;
        }
        /// <summary>
        /// โค้ดนำหน้า เช่น PO Transfer 
        /// </summary>
        public static class PrefixForGenerateCode
        {
            public const string
                AdjustWaste = "ADW", // ปรับปรุงห้องของเสีย
                ProcessGoods = "PSG", // เอกสารสินค้าแปรรูป
                GetGoodsForUse = "GFU", // ใบเบิกใช้งานเอง
                AdjustStoreFront = "ADJ", // adjust หน้าร้าน
                   CheckStoreFront = "CHK", // ตรวจนบสตอกหน้าร้าน
                CustomerCNCredit = "ISB", // ใบลูกค้าทำคืน เชื่อ
                CustomerCNCash = "ISCB", // ใบลูกค้าทำคืน สด
                GoodsReturnCN = "WCN", // ส่งคืนสินค้าในคลังของเสีย ให้ Vendor
                WmsTransferOut = "WTO", // โอนจากคลังไป สาขาอื่น
                RCVPOS = "RCVP", // ทำรับเข้าด้วย YY = ปีงบ MM เดือน xxxx
                SaleOrderFromWarehouse = "SOR", // สั่งออเดอร์รถใหญ่
                      StoreFrontAdd = "SFA", // เติมสินค้าหน้าร้าน StoreFront
                PurchaseOrder = "PO", // PO ใบสั่งซื้อ
               TransferWaste = "TW", // โอนของเสีย หน้าร้านไปคลังของเสีย (พบของเสียในคลัง ตัดออกมาเก็บห้องของเสีย)
                WHToWaste = "WHW", // โอนของเสียคลัง ไปห้องของเสีย
                TransferOut = "TO"; // โอนไปสาขาอื่น หน้าร้านไป สาขาอื่น
        }
    }
}
