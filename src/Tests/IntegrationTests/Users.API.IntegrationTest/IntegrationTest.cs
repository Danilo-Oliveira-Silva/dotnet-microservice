namespace Users.API.IntegrationTest;
using Users.API.Models;
using Users.API.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Xml;
using System.IO;

public class LoginJson {
    public string? Token { get; set; }
}

public class IntegrationTest: IClassFixture<WebApplicationFactory<Program>>
{   
    
    public HttpClient _clientTest;
    public IntegrationTest(WebApplicationFactory<Program> factory)
    {
        
        _clientTest = factory.WithWebHostBuilder(builder => {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<Context>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ContextTest>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestDatabase");
                });
                services.AddScoped<IContext, ContextTest>();
                services.AddScoped<IUserRepository, UserRepository>();
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<ContextTest>())
                {
                    appContext.Database.EnsureCreated();
                    appContext.Database.EnsureDeleted();
                    appContext.Database.EnsureCreated();
                    appContext.Users.Add(new User {UserId = 1, Name = "Danilo", Email = "danilo@email.com", Password = "123"});
                    appContext.SaveChanges();
                   
                }
            });
        }).CreateClient();
    }
   

    [Trait("Category", "Users API Tests")]
    [Theory(DisplayName = "Login")]
    [InlineData("/user/login")]
    public async Task TestLogin(string url)
    {
         var inputObjLogin = new {
            Email = "danilo@email.com",
            Password = "123"
        };
        var responselogin = await _clientTest.PostAsync(url,new StringContent(JsonConvert.SerializeObject(inputObjLogin), System.Text.Encoding.UTF8, "application/json"));
        var responseLoginString = await responselogin.Content.ReadAsStringAsync();
        LoginJson jsonLogin = JsonConvert.DeserializeObject<LoginJson>(responseLoginString)!;
        Assert.Equal(System.Net.HttpStatusCode.OK, responselogin?.StatusCode);
    }

    [Trait("Category", "Users API Tests")]
    [Theory(DisplayName = "Signup")]
    [InlineData("/user/signup")]
    public async Task TestSignup(string url)
    {
         var inputObjLogin = new {
            Name = "Danilo",
            Email = "danilos@email.com",
            LastName = "Silva",
            Password = "123456789"
        };
        var responselogin = await _clientTest.PostAsync(url,new StringContent(JsonConvert.SerializeObject(inputObjLogin), System.Text.Encoding.UTF8, "application/json"));
        var responseLoginString = await responselogin.Content.ReadAsStringAsync();
        LoginJson jsonLogin = JsonConvert.DeserializeObject<LoginJson>(responseLoginString)!;
        Assert.Equal(System.Net.HttpStatusCode.Created, responselogin?.StatusCode);
    }

}