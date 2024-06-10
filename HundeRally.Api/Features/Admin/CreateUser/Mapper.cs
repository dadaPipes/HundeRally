using HundeRally.Api.Models;

namespace Admin.CreateUser;

public class Mapper : Mapper<Request, Response, UserBase>
{
    public override UserBase ToEntity(Request req)
        => new()
        {
            Name = req.Name,
            Email = req.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            Roles = req.Roles
        };
}