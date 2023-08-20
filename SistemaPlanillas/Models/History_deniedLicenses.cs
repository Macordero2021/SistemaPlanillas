namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class History_deniedLicenses
    {
        [Key]
        public int id_History_deniedLicenses { get; set; }

        public int fk_id_user { get; set; }

        public int fk_id_license_type { get; set; }

        [Column(TypeName = "date")]
        public DateTime? startDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? endDay { get; set; }

        [StringLength(100)]
        public string notes { get; set; }
    }
}
