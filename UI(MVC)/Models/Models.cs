using Entities.DTOs;
using System.ComponentModel.DataAnnotations;

namespace UI_MVC_.Models
{
    

    public class DashboardViewModel
    {
        public List<AppointmentDto> Appointments { get; set; } = new();
        public List<PatientDto> Patients { get; set; } = new();
        public List<DoctorDto> Doctors { get; set; } = new();
    }
}
