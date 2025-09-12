using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IAppointmentService _appointmentService;

        public AdminController(IAdminService adminService, IAppointmentService appointmentService)
        {
            _adminService = adminService;
            _appointmentService = appointmentService;
        }

        [HttpPost("loginAdmin")]
        public IActionResult Login([FromBody] AdminLoginDto adminLoginDto)
        {
            var loggedAdmin = _adminService.GetByEmailAndPassword(adminLoginDto.AdminEmail, adminLoginDto.AdminPassword);
            if (loggedAdmin == null)
            {
                return Unauthorized("Geçersiz email veya şifre!");
            }
            return Ok("Giriş Başarılı!");
        }

        [HttpGet("getAllAppointmentsWithDetails")]
        public IActionResult GetAllAppointmentsWithDetails()
        {
            var appointments = _appointmentService.GetAllAppointmentsWithDetails();
            if(appointments == null || !appointments.Any())
            {
                return NotFound("Randevu bulunamadı");
            }

            return Ok(appointments);
        }

        
    }
}
