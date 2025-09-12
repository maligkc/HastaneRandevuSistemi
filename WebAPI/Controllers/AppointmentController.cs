using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {

        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("getAllAppointments")]
        public IActionResult GetAllAppointments()
        {
            var appointments = _appointmentService.GetAllAppointments();
            if(appointments == null)
            {
                return NotFound("Randevu bulunamadı");
            }
            return Ok(appointments);
        }

        [HttpGet("getDeletedAppointment")]
        public IActionResult GetDeletedAppointment()
        {
            var deletedAppointments = _appointmentService.GetDeletedAppointments();
            return Ok(deletedAppointments);
        }

        [HttpGet("getActiveAppointment")]
        public IActionResult GetActiveAppointment()
        {
            var activeAppointments = _appointmentService.GetActiveAppointments();
            return Ok(activeAppointments);
        }

        [HttpGet("getAppointmentById/{id}")]
        public IActionResult GetAppointmentById(int id)
        {
            var appointment = _appointmentService.GetAppointment(id);
            if(appointment == null)
            {
                return NotFound("Belirtilen Id'de randevu bulunamadı...");
            }
            return Ok(appointment);
        }

        [HttpPost("createAppointment")]
        public IActionResult CreateAppointment(Appointment appointment)
        {
            _appointmentService.Add(appointment);
            return Ok("Randevunuz başarıyla oluşturulmuştur!");
        }

        [HttpPost("addAppointment")]
        public IActionResult AddAppointment(AppointmentDto appointmentDto)
        {
            // DTO'dan Appointment entity'sine mapping
            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                AppointmentDate = appointmentDto.AppointmentDate,
                AppointmentTime = appointmentDto.AppointmentTime,
                AppointmentStatus = appointmentDto.AppointmentStatus
            };
            
            _appointmentService.Add(appointment);
            return Ok("Randevunuz başarıyla oluşturulmuştur!");
        }

        [HttpPut("deleteAppointment")]
        public IActionResult DeleteAppointment(Appointment appointment)
        {
            _appointmentService.Delete(appointment);
            return Ok("Randevunuz başarıyla silinmiştir!");
        }

        [HttpDelete("deleteAppointment/{id}")]
        public IActionResult DeleteAppointmentById(int id)
        {
            try
            {
                Console.WriteLine($"DeleteAppointmentById çağrıldı - ID: {id}");
                
                var appointment = _appointmentService.GetAppointment(id);
                if (appointment == null)
                {
                    Console.WriteLine($"Randevu bulunamadı - ID: {id}");
                    return NotFound("Randevu bulunamadı!");
                }
                
                Console.WriteLine($"Randevu bulundu - ID: {appointment.AppointmentId}");
                // Silme yerine status'u -1 yap (silindi olarak işaretle)
                appointment.AppointmentStatus = -1;
                _appointmentService.Update(appointment);
                Console.WriteLine("Randevu başarıyla silindi olarak işaretlendi!");
                
                return Ok("Randevunuz başarıyla silinmiştir!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteAppointmentById hatası: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest($"Randevu silinirken hata oluştu: {ex.Message}");
            }
        }

        [HttpPut("updateAppointment")]
        public IActionResult UpdateAppointment(AppointmentDto appointmentDto)
        {
            try
            {
                Console.WriteLine($"UpdateAppointment çağrıldı - AppointmentId: {appointmentDto.AppointmentId}");
                Console.WriteLine($"Gelen veri: DoctorId={appointmentDto.DoctorId}, Date={appointmentDto.AppointmentDate}, Time={appointmentDto.AppointmentTime}");

                // DTO'dan Appointment entity'sine mapping
                var appointment = _appointmentService.GetAppointment(appointmentDto.AppointmentId);
                if (appointment == null)
                {
                    Console.WriteLine($"Randevu bulunamadı - ID: {appointmentDto.AppointmentId}");
                    return NotFound("Randevu bulunamadı!");
                }

                Console.WriteLine($"Mevcut randevu bulundu - ID: {appointment.AppointmentId}");

                // Güncelleme
                appointment.DoctorId = appointmentDto.DoctorId;
                appointment.AppointmentDate = appointmentDto.AppointmentDate;
                appointment.AppointmentTime = appointmentDto.AppointmentTime;
                appointment.AppointmentStatus = appointmentDto.AppointmentStatus;

                Console.WriteLine("Randevu güncelleniyor...");
                _appointmentService.Update(appointment);
                Console.WriteLine("Randevu başarıyla güncellendi!");

                return Ok("Randevunuz başarıyla güncellenmiştir!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateAppointment hatası: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest($"Randevu güncellenirken hata oluştu: {ex.Message}");
            }
        }

    }
}
