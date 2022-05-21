using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Wikipedia.Tests
{
    public class WikipediaTests
    {
        private IWebDriver driver;
        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Url = "https://wikipedia.org";
        }

        [Test]
        public void Test()
        {
            var searchField = driver.FindElement(By.Id("searchInput"));
            searchField.Click();
            searchField.SendKeys("QA" + Keys.Enter);

            Assert.AreEqual("https://en.wikipedia.org/wiki/QA", driver.Url);
        }
        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}