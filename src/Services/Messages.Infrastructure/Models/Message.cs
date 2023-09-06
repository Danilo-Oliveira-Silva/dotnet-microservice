using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Messages.Infrastructure.Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Guid { get; set; }
    public string RoomId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}