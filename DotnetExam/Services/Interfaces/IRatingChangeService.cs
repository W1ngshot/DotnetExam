namespace DotnetExam.Services.Interfaces;

public interface IRatingChangeService
{
    public Task SendRatingChange(Guid winnerId, Guid loserId);
}