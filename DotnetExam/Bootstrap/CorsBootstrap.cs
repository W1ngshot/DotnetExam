namespace DotnetExam.Bootstrap;

public static class CorsBootstrap
{
    public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app, IConfiguration configuration)
    {
        return app.UseCors(x =>
        {
            x.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    }
}