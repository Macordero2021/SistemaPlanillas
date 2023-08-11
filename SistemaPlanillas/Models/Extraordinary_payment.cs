namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Extraordinary_payment
    {
        [Key]
        public int id_extraordinary_payment { get; set; }

        public int? fk_idUser { get; set; }

        [StringLength(500)]
        public string notes { get; set; }

        public int? fk_id_payment { get; set; }

        public decimal? payment_value { get; set; }

        public virtual payment_type payment_type { get; set; }

        public virtual Users Users { get; set; }
    }
}
