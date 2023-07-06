namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RolDepartmentUser")]
    public partial class RolDepartmentUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int? fkRoles { get; set; }

        public int? fkDepartments { get; set; }

        public int? fkUser { get; set; }
    }
}
