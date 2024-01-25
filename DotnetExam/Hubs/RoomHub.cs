using DotnetExam.Features.Game.Disconnect;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Models.Main;
using DotnetExam.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace DotnetExam.Hubs;

public class RoomHub(IUserService userService, IMediator mediator, UserManager<AppUser> userManager) : Hub<IRoom>
{
    public async Task JoinGame(string gameId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = userService.GetUserIdOrThrow();
        var result = await mediator.Send(new DisconnectCommand(userId));

        if (result is null)
        {
            return;
        }

        await Clients.Group(result.GameId.ToString())
            .PlayerDisconnected(result.State.ToString(), result.WinnerId.ToString());
    }

    public async Task SendMessage(string message, string gameId)
    {
        var user = await userManager.FindByIdAsync(userService.GetUserIdOrThrow().ToString())
                   ?? throw new NotFoundException<AppUser>();
        
        await Clients.Group(gameId).SendMessage(message, user.UserName!);
    }
}