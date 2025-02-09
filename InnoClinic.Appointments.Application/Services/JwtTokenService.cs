using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InnoClinic.Appointments.Application.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        public Guid GetAccountIdFromAccessToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);
            var accountId = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            return Guid.Parse(accountId);
        }
    }
}
