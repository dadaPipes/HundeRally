using LoginEndpoint = Public.Login.Endpoint;
using LoginRequest  = Public.Login.Request;
using LoginResponse = Public.Login.Response;

namespace HundeRally.Tests.Features.Public;

public class LoginTests(Sut App) : TestBase<Sut>
{
    [Fact, Priority(1)]
    public async Task Invalid_User_Input()
    {
        var (rsp, res) = await App.Client.POSTAsync<LoginEndpoint, LoginRequest, ErrorResponse>(new()
        {
            Email    = "x",
            Password = "y"
        });
        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Errors.Count.Should().Be(2);
        res.Errors.Keys.Should().Contain("email");
        res.Errors.Keys.Should().Contain("password");
    }

    [Fact, Priority(2)]
    public async Task User_Not_Found_Correct()
    {
        var (rsp, res) = await App.Client.POSTAsync<LoginEndpoint, LoginRequest, ErrorResponse>(new()
        {
            Email    = "nonexistent@example.com",
            Password = "anyPassword"
        });
        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Errors.Count.Should().Be(1);
        res.Errors.Keys.Should().Contain("generalErrors"); // Check for the generic error key
        res.Errors["generalErrors"].Should().Contain("Invalid login details."); // Check the content of the error
    }

    [Fact, Priority(3)]
    public async Task Valid_User_Input()
    {
        var (rsp, res) = await App.Client.POSTAsync<LoginEndpoint, LoginRequest, LoginResponse>(new()
        {
            Email    = "Judge@example.com",
            Password = "Passw0rd!"
        });
        rsp.IsSuccessStatusCode.Should().BeTrue();
        res.Message.Should().Be("Welcome Judge");
    }
}