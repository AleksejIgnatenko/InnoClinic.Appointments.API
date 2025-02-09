
namespace InnoClinic.Appointments.Application.Services
{
    public interface IJwtTokenService
    {
        Guid GetAccountIdFromAccessToken(string jwtToken);
    }
}