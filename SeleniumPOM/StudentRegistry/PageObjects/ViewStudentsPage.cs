using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Linq;

namespace StudentRegistry.PageObjects
{
    public class ViewStudentsPage : BasePage
    {
        public ViewStudentsPage(IWebDriver driver) : base(driver)
        {
        }
        public override string PageUrl => "https://mvc-app-node-express.nakov.repl.co/students";
        public ReadOnlyCollection<IWebElement> StudentsCollectionElement => driver.FindElements(By.CssSelector("body > ul > li"));
        public string[] GetRegisteredStudents()
        {
            return StudentsCollectionElement.Select(s => s.Text).ToArray();
        }
    }
}
