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
    
    public partial class VENDOR
    {
        public VENDOR()
        {
            this.BILLINGs = new HashSet<BILLING>();
            this.CARTs = new HashSet<CART>();
            this.INVOICEs = new HashSet<INVOICE>();
            this.PHOTOS = new HashSet<Photo>();
            this.SHIPPINGs = new HashSet<SHIPPING>();
        }
    
        public int VendorId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    
        public virtual ICollection<BILLING> BILLINGs { get; set; }
        public virtual ICollection<CART> CARTs { get; set; }
        public virtual ICollection<INVOICE> INVOICEs { get; set; }
        public virtual ICollection<Photo> PHOTOS { get; set; }
        public virtual ICollection<SHIPPING> SHIPPINGs { get; set; }
    }
}
