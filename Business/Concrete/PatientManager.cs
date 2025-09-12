using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class PatientManager : IPatientService
    {
        private readonly IPatientDal _patientDal;

        public PatientManager(IPatientDal patientDal)
        {
            _patientDal = patientDal;
        }

        public void Add(PatientRegisterDto registerDto)
        {
            Patient patient = new Patient();

            patient.PatientFullName = registerDto.PatientFullName;
            patient.PatientTcNo = registerDto.PatientTcNo;
            patient.PatientGender = registerDto.PatientGender;
            patient.PatientAge = registerDto.PatientAge;
            patient.PatientEmail = registerDto.PatientEmail;
            patient.PatientPhoneNumber = registerDto.PatientPhoneNumber;
            patient.PatientPassword = registerDto.PatientPassword;
            

            _patientDal.Add(patient);
        }

        public void Delete(Patient patient)
        {
            _patientDal.Delete(patient);
        }

        public List<Patient> GetAllPatients()
        {
            return _patientDal.GetAll();
        }

        public Patient GetPatient(int id)
        {
            return _patientDal.Get(p => p.PatientId == id);
        }

        public Patient Login(string email, string password)
        {
            return _patientDal.Get(p=> p.PatientEmail == email && p.PatientPassword == password);
        }

        public void Update(Patient patient)
        {
            _patientDal.Update(patient);
        }

        public List<AppointmentDto> GetPatientAppointments(int patientId)
        {
            return _patientDal.GetPatientAppointments(patientId);
        }

        public bool IsPatientExists(string fullName, string tcNo)
        {
            return _patientDal.IsPatientExists(fullName, tcNo);
        }
    }
}
