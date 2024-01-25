using DotnetExam.Models.Main;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Database.Postgres.Interfaces;

public interface IExamDbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Game> Games { get; set; }

    public Task<bool> SaveEntitiesAsync();
}