//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Photography
{
    using System;
    using System.Collections.Generic;
    
    public partial class Photo
    {
        public int PhotoId { get; set; }
        public int InventoryId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
       
        public Nullable<decimal> Price { get; set; }
        
        public virtual INVENTORY INVENTORY { get; set; }
        public virtual VENDOR VENDOR { get; set; }
    }
}
