using DotnetExam.Models;

namespace DotnetExam.Features.Game.GetRatingTop;

public record GetRatingTopResponse(List<UserInfo> Players);