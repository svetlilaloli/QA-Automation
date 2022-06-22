using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;

namespace Desktop.Tests
{
    public class DesktopTests
    {
        private const string AppiumServerUrl = "http://127.0.0.1:4723/wd/hub";
        private const string App = @"C:\Users\svetl\Documents\QAAutomation\ContactBook\ContactBook-DesktopClient\ContactBook-DesktopClient.exe";
        private const string ApiUrl = "https://contactbook.nakov.repl.co/api";
        private WindowsDriver<WindowsElement> driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            AppiumOptions options = new()
            {
                PlatformName = "Windows"
            };
            options.AddAdditionalCapability("app", App);
            driver = new(new Uri(AppiumServerUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new(driver, TimeSpan.FromSeconds(5));
        }

        [Test]
        public void Test_SearchContact_()
        {
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(ApiUrl);
            driver.FindElementByAccessibilityId("buttonConnect").Click();
            // switch window
            string currentWindow = driver.WindowHandles[0];
            driver.SwitchTo().Window(currentWindow);

            var searchField = driver.FindElementByAccessibilityId("textBoxSearch");
            searchField.Clear();
            searchField.SendKeys("steve");
            driver.FindElementByAccessibilityId("buttonSearch").Click();

            wait.Until(el => driver.FindElementByAccessibilityId("labelResult"));

            string firstName = driver.FindElement(By.XPath("//Edit[@Name=\"FirstName Row 0, Not sorted.\"]")).Text;
            string lastName = driver.FindElement(By.XPath("//Edit[@Name=\"LastName Row 0, Not sorted.\"]")).Text;
            
            Assert.Multiple(() =>
            {
                Assert.That(firstName, Is.EqualTo("Steve"));
                Assert.That(lastName, Is.EqualTo("Jobs"));
            });
        }

        [TearDown]
        public void Close()
        {
            driver.Quit();
        }
    }
}