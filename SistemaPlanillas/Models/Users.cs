namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [StringLength(100)]
        public string lastName { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(100)]
        public string phone { get; set; }

        [StringLength(100)]
        public string password { get; set; }

        public int? fkStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dateCreate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dateUpdate { get; set; }

        public int? fkCreateUser { get; set; }

        public int? fkUpdateUser { get; set; }

        public int? fkPaymentMethod { get; set; }

        [StringLength(100)]
        public string Salary { get; set; }
    }
}
