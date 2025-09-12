const apiBase = "https://localhost:7066"; // senin backend portun


// Login kontrolü
if(localStorage.getItem("isAdminLogged") !== "true"){
  window.location.href = "index.html";
}

// Menü eventleri
document.querySelectorAll(".menu-btn").forEach(btn=>{
  btn.addEventListener("click", ()=>{
    const target = btn.dataset.target;
    loadData(target);
  });
});

// Data yükleme
async function loadData(type){
  let url = "";
    if (type === "doktorlar") url = `${apiBase}/api/Doctor/getAllDoctors`;
    if (type === "hastalar") url = `${apiBase}/api/Patient/getAllPatients`;
    if (type === "randevular") url = `${apiBase}/api/Appointment/getAllAppointments`;


  document.getElementById("sectionTitle").innerText = type.toUpperCase();

  try {
    const res = await fetch(url);
    if(!res.ok) throw new Error("API hatası");
    const data = await res.json();
    renderTable(type, data);
  } catch(err){
    console.error(err);
    alert("Veri yüklenemedi: " + err.message);
  }
}

// Tablo render
function renderTable(type, data){
  let html = "<table><thead><tr>";
  
  if(type === "doktorlar"){
    html += "<th>ID</th><th>Ad Soyad</th><th>Uzmanlık</th><th>Email</th>";
    html += "</tr></thead><tbody>";
    data.forEach(d=>{
        html += `<tr><td>${d.doctorId}</td><td>${d.doctorFullName}</td><td>${d.doctorSpecialization}</td><td>${d.doctorEmail}</td></tr>`;
    });
  }
  else if(type === "hastalar"){
    html += "<th>ID</th><th>Ad Soyad</th><th>Email</th><th>Telefon</th>";
    html += "</tr></thead><tbody>";
    data.forEach(p=>{
        html += `<tr><td>${p.patientId}</td><td>${p.patientFullName}</td><td>${p.patientEmail}</td><td>${p.patientPhoneNumber ?? "-"}</td></tr>`;
    });
  }
  else if(type === "randevular"){
    html += "<th>ID</th><th>Hasta</th><th>Doktor</th><th>Tarih</th><th>Durum</th>";
    html += "</tr></thead><tbody>";
    data.forEach(r=>{
        html += `<tr><td>${r.appointmentId}</td><td>${r.patientFullName}</td><td>${r.doctorFullName}</td><td>${r.appointmentDate}</td><td>${r.appointmentStatus}</td></tr>`;
    });
  }

  html += "</tbody></table>";
  document.getElementById("dataContainer").innerHTML = html;
}
