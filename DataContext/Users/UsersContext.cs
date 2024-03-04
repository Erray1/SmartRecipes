using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartRecipes.DataContext.Users.Models;

namespace SmartRecipes.DataContext.Users;

public class  UsersContext : IdentityDbContext<User>
{
    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
    {
    }
    public UsersContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        base.OnConfiguring(optionsBuilder);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}
