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
    public class UserControllerTests
    {
        //Mock service initialized during TestInit
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Mock<IUserService> _MockUserService;
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        [TestInitialize]
        public void TestInitialize()
        {
            _MockUserService = new Mock<IUserService>();
        }

        [TestMethod]
        public void Create_UserService_Success()
        {
            using (_ = new UserController(_MockUserService.Object)) 
            { }
        
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithoutService_Fails()
        {
            using (_ = new UserController(null!))
            { }
        }

        [TestMethod]
        public async Task GetById_WithExistingAuthor_Success()
        {
            User user = SampleData.CreateChickenUser();
            int userId = 42;
            _MockUserService.Setup(svs => 
                svs.FetchByIdAsync(userId))
                    .ReturnsAsync(user);

            using (var controller = new UserController(_MockUserService.Object))
            {

                ActionResult<User> actual = await controller.Get(userId);

                Assert.IsTrue(actual.Result is OkObjectResult);
            }
        }

        [TestMethod]
        public async Task GetById_WithoutExistingUser_NotFound()
        {
            _MockUserService.Setup(svs => svs.FetchByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null!);

            using (var controller = new UserController(_MockUserService.Object))
            {

                ActionResult<User> actual = await controller.Get(42);

                Assert.IsTrue(actual.Result is NotFoundResult);
            }
        }

        [TestMethod]
        public async Task FetchAllUsers_ExistingUsers_Success()
        {
            IEnumerable<User> users = SampleData.CreateManyUsers();

            _MockUserService.Setup(svs => svs.FetchAllAsync())
                .ReturnsAsync(users.ToList);

            using (var controller = new UserController(_MockUserService.Object))
            {

                IEnumerable<User> actual = await controller.Get();

                Assert.IsTrue(actual.Any());
                Assert.AreEqual(users.Count(), actual.Count());
            }
        }

        [TestMethod]
        public async Task PostUser_Accepted_ReturnsUser()
        {
            User user = SampleData.CreateChickenUser();
            _MockUserService.Setup(svs => svs.InsertAsync(user))
                .ReturnsAsync(user);

            using (var controller = new UserController(_MockUserService.Object))
            {
                User actual = await controller.Post(user);
                Assert.AreEqual(user, actual);
            }
        }

        [TestMethod]
        public async Task PutUser_Accepted_ReturnsUser()
        {
            User user = SampleData.CreateChickenUser();
            int userId = 42;
            _MockUserService.Setup(svs => svs.UpdateAsync(42, It.IsAny<User>()))
                .ReturnsAsync(user);

            using (var controller = new UserController(_MockUserService.Object))
            {
                ;

                ActionResult<User> actual = await controller.Put(userId, user);

                Assert.IsNotNull(actual.Value);
                Assert.AreEqual(user, actual.Value);
            }
        }

        [TestMethod]
        public async Task PutUser_NoUserFound_ReturnNotFound()
        {
            User user = SampleData.CreateChickenUser();
            int userId = 42;
            _MockUserService.Setup(svs => svs.UpdateAsync(userId, It.IsAny<User>()))
                .ReturnsAsync((User) null!);

            using (var controller = new UserController(_MockUserService.Object))
            {
                ActionResult<User> actual = await controller.Put(userId, user);

                Assert.IsTrue(actual.Result is NotFoundResult);
            }

        }

        [TestMethod]
        public async Task DeleteUser_UserFound_Success()
        {
            int userId = 42;
            _MockUserService.Setup(svs => svs.DeleteAsync(userId)).ReturnsAsync(true);

            using (var controller = new UserController(_MockUserService.Object))
            {
                ActionResult actual = await controller.Delete(userId);
                Assert.IsTrue(actual is OkResult);
            }
        }

        [TestMethod]
        public async Task DeleteUser_UserNotFound_ReturnsNotFound()
        {
            int userId = 42;
            _MockUserService.Setup(svs => svs.DeleteAsync(userId)).ReturnsAsync(false);

            using (var controller = new UserController(_MockUserService.Object))
            {
                ActionResult actual = await controller.Delete(userId);
                Assert.IsTrue(actual is NotFoundResult);
            }
        }


    }
}
