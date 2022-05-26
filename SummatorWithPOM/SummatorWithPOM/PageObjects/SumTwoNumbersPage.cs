using OpenQA.Selenium;
using System;

namespace SummatorWithPOM.PageObjects
{
    public class SumTwoNumbersPage
    {
        private readonly IWebDriver driver;
        public SumTwoNumbersPage(IWebDriver driver)
        {
            this.driver = driver;
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }
        public string PageUrl => "https://sum-numbers.nakov.repl.co/";
        public IWebElement Heading => driver.FindElement(By.CssSelector("body > form > h1"));
        public IWebElement FirstNumberLabelElement => driver.FindElement(By.CssSelector("body > form > div:nth-child(2) > label"));
        public IWebElement SecondNumberLabelElement => driver.FindElement(By.CssSelector("body > form > div:nth-child(3) > label"));
        public IWebElement FirstNumberInputElement => driver.FindElement(By.Id("number1"));
        public IWebElement SecondNumberInputElement => driver.FindElement(By.Id("number2"));
        public IWebElement CalculateButton => driver.FindElement(By.Id("calcButton"));
        public IWebElement ResetButton => driver.FindElement(By.Id("resetButton"));
        public IWebElement ResultDivElement => driver.FindElement(By.Id("result"));
        public IWebElement ValidResultElement => driver.FindElement(By.CssSelector("#result > pre"));
        public IWebElement InvalidResultElement => driver.FindElement(By.CssSelector("#result > i"));
        public void Open()
        {
            driver.Url = PageUrl;
        }
        public void ResetForm()
        {
            ResetButton.Click();
        }
        public bool IsFormEmpty()
        {
            return FirstNumberInputElement.Text + SecondNumberInputElement.Text == "";
        }
        public void AddNumbers(string num1, string num2)
        {
            FirstNumberInputElement.SendKeys(num1);
            SecondNumberInputElement.SendKeys(num2);
            CalculateButton.Click();
        }
        public string GetResult()
        {
            string result;
            try 
            {
                result = ValidResultElement.Text;
            }
            catch (Exception)
            {
                result =  InvalidResultElement.Text;
            }
            return result;
        }
        public string GetPageTitle()
        {
            return driver.Title;
        }
        public string GetPageHeading()
        {
            return Heading.Text;
        }
    }
}
