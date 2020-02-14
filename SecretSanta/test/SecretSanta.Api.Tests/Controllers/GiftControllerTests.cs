using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using Gift = SecretSanta.Business.Dto.Gift;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllerTests : BaseApiControllerTests<Gift, GiftInput, Data.Gift>
    {

        protected override Gift CreateDto()
            => new Gift()
            {
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Url = Guid.NewGuid().ToString(),
                UserId = 1,
                Id = new Random().Next()
            };

        protected override GiftInput CreateInput()
            => CreateDto();

        protected override GiftInput CreateInput(Data.Gift entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new GiftInput
            {
                Title = entity.Title += "changed",
                Description = entity.Description += "changed",
                Url = entity.Url += "changed",
                UserId = entity.UserId
            };
        }


        protected override Data.Gift CreateEntity()
        => new Data.Gift()
        {
            Title = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            Url = Guid.NewGuid().ToString(),
            UserId = 1,
        };

        protected override Uri BaseUrl()
            => new Uri("http://localhost:53261/api/Gift");

        protected override void AddEntity(ApplicationDbContext context, Data.Gift entity)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            context.Users.Add(new Data.User(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
            context.SaveChanges();
            context.Gifts.Add(entity);
            context.SaveChanges();
        }

        protected override void AssertAreEqual(Gift dto, Data.Gift entity)
        { 
            Assert.IsNotNull(dto);
            Assert.IsNotNull(entity);
            //Null is asserted
#pragma warning disable CA1062 // Validate arguments of public methods
            Assert.AreEqual((dto.Id, dto.Description, dto.Url, dto.Title, dto.UserId),
                    (entity.Id, entity.Description, entity.Url, entity.Title, entity.UserId));
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        protected override void AssertAreEqual(Gift dto, GiftInput input)
        {
            Assert.IsNotNull(dto);
            Assert.IsNotNull(input);
            //Null is asserted
#pragma warning disable CA1062 // Validate arguments of public methods
            Assert.AreEqual((dto.Description, dto.Url, dto.Title, dto.UserId),
                    (input.Description, input.Url, input.Title, input.UserId));
#pragma warning restore CA1062 // Validate arguments of public methods
        }


        protected override void AssertAreEqual(GiftInput input, Data.Gift entity)
        {
            Assert.IsNotNull(input);
            Assert.IsNotNull(entity);
            //Null is asserted
#pragma warning disable CA1062 // Validate arguments of public methods
            Assert.AreEqual((input.Description, input.Url, input.Title, input.UserId),
                    (entity.Description, entity.Url, entity.Title, entity.UserId));
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        [TestMethod]
        [DataRow(nameof(GiftInput.Description))]
        [DataRow(nameof(GiftInput.Title))]
        [DataRow(nameof(GiftInput.Url))]
        [DataRow(nameof(GiftInput.UserId))]
        public async Task Post_GiftInputRequired_Returns400(string property)
        {
            var input = CreateInput();
            var prop = typeof(GiftInput).GetProperty(property);
            prop?.SetValue(input, null!);
            string jsonBody = JsonSerializer.Serialize(input);
            using StringContent stringContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PostAsync(BaseUrl(), stringContent);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }
    }

}

