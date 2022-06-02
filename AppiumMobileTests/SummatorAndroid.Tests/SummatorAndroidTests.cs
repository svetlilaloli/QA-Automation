using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace SummatorAndroid.Tests
{
    public class SummatorAndroidTests
    {
        private AndroidDriver<AndroidElement> driver;
        private const string App = @"C:\Users\svetl\Documents\QAAutomation\com.example.androidappsummator.apk";
        private const string AppiumServer = "http://127.0.0.1:4723/wd/hub";
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new AppiumOptions() 
            {
                PlatformName = "Android" 
            };
            options.AddAdditionalCapability("app", App);
            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumServer), options);
        }

        [TestCase("7", "-0.23456", "6.76544")]
        [TestCase("", "-0.5", "error")]
        public void Test_Summator(string num1, string num2, string expectedResult)
        {
            var field1 = driver.FindElementById("com.example.androidappsummator:id/editText1");
            var field2 = driver.FindElementById("com.example.androidappsummator:id/editText2");
            var resultField = driver.FindElementById("com.example.androidappsummator:id/editTextSum");
            var calcButton = driver.FindElementById("com.example.androidappsummator:id/buttonCalcSum");
            
            field1.Clear();
            field1.SendKeys(num1);
            field2.Clear();
            field2.SendKeys(num2);
            calcButton.Click();
            
            Assert.That(resultField.Text, Is.EqualTo(expectedResult));
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
        }
    }
}