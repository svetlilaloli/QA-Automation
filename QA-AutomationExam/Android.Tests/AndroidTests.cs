using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace Android.Tests
{
    public class AndroidTests
    {
        private const string App = @"C:\Users\svetl\Documents\QAAutomation\Installers-APKs\taskboard-androidclient.apk";
        private const string ApiUrl = "https://taskboard.svassileva.repl.co/api";
        private const string AppiumServer = "http://127.0.0.1/wd/hub";
        private AndroidDriver<AndroidElement> driver;

        [SetUp]
        public void Setup()
        {
            AppiumOptions options = new()
            {
                PlatformName = "Android"
            };
            options.AddAdditionalCapability("app", App);
            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void ListTasks_FirstHasTitleProjectSkeleton()
        {
            var urlField = driver.FindElementByAccessibilityId("taskboard.androidclient:id/editTextApiUrl");
            urlField.Clear();
            urlField.SendKeys(ApiUrl);
            driver.FindElementByAccessibilityId("taskboard.androidclient:id/buttonConnect").Click();

            var tasks = driver.FindElementByAccessibilityId("taskboard.androidclient:id/recyclerViewTasks");
            var title = tasks.FindElementByXPath("//androidx.recyclerview.widget.RecyclerView/android.widget.TableLayout[1]/android.widget.TableRow[3]/android.widget.TextView[2]").Text;

            Assert.That(title, Is.EqualTo("Project skeleton"));
        }
        [Test]
        public void CreateTask_ValidUniqueTitle_CheckIsListed()
        {
            var urlField = driver.FindElementByAccessibilityId("taskboard.androidclient:id/editTextApiUrl");
            urlField.Clear();
            urlField.SendKeys(ApiUrl);
            driver.FindElementByAccessibilityId("taskboard.androidclient:id/buttonConnect").Click();

            driver.FindElementByAccessibilityId("taskboard.androidclient:id/buttonAdd").Click();
            string newTaskTitle = $"AndroidTask{DateTime.Now.Ticks}";
            var titleField = driver.FindElementByAccessibilityId("taskboard.androidclient:id/editTextTitle");
            titleField.Clear();
            titleField.SendKeys(newTaskTitle);
            driver.FindElementByAccessibilityId("taskboard.androidclient:id/buttonCreate").Click();

            var searchField = driver.FindElementByAccessibilityId("taskboard.androidclient:id/editTextKeyword");
            searchField.Clear();
            searchField.SendKeys(newTaskTitle);
            driver.FindElementByAccessibilityId("taskboard.androidclient:id/buttonSearch").Click();

            var result = driver.FindElementByAccessibilityId("taskboard.androidclient:id/recyclerViewTasks");
            var title = result.FindElementByAccessibilityId("taskboard.androidclient:id/textViewTitle").Text;

            Assert.That(title, Is.EqualTo(newTaskTitle));
        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}