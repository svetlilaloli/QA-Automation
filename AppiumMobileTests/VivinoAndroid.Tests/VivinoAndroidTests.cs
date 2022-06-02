using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;

namespace VivinoAndroid.Tests
{
    public class VivinoAndroidTests
    {
        private const string App = @"C:\Users\svetl\Documents\QAAutomation\vivino.web.app.apk";
        private const string AppPackage = "vivino.web.app";
        private const string AppActivity = "com.sphinx_solution.Launcher";
        private const string AppiumServer = "http://127.0.0.1:4723/wd/hub";
        private const string Email = "test_vivino@gmail.com";
        private const string Password = "p@ss987654321";
        private AndroidDriver<AndroidElement> driver;

        [SetUp]
        public void Setup()
        {
            var options = new AppiumOptions()
            {
                PlatformName = "Android"
            };
            options.AddAdditionalCapability("app", App);
            options.AddAdditionalCapability("appPackage", AppPackage);
            options.AddAdditionalCapability("appActivity", AppActivity);
            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }
        [Test]
        public void Test_SearchForExsistingWine()
        {
            // already have an account
            driver.FindElementById("vivino.web.app:id/txthaveaccount").Click();
            // fill in email
            var emailField = driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.ScrollView/android.widget.RelativeLayout/android.widget.LinearLayout/android.widget.LinearLayout[1]/android.view.ViewGroup/android.widget.LinearLayout[1]/android.widget.FrameLayout/android.widget.EditText");
            emailField.Clear();
            emailField.SendKeys(Email);
            // fill in password
            var passwordField = driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.ScrollView/android.widget.RelativeLayout/android.widget.LinearLayout/android.widget.LinearLayout[1]/android.view.ViewGroup/android.widget.LinearLayout[2]/android.widget.FrameLayout/android.widget.EditText");
            passwordField.Clear();
            passwordField.SendKeys(Password);
            // Login button
            driver.FindElementById("vivino.web.app:id/action_signin").Click();
            // search button
            driver.FindElementById("vivino.web.app:id/wine_explorer_tab").Click();
            // search field
            driver.FindElementById("vivino.web.app:id/search_vivino").Click();
            // search page
            var searchField = driver.FindElementById("vivino.web.app:id/editText_input");
            searchField.SendKeys("Katarzyna Reserve Red 2006");
            // first result pane
            var resultsList = driver.FindElementById("vivino.web.app:id/listviewWineListActivity");
            var firstResult = resultsList.FindElementByClassName("android.widget.LinearLayout");
            firstResult.Click();
            // get rating
            var rating = driver.FindElementById("vivino.web.app:id/rating");
            Assert.That(double.Parse(rating.Text), Is.InRange(1.00, 5.00));
            // get name
            var wineName = driver.FindElementByAndroidUIAutomator(
                "new UiScrollable(new UiSelector().scrollable(true))" +
                ".scrollIntoView(new UiSelector().resourceIdMatches(" +
                "\"vivino.web.app:id/wine_name\"))");
            Assert.That(wineName.Text, Is.EqualTo("Reserve Red 2006"));
            // TODO
            // find highlight_description
            // find fact_text
        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}