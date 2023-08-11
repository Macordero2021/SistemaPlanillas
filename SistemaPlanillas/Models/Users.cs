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
            Deductions = new HashSet<Deductions>();
            Extraordinary_payment = new HashSet<Extraordinary_payment>();
            hourly_payroll = new HashSet<hourly_payroll>();
            Monthly_payroll = new HashSet<Monthly_payroll>();
            Payroll_history = new HashSet<Payroll_history>();
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

        public int fk_id_paymentmethod { get; set; }

        public int? Fk_Id_Deparment { get; set; }

        public int? fk_salary { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deductions> Deductions { get; set; }

        public virtual Departaments Departaments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Extraordinary_payment> Extraordinary_payment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hourly_payroll> hourly_payroll { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Monthly_payroll> Monthly_payroll { get; set; }

        public virtual Payment_Method Payment_Method { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payroll_history> Payroll_history { get; set; }

        public virtual Salary_type Salary_type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_RolAndDepartment> User_RolAndDepartment { get; set; }

        public virtual User_Status User_Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Updates> User_Updates { get; set; }
    }
}
