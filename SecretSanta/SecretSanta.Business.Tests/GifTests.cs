using System;
using System.Collections.Generic;
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
            Uri expectedUrl = new Uri("fakeUrl");
            const string expectedString = "Test String";
            var expectedUser = new User(id: 0, firstName: "", lastName: "", gifts: new List<Gift>());
            var gift = new Gift(id: expectedInt, 
                            title: expectedString, 
                            description: expectedString, 
                            url: expectedUrl, 
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
                        Assert.AreEqual(expected: expectedUrl, actual: actualUri);
                        break;
                }
            }
        }
    }
}