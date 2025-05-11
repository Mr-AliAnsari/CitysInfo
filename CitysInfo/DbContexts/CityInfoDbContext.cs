using CitysInfo.Entities;
using Microsoft.EntityFrameworkCore;

namespace CitysInfo.DbContexts
{
    public class CityInfoDbContext : DbContext
    {
        public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options) : base(options)
        {
            
        }
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointsOfInterest> PointsOfInterest { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData
                (
                new City()
                {
                    Id = 1,
                    Name = "Tehran",
                    Description = "The one with that with park.",
                },
                new City()
                {
                    Id = 2,
                    Name = "Ahwaz",
                    Description = "The one with the ....",
                },
                new City()
                {
                    Id = 3,
                    Name = "Shiraz",
                    Description = "The one with that with tower.",
                });
            modelBuilder.Entity<PointsOfInterest>().HasData(

                new PointsOfInterest()
                {
                    Id = 1,
                    CityId = 1,
                    Name = "Central Park",
                    Description = "The most visit unban park in the Iran.",
                },
                new PointsOfInterest()
                {
                    Id = 2,
                    CityId = 1,
                    Name = "Empire State Building",
                    Description = "A 102-story skyscraper located in Midtown Tehran",
                },
                new PointsOfInterest()
                {
                    Id = 3,
                    CityId = 2,
                    Name = "Cathedral",
                    Description = "A Gothic style cathedral, conceived by architects Jan.",
                },
                new PointsOfInterest()
                {
                    Id = 4,
                    CityId = 2,
                    Name = "َAntwerp Central Station",
                    Description = "The the finest example of railway architecture in Belgi"
                },
                new PointsOfInterest()
                {
                    Id = 5,
                    CityId = 3,
                    Name = "َEiffel Tower",
                    Description = "A wrought iron lattice tower on the Champ de Mars, name"
                },
                new PointsOfInterest()
                {
                    Id = 6,
                    CityId = 3,
                    Name = "َThe Louvre",
                    Description = "The world's largest museum."
                });

            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}
    }


}
