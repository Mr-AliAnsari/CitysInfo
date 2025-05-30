using CitysInfo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitysInfo.Configurations.User
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        void IEntityTypeConfiguration<City>.Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasMany(c => c.PointsOfInterest)
            .WithOne(c => c.City)
            .HasForeignKey(c => c.CityId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
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
        }
    }
}
