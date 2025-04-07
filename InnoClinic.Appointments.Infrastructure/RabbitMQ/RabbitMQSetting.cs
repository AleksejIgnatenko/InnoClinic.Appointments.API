namespace InnoClinic.Appointments.Infrastructure.RabbitMQ;

/// <summary>
/// Represents the RabbitMQ connection settings.
/// </summary>
public class RabbitMQSetting
{
    public string HostName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}