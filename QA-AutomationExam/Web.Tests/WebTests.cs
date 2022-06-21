using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Web.Tests
{
    public class WebTests
    {
        private const string url = "https://taskboard.svassileva.repl.co/";
        private WebDriver driver;
        [SetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void ListTasks_FirstDone_HasTitleProjectSkeleton()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Task Board")).Click();

            var doneBoard = driver.FindElement(By.CssSelector("div > div:nth-child(3)"));
            var firstTitle = doneBoard.FindElement(By.CssSelector("#task1 > tbody > tr.title > td")).Text;

            Assert.That(firstTitle, Is.EqualTo("Project skeleton"));
        }
        [Test]
        public void SearchTasks_ByKeywordHome_FirstTitleHomePage()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();

            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.Clear();
            searchField.SendKeys("Home");
            driver.FindElement(By.Id("search")).Click();

            var firstResult = driver.FindElements(By.ClassName("task")).First();
            var title = firstResult.FindElement(By.CssSelector("#task2 > tbody > tr.title > td")).Text;

            Assert.That(title, Is.EqualTo("Home page"));
        }
        [Test]
        public void SearchTasks_ByKeywordMissingRandNum_NoTasksFound()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();

            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.Clear();
            searchField.SendKeys($"missing{DateTime.Now.Ticks}");
            driver.FindElement(By.Id("search")).Click();

            var divResult = driver.FindElement(By.Id("searchResult")).Text;

            Assert.That(divResult, Is.EqualTo("No tasks found."));
        }
        [Test]
        public void CreateTask_InvalidData_ErrorReturned()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            driver.FindElement(By.Id("create")).Click();

            var divError = driver.FindElement(By.ClassName("err")).Text;

            Assert.That(divError, Is.EqualTo("Error: Title cannot be empty!"));
        }
        [Test]
        public void CreateTask_ValidData_NewTaskListed()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            string title = DateTime.Now.Ticks.ToString();

            var titleField = driver.FindElement(By.Id("title"));
            titleField.Clear();
            titleField.SendKeys(title);
            driver.FindElement(By.Id("create")).Click();

            var openBoard = driver.FindElement(By.ClassName("task"));// returns the first found
            var newTask = openBoard.FindElements(By.ClassName("task-entry")).Last();
            var newTaskTitle = newTask.FindElement(By.CssSelector("tbody > tr.title > td")).Text;

            Assert.That(newTaskTitle, Is.EqualTo(title));
        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}