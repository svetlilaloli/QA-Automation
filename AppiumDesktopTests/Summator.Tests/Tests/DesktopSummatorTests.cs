using Summator.Tests.Windows;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;

namespace Summator.Tests.Tests
{
    public class DesktopSummatorTests
    {
        //private AppiumLocalService appiumLocalService;
        private WindowsDriver<WindowsElement> driver;
        private SummatorWindow window;
        private const string AppiumServer = "http://127.0.0.1:4723/wd/hub";
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            //appiumLocalService.Start();

            var options = new AppiumOptions()
            {
                PlatformName = "Windows"
            };
            options.AddAdditionalCapability(MobileCapabilityType.App, @"C:\Users\svetl\Documents\QAAutomation\WindowsFormsApp.exe");
            //driver = new WindowsDriver<WindowsElement>(appiumLocalService, options);
            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServer), options);
            window = new SummatorWindow(driver);
        }

        [TestCase("0", "2.5677", "2.5677")]
        [TestCase("-1.4543", "0", "-1.4543")]
        [TestCase("0", "-0", "0")]
        [TestCase("9876543219876", "-987654321987654321", "-987644445444434445")]
        [TestCase("-asd", "7", "error")]
        [TestCase("", "", "error")]
        [TestCase("=", "-", "error")]
        [TestCase("0", "d", "error")]
        [TestCase("-", "+", "error")]
        public void Test_Sum(string num1, string num2, string expectedResult)
        {
            window.ClearForm();
            window.Sum(num1, num2);
            Assert.That(window.GetResult(), Is.EqualTo(expectedResult));
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.CloseApp();
            driver.Quit();
            //appiumLocalService?.Dispose();
        }
    }
}