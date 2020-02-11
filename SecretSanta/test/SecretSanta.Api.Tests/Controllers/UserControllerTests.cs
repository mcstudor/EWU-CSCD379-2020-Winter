using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using System;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using User = SecretSanta.Business.Dto.User;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests : BaseApiControllerTests<User, UserInput, Data.User>
    {

        protected override User CreateDto()
        => new User
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Id = new Random().Next()
        };

        protected override UserInput CreateInput()
            => CreateDto();

        protected override UserInput CreateInput(Data.User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new UserInput
            {
                FirstName = entity.FirstName + "changed",
                LastName = entity.LastName + "changed"
            };
        }

        protected override Data.User CreateEntity()
        => new Data.User(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        protected override Uri BaseUrl()
            => new Uri("http://localhost:53261/api/User");


        protected override void AddEntity(ApplicationDbContext context, Data.User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (context == null) throw new ArgumentNullException(nameof(context));
            context.Users.Add(entity);
            context.SaveChanges();
        }

        protected override void AssertAreEqual(User dto, Data.User entity)
        {
            Assert.IsNotNull(dto);
            Assert.IsNotNull(entity);
            //Null is asserted
#pragma warning disable CA1062 // Validate arguments of public methods
            Assert.AreEqual((dto.Id, dto.FirstName, dto.LastName), 
                (entity.Id, entity.FirstName, entity.LastName));
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        protected override void AssertAreEqual(User dto, UserInput input)
        {
            Assert.IsNotNull(dto);
            Assert.IsNotNull(input);
            //Null is asserted
#pragma warning disable CA1062 // Validate arguments of public methods
            Assert.AreEqual((dto.FirstName, dto.LastName),
                (input.FirstName, input.LastName));
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        protected override void AssertAreEqual(UserInput input, Data.User entity)
        {
            Assert.IsNotNull(entity);
            Assert.IsNotNull(input);
            //Null is asserted
#pragma warning disable CA1062 // Validate arguments of public methods
            Assert.AreEqual((entity.FirstName, entity.LastName),
                (input.FirstName, input.LastName));
#pragma warning restore CA1062 // Validate arguments of public methods
        }
    }

    
}
