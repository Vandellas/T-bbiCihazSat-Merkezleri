//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TıbbiCihazSatışMerkezleri.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class FirmaSahibidb
    {
        public int id { get; set; }
        public Nullable<int> Firmaid { get; set; }
        public string TcNo { get; set; }
        [Required]
        public string AdıSoyadı { get; set; }
        public byte[] HizmetSözleşmesi { get; set; }
    
        public virtual FirmaBilgileridb FirmaBilgileridb { get; set; }
    }
}
