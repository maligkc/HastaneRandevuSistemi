using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDoctorService
    {
        void Add(DoctorRegisterDto registerDto);
        void Delete(Doctor doctor);
        void Update(Doctor doctor);
        Doctor GetDoctor(int id);
        Doctor Login(string email,  string password);
        List<Doctor> GetAllDoctors();
        List<AppointmentDto> GetDoctorAppointments(int doctorId);
        bool IsDoctorExists(string fullName, string tcNo);
    }
}
