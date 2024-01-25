using DotnetExam.Models.Enums;
using DotnetExam.Models.Main.Abstractions;

namespace DotnetExam.Models.Main;

public class Player : BaseEntity
{
    public AppUser User { get; set; } = null!;
    public required Guid UserId { get; set; }
    public required Mark Mark { get; set; }
}