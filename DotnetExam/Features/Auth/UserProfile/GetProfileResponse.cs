namespace DotnetExam.Features.Auth.UserProfile;

public record GetProfileResponse(Guid UserId, string Username, int Rating);