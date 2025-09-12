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
    public interface IPatientDal : IEntityRepository<Patient>
    {
        List<AppointmentDto> GetPatientAppointments(int patientId);
        bool IsPatientExists(string fullName, string tcNo);
    }
}
