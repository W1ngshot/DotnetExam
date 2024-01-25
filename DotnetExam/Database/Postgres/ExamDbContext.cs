using DotnetExam.Database.Postgres.Configurations.Abstractions;
using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Models.Main;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Database.Postgres;

public class ExamDbContext(
    DbContextOptions<ExamDbContext> options,
    IEnumerable<DependencyInjectedEntityConfiguration> configurations)
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options), IExamDbContext
{
    public DbSet<AppUser> IdentityUsers => Set<AppUser>();
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var configuration in configurations)
            configuration.Configure(builder);
    }
    public async Task<bool> SaveEntitiesAsync()
    {
        await base.SaveChangesAsync();
        return true;
    }
}
