using CitysInfo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitysInfo.Configurations.User
{
    public class PointsOfInterestConfiguration : IEntityTypeConfiguration<PointsOfInterest>
    {
        void IEntityTypeConfiguration<PointsOfInterest>.Configure
            (EntityTypeBuilder<PointsOfInterest> builder)
        {
            builder.HasData(
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
        }
    }
}
