using InnoClinic.Appointments.Core.Models.AppointmentModels;
using InnoClinic.Appointments.DataAccess.Repositories;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Text.Json;

namespace InnoClinic.Appointments.Application.Services;

public class SendNotificationAboutAppointmentService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly HttpClient _httpClient;

    public SendNotificationAboutAppointmentService(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
        _httpClient = new HttpClient();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(async state => await SendNotificationAboutAppointmentAsync(state), null, TimeSpan.Zero, TimeSpan.FromDays(1));
        return Task.CompletedTask;
    }

    private async Task SendNotificationAboutAppointmentAsync(object? state)
    {
        var tomorrow = DateTime.UtcNow.Date.AddDays(1);

        var formattedDateTomorrow = tomorrow.ToString("yyyy-MM-dd");

        var appointmentsTomorrow = await _appointmentRepository.GetByDateAsync(formattedDateTomorrow);

        if (appointmentsTomorrow.Count() > 0)
        {
            foreach (var appointmentEntity in appointmentsTomorrow)
            {
                var sendNotificationAboutAppointmentRequest = new SendNotificationAboutAppointmentRequest(
                    appointmentEntity.Patient.AccountId,
                    $"{appointmentEntity.Patient.FirstName} {appointmentEntity.Patient.LastName} {appointmentEntity.Patient.MiddleName}",
                    appointmentEntity.Date, 
                    appointmentEntity.Time, 
                    appointmentEntity.MedicalService.ServiceName,
                    $"{appointmentEntity.Doctor.FirstName} {appointmentEntity.Doctor.LastName} {appointmentEntity.Doctor.MiddleName}"
                );

                var json = JsonSerializer.Serialize(sendNotificationAboutAppointmentRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await _httpClient.PostAsync("http://innoclinic_notification_api:8080/api/Notification/send-notification-about-appointment", content);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}