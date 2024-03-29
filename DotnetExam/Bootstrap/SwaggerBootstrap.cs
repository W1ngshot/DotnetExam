﻿using Microsoft.OpenApi.Models;

namespace DotnetExam.Bootstrap;

public static class SwaggerBootstrap
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc($"v{configuration["ApiData:Version"]}", new OpenApiInfo { 
                Title = "My API", 
                Version = $"v{configuration["ApiData:Version"]}" 
            });
            c.AddSecurityDefinition(
                "bearerAuth",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { 
                    new OpenApiSecurityScheme 
                    { 
                        Reference = new OpenApiReference 
                        { 
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth" 
                        } 
                    },
                    Array.Empty<string>()
                } 
            });
        });
        
        return services;
    }
}