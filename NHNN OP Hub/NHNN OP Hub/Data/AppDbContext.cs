using Microsoft.EntityFrameworkCore;
using NHNN_OP_Hub.Models;

namespace NHNN_OP_Hub.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
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
        public AppDbContext(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Configuration.GetConnectionString("DbConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
