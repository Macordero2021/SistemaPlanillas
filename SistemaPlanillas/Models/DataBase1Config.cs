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

        public virtual DbSet<Departaments> Departaments { get; set; }
        public virtual DbSet<Payment_Method> Payment_Method { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<User_RolAndDepartment> User_RolAndDepartment { get; set; }
        public virtual DbSet<User_Status> User_Status { get; set; }
        public virtual DbSet<User_Updates> User_Updates { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Payment_Method>()
                .Property(e => e.name_paymentmethod)
                .IsUnicode(false);

            modelBuilder.Entity<Payment_Method>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Payment_Method)
                .HasForeignKey(e => e.fk_id_paymentmethod)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Roles>()
                .Property(e => e.name_rol)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.User_RolAndDepartment)
                .WithRequired(e => e.Roles)
                .HasForeignKey(e => e.fk_id_rol)
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
                .Property(e => e.salary)
                .IsUnicode(false);

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

    //Composite model to retrieve the information of all the users
    public class modelCompuesto
    {
        public List<Roles> Role { get; set; }
        public List<Users> User { get; set; }
        public List<User_RolAndDepartment> RoleDeparmentUser { get; set; }
        public List<User_Status> Status { get; set; }
    }

    //Composite model to retrieve the information of and specific user
    public class modelCompuestoInd
    {
        public Users User { get; set; }
        public User_RolAndDepartment RoleDeparmentUser { get; set; }
        public Departaments Departaments { get; set; }
        public User_Status Status { get; set; }
    }
}
