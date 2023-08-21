using Users.API.Models;
using Users.API.DTO;

namespace Users.API.Repository;

public class UserRepository : IUserRepository
{
    protected readonly IContext _context;
    public UserRepository(IContext context)
    {
        _context = context;
    }

    public User Post(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }
    public User Login(AuthDTORequest login)
    {
        User userFound = _context.Users.FirstOrDefault(user => user.Email == login.Email && user.Password == login.Password)!;
        return userFound;
    }
}