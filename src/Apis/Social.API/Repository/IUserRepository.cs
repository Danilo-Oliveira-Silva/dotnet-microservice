namespace Social.API.Repository;
using Social.API.Models;

public interface IUserRepository
{
    Task<List<User>> List();
    User Create(User newUser);
    User Find(string email);
}