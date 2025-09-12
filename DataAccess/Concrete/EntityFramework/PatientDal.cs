using Core.Concrete.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class PatientDal : EfEntityRepositoryBase<Patient, EfDbContext>, IPatientDal
    {
        public List<AppointmentDto> GetPatientAppointments(int patientId)
        {
            using (var context = new EfDbContext())
            {
                var result = from a in context.Appointments
                             join p in context.Patients on a.PatientId equals p.PatientId
                             join d in context.Doctors on a.DoctorId equals d.DoctorId
                             where a.PatientId == patientId
                             select new AppointmentDto
                             {
                                 AppointmentId = a.AppointmentId,
                                 PatientFullName = p.PatientFullName ?? "",
                                 PatientTcNo = p.PatientTcNo ?? "",
                                 PatientName = p.PatientFullName ?? "",
                                 PatientEmail = p.PatientEmail ?? "",
                                 PatientPhoneNumber = p.PatientPhoneNumber ?? "",
                                 DoctorFullName = d.DoctorFullName ?? "",
                                 DoctorName = d.DoctorFullName ?? "",
                                 DoctorSpecialization = d.DoctorSpecialization ?? "",
                                 AppointmentDate = a.AppointmentDate,
                                 AppointmentTime = a.AppointmentTime,
                                 AppointmentStatus = a.AppointmentStatus ?? 0,
                                 AppointmentNotes = "",
                                 DoctorId = d.DoctorId,
                                 PatientId = p.PatientId
                             };
                return result.ToList();
            }
        }

        public bool IsPatientExists(string fullName, string tcNo)
        {
            using (var context = new EfDbContext())
            {
                return context.Patients.Any(p => p.PatientFullName == fullName && p.PatientTcNo == tcNo);
            }
        }
    }
}
