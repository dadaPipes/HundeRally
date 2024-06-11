using HundeRally.Api.Data;
using HundeRally.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HundeRally.Api.Features.Admin.DeleteUser;

[RegisterService<Data>(LifeTime.Scoped)]
public sealed class Data
{
    private readonly HundeRallyDbContext _context;

    public Data(HundeRallyDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<UserBase> GetUserByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task DeleteUserByIdAsync(UserBase user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
