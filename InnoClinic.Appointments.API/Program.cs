using InnoClinic.Appointments.API.Extensions;
using InnoClinic.Appointments.API.Middlewares;
using InnoClinic.Appointments.Application.MapperProfiles;
using InnoClinic.Appointments.Application.RabbitMQ;
using InnoClinic.Appointments.Application.Services;
using InnoClinic.Appointments.DataAccess.Context;
using InnoClinic.Appointments.DataAccess.Repositories;
using InnoClinic.Appointments.Infrastructure.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .CreateSerilog();

builder.Services.AddControllers();
builder.Services.AddCustomCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<InnoClinicAppointmentsDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<RabbitMQSetting>(
    builder.Configuration.GetSection("RabbitMQ"));

// Load JWT settings
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddScoped<IAppointmentResultService, AppointmentResultService>();
builder.Services.AddScoped<IAppointmentResultRepository, AppointmentResultRepository>();

builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();

builder.Services.AddScoped<IMedicalServiceRepository, MedicalServiceRepository>();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();

builder.Services.AddHostedService<RabbitMQListener>();

builder.Services.AddAutoMapper(typeof(MapperProfiles));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var rabbitMQService = services.GetRequiredService<IRabbitMQService>();
    await rabbitMQService.CreateQueuesAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.UseCors("CorsPolicy");

app.Run();
