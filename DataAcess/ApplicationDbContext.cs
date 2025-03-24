using DataAcess.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using System.Reflection.Emit;

namespace DataAcess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Patient> patients { get; set; }
        public DbSet<Diagnosis> diagnosis { get; set; }
        public DbSet<Doctor>Doctors { get; set; }
        public DbSet<MedicalHistory> MedicalHistory { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<PatientMedication> patientMedications { get; set; }
        public DbSet<ProgressTracking> progressTracking { get; set; }
        public DbSet<Recommendation> recommendations { get; set; }
        public DbSet<WearableReading> wearableReadings { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Apply separate configuration classes
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.Entity<PatientMedication>()
    .HasKey(pm => new { pm.PatientId, pm.MedicationId }); // تحديد المفتاح المركب

        }

    }
}
