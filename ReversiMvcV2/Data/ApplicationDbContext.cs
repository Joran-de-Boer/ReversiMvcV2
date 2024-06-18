using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ReversiMvcV2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            const string SPELER_ROLE_ID = "22e40406-8a9d-2d82-912c-5d6a640ee696";
            const string ADMIN_ROLE_ID = "b421e928-0613-9ebd-a64c-f10b6a706e73";
            const string MEDIATOR_ROLE_ID = "47D6DC9B-9B5B-4F9C-A971-82A6BE3ADD8C";
            builder.Entity<IdentityRole>(e =>
            {
                e.HasData(new IdentityRole { Id = SPELER_ROLE_ID, Name = "Speler", NormalizedName = "SPELER" });
                e.HasData(new IdentityRole { Id = ADMIN_ROLE_ID, Name = "Beheerder", NormalizedName = "BEHEERDER" });
                e.HasData(new IdentityRole { Id = MEDIATOR_ROLE_ID, Name = "Mediator", NormalizedName = "MEDIATOR" });
            });
        }


    }
}