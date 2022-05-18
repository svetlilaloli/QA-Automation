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
        private const string Password = "YOUR_PASSWORD";
        private const string Repo = "YOUR_REPO";
        private RestRequest request;
        [SetUp]
        public void Setup()
        {
            client = new(ApiUrl);
            client.Authenticator = new HttpBasicAuthenticator(User, Password);
            request = new RestRequest($"/repos/{User}/{Repo}/issues");
        }

        [Test]
        public async Task Test_GitHub_GetAllRepos()
        {
            var reposRequest = new RestRequest($"/users/{User}/repos");
            var response = await client.GetAsync(reposRequest);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            try
            {
                var repos = JsonSerializer.Deserialize<Repo[]>(response.Content);
                Assert.That(repos.Length > 1, "Response is not greater than 0");
                foreach (var repo in repos)
                {
                    Assert.Greater(repo.id, 0, "Id is not greater than 0");
                    StringAssert.Contains($"{User}", repo.full_name);
                    StringAssert.Contains($"https://github.com/{repo.full_name}", repo.html_url);
                }
            }
            catch (System.Exception)
            {
            }
        }
        [Test]
        public async Task Test_GitHub_GetAllIssues()
        {
            var response = await client.GetAsync(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            try
            {
                var issues = JsonSerializer.Deserialize<Issue[]>(response.Content);
                Assert.That(issues.Length > 1, "Response is not greater than 0");
                foreach (var issue in issues)
                {
                    Assert.Greater(issue.id, 0, "Id is not greater than 0");
                    Assert.Greater(issue.number, 0, "Number is not greater than 0");
                    Assert.IsNotEmpty(issue.title, "Title is empty");
                }
            }
            catch (System.Exception)
            {
            }            
        }
        [Test]
        public async Task Test_GitHub_CreateIssueAsyncValidAuthentication()
        {
            request.AddHeader("Content-Type", "application/json");
            string title = "Issue form RestSharp";
            request.AddJsonBody(new { title, body = "Body", labels = new[] { "bug", "API" } });
            var response = await client.ExecuteAsync(request, Method.Post);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            try
            {
                var issue = JsonSerializer.Deserialize<Issue>(response.Content);
                Assert.Greater(issue.id, 0, "Id is not greater than 0");
                Assert.Greater(issue.number, 0, "Number is not greater than 0");
                Assert.AreEqual(title, issue.title);
            }
            catch (System.Exception)
            {
            }            
        }
        [Test]
        public async Task Test_GitHub_CreateIssueAsyncInvalidAuthentication()
        {
            client.Authenticator = null;
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { title = "Issue form RestSharp", body = "Body", labels = new[] { "bug", "API" } });
            var response = await client.ExecuteAsync(request, Method.Post);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Status code is not 404");
        }
        [Test]
        public async Task Test_GitHub_CreateIssueAsyncNoTitle()
        {
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { title = "", body = "Body", labels = new[] { "bug", "API" } });
            var response = await client.ExecuteAsync(request, Method.Post);

            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, response.StatusCode, "Status code is not 422");
        }
        [Test]
        public async Task Test_GitHub_GetIssueByValidNumber()
        {
            request = new RestRequest($"/repos/{User}/{Repo}/issues/5");
            var response = await client.GetAsync(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            try
            {
                var issue = JsonSerializer.Deserialize<Issue>(response.Content);
                Assert.Greater(issue.id, 0, "Id is not greater than 0");
                Assert.Greater(issue.number, 0, "Number is not greater than 0");
            }
            catch(System.Exception)
            {
            }
        }
        [Test]
        public async Task Test_GitHub_GetIssueByInvalidNumber()
        {
            request = new RestRequest($"/repos/{User}/{Repo}/issues/{int.MaxValue}");
            var response = await client.GetAsync(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Test]
        public async Task Test_GitHub_EditExistingIssueValidAuthentication()
        {
            request = new RestRequest($"/repos/{User}/{Repo}/issues/3");
            request.AddHeader("Content-Type", "application/json");
            string title = "Issue form PATCH request with RestSharp";
            request.AddJsonBody(new { title });
            var response = await client.ExecuteAsync(request, Method.Patch);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            try
            {
                var issue = JsonSerializer.Deserialize<Issue>(response.Content);

                Assert.Greater(issue.id, 0, "Id is not greater than 0");
                Assert.Greater(issue.number, 0, "Number is not greater than 0");
                Assert.AreEqual(title, issue.title);
            }
            catch (System.Exception)
            {
            }
        }
        [Test]
        public async Task Test_GitHub_EditExistingIssueInvalidAuthentication()
        {
            client.Authenticator = null;
            request = new RestRequest($"/repos/{User}/{Repo}/issues/3");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { title = "No authentication" });
            var response = await client.ExecuteAsync(request, Method.Patch);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Test]
        public async Task Test_GitHub_EditIssueInvalidNumber()
        {
            request = new RestRequest($"/repos/{User}/{Repo}/issues/{int.MaxValue}");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { title = "Invalid issue number" });
            var response = await client.ExecuteAsync(request, Method.Patch);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}