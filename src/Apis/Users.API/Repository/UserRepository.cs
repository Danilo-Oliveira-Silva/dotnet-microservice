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

    public User Post(UserDTO user)
    {
        var findedUser = _context.Users.Where(u => u.Email == user.Email);
        if (findedUser.Count() > 0) throw new ArgumentException("E-mail already registered");
        if (user.Password.Length < 6) throw new ArgumentException("Password length exception");

        var newUser = new User{
            Name = user.Name,
            Email = user.Email,
            LastName = user.LastName,
            Password = user.Password,
            RegisterDate = DateTime.UtcNow
        };
        _context.Users.Add(newUser);
        _context.SaveChanges();
        return newUser;
    }
    public User Login(AuthDTORequest login)
    {
        User userFound = _context.Users.FirstOrDefault(user => user.Email == login.Email && user.Password == login.Password)!;
        if (userFound == null) throw new ArgumentException("User or Password doesn't match");
        return userFound;
    }
}