using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class DoctorPasswordUpdateDto
    {
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        public string CurrentPassword { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string NewPassword { get; set; }
    }
}
