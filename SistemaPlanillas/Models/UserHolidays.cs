namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserHolidays
    {
        [Key]
        public int id_holidays { get; set; }

        public int fk_id_user { get; set; }

        [StringLength(50)]
        public string Holidays { get; set; }

        public virtual Users Users { get; set; }
    }
}
