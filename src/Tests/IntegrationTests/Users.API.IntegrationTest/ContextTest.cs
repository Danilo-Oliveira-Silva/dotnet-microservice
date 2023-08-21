using Microsoft.EntityFrameworkCore;
using Users.API.Models;
using Users.API.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Users.API.IntegrationTest;

public class ContextTest : DbContext, IContext
{
    public DbSet<User> Users { get; set; }
    public ContextTest(DbContextOptions<ContextTest> options) : base(options)
    { }

    public ContextTest() { }

}