using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace VivinoAndroid.Tests
{
    public class VivinoAndroidTests
    {
        private const string AppiumServer = "http://127.0.0.1:4723/wd/hub";
        private const string App = @"C:\Users\svetl\Documents\QAAutomation\vivino.web.app.apk";
        private const string AppPackage = "vivino.web.app";
        private const string AppActivity = "com.sphinx_solution.Launcher";
        private const string Email = "YOUR_EMAIL";
        private const string Password = "YOUR_PASSWORD";
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
        public void Test_Vivino_Search()
        {
            driver.FindElementById("vivino.web.app:id/txthaveaccount").Click();
            var emailField = driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.ScrollView/android.widget.RelativeLayout/android.widget.LinearLayout/android.widget.LinearLayout[1]/android.view.ViewGroup/android.widget.LinearLayout[1]/android.widget.FrameLayout/android.widget.EditText");
            emailField.Clear();
            emailField.SendKeys(Email);
            var passwordField = driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.ScrollView/android.widget.RelativeLayout/android.widget.LinearLayout/android.widget.LinearLayout[1]/android.view.ViewGroup/android.widget.LinearLayout[2]/android.widget.FrameLayout/android.widget.EditText");
            passwordField.Clear();
            passwordField.SendKeys(Password);
            driver.FindElementById("vivino.web.app:id/action_signin").Click();
            // find search tab
            driver.FindElementById("vivino.web.app:id/wine_explorer_tab").Click();
            driver.FindElementById("vivino.web.app:id/search_vivino").Click();
            var searchField = driver.FindElementById("vivino.web.app:id/editText_input");
            searchField.Clear();
            searchField.SendKeys("Katarzyna Reserve Red 2006");

            var resultsList = driver.FindElementById("vivino.web.app:id/listviewWineListActivity");
            resultsList.FindElementByClassName("android.widget.FrameLayout").Click();// returns the first match

            var wineData = driver.FindElementByAndroidUIAutomator(
                "new UiScrollable(new UiSelector().scrollable(true))" +
                ".scrollIntoView(new UiSelector().resourceIdMatches(" +
                "\"vivino.web.app:id/wine_data\"))");

            var rating = wineData.FindElementById("vivino.web.app:id/rating");
            Assert.That(double.Parse(rating.Text), Is.InRange(1.00, 5.00));

            var winePage = driver.FindElementByAndroidUIAutomator(
                "new UiScrollable(new UiSelector().scrollable(true))" +
                ".scrollIntoView(new UiSelector().resourceIdMatches(" +
                "\"vivino.web.app:id/wine_page\"))");

            var wineName = winePage.FindElementById("vivino.web.app:id/wine_name");
            Assert.That(wineName.Text, Is.EqualTo("Reserve Red 2006"));

            // TODO
            // scroll
            // find highlight_description
            // find facts
        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}