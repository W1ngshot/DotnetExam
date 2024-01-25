using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Responses;

namespace DotnetExam.Features.Game.Disconnect;

public record DisconnectCommand(Guid UserId) : ICommand<SuccessResponse>;