using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using System;
using System.Reflection;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using Group = SecretSanta.Business.Dto.Group;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GroupControllerTests : BaseApiControllerTests<Group, GroupInput, Data.Group>
    {
        protected override Group CreateDto()
            => new Group
            {
                Title = Guid.NewGuid().ToString(), 
                Id = new Random().Next()
            };



        protected override GroupInput CreateInput()
            => CreateDto();

        protected override GroupInput CreateInput(Data.Group entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));
            return new GroupInput{ Title = entity.Title + "changed"};
        }

        protected override Data.Group CreateEntity()
         => new Data.Group(Guid.NewGuid().ToString());

        protected override Uri BaseUrl()
            => new Uri("http://localhost:53261/api/Group");


        protected override void AddEntity(ApplicationDbContext context, Data.Group entity)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            context.Groups.Add(entity);
            context.SaveChangesAsync();
        }

        protected override void AssertAreEqual(Group dto, Data.Group entity)
        {
            Assert.IsNotNull(dto);
            Assert.IsNotNull(entity);
            //Null is asserted
#pragma warning disable CA1062 // Validate arguments of public methods
            Assert.AreEqual((dto.Id, dto.Title), (entity.Id, entity.Title));
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        protected override void AssertAreEqual(Group dto, GroupInput input)
        {
            Assert.IsNotNull(dto);
            Assert.IsNotNull(input);
            //Null is asserted
#pragma warning disable CA1062 // Validate arguments of public methods
            Assert.AreEqual(dto.Title, input.Title);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        protected override void AssertAreEqual(GroupInput input, Data.Group entity)
        {
            Assert.IsNotNull(input);
            Assert.IsNotNull(entity);
            //Null is asserted
#pragma warning disable CA1062 // Validate arguments of public methods
            Assert.AreEqual(input.Title, entity.Title);
#pragma warning restore CA1062 // Validate arguments of public methods
        }
    }



}
