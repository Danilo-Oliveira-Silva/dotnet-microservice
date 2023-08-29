using MongoDB.Driver;
using Social.API.Models;
using Social.API.Context;

namespace Social.API.Repository;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserRepository() {
         var mongoDatabase = ContextConnection.GetDatabase();
        _usersCollection = mongoDatabase.GetCollection<User>("Users");
    }

    public async Task<List<User>> List()
    {
        var allUsers =  _usersCollection.Find(_ => true).ToList();
        return allUsers;
    }

    public User Create(User newUser)
    {
        _usersCollection.InsertOne(newUser);
        var userCreated = Find(newUser.Email);
        return userCreated;
    }

    public User Find(string email)
    {
        var userFinded = _usersCollection.Find(u => u.Email == email).ToList();
        User userReturned = default!;
        if (userFinded.Count() > 0) userReturned = userFinded.First();
        return userReturned!;
    }
}
