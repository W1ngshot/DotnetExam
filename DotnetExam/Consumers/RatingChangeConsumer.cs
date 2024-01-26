using DotnetExam.Models;
using DotnetExam.Services;
using MassTransit;

namespace DotnetExam.Consumers;

public class RatingChangeConsumer(RatingService ratingService) : IConsumer<RatingChange>
{
    public async Task Consume(ConsumeContext<RatingChange> context)
    {
        await ChangeRating(context.Message.UserId, context.Message.Change);
    }

    private async Task ChangeRating(Guid userId, int ratingChange)
    {
        await ratingService.AddRatingAsync(userId, ratingChange);
    }
}