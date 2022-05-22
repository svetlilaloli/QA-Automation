using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;

namespace UrlShortener.Tests
{
    public class UrlShortenerTests
    {
        private IWebDriver driver;
        private ChromeOptions options;
        [OneTimeSetUp]
        public void Setup()
        {
            options = new ChromeOptions();
            if (!Debugger.IsAttached)
            {
                options.AddArguments("--headless");
            }
            driver = new ChromeDriver(options);
        }

        [Test]
        public void Test_HomePage_TitleMatches()
        {
            driver.Url = "https://shorturl.nakov.repl.co";
            string titleElement = driver.Title;
            StringAssert.Contains("URL Shortener", titleElement);
        }
        [Test]
        public void Test_ShortUrlsPage_TableFirstAndSecondCells()
        {
            driver.Url = "https://shorturl.nakov.repl.co/urls";
            string titleText;
            string firstTableCellText;
            string secondTableCellText;

            titleText = driver.FindElement(By.CssSelector("body > main > h1")).Text;
            firstTableCellText = driver.FindElement(By.CssSelector("body > main > table > tbody > tr:nth-child(1) > td:nth-child(1)")).Text;
            secondTableCellText = driver.FindElement(By.CssSelector("body > main > table > tbody > tr:nth-child(1) > td:nth-child(2)")).Text;

            StringAssert.Contains("Short URLs", titleText);
            StringAssert.Contains("https://nakov.com", firstTableCellText);
            StringAssert.Contains("http://shorturl.nakov.repl.co/go/nak", secondTableCellText);
        }
        [Test]
        public void Test_AddUrlPage_AddValidData()
        {
            driver.Url = "https://shorturl.nakov.repl.co/add-url";
            string titleText = driver.FindElement(By.CssSelector("body > main > h1")).Text;
            string urlToAdd = $"http://myurlat{DateTime.Now.Ticks}.com";

            driver.FindElement(By.Id("url")).SendKeys(urlToAdd);
            driver.FindElement(By.CssSelector("body > main > form > table > tbody > tr:nth-child(3) > td > button")).Click();
            driver.Url = "https://shorturl.nakov.repl.co/urls";
            var links = driver.FindElements(By.LinkText(urlToAdd));
            
            StringAssert.Contains("Add Short URL", titleText);
            Assert.AreEqual(1, links.Count);
        }
        [TestCase("asd")]
        [TestCase("asd.com")]
        public void Test_AddUrlPage_AddInvalidData(string data)
        {
            driver.Url = "https://shorturl.nakov.repl.co/add-url";
            string titleText = driver.FindElement(By.CssSelector("body > main > h1")).Text;

            driver.FindElement(By.Id("url")).SendKeys(data);
            driver.FindElement(By.CssSelector("body > main > form > table > tbody > tr:nth-child(3) > td > button")).Click();
            bool errorMessageIsDisplayed = driver.FindElement(By.CssSelector("body > div")).Displayed;

            StringAssert.Contains("Add Short URL", titleText);
            Assert.IsTrue(errorMessageIsDisplayed);
        }
        [Test]
        public void Test_ShortUrlsPage_VisitsAreUpdated()
        {
            driver.Url = "https://shorturl.nakov.repl.co/urls";
            string urlToOpen = "http://shorturl.nakov.repl.co/go/nak";
            string visitsCountText;
            int initialCount;
            int incrementedCount;
            bool isNumber;

            visitsCountText = driver.FindElement(By.CssSelector("body > main > table > tbody > tr:nth-child(1) > td:nth-child(4)")).Text;
            isNumber = int.TryParse(visitsCountText, out initialCount);
            if (!isNumber)
            {
                Assert.Fail("Initial visits count is not a number");
            }
            driver.FindElement(By.LinkText(urlToOpen)).Click();
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            StringAssert.Contains("Svetlin Nakov - Svetlin Nakov – Official Web Site and Blog", driver.Title);
            driver.SwitchTo().Window(driver.WindowHandles[0]);
            
            isNumber = int.TryParse(driver.FindElement(By.CssSelector("body > main > table > tbody > tr:nth-child(1) > td:nth-child(4)")).Text, out incrementedCount);
            if (!isNumber)
            {
                Assert.Fail("Incremented visits count is not a number");
            }
            Assert.That(incrementedCount, Is.EqualTo(initialCount + 1));
        }
        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}