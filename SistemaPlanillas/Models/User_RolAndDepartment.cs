namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_RolAndDepartment
    {
        public int id { get; set; }

        public int fk_id_rol { get; set; }

        public int fk_id_departament { get; set; }

        public int fk_id_user { get; set; }

        public virtual Departaments Departaments { get; set; }

        public virtual Roles Roles { get; set; }

        public virtual Users Users { get; set; }
    }
}
