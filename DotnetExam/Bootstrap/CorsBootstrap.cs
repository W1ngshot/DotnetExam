namespace DotnetExam.Bootstrap;

public static class CorsBootstrap
{
    public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app)
    {
        return app.UseCors(x =>
        {
            x.WithOrigins("http://localhost:5173", "http://localhost:3000 ")
                .AllowAnyHeader()
                .AllowCredentials()
                .AllowAnyMethod();
        });
    }
}