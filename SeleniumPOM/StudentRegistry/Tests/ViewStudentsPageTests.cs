using NUnit.Framework;
using StudentRegistry.PageObjects;

namespace StudentRegistry.Tests
{
    public class ViewStudentsPageTests : BaseTest
    {
        private ViewStudentsPage page;
        [SetUp]
        public void SetUp()
        {
            page = new ViewStudentsPage(driver);
            page.Open();
        }
        [Test]
        public void Test_ViewStudentsPage_Content()
        {
            Assert.AreEqual(page.GetPageTitle(), "Students");
            Assert.AreEqual(page.GetPageHeading(), "Registered Students");
            var students = page.GetRegisteredStudents();
            foreach (var student in students)
            {
                Assert.IsTrue(student.IndexOf('(') > 0);
                Assert.IsTrue(student.LastIndexOf(')') == student.Length - 1);
            }
        }
        [Test]
        public void Test_ViewStudentsPage_Links()
        {
            page.ViewStudentsLinkElement.Click();
            Assert.IsTrue(new ViewStudentsPage(driver).IsOpen());
            page.HomeLinkElement.Click();
            Assert.IsTrue(new HomePage(driver).IsOpen());
            page.Open();
            page.AddStudentLinkElement.Click();
            Assert.IsTrue(new AddStudentPage(driver).IsOpen());
        }
    }
}
