﻿using DotnetExam.Models.Main;
using DotnetExam.Services.Configs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DotnetExam.Services;

public class RatingService
{
    private readonly IMongoCollection<RatingModel> _collection;

    public RatingService(IOptions<MongoDbConfig> settings)
    {
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

    public async Task AddRating(Guid userId, int ratingChange)
    {
        var ratingModel = await _collection.Find(model => model.UserId == userId).FirstOrDefaultAsync();
        ratingModel.Rating += ratingChange;
        await _collection.ReplaceOneAsync(model => model.UserId == userId, ratingModel);
    }

    public async Task<RatingModel> GetUserInfoAsync(Guid userId) =>
        await _collection.Find(model => model.UserId == userId).FirstOrDefaultAsync();

    public async Task<int> GetUserRatingAsync(Guid userId) =>
        (await _collection.Find(model => model.UserId == userId).FirstOrDefaultAsync()).Rating;

    public async Task<List<RatingModel>> GetTopRatingUsers(int count) =>
        (await _collection.Find(_ => true).ToListAsync())
        .OrderByDescending(x => x.Rating)
        .Take(count)
        .ToList();
}