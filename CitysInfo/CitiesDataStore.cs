using CitysInfo.Models;

namespace CitysInfo
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        //public static CitiesDataStore current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Tehran",
                    Description = "This Is My City",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Jaye 1",
                            Description = "Jaye Didani 1"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Jaye 2",
                            Description = "Jaye Didani 2"
                        },
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Ahwaz",
                    Description = "This Is My City",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 3,
                            Name = "Jaye 3",
                            Description = "Jaye Didani 3"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 4,
                            Name = "Jaye 4",
                            Description = "Jaye Didani 4"
                        },
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Shiraz",
                    Description = "This Is My City",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 5,
                            Name = "Jaye 5",
                            Description = "Jaye Didani 5"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 6,
                            Name = "Jaye 6",
                            Description = "Jaye Didani 6"
                        },
                    }
                },
            };
        }
    }
}
