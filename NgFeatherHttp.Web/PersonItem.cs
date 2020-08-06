using System.Text.Json.Serialization;

namespace NgFeatherHttp.Web
{
    public class PersonItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

    }
}
