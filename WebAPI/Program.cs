using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



builder.Services.AddControllers();

builder.Services.AddScoped<IAdminService, AdminManager>();
builder.Services.AddScoped<IAppointmentService, AppointmentManager>();
builder.Services.AddScoped<IDoctorService, DoctorManager>();
builder.Services.AddScoped<IPatientService, PatientManager>();

builder.Services.AddScoped<IAdminDal, AdminDal>();
builder.Services.AddScoped<IAppointmentDal, AppointmentDal>();
builder.Services.AddScoped<IDoctorDal, DoctorDal>();
builder.Services.AddScoped<IPatientDal, PatientDal>();





builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Swagger artýk root (/)'ta
    });
}


app.UseCors("AllowAll");
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
