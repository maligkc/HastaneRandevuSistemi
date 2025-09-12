using Core.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IDoctorDal : IEntityRepository<Doctor>
    {
        List<AppointmentDto> GetDoctorAppointments(int doctorId);
        bool IsDoctorExists(string fullName, string tcNo);
    }
}
