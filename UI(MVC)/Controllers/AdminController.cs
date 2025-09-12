using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using UI_MVC_.Models;

namespace UI_MVC_.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> Login(AdminLoginDto dto)
        {
            var jsonData = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("Admin/loginAdmin", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Giriş başarılı!";
                    return RedirectToAction("Dashboard", "Admin");
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
        public async Task<IActionResult> Doctors()
        {
            try
            {
                var response = await _httpClient.GetAsync("Doctor/getAllDoctors");
                if (!response.IsSuccessStatusCode)
                {
                    return PartialView("_DoctorsPartial", new List<DoctorDto>());
                }
                var json = await response.Content.ReadAsStringAsync();
                var doctors = JsonConvert.DeserializeObject<List<DoctorDto>>(json);
                return PartialView("_DoctorsPartial", doctors);
            }
            catch (Exception ex)
            {
                return PartialView("_DoctorsPartial", new List<DoctorDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Patients()
        {
            try
            {
                var response = await _httpClient.GetAsync("Patient/getAllPatients");
                if (!response.IsSuccessStatusCode)
                {
                    return PartialView("_PatientsPartial", new List<PatientDto>());
                }
                var json = await response.Content.ReadAsStringAsync();
                var patients = JsonConvert.DeserializeObject<List<PatientDto>>(json);
                return PartialView("_PatientsPartial", patients);
            }
            catch (Exception ex)
            {
                return PartialView("_PatientsPartial", new List<PatientDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Appointments()
        {
            try
            {
                var response = await _httpClient.GetAsync("Admin/getAllAppointmentsWithDetails");
                if (!response.IsSuccessStatusCode)
                {
                    return PartialView("_AppointmentsPartial", new List<AppointmentDto>());
                }

                var json = await response.Content.ReadAsStringAsync();
                var appointments = JsonConvert.DeserializeObject<List<AppointmentDto>>(json);

                return PartialView("_AppointmentsPartial", appointments);
            }
            catch (Exception ex)
            {
                return PartialView("_AppointmentsPartial", new List<AppointmentDto>());
            }
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePatient(PatientDto patient)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(patient);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("Patient/updatePatient", content);

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
        public async Task<IActionResult> DeletePatient(int id)
        {
            try
            {
                // Body göndermeye gerek yok, id URL’de
                var response = await _httpClient.DeleteAsync($"Patient/deletePatient/{id}");

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
        public async Task<IActionResult> UpdateDoctor(DoctorDto doctor)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(doctor);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("Doctor/updateDoctor", content);

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
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                
                var response = await _httpClient.DeleteAsync($"Doctor/deleteDoctor/{id}");

                if (response.IsSuccessStatusCode)
                    return Json(new { success = true });

                return Json(new { success = false, message = "API'den hata döndü" });
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
