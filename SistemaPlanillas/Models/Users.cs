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
            License_Application = new HashSet<License_Application>();
            License_Application1 = new HashSet<License_Application>();
            License_Application2 = new HashSet<License_Application>();
            Monthly_payroll = new HashSet<Monthly_payroll>();
            Payroll_history = new HashSet<Payroll_history>();
            Salary = new HashSet<Salary>();
            User_RolAndDepartment = new HashSet<User_RolAndDepartment>();
            User_Updates = new HashSet<User_Updates>();
            UserHolidays = new HashSet<UserHolidays>();
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deductions> Deductions { get; set; }

        public virtual Departaments Departaments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Extraordinary_payment> Extraordinary_payment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hourly_payroll> hourly_payroll { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<License_Application> License_Application { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<License_Application> License_Application1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<License_Application> License_Application2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Monthly_payroll> Monthly_payroll { get; set; }

        public virtual Payment_Method Payment_Method { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payroll_history> Payroll_history { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Salary> Salary { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_RolAndDepartment> User_RolAndDepartment { get; set; }

        public virtual User_Status User_Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Updates> User_Updates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserHolidays> UserHolidays { get; set; }
    }
}
