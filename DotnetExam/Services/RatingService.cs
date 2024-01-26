using DotnetExam.Models.Main;
using DotnetExam.Services.Configs;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace DotnetExam.Services;

public class RatingService
{
    private readonly IMongoCollection<RatingModel> _collection;

    public RatingService(IOptions<MongoDbConfig> settings)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<RatingModel>(settings.Value.CollectionName);
    }

    public async Task CreateUserRatingAsync(Guid userId, string username)
    {
        var ratingModel = new RatingModel
        {
            UserId = userId,
            Username = username,
            Rating = 0
        };
        await _collection.InsertOneAsync(ratingModel);
    }

    public async Task AddRatingAsync(Guid userId, int ratingChange)
    {
        var ratingModel = await _collection.Find(model => model.UserId == userId).FirstOrDefaultAsync();
        ratingModel.Rating += ratingChange;
        await _collection.ReplaceOneAsync(model => model.UserId == userId, ratingModel);
    }

    public async Task<RatingModel> GetUserInfoAsync(Guid userId) =>
        await _collection.Find(model => model.UserId == userId).FirstOrDefaultAsync();

    public async Task<int> GetUserRatingAsync(Guid userId) =>
        (await _collection.Find(model => model.UserId == userId).FirstOrDefaultAsync()).Rating;

    public async Task<List<RatingModel>> GetUserListRatingAsync(List<Guid> userIds) => 
        await _collection.Find(model => userIds.Any(x => x == model.UserId)).ToListAsync();

    public async Task<List<RatingModel>> GetTopRatingUsers(int count) =>
        await _collection.Find(_ => true)
            .SortByDescending(model => model.Rating)
            .Limit(count)
            .ToListAsync();
}