using System;
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

        public virtual DbSet<departaments> departaments { get; set; }
        public virtual DbSet<payment_method> payment_method { get; set; }
        public virtual DbSet<Rol_Departament_User> Rol_Departament_User { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Status_user> Status_user { get; set; }
        public virtual DbSet<update_users> update_users { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<departaments>()
                .Property(e => e.name_departament)
                .IsUnicode(false);

            modelBuilder.Entity<departaments>()
                .HasMany(e => e.Rol_Departament_User)
                .WithRequired(e => e.departaments)
                .HasForeignKey(e => e.fk_id_departament)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<payment_method>()
                .Property(e => e.name_paymentmethod)
                .IsUnicode(false);

            modelBuilder.Entity<payment_method>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.payment_method)
                .HasForeignKey(e => e.fk_id_paymentmethod)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Roles>()
                .Property(e => e.name_rol)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Rol_Departament_User)
                .WithRequired(e => e.Roles)
                .HasForeignKey(e => e.fk_id_rol)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Status_user>()
                .Property(e => e.name_status)
                .IsUnicode(false);

            modelBuilder.Entity<Status_user>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Status_user)
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
                .HasMany(e => e.Rol_Departament_User)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.fk_id_user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.update_users)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.fk_user_create)
                .WillCascadeOnDelete(false);
        }
    }
}
