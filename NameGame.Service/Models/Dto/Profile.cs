using Newtonsoft.Json;

namespace NameGame.Service.Models.Dto
{
    public class Profile
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("headshot")]
        public Image Image { get; set; }
    }

    public class Image
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
