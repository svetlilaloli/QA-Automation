using NUnit.Framework;
using StudentRegistry.PageObjects;

namespace StudentRegistry.Tests
{
    public class HomePageTests : BaseTest
    {
        private HomePage page;
        [SetUp]
        public void SetUp()
        {
            page = new HomePage(driver);
            page.Open();
        }
        [Test]
        public void Test_HomePage_Content()
        {
            Assert.AreEqual("MVC Example", page.GetPageTitle());
            Assert.AreEqual("Students Registry", page.GetPageHeading());
            page.GetStudentsCount();
        }
        [Test]
        public void Test_HomePage_Links()
        {
            page.HomeLinkElement.Click();
            Assert.IsTrue(new HomePage(driver).IsOpen());
            page.ViewStudentsLinkElement.Click();
            Assert.IsTrue(new ViewStudentsPage(driver).IsOpen());
            page.Open();
            page.AddStudentLinkElement.Click();
            Assert.IsTrue(new AddStudentPage(driver).IsOpen());
        }
    }
}
