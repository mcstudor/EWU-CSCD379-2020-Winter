using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using SecretSanta.Data.Tests;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GroupServiceTests : TestBase
    {
        [TestMethod]
        public async Task CreateGroup_ShouldSaveIntoDatabase()
        {
            int? groupId = null;

            using (var dbContext = new ApplicationDbContext(Options))
            {
                var group = SampleData.CreateFastFoodGroup();
                dbContext.Groups.Add(group);

                var group2 = SampleData.CreateFastFoodGroup();
                dbContext.Groups.Add(group2);

                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                groupId = group.Id;
            }

            using (var dbContext = new ApplicationDbContext(Options))
            {
                Group group =
                    await dbContext.Groups.SingleOrDefaultAsync(usr => usr.Id == groupId);
                Assert.IsNotNull(group);
                Assert.AreEqual(SampleData.FastFoodTitle, group.Title);
            }
        }

        [TestMethod]
        public async Task CreateGroup_ShouldSetFingerPrintDataOnInitialSave()
        {
            IHttpContextAccessor httpContextAccessor = GetHttpContextAccessorMock(SampleData.GetCheeseUsername());

            int? groupId;
            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var group = SampleData.CreateFastFoodGroup();
                dbContext.Groups.Add(group);

                var group2 = SampleData.CreateFastFoodGroup();
                dbContext.Groups.Add(group2);

                await dbContext.SaveChangesAsync();

                groupId = group.Id;
            }

            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Group group =
                    await dbContext.Groups.SingleOrDefaultAsync(usr => usr.Id == groupId);
                Assert.IsNotNull(group);
                Assert.AreEqual(SampleData.GetCheeseUsername(), group.CreatedBy);
                Assert.AreEqual(SampleData.GetCheeseUsername(), group.ModifiedBy);
                Assert.AreEqual(SampleData.FastFoodTitle, group.Title);
            }
        }

        [TestMethod]
        public async Task CreateGroup_ShouldSetFingerPrintDataOnUpdate()
        {
            IHttpContextAccessor httpContextAccessor = GetHttpContextAccessorMock(SampleData.GetCheeseUsername());

            int? groupId;
            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var group = SampleData.CreateFastFoodGroup();
                dbContext.Groups.Add(group);

                var group2 = SampleData.CreateFastFoodGroup();
                dbContext.Groups.Add(group2);

                await dbContext.SaveChangesAsync();

                groupId = group.Id;
            }

            httpContextAccessor = GetHttpContextAccessorMock(SampleData.GetChickenUsername());

            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Group group =
                    await dbContext.Groups.SingleOrDefaultAsync(usr => usr.Id == groupId);
                group.Title = "Sit Down Dining";

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Group group =
                    await dbContext.Groups.SingleOrDefaultAsync(usr => usr.Id == groupId);
                Assert.IsNotNull(group);
                Assert.AreEqual(SampleData.GetCheeseUsername(), group.CreatedBy);
                Assert.AreEqual(SampleData.GetChickenUsername(), group.ModifiedBy);

            }

        }

        [TestMethod]
        public async Task UpdateGroup_ShouldSaveIntoDatabase()
        {
            using var dbContext = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContext, Mapper);
            var group = SampleData.CreateFastFoodGroup();
            var group2 = SampleData.CreateFastFoodGroup();

            await service.InsertAsync(group);
            await service.InsertAsync(group2);

            using var dbContext2 = new ApplicationDbContext(Options);
            Group fetchedGroup = await dbContext2.Groups
                .SingleOrDefaultAsync(usr => usr.Id == group.Id);

            fetchedGroup.Title = "Sit Down Dining";

            using var dbContext3 = new ApplicationDbContext(Options);
            var service2 = new GroupService(dbContext3, Mapper);
            await service2.UpdateAsync(2, fetchedGroup);

            using var dbContext4 = new ApplicationDbContext(Options);
            Group savedGroup = await dbContext4.Groups
                .SingleOrDefaultAsync(usr => usr.Id == group.Id);
            Group otherGroup = await dbContext4.Groups
                .SingleOrDefaultAsync(usr => usr.Id == 2);

            Assert.AreEqual(SampleData.FastFoodTitle, savedGroup.Title);
            Assert.AreNotEqual(savedGroup.Title, otherGroup.Title);

        }

    }
    }
