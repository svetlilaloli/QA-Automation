using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SummatorWithPOM.PageObjects;

namespace SummatorWithPOM.Tests
{
    public class SumTwoNumbersPageTests
    {
        private SumTwoNumbersPage page;
        private IWebDriver driver;
        private ChromeOptions options;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            options = new ChromeOptions();
            if (!Debugger.IsAttached)
            {
                options.AddArguments("--headless");
            }
            driver = new ChromeDriver(options);
            page = new SumTwoNumbersPage(driver);
            page.Open();
        }
        [Test]
        public void Test_Content()
        {
            Assert.AreEqual("Sum Two Numbers", page.GetPageTitle());
            Assert.AreEqual("Sum Two Numbers", page.GetPageHeading());
            Assert.AreEqual("", page.FirstNumberInputElement.Text);
            Assert.AreEqual("", page.SecondNumberInputElement.Text);
            Assert.AreEqual("Calculate", page.CalculateButton.GetAttribute("value"));
            Assert.AreEqual("Reset", page.ResetButton.GetAttribute("value"));
            Assert.AreEqual("", page.ResultDivElement.Text);
        }
        // valid input
        [TestCase("12345678901234563456788888", "1234567890123456789", "1.2345680135802454e+25")]
        [TestCase("0", "0", "0")]
        // invalid input
        [TestCase("", "78", "invalid input")]
        [TestCase("0.123", "", "invalid input")]
        [TestCase("--3.45", "89", "invalid input")]
        [TestCase("-6.35", "++5678", "invalid input")]
        [TestCase("asd", "7856", "invalid input")]
        [TestCase("677", "dsa", "invalid input")]
        public void Test_SumInvalidInput(string num1, string num2, string expectedResult)
        {
            page.ResetForm();
            page.AddNumbers(num1, num2);
            Assert.AreEqual($"{expectedResult}", page.GetResult());
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
        }
    }
}
