namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class License_Application
    {
        [Key]
        public int id_licenseApplication { get; set; }

        public int fk_id_user { get; set; }

        public int fk_id_license_type { get; set; }

        [Column(TypeName = "date")]
        public DateTime? startDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? endDay { get; set; }

        [StringLength(30)]
        public string daysLicense { get; set; }

        [StringLength(100)]
        public string notes { get; set; }

        [StringLength(30)]
        public string status_license { get; set; }

        public virtual License_Type License_Type { get; set; }

        public virtual License_Type License_Type1 { get; set; }

        public virtual License_Type License_Type2 { get; set; }

        public virtual Users Users { get; set; }

        public virtual Users Users1 { get; set; }

        public virtual Users Users2 { get; set; }
    }
}
