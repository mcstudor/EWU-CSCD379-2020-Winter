using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;
using NUnit.Framework;

namespace SecretSanta.Business.Tests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void Create_User_Success()
        {
            var expectedInt = 0001;
            var expectedString = "Test String";
            var otherUser = new User(2, "", "", new List<Gift>());
            var expectedGifts = new List<Gift>
            {
                new Gift(1, "", "", new Uri("http://www.fakeurl.com"), otherUser),
                new Gift(2, "", "", new Uri("http://www.fakeurl.com"), otherUser),
                new Gift(3, "", "", new Uri("http://www.fakeurl.com"), otherUser)
            };

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
    }
}
