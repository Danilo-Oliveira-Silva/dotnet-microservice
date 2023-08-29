namespace Social.API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


public class Connection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Guid { get; set; }
    public string UserA { get; set; } = string.Empty;
    public string UserB { get; set; } = string.Empty;
    public int Status { get; set;}
}