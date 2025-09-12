using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string PatientFullName { get; set; }
        public string PatientTcNo { get; set; }
        public int? PatientAge { get; set; }
        public string PatientGender { get; set; }
        public string PatientEmail { get; set; }
        public string? PatientPassword { get; set; }
        public string PatientPhoneNumber { get; set; }
    }
}
