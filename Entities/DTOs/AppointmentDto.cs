using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public string? PatientFullName { get; set; }
        public string? PatientTcNo { get; set; }
        public string? PatientName { get; set; }
        public string? PatientEmail { get; set; }
        public string? PatientPhoneNumber { get; set; }
        public string? DoctorFullName { get; set; }
        public string? DoctorName { get; set; }
        public string? DoctorSpecialization { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public int AppointmentStatus { get; set; }
        public string? AppointmentNotes { get; set; }

        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }

}
