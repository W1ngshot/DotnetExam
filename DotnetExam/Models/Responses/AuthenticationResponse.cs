using DotnetExam.Models.Main;

namespace DotnetExam.Models.Responses;

public record AuthenticationResponse(string Token, RefreshToken RefreshToken);