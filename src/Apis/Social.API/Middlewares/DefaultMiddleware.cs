namespace Social.API.Middlewares;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Social.API.Repository;
using Social.API.Models;
public class DefaultMiddleware
{
    private readonly ILogger<DefaultMiddleware> _logger;
    private readonly RequestDelegate _next;
    private readonly IUserRepository _repository;
    public DefaultMiddleware(ILogger<DefaultMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
        _repository = new UserRepository();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task Invoke(HttpContext context)
    {
        var tokenb = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (!string.IsNullOrEmpty(tokenb))
        {
            var token = context.User;
            var email = token?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var name = token?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            Console.WriteLine("token: "+email +" - " + name);

            if (context.Request.Path == "/user/list")
            {
                var user = _repository.Find(email!);
                if (user == null) user = _repository.Create(new User{ Name = name!, Email = email! });
                context.Items["userid"] = user.Guid;
            }
        }

        await _next.Invoke(context);
    }
}