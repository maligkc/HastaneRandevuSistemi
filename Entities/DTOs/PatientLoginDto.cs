using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class PatientLoginDto
    {
        [Required]
        public string? PatientEmail { get; set; }
        [Required]
        public string? PatientPassword { get; set; }
    }
}
