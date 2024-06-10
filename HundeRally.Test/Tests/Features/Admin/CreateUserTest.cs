using CreateUserEndpoint = Admin .CreateUser.Endpoint;
using CreateUserRequest  = Admin .CreateUser.Request;
using CreateUserResponse = Admin .CreateUser.Response;
using LoginEndpoint      = Public.Login.Endpoint;
using LoginRequest       = Public.Login.Request;
using LoginResponse      = Public.Login.Response;

namespace HundeRally.Tests.Features.Admin;

public class CreateUserTests(Sut App) : TestBase<Sut>
{
    [Fact, Priority(1)]
    public async Task Create_User_And_Add_To_Database_Fails_Without_Login()
    {
        // Arrange & Act: Attempt to create a user without authentication
        var createUserRsp = await App.Client.POSTAsync<CreateUserEndpoint, CreateUserRequest>(new()
        {
            Email    = "newuser@example.com",
            Password = "Password123",
            Name     = "New User",
            Roles    = ["DogHandler"]
        });

        // Assert: Check for unauthorized response
        createUserRsp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact, Priority(2)]
    public async Task Create_User_And_Add_To_Database_Fails_Without_Admin_Role()
    {
        // Arrange: Login as a non-admin user
        var (loginRsp, loginRes) = await App.Client.POSTAsync<LoginEndpoint, LoginRequest, LoginResponse>(new()
        {
            Email  = "Judge@example.com",
            Password = "Passw0rd!"
        });
        loginRsp.StatusCode.Should().Be(HttpStatusCode.OK);
        loginRes.Message.Should().Be("Welcome Judge");

        // Act: Attempt to create a user
        var createUserRsp = await App.Client.POSTAsync<CreateUserEndpoint, CreateUserRequest>(new()
        {
            Email    = "newuser@example.com",
            Password = "Password123",
            Name     = "New User"
        });

        // Assert: Check for unauthorized response
        createUserRsp.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact, Priority(3)]
    public async Task Create_User_And_Add_To_Database_Success_With_Admin_Role()
    {
        // Arrange: Login as an admin user
        var (loginRsp, loginRes) = await App.Client.POSTAsync<LoginEndpoint, LoginRequest, LoginResponse>(new()
        {
            Email    = "Admin@example.com",
            Password = "Passw0rd!"
        });
        loginRsp.StatusCode.Should().Be(HttpStatusCode.OK);
        loginRes.Message.Should().Be("Welcome Admin");

        // Act: Attempt to create a user
        var (createUserRsp, createUserRes) = await App.Client.POSTAsync<CreateUserEndpoint, CreateUserRequest, CreateUserResponse>(new()
        {
            Email    = "newuser@example.com",
            Password = "Password123",
            Name     = "New User",
            Roles    = ["DogHandler"]
        });

        // Assert: Check for successful response and message from user creation
        createUserRsp.StatusCode.Should().Be(HttpStatusCode.OK);
        createUserRes.Message.Should().Be("User successfully created");
    }
}
