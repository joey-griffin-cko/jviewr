using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class JiraField
    {
        public JiraStatus Status { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }

        [JsonProperty("customfield_10124")]
        public double StoryPoints { get; set; }
        public CommentList Comment { get; set; }
        public IssueType IssueType { get; set; }
        public Epic Epic { get; set; }
        public JiraUser Assignee { get; set; }
        public IEnumerable<FixVersion> FixVersion { get; set; }
    }

    public class IssueType 
    {
        public string IconUrl { get; set; }
        public string Name { get; set; }
    }

    public class CommentList
    {
        public IEnumerable<Comment> Comments { get; set; }
    }

    public class Comment
    {
        public User Author { get; set; }
    }

    public class JiraUser
    {
        public string DisplayName { get; set; }
        public Avatars AvatarUrls { get; set; }
    }

    public class Avatars
    {
        [JsonProperty("48x48")]
        public string Url { get; set; }
    }

    public class Epic
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }

    public class FixVersion
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}