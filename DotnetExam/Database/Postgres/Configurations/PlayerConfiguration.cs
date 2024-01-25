using DotnetExam.Database.Postgres.Configurations.Abstractions;
using DotnetExam.Models.Main;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetExam.Database.Postgres.Configurations;

public class PlayerConfiguration : DependencyInjectedEntityConfiguration<Player>
{
    public override void Configure(EntityTypeBuilder<Player> builder)
    {
        builder
            .HasOne(player => player.User)
            .WithMany()
            .HasForeignKey(player => player.UserId);

        builder
            .Property(player => player.Mark)
            .HasConversion<string>();
    }
}