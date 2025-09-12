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
    public class DoctorManager : IDoctorService
    {

        private readonly IDoctorDal _doctorDal;

        public DoctorManager(IDoctorDal doctorDal)
        {
            _doctorDal = doctorDal;
        }

        public void Add(DoctorRegisterDto registerDto)
        {
            Doctor doctor = new Doctor();

            doctor.DoctorFullName = registerDto.DoctorFullName;
            doctor.DoctorTcNo = registerDto.DoctorTcNo;
            doctor.DoctorEmail = registerDto.DoctorEmail;
            doctor.DoctorPhoneNumber = registerDto.DoctorPhoneNumber;
            doctor.DoctorPassword = registerDto.DoctorPassword;
            doctor.DoctorSpecialization = registerDto.DoctorSpecialization;

            _doctorDal.Add(doctor);
        }

        public void Delete(Doctor doctor)
        {
            _doctorDal.Delete(doctor);
        }

        public List<Doctor> GetAllDoctors()
        {
            return _doctorDal.GetAll();
        }

        public Doctor GetDoctor(int id)
        {
            return _doctorDal.Get(p => p.DoctorId == id);
        }

        public Doctor Login(string email, string password)
        {
            return _doctorDal.Get(p => p.DoctorEmail == email && p.DoctorPassword == password);
        }

        public void Update(Doctor doctor)
        {
            _doctorDal.Update(doctor);
        }

        public List<AppointmentDto> GetDoctorAppointments(int doctorId)
        {
            return _doctorDal.GetDoctorAppointments(doctorId);
        }

        public bool IsDoctorExists(string fullName, string tcNo)
        {
            return _doctorDal.IsDoctorExists(fullName, tcNo);
        }
    }
}
