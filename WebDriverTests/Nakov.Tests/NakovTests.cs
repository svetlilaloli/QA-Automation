using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Diagnostics;

namespace Nakov.Tests
{
    public class NakovTests
    {
        private IWebDriver driver;
        private ChromeOptions options;
        [OneTimeSetUp]
        public void Setup()
        {
            options = new ChromeOptions();
            if (!Debugger.IsAttached)
            {
                options.AddArguments("--headless", "--window-size=1920,1200");
            }
            driver = new ChromeDriver(options);
            //driver = new FirefoxDriver();
            driver.Url = "https://nakov.com";
        }
        [Test]
        public void Test_Search()
        {
            driver.FindElement(By.Id("sh")).Click();
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