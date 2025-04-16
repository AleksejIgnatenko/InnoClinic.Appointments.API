using InnoClinic.Appointments.Application.Services;

namespace InnoClinic.Appointments.TestSuiteNUnit.ServiceTests;

class JwtTokenServiceTests
{
    private readonly JwtTokenService _jwtTokenService;

    private string jwtToken;
    public JwtTokenServiceTests()
    {
        jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjM3MWNiMjcwLTA1NDgtNDY0Ny1iZTQ4LTA1NmM2MGYyZGFkYyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlBhdGllbnQiLCJleHAiOjE3NDQ2NjU2MDYsImlzcyI6Iklubm9DbGluaWNBdXRob3JpemVkSXNzdWVyIiwiYXVkIjoiSW5ub0NsaW5pY0F1dGhvcml6ZWRBdWRpZW5jZSJ9.X6rYEUn1La2fzHlVGgrCOkqtRR4PZzLyuILo2Y8zpbc";
        _jwtTokenService = new JwtTokenService();
    }

    [Test]
    public void GetAccountIdFromAccessToken_ValidToken_ReturnsAccountId()
    {
        // Act
        Guid accountId = _jwtTokenService.GetAccountIdFromAccessToken(jwtToken);

        // Assert
        Assert.IsNotNull(accountId);
        Assert.AreEqual(Guid.Parse("371cb270-0548-4647-be48-056c60f2dadc"), accountId);
    }

    [Test]
    public void GetAccountIdFromAccessToken_InvalidToken_ReturnsEmptyGuid()
    {
        // Arrange
        var invalidJwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJQYXRpZW50IiwiZXhwIjoxNzQ0NjY1NjA2LCJpc3MiOiJJbm5vQ2xpbmljQXV0aG9yaXplZElzc3VlciIsImF1ZCI6Iklubm9DbGluaWNBdXRob3JpemVkQXVkaWVuY2UifQ.0xui_3O4MPDuOJdoBRJDj0r4phPxkBfc6NCQyVSLGcY";

        // Act
        Guid accountId = _jwtTokenService.GetAccountIdFromAccessToken(invalidJwtToken);

        // Assert
        Assert.AreEqual(Guid.Empty, accountId);
    }
}
