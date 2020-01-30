using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GiftTests : TestBase
    {
        [TestMethod]
        public async Task Gift_CanBeSavedToDatabase()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(SampleData.CreateCheeseBurgerGift()); ;
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            // Act
            // Assert
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.ToListAsync();

                Assert.AreEqual(1, gifts.Count);
                Assert.AreEqual((SampleData.CheeseBurgerDescription, SampleData.CheeseBurgerTitle, SampleData.CheeseBurgerUrl),
                                (gifts[0].Description, gifts[0].Title, gifts[0].Url));
            }
        }

        [TestMethod]
        public async Task AddGift_ShouldHaveFingerPrint()
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(SampleData.CreateChickenGift());
                await dbContext.SaveChangesAsync();
            }

            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.Include(g => g.User).ToListAsync();
                Assert.IsNotNull(gifts[0].CreatedBy);
                Assert.IsNotNull(gifts[0].ModifiedBy);
                Assert.IsNotNull(gifts[0].CreatedOn);
                Assert.IsNotNull(gifts[0].ModifiedOn);
                Assert.AreEqual(1, gifts[0].Id);
            }
        }

        [TestMethod]
        public async Task AddGift_WithUser_ShouldCreateForeignRelationship()
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(SampleData.CreateCheeseBurgerGift());
                await dbContext.SaveChangesAsync();
            }
            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.Include(g => g.User).ToListAsync();
                Assert.AreEqual(1, gifts.Count);
                Assert.AreEqual((SampleData.CheeseBurgerDescription, SampleData.CheeseBurgerTitle, SampleData.CheeseBurgerUrl),
                    (gifts[0].Description, gifts[0].Title, gifts[0].Url));
                Assert.AreEqual(1, gifts[0].Id);
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetTitleToNull_ThrowsArgumentNullException()
        {
            _ = new Gift(null!, 
                SampleData.CheeseBurgerDescription, 
                SampleData.CheeseBurgerUrl,
                SampleData.CreateChickenUser());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetDescriptionToNull_ThrowsArgumentNullException()
        {
            _ = new Gift(SampleData.ChickenTitle,
                null!,
                SampleData.CheeseBurgerUrl,
                SampleData.CreateChickenUser());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetUrlToNull_ThrowsArgumentNullException()
        {
            _ = new Gift(SampleData.ChickenTitle,
                SampleData.CheeseBurgerDescription,
                null!,
                SampleData.CreateChickenUser());
        }
    }
}
