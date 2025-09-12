using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UI_MVC_.Models;

namespace UI_MVC_.Controllers
{
    public class PatientController : Controller
    {
        private readonly HttpClient _httpClient;

        public PatientController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5057/api/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(PatientLoginDto dto)
        {
            var jsonData = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("Patient/loginPatient", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var patient = JsonConvert.DeserializeObject<PatientDto>(responseContent);
                    
                    // Session'a hasta bilgilerini kaydet
                    HttpContext.Session.SetString("PatientId", patient.PatientId.ToString());
                    HttpContext.Session.SetString("PatientName", patient.PatientFullName ?? "");
                    
                    TempData["Success"] = "Giriş başarılı!";
                    return RedirectToAction("Dashboard", "Patient");
                }
                ViewBag.Error = "Email veya şifre hatalı!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "API'ye ulaşılamıyor: " + ex.Message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            // TempData'yı temizle
            TempData.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(PatientRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var jsonData = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("Patient/registerPatient", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
                    return RedirectToAction("Login", "Patient");
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                
                if (errorContent.Contains("Böyle bir kişi zaten mevcut"))
                {
                    ViewBag.Error = "Böyle bir kişi zaten mevcut! Lütfen farklı bilgiler girin.";
                }
                else
                {
                    ViewBag.Error = "Kayıt sırasında hata oluştu: " + errorContent;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "API'ye ulaşılamıyor: " + ex.Message;
            }
            return View(dto);
        }

        public IActionResult Dashboard()
        {
            var patientId = HttpContext.Session.GetString("PatientId");
            if (string.IsNullOrEmpty(patientId))
            {
                TempData["Error"] = "Lütfen önce giriş yapın!";
                return RedirectToAction("Login", "Patient");
            }
            return View();
        }

        public async Task<IActionResult> MyAppointments()
        {
            var patientId = HttpContext.Session.GetString("PatientId");
            if (string.IsNullOrEmpty(patientId))
            {
                TempData["Error"] = "Lütfen önce giriş yapın!";
                return RedirectToAction("Login", "Patient");
            }

            try
            {
                var response = await _httpClient.GetAsync($"Patient/getPatientAppointments/{patientId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var appointments = JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
                    return PartialView("_MyAppointmentsPartial", appointments);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Randevular yüklenirken hata oluştu: " + ex.Message;
            }
            return PartialView("_MyAppointmentsPartial", new List<AppointmentDto>());
        }

        public async Task<IActionResult> MyProfile()
        {
            var patientId = HttpContext.Session.GetString("PatientId");
            if (string.IsNullOrEmpty(patientId))
            {
                TempData["Error"] = "Lütfen önce giriş yapın!";
                return RedirectToAction("Login", "Patient");
            }

            try
            {
                var response = await _httpClient.GetAsync($"Patient/getPatientById/{patientId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var patient = JsonConvert.DeserializeObject<PatientDto>(json);
                    return PartialView("_MyProfilePartial", patient);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Profil bilgileri yüklenirken hata oluştu: " + ex.Message;
            }
            return PartialView("_MyProfilePartial", new PatientDto());
        }

        [HttpGet]
        public async Task<IActionResult> BookAppointment()
        {
            var patientId = HttpContext.Session.GetString("PatientId");
            if (string.IsNullOrEmpty(patientId))
            {
                TempData["Error"] = "Lütfen önce giriş yapın!";
                return RedirectToAction("Login", "Patient");
            }

            try
            {
                var response = await _httpClient.GetAsync("Doctor/getAllDoctors");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var doctors = JsonConvert.DeserializeObject<List<DoctorDto>>(json);
                    ViewBag.Doctors = doctors;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Doktorlar yüklenirken hata oluştu: " + ex.Message;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(AppointmentDto appointment)
        {
            var patientId = HttpContext.Session.GetString("PatientId");
            if (string.IsNullOrEmpty(patientId))
            {
                TempData["Error"] = "Lütfen önce giriş yapın!";
                return RedirectToAction("Login", "Patient");
            }

            appointment.PatientId = int.Parse(patientId);
            appointment.AppointmentStatus = 1; // Randevu durumunu otomatik olarak 1 (Aktif) -1 (iptal silinmiş)

            var jsonData = JsonConvert.SerializeObject(appointment);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("Appointment/addAppointment", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Randevunuz başarıyla oluşturuldu!";
                    return RedirectToAction("Dashboard", "Patient");
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                ViewBag.Error = "Randevu alınırken hata oluştu: " + errorContent;
            }
            catch (Exception ex)
            {
                ViewBag.Error = "API'ye ulaşılamıyor: " + ex.Message;
            }

            // Hata durumunda doktorları tekrar yükle
            try
            {
                var response = await _httpClient.GetAsync("Doctor/getAllDoctors");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var doctors = JsonConvert.DeserializeObject<List<DoctorDto>>(json);
                    ViewBag.Doctors = doctors;
                }
            }
            catch { }

            return View(appointment);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppointment([FromBody] AppointmentDto appointment)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(appointment);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("Appointment/updateAppointment", content);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "API'den hata döndü" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Appointment/deleteAppointment/{id}");

                if (response.IsSuccessStatusCode)
                    return Json(new { success = true });

                return Json(new { success = false, message = "API'den hata döndü" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromBody] PatientDto patient)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(patient);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("Patient/updatePatient", content);

                if (response.IsSuccessStatusCode)
                {
                    // Session'ı güncelle
                    HttpContext.Session.SetString("PatientName", patient.PatientFullName);
                    return Json(new { success = true });
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return Json(new { success = false, message = $"API Hatası: {response.StatusCode} - {errorContent}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordUpdateDto passwordData)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(passwordData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("Patient/updatePassword", content);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return Json(new { success = false, message = errorContent });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointmentDetails(int appointmentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Appointment/getAppointmentById/{appointmentId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var appointment = JsonConvert.DeserializeObject<AppointmentDto>(json);
                    return Json(new { success = true, appointment = appointment });
                }
                return Json(new { success = false, message = "Randevu bulunamadı" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            try
            {
                var response = await _httpClient.GetAsync("Doctor/getAllDoctors");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var doctors = JsonConvert.DeserializeObject<List<DoctorDto>>(json);
                    return Json(doctors);
                }
                return Json(new List<DoctorDto>());
            }
            catch (Exception ex)
            {
                return Json(new List<DoctorDto>());
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
