using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using System;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllerTests : BaseApiControllerTests<Gift, GiftInput, IGiftService>
    {
        protected override BaseApiController<Gift, GiftInput> CreateController(IGiftService service)
        => new GiftController(service);

        protected override Gift CreateDto()
            => new Gift()
            {
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Url = Guid.NewGuid().ToString(),
                User = new UserInput()
                {
                    FirstName = Guid.NewGuid().ToString(),
                    LastName = Guid.NewGuid().ToString(),
                },
                Id = new Random().Next()
            };

        protected override GiftInput CreateInput()
            => CreateDto();
    }

}

