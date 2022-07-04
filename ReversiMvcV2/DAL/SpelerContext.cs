using Microsoft.EntityFrameworkCore;
using ReversiMvcV2.Models;

namespace ReversiMvcV2.DAL
{
    public class SpelerContext : DbContext
    {
        public SpelerContext(DbContextOptions<SpelerContext> options) : base(options) { }
        public DbSet<Speler> Spelers { get; set; }
        
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    var configuration = new ConfigurationBuilder()
    //        .SetBasePath(Directory.GetCurrentDirectory())
    //        .AddJsonFile("appsettings.json")
    //        .Build();

    //    var connectionString = configuration.GetConnectionString("ReversiDb2");
    //    optionsBuilder.UseSqlServer(connectionString);
    //}
}
