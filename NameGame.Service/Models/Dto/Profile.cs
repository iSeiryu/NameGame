using System.Text.Json.Serialization;

namespace NameGame.Service.Models.Dto {
    public class Profile {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("headshot")]
        public Image Image { get; set; }
    }

    public class Image {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
