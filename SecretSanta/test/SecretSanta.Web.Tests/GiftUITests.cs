using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SecretSanta.Web.Api;

namespace SecretSanta.Web.Tests
{
    [TestClass]
    public class GiftUITests
    {
        [NotNull]
        public TestContext? TestContext { get; set; }
        [NotNull]
        public IWebDriver Driver { get; set; }

        private static string ApiUrl { get; } = "https://*:44388";
        private static string WebUrl { get; } = "https://*:5001";
        private static Process? ApiHostProcess {get; set; }
        private static Process? WebHostProcess {get; set; }
        private static string AppURL { get; } = "https://localhost:5001/Gifts";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            
            ApiHostProcess = Process.Start("dotnet.exe", "run  -p ..\\..\\..\\..\\..\\src\\SecretSanta.Api\\SecretSanta.Api.csproj --environment \"Test\" --Urls " + ApiUrl);
            WebHostProcess = Process.Start("dotnet.exe", "run -p ..\\..\\..\\..\\..\\src\\SecretSanta.Web\\SecretSanta.Web.csproj -- --Url " + WebUrl);
            ApiHostProcess.WaitForExit(5 * 1000);
        }

        private static async Task<string> AddTestUser()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:44388");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                UserClient userClient = new UserClient(httpClient);
                IEnumerable<User> users = await userClient.GetAllAsync();
                if (users.Any())
                {
                    return users.First().FirstName;
                } else 
                { 
                    User response = await userClient.PostAsync(new UserInput
                    {
                        FirstName = "Ronald",
                        LastName = "McDonald"
                    });
                    return response.FirstName;
                }
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ApiHostProcess?.CloseMainWindow();
            ApiHostProcess?.Close();
            WebHostProcess?.CloseMainWindow();
            WebHostProcess?.Close();
        }

        [TestInitialize]
        public void TestInitialize()
        {

            string browser = "Chrome";
            switch (browser)
            {
                case "Chrome":
                    Driver = new ChromeDriver();
                    break;
                default:
                    Driver = new ChromeDriver();
                    break;
            }
            Driver.Manage().Timeouts().ImplicitWait = new System.TimeSpan(0, 0, 10);
        }

        [TestMethod]
        public void GiftCreateButton_Exists_Success()
        {
            Driver.Navigate().GoToUrl(new Uri(AppURL));
            string text = Driver.FindElement(By.Id("gift-create-btn")).Text;
            Assert.IsTrue(text.Contains("Create New"));
        }

        [TestMethod]
        public void GiftCreate_InputFieldsExist_Success()
        {
            Driver.Navigate().GoToUrl(new Uri(AppURL));
            Driver.FindElement(By.Id("gift-create-btn")).Click();
            Driver.FindElement(By.Id("gift-title-input"));
            Driver.FindElement(By.Id("gift-description-input"));
            Driver.FindElement(By.Id("gift-url-input"));
            Driver.FindElement(By.Id("gift-user-input"));
            Driver.FindElement(By.Id("submit"));
        }

        [TestMethod]
        public async Task GiftCreate_CreateGift_GiftExists()
        {
            string expectedTitle = "chicken mcnugget";
            string expectedDescription = "Hot breaded chicken";
            string expectedUrl = "https://chick.en";
            string expectedUserInfo = await AddTestUser();

            Driver.Navigate().GoToUrl(new Uri(AppURL));
            Driver.FindElement(By.Id("gift-create-btn")).Click();
            Driver.FindElement(By.Id("gift-title-input")).SendKeys(expectedTitle);
            Driver.FindElement(By.Id("gift-description-input")).SendKeys(expectedDescription);
            Driver.FindElement(By.Id("gift-url-input")).SendKeys(expectedUrl);
            Driver.FindElement(By.Id("gift-user-input")).SendKeys(expectedUserInfo);
            Driver.FindElement(By.Id("submit")).Click();
            string actualTitle = Driver.FindElement(By.Id("gift-title")).Text;
            string actualDescription = Driver.FindElement(By.Id("gift-description")).Text;
            string actualUrl = Driver.FindElement(By.Id("gift-url")).Text;
            string actualId = Driver.FindElement(By.Id("gift-id")).Text;
            Assert.AreEqual((expectedTitle, expectedDescription, expectedUrl),
                (actualTitle, actualDescription, actualUrl));

            Screenshot ss = ((ITakesScreenshot)Driver).GetScreenshot();
            string path = $"{Directory.GetCurrentDirectory()}CreateGiftTestResult.png";
            ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(path, ScreenshotImageFormat.Png);
            TestContext.AddResultFile(path);

        }

        [TestCleanup()]
        public void TestCleanup()
        {
            Driver.Quit();
        }
    }

}
