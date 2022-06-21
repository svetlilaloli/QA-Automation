using RestSharp;
using System.Net;
using System.Text.Json;
using NUnit.Framework;

namespace Api.Tests
{
    public class ApiTests
    {
        private const string ApiUrl = "http://taskboard.svassileva.repl.co/api/tasks";
        private RestClient client;
        private RestRequest request;
        private RestResponse response;
        [SetUp]
        public void Setup()
        {
            client = new();
        }

        [Test]
        public void ListTasks_FirstDone_HasTitleProjectSkeleton()
        {
            request = new(ApiUrl);
            
            response = client.Execute(request);
            var allTasks = JsonSerializer.Deserialize<Task[]>(response.Content);
            var firstDoneTask = allTasks.Where(t => t.board.name == "Done").First();
            
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(allTasks.Length, Is.GreaterThan(0));
                Assert.That(firstDoneTask.title, Is.EqualTo("Project skeleton"));
            });
        }
        [Test]
        public void SearchTasks_ByKeywordHome_FirstTitleHomePage()
        {
            request = new($"{ApiUrl}/search/home");

            response = client.Execute(request);
            var allTasks = JsonSerializer.Deserialize<Task[]>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(allTasks.Length, Is.GreaterThan(0));
                Assert.That(allTasks.First().title, Is.EqualTo("Home page"));
            });
        }
        [Test]
        public void SearchTasks_ByKeywordMissingRandNum_EmptyResults()
        {
            request = new($"{ApiUrl}/search/missing{DateTime.Now.Ticks}");

            response = client.Execute(request);
            var allTasks = JsonSerializer.Deserialize<Task[]>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(allTasks.Length, Is.EqualTo(0));
            });
        }
        [Test]
        public void CreateTask_InvalidData_ErrorReturned()
        {
            var invalidTask = new
            {
                title = "",
                description = "description"
            };
            request = new(ApiUrl, Method.Post);
            request.AddJsonBody(invalidTask);
            response = client.Execute(request);
            
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(response.Content, Contains.Substring("Title cannot be empty!"));
            });
        }
        [Test]
        public void CreateTask_ValidData_NewTaskListed()
        {
            Task validTask = new()
            {
                title = "New Task",
                description = "Description."
            };
            request = new(ApiUrl, Method.Post);
            request.AddJsonBody(validTask);
            response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            request = new(ApiUrl);
            response = client.Execute(request);
            var allTasks = JsonSerializer.Deserialize<Task[]>(response.Content);
            var lastTask = allTasks.Last();
            
            Assert.Multiple(() =>
            {
                Assert.That(lastTask.title, Is.EqualTo(validTask.title));
                Assert.That(lastTask.description, Is.EqualTo(validTask.description));
            });
        }
    }
}