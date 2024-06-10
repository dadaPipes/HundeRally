using Public.Login;
using HundeRally.Api.Models;

namespace HundeRally.Api.Features.Public.Login;

public class Mapper : Mapper<Request, Response, UserBase>
{
    public override UserBase ToEntity(Request req)
        => new()
        {
            Email = req.Email,
            PasswordHash = req.Password
        };
}