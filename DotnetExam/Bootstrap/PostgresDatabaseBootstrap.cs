using DotnetExam.Database.Postgres;
using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Models.Main;
using DotnetExam.Services.Configs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Bootstrap;

public static class PostgresDatabaseBootstrap
{
    public static IServiceCollection AddDatabaseWithIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredLength = PasswordConfig.MinimumLength;
                options.Password.RequireNonAlphanumeric = PasswordConfig.RequireNonAlphanumeric;
                options.Password.RequireLowercase = PasswordConfig.RequireLowercase;
                options.Password.RequireUppercase = PasswordConfig.RequireUppercase;
                options.Password.RequireDigit = PasswordConfig.RequireDigit;
            })
            .AddEntityFrameworkStores<ExamDbContext>();

        services.AddDbContext<IExamDbContext, ExamDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"));
        });

        services.AddDatabaseConfigurations();

        return services;
    }
}