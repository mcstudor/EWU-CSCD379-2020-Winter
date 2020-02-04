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
    public class GroupControllerTests
    {
        //Mock service initialized during TestInit
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Mock<IGroupService> _MockGroupService;
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        [TestInitialize]
        public void TestInitialize()
        {
            _MockGroupService = new Mock<IGroupService>();
        }

        [TestMethod]
        public void Create_GroupService_Success()
        {
            using (_ = new GroupController(_MockGroupService.Object)) 
            { }
        
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithoutService_Fails()
        {
            using (_ = new GroupController(null!))
            { }
        }

        [TestMethod]
        public async Task GetById_WithExistingAuthor_Success()
        {
            Group group = SampleData.CreateFastFoodGroup();
            int groupId = 42;
            _MockGroupService.Setup(svs => 
                svs.FetchByIdAsync(groupId))
                    .ReturnsAsync(group);

            using (var controller = new GroupController(_MockGroupService.Object))
            {

                ActionResult<Group> actual = await controller.Get(groupId);

                Assert.IsTrue(actual.Result is OkObjectResult);
            }
        }

        [TestMethod]
        public async Task GetById_WithoutExistingGroup_NotFound()
        {
            _MockGroupService.Setup(svs => svs.FetchByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Group)null!);

            using (var controller = new GroupController(_MockGroupService.Object))
            {

                ActionResult<Group> actual = await controller.Get(42);

                Assert.IsTrue(actual.Result is NotFoundResult);
            }
        }

        [TestMethod]
        public async Task FetchAllGroups_ExistingGroups_Success()
        {
            IEnumerable<Group> groups = SampleData.CreateManyGroups();

            _MockGroupService.Setup(svs => svs.FetchAllAsync())
                .ReturnsAsync(groups.ToList);

            using (var controller = new GroupController(_MockGroupService.Object))
            {

                IEnumerable<Group> actual = await controller.Get();

                Assert.IsTrue(actual.Any());
                Assert.AreEqual(groups.Count(), actual.Count());
            }
        }

        [TestMethod]
        public async Task PostGroup_Accepted_ReturnsGroup()
        {
            Group group = SampleData.CreateFastFoodGroup();
            _MockGroupService.Setup(svs => svs.InsertAsync(group))
                .ReturnsAsync(group);

            using (var controller = new GroupController(_MockGroupService.Object))
            {
                Group actual = await controller.Post(group);
                Assert.AreEqual(group, actual);
            }
        }

        [TestMethod]
        public async Task PutGroup_Accepted_ReturnsGroup()
        {
            Group group = SampleData.CreateFastFoodGroup();
            int groupId = 42;
            _MockGroupService.Setup(svs => svs.UpdateAsync(42, It.IsAny<Group>()))
                .ReturnsAsync(group);

            using (var controller = new GroupController(_MockGroupService.Object))
            {
                ;

                ActionResult<Group> actual = await controller.Put(groupId, group);

                Assert.IsNotNull(actual.Value);
                Assert.AreEqual(group, actual.Value);
            }
        }

        [TestMethod]
        public async Task PutGroup_NoGroupFound_ReturnNotFound()
        {
            Group group = SampleData.CreateFastFoodGroup();
            int groupId = 42;
            _MockGroupService.Setup(svs => svs.UpdateAsync(groupId, It.IsAny<Group>()))
                .ReturnsAsync((Group) null!);

            using (var controller = new GroupController(_MockGroupService.Object))
            {
                ActionResult<Group> actual = await controller.Put(groupId, group);

                Assert.IsTrue(actual.Result is NotFoundResult);
            }

        }

        [TestMethod]
        public async Task DeleteGroup_GroupFound_Success()
        {
            int groupId = 42;
            _MockGroupService.Setup(svs => svs.DeleteAsync(groupId)).ReturnsAsync(true);

            using (var controller = new GroupController(_MockGroupService.Object))
            {
                ActionResult actual = await controller.Delete(groupId);
                Assert.IsTrue(actual is OkResult);
            }
        }

        [TestMethod]
        public async Task DeleteGroup_GroupNotFound_ReturnsNotFound()
        {
            int groupId = 42;
            _MockGroupService.Setup(svs => svs.DeleteAsync(groupId)).ReturnsAsync(false);

            using (var controller = new GroupController(_MockGroupService.Object))
            {
                ActionResult actual = await controller.Delete(groupId);
                Assert.IsTrue(actual is NotFoundResult);
            }
        }


    }
}
