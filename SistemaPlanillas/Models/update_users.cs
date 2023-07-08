namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class update_users
    {
        public int id { get; set; }

        [Column(TypeName = "date")]
        public DateTime date_create { get; set; }

        [Column(TypeName = "date")]
        public DateTime date_update { get; set; }

        public int fk_user_create { get; set; }

        public int? id_updateuser { get; set; }

        public virtual Users Users { get; set; }
    }
}
