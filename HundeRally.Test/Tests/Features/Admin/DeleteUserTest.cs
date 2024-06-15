using LoginEndpoint = Public.Login.Endpoint;
using LoginRequest = Public.Login.Request;
using LoginResponse = Public.Login.Response;
using CreateUserEndpoint = Admin.CreateUser.Endpoint;
using CreateUserRequest = Admin.CreateUser.Request;
using CreateUserResponse = Admin.CreateUser.Response;

using DeleteUserEndpoint = HundeRally.Api.Features.Admin.DeleteUser.Endpoint;
using DeleteUserRequest = HundeRally.Api.Features.Admin.DeleteUser.Request;
using DeleteUserResponse = HundeRally.Api.Features.Admin.DeleteUser.Response;

namespace HundeRally.Tests.Features.Admin
{
    public class DeleteUserTests(Sut App) : TestBase<Sut>
    {
        [Fact, Priority(1)]
        public async Task Delete_User_Fails_Without_Authorization()
        {
            // Arrange: Assume the user with ID 100 exists but the client is not authorized
            var (deleteUserRsp, _) = await App.Client.DELETEAsync<DeleteUserEndpoint, DeleteUserRequest, DeleteUserResponse>(new()
            {
                Id = 100
            });

            // Assert: Check for unauthorized response
            deleteUserRsp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact, Priority(2)]
        public async Task Delete_User_Fails_When_User_Does_Not_Exist()
        {
            // Arrange: Login as Admin
            var (loginRsp, loginRes) = await App.Client.POSTAsync<LoginEndpoint, LoginRequest, LoginResponse>(new()
            {
                Email = "Admin@example.com",
                Password = "Passw0rd!"
            });
            loginRsp.StatusCode.Should().Be(HttpStatusCode.OK);
            loginRes.Message.Should().Be("Welcome Admin");

            // Act: Assume the user with ID 999 does not exist
            var deleteUserRsp = await App.Client.DELETEAsync<DeleteUserEndpoint, DeleteUserRequest>(new()
            {
                Id = 999
            });

            // Assert: Check for not found response
            deleteUserRsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact, Priority(3)]
        public async Task Delete_User_Successfully_As_Admin()
        {
            // Arrange: Login as Admin
            var (loginRsp, loginRes) = await App.Client.POSTAsync<LoginEndpoint, LoginRequest, LoginResponse>(new()
            {
                Email = "Admin@example.com",
                Password = "Passw0rd!"
            });
            loginRsp.StatusCode.Should().Be(HttpStatusCode.OK);
            loginRes.Message.Should().Be("Welcome Admin");

            // Act: Delete user using the ID from the database
            var (deleteUserRsp, deleteRes) = await App.Client.DELETEAsync<DeleteUserEndpoint, DeleteUserRequest, DeleteUserResponse>(new()
            {
                Id = 3 // Correctly use the ID from the createUserRes object
            });

            // Assert: Check for successful deletion response
            deleteUserRsp.StatusCode.Should().Be(HttpStatusCode.OK);
            deleteRes.Message.Should().Be("User succesfully deleted");
        }

    }
}
