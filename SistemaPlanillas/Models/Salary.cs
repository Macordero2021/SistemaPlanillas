namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Salary")]
    public partial class Salary
    {
        public int id { get; set; }

        public decimal? SalaryAmount { get; set; }

        public int fk_user { get; set; }

        public int fk_salary_type { get; set; }

        public virtual Salary_Type Salary_Type { get; set; }

        public virtual Users Users { get; set; }
    }
}
