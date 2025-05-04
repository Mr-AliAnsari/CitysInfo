using CitysInfo.DbContexts;
using CitysInfo.Entities;
using CitysInfo.Models;
using CitysInfo.Services;
using Microsoft.EntityFrameworkCore;

namespace CitysInfo.Repositories
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoDbContext _context;
        public CityInfoRepository(CityInfoDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// City
        /// </summary>
        //public async Task<City?> GetByIdForCityAsync(
        //    int cityId, bool includePointsOfInterest)
        //{
        //    if (includePointsOfInterest)
        //    {
        //        return await _context.Cities.Include(u => u.PointsOfInterest)
        //            .FirstOrDefaultAsync(u => u.Id == cityId);
        //    }
        //    return await _context.Cities.FirstOrDefaultAsync(u => u.Id == cityId);
        //}

        //فقط بررسی می‌کند که آیا رکوردی وجود دارد یا نه.
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(u => u.Name).ToListAsync();
        }

        //Tupel
        //public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync

        //بهتر از تاپل، تایپ جنریک
        public async Task<PaginateResult<CityWithoutPointOfInterestDto>> GetCitiesAsync
            (string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            //collection to start from
            var collection = _context.Cities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(u => u.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(c => c.Name.Contains(searchQuery)
                || c.Description!.Contains(searchQuery));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(u => u.Name)
                .Skip(pageSize * (pageNumber - 1)) // از چه رکوردی شروع کنیم؟
                .Take(pageSize) // چند رکورد بگیریم؟
                .Select(c => new CityWithoutPointOfInterestDto // مپ در همین لایه
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                })
                .ToListAsync();

            //return new PaginateResult<City>(collectionToReturn, paginationMetadata);
            return new PaginateResult<CityWithoutPointOfInterestDto>(collectionToReturn, paginationMetadata);
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(u => u.Id == cityId);
        }

        /// <summary>
        /// PointsOfInterest
        /// </summary>
        public async Task<PointsOfInterest?> GetByIdForPointOfInterestAsync(
            int cityId, int pointOfInterestId)
        {
            return await _context.PointsOfInterest
                .FirstOrDefaultAsync(u => u.CityId == cityId && u.Id == pointOfInterestId);
        }

        public async Task<IEnumerable<PointsOfInterest>> GetPointsOfInterestForCityAsync(
            int cityId)
        {
            return await _context.PointsOfInterest.Where(u => u.CityId == cityId).ToListAsync();
        }

        public async Task<City?> GetByIdForCityAsync(
           int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await _context.Cities.Include(u => u.PointsOfInterest)
                    .FirstOrDefaultAsync(u => u.Id == cityId);
            }
            return await _context.Cities.FirstOrDefaultAsync(u => u.Id == cityId);
        }

        public async Task AddPointsOfInterestForCityAsync(int cityId, PointsOfInterest pointOfInterest)
        {
            var city = await GetByIdForCityAsync(cityId, false);
            if (city != null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        //public async Task DeletePointsOfInterestForCityAsync(int cityId, PointsOfInterest pointOfInterest)
        //{
        //    //await _context.PointsOfInterest.Remove(pointOfInterest);

        //    var city = await GetByIdForCityAsync(cityId, false);
        //    if (city != null)
        //    {
        //        city.PointsOfInterest.Remove(pointOfInterest);
        //    }
        //}

        public void DeletePointsOfInterestForCityAsync(PointsOfInterest pointOfInterest)
        {
            _context.PointsOfInterest.Remove(pointOfInterest);
        }

        public async Task<bool> SaveChangesAsync()
        {
            //just boolian
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CityNameMatchesCityId(int cityId, string? cityName)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
        }
    }
}
