using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Users.API.Repository;
using Users.API.Models;
using Users.API.DTO;
using Users.API.Services;

namespace Users.API.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("signup")]
        public IActionResult Post([FromBody] User user)
        {
            User newUser = _repository.Post(user);
            string token = new TokenManager().Generate(newUser);
            AuthDTOResponse response = new AuthDTOResponse { Token = token };
            return Created("", response);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthDTORequest login)
        {
            User userFound = _repository.Login(login);
            string token = new TokenManager().Generate(userFound);
            AuthDTOResponse response = new AuthDTOResponse { Token = token };
            return Ok(response);
        }
    }
}