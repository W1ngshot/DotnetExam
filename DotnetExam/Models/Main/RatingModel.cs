using MongoDB.Bson;

namespace DotnetExam.Models.Main;

public class RatingModel
{
    public ObjectId _id { get; set; }
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
    public int Rating { get; set; }
}