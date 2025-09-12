using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("getAllDoctors")]
        public IActionResult AllDoctors() 
        {

            var doctors = _doctorService.GetAllDoctors();
            if (doctors == null)
            {
                return NotFound("Doktor bulunamadı...");
            }
            return Ok(doctors);
        }

        [HttpGet("getDoctorById/{id}")]
        public IActionResult GetDoctorById(int id)
        {
            var doctor = _doctorService.GetDoctor(id);
            if(doctor == null)
            {
                return NotFound("Belirtilen ID'de doktor bulunamadı...");
            }
            return Ok(doctor);
        }


        [HttpPost("loginDoctor")]
        public IActionResult Login([FromBody] DoctorLoginDto loginDto)
        {
            var doctor = _doctorService.Login(loginDto.DoctorEmail, loginDto.DoctorPassword);
            if(doctor == null)
            {
                return Unauthorized("Geçersiz email veya şifre!");
            }
            return Ok(doctor);
        }

        [HttpPost("registerDoctor")]
        public IActionResult Register([FromBody] DoctorRegisterDto registerDto)
        {
            // Aynı isim ve TC kimlik numarasına sahip doktor var mı kontrol et
            if (_doctorService.IsDoctorExists(registerDto.DoctorFullName, registerDto.DoctorTcNo))
            {
                return BadRequest("Böyle bir kişi zaten mevcut!");
            }

            _doctorService.Add(registerDto);
            return Ok("Doktor kaydı başarıyla oluşturuldu!");
        }

        [HttpPut("updateDoctor")]
        public IActionResult UpdateDoctor(DoctorDto doctorDto)
        {
            Console.WriteLine($"UpdateDoctor called with DoctorId: {doctorDto.DoctorId}");
            
            // Tüm doktorları listele
            var allDoctors = _doctorService.GetAllDoctors();
            Console.WriteLine($"All doctors in database: {allDoctors.Count}");
            foreach (var d in allDoctors)
            {
                Console.WriteLine($"Doctor ID: {d.DoctorId}, Name: {d.DoctorFullName}");
            }
            
            var doctor = _doctorService.GetDoctor(doctorDto.DoctorId);
            if (doctor == null)
            {
                Console.WriteLine($"Doctor not found with ID: {doctorDto.DoctorId}");
                return NotFound("Doktor bulunamadı!");
            }
            Console.WriteLine($"Doctor found: {doctor.DoctorFullName}");

            // DTO'dan Doctor entity'sine mapping
            doctor.DoctorFullName = doctorDto.DoctorFullName;
            doctor.DoctorTcNo = doctorDto.DoctorTcNo;
            doctor.DoctorEmail = doctorDto.DoctorEmail;
            doctor.DoctorPhoneNumber = doctorDto.DoctorPhoneNumber;
            doctor.DoctorSpecialization = doctorDto.DoctorSpecialization;
            
            // Şifre güncelleniyorsa
            if (!string.IsNullOrEmpty(doctorDto.DoctorPassword))
            {
                doctor.DoctorPassword = doctorDto.DoctorPassword;
            }

            _doctorService.Update(doctor);
            return Ok("Doktor kaydı başarıyla güncellendi!");
        }
        [HttpDelete("deleteDoctor/{id}")]
        public IActionResult DeleteDoctor(int id)
        {
            var doctor = _doctorService.GetDoctor(id);
            _doctorService.Delete(doctor);
            return Ok("Doktor kaydı başarıyla silinmiştir!");
        }

        [HttpPut("updatePassword")]
        public IActionResult UpdatePassword([FromBody] DoctorPasswordUpdateDto passwordData)
        {
            try
            {
                var doctor = _doctorService.GetDoctor(passwordData.DoctorId);
                if (doctor == null)
                {
                    return NotFound("Doktor bulunamadı!");
                }

                // Mevcut şifreyi kontrol et
                if (doctor.DoctorPassword != passwordData.CurrentPassword)
                {
                    return BadRequest("Mevcut şifre uyuşmuyor!");
                }

                // Yeni şifreyi güncelle
                doctor.DoctorPassword = passwordData.NewPassword;
                _doctorService.Update(doctor);

                return Ok("Şifre başarıyla güncellendi!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Şifre güncellenirken hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("getDoctorAppointments/{doctorId}")]
        public IActionResult GetDoctorAppointments(int doctorId)
        {
            var appointments = _doctorService.GetDoctorAppointments(doctorId);
            if (appointments == null || !appointments.Any())
            {
                return Ok(new List<AppointmentDto>());
            }
            return Ok(appointments);
        }
    }
}
