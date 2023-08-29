namespace Social.API.Repository;
using Social.API.Models;

public interface IConnectionRepository
{
    List<Connection> ListConnections(string UserId, string status);
    Connection Invite(string UserA, string UserB);

    Connection Find(string UserA, string UserB);

}