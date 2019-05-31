using System.Collections.Generic;

namespace joesview.Models
{
    public class GithubSettings
    {
        public string Token { get; set; }
        public List<string> Repositories { get; set; }
    }
}