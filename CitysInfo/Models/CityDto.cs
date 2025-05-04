using System.ComponentModel.DataAnnotations.Schema;

namespace CitysInfo.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        //[NotMapped]
        public int NumberOfPointOfInterest
        {
            get
            {
                return PointsOfInterest.Count;
            }
        }
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; }
        = new List<PointOfInterestDto>();
    }
}
