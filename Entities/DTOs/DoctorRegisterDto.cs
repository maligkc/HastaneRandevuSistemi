using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class DoctorRegisterDto
    {
        [Required]
        public string? DoctorFullName { get; set; }
        [Required]
        public string? DoctorTcNo { get; set; }
        [Required]
        public string? DoctorSpecialization { get; set; }
        [Required]
        public string? DoctorPhoneNumber { get; set; }
        [Required]
        public string? DoctorEmail { get; set; }
        [Required]
        public string? DoctorPassword { get; set; }
    }
}
