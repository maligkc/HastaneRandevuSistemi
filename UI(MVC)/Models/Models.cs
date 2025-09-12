using Entities.DTOs;
using System.ComponentModel.DataAnnotations;

namespace UI_MVC_.Models
{
    //public class AdminLoginDto
    //{
    //    public string AdminEmail { get; set; }
    //    public string AdminPassword { get; set; }
    //}

    //public class Appointment
    //{
    //    public int AppointmentId { get; set; }
    //    public int PatientId { get; set; }
    //    public int DoctorId { get; set; }
    //    public DateTime AppointmentDate { get; set; }
    //    public TimeSpan AppointmentTime { get; set; }
    //    public int? AppointmentStatus { get; set; }
    //    public Patient? Patient { get; set; }
    //    public Doctor? Doctor { get; set; }
    //}

    //public class Doctor
    //{
    //    public int DoctorId { get; set; }
    //    public string? DoctorFullName { get; set; }
    //    public string? DoctorTcNo { get; set; }
    //    public string? DoctorSpecialization { get; set; }
    //    public string? DoctorPhoneNumber { get; set; }
    //    public string? DoctorEmail { get; set; }
    //    public string? DoctorPassword { get; set; }

    //    public ICollection<Appointment> Appointments { get; set; }
    //}

    
    //public class Patient
    //{
        
    //    public int PatientId { get; set; }
    //    public string? PatientFullName { get; set; }
    //    public string? PatientTcNo { get; set; }
    //    public int? PatientAge { get; set; }
    //    public string? PatientGender { get; set; }
    //    public string? PatientEmail { get; set; }
    //    public string? PatientPassword { get; set; }
    //    public string? PatientPhoneNumber { get; set; }

    //    public ICollection<Appointment> Appointments { get; set; }

    //}

    public class DashboardViewModel
    {
        public List<AppointmentDto> Appointments { get; set; } = new();
        public List<PatientDto> Patients { get; set; } = new();
        public List<DoctorDto> Doctors { get; set; } = new();
    }
}
