using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class AdminLoginDto
    {
        [Required]
        public string AdminEmail { get; set; }
        [Required]
        public string AdminPassword { get; set; }
    }
}
