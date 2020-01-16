using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace SecretSanta.Business.Tests
{
    
    [TestFixture]
    public class GiftTests
    {
        [Test]
        public void Create_Gift_Success()
        {

            const int expectedInt = 0001;
            const string expectedString = "Test String";
            var expectedUser = new User(id: 0, firstName: "First Name", lastName: "Last Name", gifts: new List<Gift>());
            var gift = new Gift(id: expectedInt, 
                            title: expectedString, 
                            description: expectedString, 
                            url: expectedString, 
                                user: expectedUser);

            foreach (var property in typeof(Gift).GetProperties())
            {
                switch (property.GetValue(obj: gift, null))
                {
                    case int actualInt:
                        Assert.AreEqual(expected: expectedInt, actual: actualInt);
                        break;
                    case string actualString:
                        Assert.AreEqual(expected: expectedString, actual: actualString);
                        break;
                    case User actualUser:
                        Assert.AreEqual(expected: expectedUser, actual: actualUser);
                        break;
                    case Uri actualUri:
                        Assert.AreEqual(expected: expectedString, actual: actualUri);
                        break;
                }
            }
        }

        [TestCase(null, "Description", "Url")]
        [TestCase("Title", null, "Url")]
        [TestCase("Title", "Description", null)]

        [TestCase("", "Description", "Url")]
        [TestCase("Title", "", "Url")]
        [TestCase("Title", "Description", "")]
        public void Create_GiftNullOrWhite_Failure(string title, string description, string url)
        {
            Assert.Throws<ArgumentException>(
                () => _ = new Gift(1, title, description, url, new User(1, "First Name", "Last Name", new List<Gift>())));
        }

        [TestCase("")]
        [TestCase(null)]
        public void Set_GiftInvalidValues_Failure(string setValue)
        {
            var testGift = new Gift(1,
                "Test",
                "Test",
                "Test",
                new User(id: 0, firstName: "First Name", lastName: "Last Name", gifts: new List<Gift>()));
            foreach (var property in typeof(Gift).GetProperties(BindingFlags.Public))
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    try
                    {
                        property.SetValue(testGift, setValue);
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