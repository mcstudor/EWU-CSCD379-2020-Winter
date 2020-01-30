using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using SecretSanta.Data.Tests;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftServiceTests : TestBase
    {
        [TestMethod]
        public async Task CreateGift_ShouldSaveIntoDatabase()
        {
            using var dbContext = new ApplicationDbContext(Options);

            IGiftService service = new GiftService(dbContext, Mapper);

            var user = SampleData.CreateCheeseBurgerUser();
            var gift = SampleData.CreateCheeseBurgerGift();
            gift.User = user;

            await service.InsertAsync(gift);

            Assert.IsNotNull(gift.Id);
            Assert.IsNotNull(user.Id);
            Assert.AreSame(gift.User, user);
            Assert.AreEqual(user.Id, gift.User.Id);
        }

        [TestMethod]
        public async Task FetchByIdGift_ShouldIncludeAuthor()
        {
            using var dbContext = new ApplicationDbContext(Options);

            IGiftService service = new GiftService(dbContext, Mapper);

            var user = SampleData.CreateCheeseBurgerUser();
            var gift = SampleData.CreateCheeseBurgerGift();
            gift.User = user;

            await service.InsertAsync(gift);

            using var dbContext2 = new ApplicationDbContext(Options);
            service = new GiftService(dbContext, Mapper);
            gift = await service.FetchByIdAsync(gift.Id!.Value);

            Assert.IsNotNull(gift.User);
            Assert.AreSame(user, gift.User);
        }

        [TestMethod]
        public async Task CreateGift_ShouldSetFingerPrintDataOnInitialSave()
        {
            IHttpContextAccessor httpContextAccessor = GetHttpContextAccessorMock(SampleData.GetCheeseUsername());

            int? giftId;
            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift = SampleData.CreateCheeseBurgerGift();
                dbContext.Gifts.Add(gift);

                var gift2 = SampleData.CreateCheeseBurgerGift();
                dbContext.Gifts.Add(gift2);

                await dbContext.SaveChangesAsync();

                giftId = gift.Id;
            }

            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Gift gift =
                    await dbContext.Gifts.SingleOrDefaultAsync(gft => gft.Id == giftId);
                Assert.IsNotNull(gift);
                Assert.AreEqual(SampleData.GetCheeseUsername(), gift.CreatedBy);
                Assert.AreEqual(SampleData.GetCheeseUsername(), gift.ModifiedBy);
                Assert.AreEqual(SampleData.CheeseBurgerDescription, gift.Description);
            }
        }

        [TestMethod]
        public async Task CreateGift_ShouldSetFingerPrintDataOnUpdate()
        {
            IHttpContextAccessor httpContextAccessor = GetHttpContextAccessorMock(SampleData.GetCheeseUsername());

            int? giftId;
            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift = SampleData.CreateCheeseBurgerGift();
                dbContext.Gifts.Add(gift);

                var gift2 = SampleData.CreateCheeseBurgerGift();
                dbContext.Gifts.Add(gift2);

                await dbContext.SaveChangesAsync();

                giftId = gift.Id;
            }

            httpContextAccessor = GetHttpContextAccessorMock(SampleData.GetChickenUsername());


            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Gift gift =
                    await dbContext.Gifts.SingleOrDefaultAsync(gft => gft.Id == giftId);
                gift.Title = SampleData.ChickenTitle;
                gift.Description = SampleData.ChickenDescription;

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Gift gift =
                    await dbContext.Gifts.SingleOrDefaultAsync(gft => gft.Id == giftId);
                Assert.IsNotNull(gift);
                Assert.AreEqual(SampleData.GetCheeseUsername(), gift.CreatedBy);
                Assert.AreEqual(SampleData.GetChickenUsername(), gift.ModifiedBy);

            }

        }

        [TestMethod]
        public async Task UpdateGift_ShouldSaveIntoDatabase()
        {
            using var dbContext = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContext, Mapper);
            var gift = SampleData.CreateCheeseBurgerGift();
            var gift2 = SampleData.CreateCheeseBurgerGift();

            await service.InsertAsync(gift);
            await service.InsertAsync(gift2);

            using var dbContext2 = new ApplicationDbContext(Options);
            Gift fetchedGift = await dbContext2.Gifts
                .SingleOrDefaultAsync(gft => gft.Id == gift.Id);

            fetchedGift.Title = SampleData.ChickenTitle;
            fetchedGift.Description = SampleData.ChickenDescription;

            using var dbContext3 = new ApplicationDbContext(Options);
            var service2 = new GiftService(dbContext3, Mapper);
            await service2.UpdateAsync(2, fetchedGift);

            using var dbContext4 = new ApplicationDbContext(Options);
            Gift savedGift = await dbContext4.Gifts
                .SingleOrDefaultAsync(gft => gft.Id == gift.Id);
            Gift otherGift = await dbContext4.Gifts
                .SingleOrDefaultAsync(gft => gft.Id == 2);

            Assert.AreEqual((SampleData.CheeseBurgerTitle, SampleData.CheeseBurgerDescription),
                (savedGift.Title, savedGift.Description));
            Assert.AreNotEqual((savedGift.Title, savedGift.Description), (otherGift.Title, otherGift.Description));

        }


    }
}
