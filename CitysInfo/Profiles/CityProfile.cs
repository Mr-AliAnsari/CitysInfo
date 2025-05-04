using AutoMapper;
using CitysInfo.Entities;
using CitysInfo.Models;

namespace CitysInfo.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityWithoutPointOfInterestDto>();
            CreateMap<City, CityDto>();
                //اگر نام فلدها مشابه هم نبود
            //.ForMember(u => u.PointsOfInterest, o => o.MapFrom(src => src.PointsOfInterest));

        }
    }
}
