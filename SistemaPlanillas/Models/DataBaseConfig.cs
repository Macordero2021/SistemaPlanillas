using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SistemaPlanillas.Models
{
    public partial class DataBaseConfig : DbContext
    {
        public DataBaseConfig()
            : base("name=DataBaseConfig")
        {
        }

        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<RolDepartmentUser> RolDepartmentUser { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departments>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<Roles>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<Status>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<Users>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.lastName)
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
                .Property(e => e.Salary)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMethod>()
                .Property(e => e.name)
                .IsFixedLength();
        }
    }
}
