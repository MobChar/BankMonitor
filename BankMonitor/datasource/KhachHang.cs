//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BankMonitor.datasource
{
    using System;
    using System.Collections.Generic;
    
    public partial class KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachHang()
        {
            this.TaiKhoans = new HashSet<TaiKhoan>();
        }
    
        public string CMND { get; set; }
        public string HO { get; set; }
        public string TEN { get; set; }
        public string DIACHI { get; set; }
        public string PHAI { get; set; }
        public System.DateTime NGAYCAP { get; set; }
        public string SODT { get; set; }
        public string MACN { get; set; }
        public System.Guid rowguid { get; set; }
    
        public virtual ChiNhanh ChiNhanh { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
