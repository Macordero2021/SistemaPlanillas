namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Monthly_payroll
    {
        [Key]
        public int id_monthly_payroll { get; set; }

        public int? fk_iduser { get; set; }

        public int? total_workDays { get; set; }

        [StringLength(500)]
        public string notes { get; set; }

        public decimal? total_earnings { get; set; }

        [StringLength(50)]
        public string Payment_Status { get; set; }

        public virtual Users Users { get; set; }
    }
}
