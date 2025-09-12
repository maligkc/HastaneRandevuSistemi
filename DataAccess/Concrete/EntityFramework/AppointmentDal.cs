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
    public class AppointmentDal : EfEntityRepositoryBase<Appointment, EfDbContext>, IAppointmentDal
    {
        public List<AppointmentDto> GetAllAppointmentsWithDetails()
        {
            using (var context = new EfDbContext())
            {
                var result = from a in context.Appointments
                             join p in context.Patients on a.PatientId equals p.PatientId
                             join d in context.Doctors on a.DoctorId equals d.DoctorId
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
    }
}
