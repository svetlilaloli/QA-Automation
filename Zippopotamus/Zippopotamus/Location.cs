using System.Text.Json.Serialization;

namespace Zippopotamus
{
    public class Location
    {
        [JsonPropertyName("post code")]
        public string PostCode { get; set; }
        public string Country { get; set; }
        [JsonPropertyName("country abbreviation")]
        public string CountryAbbreviation { get; set; }
        public Place[] Places { get; set; }
    }
}
