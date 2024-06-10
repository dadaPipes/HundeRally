using LogoutEndpoint = Public.Logout.Endpoint;
using LoginEndpoint  = Public.Login.Endpoint;
using LoginRequest   = Public.Login.Request;
using LoginResponse  = Public.Login.Response;

namespace HundeRally.Tests.Features.Public;

public class LogoutTests(Sut App) : TestBase<Sut>
{
    [Fact, Priority(1)]
    public async Task Logout_Fails_Without_Authentication()
    {
        // Arrange & Act: Attempt to logout without prior login
        var logoutRsp = await App.Client.POSTAsync<LogoutEndpoint, EmptyRequest>();

        // Assert: Check for unauthorized response
        logoutRsp.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact, Priority(2)]
    public async Task Logout_Success_With_Authentication()
    {
        // Arrange: Login to simulate an authenticated user
        var loginRsp = await App.Client.POSTAsync<LoginEndpoint, LoginRequest, LoginResponse>(new()
            {
                Email    = "Judge@example.com",
                Password = "Passw0rd!"
            });

        // Ensure login was successful
        Assert.True(loginRsp.Response.IsSuccessStatusCode);

        // Act: Attempt to logout
        var logoutRsp = await App.Client.POSTAsync<LogoutEndpoint, EmptyRequest>();

        // Assert: Check for successful logout
        logoutRsp.Response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}