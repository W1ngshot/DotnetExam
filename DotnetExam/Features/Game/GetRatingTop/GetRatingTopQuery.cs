using DotnetExam.Infrastructure.Mediator.Query;

namespace DotnetExam.Features.Game.GetRatingTop;

public record GetRatingTopQuery(int Count) : IQuery<GetRatingTopResponse>;