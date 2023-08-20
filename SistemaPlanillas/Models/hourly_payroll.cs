namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hourly_payroll
    {
        [Key]
        public int id_hourly_payroll { get; set; }

        public int? fk_iduser { get; set; }

        [StringLength(20)]
        public string work_day { get; set; }

        public int? worked_hours { get; set; }

        [StringLength(500)]
        public string notes { get; set; }

        public decimal? total_earnings { get; set; }

        [StringLength(50)]
        public string Payment_Status { get; set; }

        public virtual Users Users { get; set; }
    }
}
