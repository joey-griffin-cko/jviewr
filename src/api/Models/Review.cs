using System;
using Newtonsoft.Json;

namespace api.Models
{
    public class Review
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}