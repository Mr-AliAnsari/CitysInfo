using CitysInfo.Entities;
using CitysInfo.Models;
using CitysInfo.Services;

namespace CitysInfo.Repositories
{
    public interface ICityInfoRepository
    {
        Task<City?> GetByIdForCityAsync(int cityId, bool includePointsOfInterest = false);
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<PaginateResult<CityWithoutPointOfInterestDto>> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

        Task<bool> CityExistsAsync(int cityId);

        Task<PointsOfInterest?> GetByIdForPointOfInterestAsync(int cityId, int pointOfInterestId);
        Task<IEnumerable<PointsOfInterest>> GetPointsOfInterestForCityAsync(int cityId);

        Task AddPointsOfInterestForCityAsync(int cityId, PointsOfInterest pointOfInterest);
        //Task DeletePointsOfInterestForCityAsync(int cityId,PointsOfInterest pointOfInterest);
        void DeletePointsOfInterestForCityAsync(PointsOfInterest pointOfInterest);

        Task<bool> CityNameMatchesCityId(int cityId, string? cityName);
        Task<bool> SaveChangesAsync();
    }
}
