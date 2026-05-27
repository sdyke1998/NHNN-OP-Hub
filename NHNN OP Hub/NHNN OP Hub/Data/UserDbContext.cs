using Microsoft.EntityFrameworkCore;
using NHNN_OP_Hub.Models;

namespace NHNN_OP_Hub.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected readonly IConfiguration Configuration;
        public UserDbContext(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Configuration.GetConnectionString("UserAuthConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
