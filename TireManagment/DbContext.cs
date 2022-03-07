using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.DbModels;

namespace TireManagment
{
    public class DbContext : IdentityDbContext<AppUser>
    {
        private DbContextOptions _options;
        public DbContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tire>(tire => { tire.HasIndex(t => t.Serial).IsUnique(); });
            modelBuilder.Entity<TruckCategory>()
      .Property(c => c.SubmitDate)
      .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<TruckTire>()
     .Property(t => t.LastUdateDate)
     .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<TruckCategory>()
    .Property(t => t.CategoryId)
      .HasComputedColumnSql("[Id]");
            modelBuilder.Entity<Truck>()
  .Property(t => t.TruckId)
    .HasComputedColumnSql("[Id]");
            modelBuilder.Entity<Tire>()
  .Property(t => t.TireId)
    .HasComputedColumnSql("[Id]");

        }
        //  public DbSet<CemexMarketing.Models.RegisterViewModel> RegisterViewModel { get; set; }
        //public DbSet<CemexMarketing.Models.LoginViewModel> LoginViewModel { get; set; }

        public DbSet<Truck> trucks { get; set; }
        public DbSet<Tire> tires { get; set; }
        public DbSet<TruckCategory> categories { get; set; }
        public  DbSet<TireBrand>brands { get; set; }
        public DbSet<TireDistribution> tiredsitributions { get; set; }
        public DbSet<TruckTire> TruckTire { get; set; }
        public DbSet<TireMovement> TireMovement { get; set; }
        public DbSet<MovementDetails>  MovementDetails { get; set; }
    }
}
