using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitysInfo.Entities
{
    public class City
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public ICollection<PointsOfInterest>? PointsOfInterest { get; set; }
            = new List<PointsOfInterest>();

        //public City(string name)
        //{
        //    //If we want to not accept a null value at initialization time
        //    //and raise an error, we should use ArgumentNullException.
        //    Name = name; // ?? throw new ArgumentNullException(nameof(name));
        //}
    }
}
