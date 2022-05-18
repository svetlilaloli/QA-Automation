using System.Text.Json.Serialization;

namespace Zippopotamus
{
    public class Place
    {
        [JsonPropertyName("place name")]
        public string PlaceName { get; set; }
        public string State { get; set; }
        [JsonPropertyName("state abbreviation")]
        public string StateAbbreviation { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
    }
}