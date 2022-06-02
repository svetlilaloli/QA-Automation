using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;

namespace ContactBookAndroid.Tests
{
    public class ContactBookAndroidTests
    {
        private const string App = @"C:\Users\svetl\Documents\QAAutomation\contactbook-androidclient.apk";
        private const string AppiumServer = "http://127.0.0.1:4723/wd/hub";
        private const string ApiUrl = "https://contactbook.nakov.repl.co/api";
        private AndroidDriver<AndroidElement> driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            var options = new AppiumOptions()
            {
                PlatformName = "Android"
            };
            options.AddAdditionalCapability("app", App);
            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        [TestCase("Steve", "Jobs", "1")]
        [TestCase("Jones", "", "0")]
        [TestCase("e", "", "3")]
        public void Test_ContactSearch(string firstN, string lastN, string expectedNumber)
        {
            var api = driver.FindElementById("contactbook.androidclient:id/editTextApiUrl");
            api.Clear();
            api.SendKeys(ApiUrl);
            driver.FindElementById("contactbook.androidclient:id/buttonConnect").Click();
            
            var searchContactTextBox = driver.FindElementById("contactbook.androidclient:id/editTextKeyword");
            searchContactTextBox.Clear();
            searchContactTextBox.SendKeys(firstN);
            driver.FindElementById("contactbook.androidclient:id/buttonSearch").Click();

            var result = driver.FindElementById("contactbook.androidclient:id/textViewSearchResult");
            wait.Until(t => result.Text != "");
            Assert.That(result.Text, Does.Contain($"Contacts found: {expectedNumber}"));

            if (expectedNumber == "1")
            {
                var firstName = driver.FindElementById("contactbook.androidclient:id/textViewFirstName");
                Assert.That(firstName.Text, Is.EqualTo(firstN));

                var lastName = driver.FindElementById("contactbook.androidclient:id/textViewLastName");
                Assert.That(lastName.Text, Is.EqualTo(lastN));
            }
        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}