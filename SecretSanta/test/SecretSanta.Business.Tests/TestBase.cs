using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SecretSanta.Business.Tests
{
    public class TestBase : SecretSanta.Data.Tests.TestBase
    {
        //Set during initialize
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        protected IMapper Mapper { get; private set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        [TestInitialize]
        public override void InitializeTests()
        {
            base.InitializeTests();
            Mapper = AutomapperProfileConfiguration.CreateMapper();
        }

        public static IHttpContextAccessor GetHttpContextAccessorMock(string username) => 
            Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User
                    .FindFirst(ClaimTypes.NameIdentifier) ==
                         new Claim(ClaimTypes.NameIdentifier,
                        username));

    }
}
