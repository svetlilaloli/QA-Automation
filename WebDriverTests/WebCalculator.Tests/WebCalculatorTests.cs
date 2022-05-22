using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Diagnostics;

namespace WebCalculator.Tests
{
    public class WebCalculatorTests
    {
        private ChromeOptions options;
        private IWebDriver driver;
        private IWebElement inputFirstNumber;
        private IWebElement inputSecondNumber;
        private IWebElement selectOperation;
        private IWebElement calcButton;
        private IWebElement resetButton;
        private IWebElement divResult;
        private const int maxIntValue = int.MaxValue;
        [OneTimeSetUp]
        public void Setup()
        {
            options = new ChromeOptions();
            if (!Debugger.IsAttached)
            {
                options.AddArguments("--headless");
            }
            driver = new ChromeDriver(options);
            //driver = new FirefoxDriver();
            driver.Url = "https://number-calculator.nakov.repl.co";
            inputFirstNumber = driver.FindElement(By.Id("number1"));
            inputSecondNumber = driver.FindElement(By.Id("number2"));
            selectOperation = driver.FindElement(By.Id("operation"));
            calcButton = driver.FindElement(By.Id("calcButton"));
            resetButton = driver.FindElement(By.Id("resetButton"));
            divResult = driver.FindElement(By.Id("result"));
        }
        // valid input 
        [TestCase("3", "+", "33", "36")]
        [TestCase("0.12", "-", "-2.09", "2.21")]
        [TestCase("1.5e53", "*", "150", "2.25e+55")]
        [TestCase("0", "/", "9.3456", "0")]
        [TestCase(maxIntValue, "*", maxIntValue, "4611686014132420609")]
        // invalid input
        [TestCase("-9.321", "/", "0", "-Infinity")]
        [TestCase("", "+", "9", "invalid input")]
        [TestCase("654", "-", "", "invalid input")]
        [TestCase("asd", "/", "20", "invalid input")]
        [TestCase("20", "*", "asd", "invalid input")]
        [TestCase("55.09", "!!!", "20", "invalid operation")]
        [TestCase("65.09", "", "20", "invalid operation")]
        [TestCase("43.9", "-- select an operation --", "20", "invalid operation")]
        public void Test_Calculation(string num1, string op, string num2, string expectedResult)
        {
            // make sure the fields are clear
            resetButton.Click();

            inputFirstNumber.SendKeys(num1);
            selectOperation.SendKeys(op);
            inputSecondNumber.SendKeys(num2);
            calcButton.Click();
            Assert.That(divResult.Text, Is.EqualTo($"Result: {expectedResult}"));
        }
        [Test]
        public void Test_ResetButton()
        {
            resetButton.Click();
            inputFirstNumber.SendKeys("1");
            selectOperation.SendKeys("*");
            inputSecondNumber.SendKeys("1");
            calcButton.Click();

            Assert.AreNotEqual(inputFirstNumber.GetAttribute("value"), "");
            Assert.AreNotEqual(inputSecondNumber.GetAttribute("value"), "");
            Assert.AreNotEqual(selectOperation.GetAttribute("value"), "-- select an operation --");

            resetButton.Click();

            Assert.AreEqual(inputFirstNumber.GetAttribute("value"), "");
            Assert.AreEqual(inputSecondNumber.GetAttribute("value"), "");
            Assert.AreEqual(selectOperation.GetAttribute("value"), "-- select an operation --");
        }
        [OneTimeTearDown]
        public void ShutDown()
        {
            driver.Quit();
        }
    }
}