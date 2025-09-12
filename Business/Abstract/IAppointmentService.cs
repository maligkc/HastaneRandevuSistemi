using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAppointmentService
    {
        void Add(Appointment appointment);
        void Delete(Appointment appointment);
        void Update(Appointment appointment);
        Appointment GetAppointment(int id);
        List<Appointment> GetAllAppointments();
        List<Appointment> GetDeletedAppointments();
        public List<Appointment> GetActiveAppointments();
        List<AppointmentDto> GetAllAppointmentsWithDetails();
    }
}
