using DotnetExam.Infrastructure.Mediator.Query;
using DotnetExam.Models;
using DotnetExam.Services;

namespace DotnetExam.Features.Game.GetRatingTop;

public class GetRatingTopQueryHandler(RatingService ratingService)
    : IQueryHandler<GetRatingTopQuery, GetRatingTopResponse>
{
    public async Task<GetRatingTopResponse> Handle(GetRatingTopQuery request, CancellationToken cancellationToken)
    {
        var ratingModels = await ratingService.GetTopRatingUsers(request.Count);
        return new GetRatingTopResponse(ratingModels.Select(model =>
            new UserInfo(model.UserId, model.Username, model.Rating)).ToList());
    }
}