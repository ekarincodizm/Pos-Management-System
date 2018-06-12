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
    
    public partial class StoreFrontStockDetails
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int DocDtlNumber { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool Enable { get; set; }
        public decimal ActionQty { get; set; }
        public int FKStoreFrontStock { get; set; }
        public int FKTransactionType { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public int FKProductDetails { get; set; }
        public decimal ResultQty { get; set; }
        public decimal PackSize { get; set; }
        public string DocRefer { get; set; }
        public Nullable<int> DocReferDtlNumber { get; set; }
        public decimal CostOnlyPerUnit { get; set; }
        public decimal SellPricePerUnit { get; set; }
    
        public virtual StoreFrontStock StoreFrontStock { get; set; }
        public virtual ProductDetails ProductDetails { get; set; }
        public virtual TransactionType TransactionType { get; set; }
    }
}