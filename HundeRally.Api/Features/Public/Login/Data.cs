using HundeRally.Api.Data;
using HundeRally.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Public.Login;

[RegisterService<Data>(LifeTime.Scoped)]
public sealed class Data
{
    readonly HundeRallyDbContext _context;

    public Data(HundeRallyDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<UserBase> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}