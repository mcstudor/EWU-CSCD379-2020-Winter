using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;
using SecretSanta.Data.Tests;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class UserServiceTests : TestBase
    {

        [TestMethod]
        public async Task CreateUser_ShouldSaveIntoDatabase()
        {
            int? userId;

            using (var dbContext = new ApplicationDbContext(Options))
            {
                var user = SampleData.CreateCheeseBurgerUser();
                dbContext.Users.Add(user);

                var user2 = SampleData.CreateCheeseBurgerUser();
                dbContext.Users.Add(user2);

                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                userId = user.Id;
            }

            using(var dbContext = new ApplicationDbContext(Options))
            {
                User user =
                    await dbContext.Users.SingleOrDefaultAsync(usr => usr.Id == userId);
                Assert.IsNotNull(user);
                Assert.AreEqual(SampleData.CheeseBurgerFirstName, user.FirstName);
            }
        }

        [TestMethod]
        public async Task CreateUser_ShouldSetFingerPrintDataOnInitialSave()
        {
            IHttpContextAccessor httpContextAccessor = GetHttpContextAccessorMock(SampleData.GetCheeseUsername());

            int? userId;
            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user = SampleData.CreateCheeseBurgerUser();
                dbContext.Users.Add(user);

                var user2 = SampleData.CreateCheeseBurgerUser();
                dbContext.Users.Add(user2);

                await dbContext.SaveChangesAsync();

                userId = user.Id;
            }

            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                User user =
                    await dbContext.Users.SingleOrDefaultAsync(usr => usr.Id == userId);
                Assert.IsNotNull(user);
                Assert.AreEqual(SampleData.GetCheeseUsername(), user.CreatedBy);
                Assert.AreEqual(SampleData.GetCheeseUsername(), user.ModifiedBy);
                Assert.AreEqual(SampleData.CheeseBurgerFirstName, user.FirstName);
            }
        }

        [TestMethod]
        public async Task CreateUser_ShouldSetFingerPrintDataOnUpdate()
        {
            IHttpContextAccessor httpContextAccessor = GetHttpContextAccessorMock(SampleData.GetCheeseUsername());

            int? userId;
            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user = SampleData.CreateCheeseBurgerUser();
                dbContext.Users.Add(user);

                var user2 = SampleData.CreateCheeseBurgerUser();
                dbContext.Users.Add(user2);

                await dbContext.SaveChangesAsync();

                userId = user.Id;
            }

            httpContextAccessor = GetHttpContextAccessorMock(SampleData.GetChickenUsername());


            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                User user =
                    await dbContext.Users.SingleOrDefaultAsync(usr => usr.Id == userId);
                user.FirstName = SampleData.ChickenFirstName;
                user.LastName = SampleData.ChickenLastName;

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                User user =
                    await dbContext.Users.SingleOrDefaultAsync(usr => usr.Id == userId);
                Assert.IsNotNull(user);
                Assert.AreEqual(SampleData.GetCheeseUsername(), user.CreatedBy);
                Assert.AreEqual(SampleData.GetChickenUsername(), user.ModifiedBy);

            }

        }

        [TestMethod]
        public async Task UpdateUser_ShouldSaveIntoDatabase()
        {
            using var dbContext = new ApplicationDbContext(Options);
            IUserService service = new UserService(dbContext, Mapper);
            var user = SampleData.CreateCheeseBurgerUser();
            var user2 = SampleData.CreateCheeseBurgerUser();

            await service.InsertAsync(user);
            await service.InsertAsync(user2);

            using var dbContext2 = new ApplicationDbContext(Options);
            User fetchedUser = await dbContext2.Users
                .SingleOrDefaultAsync(usr => usr.Id == user.Id);

            fetchedUser.FirstName = SampleData.ChickenFirstName;
            fetchedUser.LastName = SampleData.ChickenLastName;

            using var dbContext3 = new ApplicationDbContext(Options);
            var service2 = new UserService(dbContext3, Mapper);
            await service2.UpdateAsync(2, fetchedUser);

            using var dbContext4 = new ApplicationDbContext(Options);
            User savedUser = await dbContext4.Users
                .SingleOrDefaultAsync(usr => usr.Id == user.Id);
            User otherUser = await dbContext4.Users
                .SingleOrDefaultAsync(usr => usr.Id == 2);

            Assert.AreEqual((SampleData.CheeseBurgerFirstName, SampleData.CheeseBurgerLastName),
                (savedUser.FirstName, savedUser.LastName));
            Assert.AreNotEqual((savedUser.FirstName, savedUser.LastName), (otherUser.FirstName, otherUser.LastName));

        }
    }
}
