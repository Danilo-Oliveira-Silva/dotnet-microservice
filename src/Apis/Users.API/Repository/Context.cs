using Users.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Users.API.Repository;

public class Context : DbContext, IContext
{
    public Context(DbContextOptions<Context> options) : base(options) {}
    public Context() { }

    public DbSet<User> Users { get; set;} = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=127.0.0.1;Database=Gaming;User=SA;Password=MySqlServer123!;TrustServerCertificate=true";
        optionsBuilder.UseSqlServer(connectionString);
    }
}