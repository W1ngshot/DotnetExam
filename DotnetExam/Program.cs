using DotnetExam.Bootstrap;
using DotnetExam.Hubs;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Routing;
using DotnetExam.Middleware;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services
    .AddDatabaseWithIdentity(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddJwtAuthentication(builder.Configuration)
    .AddCustomSwagger(builder.Configuration)
    .AddAuthorization();

builder.Services
    .AddHelperServices(builder.Configuration)
    .AddFluentValidation()
    .AddMediatrConfiguration()
    .AddMassTransitRabbit(builder.Configuration)
    .AddCors()
    .AddSignalR();

var app = builder.Build();
await app.TryMigrateDatabaseAsync();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCorsConfiguration(builder.Configuration);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseCustomEndpoints();
app.MapHub<RoomHub>("/api/room");

app.Run();