using Microsoft.AspNetCore.Mvc;
using Social.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Social.API.Dto;

namespace Social.API.Controllers;


[ApiController]
[Route("[controller]")]
public class ConnectionController : ControllerBase
{
    private readonly IConnectionRepository _connectionRepository;
    private readonly IUserRepository _userRepository;
    
    public ConnectionController(IConnectionRepository connectionRepository, IUserRepository userRepository)
    {
        _connectionRepository = connectionRepository;
        _userRepository = userRepository;
    }


    [HttpGet("invite/send")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Client")]
    public IActionResult SendInvite([FromBody] InviteDto inviteDto)
    {
        var token = HttpContext.User.Identity as ClaimsIdentity;
        var email = token?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        var user  = _userRepository.Find(email!);

        var connection = _connectionRepository.Invite(user.Guid!, inviteDto.UserId!);
        return Ok(connection);
    }

    [HttpGet("invite/received")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Client")]
    public IActionResult InviteReceived()
    {
        var token = HttpContext.User.Identity as ClaimsIdentity;
        var email = token?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        var user  = _userRepository.Find(email!);

        var usersList = _connectionRepository.ListConnections(user.Guid!, "received");
        return Ok(usersList);
    }

    [HttpGet("invite/sent")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Client")]
    public IActionResult InviteSent()
    {
        var token = HttpContext.User.Identity as ClaimsIdentity;
        var email = token?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        var user  = _userRepository.Find(email!);

        var usersList = _connectionRepository.ListConnections(user.Guid!, "sent");
        return Ok(usersList);
    }


    [HttpGet()]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Client")]
    public IActionResult AllConnections()
    {
        var token = HttpContext.User.Identity as ClaimsIdentity;
        var email = token?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        var user  = _userRepository.Find(email!);

        var usersList = _connectionRepository.ListConnections(user.Guid!, "connection");
        return Ok(usersList);
    }
}
