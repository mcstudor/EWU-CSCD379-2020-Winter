using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using System;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests : BaseApiControllerTests<User, UserInput, IUserService>
    {
        protected override BaseApiController<User, UserInput> CreateController(IUserService service)
        => new UserController(service);

        protected override User CreateDto()
        => new User
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Id = new Random().Next()
        };

        protected override UserInput CreateInput()
            => CreateDto();
    }

    
}
