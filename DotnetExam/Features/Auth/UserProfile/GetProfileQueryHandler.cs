using DotnetExam.Infrastructure.Mediator.Query;
using DotnetExam.Services;

namespace DotnetExam.Features.Auth.UserProfile;

public class GetProfileQueryHandler(RatingService ratingService) : IQueryHandler<GetProfileQuery, GetProfileResponse>
{
    public async Task<GetProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var ratingModel = await ratingService.GetUserInfoAsync(request.UserId);
        return new GetProfileResponse(ratingModel.UserId, ratingModel.Username, ratingModel.Rating);
    }
}