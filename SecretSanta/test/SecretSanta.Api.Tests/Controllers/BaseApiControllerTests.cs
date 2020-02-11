using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using SecretSanta.Data;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public abstract class BaseApiControllerTests<TDto, TInputDto, TEntity>
        where TInputDto : class
        where TDto : class, TInputDto, IEntity
        where TEntity : Data.EntityBase
    {
        // Initialized in startup
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        protected SecretSantaWebApplicationFactory Factory { get; set; }
        protected HttpClient Client { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.


        protected abstract TDto CreateDto();
        protected abstract TInputDto CreateInput();
        protected abstract TInputDto CreateInput(TEntity entity);
        protected abstract TEntity CreateEntity();
        protected abstract Uri BaseUrl();
        protected abstract void AddEntity(ApplicationDbContext context, TEntity entity);
        protected abstract void AssertAreEqual(TDto dto, TEntity entity);
        protected abstract void AssertAreEqual(TDto dto, TInputDto input);
        protected abstract void AssertAreEqual(TInputDto input, TEntity entity);


        [TestInitialize]
        public void TestSetup()
        {
            Factory = new SecretSantaWebApplicationFactory();

            using ApplicationDbContext context = Factory.GetDbContext();
            context.Database.EnsureCreated();

            Client = Factory.CreateClient();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Factory.Dispose();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_RequiresService()
        {
            new ThrowingController();
        }

        [TestMethod]
        public async Task Get_FetchesAllItems()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            var expected = CreateEntity();
            AddEntity(context, expected);

            HttpResponseMessage response = await Client.GetAsync(BaseUrl());
            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();
            TDto[] actual = JsonSerializer.Deserialize<TDto[]>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            AssertAreEqual(actual.First(), expected);
        }

        [TestMethod]
        public async Task Get_WhenEntityDoesNotExist_ReturnsEmptyList()
        {
            HttpResponseMessage response = await Client.GetAsync(BaseUrl());
            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();
            TDto[] actual = JsonSerializer.Deserialize<TDto[]>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsFalse(actual.Any());
        }


        [TestMethod]
        public async Task Get_WhenEntityExists_ReturnsItem()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            var expected = CreateEntity();
            AddEntity(context, expected);
            Uri url = new Uri($"{BaseUrl().OriginalString}/{expected.Id}");
            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();
            TDto actual = JsonSerializer.Deserialize<TDto>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            AssertAreEqual(actual, expected);
        }

        [TestMethod]
        public async Task Put_UpdatesItem()
        {
            TEntity entity = CreateEntity();
            TInputDto input = CreateInput(entity);
            using ApplicationDbContext context = Factory.GetDbContext();
            AddEntity(context, entity);

            string jsonBody = JsonSerializer.Serialize(input);
            using StringContent stringContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            Uri url = new Uri($"{BaseUrl().OriginalString}/{entity.Id}");
            HttpResponseMessage response = await Client.PutAsync(url, stringContent);
            response.EnsureSuccessStatusCode();
            string returnedJson = await response.Content.ReadAsStringAsync();
            TDto actual = JsonSerializer.Deserialize<TDto>(returnedJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            AssertAreEqual(actual, input);

        }

        [TestMethod]
        public async Task Post_InsertsItem()
        {
            TInputDto input = CreateInput();
            using ApplicationDbContext context = Factory.GetDbContext();
            AddEntity(context, CreateEntity());

            string jsonBody = JsonSerializer.Serialize(input);
            using StringContent stringContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await Client.PostAsync(BaseUrl(), stringContent);
            response.EnsureSuccessStatusCode();
            string returnedJson = await response.Content.ReadAsStringAsync();
            TDto actual = JsonSerializer.Deserialize<TDto>(returnedJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            AssertAreEqual(actual, input);

        }

        [TestMethod]
        public async Task Delete_WhenItemDoesNotExist_ReturnsNotFound()
        {
            Uri url = new Uri($"{BaseUrl().OriginalString}/1");
            HttpResponseMessage response = await Client.DeleteAsync(url);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task Delete_WhenItemExists_ReturnsOk()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            TEntity entity = CreateEntity();
            AddEntity(context, entity);
            Uri url = new Uri($"{BaseUrl().OriginalString}/{entity.Id}");
            HttpResponseMessage response = await Client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

        }

        private class ThrowingController : BaseApiController<TDto, TInputDto>
        {
            public ThrowingController() : base(null!)
            { }
        }
    }
}
