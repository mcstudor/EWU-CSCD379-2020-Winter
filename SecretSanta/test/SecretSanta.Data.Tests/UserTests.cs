using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class UserTests : TestBase
    {
        [TestMethod]
        public void User_CanBeCreate_AllPropertiesGetSet()
        {
            // Arrange
            User user = new User{Id = 1,FirstName = "Inigo", LastName = "Montoya",Gifts = new List<Gift>()};

            // Act
            // Assert
            Assert.AreEqual(1, user.Id);
            Assert.AreEqual("Inigo", user.FirstName);
            Assert.AreEqual("Montoya", user.LastName);
            Assert.IsNotNull(user.Gifts);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void User_SetFirstNameToNull_ThrowsArgumentNullException()
        {
            User user = new User { Id = 1, FirstName = null!, LastName = "Montoya", Gifts = new List<Gift>() };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void User_SetLastNameToNull_ThrowsArgumentNullException()
        {
            User user = new User { Id = 1, FirstName = "Inigo", LastName = null!, Gifts = new List<Gift>() };
        }

        [TestMethod]
        public async Task CreateUser_ShouldSaveIntoDatabase()
        {
            int userId = -1;

            using (var dbContext = new ApplicationDbContext(Options))
            {
                var user = SampleData.CreateCheeseBurgerUser();
                dbContext.Users.Add(user);
                var user2 = SampleData.CreateChickenUser();
                dbContext.Users.Add(user2);

                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                userId = user.Id;
            }

            using (var dbContext = new ApplicationDbContext(Options))
            {
                User user = await dbContext.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
                Assert.IsNotNull(user);
                Assert.AreEqual(SampleData.CheeseBurgerFirstName, user.FirstName);
            }
        }

        [TestMethod]
        public async Task CreateUser_ShouldSetFingerPrintDataOnInitialSave()
        {
            const string username = "rmcdonald";
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(
                e => e.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) ==
                     new Claim(ClaimTypes.NameIdentifier, username ));
            
            int userId = -1;

            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user = SampleData.CreateCheeseBurgerUser();
                dbContext.Users.Add(user);
                var user2 = SampleData.CreateChickenUser();
                dbContext.Users.Add(user2);

                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                userId = user.Id;
            }

            using (var dbContext = new ApplicationDbContext(Options))
            {
                User user = await dbContext.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
                Assert.IsNotNull(user);
                Assert.AreEqual(username, user.CreatedBy);
                Assert.AreEqual(username, user.ModifiedBy);
                Assert.AreEqual(SampleData.CheeseBurgerFirstName, user.FirstName);
            }

        }
        
        [TestMethod]
        public async Task CreateUser_ShouldSetFingerPrintDataOnUpdate()
        {
            const string firstUsername = "rmcdonald";
            const string secondUsername = "csanders";
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(
                e => e.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) ==
                     new Claim(ClaimTypes.NameIdentifier, firstUsername ));
            
            int userId = -1;

            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user = SampleData.CreateCheeseBurgerUser();
                dbContext.Users.Add(user);
                var user2 = SampleData.CreateChickenUser();
                dbContext.Users.Add(user2);

                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                userId = user.Id;
            }

            httpContextAccessor = Mock.Of<IHttpContextAccessor>(
                e => e.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) ==
                     new Claim(ClaimTypes.NameIdentifier, secondUsername ));
            
            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                User user = await dbContext.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
                user.FirstName = SampleData.ChickenFirstName;
                user.LastName = SampleData.ChickenLastName;

                await dbContext.SaveChangesAsync();

            }

            using (var dbContext = new ApplicationDbContext(Options))
            {
                User user = await dbContext.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
                Assert.IsNotNull(user);
                Assert.AreEqual(firstUsername, user.CreatedBy);
                Assert.AreEqual(secondUsername, user.ModifiedBy);
            }

        }
    }
}
