using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitysInfo.Entities
{
    public class PointsOfInterest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [ForeignKey(nameof(CityId))]
        public City? City { get; set; }
        public int CityId { get; set; }
        //public PointsOfInterest(string name)
        //{
        //    Name = name;
        //}
    }
}
