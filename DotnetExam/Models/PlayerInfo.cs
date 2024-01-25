using DotnetExam.Models.Enums;

namespace DotnetExam.Models;

public record PlayerInfo(Guid Id, string Username, int Rating, Mark Mark);