namespace HundeRally.Api.Models;

public class Administrator : User
{
    public Administrator()
    {
        Roles.Add("Admin");
    }
}