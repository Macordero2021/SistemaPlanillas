namespace SistemaPlanillas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            User_RolAndDepartment = new HashSet<User_RolAndDepartment>();
            User_Updates = new HashSet<User_Updates>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [StringLength(100)]
        public string lastname { get; set; }

        [Required]
        [StringLength(100)]
        public string email { get; set; }

        [StringLength(100)]
        public string phone { get; set; }

        [Required]
        [StringLength(100)]
        public string password { get; set; }

        public int fk_id_status { get; set; }

        [StringLength(100)]
        public string salary { get; set; }

        public int fk_id_paymentmethod { get; set; }

        public int? Fk_Id_Deparment { get; set; }

        public virtual Departaments Departaments { get; set; }

        public virtual Payment_Method Payment_Method { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_RolAndDepartment> User_RolAndDepartment { get; set; }

        public virtual User_Status User_Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Updates> User_Updates { get; set; }
    }
}
