using HundeRally.Api.Data;
using HundeRally.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.CreateUser;

[RegisterService<Data>(LifeTime.Scoped)]
public sealed class Data
{
    readonly HundeRallyDbContext _context;

    public Data(HundeRallyDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<UserBase> CreateUserAsync(UserBase user)
    {
        var result = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> UserAlreadyExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}