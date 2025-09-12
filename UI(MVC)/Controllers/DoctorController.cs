using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UI_MVC_.Models;

namespace UI_MVC_.Controllers
{
    public class DoctorController : Controller
    {
        private readonly HttpClient _httpClient;

        public DoctorController(IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> Login(DoctorLoginDto dto)
        {
            var jsonData = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("Doctor/loginDoctor", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var doctor = JsonConvert.DeserializeObject<DoctorDto>(responseContent);
                    
                    // Session'a doktor bilgilerini kaydet
                    HttpContext.Session.SetString("DoctorId", doctor.DoctorId.ToString());
                    HttpContext.Session.SetString("DoctorName", doctor.DoctorFullName ?? "");
                    
                    TempData["Success"] = "Giriş başarılı!";
                    return RedirectToAction("Dashboard", "Doctor");
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
        public async Task<IActionResult> Register(DoctorRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var jsonData = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("Doctor/registerDoctor", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
                    return RedirectToAction("Login", "Doctor");
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                
                // Eğer "Böyle bir kişi zaten mevcut!" hatası ise özel mesaj göster
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
            var doctorId = HttpContext.Session.GetString("DoctorId");
            if (string.IsNullOrEmpty(doctorId))
            {
                TempData["Error"] = "Lütfen önce giriş yapın!";
                return RedirectToAction("Login", "Doctor");
            }
            return View();
        }

        public async Task<IActionResult> MyAppointments()
        {
            var doctorId = HttpContext.Session.GetString("DoctorId");
            if (string.IsNullOrEmpty(doctorId))
            {
                TempData["Error"] = "Lütfen önce giriş yapın!";
                return RedirectToAction("Login", "Doctor");
            }

            try
            {
                var response = await _httpClient.GetAsync($"Doctor/getDoctorAppointments/{doctorId}");
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
            var doctorId = HttpContext.Session.GetString("DoctorId");
            if (string.IsNullOrEmpty(doctorId))
            {
                TempData["Error"] = "Lütfen önce giriş yapın!";
                return RedirectToAction("Login", "Doctor");
            }

            try
            {
                var response = await _httpClient.GetAsync($"Doctor/getDoctorById/{doctorId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var doctor = JsonConvert.DeserializeObject<DoctorDto>(json);
                    return PartialView("_MyProfilePartial", doctor);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Profil bilgileri yüklenirken hata oluştu: " + ex.Message;
            }
            return PartialView("_MyProfilePartial", new DoctorDto());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromBody] DoctorDto doctor)
        {
            try
            {
                Console.WriteLine($"UpdateProfile called with DoctorId: {doctor.DoctorId}");
                Console.WriteLine($"UpdateProfile Doctor Data: {JsonConvert.SerializeObject(doctor)}");
                
                var jsonData = JsonConvert.SerializeObject(doctor);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("Doctor/updateDoctor", content);

                if (response.IsSuccessStatusCode)
                {
                    // Session'ı güncelle
                    HttpContext.Session.SetString("DoctorName", doctor.DoctorFullName ?? "");
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
        public async Task<IActionResult> UpdatePassword([FromBody] DoctorPasswordUpdateDto passwordData)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(passwordData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("Doctor/updatePassword", content);

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

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
