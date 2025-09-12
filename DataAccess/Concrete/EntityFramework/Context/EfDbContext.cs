using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.Context
{
    public class EfDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=MEMO\\SQLEXPRESS;Database=HastaneRandevuSistemi;Integrated Security=true; Encrypt=True;TrustServerCertificate=True;");
        }

        // veritabanındaki ilişkiler
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)  // Appointment → 1 Patient ilişkisi
                .WithMany(p => p.Appointments) // 1 Patient → birçok Appointment olabilir
                .HasForeignKey(a => a.PatientId);  // FK olarak Appointment.PatientId kullanılacak

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId);
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
    }
}
