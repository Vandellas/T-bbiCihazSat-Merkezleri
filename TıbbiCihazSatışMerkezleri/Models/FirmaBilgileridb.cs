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
    
    public partial class FirmaBilgileridb
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FirmaBilgileridb()
        {
            this.FirmaDosyalarıdb = new HashSet<FirmaDosyalarıdb>();
            this.MesulMüdürüdb = new HashSet<MesulMüdürüdb>();
            this.FirmaSahibidb = new HashSet<FirmaSahibidb>();
            this.FirmaDurum = new HashSet<FirmaDurum>();
            this.PersonelBilgileridb = new HashSet<PersonelBilgileridb>();
        }
    
        public int id { get; set; }
        public string il { get; set; }
        public string ilce { get; set; }
        public string TelNo { get; set; }
        public string TelNo2 { get; set; }
        public string FaxNo { get; set; }
        public string WebSite { get; set; }
        public string FirmaEposta { get; set; }
        public string VergiDairesi { get; set; }
        public string Mahalle { get; set; }
        public string Cadde { get; set; }
        public string Sokak { get; set; }
        public string DışKapıNo { get; set; }
        public string İçKapıNo { get; set; }
        public string VergiNo { get; set; }
        public string Type { get; set; }
        public string FirmaAdı { get; set; }
        public string PostaKodu { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FirmaDosyalarıdb> FirmaDosyalarıdb { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MesulMüdürüdb> MesulMüdürüdb { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FirmaSahibidb> FirmaSahibidb { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FirmaDurum> FirmaDurum { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonelBilgileridb> PersonelBilgileridb { get; set; }
    }
}
