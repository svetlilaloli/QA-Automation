using OpenQA.Selenium;
using System;

namespace StudentRegistry.PageObjects
{
    public class BasePage
    {
        protected readonly IWebDriver driver;
        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }
        public virtual string PageUrl { get; }
        public IWebElement HomeLinkElement => driver.FindElement(By.XPath("/html/body/a[1]"));
        public IWebElement ViewStudentsLinkElement => driver.FindElement(By.XPath("/html/body/a[2]"));
        public IWebElement AddStudentLinkElement => driver.FindElement(By.XPath("/html/body/a[3]"));
        public IWebElement HeadingElement => driver.FindElement(By.CssSelector("body > h1"));
        public virtual void Open()
        {
            driver.Url = PageUrl;
        }
        public bool IsOpen()
        {
            return driver.Url == PageUrl;
        }
        public string GetPageTitle()
        {
            return driver.Title;
        }
        public string GetPageHeading()
        {
            return HeadingElement.Text;
        }
    }
}
