using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SistemaPlanillas.Models
{
    public partial class DataBase1Config : DbContext
    {
        public DataBase1Config()
            : base("name=DataBase1Config")
        {
        }

        public virtual DbSet<Deduction_type> Deduction_type { get; set; }
        public virtual DbSet<Deductions> Deductions { get; set; }
        public virtual DbSet<Departaments> Departaments { get; set; }
        public virtual DbSet<Extraordinary_payment> Extraordinary_payment { get; set; }
        public virtual DbSet<hourly_payroll> hourly_payroll { get; set; }
        public virtual DbSet<Monthly_payroll> Monthly_payroll { get; set; }
        public virtual DbSet<Payment_Method> Payment_Method { get; set; }
        public virtual DbSet<payment_type> payment_type { get; set; }
        public virtual DbSet<Payroll_history> Payroll_history { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Salary> Salary { get; set; }
        public virtual DbSet<Salary_Type> Salary_Type { get; set; }
        public virtual DbSet<User_RolAndDepartment> User_RolAndDepartment { get; set; }
        public virtual DbSet<User_Status> User_Status { get; set; }
        public virtual DbSet<User_Updates> User_Updates { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Deduction_type>()
                .Property(e => e.deduction_name)
                .IsUnicode(false);

            modelBuilder.Entity<Deduction_type>()
                .HasMany(e => e.Deductions)
                .WithOptional(e => e.Deduction_type)
                .HasForeignKey(e => e.fk_idDeductionType);

            modelBuilder.Entity<Deductions>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<Deductions>()
                .Property(e => e.deduction_value)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Departaments>()
                .Property(e => e.name_departament)
                .IsUnicode(false);

            modelBuilder.Entity<Departaments>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Departaments)
                .HasForeignKey(e => e.Fk_Id_Deparment);

            modelBuilder.Entity<Departaments>()
                .HasMany(e => e.User_RolAndDepartment)
                .WithRequired(e => e.Departaments)
                .HasForeignKey(e => e.fk_id_departament)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Extraordinary_payment>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<Extraordinary_payment>()
                .Property(e => e.payment_value)
                .HasPrecision(10, 2);

            modelBuilder.Entity<hourly_payroll>()
                .Property(e => e.work_day)
                .IsUnicode(false);

            modelBuilder.Entity<hourly_payroll>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<hourly_payroll>()
                .Property(e => e.total_earnings)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Monthly_payroll>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<Monthly_payroll>()
                .Property(e => e.total_earnings)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payment_Method>()
                .Property(e => e.name_paymentmethod)
                .IsUnicode(false);

            modelBuilder.Entity<Payment_Method>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Payment_Method)
                .HasForeignKey(e => e.fk_id_paymentmethod)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<payment_type>()
                .Property(e => e.payment_name)
                .IsUnicode(false);

            modelBuilder.Entity<payment_type>()
                .HasMany(e => e.Extraordinary_payment)
                .WithOptional(e => e.payment_type)
                .HasForeignKey(e => e.fk_id_payment);

            modelBuilder.Entity<payment_type>()
                .HasMany(e => e.Payroll_history)
                .WithOptional(e => e.payment_type)
                .HasForeignKey(e => e.fk_id_payment);

            modelBuilder.Entity<Payroll_history>()
                .Property(e => e.payment_day)
                .IsUnicode(false);

            modelBuilder.Entity<Payroll_history>()
                .Property(e => e.payment_amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Roles>()
                .Property(e => e.name_rol)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.User_RolAndDepartment)
                .WithRequired(e => e.Roles)
                .HasForeignKey(e => e.fk_id_rol)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Salary>()
                .Property(e => e.SalaryAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Salary_Type>()
                .Property(e => e.SalaryType)
                .IsUnicode(false);

            modelBuilder.Entity<Salary_Type>()
                .HasMany(e => e.Salary)
                .WithRequired(e => e.Salary_Type)
                .HasForeignKey(e => e.fk_salary_type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User_Status>()
                .Property(e => e.name_status)
                .IsUnicode(false);

            modelBuilder.Entity<User_Status>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.User_Status)
                .HasForeignKey(e => e.fk_id_status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.lastname)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Deductions)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.fk_idUser);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Extraordinary_payment)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.fk_idUser);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.hourly_payroll)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.fk_iduser);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Monthly_payroll)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.fk_iduser);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Payroll_history)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.fk_idUser);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Salary)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.fk_user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.User_RolAndDepartment)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.fk_id_user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.User_Updates)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.fk_user_create)
                .WillCascadeOnDelete(false);
        }
    }

    //Composite model to retrieve the information for an user or users
    public class UserCompositeModel
    {
        public Users User { get; set; }
        public Roles Role { get; set; }
        public Departaments Department { get; set; }
        public User_Status Status { get; set; }
        public Salary Salary { get; set; }
    }

    //Composite model to retrieve the information of all the users
    public class UserToEdit
    {
        public Users userToEdit { get; set; }
        public User_Status statusToEdit { get; set; }
        public Roles roleToEdit { get; set; }
        public List<Roles> RolesList { get; set; }
        public List<User_Status> StatusList { get; set; }
    }
}
