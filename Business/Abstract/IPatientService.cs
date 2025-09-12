using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IPatientService
    {
        void Add(PatientRegisterDto registerDto);
        void Delete(Patient patient);
        void Update(Patient patient);
        Patient GetPatient(int id);
        Patient Login(string email, string password);
        List<Patient> GetAllPatients();
        List<AppointmentDto> GetPatientAppointments(int patientId);
        bool IsPatientExists(string fullName, string tcNo);
    }
}
