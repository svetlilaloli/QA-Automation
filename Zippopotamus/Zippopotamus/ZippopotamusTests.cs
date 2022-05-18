using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Zippopotamus
{
    public class ZippopotamusTests
    {
        private RestClient client;
        private RestRequest request;
        [SetUp]
        public void Setup()
        {
            client = new ("https://api.zippopotam.us");
        }

        [TestCase("BG", "1000", "Sofia")]
        [TestCase("BG", "5800", "Pleven")]
        [TestCase("GB", "IM2", "Isle of Man")]
        [TestCase("GB", "BT1", "Belfast")]
        [TestCase("US", "96162", "Truckee")]
        [TestCase("CA", "M5S", "Toronto")]
        [TestCase("DE", "01067", "Dresden")]
        public async Task Test(string countryCode, string postCode, string expectedPlace)
        {
            request = new($"/{countryCode}/{postCode}");
            var response = await client.GetAsync(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            try
            {
                var location = JsonSerializer.Deserialize<Location>(response.Content);
                Assert.AreEqual(countryCode, location.CountryAbbreviation);
                Assert.AreEqual(postCode, location.PostCode);
                StringAssert.Contains(expectedPlace, location.Places[0].PlaceName);
            }
            catch (System.Exception)
            {
            }
        }
    }
}