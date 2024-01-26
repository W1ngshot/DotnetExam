using DotnetExam.Models;
using DotnetExam.Services.Interfaces;
using MassTransit;

namespace DotnetExam.Services;

public class RatingChangeService(IPublishEndpoint publishEndpoint) : IRatingChangeService
{
    private const int WinPoints = 3;
    private const int LosePoints = -1;
    
    public async Task SendRatingChange(Guid winnerId, Guid loserId)
    {
        await publishEndpoint.Publish(new RatingChange(winnerId, WinPoints));
        await publishEndpoint.Publish(new RatingChange(loserId, LosePoints));
    }
}