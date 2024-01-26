using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models;
using DotnetExam.Models.Events;
using DotnetExam.Services;
using DotnetExam.Services.Interfaces;

namespace DotnetExam.Features.Game.SendMessage;

public class SendMessageCommandHandler(IEventSenderService eventSenderService, RatingService ratingService)
    : ICommandHandler<SendMessageCommand, SendMessageResponse>
{
    public async Task<SendMessageResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var sender = await ratingService.GetUserInfoAsync(request.UserId);
        var senderInfo = new UserInfo(sender.UserId, sender.Username, sender.Rating);

        await eventSenderService.SendMessageEvent(
            new SendMessageEvent(request.GameId, request.IdempotenceKey, request.Message, senderInfo));
        return new SendMessageResponse(request.GameId, request.IdempotenceKey, request.Message, senderInfo);
    }
}