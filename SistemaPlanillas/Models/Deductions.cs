namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Deductions
    {
        [Key]
        public int id_deduction { get; set; }

        public int? fk_idUser { get; set; }

        [StringLength(500)]
        public string notes { get; set; }

        public int? fk_idDeductionType { get; set; }

        public decimal? deduction_value { get; set; }

        public virtual Deduction_type Deduction_type { get; set; }

        public virtual Users Users { get; set; }
    }
}
