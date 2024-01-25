using DotnetExam.Database.Postgres.Configurations.Abstractions;
using DotnetExam.Database.Postgres.Extensions;
using DotnetExam.Models.Main;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace DotnetExam.Database.Postgres.Configurations;

public class AppUserConfiguration(IOptions<JsonOptions> jsonOptions) : DependencyInjectedEntityConfiguration<AppUser>
{
    private readonly JsonOptions _jsonOptions = jsonOptions.Value;

    public override void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.RefreshTokens)
            .HasJsonConversion<IReadOnlyList<RefreshToken>, List<RefreshToken>>(_jsonOptions.SerializerOptions);
    }
}