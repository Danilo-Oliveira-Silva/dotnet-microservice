using Users.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Users.API.Repository;
public interface IContext
{
    public DbSet<User> Users { get; set; }
    public int SaveChanges();
}