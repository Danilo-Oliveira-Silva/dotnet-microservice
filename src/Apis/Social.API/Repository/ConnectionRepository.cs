using MongoDB.Driver;
using MongoDB.Bson;
using Social.API.Models;
using Social.API.Context;

namespace Social.API.Repository;

public class ConnectionRepository : IConnectionRepository
{
    private readonly IMongoCollection<Connection> _connectionsCollection;

    public ConnectionRepository() {
         var mongoDatabase = ContextConnection.GetDatabase();
        _connectionsCollection = mongoDatabase.GetCollection<Connection>("Connections");
    }

    public Connection Invite (string UserA, string UserB)
    {
        Connection connection = Find(UserA, UserB);
        if (connection == null)
        {
            _connectionsCollection.InsertOne(new Connection { UserA = UserA, UserB = UserB, Status = 0 });            
        }
        else
        {
            var filter = Builders<Connection>.Filter.Where(c => c.Guid == connection.Guid);
            var update = Builders<Connection>.Update.Set("Status", 1);
            _connectionsCollection.UpdateOne(filter, update);
        }

        return Find(UserA, UserB);
    }

    public Connection Find(string UserA, string UserB)
    {
        Connection connectionReturned = default!;
        var connectionFinded = _connectionsCollection.Find(c => c.UserA == UserA && c.UserB == UserB).ToList();
        if (connectionFinded.Count == 0) connectionFinded = _connectionsCollection.Find(c => c.UserA == UserB && c.UserB == UserA).ToList();
        if (connectionFinded.Count > 0) connectionReturned = connectionFinded.First();
        return connectionReturned!;
    }

    public List<Connection> ListConnections(string UserId, string status)
    {
        var allConnections = new List<Connection>();
        
        if (status == "connection")
            allConnections = _connectionsCollection.Find(c => c.Status == 1 && (c.UserA == UserId || c.UserB == UserId)).ToList();
        else if (status == "sent")
            allConnections = _connectionsCollection.Find(c => c.Status == 0 && c.UserA == UserId).ToList();
        else if (status == "received")
            allConnections = _connectionsCollection.Find(c => c.Status == 0 && c.UserB == UserId).ToList();
        
        return allConnections;
    }
    
}