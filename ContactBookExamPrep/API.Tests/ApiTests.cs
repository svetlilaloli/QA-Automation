using RestSharp;
using System.Net;
using System.Text.Json;

namespace API.Tests
{
    public class ApiTests
    {
        private const string ApiUrl = "https://contactbook.nakov.repl.co/api/contacts";
        private RestClient client;
        private RestRequest request;
        private RestResponse response;

        [SetUp]
        public void Setup()
        {
            client = new();
        }

        [Test]
        public void Test_ListAllContacts_CheckFirstContact()
        {
            request = new(ApiUrl);
            response = client.Execute(request);
            var allContacts = JsonSerializer.Deserialize<ContactItem[]>(response.Content);
            string fullName = $"{allContacts[0].firstName} {allContacts[0].lastName}";

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(allContacts.Length, Is.GreaterThan(0));
                Assert.That(fullName, Is.EqualTo("Steve Jobs"));
            });
        }

        [Test]
        public void Test_ValidSearch_CheckFirstResult()
        {
            request = new(ApiUrl + "/search/albert");
            response = client.Execute(request);
            var allContacts = JsonSerializer.Deserialize<ContactItem[]>(response.Content);
            string fullName = $"{allContacts[0].firstName} {allContacts[0].lastName}";

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(allContacts.Length, Is.GreaterThan(0));
                Assert.That(fullName, Is.EqualTo("Albert Einstein"));
            });
        }

        [Test]
        public void Test_InvalidSearch_CheckIsEmpty()
        {
            request = new(ApiUrl + $"/search/missing{DateTime.Now.Ticks}");
            response = client.Execute(request);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content, Is.EqualTo("[]"));
            });
        }
        [Test]
        public void Test_AddContact_InvalidData()
        {
            request = new(ApiUrl);
            var requestBody = new ContactItem
            {
                firstName = "Me"
            };
            request.AddBody(requestBody);
            response = client.Execute(request, Method.Post);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Last name cannot be empty!\"}"));
            });
        }
        [Test]
        public void Test_AddContact_ValidData()
        {
            request = new(ApiUrl);
            var requestBody = new ContactItem
            {
                firstName = "Marie",
                lastName = "Curie",
                email = "marie67@gmail.com",
                phone = "+1 800 200 300",
                comments = "Old friend"
            };
            request.AddBody(requestBody);
            response = client.Execute(request, Method.Post);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            response = client.Execute(request, Method.Get);
            var allContacts = JsonSerializer.Deserialize<ContactItem[]>(response.Content);
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(allContacts.Last().firstName, Is.EqualTo(requestBody.firstName));
                Assert.That(allContacts.Last().lastName, Is.EqualTo(requestBody.lastName));
            });
        }
    }
}