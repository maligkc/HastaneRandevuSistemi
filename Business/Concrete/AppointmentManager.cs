using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AppointmentManager : IAppointmentService
    {
        private readonly IAppointmentDal _appointmentDal;

        public AppointmentManager(IAppointmentDal appointmentDal)
        {
            _appointmentDal = appointmentDal;
        }

        public void Add(Appointment appointment)
        {
            _appointmentDal.Add(appointment);

        }

        public void Delete(Appointment appointment)
        {
            appointment.AppointmentStatus = 2;
            _appointmentDal.Update(appointment);
        }

        public List<AppointmentDto> GetAllAppointmentsWithDetails()
        {
            return _appointmentDal.GetAllAppointmentsWithDetails();
        }

        public List<Appointment> GetAllAppointments()
        {
            

            return _appointmentDal.GetAll();
        }

        public List<Appointment> GetActiveAppointments()
        {
            return _appointmentDal.GetAll(p => p.AppointmentStatus == 1);
        }


        public Appointment GetAppointment(int id)
        {
            return _appointmentDal.Get(p => p.AppointmentId == id);
        }

        public List<Appointment> GetDeletedAppointments()
        {
            return _appointmentDal.GetAll(p => p.AppointmentStatus == 2);
        }

        public void Update(Appointment appointment)
        {
            _appointmentDal.Update(appointment);
        }
        //public void Update(AppointmentDto appointmentDto)
        //{
        //    _appointmentDal.Update(appointmentDto);
        //}
    }
}
