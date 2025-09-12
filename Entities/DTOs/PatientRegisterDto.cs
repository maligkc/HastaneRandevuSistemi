using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class PatientRegisterDto
    {
        [Required]
        public string? PatientFullName { get; set; }
        [Required]
        public string? PatientTcNo { get; set; }
        [Required]
        public int? PatientAge { get; set; }
        [Required]
        public string? PatientGender { get; set; }
        [Required]
        public string? PatientEmail { get; set; }
        [Required]
        public string? PatientPassword { get; set; }
        [Required]
        public string? PatientPhoneNumber { get; set; }

    }
}
