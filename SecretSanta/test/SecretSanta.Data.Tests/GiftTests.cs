using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GiftTests : TestBase
    {
        [TestMethod]
        public void Gift_CanBeCreate_AllPropertiesGetSet()
        {
            // Arrange
            Gift gift = new Gift{Id = 1, Title = "Ring 2", Description = "Amazing way to keep the creepers away", Url = "www.ring.com",User = 
                new User{Id = 1,FirstName = "Inigo",LastName = "Montoya",Gifts = new List<Gift>()}};

            // Act

            // Assert
            Assert.AreEqual(1, gift.Id);
            Assert.AreEqual("Ring 2", gift.Title);
            Assert.AreEqual("Amazing way to keep the creepers away", gift.Description);
            Assert.AreEqual("www.ring.com", gift.Url);
            Assert.IsNotNull(gift.User);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetTitleToNull_ThrowsArgumentNullException()
        {
            Gift gift = new Gift
            {
                Id = 1,
                Title = null!,
                Description = "Amazing way to keep the creepers away",
                Url = "www.ring.com",
                User =
                    new User {Id = 1, FirstName = "Inigo", LastName = "Montoya", Gifts = new List<Gift>()}
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetDescriptionToNull_ThrowsArgumentNullException()
        {
            _ = new Gift
            {
                Id = 1,
                Title = "Ring 2",
                Description = null!,
                Url = "www.ring.com",
                User =
                    new User {Id = 1, FirstName = "Inigo", LastName = "Montoya", Gifts = new List<Gift>()}
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetUrlToNull_ThrowsArgumentNullException()
        {
            _ = new Gift
            {
                Id = 1,
                Title = "Ring 2",
                Description = "Amazing way to keep the creepers away",
                Url = null!,
                User =
                    new User {Id = 1, FirstName = "Inigo", LastName = "Montoya", Gifts = new List<Gift>()}
            };
        }

        [TestMethod]
        public async Task AddGift_WithUser_ShouldCreateForeignRelationship()
        {
            var user = SampleData.CreateCheeseBurgerUser();
            var gift = SampleData.CreateCheeseBurgerGift();
            gift.User = user;

            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options))
            {
                gift.User = user;
                dbContext.Gifts.Add(gift);
                await dbContext.SaveChangesAsync();
            }

            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.Include(u => u.User).ToListAsync();
                Assert.AreEqual(1, gifts.Count);
                Assert.AreEqual(gift.Title, gifts.First().Title);
                Assert.IsNotNull(gifts.First().User);
            }
        }
    }
}
