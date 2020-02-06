using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public abstract class BaseApiControllerTests<TDto, TInputDto, TService>
        where TInputDto : class
        where TDto : class, TInputDto, IEntity
        where TService : class, IEntityService<TDto, TInputDto>
    {
        protected abstract BaseApiController<TDto, TInputDto> CreateController(TService service);

        protected abstract TDto CreateDto();
        protected abstract TInputDto CreateInput();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_RequiresService()
        {
            new ThrowingController();
        }

        [TestMethod]
        public async Task Get_FetchesAllItems()
        {
            Mock<TService> service = new Mock<TService>();
            List<TDto> expected = new List<TDto>
            {
                CreateDto(), 
                CreateDto(), 
                CreateDto()
            };
            service.Setup(svs => svs.FetchAllAsync()).ReturnsAsync(expected);

            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            IEnumerable<TDto> actual = await controller.Get();

            CollectionAssert.AreEqual(expected, actual.ToList());
        }

        [TestMethod]
        public async Task Get_WhenEntityDoesNotExist_ReturnsNotFound()
        {
            Mock<TService> service = new Mock<TService>();

            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            IActionResult result = await controller.Get(1);

            Assert.IsTrue(result is NotFoundResult);
        }


        [TestMethod]
        public async Task Get_WhenEntityExists_ReturnsItem()
        {
            Mock<TService> service = new Mock<TService>();
            TDto entity = CreateDto();

            service.Setup(svs => svs.FetchByIdAsync(entity.Id)).ReturnsAsync(entity);

            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            IActionResult result = await controller.Get(entity.Id);

            var okResult = result as OkObjectResult;
            
            Assert.AreEqual(entity, okResult?.Value);
        }

        [TestMethod]
        public async Task Put_UpdatesItem()
        {
            Mock<TService> service = new Mock<TService>();
            TDto dto = CreateDto();
            TInputDto input = CreateInput();
            service.Setup(svs => svs.UpdateAsync(dto.Id, input)).ReturnsAsync(dto);

            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            TDto? result = await controller.Put(dto.Id, input);

            Assert.AreEqual(dto, result);
        }

        [TestMethod]
        public async Task Post_InsertsItem()
        {
            Mock<TService> service = new Mock<TService>();
            TInputDto entity = CreateInput();
            TInputDto actual = null!;
            service.Setup(svs => svs.InsertAsync(entity))
                    .Callback((TInputDto i) => actual = i);
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            TDto? result = await controller.Post(entity);

            Assert.IsNotNull(actual);
            Assert.AreEqual(entity, actual);
        }

        [TestMethod]
        public async Task Delete_WhenItemDoesNotExist_ReturnsNotFound()
        {
            Mock<TService> service = new Mock<TService>();
            service.Setup(svs => svs.DeleteAsync(1)).ReturnsAsync(false);
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);
            

            IActionResult result = await controller.Delete(1);

            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public async Task Delete_WhenItemExists_ReturnsOk()
        {
            Mock<TService> service = new Mock<TService>();
            int entityId = 1;
            service.Setup(svs => svs.DeleteAsync(entityId)).ReturnsAsync(true);
            BaseApiController<TDto, TInputDto> controller = CreateController(service.Object);

            IActionResult result = await controller.Delete(entityId);

            Assert.IsTrue(result is OkResult);
        }

        private class ThrowingController : BaseApiController<TDto, TInputDto>
        {
            public ThrowingController() : base(null!)
            { }
        }
    }
}
