using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public string? DoctorFullName { get; set; }
        public string? DoctorTcNo { get; set; }
        public string? DoctorSpecialization { get; set; }
        public string? DoctorPhoneNumber { get; set; }
        public string? DoctorEmail { get; set; }
        public string? DoctorPassword { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
