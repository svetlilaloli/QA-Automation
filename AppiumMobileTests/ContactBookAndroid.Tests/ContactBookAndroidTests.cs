using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;

namespace ContactBookAndroid.Tests
{
    public class ContactBookAndroidTests
    {
        private const string AppiumServer = "http://127.0.0.1:4723/wd/hub";
        private const string App = @"C:\Users\svetl\Documents\QAAutomation\contactbook-androidclient.apk";
        private const string ApiService = "https://contactbook.nakov.repl.co/api";
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
        [TestCase("e", "", "4")]
        public void Test_ContactBook_Android_Search(string firstName, string lastName, string expectedResult)
        {
            var urlTextField = driver.FindElementById("contactbook.androidclient:id/editTextApiUrl");
            urlTextField.Clear();
            urlTextField.SendKeys(ApiService);
            driver.FindElementById("contactbook.androidclient:id/buttonConnect").Click();

            var searchField = driver.FindElementById("contactbook.androidclient:id/editTextKeyword");
            searchField.Clear();
            searchField.SendKeys(firstName);
            driver.FindElementById("contactbook.androidclient:id/buttonSearch").Click();

            var resultsList = driver.FindElementById("contactbook.androidclient:id/textViewSearchResult");
            wait.Until(t => resultsList.Text != "");
            Assert.That(resultsList.Text, Contains.Substring($"Contacts found: {expectedResult}"));

            if (expectedResult == "1")
            {
                var resultFirstName = driver.FindElementById("contactbook.androidclient:id/textViewFirstName").Text;
                Assert.That(resultFirstName, Is.EqualTo(firstName));

                var resultLastName = driver.FindElementById("contactbook.androidclient:id/textViewLastName").Text;
                Assert.That(resultLastName, Is.EqualTo(lastName));
            }
        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}