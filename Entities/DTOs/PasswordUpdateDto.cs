using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class PasswordUpdateDto
    {
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public string CurrentPassword { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string NewPassword { get; set; }
    }
}
