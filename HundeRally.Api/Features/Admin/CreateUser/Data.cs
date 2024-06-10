using HundeRally.Api.Data;
using HundeRally.Api.Models;

namespace Admin.CreateUser;

[RegisterService<Data>(LifeTime.Scoped)]
public sealed class Data
{
    readonly HundeRallyDbContext _context;

    public Data(HundeRallyDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<User> AddUserAsync(User user)
    {
        var result = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return result.Entity;
    }
}
