//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pos_Management_System
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_StockCount01
    {
        public bool Enable { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int FKProduct { get; set; }
        public int FKProductDetails { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal QtyUnit1 { get; set; }
        public decimal QtyUnit2 { get; set; }
        public decimal Packsize { get; set; }
        public string FKDocNo { get; set; }
        public int Number { get; set; }
        public decimal SellPricePerUnit { get; set; }
        public decimal CostOnlyPerUnit { get; set; }
        public decimal Total { get; set; }
        public decimal CostVat { get; set; }
        public int FKWarehouse { get; set; }
    }
}