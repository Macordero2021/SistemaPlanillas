namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Payroll_history
    {
        [Key]
        public int id_payroll_history { get; set; }

        public int? fk_idUser { get; set; }

        [StringLength(20)]
        public string payment_day { get; set; }

        public decimal? payment_amount { get; set; }

        public int? fk_id_payment { get; set; }

        [StringLength(10)]
        public string payment_type { get; set; }

        public virtual Users Users { get; set; }
    }
}
