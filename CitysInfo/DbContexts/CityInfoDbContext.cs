using CitysInfo.Configurations.User;
using CitysInfo.Domain_Models.User;
using CitysInfo.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CitysInfo.DbContexts
{
    public class CityInfoDbContext : IdentityDbContext<User, Role, int>
    {
        public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options) : base(options)
        {

        }
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointsOfInterest> PointsOfInterest { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration<City>(new CityConfiguration());
            //modelBuilder.ApplyConfiguration<PointsOfInterest>(new PointsOfInterestConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CityInfoDbContext).Assembly);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}
    }


}
