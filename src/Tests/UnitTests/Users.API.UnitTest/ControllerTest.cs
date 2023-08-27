namespace Users.API.UnitTest;
using Users.API.Controllers;
using Users.API.Repository;
using Users.API.Models;
using Users.API.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class ControllerTest
{
    UserController _controller;
    
    [Trait("Category", "Users API Tests")]
    [Theory(DisplayName = "Login")]
    [InlineData("")]
    public void TestControllerLogin(string dataString)
    {
        Moq.Mock<IUserRepository> moqRepository = new Moq.Mock<IUserRepository>();
        moqRepository.Setup(r => r.Login(It.IsAny<AuthDTORequest>())).Returns(new User{ UserId = 1, Name = "Danilo", Email = "danilo@email.com", Password = "123"});
        
        _controller = new UserController(moqRepository.Object);
        var result = _controller.Login(new AuthDTORequest { Email = "danilo@email.com", Password = "123"});
        var okResult = result as OkObjectResult;

        Assert.Equal(200, okResult.StatusCode);
    }

    [Trait("Category", "Users API Tests")]
    [Theory(DisplayName = "Signup")]
    [InlineData("")]
    public void TestControllerSignup(string dataString)
    {
        Moq.Mock<IUserRepository> moqRepository = new Moq.Mock<IUserRepository>();
        moqRepository.Setup(r => r.Post(It.IsAny<UserDTO>())).Returns(new User{ UserId = 1, Name = "Danilo", Email = "danilo@email.com", Password = "123"});
        
        _controller = new UserController(moqRepository.Object);
        var result = _controller.Post(new UserDTO { Name = "Danilo", Email = "danilo@email.com", Password = "123"});
        var okResult = result as ObjectResult;

        Assert.Equal(201, okResult.StatusCode);
    }
}