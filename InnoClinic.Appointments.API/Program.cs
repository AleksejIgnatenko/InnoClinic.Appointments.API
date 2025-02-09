using System.Text;
using InnoClinic.Appointments.API.Middlewares;
using InnoClinic.Appointments.Application.MapperProfiles;
using InnoClinic.Appointments.Application.RabbitMQ;
using InnoClinic.Appointments.Application.Services;
using InnoClinic.Appointments.DataAccess.Context;
using InnoClinic.Appointments.DataAccess.Repositories;
using InnoClinic.Appointments.Infrastructure.Jwt;
using InnoClinic.Appointments.Infrastructure.RabbitMQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<InnoClinicAppointmentsDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<RabbitMQSetting>(
    builder.Configuration.GetSection("RabbitMQ"));

var jwtOptions = builder.Configuration.GetSection("JwtSettings").Get<JwtOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.SecretKey))
    };
});

builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();

builder.Services.AddScoped<IMedicalServiceRepository, MedicalServiceRepository>();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();

builder.Services.AddHostedService<RabbitMQListener>();

builder.Services.AddAutoMapper(typeof(MapperProfiles));

var app = builder.Build();

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

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:4000", "http://localhost:4001");
    x.WithMethods().AllowAnyMethod();
});

app.Run();
