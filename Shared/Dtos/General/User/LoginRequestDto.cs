﻿using System.ComponentModel.DataAnnotations;

namespace Dtos.General
{
    public class LoginRequestDto
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
