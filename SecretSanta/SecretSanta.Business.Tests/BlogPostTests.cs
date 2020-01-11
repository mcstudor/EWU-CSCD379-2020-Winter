
using System;
using System.Net.Mime;

namespace SecretSanta.Business.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BlogPostTests
    {
        [TestMethod]
        public void Create_BlogPost_Success()
        {
            const string title = "Sample Blog Post";
            const string content = "Hello world";
            const string author = "Some Author";
            var date = DateTime.Now;

            BlogPost blogPost = new BlogPost(
                title,
                content,
                date,
                author);

            Assert.AreEqual(title, blogPost.Title, "Title Value is unexpected");
            Assert.AreEqual(content, blogPost.Content, "Content Value is unexpected");
            Assert.AreEqual(author, blogPost.Author, "Author Value is unexpected");
            Assert.AreEqual(date, blogPost.Date, "Author Value is unexpected");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_VerifyPropertiesAreNotNull_NotNull()
        {
            BlogPost blogPost = new BlogPost(null, "", DateTime.Now, "");

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Properties_AssignNull_ThrowArgumentNullException()
        {
            BlogPost blogPost = new BlogPost("", "", DateTime.Now, "");
            blogPost.Content = null;
        }
    }
}
