using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WageClerk.Data.Models
{
    public partial class InnovationDayContext : DbContext
    {
        public InnovationDayContext()
        {
        }

        public InnovationDayContext(DbContextOptions<InnovationDayContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }
       // public virtual DbSet<EmployeeShift> EmployeeShift { get; set; }
        public virtual DbSet<Shift> Shift { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=wage-clerk-dot-net.cbjdjeni5y4d.ap-southeast-2.rds.amazonaws.com;Database=InnovationDay;User Id=admin;Password=9cr4Jg7Azw0UVAMyStIvCql;Trusted_Connection=True;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            
            modelBuilder.Entity<Shift>(entity =>
            {
                entity.Property(e => e.ClockIn).HasColumnType("datetime");

                entity.Property(e => e.ClockOut).HasColumnType("datetime");

                entity.Property(e => e.ShiftRating)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
