using AutoMapper;
using CitysInfo.Entities;
using CitysInfo.Models;

namespace CitysInfo.Profiles
{
    public class PointsOfInterestProfile : Profile
    {
        public PointsOfInterestProfile()
        {
            // for send to client
            CreateMap<PointsOfInterest, PointOfInterestDto>();
            // for save to database
            CreateMap<PointOfInterestForCreationDto, PointsOfInterest>();
            CreateMap<PointOfInterestForUpdateDto, PointsOfInterest>();
            CreateMap<PointsOfInterest, PointOfInterestForUpdateDto>();

            //مثالی برای زمای که کلاسها ،فیلدهای هم نامی ندارند
            // CreateMap<PointsOfInterest, PointOfInterestDto>()
            //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
        }
    }
}
