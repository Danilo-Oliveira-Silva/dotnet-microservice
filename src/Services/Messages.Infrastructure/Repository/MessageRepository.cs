using Messages.Infrastructure.Models;
using MongoDB.Driver;
using Messages.Infrastructure.Context;

namespace Messages.Infrastructure.Repository;

public class MessageRepository
{
    private readonly IMongoCollection<Message> _messageCollection;

    public MessageRepository() {

        var mongoDatabase = ContextConnection.GetDatabase();

        //var mongoClient = new MongoClient("mongodb://root:Mottu2023!@localhost:27017");
        //var mongoDatabase = mongoClient.GetDatabase("Mottu");
        _messageCollection = mongoDatabase.GetCollection<Message>("Messages");
    }

    public void Create(Message message)
    {
         _messageCollection.InsertOne(message);
    }

}
