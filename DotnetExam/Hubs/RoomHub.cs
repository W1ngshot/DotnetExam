using DotnetExam.Features.Game.Disconnect;
using DotnetExam.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DotnetExam.Hubs;

[Authorize]
public class RoomHub(IUserService userService, IMediator mediator) : Hub<IRoom>
{
    public async Task JoinGame(string gameId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = userService.GetUserIdOrThrow();
        // await mediator.Send(new DisconnectCommand(userId));
    }
}