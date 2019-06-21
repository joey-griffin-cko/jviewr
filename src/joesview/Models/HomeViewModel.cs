using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace joesview.Models
{
    public class HomeViewModel
    {
        public IEnumerable<IGrouping<string, GitThing>> PRs { get; set; } = new List<IGrouping<string, GitThing>>();
        public IEnumerable<IGrouping<string, SprintTask>> Issues { get; set; } = new List<IGrouping<string, SprintTask>>();
    }

    public class SprintTask
    {
        public string Key { get; set; }
        public string Url => $"https://checkout.atlassian.net/browse/{Key}";
        public JiraField Fields { get; set; }
    }

    public class JiraField
    {
        public JiraStatus Status { get; set; }
        public string Summary { get; set; }
    }

    public class JiraStatus
    {
        public string Name { get; set; }
    }

    public class GitThing
    {
        public string Repo { get; set; }
        public string RepoUrl => $"https://github.com/{Repo}";

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("html_url")]
        public string Url { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public bool HasApprovals => Reviews.Any(x => x.State == "APPROVED");

        public override bool Equals(object obj)
        {
            if (obj is GitThing)
            {
                var other = obj as GitThing;
                return RepoUrl == other.RepoUrl && Number == other.Number && Body == other.Body;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 352033288;
            hashCode = hashCode * -1521134295 + Url.GetHashCode();
            hashCode = hashCode * -1521134295 + Title.GetHashCode();
            hashCode = hashCode * -1521134295 + RepoUrl.GetHashCode();
            hashCode = hashCode * -1521134295 + Number.GetHashCode();

            return hashCode;
        }
    }

    public class User
    {
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }
    }

    public class Review
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}