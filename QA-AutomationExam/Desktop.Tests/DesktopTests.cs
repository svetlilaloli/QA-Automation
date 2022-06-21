using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Desktop.Tests
{
    public class DesktopTests
    {
        private const string App = @"C:\Users\svetl\Documents\QAAutomation\TaskBoard.DesktopClient-v1.0\TaskBoard.DesktopClient.exe";
        private const string ApiUrl = "https://taskboard.svassileva.repl.co/api";
        private const string AppiumServerUrl = "http://127.0.0.1:4723/wd/hub";
        private WindowsDriver<WindowsElement> driver;

        [SetUp]
        public void Setup()
        {
            AppiumOptions options = new()
            {
                PlatformName = "Windows"
            };
            options.AddAdditionalCapability("app", App);
            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServerUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void ListTasks_CheckHoldTitleProjectSkeleton()
        {
            var apiTextField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            apiTextField.Clear();
            apiTextField.SendKeys(ApiUrl);
            driver.FindElementByAccessibilityId("buttonConnect").Click();

            var openTable = driver.FindElementsByAccessibilityId("listViewTasks");

            foreach (var task in openTable)
            {
                if (task.Text.StartsWith("Project"))
                {
                    Assert.That(task.Text, Is.EqualTo("Project skeleton"));
                    break;
                }
            }
        }
        [Test]
        public void CreateTask_ValidUniqueTitle_CheckIsListed()
        {
            // connect to API
            var apiTextField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            apiTextField.Clear();
            apiTextField.SendKeys(ApiUrl);
            driver.FindElementByAccessibilityId("buttonConnect").Click();
            
            // find add button
            driver.FindElementByAccessibilityId("buttonAdd").Click();
            
            // create new task
            string newTitle = $"DesktopTask{DateTime.Now.Ticks}";
            var titleField = driver.FindElementByAccessibilityId("textBoxTitle");
            titleField.Clear();
            titleField.SendKeys(newTitle);
            driver.FindElementByAccessibilityId("buttonCreate").Click();
            
            // search for the new title
            var searchField = driver.FindElementByAccessibilityId("textBoxSearchText");
            searchField.Clear();
            searchField.SendKeys(newTitle);
            driver.FindElementByAccessibilityId("buttonSearch").Click();

            // check if the task is listed
            var openTable = driver.FindElementsByAccessibilityId("listViewTasks");

            foreach (var task in openTable)
            {
                if (task.Text.StartsWith(newTitle))
                {
                    Assert.That(task.Text, Is.EqualTo(newTitle));
                    break;
                }
            }
        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}