using Newtonsoft.Json;

namespace api.Models
{
    public class User
    {
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }
    }
}