using NUnit.Framework;
using StudentRegistry.PageObjects;
using System;

namespace StudentRegistry.Tests
{
    public class AddStudentPageTests : BaseTest
    {
        private AddStudentPage page;
        [SetUp]
        public void SetUp()
        {
            page = new AddStudentPage(driver);
            page.Open();
        }
        [Test]
        public void Test_AddStudentPage_Content()
        {
            Assert.AreEqual("Add Student", page.GetPageTitle());
            Assert.AreEqual("Register New Student", page.GetPageHeading());
            Assert.AreEqual("", page.NameInputElement.Text);
            Assert.AreEqual("", page.EmailInputElement.Text);
            Assert.AreEqual("Add", page.AddButton.Text);
        }
        [Test]
        public void Test_AddStudentPage_Links()
        {
            page.AddStudentLinkElement.Click();
            Assert.IsTrue(new AddStudentPage(driver).IsOpen()); 
            page.HomeLinkElement.Click();
            Assert.IsTrue(new HomePage(driver).IsOpen());
            page.Open();
            page.ViewStudentsLinkElement.Click();
            Assert.IsTrue(new ViewStudentsPage(driver).IsOpen());            
        }
        [Test]
        public void Test_AddStudentPage_AddValidStudent()
        {
            string name = "me" + DateTime.Now.Ticks;
            string email = "my" + DateTime.Now.Ticks + "@email.com";
            page.AddStudent(name, email);
            var viewStudentsPage = new ViewStudentsPage(driver);
            viewStudentsPage.Open();
            Assert.IsTrue(viewStudentsPage.IsOpen());
            Assert.Contains($"{name} ({email})", viewStudentsPage.GetRegisteredStudents());
        }
        public void Test_AddStudentPage_AddInvalidStudent()
        {
            page.AddStudent("", $"{DateTime.Now.Ticks}@ema.il");
            Assert.IsTrue(page.IsOpen());
            Assert.AreEqual("Cannot add student", page.GetErrorMessage());
            page.AddStudent($"my{DateTime.Now.Ticks}name", "");
            Assert.IsTrue(page.IsOpen());
            Assert.AreEqual("Cannot add student", page.GetErrorMessage());
        }
    }
}
