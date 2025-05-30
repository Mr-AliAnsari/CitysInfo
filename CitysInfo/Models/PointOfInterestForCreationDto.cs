﻿using System.ComponentModel.DataAnnotations;

namespace CitysInfo.Models
{
    public class PointOfInterestForCreationDto
    {
        [Required(ErrorMessage = "you should provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }

}
