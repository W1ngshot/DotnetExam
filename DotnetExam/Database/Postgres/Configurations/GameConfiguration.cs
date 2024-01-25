using DotnetExam.Database.Postgres.Configurations.Abstractions;
using DotnetExam.Infrastructure;
using DotnetExam.Models.Main;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetExam.Database.Postgres.Configurations;

public class GameConfiguration : DependencyInjectedEntityConfiguration<Game>
{
    public override void Configure(EntityTypeBuilder<Game> builder)
    {
        builder
            .HasOne(game => game.Host)
            .WithOne()
            .HasForeignKey<Game>("HostId");
        
        builder
            .Property(game => game.State)
            .HasConversion<string>();

        builder
            .Property(game => game.Board)
            .HasConversion<BoardConverter>();
    }
}