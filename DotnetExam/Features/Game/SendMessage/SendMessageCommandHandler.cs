using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models;
using DotnetExam.Models.Events;
using DotnetExam.Models.Main;
using DotnetExam.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DotnetExam.Features.Game.SendMessage;

public class SendMessageCommandHandler(IEventSenderService eventSenderService, UserManager<AppUser> userManager)
    : ICommandHandler<SendMessageCommand, SendMessageResponse>
{
    public async Task<SendMessageResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var sender = await userManager.FindByIdAsync(request.UserId.ToString())
                     ?? throw new NotFoundException<AppUser>();
        var senderInfo = new ChatUser(sender.Id, sender.UserName!, 0);

        await eventSenderService.SendMessageEvent(
            new SendMessageEvent(request.GameId, request.IdempotenceKey, request.Message, senderInfo));
        return new SendMessageResponse(request.GameId, request.IdempotenceKey, request.Message, senderInfo);
    }
}