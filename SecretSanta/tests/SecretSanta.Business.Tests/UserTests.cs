using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace SecretSanta.Business.Tests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void Create_User_Success()
        {
            const int expectedInt = 0001;
            const string expectedString = "Test String";
            var otherUser = new User(2, "First name", "Last Name", new List<Gift>());
            var expectedGift = new Gift(1, "Title", "Description", "Url", otherUser);
            var expectedGifts = new List<Gift> {expectedGift, expectedGift, expectedGift, expectedGift};

            var user = new User(expectedInt, expectedString, expectedString, expectedGifts);
            foreach (var property in typeof(User).GetProperties())
            {
                switch (property.GetValue(obj: user, null))
                {
                    case int actualInt:
                        Assert.AreEqual(expected: expectedInt, actual: actualInt);
                        break;
                    case string actualString:
                        Assert.AreEqual(expected: expectedString, actual: actualString);
                        break;
                    case IEnumerable<Gift> actualGifts:
                        CollectionAssert.AreEqual(expectedGifts, actualGifts);
                        break;
                }
            }


        }

        [TestCase(null, "Last Name")]
        [TestCase("First Name", null)]
        [TestCase("", "Last Name")]
        [TestCase("First Name", "")]
        public void Create_UserNullOrWhite_Failure(string firstName, string lastName)
        {
            Assert.Throws<ArgumentException>(() => _ = new User(1, firstName, lastName, new List<Gift>()));
        }


        [TestCase("")]
        [TestCase(null)]
        public void Set_UserInvalidValues_Failure(string setValue)
        {
            var testUser = new User(1,
                "Test",
                "Test",
                new List<Gift>());
            foreach (var property in typeof(Gift).GetProperties(BindingFlags.Public))
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    try
                    {
                        property.SetValue(testUser, setValue);
                    }
                    catch (TargetInvocationException e)
                    {
                        if (e.InnerException != null) throw e.InnerException;
                    }
                });
            }
        }
    }
}
