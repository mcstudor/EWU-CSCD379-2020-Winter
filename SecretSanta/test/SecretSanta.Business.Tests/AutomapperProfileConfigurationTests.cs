using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class AutomapperProfileConfigurationTests
    {
        //Set during initialize
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private IMapper _Mapper;
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        [TestInitialize]
        public void TestInitialize()
        {
            _Mapper = AutomapperProfileConfiguration.CreateMapper();
        }

        class MockUser : User
        {
            public MockUser(int id, string firstName, string lastName)
                : base(firstName, lastName)
            {
                base.Id = id;
            }
        }

        class MockGift : Gift
        {
            public MockGift(int id, string title, string description, string url)
            {
                base.Id = id;
                base.Title = title;
                base.Description = description;
                base.Url = url;
            }
        }

        class MockGroup : Group
        {
            public MockGroup(int id, string title)
                : base(title)
            {
                base.Id = id;
            }
        }

        [TestMethod]
        public void Map_User_SuccessWithNoIdMapped()
        {
            User source = new MockUser(42,
                SampleData.CheeseBurgerFirstName,
                SampleData.CheeseBurgerLastName);
            User target = _Mapper.Map<User>(source);
            Assert.AreNotEqual(source.Id, target.Id);
            Assert.AreEqual((source.FirstName, source.LastName),
                (target.FirstName, target.LastName));
        }

        [TestMethod]
        public void Map_Gift_SuccessWithNoIdMapped()
        {
            Gift source = new MockGift(42,
                SampleData.CheeseBurgerTitle,
                SampleData.CheeseBurgerDescription,
                SampleData.CheeseBurgerUrl);
            Gift target = _Mapper.Map<Gift>(source);
            Assert.AreNotEqual(source.Id, target.Id);
            Assert.AreEqual((source.Title, source.Description, source.Url),
                (target.Title, target.Description, target.Url));
        }

        [TestMethod]
        public void Map_Group_SuccessWithNoIdMapped()
        {
            Group source = new MockGroup(42,
                SampleData.FastFoodTitle);
            Group target = _Mapper.Map<Group>(source);
            Assert.AreNotEqual(source.Id, target.Id);
            Assert.AreEqual(source.Title, target.Title);
        }



    }
}

