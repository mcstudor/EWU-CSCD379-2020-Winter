using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using SecretSanta.Data.Tests;

namespace SecretSanta.Api.Tests
{
    [TestClass]
    public class GiftControllerTests
    {
        //Mock service initialized during TestInit
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Mock<IGiftService> _MockGiftService;
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        [TestInitialize]
        public void TestInitialize()
        {
            _MockGiftService = new Mock<IGiftService>();
        }

        [TestMethod]
        public void Create_GiftService_Success()
        {
            using (_ = new GiftController(_MockGiftService.Object)) 
            { }
        
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithoutService_Fails()
        {
            using (_ = new GiftController(null!))
            { }
        }

        [TestMethod]
        public async Task GetById_WithExistingAuthor_Success()
        {
            Gift gift = SampleData.CreateChickenGift();
            int giftId = 42;
            _MockGiftService.Setup(svs => 
                svs.FetchByIdAsync(giftId))
                    .ReturnsAsync(gift);

            using (var controller = new GiftController(_MockGiftService.Object))
            {

                ActionResult<Gift> actual = await controller.Get(giftId);

                Assert.IsTrue(actual.Result is OkObjectResult);
            }
        }

        [TestMethod]
        public async Task GetById_WithoutExistingGift_NotFound()
        {
            _MockGiftService.Setup(svs => svs.FetchByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Gift)null!);

            using (var controller = new GiftController(_MockGiftService.Object))
            {

                ActionResult<Gift> actual = await controller.Get(42);

                Assert.IsTrue(actual.Result is NotFoundResult);
            }
        }

        [TestMethod]
        public async Task FetchAllGifts_ExistingGifts_Success()
        {
            IEnumerable<Gift> gifts = SampleData.CreateManyGifts();

            _MockGiftService.Setup(svs => svs.FetchAllAsync())
                .ReturnsAsync(gifts.ToList);

            using (var controller = new GiftController(_MockGiftService.Object))
            {

                IEnumerable<Gift> actual = await controller.Get();

                Assert.IsTrue(actual.Any());
                Assert.AreEqual(gifts.Count(), actual.Count());
            }
        }

        [TestMethod]
        public async Task PostGift_Accepted_ReturnsGift()
        {
            Gift gift = SampleData.CreateChickenGift();
            _MockGiftService.Setup(svs => svs.InsertAsync(gift))
                .ReturnsAsync(gift);

            using (var controller = new GiftController(_MockGiftService.Object))
            {
                Gift actual = await controller.Post(gift);
                Assert.AreEqual(gift, actual);
            }
        }

        [TestMethod]
        public async Task PutGift_Accepted_ReturnsGift()
        {
            Gift gift = SampleData.CreateChickenGift();
            int giftId = 42;
            _MockGiftService.Setup(svs => svs.UpdateAsync(42, It.IsAny<Gift>()))
                .ReturnsAsync(gift);

            using (var controller = new GiftController(_MockGiftService.Object))
            {
                ;

                ActionResult<Gift> actual = await controller.Put(giftId, gift);

                Assert.IsNotNull(actual.Value);
                Assert.AreEqual(gift, actual.Value);
            }
        }

        [TestMethod]
        public async Task PutGift_NoGiftFound_ReturnNotFound()
        {
            Gift gift = SampleData.CreateChickenGift();
            int giftId = 42;
            _MockGiftService.Setup(svs => svs.UpdateAsync(giftId, It.IsAny<Gift>()))
                .ReturnsAsync((Gift) null!);

            using (var controller = new GiftController(_MockGiftService.Object))
            {
                ActionResult<Gift> actual = await controller.Put(giftId, gift);

                Assert.IsTrue(actual.Result is NotFoundResult);
            }

        }

        [TestMethod]
        public async Task DeleteGift_GiftFound_Success()
        {
            int giftId = 42;
            _MockGiftService.Setup(svs => svs.DeleteAsync(giftId)).ReturnsAsync(true);

            using (var controller = new GiftController(_MockGiftService.Object))
            {
                ActionResult actual = await controller.Delete(giftId);
                Assert.IsTrue(actual is OkResult);
            }
        }

        [TestMethod]
        public async Task DeleteGift_GiftNotFound_ReturnsNotFound()
        {
            int giftId = 42;
            _MockGiftService.Setup(svs => svs.DeleteAsync(giftId)).ReturnsAsync(false);

            using (var controller = new GiftController(_MockGiftService.Object))
            {
                ActionResult actual = await controller.Delete(giftId);
                Assert.IsTrue(actual is NotFoundResult);
            }
        }


    }
}
