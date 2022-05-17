using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API_RestSharp
{
    public class Tests
    {
        private RestClient client;
        private const string ApiUrl = "https://api.github.com";
        private const string User = "YOUR_USERNAME";
        private const string Password = "YOUR_TOKEN";
        private const string Repo = "YOUR_REPO";
        private RestRequest request;
        [SetUp]
        public void Setup()
        {
            client = new (ApiUrl);
            client.Authenticator = new HttpBasicAuthenticator(User, Password);
            request = new RestRequest($"/repos/{User}/{Repo}/issues");
        }

        [Test]
        public async Task Test_GitHub_GetAllRepos()
        {
            var reposRequest = new RestRequest($"/users/{User}/repos");
            var response = await client.GetAsync(reposRequest);
            var repos = JsonSerializer.Deserialize<Repo[]>(response.Content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Status code is not 200");
            Assert.That(repos.Length > 0, "Response is not greater than 0");
        }
        [Test]
        public async Task Test_GitHub_GetAllIssues()
        {
            var response = await client.GetAsync(request);
            var issues = JsonSerializer.Deserialize<Issue[]>(response.Content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Status code is not 200");
            Assert.That(issues.Length > 0, "Response is not greater than 0");
            foreach (var issue in issues)
            {
                Assert.Greater(issue.id, 0, "Id is not greater than 0");
                Assert.Greater(issue.number, 0, "Number is not greater than 0");
                Assert.IsNotEmpty(issue.title, "Title is empty");
            }
        }
        [Test]
        public async Task Test_GitHub_CreateIssue()
        {
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { title = "Issue form RestSharp", body = "Body", labels = new[] { "bug", "API"} });
            var response = await client.ExecuteAsync(request, Method.Post);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "Status code is not 201");
            Assert.Greater(issue.id, 0, "Id is not greater than 0");
            Assert.Greater(issue.number, 0, "Number is not greater than 0");
            Assert.IsNotEmpty(issue.title, "Title is empty");
        }
    }
}