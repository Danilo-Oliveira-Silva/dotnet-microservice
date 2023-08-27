using Users.API.DTO;
using Users.API.Models;

namespace Users.API.Repository;

public interface IUserRepository
{
    User Post(UserDTO user);
    User Login(AuthDTORequest login);
}