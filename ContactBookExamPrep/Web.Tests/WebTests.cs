using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Web.Tests
{
    public class WebTests
    {
        private ChromeDriver driver;
        private const string AppUrl = "https://contactbook.nakov.repl.co";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [Test]
        public void Test_ListContacts_CheckFirstContact()
        {
            driver.Navigate().GoToUrl(AppUrl);
            driver.FindElement(By.LinkText("Contacts")).Click();

            string fName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.fname > td")).Text;
            string lName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.lname > td")).Text;

            Assert.Multiple(() =>
            {
                Assert.That(fName, Is.EqualTo("Steve"));
                Assert.That(lName, Is.EqualTo("Jobs"));
            });
        }
        [Test]
        public void Test_SearchContacts_ByValidKeyword()
        {
            // open search page
            driver.Navigate().GoToUrl(AppUrl);
            driver.FindElement(By.LinkText("Search")).Click();
            // search for "albert"
            driver.FindElement(By.Id("keyword")).SendKeys("albert");
            driver.FindElement(By.Id("search")).Click();
            // get all results
            var allResults = driver.FindElement(By.CssSelector("div.contacts-grid"));
            // get first result's names
            string fName = allResults.FindElement(By.CssSelector("#contact3 > tbody > tr.fname > td")).Text;
            string lName = allResults.FindElement(By.CssSelector("#contact3 > tbody > tr.lname > td")).Text;

            Assert.Multiple(() =>
            {
                Assert.That(fName, Is.EqualTo("Albert"));
                Assert.That(lName, Is.EqualTo("Einstein"));
            });
        }
        [Test]
        public void Test_SearchContacts_ByInalidKeyword()
        {
            // open search page
            driver.Navigate().GoToUrl(AppUrl);
            driver.FindElement(By.LinkText("Search")).Click();
            // search for "invalid2635"
            driver.FindElement(By.Id("keyword")).SendKeys("invalid2635");
            driver.FindElement(By.Id("search")).Click();
            // get all results
            var searchResult = driver.FindElement(By.Id("searchResult")).Text;

            Assert.That(searchResult, Is.EqualTo("No contacts found."));
        }
        [TestCase("John", "Smith", "", "Invalid email")]
        [TestCase("John", "Smith", "abv", "Invalid email")]
        [TestCase("John", "", "abv", "Last name cannot be empty")]
        [TestCase("", "Smith", "abv", "First name cannot be empty")]
        public void Test_CreateContact_WithInvalidData(string fName, string lName, string email, string errorMessage)
        {
            // open create contact page
            driver.Navigate().GoToUrl(AppUrl);
            driver.FindElement(By.LinkText("Create")).Click();
            // create contact with invalid data
            driver.FindElement(By.Id("firstName")).SendKeys(fName);
            driver.FindElement(By.Id("lastName")).SendKeys(lName);
            driver.FindElement(By.Id("email")).SendKeys(email);
            driver.FindElement(By.Id("create")).Click();
            // get the error message
            var errorMsg = driver.FindElement(By.CssSelector("div.err")).Text;

            Assert.That(errorMsg, Is.EqualTo($"Error: {errorMessage}!"));
        }
        [Test]
        public void Test_CreateContact_WithValidData()
        {
            string firstName = $"Test {DateTime.Now.Ticks}";
            string lastName = $"Testov {DateTime.Now.Ticks}";
            string email = "name@domain.com";
            // open create contact page
            driver.Navigate().GoToUrl(AppUrl);
            driver.FindElement(By.LinkText("Create")).Click();
            // create contact with valid data
            driver.FindElement(By.Id("firstName")).SendKeys(firstName);
            driver.FindElement(By.Id("lastName")).SendKeys(lastName);
            driver.FindElement(By.Id("email")).SendKeys(email);
            driver.FindElement(By.Id("create")).Click();
            // get all contacts
            var allContacts = driver.FindElements(By.CssSelector("div.contacts-grid > a"));
            // get the last contact
            var newContact = allContacts.Last();
            // check names
            string fName = newContact.FindElement(By.CssSelector("tr.fname > td")).Text;
            string lName = newContact.FindElement(By.CssSelector("tr.lname > td")).Text;
            
            Assert.Multiple(() =>
            {
                Assert.That(fName, Is.EqualTo(firstName));
                Assert.That(lName, Is.EqualTo(lastName));
            });
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}