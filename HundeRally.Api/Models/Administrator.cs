namespace HundeRally.Api.Models;

public class Administrator : UserBase
{
    public Administrator()
    {
        Roles.Add("Admin");
    }
}