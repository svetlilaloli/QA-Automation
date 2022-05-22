using OpenQA.Selenium;

namespace StudentRegistry.PageObjects
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver)
        {
        }
        public override string PageUrl => "https://mvc-app-node-express.nakov.repl.co/";
        public IWebElement RegisteredStudentsElement => driver.FindElement(By.CssSelector("body > p"));
        public IWebElement RegisteredStudentsCountElement => driver.FindElement(By.CssSelector("body > p > b"));
        public int GetStudentsCount()
        {
            return int.Parse(RegisteredStudentsCountElement.Text);
        }
    }
}
