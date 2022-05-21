using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace Nakov.Tests
{
    public class NakovTests
    {
        private IWebDriver driver; 
        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            driver.Url = "https://nakov.com";
        }
        [Test]
        public void Test_Search()
        {
            driver.FindElement(By.XPath("//*[@id=\"sh\"]/a")).Click();
            var searchElement = driver.FindElement(By.Id("s"));
            searchElement.Click();
            searchElement.SendKeys("QA" + Keys.Enter);
            
            Assert.That(driver.FindElement(By.XPath("/html/body/section/hgroup/h3")).Text, Is.EqualTo("Search Results for – \"QA\""));
        }
      [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}