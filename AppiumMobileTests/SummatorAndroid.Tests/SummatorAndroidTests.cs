using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace SummatorAndroid.Tests
{
    public class SummatorAndroidTests
    {
        private const string AppiumServer = "http://127.0.0.1:4723/wd/hub";
        private const string App = @"C:\Users\svetl\Documents\QAAutomation\com.example.androidappsummator.apk";
        private AndroidDriver<AndroidElement> driver;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var options = new AppiumOptions()
            {
                PlatformName = "Android"
            };
            options.AddAdditionalCapability("app", App);
            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumServer), options);
        }

        [TestCase("", "", "error")]
        [TestCase("", "asd", "error")]
        [TestCase("+", "", "error")]
        [TestCase("", "-", "error")]
        [TestCase("0", "", "error")]
        [TestCase("", "-0", "error")]
        [TestCase("-0.657", "-9.765", "-10.422")]
        [TestCase("987654321", "-987654321", "0")]
        public void Test_Summator_Android(string num1, string num2, string expectedResult)
        {
            var firstNumber = driver.FindElementById("com.example.androidappsummator:id/editText1");
            firstNumber.Clear();
            firstNumber.SendKeys(num1);
            var secondNumber = driver.FindElementById("com.example.androidappsummator:id/editText2");
            secondNumber.Clear();
            secondNumber.SendKeys(num2);
            driver.FindElementById("com.example.androidappsummator:id/buttonCalcSum").Click();
            var resultField = driver.FindElementById("com.example.androidappsummator:id/editTextSum");
            Assert.That(resultField.Text, Is.EqualTo(expectedResult));
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
        }
    }
}