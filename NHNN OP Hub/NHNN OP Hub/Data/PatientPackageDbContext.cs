using Microsoft.EntityFrameworkCore;
using NHNN_OP_Hub.Models;

namespace NHNN_OP_Hub.Data
{
    public class PatientPackageDbContext : DbContext
    {
        public DbSet<PatientPackage> PatientPackages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PatientPackage>()
                .HasDiscriminator<string>("PackageType")
                .HasValue<PostingPackage>("Posting")
                .HasValue<OutpatientPackage>("Outpatient");
        }

        protected readonly IConfiguration Configuration;
        public PatientPackageDbContext(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Configuration.GetConnectionString("PackagesConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}