using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {

        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("getAllPatients")]
        public IActionResult AllPatients()
        {

            var patients = _patientService.GetAllPatients();
            if (patients == null)
            {
                return NotFound("Hasta bulunamadı...");
            }
            return Ok(patients);
        }

        [HttpGet("getPatientById/{id}")]
        public IActionResult GetPatientById(int id)
        {
            var patient = _patientService.GetPatient(id);
            if (patient == null)
            {
                return NotFound("Belirtilen ID'de hasta bulunamadı...");
            }
            return Ok(patient);
        }



        [HttpPost("loginPatient")]
        public IActionResult Login([FromBody] PatientLoginDto loginDto)
        {
            var patient = _patientService.Login(loginDto.PatientEmail, loginDto.PatientPassword);
            if (patient == null)
            {
                return Unauthorized("Geçersiz email veya şifre!");
            }
            return Ok(patient);
        }

        [HttpPost("registerPatient")]
        public IActionResult Register([FromBody] PatientRegisterDto registerDto)
        {
            // Aynı isim ve TC kimlik numarasına sahip hasta var mı kontrol et
            if (_patientService.IsPatientExists(registerDto.PatientFullName, registerDto.PatientTcNo))
            {
                return BadRequest("Böyle bir kişi zaten mevcut!");
            }

            _patientService.Add(registerDto);
            return Ok("Hasta kaydı başarıyla oluşturuldu!");
        }

        [HttpPut("updatePatient")]
        public IActionResult UpdatePatient([FromBody] PatientDto patientDto)
        {
            var patient = _patientService.GetPatient(patientDto.PatientId);
            if (patient == null)
            {
                return NotFound("Hasta bulunamadı!");
            }

            // DTO'dan Patient entity'sine mapping
            patient.PatientFullName = patientDto.PatientFullName;
            patient.PatientTcNo = patientDto.PatientTcNo;
            patient.PatientEmail = patientDto.PatientEmail;
            patient.PatientPhoneNumber = patientDto.PatientPhoneNumber;
            patient.PatientAge = patientDto.PatientAge;
            patient.PatientGender = patientDto.PatientGender;
            
            // Şifre güncelleniyorsa
            if (!string.IsNullOrEmpty(patientDto.PatientPassword))
            {
                patient.PatientPassword = patientDto.PatientPassword;
            }
            // Şifre boşsa, mevcut şifreyi koru (hiçbir şey yapma)

            _patientService.Update(patient);
            return Ok("Hasta kaydı başarıyla güncellendi!");
        }

        [HttpPut("updatePassword")]
        public IActionResult UpdatePassword([FromBody] PasswordUpdateDto passwordData)
        {
            try
            {
                var patient = _patientService.GetPatient(passwordData.PatientId);
                if (patient == null)
                {
                    return NotFound("Hasta bulunamadı!");
                }

                // Mevcut şifreyi kontrol et
                if (patient.PatientPassword != passwordData.CurrentPassword)
                {
                    return BadRequest("Mevcut şifre uyuşmuyor!");
                }

                // Yeni şifreyi güncelle
                patient.PatientPassword = passwordData.NewPassword;
                _patientService.Update(patient);

                return Ok("Şifre başarıyla güncellendi!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Şifre güncellenirken hata oluştu: {ex.Message}");
            }
        }
        [HttpDelete("deletePatient/{id}")]
        public IActionResult DeletePattient(int id)
        {
            var patient = _patientService.GetPatient(id);
            _patientService.Delete(patient);
            return Ok("Hasta kaydı başarıyla silinmiştir!");
        }

        [HttpGet("getPatientAppointments/{patientId}")]
        public IActionResult GetPatientAppointments(int patientId)
        {
            var appointments = _patientService.GetPatientAppointments(patientId);
            if (appointments == null || !appointments.Any())
            {
                return Ok(new List<AppointmentDto>());
            }
            return Ok(appointments);
        }
    }
}
