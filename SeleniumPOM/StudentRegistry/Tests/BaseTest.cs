using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace StudentRegistry.Tests
{
    public class BaseTest
    {
        protected IWebDriver driver;
        [OneTimeSetUp]
        protected void OneTimeSetUp()
        {
            driver = new ChromeDriver();
        }
        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            driver.Quit();
        }
    }
}
