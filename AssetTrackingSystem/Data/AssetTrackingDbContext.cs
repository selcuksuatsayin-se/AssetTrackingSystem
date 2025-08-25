using Microsoft.EntityFrameworkCore;
using AssetTrackingSystem.Models;

namespace AssetTrackingSystem.Data
{
    public class AssetTrackingDbContext : DbContext
    {
        public AssetTrackingDbContext(DbContextOptions<AssetTrackingDbContext> options) : base(options)
        {
        }

        // DbSet'ler - Her model için bir tablo
        public DbSet<Computer> Computers { get; set; }
        public DbSet<DisplayMonitor> DisplayMonitors { get; set; }
        public DbSet<JabraHeadset> JabraHeadsets { get; set; }
        public DbSet<MobilePhone> MobilePhones { get; set; }
        public DbSet<Printer> Printers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tablo isimlerini özelleştirme
            modelBuilder.Entity<Computer>().ToTable("Computers");
            modelBuilder.Entity<DisplayMonitor>().ToTable("DisplayMonitors");
            modelBuilder.Entity<JabraHeadset>().ToTable("JabraHeadsets");
            modelBuilder.Entity<MobilePhone>().ToTable("MobilePhones");
            modelBuilder.Entity<Printer>().ToTable("Printers");

            // Computer entity konfigürasyonu
            modelBuilder.Entity<Computer>(entity =>
            {
                entity.Property(e => e.UserName).HasMaxLength(100);
                entity.Property(e => e.UserSurname).HasMaxLength(100);
                entity.Property(e => e.Model).HasMaxLength(200);
                entity.Property(e => e.HostName).HasMaxLength(100);
                entity.Property(e => e.OperatingSystem).HasMaxLength(100);
                entity.Property(e => e.SerialNumber).HasMaxLength(100);
                entity.Property(e => e.MAC).HasMaxLength(50);
                entity.Property(e => e.AssetNo).HasMaxLength(50);
                entity.Property(e => e.InvoiceNr).HasMaxLength(50);
                entity.Property(e => e.Company).HasMaxLength(200);
                entity.Property(e => e.Type).HasMaxLength(50);
                entity.Property(e => e.Note).HasMaxLength(1000);
                entity.Property(e => e.FaultNote).HasMaxLength(1000);
            });

            // DisplayMonitor entity konfigürasyonu
            modelBuilder.Entity<DisplayMonitor>(entity =>
            {
                entity.Property(e => e.UserName).HasMaxLength(100);
                entity.Property(e => e.UserSurname).HasMaxLength(100);
                entity.Property(e => e.Model).HasMaxLength(200);
                entity.Property(e => e.SerialNumber).HasMaxLength(100);
                entity.Property(e => e.Note).HasMaxLength(1000);
                entity.Property(e => e.FaultNote).HasMaxLength(1000);
            });

            // JabraHeadset entity konfigürasyonu
            modelBuilder.Entity<JabraHeadset>(entity =>
            {
                entity.Property(e => e.UserName).HasMaxLength(100);
                entity.Property(e => e.UserSurname).HasMaxLength(100);
                entity.Property(e => e.Model).HasMaxLength(200);
                entity.Property(e => e.SerialNumber).HasMaxLength(100);
                entity.Property(e => e.Note).HasMaxLength(1000);
                entity.Property(e => e.FaultNote).HasMaxLength(1000);
            });

            // MobilePhone entity konfigürasyonu
            modelBuilder.Entity<MobilePhone>(entity =>
            {
                entity.Property(e => e.UserName).HasMaxLength(100);
                entity.Property(e => e.UserSurname).HasMaxLength(100);
                entity.Property(e => e.Model).HasMaxLength(200);
                entity.Property(e => e.IMEI).HasMaxLength(50);
                entity.Property(e => e.Memory).HasMaxLength(50);
                entity.Property(e => e.Note).HasMaxLength(1000);
                entity.Property(e => e.FaultNote).HasMaxLength(1000);
            });

            // Printer entity konfigürasyonu
            modelBuilder.Entity<Printer>(entity =>
            {
                entity.Property(e => e.Location).HasMaxLength(200);
                entity.Property(e => e.HostName).HasMaxLength(100);
                entity.Property(e => e.Model).HasMaxLength(100);
                entity.Property(e => e.IP).HasMaxLength(50);
                entity.Property(e => e.SerialNumber).HasMaxLength(100);
                entity.Property(e => e.Note).HasMaxLength(1000);
                entity.Property(e => e.FaultNote).HasMaxLength(1000);
            });
        }
    }
}