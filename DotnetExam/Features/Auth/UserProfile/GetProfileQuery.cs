using DotnetExam.Infrastructure.Mediator.Query;

namespace DotnetExam.Features.Auth.UserProfile;

public record GetProfileQuery(Guid UserId) : IQuery<GetProfileResponse>;