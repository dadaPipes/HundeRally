﻿using HundeRally.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HundeRally.Api.Data
{
    public class HundeRallyDbContext(DbContextOptions<HundeRallyDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Administrator> Admins { get; set; }
    }
}
