using System.ComponentModel.DataAnnotations;

namespace Dtos.General
{
    public class RegisterRequestDto
    {
        public RegisterRequestDto() : base()
        {

        }

        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
