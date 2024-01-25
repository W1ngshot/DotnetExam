using DotnetExam.Features.Game.Connect;
using DotnetExam.Features.Game.Disconnect;
using DotnetExam.Features.Game.Move;
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
        var response = await mediator.Send(new ConnectCommand(Guid.Parse(gameId)));
        
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        await Clients.Caller.SendGameInfo(response.HostName, response.OpponentName, response.State.ToString());
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
        
        //TODO проверка на существование комнаты
        await Clients.Group(gameId).SendMessage(message, user.UserName!);
    }
    
    public async Task MakeMove(int x, int y)
    {
        var result = await mediator.Send(
            new MoveCommand(userService.GetUserIdOrThrow(), x, y));

        await Clients.Group(result.GameId.ToString())
            .Move(result.X, result.Y, result.Mark.ToString(), result.State.ToString());
    }
}