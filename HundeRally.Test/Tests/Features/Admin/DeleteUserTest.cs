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

        [Fact, Priority(2)]
        public async Task Delete_User_Fails_Without_Authorization()
        {
            // Arrange: Assume the user with ID 100 exists but the client is not authorized
            var deleteUserRsp = await App.Client.DELETEAsync<DeleteUserEndpoint, DeleteUserRequest>(new()
            {
                Id = 100
            });

            // Assert: Check for unauthorized response
            deleteUserRsp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        /*
        [Fact, Priority(3)]
        public async Task Delete_User_Successfully()
        {
            // Arrange: First, create a user to delete
            (HttpResponseMessage createUserRsp, CreateUserResponse createUserRes) = await App.Client.POSTAsync<CreateUserEndpoint, CreateUserRequest, CreateUserResponse>(new()
            {
                Email = "deletable@example.com",
                Password = "Password123",
                Name = "Deletable User",
                Roles = ["DogHandler"]
            });
            createUserRsp.StatusCode.Should().Be(HttpStatusCode.OK);

            // Ensure the response is read correctly
            var createdUser = createUserRes;

            // Act: Now delete the user
            var deleteUserRsp = await App.Client.DELETEAsync<DeleteUserEndpoint, DeleteUserRequest>(new()
            {
                Id = createdUser.Id // Use the ID from the created user
            });

            // Assert: Check for successful deletion response
            deleteUserRsp.StatusCode.Should().Be(HttpStatusCode.OK);
            var response = await deleteUserRsp.Content.ReadAsAsync<DeleteUserResponse>();
            response.Message.Should().Be("User successfully deleted");
        }*/
    }
}
