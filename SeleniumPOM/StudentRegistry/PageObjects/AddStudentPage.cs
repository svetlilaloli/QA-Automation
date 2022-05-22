using OpenQA.Selenium;

namespace StudentRegistry.PageObjects
{
    public class AddStudentPage : BasePage
    {
        public AddStudentPage(IWebDriver driver) : base(driver)
        {
        }
        public override string PageUrl => "https://mvc-app-node-express.nakov.repl.co/add-student";
        public IWebElement NameLabelElement => driver.FindElement(By.CssSelector("body > form > div:nth-child(1) > label"));
        public IWebElement NameInputElement => driver.FindElement(By.Id("name"));
        public IWebElement EmailLabelElement => driver.FindElement(By.CssSelector("body > form > div:nth-child(2) > label"));
        public IWebElement EmailInputElement => driver.FindElement(By.Id("email"));
        public IWebElement AddButton => driver.FindElement(By.CssSelector("body > form > button"));
        public IWebElement ErrorMessageElement => driver.FindElement(By.PartialLinkText("Cannot add student."));
        public void AddStudent(string name, string email)
        {
            NameInputElement.SendKeys(name);
            EmailInputElement.SendKeys(email);
            AddButton.Click();
        }
        public string GetErrorMessage()
        {
            return ErrorMessageElement.Text;
        }
    }
}
